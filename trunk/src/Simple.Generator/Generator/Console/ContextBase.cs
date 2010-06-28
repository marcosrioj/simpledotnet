﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Simple.Generator.Console
{
    public abstract class ContextBase : MarshalByRefObject, IContext
    {
        GeneratorResolver resolver = null;
        public string Name { get; private set; }
        protected abstract GeneratorResolver Configure(string name, bool defaultContext);
        private ILog logger = null;

        protected bool OverrideLogConfig { get { return true; } }

        public void Init(string name, bool defaultContext)
        {
            this.Name = name;
            this.logger = Simply.Do.Log(this);
            resolver = Configure(name, defaultContext);
            if (OverrideLogConfig)
                Simply.Do.Configure.Log4net().FromXmlString(DefaultConfig.Log4net);
        }


        public void Execute(string command)
        {
            try
            {
                logger.InfoFormat("Running on context: {0}", Name ?? "<default>");
                resolver.Resolve(command).Execute();
            }
            catch (GeneratorException e)
            {
                logger.Warn(e.Message);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
            }
        }
    }
}
