// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var lines = File.ReadAllLines("input.txt");

var rounds = lines.Select(line => new Round(line)).ToList();

var totalSelfPointsForAllRounds = rounds.Sum(r => r.SelfTotalPoints);

Console.WriteLine(totalSelfPointsForAllRounds);
Console.Read();


public class Round
{
    public UserChoice OpponentChoice { get; set; }
    public UserChoice SelfChoice
    {
        get
        {
            if (RequiredOutcome == RoundOutcome.Draw)
            {
                return OpponentChoice;
            }

            if (RequiredOutcome == RoundOutcome.OpponentWin)
            {
                if (OpponentChoice == UserChoice.Rock) return UserChoice.Scissors;
                if (OpponentChoice == UserChoice.Paper) return UserChoice.Rock;
                if (OpponentChoice == UserChoice.Scissors) return UserChoice.Paper;
            }

            if (RequiredOutcome == RoundOutcome.SelfWin)
            {
                if (OpponentChoice == UserChoice.Rock) return UserChoice.Paper;
                if (OpponentChoice == UserChoice.Paper) return UserChoice.Scissors;
                if (OpponentChoice == UserChoice.Scissors) return UserChoice.Rock;
            }

            throw new InvalidOperationException();
        }
    }

    public string InputString { get; set; }
    public Round(string inputString)
    {
        InputString = inputString;

        var choices = inputString.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (choices[0] == "A") OpponentChoice = UserChoice.Rock;
        if (choices[0] == "B") OpponentChoice = UserChoice.Paper;
        if (choices[0] == "C") OpponentChoice = UserChoice.Scissors;
        
        if (choices[1] == "X") RequiredOutcome = RoundOutcome.OpponentWin;
        if (choices[1] == "Y") RequiredOutcome = RoundOutcome.Draw;
        if (choices[1] == "Z") RequiredOutcome = RoundOutcome.SelfWin;
    }

    public int SelfChoicePoints
    {
        get
        {
            if (SelfChoice == UserChoice.Rock) return 1;
            if (SelfChoice == UserChoice.Paper) return 2;
            if (SelfChoice == UserChoice.Scissors) return 3;
            throw new InvalidOperationException();
        }
    }

    public RoundOutcome RequiredOutcome { get; set; }


    public RoundOutcome RoundOutcome
    {
        get
        {
            if (OpponentChoice == UserChoice.Rock && SelfChoice == UserChoice.Scissors ||
                OpponentChoice == UserChoice.Paper && SelfChoice == UserChoice.Rock ||
                OpponentChoice == UserChoice.Scissors && SelfChoice == UserChoice.Paper)
            {
                return RoundOutcome.OpponentWin;
            }

            if (SelfChoice == UserChoice.Rock && OpponentChoice == UserChoice.Scissors ||
                SelfChoice == UserChoice.Paper && OpponentChoice == UserChoice.Rock ||
                SelfChoice == UserChoice.Scissors && OpponentChoice == UserChoice.Paper)
            {
                return RoundOutcome.SelfWin;
            }

            return RoundOutcome.Draw;
        }
    }

    public int SelfBonusPoints
    {
        get
        {
            int bonus =  0;

            if (RoundOutcome == RoundOutcome.Draw)
                bonus = 3;
            if (RoundOutcome == RoundOutcome.SelfWin)
                bonus = 6;

            return bonus;
        }
    }
    public int SelfTotalPoints => SelfChoicePoints + SelfBonusPoints;
}

public enum RoundOutcome
{
    SelfWin,
    OpponentWin,
    Draw
}


public enum UserChoice
{
    Rock,
    Paper,
    Scissors
}




