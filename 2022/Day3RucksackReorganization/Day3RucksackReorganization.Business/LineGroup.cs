namespace Day3RucksackReorganization.Business;

public class LineGroup
{
    public char BadgeChar { get; set; }
    public int BadgeScore => BadgeChar.GetScore();
    public Line Line1 { get; set; }
    public Line Line2 { get; set; }
    public Line Line3 { get; set; }

    public LineGroup(Line line1, Line line2, Line line3)
    {
        Line1 = line1;
        Line2 = line2;
        Line3 = line3;

        BadgeChar = line1.InputString.Intersect(line2.InputString.Intersect(line3.InputString))
            .Single();
    }
}