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
    public virtual Dice DamageDice { get; protected set; }
    virtual public int HP { get; protected set; }
    virtual public int CarryingCapacity { get; protected set; }
    public virtual Dice HitChance { get; protected set; }
    public virtual Dice DefenseRating { get; protected set; }

    //constructor
    public Unit(Race myRace, Dice damage, int hp, int carryingCapacity, Dice hitChance, Dice defenseRating)
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
        int rollHitChance = HitChance.Roll();
        int rollAttack = DamageDice.Roll();

        Console.WriteLine($"{UnitRace} Attacked {unitToHit.UnitRace} For {rollAttack} Damage");

        unitToHit.Defend(rollHitChance, rollAttack);
    }
    public virtual void Heal()
    {
        Random random = new Random();
        int healChance = random.Next(1, 11);

        if (healChance <= 5)
        {
            HP += 20;
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
        int defence = DefenseRating.Roll();

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
    public Tank(Race myRace, Dice damage, int hp, int carryingCapacity, Dice hitChance, Dice defenseRating) : base(myRace, damage, hp, carryingCapacity, hitChance, defenseRating) { }

    public override void Attack(Unit unit)
    {
        base.Attack(unit);
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
    public Mind_Control(Race myRace, Dice damage, int hp, int carryingCapacity, Dice hitChance, Dice defenseRating) : base(myRace, damage, hp, carryingCapacity, hitChance, defenseRating) { }

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

    class FightSimulator
    {
        public static void Fight(List<Unit> Player1, List<Unit> Player2)
        {
            Dice weatherDice = new Dice(1, 6, 0);

            while (Player1.Count > 0 && Player2.Count > 0)
            {
                // Player1 attacks Player2
                foreach (Unit attackerA in Player1)
                {
                    // Are there's still Units in Player2 ?
                    if (Player2.Count > 0)
                    {
                        // Select the first unit in Team B as the target
                        Unit targetB = Player2[0];

                        // Attack Team B
                        attackerA.Attack(targetB);

                        // Remove the targets
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
        }

        private static Dice RollWeather(Dice weatherDice, Unit attackerA)
        {
            switch (weatherDice.Roll())
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
            Dice DeafendDice = new Dice(1, 6, 2);

            List<Unit> player1Team = new List<Unit>
            {
            new Tank(Race.Human, attackDice, 100, 50, hitCHanceDice, DeafendDice),
            new Tank(Race.Human, attackDice, 70, 50, hitCHanceDice, DeafendDice),
            new Tank(Race.Human, attackDice, 40, 50, hitCHanceDice, DeafendDice),
            new Tank(Race.Animal, attackDice, 60, 50, hitCHanceDice, DeafendDice),
            new Tank(Race.Animal, attackDice, 80, 50, hitCHanceDice, DeafendDice),
            };

            List<Unit> player2Team = new List<Unit>
            {
            new Mind_Control(Race.Animal, attackDice, 50, 50, hitCHanceDice, DeafendDice),
            new Mind_Control(Race.God, attackDice, 100, 50, hitCHanceDice, DeafendDice),
            new Mind_Control(Race.God, attackDice, 100, 50, hitCHanceDice, DeafendDice),
            new Mind_Control(Race.God, attackDice, 100, 50, hitCHanceDice, DeafendDice),
            };

            FightSimulator.Fight(player1Team, player2Team);

            // The Winner
            int resources = 0;
            if (player1Team.Count > 0)
            {
                foreach(Unit unit in player1Team)
                {
                    resources += unit.CarryingCapacity;
                }
                Console.WriteLine ($"Player 1 Wins! and got {resources} resources");
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
}