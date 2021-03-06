﻿namespace TensorSharp.Tests
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FlowTests
    {
        [TestMethod]
        public void CreateConstantSingleValue()
        {
            INode<int> value = Flow.Constant<int>(42);

            Assert.IsNotNull(value);
            Assert.AreEqual(42, value.GetValue());
        }

        [TestMethod]
        public void CreateConstantVector()
        {
            INode<int> value = Flow.Constant<int>(new int[] { 1, 2, 3 });

            Assert.IsNotNull(value);
            Assert.AreEqual(1, value.GetValue(0));
            Assert.AreEqual(2, value.GetValue(1));
            Assert.AreEqual(3, value.GetValue(2));

            Assert.AreEqual(1, value.Rank);
            Assert.IsTrue(value.Shape.SequenceEqual(new int[] { 3 }));
        }

        [TestMethod]
        public void CreateConstantMatrix()
        {
            INode<int> value = Flow.Constant<int>(new int[][] { new int[] { 1, 2, 3 }, new int[] { 4, 5, 6 } });

            Assert.AreEqual(2, value.Rank);
            Assert.IsNotNull(value.Shape);
            Assert.IsTrue(value.Shape.SequenceEqual(new int[] { 2, 3 }));

            Assert.AreEqual(1, value.GetValue(0, 0));
            Assert.AreEqual(2, value.GetValue(0, 1));
            Assert.AreEqual(3, value.GetValue(0, 2));

            Assert.AreEqual(4, value.GetValue(1, 0));
            Assert.AreEqual(5, value.GetValue(1, 1));
            Assert.AreEqual(6, value.GetValue(1, 2));
        }

        [TestMethod]
        public void AddSingleValues()
        {
            INode<int> left = new SingleValue<int>(1);
            INode<int> right = new SingleValue<int>(41);

            var add = Flow.Add(left, right);

            Assert.AreEqual(left.Rank, add.Rank);
            Assert.IsTrue(left.Shape.SequenceEqual(right.Shape));

            var result = add.Evaluate();

            Assert.IsNotNull(result);   
            Assert.AreEqual(left.Rank, result.Rank);
            Assert.IsTrue(left.Shape.SequenceEqual(result.Shape));
            Assert.AreEqual(42, result.GetValue());
        }
    }
}
