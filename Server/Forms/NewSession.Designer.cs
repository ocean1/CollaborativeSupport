namespace Server
{
    partial class NewSession
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewSession));
            this.StartServerBtn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pwdTxtBox = new System.Windows.Forms.TextBox();
            this.portTxtBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.connectionProgressBar = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // StartServerBtn
            // 
            this.StartServerBtn.Location = new System.Drawing.Point(71, 232);
            this.StartServerBtn.Name = "StartServerBtn";
            this.StartServerBtn.Size = new System.Drawing.Size(75, 23);
            this.StartServerBtn.TabIndex = 11;
            this.StartServerBtn.Text = "Start Server";
            this.StartServerBtn.UseVisualStyleBackColor = true;
            this.StartServerBtn.Click += new System.EventHandler(this.StartServerBtn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtUsername);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.pwdTxtBox);
            this.groupBox2.Controls.Add(this.portTxtBox);
            this.groupBox2.Location = new System.Drawing.Point(71, 92);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(238, 129);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Server Info:";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(72, 104);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(153, 17);
            this.checkBox1.TabIndex = 15;
            this.checkBox1.Text = "Remember for next session";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.label1.Location = new System.Drawing.Point(5, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Port:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Password:";
            // 
            // pwdTxtBox
            // 
            this.pwdTxtBox.Location = new System.Drawing.Point(62, 45);
            this.pwdTxtBox.Name = "pwdTxtBox";
            this.pwdTxtBox.Size = new System.Drawing.Size(163, 20);
            this.pwdTxtBox.TabIndex = 12;
            this.pwdTxtBox.Text = "password";
            this.pwdTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.pwdTxtBox.UseSystemPasswordChar = true;
            this.pwdTxtBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressHandler);
            // 
            // portTxtBox
            // 
            this.portTxtBox.Location = new System.Drawing.Point(62, 71);
            this.portTxtBox.Name = "portTxtBox";
            this.portTxtBox.Size = new System.Drawing.Size(163, 20);
            this.portTxtBox.TabIndex = 2;
            this.portTxtBox.Text = "6500";
            this.portTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.portTxtBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressHandler);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(67, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(232, 20);
            this.label5.TabIndex = 13;
            this.label5.Text = "Avvia nuova sessione del server";
            // 
            // connectionProgressBar
            // 
            this.connectionProgressBar.BackColor = System.Drawing.SystemColors.Control;
            this.connectionProgressBar.Location = new System.Drawing.Point(71, 232);
            this.connectionProgressBar.Margin = new System.Windows.Forms.Padding(8);
            this.connectionProgressBar.MarqueeAnimationSpeed = 20;
            this.connectionProgressBar.Name = "connectionProgressBar";
            this.connectionProgressBar.Size = new System.Drawing.Size(238, 23);
            this.connectionProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.connectionProgressBar.TabIndex = 14;
            this.connectionProgressBar.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "&Name:";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(62, 19);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(163, 20);
            this.txtUsername.TabIndex = 17;
            this.txtUsername.Text = "username";
            this.txtUsername.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // NewSession
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(381, 297);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.connectionProgressBar);
            this.Controls.Add(this.StartServerBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "NewSession";
            this.Text = "New Server Session";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartServerBtn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox portTxtBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox pwdTxtBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ProgressBar connectionProgressBar;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUsername;

    }
}