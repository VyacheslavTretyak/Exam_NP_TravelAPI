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
			LoadCountries();
			LoadCities();
			LoadHotels();
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
