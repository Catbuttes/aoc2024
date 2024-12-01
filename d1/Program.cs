internal class Program
{
    private static void Main(string[] args)
    {
        Part1();
        Part2();
    }

    private static void Part1()
    {
        List<int> l1 = new();
        List<int> l2 = new();

        List<string> raw = File.ReadLines("input.txt").ToList<string>();

        foreach (var line in raw)
        {
            var split = line.Split("   ");
            l1.Add(int.Parse(split[0]));
            l2.Add(int.Parse(split[1]));
        }

        l1.Sort();
        l2.Sort();

        int accumulator = 0;

        for (int i = 0; i < l1.Count; i++)
        {
            accumulator += Math.Abs(l1[i] - l2[i]);
        }

        Console.WriteLine($"Part 1 Total Distance is: {accumulator}");
    }

    private static void Part2()
    {
        List<int> left = new();
        List<int> right = new();

        List<string> raw = File.ReadLines("input.txt").ToList<string>();

        foreach (var line in raw)
        {
            var split = line.Split("   ");
            left.Add(int.Parse(split[0]));
            right.Add(int.Parse(split[1]));
        }

        left.Sort();
        right.Sort();

        int accumulator = 0;

        for (int i = 0; i < left.Count; i++)
        {
            int rightCount = left[i] * (right.Where(value => value == left[i]).Count());
            accumulator += rightCount;
        }

        Console.WriteLine($"Part 2 Similarity is: {accumulator}");
    }
}