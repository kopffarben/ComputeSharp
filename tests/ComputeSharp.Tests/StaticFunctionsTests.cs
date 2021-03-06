﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComputeSharp.Tests
{
    [TestClass]
    [TestCategory("StaticFunctions")]
    public class StaticFunctionsTests
    {
        [TestMethod]
        public void FloatToFloatFunc()
        {
            using ReadWriteBuffer<float> buffer = Gpu.Default.AllocateReadWriteBuffer<float>(1);

            Func<float, float> square = StaticMethodsContainer.Square;

            Gpu.Default.For(1, id => buffer[0] = square(3));

            float[] result = buffer.GetData();

            Assert.IsTrue(MathF.Abs(result[0] - 9) < 0.0001f);
        }

        [TestMethod]
        public void InternalFloatToFloatFunc()
        {
            using ReadWriteBuffer<float> buffer = Gpu.Default.AllocateReadWriteBuffer<float>(1);

            Func<float, float> square = StaticMethodsContainer.InternalSquare;

            Gpu.Default.For(1, id => buffer[0] = square(3));

            float[] result = buffer.GetData();

            Assert.IsTrue(MathF.Abs(result[0] - 9) < 0.0001f);
        }

        [TestMethod]
        public void ChangingFuncsFromSameMethod()
        {
            using ReadWriteBuffer<float> buffer = Gpu.Default.AllocateReadWriteBuffer<float>(1);

            Func<float, float> func = StaticMethodsContainer.Square;

            Gpu.Default.For(1, id => buffer[0] = func(3));

            float[] result = buffer.GetData();

            Assert.IsTrue(MathF.Abs(result[0] - 9) < 0.0001f);

            func = StaticMethodsContainer.Negate;

            Gpu.Default.For(1, id => buffer[0] = func(3));

            result = buffer.GetData();

            Assert.IsTrue(MathF.Abs(result[0] + 3) < 0.0001f);
        }

        [TestMethod]
        public void ChangingFuncsFromExternalMethod()
        {
            MethodRunner(StaticMethodsContainer.Square, 3, 9);

            MethodRunner(StaticMethodsContainer.Negate, 3, -3);
        }

        private void MethodRunner(Func<float, float> func, float input, float expected)
        {
            using ReadWriteBuffer<float> buffer = Gpu.Default.AllocateReadWriteBuffer<float>(1);

            Gpu.Default.For(1, id => buffer[0] = func(input));

            float[] result = buffer.GetData();

            Assert.IsTrue(MathF.Abs(result[0] - expected) < 0.0001f);
        }

        public delegate float Squarer(float x);

        [TestMethod]
        public void CustomStaticDelegate()
        {
            using ReadWriteBuffer<float> buffer = Gpu.Default.AllocateReadWriteBuffer<float>(1);

            Squarer square = StaticMethodsContainer.Square;

            Gpu.Default.For(1, id => buffer[0] = square(3));

            float[] result = buffer.GetData();

            Assert.IsTrue(MathF.Abs(result[0] - 9) < 0.0001f);
        }

        [TestMethod]
        public void InlineStatelessDelegate()
        {
            using ReadWriteBuffer<float> buffer = Gpu.Default.AllocateReadWriteBuffer<float>(1);

            Func<float, float> f = x => x * x;

            Gpu.Default.For(1, id => buffer[0] = f(3));

            float[] result = buffer.GetData();

            Assert.IsTrue(MathF.Abs(result[0] - 9) < 0.0001f);
        }

        [TestMethod]
        public void StatelessLocalFunctionToDelegate()
        {
            using ReadWriteBuffer<float> buffer = Gpu.Default.AllocateReadWriteBuffer<float>(1);

            float Func(float x) => x * x;

            Func<float, float> f = Func;

            Gpu.Default.For(1, id => buffer[0] = f(3));

            float[] result = buffer.GetData();

            Assert.IsTrue(MathF.Abs(result[0] - 9) < 0.0001f);
        }

        [TestMethod]
        public void StatelessLocalFunction()
        {
            using ReadWriteBuffer<float> buffer = Gpu.Default.AllocateReadWriteBuffer<float>(1);

            float f(float x) => x * x;

            Gpu.Default.For(1, id => buffer[0] = f(3));

            float[] result = buffer.GetData();

            Assert.IsTrue(MathF.Abs(result[0] - 9) < 0.0001f);
        }

        public static float Square(float x) => x * x;

        [TestMethod]
        public void StaticMethodInSameClass()
        {
            using ReadWriteBuffer<float> buffer = Gpu.Default.AllocateReadWriteBuffer<float>(1);

            Gpu.Default.For(1, id => buffer[0] = Square(3));

            float[] result = buffer.GetData();

            Assert.IsTrue(MathF.Abs(result[0] - 9) < 0.0001f);
        }

        public static Func<float, float> SquareFunc { get; } = x => x * x;

        [TestMethod]
        public void StaticFunctionInSameClass()
        {
            using ReadWriteBuffer<float> buffer = Gpu.Default.AllocateReadWriteBuffer<float>(1);

            Gpu.Default.For(1, id => buffer[0] = SquareFunc(3));

            float[] result = buffer.GetData();

            Assert.IsTrue(MathF.Abs(result[0] - 9) < 0.0001f);
        }

        [TestMethod]
        public void StaticFunctionInExternalClass()
        {
            using ReadWriteBuffer<float> buffer = Gpu.Default.AllocateReadWriteBuffer<float>(1);

            Gpu.Default.For(1, id => buffer[0] = StaticPropertiesContainer.SquareFunc(3));

            float[] result = buffer.GetData();

            Assert.IsTrue(MathF.Abs(result[0] - 9) < 0.0001f);
        }

        [TestMethod]
        public void ReadmeSample()
        {
            float[] buffer = { 3 };
            using ReadWriteBuffer<float> xBuffer = Gpu.Default.AllocateReadWriteBuffer(buffer);

            static float Sigmoid(float x) => 1 / (1 + Hlsl.Exp(-x));

            void Kernel(ThreadIds id)
            {
                int offset = id.X + id.Y * 1;
                float pow = Hlsl.Pow(xBuffer[offset], 2); // 9
                xBuffer[offset] = Sigmoid(pow); // 0.9998766
            }

            Gpu.Default.For(1, Kernel);

            float[] result = xBuffer.GetData();

            Assert.IsTrue(MathF.Abs(result[0] - 0.9998766f) < 0.0001f);
        }
    }
}
