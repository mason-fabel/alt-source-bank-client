using System.Collections.Generic;

namespace AltSourceBankClient {
	class User {
		public string Name;
		public string Email;
		public string Password;
		public string Key;
		public List<Transaction> Transactions;

		public User() {
			Name = "";
			Email = "";
			Password = "";
			Key = "";
			Transactions = new List<Transaction>();
		}
	}
}