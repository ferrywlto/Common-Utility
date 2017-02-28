using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.sky88games.bet.Emulator
{
    public class EmulatorClientSicbo : EmulatorClient
    {
        #region variable declarations
        //variables from config file

        //variables NOT from config file
        const string get_dice_url = "client/sicbo/getDices.aspx?";

        #endregion
        protected override void handleGameSpecificState()
        {
            switch (currentState)
            {
                case "DONE_DICE":
                    if (!webStrSucceed(get_dice_url)) exit();
                    break;
                default: break;
            }
        }
        #region Utility funcitons
        protected override void genBetSlips()
        {
            for (int i = 0; i < (rand.Next() % 3) + 1; i++)
            {
                switch (rand.Next() % 52)
                {
                    case 0: appendBetSlip("1AND2"); break;
                    case 1: appendBetSlip("1AND3"); break;
                    case 2: appendBetSlip("1AND4"); break;
                    case 3: appendBetSlip("1AND5"); break;
                    case 4: appendBetSlip("1AND6"); break;
                    case 5: appendBetSlip("2AND3"); break;
                    case 6: appendBetSlip("2AND4"); break;
                    case 7: appendBetSlip("2AND5"); break;
                    case 8: appendBetSlip("2AND6"); break;
                    case 9: appendBetSlip("3AND4"); break;
                    case 10: appendBetSlip("3AND5"); break;
                    case 11: appendBetSlip("3AND6"); break;
                    case 12: appendBetSlip("4AND5"); break;
                    case 13: appendBetSlip("4AND6"); break;
                    case 14: appendBetSlip("5AND6"); break;
                    case 15: appendBetSlip("BIG"); break;
                    case 16: appendBetSlip("DOUBLE1"); break;
                    case 17: appendBetSlip("DOUBLE2"); break;
                    case 18: appendBetSlip("DOUBLE3"); break;
                    case 19: appendBetSlip("DOUBLE4"); break;
                    case 20: appendBetSlip("DOUBLE5"); break;
                    case 21: appendBetSlip("DOUBLE6"); break;
                    case 22: appendBetSlip("EVEN"); break;
                    case 23: appendBetSlip("ODD"); break;
                    case 24: appendBetSlip("SINGLE1"); break;
                    case 25: appendBetSlip("SINGLE2"); break;
                    case 26: appendBetSlip("SINGLE3"); break;
                    case 27: appendBetSlip("SINGLE4"); break;
                    case 28: appendBetSlip("SINGLE5"); break;
                    case 29: appendBetSlip("SINGLE6"); break;
                    case 30: appendBetSlip("SMALL"); break;
                    case 31: appendBetSlip("TOTAL10"); break;
                    case 32: appendBetSlip("TOTAL11"); break;
                    case 33: appendBetSlip("TOTAL12"); break;
                    case 34: appendBetSlip("TOTAL13"); break;
                    case 35: appendBetSlip("TOTAL14"); break;
                    case 36: appendBetSlip("TOTAL15"); break;
                    case 37: appendBetSlip("TOTAL16"); break;
                    case 38: appendBetSlip("TOTAL17"); break;
                    case 39: appendBetSlip("TOTAL4"); break;
                    case 40: appendBetSlip("TOTAL5"); break;
                    case 41: appendBetSlip("TOTAL6"); break;
                    case 42: appendBetSlip("TOTAL7"); break;
                    case 43: appendBetSlip("TOTAL8"); break;
                    case 44: appendBetSlip("TOTAL9"); break;
                    case 45: appendBetSlip("TRIPLE"); break;
                    case 46: appendBetSlip("TRIPLE1"); break;
                    case 47: appendBetSlip("TRIPLE2"); break;
                    case 48: appendBetSlip("TRIPLE3"); break;
                    case 49: appendBetSlip("TRIPLE4"); break;
                    case 50: appendBetSlip("TRIPLE5"); break;
                    case 51: appendBetSlip("TRIPLE6"); break;
                    default: break;
                }
            }
        }
        public void appendBetSlip(string betType)
        {
            betSlip += betType +":"+ betAmount + ";";
        }
        #endregion
    }
}
