using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;

using DatabaseToolsGUI;

namespace DatabaseRestorer
{
    public partial class RestoreForm : MainForm
    {
        const string MSG_RESTORE_ERR = "數據庫 {0} 還原失敗, 請確定 SugarPOS 及 SugarPrinter 已關閉及擁有還原檔存取權限";
        
        const string RESTORE_COMMAND =
            @"USE [master]
            ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
            RESTORE DATABASE [{0}] FROM  DISK = N'{1}' WITH  FILE = 1,  NOUNLOAD,  REPLACE,  STATS = 5
            ALTER DATABASE [{0}] SET MULTI_USER";

        string restoreFilePath = string.Empty;

        public RestoreForm() : base()
        {
            InitializeComponent();
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            dlgRestoreFile.ShowDialog(this);
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(restoreFilePath))
                restoreDatabase();
            else
                lblPath.Text = "未選取檔案";
        }
        private void restoreDatabase()
        {
            using (SqlConnection conn = getOpenedSQLConnection())
            {
                if (conn == null) return;

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = String.Format(RESTORE_COMMAND, DbName, restoreFilePath);
                try { 
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("數據庫還原成功", "訊息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch { showErrorMessage(String.Format(MSG_RESTORE_ERR, DbName)); }
            }
        }

        private void dlgRestoreFile_FileOk(object sender, CancelEventArgs e)
        {
            restoreFilePath = dlgRestoreFile.FileName;
            lblPath.Text = restoreFilePath;
        }
    }
}
