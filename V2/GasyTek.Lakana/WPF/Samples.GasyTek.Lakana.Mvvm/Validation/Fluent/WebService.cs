using System;
using System.Threading;
using System.Linq;

namespace Samples.GasyTek.Lakana.Mvvm.Validation.Fluent
{
    /// <summary>
    /// A fake webservice.
    /// </summary>
    public class WebService
    {
        private static readonly string[] CodeCollection = {"ABC", "BCD", "XYZ"};

        /// <summary>
        /// Check whether code exists or not.
        /// </summary>
        public static bool CodeExist(string code)
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            return CodeCollection.Contains(code);
        }
    }
}
