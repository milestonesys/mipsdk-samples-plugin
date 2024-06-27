using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using VideoOS.Platform;

namespace MessageTester.MessageDataControls
{
    public abstract class MessageDataSuper : UserControl ,INotifyPropertyChanged 
    {
        public virtual FQID Destination { get => null; }
        public virtual FQID Related { get => null; }
        public virtual object Data { get => null; }

        private bool _isReady = false;
        public bool IsReadyToSend  
        {
            get => _isReady;
            protected set
            {
                _isReady = value;
                NotifyPropertyChanged(nameof(IsReadyToSend));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
