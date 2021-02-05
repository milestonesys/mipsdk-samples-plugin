using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace SCToast.Client
{
    /// <summary>
    /// Side panel user control that demonstrates how to use the Toast functionality from a MIP plugin. The user control
    /// allows creating, updating and removing toasts. Toast related callbacks from the environment in also listed by the
    /// side panel.
    /// </summary>
    public partial class SCToastSidePanelUserControl : SidePanelWpfUserControl
    {
        /// <summary>
        /// List of toasts that are currently active. ObservableCollection so it can be used directly as ItemsSource in the UI.
        /// </summary>
        private ObservableCollection<SmartClientToastData> _activeToasts = new ObservableCollection<SmartClientToastData>();

        public SCToastSidePanelUserControl()
        {
            InitializeComponent();

            _toastListView.ItemsSource = _activeToasts;

            UpdateButtonState();
        }

        private void _addToastButton_OnClick(object sender, RoutedEventArgs e)
        {
            //Open a new TextToastDetailsWindow window that allows the user to create the toast data
            TextToastDetailsWindow textToastDetailsWindow = new TextToastDetailsWindow(Dismissed, Activated, Expired);
            textToastDetailsWindow.ShowDialog();

            if (textToastDetailsWindow.SmartClientTextToastData != null)
            {
                //If toast data was created then add it to the active toasts collection and signal to the environment that the toast should be displayed
                _activeToasts.Add(textToastDetailsWindow.SmartClientTextToastData);
                EnvironmentManager.Instance.PostMessage(new Message(MessageId.SmartClient.ToastShowCommand, textToastDetailsWindow.SmartClientTextToastData));
            }
        }

        private void _updateToastButton_OnClick(object sender, RoutedEventArgs e)
        {
            SmartClientTextToastData selectedSmartClientTextToastData = (SmartClientTextToastData) _toastListView.SelectedItem;

            //Open a new TextToastDetailsWindow window that allows the user to edit the toast data
            TextToastDetailsWindow textToastDetailsWindow = new TextToastDetailsWindow(selectedSmartClientTextToastData);
            textToastDetailsWindow.ShowDialog();

            if (textToastDetailsWindow.SmartClientTextToastData != null)
            {
                //If toast data was updated then replace it in the active toasts collection and signal to the environment that the toast should be displayed
                int index = _activeToasts.IndexOf(selectedSmartClientTextToastData);
                if (index >= 0)
                {
                    //replace item in _activeToasts
                    _activeToasts.RemoveAt(index);
                    _activeToasts.Insert(index, textToastDetailsWindow.SmartClientTextToastData);
                }
                else
                {
                    //item does not exists in _activeToasts. Can happen if it expires while dialog is open. Add item again
                    _activeToasts.Add(textToastDetailsWindow.SmartClientTextToastData);
                }
                EnvironmentManager.Instance.PostMessage(new Message(MessageId.SmartClient.ToastShowCommand, textToastDetailsWindow.SmartClientTextToastData));
            }
        }

        private void _removeToastButton_OnClick(object sender, RoutedEventArgs e)
        {
            //Remove the toast from the active toasts collection and signal to the environment that the toast should be removed
            EnvironmentManager.Instance.PostMessage(new Message(MessageId.SmartClient.ToastHideCommand, ((SmartClientTextToastData)_toastListView.SelectedItem).Id));
            _activeToasts.Remove((SmartClientTextToastData)_toastListView.SelectedItem);
        }

        private void _toastListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonState();
        }

        /// <summary>
        /// Method that is passed as the Activated action when asking the environment to show toast messages. Will be called by
        /// the environment when the user activated a toast.
        /// </summary>
        /// <param name="smartClientToastData">The toast data associated with the toast that was activated.</param>
        private void Activated(SmartClientToastData smartClientToastData)
        {
            //Add callback info to _infoTextBox
            _infoTextBox.Text += $"{DateTime.Now.ToLongTimeString()} : Toast '{_activeToasts.Single(x => x == smartClientToastData).ToDisplayText()}' activated." + Environment.NewLine;
            _infoTextBox.ScrollToEnd();

            //if remove on activation is enabled then remove the toast from the active toasts collection and signal to the environment that the toast should be removed
            if (_removeOnActivationCheckBox.IsChecked.GetValueOrDefault(false))
            {
                _activeToasts.Remove(_activeToasts.Single(x => x == smartClientToastData));
                EnvironmentManager.Instance.PostMessage(new Message(MessageId.SmartClient.ToastHideCommand, smartClientToastData.Id));
            }
        }

        /// <summary>
        /// Method that is passed as the Dismissed action when asking the environment to show toast messages. Will be called by
        /// the environment when the user dismisses a toast.
        /// </summary>
        /// <param name="smartClientToastData">The toast data associated with the toast that was dismissed.</param>
        private void Dismissed(SmartClientToastData smartClientToastData)
        {
            //Add callback info to _infoTextBox
            _infoTextBox.Text += $"{DateTime.Now.ToLongTimeString()} : Toast '{_activeToasts.Single(x => x == smartClientToastData).ToDisplayText()}'' dismissed." + Environment.NewLine;
            _infoTextBox.ScrollToEnd();

            //Remove the toast from the active toasts collection
            _activeToasts.Remove(_activeToasts.Single(x => x == smartClientToastData));
        }

        /// <summary>
        /// Method that is passed as the Expired action when asking the environment to show toast messages. Will be called by
        /// the environment when a toast expires.
        /// </summary>
        private void Expired(SmartClientToastData smartClientToastData)
        {
            //Add callback info to _infoTextBox
            _infoTextBox.Text += $"{DateTime.Now.ToLongTimeString()} : Toast '{_activeToasts.Single(x => x == smartClientToastData).ToDisplayText()}' expired." + Environment.NewLine;
            _infoTextBox.ScrollToEnd();

            //Remove the toast from the active toasts collection
            _activeToasts.Remove(_activeToasts.Single(x => x == smartClientToastData));
        }

        private void UpdateButtonState()
        {
            _addToastButton.IsEnabled = true;
            _updateToastButton.IsEnabled = _toastListView.SelectedItem != null;
            _removeToastButton.IsEnabled = _toastListView.SelectedItem != null;
        }
    }

    internal static class SmartClientToastDataExtensions
    {
        public static string ToDisplayText(this SmartClientToastData smartClientToastData)
        {
            return (smartClientToastData as SmartClientTextToastData)?.PrimaryText ?? smartClientToastData.Id.ToString();
        }
    }

    internal class SmartClientToastDataToDisplayTextConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (value as SmartClientToastData)?.ToDisplayText();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}