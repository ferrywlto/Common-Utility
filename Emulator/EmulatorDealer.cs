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
using com.sky88games.bet;

namespace com.sky88games.bet.Emulator
{
    public abstract class EmulatorDealer : EmulatorBase
    {
        #region variable declarations
        //variables from config file
        readonly int interval_place_bet;

        //variables NOT from config file
        const string reset_game_url = "dealer/core/reset.aspx?";
        const string state_change_url = "dealer/core/stateChange.aspx?newState={0}";
        #endregion

        public EmulatorDealer() : base()
        {
            try
            {
                interval_place_bet = int.Parse(ConfigurationManager.AppSettings["interval_place_bet"]);
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid config file value:" + Environment.NewLine +
                    e.Message + Environment.NewLine + e.StackTrace);
            } 
        }

        public override void run()
        {
            perform_application_start_logic();
            if (!webStrSucceed(reset_game_url)) exit();
            while (gameNum < loop)
            {
                resetValues();
                perform_pre_game_logic();
                perform_actual_game_logic();
                perform_end_game_logic();
            }
            gameNum++;
        }
        protected abstract string create_game_url { get; }
        protected abstract string calc_payout_url { get; }
        protected abstract void perform_actual_game_logic();
       
        protected void perform_pre_game_logic()
        {
            if (!createNewGame()) exit();
            if (!state_change("READY_BET")) exit();
            if (!state_change("LAST_CALL")) exit();
            if (!state_change("STOP_BET")) exit();
            wait(interval_place_bet);
        }
        protected void perform_end_game_logic()
        {
            if (!state_change("RESULT")) exit();
            if (!state_change("ANNOUNCE")) exit();
            if (!state_change("CALC_PAY")) exit();
            if (!webStrSucceed(calc_payout_url)) exit();
            if (!state_change("CALC_PAY_DONE")) exit();
            if (!state_change("PAYOUT")) exit();
            if (!state_change("PREPARE_NEXT")) exit();
        }
        
        protected bool state_change(string newState)
        {
            bool succeed = webStrSucceed(String.Format(state_change_url, newState));
            if (succeed)
                currentState = newState;
            wait(interval);
            return succeed;
        }

        bool createNewGame()
        {
            webstring(String.Format(create_game_url, "emu-game" + gameNum));
            bool succeed = checkCallSucceed();
            if (succeed)
            {
                currentGameID = int.Parse(webstr_result.Split('#')[1].Split(';')[0].Split(':')[1]);
                currentState = "NEW_GAME";
            }
            wait(interval);
            return succeed;
        }
    }
}
