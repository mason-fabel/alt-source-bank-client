using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltSourceBankClient {
	class Menu {
		private List<MenuItem> Items;

		public Menu() {
			Items = new List<MenuItem>();
		}

		public void AddItem(MenuItem item) {
			Items.Add(item);

			return;
		}

		public void Print() {
			int i;

			Console.WriteLine("Select an action:");

			i = 0;
			foreach (MenuItem item in Items) {
				Console.WriteLine("[{0}]: {1}", ++i, item.Text);
			}

			Console.Write("> ");

			return;
		}

		public void Execute(int index) {
			Console.WriteLine("{0}", Items.ElementAt(index - 1).Text);
			Items.ElementAt(index - 1).Action();

			return;
		}
	}
}