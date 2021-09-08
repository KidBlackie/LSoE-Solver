using System;
using System.Text;
using Newtonsoft.Json;

namespace LSoE_Solver
{
    public abstract class MatrixBase<T> : IEquatable<MatrixBase<T>>, ICloneable
        where T : unmanaged, IComparable<T>, IComparable, IEquatable<T>, IConvertible
    {
        public int Rows { get; }
        public int Columns { get; }
        [JsonRequired]
        protected T[,] Matrix { get; set; }

        public T this[int row, int col]
        {
            get => Matrix[row, col];
            set => Matrix[row, col] = value;
        }
        protected MatrixBase(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Matrix = new T[rows, columns];
        }

        [JsonConstructor]
        protected MatrixBase([JsonProperty(nameof(Rows))]int rows, [JsonProperty(nameof(Columns))]int columns, [JsonProperty(nameof(Matrix))]T[,] inMatrix)
        {
            Rows = rows;
            Columns = columns;
            Matrix = inMatrix;
        }


        public T[] GetRow(int row)
        {
            T[] rowVals = new T[Columns];
            for (var col = 0; col < Columns; col++) rowVals[col] = Matrix[row, col];
            return rowVals;
        }

        public T[] GetCol(int col)
        {
            T[] colVals = new T[Columns];
            for (var row = 0; row < Columns; row++) colVals[col] = Matrix[col, row];
            return colVals;
        }

        public void SwapRows(int row1, int row2)
        {
            T tempVal;

            for (var i = 0; i < Columns; i++)
            {
                tempVal = this[row1, i];
                this[row1, i] = this[row2, i];
                this[row2, i] = tempVal;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append($"{"Matrix",-10}|");
            for (var i = 0; i < Columns; i++)
                sb.Append($"{i + 1,-9}|");
            sb.Append("\n");
            for (var i = 0; i < Rows; i++)
            {
                sb.Append($"Row: {i + 1,-5}|");
                for (var j = 0; j < Columns; j++) sb.Append($"{Matrix[i, j],-9}|");

                sb.Append("\n");
            }

            return sb.ToString();
        }

        public MatrixBase<T> GetReducedMatrix()
        {
            var mat = (MatrixBase<T>) Clone();
            mat.ReduceMatrix();
            return mat;
        }

        public TMatrix GetReducedMatrix<TMatrix>(MatrixBase<T> baseMat) where TMatrix : MatrixBase<T>
        {
            var res = GetReducedMatrix();
            return res as TMatrix ?? throw new InvalidCastException();
        }


        public abstract void ReduceMatrix();
        public abstract void MultiplyRowByConstant(int row, T factor);
        public abstract void AddRows(int source, int dest, T factor);
        public abstract void AddMultiples(int rows, int firstRow, int col);
        public abstract int GetPivot(int rows, int column, int start);

        public virtual object Clone()
        {
            var clone = (MatrixBase<T>) MemberwiseClone();
            clone.Matrix = Matrix.Clone() as T[,] ?? throw new InvalidCastException();
            return clone;
        }

        public bool Equals(MatrixBase<T>? other)
        {
            if (other == null) return false;
            for (var row = 0; row < Rows; row++)
            for (var col = 0; col < Columns; col++)
                if (!this[row, col].Equals(other[row, col]))
                    return false;

            return true;
        }
    }

    public class Matrixf : MatrixBase<float>
    {
        public Matrixf(int rows, int columns) : base(rows, columns)
        {
        }

        public Matrixf(int rows, int columns, float[,] inMatrix) : base(rows, columns, inMatrix)
        {
        }

        public override void ReduceMatrix()
        {
            var firstRow = 0;
            for (var col = 0; col < Columns; col++)
            {
                var nonZero = GetPivot(Rows, col, firstRow);
                if (nonZero == -1) continue;
                SwapRows(nonZero, firstRow);

                var factor = 1 / this[firstRow, col];
                MultiplyRowByConstant(firstRow, factor);

                AddMultiples(Rows, firstRow, col);

                firstRow++;
            }
        }

        public override void MultiplyRowByConstant(int row, float factor)
        {
            for (var i = 0; i < Columns; i++)
                this[row, i] *= factor;
        }

        public override void AddRows(int source, int dest, float factor)
        {
            for (var i = 0; i < Columns; i++) this[dest, i] += this[source, i] * factor;
        }

        public override void AddMultiples(int rows, int firstRow, int col)
        {
            for (var row = 0; row < rows; row++)
            {
                if (row == firstRow)
                    continue;
                if (this[row, col] != 0)
                {
                    var factor = -this[row, col] / this[firstRow, col];
                    AddRows(firstRow, row, factor);
                }
            }
        }

        public override int GetPivot(int rows, int column, int start)
        {
            for (var row = start; row < rows; row++)
                if (this[row, column] != 0)
                    return row;
            return -1;
        }
    }

    public class Matrixd : MatrixBase<double>
    {
        public Matrixd(int rows, int columns) : base(rows, columns)
        {
        }

        [JsonConstructor]
        public Matrixd([JsonProperty(nameof(Rows))] int rows, [JsonProperty(nameof(Columns))] int columns, [JsonProperty(nameof(Matrix))] double[,] inMatrix) : 
            base(rows, columns, inMatrix)
        {
        }

        public override void ReduceMatrix()
        {
            var firstRow = 0;
            for (var col = 0; col < Columns; col++)
            {
                var nonZero = GetPivot(Rows, col, firstRow);
                if (nonZero == -1) continue;
                SwapRows(nonZero, firstRow);

                var factor = 1 / this[firstRow, col];
                MultiplyRowByConstant(firstRow, factor);

                AddMultiples(Rows, firstRow, col);

                firstRow++;
            }
        }

        public override void MultiplyRowByConstant(int row, double factor)
        {
            for (var i = 0; i < Columns; i++)
                this[row, i] *= factor;
        }

        public override void AddRows(int source, int dest, double factor)
        {
            for (var i = 0; i < Columns; i++)
            {
                var amount = this[source, i] * factor;
                this[dest, i] += amount;
            }
        }

        public override void AddMultiples(int rows, int firstRow, int col)
        {
            for (var row = 0; row < rows; row++)
            {
                if (row == firstRow)
                    continue;
                if (this[row, col] != 0)
                {
                    var factor = -this[row, col] / this[firstRow, col];
                    AddRows(firstRow, row, factor);
                }
            }
        }

        public override int GetPivot(int rows, int column, int start)
        {
            for (var row = start; row < rows; row++)
            {
                var val = this[row, column];
                if (this[row, column] != 0) return row;
            }

            return -1;
        }
    }
}