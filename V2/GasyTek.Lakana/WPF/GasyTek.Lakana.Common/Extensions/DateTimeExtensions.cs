using System;

namespace GasyTek.Lakana.Common.Extensions
{
    /// <summary>
    /// Enum that explicit the result of DateTime.CompareTo method.
    /// </summary>
    public enum DateComparisonResult
    {
        /// <summary>
        /// Earlier.
        /// </summary>
        Earlier = -1,
        /// <summary>
        /// Later.
        /// </summary>
        Later = 1,
        /// <summary>
        /// TheSame.
        /// </summary>
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
