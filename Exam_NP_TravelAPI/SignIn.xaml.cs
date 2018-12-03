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
	public partial class SignIn : Window
	{
		public string Login { get; set; }
		public string Password { get; set; }
		public SignIn()
		{
			InitializeComponent();
			SignInButton.Click += SignInButton_Click;
		}

		private void SignInButton_Click(object sender, RoutedEventArgs e)
		{
			Login = LoginTextBox.Text;
			Password = PasswordTextBox.Password;
			DialogResult = true;
		}
	}
}
