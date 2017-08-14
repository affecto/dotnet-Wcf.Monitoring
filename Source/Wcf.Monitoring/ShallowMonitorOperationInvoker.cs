using System;
using System.ServiceModel.Dispatcher;

namespace Affecto.Wcf.Monitoring
{
    internal class ShallowMonitorOperationInvoker : IOperationInvoker
    {
        public object[] AllocateInputs()
        {
            return new object[0];
        }

        public virtual object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            outputs = new object[0];
            return "OK";
        }

        public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public bool IsSynchronous
        {
            get { return true; }
        }
    }
}