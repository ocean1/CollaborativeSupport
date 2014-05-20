using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;

namespace CommonUtils
{
    public partial class ChooseFormatDlg : Form
    {
        private IDataObject iDataObject;
        
        public object dataObject;
        public string description;
        public string format;

        private bool autoSelectEnabled;
        private bool descriptionIsValid;
        private string defaultFormat;

        public ChooseFormatDlg(IDataObject dataObject)
        {
            InitializeComponent();
            this.iDataObject = dataObject;
        }

        private void ChooseFormatDlg_Load(object sender, EventArgs e)
        {

            if (iDataObject.GetDataPresent(DataFormats.UnicodeText)){
                defaultFormat = DataFormats.UnicodeText;
                autoSelectEnabled = true;}
            else if (iDataObject.GetDataPresent(DataFormats.Bitmap))
            {
                defaultFormat = DataFormats.Bitmap;
                autoSelectEnabled = true;
            }
            else if (iDataObject.GetDataPresent(DataFormats.WaveAudio))
            {
                defaultFormat = DataFormats.WaveAudio;
                autoSelectEnabled = true;
            }

            if (autoSelectEnabled && iDataObject.GetFormats(false).Length > 1) formatComboBox.Items.Add("Autoselect");
            foreach (string format in iDataObject.GetFormats(false))
            {
                this.formatComboBox.Items.Add(format);
            }

            formatComboBox.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!descriptionIsValid) return;

            if (formatComboBox.SelectedItem.ToString() == "Autoselect")
                format = defaultFormat;
            else
                format = formatComboBox.SelectedItem.ToString();

            dataObject = iDataObject.GetData(format);

            if (dataObject.GetType().IsSerializable && (dataObject.GetType() is ISerializable))
            {
                description = descriptionTxt.Text;
                errorProvider.SetError(formatComboBox, "");
                DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                errorProvider.SetError(formatComboBox, "Can't send this kind of format, please select another one!");
            }

        }


        private void descriptionTxt_Validating(object sender, CancelEventArgs e)
        {
            if (descriptionTxt.Text.Equals(""))
            {
                errorProvider.SetError(descriptionTxt, "Description should not be empty!");
                descriptionIsValid = false;
            }
            else
            {
                errorProvider.SetError(descriptionTxt, "");
                descriptionIsValid = true;
            }
        }

        private void formatComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {



        }


    }
}
