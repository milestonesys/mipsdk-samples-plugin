using System.Windows.Forms;
using SCSearchAgent.Properties;
using VideoOS.Platform.Search.FilterValues;

namespace SCSearchAgent.SCAnimalsSearchAgent.SearchUserControl.WinForms
{
    public partial class SCAnimalsSearchAreaFilterEditControl : UserControl
    {
        internal StringFilterValue FilterValue { get; set; }


        public SCAnimalsSearchAreaFilterEditControl()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            pictureBox1.Image = Resources.Area_1a;
            FilterValue.Text = "Custom Area #1";
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            pictureBox1.Image = Resources.Area_2a;
            FilterValue.Text = "Custom Area #2";
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            pictureBox1.Image = Resources.Area_3a;
            FilterValue.Text = "Custom Area #3";
        }
    }
}
