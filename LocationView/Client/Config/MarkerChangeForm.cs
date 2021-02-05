using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.UI;

namespace LocationView.Client.Config
{
    public partial class MarkerChangeForm : Form
    {
        public enum Mode
        {
            Add = 0,
            Edit
        }

        public static readonly Dictionary<Mode, string> Titles =
            new Dictionary<Mode, string>()
            {
                { Mode.Add, "Add Marker"},
                { Mode.Edit, "Edit Marker"},
            };

        public MarkerChangeForm(Mode mode = Mode.Add)
        {
            InitializeComponent();

            InitializeMarkerTypes();

            WorkMode = mode;
        }

        private void InitializeMarkerTypes()
        {
            comboBoxMarker.Items.Clear();
            foreach (MarkerTypes type in Enum.GetValues(typeof(MarkerTypes)))
            {
                comboBoxMarker.Items.Add(MarkerNames.GetName(type));
            }

            SetSelectedMarkerType("");
        }

        private void SetSelectedMarkerType(string markerType)
        {
            var index = comboBoxMarker.Items.IndexOf(markerType);
            if ((index < 0) || (index >= comboBoxMarker.Items.Count))
                index = 0;
            comboBoxMarker.SelectedIndex = index;
        }

        private Mode _mode = Mode.Add;

        internal Mode WorkMode
        {
            get { return _mode; }
            private set 
            { 
                _mode = value;
                this.Text = Titles[_mode];
            }
        }

        private readonly Marker _marker = new Marker();

        internal Marker Marker
        {
            get
            {
                return new Marker()
                       {
                           DeviceId = _marker.DeviceId,
                           DeviceName = _marker.DeviceName,
                           MarkerType = _marker.MarkerType
                       };
            }
            set
            {
                _marker.InitFrom(value);

                _selectedMetadataItem = Configuration.Instance.GetItem(_marker.DeviceId, Kind.Metadata);
                buttonDevice.Text = _selectedMetadataItem.Name;

                SetSelectedMarkerType(MarkerNames.GetName(_marker.MarkerType));
            }
        }

        private Item _selectedMetadataItem = null;

        private void ButtonDeviceClick(object sender, EventArgs e)
        {
			var form = new ItemPickerForm();
			form.KindFilter = Kind.Metadata;
			form.SelectedItem = _selectedMetadataItem;
			form.AutoAccept = true;
			form.Init();
			if (form.ShowDialog() == DialogResult.OK)
			{
				_selectedMetadataItem = form.SelectedItem;

			    _marker.DeviceId = _selectedMetadataItem.FQID.ObjectId;
			    _marker.DeviceName = _selectedMetadataItem.Name;

                buttonDevice.Text = _selectedMetadataItem.Name;
			}
        }

        private void ComboBoxMarkerSelectedIndexChanged(object sender, EventArgs e)
        {
            var selIndex = comboBoxMarker.SelectedIndex;
            if ((selIndex >= 0) &
                (selIndex < comboBoxMarker.Items.Count))
            {
                _marker.MarkerType = MarkerNames.GetType((string)comboBoxMarker.Items[selIndex]);
            }
        }
    }
}
