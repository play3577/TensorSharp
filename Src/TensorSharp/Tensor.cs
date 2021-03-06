﻿namespace TensorSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TensorSharp.Operations;

    public class Tensor<T>
    {
        private int[] dimensions;
        private T[] values;
        private int size;
        private bool seal;

        public Tensor(params int[] dimensions)
        {
            this.dimensions = (int[])dimensions.Clone();

            this.size = 1;

            for (int k = 0; k < dimensions.Length; k++)
                this.size *= dimensions[k];
        }

        public int Rank { get { return this.dimensions.Length; } }

        public int[] Shape { get { return this.dimensions; } }

        public bool Sealed { get { return this.seal; } }

        public Tensor<T> CloneWithNewValues(T[] newValues)
        {
            Tensor<T> tensor = new Tensor<T>(this.dimensions);
            tensor.values = newValues;
            return tensor;
        }

        public int GetDimensionLength(int ndim)
        {
            return this.dimensions[ndim];
        }

        public T GetValue(params int[] coordinates)
        {
            if (coordinates.Any(c => c < 0))
                throw new TensorException("Invalid coordinate");

            if (this.values == null)
                return default(T);

            return this.values[this.CalculatePosition(coordinates)];
        }

        public void SetValue(T value, params int[] coordinates)
        {
            if (coordinates.Any(c => c < 0))
                throw new TensorException("Invalid coordinate");

            if (this.seal)
                throw new TensorException("Tensor is sealed");

            if (this.values == null)
                this.values = new T[this.size];

            this.values[this.CalculatePosition(coordinates)] = value;
        }

        public void Seal()
        {
            this.seal = true;
        }

        public Tensor<S> Add<S>(Tensor<T> tensor)
        {
            if (typeof(T) == typeof(int) && typeof(S) == typeof(int))
            {
                var operation = new AddIntegerIntegerOperation();

                Tensor<int> result = operation.Evaluate(this as Tensor<int>, tensor as Tensor<int>);

                return result as Tensor<S>;
            }

            if (typeof(T) == typeof(double) && typeof(S) == typeof(double))
            {
                var operation = new AddDoubleDoubleOperation();

                Tensor<double> result = operation.Evaluate(this as Tensor<double>, tensor as Tensor<double>);

                return result as Tensor<S>;
            }

            return null;
        }

        public Tensor<S> Subtract<S>(Tensor<T> tensor)
        {
            if (typeof(T) == typeof(int) && typeof(S) == typeof(int))
            {
                var operation = new SubtractIntegerIntegerOperation();

                Tensor<int> result = operation.Evaluate(this as Tensor<int>, tensor as Tensor<int>);

                return result as Tensor<S>;
            }

            if (typeof(T) == typeof(double) && typeof(S) == typeof(double))
            {
                var operation = new SubtractDoubleDoubleOperation();

                Tensor<double> result = operation.Evaluate(this as Tensor<double>, tensor as Tensor<double>);

                return result as Tensor<S>;
            }

            return null;
        }

        public Tensor<S> Multiply<S>(Tensor<T> tensor)
        {
            if (typeof(T) == typeof(int) && typeof(S) == typeof(int))
            {
                var operation = new MultiplyIntegerIntegerOperation();

                Tensor<int> result = operation.Evaluate(this as Tensor<int>, tensor as Tensor<int>);

                return result as Tensor<S>;
            }

            if (typeof(T) == typeof(double) && typeof(S) == typeof(double))
            {
                var operation = new MultiplyDoubleDoubleOperation();

                Tensor<double> result = operation.Evaluate(this as Tensor<double>, tensor as Tensor<double>);

                return result as Tensor<S>;
            }

            return null;
        }

        public Tensor<S> Divide<S>(Tensor<T> tensor)
        {
            if (typeof(T) == typeof(int) && typeof(S) == typeof(int))
            {
                var operation = new DivideIntegerIntegerOperation();

                Tensor<int> result = operation.Evaluate(this as Tensor<int>, tensor as Tensor<int>);

                return result as Tensor<S>;
            }

            if (typeof(T) == typeof(double) && typeof(S) == typeof(double))
            {
                var operation = new DivideDoubleDoubleOperation();

                Tensor<double> result = operation.Evaluate(this as Tensor<double>, tensor as Tensor<double>);

                return result as Tensor<S>;
            }

            return null;
        }

        public Tensor<T> Negate()
        {
            if (typeof(T) == typeof(int))
            {
                var operation = new NegateIntegerOperation();

                Tensor<int> result = operation.Evaluate(this as Tensor<int>);

                return result as Tensor<T>;
            }

            if (typeof(T) == typeof(double))
            {
                var operation = new NegateDoubleOperation();

                Tensor<double> result = operation.Evaluate(this as Tensor<double>);

                return result as Tensor<T>;
            }

            return null;
        }

        internal T[] GetValues()
        {
            return this.values;
        }

        private int CalculatePosition(int[] coordinates)
        {
            int multiplier = 1;
            int position = 0;

            for (int k = 0; k < this.dimensions.Length; k++)
            {
                position += multiplier * coordinates[k];
                multiplier *= this.dimensions[k];
            }
            
            return position;
        }
    }
}
