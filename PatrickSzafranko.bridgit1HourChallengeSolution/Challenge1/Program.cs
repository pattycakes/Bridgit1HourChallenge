using Interview;

/*
CHALLENGE 1
You need to make DateRangeLister.ListDates "production ready". This means it should perform well, be well written, robust, and provide accurate results.
The function should provide valid output for any valid DateTime start/end vars passed in.

You do NOT need to create any tests for this challenge.
*/

var start = new DateTime(2022, 01, 01);
var end = new DateTime(2022, 12, 31);

var dates = DateRangeLister.ListDates(start, end);
Console.WriteLine(string.Join(Environment.NewLine, dates));


namespace Interview
{
    public class DateRangeLister
    {
        //Returns string array for each date between start and end, inclusive
        //Date strings should be in the format YYYY-M-D
        public static string[] ListDates(DateTime start, DateTime end)
        {
            //string[] dates = new string[(int)(end - start).TotalDays];

            //for (int i = 0; i < dates.Length; i++)
            //{
            //    dates[i] = $"{start.Year + (i / 365)}-{start.Month + (i / 31)}-{start.Day + (i % 31)}";
            //}
            //return dates;

            List<string> output = new List<string>();

            if (end < start)
            {
                output.Add("Error with input data : start date value is after end date value.");
            }
            else
            {
                for (var dt = start; dt <= end; dt = dt.AddDays(1))
                {
                    output.Add(dt.ToString("yyyy-M-d"));
                }
            }

            return output.ToArray();
        }
    }
}