using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outlook_Date_Range
{
    class Program
    {
        public static void Main(string[] args)
        {
            Program p = new Program();
            string timezoneId = TimeZone.CurrentTimeZone.StandardName;
            var UtcOffSet = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
            var daterangelist = p.EmailCardViewDateGrouping(timezoneId, UtcOffSet);
            foreach(var item in daterangelist)
            {
                Console.WriteLine("Date {0}    || Start Time {1}     || End Date {2}   ", item.DateValue, item.StartDate.ToString(), item.EndDate.ToString());
                Console.WriteLine("<-------->");
            }
            Console.ReadKey();
        }
        public List<DateFilters> EmailCardViewDateGrouping(string Timezone, TimeSpan UTCOffset)
        {
            var dateFilterList = new List<DateFilters>();
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById(Timezone);
            var currentDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cstZone); // Tuesday
            var startWeekDay = currentDate.DayOfWeek;
            var startWeekDayNumber = (int)startWeekDay;

            for (int i = 0; i <= startWeekDayNumber; i++)
            {
                if (i == 0)
                {
                    //Today
                    var startDate = currentDate.Date.AddDays(-i);
                    var endDate = startDate.AddDays(1).AddTicks(-1);
                    dateFilterList.Add(new DateFilters { DateValue = "Today", StartDate = startDate, EndDate = endDate, flag = false });
                }
                else if (i == 1)
                {
                    //Yesterday
                    var startDate = currentDate.Date.AddDays(-i);
                    var endDate = startDate.AddDays(1).AddTicks(-1);
                    dateFilterList.Add(new DateFilters { DateValue = "Yesterday", StartDate = startDate, EndDate = endDate, flag = false });

                }
                else
                {
                    //Days
                    var res = startWeekDay - i;
                    var startDate = currentDate.Date.AddDays(-i);
                    var endDate = startDate.AddDays(1).AddTicks(-1);
                    dateFilterList.Add(new DateFilters { DateValue = res.ToString(), StartDate = startDate, EndDate = endDate, flag = false });
                }
            }

            //Last Week
            var lastWeekEndDate = currentDate.Date.AddDays(-(startWeekDayNumber + 1));
            var lastWeekStartDate = lastWeekEndDate.AddDays(-6);
            lastWeekEndDate = lastWeekEndDate.AddDays(1).AddTicks(-1);
            dateFilterList.Add(new DateFilters { DateValue = "Last Week", StartDate = lastWeekStartDate, EndDate = lastWeekEndDate, flag = false });

            //Two Weeks Ago
            var lastTwoWeekEndDate = lastWeekStartDate.Date.AddDays(-1);
            var lastTwoWeekStartDate = lastTwoWeekEndDate.AddDays(-6);
            lastTwoWeekEndDate = lastTwoWeekEndDate.AddDays(1).AddTicks(-1);
            dateFilterList.Add(new DateFilters { DateValue = "Two Weeks Ago", StartDate = lastTwoWeekStartDate, EndDate = lastTwoWeekEndDate, flag = false });

            //Three Weeks Ago
            var lastThreeWeekEndDate = lastTwoWeekStartDate.Date.AddDays(-1);
            var lastThreeWeekStartDate = lastThreeWeekEndDate.AddDays(-6);
            lastThreeWeekEndDate = lastThreeWeekEndDate.AddDays(1).AddTicks(-1);
            dateFilterList.Add(new DateFilters { DateValue = "Three Weeks Ago", StartDate = lastThreeWeekStartDate, EndDate = lastThreeWeekEndDate, flag = false });

            //Last Month
            var lastMonthEndDate = lastThreeWeekStartDate.Date.AddDays(-1);
            var lastMonthStartDate = new DateTime(lastMonthEndDate.Year, lastMonthEndDate.Month, 1);
            lastMonthEndDate = lastMonthEndDate.AddDays(1).AddTicks(-1);
            dateFilterList.Add(new DateFilters { DateValue = "Last Month", StartDate = lastMonthStartDate, EndDate = lastMonthEndDate, flag = false });

            //Older 02-02-0002
            var olderEndDate = lastMonthStartDate.Date.AddDays(-1);
            olderEndDate = olderEndDate.AddDays(1).AddTicks(-1);
            dateFilterList.Add(new DateFilters { DateValue = "Older", StartDate = DateTime.MinValue, EndDate = olderEndDate, flag = false });

            return dateFilterList;
        }
    }
    public class DateFilters
    {
        public string DateValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool flag { get; set; }
        public bool tempFlag { get; set; }
    }
}
