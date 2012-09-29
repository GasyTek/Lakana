using System;
using System.Linq;
using System.Threading;

namespace Samples.GasyTek.Lakana.Model
{
    public static class WebService
    {
        public static bool IsPhoneUnique(string phone)
        {
            // simulate a long operation
            Thread.Sleep(TimeSpan.FromSeconds(2));

            var existingPhones = new[] { "0000", "0001", "0002", "1234" };
            return existingPhones.Contains(phone) == false;
        }
    }
}