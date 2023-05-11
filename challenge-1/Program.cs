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
