using System;
using System.Collections.Generic;
using System.Linq;
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
	public partial class SignUp : Window
	{
		public string Login { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		public SignUp()
		{
			InitializeComponent();
			SignInButton.Click += SignInButton_Click;
			LoginTextBox.TextChanged += LoginTextBox_TextChanged;
			PasswordTextBox.PasswordChanged += PasswordTextBox_PasswordChanged;
			EmailTextBox.TextChanged += LoginTextBox_TextChanged;
			SignInButton.IsEnabled = false;
		}		

		private void PasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
		{
			SignInButton.IsEnabled = LoginTextBox.Text.Length > 0 && EmailTextBox.Text.Length > 0 && PasswordTextBox.Password.Length > 0;
		}

		private void LoginTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			SignInButton.IsEnabled = LoginTextBox.Text.Length > 0 && EmailTextBox.Text.Length > 0 && PasswordTextBox.Password.Length > 0;
		}

		private void SignInButton_Click(object sender, RoutedEventArgs e)
		{
			Login = LoginTextBox.Text;
			Password = PasswordTextBox.Password;
			Email = EmailTextBox.Text;
			DialogResult = true;
		}
	}
}
