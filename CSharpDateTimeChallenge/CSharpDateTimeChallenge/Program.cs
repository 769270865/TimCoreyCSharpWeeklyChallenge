using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpDateTimeChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            string dateInput = "";
            string dateFormatInput = "";

            DateTime inputDateTime;
            DateTime currentDate = DateTime.Now;
            int dayDifferentFromInputDate;


            Console.Write("Give me a date: ");
            dateInput = Console.ReadLine();

            Console.Write("What date format do you want to use(dd-day mm-month yy-year, seperated by slash /): ");
            dateFormatInput = Console.ReadLine();

            inputDateTime = DateTimeHelper.ParseDate(dateInput, dateFormatInput);
            dayDifferentFromInputDate = inputDateTime.Subtract(currentDate).Days;

            Console.WriteLine(dayDifferentFromInputDate > 0 ? $"This day is {dayDifferentFromInputDate} days in future" :
                                                              $"It has been {Math.Abs(dayDifferentFromInputDate)} days since {dateInput}");


            string timeInput = "";
            string timeFormat = "";
            DateTime inputTime;


            Console.Write("Give me a time: ");
            timeInput = Console.ReadLine();
            Console.Write("What time format do you want to use(HH-hour:mm-minute): ");
            timeFormat = Console.ReadLine();
            inputTime = DateTimeHelper.ParseTime(timeFormat, timeInput);
            Console.WriteLine($"{timeInput} {DateTimeHelper.GenerateTimeCompareMessage(inputTime)}");

            Console.ReadLine();
        }


    }
    
}
