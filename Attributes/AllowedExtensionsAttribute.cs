namespace GameZone.Attributes
{
	public class AllowedExtensionsAttribute:ValidationAttribute
	{
		//here i will add the validations by myself
		//the extensions will not be static but will be expected as parameters

		//private field that will expect the data that is allowed
		private readonly string _allowedExtensions;

        public AllowedExtensionsAttribute(string allowedExtensions)
        {
            _allowedExtensions = allowedExtensions;
        }

		protected override ValidationResult? IsValid
			(object? value, ValidationContext validationContext)
		{
			//value is object--> i need to to be IFormFile
			var file = value as IFormFile;

			if (file is not null)
			{
				//i need to compare the extension if is valid
				var extension = Path.GetExtension (file.FileName);
				//now i have the extension

				//var isAllowed = _allowedExtensions.Contains (extension);   ----> not right
				//cuz i wrote the allowed extensions as one string with commas
				//i need to convert it to array of strings, each element is an extension

				var isAllowed = _allowedExtensions.Split(separator:',').Contains(extension, StringComparer.OrdinalIgnoreCase);
				//StringComparer.OrdinalIgnoreCase---> cuz if the file is written as the extention is capitalized but extensions are all small(ex:jpeg)

				if(!isAllowed)
				{
					return new ValidationResult(errorMessage: $"Only {_allowedExtensions} are allowed!");
				}
			}
			return ValidationResult.Success;
		}

	}
}
