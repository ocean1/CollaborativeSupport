namespace Server
{
    partial class MainForm
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Liberare le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            if (server.videoManager != null)
                server.videoManager.Dispose();

            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SelectAreaBtn = new System.Windows.Forms.Button();
            this.SelectWindowBtn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.shortcutKeyTxt = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.shortcutKeyTxt);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.SelectAreaBtn);
            this.groupBox1.Controls.Add(this.SelectWindowBtn);
            this.groupBox1.Location = new System.Drawing.Point(470, 473);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(373, 52);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Screencast:";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(11, 28);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(65, 17);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Enabled";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // SelectAreaBtn
            // 
            this.SelectAreaBtn.Enabled = false;
            this.SelectAreaBtn.Location = new System.Drawing.Point(168, 24);
            this.SelectAreaBtn.Name = "SelectAreaBtn";
            this.SelectAreaBtn.Size = new System.Drawing.Size(90, 23);
            this.SelectAreaBtn.TabIndex = 6;
            this.SelectAreaBtn.Text = "Screen Area";
            this.SelectAreaBtn.UseVisualStyleBackColor = true;
            this.SelectAreaBtn.Click += new System.EventHandler(this.Select_Area_Click);
            // 
            // SelectWindowBtn
            // 
            this.SelectWindowBtn.Enabled = false;
            this.SelectWindowBtn.Location = new System.Drawing.Point(84, 24);
            this.SelectWindowBtn.Name = "SelectWindowBtn";
            this.SelectWindowBtn.Size = new System.Drawing.Size(78, 23);
            this.SelectWindowBtn.TabIndex = 1;
            this.SelectWindowBtn.Text = "Window";
            this.SelectWindowBtn.UseVisualStyleBackColor = true;
            this.SelectWindowBtn.Click += new System.EventHandler(this.SelectWindowBtn_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(96, 48);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "FullScreen";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(266, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "shortcut key:";
            // 
            // shortcutKeyTxt
            // 
            this.shortcutKeyTxt.Location = new System.Drawing.Point(264, 26);
            this.shortcutKeyTxt.Name = "shortcutKeyTxt";
            this.shortcutKeyTxt.ReadOnly = true;
            this.shortcutKeyTxt.Size = new System.Drawing.Size(100, 20);
            this.shortcutKeyTxt.TabIndex = 9;
            this.shortcutKeyTxt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.shortcutKeyTxt_KeyDown);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(855, 537);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.Controls.SetChildIndex(this.panel4, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button SelectAreaBtn;
        private System.Windows.Forms.Button SelectWindowBtn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox shortcutKeyTxt;
        private System.Windows.Forms.Label label3;
    }
}
