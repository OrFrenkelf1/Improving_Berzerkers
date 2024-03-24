using System;
using System.Collections.Generic;
using System.Linq;

namespace ImprovingBerzerkers
{
    public class Deck<T> where T : IComparable<T>
    {
        private List<T> _cards;
        private List<T> _discardPile;

        public int Size { get; private set; }
        public int Remaining => _cards.Count;

        public Deck(int size)
        {
            _cards = new List<T>(size);
            _discardPile = new List<T>();

            for (int i = 0; i < size; i++)
                _cards.Add((T)(object)Random.Shared.Next(1, 21));

            Size = size;
            Reshuffle();
        }

        //reshuffles the current deck
        public void Shuffle()
        {
            Random random = new Random();
            _cards = _cards.OrderBy(x => random.Next()).ToList();
        }

        //add the discard pile to the deck and reshuffles it
        public void Reshuffle()
        {
            _cards.AddRange(_discardPile);
            _discardPile.Clear();
            Shuffle();
        }

        //try to draw a card from the top of the deck, if there is any
        public bool TryDraw(out T card)
        {
            if (_cards.Count == 0)
            {
                card = default;
                return false;
            }
                
            _discardPile.Add(_cards[0]);
            card = _cards[0];
            _cards.RemoveAt(0);
            return true;
        }

        //peeks at the top of the Deck
        public T Peek()
        {
            if (_cards.Count == 0)
                throw new InvalidOperationException("Deck is empty.");

            return _cards[0];
        }

        public uint MaxValue { get; set; }

        public int Bag(int min, int max)
        {
            throw new NotImplementedException();
        }

        public int Roll(uint min, uint max)
        {
            throw new NotImplementedException();
        }
    }
}
