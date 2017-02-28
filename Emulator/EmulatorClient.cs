using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Data.SqlClient;
using System.Net;
using System.Configuration;

namespace com.sky88games.bet.Emulator
{
	public abstract class EmulatorClient : EmulatorBase
    {
        #region variable declarations
        //variables from config file
        protected readonly int betAmount;

        //variables NOT from config file
        protected string betSlip;
        protected bool betSucceed;
        
        Decimal balance = 0;
        string previousState;

        const string check_state_url = "common/checkState.aspx?";
        const string get_game_id_url = "client/core/getGameID.aspx?";
        const string get_balance_url = "client/core/getBalance.aspx?";
        const string get_payout_url = "client/core/getPayout.aspx?";
        const string place_bet_url = "client/core/placeBet.aspx?betslips={0}";
        #endregion

        protected virtual void perform_pre_game_logic() {}
        protected abstract void handleGameSpecificState();
        protected abstract void genBetSlips();

        public EmulatorClient() : base()
        {
            try
            {
                betAmount = int.Parse(ConfigurationManager.AppSettings["betAmount"]);
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid config file value:" + Environment.NewLine +
                    e.Message + Environment.NewLine + e.StackTrace);
            } 
        }
        protected override void resetValues()
        {
            betSlip = string.Empty;
            betSucceed = false;
        }
        public override void run()
        {
            perform_application_start_logic();
            if (!webStrSucceed(get_balance_url))
                exit();
            else
                balance = Decimal.Parse(splitValue(splitPairs(webstr_result)[0])[1]);

            perform_pre_game_logic();

            bool sync = false;
            print("waiting new game start to sync...");
            do
            {
                if (!webStrSucceed(check_state_url))
                    exit();
                else
                {
                    string tmpState = parseGameState(webstr_result);
                    if (tmpState.Equals("NEW_GAME"))
                    {
                        if (!webStrSucceed(get_game_id_url)) exit();
                        currentGameID = int.Parse(webstr_result.Split('#')[1].Split(';')[0].Split(':')[1]);
                        currentState = "NEW_GAME";
                        gameNum++;
                        sync = true;
                    }
                    wait(interval);
                }
            } while(!sync);

            while (gameNum < loop && balance >= betAmount)
            {
                if (!webStrSucceed(check_state_url))
                    exit();
                else
                {
                    currentState = parseGameState(webstr_result);
                    if (!currentState.Equals(previousState))
                        handleGameState();
                    wait(interval);
                }
            }
            print(loop + " games performed successfully, exiting program...");
        }
        void handleGameState()
        {
            switch (currentState)
            {
                case "NEW_GAME":
                    if (!webStrSucceed(get_game_id_url)) exit();
                    currentGameID = int.Parse(webstr_result.Split('#')[1].Split(';')[0].Split(':')[1]);
                    gameNum++;
                    break;
                case "STOP_BET":
                    if (!betSucceed)
                    {
                        genBetSlips();
                        if (!webStrSucceed(String.Format(place_bet_url, betSlip))) exit();
                        betSucceed = true;
                    }
                    break;
                case "CALC_PAY_DONE": 
                    print("betSucceed:" + betSucceed); 
                    if (betSucceed) 
                    {
                        if (!webStrSucceed(get_payout_url)) exit();

                        string[] pairs = splitPairs(webstr_result);
                        string output = string.Empty;
                        for (int i = 0; i < pairs.Length; i++)
                        {
                            if (pairs[i].StartsWith("BALANCE"))
                            {
                                string value = pairs[i].Split(':')[1];
                                balance = Decimal.Parse(value.Substring(0, value.Length - 1));
                                break;
                            }
                        }
                    } 
                    break;
                case "PREPARE_NEXT": resetValues(); break;
                default: handleGameSpecificState(); break;
            }
            previousState = currentState;
        }
        
        #region Utility funcitons
        string parseGameState(string stateResult)
        {
            string[] values = splitPairs(stateResult);
            for (int i = 0; i < values.Length; i++)
            {
                string[] entry = splitValue(values[i]);
                if (entry[0].Equals("GAME_STATE"))
                    return entry[1];
            }
            return string.Empty;
        }
        #endregion
    }
}