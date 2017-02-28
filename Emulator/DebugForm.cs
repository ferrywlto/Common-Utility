using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace com.sky88.game.bet.baccarat
{
    partial class DebugForm : Form
    {
        public DebugForm()
        {
            InitializeComponent();
        }

        public void printDebugMsg(string msg)
        {
            txtMsg.AppendText(msg + Environment.NewLine);
            txtMsg.ScrollToCaret();
        }

        private void DebugForm_Load(object sender, EventArgs e)
        {
        }
    }
}
