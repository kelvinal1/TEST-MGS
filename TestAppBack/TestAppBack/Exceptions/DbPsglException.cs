using System;
using System.Runtime.Serialization;

namespace CurbeCorporativo.EXCEPTIONS
{
    public class DbPsglException : Exception
    {
        public DbPsglException()
        {
        }

        public DbPsglException(string message) : base(message)
        {
        }

        public DbPsglException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DbPsglException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
