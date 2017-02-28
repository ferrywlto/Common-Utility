using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.sky88games.bet.Emulator
{
    public class EmulatorClientBaccarat : EmulatorClient
    {
        #region variable declarations
        //variables from config file

        //variables NOT from config file
        const string get_result_url = "client/baccarat/getResult.aspx?";
        const string get_first_cards_url = "client/baccarat/getFirstCards.aspx?";
        const string get_player_card3_url = "client/baccarat/getPlayerCard3.aspx?";
        const string get_banker_card3_url = "client/baccarat/getBankerCard3.aspx?";
        const string get_history_bead_url = "client/baccarat/getHistoryBead.aspx?";
        const string get_history_big_url = "client/baccarat/getHistoryBig.aspx?";

        #endregion

        protected override void perform_pre_game_logic()
        {
            if (!webStrSucceed(get_history_bead_url)) exit();

            if (!webStrSucceed(get_history_big_url)) exit();

        }

        protected override void handleGameSpecificState()
        {
            switch (currentState)
            {
                case "RESULT":
                    if (!webStrSucceed(get_result_url)) exit();
                    break;
                case "DONE_FIRST":
                    if (!webStrSucceed(get_first_cards_url)) exit();
                    break;
                case "DONE_PLAYER3":
                    if (!webStrSucceed(get_player_card3_url)) exit();
                    break;
                case "DONE_BANKER3":
                    if (!webStrSucceed(get_banker_card3_url)) exit();
                    break;
                default: break;
            }
        }
        #region Utility funcitons
        protected override void genBetSlips()
        {
            for (int i = 0; i < (rand.Next() % 3) + 1; i++)
            {
                switch (rand.Next() % 5)
                {
                    case 0: betSlip += "BANKER:" + betAmount + ";"; break;
                    case 1: betSlip += "PLAYER:" + betAmount + ";"; break;
                    case 2: betSlip += "BANKER_PAIR:" + betAmount + ";"; break;
                    case 3: betSlip += "PLAYER_PAIR:" + betAmount + ";"; break;
                    case 4: betSlip += "TIE:" + betAmount + ";"; break;
                    default: break;
                }
            }
        }
        #endregion
    }
}
