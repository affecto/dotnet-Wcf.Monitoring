using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Affecto.Wcf.Monitoring
{
    internal class MonitorEndpointBehavior : IOperationBehavior
    {
        private readonly Func<IOperationInvoker> operationInvokerFactory;

        public MonitorEndpointBehavior(Func<IOperationInvoker> operationInvokerFactory)
        {
            this.operationInvokerFactory = operationInvokerFactory;
        }

        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.Invoker = operationInvokerFactory.Invoke();
        }

        public void Validate(OperationDescription operationDescription)
        {
        }
    }
}