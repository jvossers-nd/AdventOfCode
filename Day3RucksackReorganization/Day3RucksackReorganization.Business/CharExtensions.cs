namespace Day3RucksackReorganization.Business;

public static class CharExtensions
{
    public static int GetScore(this char c)
    {
        if (c > 96)
        {
            return c - 96;
        }

        return c - 64 + 26;
    }
}