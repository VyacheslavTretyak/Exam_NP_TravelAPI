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
	public partial class AddHotel : Window
	{
		public Hotel Hotel{ get; set; }
		private string api;
		private string token;
		public AddHotel(string api, string token)
		{
			this.api = api;
			this.token = token;
			InitializeComponent();
			SaveButton.Click += SaveButton_Click;
			SaveButton.IsEnabled = false;
			ComboBoxCountries.SelectionChanged += ComboBoxCountries_SelectionChanged;
			ComboBoxCities.SelectionChanged += ComboBoxCities_SelectionChanged;
			ComboBoxStars.SelectionChanged += ComboBoxStars_SelectionChanged;
			HotelNameTextBox.TextChanged += HotelNameTextBox_TextChanged;
			CostTextBox.TextChanged += CostTextBox_TextChanged;
			InfoTextBox.TextChanged += CostTextBox_TextChanged;
			SaveButton.IsEnabled = false;

			LoadCountries();
			
		}
		private void CostTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			EnableButton();
		}

		private void ComboBoxStars_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			EnableButton();
		}

		private void ComboBoxCities_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			EnableButton();
		}

		private void EnableButton()
		{
			SaveButton.IsEnabled = HotelNameTextBox.Text.Length > 0 && ComboBoxCountries.SelectedItem != null && ComboBoxCities.SelectedItem != null && ComboBoxStars.SelectedItem != null && CostTextBox.Text.Length > 0 && InfoTextBox.Text.Length > 0;
		}
		private void HotelNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			EnableButton();
		}
		private void ComboBoxCountries_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBoxCities.IsEnabled = true;
			ComboBoxCities.Items.Clear();			
			if (ComboBoxCountries.SelectedItem != null)
			{
				LoadCities((ComboBoxCountries.SelectedItem as Country).Id);
			}
			EnableButton();
		}
		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			Hotel = new Hotel()
			{
				HotelName = HotelNameTextBox.Text,
				CountryId = (ComboBoxCountries.SelectedItem as Country).Id,
				CityId = (ComboBoxCities.SelectedItem as City).Id,
				Stars = (ComboBoxStars.SelectedItem as ComboBoxItem).Content.ToString(),
				Cost = int.Parse(CostTextBox.Text),
				Info = InfoTextBox.Text
			};
			DialogResult = true;
		}
		private void LoadCities(int countryId)
		{
			string responseJson;
			Response<City> res = Task<Response<City>>.Factory.StartNew(() =>
			{
				WebRequest request = WebRequest.Create(api);
				request.Method = "POST";
				string param = "getCities";
				string data = $"token={token}&param={param}&countryId={countryId}";
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
				var obj = JsonConvert.DeserializeObject<Response<City>>(responseJson);
				return obj;
			}).Result;
			if (res.result == 200)
			{
				ComboBoxCities.Items.Clear();
				foreach (var item in res.rows)
				{
					ComboBoxCities.Items.Add(item as City);
				}
			}
			else
			{
				MessageBox.Show($"HTTP server response: {res.result}");
			}
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
