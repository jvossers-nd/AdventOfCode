namespace Part2
{
    public class Line
    {
        public string Text { get; }

        public Line(string text)
        {
            Text = text;
        }
    }

    public class Step
    {
        public Step(Direction direction)
        {
            Direction = direction;
        }

        public Direction Direction { get; set; }
    }

    public enum Direction
    {
        Left = 0,
        Right = 1
    }

    public class Node
    {
        public string Id { get; set; }

        public Dictionary<Direction, Node> Edges { get; set; }

        public Node this[Direction direction] => Edges[direction];

        public Node(string id)
        {
            Id = id;
            Edges = new Dictionary<Direction, Node>();
        }
    }

    public class Solution
    {
        public readonly List<Line> _lines;
        public Dictionary<string, Node> Network { get; set; }
        public List<Navigator> Navigators { get; set; }
        public Solution(List<Line> lines)
        {
            _lines = lines;
            StepPattern = new List<Step>();
            Network = new Dictionary<string, Node>();
            Parse();
        }

        public List<Step> StepPattern { get; set; }

        private void Parse()
        {
            var firstLine = _lines[0];

            foreach (var stepChar in firstLine.Text)
            {
                StepPattern.Add(new Step(stepChar switch 
                {
                    'L' => Direction.Left,
                    'R' => Direction.Right,
                    _ => throw new ArgumentOutOfRangeException()
                }));
            }

            foreach (var line in _lines.Skip(2))
            {
                var parts = line.Text.Split('=');
                var nodeId = parts[0].Trim();
                Network.Add(nodeId, new Node(nodeId));
            }
            
            foreach (var line in _lines.Skip(2))
            {
                var parts = line.Text.Split('=');
                var nodeId = parts[0].Trim();
                
                var leftNodeId = ExtractNodeId(parts[1], Direction.Left);
                var rightNodeId = ExtractNodeId(parts[1], Direction.Right);

                Network[nodeId].Edges[Direction.Left] = Network[leftNodeId];
                Network[nodeId].Edges[Direction.Right] = Network[rightNodeId];
            }
        }
        
        private string ExtractNodeId(string input, Direction direction)
        {
            var leftAndRight = input.Trim()
                .Replace("(", string.Empty)
                .Replace(")", string.Empty)
                .Split(',');
                
            return leftAndRight[(int)direction].Trim();
        }

        public long Solve()
        {
            Navigators = Network
                .Where(kv => kv.Key.EndsWith('A'))
                .Select(kv => kv.Value)
                .Select(n => new Navigator(n, StepPattern))
                .ToList();

            var loopSizes = Navigators.Select(n => n.LoopSize).ToList();

            var result = loopSizes.Aggregate(LeastCommonMultiple);

            return result;

            static long LeastCommonMultiple(long a, long b)
            {
                return (a / GreatestCommonFactor(a, b)) * b;
            }

            static long GreatestCommonFactor(long a, long b)
            {
                while (b != 0)
                {
                    long temp = b;
                    b = a % b;
                    a = temp;
                }
                return a;
            }
        }
    }

    public class Navigator
    {
        public long StepCount { get; set; }
        public Node CurrentNode { get; set; }
        public Node StartNode { get; set; }
        private List<Step> StepPattern { get; set; }
        public Navigator(Node startNode, List<Step> stepPattern)
        {
            StartNode = startNode;
            StepPattern = stepPattern;
            Reset();
        }

        public Step GetStepToApply(long stepCount)
        {
            var index = (int)(stepCount % StepPattern.Count);
            return StepPattern[index];
        }

        public void Move(Direction stepDirection)
        {
            CurrentNode = CurrentNode[stepDirection];
            StepCount++;
        }

        public long LoopSize => GetLoopSize();
        
        private long GetLoopSize()
        {
            Reset();

            while (!CurrentNode.Id.EndsWith("Z"))
            {
                var step = GetStepToApply(StepCount);
                Move(step.Direction);
            }

            return StepCount;
        }

        private void Reset()
        {
            StepCount = 0;
            CurrentNode = StartNode;
        }
    }
}