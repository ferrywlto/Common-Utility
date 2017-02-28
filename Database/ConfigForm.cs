using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DatabaseToolsGUI
{
    public partial class ConfigForm : Form
    {
        MainForm frmParent;
        public ConfigForm(MainForm parent)
        {
            InitializeComponent();

            frmParent = parent;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            frmParent.DbName = cbDatabase.Text;
            frmParent.DbServer = txtInstance.Text;
            frmParent.DbUser = txtLogin.Text;
            frmParent.DbPassword = txtPassword.Text;
            this.Hide();
        }
        public void update_fields(string server, string database, string login, string password)
        {
            txtInstance.Text = server;
            cbDatabase.Text = database;
            txtLogin.Text = login;
            txtPassword.Text = password;
        }
    }
}
