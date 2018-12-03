using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam_NP_TravelAPI
{
	public class City
	{		
		public int Id { get; set; }
		public string CityName { get; set; }
		public int CountryId { get; set; }
		public string CountryName { get; set; }
		public override string ToString()
		{
			return CityName;
		}
	}
}
