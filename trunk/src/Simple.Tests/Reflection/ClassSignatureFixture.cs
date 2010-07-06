﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simple.Reflection;
using NUnit.Framework;

namespace Simple.Tests.Reflection
{
    public class ClassSignatureFixture
    {
        [Test]
        public void SingleImplementation()
        {
            var sig = new ClassSignature(typeof(SingleImplementationClass));
            Assert.AreEqual("ClassSignatureFixture.ITest1", sig.MakeImplementingSignature());
        }
        [Test]
        public void DoubleImplementation()
        {
            var sig = new ClassSignature(typeof(DoubleImplementationClass));
            Assert.AreEqual("ClassSignatureFixture.ITest1, ClassSignatureFixture.ITest2", sig.MakeImplementingSignature());
        }

        [Test]
        public void GenericImplementation()
        {
            var sig = new ClassSignature(typeof(GenericImplementationClass));
            Assert.AreEqual("ClassSignatureFixture.ITest1, ClassSignatureFixture.ITest2, ClassSignatureFixture.ITest3<Int32>", sig.MakeImplementingSignature());
        }

        [Test]
        public void UnresolvedGenericImplementation()
        {
            var sig = new ClassSignature(typeof(UnresolvedGenericImplementationClass<>));
            Assert.AreEqual("ClassSignatureFixture.ITest3<T>", sig.MakeImplementingSignature());
        }
        [Test]
        public void ResolvedGenericImplementation()
        {
            var sig = new ClassSignature(typeof(UnresolvedGenericImplementationClass<String>));
            Assert.AreEqual("ClassSignatureFixture.ITest3<String>", sig.MakeImplementingSignature());
        }

        [Test]
        public void ConstrainedGenericImplementation()
        {
            var sig = new ClassSignature(typeof(ConstrainedGenericClass<>));
            Assert.AreEqual("ClassSignatureFixture.ITest3<T> where T : struct, IConvertible", sig.MakeImplementingSignature());
        }

        [Test]
        public void DoubleConstrainedGenericImplementation()
        {
            var sig = new ClassSignature(typeof(DoubleConstrainedGenericClass<>));
            Assert.AreEqual("ClassSignatureFixture.ITest3<T>, ClassSignatureFixture.ITest4<T> where T : struct, IConvertible", sig.MakeImplementingSignature());
        }

        [Test]
        public void ResolvedConstrainedGenericImplementation()
        {
            var sig = new ClassSignature(typeof(DoubleConstrainedGenericClass<int>));
            Assert.AreEqual("ClassSignatureFixture.ITest3<Int32>, ClassSignatureFixture.ITest4<Int32>", sig.MakeImplementingSignature());
        }

        class SingleImplementationClass : ITest1 { }
        class DoubleImplementationClass : ITest2, ITest1 { }
        class GenericImplementationClass : ITest3<int>, ITest2, ITest1 { }
        class UnresolvedGenericImplementationClass<T> : ITest3<T> { }
        class ConstrainedGenericClass<T> : ITest3<T>
            where T : struct, IConvertible
        { }

        class DoubleConstrainedGenericClass<T> : ITest3<T>, ITest4<T>
            where T : struct, IConvertible
        { }


        interface ITest1 { }
        interface ITest2 { }
        interface ITest3<T> { }
        interface ITest4<T> where T : struct { }
    }
}