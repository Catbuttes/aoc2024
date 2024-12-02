using System.Collections;
using System.Text.Json;

internal class Report
{
    private List<int> levels { get; set; } = new();
    private bool damperAvailable { get; set; } = true;

    public Report(string rawReport)
    {
        var rawLevels = rawReport.Split(" ");

        foreach (var rawLvl in rawLevels)
        {
            levels.Add(int.Parse(rawLvl));
        }
    }

    private (ReportSafety safety, TrendDirection direction) EvaluateLevel(int previousIndex, int currentIndex, TrendDirection currentTrend)
    {
        TrendDirection direction = currentTrend;
        ReportSafety safety = ReportSafety.Unsafe;

        if (previousIndex == 0)
        {
            direction = TrendDirection.Unknown;
        }

        // If the readings are the same, unsafe
        if (levels[previousIndex] == levels[currentIndex])
        {
            safety = ReportSafety.Unsafe;
            // Console.WriteLine($"[{safety} SAME] - [{levels[previousIndex]}, {levels[currentIndex]}]");
            return (safety, direction);
        }

        // If the diff is more than 3, unsafe
        var diff = Math.Abs(levels[previousIndex] - levels[currentIndex]);
        if (diff > 3)
        {
            safety = ReportSafety.Unsafe;
            // Console.WriteLine($"[{safety} DELTA] - [{levels[previousIndex]}, {levels[currentIndex]}]");

            return (safety, direction);
        }

        switch (direction)
        {
            case TrendDirection.Unknown:
                if (levels[previousIndex] < levels[currentIndex])
                {
                    direction = TrendDirection.Increasing;
                    safety = ReportSafety.Safe;
                }

                if (levels[previousIndex] > levels[currentIndex])
                {
                    direction = TrendDirection.Decreasing;
                    safety = ReportSafety.Safe;

                }

                break;

            case TrendDirection.Decreasing:
                if (levels[previousIndex] > levels[currentIndex])
                {
                    safety = ReportSafety.Safe;
                }
                break;
            case TrendDirection.Increasing:
                if (levels[previousIndex] < levels[currentIndex])
                {
                    safety = ReportSafety.Safe;
                }
                break;
        }

        // Console.WriteLine($"[{safety} GEN] - [{levels[previousIndex]}, {levels[currentIndex]}]");

        return (safety, direction);
    }

    public ReportSafety DampedIsSafe(int skipIndex = -1)
    {
        TrendDirection direction = TrendDirection.Unknown;
        ReportSafety safety = ReportSafety.Unsafe;
        var current = 1;
        var previous = 0;

        if (skipIndex == 0)
        {
            current = 2;
            previous = 1;
        }

        while (current < levels.Count)
        {
            if (current == skipIndex)
            {
                current += 1;
            }
            if (current == levels.Count)
            {
                return safety;
            }

            var evaluation = EvaluateLevel(previous, current, direction);

            if (skipIndex == -1 && evaluation.safety == ReportSafety.Unsafe)
            {
                for (int i = 0; i < levels.Count; i++)
                {
                    var reeval = DampedIsSafe(i);
                    if (reeval == ReportSafety.Safe)
                    {
                        return reeval;
                    }
                }

            }

            safety = evaluation.safety;
            direction = evaluation.direction;

            previous = current;
            current += 1;

            if (safety == ReportSafety.Unsafe)
            {
                Console.WriteLine($"[{safety}] {JsonSerializer.Serialize(levels)}");

                return safety;
            }
        }

        Console.WriteLine($"[{safety}] {JsonSerializer.Serialize(levels)}");

        return safety;
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