using System;
namespace TinyDemo.Common.Models
{
    public class JWTSettings
    {
        public string Secret { get; set; }
        public int Expiry { get; set; }
    }
}
