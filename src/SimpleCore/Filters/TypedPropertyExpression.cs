﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Simple.Filters
{
    [Serializable]
    public class TypedPropertyExpression<T> : PropertyExpression
    {
        [DataMember]
        public T Value { get; set; }

        public TypedPropertyExpression(PropertyName propertyName, T value)
            : base(propertyName)
        {
            this.Value = value;
        }
    }
}
