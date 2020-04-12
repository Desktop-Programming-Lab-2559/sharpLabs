using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace lab10
{
    public class Matrix : ICloneable, IEnumerable, IEnumerator
    {
        private List<List<double>> _matrix;
        private int _enumeratorIndex = 0;

        public Matrix()
        {
            _matrix = new List<List<double>>(1);
            _matrix.Add(new List<double>(1) {0});
        }

        public Matrix(int size)
        {
            if (size < 1) throw new SizeException();
            _matrix = new List<List<double>>(size);
            for (int i = 0; i < size; i++)
            {
                _matrix.Add(new List<double>(size));
                for (int j = 0; j < size; j++)
                {
                    _matrix[i].Add(default);
                }
            }
        }

        public Matrix(params double[] m)
        {
            int size = (int) Math.Sqrt(m.Length);
            
            _matrix = new List<List<double>>(size);
            for (int i = 0; i < size; i++)
            {
                _matrix.Add(new List<double>(size));
                for (int j = 0; j < size; j++)
                {
                    _matrix[i].Add(default);
                }
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    _matrix[i][j] = m[i * size + j];
                }
            }
        }

        public Matrix(Matrix matrix)
        {
            var size = matrix.Size;
            _matrix = new List<List<double>>(size);
            for (int i = 0; i < size; i++)
            {
                _matrix.Add(new List<double>(size));
                for (int j = 0; j < size; j++)
                {
                    _matrix[i].Add(matrix._matrix[i][j]);
                }
            }
        }

        public int Size => _matrix.Count;
        
        public object Clone()
        {
            return new Matrix(this);
        }

        private void Add(Matrix matrix)
        {
            if (Size != matrix.Size) Matrix.Resize(this, matrix);

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _matrix[i][j] += matrix._matrix[i][j];
                }
            }
        }

        private static void Resize(Matrix a, Matrix b)
        {
            if (a.Size == b.Size) return;

            if (a.Size < b.Size)
            {
                var t = a;
                a = b;
                b = t;
            }

            while (b._matrix.Count != a._matrix.Count)
            {
                b._matrix.Add(new List<double>(a.Size));
                b._matrix[^1] = new List<double>(a.Size);
            }

            for (int i = 0; i < b.Size; i++)
            {
                while (b._matrix[i].Count != a.Size)
                {
                    b._matrix[i].Add(0);
                }
            }
        }

        private void Sub(Matrix matrix)
        {
            if (Size != matrix.Size) Resize(this, matrix);

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _matrix[i][j] -= matrix._matrix[i][j];
                }
            }
        }

        public void Mul(int k)
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _matrix[i][j] *= k;
                }
            }
        }

        private void Mul(double d)
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _matrix[i][j] *= d;
                }
            }
        }

        private void Mul(Matrix m)
        {
            if (Size != m.Size) Resize(this, m);
            Matrix t = new Matrix(this);
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < Size; k++)
                    {
                        sum += t._matrix[i][k] * m._matrix[k][j];
                    }
                    _matrix[i][j] = sum;
                }
            }
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            foreach (var list in _matrix)
            {
                foreach (var num in list)
                {
                    s.Append($"{num} ");
                }

                s.Append('\n');
            }

            return s.Remove(s.Length - 1, 1).ToString();
        }

        public string ToLine()
        {
            var s = new StringBuilder();
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    s.Append($"{_matrix[i][j]} ");
                }

                s.Append("// ");
            }

            s.Remove(s.Length - 4, 4);

            return s.ToString();
        }

        static public Matrix Sum(Matrix a, Matrix b)
        {
            Matrix t = new Matrix(a);
            t.Add(b);
            return t;
        }
        
        static public Matrix Subtract(Matrix a, Matrix b)
        {
            Matrix t = new Matrix(a);
            t.Sub(b);
            return t;
        }
        
        static public Matrix Multiply(Matrix a, Matrix b)
        {
            Matrix t = new Matrix(a);
            t.Mul(b);
            return t;
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            return Sum(a, b);
        }
        
        public static Matrix operator -(Matrix a, Matrix b)
        {
            return Subtract(a, b);
        }
        
        public static Matrix operator *(Matrix a, Matrix b)
        {
            return Multiply(a, b);
        }

        public static Matrix operator *(Matrix a, double n)
        {
            return Multiply(a, n);
        }

        public static Matrix operator /(Matrix a, Matrix b)
        {
            return a * Inverse(b);
        }

        private static Matrix Multiply(Matrix matrix, double d)
        {
            var res = new Matrix(matrix);
            res.Mul(d);
            return res;
        }

        public Matrix TransposeNew()
        {
            var m = new Matrix(this);
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    m._matrix[i][j] = _matrix[j][i];
                }
            }
            return m;
        }
        
        public void TransposeThis()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = i; j < Size; j++)
                {
                    var t = _matrix[j][i];
                    _matrix[j][i] = _matrix[i][j];
                    _matrix[i][j] = t;
                }
            }
        }

        private void Swap(int first, int second)
        {
            List<double> t = new List<double>(_matrix[first]);
            _matrix[first] = _matrix[second];
            _matrix[second] = t;
        }

        public double Determinant()
        {
            // bool isZero = false;
            ToUpperTriangleForm();
            double det = 1;
            for (int i = 0; i < Size; i++)
            {
                det *= _matrix[i][i];
            }

            return det;
        }

        private int FindNotZero(int index)
        {
            for (int i = index + 1; i < Size; i++)
            {
                if (Math.Abs(_matrix[index][i]) > double.Epsilon) return i;
            }

            return index;
        }

        private bool IsZeroColumn(int column)
        {
            var res = true;
            for (int j = 0; j < Size; j++)
            {
                res &= Math.Abs(_matrix[j][column]) < double.Epsilon;
            }

            return res;
        }

        private bool IsZeroRow(int row)
        {
            var res = true;
            for (int i = 0; i < Size; i++)
            {
                res &= Math.Abs(_matrix[row][i]) < double.Epsilon;
            }

            return res;
        }

        public void ToUpperTriangleForm()
        {
            for (int i = 0; i < Size; i++)
            {
                // if (IsZeroColumn(i)) return 0;
                if (Math.Abs(_matrix[i][i]) < double.Epsilon)
                {
                    var index = FindNotZero(i);
                    if (i != index) Swap(i, FindNotZero(i));
                }
                
                for (int j = i; j < Size - 1; j++)
                {
                    double coefficient = _matrix[i + 1][j] / _matrix[i][j];
                    for (int k = j; k < Size; k++)
                    {
                        _matrix[j + 1][k] -= coefficient * _matrix[i][k];
                    }
                }
            }
        }

        public static Matrix Inverse(Matrix m)
        {
            if (Math.Abs(m.Determinant()) < double.Epsilon) throw new InverseMatrixDoesntExistException(); 
            var inverse = new Matrix(m.Size);

            for (int i = 0; i < inverse.Size; i++)
            {
                for (int j = 0; j < m.Size; j++)
                {
                    inverse._matrix[i][j] = m.Pop(i, j).Determinant();
                }
            }

            inverse *= 1 / m.Determinant();
            
            return inverse.TransposeNew();
        }

        public Matrix Pop(int row, int column)
        {
            Matrix less = new Matrix(this);
            for (int i = 0; i < Size; i++)
            {
                less._matrix[i].RemoveAt(column);
            }
            less._matrix.RemoveAt(row);
            return less;
        }
        
        public bool IsZero()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (Math.Abs(_matrix[i][j]) > double.Epsilon)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool operator <(Matrix a, Matrix b) => a.Determinant() < b.Determinant();

        public static bool operator >(Matrix a, Matrix b) => a.Determinant() > b.Determinant();

        public static bool operator ==(Matrix a, Matrix b)
        {
            if (a.Size != b.Size) return false;
            
            for (int i = 0; i < a.Size; i++)
            {
                for (int j = 0; j < a.Size; j++)
                {
                    if (Math.Abs(a._matrix[i][j] - b._matrix[i][j]) > double.Epsilon) return false;
                }
            }

            return true;
        }

        public static bool operator !=(Matrix a, Matrix b)
        {
            if (a.Size != b.Size) return true;
            
            for (int i = 0; i < a.Size; i++)
            {
                for (int j = 0; j < a.Size; j++)
                {
                    if (Math.Abs(a._matrix[i][j] - b._matrix[i][j]) > double.Epsilon) return true;
                }
            }

            return false;
        }

        public IEnumerator GetEnumerator()
        {
            _enumeratorIndex = -1;
            return this;
        }

        public bool MoveNext()
        {
            return ++_enumeratorIndex < Size * Size;
        }

        public void Reset()
        {
            _enumeratorIndex = 0;
        }

        object IEnumerator.Current => _matrix[_enumeratorIndex / Size][_enumeratorIndex % Size];
        
        //                                EXCEPTIONS
        
        [Serializable]
        public class SizeException : Exception
        {
            //
            // For guidelines regarding the creation of new exception types, see
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
            // and
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
            //

            public SizeException()
            {
            }

            public SizeException(string message) : base(message)
            {
            }

            public SizeException(string message, Exception inner) : base(message, inner)
            {
            }

            protected SizeException(
                SerializationInfo info,
                StreamingContext context) : base(info, context)
            {
            }
        }
        
        [Serializable]
        public class InverseMatrixDoesntExistException : Exception
        {
            //
            // For guidelines regarding the creation of new exception types, see
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
            // and
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
            //

            public InverseMatrixDoesntExistException()
            {
            }

            public InverseMatrixDoesntExistException(string message) : base(message)
            {
            }

            public InverseMatrixDoesntExistException(string message, Exception inner) : base(message, inner)
            {
            }

            protected InverseMatrixDoesntExistException(
                SerializationInfo info,
                StreamingContext context) : base(info, context)
            {
            }
        }
    }
}