﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simple.Tests.Resources;
using NUnit.Framework;
using NHibernate;
using NHibernate.Validator.Engine;
using Simple.Validation;
using Simple.IO.Serialization;
using Simple.Entities;

namespace Simple.Tests.Data
{
    public class ValidationFixture : BaseDataFixture
    {
        [Test]
        public void TestValidateOnSave()
        {
            var c = Customer.ListAll(1).FirstOrDefault();
            Assert.IsNotNull(c);

            c.CompanyName = new string('0', 42);
            c.ContactName = new string('0', 42);

            Assert.Throws<ValidationException>(() => c.Save());
            MySimply.GetSession().Clear();
            Assert.Throws<ValidationException>(() => c.Update());
            MySimply.GetSession().Clear();
            Assert.Throws<ValidationException>(() => c.SaveOrUpdate());
            MySimply.GetSession().Clear();
            Assert.Throws<ValidationException>(() => c.Persist());
            MySimply.GetSession().Clear();
        }

        [Test]
        public void TestSerializeValidationException()
        {
            var c = Customer.ListAll(1).FirstOrDefault();
            Assert.IsNotNull(c);

            c.CompanyName = new string('0', 42);
            c.ContactName = new string('0', 42);

            var obj = new ValidationException(c.Validate().ToArray());
            var bytes = SimpleSerializer.Binary().Serialize(obj);
            var obj2 = SimpleSerializer.Binary().Deserialize(bytes) as ValidationException;

            Assert.IsNotNull(obj2.Items);
            Assert.AreEqual(obj.Items.Count, obj2.Items.Count);
        }

        protected void GenericTest(Func<Customer, IList<ValidationItem>> func, params string[] props)
        {
            var c = Customer.ListAll(1).FirstOrDefault();
            Assert.IsNotNull(c);

            c.CompanyName = new string('0', 42);
            c.ContactName = new string('0', 42);

            var list = func(c);
            Assert.AreEqual(props.Length, list.Count);

            foreach (string s in props)
                Assert.IsTrue(list.Any(x => x.PropertyName == s));
        }

        [Test]
        public void TestServiceValidation()
        {
            GenericTest(x => Customer.Service.Validate(x), "CompanyName", "ContactName");
        }

        [Test]
        public void TestDirectValidation()
        {
            GenericTest(x => x.Validate(), "CompanyName", "ContactName");
        }

        [Test]
        public void TestServicePropertyValidation()
        {
            GenericTest(x => Customer.Service.ValidateProperty("CompanyName", x.CompanyName),
                "CompanyName");
        }

        [Test]
        public void TestDirectPropertyValidation()
        {
            GenericTest(x => x.Validate(c => c.CompanyName), "CompanyName");
        }

        [Test]
        public void TestValidateInstanceByProperty()
        {
            Region r = new Region() { Description = new string('a', 101) };
            Assert.Throws<ValidationException>(() => r.Validate().AndThrow());
            new Region() { Description = new string('a', 99) }.Validate().AndThrow();

        }
       
    }
}

