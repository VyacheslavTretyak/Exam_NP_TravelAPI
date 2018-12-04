namespace Exam_NP_TravelAPI
{
	public class Hotel
	{
		public int Id { get; set; }
		public string HotelName { get; set; }
		public string CityName { get; set; }
		public string CountryName { get; set; }
		public int CountryId { get; set; }
		public int CityId { get; set; }
		public int Cost { get; set; }
		public string Info { get; set; }
		public int stars;
		public string Stars
		{
			get { return new string('*', stars); }
			set {
				stars = value.Length;
			}
		}
	}
}