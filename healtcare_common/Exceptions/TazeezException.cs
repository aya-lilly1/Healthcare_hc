using System;

namespace HealthCare_Common.Extensions
{
    public class TazeezException : Exception
    {
        public int ErrorCode { get; set; }

        public TazeezException() : base("Tazeez Exception")
        {
        }

        public TazeezException(string message) : base(message)
        {
        }

        public TazeezException(int statusCode, string message) : base(message)
        {
            ErrorCode = statusCode;
        }

        public TazeezException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public TazeezException(int statusCode, string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = statusCode;
        }
    }
}
