﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simple.DynamicProxy;
using System.Reflection;
using Simple.Reflection;

namespace Simple.Services
{
    public interface IServiceCommonProvider
    {
        object ProxyObject(object obj, IInterceptor intercept);
        ICallHeadersHandler HeaderHandler { get; }
    }

    public interface IServiceClientProvider : IServiceCommonProvider
    {
        object Create(Type type);
    }

    public class NullServiceClientProvider : IServiceClientProvider
    {
        private object _cache = null;

        #region IServiceFactory Members

        public object Create(Type contract)
        {
            lock (this)
            {
                if (_cache == null)
                {
                    _cache = DynamicProxyFactory.Instance.CreateProxy(null, (o, m, p) =>
                    {
                        Type retType = ((MethodInfo)m).ReturnType;
                        return TypesHelper.GetBoxedDefaultInstance(retType);
                    });
                }

                return _cache;
            }
        }

        public object ProxyObject(object obj, IInterceptor intercept)
        {
            return obj;
        }


        #endregion

        #region IServiceCommonProvider Members


        public ICallHeadersHandler HeaderHandler
        {
            get { return new NullCallHeadersHandler(); }
        }

        #endregion
    }
}
