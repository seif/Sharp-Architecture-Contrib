namespace SharpArchContrib.Castle.Logging
{
    using System;
    using System.Collections.Generic;

    using global::Castle.Core;
    using global::Castle.MicroKernel;

    public class ExceptionHandlerFacility : AttributeControlledFacilityBase
    {
        public ExceptionHandlerFacility()
            : base(typeof(ExceptionHandlerInterceptor), LifestyleType.Singleton)
        {
        }

        protected override void RegisterModelInterceptorsSelector()
        {
            this.Kernel.ProxyFactory.AddInterceptorSelector(new AttributeBasedInteceptorSelector<ExceptionHandlerAttribute>());
        }
    }
}