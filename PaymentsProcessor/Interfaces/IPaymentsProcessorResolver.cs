using PaymentsProcessor.Enums;

namespace PaymentsProcessor.Interfaces {
	/// <summary>
	/// interface to get the paymentprocessor
	/// </summary>
	public interface IPaymentsProcessorResolver {

		IPaymentProcessor GetPaymentProcessor(int providerid);
		IPaymentProcessor GetPaymentProcessor(PaymentPortalProvider provider);
	}
}
