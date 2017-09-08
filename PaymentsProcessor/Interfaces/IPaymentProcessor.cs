using PaymentsProcessor.DataModels;

namespace PaymentsProcessor.Interfaces {
	public interface IPaymentProcessor {

		/// <summary>
		/// Executes an authorized payment.
		/// </summary>
		/// <param name="paymentid">The payment authorization token issued by the payment processor company.</param>
		/// <returns>The id of the charge.</returns>
		string ExecutePayment(PaymentData payment);

		/// <summary>
		///  Refunds the specified amount of a previously executed payment.
		/// </summary>
		/// <param name="paymentid">The payment authorization token issued by the payment processor company.</param>
		/// <param name="amount">The amount to refund in cents.</param>
		/// <param name="notes">Additional notes to attach to the refund.</param>
		/// <returns>The id of the refund.</returns>
		string RefundPayment(string paymentid, int amount, string notes = null);

		/// <summary>
		/// Fully refunds a previously executed payment.
		/// </summary>
		/// <param name="paymentid">The payment authorization token issued by the payment processor company.</param>
		/// <param name="notes">Additional notes to attach to the refund.</param>
		/// <returns>The id of the refund.</returns>
		string RefundPaymentFully(string paymentid, string notes = null);
	}
}
