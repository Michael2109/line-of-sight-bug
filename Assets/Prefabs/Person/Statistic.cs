using System;

namespace Game_Objects.Person.Scripts
{
    public class Statistic
    {
        private static float TOLERANCE = 0.000001f;

        public float Percentage { get;  set; }

        public Statistic() 
        {
        }
    
        public Statistic(float percentage) 
        {
            this.Percentage = percentage;
        }

        public void Increase(float amount)
        {
            if (Math.Abs(Percentage - 1f) > TOLERANCE)
            {
                if (Percentage + amount >= 1)
                {
                    Percentage = 1;
                }
                else
                {
                    Percentage += amount;
                }
            }
        }

        public void Decrease(float amount)
        {
            if (Math.Abs(Percentage) > TOLERANCE)
            {
                if (Percentage - amount <= 0)
                {
                    Percentage = 0;
                }
                else
                {
                    Percentage -= amount;
                }
            }
        }

        public bool IsMax()
        {
            return Math.Abs(Percentage - 1) < TOLERANCE;
        }

        public bool IsMin()
        {
            return Math.Abs(Percentage) < TOLERANCE;
        }
    }
}
