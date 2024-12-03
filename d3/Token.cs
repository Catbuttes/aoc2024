using System.Text.RegularExpressions;

class Token
{
    public int Index { get; set; } = 0;
    public Operation Type { get; set; } = Operation.Noop;
    public List<int> Operands { get; set; } = new();

    public static Token FromMatch(Match regexMatch)
    {
        if (regexMatch.Groups[0].Value == "do()")
        {
            return new()
            {
                Index = regexMatch.Index,
                Type = Operation.Do
            };
        }

        if (regexMatch.Groups[0].Value == "don't()")
        {
            return new()
            {
                Index = regexMatch.Index,
                Type = Operation.Dont
            };
        }

        if (regexMatch.Groups[0].Value.StartsWith("mul("))
        {
            return new()
            {
                Index = regexMatch.Index,
                Type = Operation.Mul,
                Operands = [
                    int.Parse(regexMatch.Groups[1].Value),
                    int.Parse(regexMatch.Groups[2].Value)
                ]
            };
        }


        return new();
    }
}

enum Operation
{
    Noop,
    Do,
    Dont,
    Mul
}