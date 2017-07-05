using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AltSourceBankClient {
	class HttpHelper {
		private HttpClient Client;

		public HttpHelper(string baseUrl) {
			Client = new HttpClient();
			Client.BaseAddress = new Uri(baseUrl);
		}
		
		public async Task<string> Get (string url) {
			string output;
			HttpResponseMessage response;

			response = await Client.GetAsync(url);
			if (response.IsSuccessStatusCode) {
				output = await response.Content.ReadAsStringAsync();
			} else {
				output = "Unable to load page " + url + ": " + response.StatusCode;
			}

			return output;
		}

		public async Task<string> Post(string url, string json) {
			string output;
			StringContent content;
			HttpResponseMessage response;

			content = new StringContent(json, Encoding.UTF8, "application/json");
			response = await Client.PostAsync(url, content);

			if (response.IsSuccessStatusCode) {
				output = await response.Content.ReadAsStringAsync();
			} else {
				output = "Unable to load page " + url + ": " + response.StatusCode;
			}

			return output;
		}
	}
}