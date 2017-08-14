using System;

namespace Affecto.Wcf.Monitoring
{
    internal class DeepMonitorOperationInvoker : ShallowMonitorOperationInvoker
    {
        private readonly Func<IHealthCheckService> healthCheckServiceFactory;

        public DeepMonitorOperationInvoker(Func<IHealthCheckService> healthCheckServiceFactory)
        {
            if (healthCheckServiceFactory == null)
            {
                throw new ArgumentNullException("healthCheckServiceFactory");
            }

            this.healthCheckServiceFactory = healthCheckServiceFactory;
        }

        public override object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            IHealthCheckService service = healthCheckServiceFactory.Invoke();
            service.CheckHealth();
            return base.Invoke(instance, inputs, out outputs);
        }
    }
}