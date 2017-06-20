namespace GPSTracker.Models
{
    public class Item : BaseDataObject
	{
		string text = string.Empty;
		public string Text
		{
			get { return text; }
			set { SetProperty(ref text, value); }
		}

		string description = string.Empty;
		public string Description
		{
			get { return description; }
			set { SetProperty(ref description, value); }
		}

		public double Longitude { get; set; }
		public double Latitude { get; set; }
	}
}
