using System;
using System.Windows;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Messaging;

namespace AdminTabPlugin.Admin
{
    public partial class AdminTabCameraUserControl : VideoOS.Platform.Admin.TabUserControl
    {
        private Item _associatedItem;
        private bool _ignoreChanged = false;
        private AssociatedProperties _associatedProperties;

        public AdminTabCameraUserControl(Item item)
        {
            InitializeComponent();

            _associatedItem = item;
            labelItemName.Text = item.Name;
            ptzGroupBox.Visible = item.Properties.ContainsKey("PTZ") && item.Properties["PTZ"] == "Yes";
        }

        public override void Init()
        {
            base.Init();
            _ignoreChanged = true;
            textBox1.Text = "";
            textBox2.Text = "";

            _associatedProperties = Configuration.Instance.GetAssociatedProperties(_associatedItem, AdminTabPluginDefinition.AdminTabPluginTabPlugin);
            if (_associatedProperties.Properties.ContainsKey("Property1"))
                textBox1.Text = _associatedProperties.Properties["Property1"];

            if (_associatedProperties.Properties.ContainsKey("Property2"))
                textBox2.Text = _associatedProperties.Properties["Property2"];
            _ignoreChanged = false;
        }

        public override void Close()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            base.Close();
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            if (!_ignoreChanged)
            {
                _associatedProperties.Properties["Property1"] = textBox1.Text;
                _associatedProperties.Properties["Property2"] = textBox2.Text;
                FireConfigurationChanged();
            }
        }

        public override bool ValidateAndSave()
        {
            Configuration.Instance.SaveAssociatedProperties(_associatedProperties);
            return true;
        }

        private void SendPtzMove(string ptzCommand)
        {
            EnvironmentManager.Instance.PostMessage(new Message(MessageId.Control.PTZMoveCommand) { Data = ptzCommand }, _associatedItem.FQID);
        }

        private void upLeftButton_Click(object sender, EventArgs e)
        {
            SendPtzMove(PTZMoveCommandData.UpLeft);
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            SendPtzMove(PTZMoveCommandData.Up);
        }

        private void upRightButton_Click(object sender, EventArgs e)
        {
            SendPtzMove(PTZMoveCommandData.UpRight);
        }

        private void leftButton_Click(object sender, EventArgs e)
        {
            SendPtzMove(PTZMoveCommandData.Left);
        }

        private void homeButton_Click(object sender, EventArgs e)
        {
            SendPtzMove(PTZMoveCommandData.Home);
        }

        private void rightButton_Click(object sender, EventArgs e)
        {
            SendPtzMove(PTZMoveCommandData.Right);
        }

        private void downLeftButton_Click(object sender, EventArgs e)
        {
            SendPtzMove(PTZMoveCommandData.DownLeft);
        }

        private void downButton_Click(object sender, EventArgs e)
        {
            SendPtzMove(PTZMoveCommandData.Down);
        }

        private void downRightButton_Click(object sender, EventArgs e)
        {
            SendPtzMove(PTZMoveCommandData.DownRight);
        }

        private void getAbsolutePositionButton_Click(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            var answer = EnvironmentManager.Instance.SendMessage(new Message(MessageId.Control.PTZGetAbsoluteRequest), _associatedItem.FQID);
            if (answer.Count != 0 && answer[0] is PTZGetAbsoluteRequestData)
            {
                var position = (PTZGetAbsoluteRequestData)answer[0];
                try
                {
                    panNumericUpDown.Value = Convert.ToDecimal(position.Pan);
                    tiltNumericUpDown.Value = Convert.ToDecimal(position.Tilt);
                    zoomNumericUpDown.Value = Convert.ToDecimal(position.Zoom);
                }
                catch (OverflowException)
                {
                    MessageBox.Show("At least one of the returned values could not be parsed. Most likely because the device did not provide it.");
                }
            }
            UseWaitCursor = false;
        }

        private void setAbsolutePositionButton_Click(object sender, EventArgs e)
        {
            EnvironmentManager.Instance.PostMessage(
                new Message(MessageId.Control.PTZMoveAbsoluteCommand)
                {
                    Data = new PTZMoveAbsoluteCommandData()
                    {
                        Pan = (double)panNumericUpDown.Value,
                        Tilt = (double)tiltNumericUpDown.Value,
                        Zoom = (double)zoomNumericUpDown.Value
                    }
                }, _associatedItem.FQID);
        }
    }
}
