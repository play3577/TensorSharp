﻿namespace TensorSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Vector<T> : INode<T>
    {
        private T[] values;
        private int[] shape;

        public Vector(int length)
        {
            this.values = new T[length];
            this.shape = new int[] { length };
        }

        public Vector(T[] values)
        {
            this.values = values;
            this.shape = new int[] { values.Length };
        }

        public int Rank { get { return 1; } }

        public int[] Shape { get { return this.shape; } }

        public T GetValue(params int[] coordinates)
        {
            return this.values[coordinates[0]];
        }

        public void SetValue(T value, params int[] coordinates)
        {
            this.values[coordinates[0]] = value;
        }

        public INode<T> Evaluate()
        {
            return this;
        }
    }
}
