using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows;
using VideoOS.Platform.Messaging;

namespace SCToast.Client
{
    /// <summary>
    /// Dialog that allows creating or editing a TextToast type. 
    /// </summary>
    public partial class TextToastDetailsWindow : Window
    {
        private static Icon _iconGreen;
        private static Icon _iconRed;

        static TextToastDetailsWindow()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string name = assembly.GetName().Name;

            _iconGreen = new Icon(assembly.GetManifestResourceStream(name + ".Resources.IconGreen.ico"));
            _iconRed = new Icon(assembly.GetManifestResourceStream(name + ".Resources.IconRed.ico"));
        }

        private readonly Action<SmartClientToastData> _dismissed;
        private readonly Action<SmartClientToastData> _activated;
        private readonly Action<SmartClientToastData> _expired;

        public SmartClientTextToastData SmartClientTextToastData { get; private set; } = null;

        public TextToastDetailsWindow(Action<SmartClientToastData> dismissed, Action<SmartClientToastData> activated, Action<SmartClientToastData> expired)
        {
            Owner = Application.Current.MainWindow;

            InitializeComponent();

            _durationComboBox.ItemsSource = new List<DurationComboBoxItem>()
            {
                new DurationComboBoxItem() { DisplayText="Default", Duration=TimeSpan.Zero },
                new DurationComboBoxItem() { DisplayText="5 seconds", Duration=TimeSpan.FromSeconds(5) },
                new DurationComboBoxItem() { DisplayText="15 seconds", Duration=TimeSpan.FromSeconds(15) },
                new DurationComboBoxItem() { DisplayText="30 seconds", Duration=TimeSpan.FromSeconds(30) },
                new DurationComboBoxItem() { DisplayText="1 Minute", Duration=TimeSpan.FromMinutes(1) },
                new DurationComboBoxItem() { DisplayText="5 Minutes", Duration=TimeSpan.FromMinutes(5) },
                new DurationComboBoxItem() { DisplayText="1 Hour", Duration=TimeSpan.FromHours(1) },
                new DurationComboBoxItem() { DisplayText="Never", Duration=TimeSpan.MaxValue },
            };

            _iconComboBox.ItemsSource = new List<IconComboBoxItem>()
            {
                new IconComboBoxItem() { DisplayText = "[none]", Icon = null },
                new IconComboBoxItem() { DisplayText = "Green", Icon = _iconGreen },
                new IconComboBoxItem() { DisplayText = "Red", Icon = _iconRed },
            };

            _dismissed = dismissed;
            _activated = activated;
            _expired = expired;

            _idTextBlock.Text = Guid.NewGuid().ToString();
            _durationComboBox.SelectedIndex = 0;
            _iconComboBox.SelectedIndex = 0;
        }

        public TextToastDetailsWindow(SmartClientTextToastData smartClientTextToastData) : this(smartClientTextToastData.Dismissed, smartClientTextToastData.Activated, smartClientTextToastData.Expired)
        {
            _idTextBlock.Text = smartClientTextToastData.Id.ToString();
            _durationComboBox.SelectedItem = ((List<DurationComboBoxItem>)_durationComboBox.ItemsSource).Single(x => x.Duration == smartClientTextToastData.Duration);
            _iconComboBox.SelectedItem = ((List<IconComboBoxItem>)_iconComboBox.ItemsSource).Single(x => x.Icon == smartClientTextToastData.Icon);
            _primaryTextTextBox.Text = smartClientTextToastData.PrimaryText;
            _secondaryTextTextBox.Text = smartClientTextToastData.SecondaryText;
            _countTextTextBox.Text = smartClientTextToastData.CountText;
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            SmartClientTextToastData = new SmartClientTextToastData(Guid.Parse(_idTextBlock.Text), ((DurationComboBoxItem)_durationComboBox.SelectedItem).Duration, _dismissed, _activated, _expired, ((IconComboBoxItem)_iconComboBox.SelectedItem).Icon, _primaryTextTextBox.Text, _secondaryTextTextBox.Text, _countTextTextBox.Text);
            Close();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private class DurationComboBoxItem
        {
            public TimeSpan Duration { get; set; }
            public string DisplayText { get; set; }
        }

        private class IconComboBoxItem
        {
            public Icon Icon { get; set; }
            public string DisplayText { get; set; }
        }
    }
}
