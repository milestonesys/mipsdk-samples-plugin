using System;

namespace DemoAccessControlPlugin.Client
{
    internal class DemoApplicationClientException : Exception
    {
        public DemoApplicationClientException(string message)
            : base(message)
        { }

        public DemoApplicationClientException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
