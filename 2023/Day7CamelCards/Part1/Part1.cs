using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Part1
{
    public enum HandClassification
    {
        FiveOfAKind = 100,
        FourOfAKind = 90,
        FullHouse = 80,
        ThreeOfAKind = 70,
        TwoPair = 60,
        OnePair = 50,
        HighCard = 40
    }

    public class Hand
    {
        private readonly string _text;
        public int Bid { get; set; }
        public int Winnings => Rank * Bid;
        public int Rank { get; set; }
        public string CardText => _text.Trim().Split(" ")[0];

        public int Strength => (int) Classification;

        public List<Card> Cards { get; set; }

        public HandClassification Classification { get; set; }
        
        public string SecondaryStrength => String.Join("", Cards.Select(c => c.StrengthSignature));
        
        public Hand(string text)
        {
            _text = text.Trim();
            Parse();
        }

        public override string ToString()
        {
            return $"{Rank},{CardText},{Classification},{Strength},{SecondaryStrength},{Bid},{Winnings},{_text}";
        }

        private void Parse()
        {
            var parts =_text.Split(' ');

            Cards = parts[0].Select(c => new Card(c)).ToList();
            Bid = int.Parse(parts[1]);
            Classification = Classify();
        }

        private HandClassification Classify()
        {
            var groups = Cards.GroupBy(c => c.Char);

            if (IsFiveOfAKind(groups)) return HandClassification.FiveOfAKind;
            if (IsFourOfAKind(groups)) return HandClassification.FourOfAKind;
            if (IsFullHouse(groups)) return HandClassification.FullHouse;
            if (IsThreeOfAKind(groups)) return HandClassification.ThreeOfAKind;
            if (IsTwoPair(groups)) return HandClassification.TwoPair;
            if (IsOnePair(groups)) return HandClassification.OnePair;
            return HandClassification.HighCard;

        }
        
        private bool IsFiveOfAKind(IEnumerable<IGrouping<char, Card>> groups)
        {
            return groups.Any(g => g.Count() == 5);
        }

        private bool IsFourOfAKind(IEnumerable<IGrouping<char, Card>> groups)
        {
            return groups.Any(g => g.Count() == 4);
        }

        private bool IsFullHouse(IEnumerable<IGrouping<char, Card>> groups)
        {
            return
                groups.Count(g => g.Count() == 3) == 1 &&
                groups.Count(g => g.Count() == 2) == 1;
        }

        private bool IsThreeOfAKind(IEnumerable<IGrouping<char, Card>> groups)
        {
            return groups.Any(g => g.Count() == 3);
        }

        private bool IsTwoPair(IEnumerable<IGrouping<char, Card>> groups)
        {
            return groups.Count(g => g.Count() == 2) == 2;
        }

        private bool IsOnePair(IEnumerable<IGrouping<char, Card>> groups)
        {
            return groups.Any(g => g.Count() == 2);
        }
    }

    public class Card
    {
        public char Char { get; set;}
        public int Strength { get; set; }
        public char StrengthSignature{ get; set; }

        public Card(char c)
        {
            Char = c;
            Parse();
        }

        private void Parse()
        {
            Strength = Char switch
            {
                'A' => 14,
                'K' => 13,
                'Q' => 12,
                'J' => 11,
                'T' => 10,
                _ => int.Parse(Char.ToString())
            };

            StrengthSignature = Char switch
            {
                '2' => 'A',
                '3' => 'B',
                '4' => 'C',
                '5' => 'D',
                '6' => 'E',
                '7' => 'F',
                '8' => 'G',
                '9' => 'H',
                'T' => 'I',
                'J' => 'J',
                'Q' => 'K',
                'K' => 'L',
                'A' => 'M',
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public class Solution
    {
        public List<Hand> Hands { get; }

        public Solution(List<Hand> hands)
        {
            Hands = hands;
        }

        public int Solve()
        {
            var orderedHands = Hands
                .OrderBy(h => h.Strength)
                .ThenBy(h => h.SecondaryStrength).ToList();

            for (int rank = orderedHands.Count; rank > 0; rank--)
            {
                var hand = orderedHands[rank - 1];
                hand.Rank = rank;
            }
            
            var totalWinnings = orderedHands.Sum(h => h.Winnings);

            return totalWinnings;
        }
    }

    public class Tests
    {
        private readonly ITestOutputHelper _output;

        public Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData("KAJ32 100", "LMJBA")]
        [InlineData("QT98 100", "KIHG")]
        public void SignatureTest(string hand, string secondaryStrength)
        {
            //'2' => 'A',
            //'3' => 'B',
            //'4' => 'C',
            //'5' => 'D',
            //'6' => 'E',
            //'7' => 'F',
            //'8' => 'G',
            //'9' => 'H',
            //'T' => 'I',
            //'J' => 'J',
            //'Q' => 'K',
            //'K' => 'L',
            //'A' => 'M',
            new Hand(hand).SecondaryStrength.Should().Be(secondaryStrength);
        }
        
        [Theory]
        [InlineData("KKKKK 1", HandClassification.FiveOfAKind)]
        [InlineData("KKKKQ 1", HandClassification.FourOfAKind)]
        [InlineData("KKKQQ 1", HandClassification.FullHouse)]
        [InlineData("KKKQJ 1", HandClassification.ThreeOfAKind)]
        [InlineData("KKQQJ 1", HandClassification.TwoPair)]
        [InlineData("KKQJA 1", HandClassification.OnePair)]
        [InlineData("2345A 1", HandClassification.HighCard)]
        [InlineData("2345K 1", HandClassification.HighCard)]
        [InlineData("2345Q 1", HandClassification.HighCard)]
        [InlineData("2345J 1", HandClassification.HighCard)]
        [InlineData("2345T 1", HandClassification.HighCard)]
        [InlineData("23459 1", HandClassification.HighCard)]
        [InlineData("23458 1", HandClassification.HighCard)]
        [InlineData("23457 1", HandClassification.HighCard)]
        [InlineData("23456 1", HandClassification.HighCard)]
        public void TestHandType(string handText, HandClassification expectedClassification)
        {
            var solution = new Solution(new List<Hand>()
            {
                new Hand(handText)
            });

            solution.Hands[0].Strength.Should().Be((int)expectedClassification);
        }

        [Fact]
        public void Test()
        {
            var solution = new Solution(File.ReadAllLines("input.test.txt").Select(line => new Hand(line)).ToList());

            solution.Hands.Count.Should().Be(5);
            solution.Hands[0].Bid.Should().Be(765);
            solution.Hands[0].Cards[0].Char.Should().Be('3');
            solution.Hands[0].Cards[1].Char.Should().Be('2');
            solution.Hands[0].Cards[2].Char.Should().Be('T');
            solution.Hands[0].Cards[3].Char.Should().Be('3');
            solution.Hands[0].Cards[4].Char.Should().Be('K');

            solution.Hands[0].SecondaryStrength.Should().Be("BAIBL");
            solution.Hands[0].Cards[0].StrengthSignature.Should().Be('B');
            solution.Hands[0].Cards[1].StrengthSignature.Should().Be('A');
            solution.Hands[0].Cards[2].StrengthSignature.Should().Be('I');
            solution.Hands[0].Cards[3].StrengthSignature.Should().Be('B');
            solution.Hands[0].Cards[4].StrengthSignature.Should().Be('L');

            solution.Hands[0].Strength.Should().Be((int)HandClassification.OnePair); // 32T3K IsOnePair
            solution.Hands[1].Strength.Should().Be((int)HandClassification.ThreeOfAKind); // T55J5 IsThreeOfAKind
            solution.Hands[2].Strength.Should().Be((int)HandClassification.TwoPair); // KK677 IsTwoPair
            solution.Hands[3].Strength.Should().Be((int)HandClassification.TwoPair); // KTJJT IsTwoPair
            solution.Hands[4].Strength.Should().Be((int)HandClassification.ThreeOfAKind); // QQQJA IsThreeOfAKind
            
            var answer = solution.Solve();

            solution.Hands[0].Rank.Should().Be(1); // 32T3K IsOnePair
            solution.Hands[1].Rank.Should().Be(4); // T55J5 IsThreeOfAKind
            solution.Hands[2].Rank.Should().Be(3); // KK677 IsTwoPair
            solution.Hands[3].Rank.Should().Be(2); // KTJJT IsTwoPair
            solution.Hands[4].Rank.Should().Be(5); // QQQJA IsThreeOfAKind

            answer.Should().Be(6440);
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

            solution.Hands.Count.Should().Be(1000);
            solution.Hands.TrueForAll(h => h.Winnings == h.Rank * h.Bid);

            _output.WriteLine(answer.ToString()); 
            // 250225871 too low
            // 250232501 correct
        }
    }
}