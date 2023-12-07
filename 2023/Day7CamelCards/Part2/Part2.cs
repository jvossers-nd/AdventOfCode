using System.Text.RegularExpressions;

namespace Part2
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
        private static Regex _regex = new Regex(Regex.Escape("J"));
        private readonly string _text;
        public int Bid { get; set; }
        public int Winnings => Rank * Bid;
        public int Rank { get; set; }
        public string CardText { get; set; }
        public int Strength => (int) Classification;
        public List<Card> Cards { get; set; }
        public HandClassification Classification { get; set; }
        public string SecondaryStrength => String.Join("", Cards.Select(c => c.StrengthSignature));
        public bool HasJoker => Cards.Any(c => c.IsJoker);

        public Hand(string text)
        {
            _text = text.Trim();
            Parse();
        }

        public List<Hand> ChildHands
        {
            get
            {
                List<Hand> hands = new List<Hand>();

                if (HasJoker)
                {
                    hands.Add(SpawnChildHand('2'));
                    hands.Add(SpawnChildHand('3'));
                    hands.Add(SpawnChildHand('4'));
                    hands.Add(SpawnChildHand('5'));
                    hands.Add(SpawnChildHand('6'));
                    hands.Add(SpawnChildHand('7'));
                    hands.Add(SpawnChildHand('8'));
                    hands.Add(SpawnChildHand('9'));
                    hands.Add(SpawnChildHand('T'));
                    hands.Add(SpawnChildHand('Q'));
                    hands.Add(SpawnChildHand('K'));
                    hands.Add(SpawnChildHand('A'));
                }

                Hand SpawnChildHand(char replacement)
                {
                    return new Hand(_regex.Replace(_text, replacement.ToString(), 1));
                }

                return hands;
                // expand/replace leftmost J 
            }
        }

        public override string ToString()
        {
            return $"{Rank},{CardText},{Classification},{Strength},{SecondaryStrength},{Bid},{Winnings},{_text}";
        }

        private void Parse()
        {
            var parts =_text.Split(' ');

            CardText = parts[0];
            Cards = CardText.Select(c => new Card(c)).ToList();
            Bid = int.Parse(parts[1]);
            Classification = Classify();
        }

        private HandClassification Classify()
        {
            if (HasJoker)
            {
                return ChildHands.Max(h => h.Classification);
            }
            else
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
            bool has3 = groups.Count(g => g.Count() == 3) == 1;
            bool has2 = groups.Count(g => g.Count() == 2) == 1;

            return has3 && has2;
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
        public bool IsJoker => Char == 'J';

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
                //'J' => 'J',
                'J' => '0',
                'Q' => 'K',
                'K' => 'L',
                'A' => 'M',
                _ => throw new ArgumentOutOfRangeException("Char", $"value: [{Char}]")
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
}