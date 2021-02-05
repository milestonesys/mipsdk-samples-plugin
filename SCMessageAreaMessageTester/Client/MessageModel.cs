using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using VideoOS.Platform.Messaging;

namespace SCMessageAreaMessageTester.Client
{
    class MessageModel : INotifyPropertyChanged
    {
        private static int _count = 0;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public object MessageId { get; private set; }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged("Message");
                }
            }
        }

        public List<SmartClientMessageDataType> MessageTypes
        {
            get { return Enum.GetValues(typeof(SmartClientMessageDataType)).Cast<SmartClientMessageDataType>().ToList(); }
        }

        private SmartClientMessageDataType _messageType;
        public SmartClientMessageDataType MessageType
        {
            get { return _messageType; }
            set
            {
                if (_messageType != value)
                {
                    _messageType = value;
                    OnPropertyChanged("MessageType");
                }
            }
        }

        public List<SmartClientMessageDataPriority> Priorities
        {
            get { return Enum.GetValues(typeof(SmartClientMessageDataPriority)).Cast<SmartClientMessageDataPriority>().ToList(); }
        }

        private SmartClientMessageDataPriority _priority;
        public SmartClientMessageDataPriority Priority
        {
            get { return _priority; }
            set
            {
                if (_priority != value)
                {
                    _priority = value;
                    OnPropertyChanged("Priority");
                }
            }
        }

        private bool _isClosable;
        public bool IsClosable 
        {
            get { return _isClosable; }
            set
            {
                if(_isClosable != value)
                {
                    _isClosable = value;
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IsClosable"));
                    OnPropertyChanged("IsClosable");
                }
            }
        }

        private string _buttonText;
        public string ButtonText 
        {
            get { return _buttonText; }
            set
            {
                if(_buttonText != value)
                {
                    _buttonText = value;
                    OnPropertyChanged("ButtonText");
                }
            }
        }

        public List<SmartClientMessageDataTaskState> TaskStates
        {
            get { return Enum.GetValues(typeof(SmartClientMessageDataTaskState)).Cast<SmartClientMessageDataTaskState>().ToList(); }
        }


        private SmartClientMessageDataTaskState _taskState;
        public SmartClientMessageDataTaskState TaskState
        {
            get { return _taskState; }
            set
            {
                if (_taskState != value)
                {
                    _taskState = value;
                    OnPropertyChanged("TaskState");
                }
            }
        }

        private double _taskProgress;
        public double TaskProgress 
        {
            get { return _taskProgress; }
            set
            {
                if (_taskProgress != value)
                {
                    _taskProgress = value;
                    OnPropertyChanged("TaskProgress");
                }
            }
        }

        private string _taskText;
        public string TaskText
        {
            get { return _taskText; }
            set
            {
                if (_taskText != value)
                {
                    _taskText = value;
                    OnPropertyChanged("TaskText");
                }
            }
        }

        public MessageModel()
        {
            MessageId = new object();
            Message = "Message #" + ++_count;

            //init model values with the default values from SmartClientMessageData
            SmartClientMessageData smartClientMessageData = new SmartClientMessageData();
            MessageType = smartClientMessageData.MessageType;
            Priority = smartClientMessageData.Priority;
            IsClosable = smartClientMessageData.IsClosable;
            ButtonText = smartClientMessageData.ButtonText;
            TaskState = smartClientMessageData.TaskState;
            TaskProgress = smartClientMessageData.TaskProgress;
            TaskText = smartClientMessageData.TaskText;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
