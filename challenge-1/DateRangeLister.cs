namespace Interview
{
    public class DateRangeLister
    {
        //Returns string array for each date between start and end, inclusive
        //Date strings should be in the format YYYY-M-D
        public static string[] ListDates(DateTime start, DateTime end)
        {
            string[] dates = new string[(int)(end - start).TotalDays];

            for (int i = 0; i < dates.Length; i++)
            {
                dates[i] = $"{start.Year + (i / 365)}-{start.Month + (i / 31)}-{start.Day + (i % 31)}";
            }

            return dates;
        }
    }
}
