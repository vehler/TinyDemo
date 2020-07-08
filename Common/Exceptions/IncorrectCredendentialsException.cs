using System;
namespace TinyDemo.Common.Helpers
{
    public class IncorrectCredendentialsException : Exception
    {
        public IncorrectCredendentialsException(string message) : base(message)
        {
        }
    }
}
