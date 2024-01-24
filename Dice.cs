using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public struct Dice
{
    private uint _scalar;
    private uint _baseDie;
    private int _modifier;

    //constructor
    public Dice(uint scalar, uint baseDie, int modifier)
    {
        _scalar = scalar;
        _baseDie = baseDie;
        _modifier = modifier;
    }

    public int Roll()
    {
        int value = 0;

        for (int i = 0; i < _scalar; i++)
        {
            value += Random.Shared.Next(1, (int)_baseDie+1) ;
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
}

