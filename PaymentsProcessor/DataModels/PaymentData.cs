using Newtonsoft.Json.Linq;

namespace PaymentsProcessor.DataModels {
	public class PaymentData {

		public string payment_id { get; set; }
		public string customer_id { get; set; }

		public int amount { get; set; }
		public string description { get; set; }
		public string origin_of_sale { get; set; }

		public string address_line_1 { get; set; }
		public string address_line_2 { get; set; }
		public string country { get; set; }
		public string postalcode { get; set; }

		public JObject metadata { get; set; }
	}
}
