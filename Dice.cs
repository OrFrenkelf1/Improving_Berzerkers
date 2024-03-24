using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

public abstract class Dice<T> where T : IComparable<T>
{
    protected uint _scalar; // means the amount of dice
    protected T _baseDie;
    protected T _modifier;

    // constructor
    public Dice(uint scalar, T baseDie, T modifier)
    {
        _scalar = scalar;
        _baseDie = baseDie;
        _modifier = modifier;
    }

    public abstract T Roll();

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
}
public class IntDice : Dice<int>
{
    public IntDice(uint scalar, int baseDie, int modifier) : base(scalar, baseDie, modifier) { }

    public override int Roll()
    {
        int value = 0;

        for (int i = 0; i < _scalar; i++)
        {
            value += Random.Shared.Next( 1 , _baseDie );
        }
        return value + (int)_modifier;
    }
}