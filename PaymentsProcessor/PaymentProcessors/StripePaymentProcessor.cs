using PaymentsProcessor.DataModels;
using PaymentsProcessor.Interfaces;
using Stripe;
using System;
using System.Collections.Generic;

namespace PaymentsProcessor.PaymentProcessors {

	public class StripePaymentProcessor : IPaymentProcessor {

		protected readonly string stripeSecretKey;
		private StripeServiceFactory stripeServiceFactory;

		public StripePaymentProcessor(string stripeSecretKey) {
			this.stripeSecretKey = stripeSecretKey;
			this.stripeServiceFactory = new StripeServiceFactory(stripeSecretKey);
		}

		public string ExecutePayment(PaymentData payment) {
			var metadata = payment.metadata == null ? null : payment.metadata.ToObject<Dictionary<string, string>>();

			var chargeDetails = new StripeChargeCreateOptions() {
				Amount = payment.amount,
				Currency = "usd",
				Description = payment.description,
				SourceTokenOrExistingSourceId = payment.payment_id,
				Capture = true,
				CustomerId = payment.customer_id,
				Metadata = metadata
			};

			try {
				var stripeCharge = stripeServiceFactory.CreateNewChargeService()
					.Create(chargeDetails);
				return stripeCharge.Id;
			} catch (Exception ex) {
				throw ex;
			}

		}

		public string RefundPaymentFully(string paymentid, string notes = null) {
			var stripeRefund = stripeServiceFactory.CreateNewRefundService()
				.Create(paymentid, new StripeRefundCreateOptions() {
					Reason = notes
				});
			return stripeRefund.Id;
		}

		public string RefundPayment(string paymentid, int amount, string notes = null) {
			var stripeRefund = stripeServiceFactory.CreateNewRefundService()
				.Create(paymentid, new StripeRefundCreateOptions() {
					Amount = amount,
					Reason = notes
				});
			return stripeRefund.Id;
		}

		private class StripeServiceFactory {

			private readonly string stripeSecret;

			public StripeServiceFactory(string stripeSecret) {
				this.stripeSecret = stripeSecret;
			}

			public StripeTokenService CreateNewTokenService() {
				return new StripeTokenService(stripeSecret);
			}

			public StripeChargeService CreateNewChargeService() {
				return new StripeChargeService(stripeSecret);
			}

			public StripeCustomerService CreateNewCustomerService() {
				return new StripeCustomerService(stripeSecret);
			}

			public StripeRefundService CreateNewRefundService() {
				return new StripeRefundService(stripeSecret);
			}

		}
	}
}
