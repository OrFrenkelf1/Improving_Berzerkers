using ImprovingBerzerkers;

public class RandomFighter
{
    public void Fight(Deck<int> deck, Dice<int> dice)
    {
        int deckWins = 0;
        int diceWins = 0;
        int ties = 0;

        Console.WriteLine("RandomFighter");

        while (deck.TryDraw(out int deckCard))
        {
            int diceRoll = dice.Roll();

            if (deckCard > diceRoll)
                deckWins++;

            else if (deckCard < diceRoll)
                diceWins++;

            else
                ties++;
        }
        Console.WriteLine($"Deck wins: {deckWins}, Dice wins: {diceWins}, Ties: {ties}");
    }
}
