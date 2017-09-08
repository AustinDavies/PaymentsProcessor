namespace PaymentsProcessor.Interfaces {
	public interface IProduct {

		string name { get; set; }
		string sku { get; set; }
		decimal cost { get; set; }
		int productid { get; set; }
	}
}
