using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System;

namespace com.sky88.game.bet.baccarat
{
    public partial class DealingController_Barcode : Form
    {
        #region Variable declarations
        const string urlNewGame = "{0}newGame.aspx?sessionID={1}&dealerID={2}&tableID={3}&name={4}";
        const string urlCheckState = "{0}checkState.aspx?sessionID={1}&dealerID={2}&tableID={3}";
        const string urlChangeState = "{0}stateChange.aspx?sessionID={1}&dealerID={2}&gameID={3}&spName={4}";
        const string urlLogin = "{0}login.aspx?username={1}&password={2}";
        const string urlFirst = "{0}setFirstCards.aspx?sessionID={1}&dealerID={2}&gameID={3}&p1={4}&p2={5}&b1={6}&b2={7}&pt={8}&bt={9}";
        const string urlP3 = "{0}setPlayerCard3.aspx?sessionID={1}&dealerID={2}&gameID={3}&p3={4}&pt={5}";
        const string urlB3 = "{0}setBankerCard3.aspx?sessionID={1}&dealerID={2}&gameID={3}&b3={4}&bt={5}";
        const string strPrintP3 = "gameID:{0} set player 3rd card: pc3:{1} pt:{2} -> {3}";
        const string strPrintB3 = "gameID:{0} set banker 3rd card: bc3:{1} bt:{2} -> {3}";
        const string strPrintFirsts = "gameID:{0} set first cards: pc1:{1}  pc2:{2} bc1:{3} bc2:{4} pt:{5} bt:{6} -> {7}";
        const string strPrintStateChange = "state change gameID:{0} {1} -> {2}";
        const string OK = "OK#";
        
        readonly int tableID;
        readonly string server_url;
        readonly DealingControllerConfig conf;
        readonly WebClient web_client;
        
        ResourceManager resman;
        CultureInfo culture;
        BackgroundWorker readCardWorker, serverCallWorker;
        DebugForm debugForm;

        string sessionID;
        string currentState;
        int currentGameID = 0;
        int iDealerID;

        string cardP1, cardP2, cardP3, cardB1, cardB2, cardB3;
        delegate void AsyncCallDelegate(object args);

        //These are local game result cache to determine third card logic
        int _iPTotal = 0, _iBTotal = 0, _iPCard1 = 0, _iPCard2 = 0, _iPCard3 = 0, _iBCard1 = 0, _iBCard2 = 0, _iBCard3 = 0;
        #endregion

        #region UI Events & Constructor
        public DealingController_Barcode()
        {
            InitializeComponent();
            web_client = new WebClient();
            try
            {
                conf = DealingControllerConfig.getInstance();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                this.Dispose();
                Environment.Exit(0);
            }
            tableID = conf.tableID;
            server_url = "http://" + conf.server_url + "/";
        }
        private void DealingController_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            clearAllCardFields();
            switchNewGameButton(false);
            this.Left = 0;
            this.Top = 0;
            try
            {
                /* enable back when continue */
                /*
                resman = new ResourceManager("DealingController.Resources", Assembly.GetExecutingAssembly());
                culture = CultureInfo.CreateSpecificCulture(ConfigurationManager.AppSettings["language"].ToString());
                */
                string isDebug = ConfigurationManager.AppSettings["isDebug"].ToString();
                if (Boolean.Parse(isDebug))
                {
                    debugForm = new DebugForm();
                    debugForm.Show(this);
                    debugForm.Left = this.Left;
                    debugForm.Top = this.Top;
                    debugForm.Opacity = 0.3;
                }
                Text = "Dealing controller - Table: " + ConfigurationManager.AppSettings["tableID"].ToString();
            }
            catch
            {
                MessageBox.Show("Invalid config file value. Program Exit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Dispose();
                Environment.Exit(0);
            }
            #region initialize workers

            serverCallWorker = new BackgroundWorker();
            serverCallWorker.DoWork += serverCallWorker_DoWork;
            serverCallWorker.RunWorkerCompleted += serverCallWorker_RunWorkerCompleted;
            #endregion
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (btnLogin.Text.Equals("登入"))
            {
                btnLogin.Enabled = false;
                switchCloseButton(false);
                loginDealer(txtUser.Text, txtPassword.Text);
            }
            else
            {
                sessionID = null;
                btnLogin.Text = "登入";
                plLogin.Enabled = true;
                txtUser.Enabled = true;
                txtPassword.Enabled = true;
                plMain.Enabled = false;
                switchCloseButton(true);
                switchNewGameButton(false);
            }
        }

        private void btnStartGame_Click(object sender, EventArgs e)
        {
            clearAllCardFields();
            switchNewGameButton(false);
            switchLoginButton(false);
            newGame();
        }
        #endregion        
        
        #region Pure UI Control Code
        void clearAllCardFields()
        {
            picB1.BackgroundImage = Image.FromFile(conf.imgNC);
            picB2.BackgroundImage = Image.FromFile(conf.imgNC);
            picB3.BackgroundImage = Image.FromFile(conf.imgNC);
            picP1.BackgroundImage = Image.FromFile(conf.imgNC);
            picP2.BackgroundImage = Image.FromFile(conf.imgNC);
            picP3.BackgroundImage = Image.FromFile(conf.imgNC);

            txtCard1.Text = string.Empty;
            txtCard2.Text = string.Empty;
            txtCard3.Text = string.Empty;
            txtCard4.Text = string.Empty;
            txtCard5.Text = string.Empty;
            txtCard6.Text = string.Empty;
        }
        void switchAbortButton(bool isOn) { throw new NotImplementedException(); }
        void switchLoginButton(bool isOn)
        {
            if (isOn)
            {
                btnLogin.Enabled = true;
                btnLogin.BackColor = Color.LimeGreen;
            }
            else
            {
                btnLogin.BackColor = Color.Gray;
                btnLogin.Enabled = false;
            }
        }
        void switchCloseButton(bool isOn)
        {
            if (isOn)
            {
                btnClose.Enabled = true;
                btnClose.BackColor = Color.Red;
            }
            else
            {
                btnClose.BackColor = Color.Gray;
                btnClose.Enabled = false;
            }
        }
        void switchNewGameButton(bool isOn)
        {
            if (isOn)
            {
                btnStartGame.Enabled = true;
                btnStartGame.BackColor = Color.LimeGreen;
            }
            else
            {
                btnStartGame.BackColor = Color.Gray;
                btnStartGame.Enabled = false;
            }
        }
        #endregion

        #region Utility Functions

        int getCardValue(string ch)
        {
            switch (ch)
            {
                case "A": return 1;
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9": return int.Parse(ch);
                case "X":
                case "J":
                case "Q":
                case "K": return 0;
                default: return 0;
            }
        }
        decimal convert2Second(int delay) { return Math.Floor((decimal)(delay / 1000)); }
        bool isCallOK(string result) { return result.StartsWith(OK); }
        string[] splitPairs(string input) { return input.Substring(input.IndexOf(OK) + 3).Split(';'); }
        string[] splitValue(string input) { return input.Split(':'); }
        string extractState(string input) { return splitValue(splitPairs(input)[0])[1]; }
        string getWinnerString() { return (_iPTotal > _iBTotal) ? "Player win." : (_iPTotal < _iBTotal) ? "Banker win." : "Tie."; }
        string getGameDoneString() { return "Game is done. Player: " + _iPTotal + "Banker:" + _iBTotal + getWinnerString(); }
        string getNewGameName() { return "t" + tableID + "_" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"); }
        void showInstruction(string instruction) { txtMsg.Text = instruction; Thread.Sleep(50); txtMsg.Clear(); txtMsg.AppendText(instruction); }
        void resetGameValues() 
        { 
            _iPTotal = _iBTotal = _iPCard1 = _iPCard2 = _iPCard3 = _iBCard1 = _iBCard2 = _iBCard3 = 0;
            cardP1 = cardP2 = cardP3 = cardB1 = cardB2 = cardB3 = string.Empty;
        }
        void wait(int time) { Thread.Sleep(time); }
        void printError(string msg) { print("[ERROR] " + msg); }
        void printError(Exception e) { printError(e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine); }
        void print(string msg)
        {
            string message = "[" + System.DateTime.Now.ToLongTimeString() + "] " + msg;
            if (debugForm != null) debugForm.printDebugMsg(message);

            Console.WriteLine(message);
            using (TextWriter tw = new System.IO.StreamWriter(conf.fileLog, true)) { tw.WriteLine(message); }
        }
        void writeStateFile(string state)
        {
            using (TextWriter tw = new StreamWriter(conf.fileState, false))
            {
                tw.WriteLine(state);
            }
        }
        #endregion

        #region Read card values from files       
        void setFirstCards() { readP1(); }
        void setPlayerCard3() { readP3(); }
        void setBankerCard3() { readB3(); }

        void hReadB2Complete(object args)
        {
            ServerCallParam param = new ServerCallParam();
            param.url = string.Format(urlFirst, server_url, sessionID, iDealerID, currentGameID, cardP1, cardP2, cardB1, cardB2, _iPTotal, _iBTotal);
            param.callback = hSetFirstCardsDone;
            serverCallWorker.RunWorkerAsync(param);
        }
        void hReadP3Complete(object args) 
        {
            ServerCallParam param = new ServerCallParam();
            param.url = string.Format(urlP3, server_url, sessionID, iDealerID, currentGameID, cardP3, _iPTotal);
            param.callback = hSetPlayer3Done;
            serverCallWorker.RunWorkerAsync(param);
        }
        void hReadB3Complete(object args)
        {
            ServerCallParam param = new ServerCallParam();
            param.url = string.Format(urlB3, server_url, sessionID, iDealerID, currentGameID, cardB3, _iBTotal);
            param.callback = hSetBanker3Done;
            serverCallWorker.RunWorkerAsync(param);
        }
        void hSetFirstCardsDone(object args)
        {
            ServerCallParam param = (ServerCallParam)args;
            print(string.Format(strPrintFirsts, currentGameID, _iPCard1, _iPCard2, _iBCard1, _iBCard2, _iPTotal, _iBTotal, param.callResult));
            if (isCallOK(param.callResult))
                state_change("DEAL_FIRST_>_DONE_FIRST");
            else
                throw new Exception("Set first cards failed. Server returned #FAIL");
        }
        void hSetBanker3Done(object args)
        {
            ServerCallParam param = (ServerCallParam)args;
            print(string.Format(strPrintB3, currentGameID, _iBCard3, _iBTotal, param.callResult));
            if (isCallOK(param.callResult))
                state_change("DEAL_BANKER3_>_DONE_BANKER3");
            else
                throw new Exception("Set banker 3rd card failed. Server returned #FAIL");
        }
        void hSetPlayer3Done(object args)
        {
            ServerCallParam param = (ServerCallParam)args;
            print(string.Format(strPrintP3, currentGameID, _iPCard3, _iPTotal, param.callResult));
            if (isCallOK(param.callResult))
                state_change("DEAL_PLAYER3_>_DONE_PLAYER3");
            else
                throw new Exception("Set player 3rd card failed. Server returned #FAIL");
        }
        #endregion        

        #region Async check state
        class ServerCallParam { public string url, callResult, stateChange = string.Empty; public int retryCount = 10; public AsyncCallDelegate callback;  }
        void serverCallWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ServerCallParam param = (ServerCallParam)e.Argument;
            try
            {
                param.callResult = web_client.DownloadString(param.url);
            }
            catch
            {
                param.callResult = "ERROR";
            }
            e.Result = param;
        }
        void serverCallWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null) throw e.Error;

            ServerCallParam param = (ServerCallParam)e.Result;
            if (param.callResult.Equals("ERROR"))
            {
                if (param.retryCount > 0)
                {
                    param.retryCount--;
                    print("Problem occur when changing state, retrying..." + param.retryCount);
                    showInstruction("重試與伺服器連接中:" + param.retryCount);
                    serverCallWorker.RunWorkerAsync(param);
                }
                else
                {
                    showInstruction("與伺服器失去聯絡，請立即通知管理員。");
                    //throw new Exception("State Change Failed after " + conf.server_retry + " tries!");
                }
            }
            else if (param.callback != null)
            {
                param.callback(param);
            }
        }
        void loginDealer(string username, string password)
        {
            ServerCallParam param = new ServerCallParam();
            param.url = string.Format(urlLogin, server_url, username, password);
            param.callback = handleLogin;
            serverCallWorker.RunWorkerAsync(param);
        }
        void handleLogin(object args)
        {
            ServerCallParam param = (ServerCallParam)args;
            if (isCallOK(param.callResult))
            {
                txtUser.Enabled = false;
                txtPassword.Enabled = false;
                btnLogin.Enabled = true;
                btnLogin.Text = "登出";
                //"OK#DEALER_ID:1;SESSION_ID:0C524B4B-F170-4FD6-A500-AC70868DEFB9;"
                string[] pairs = splitPairs(param.callResult);
                iDealerID = int.Parse(splitValue(pairs[0])[1]);
                sessionID = splitValue(pairs[1])[1];
                print("SessionID:" + sessionID);
                //These line should be used in development environment only
                //in production the previous state must be PREPARE_NEXT even the previous game aborted.
                resetState();
            }
            else
            {
                MessageBox.Show("登入失敗，請重試。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                switchLoginButton(true);
                switchCloseButton(true);
            }
        }
        //for debug purpose
        void resetState()
        {
            ServerCallParam param = new ServerCallParam();
            param.url = server_url + "resetGame.aspx?tableID=" + tableID;
            param.callback = hResetState;
            serverCallWorker.RunWorkerAsync(param);
        }
        void hResetState(object args)
        {
            ServerCallParam param = (ServerCallParam)args;
            if (isCallOK(param.callResult))
            {
                print("Checking state after login...");
                checkState(hCheckState_Login_Completed);
            }
            else
            {
                print("Reset state fai!");
            }
        }
        void hCheckState_Login_Completed(object args)
        {
            ServerCallParam param = (ServerCallParam)args;
            if (isCallOK(param.callResult))
            {
                currentState = extractState(param.callResult);
                btnLogin.Text = "登出";
                switchNewGameButton(true);
                plMain.Enabled = true;
            }
            else
            {
                print("check state after login failed");
            }
        }
        void newGame()
        {
            resetGameValues();
            string gameName = getNewGameName();

            ServerCallParam param = new ServerCallParam();
            param.url = string.Format(urlNewGame, server_url, sessionID, iDealerID, tableID, gameName);
            param.callback = handleNewGame;
            serverCallWorker.RunWorkerAsync(param);
        }
        void handleNewGame(object args)
        {
            ServerCallParam param = (ServerCallParam)args;
            print("create game result:" + param.callResult);

            if (isCallOK(param.callResult))
            {
                currentGameID = int.Parse(param.callResult.Split('#')[1]);

                showInstruction("新局開始");
                wait(conf.dNewGame);
                checkState(hCheckState);
            }
            else
            {
                showInstruction("開局失敗，請再試。");
                printError("Failed to create game.");
                switchNewGameButton(true);
            }
        }               
        void checkState(AsyncCallDelegate callback = null)
        {
            ServerCallParam param = new ServerCallParam();
            param.url = string.Format(urlCheckState, server_url, sessionID, iDealerID, tableID);
            param.callback = callback;
            serverCallWorker.RunWorkerAsync(param);
        } 
        void hCheckState(object args)
        {
            ServerCallParam param = (ServerCallParam)args;
            if (isCallOK(param.callResult))
            {
                currentState = extractState(param.callResult);
                print("currentState:" + currentState);
                switch (currentState)
                {
                    case "NEW_GAME":    state_change("NEW_GAME_>_READY_BET"); break;
                    case "READY_BET":   state_change("READY_BET_>_LAST_CALL"); break;
                    case "LAST_CALL":   state_change("LAST_CALL_>_STOP_BET"); break;
                    case "STOP_BET":    state_change("STOP_BET_>_DEAL_FIRST"); break;

                    case "GAME_DONE":   state_change("GAME_DONE_>_RESULT"); break;
                    case "RESULT":      state_change("RESULT_>_ANNOUNCE"); break;
                    case "ANNOUNCE":    state_change("ANNOUNCE_>_CALC_PAY"); break;
                    case "CALC_PAY":    state_change("CALC_PAY_>_CALC_PAY_DONE"); break;
                    case "CALC_PAY_DONE":   state_change("CALC_PAY_DONE_>_PAYOUT"); break;
                    case "PAYOUT":      state_change("PAYOUT_>_PREPARE_NEXT"); break;
                    case "PREPARE_NEXT":
                        switchNewGameButton(true);
                        switchLoginButton(true);
                        break;

                    case "DEAL_FIRST": setFirstCards(); break;
                    case "DONE_FIRST": state_change("DONE_FIRST_>_WATCH_FIRST"); break;
                    case "WATCH_FIRST":
                        // Determine whether need to deal 3rd banker card or not
                        if ( (_iPTotal >= 8 || _iBTotal >= 8) || ( _iPTotal >= 6 && _iBTotal >= 6) ) 
                            state_change("WATCH_FIRST_>_GAME_DONE");
                        else
                            state_change("WATCH_FIRST_>_OPEN_FIRST");
                        break;
                    case "OPEN_FIRST":
                        // pTotal 6 - 7
                        if (_iPTotal >= 6 && _iBTotal < 6)
                            state_change("OPEN_FIRST_>_DEAL_BANKER3");
                        else // pTotal 0 - 5
                            state_change("OPEN_FIRST_>_DEAL_PLAYER3");
                        break;
                    case "DEAL_BANKER3": setBankerCard3(); break;
                    case "DONE_BANKER3": state_change("DONE_BANKER3_>_WATCH_BANKER3");  break;
                    case "WATCH_BANKER3": state_change("WATCH_BANKER3_>_GAME_DONE");  break;
                    case "DEAL_PLAYER3": setPlayerCard3(); break;
                    case "DONE_PLAYER3": state_change("DONE_PLAYER3_>_WATCH_PLAYER3"); break;
                    case "WATCH_PLAYER3":
                        if ((_iBTotal == 6 && (_iPCard3 == 6 || _iPCard3 == 7)) ||
                        (_iBTotal == 5 && (_iPCard3 >= 4 && _iPCard3 <= 7)) ||
                        (_iBTotal == 4 && (_iPCard3 >= 2 && _iPCard3 <= 7)) ||
                        (_iBTotal == 3 && (_iPCard3 != 8)) ||
                        _iBTotal <= 2)
                        {
                            state_change("WATCH_PLAYER3_>_OPEN_PLAYER3"); break; 
                        }
                        else
                        {
                            state_change("WATCH_PLAYER3_>_GAME_DONE"); break;
                        }
                    case "OPEN_PLAYER3": state_change("OPEN_PLAYER3_>_DEAL_BANKER3"); break;
                    default: break;
                }
            }                   
        }
        void state_change(string stateChange)
        {
            ServerCallParam param = new ServerCallParam();
            param.url = string.Format(urlChangeState, server_url, sessionID, iDealerID, currentGameID, "sp_state_change:" + stateChange);
            param.stateChange = stateChange;
            param.callback = hChangeState;
            serverCallWorker.RunWorkerAsync(param);
        }
        void hChangeState(object args)
        {
            ServerCallParam param = (ServerCallParam)args;
            print(string.Format(strPrintStateChange, currentGameID, param.url.Substring(param.url.IndexOf("spName=") + 7), param.callResult));
            if (isCallOK(param.callResult))
            {
                switch (param.stateChange)
                {
                    case "NEW_GAME_>_READY_BET": showMsgWaitThenCheckState("開始投注", "Allow bet, last call in " + convert2Second(conf.dAllowBet) + " seconds.", conf.dAllowBet); break;
                    case "READY_BET_>_LAST_CALL": showMsgWaitThenCheckState("最後投注", "Last call, stop bet in " + convert2Second(conf.dLastCall) + " seconds.", conf.dLastCall); break;
                    case "LAST_CALL_>_STOP_BET": showMsgWaitThenCheckState("停止投注", "Synchoizing bet information, wait...", conf.dStopBet); break;
                    case "STOP_BET_>_DEAL_FIRST": showMsgWaitThenCheckState("開始派牌", "Start dealing first two cards.", conf.dDeal1st); break;

                    case "GAME_DONE_>_RESULT": showMsgWaitThenCheckState("計算結果", "Calculating result, please wait...", conf.dResult); break;
                    case "RESULT_>_ANNOUNCE": showMsgWaitThenCheckState("公佈贏家", "Paying out, please wait...", conf.dAnnounce); break;
                    case "ANNOUNCE_>_CALC_PAY": showMsgWaitThenCheckState("計算派彩", "Calculating payout, please wait...", conf.dCalcPay); break;
                    case "CALC_PAY_>_CALC_PAY_DONE": showMsgWaitThenCheckState("計算完畢", "Calculation done, please wait...", conf.dCalcDone); break;
                    case "CALC_PAY_DONE_>_PAYOUT": showMsgWaitThenCheckState("派彩", "Paying out, please wait...", conf.dPayout); break;
                    case "PAYOUT_>_PREPARE_NEXT": showMsgWaitThenCheckState("準備新局", "Game ended. Press button again to start a new game.", conf.dPrepareNext); break;

                    case "DEAL_FIRST_>_DONE_FIRST": showMsgWaitThenCheckState("派牌完畢", "Upload succeed. Wait to ensure all client in sync.", conf.dDone1st); break;
                    case "DONE_FIRST_>_WATCH_FIRST": showMsgWaitThenCheckState("玩家睇牌", "Player can start watching cards now, please wait...", conf.dWatch1st); break;
                    case "WATCH_FIRST_>_OPEN_FIRST": showMsgWaitThenCheckState("開牌", "wait...", conf.dOpen1st); break;

                    case "DEAL_BANKER3_>_DONE_BANKER3": showMsgWaitThenCheckState("補牌完畢", "Upload succeed. Wait to ensure all client in sync.", conf.dDoneB3); break;
                    case "DONE_BANKER3_>_WATCH_BANKER3": showMsgWaitThenCheckState("玩家睇牌", "Player can start watching banker third card now, please wait...", conf.dWatchB3); break;

                    case "OPEN_FIRST_>_DEAL_PLAYER3": showMsgWaitThenCheckState("閒家補牌", "Start dealing player third card. Press button when finish.", conf.dDealP3); break;
                    case "DEAL_PLAYER3_>_DONE_PLAYER3": showMsgWaitThenCheckState("補牌完畢", "Upload succeed. Wait to ensure all client in sync.", conf.dDoneP3); break;
                    case "DONE_PLAYER3_>_WATCH_PLAYER3": showMsgWaitThenCheckState("玩家睇牌", "Player can start watching player third card now, please wait...", conf.dWatchP3); break;
                    case "WATCH_PLAYER3_>_OPEN_PLAYER3": showMsgWaitThenCheckState("開牌", "wait...", conf.dOpenP3); break;

                    case "OPEN_FIRST_>_DEAL_BANKER3":
                    case "OPEN_PLAYER3_>_DEAL_BANKER3":
                        showMsgWaitThenCheckState("莊家補牌", "Start dealing banker third card. Press button when finish.", conf.dDealB3); break;

                    case "WATCH_FIRST_>_GAME_DONE":
                    case "WATCH_BANKER3_>_GAME_DONE":
                    case "WATCH_PLAYER3_>_GAME_DONE":
                        showMsgWaitThenCheckState("牌局結束", getGameDoneString(), conf.dGameDone); break;
                    default: break;
                }
            }
            else
            {
                printError("State change returned #FAIL.");
                showInstruction("Game Terminated.");
                throw new Exception("Server returned #FAIL when attempting to change state.");
            }
        }
        void showMsgWaitThenCheckState(string instruction, string message, int delay)
        {
            showInstruction(instruction);
            print(message);
            wait(delay);
            checkState(hCheckState);
        }
        #endregion

        #region barcode from Vivian
        private void txtCard1_TextChanged(object sender, EventArgs e)
        {
            if (txtCard1.Text.Length == 4)
            {
                cardP1 = txtCard1.Text = processScannedText(txtCard1);
                print("Player card 1: " + cardP1);
                //picP1.BackgroundImage = Image.FromFile(conf.imgP1);
                _iPCard1 = getCardValue(txtCard1.Text.Substring(1));
                _iPTotal = (_iPTotal + _iPCard1) % 10;
                txtCard1.TextChanged -= txtCard1_TextChanged;
                txtCard1.Enabled = false;
                readB1();
            }
        }

        private void txtCard2_TextChanged(object sender, EventArgs e)
        {
            if (txtCard2.Text.Length == 4)
            {
                cardP2 = txtCard2.Text = processScannedText(txtCard2);
                print("Player card 2: " + cardP2);
                //picP3.BackgroundImage = Image.FromFile(conf.imgP2);
                _iPCard2 = getCardValue(txtCard2.Text.Substring(1));
                _iPTotal = (_iPTotal + _iPCard2) % 10;
                txtCard2.TextChanged -= txtCard2_TextChanged;
                txtCard2.Enabled = false;
                readB2();
            }
        }

        private void txtCard3_TextChanged(object sender, EventArgs e)
        {
            if (txtCard3.Text.Length == 4)
            {
                cardP3 = txtCard3.Text = processScannedText(txtCard3);
                print("Player card 3: " + cardP3);
                //picP3.BackgroundImage = Image.FromFile(conf.imgP3);
                _iPCard3 = getCardValue(txtCard3.Text.Substring(1));
                _iPTotal = (_iPTotal + _iPCard3) % 10;
                txtCard3.TextChanged -= txtCard3_TextChanged;
                txtCard3.Enabled = false;
                hReadP3Complete(null);
            }
        }

        private void txtCard4_TextChanged(object sender, EventArgs e)
        {
            if (txtCard4.Text.Length == 4)
            {
                cardB1 = txtCard4.Text = processScannedText(txtCard4);
                print("Banker card 1: " + cardB1);
                //picB1.BackgroundImage = Image.FromFile(conf.imgB1);
                _iBCard1 = getCardValue(txtCard4.Text.Substring(1));
                _iBTotal = (_iBTotal + _iBCard1) % 10;
                txtCard4.TextChanged -= txtCard4_TextChanged;
                txtCard4.Enabled = false;
                readP2();
            }
        }

        private void txtCard5_TextChanged(object sender, EventArgs e)
        {
            if (txtCard5.Text.Length == 4)
            {
                cardB2 = txtCard5.Text = processScannedText(txtCard5);
                print("Banker card 2: " + cardB2);
                //picB2.BackgroundImage = Image.FromFile(conf.imgB2);
                _iBCard2 = getCardValue(txtCard5.Text.Substring(1));
                _iBTotal = (_iBTotal + _iBCard2) % 10;
                txtCard5.TextChanged -= txtCard5_TextChanged;
                txtCard5.Enabled = false;
                hReadB2Complete(null);
            }
        }

        private void txtCard6_TextChanged(object sender, EventArgs e)
        {
            if (txtCard6.Text.Length == 4)
            {
                cardB3 = txtCard6.Text = processScannedText(txtCard6);
                print("Banker card 3: " + cardB3);
                //picB3.BackgroundImage = Image.FromFile(conf.imgB3);
                _iBCard3 = getCardValue(txtCard6.Text.Substring(1));
                _iBTotal = (_iBTotal + _iBCard3) % 10;
                txtCard6.TextChanged -= txtCard6_TextChanged;
                txtCard6.Enabled = false;
                hReadB3Complete(null);
            }
        }
 
        string processScannedText(TextBox txtBox)
        {
            string cardText = txtBox.Text.ToUpper().Substring(2);
            if (cardText.EndsWith("X"))
                cardText = cardText.Replace("X", "10");
            return cardText;
        }

        void readP1()
        {
            print("Reading P1...");
            showInstruction("請派閒一");
            this.txtCard1.Enabled = true;
            this.txtCard1.Focus();
            this.txtCard1.TextChanged += new System.EventHandler(this.txtCard1_TextChanged);
        }
        void readP2()
        {
            print("Reading P2...");
            showInstruction("請派閒二");
            this.txtCard2.Enabled = true;
            this.txtCard2.Focus();
            this.txtCard2.TextChanged += new System.EventHandler(this.txtCard2_TextChanged);
        }
        void readP3()
        {
            print("Reading P3...");
            showInstruction("請派閒三");
            this.txtCard3.Enabled = true;
            this.txtCard3.Focus();
            this.txtCard3.TextChanged += new System.EventHandler(this.txtCard3_TextChanged);
        }
        void readB1()
        {
            print("Reading B1...");
            showInstruction("請派莊一");
            this.txtCard4.Enabled = true;
            this.txtCard4.Focus();
            this.txtCard4.TextChanged += new System.EventHandler(this.txtCard4_TextChanged);
        }
        void readB2()
        {
            print("Reading B2...");
            showInstruction("請派莊二");
            this.txtCard5.Enabled = true;
            this.txtCard5.Focus();
            this.txtCard5.TextChanged += new System.EventHandler(this.txtCard5_TextChanged);
        }
        void readB3()
        {
            print("Reading B3...");
            showInstruction("請派莊三");
            this.txtCard6.Enabled = true;
            this.txtCard6.Focus();
            this.txtCard6.TextChanged += new System.EventHandler(this.txtCard6_TextChanged);
        }
        #endregion

    }
}
