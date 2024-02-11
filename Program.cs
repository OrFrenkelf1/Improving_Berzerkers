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
            Console.WriteLine($"{UnitRace} Healed for 20 HP");
        }
        else
        {
            Console.WriteLine($"{UnitRace} Did not heal");
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
            Console.WriteLine($"{UnitRace} successfully defended and increased the damage by 10");
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

        Console.WriteLine($"{UnitRace} defended against an attack and received {newDamage} damage.");
    }
    public override void Heal()
    {
        base.Heal();
        HP += 10;

        Console.WriteLine($"{UnitRace} Healed for 30 HP");
    }


}
class FightSimulator
{
    public static void Fight(List<Unit> Player1, List<Unit> Player2)
    {
        Dice weatherDice = new Dice(1, 6, 0);
        int turns = 0;
        while (Player1.Count > 0 && Player2.Count > 0)
        {
            turns++;

            foreach (Unit attackerA in Player1)
            {
                // Are there's still Units in Player2 ?
                if (Player2.Count > 0)
                {
                    // First unit in Team B
                    Unit targetB = Player2[0];

                    attackerA.Attack(targetB);

                    if (targetB.HP <= 0)
                        Player2.Remove(targetB);
                }
                weatherDice = RollWeather(weatherDice, attackerA);

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
                weatherDice = RollWeather(weatherDice, attackerB);

                attackerB.Heal();
            }
        }
        Console.WriteLine(turns);
    }

    private static Dice RollWeather(Dice weatherDice, Unit attackerA)
    {
        switch (weatherDice.Roll(20, 50))
        {
            case 2:
                attackerA.SetWeatherEffect(Weather.Rainy);
                Console.WriteLine("It is now Rainy");
                break;
            case 3:
                attackerA.SetWeatherEffect(Weather.Foggy);
                Console.WriteLine("It is now Foggy");
                break;
            case 5:
                attackerA.SetWeatherEffect(Weather.Rainy);
                Console.WriteLine("It is now Rainy");
                break;
            case 6:
                attackerA.SetWeatherEffect(Weather.Foggy);
                Console.WriteLine("It is now Foggy");
                break;
            default:
                attackerA.SetWeatherEffect(Weather.Sunny);
                Console.WriteLine("It is now Sunny");
                break;
        }

        return weatherDice;
    }
}
class Program
{
    static void Main()
    {
        Dice attackDice = new Dice(2, 20, 10);
        Dice hitCHanceDice = new Dice(1, 20, 10);
        Dice defendDice = new Dice(1, 6, 2);


        List<Unit> player1Team = new List<Unit>
        {
            new Tank(Race.Human, attackDice, 100, 50, hitCHanceDice, defendDice),
            new Tank(Race.Human, attackDice, 70, 50, hitCHanceDice, defendDice),
            new Tank(Race.Human, attackDice, 40, 50, hitCHanceDice, defendDice),
            new Tank(Race.Animal, attackDice, 60, 50, hitCHanceDice, defendDice),
            new Tank(Race.Animal, attackDice, 80, 50, hitCHanceDice, defendDice),
        };

        List<Unit> player2Team = new List<Unit>
        {
            new Mind_Control(Race.Animal, attackDice, 50, 50, hitCHanceDice, defendDice),
            new Mind_Control(Race.God, attackDice, 100, 50, hitCHanceDice, defendDice),
            new Mind_Control(Race.God, attackDice, 100, 50, hitCHanceDice, defendDice),
            new Mind_Control(Race.God, attackDice, 100, 50, hitCHanceDice, defendDice),
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
            Console.WriteLine($"Player 1 Wins! and got {resources} resources");
        }
        else
        {
            foreach (Unit unit in player2Team)
            {
                resources += unit.CarryingCapacity;
            }
            Console.WriteLine($"Player 2 Wins! and got {resources} resources");
        }
    }
}