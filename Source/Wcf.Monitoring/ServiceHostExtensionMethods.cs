using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Affecto.Wcf.Monitoring
{
    public static class MonitoringExtensionMethods
    {
        /// <summary>
        /// Generate shallow monitoring endpoints for all service contracts hosted by a service host.
        /// </summary>
        /// <param name="serviceHost">WCF service host</param>
        public static void GenerateShallowMonitoringEndpoints(this ServiceHostBase serviceHost)
        {
            GenerateMonitoringEndpoints(serviceHost, "Monitor-Shallow", () => new ShallowMonitorOperationInvoker());
        }

        /// <summary>
        /// Generate deep monitoring endpoints for all service contracts hosted by a service host.
        /// </summary>
        /// <param name="serviceHost">WCF service host</param>
        /// <param name="healthCheckServiceFactory">Factory for retrieving a deep health check service instance.</param>
        public static void GenerateDeepMonitoringEndpoints(this ServiceHostBase serviceHost, Func<IHealthCheckService> healthCheckServiceFactory)
        {
            GenerateMonitoringEndpoints(serviceHost, "Monitor-Deep", () => new DeepMonitorOperationInvoker(healthCheckServiceFactory));
        }

        /// <summary>
        /// Generate shallow and deep monitoring endpoints for all service contracts hosted by a service host.
        /// </summary>
        /// <param name="serviceHost">WCF service host</param>
        /// <param name="healthCheckServiceFactory">Factory for retrieving a deep health check service instance.</param>
        public static void GenerateMonitoringEndpoints(this ServiceHostBase serviceHost, Func<IHealthCheckService> healthCheckServiceFactory)
        {
            serviceHost.GenerateShallowMonitoringEndpoints();
            serviceHost.GenerateDeepMonitoringEndpoints(healthCheckServiceFactory);
        }

        private static void GenerateMonitoringEndpoints(ServiceHostBase serviceHost, string operationName, Func<IOperationInvoker> operationInvokerFactory)
        {
            foreach (ServiceEndpoint endpoint in serviceHost.Description.Endpoints)
            {
                ContractDescription contract = endpoint.Contract;

                if (contract.Operations.Any(o => o.Name == operationName))
                {
                    continue;
                }

                var operationDescription = new OperationDescription(operationName, contract);
                var inputMessage = new MessageDescription(contract.Namespace + contract.Name + "/" + operationName + "_Request", MessageDirection.Input);

                var outputMessage = new MessageDescription(contract.Namespace + contract.Name + "/" + operationName + "_Response", MessageDirection.Output);
                outputMessage.Body.ReturnValue = new MessagePartDescription("Result", contract.Namespace) { Type = typeof(string) };

                operationDescription.Messages.Add(inputMessage);
                operationDescription.Messages.Add(outputMessage);
                operationDescription.Behaviors.Add(new DataContractSerializerOperationBehavior(operationDescription));
                operationDescription.Behaviors.Add(new MonitorEndpointBehavior(operationInvokerFactory));

                contract.Operations.Add(operationDescription);
            }
        }
    }
}