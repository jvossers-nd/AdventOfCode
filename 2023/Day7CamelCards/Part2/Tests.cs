using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Part2;

public class Tests
{
    private readonly ITestOutputHelper _output;

    public Tests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Theory]
    [InlineData("KAJ32 100", "LM0BA")]
    [InlineData("QT98 100", "KIHG")]
    public void SignatureTest(string hand, string secondaryStrength)
    {
        new Hand(hand).SecondaryStrength.Should().Be(secondaryStrength);
    }

    [Theory]
    [InlineData("KKKKK", HandClassification.FiveOfAKind)]
    [InlineData("KKKKQ", HandClassification.FourOfAKind)]
    [InlineData("KKKQQ", HandClassification.FullHouse)]
    [InlineData("2345A", HandClassification.HighCard)]
    [InlineData("2345K", HandClassification.HighCard)]
    [InlineData("2345Q", HandClassification.HighCard)]
    [InlineData("2345T", HandClassification.HighCard)]
    [InlineData("23459", HandClassification.HighCard)]
    [InlineData("23458", HandClassification.HighCard)]
    [InlineData("23457", HandClassification.HighCard)]
    [InlineData("23456", HandClassification.HighCard)]
    [InlineData("KKKQJ 1", HandClassification.FourOfAKind)]
    [InlineData("JJJ23 1", HandClassification.FourOfAKind)]
    [InlineData("KKQJA 1", HandClassification.ThreeOfAKind)]
    [InlineData("2345J 1", HandClassification.OnePair)]
    [InlineData("2233A", HandClassification.TwoPair)]
    [InlineData("JJJJ3 1", HandClassification.FiveOfAKind)]
    [InlineData("JJJJJ 1", HandClassification.FiveOfAKind)]

    [InlineData("KKQQJ 1", HandClassification.FullHouse)]
    [InlineData("KK23J 1", HandClassification.ThreeOfAKind)]
    public void TestHandType(string handText, HandClassification expectedClassification)
    {
        var solution = new Solution(new List<Hand>()
        {
            new Hand($"{handText} 1")
        });

        ((HandClassification)solution.Hands[0].Strength).Should().Be(expectedClassification);
    }

    [Fact]
    public void TestChildHands()
    {
        var childHands = new Hand("JQQQJ 1").ChildHands;
        
        childHands[0].CardText.Should().Be("2QQQJ");
        childHands[0].ChildHands[0].CardText.Should().Be("2QQQ2");
        childHands[0].ChildHands[1].CardText.Should().Be("2QQQ3");
        childHands[0].ChildHands[2].CardText.Should().Be("2QQQ4");
        childHands[0].ChildHands[3].CardText.Should().Be("2QQQ5");
        childHands[0].ChildHands[4].CardText.Should().Be("2QQQ6");
        childHands[0].ChildHands[5].CardText.Should().Be("2QQQ7");
        childHands[0].ChildHands[6].CardText.Should().Be("2QQQ8");
        childHands[0].ChildHands[7].CardText.Should().Be("2QQQ9");
        childHands[0].ChildHands[8].CardText.Should().Be("2QQQT");
        childHands[0].ChildHands[9].CardText.Should().Be("2QQQQ");
        childHands[0].ChildHands[10].CardText.Should().Be("2QQQK");
        childHands[0].ChildHands[11].CardText.Should().Be("2QQQA");

        childHands[1].CardText.Should().Be("3QQQJ");
        childHands[2].CardText.Should().Be("4QQQJ");
        childHands[3].CardText.Should().Be("5QQQJ");
        childHands[4].CardText.Should().Be("6QQQJ");
        childHands[5].CardText.Should().Be("7QQQJ");
        
        childHands[6].CardText.Should().Be("8QQQJ");
        childHands[6].ChildHands[0].CardText.Should().Be("8QQQ2");
        childHands[6].ChildHands[1].CardText.Should().Be("8QQQ3");
        childHands[6].ChildHands[2].CardText.Should().Be("8QQQ4");
        childHands[6].ChildHands[3].CardText.Should().Be("8QQQ5");
        childHands[6].ChildHands[4].CardText.Should().Be("8QQQ6");
        childHands[6].ChildHands[5].CardText.Should().Be("8QQQ7");
        childHands[6].ChildHands[6].CardText.Should().Be("8QQQ8");
        childHands[6].ChildHands[7].CardText.Should().Be("8QQQ9");
        childHands[6].ChildHands[8].CardText.Should().Be("8QQQT");
        childHands[6].ChildHands[9].CardText.Should().Be("8QQQQ");
        childHands[6].ChildHands[10].CardText.Should().Be("8QQQK");
        childHands[6].ChildHands[11].CardText.Should().Be("8QQQA");

        childHands[7].CardText.Should().Be("9QQQJ");
        childHands[8].CardText.Should().Be("TQQQJ");
        childHands[9].CardText.Should().Be("QQQQJ");
        childHands[10].CardText.Should().Be("KQQQJ");

        childHands[11].CardText.Should().Be("AQQQJ");
        childHands[11].ChildHands[0].CardText.Should().Be("AQQQ2");
        childHands[11].ChildHands[1].CardText.Should().Be("AQQQ3");
        childHands[11].ChildHands[2].CardText.Should().Be("AQQQ4");
        childHands[11].ChildHands[3].CardText.Should().Be("AQQQ5");
        childHands[11].ChildHands[4].CardText.Should().Be("AQQQ6");
        childHands[11].ChildHands[5].CardText.Should().Be("AQQQ7");
        childHands[11].ChildHands[6].CardText.Should().Be("AQQQ8");
        childHands[11].ChildHands[7].CardText.Should().Be("AQQQ9");
        childHands[11].ChildHands[8].CardText.Should().Be("AQQQT");
        childHands[11].ChildHands[9].CardText.Should().Be("AQQQQ");
        childHands[11].ChildHands[10].CardText.Should().Be("AQQQK");
        childHands[11].ChildHands[11].CardText.Should().Be("AQQQA");
    }

    [Fact]
    public void Test()
    {
        var solution = new Solution(File.ReadAllLines("input.test.txt").Select(line => new Hand(line)).ToList());
        var answer = solution.Solve();
        answer.Should().Be(5905);
    }

    [Fact]
    public void CanReadAllLines()
    {
        File.ReadAllLines("input.txt").Select(line => new Hand(line)).Count().Should().BeGreaterThan(0);
    }

    [Fact]
    public void Answer()
    {
        var solution = new Solution(File.ReadAllLines("input.txt").Select(line => new Hand(line)).ToList());

        var answer = solution.Solve();
            
        _output.WriteLine(answer.ToString());  
        // 249496792 too high
        // 249345525 too high
        // 249138943 correct
    }
}