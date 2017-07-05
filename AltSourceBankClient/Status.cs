using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltSourceBankClient {
	class Status {
		public bool LoggedIn;
		public bool Running;
		public User User;

		public Status() {
			LoggedIn = false;
			Running = true;
			User = new User();
		}
	}
}