using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AltSourceBankClient {
	class AltSourceBankClient {
		static void Main(string[] args) {
			Menu menu;
			Status status;

			status = new Status();

			while (status.Running) {
				menu = BuildMenu(status);
				menu.Print();
				menu.Execute(int.Parse(Console.ReadLine()));
				Console.WriteLine();
			}

			return;
		}

		private static Menu BuildMenu(Status status) {
			HttpHelper http;
			Menu menu;
			MenuItem item;

			http = new HttpHelper("http://localhost:64952");
			menu = new Menu();

			item = new MenuItem();
			item.Text = "Create User";
			item.Action = delegate () {
				Task<string> httpTask;
				string name;
				string email;
				string password;
				string passwordConfirm;

				Console.Write("Name: ");
				name = Console.ReadLine().Trim();
				Console.Write("Email: ");
				email = Console.ReadLine().Trim();
				Console.Write("Password: ");
				password = PasswordHelper.ReadPassword();
				Console.Write("Confirm Password: ");
				passwordConfirm = PasswordHelper.ReadPassword();

				if (password != passwordConfirm) {
					Console.WriteLine("Passwords do not match");
				} else {
					httpTask = http.Post("/User/Create/", JsonConvert.SerializeObject(new {
						name = name,
						email = email,
						password = password
					}));
				}

				return;
			};
			menu.AddItem(item);

			if (!status.LoggedIn) {
				item = new MenuItem();
				item.Text = "Log In";
				item.Action = delegate () {
					Task<string> httpTask;
					string email;
					string password;

					Console.Write("Email: ");
					email = Console.ReadLine().Trim();
					Console.Write("Password: ");
					password = PasswordHelper.ReadPassword();

					httpTask = http.Post("/User/Login/", JsonConvert.SerializeObject(new {
						email = email,
						password = password
					}));

					try {
						status.User = JsonConvert.DeserializeObject<User>(httpTask.Result);
						status.LoggedIn = true;
					} catch {
						Console.WriteLine("Unable to log in as {0}", email);
					}

					return;
				};
				menu.AddItem(item);
			} else {
				item = new MenuItem();
				item.Text = "Log Out";
				item.Action = delegate () {
					Task<string> httpTask;

					httpTask = http.Post("/User/Logout/", JsonConvert.SerializeObject(new {
						key = status.User.Key,
					}));

					status.LoggedIn = false;

					return;
				};
				menu.AddItem(item);

				item = new MenuItem();
				item.Text = "Check Balance";
				item.Action = delegate () {
					Task<string> httpTask;

					httpTask = http.Post("/User/Balance/", JsonConvert.SerializeObject(new {
						key = status.User.Key,
					}));

					try {
						var balance = JsonConvert.DeserializeObject(httpTask.Result);
						Console.WriteLine("Balance: ${0:F2}", balance);
					} catch{
						Console.WriteLine("Unable to retrieve balance for {0}", status.User.Name);
					}

					return;
				};
				menu.AddItem(item);

				item = new MenuItem();
				item.Text = "Deposit";
				item.Action = delegate () {
					double amount;
					Task<string> httpTask;

					Console.Write("Amount: $");
					amount = double.Parse(Console.ReadLine().Trim());

					httpTask = http.Post("/User/Deposit/", JsonConvert.SerializeObject(new {
						key = status.User.Key,
						amount = amount
					}));

					return;
				};
				menu.AddItem(item);

				item = new MenuItem();
				item.Text = "Withdraw";
				item.Action = delegate () {
					double amount;
					Task<string> httpTask;

					Console.Write("Amount: $");
					amount = double.Parse(Console.ReadLine().Trim());

					httpTask = http.Post("/User/Withdrawal/", JsonConvert.SerializeObject(new {
						key = status.User.Key,
						amount = amount
					}));

					return;
				};
				menu.AddItem(item);

				item = new MenuItem();
				item.Text = "Transaction History";
				item.Action = delegate () {
					List<Transaction> transactions;
					Task<string> httpTask;

					httpTask = http.Post("/User/History/", JsonConvert.SerializeObject(new {
						key = status.User.Key
					}));

					try {
						transactions = JsonConvert.DeserializeObject<List<Transaction>>(httpTask.Result);

						Console.WriteLine("DATE                    TYPE            AMOUNT          BALANCE    ");
						Console.WriteLine("===================================================================");
						foreach (Transaction t in transactions) {
							Console.WriteLine("{0}\t{1}\t${2,10:C2}\t${3,10:C2}", t.Date.ToString(), t.Amount > 0 ? "Deposit   " : "Withdrawal", t.Amount, t.Balance);
						}
					} catch{
						Console.WriteLine("Unable to retrieve history for {0}", status.User.Name);
					}

					return;
				};
				menu.AddItem(item);
			}

			item = new MenuItem();
			item.Text = "Exit";
			item.Action = delegate() {
				status.Running = false;

				return;
			};
			menu.AddItem(item);

			return menu;
		}
	}
}