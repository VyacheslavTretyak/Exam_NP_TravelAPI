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
	public partial class MainWindow : Window
    {
		public string Token { get; set; } = "ps_rpo_1";
		public string Api { get; set; } = "http://localhost/apiExem/api.php";
		private ObservableCollection<Hotel> hotels;
		private User User { get; set; } = null;
		private string responseJson;
		public ObservableCollection<Hotel> Hotels
		{
			get { return hotels; }
			set { hotels = value; }
		}

		public MainWindow()
        {
            InitializeComponent();

			ComboBoxCountries.SelectionChanged += ComboBoxCountries_SelectionChanged;
			ComboBoxCities.SelectionChanged += ComboBoxCities_SelectionChanged;
			MenuListBox.SelectionChanged += MenuListBox_SelectionChanged;
			Popup.Opened += Popup_Opened;

			LoadCountries();
        }

		private void Popup_Opened(object sender, RoutedEventArgs e)
		{
			MenuListBox.Items.Clear();
			MenuListBox.Items.Add(new ListBoxItem() { Content = "Sign In" });
			MenuListBox.Items.Add(new ListBoxItem() { Content = "Sign Up" });
			var adminItem = new ListBoxItem() { Content = "Admin", Name = "AdminItem" };
			MenuListBox.Items.Add(adminItem);
			if (User != null && User.RoleId == 1)
			{
				adminItem.Foreground = new SolidColorBrush(Colors.White);
			}
			else
			{
				adminItem.Foreground = new SolidColorBrush(Colors.Gray);
			}
			MenuListBox.Items.Add(new ListBoxItem() { Content = "Exit" });
		}

		private void MenuListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{			
			if(MenuListBox.SelectedItem == null)
			{
				return;
			}
			string s = (MenuListBox.SelectedItem as ListBoxItem).Content.ToString();
			switch (s)
			{
				case "Sign In":
					SignIn();
					break;
				case "Sign Up":
					SignUp();
					break;
				case "Admin":
					Admin();
					break;
				case "Exit":
					Close();
					break;
			}
		}

		private void Admin()
		{
			if(User == null || User.RoleId != 1)
			{
				return;
			}
			Admin adminWnd = new Admin(Api, Token);
			adminWnd.ShowDialog();
			LoadCountries();
		}

		private void SignUp()
		{
			SignUp signUpWnd = new SignUp();
			signUpWnd.ShowDialog();
			if (signUpWnd.DialogResult == true)
			{
				Response<User> res = Task<Response<User>>.Factory.StartNew(() =>
				{
					WebRequest request = WebRequest.Create(Api);
					request.Method = "POST";
					string param = "registration";
					string md5 = CalculateMD5Hash(signUpWnd.Password);					
					string data = $"token={Token}&param={param}&login={signUpWnd.Login}&password={md5}&email={signUpWnd.Email}";
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
					var obj = JsonConvert.DeserializeObject<Response<User>>(responseJson);
					return obj;
				}).Result;
				if (res.result == 200)
				{
					User = res.rows[0];
					LoginTextBlock.Text = User.Login;
				}
				else
				{
					MessageBox.Show($"HTTP server response: {res.result}");
				}
			}
		}

		private string CalculateMD5Hash(string input)
		{
			// step 1, calculate MD5 hash from input
			MD5 md5 = System.Security.Cryptography.MD5.Create();
			byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
			byte[] hash = md5.ComputeHash(inputBytes);
			// step 2, convert byte array to hex string
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < hash.Length; i++)
			{
				sb.Append(hash[i].ToString("X2"));
			}
			return sb.ToString();
		}

		private void SignIn()
		{
			SignIn signInWnd = new SignIn();
			signInWnd.ShowDialog();
			if (signInWnd.DialogResult == true)
			{
				Response<User> res = Task<Response<User>>.Factory.StartNew(() =>
				{
					WebRequest request = WebRequest.Create(Api);
					request.Method = "POST";
					string param = "login";
					string login = signInWnd.Login;
					//TODO hash password
					string md5 = CalculateMD5Hash(signInWnd.Password);
					//string md5 = "202cb962ac59075b964b07152d234b70";
					string data = $"token={Token}&param={param}&login={login}&password={md5}";
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
					var obj = JsonConvert.DeserializeObject<Response<User>>(responseJson);
					return obj;
				}).Result;
				if (res.result == 200)
				{
					if (res.rows.Count == 0)
					{
						MessageBox.Show($"Not correct password!");
					}
					else
					{
						User = res.rows[0];
						LoginTextBlock.Text = User.Login;
					}
				}
				else
				{
					MessageBox.Show($"HTTP server response: {res.result}");
				}
			}
		}

		private void ComboBoxCities_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ListHotels.Visibility = Visibility.Visible;			
			if (ComboBoxCities.SelectedIndex != -1)
			{
				LoadHotels((ComboBoxCities.SelectedItem as City).Id);
			}
		}

		private void LoadHotels(int cityId)
		{
			Response<Hotel> res = Task<Response<Hotel>>.Factory.StartNew(() =>
			{
				WebRequest request = WebRequest.Create(Api);
				request.Method = "POST";
				string param = "getHotels";
				string data = $"token={Token}&param={param}&cityId={cityId}";
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

		private void ComboBoxCountries_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBoxCities.IsEnabled = true;
			ComboBoxCities.Items.Clear();
			ListHotels.Visibility = Visibility.Hidden;
			if (ComboBoxCountries.SelectedItem != null)
			{
				LoadCities((ComboBoxCountries.SelectedItem as Country).Id);
			}
		}

		
		private void LoadCities(int countryId)
		{
			Response<City> res = Task<Response<City>>.Factory.StartNew(() =>
			{
				WebRequest request = WebRequest.Create(Api);
				request.Method = "POST";
				string param = "getCities";
				string data = $"token={Token}&param={param}&countryId={countryId}";
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
			Response<Country> res = Task<Response<Country>>.Factory.StartNew(() =>
			{
				WebRequest request = WebRequest.Create(Api);
				request.Method = "POST";
				string param = "getCountries";
				string data = $"token={Token}&param={param}";
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
