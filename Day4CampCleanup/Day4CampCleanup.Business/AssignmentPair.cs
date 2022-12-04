namespace Day4CampCleanup.Business;

public class AssignmentPair
{
    public string TextInput { get; }
        

    public bool FullyContained
    {
        get
        {
            var leftLowerBoundery = int.Parse(TextInput.Split(',')[0].Split('-')[0]);
            var leftUpperBoundery = int.Parse(TextInput.Split(',')[0].Split('-')[1]);
            var rightLowerBoundery = int.Parse(TextInput.Split(',')[1].Split('-')[0]);
            var rightUpperBoundery = int.Parse(TextInput.Split(',')[1].Split('-')[1]);

            var leftRange = Enumerable.Range(leftLowerBoundery, leftUpperBoundery - leftLowerBoundery + 1).ToList();
            var rightRange = Enumerable.Range(rightLowerBoundery, rightUpperBoundery - rightLowerBoundery + 1).ToList();

            // intersect and compare count of result with count of left and right. if one match then true
            var intersection = leftRange.Intersect(rightRange).ToList();
            var fullyContained = intersection.Count == leftRange.Count() || intersection.Count == rightRange.Count();

            return fullyContained;
        }
    }

    public bool AnyOverlap
    {
        get
        {
            var leftLowerBoundery = int.Parse(TextInput.Split(',')[0].Split('-')[0]);
            var leftUpperBoundery = int.Parse(TextInput.Split(',')[0].Split('-')[1]);
            var rightLowerBoundery = int.Parse(TextInput.Split(',')[1].Split('-')[0]);
            var rightUpperBoundery = int.Parse(TextInput.Split(',')[1].Split('-')[1]);

            var leftRange = Enumerable.Range(leftLowerBoundery, leftUpperBoundery - leftLowerBoundery + 1).ToList();
            var rightRange = Enumerable.Range(rightLowerBoundery, rightUpperBoundery - rightLowerBoundery + 1).ToList();

            var intersection = leftRange.Intersect(rightRange).ToList();
            var anyOverlap = intersection.Any();

            return anyOverlap;
        }
    }

    public AssignmentPair(string textInput)
    {
        TextInput = textInput;
    }
}