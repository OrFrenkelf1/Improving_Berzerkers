using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

public interface IRandomProvider
{
    int Roll(uint min, uint max);
    int Bag(int min, int max);
    uint MaxValue { get; set; }
}

public class Dice<T> : IRandomProvider where T : IComparable<T>
{
    private uint _scalar; // means the amount of dice
    private T _baseDie;
    private T _modifier;
    public uint MaxValue { get; set; }

    private List<T> listOfNumbers;

    // constructor
    public Dice(uint scalar, T baseDie, T modifier)
    {
        _scalar = scalar;
        _baseDie = baseDie;
        _modifier = modifier;

        listOfNumbers = new List<T>((int)_scalar);
    }

    public int Roll(uint min, uint max)
    {
        dynamic value = 0;

        for (int i = 0; i < _scalar; i++)
        {
            value += (dynamic)Random.Shared.Next((int)min, (int)max + 1);
        }

        return value + (dynamic)_modifier;
    }

    public override string ToString()
    {
        return (_scalar.ToString() + _baseDie.ToString() + _modifier.ToString());
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode() // You need to implement this properly
    {
        return base.GetHashCode();
    }

    public int Bag(int min, int max)
    {
        if (listOfNumbers.Count >= _scalar)
        {
            listOfNumbers.Clear();
            Console.WriteLine("Bag list was reset");
        }
        dynamic value = 0;
        dynamic randomDieRoll = 0;

        for (int i = 0; i < _scalar; i++)
        {
            randomDieRoll = Random.Shared.Next(min, max + 1);
            Console.WriteLine("Rolled " + randomDieRoll);

            Console.WriteLine("Added " + randomDieRoll);
            listOfNumbers.Add(randomDieRoll);

            value += randomDieRoll;
        }

        return value + (dynamic)_modifier;
    }
}
