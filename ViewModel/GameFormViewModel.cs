namespace GameZone.ViewModel
{
	public class GameFormViewModel
	{
		[MaxLength(length: 250)]
		public string Name { get; set; } = string.Empty;

		[Display(Name = "Category")]
		public int CategoryId { get; set; }

		public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();
		//IEnumerable as the user won't choose from integers and send it to the database 
		//the user choose one string item from the drop down list or select list

		[Display(Name = "Supported Devices")]
		public List<int> SelectedDevices { get; set; } = default!;

		public IEnumerable<SelectListItem> Devices { get; set; } = Enumerable.Empty<SelectListItem>();
		//IEnumerable as the user won't choose from list of integers and send them to the database 
		//the user choose multible string items from the drop down list or select list

		[MaxLength(length: 2500)]
		public string Description { get; set; } = string.Empty;
	}
}
