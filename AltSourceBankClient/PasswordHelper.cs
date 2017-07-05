using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltSourceBankClient {
	class PasswordHelper {
		public static string ReadPassword() {
			bool inputting;
			string password;
			ConsoleKeyInfo key;

			password = "";
			inputting = true;

			while (inputting) {
				key = Console.ReadKey(true);

				switch (key.Key) {
					case ConsoleKey.Enter:
						inputting = false;
						break;
					case ConsoleKey.Backspace:
					case ConsoleKey.Delete:
						if (password.Length > 0) password = password.Remove(password.Length - 1);
						break;
					default:
						password += key.KeyChar;
						break;
				}
			}

			Console.Write("\n");

			return password;
		}
	}
}