using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam_NP_TravelAPI
{
	class City
	{
		public int id;
		public string cityName;
		public int countryId;
		public override string ToString()
		{
			return cityName;
		}
	}
}
