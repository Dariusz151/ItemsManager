using System;

namespace ItemsManager.Common.Exceptions
{
    public class SmartFridgeException : Exception
    {
        public string Code { get; }

        public SmartFridgeException()
        {
        }

        public SmartFridgeException(string code)
        {
            Code = code;
        }

        public SmartFridgeException(string message, params object[] args) : this(string.Empty, message, args)
        {
        }

        public SmartFridgeException(string code, string message, params object[] args) : this(null, code, message, args)
        {
        }

        public SmartFridgeException(Exception innerException, string message, params object[] args)
            :this (innerException, string.Empty, message, args)
        {
        }

        public SmartFridgeException(Exception innerException, string code, string message, params object[] args)
            : base (string.Format(message, args), innerException)
        {
            Code = code;
        }
    }
}
