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

namespace DatabaseToolsGUI
{
    public partial class MainForm : Form
    {
        const string CONNSTR = @"Data Source={0};Initial Catalog={1};User ID={2};Password={3}";
        const string OLD_PW = "TWINKLETWINKLE35909337";
        const string NEW_PW = "123";

        public string DbServer = @"sugar01\SQLEXPRESS";
        public string DbName = "SugarPOS";
        public string DbUser = "sa";
        public string DbPassword = OLD_PW;

        ConfigForm frmConfig;
        public MainForm()
        {
            InitializeComponent();
            frmConfig = new ConfigForm(this);
        }

        protected SqlConnection getOpenedSQLConnection()
        {
            SqlConnection conn = new SqlConnection(string.Format(CONNSTR, DbServer, DbName, DbUser, DbPassword));
            try { conn.Open(); }
            catch
            {
                conn = new SqlConnection(string.Format(CONNSTR, DbServer, DbName, DbUser, OLD_PW));
                try { conn.Open(); }
                catch
                {
                    conn = new SqlConnection(string.Format(CONNSTR, DbServer, DbName, DbUser, NEW_PW));
                    try { conn.Open(); }
                    catch
                    {
                        showErrorMessage("連接數據庫失敗");
                        return null;
                    }
                }
            }
            return conn;
        }

        protected void showMessage(string msg)
        {
            MessageBox.Show(msg, "訊息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        protected void showErrorMessage(string errMsg)
        {
            MessageBox.Show(errMsg, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void menuItem_Click(object sender, EventArgs e)
        {
            frmConfig.update_fields(DbServer, DbName, DbUser, DbPassword);
            frmConfig.Show();
            frmConfig.SetDesktopLocation(this.Location.X, this.Location.Y + this.Size.Height + 20);
        }
    }
}
