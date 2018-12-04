using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Exam_NP_TravelAPI
{
	/// <summary>
	/// Interaction logic for SignIn.xaml
	/// </summary>
	public partial class AddCity : Window
	{
		public City City{ get; set; }
		private string api;
		private string token;
		public AddCity(string api, string token)
		{
			this.api = api;
			this.token = token;
			InitializeComponent();
			SaveButton.Click += SaveButton_Click;
			SaveButton.IsEnabled = false;
			ComboBoxCountries.SelectionChanged += ComboBoxCountries_SelectionChanged;
			CityNameTextBox.TextChanged += CityNameTextBox_TextChanged;
			SaveButton.IsEnabled = false;

			LoadCountries();
			
		}

		private void CityNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			SaveButton.IsEnabled = CityNameTextBox.Text.Length > 0 && ComboBoxCountries.SelectedItem != null;
		}

		private void ComboBoxCountries_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SaveButton.IsEnabled = ComboBoxCountries.SelectedItem != null;			
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			City = new City()
			{
				CityName = CityNameTextBox.Text,
				CountryId = (ComboBoxCountries.SelectedItem as Country).Id
			};
			DialogResult = true;
		}
		private void LoadCountries()
		{
			string responseJson;
			Response<Country> res = Task<Response<Country>>.Factory.StartNew(() =>
			{
				WebRequest request = WebRequest.Create(api);
				request.Method = "POST";
				string param = "getCountries";
				string data = $"token={token}&param={param}";
				byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(data);
				request.ContentType = "application/x-www-form-urlencoded";
				request.ContentLength = byteArray.Length;
				using (Stream dataStream = request.GetRequestStream())
				{
					dataStream.Write(byteArray, 0, byteArray.Length);
				}
				WebResponse response = request.GetResponse();
				using (Stream stream = response.GetResponseStream())
				{
					using (StreamReader reader = new StreamReader(stream))
					{
						responseJson = reader.ReadToEnd();
					}
				}
				response.Close();
				var obj = JsonConvert.DeserializeObject<Response<Country>>(responseJson);
				return obj;
			}).Result;
			if (res.result == 200)
			{
				ComboBoxCountries.Items.Clear();
				ComboBoxCountries.SelectedItem = null;
				foreach (var item in res.rows)
				{
					ComboBoxCountries.Items.Add(item as Country);
				}
			}
			else
			{
				MessageBox.Show($"HTTP server response: {res.result}");
			}
		}
	}
}
