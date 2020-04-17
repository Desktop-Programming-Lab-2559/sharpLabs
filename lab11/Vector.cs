// b) Реализовать обобщенный класс вектор и все операции векторной алгебры, а также нахождение модуля и
// скалярного произведения двух векторов. Реализовать статическую функцию, которая проводит процесс ортогонализации
// переданного множества векторов. Аргументом этой функции является коллекция векторов.
// Реализовать способ сравнения векторов, методы преобразования в(из) массив(а) элементов соответствующего типа.
// При описании шаблона укажите ограничения: наличие пустого конструктора у типа-аргумента.
// Продемонстрируйте работу вашего обобщённого класса с классом комплексных чисел из пункта

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace lab11
{
    public class VectorL<T> : ICloneable, IEnumerable, IComparable, IComparable<VectorL<T>> where T : new()
    {
        private List<T> _vec;

        protected bool Equals(VectorL<T> other)
        {
            return Equals(_vec, other._vec);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((VectorL<T>) obj);
        }

        public override int GetHashCode()
        {
            return (_vec != null ? _vec.GetHashCode() : 0);
        }

        public VectorL()
        {
            _vec = new List<T> {new T()};
        }

        public VectorL(params T[] arr)
        {
            _vec = new List<T>(arr.Length);
            foreach (var c in arr) _vec.Add(c);
        }

        public VectorL(VectorL<T> v)
        {
            _vec = new List<T>(v._vec);
        }

        public int Size => _vec.Count;

        public object Clone()
        {
            return new VectorL<T>(this);
        }

        public IEnumerator GetEnumerator()
        {
            return _vec.GetEnumerator();
        }

        public void Add(VectorL<T> v)
        {
            if (Size != v.Size) throw new DifferentSizeException($"{Size} != {v.Size}");

            for (var i = 0; i < Size; i++) _vec[i] += (dynamic) v._vec[i];
        }

        public void Subtract(VectorL<T> v)
        {
            if (Size != v.Size) throw new DifferentSizeException($"{Size} != {v.Size}");

            for (var i = 0; i < Size; i++) _vec[i] -= (dynamic) v._vec[i];
        }

        public T ScalarMul(VectorL<T> v)
        {
            if (Size != v.Size) throw new DifferentSizeException($"{Size} != {v.Size}");

            var res = new T();
            for (var i = 0; i < Size; i++) res += (dynamic) _vec[i] * v._vec[i];

            return res;
        }

        public T Abs()
        {
            var res = new T();
            res = _vec.Aggregate(res, (current, c) => current + (dynamic)c * c);
            res = res switch
            {
                ComplexL c => (dynamic) c.Root(2)[0],
                _ => (T)Math.Sqrt((dynamic) res)
            };

            return res;
        }

        public static List<VectorL<T>> Orthogonality(IEnumerable<VectorL<T>> arr)
        {
            var basis = arr.ToArray();
            var ortogonolised = new List<VectorL<T>>(basis.Length);
            if (basis.Length == 0) return ortogonolised;

            ortogonolised.Add(basis[0]);
            for (var i = 1; i < basis.Length; i++)
            {
                var newVec = ortogonolised.Aggregate(
                    new VectorL<T>(basis[i]),
                    (current, b) => (VectorL<T>) (current - (dynamic) (basis[i] * b) / (b * b) * b)
                );
                switch (newVec) // отбрасывание линейно зависимого вектора
                {
                    case VectorL<ComplexL> v:
                        if (v.Abs().Abs > double.Epsilon)
                            ortogonolised.Add(newVec);
                        break;
                    default:
                        if (Math.Abs((dynamic)newVec.Abs()) > double.Epsilon)
                            ortogonolised.Add(newVec);
                        break;
                }
            }

            return ortogonolised;
        }

        public static VectorL<T> operator +(VectorL<T> a, VectorL<T> b)
        {
            return Sum(a, b);
        }

        private static VectorL<T> Sum(VectorL<T> a, VectorL<T> b)
        {
            var res = new VectorL<T>(a);
            res.Add(b);
            return res;
        }
        
        public static VectorL<T> operator -(VectorL<T> a, VectorL<T> b)
        {
            return Subtract(a, b);
        }

        private static VectorL<T> Subtract(VectorL<T> a, VectorL<T> b)
        {
            var res = new VectorL<T>(a);
            res.Subtract(b);
            return res;
        }

        public static T operator *(VectorL<T> a, VectorL<T> b)
        {
            return a.ScalarMul(b);
        }

        public static VectorL<T> operator *(VectorL<T> v, T c)
        {
            return c * v;
        }

        public static VectorL<T> operator *(T c, VectorL<T> v)
        {
            return Mul(v, c);
        }

        private static VectorL<T> Mul(VectorL<T> v, T c)
        {
            var res = new VectorL<T>(v);
            res.Mul(c);
            return res;
        }

        private void Mul(T c)
        {
            for (var i = 0; i < Size; i++) _vec[i] *= (dynamic) c;
        }

        public static bool operator <(VectorL<T> a, VectorL<T> b)
        {
            return a.CompareTo(b) < 0;
        }
        
        public static bool operator ==(VectorL<T> a, VectorL<T> b)
        {
            return a.CompareTo(b) == 0;
        }

        public static bool operator !=(VectorL<T> a, VectorL<T> b)
        {
            return a.CompareTo(b) != 0;
        }

        // private static bool IsLess(VectorL<T> a, VectorL<T> b)
        // {
        //     if (a.Size < b.Size) return true;
        //     if (a.Size > b.Size) return false;
        //     
        //     var res = true;
        //     for (int i = 0; i < a.Size; i++)
        //     {
        //         res &= (dynamic)a._vec[i] < b._vec[i];
        //     }
        //     return res;
        // }
        
        public static bool operator >(VectorL<T> a, VectorL<T> b)
        {
            return a.CompareTo(b) > 0;
        }

        public T[] ToArray()
        {
            return _vec.ToArray();
        }

        public static VectorL<T> FromArray(T[] arr)
        {
            return new VectorL<T>(arr);
        }

        public override string ToString()
        {
            var s = new StringBuilder("(");
            foreach (var c in _vec)
            {
                s.Append($"{c}, ");
            }

            s.Remove(s.Length - 2, 2);
            s.Append(")");

            return s.ToString();
        }

        public int CompareTo(object? obj)
        {
            var vectorL = obj as VectorL<T>;
            if (vectorL != null)
                return CompareTo(vectorL);
            throw new ArgumentException("Types doesn't comparable");
        }

        public int CompareTo(VectorL<T> other)
        {
            var t = new VectorL<T>(this);
            t -= other;
            switch (t)
            {
                case VectorL<ComplexL> vector:
                    for (int i = 0; i < vector.Size; i++)
                    {
                        if (!(Math.Abs(vector._vec[i].Abs) > double.Epsilon)) continue;
                        if (vector._vec[i].Abs > 0) return 1;
                        return -1;
                    }
                    break;
                default:
                    for (int i = 0; i < t.Size; i++)
                    {
                        if (Math.Abs((dynamic)t._vec[i]) < double.Epsilon) continue;
                        if ((dynamic) t._vec[i] < 0) return -1;
                        return 1;
                    }
                    break;
            }

            return 0;
        }
        
        [Serializable]
        public class DifferentSizeException : Exception
        {
            //
            // For guidelines regarding the creation of new exception types, see
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
            // and
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
            //

            public DifferentSizeException()
            {
            }

            public DifferentSizeException(string message) : base(message)
            {
            }

            public DifferentSizeException(string message, Exception inner) : base(message, inner)
            {
            }

            protected DifferentSizeException(
                SerializationInfo info,
                StreamingContext context) : base(info, context)
            {
            }
        }
    }
}