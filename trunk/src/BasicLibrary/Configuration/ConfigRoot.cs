﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Reflection;
using System.Diagnostics;
using BasicLibrary.Common;
using System.Collections.Specialized;

namespace BasicLibrary.Configuration
{
    public class ConfigRoot<T> : ConfigElement where T : IConfigElement, new()
    {
        public static bool IsLoading
        {
            get
            {
                return FileConfigProvider<T>.Instance.IsLoading;
            }
        }

        [LocalizationProviderIgnore]
        public static T Get()
        {
            return FileConfigProvider<T>.Instance.Get();
        }

        [LocalizationProviderIgnore]
        public static T Get(string location)
        {
            return FileConfigProvider<T>.Instance.Get();
        }
    }
}