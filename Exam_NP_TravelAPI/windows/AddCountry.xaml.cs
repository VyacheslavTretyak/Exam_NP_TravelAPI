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
	public partial class AddCountry : Window
	{
		public Country Country { get; set; }		
		public AddCountry()
		{
			InitializeComponent();
			SaveButton.Click += SaveButton_Click;
			CountryNameTextBox.TextChanged += CountryNameTextBox_TextChanged;
			SaveButton.IsEnabled = false;
			
		}

		private void CountryNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			SaveButton.IsEnabled = CountryNameTextBox.Text.Length > 0;
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			Country = new Country()
			{
				CountryName = CountryNameTextBox.Text
			};
			DialogResult = true;
		}
	}
}
