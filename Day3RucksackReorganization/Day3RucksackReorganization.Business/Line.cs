namespace Day3RucksackReorganization.Business
{
    public class Line
    {

        public string Left { get; set; }
        public string Right { get; set; }
        public string InputString { get; set; }
        public char DuplicateChar => Left.First(c => Right.Contains(c, StringComparison.InvariantCulture));
        public int Score => DuplicateChar.GetScore();

        public Line(string inputString)
        {
            InputString = inputString;

            int halfLength = inputString.Length / 2;

            Left = inputString.Substring(0, halfLength);
            Right = inputString.Substring(halfLength, halfLength);
        }
    }
}