namespace SharpArchContrib.Castle.Logging
{
    using global::Castle.Core;

    public class LogFacility : AttributeControlledFacilityBase<LogAttribute, LogInterceptor>
    {
        public LogFacility()
            : base(LifestyleType.Singleton)
        {
        }
    }
}