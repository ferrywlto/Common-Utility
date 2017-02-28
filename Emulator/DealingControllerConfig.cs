using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace com.sky88.game.bet.baccarat
{
    class DealingControllerConfig
    {
        const string
            CONF_KEY_TABLEID = "tableID",
            CONF_KEY_FILEPATH = "cardFileFolderPath",
            CONF_KEY_P1 = "playerCard1File",
            CONF_KEY_P2 = "playerCard2File",
            CONF_KEY_P3 = "playerCard3File",
            CONF_KEY_B1 = "bankerCard1File",
            CONF_KEY_B2 = "bankerCard2File",
            CONF_KEY_B3 = "bankerCard3File",
            CONF_KEY_LOG = "logFile",
            CONF_KEY_STATE = "stateFile",
            CONF_KEY_SERVER_URL = "server_url",
            CONF_KEY_SERVER_RETRY = "server_retry";

        public string imgNC, fileState, fileLog, server_url,
            fileP1, fileP2, fileP3, 
            fileB1, fileB2, fileB3, 
            imgP1, imgP2, imgP3,
            imgB1, imgB2, imgB3;

        public int server_retry, tableID, dDefault, dAllowBet,
            dNewGame, dLastCall, dStopBet, dSyncBet,
            dDeal1st, dDone1st, dWatch1st, dOpen1st,
            dDealP3, dDoneP3, dWatchP3, dOpenP3,
            dDealB3, dDoneB3, dWatchB3, dOpenB3,
            dGameDone, dResult, dAnnounce, dCalcPay,
            dCalcDone, dPayout, dPrepareNext, dCardRead;

        private static DealingControllerConfig self;

        public static DealingControllerConfig getInstance()
        {
            if (self == null)
                self = new DealingControllerConfig();
            return self;
        }

        private DealingControllerConfig()
        {
            checkApplicationSetting();
        }
        
        string getCardFile(string card) { return sSetting("card_value_" + card); }
        string getCardImage(string card) { return sSetting("image_" + card); }
        int getDelay(string state = "") { return iSetting("delay_" + state); }
        int iSetting(string key) { return int.Parse(ConfigurationManager.AppSettings[key]); }
        string sSetting(string key) { return ConfigurationManager.AppSettings[key]; }
        void setupDelayValues()
        {
            dDefault = getDelay("default");
            dNewGame = getDelay("new_game");
            dAllowBet = getDelay("allow_bet");
            dLastCall = getDelay("last_call");
            dStopBet = getDelay("stop_bet");
            dSyncBet = getDelay("sync_bet");
            dDeal1st = getDelay("deal_1st");
            dDone1st = getDelay("done_1st");
            dWatch1st = getDelay("watch_1st");
            dOpen1st = getDelay("open_1st");
            dDealP3 = getDelay("deal_p3");
            dDoneP3 = getDelay("done_p3");
            dWatchP3 = getDelay("watch_p3");
            dOpenP3 = getDelay("open_p3");
            dDealB3 = getDelay("deal_b3");
            dDoneB3 = getDelay("done_b3");
            dWatchB3 = getDelay("watch_b3");
            dOpenB3 = getDelay("open_b3");
            dGameDone = getDelay("game_done");
            dResult = getDelay("result");
            dAnnounce = getDelay("announce");
            dCalcPay = getDelay("calc_pay");
            dCalcDone = getDelay("calc_done");
            dPayout = getDelay("payout");
            dPrepareNext = getDelay("prepare_next");
            dCardRead = getDelay("card_read");
        }
        void checkApplicationSetting()
        {
            try
            {
                server_url = sSetting(CONF_KEY_SERVER_URL);
                server_retry = iSetting(CONF_KEY_SERVER_RETRY);
                tableID = iSetting(CONF_KEY_TABLEID);

                fileState = sSetting(CONF_KEY_STATE);
                fileLog = sSetting(CONF_KEY_LOG);
                fileP1 = getCardFile("p1");
                fileP2 = getCardFile("p2");
                fileP3 = getCardFile("p3");
                fileB1 = getCardFile("b1");
                fileB2 = getCardFile("b2");
                fileB3 = getCardFile("b3");
                
                imgP1 = getCardImage("p1");
                imgP2 = getCardImage("p2");
                imgP3 = getCardImage("p3");
                imgB1 = getCardImage("b1");
                imgB2 = getCardImage("b2");
                imgB3 = getCardImage("b3");
                imgNC = getCardImage("nc");

                List<String> files = new List<string>();
                files.Add(fileState);
                files.Add(fileLog);
                files.Add(fileP1);
                files.Add(fileP2);
                files.Add(fileP3);
                files.Add(fileB1);
                files.Add(fileB2);
                files.Add(fileB3);
                files.Add(imgP1);
                files.Add(imgP2);
                files.Add(imgP3);
                files.Add(imgB1);
                files.Add(imgB2);
                files.Add(imgB3);
                files.Add(imgNC);

                for (int i = 0; i < files.Count; i++)
                {
                    if(String.IsNullOrEmpty(files[i])) 
                        throw new Exception("Invaild config values.");
                    else if (!File.Exists(files[i]))
                        throw new Exception("File " + files[i] + " exists!");
                }

                setupDelayValues();
            }
            catch
            {
                throw new Exception("Invaild config values.");
            }
        }
    }
}
