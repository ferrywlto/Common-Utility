using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.sky88games.bet.Emulator
{
    public class EmulatorDealerSicbo : EmulatorDealer
    {
        #region variable declarations
        int d1, d2, d3;

        const string set_dices_url = "dealer/sicbo/setDices.aspx?d1={0}&d2={1}&d3={2}";

        #endregion
        protected override string calc_payout_url
        {
            get { return "dealer/sicbo/payout.aspx?"; }
        }
        protected override string create_game_url
        {
            get { return "dealer/core/newGame.aspx?gameName={0}&gameType=sicbo"; }
        }
        protected override void resetValues()
        {
            d1 = d2 = d3 = 0;
        }
        protected override void perform_actual_game_logic()
        {
            if (!state_change("ROLL_DICE")) exit();
            rollDices();
            if (!webStrSucceed(String.Format(set_dices_url, d1, d2, d3))) exit();
            if (!state_change("DONE_DICE")) exit();
            if (!state_change("OPEN_DICE")) exit();
            if (!state_change("GAME_DONE")) exit();
        }
        void rollDices()
        {
            d1 = rand.Next() % 6 + 1;
            d2 = rand.Next() % 6 + 1;
            d3 = rand.Next() % 6 + 1;
        }
    }
}
