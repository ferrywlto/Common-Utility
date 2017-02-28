using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

using Security;

public partial class frmTransformer : Form
{  
    AESStringEncryptor aesTransformer;
    #region UI Event
    public frmTransformer()
    {
        InitializeComponent();
    }
    private void Form1_Load(object sender, EventArgs e)
    {
        aesTransformer = new AESStringEncryptor();
    }
    private void btnSave_Click(object sender, EventArgs e)
    {
        dlgSaveCipher.ShowDialog();
    }
    private void btnLoadTxt_Click(object sender, EventArgs e)
    {
        dlgOpenPlain.ShowDialog();
    }
    private void btnLoadKey_Click(object sender, EventArgs e)
    {
        dlgOpenKey.ShowDialog();
    }
    private void dlgSave_FileOk(object sender, CancelEventArgs e)
    {
        saveFile(dlgSaveCipher.FileName, txtTransform.Text);
    }
    private void dlgOpenPlain_FileOk(object sender, CancelEventArgs e)
    {
        loadFile(dlgOpenPlain.FileName);
    }
    private void dlgOpenKey_FileOk(object sender, CancelEventArgs e)
    {
        loadFile(dlgOpenKey.FileName);
    }
    private void btnNewKey_Click(object sender, EventArgs e)
    {
        txtKey.Text = aesTransformer.newAESKey();
    }
    private void btnSaveKey_Click(object sender, EventArgs e)
    {
        dlgSaveKey.ShowDialog();
    }
    private void dlgSaveKey_FileOk(object sender, CancelEventArgs e)
    {
        saveFile(dlgSaveKey.FileName, txtKey.Text);
    }
    private void btnTransform_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(txtOriginal.Text) || string.IsNullOrEmpty(txtKey.Text))
            {
                MessageBox.Show("Plain text or key cannot empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                txtTransform.Text = aesTransformer.AES_Crypto(
                    rdbEncrypt.Checked ? AESStringEncryptor.CryptoType.Encrypt : AESStringEncryptor.CryptoType.Decrypt,
                txtKey.Text, txtOriginal.Text);
            }
        }
        catch
        {
            MessageBox.Show("Error occurred when transforming text. Check whether the key and input text in valid format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    #endregion
    private void showIOError() { MessageBox.Show("File I/O Error.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    private void loadFile(string filePath)
    {
        try
        {
            using (StreamReader sr = File.OpenText(filePath))
            {
                txtOriginal.Text = sr.ReadToEnd();
                sr.Close();
            }
        }
        catch { showIOError(); }
    }
    private void saveFile(string filePath, string textToSave)
    {
        try
        {
            using (StreamWriter sw = File.CreateText(filePath))
            {
                sw.WriteLine(textToSave);
                sw.Flush();
                sw.Close();
            }
        }
        catch { showIOError(); }
    }


}