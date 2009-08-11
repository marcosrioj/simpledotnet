﻿using System;
using log4net;
using Simple.Logging;
using System.Reflection;
using Simple.ConfigSource;

namespace Simple
{
    public class Simply : AggregateFactory<Simply>
    {
        public SimplyConfigure Configure
        {
            get
            {
                return SimplyConfigure.Do[ConfigKey];
            }
        }

        public SimplyRelease Release
        {
            get
            {
                return SimplyRelease.Do[ConfigKey];
            }
        }
    }

    public class SimplyConfigure : AggregateFactory<SimplyConfigure>
    {
    }

    public class SimplyRelease : AggregateFactory<SimplyRelease>
    {

    }
}
