using FluentAssertions;
using Xunit;

namespace Day8TreetopTreeHouse
{
    public class Tests
    {
        [Fact]
        public void ShouldReadLines()
        {
            File.ReadAllLines("input.txt").Should().NotBeEmpty();
        }

        [Fact]
        public void ShouldReadTestLines()
        {
            File.ReadAllLines("input.test.txt").Should().NotBeEmpty();
        }

        [Fact]
        public void SolutionPart1()
        {
            var lines = File.ReadAllLines("input.txt");

            var sut = new ForestBuilder();

            var forest = sut.FromGrid(lines);

            var trees = forest.Trees.Where(t => t.IsVisible).ToList();

            Console.WriteLine(trees.Count);
        }

        [Fact]
        public void SolutionPart2()
        {
            var lines = File.ReadAllLines("input.txt");

            var sut = new ForestBuilder();

            var forest = sut.FromGrid(lines);

            var maxScore = forest.Trees.Max(t => t.ScenicScore);

            // You guessed 807576. too high
            Console.WriteLine(maxScore);
        }

        [Fact]
        public void ShouldParseTrees()
        {
            var lines = File.ReadAllLines("input.test.txt");

            var sut = new ForestBuilder();

            var forest = sut.FromGrid(lines);

            forest.Width.Should().Be(5);
            forest.Height.Should().Be(5);

            forest.Trees.Should().BeEquivalentTo(new List<object>{
                new { Height = 3, X = 0, Y = 0, IsVisible = true },
                new { Height = 0, X = 1, Y = 0, IsVisible = true },
                new { Height = 3, X = 2, Y = 0, IsVisible = true },
                new { Height = 7, X = 3, Y = 0, IsVisible = true },
                new { Height = 3, X = 4, Y = 0, IsVisible = true },
                //The top-left 5 is visible from the left and top. (It isn't visible from the right or bottom since other trees of height 5 are in the way.)
                //The top-middle 5 is visible from the top and right.
                //The top-right 1 is not visible from any direction; for it to be visible, there would need to only be trees of height 0 between it and an edge.
                new { Height = 2, X = 0, Y = 1, IsVisible = true },
                new { Height = 5, X = 1, Y = 1, IsVisible = true },  //The top-left 5
                new { Height = 5, X = 2, Y = 1, IsVisible = true },  //The top-middle 5
                new { Height = 1, X = 3, Y = 1, IsVisible = false },  //The top-right 1
                new { Height = 2, X = 4, Y = 1, IsVisible = true },
                //The left-middle 5 is visible, but only from the right.
                //The center 3 is not visible from any direction; for it to be visible, there would need to be only trees of at most height 2 between it and an edge.
                //The right-middle 3 is visible from the right.
                new { Height = 6, X = 0, Y = 2, IsVisible = true },
                new { Height = 5, X = 1, Y = 2, IsVisible = true }, //The left-middle 5
                new { Height = 3, X = 2, Y = 2, IsVisible = false }, //The center 3 
                new { Height = 3, X = 3, Y = 2, IsVisible = true }, //The right-middle 3
                new { Height = 2, X = 4, Y = 2, IsVisible = true },
                //In the bottom row, the middle 5 is visible, but the 3 and 4 are not.
                new { Height = 3, X = 0, Y = 3, IsVisible = true },
                new { Height = 3, X = 1, Y = 3, IsVisible = false }, // the 3
                new { Height = 5, X = 2, Y = 3, IsVisible = true },  // the middle 5
                new { Height = 4, X = 3, Y = 3, IsVisible = false }, // the 4
                new { Height = 9, X = 4, Y = 3, IsVisible = true },
                //
                new { Height = 3, X = 0, Y = 4, IsVisible = true },
                new { Height = 5, X = 1, Y = 4, IsVisible = true },
                new { Height = 3, X = 2, Y = 4, IsVisible = true },
                new { Height = 9, X = 3, Y = 4, IsVisible = true },
                new { Height = 0, X = 4, Y = 4, IsVisible = true }
            }); 

            // , option => option.Excluding(x => x.Path.EndsWith("Forest"))
        }
    }

    public class ForestBuilder
    {
        public Forest FromGrid(string[] lines)
        {
            var forest = new Forest();
            forest.Height = lines.Length;
            forest.Width = lines.SelectMany(line => line.ToCharArray()).Count() / forest.Height;

            int y = 0;

            foreach (var line in lines)
            {
                int x = 0;    

                foreach (var c in line)
                {
                    forest.Trees.Add(new Tree(int.Parse(c.ToString()), x, y, forest));

                    x++;
                }

                y++;
            }

            return forest;
        }
    }

    public class Forest
    {
        public List<Tree> Trees { get;  }
        public int Width { get; set; }
        public int Height { get; set; }

        public Forest()
        {
            Trees = new List<Tree>();
        }
    }

    public class Tree
    {
        public int Height { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public Forest Forest { get; }

        public Tree(int height, int x, int y, Forest forest)
        {
            Height = height;
            Forest = forest;
            X = x;
            Y = y;
        }

        public bool IsVisible => CalculateVisibility();

        public int ScenicScore => ScenicScoreTop * ScenicScoreRight * ScenicScoreBottom * ScenicScoreLeft;
        public int ScenicScoreTop => CalculateScenicScore(Orientation.Top);
        public int ScenicScoreRight =>  CalculateScenicScore(Orientation.Right);
        public int ScenicScoreBottom =>  CalculateScenicScore(Orientation.Bottom);
        public int ScenicScoreLeft =>  CalculateScenicScore(Orientation.Left);

        private int CalculateScenicScore(Orientation orientation)
        {
            var candidates = new List<Tree>();
            int score = 0;

            switch (orientation)
            {
                case Orientation.Top:
                    candidates = Forest.Trees.Where(t => t.X == X && t.Y < Y).OrderByDescending(t => t.Y).ToList();
                    break;
                case Orientation.Right:
                    candidates = Forest.Trees.Where(t => t.X > X && t.Y == Y).OrderBy(t => t.X).ToList();
                    break;
                case Orientation.Bottom:
                    candidates = Forest.Trees.Where(t => t.X == X && t.Y > Y).OrderBy(t => t.Y).ToList();
                    break;
                case Orientation.Left:
                    candidates = Forest.Trees.Where(t => t.X < X && t.Y == Y).OrderByDescending(t => t.X).ToList();
                    break;
                default:
                    break;
            }

            foreach (var tree in candidates)
            {
                score++;

                if (tree.Height >= Height)
                {
                    break;
                }
            }

            return score;
        }

        private bool CalculateVisibility()
        {
            bool onEdgeTop = OnEdge(Orientation.Top);
            bool onEdgeRight = OnEdge(Orientation.Right);
            bool onEdgeBottom = OnEdge(Orientation.Bottom);
            bool onEdgeLeft = OnEdge(Orientation.Left);

            bool allTreesSmallerTop = AllTreesSmaller(Orientation.Top);
            bool allTreesSmallerRight = AllTreesSmaller(Orientation.Right);
            bool allTreesSmallerBottom = AllTreesSmaller(Orientation.Bottom);
            bool allTreesSmallerLeft = AllTreesSmaller(Orientation.Left);

            bool visibleFromTop = onEdgeTop || allTreesSmallerTop;
            bool visibleFromRight = onEdgeRight || allTreesSmallerRight;
            bool visibleFromBottom = onEdgeBottom || allTreesSmallerBottom;
            bool visibleFromLeft = onEdgeLeft || allTreesSmallerLeft;

            return visibleFromTop | visibleFromRight | visibleFromBottom | visibleFromLeft;
        }

        private bool OnEdge(Orientation orientation)
        {
            switch (orientation)
            {
                case Orientation.Top:
                    return Y == 0;
                case Orientation.Right:
                    return X == Forest.Width-1;
                case Orientation.Bottom:
                    return Y == Forest.Height-1;
                case Orientation.Left:
                    return X == 0;
                default:
                    throw new Exception();
            }
        }

        private bool AllTreesSmaller(Orientation orientation)
        {
            switch (orientation)
            {
                case Orientation.Top:
                    // all trees with same X and smaller Y
                    return !Forest.Trees.Any(t => t.X == X && t.Y < Y && t.Height >= Height);
                case Orientation.Right:
                    // all trees with same Y and greater X
                    return !Forest.Trees.Any(t => t.X > X && t.Y == Y && t.Height >= Height);
                case Orientation.Bottom:
                    // all trees with same X and greater Y
                    return !Forest.Trees.Any(t => t.X == X && t.Y > Y && t.Height >= Height);
                case Orientation.Left:
                    // all trees with same Y and smaller X
                    return !Forest.Trees.Any(t => t.X < X && t.Y == Y && t.Height >= Height);
                default:
                    throw new Exception();
            }
        }
    }

    public enum Orientation
    {
        Top,
        Right,
        Bottom,
        Left
    }
}
