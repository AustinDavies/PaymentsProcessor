using PaymentsProcessor.Enums;
using PaymentsProcessor.Interfaces;
using System.Collections.Generic;

namespace PaymentsProcessor.Factories {
	/// <summary>
	/// Factory class used for IoC container setup. Interface should be used as the user facing dependency.
	/// </summary>
	internal class PaymentsProcessorFactory : IPaymentsProcessorResolver {

		private Dictionary<int, IPaymentProcessor> clients;

		public PaymentsProcessorFactory() {
			clients = new Dictionary<int, IPaymentProcessor>();
		}

		public virtual PaymentsProcessorFactory AttachPaymentProcessor(int providerid, IPaymentProcessor paymentProcessor) {
			clients.Add(providerid, paymentProcessor);
			return this;
		}

		public virtual PaymentsProcessorFactory AttachPaymentProcessor(PaymentPortalProvider provider, IPaymentProcessor paymentProcessor) {
			return AttachPaymentProcessor((int)provider, paymentProcessor);
		}

		public virtual IPaymentProcessor GetPaymentProcessor(int providerid) {
			return clients[providerid];
		}

		public IPaymentProcessor GetPaymentProcessor(PaymentPortalProvider provider) {
			return GetPaymentProcessor((int)provider);
		}
	}
}
