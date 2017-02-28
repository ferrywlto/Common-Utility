
partial class frmTransformer
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.dlgOpenPlain = new System.Windows.Forms.OpenFileDialog();
            this.btnLoadTxt = new System.Windows.Forms.Button();
            this.btnTransform = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTransform = new System.Windows.Forms.Label();
            this.txtOriginal = new System.Windows.Forms.TextBox();
            this.txtTransform = new System.Windows.Forms.TextBox();
            this.btnLoadKey = new System.Windows.Forms.Button();
            this.rdbEncrypt = new System.Windows.Forms.RadioButton();
            this.rdbDecrypt = new System.Windows.Forms.RadioButton();
            this.dlgSaveCipher = new System.Windows.Forms.SaveFileDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.dlgOpenKey = new System.Windows.Forms.OpenFileDialog();
            this.btnNewKey = new System.Windows.Forms.Button();
            this.btnSaveKey = new System.Windows.Forms.Button();
            this.dlgSaveKey = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // dlgOpenPlain
            // 
            this.dlgOpenPlain.Filter = "Text Files|*.txt";
            this.dlgOpenPlain.FileOk += new System.ComponentModel.CancelEventHandler(this.dlgOpenPlain_FileOk);
            // 
            // btnLoadTxt
            // 
            this.btnLoadTxt.Location = new System.Drawing.Point(413, 36);
            this.btnLoadTxt.Name = "btnLoadTxt";
            this.btnLoadTxt.Size = new System.Drawing.Size(75, 23);
            this.btnLoadTxt.TabIndex = 0;
            this.btnLoadTxt.Text = "Load Text";
            this.btnLoadTxt.UseVisualStyleBackColor = true;
            this.btnLoadTxt.Click += new System.EventHandler(this.btnLoadTxt_Click);
            // 
            // btnTransform
            // 
            this.btnTransform.Location = new System.Drawing.Point(413, 220);
            this.btnTransform.Name = "btnTransform";
            this.btnTransform.Size = new System.Drawing.Size(99, 24);
            this.btnTransform.TabIndex = 1;
            this.btnTransform.Text = "Transform Text";
            this.btnTransform.UseVisualStyleBackColor = true;
            this.btnTransform.Click += new System.EventHandler(this.btnTransform_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(413, 259);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 24);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save Text";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.label2.Location = new System.Drawing.Point(-2, 223);
            this.label2.MaximumSize = new System.Drawing.Size(300, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Original Text:";
            // 
            // lblTransform
            // 
            this.lblTransform.AutoSize = true;
            this.lblTransform.Location = new System.Drawing.Point(15, 203);
            this.lblTransform.Name = "lblTransform";
            this.lblTransform.Size = new System.Drawing.Size(93, 13);
            this.lblTransform.TabIndex = 6;
            this.lblTransform.Text = "Transformed Text:";
            // 
            // txtOriginal
            // 
            this.txtOriginal.Location = new System.Drawing.Point(15, 36);
            this.txtOriginal.Multiline = true;
            this.txtOriginal.Name = "txtOriginal";
            this.txtOriginal.ReadOnly = true;
            this.txtOriginal.Size = new System.Drawing.Size(381, 56);
            this.txtOriginal.TabIndex = 7;
            // 
            // txtTransform
            // 
            this.txtTransform.Location = new System.Drawing.Point(15, 230);
            this.txtTransform.Multiline = true;
            this.txtTransform.Name = "txtTransform";
            this.txtTransform.ReadOnly = true;
            this.txtTransform.Size = new System.Drawing.Size(381, 56);
            this.txtTransform.TabIndex = 8;
            // 
            // btnLoadKey
            // 
            this.btnLoadKey.Location = new System.Drawing.Point(413, 74);
            this.btnLoadKey.Name = "btnLoadKey";
            this.btnLoadKey.Size = new System.Drawing.Size(75, 23);
            this.btnLoadKey.TabIndex = 9;
            this.btnLoadKey.Text = "Load Key";
            this.btnLoadKey.UseVisualStyleBackColor = true;
            this.btnLoadKey.Click += new System.EventHandler(this.btnLoadKey_Click);
            // 
            // rdbEncrypt
            // 
            this.rdbEncrypt.AutoSize = true;
            this.rdbEncrypt.Checked = true;
            this.rdbEncrypt.Location = new System.Drawing.Point(413, 188);
            this.rdbEncrypt.Name = "rdbEncrypt";
            this.rdbEncrypt.Size = new System.Drawing.Size(61, 17);
            this.rdbEncrypt.TabIndex = 11;
            this.rdbEncrypt.TabStop = true;
            this.rdbEncrypt.Text = "Encrypt";
            this.rdbEncrypt.UseVisualStyleBackColor = true;
            // 
            // rdbDecrypt
            // 
            this.rdbDecrypt.AutoSize = true;
            this.rdbDecrypt.Location = new System.Drawing.Point(480, 188);
            this.rdbDecrypt.Name = "rdbDecrypt";
            this.rdbDecrypt.Size = new System.Drawing.Size(62, 17);
            this.rdbDecrypt.TabIndex = 12;
            this.rdbDecrypt.Text = "Decrypt";
            this.rdbDecrypt.UseVisualStyleBackColor = true;
            // 
            // dlgSaveCipher
            // 
            this.dlgSaveCipher.Filter = "Text Files|*.txt";
            this.dlgSaveCipher.FileOk += new System.ComponentModel.CancelEventHandler(this.dlgSave_FileOk);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Key:";
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(15, 133);
            this.txtKey.Multiline = true;
            this.txtKey.Name = "txtKey";
            this.txtKey.ReadOnly = true;
            this.txtKey.Size = new System.Drawing.Size(381, 56);
            this.txtKey.TabIndex = 15;
            // 
            // dlgOpenKey
            // 
            this.dlgOpenKey.Filter = "Text Files|*.txt";
            this.dlgOpenKey.FileOk += new System.ComponentModel.CancelEventHandler(this.dlgOpenKey_FileOk);
            // 
            // btnNewKey
            // 
            this.btnNewKey.Location = new System.Drawing.Point(413, 112);
            this.btnNewKey.Name = "btnNewKey";
            this.btnNewKey.Size = new System.Drawing.Size(75, 23);
            this.btnNewKey.TabIndex = 16;
            this.btnNewKey.Text = "New Key";
            this.btnNewKey.UseVisualStyleBackColor = true;
            this.btnNewKey.Click += new System.EventHandler(this.btnNewKey_Click);
            // 
            // btnSaveKey
            // 
            this.btnSaveKey.Location = new System.Drawing.Point(413, 150);
            this.btnSaveKey.Name = "btnSaveKey";
            this.btnSaveKey.Size = new System.Drawing.Size(75, 23);
            this.btnSaveKey.TabIndex = 17;
            this.btnSaveKey.Text = "Save Key";
            this.btnSaveKey.UseVisualStyleBackColor = true;
            this.btnSaveKey.Click += new System.EventHandler(this.btnSaveKey_Click);
            // 
            // dlgSaveKey
            // 
            this.dlgSaveKey.Filter = "Text Files|*.txt";
            this.dlgSaveKey.FileOk += new System.ComponentModel.CancelEventHandler(this.dlgSaveKey_FileOk);
            // 
            // frmTransformer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 310);
            this.Controls.Add(this.btnSaveKey);
            this.Controls.Add(this.btnNewKey);
            this.Controls.Add(this.txtKey);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rdbDecrypt);
            this.Controls.Add(this.rdbEncrypt);
            this.Controls.Add(this.btnLoadKey);
            this.Controls.Add(this.txtTransform);
            this.Controls.Add(this.txtOriginal);
            this.Controls.Add(this.lblTransform);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnTransform);
            this.Controls.Add(this.btnLoadTxt);
            this.Name = "frmTransformer";
            this.Text = "AES Text Transformer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.OpenFileDialog dlgOpenPlain;
    private System.Windows.Forms.Button btnLoadTxt;
    private System.Windows.Forms.Button btnTransform;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label lblTransform;
    private System.Windows.Forms.TextBox txtOriginal;
    private System.Windows.Forms.TextBox txtTransform;
    private System.Windows.Forms.Button btnLoadKey;
    private System.Windows.Forms.RadioButton rdbEncrypt;
    private System.Windows.Forms.RadioButton rdbDecrypt;
    private System.Windows.Forms.SaveFileDialog dlgSaveCipher;
    private System.ComponentModel.BackgroundWorker backgroundWorker1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox txtKey;
    private System.Windows.Forms.OpenFileDialog dlgOpenKey;
    private System.Windows.Forms.Button btnNewKey;
    private System.Windows.Forms.Button btnSaveKey;
    private System.Windows.Forms.SaveFileDialog dlgSaveKey;
}


