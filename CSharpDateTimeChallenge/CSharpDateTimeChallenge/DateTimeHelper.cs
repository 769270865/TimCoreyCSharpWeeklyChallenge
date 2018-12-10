using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpDateTimeChallenge
{
    public class DateTimeHelper
    {
        /// <summary>
        /// Parse date from string based on format
        /// </summary>
        /// <param name="date">date string</param>
        /// <param name="dateFormat">date format dd-day, mm-month, yy-year; seperate by slash(/) </param>
        /// <returns>Parsed DateTime</returns>
        public static DateTime ParseDate(string date, string dateFormat)
        {

            int[] dateNumbers = new int[3];


            dateNumbers = convertDateString(dateFormat, date);

            DateTime dateTime = new DateTime(dateNumbers[0], dateNumbers[1], dateNumbers[2]);

            return dateTime;
        }
        /// <summary>
        /// Parse time from string based on format
        /// </summary>
        /// <param name="timeFormat">Time format HH-hour,mm-minute;seperate by :</param>
        /// <param name="time">time string</param>
        /// <returns>Parsed DateTime with date as current date</returns>
        public static DateTime ParseTime(string timeFormat, string time)
        {
            int[] timeNumber = new int[2];

            timeNumber = convertTimeString(timeFormat, time);

            DateTime currentTime = DateTime.Now;

            DateTime dateTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, timeNumber[0], timeNumber[1], 0);


            if (currentTime.TimeOfDay < dateTime.TimeOfDay)
            {
                dateTime.Subtract(new TimeSpan(24, 0, 0));
            }

            return dateTime;

        }
        /// <summary>
        /// Generate time comparson message with current time
        /// </summary>
        /// <param name="time">Time </param>
        /// <returns>"was {difference.Hours} hour and {difference.Minutes} minutes ago" for history date 
        /// and "is {difference.Hours} hours and {difference.Minutes} from now" for furutre date</returns>
        public static string GenerateTimeCompareMessage(DateTime time)
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan difference;

            string message = "";


            if (currentTime.CompareTo(time) > 0)
            {
                difference = currentTime.Subtract(time);
                message = $"was {difference.Hours} hour and {difference.Minutes} minutes ago";
            }
            else
            {
                difference = time.Subtract(currentTime);
                message = $"is {difference.Hours} hours and {difference.Minutes} from now";
            }

            return message;

        }


        /// <summary>
        /// Generate formatted date number array in the format of [yy,mm,dd]
        /// </summary>
        /// <param name="dateFormat">Format string in yy/mm/dd</param>
        /// <param name="date">Date string</param>
        /// <returns>Date in format [yy,mm,dd]</returns>
        private static int[] convertDateString(string dateFormat, string date)
        {
            string[] dateNumbers = date.Split('/');
            string[] dateFormatString = dateFormat.Split('/');
            int[] formattedDateNumbers = new int[3];


            for (int i = 0; i < dateFormatString.Length; i++)
            {
                switch (dateFormatString[i])
                {
                    case "yy":
                        formattedDateNumbers[0] = int.Parse(dateNumbers[i]);
                        break;
                    case "mm":
                        formattedDateNumbers[1] = int.Parse(dateNumbers[i]);
                        break;
                    case "dd":
                        formattedDateNumbers[2] = int.Parse(dateNumbers[i]);
                        break;
                    default:
                        break;
                }
            }
            return formattedDateNumbers;
        }
        /// <summary>
        /// Generate formmatted time number array in the format of [HH:mm]
        /// </summary>
        /// <param name="timeFormat">Format string in HH:mm</param>
        /// <param name="time">Time data</param>
        /// <returns>Time in format [HH:mm]</returns>
        private static int[] convertTimeString(string timeFormat, string time)
        {
            string[] timeNumbers = time.Split(':');
            string[] timeFormatString = timeFormat.Split(':');
            int[] formattedTimeNumbers = new int[2];

            for (int i = 0; i < formattedTimeNumbers.Length; i++)
            {
                switch (timeFormatString[i])
                {
                    case "HH":
                        formattedTimeNumbers[0] = int.Parse(timeNumbers[i]);
                        break;
                    case "mm":
                        formattedTimeNumbers[1] = int.Parse(timeNumbers[i]);
                        break;
                    default:
                        break;
                }
            }
            return formattedTimeNumbers;

        }
    }
}
