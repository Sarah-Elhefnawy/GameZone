using GameZone.Attributes;

namespace GameZone.ViewModel
{
	public class CreateGameFormViewModel : GameFormViewModel
	{
		//validate extention and size
		//[Extension] -----> works best with apis but the mvc not the best as it creates few problems with the client side validation
		[AllowedExtensions(FileSettings.AllowedExtensions),
			MaxFileSize(FileSettings.MaxFileSizeInBytes)]
		public IFormFile Cover { get; set; } = default!;
        //IFormFile not string as in Game model cuz we recieve it from the form by this data type
    }
	
}
