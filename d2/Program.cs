// See https://aka.ms/new-console-template for more information

List<Report> reports = new();

List<string> raw = File.ReadLines("input.txt").ToList<string>();

foreach (var line in raw)
{
    reports.Add(new(line));
}

var safe = reports.Where(r => r.IsSafe() == ReportSafety.Safe).Count();
var dampedSafe = reports.Where(r => r.DampedIsSafe() == ReportSafety.Safe).Count();
var dampedUnSafe = reports.Where(r => r.DampedIsSafe() == ReportSafety.Unsafe).Count();


Console.WriteLine($"Reports total: {reports.Count}");
Console.WriteLine($"Part 1: Reports are safe: {safe}");
Console.WriteLine($"Part 2: Damped Reports are safe: {dampedSafe}");
Console.WriteLine($"Part 2: Damped Reports are unsafe: {dampedUnSafe}");
