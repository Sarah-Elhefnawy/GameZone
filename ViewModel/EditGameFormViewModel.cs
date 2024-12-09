using GameZone.Attributes;

namespace GameZone.ViewModel
{
	public class EditGameFormViewModel : GameFormViewModel
	{
		public int Id { get; set; }

        public string? CurrentCover { get; set; }

        //validate extention and size
        //[Extension] -----> works best with apis but the mvc not the best as it creates few problems with the client side validation
        [AllowedExtensions(FileSettings.AllowedExtensions),
			MaxFileSize(FileSettings.MaxFileSizeInBytes)]
		public IFormFile? Cover { get; set; } = default!;
		//nullable --> ?
	}
}
