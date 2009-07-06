﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.Services
{
    public interface IServiceHostProvider
    {
        void Init();
        void Host(Type type, Type contract);
        void Start();
    }

    public class NullServiceHostProvider : IServiceHostProvider
    {

        public void Host(Type type, Type contract)
        {
        }

        public void Init()
        {
            
        }

        public void Start()
        {

        }
    }
}
