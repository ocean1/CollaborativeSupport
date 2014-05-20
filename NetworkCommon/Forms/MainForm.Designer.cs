namespace CommonUtils
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
                cm.Dispose();
            }

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
            this.UsersListBox = new System.Windows.Forms.ListBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.chatTxtBox = new System.Windows.Forms.RichTextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.esciToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.esciToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.chiudiSessioneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.esciToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.aaaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.theToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.powerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ofToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.greyskullToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.clipboardMonitor1 = new CommonUtils.ClipboardMonitor(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.clipDataListView = new System.Windows.Forms.ListView();
            this.user = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.description = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.format = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.clipSendBtn = new System.Windows.Forms.Button();
            this.clipTxtBox = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // UsersListBox
            // 
            this.UsersListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UsersListBox.Enabled = false;
            this.UsersListBox.FormattingEnabled = true;
            this.UsersListBox.Location = new System.Drawing.Point(0, 19);
            this.UsersListBox.Name = "UsersListBox";
            this.UsersListBox.Size = new System.Drawing.Size(373, 160);
            this.UsersListBox.Sorted = true;
            this.UsersListBox.TabIndex = 10;
            // 
            // txtMessage
            // 
            this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessage.Location = new System.Drawing.Point(12, 504);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(370, 20);
            this.txtMessage.TabIndex = 12;
            this.txtMessage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMessage_KeyPress);
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Location = new System.Drawing.Point(388, 501);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 13;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // chatTxtBox
            // 
            this.chatTxtBox.BackColor = System.Drawing.SystemColors.Control;
            this.chatTxtBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chatTxtBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chatTxtBox.Location = new System.Drawing.Point(0, 0);
            this.chatTxtBox.Name = "chatTxtBox";
            this.chatTxtBox.ReadOnly = true;
            this.chatTxtBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.chatTxtBox.Size = new System.Drawing.Size(450, 484);
            this.chatTxtBox.TabIndex = 14;
            this.chatTxtBox.Text = "";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.chatTxtBox);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(452, 486);
            this.panel1.TabIndex = 15;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // esciToolStripMenuItem
            // 
            this.esciToolStripMenuItem.Name = "esciToolStripMenuItem";
            this.esciToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.esciToolStripMenuItem.Text = "Chiudi sessione";
            // 
            // esciToolStripMenuItem1
            // 
            this.esciToolStripMenuItem1.Name = "esciToolStripMenuItem1";
            this.esciToolStripMenuItem1.Size = new System.Drawing.Size(156, 22);
            this.esciToolStripMenuItem1.Text = "Esci";
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem1.Text = "File";
            // 
            // chiudiSessioneToolStripMenuItem
            // 
            this.chiudiSessioneToolStripMenuItem.Name = "chiudiSessioneToolStripMenuItem";
            this.chiudiSessioneToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.chiudiSessioneToolStripMenuItem.Text = "Chiudi sessione";
            // 
            // esciToolStripMenuItem2
            // 
            this.esciToolStripMenuItem2.Name = "esciToolStripMenuItem2";
            this.esciToolStripMenuItem2.Size = new System.Drawing.Size(156, 22);
            this.esciToolStripMenuItem2.Text = "Esci";
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem1.Text = "About";
            // 
            // aaaToolStripMenuItem
            // 
            this.aaaToolStripMenuItem.Name = "aaaToolStripMenuItem";
            this.aaaToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.aaaToolStripMenuItem.Text = "aaa";
            // 
            // theToolStripMenuItem
            // 
            this.theToolStripMenuItem.Name = "theToolStripMenuItem";
            this.theToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.theToolStripMenuItem.Text = "the";
            // 
            // powerToolStripMenuItem
            // 
            this.powerToolStripMenuItem.Name = "powerToolStripMenuItem";
            this.powerToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.powerToolStripMenuItem.Text = "power";
            // 
            // ofToolStripMenuItem
            // 
            this.ofToolStripMenuItem.Name = "ofToolStripMenuItem";
            this.ofToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.ofToolStripMenuItem.Text = "of";
            // 
            // greyskullToolStripMenuItem
            // 
            this.greyskullToolStripMenuItem.Name = "greyskullToolStripMenuItem";
            this.greyskullToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.greyskullToolStripMenuItem.Text = "greyskull";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.clipboardMonitor1);
            this.panel2.Controls.Add(this.UsersListBox);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(470, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(373, 190);
            this.panel2.TabIndex = 16;
            // 
            // clipboardMonitor1
            // 
            this.clipboardMonitor1.BackColor = System.Drawing.Color.Red;
            this.clipboardMonitor1.Location = new System.Drawing.Point(345, 1);
            this.clipboardMonitor1.Name = "clipboardMonitor1";
            this.clipboardMonitor1.Size = new System.Drawing.Size(28, 24);
            this.clipboardMonitor1.TabIndex = 21;
            this.clipboardMonitor1.Text = "clipboardMonitor1";
            this.clipboardMonitor1.Visible = false;
            this.clipboardMonitor1.ClipboardChange += new System.EventHandler<CommonUtils.ClipboardMonitor.ClipboardChangeEventArgs>(this.clipboardMonitor1_ClipboardChange);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.label1.Size = new System.Drawing.Size(373, 190);
            this.label1.TabIndex = 0;
            this.label1.Text = "Users:";
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.clipDataListView);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Location = new System.Drawing.Point(0, 1);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(373, 282);
            this.panel3.TabIndex = 17;
            // 
            // clipDataListView
            // 
            this.clipDataListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clipDataListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.user,
            this.description,
            this.format});
            this.clipDataListView.FullRowSelect = true;
            this.clipDataListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.clipDataListView.Location = new System.Drawing.Point(0, 19);
            this.clipDataListView.MultiSelect = false;
            this.clipDataListView.Name = "clipDataListView";
            this.clipDataListView.Size = new System.Drawing.Size(373, 248);
            this.clipDataListView.TabIndex = 1;
            this.clipDataListView.UseCompatibleStateImageBehavior = false;
            this.clipDataListView.View = System.Windows.Forms.View.Details;
            this.clipDataListView.DoubleClick += new System.EventHandler(this.clipDataList_DoubleClick);
            // 
            // user
            // 
            this.user.Text = "username";
            this.user.Width = 75;
            // 
            // description
            // 
            this.description.Text = "description";
            this.description.Width = 142;
            // 
            // format
            // 
            this.format.Text = "format";
            this.format.Width = 89;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.label2.Size = new System.Drawing.Size(373, 282);
            this.label2.TabIndex = 0;
            this.label2.Text = "Clipboard Sharing:";
            // 
            // clipSendBtn
            // 
            this.clipSendBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.clipSendBtn.BackColor = System.Drawing.SystemColors.Control;
            this.clipSendBtn.Enabled = false;
            this.clipSendBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.clipSendBtn.Location = new System.Drawing.Point(281, 288);
            this.clipSendBtn.Name = "clipSendBtn";
            this.clipSendBtn.Size = new System.Drawing.Size(92, 23);
            this.clipSendBtn.TabIndex = 19;
            this.clipSendBtn.Text = "Send Clipboard";
            this.clipSendBtn.UseVisualStyleBackColor = false;
            this.clipSendBtn.Click += new System.EventHandler(this.clipSendBtn_Click);
            // 
            // clipTxtBox
            // 
            this.clipTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.clipTxtBox.Location = new System.Drawing.Point(2, 290);
            this.clipTxtBox.Name = "clipTxtBox";
            this.clipTxtBox.ReadOnly = true;
            this.clipTxtBox.Size = new System.Drawing.Size(275, 20);
            this.clipTxtBox.TabIndex = 18;
            this.clipTxtBox.Text = "Clipboard content";
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.Controls.Add(this.clipSendBtn);
            this.panel4.Controls.Add(this.clipTxtBox);
            this.panel4.Controls.Add(this.panel3);
            this.panel4.Location = new System.Drawing.Point(470, 211);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(373, 313);
            this.panel4.TabIndex = 20;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(855, 537);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.panel4);
            this.MinimumSize = new System.Drawing.Size(600, 450);
            this.Name = "MainForm";
            this.Text = "Collaboration Server";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox UsersListBox;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.RichTextBox chatTxtBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem esciToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem esciToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem chiudiSessioneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem esciToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem aaaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem theToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem powerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ofToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem greyskullToolStripMenuItem;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView clipDataListView;
        private System.Windows.Forms.ColumnHeader user;
        private System.Windows.Forms.ColumnHeader format;
        private System.Windows.Forms.Button clipSendBtn;
        private System.Windows.Forms.TextBox clipTxtBox;
        private System.Windows.Forms.ColumnHeader description;
        protected System.Windows.Forms.Panel panel4;
        private ClipboardMonitor clipboardMonitor1;

    }
}

