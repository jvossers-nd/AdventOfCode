using Part2;

namespace Part2ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("starting...");

            var solution = new Solution(File.ReadAllLines("input.txt").Select(line => new Line(line)).ToList());

            Console.WriteLine("SOLUTION: " + solution.Solve()); // 15290096
            Console.Read();
        }
    }
}
