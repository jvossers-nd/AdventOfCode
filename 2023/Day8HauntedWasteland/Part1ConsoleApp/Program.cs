using Part1;

namespace Part1ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var solution = new Solution(File.ReadAllLines("input.txt").Select(line => new Line(line)).ToList());

            Console.WriteLine(solution.Solve().ToString()); 
            Console.Read();

            // 141,471,539,980 is too high
        }
    }
}
