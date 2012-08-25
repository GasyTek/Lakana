using System;

namespace GasyTek.Tantana.Infrastructure.Extensions
{
    /// <summary>
    /// Enum that explicit the result of DateTime.CompareTo method.
    /// </summary>
    public enum DateComparisonResult
    {
        Earlier = -1,
        Later = 1,
        TheSame = 0
    };

    /// <summary>
    /// Provide a set of utility classes related to Date Time struct.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Compare a dateTime is bigger than whenDateTime. 
        /// </summary>
        /// <param name="dateTime">A date time</param>
        /// <param name="whenDateTime">When date time</param>
        /// <returns>True if dateTime is bigger then whenDateTime</returns>
        public static bool After(this DateTime dateTime, DateTime whenDateTime)
        {
            return dateTime > whenDateTime;
        }

        /// <summary>
        /// Compare a dateTime is smaller than whenDateTime. 
        /// </summary>
        /// <param name="dateTime">A date time</param>
        /// <param name="whenDateTime">When date time</param>
        /// <returns>True if dateTime is bigger then whenDateTime</returns>
        public static bool Before(this DateTime dateTime, DateTime whenDateTime)
        {
            return dateTime < whenDateTime;
        }
    }
}
