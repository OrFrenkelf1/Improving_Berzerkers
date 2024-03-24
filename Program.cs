// ---- C# II (Dor Ben Dor) ----
// Or Frenkel
// -----------------------------
using ImprovingBerzerkers;

class Program
{
    static void Main(string[] args)
    {
        Deck<int> deck = new Deck<int>(40);

        Dice<int> dice = new IntDice(1, 20, 0);

        RandomFighter randomFighter = new RandomFighter();
        randomFighter.Fight(deck, dice);
    }
}