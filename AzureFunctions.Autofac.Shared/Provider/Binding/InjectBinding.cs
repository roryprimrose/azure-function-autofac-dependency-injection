﻿using Autofac;
using AzureFunctions.Autofac.Configuration;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AzureFunctions.Autofac
{
    internal class InjectBinding : IBinding
    {
        private Type type;
        private string name;
        private string className;
        public bool FromAttribute => true;
        public InjectBinding(Type type, String name, String className)
        {
            this.type = type;
            this.name = name;
            this.className = className;
        }

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context) =>
            Task.FromResult((IValueProvider)new InjectValueProvider(value));

        public Task<IValueProvider> BindAsync(BindingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            var value = DependencyInjection.Resolve(type, name, this.className);
            
            // async/await not required here because there is no continuation
            // Simply returning the task result is better for perf because a state machine for the member is not compiled into the binary
            return BindAsync(value, context.ValueContext);
        }

        public ParameterDescriptor ToParameterDescriptor() => new ParameterDescriptor();
    }
}