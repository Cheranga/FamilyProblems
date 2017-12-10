using System;

namespace Problem1.Exceptions
{
    public class DoNotCheatException : Exception
    {
        public DoNotCheatException(string message = "") : base(SetMessage(message))
        {
            
        }

        private static string SetMessage(string message)
        {
            return string.IsNullOrEmpty(message) ? "You already have a partner. Please behave yourself!" : message;
        }
    }
}