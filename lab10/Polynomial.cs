using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace lab10
{
    public class Polynomial<T> : IEnumerable, ICloneable where T : new()
    {
        private List<T> _coefficients;

        public Polynomial()
        {
            _coefficients = new List<T>();
        }

        public Polynomial(params T[] c)
        {
            _coefficients = new List<T>(c);
        }

        private Polynomial(Polynomial<T> polynomial)
        {
            _coefficients = new List<T>(polynomial._coefficients);
        }

        private int Size => _coefficients.Count;

        public int Degree => _coefficients.Count - 1;

        public object Clone()
        {
            return new Polynomial<T>(this);
        }

        public IEnumerator GetEnumerator()
        {
            return _coefficients.GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Polynomial<T>) obj);
        }

        public static Polynomial<T> Zero(int degree = 0)
        {
            if (degree < 0) throw new NegativeDegreeException();
            var p = new Polynomial<T>();
            for (var i = 0; i <= degree; i++)
                p._coefficients.Add(!typeof(T).IsValueType ? default : new T());

            return p;
        }

        private void Add(Polynomial<T> p)
        {
            while (Size < p.Size) SetCoefficient(!typeof(T).IsValueType ? default : new T(), Size + 1);

            for (var i = 0; i < p.Size; i++)
            {
                dynamic b = p._coefficients[i];
                _coefficients[i] += b;
            }
        }

        private void SetCoefficient(T с, int degree)
        {
            while (Size < degree + 1) _coefficients.Add(typeof(T).IsValueType ? default : new T());

            _coefficients[degree] = с;
        }

        private void Subtract(Polynomial<T> p)
        {
            while (Size < p.Size) SetCoefficient(!typeof(T).IsValueType ? default : new T(), Size + 1);

            for (var i = 0; i < Size; i++)
            {
                dynamic b = p._coefficients[i];
                _coefficients[i] -= b;
            }
        }

        private void Multiply(Polynomial<T> p)
        {
            var res = Zero(p.Size + Size - 1);
            for (var i = 0; i < Size; i++)
            for (var j = 0; j < p.Size; j++)
            {
                dynamic c = _coefficients[i];
                if (res._coefficients[i + j] == null) res._coefficients[i + j] = new T();
                res._coefficients[i + j] += c * p._coefficients[j];
            }

            _coefficients = res._coefficients;
        }

        public static (Polynomial<T>, Polynomial<T>) Divide(Polynomial<T> a, Polynomial<T> b)
        {
            var tmp = new Polynomial<T>(a);

            if (a.Size < b.Size) return (Zero(), a);
            if (a.Size == b.Size)
            {
                var r = Zero();
                dynamic c = a._coefficients[^1];
                r._coefficients[0] = c / b._coefficients[^1];
                tmp -= b * c;
                tmp.Trim();
                return (r, tmp);
            }

            var res = new Polynomial<T>();

            while (tmp.Size >= b.Size)
            {
                dynamic rCoef = tmp._coefficients[^1];
                var c = rCoef / b._coefficients[^1];
                res.SetCoefficient(c, tmp.Size - b.Size);
                var addedB = new Polynomial<T>(b);
                for (var i = 0; i < tmp.Size - b.Size; i++)
                    addedB._coefficients.Insert(0, typeof(T).IsValueType ? default : new T());
                tmp -= addedB * c;
                tmp.Trim();
            }

            return (res, tmp);
        }

        private void Trim()
        {
            for (var i = Size - 1; i > 0; i--)
                switch (_coefficients[^1])
                {
                    case Matrix m:
                        if (m.IsZero()) _coefficients.RemoveAt(_coefficients.Count - 1);
                        break;
                    default:
                        if (Math.Abs((dynamic) _coefficients[^1]) < double.Epsilon)
                            _coefficients.RemoveAt(_coefficients.Count - 1);
                        break;
                }
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            for (var i = _coefficients.Count - 1; i >= 0; i--)
                if (_coefficients[0] is Matrix)
                {
                    dynamic c = _coefficients[i];
                    s.Append($"+({c.ToLine()})x^{i}");
                }
                else
                {
                    s.Append($"+({_coefficients[i]})x^{i}");
                }

            return s.ToString();
        }

        public static Polynomial<T> operator +(Polynomial<T> a, Polynomial<T> b)
        {
            return Sum(a, b);
        }

        public static Polynomial<T> operator -(Polynomial<T> a, Polynomial<T> b)
        {
            return Subtract(a, b);
        }

        public static Polynomial<T> operator *(Polynomial<T> a, Polynomial<T> b)
        {
            return Multiply(a, b);
        }

        public static Polynomial<T> operator *(Polynomial<T> a, T k)
        {
            return Multiply(a, k);
        }

        public static (Polynomial<T>, Polynomial<T>) operator /(Polynomial<T> a, Polynomial<T> b)
        {
            return Divide(a, b);
        }

        public static bool operator ==(Polynomial<T> a, Polynomial<T> b)
        {
            return a != null && a.Equals(b);
        }

        public static bool operator !=(Polynomial<T> a, Polynomial<T> b)
        {
            return a != null && a.NotEquals(b);
        }

        private bool NotEquals(Polynomial<T> p)
        {
            return !Equals(_coefficients, p._coefficients);
        }

        public static bool operator <(Polynomial<T> a, Polynomial<T> b)
        {
            return a.Less(b);
        }

        public static bool operator >(Polynomial<T> a, Polynomial<T> b)
        {
            return !a.Less(b);
        }

        private bool Less(Polynomial<T> p)
        {
            if (Degree < p.Degree) return true;
            if (Degree > p.Degree) return false;

            var res = true;
            for (var i = 0; i < Degree && res; i++) res &= (dynamic) _coefficients[i] < p._coefficients[i];

            return res;
        }

        private static Polynomial<T> Multiply(Polynomial<T> a, Polynomial<T> b)
        {
            var res = new Polynomial<T>(a);
            res.Multiply(b);
            res.Trim();
            return res;
        }

        private static Polynomial<T> Multiply(Polynomial<T> a, T k)
        {
            var res = new Polynomial<T>(a);
            res.Multiply(k);
            res.Trim();
            return res;
        }

        private void Multiply(T d)
        {
            for (var i = 0; i < Size; i++)
            {
                dynamic c = _coefficients[i];
                _coefficients[i] = c * d;
            }

            Trim();
        }

        private static Polynomial<T> Sum(Polynomial<T> a, Polynomial<T> b)
        {
            var res = new Polynomial<T>(a);
            res.Add(b);
            res.Trim();
            return res;
        }

        private static Polynomial<T> Subtract(Polynomial<T> a, Polynomial<T> b)
        {
            var res = new Polynomial<T>(a);
            res.Subtract(b);
            res.Trim();
            return res;
        }

        public T Compute(T point)
        {
            if (_coefficients[0] is Matrix)
            {
                var res = new T();
                dynamic p = point;
                dynamic t = (T) p.Clone();

                for (var i = 0; i < Size; i++)
                {
                    res += _coefficients[i] * t;
                    t *= p;
                }

                return res;
            }
            else
            {
                T res = default;
                for (var i = 0; i < Size; i++)
                {
                    dynamic c = _coefficients[i], p = point;
                    res += c * Math.Pow(p, i);
                }

                return res;
            }
        }

        private Polynomial<T> GetMonomial(int degree)
        {
            var p = Zero(degree);
            p._coefficients[degree] = _coefficients[degree];
            return p;
        }

        public Polynomial<T> Compose(Polynomial<T> p)
        {
            if (Size < 1) return (Polynomial<T>) Clone();
            var res = Zero();

            res += GetMonomial(0);
            for (var i = 1; i < Size; i++)
            {
                var m = GetMonomial(i);
                var a = m * p;
                res.Add(a);
            }

            res.Trim();

            return res;
        }

        public bool Equals(Polynomial<T> other)
        {
            return Equals(_coefficients, other._coefficients);
        }

        public override int GetHashCode()
        {
            return _coefficients != null ? _coefficients.GetHashCode() : 0;
        }

        [Serializable]
        public class TypeException : Exception
        {
            public TypeException()
            {
            }

            public TypeException(string message) : base(message)
            {
            }

            public TypeException(string message, Exception inner) : base(message, inner)
            {
            }

            protected TypeException(
                SerializationInfo info,
                StreamingContext context) : base(info, context)
            {
            }
        }

        [Serializable]
        public class NegativeDegreeException : Exception
        {
            public NegativeDegreeException()
            {
            }

            public NegativeDegreeException(string message) : base(message)
            {
            }

            public NegativeDegreeException(string message, Exception inner) : base(message, inner)
            {
            }

            protected NegativeDegreeException(
                SerializationInfo info,
                StreamingContext context) : base(info, context)
            {
            }
        }
    }
}