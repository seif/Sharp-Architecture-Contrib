using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using SharpArchContrib.Core;
using SharpArchContrib.Data.NHibernate;

namespace SharpArchContrib.Data.CastleWindsor {
    public static class ComponentRegistrar {
        public static void AddComponentsTo(IWindsorContainer container) {
            AddComponentsTo(container, typeof(NHibernateTransactionManager));
        }

        public static void AddComponentsTo(IWindsorContainer container, Type transactionManagerType) {
            ParameterCheck.ParameterRequired(container, "container");
            ParameterCheck.ParameterRequired(transactionManagerType, "transactionManagerType");

            if (!container.Kernel.HasComponent("TransactionManager")) {
                Core.CastleWindsor.ComponentRegistrar.AddComponentsTo(container);
                container.Register(Component.For<ITransactionManager>().ImplementedBy(transactionManagerType)
                                    .Named("TransactionManager"));
            }
        }
    }
}