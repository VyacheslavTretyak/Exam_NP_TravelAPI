using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam_NP_TravelAPI
{
	public class Response<T>
	{
		public int result;
		public List<T> rows;
	}
}
