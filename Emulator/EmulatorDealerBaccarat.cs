using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace com.sky88games.bet.Emulator
{
    public class EmulatorDealerBaccarat : EmulatorDealer
    {
        #region variable declarations
        readonly int interval_watch_first, interval_watch_p3, interval_watch_b3;

        int pTotal, bTotal, 
            pCard1, pCard2, pCard3, 
            bCard1, bCard2, bCard3;

        string p1, p2, p3, b1, b2, b3;

        const string first_card_url = "dealer/baccarat/setFirstCards.aspx?p1={0}&p2={1}&b1={2}&b2={3}&pt={4}&bt={5}";
        const string player_card3_url = "dealer/baccarat/setPlayerCard3.aspx?p3={0}&pt={1}";
        const string banker_card3_url = "dealer/baccarat/setBankerCard3.aspx?b3={0}&bt={1}";
        const string calc_result_url = "dealer/baccarat/calcResult.aspx?";

        #endregion

        public EmulatorDealerBaccarat() : base()
        {
            interval_watch_first = int.Parse(ConfigurationManager.AppSettings["interval_watch_first"]);
            interval_watch_p3 = int.Parse(ConfigurationManager.AppSettings["interval_watch_p3"]);
            interval_watch_b3 = int.Parse(ConfigurationManager.AppSettings["interval_watch_b3"]);
        }

        protected override void resetValues()
        {
            pCard1 = pCard2 = pCard3 =
            bCard1 = bCard2 = bCard3 =
            pTotal = bTotal = 0;
            p1 = p2 = p3 = b1 = b2 = b3 = string.Empty;
        }
        protected override string calc_payout_url
        {
            get { return "dealer/baccarat/payout.aspx?"; }
        }
        protected override string create_game_url
        {
            get { return "dealer/core/newGame.aspx?gameName={0}&gameType=baccarat"; }
        }
        protected override void perform_actual_game_logic()
        {
            perform_first_cards_logic();

            if (pTotal >= 8 || bTotal >= 8) // pTotal 8 - 9
            {
                if (!state_change("GAME_DONE")) exit();
            }
            else if (pTotal == 6 || pTotal == 7) // pTotal 6 - 7
            {
                if (bTotal < 6)
                {
                    if (!state_change("OPEN_FIRST")) exit();
                    perform_banker_card3_logic();
                }
                else if (!state_change("GAME_DONE")) exit();
            }
            else // pTotal 0 - 5
            {
                if (!state_change("OPEN_FIRST")) exit();
                perform_player_card3_logic();

                if (needBankerCard3AfterPlayerCard3())
                {
                    if (!state_change("OPEN_PLAYER3")) exit();
                    perform_banker_card3_logic();
                }
                else if (!state_change("GAME_DONE")) exit();
            }
            if (!webStrSucceed(calc_result_url)) exit();
        }
        void perform_first_cards_logic()
        {
            if (!state_change("DEAL_FIRST")) exit(); else getFirstCards();
            if (!webStrSucceed(String.Format(first_card_url, p1, p2, b1, b2, pTotal, bTotal))) exit();
            if (!state_change("DONE_FIRST")) exit();
            if (!state_change("WATCH_FIRST")) exit();
            wait(interval_watch_first);
        }
        void perform_player_card3_logic()
        {
            if (!state_change("DEAL_PLAYER3")) exit(); else getPlayerCard3();
            if (!webStrSucceed(String.Format(player_card3_url, p3, pTotal))) exit();
            if (!state_change("DONE_PLAYER3")) exit();
            if (!state_change("WATCH_PLAYER3")) exit();
            wait(interval_watch_p3);
        }
        void perform_banker_card3_logic()
        {
            if (!state_change("DEAL_BANKER3")) exit(); else getBankerCard3();
            if (!webStrSucceed(String.Format(banker_card3_url, b3, bTotal))) exit();
            if (!state_change("DONE_BANKER3")) exit();
            if (!state_change("WATCH_BANKER3")) exit();
            wait(interval_watch_b3);
            if (!state_change("GAME_DONE")) exit();
        }
        void getFirstCards()
        {
            p1 = getSuit() + getRank();
            p2 = getSuit() + getRank();
            b1 = getSuit() + getRank();
            b2 = getSuit() + getRank();

            pCard1 = getValueByCard(p1.Substring(1));
            pCard2 = getValueByCard(p2.Substring(1));
            bCard1 = getValueByCard(b1.Substring(1));
            bCard2 = getValueByCard(b2.Substring(1));
            pTotal = (pCard1 + pCard2) % 10;
            bTotal = (bCard1 + bCard2) % 10;
        }
        void getPlayerCard3()
        {
            p3 = getSuit() + getRank();
            pCard3 = getValueByCard(p3.Substring(1));
            pTotal = (pTotal + pCard3) % 10;
        }
        void getBankerCard3()
        {
            b3 = getSuit() + getRank();
            bCard3 = getValueByCard(b3.Substring(1));
            bTotal = (bTotal + bCard3) % 10;
        }
        #region Utility Functions
        bool needBankerCard3AfterPlayerCard3()
        {
            return
            (bTotal == 6 && (pCard3 == 6 || pCard3 == 7)) ||
            (bTotal == 5 && (pCard3 >= 4 && pCard3 <= 7)) ||
            (bTotal == 4 && (pCard3 >= 2 && pCard3 <= 7)) ||
            (bTotal == 3 && (pCard3 != 8)) || bTotal <= 2;
        }
        int getValueByCard(string card)
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
        string getRank()
        {
            switch (rand.Next() % 13)
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
        string getSuit()
        {
            switch (rand.Next() % 4)
            {
                case 0: return "S";
                case 1: return "H";
                case 2: return "C";
                case 3: return "D";
                default: return "Spade";
            }
        }
        #endregion
    }
}
