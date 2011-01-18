namespace SharpArchContrib.Castle
{
    using System;
    using System.Linq;
    using System.Reflection;

    using global::Castle.Core.Interceptor;

    using SharpArchContrib.Castle.Logging;

    public class AttributeBasedInteceptorSelector<T> : IInterceptorSelector where T : Attribute
    {
        private readonly AttributeExtractor<T> attributeExtractor;

        public AttributeBasedInteceptorSelector(AttributeExtractor<T> attributeExtractor)
        {
            this.attributeExtractor = attributeExtractor;
        }

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo methodInfo, IInterceptor[] interceptors)
        {
            if (this.ShouldIntercept(methodInfo))
            {
                return interceptors.Where(x => x as AttributeBasedInterceptor<T> != null).ToArray();
            }

            return null;
        }

        public bool ShouldIntercept(MethodInfo methodInfo)
        {
            // we take the most permissive log settings from the attributes we find
            // If there is at least one attribute, the call gets wrapped with a transaction
            var assemblyLogAttributes = this.attributeExtractor.GetAssemblyAttributes(methodInfo);
            var classLogAttributes = this.attributeExtractor.GetClassAttributes(methodInfo);
            var methodLogAttributes = this.attributeExtractor.GetMethodAttributes(methodInfo);

            return assemblyLogAttributes.Any() || classLogAttributes.Any() || methodLogAttributes.Any();
        }
    }
}