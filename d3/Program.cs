using System.Text.RegularExpressions;

string[] lines = File.ReadAllLines("input.txt");
string raw = string.Join('\n', lines);

var mulFinder = "mul\\(([0-9]+),([0-9]+)\\)";

var muls = Regex.Matches(raw, mulFinder, RegexOptions.Multiline);

int acc = 0;

foreach (Match? mul in muls)
{
    int left = int.Parse(mul!.Groups[1].Value);
    int right = int.Parse(mul!.Groups[2].Value);
    Console.WriteLine($"{mul} - {mul!.Groups[1]} * {mul!.Groups[2]} = {left * right}");

    acc += (left * right);
}

Console.WriteLine($"Sum: {acc}");