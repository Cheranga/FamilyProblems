using System;

namespace Lengaburu.Core.Exceptions
{
    public class CitizenDoesNotExist : Exception
    {
        public CitizenDoesNotExist(string message = "") : base(SetMessage(message))
        {
            
        }

        private static string SetMessage(string message)
        {
            return string.IsNullOrEmpty(message) ? "Citizen does not exist" : message;
        }
    }
}