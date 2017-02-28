using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Data.SqlClient;
using System.Net;
using com.sky88.game.bet.baccarat;

namespace com.sky88.game.bet.baccarat
{
    class SicboController
    {
        #region variable declarations
        const string USAGE_MSG = "usage: \n" +
            "\t-url server address, default localhost (e.g. 192.168.1.114/testPoker)\n" +
            "\t-t tableID - default 1\n" +
            "\t-l loop - how many games to simulate, default 100\n" +
            "\t-b amount - amount of each bet\n" +
            "\t-p playerID - default 1\n" +
            "\t-i interval - default for each state change. Default 2000 milliseconds.\n" +
            "\t-i:BET interval - additional delay for placing bet. Default 0.\n" +
            "\t-i:FIRST interval - additional delay for watching the first cards. Default 0.\n" +
            "\t-i:P3 interval - additional delay for watching the 3rd player card. Default 0.\n" +
            "\t-i:B3 interval - additional delay for watching the 3rd banker card. Default 0.\n" +
            "\t-log log file name\n";

        static WebClient web_client;
        static Random r = new Random(DateTime.Now.Millisecond);
        static bool state_change_result;
        static int currentGameID = 0;
        static int interval = 3000; // in millsecond
        static int interval_extra_bet = 0, interval_extra_first = 0, interval_extra_p3 = 0, interval_extra_b3 = 0;
        static int loop = 100, gameNum = 0;
        static int pTotal = 0, bTotal = 0, pCard1 = 0, pCard2 = 0, pCard3 = 0, bCard1 = 0, bCard2 = 0, bCard3 = 0, betAmount = 100, tableID = 1;
        static string logFilename = "log.txt";
        static string server_url = "localhost/testsicbo/dealer";
        static string username = "dealer", password = "12345";
        static int iDealerID;
        static string sessionID;
        #endregion

        static void parseParamaters(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    for (int i = 0; i < args.Length; i += 2)
                    {
                        switch (args[i])
                        {
                            case "-url": server_url = args[i + 1]; break;
                            case "-log": logFilename = args[i + 1]; break;
                            case "-i": interval = int.Parse(args[i + 1]); break;
                            case "-i:BET": interval_extra_bet = int.Parse(args[i + 1]); break;
                            case "-i:FIRST": interval_extra_first = int.Parse(args[i + 1]); break;
                            case "-i:P3": interval_extra_p3 = int.Parse(args[i + 1]); break;
                            case "-i:B3": interval_extra_b3 = int.Parse(args[i + 1]); break;
                            case "-b": betAmount = int.Parse(args[i + 1]); break;
                            case "-u": username = args[i + 1]; break;
                            case "-p": password = args[i + 1]; break;
                            case "-t": tableID = int.Parse(args[i + 1]); break;
                            case "-l": loop = int.Parse(args[i + 1]); break;
                            case "-?": printError(USAGE_MSG); Environment.Exit(0); return;
                            default: printError(USAGE_MSG); Environment.Exit(0); return;
                        }
                    }
                }
            }
            catch { printError(USAGE_MSG); Environment.Exit(0); return; }
        }
        static void Main(string[] args)
        {
            parseParamaters(args);
            web_client = new WebClient();
            server_url = "http://" + server_url + "/";
            resetState();

            if (!loginDealer()) Environment.Exit(0);

            while (gameNum < loop)
            {
                pTotal = 0;
                bTotal = 0;
                pCard1 = 0;
                pCard2 = 0;
                pCard3 = 0;
                bCard1 = 0;
                bCard2 = 0;
                bCard3 = 0;

                string createGameResult;
                if(checkCallSucceed(createGameResult = web_client.DownloadString(server_url + "newGame.aspx?sessionID="+sessionID+"&dealerID="+iDealerID+"&tableID=" + tableID + "&name=emu-t" + tableID + "-game" + gameNum)))
                {
                    currentGameID = int.Parse(createGameResult.Split('#')[1]);
                }
                else
                {
                    printError("Create game fail!");
                    break;
                }
                print("create game result:" + createGameResult);
                print("Create game: emu-t" + tableID + "-game" + gameNum + " at table:" + tableID + " -> SUCCESS");
                print("New game ID:" + currentGameID);
                wait(interval);

                if (!state_change("sp_state_change:NEW_GAME_>_READY_BET")) break;
                if (!state_change("sp_state_change:READY_BET_>_LAST_CALL")) break;
                if (!state_change("sp_state_change:LAST_CALL_>_STOP_BET")) break; else wait(interval_extra_bet);

                if (!state_change("sp_state_change:STOP_BET_>_ROLL_DICE")) break;
                if (!setDices()) break;
                if (!state_change("sp_state_change:ROLL_DICE_>_DONE_DICE")) break;
                if (!state_change("sp_state_change:DONE_DICE_>_OPEN_DICE")) break;
                if (!state_change("sp_state_change:OPEN_DICE_>_GAME_DONE")) break;
#region old
                /*
                if (!state_change("sp_state_change:STOP_BET_>_DEAL_FIRST")) break;
                if (!setFirstCards()) break;
                // apply player third card logic

                if (!state_change("sp_state_change:DEAL_FIRST_>_DONE_FIRST")) break;
                if (!state_change("sp_state_change:DONE_FIRST_>_WATCH_FIRST")) break; else wait(interval_extra_first);

                if (pTotal >= 8 || bTotal >= 8) // pTotal 8 - 9
                {
                    if (!state_change("sp_state_change:WATCH_FIRST_>_GAME_DONE")) break;
                }
                else if (pTotal == 6 || pTotal == 7) // pTotal 6 - 7
                {
                    if (bTotal < 6)
                    {
                        if (!state_change("sp_state_change:WATCH_FIRST_>_OPEN_FIRST")) break;
                        if (!state_change("sp_state_change:OPEN_FIRST_>_DEAL_BANKER3")) break;
                        if (!setBankerCard3()) break;
                        if (!state_change("sp_state_change:DEAL_BANKER3_>_DONE_BANKER3")) break;
                        if (!state_change("sp_state_change:DONE_BANKER3_>_WATCH_BANKER3")) break; else wait(interval_extra_b3);
                        if (!state_change("sp_state_change:WATCH_BANKER3_>_GAME_DONE")) break;
                    }
                    else
                    {
                        if (!state_change("sp_state_change:WATCH_FIRST_>_GAME_DONE")) break;
                    }
                }
                else // pTotal 0 - 5
                {
                    if (!state_change("sp_state_change:WATCH_FIRST_>_OPEN_FIRST")) break;
                    if (!state_change("sp_state_change:OPEN_FIRST_>_DEAL_PLAYER3")) break;
                    if (!setPlayerCard3()) break;
                    if (!state_change("sp_state_change:DEAL_PLAYER3_>_DONE_PLAYER3")) break;
                    if (!state_change("sp_state_change:DONE_PLAYER3_>_WATCH_PLAYER3")) break; else wait(interval_extra_p3);
                        
                    if ((bTotal == 6 && (pCard3 == 6 || pCard3 == 7)) ||
                        (bTotal == 5 && (pCard3 >= 4 && pCard3 <= 7)) ||
                        (bTotal == 4 && (pCard3 >= 2 && pCard3 <= 7)) ||
                        (bTotal == 3 && (pCard3 != 8)) || bTotal <= 2)
                    {
                        if (!state_change("sp_state_change:WATCH_PLAYER3_>_OPEN_PLAYER3")) break;
                        if (!state_change("sp_state_change:OPEN_PLAYER3_>_DEAL_BANKER3")) break;
                        if (!setBankerCard3()) break;
                        if (!state_change("sp_state_change:DEAL_BANKER3_>_DONE_BANKER3")) break;
                        if (!state_change("sp_state_change:DONE_BANKER3_>_WATCH_BANKER3")) break; else wait(interval_extra_b3);
                        if (!state_change("sp_state_change:WATCH_BANKER3_>_GAME_DONE")) break;
                    }
                    else
                    {
                        if (!state_change("sp_state_change:WATCH_PLAYER3_>_GAME_DONE")) break;
                    }
                }*/
#endregion
                // no more need to set value at the end, as each time set card will also provide the values
                //if (!setGameValues()) break;
                if (!state_change("sp_state_change:GAME_DONE_>_RESULT")) break;
                if (!state_change("sp_state_change:RESULT_>_ANNOUNCE")) break;
                if (!state_change("sp_state_change:ANNOUNCE_>_CALC_PAY")) break;
                if (!state_change("sp_state_change:CALC_PAY_>_CALC_PAY_DONE")) break;
                if (!state_change("sp_state_change:CALC_PAY_DONE_>_PAYOUT")) break;
                if (!state_change("sp_state_change:PAYOUT_>_PREPARE_NEXT")) break;
            }
            gameNum++;
        }

        static bool resetState()
        {
            if (checkCallSucceed(web_client.DownloadString(server_url + "/resetGame.aspx?tableID=" + tableID)))
            {
                print("Reset game state back to PREPARE_NEXT succeed.");
                return true;
            }
            else
            {
                printError("Reset game state failed.");
                return false;
            }
        }
        static bool state_change(string spName)
        {
            state_change_result = checkCallSucceed(web_client.DownloadString(server_url + "stateChange.aspx?sessionID=" + sessionID + "&dealerID=" + iDealerID + "&gameID=" + currentGameID + "&spName=" + spName));
            print("gameID: " + currentGameID + " " + spName + " -> " + state_change_result);
            wait(interval);
            return state_change_result;
        }
        static bool loginDealer()
        {
            string result = string.Empty;
            result = web_client.DownloadString(server_url + "login.aspx?username=" + username + "&password=" + password);

            if (checkCallSucceed(result))
            {
                string[] pairs = splitPairs(result);
                iDealerID = int.Parse(splitValue(pairs[0])[1]);
                sessionID = splitValue(pairs[1])[1];
                print("dealer ID:" + iDealerID + " login success. session:" + sessionID);
                return true;
            }
            else
            {
                print("dealer login failed.");
                return false;
            }
        }
        static bool setDices()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            int d1 = rnd.Next() % 6 + 1;
            int d2 = rnd.Next() % 6 + 1;
            int d3 = rnd.Next() % 6 + 1;
            string param = "sessionID=" + sessionID +
                "&dealerID=" + iDealerID +
                "&gameID=" + currentGameID +
                "&d1={0}&d2={1}&d3={2}";
            param = String.Format(param, d1, d2, d3);
            state_change_result = checkCallSucceed(web_client.DownloadString(server_url + "setDice.aspx?" + param));
            wait(interval);
            print("set dices: " + param + " -> " + state_change_result);
            return state_change_result;
        }
        static bool setFirstCards()
        {
            string pr1, pc1, pr2, pc2, br1, bc1, br2, bc2;
            pr1 = getRank();
            pr2 = getRank();
            br1 = getRank();
            br2 = getRank();
            pc1 = getCard(); 
            pc2 = getCard(); 
            bc1 = getCard(); 
            bc2 = getCard(); 
            pCard1 = getValueByCard(pc1);
            pCard2 = getValueByCard(pc2);
            bCard1 = getValueByCard(bc1);
            bCard2 = getValueByCard(bc2);
            pTotal = (pCard1 + pCard2) % 10;
            bTotal = (bCard1 + bCard2) % 10;
            string param = "sessionID=" + sessionID + 
                "&dealerID=" + iDealerID +
                "&gameID=" + currentGameID + 
                "&p1=" + pr1 + pc1 + "&p2=" + pr2 + pc2 + 
                "&b1=" + br1 + bc1 + "&b2=" + br2 + bc2 + 
                "&pt=" + pTotal + "&bt=" + bTotal;
            state_change_result = checkCallSucceed(web_client.DownloadString(server_url + "setFirstCards.aspx?" + param));
            print("gameID: " + currentGameID + " set first cards: pr1:" + pr1 + " pc1:" + pc1 + " pr2:" + pr2 + " pc2:" + pc2 + " br1:" + br1 + " bc1:" + bc1 + " br2:" + br2 + " bc2:" + bc2 + " -> " + state_change_result);
            wait(interval);
            return state_change_result;
        }
        static bool setPlayerCard3()
        {
            string pr3, pc3;
            pr3 = getRank();
            pc3 = getCard();
            pCard3 = getValueByCard(pc3);
            pTotal = (pTotal + pCard3) % 10;
            string param = "sessionID=" + sessionID +
                "&dealerID=" + iDealerID +
                "&gameID=" + currentGameID +
                "&p3=" + pr3 + pc3 +
                "&pt=" + pTotal;
            state_change_result = checkCallSucceed(web_client.DownloadString(server_url + "setPlayerCard3.aspx?" + param));
            print("gameID: " + currentGameID + " set player 3rd card: pr3:" + pr3 + " pc3:" + pc3 + " pt:" + pTotal + " -> " + state_change_result);
            wait(interval);
            return state_change_result;
        }
        static bool setBankerCard3()
        {
            string br3, bc3;
            br3 = getRank();
            bc3 = getCard();
            bCard3 = getValueByCard(bc3);
            bTotal = (bTotal + bCard3) % 10;
            string param = "sessionID=" + sessionID +
                "&dealerID=" + iDealerID +
                "&gameID=" + currentGameID +
                "&b3=" + br3 + bc3 +
                "&bt=" + bTotal;
            state_change_result = checkCallSucceed(web_client.DownloadString(server_url + "setBankerCard3.aspx?" + param));
            print("gameID: " + currentGameID + " set banker 3rd card: br3:" + br3 + " bc3:" + bc3 + " bt:" + bTotal + " -> " + state_change_result);
            wait(interval);
            return state_change_result;
        }
        /*
        static bool setGameValues()
        {
            string param = "sessionID=" + sessionID +
                "&dealerID=" + iDealerID +
                "&gameID=" + currentGameID + 
                "&bt=" + bTotal + 
                "&pt=" + pTotal;
            state_change_result = checkCallSucceed(web_client.DownloadString(server_url + "setTotal.aspx?" + param));
            print("gameID: " + currentGameID + " set total: player:" + pTotal + " banker:" + bTotal + " -> " + state_change_result);
            wait(interval);
            return state_change_result;
        }
        */
        #region Utility Functions
        static int getValueByCard(string card)
        {
            switch (card)
            {
                case "A": return 1;
                case "2": return 2;
                case "3": return 3;
                case "4": return 4;
                case "5": return 5;
                case "6": return 6;
                case "7": return 7;
                case "8": return 8;
                case "9": return 9;
                case "10": return 0;
                case "J": return 0;
                case "Q": return 0;
                case "K": return 0;
                default: return 0;
            }
        }
        static string getCard()
        {
            switch (r.Next() % 13)
            {
                case 0: return "A";
                case 1: return "2";
                case 2: return "3";
                case 3: return "4";
                case 4: return "5";
                case 5: return "6";
                case 6: return "7";
                case 7: return "8";
                case 8: return "9";
                case 9: return "10";
                case 10: return "J";
                case 11: return "Q";
                case 12: return "K";
                default: return "A";
            }
        }
        static string getRank()
        {
            switch (r.Next() % 4)
            {
                case 0: return "S";
                case 1: return "H";
                case 2: return "C";
                case 3: return "D";
                default: return "Spade";
            }
        }
        const string OK = "OK#";
        static bool checkCallSucceed(string result)
        {
            return result.StartsWith(OK);
        }
        static string[] splitPairs(string input)
        {
            return input.Substring(input.IndexOf(OK) + 3).Split(';');
        }
        static string[] splitValue(string input)
        {
            return input.Split(':');
        }
        static void wait(int time)
        {
            System.Threading.Thread.Sleep(time);
        }
        static void printError(string msg)
        {
            print("[ERROR] "+msg);
        }
        static void print(string msg)
        {
            string message = "[" + System.DateTime.Now.ToLongTimeString() + "] " + msg;
            Console.WriteLine(message);

            TextWriter tw = new System.IO.StreamWriter(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/" + logFilename, true);
            tw.WriteLine(message);
            tw.Close();
        }
        #endregion
    }
}
