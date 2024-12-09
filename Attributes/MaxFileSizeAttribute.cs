namespace GameZone.Attributes
{
	public class MaxFileSizeAttribute:ValidationAttribute
	{
		//here i will add the validations by myself
		//the max size will not be static but will be expected as parameters

		//private field that will expect the max int size that is allowed
		private readonly int _maxFileSize;

		public MaxFileSizeAttribute(int maxFileSize)
		{
			_maxFileSize = maxFileSize;
		}

		protected override ValidationResult? IsValid
			(object? value, ValidationContext validationContext)
		{
			//value is object--> i need to to be IFormFile
			var file = value as IFormFile;

			//      the change is here only as will not be validating on the extension but the file siz
			if (file is not null)
			{
				//   length is not mega bytes but bytes only
				//    _maxFileSize is needed to be in bytes too
				if (file.Length > _maxFileSize)
				{
					return new ValidationResult(errorMessage: $"Maximun allowed size is {_maxFileSize} bytes");
				}
			}
			return ValidationResult.Success;
		}
	}
}
