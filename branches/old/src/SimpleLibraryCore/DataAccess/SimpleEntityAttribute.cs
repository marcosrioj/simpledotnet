﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleLibrary.DataAccess
{
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class SimpleEntityAttribute : Attribute
    {
        public SimpleEntityAttribute()
        { }
    }
}