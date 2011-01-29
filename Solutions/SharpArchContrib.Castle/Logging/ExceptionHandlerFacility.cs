namespace SharpArchContrib.Castle.Logging
{
    using System;
    using System.Collections.Generic;

    using global::Castle.Core;
    using global::Castle.MicroKernel;

    public class ExceptionHandlerFacility : AttributeControlledFacilityBase<ExceptionHandlerAttribute, ExceptionHandlerInterceptor>
    {
        public ExceptionHandlerFacility()
            : base(LifestyleType.Singleton)
        {
        }
    }
}