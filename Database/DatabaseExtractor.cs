using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DatabaseToolsGUI;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace DatabaseExtractor
{
    public partial class BackupForm : MainForm
    {
        const string BACKUP_COMMAND = @"BACKUP DATABASE [{0}] TO  DISK = N'{1}' WITH NOFORMAT, INIT,  NAME = N'{0}-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";
        const string BACKUP_PATH = @"{0}\{1}_Backup_{2}.bak";
        const string MSG_BACKUP_ERR = "備份數據庫 {0} 失敗, 請確定 SugarPOS 及 SugarPrinter 已關閉及擁有路徑存取權限";

        public BackupForm() : base()
        {
            InitializeComponent();
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            dlgBackupFolder.ShowDialog(this);
            lblPath.Text = dlgBackupFolder.SelectedPath;
        }
        private void btnBackup_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(dlgBackupFolder.SelectedPath))
            {
                backupDatabase();
            }
            else
            {
                lblPath.Text = "未選取路徑";
            }
        }
        private void backupDatabase()
        {
            string backupDateStr = DateTime.Now.ToString("yyyyMMddhhmmss");

            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = getOpenedSQLConnection())
            {
                if (conn != null)
                {
                    cmd.Connection = conn;

                    if (executeBackupCommand(cmd, DbName))
                        showMessage("備份完成");
                }
            }
        }
        private bool executeBackupCommand(SqlCommand dbCmd, string dbName)
        {
            string filePath = String.Format(BACKUP_PATH, dlgBackupFolder.SelectedPath, dbName, DateTime.Now.ToString("yyyyMMddHHmmss"));
            try
            {
                dbCmd.CommandText = String.Format(BACKUP_COMMAND, dbName, filePath);
                dbCmd.ExecuteNonQuery();
            }
            catch
            {
                showErrorMessage(String.Format(MSG_BACKUP_ERR, dbName));
                return false;
            }
            return true;
        }
    }
}
