// See https://aka.ms/new-console-template for more information

List<Report> reports = new();

List<string> raw = File.ReadLines("input.txt").ToList<string>();

foreach (var line in raw)
{
    reports.Add(new(line));
}

var safe = reports.Where(r => r.IsSafe() == ReportSafety.Safe).Count();

Console.WriteLine($"Part 1: Reports are safe: {safe}");
