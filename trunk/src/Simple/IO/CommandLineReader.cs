﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using Simple.Reflection;

namespace Simple.IO
{
    public class CommandLineReader
    {
        private StringDictionary parameters;

        public CommandLineReader() : this(Environment.GetCommandLineArgs()) { }

        public CommandLineReader(params string[] args)
        {
            parameters = new StringDictionary();
            var spliter = new Regex(@"^-{1,2}|^/|=|:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            var remover = new Regex(@"^['""]?(.*?)['""]?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            string parameter = null;
            string[] parts;

            // Valid parameters forms:
            // {-,/,--}param{ ,=,:}((",')value(",'))
            // Examples: 
            // -param1 value1 --param2 /param3:"Test-:-work" 
            //   /param4=happy -param5 '--=nice=--'

            foreach (string txt in args)
            {
                // Look for new parameters (-,/ or --) and a
                // possible enclosed value (=,:)
                parts = spliter.Split(txt, 3);

                switch (parts.Length)
                {
                    // Found a value (for the last parameter 
                    // found (space separator))

                    case 1:
                        if (parameter != null)
                        {
                            if (!parameters.ContainsKey(parameter))
                            {
                                parts[0] = remover.Replace(parts[0], "$1");
                                parameters.Add(parameter, parts[0]);
                            }
                            parameter = null;
                        }
                        break;
                    case 2:
                        if (parameter != null)
                        {
                            if (!parameters.ContainsKey(parameter))
                                parameters.Add(parameter, "true");
                        }
                        parameter = parts[1];
                        break;
                    case 3:
                        if (parameter != null)
                        {
                            if (!parameters.ContainsKey(parameter))
                                parameters.Add(parameter, "true");
                        }

                        parameter = parts[1];

                        if (!parameters.ContainsKey(parameter))
                        {
                            parts[2] = remover.Replace(parts[2], "$1");
                            parameters.Add(parameter, parts[2]);
                        }

                        parameter = null;
                        break;
                }
            }

            if (parameter != null)
            {
                if (!parameters.ContainsKey(parameter))
                    parameters.Add(parameter, "true");
            }
        }

        public string Get(string name)
        {
            return Get(name, string.Empty);
        }

        public T Get<T>(string name)
        {
            return Get(name, default(T));
        }

        public T Get<T>(string name, T defaultValue)
        {
            Type realType = (typeof(T).IsGenericType && typeof(Nullable<>) == typeof(T).GetGenericTypeDefinition()) 
                ? typeof(T).GetGenericArguments()[0] 
                : typeof(T);

            if (parameters.ContainsKey(name))
                try
                {
                    return (T)Convert.ChangeType(parameters[name], realType);
                }
                catch (FormatException)
                {
                    return defaultValue;
                }
            else
                return defaultValue;
        }
    }
}