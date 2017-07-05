using System;

namespace AltSourceBankClient {
	class MenuItem {
		public delegate void MenuAction();
		public string Text;
		public MenuAction Action;

		public MenuItem() {
			Text = "";
			Action = null;
		}
	}
}