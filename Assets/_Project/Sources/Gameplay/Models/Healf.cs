using System;
using UnityEngine;

namespace GOBA
{
    [Serializable]
    public class Healf
    {
        public Healf()
        {

        }

        public Healf(float amount)
        {
            Current = amount;
            Max = amount;
        }

        public event Action<float> CurrentChanged;
        public event Action<float> MaxChanged;


        [field: SerializeField] public float Current { get; private set; }
        [field: SerializeField] public float Max { get; private set; }

        public void Add(float amount)
        {
            if (amount <= 0)
                return;

            Current += Round(amount);
            Current = Mathf.Clamp(Current, 0, Max);
            CurrentChanged?.Invoke(Current);
        }

        public void AddMax(float amount)
        {
            if (amount <= 0)
                return;

            Max += Round(amount);
            MaxChanged?.Invoke(Max);
        }

        public void Remove(float damage)
        {
            if (damage <= 0)
                return;

            var tempAmount = Current - damage;
            if (tempAmount < 0)
                tempAmount = 0;

            Current = Round(tempAmount);
            CurrentChanged?.Invoke(Current);
        }

        public void RemoveMax(float amount)
        {
            if (amount <= 0)
                return;

            var tempAmount = Max - amount;
            if (tempAmount < 0)
                tempAmount = 0;

            Max = Round(tempAmount);

            if (Max > Current)
            {
                Current = Mathf.Clamp(Current, 0, Max);
                CurrentChanged?.Invoke(Current);
            }

            MaxChanged?.Invoke(Max);
        }

        private float Round(float value)
        {
            return MathF.Round(value, 2);
        }
    }
}