namespace Exam_NP_TravelAPI
{
	public class Hotel
	{
		public int id { get; set; }
		public string hotelName { get; set; }
		public int cityId { get; set; }
		public int cost { get; set; }
		public string info { get; set; }
		public int stars;
		public string Stars
		{
			get { return new string('*', stars); }
			set { stars = value.Length; }
		}
	}
}