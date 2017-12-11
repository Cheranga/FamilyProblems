using System;

namespace Lengaburu.Core.Exceptions
{
    public class PersonDoesNotExist : Exception
    {
        public PersonDoesNotExist(string message = "") : base(SetMessage(message))
        {
            
        }

        private static string SetMessage(string message)
        {
            return string.IsNullOrEmpty(message) ? "Person does not exist" : message;
        }
    }
}