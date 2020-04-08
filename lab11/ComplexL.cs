using System;
using System.Collections.Generic;
using System.Numerics;

namespace lab11
{
    public class ComplexL: ICloneable
    {
        private double _real;
        private double _imagine;

        public ComplexL(double real)
        {
            _real = real;
            _imagine = 0;
        }
        public ComplexL(double real, double imagine)
        {
            _real = real;
            _imagine = imagine;
        }

        public ComplexL(ComplexL c)
        {
            _real = c._real;
            _imagine = c._imagine;
            DivideByZeroEvent = c.DivideByZeroEvent;
        }

        private void Add(ComplexL c)
        {
            _real += c._real;
            _imagine += c._imagine;
        }

        private void Subtract(ComplexL c)
        {
            _real -= c._real;
            _imagine -= c._real;
        }

        private void Multiply(ComplexL c)
        {
            var t = _real;
            _real = _real * c._real - _imagine * c._imagine;
            _imagine = t * c._imagine + _imagine * c._real;
        }

        private void Multiply(double num)
        {
            _real *= num;
            _imagine *= num;
        }

        private void Divide(ComplexL c)
        {
            if (Math.Abs(c.Abs) < double.Epsilon)
            {
                DivideByZeroEvent?.Invoke(this, new ComplexEventArgs("Divide by zero", this, c));
                return;
            }
            ComplexL t = new ComplexL(this);
            t.Multiply(c.Conjugate());
            t.Divide(c.Abs);
        }

        private void Divide(double num)
        {
            Multiply(1/num); 
        }

        public double Abs => Math.Sqrt(_real * _real + _imagine * _imagine);

        public ComplexL Conjugate()
        {
            return new ComplexL(_real, -_imagine);
        }

        public static ComplexL Zero => new ComplexL(0, 0);
        public static ComplexL One => new ComplexL(1, 0);
        public static ComplexL NegativeOne => new ComplexL(-1, 0);

        public static ComplexL ImagineOne => new ComplexL(0, 1);
        public static ComplexL NegativeImagineOne => new ComplexL(0, -1);

        static ComplexL SumOf(ComplexL a, ComplexL b)
        {
            ComplexL res = new ComplexL(a);
            res.Add(b);
            return res;
        }
        
        static ComplexL SubtractOf(ComplexL a, ComplexL b)
        {
            ComplexL res = new ComplexL(a);
            res.Subtract(b);
            return res;
        }
        
        static ComplexL DivideOf(ComplexL a, ComplexL b)
        {
            ComplexL res = new ComplexL(a);
            res.Divide(b);
            return res;
        }
        
        static ComplexL MultiplyOf(ComplexL a, ComplexL b)
        {
            ComplexL res = new ComplexL(a);
            res.Multiply(b);
            return res;
        }

        public static ComplexL operator +(ComplexL a, ComplexL b)
        {
            return SumOf(a, b);
        }
        
        public static ComplexL operator -(ComplexL a, ComplexL b)
        {
            return SubtractOf(a, b);
        }
        public static ComplexL operator *(ComplexL a, ComplexL b)
        {
            return MultiplyOf(a, b);
        }
        public static ComplexL operator /(ComplexL a, ComplexL b)
        {
            return DivideOf(a, b);
        }

        public object Clone()
        {
            return new ComplexL(this);
        }

        public override string ToString()
        {
            return $"{_real} " + (_imagine < 0 ? $"{_imagine}i" : $"+ {_imagine}i");
        }

        private double GetArg()
        {
            if (Math.Abs(_real) < double.Epsilon && Math.Abs(_imagine) < double.Epsilon) return 0;
            var res = Math.Atan(_imagine / _real);
            if (_real < 0) res += _imagine > 0 ? Math.PI : -Math.PI;
            // if (_imagine > 0 && _real < 0) res += Math.PI;
            // if (_imagine < 0 && _real < 0) res -= Math.PI;
            return res;
        }

        public double Arg => GetArg();

        public List<ComplexL> Root(int root)
        {
            var list = new List<ComplexL>();
            double abs = Math.Pow(Abs, 1.0 / root), arg = Arg;
            for (int i = 0; i < root; i++)
            {
                var re = abs * Math.Cos((arg + 2 * Math.PI * i) / root);
                var im = abs * Math.Sin((arg + 2 * Math.PI * i) / root);
                list.Add(new ComplexL(re, im));
            }
            return list;
        }

        public ComplexL Pow(int degree)
        {
            var abs = Math.Pow(Abs, degree);
            var re = abs * Math.Cos(degree * Arg);
            var im = abs * Math.Sin(degree * Arg);
            return new ComplexL(re, im);
        }

        public delegate void ComplexHandler(object sender, ComplexEventArgs args);

        public event ComplexHandler DivideByZeroEvent;
    }

    public class ComplexEventArgs
    {
        public string Message { get; }
        public ComplexL First { get; }
        public ComplexL Second { get; }

        public ComplexEventArgs(string msg)
        {
            Message = msg;
        }

        public ComplexEventArgs(string msg, ComplexL first)
        {
            Message = msg;
            First = first;
        }

        public ComplexEventArgs(string message, ComplexL first, ComplexL second)
        {
            Message = message;
            First = first;
            Second = second;
        }
    }
}