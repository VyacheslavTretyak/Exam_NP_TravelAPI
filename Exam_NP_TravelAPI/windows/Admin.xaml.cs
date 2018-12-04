using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Exam_NP_TravelAPI
{
	public partial class Admin : Window
	{
		private string token;
		private string api;				
		private string responseJson;
		private ObservableCollection<Hotel> hotels;
		public ObservableCollection<Hotel> Hotels
		{
			get { return hotels; }
			set { hotels = value; }
		}
		private ObservableCollection<City> cities;
		public ObservableCollection<City> Cities
		{
			get { return cities; }
			set { cities = value; }
		}
		private ObservableCollection<Country> countries;
		public ObservableCollection<Country> Countries
		{
			get { return countries; }
			set { countries = value; }
		}

		public Admin(string api, string token)
		{
			this.api = api;
			this.token = token;
			InitializeComponent();

			ListCountries.SelectionChanged += ListCountries_SelectionChanged;
			ListCities.SelectionChanged += ListCities_SelectionChanged;
			ListHotels.SelectionChanged += ListHotels_SelectionChanged;
			AddCountryButton.Click += AddCountryButton_Click;
			RemCountryButton.Click += RemCountryButton_Click;
			AddCityButton.Click += AddCityButton_Click;
			RemCityButton.Click += RemCityButton_Click;
			AddHotelButton.Click += AddHotelButton_Click;
			RemHotelButton.Click += RemHotelButton_Click;

			LoadCountries();
			LoadCities();
			LoadHotels();
		}

		private void RemHotelButton_Click(object sender, RoutedEventArgs e)
		{
			RemoveHotel(ListHotels.SelectedItem as Hotel);
		}
		private void ListHotels_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			RemHotelButton.IsEnabled = ListHotels.SelectedItem != null;
		}
		private void RemoveHotel(Hotel hotel)
		{
			string jsonData = JsonConvert.SerializeObject(hotel);
			Response<Hotel> res = Task<Response<Hotel>>.Factory.StartNew(() =>
			{
				WebRequest request = WebRequest.Create(api);
				request.Method = "POST";
				string param = "remove";
				string data = $"token={token}&param={param}&table=hotels&data={jsonData}";
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
				var obj = JsonConvert.DeserializeObject<Response<Hotel>>(responseJson);
				return obj;
			}).Result;
			if (res.result == 200)
			{
				LoadHotels();
			}
			else
			{
				MessageBox.Show($"HTTP server response: {res.result}");
			}
		}
		private void AddHotelButton_Click(object sender, RoutedEventArgs e)
		{
			AddHotel addHotelWnd = new AddHotel(api, token);
			addHotelWnd.ShowDialog();
			if (addHotelWnd.DialogResult != true)
			{
				return;
			}
			string jsonData = JsonConvert.SerializeObject(addHotelWnd.Hotel);
			Response<Hotel> res = Task<Response<Hotel>>.Factory.StartNew(() =>
			{
				WebRequest request = WebRequest.Create(api);
				request.Method = "POST";
				string param = "insertHotel";
				string data = $"token={token}&param={param}&data={jsonData}";
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
				var obj = JsonConvert.DeserializeObject<Response<Hotel>>(responseJson);
				return obj;
			}).Result;
			if (res.result == 200)
			{
				LoadHotels();
			}
			else
			{
				MessageBox.Show($"HTTP server response: {res.result}");
			}
		}
		private void ListCities_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			RemCityButton.IsEnabled = ListCities.SelectedItem != null;
		}
		private void RemCityButton_Click(object sender, RoutedEventArgs e)
		{
			RemoveCity(ListCities.SelectedItem as City);
		}
		private void RemoveCity(City city)
		{
			string jsonData = JsonConvert.SerializeObject(city);
			Response<City> res = Task<Response<City>>.Factory.StartNew(() =>
			{
				WebRequest request = WebRequest.Create(api);
				request.Method = "POST";
				string param = "remove";
				string data = $"token={token}&param={param}&table=cities&data={jsonData}";
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
				LoadCities();
			}
			else
			{
				MessageBox.Show($"HTTP server response: {res.result}");
			}
		}
		private void AddCityButton_Click(object sender, RoutedEventArgs e)
		{
			AddCity addCityWnd = new AddCity(api, token);
			addCityWnd.ShowDialog();
			if (addCityWnd.DialogResult != true)
			{
				return;
			}
			string jsonData = JsonConvert.SerializeObject(addCityWnd.City);
			Response<Hotel> res = Task<Response<Hotel>>.Factory.StartNew(() =>
			{
				WebRequest request = WebRequest.Create(api);
				request.Method = "POST";
				string param = "insertCity";
				string data = $"token={token}&param={param}&data={jsonData}";
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
				var obj = JsonConvert.DeserializeObject<Response<Hotel>>(responseJson);
				return obj;
			}).Result;
			if (res.result == 200)
			{
				LoadCities();
			}
			else
			{
				MessageBox.Show($"HTTP server response: {res.result}");
			}
		}
		private void RemCountryButton_Click(object sender, RoutedEventArgs e)
		{
			RemoveCountry(ListCountries.SelectedItem as Country);
		}
		private void ListCountries_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			RemCountryButton.IsEnabled = ListCountries.SelectedItem != null;		
		}
		private void RemoveCountry(Country country)
		{			
			string jsonData = JsonConvert.SerializeObject(country);
			Response<Country> res = Task<Response<Country>>.Factory.StartNew(() =>
			{
				WebRequest request = WebRequest.Create(api);
				request.Method = "POST";
				string param = "remove";
				string data = $"token={token}&param={param}&table=countries&data={jsonData}";
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
				LoadCountries();
			}
			else
			{
				MessageBox.Show($"HTTP server response: {res.result}");
			}
		}
		private void AddCountryButton_Click(object sender, RoutedEventArgs e)
		{
			AddCountry addCountryWnd = new AddCountry();
			addCountryWnd.ShowDialog();
			if(addCountryWnd.DialogResult != true)
			{
				return;
			}
			string jsonData = JsonConvert.SerializeObject(addCountryWnd.Country);
			Response<Hotel> res = Task<Response<Hotel>>.Factory.StartNew(() =>
			{
				WebRequest request = WebRequest.Create(api);
				request.Method = "POST";
				string param = "insertCountry";
				string data = $"token={token}&param={param}&data={jsonData}";
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
				var obj = JsonConvert.DeserializeObject<Response<Hotel>>(responseJson);
				return obj;
			}).Result;
			if (res.result == 200)
			{
				LoadCountries();
			}
			else
			{
				MessageBox.Show($"HTTP server response: {res.result}");
			}
		}

		private void LoadHotels()
		{
			Response<Hotel> res = Task<Response<Hotel>>.Factory.StartNew(() =>
			{
				WebRequest request = WebRequest.Create(api);
				request.Method = "POST";
				string param = "getHotels";
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
				var obj = JsonConvert.DeserializeObject<Response<Hotel>>(responseJson);
				return obj;
			}).Result;
			if (res.result == 200)
			{
				hotels = new ObservableCollection<Hotel>();				
				foreach (var item in res.rows)
				{
					Hotel hotel = item as Hotel;
					hotels.Add(hotel);
				}
				ListHotels.ItemsSource = null;
				ListHotels.ItemsSource = Hotels;
				DataContext = this;
			}
			else
			{
				MessageBox.Show($"HTTP server response: {res.result}");
			}
		}
		private void LoadCities()
		{
			Response<City> res = Task<Response<City>>.Factory.StartNew(() =>
			{
				WebRequest request = WebRequest.Create(api);
				request.Method = "POST";
				string param = "getCities";
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
				var obj = JsonConvert.DeserializeObject<Response<City>>(responseJson);
				return obj;
			}).Result;
			if (res.result == 200)
			{
				cities = new ObservableCollection<City>();
				foreach (var item in res.rows)
				{
					City city = item as City;
					cities.Add(city);
				}
				ListCities.ItemsSource = null;
				ListCities.ItemsSource = Cities;
				DataContext = this;
			}
			else
			{
				MessageBox.Show($"HTTP server response: {res.result}");
			}
		}
		private void LoadCountries()
		{
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
				countries = new ObservableCollection<Country>();
				foreach (var item in res.rows)
				{
					Country country = item as Country;
					countries.Add(country);
				}
				ListCountries.ItemsSource = null;
				ListCountries.ItemsSource = Countries;
				DataContext = this;
			}
			else
			{
				MessageBox.Show($"HTTP server response: {res.result}");
			}
		}
	}	
}
