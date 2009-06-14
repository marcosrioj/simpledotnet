﻿using System;
using System.Collections.Generic;
using System.Text;
using Simple.Config;
using NHibernate.Cfg;
using NHibernate;
using Simple.Threading;
using System.IO;
using System.Xml;
using Simple.Cache;
using log4net;
using Simple.Logging;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using System.Reflection;
using FluentNHibernate.Conventions.Helpers;
using Simple.DataAccess.Conventions;

namespace Simple.DataAccess
{
    public class MultiSessionFactory
    {
        public NHibernate.Cfg.Configuration DefaultConfig { get; set; }
        public ISessionFactory DefaultSessionFactory { get; set; }

        protected Dictionary<string, NHibernate.Cfg.Configuration> Configs { get; set; }
        protected Dictionary<string, ISessionFactory> SessionFactories { get; set; }

        protected ThreadData<MultiSessionFactory> MyData { get; set; }
        protected string ISESSION_KEY = typeof(ISession).GUID.ToString();
        protected DataConfigElement DataConfig { get; set; }
        protected BusinessElement BusinessConfig { get; set; }
        protected ILog Logger = MainLogger.Get<MultiSessionFactory>();

        public MultiSessionFactory(DataConfigElement dataConfig, BusinessElement businessConfig)
        {
            DataConfig = dataConfig;
            BusinessConfig = businessConfig;
            SessionFactories = new Dictionary<string, ISessionFactory>();
            MyData = new ThreadData<MultiSessionFactory>();
            Configs = new Dictionary<string, NHibernate.Cfg.Configuration>();
            InitializeSessionFactories();
        }

        protected ISession GetThreadSession(string factoryName)
        {
            return GetThreadSession(factoryName, true);
        }

        protected ISession GetThreadSession(string factoryName, bool createIfDoesNotExist)
        {
            lock (this)
            {
                string realName = factoryName ?? "";
                ISession session = (ISession)MyData[ISESSION_KEY + realName, 0];
                if (session == null || !((ISession)session).IsOpen)
                {
                    if (!createIfDoesNotExist) return null;

                    session = GetSessionFactory(factoryName).OpenSession();
                    MyData[ISESSION_KEY + realName, 0] = session;
                }
                return session;
            }
        }

        public NHibernate.Cfg.Configuration GetConfig()
        {
            return GetConfig(null);
        }

        public NHibernate.Cfg.Configuration GetConfig(string factoryName)
        {
            if (factoryName == null)
                return DefaultConfig;
            else
                return Configs[factoryName];
        }

        public void ClearThreadSessions()
        {
            lock (this)
            {
                foreach (ISession session in GetAllSessions())
                {
                    if (session.IsOpen && session.IsConnected) session.Clear();
                }
            }
        }

        public void ReleaseThreadSessions()
        {
            lock (this)
            {
                foreach (ISession session in GetAllSessions())
                {
                    if (session.IsOpen) session.Close();
                }
            }
        }

        protected IEnumerable<ISession> GetAllSessions()
        {
            ISession session = GetThreadSession(null, false);
            if (session != null) yield return session;
            foreach (string factoryName in SessionFactories.Keys)
            {
                session = GetThreadSession(factoryName, false);
                if (session != null)
                    yield return session;
            }
        }

        public ISessionFactory GetSessionFactory(string factoryName)
        {
            if (factoryName == null)
            {
                return DefaultSessionFactory;
            }
            else
            {
                try
                {
                    return SessionFactories[factoryName];
                }
                catch (KeyNotFoundException) { throw new InvalidOperationException("Invalid factory name: " + factoryName); }
            }
        }

        protected void ConfigureFromConfig(NHibernate.Cfg.Configuration config, SessionFactoryElement factoryElement)
        {
            if (factoryElement.NHibernateConfig != null)
            {
                MemoryStream stream = new MemoryStream();
                XmlWriter writer = XmlWriter.Create(stream);
                factoryElement.NHibernateConfig.LastXmlElement.WriteTo(writer);
                writer.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                XmlReader reader = XmlReader.Create(stream);
                config.Configure(reader);
                stream.Close();
            }
            else
            {
                config.Configure(FileCacher.GetBasedFile(factoryElement.ConfigFile));
            }
        }

        protected void ConfigureFluently(NHibernate.Cfg.Configuration config, SessionFactoryElement factoryElement)
        {
            Assembly serverAssembly = BusinessConfig.ServerAssembly.LoadAssembly();
            Assembly contractsAssembly = BusinessConfig.ContractsAssembly.LoadAssembly();

            config = Fluently.Configure(config).Mappings(
                m =>
                {
                    m.FluentMappings
                    .AddFromAssembly(serverAssembly)
                    .AddFromAssembly(contractsAssembly).ConventionDiscovery.Setup(
                        f =>
                        {
                            f.Add(LazyConvention.ForClass(false));
                            f.Add(LazyConvention.ForHasOne(false));
                            f.AddAssembly(contractsAssembly);
                            f.AddAssembly(serverAssembly);
                        });
                }).BuildConfiguration();
        }


        protected NHibernate.Cfg.Configuration GetFactoryConfiguration(SessionFactoryElement factoryElement)
        {
            NHibernate.Cfg.Configuration config = new NHibernate.Cfg.Configuration();
            ConfigureFromConfig(config, factoryElement);
            ConfigureFluently(config, factoryElement);
            return config;
        }

        protected void InitializeSessionFactories()
        {
            lock (this)
            {
                DefaultConfig = GetFactoryConfiguration(DataConfig.DefaultSessionFactory);
                DefaultSessionFactory = DefaultConfig.BuildSessionFactory();

                foreach (SessionFactoryElement factoryConfig in DataConfig.SessionFactories)
                {
                    NHibernate.Cfg.Configuration config = GetFactoryConfiguration(factoryConfig);
                    SessionFactories[factoryConfig.Name] = config.BuildSessionFactory();
                    Configs[factoryConfig.Name] = config;
                }
            }
        }


        public ISession GetSession()
        {
            return GetSession(null);
        }

        public ISession GetSession(string factoryName)
        {
            return GetSession(factoryName, false);
        }

        public ISession GetSession(bool forceNewSession)
        {
            return GetSession(null, forceNewSession);
        }

        public ISession GetSession(string factoryName, bool forceNewSession)
        {
            if (forceNewSession)
                return GetSessionFactory(factoryName).OpenSession();
            else
                return GetThreadSession(factoryName);
        }

    }
}