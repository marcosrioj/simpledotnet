﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLibrary.Filters;
using System.Runtime.Serialization;

namespace SimpleLibrary.Filters
{
    [DataContract]
    public abstract class Expression : Filter
    {
        public static NotExpression NotEquals(string propertyName, object value)
        {
            return Equals(propertyName, value).Negate();
        }
        public static SimpleExpression Equals(string propertyName, object value)
        {
            return new SimpleExpression(propertyName, value, SimpleExpression.EqualsExpression);
        }
        public static SimpleExpression GreaterThan(string propertyName, object value)
        {
            return new SimpleExpression(propertyName, value, SimpleExpression.GreaterThanExpression);
        }
        public static SimpleExpression GreaterThanOrEquals(string propertyName, object value)
        {
            return new SimpleExpression(propertyName, value, SimpleExpression.GreaterThanOrEqualsExpression);
        }
        public static SimpleExpression LesserThan(string propertyName, object value)
        {
            return new SimpleExpression(propertyName, value, SimpleExpression.LesserThanExpression);
        }
        public static SimpleExpression LesserThanOrEquals(string propertyName, object value)
        {
            return new SimpleExpression(propertyName, value, SimpleExpression.LesserThanOrEqualsExpression);
        }

        public static LikeExpression Like(string propertyName, string value)
        {
            return Like(propertyName, value, LikeExpression.DefaultIgnoreCase);
        }


        public static LikeExpression Like(string propertyName, string value, bool ignoreCase)
        {
            return new LikeExpression(propertyName, value, ignoreCase);
        }

        public static LikeExpression Contains(string propertyName, string value)
        {
            return Contains(propertyName, value, LikeExpression.DefaultIgnoreCase);
        }

        public static LikeExpression Contains(string propertyName, string value, bool ignoreCase)
        {
            return Like(propertyName, "%" + value + "%", ignoreCase);
        }

        public static LikeExpression StartsWith(string propertyName, string value)
        {
            return StartsWith(propertyName, value, LikeExpression.DefaultIgnoreCase);
        }

        public static LikeExpression StartsWith(string propertyName, string value, bool ignoreCase)
        {
            return Like(propertyName, value + "%", ignoreCase);
        }
        public static LikeExpression EndsWith(string propertyName, string value)
        {
            return EndsWith(propertyName, value, LikeExpression.DefaultIgnoreCase);
        }


        public static LikeExpression EndsWith(string propertyName, string value, bool ignoreCase)
        {
            return Like(propertyName, "%" + value, ignoreCase);
        }
        public static BetweenExpression Between(string propertyName, object lo, object hi)
        {
            return new BetweenExpression(propertyName, lo, hi);
        }
        public static BooleanExpression Boolean(bool value)
        {
            return new BooleanExpression(value);
        }
        public static ExampleFilter Example(object entity)
        {
            return new ExampleFilter(entity);
        }

    }
}