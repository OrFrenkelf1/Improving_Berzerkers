using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IRandomProvider
{
    public int Roll(uint min, uint max);

    public int Bag(int min, int max);

    public uint MaxValue { get; set; }
}

public struct Dice : IRandomProvider
{
    private uint _scalar; //means the amount of dice
    private uint _baseDie;
    private int _modifier;
    public uint MaxValue { get => _baseDie; set => _baseDie = value; }

    private List<int> listOfNumbers;

    //constructor
    public Dice(uint scalar, uint baseDie, int modifier)
    {
        _scalar = scalar;
        _baseDie = baseDie;
        _modifier = modifier;

        listOfNumbers = new List<int>((int)_baseDie);
    }

    public int Roll(uint min, uint max)
    {
        int value = 0;

        for (int i = 0; i < _scalar; i++)
        {
            value += Random.Shared.Next((int)min, (int)max + 1);
        }

        return value + _modifier;
    }
    public override string ToString()
    {
        return (_scalar.ToString() + _baseDie.ToString() + _modifier.ToString());
    }
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return base.Equals(obj);
    }
    public override int GetHashCode() //I donno what to do here
    {
        int hash = 1;

        return hash;
    }

    public int Bag(int min, int max)
    {
        if (listOfNumbers.Count >= _baseDie)
        {
            listOfNumbers.Clear();
            Console.WriteLine("Bag list was reset");
        }
        int value = 0;
        int randomDieRoll = 0;

        for (int i = 0; i < _scalar; i++)
        {
            randomDieRoll = Random.Shared.Next(min, max + 1);
            Console.WriteLine("Rolled " + randomDieRoll);


            Console.WriteLine("Added " + randomDieRoll);
            listOfNumbers.Add(randomDieRoll);

            value += randomDieRoll;
        }

        return value + _modifier;
    }

}

