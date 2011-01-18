namespace SharpArchContrib.Castle.Logging
{
    using global::Castle.Core;

    public class LogFacility : AttributeControlledFacilityBase
    {
        public LogFacility()
            : base(typeof(LogInterceptor), LifestyleType.Singleton)
        {
        }
    }
}