using System;
using System.Collections.Generic;
using Castle.Core;
using Castle.DynamicProxy;
using Castle.MicroKernel;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Registration;
using SharpArchContrib.Core;

namespace SharpArchContrib.Castle {
    public abstract class AttributeControlledFacilityBase : AbstractFacility {
        private readonly Type interceptorType;
        private readonly LifestyleType lifestyleType;

        public AttributeControlledFacilityBase(Type interceptorType, LifestyleType lifestyleType) {
            ParameterCheck.ParameterRequired(interceptorType, "interceptorType");

            this.interceptorType = interceptorType;
            this.lifestyleType = lifestyleType;
        }

        protected override void Init() {
            Kernel.Register(Component.For<IInterceptor>().ImplementedBy(interceptorType)
                            .Named(interceptorType.Name).LifeStyle.Is(lifestyleType));
            
            Kernel.ComponentRegistered += KernelComponentRegistered;
        }

        private void KernelComponentRegistered(string key, IHandler handler) {
            AddInterceptorIfNeeded(handler, GetAttributes(handler));
        }

        protected abstract List<Attribute> GetAttributes(IHandler handler);

        private bool AddInterceptorIfNeeded(IHandler handler, List<Attribute> attributes) {
            foreach (Attribute attribute in attributes) {
                handler.ComponentModel.Interceptors.Add(new InterceptorReference(interceptorType.Name));
                return true;
            }

            return false;
        }
    }
}