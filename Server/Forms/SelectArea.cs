using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Server
{

    public partial class SelectArea : Form
    {
        private MainForm frm1;

        public SelectArea()
        {
            InitializeComponent();
        }

        public SelectArea(Form frm1) : this()
        {
            

            // SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.TransparencyKey = this.BackColor;

            // this.BackColor = Color.FromArgb(50,200,200,00);
            this.MouseDown += new MouseEventHandler(this.Mouse_Click);
            this.frm1 = (MainForm)frm1;
        }

        /*protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                e = null;
            }

            base.OnMouseClick(e);
        }*/

        private void Mouse_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.frm1.AreaSelected = this.RectangleToScreen(this.DisplayRectangle);

                // Point ClickPoint = new Point(System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y);
                this.Close();
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
