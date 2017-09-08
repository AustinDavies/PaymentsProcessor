namespace PaymentsProcessor.Enums {
	/// <summary>
	/// Defines the payment portal provider as it relates to the 'paymentportal_paymentportalid' column in the purchases table. 
	/// </summary>
	public enum PaymentPortalProvider {
		STRIPE = 1,
		PAYPAL = 2,
		RAZORPAY = 30,
		AUTHORIZE_NET = 32,
		NONE = 0
	}
}
