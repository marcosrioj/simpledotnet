﻿using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Linq.Expressions;
using Simple.Persistence;
using Simple.IO;

namespace Simple.Tests.Experiences
{
    [TestFixture]
    public class LambdaFixture
    {
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestSerializeExpression()
        {
            Expression<Predicate<int>> lambda = 
                x => x == 2 && x == 3;

            XmlHelper.QuickSerialize(lambda);
        }
    }
}
