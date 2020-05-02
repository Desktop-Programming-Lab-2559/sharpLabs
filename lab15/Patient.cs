using System;

namespace lab15
{
    public class Patient : Human
    {
        public Patient()
        {
            // IsInfected = true;
            if (new Random().Next(100) == 0) IsInfected = true;
        }
    }
}