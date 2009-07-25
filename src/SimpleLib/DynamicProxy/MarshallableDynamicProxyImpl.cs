﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Reflection;

namespace Simple.DynamicProxy
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class MarshallableDynamicProxyImpl : RealProxy, IDynamicProxy
    {
        public string URI { get; protected set; }
        public MarshalByRefObject ProxyTargetTyped { get; protected set; }
        public object ProxyTarget { get { return ProxyTargetTyped; } }
        public InvocationDelegate InvocationHandler { get; set; }

        public MarshallableDynamicProxyImpl(MarshalByRefObject targetObject, InvocationDelegate invoker)
            : this(targetObject.GetType(), targetObject, invoker)
        {
        }


        public MarshallableDynamicProxyImpl(Type type1, MarshalByRefObject targetObject, InvocationDelegate invoker)
            : base(type1)
        {
            ProxyTargetTyped = targetObject;
            InvocationHandler = invoker;

            ObjRef myObjRef = RemotingServices.Marshal(ProxyTargetTyped);
            URI = myObjRef.URI;
        }

        public override IMessage Invoke(IMessage message)
        {
            if (message is IConstructionCallMessage)
            {
                IConstructionReturnMessage myIConstructionReturnMessage =
                   this.InitializeServerObject((IConstructionCallMessage)message);
                ConstructionResponse constructionResponse = new
                   ConstructionResponse(null, (IMethodCallMessage)message);
                return constructionResponse;
            }
            IMethodCallMessage methodMessage = new MethodCallMessageWrapper((IMethodCallMessage)message);
            MethodBase method = methodMessage.MethodBase;

            object returnValue = null;
            if (method.DeclaringType == typeof(IDynamicProxy))
                returnValue = method.Invoke(this, methodMessage.Args);
            else
                returnValue = InvocationHandler(ProxyTargetTyped, method, methodMessage.Args);

            ReturnMessage returnMessage = new ReturnMessage(returnValue, methodMessage.Args, methodMessage.ArgCount, methodMessage.LogicalCallContext, methodMessage);
            return returnMessage;
        }
        public override ObjRef CreateObjRef(Type ServerType)
        {
            CustomObjRef myObjRef = new CustomObjRef(ProxyTargetTyped, ServerType);
            myObjRef.URI = URI;
            return myObjRef;
        }
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public class CustomObjRef : ObjRef
        {
            public CustomObjRef(MarshalByRefObject obj, Type type)
                : base(obj, type)
            {
            }

            [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info,
                                               StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        public bool Strict
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public Type[] SupportedTypes
        {
            get
            {
                return new Type[0];
            }
            set
            {
            }
        }

    }
}
