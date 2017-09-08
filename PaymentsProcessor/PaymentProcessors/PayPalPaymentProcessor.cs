using PaymentsProcessor.Interfaces;
using PayPal.Api;
using System.Collections.Generic;
using System.Linq;

namespace PaymentsProcessor.PaymentProcessors {
	public class PayPalPaymentProcessor : IPaymentProcessor {

		private string clientId, clientSecret, mode;
		private string accessToken => _GetAccessToken();
		private APIContext apiContext => _GetAPIContext();


		public PayPalPaymentProcessor(string clientId, string clientSecret, string mode) {
			this.clientId = clientId;
			this.clientSecret = clientSecret;
			this.mode = mode;
		}

		public Payment CreatePayment(ICollection<IProduct> products) {
			var transaction = new PayPal.Api.Transaction() {
				amount = new PayPal.Api.Amount() {
					currency = "USD",
					total = products.Sum(a => a.cost).ToString()
				},
				description = string.Join(", ", products.Select(a => a.name)),
				item_list = new PayPal.Api.ItemList() {
					items = products.Select(a => new PayPal.Api.Item() {
						name = a.name,
						currency = "USD",
						price = a.cost.ToString(),
						quantity = products.Where(x => x.productid == a.productid).Count().ToString(),
						sku = a.sku
					}).ToList()
				}
			};
			var payment = new PayPal.Api.Payment() {
				intent = "sale",
				transactions = new System.Collections.Generic.List<PayPal.Api.Transaction>() { transaction },
				redirect_urls = new PayPal.Api.RedirectUrls() {
					return_url = "/"
				}
			};
			return PayPal.Api.Payment.Create(apiContext, payment);

		}

		public string ExecutePayment(DataModels.PaymentData paymentData) {
			var amountToBeExecuted = (paymentData.amount / 100M).ToString();
			var payment = Payment.Execute(apiContext, paymentData.payment_id,
				new PaymentExecution() {
					payer_id = paymentData.customer_id,
					transactions = new List<Transaction>() {
						new Transaction() {
							amount = new Amount() {
								total = amountToBeExecuted,
								currency = "USD"
							},
							description = paymentData.description
						}
					}
				});
			if (payment.state.ToLower() != "approved") { throw new PayPal.PayPalException("Your payment was not approved."); }
			return payment.id;
		}

		public string RefundPayment(string paymentid, int amount, string notes = null) {
			var payment = Payment.Get(apiContext, paymentid);
			var sale = payment.transactions[0].related_resources[0].sale;
			var amountInDollars = amount / 100M;
			var refund = new RefundRequest() {
				amount = new Amount() {
					currency = "USD",
					total = amountInDollars.ToString()
				},
				reason = notes
			};
			var response = Sale.Refund(apiContext, sale.id, refund);
			return response.id;
		}

		public string RefundPaymentFully(string paymentid, string notes = null) {
			var payment = Payment.Get(apiContext, paymentid);
			var sale = payment.transactions[0].related_resources[0].sale;
			var fullAmountToRefund = payment.transactions[0].amount;
			var refund = new RefundRequest() {
				amount = fullAmountToRefund,
				reason = notes
			};
			var response = Sale.Refund(apiContext, sale.id, refund);
			return response.id;
		}

		private APIContext _GetAPIContext() {
			var apiContext = new APIContext(accessToken);
			var config = apiContext.GetConfigWithDefaults() ?? new Dictionary<string, string>();
			if (config.ContainsKey("mode")) {
				config["mode"] = mode;
			} else {
				config.Add("mode", mode);
			}
			apiContext.Config = config;
			return apiContext;
		}

		private string _GetAccessToken() {
			var config = new Dictionary<string, string>() {
				{ "mode", mode },
				{ "clientId", clientId },
				{ "clientSecret", clientSecret }
			};
			var authCredential = new OAuthTokenCredential(config);
			return authCredential.GetAccessToken();
		}
	}
}
