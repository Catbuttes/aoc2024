using System.Collections;

internal class Report
{
    private List<int> levels { get; set; } = new();

    public Report(string rawReport)
    {
        var rawLevels = rawReport.Split(" ");

        foreach (var rawLvl in rawLevels)
        {
            levels.Add(int.Parse(rawLvl));
        }
    }

    public ReportSafety IsSafe()
    {
        int? previous = null;
        TrendDirection direction = TrendDirection.Unknown;


        foreach (var lvl in levels)
        {

            if (previous == null)
            {
                previous = lvl;
                continue;
            }

            // No change from previous - always unsafe.
            if (previous == lvl)
            {
                return ReportSafety.Unsafe;
            }

            // If the diff is more than 3, unsafe
            var diff = Math.Abs((previous ?? 0) - lvl);
            if (diff > 3)
            {

                return ReportSafety.Unsafe;
            }

            switch (direction)
            {
                case TrendDirection.Increasing:
                    // Trend is increasing, but this is a decrease. Unsafe
                    if (previous > lvl)
                    {

                        return ReportSafety.Unsafe;
                    }

                    break;
                case TrendDirection.Decreasing:
                    // Trend is decreasing but this is an increase. Unsafe
                    if (previous < lvl)
                    {

                        return ReportSafety.Unsafe;
                    }

                    break;
                default:
                    // No trend established yet.
                    if (previous > lvl)
                    {
                        direction = TrendDirection.Decreasing;
                    }
                    else
                    {
                        direction = TrendDirection.Increasing;
                    }
                    break;
            }

            previous = lvl;
        }

        // If we have made it all the way to here without tripping an unsafe condition
        // Then is it safe
        return ReportSafety.Safe;
    }
}

enum ReportSafety
{
    Safe,
    Unsafe
}

enum TrendDirection
{
    Increasing,
    Decreasing,
    Static,
    Unknown
}