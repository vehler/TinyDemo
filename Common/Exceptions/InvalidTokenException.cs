using System;
namespace TinyDemo.Common.Helpers
{
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException(string message) : base(message)
        {
        }
    }
}
