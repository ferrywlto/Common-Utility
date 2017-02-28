using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
namespace com.sky88.game.bet.baccarat
{
    public class BaccaratStoredProcedureLocator
    {
        private static BaccaratStoredProcedureLocator self = null;
        public static SqlConnection sqlConn = null;
		private const string paramDelimiters = ";";
        //private const string rowDelimiters = ";";
        private List<SqlCommand> commands = null;
        private SqlCommand
			sp_action_player_get_id_by_login,
			sp_action_player_get_table_list,
			sp_action_player_get_player_info,
			sp_action_player_get_balance,
			sp_action_player_get_state,
			sp_action_player_get_first_cards,
			sp_action_player_get_player_card3,
			sp_action_player_get_banker_card3,
			sp_action_player_place_bet,
			sp_action_player_get_result,
			sp_action_player_get_payout,
			sp_action_player_get_game_id,

			sp_action_admin_get_player_info,
            sp_action_admin_get_balance,
            sp_action_admin_get_state,
            sp_action_admin_get_first_cards,
            sp_action_admin_get_player_card3,
            sp_action_admin_get_banker_card3,
            sp_action_admin_place_bet,
            sp_action_admin_cancel_bet,
            sp_action_admin_get_payout,
            sp_action_admin_get_game_id,
            sp_action_admin_get_result,

            sp_action_game_set_total,
            sp_action_game_set_first_cards,
            sp_action_game_set_player_card3,
            sp_action_game_set_banker_card3,

            sp_state_change_NEW_GAME,
            sp_state_change_NEW_GAME___READY_BET,
            sp_state_change_READY_BET___LAST_CALL,
            sp_state_change_LAST_CALL___STOP_BET,
            sp_state_change_STOP_BET___DEAL_FIRST,
            sp_state_change_DEAL_FIRST___DONE_FIRST,
            sp_state_change_DONE_FIRST___WATCH_FIRST,
            sp_state_change_WATCH_FIRST___OPEN_FIRST,
            sp_state_change_WATCH_FIRST___GAME_DONE,
            sp_state_change_OPEN_FIRST___DEAL_PLAYER3,
            sp_state_change_OPEN_FIRST___DEAL_BANKER3,
            sp_state_change_DEAL_PLAYER3___DONE_PLAYER3,
            sp_state_change_DONE_PLAYER3___WATCH_PLAYER3,
            sp_state_change_WATCH_PLAYER3___GAME_DONE,
            sp_state_change_WATCH_PLAYER3___OPEN_PLAYER3,
            
            sp_state_change_OPEN_PLAYER3___DEAL_BANKER3,
            sp_state_change_DEAL_BANKER3___DONE_BANKER3,
            sp_state_change_DONE_BANKER3___WATCH_BANKER3,
            sp_state_change_WATCH_BANKER3___GAME_DONE,

            sp_state_change_GAME_DONE___RESULT,
            sp_state_change_RESULT___ANNOUNCE,
            sp_state_change_ANNOUNCE___CALC_PAY,
            sp_state_change_CALC_PAY___CALC_PAY_DONE,
            sp_state_change_CALC_PAY_DONE___PAYOUT,
            sp_state_change_PAYOUT___PREPARE_NEXT;

        #region SQL Command Related
        protected SqlCommand buildCommand(string name)
        {
            SqlCommand cmd = new SqlCommand(name);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            addOutputParam(cmd, "returnCode", System.Data.SqlDbType.Int);
            return cmd;
        }
        protected void addInputParam(SqlCommand cmd, string name, System.Data.SqlDbType type, int size = 0)
        {
            if (size == 0)
                cmd.Parameters.Add(name, type);
            else
                cmd.Parameters.Add(name, type, size);
        }
        protected void addOutputParam(SqlCommand cmd, string name, System.Data.SqlDbType type, int size = 0)
        {
            if (size == 0)
                cmd.Parameters.Add(name, type);
            else
                cmd.Parameters.Add(name, type, size);
            cmd.Parameters[name].Direction = System.Data.ParameterDirection.Output;
        }      


        protected void initCommandList()
        {
            commands = new List<SqlCommand>();
			commands.Add(sp_action_player_get_id_by_login);
			commands.Add(sp_action_player_get_table_list);
			commands.Add(sp_action_player_get_player_info);
			commands.Add(sp_action_player_get_balance);
			commands.Add(sp_action_player_get_state);
			commands.Add(sp_action_player_get_first_cards);
			commands.Add(sp_action_player_get_player_card3);
			commands.Add(sp_action_player_get_banker_card3);
			commands.Add(sp_action_player_place_bet);
			commands.Add(sp_action_player_get_result);
			commands.Add(sp_action_player_get_payout);
			commands.Add(sp_action_player_get_game_id);

			commands.Add(sp_action_admin_get_player_info);
            commands.Add(sp_action_admin_get_balance);
            commands.Add(sp_action_admin_get_state);
            commands.Add(sp_action_admin_get_first_cards);
            commands.Add(sp_action_admin_get_player_card3);
            commands.Add(sp_action_admin_get_banker_card3);
            commands.Add(sp_action_admin_place_bet);
            commands.Add(sp_action_admin_cancel_bet);
            commands.Add(sp_action_admin_get_payout);
            commands.Add(sp_action_admin_get_game_id);
            commands.Add(sp_action_admin_get_result);
            commands.Add(sp_action_game_set_total);
            commands.Add(sp_action_game_set_first_cards);
            commands.Add(sp_action_game_set_player_card3);
            commands.Add(sp_action_game_set_banker_card3);
            commands.Add(sp_state_change_NEW_GAME);
            commands.Add(sp_state_change_NEW_GAME___READY_BET);
            commands.Add(sp_state_change_READY_BET___LAST_CALL);
            commands.Add(sp_state_change_LAST_CALL___STOP_BET);
            commands.Add(sp_state_change_STOP_BET___DEAL_FIRST);

            commands.Add(sp_state_change_DEAL_FIRST___DONE_FIRST);
            commands.Add(sp_state_change_DONE_FIRST___WATCH_FIRST);           
            commands.Add(sp_state_change_WATCH_FIRST___GAME_DONE);
            commands.Add(sp_state_change_WATCH_FIRST___OPEN_FIRST);
            commands.Add(sp_state_change_OPEN_FIRST___DEAL_PLAYER3);
            commands.Add(sp_state_change_OPEN_FIRST___DEAL_BANKER3);

            commands.Add(sp_state_change_DEAL_PLAYER3___DONE_PLAYER3);
            commands.Add(sp_state_change_DONE_PLAYER3___WATCH_PLAYER3);
            commands.Add(sp_state_change_WATCH_PLAYER3___GAME_DONE);
            commands.Add(sp_state_change_WATCH_PLAYER3___OPEN_PLAYER3);
            commands.Add(sp_state_change_OPEN_PLAYER3___DEAL_BANKER3);

            commands.Add(sp_state_change_DEAL_BANKER3___DONE_BANKER3);
            commands.Add(sp_state_change_DONE_BANKER3___WATCH_BANKER3);
            commands.Add(sp_state_change_WATCH_BANKER3___GAME_DONE);

            commands.Add(sp_state_change_GAME_DONE___RESULT);
            commands.Add(sp_state_change_RESULT___ANNOUNCE);
            commands.Add(sp_state_change_ANNOUNCE___CALC_PAY);
            commands.Add(sp_state_change_CALC_PAY___CALC_PAY_DONE);
            commands.Add(sp_state_change_CALC_PAY_DONE___PAYOUT);
            commands.Add(sp_state_change_PAYOUT___PREPARE_NEXT);            
        }
		protected void initClientCommands()
		{
			sp_action_player_get_id_by_login = buildCommand("sp_action_player_get_id_by_login");
			addInputParam (sp_action_player_get_id_by_login, "username", System.Data.SqlDbType.VarChar, 50);
			addInputParam (sp_action_player_get_id_by_login, "password", System.Data.SqlDbType.VarChar, 50);
			addOutputParam(sp_action_player_get_id_by_login, "playerID", System.Data.SqlDbType.Int);
			addOutputParam(sp_action_player_get_id_by_login, "sessionID", System.Data.SqlDbType.Char, 36);

			sp_action_player_get_player_info = buildCommand ("sp_action_player_get_player_info");
			addInputParam (sp_action_player_get_player_info, "playerID", System.Data.SqlDbType.BigInt);
			addInputParam (sp_action_player_get_player_info, "sessionID", System.Data.SqlDbType.Char, 36);
			addOutputParam(sp_action_player_get_player_info, "name", System.Data.SqlDbType.NVarChar, 50);
			addOutputParam(sp_action_player_get_player_info, "balance", System.Data.SqlDbType.Money);
			addOutputParam(sp_action_player_get_player_info, "table", System.Data.SqlDbType.Int);
		
			sp_action_player_get_table_list = buildCommand("sp_action_player_get_table_list");
			addInputParam(sp_action_player_get_table_list, "playerID", System.Data.SqlDbType.Int);
			addInputParam(sp_action_player_get_table_list, "sessionID", System.Data.SqlDbType.Char, 36);
			
			sp_action_player_get_state = buildCommand("sp_action_player_get_state");
			addInputParam (sp_action_player_get_state, "playerID", System.Data.SqlDbType.BigInt);
			addInputParam (sp_action_player_get_state, "sessionID", System.Data.SqlDbType.Char, 36);
			addInputParam (sp_action_player_get_state, "tableID", System.Data.SqlDbType.Int);
			addOutputParam(sp_action_player_get_state, "gameState", System.Data.SqlDbType.VarChar, 13);
			addOutputParam(sp_action_player_get_state, "prevActionID", System.Data.SqlDbType.BigInt);
			addOutputParam(sp_action_player_get_state, "currActionID", System.Data.SqlDbType.BigInt);
			
			sp_action_player_get_first_cards = buildCommand("sp_action_player_get_first_cards");
			addInputParam (sp_action_player_get_first_cards, "playerID", System.Data.SqlDbType.BigInt);
			addInputParam (sp_action_player_get_first_cards, "sessionID", System.Data.SqlDbType.Char, 36);
			addInputParam (sp_action_player_get_first_cards, "gameID", System.Data.SqlDbType.BigInt);
			addOutputParam(sp_action_player_get_first_cards, "cards", System.Data.SqlDbType.VarChar, 58);
			
			sp_action_player_get_player_card3 = buildCommand("sp_action_player_get_player_card3");
			addInputParam (sp_action_player_get_player_card3, "playerID", System.Data.SqlDbType.BigInt);
			addInputParam (sp_action_player_get_player_card3, "sessionID", System.Data.SqlDbType.Char, 36);
			addInputParam (sp_action_player_get_player_card3, "gameID", System.Data.SqlDbType.BigInt);
			addOutputParam(sp_action_player_get_player_card3, "card", System.Data.SqlDbType.VarChar, 22);

			sp_action_player_get_banker_card3 = buildCommand("sp_action_player_get_banker_card3");
			addInputParam (sp_action_player_get_banker_card3, "playerID", System.Data.SqlDbType.BigInt);
			addInputParam (sp_action_player_get_banker_card3, "sessionID", System.Data.SqlDbType.Char, 36);
			addInputParam (sp_action_player_get_banker_card3, "gameID", System.Data.SqlDbType.BigInt);
			addOutputParam(sp_action_player_get_banker_card3, "card", System.Data.SqlDbType.VarChar, 22);
			
			sp_action_player_place_bet = buildCommand("sp_action_player_place_bet");
			addInputParam (sp_action_player_place_bet, "playerID", System.Data.SqlDbType.BigInt);
			addInputParam (sp_action_player_place_bet, "sessionID", System.Data.SqlDbType.Char, 36);
			addInputParam (sp_action_player_place_bet, "gameID", System.Data.SqlDbType.BigInt);
			addInputParam (sp_action_player_place_bet, "betType", System.Data.SqlDbType.VarChar, 11);
			addInputParam (sp_action_player_place_bet, "betAmount", System.Data.SqlDbType.Money);
			
			sp_action_player_get_payout = buildCommand("sp_action_player_get_payout");
			addInputParam (sp_action_player_get_payout, "playerID", System.Data.SqlDbType.BigInt);
			addInputParam (sp_action_player_get_payout, "sessionID", System.Data.SqlDbType.Char, 36);
			addInputParam (sp_action_player_get_payout, "gameID", System.Data.SqlDbType.BigInt);
			addOutputParam(sp_action_player_get_payout, "payout", System.Data.SqlDbType.Money);
			addOutputParam(sp_action_player_get_payout, "balance", System.Data.SqlDbType.Money);
			
			sp_action_player_get_game_id = buildCommand("sp_action_player_get_game_id");
			addInputParam (sp_action_player_get_game_id, "playerID", System.Data.SqlDbType.BigInt);
			addInputParam (sp_action_player_get_game_id, "sessionID", System.Data.SqlDbType.Char, 36);
			addInputParam (sp_action_player_get_game_id, "tableID", System.Data.SqlDbType.Int);
			addOutputParam(sp_action_player_get_game_id, "gameID", System.Data.SqlDbType.BigInt);
			
			sp_action_player_get_result = buildCommand("sp_action_player_get_result");
			addInputParam (sp_action_player_get_result, "playerID", System.Data.SqlDbType.BigInt);
			addInputParam (sp_action_player_get_result, "sessionID", System.Data.SqlDbType.Char, 36);
			addInputParam (sp_action_player_get_result, "gameID", System.Data.SqlDbType.BigInt);
			addOutputParam(sp_action_player_get_result, "bankerTotal", System.Data.SqlDbType.TinyInt);
			addOutputParam(sp_action_player_get_result, "playerTotal", System.Data.SqlDbType.TinyInt);
			addOutputParam(sp_action_player_get_result, "winnerType", System.Data.SqlDbType.Char, 1);
			addOutputParam(sp_action_player_get_result, "pairType", System.Data.SqlDbType.VarChar, 2);
			
			sp_action_player_get_balance = buildCommand("sp_action_player_get_balance");
			addInputParam (sp_action_player_get_balance, "playerID", System.Data.SqlDbType.Int);
			addInputParam (sp_action_player_get_balance, "sessionID", System.Data.SqlDbType.Char, 36);
			addOutputParam(sp_action_player_get_balance, "balance", System.Data.SqlDbType.Money);
		}
		protected void initAdminCommands()
        {
			sp_action_admin_get_player_info = buildCommand ("sp_action_admin_get_player_info");
			addInputParam (sp_action_admin_get_player_info, "playerID", System.Data.SqlDbType.BigInt);
			addOutputParam (sp_action_admin_get_player_info, "name", System.Data.SqlDbType.NVarChar, 50);
			addOutputParam (sp_action_admin_get_player_info, "balance", System.Data.SqlDbType.Money);
			addOutputParam (sp_action_admin_get_player_info, "table", System.Data.SqlDbType.Int);

            sp_action_admin_get_state = buildCommand("sp_action_admin_get_state");
            addInputParam(sp_action_admin_get_state, "tableID", System.Data.SqlDbType.Int);
            addOutputParam(sp_action_admin_get_state, "gameState", System.Data.SqlDbType.VarChar, 13);
            addOutputParam(sp_action_admin_get_state, "prevActionID", System.Data.SqlDbType.BigInt);
            addOutputParam(sp_action_admin_get_state, "currActionID", System.Data.SqlDbType.BigInt);

            sp_action_admin_get_first_cards = buildCommand("sp_action_admin_get_first_cards");
            addInputParam(sp_action_admin_get_first_cards, "gameID", System.Data.SqlDbType.BigInt);
            addOutputParam(sp_action_admin_get_first_cards, "cards", System.Data.SqlDbType.VarChar, 58);

            sp_action_admin_get_player_card3 = buildCommand("sp_action_admin_get_player_card3");
            addInputParam(sp_action_admin_get_player_card3, "gameID", System.Data.SqlDbType.BigInt);
            addOutputParam(sp_action_admin_get_player_card3, "card", System.Data.SqlDbType.VarChar, 22);

            sp_action_admin_get_banker_card3 = buildCommand("sp_action_admin_get_banker_card3");
            addInputParam(sp_action_admin_get_banker_card3, "gameID", System.Data.SqlDbType.BigInt);
            addOutputParam(sp_action_admin_get_banker_card3, "card", System.Data.SqlDbType.VarChar, 22);

            sp_action_admin_place_bet = buildCommand("sp_action_admin_place_bet");
            addInputParam(sp_action_admin_place_bet, "playerID", System.Data.SqlDbType.BigInt);
            addInputParam(sp_action_admin_place_bet, "gameID", System.Data.SqlDbType.BigInt);
            addInputParam(sp_action_admin_place_bet, "betType", System.Data.SqlDbType.VarChar, 11);
            addInputParam(sp_action_admin_place_bet, "betAmount", System.Data.SqlDbType.Money);

            sp_action_admin_cancel_bet = buildCommand("sp_action_admin_cancel_bet");
            addInputParam(sp_action_admin_cancel_bet, "playerID", System.Data.SqlDbType.BigInt);
            addInputParam(sp_action_admin_cancel_bet, "gameID", System.Data.SqlDbType.BigInt);

            sp_action_admin_get_payout = buildCommand("sp_action_admin_get_payout");
            addInputParam(sp_action_admin_get_payout, "playerID", System.Data.SqlDbType.BigInt);
            addInputParam(sp_action_admin_get_payout, "gameID", System.Data.SqlDbType.BigInt);
            addOutputParam(sp_action_admin_get_payout, "payout", System.Data.SqlDbType.Money);
            addOutputParam(sp_action_admin_get_payout, "balance", System.Data.SqlDbType.Money);

            sp_action_admin_get_game_id = buildCommand("sp_action_admin_get_game_id");
            addInputParam(sp_action_admin_get_game_id, "tableID", System.Data.SqlDbType.Int);
            addOutputParam(sp_action_admin_get_game_id, "gameID", System.Data.SqlDbType.BigInt);

            sp_action_admin_get_result = buildCommand("sp_action_admin_get_result");
            addInputParam(sp_action_admin_get_result, "gameID", System.Data.SqlDbType.BigInt);
            addOutputParam(sp_action_admin_get_result, "bankerTotal", System.Data.SqlDbType.TinyInt);
            addOutputParam(sp_action_admin_get_result, "playerTotal", System.Data.SqlDbType.TinyInt);
            addOutputParam(sp_action_admin_get_result, "winnerType", System.Data.SqlDbType.Char, 1);
            addOutputParam(sp_action_admin_get_result, "pairType", System.Data.SqlDbType.VarChar, 2);

			sp_action_admin_get_balance = buildCommand("sp_action_admin_get_balance");
			addInputParam(sp_action_admin_get_balance, "playerID", System.Data.SqlDbType.Int);
			addOutputParam(sp_action_admin_get_balance, "balance", System.Data.SqlDbType.Money);
        }
        protected void initDealerCommands()
        {
            sp_state_change_NEW_GAME = buildCommand("sp_state_change:NEW_GAME");
            addInputParam(sp_state_change_NEW_GAME, "name", System.Data.SqlDbType.VarChar, 50);
            addInputParam(sp_state_change_NEW_GAME, "tableID", System.Data.SqlDbType.Int);
            addOutputParam(sp_state_change_NEW_GAME, "newGameID", System.Data.SqlDbType.BigInt);

            sp_state_change_NEW_GAME___READY_BET            = buildGameIDCommand("sp_state_change:NEW_GAME_>_READY_BET");
            sp_state_change_READY_BET___LAST_CALL           = buildGameIDCommand("sp_state_change:READY_BET_>_LAST_CALL");
            sp_state_change_LAST_CALL___STOP_BET            = buildGameIDCommand("sp_state_change:LAST_CALL_>_STOP_BET");
            sp_state_change_STOP_BET___DEAL_FIRST           = buildGameIDCommand("sp_state_change:STOP_BET_>_DEAL_FIRST");
 
            sp_state_change_DEAL_FIRST___DONE_FIRST         = buildGameIDCommand("sp_state_change:DEAL_FIRST_>_DONE_FIRST");
            sp_state_change_DONE_FIRST___WATCH_FIRST        = buildGameIDCommand("sp_state_change:DONE_FIRST_>_WATCH_FIRST");
            sp_state_change_WATCH_FIRST___GAME_DONE         = buildGameIDCommand("sp_state_change:WATCH_FIRST_>_GAME_DONE");   
            sp_state_change_WATCH_FIRST___OPEN_FIRST        = buildGameIDCommand("sp_state_change:WATCH_FIRST_>_OPEN_FIRST");              
            sp_state_change_OPEN_FIRST___DEAL_PLAYER3       = buildGameIDCommand("sp_state_change:OPEN_FIRST_>_DEAL_PLAYER3");
            sp_state_change_OPEN_FIRST___DEAL_BANKER3       = buildGameIDCommand("sp_state_change:OPEN_FIRST_>_DEAL_BANKER3");

            sp_state_change_DEAL_PLAYER3___DONE_PLAYER3     = buildGameIDCommand("sp_state_change:DEAL_PLAYER3_>_DONE_PLAYER3");
            sp_state_change_DONE_PLAYER3___WATCH_PLAYER3    = buildGameIDCommand("sp_state_change:DONE_PLAYER3_>_WATCH_PLAYER3");
            sp_state_change_WATCH_PLAYER3___GAME_DONE       = buildGameIDCommand("sp_state_change:WATCH_PLAYER3_>_GAME_DONE");
            sp_state_change_WATCH_PLAYER3___OPEN_PLAYER3    = buildGameIDCommand("sp_state_change:WATCH_PLAYER3_>_OPEN_PLAYER3");
            sp_state_change_OPEN_PLAYER3___DEAL_BANKER3     = buildGameIDCommand("sp_state_change:OPEN_PLAYER3_>_DEAL_BANKER3");

            sp_state_change_DEAL_BANKER3___DONE_BANKER3     = buildGameIDCommand("sp_state_change:DEAL_BANKER3_>_DONE_BANKER3");
            sp_state_change_DONE_BANKER3___WATCH_BANKER3    = buildGameIDCommand("sp_state_change:DONE_BANKER3_>_WATCH_BANKER3");
            sp_state_change_WATCH_BANKER3___GAME_DONE       = buildGameIDCommand("sp_state_change:WATCH_BANKER3_>_GAME_DONE");

            sp_state_change_GAME_DONE___RESULT              = buildGameIDCommand("sp_state_change:GAME_DONE_>_RESULT");
            sp_state_change_RESULT___ANNOUNCE               = buildGameIDCommand("sp_state_change:RESULT_>_ANNOUNCE");
            sp_state_change_ANNOUNCE___CALC_PAY             = buildGameIDCommand("sp_state_change:ANNOUNCE_>_CALC_PAY");
            sp_state_change_CALC_PAY___CALC_PAY_DONE        = buildGameIDCommand("sp_state_change:CALC_PAY_>_CALC_PAY_DONE");
            sp_state_change_CALC_PAY_DONE___PAYOUT          = buildGameIDCommand("sp_state_change:CALC_PAY_DONE_>_PAYOUT");
            sp_state_change_PAYOUT___PREPARE_NEXT           = buildGameIDCommand("sp_state_change:PAYOUT_>_PREPARE_NEXT");
            
            sp_action_game_set_total = buildGameIDCommand("sp_action_game_set_total");
            addInputParam(sp_action_game_set_total, "bankerTotal", System.Data.SqlDbType.TinyInt);
            addInputParam(sp_action_game_set_total, "playerTotal", System.Data.SqlDbType.TinyInt);

            sp_action_game_set_first_cards = buildGameIDCommand("sp_action_game_set_first_cards");
            addInputParam(sp_action_game_set_first_cards, "bankerRank1", System.Data.SqlDbType.VarChar, 7);
            addInputParam(sp_action_game_set_first_cards, "bankerCard1", System.Data.SqlDbType.VarChar, 2);
            addInputParam(sp_action_game_set_first_cards, "bankerRank2", System.Data.SqlDbType.VarChar, 7);
            addInputParam(sp_action_game_set_first_cards, "bankerCard2", System.Data.SqlDbType.VarChar, 2);
            addInputParam(sp_action_game_set_first_cards, "playerRank1", System.Data.SqlDbType.VarChar, 7);
            addInputParam(sp_action_game_set_first_cards, "playerCard1", System.Data.SqlDbType.VarChar, 2);
            addInputParam(sp_action_game_set_first_cards, "playerRank2", System.Data.SqlDbType.VarChar, 7);
            addInputParam(sp_action_game_set_first_cards, "playerCard2", System.Data.SqlDbType.VarChar, 2);
            addInputParam(sp_action_game_set_first_cards, "playerTotal", System.Data.SqlDbType.TinyInt);
            addInputParam(sp_action_game_set_first_cards, "bankerTotal", System.Data.SqlDbType.TinyInt);

            sp_action_game_set_player_card3 = buildGameIDCommand("sp_action_game_set_player_card3");
            addInputParam(sp_action_game_set_player_card3, "playerRank3", System.Data.SqlDbType.VarChar, 7);
            addInputParam(sp_action_game_set_player_card3, "playerCard3", System.Data.SqlDbType.VarChar, 2);
            addInputParam(sp_action_game_set_player_card3, "playerTotal", System.Data.SqlDbType.TinyInt);

            sp_action_game_set_banker_card3 = buildGameIDCommand("sp_action_game_set_banker_card3");
            addInputParam(sp_action_game_set_banker_card3, "bankerRank3", System.Data.SqlDbType.VarChar, 7);
            addInputParam(sp_action_game_set_banker_card3, "bankerCard3", System.Data.SqlDbType.VarChar, 2);
            addInputParam(sp_action_game_set_banker_card3, "bankerTotal", System.Data.SqlDbType.TinyInt);
        }
        protected void initCommands()
        {
            initClientCommands();
			/* Admin commands may not need in production environment */
            initAdminCommands();
			initDealerCommands();
        }
        protected SqlCommand buildGameIDCommand(string name)
        {
            SqlCommand cmd = buildCommand(name);
            addInputParam(cmd, "gameID", System.Data.SqlDbType.BigInt);
            return cmd;
        }
        #endregion

        #region SQL Connection Related
        protected void OpenDBConn()
        {
            try
            {
                if (sqlConn != null)
                    if (sqlConn.State == System.Data.ConnectionState.Closed)
                        sqlConn.Open();
            }
            catch (Exception e) { //EventLogger.print(e.StackTrace);
            }
        }
        protected void CloseDBConn()
        {
            try
            {
                if (sqlConn != null)
                    if (sqlConn.State == System.Data.ConnectionState.Open)
                        sqlConn.Close();
            }
            catch (Exception e) { //EventLogger.print(e.StackTrace);
            }
        }
        
        
        protected void setSQLConnToCommands()
        {
            
            /*will use connection from multiple SQL server
             * cmd name indiciates  which SQKL server are using
             * 
             */ 
            for (int i = 0; i < commands.Count; i++)
                commands.ElementAt(i).Connection = sqlConn;
        }



        public static void setSQLConnection(SqlConnection conn)
        {
            sqlConn = conn;
            self.setSQLConnToCommands();
        }
        #endregion

        #region Handle Request From Client
		public string getPlayerIDByLogin(string username, string password)
		{
			string result = string.Empty;
			OpenDBConn();
			sp_action_player_get_id_by_login.Parameters["username"].Value = username;
			sp_action_player_get_id_by_login.Parameters["password"].Value = password;
			try
			{
				sp_action_player_get_id_by_login.ExecuteNonQuery();
				if ((int)sp_action_player_get_id_by_login.Parameters["returnCode"].Value == 0) {
					result += "PLAYER_ID:"+ sp_action_player_get_id_by_login.Parameters["playerID"].Value + ";";
					result += "SESSION_ID:"+ sp_action_player_get_id_by_login.Parameters["sessionID"].Value + ";";
				}
			}
            catch (Exception e) { //EventLogger.print(e.StackTrace); 
                return "FAIL#"; }
			CloseDBConn();
			return formatReturnString(result);
		}
		/* method_name2 is for internal use, call admin commands, no authentication needed, while method_name is normal */
		public string getPlayerInfo(int playerID, string sessionID)
		{
			string result = string.Empty;
			OpenDBConn ();
			sp_action_player_get_player_info.Parameters ["playerID"].Value = playerID;
			sp_action_player_get_player_info.Parameters ["sessionID"].Value = playerID;
			try
			{
				if((int)sp_action_player_get_player_info.Parameters["returnCode"].Value == 0){
					result += "NAME:" + sp_action_player_get_player_info.Parameters["name"].Value + ";";
					result += "BALANCE:" + sp_action_player_get_player_info.Parameters["balance"].Value + ";";
					if(sp_action_player_get_player_info.Parameters["table"].Value != null) 
						result += "TABLE:" + sp_action_player_get_player_info.Parameters["table"].Value + ";";
					else
						result += "TABLE:NULL;";
				}
			}
            catch (Exception e) { //EventLogger.print(e.StackTrace); 
                return "FAIL#"; }
			CloseDBConn ();
			return formatReturnString(result);
		}
		public string getPlayerInfo2(int playerID)
		{
			string result = string.Empty;
			OpenDBConn ();
			sp_action_admin_get_player_info.Parameters ["playerID"].Value = "";
			try
			{
				if((int)sp_action_admin_get_player_info.Parameters["returnCode"].Value == 0){
					result += "NAME:" + sp_action_admin_get_player_info.Parameters["name"].Value + ";";
					result += "BALANCE:" + sp_action_admin_get_player_info.Parameters["balance"].Value + ";";
					if(sp_action_admin_get_player_info.Parameters["table"].Value != null) 
						result += "TABLE:" + sp_action_admin_get_player_info.Parameters["table"].Value + ";";
					else
						result += "TABLE:NULL;";
				}
			}
            catch (Exception e) { //EventLogger.print(e.StackTrace); 
                return "FAIL#"; }
			CloseDBConn ();
			return formatReturnString(result);
		}

		public string getBalance(int playerID, string sessionID)
		{
			string result = string.Empty;
			OpenDBConn ();
			sp_action_player_get_balance.Parameters ["playerID"].Value = playerID;
			sp_action_player_get_balance.Parameters ["sessionID"].Value = sessionID;
			try {
				sp_action_player_get_balance.ExecuteNonQuery ();
				if ((int)sp_action_player_get_balance.Parameters ["returnCode"].Value == 0) {
					result += "BALANCE:" + sp_action_player_get_balance.Parameters ["balance"].Value + ";";
				}
			}
            catch (Exception e) { //EventLogger.print(e.StackTrace); 
                return "FAIL#"; }
			CloseDBConn ();
			return formatReturnString(result);
		}

		public string getBalance2(int playerID)
        {
            string result = string.Empty;
            OpenDBConn();
            sp_action_admin_get_balance.Parameters["playerID"].Value = playerID;
            try
            {
                sp_action_admin_get_balance.ExecuteNonQuery();
                if ((int)sp_action_admin_get_balance.Parameters["returnCode"].Value == 0)
                {
                    result += "BALANCE:" + sp_action_admin_get_balance.Parameters["balance"].Value + ";";
                }
            }
            catch (Exception e) { //EventLogger.print(e.StackTrace); 
                return "FAIL#"; }
            CloseDBConn();
            return formatReturnString(result);
        }
        public string getResult(int gameID, int playerID, string sessionID)
        {
            string result = string.Empty;
            OpenDBConn();
            sp_action_player_get_result.Parameters["gameID"].Value = gameID;
			sp_action_player_get_result.Parameters ["playerID"].Value = playerID;
			sp_action_player_get_result.Parameters ["sessionID"].Value = sessionID;
            try
            {
				sp_action_player_get_result.ExecuteNonQuery();
				if ((int)sp_action_player_get_result.Parameters["returnCode"].Value == 0)
                {
					result += "BANKER_TOTAL:" + sp_action_player_get_result.Parameters["bankerTotal"].Value + ";";
					result += "PLAYER_TOTAL:" + sp_action_player_get_result.Parameters["playerTotal"].Value + ";";
					result += "WINNER_TYPE:" + sp_action_player_get_result.Parameters["winnerType"].Value + ";";
					result += "PAIR_TYPE:" + sp_action_player_get_result.Parameters["pairType"].Value + ";";
                }
            }
            catch (Exception e) { //EventLogger.print(e.StackTrace); 
                return "FAIL#"; }
            CloseDBConn();
            return formatReturnString(result);
        }
		public string getResult2(int gameID)
		{
			string result = string.Empty;
			OpenDBConn();
			sp_action_admin_get_result.Parameters["gameID"].Value = gameID;
			try
			{
				sp_action_admin_get_result.ExecuteNonQuery();
				if ((int)sp_action_admin_get_result.Parameters["returnCode"].Value == 0)
				{
					result += "BANKER_TOTAL:" + sp_action_admin_get_result.Parameters["bankerTotal"].Value + ";";
					result += "PLAYER_TOTAL:" + sp_action_admin_get_result.Parameters["playerTotal"].Value + ";";
					result += "WINNER_TYPE:" + sp_action_admin_get_result.Parameters["winnerType"].Value + ";";
					result += "PAIR_TYPE:" + sp_action_admin_get_result.Parameters["pairType"].Value + ";";
				}
			}
            catch (Exception e) { //EventLogger.print(e.StackTrace); 
                return "FAIL#"; }
			CloseDBConn();
			return formatReturnString(result);
		}
		public string getGameID(int tableID, int playerID, string sessionID)
        {
            string result = string.Empty;
            OpenDBConn();
            sp_action_player_get_game_id.Parameters["tableID"].Value = tableID;
			sp_action_player_get_game_id.Parameters ["playerID"].Value = playerID;
			sp_action_player_get_game_id.Parameters ["sessionID"].Value = sessionID;
            try
            {
				sp_action_player_get_game_id.ExecuteNonQuery();
				if ((int)sp_action_player_get_game_id.Parameters["returnCode"].Value == 0)
                {
					result += "GAME_ID:" + sp_action_player_get_game_id.Parameters["gameID"].Value + ";";
                }
            }
            catch (Exception e) { //EventLogger.print(e.StackTrace); 
                return "FAIL#"; }
            CloseDBConn();
            return formatReturnString(result);
        }
		public string getGameID2(int tableID)
		{
			string result = string.Empty;
			OpenDBConn();
			sp_action_admin_get_game_id.Parameters["tableID"].Value = tableID;
			try
			{
				sp_action_admin_get_game_id.ExecuteNonQuery();
				if ((int)sp_action_admin_get_game_id.Parameters["returnCode"].Value == 0)
				{
					result += "GAME_ID:" + sp_action_admin_get_game_id.Parameters["gameID"].Value + ";";
				}
			}
            catch (Exception e) { //EventLogger.print(e.StackTrace); 
                return "FAIL#"; }
			CloseDBConn();
			return formatReturnString(result);
		}
        public string getTableList(int playerID, string sessionID)
        {
            string result = string.Empty;
            OpenDBConn();
            sp_action_player_get_table_list.Parameters["playerID"].Value = playerID;
            sp_action_player_get_table_list.Parameters["sessionID"].Value = sessionID;

            try
            {
                sp_action_player_get_table_list.ExecuteNonQuery();
                if ((int)sp_action_player_get_table_list.Parameters["returnCode"].Value == 0)
                {
                    result += "TABLE_ID:" + sp_action_player_get_table_list.Parameters["table_id"].Value + ";";
					result += "GAME_TYPE:" + sp_action_player_get_table_list.Parameters["game_type"].Value + ";";
					result += "MIN_BET:" + sp_action_player_get_table_list.Parameters["min_bet"].Value + ";";
					result += "MAX_BET:" + sp_action_player_get_table_list.Parameters["max_bet"].Value + ";";
                }
            }
            catch (Exception e) { //EventLogger.print(e.StackTrace); 
                return "FAIL#"; }
            CloseDBConn();
			return formatReturnString(result);
        }
		public string getState(int tableID, int playerID, string sessionID)
        {
            string result = string.Empty;
            OpenDBConn();
            sp_action_player_get_state.Parameters["tableID"].Value = tableID;
			sp_action_player_get_state.Parameters["playerID"].Value = playerID;
			sp_action_player_get_state.Parameters["sessionID"].Value = sessionID;
            try
            {
				sp_action_player_get_state.ExecuteNonQuery();
				if ((int)sp_action_player_get_state.Parameters["returnCode"].Value == 0)
                {
					result += "GAME_STATE:" + sp_action_player_get_state.Parameters["gameState"].Value + ";";
					result += "PREV_ACTION_ID:" + sp_action_player_get_state.Parameters["prevActionID"].Value + ";";
					result += "CURR_ACTION_ID:" + sp_action_player_get_state.Parameters["currActionID"].Value + ";";
                }
            }
            catch (Exception e) { //EventLogger.print(e.StackTrace); 
                return "FAIL#"; }
            CloseDBConn();
            return formatReturnString(result);
        }
		public string getState2(int tableID)
		{
			string result = string.Empty;
			OpenDBConn();
			sp_action_admin_get_state.Parameters["tableID"].Value = tableID;
			try
			{
				sp_action_admin_get_state.ExecuteNonQuery();
				if ((int)sp_action_admin_get_state.Parameters["returnCode"].Value == 0)
				{
					result += "GAME_STATE:" + sp_action_admin_get_state.Parameters["gameState"].Value + ";";
					result += "PREV_ACTION_ID:" + sp_action_admin_get_state.Parameters["prevActionID"].Value + ";";
					result += "CURR_ACTION_ID:" + sp_action_admin_get_state.Parameters["currActionID"].Value + ";";
				}
			}
            catch (Exception e) { //EventLogger.print(e.StackTrace); 
                return "FAIL#"; }
			CloseDBConn();
			return formatReturnString(result);
		}
		public string getFirstCards(int gameID, int playerID, string sessionID)
        {
            string result = string.Empty;
            OpenDBConn();
            sp_action_player_get_first_cards.Parameters["gameID"].Value = gameID;
			sp_action_player_get_first_cards.Parameters["playerID"].Value = playerID;
			sp_action_player_get_first_cards.Parameters["sessionID"].Value = sessionID;
            try
            {
				sp_action_player_get_first_cards.ExecuteNonQuery();
				if ((int)sp_action_player_get_first_cards.Parameters["returnCode"].Value == 0)
                {
					result += sp_action_player_get_first_cards.Parameters["cards"].Value;
                }
            }
            catch (Exception e) { //EventLogger.print(e.StackTrace); 
                return "FAIL#"; }
            CloseDBConn();
            return formatReturnString(result);
        }
		public string getFirstCards2(int gameID)
		{
			string result = string.Empty;
			OpenDBConn();
			sp_action_admin_get_first_cards.Parameters["gameID"].Value = gameID;
			try
			{
				sp_action_admin_get_first_cards.ExecuteNonQuery();
				if ((int)sp_action_admin_get_first_cards.Parameters["returnCode"].Value == 0)
				{
					result += sp_action_admin_get_first_cards.Parameters["cards"].Value;
				}
			}
            catch (Exception e) { //EventLogger.print(e.StackTrace); 
                return "FAIL#"; }
			CloseDBConn();
			return formatReturnString(result);
		}
		public string getPlayerCard3(int gameID, int playerID, string sessionID)
		{
			string result = string.Empty;
			OpenDBConn();
			sp_action_player_get_player_card3.Parameters["gameID"].Value = gameID;
			sp_action_player_get_player_card3.Parameters["playerID"].Value = playerID;
			sp_action_player_get_player_card3.Parameters["sessionID"].Value = sessionID;
			try
			{
				sp_action_player_get_player_card3.ExecuteNonQuery();
				if ((int)sp_action_player_get_player_card3.Parameters["returnCode"].Value == 0)
				{
					result += sp_action_player_get_player_card3.Parameters["card"].Value;
				}
			}
            catch (Exception e) { //EventLogger.print(e.StackTrace); 
                return "FAIL#"; }
			CloseDBConn();
			return formatReturnString(result);
		}
		public string getPlayerCard32(int gameID)
        {
            string result = string.Empty;
            OpenDBConn();
            sp_action_admin_get_player_card3.Parameters["gameID"].Value = gameID;
            try
            {
                sp_action_admin_get_player_card3.ExecuteNonQuery();
                if ((int)sp_action_admin_get_player_card3.Parameters["returnCode"].Value == 0)
                {
					result += sp_action_admin_get_player_card3.Parameters["card"].Value;
                }
            }
            catch (Exception e) { //EventLogger.print(e.StackTrace); 
                return "FAIL#"; }
            CloseDBConn();
            return formatReturnString(result);
        }

		public string getBankerCard3(int gameID, int playerID, string sessionID)
        {
            string result = string.Empty;
            OpenDBConn();
            sp_action_player_get_banker_card3.Parameters["gameID"].Value = gameID;
            sp_action_player_get_banker_card3.Parameters["playerID"].Value = playerID;
            sp_action_player_get_banker_card3.Parameters["sessionID"].Value = sessionID;
            try
            {
				sp_action_player_get_banker_card3.ExecuteNonQuery();
				if ((int)sp_action_player_get_banker_card3.Parameters["returnCode"].Value == 0)
                {
					result += sp_action_player_get_banker_card3.Parameters["card"].Value;
                }
            }
            catch (Exception e) { //EventLogger.print(e.StackTrace); 
                return "FAIL#"; }
            CloseDBConn();
            return formatReturnString(result);
        }
		public string getBankerCard32(int gameID)
		{
			string result = string.Empty;
			OpenDBConn();
			sp_action_admin_get_banker_card3.Parameters["gameID"].Value = gameID;
			try
			{
				sp_action_admin_get_banker_card3.ExecuteNonQuery();
				if ((int)sp_action_admin_get_banker_card3.Parameters["returnCode"].Value == 0)
				{
					result += sp_action_admin_get_banker_card3.Parameters["card"].Value;
				}
			}
            catch (Exception e) { //EventLogger.print(e.StackTrace); 
                return "FAIL#"; }
			CloseDBConn();
			return formatReturnString(result);
		}
		public string getPayout(int gameID, int playerID, string sessionID)
        {
            string result = string.Empty;
            OpenDBConn();
            sp_action_player_get_payout.Parameters["gameID"].Value = gameID;
			sp_action_player_get_payout.Parameters["playerID"].Value = playerID;
			sp_action_player_get_payout.Parameters["sessionID"].Value = sessionID;
            try
            {
				sp_action_player_get_payout.ExecuteNonQuery();
				if ((int)sp_action_player_get_payout.Parameters["returnCode"].Value == 0)
                {
					result += "PAYOUT:" + sp_action_player_get_payout.Parameters["payout"].Value + ";";
					result += "BALANCE:" + sp_action_player_get_payout.Parameters["balance"].Value + ";";
                }
            }
            catch (Exception e) { //EventLogger.print(e.StackTrace); 
                return "FAIL#"; }
            CloseDBConn();
            return formatReturnString(result);
        }
		public string getPayout2(int gameID, int playerID)
		{
			string result = string.Empty;
			OpenDBConn();
			sp_action_admin_get_payout.Parameters["gameID"].Value = gameID;
			sp_action_admin_get_payout.Parameters["playerID"].Value = playerID;
			try
			{
				sp_action_admin_get_payout.ExecuteNonQuery();
				if ((int)sp_action_admin_get_payout.Parameters["returnCode"].Value == 0)
				{
					result += "PAYOUT:" + sp_action_admin_get_payout.Parameters["payout"].Value + ";";
					result += "BALANCE:" + sp_action_admin_get_payout.Parameters["balance"].Value + ";";
				}
			}
            catch (Exception e) { //EventLogger.print(e.StackTrace); 
                return "FAIL#"; }
			CloseDBConn();
			return formatReturnString(result);
		}
		public string placeBet(int gameID, int playerID, string betslips, string sessionID)
        {
            string failReason = string.Empty;
            int partialResult = -1;
            try
            {
                string[] betslip = betslips.Split(';');
                // banker:100,PLAYER_PAIR:100...
                if (betslip.Length > 0)
                {
                    OpenDBConn();
                    for (int i = 0; i < betslip.Length; i++)
                    {
                        if (betslip[i] != string.Empty)
                        {
                            string[] betEntry = betslip[i].Split(':');
                            string betType = betEntry[0];
                            string betAmount = betEntry[1];

							sp_action_player_place_bet.Parameters["gameID"].Value = gameID;
							sp_action_player_place_bet.Parameters["playerID"].Value = playerID;
							sp_action_player_place_bet.Parameters["betType"].Value = betType;
							sp_action_player_place_bet.Parameters["betAmount"].Value = decimal.Parse(betAmount);
							sp_action_player_place_bet.Parameters["sessionID"].Value = sessionID;
                            try
                            {
								sp_action_player_place_bet.ExecuteNonQuery();
								partialResult = (int)sp_action_player_place_bet.Parameters["returnCode"].Value;
                                if (partialResult != 0)
                                    break;
                            }
                            catch { }
                        }
                    }
                    CloseDBConn();
                }
                else { return formatReturnString(string.Empty, "INVALID BET SLIP FORMAT#" + partialResult); }
            }
            catch (NullReferenceException e) {
                //EventLogger.print(e.StackTrace);
                return formatReturnString(string.Empty, "NO BET SLIP#" + partialResult); }
            if (partialResult != 0) //means have error, rollback
            {
                cancelBet(gameID, playerID);
                return formatReturnString(string.Empty, "BET REJECTED#" + partialResult);
            }
            else
                return formatReturnString("BET ACCEPTED");
        }
		public string placeBet2(int gameID, int playerID, string betslips)
		{
			string failReason = string.Empty;
			int partialResult = -1;
			try
			{
				string[] betslip = betslips.Split(';');
				// banker:100,PLAYER_PAIR:100...
				if (betslip.Length > 0)
				{
					OpenDBConn();
					for (int i = 0; i < betslip.Length; i++)
					{
						if (betslip[i] != string.Empty)
						{
							string[] betEntry = betslip[i].Split(':');
							string betType = betEntry[0];
							string betAmount = betEntry[1];
							
							sp_action_admin_place_bet.Parameters["gameID"].Value = gameID;
							sp_action_admin_place_bet.Parameters["playerID"].Value = playerID;
							sp_action_admin_place_bet.Parameters["betType"].Value = betType;
							sp_action_admin_place_bet.Parameters["betAmount"].Value = decimal.Parse(betAmount);
							try
							{
								sp_action_admin_place_bet.ExecuteNonQuery();
								partialResult = (int)sp_action_admin_place_bet.Parameters["returnCode"].Value;
								if (partialResult != 0)
									break;
							}
							catch { }
						}
					}
					CloseDBConn();
				}
				else { return formatReturnString(string.Empty, "INVALID BET SLIP FORMAT#" + partialResult); }
			}
			catch (NullReferenceException e) { 
                //EventLogger.print(e.StackTrace);
                return formatReturnString(string.Empty, "NO BET SLIP#" + partialResult); }
			if (partialResult != 0) //means have error, rollback
			{
				cancelBet(gameID, playerID);
				return formatReturnString(string.Empty, "BET REJECTED#" + partialResult);
			}
			else
				return formatReturnString("BET ACCEPTED");
		}
        /* cancel all bet of a player in a game, dont call unless place bet fail, act as rollback 
         * IN asp, calling place bet when not in REDAY_BET and LAST_CALL will make it call cancel bet too as sp return -1, 
         * it looks like having problem, however because the cancel bet sp won’t able to pass, 
         * so even malicious call to place bet after LAST_CALL will not cancel previous succeed bet. 
         */
        protected bool cancelBet(int gameID, int playerID)
        {
            OpenDBConn();
            sp_action_admin_cancel_bet.Parameters["gameID"].Value = gameID;
            sp_action_admin_cancel_bet.Parameters["playerID"].Value = playerID;
            try
            {
                sp_action_admin_cancel_bet.ExecuteNonQuery();
                return (int)sp_action_admin_cancel_bet.Parameters["returnCode"].Value == 0 ? true : false;
            }
            catch (Exception e) { //EventLogger.print(e.StackTrace); 
            }
            CloseDBConn();
            return false;
        }
        #endregion

        #region Handle Admin Request
        protected SqlCommand getStateCommandFromName(string name)
        {
            switch (name)
            {
                case "sp_state_change:NEW_GAME_>_READY_BET": return sp_state_change_NEW_GAME___READY_BET;
                case "sp_state_change:READY_BET_>_LAST_CALL": return sp_state_change_READY_BET___LAST_CALL;
                case "sp_state_change:LAST_CALL_>_STOP_BET": return sp_state_change_LAST_CALL___STOP_BET;

                case "sp_state_change:STOP_BET_>_DEAL_FIRST": return sp_state_change_STOP_BET___DEAL_FIRST;
                case "sp_state_change:DEAL_FIRST_>_DONE_FIRST": return sp_state_change_DEAL_FIRST___DONE_FIRST;
                case "sp_state_change:DONE_FIRST_>_WATCH_FIRST": return sp_state_change_DONE_FIRST___WATCH_FIRST;
                case "sp_state_change:WATCH_FIRST_>_OPEN_FIRST": return sp_state_change_WATCH_FIRST___OPEN_FIRST;
                case "sp_state_change:WATCH_FIRST_>_GAME_DONE": return sp_state_change_WATCH_FIRST___GAME_DONE;
                case "sp_state_change:OPEN_FIRST_>_DEAL_PLAYER3": return sp_state_change_OPEN_FIRST___DEAL_PLAYER3;
                case "sp_state_change:OPEN_FIRST_>_DEAL_BANKER3": return sp_state_change_OPEN_FIRST___DEAL_BANKER3;

                case "sp_state_change:DEAL_PLAYER3_>_DONE_PLAYER3": return sp_state_change_DEAL_PLAYER3___DONE_PLAYER3;
                case "sp_state_change:DONE_PLAYER3_>_WATCH_PLAYER3": return sp_state_change_DONE_PLAYER3___WATCH_PLAYER3;
                case "sp_state_change:WATCH_PLAYER3_>_OPEN_PLAYER3": return sp_state_change_WATCH_PLAYER3___OPEN_PLAYER3;
                case "sp_state_change:WATCH_PLAYER3_>_GAME_DONE": return sp_state_change_WATCH_PLAYER3___GAME_DONE;

                case "sp_state_change:OPEN_PLAYER3_>_DEAL_BANKER3": return sp_state_change_OPEN_PLAYER3___DEAL_BANKER3;
                case "sp_state_change:DEAL_BANKER3_>_DONE_BANKER3": return sp_state_change_DEAL_BANKER3___DONE_BANKER3;
                case "sp_state_change:DONE_BANKER3_>_WATCH_BANKER3": return sp_state_change_DONE_BANKER3___WATCH_BANKER3;
                case "sp_state_change:WATCH_BANKER3_>_GAME_DONE": return sp_state_change_WATCH_BANKER3___GAME_DONE;

                case "sp_state_change:GAME_DONE_>_RESULT": return sp_state_change_GAME_DONE___RESULT;
                case "sp_state_change:RESULT_>_ANNOUNCE": return sp_state_change_RESULT___ANNOUNCE;
                case "sp_state_change:ANNOUNCE_>_CALC_PAY": return sp_state_change_ANNOUNCE___CALC_PAY;
                case "sp_state_change:CALC_PAY_>_CALC_PAY_DONE": return sp_state_change_CALC_PAY___CALC_PAY_DONE;
                case "sp_state_change:CALC_PAY_DONE_>_PAYOUT": return sp_state_change_CALC_PAY_DONE___PAYOUT;
                case "sp_state_change:PAYOUT_>_PREPARE_NEXT": return sp_state_change_PAYOUT___PREPARE_NEXT;

                default: break;
            }
            return null;
        }

        public string state_change_NEW_GAME(string name, int tableID)
        {
            string result = string.Empty;
            OpenDBConn();
            sp_state_change_NEW_GAME.Parameters["name"].Value = name;
            sp_state_change_NEW_GAME.Parameters["tableID"].Value = tableID;
            sp_state_change_NEW_GAME.ExecuteNonQuery();
            if ((int)sp_state_change_NEW_GAME.Parameters["returnCode"].Value == 0)
                result = formatReturnString(sp_state_change_NEW_GAME.Parameters["newGameID"].Value.ToString());
            else
                result = "FAIL#";
            CloseDBConn();
            return result;
        }
        public bool set_first_cards(int gameID, string playerRank1, string playerCard1, string playerRank2, string playerCard2, string bankerRank1, string bankerCard1, string bankerRank2, string bankerCard2, int playerTotal, int bankerTotal)
        {
            OpenDBConn();
            sp_action_game_set_first_cards.Parameters["gameID"].Value = gameID;
            sp_action_game_set_first_cards.Parameters["playerRank1"].Value = playerRank1;
            sp_action_game_set_first_cards.Parameters["playerCard1"].Value = playerCard1;
            sp_action_game_set_first_cards.Parameters["playerRank2"].Value = playerRank2;
            sp_action_game_set_first_cards.Parameters["playerCard2"].Value = playerCard2;
            sp_action_game_set_first_cards.Parameters["bankerRank1"].Value = bankerRank1;
            sp_action_game_set_first_cards.Parameters["bankerCard1"].Value = bankerCard1;
            sp_action_game_set_first_cards.Parameters["bankerRank2"].Value = bankerRank2;
            sp_action_game_set_first_cards.Parameters["bankerCard2"].Value = bankerCard2;
            sp_action_game_set_first_cards.Parameters["playerTotal"].Value = playerTotal;
            sp_action_game_set_first_cards.Parameters["bankerTotal"].Value = bankerTotal;
            sp_action_game_set_first_cards.ExecuteNonQuery();
            bool result = ((int)sp_action_game_set_first_cards.Parameters["returnCode"].Value == 0);
            CloseDBConn();
            return result;
        }
        public bool set_player_card3(int gameID, string playerRank3, string playerCard3, int playerTotal)
        {
            OpenDBConn();
            sp_action_game_set_player_card3.Parameters["gameID"].Value = gameID;
            sp_action_game_set_player_card3.Parameters["playerRank3"].Value = playerRank3;
            sp_action_game_set_player_card3.Parameters["playerCard3"].Value = playerCard3;
            sp_action_game_set_player_card3.Parameters["playerTotal"].Value = playerTotal;
            sp_action_game_set_player_card3.ExecuteNonQuery();
            bool result = ((int)sp_action_game_set_player_card3.Parameters["returnCode"].Value == 0);
            CloseDBConn();
            return result;
        }
        public bool set_banker_card3(int gameID, string bankerRank3, string bankerCard3, int bankerTotal)
        {
            OpenDBConn();
            sp_action_game_set_banker_card3.Parameters["gameID"].Value = gameID;
            sp_action_game_set_banker_card3.Parameters["bankerRank3"].Value = bankerRank3;
            sp_action_game_set_banker_card3.Parameters["bankerCard3"].Value = bankerCard3;
            sp_action_game_set_banker_card3.Parameters["bankerTotal"].Value = bankerTotal;
            sp_action_game_set_banker_card3.ExecuteNonQuery();
            bool result = ((int)sp_action_game_set_banker_card3.Parameters["returnCode"].Value == 0);
            CloseDBConn();
            return result;
        }
        public bool set_total(int gameID, int playerTotal, int bankerTotal)
        {
            OpenDBConn();
            sp_action_game_set_total.Parameters["gameID"].Value = gameID;
            sp_action_game_set_total.Parameters["playerTotal"].Value = playerTotal;
            sp_action_game_set_total.Parameters["bankerTotal"].Value = bankerTotal;
            sp_action_game_set_total.ExecuteNonQuery();
            bool result = ((int)sp_action_game_set_total.Parameters["returnCode"].Value == 0);
            CloseDBConn();
            return result;
        }
        public bool state_change(int gameID, string stateChange)
        {
            OpenDBConn();
            SqlCommand cmd = getStateCommandFromName(stateChange);
            cmd.Parameters["gameID"].Value = gameID;
            cmd.ExecuteNonQuery();
            bool result = ((int)cmd.Parameters["returnCode"].Value == 0);
            CloseDBConn();
            return result;
        }
        #endregion

        public static BaccaratStoredProcedureLocator getInstance()
        {
            if (self == null)
                self = new BaccaratStoredProcedureLocator();
            return self;
        }
        public static BaccaratStoredProcedureLocator getInstance(string logFilename)
        {
            if (self == null)
                self = new BaccaratStoredProcedureLocator(logFilename);
            return self;
        }
        private BaccaratStoredProcedureLocator()
        {
            startLogging("dealer_logx_" + System.DateTime.Today.ToString("yyyyMMDD") + ".txt");
            initCommands();
            initCommandList();
            setSQLConnToCommands();
        }
        private BaccaratStoredProcedureLocator(string logFileName)
        {
            startLogging(logFileName);
            initCommands();
            initCommandList();
            setSQLConnToCommands();
        }
        protected string formatReturnString(string input, string failMsg = null)
        {
            if (input == string.Empty)
                input = "FAIL#" + (failMsg == null ? string.Empty : failMsg);
            else
                input = "OK#" + input;
            return input;
        }

        protected TextWriter tw;

        public void startLogging(string logFilePath)
        {
            if (tw != null)
                tw.Close();
            else
                tw = new System.IO.StreamWriter(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/" + logFilePath, true);
        }
        public void print(string msg)
        {
            string message = "[" + System.DateTime.Now.ToLongTimeString() + "] " + msg;
            Console.WriteLine(message);
            if (tw != null)
            {
                try
                {
                    tw.WriteLine(message);
                }
                catch
                {
                    tw.Close();
                }
            }
        }
    }
}
