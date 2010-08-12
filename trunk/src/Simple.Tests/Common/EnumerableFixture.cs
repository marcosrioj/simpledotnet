﻿using System.Linq;
using NUnit.Framework;
using SharpTestsEx;
using System.Text;

namespace Simple.Tests.Common
{
    public class EnumerableFixture
    {
        [Test]
        public void CanZipTwoSameLengthSequences()
        {
            var seq1 = new[] { 1, 2, 3 };
            var seq2 = new[] { "um", "dois", "três" };
            var seq3 = seq1.Zip(seq2, (x, y) => x + y).ToList();

            CollectionAssert.AreEqual(new[] { "1um", "2dois", "3três" }, seq3);
        }

        [Test]
        public void CanZipTwoDifferentLengthSequences()
        {
            var seq1 = new[] { 1, 2, 3, 4 };
            var seq2 = new[] { "um", "dois", "três" };
            var seq3 = seq1.Zip(seq2, (x, y) => x + y).ToList();

            CollectionAssert.AreEqual(new[] { "1um", "2dois", "3três" }, seq3);
        }

        [Test]
        public void CanZipThreeSequences()
        {
            var seq1 = new[] { 1, 2, 3, 4 };
            var seq2 = new[] { 1, 2 };
            var seq3 = new[] { "um", "dois", "três" };

            var seq4 = seq1.Zip(seq2, (x, y) => x * y).Zip(seq3, (x, y) => x + y).ToList();

            CollectionAssert.AreEqual(new[] { "1um", "4dois" }, seq4);
        }

        [Test]
        public void CanAggregateJoinArrayOfStrings()
        {
            var seq = new[] { "um", "dois", "três" };
            var str = seq.AggregateJoin((x, y) => x + "," + y);

            CollectionAssert.AreEqual("um,dois,três", str);
        }

        [Test]
        public void CanStringJoinArrayOfStrings()
        {
            var seq = new[] { "um", "dois", "três" };
            var str = seq.StringJoin();

            str.Should().Be("umdoistrês");
        }

        [Test]
        public void CanStringJoinArrayOfStringsWithCommaSeparator()
        {
            var seq = new[] { "um", "dois", "três" };
            var str = seq.StringJoin(",");

            str.Should().Be("um,dois,três");
        
        }

        [Test]
        public void CanEagerForeachWithEmptyCollection()
        {
            var seq = new string[0];

            var builder = new StringBuilder();
            seq.EagerForeach(x => builder.Append(x));

            builder.ToString().Should().Be("");
        }


        [Test]
        public void CanEagerForeachWithoutBetween()
        {
            var seq = new[] { "um", "dois", "três" };
            
            var builder = new StringBuilder();
            seq.EagerForeach(x => builder.Append(x));

            builder.ToString().Should().Be("umdoistrês");
        }

        [Test]
        public void CanEagerForeachWithBetweenInEmptyCollection()
        {
            var seq = new string[0] { };

            var builder = new StringBuilder();
            seq.EagerForeach(x => builder.Append(x), x => builder.Append(","));

            builder.ToString().Should().Be("");
        }

        [Test]
        public void CanEagerForeachWithBetween()
        {
            var seq = new[] { "um", "dois", "três" };

            var builder = new StringBuilder();
            seq.EagerForeach(x => builder.Append(x), x=>builder.Append(","));

            builder.ToString().Should().Be("um,dois,três");
        }

        [Test]
        public void CanStringJoinArrayOfIntsWithCommaSeparator()
        {
            var seq = new[] { 1, 2, 3 };
            var str = seq.StringJoin(",");

            str.Should().Be("1,2,3");
        }
    }
}
