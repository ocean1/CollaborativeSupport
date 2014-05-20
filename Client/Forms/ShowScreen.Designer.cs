namespace Client
{
    partial class ShowScreen
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
            this.screenPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.screenPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // screenPictureBox
            // 
            this.screenPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.screenPictureBox.Location = new System.Drawing.Point(0, 0);
            this.screenPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.screenPictureBox.Name = "screenPictureBox";
            this.screenPictureBox.Size = new System.Drawing.Size(352, 211);
            this.screenPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.screenPictureBox.TabIndex = 0;
            this.screenPictureBox.TabStop = false;
            this.screenPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.screenPictureBox_Paint);
            // 
            // ShowScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(352, 211);
            this.Controls.Add(this.screenPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ShowScreen";
            this.Text = "Server Screen";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShowScreen_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.screenPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox screenPictureBox;
    }
}