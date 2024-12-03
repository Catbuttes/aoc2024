using System.Text.RegularExpressions;
using System.Linq;

internal class Program
{
    private static void Main(string[] args)
    {
        Part1();
        Part2();
    }
    private static void Part1()
    {
        string[] lines = File.ReadAllLines("input.txt");
        string raw = string.Join('\n', lines);

        var mulFinder = "mul\\(([0-9]+),([0-9]+)\\)";

        var muls = Regex.Matches(raw, mulFinder, RegexOptions.Multiline);

        int acc = 0;

        foreach (Match? mul in muls)
        {
            int left = int.Parse(mul!.Groups[1].Value);
            int right = int.Parse(mul!.Groups[2].Value);
            //Console.WriteLine($"{mul} - {mul!.Groups[1]} * {mul!.Groups[2]} = {left * right}");

            acc += left * right;
        }

        Console.WriteLine($"Part 1: Sum: {acc}");
    }

    private static void Part2()
    {
        string[] lines = File.ReadAllLines("input.txt");
        string raw = string.Join('\n', lines);

        var tokens = Tokenize(raw);
        var result = Execute(tokens);

        Console.WriteLine($"Part 2: Sum: {result}");


    }

    private static List<Token> Tokenize(string input)
    {
        var muls = Regex.Matches(
            input,
            "mul\\(([0-9]+),([0-9]+)\\)",
            RegexOptions.Multiline
        ).ToList();

        var enables = Regex.Matches(
            input,
            "do\\(\\)",
            RegexOptions.Multiline
        ).ToList();

        var disables = Regex.Matches(
            input,
            "don't\\(\\)",
            RegexOptions.Multiline
        ).ToList();

        List<Match> unprocessedHeap = [
            .. muls,
            .. enables,
            .. disables
        ];

        var sortedHeap = unprocessedHeap.OrderBy(item => item.Index);

        List<Token> executionPlan = new();

        foreach (var t in sortedHeap)
        {
            executionPlan.Add(Token.FromMatch(t));
        }

        return executionPlan;
    }

    private static int Execute(List<Token> executionPlan)
    {
        int accumulator = 0;
        bool processing = true; //Implicit do() at the start
        foreach (var step in executionPlan)
        {
            switch (step.Type)
            {
                case Operation.Do:
                    processing = true;
                    break;
                case Operation.Dont:
                    processing = false;
                    break;
                case Operation.Mul:
                    if (processing)
                    {
                        accumulator += step.Operands[0] * step.Operands[1];
                    }
                    break;
                default:
                    break;
            }

        }

        return accumulator;
    }
}