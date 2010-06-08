﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Simple.Tests.Metadata.Runs;
using Simple.IO.Serialization;
using System.IO;

namespace Simple.Tests.Metadata
{

    [TestFixture]
    public class RunAndRollBackFixture
    {
        public IList<DatabasesXml.Entry> TestCases()
        {
            var xml = SimpleSerializer.Xml<DatabasesXml>().DeserializeTypedFromString(Database.Databases);
            return xml.Entries;
        }

        [TestCaseSource("TestCases")]
        public void AllTypesTest(DatabasesXml.Entry entry)
        {
            new AllTypesTest(entry).ExecuteAll();
        }

        [TestCaseSource("TestCases")]
        public void ChangeColumnTest(DatabasesXml.Entry entry)
        {
            new ChangeColumnTest(entry).ExecuteAll();
        }

        [TestCaseSource("TestCases")]
        public void ChangeTableTest(DatabasesXml.Entry entry)
        {
            new ChangeTableTest(entry).ExecuteAll();
        }

        [TestCaseSource("TestCases")]
        public void DoubleForeignKeyTest(DatabasesXml.Entry entry)
        {
            new DoubleForeignKeyTest(entry).ExecuteAll();
        }

        [TestCaseSource("TestCases")]
        public void SimpleTableTest(DatabasesXml.Entry entry)
        {
            new SimpleTableTest(entry).ExecuteAll();
        }
    }
}