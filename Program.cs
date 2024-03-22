// ---- C# II (Dor Ben Dor) ----
// Or Frenkel
// -----------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

//enums
enum Race { Human, Animal, God }
enum Weather { Sunny, Rainy, Foggy }

abstract class Unit
{
    protected Race UnitRace { get; set; }
    protected Weather UnitWeatherEffect { get; set; }
    protected virtual IRandomProvider DamageDice { get; set; }
    virtual public int HP { get; protected set; }
    virtual public int CarryingCapacity { get; protected set; }
    protected virtual IRandomProvider HitChance { get; set; }
    protected virtual IRandomProvider DefenseRating { get; set; }

    //constructor
    public Unit(Race myRace, IRandomProvider damage, int hp, int carryingCapacity, IRandomProvider hitChance, IRandomProvider defenseRating)
    {
        UnitRace = myRace;
        DamageDice = damage;
        HP = hp;
        CarryingCapacity = carryingCapacity;
        HitChance = hitChance;
        DefenseRating = defenseRating;
        UnitWeatherEffect = Weather.Sunny;
    }

    public virtual void SetWeatherEffect(Weather recivedWeather)
    {
        UnitWeatherEffect = recivedWeather;
    }

    public virtual void Attack(Unit unitToHit)
    {
        int rollHitChance = HitChance.Roll(1, HitChance.MaxValue);
        int rollAttack = DamageDice.Roll(1, HitChance.MaxValue);

        Console.WriteLine($"{UnitRace} Attacked {unitToHit.UnitRace} For {rollAttack} Damage");

        unitToHit.Defend(rollHitChance, rollAttack);
    }
    public virtual void Heal()
    {
        Random random = new Random();
        int healChance = random.Next(1, 11);

        if (healChance <= 5)
        {
            HP += 1;
            Console.WriteLine($"\n{UnitRace} Healed for 1 HP\n");
        }
        else
        {
            Console.WriteLine($"\n{UnitRace} Falied to heal!\n");
            return;
        }
    }
    public virtual void Defend(int hitChance, int damage)
    {
        int defence = DefenseRating.Roll(20, 50);

        if (hitChance > defence) //damage don't hit if eaqule the defence rating
        {
            HP -= damage;

            if (HP < 0)
                HP = 0;
        }
        else
        {
            Console.WriteLine("Attack Deffended");
        }
    }
}

class Tank : Unit
{
    //constructor
    public Tank(Race myRace, IRandomProvider damage, int hp, int carryingCapacity, IRandomProvider hitChance, IRandomProvider defenseRating) : base(myRace, damage, hp, carryingCapacity, hitChance, defenseRating) { }

    public override void Attack(Unit unit)
    {
        int rollHitChance = HitChance.Roll(1, 20);
        int rollAttack = DamageDice.Bag(1, 6);

        unit.Defend(rollHitChance, rollAttack);
    }
    public override void Defend(int hitChance, int damage)
    {
        base.Defend(hitChance, damage);

        if (hitChance > 8) // Tank has a higher chance to defend against attacks
        {
            damage += 10; // Increase the damage
            Console.WriteLine($"\n{UnitRace} successfully defended! damage + 10");
        }
    }
}
class Mind_Control : Unit
{
    // Constructor
    public Mind_Control(Race myRace, IRandomProvider damage, int hp, int carryingCapacity, IRandomProvider hitChance, IRandomProvider defenseRating) : base(myRace, damage, hp, carryingCapacity, hitChance, defenseRating) { }

    public override void Attack(Unit unit)
    {
        base.Attack(unit);
    }
    public override void Defend(int hitChance, int damage)
    {
        int newDamage = damage / 2;

        base.Defend(hitChance, newDamage);

        Console.WriteLine($"\n{UnitRace} defended against an attack! +{newDamage} damage.");
    }
    public override void Heal()
    {
        base.Heal();
        HP += 1;

        Console.WriteLine($"\n{UnitRace} Healed! + 1 HP!");
    }


}
class FightSimulator
{
    public static void Fight(List<Unit> Player1, List<Unit> Player2)
    {
        Dice<int> weatherDice = new Dice<int>(1, 6, 0); // Create a new instance of Dice<int>
        int turns = 0;
        while (Player1.Count > 0 && Player2.Count > 0)
        {
            turns++;

            foreach (Unit attackerA in Player1)
            {
                if (Player2.Count > 0)
                {
                    Unit targetB = Player2[0];

                    attackerA.Attack(targetB);

                    if (targetB.HP <= 0)
                        Player2.Remove(targetB);
                }
                weatherDice.Roll(1, 6);

                attackerA.Heal();
            }

            foreach (var attackerB in Player2)
            {
                if (Player1.Count > 0)
                {
                    Unit targetA = Player1[0];

                    attackerB.Attack(targetA);

                    if (targetA.HP <= 0)
                        Player1.Remove(targetA);
                }
                weatherDice.Roll(1, 6);

                attackerB.Heal();
            }
        }
        Console.WriteLine(turns);
    }

    private static Dice<int> RollWeather(Dice<int> weatherDice, Unit attackerA)
    {
        switch (weatherDice.Roll(20, 50))
        {
            case 2:
                attackerA.SetWeatherEffect(Weather.Rainy);
                Console.WriteLine("\nWeather It is now Rainy\n");
                break;
            case 3:
                attackerA.SetWeatherEffect(Weather.Foggy);
                Console.WriteLine("\nIt is now Foggy\n");
                break;
            case 5:
                attackerA.SetWeatherEffect(Weather.Rainy);
                Console.WriteLine("\nIt is now Rainy\n");
                break;
            case 6:
                attackerA.SetWeatherEffect(Weather.Foggy);
                Console.WriteLine("\nIt is now Foggy\n");
                break;
            default:
                attackerA.SetWeatherEffect(Weather.Sunny);
                Console.WriteLine("\nIt is now Sunny\n");
                break;
        }

        return weatherDice;
    }
}
class Program
{
    static void Main()
    {
        Dice<int> attackDice = new Dice<int>(2, 20, 10);
        Dice<int> hitChanceDice = new Dice<int>(1, 20, 10);
        Dice<int> defendDice = new Dice<int>(1, 6, 2);


        List<Unit> player1Team = new List<Unit>
        {
            new Tank(Race.Human, attackDice, 100, 50, hitChanceDice, defendDice),
            new Tank(Race.Human, attackDice, 70, 50, hitChanceDice, defendDice),
            new Tank(Race.Human, attackDice, 40, 50, hitChanceDice, defendDice),
            new Tank(Race.Animal, attackDice, 60, 50, hitChanceDice, defendDice),
            new Tank(Race.Animal, attackDice, 80, 50, hitChanceDice, defendDice),
        };

        List<Unit> player2Team = new List<Unit>
        {
            new Mind_Control(Race.Animal, attackDice, 50, 50, hitChanceDice, defendDice),
            new Mind_Control(Race.God, attackDice, 100, 50, hitChanceDice, defendDice),
            new Mind_Control(Race.God, attackDice, 100, 50, hitChanceDice, defendDice),
            new Mind_Control(Race.God, attackDice, 100, 50, hitChanceDice, defendDice),
        };

        FightSimulator.Fight(player1Team, player2Team);

        // The Winner
        int resources = 0;
        if (player1Team.Count > 0)
        {
            foreach (Unit unit in player1Team)
            {
                resources += unit.CarryingCapacity;
            }
            Console.WriteLine($"\nVictory!\nPlayer 1 Wins!\nand got {resources} resources!!!\nAnd Respect");
        }
        else
        {
            foreach (Unit unit in player2Team)
            {
                resources += unit.CarryingCapacity;
            }
            Console.WriteLine($"\nVictory!\nPlayer 2 Wins!\nand got {resources} resources!!!\nAnd Respect");
        }
    }
}