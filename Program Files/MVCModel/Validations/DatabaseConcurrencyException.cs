using System;

namespace MVCModel.Validations
{
    public class DatabaseConcurrencyException : Exception
    {
        public DatabaseConcurrencyException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
