namespace SharpArchContrib.Castle.Logging
{
    using System;
    using System.Collections.Generic;

    using global::Castle.Core;
    using global::Castle.MicroKernel;
    using global::Castle.MicroKernel.ModelBuilder;

    using SharpArchContrib.Core.Logging;

    public class ExceptionHandlerFacility : AttributeControlledFacilityBase<ExceptionHandlerAttributeSettings, ExceptionHandlerInterceptor>
    {
        public ExceptionHandlerFacility()
            : base(LifestyleType.Singleton)
        {
        }

        protected override void AddContributor()
        {
            this.Kernel.ComponentModelBuilder.AddContributor(
                new ExceptionHandlerComponentInspector());
        }
    }
}