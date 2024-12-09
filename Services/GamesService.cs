
namespace GameZone.Services
{
	public class GamesService : IGamesService
	{
		private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;
		/* since i need to use this path more than one time 
		 * then i will add a field to prevent dublication
		 */
		private readonly string _imagesPath;

		public GamesService(ApplicationDbContext context,
		IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_webHostEnvironment = webHostEnvironment;
			// WebRootPath==> i am telling him to ge to the images folder inside www
			// "/assets/images/games" is considered hard coded wich is not the best
			// cuz you will use it again in multible places and will need to alter more than 1 time
			_imagesPath = $"{_webHostEnvironment.WebRootPath}{FileSettings.ImagesPath}";
			//i use ${} to combine variables into one variables
		}

		public IEnumerable<Game> GetAll()
		{
			return _context.Games
				.Include(g => g.Category)
				.Include(g => g.Devices)
				.ThenInclude(d => d.Device)
				.AsNoTracking()
				.ToList();
			 
		}
        public Game? GetById(int id)
        {
			return _context.Games
				.Include(g => g.Category)
				.Include(g => g.Devices)
				.ThenInclude(d => d.Device)
				.AsNoTracking()
				.SingleOrDefault(g => g.Id == id);
        }

        public async Task Create(CreateGameFormViewModel model)
		{
			var coverName = await SaveCover(model.Cover);

			Game game = new()
			{
				//here are initializations

				Name = model.Name,
				Description = model.Description,
				CategoryId = model.CategoryId,
				Cover = coverName,


				/* ===> i need to do the same technique as we did when we initialized of select list we used .Select to change datatype to a different type(PROJECTION)
				 * selecteddevices is technically a list of integers but i want to change it to list of <GameDevice> of model
				 * "d" for Device, "=>" goes to, "new GameDevice" initialization, 
				 * "DeviceId" the only value needed the initialization cuz gameId will be assigned automatically to the game which the user is creating,
				 * "=d" which is the the current chosen id, ".ToList" to change it to list type
				 */
				Devices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList()
			};

			//.net is smart enough to understand that when you assign to game then it means save inside games
			_context.Add(game);
			_context.SaveChanges();
		}

		public async Task<Game?> Update(EditGameFormViewModel model)
		{
			//select game to update it and to make sure that id is correct as it could be changed
			var game = _context.Games
				.Include(g => g.Devices)
				.SingleOrDefault(g => g.Id == model.Id);

			if (game is null)
				return null;

			var hasNewCover = model.Cover is not null;
			var oldCover = game.Cover;
			
			//alter date in database
			game.Name = model.Name;
			game.Description= model.Description;
			game.CategoryId = model.CategoryId;
			game.Devices=model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList();

			if(hasNewCover)
			{
				game.Cover = await SaveCover(model.Cover!);
			}

			var effectedRows = _context.SaveChanges();

			if(effectedRows> 0)
			{
				//i need to delete old photo if there was a new photo
				if(hasNewCover)
				{
					var cover = Path.Combine(_imagesPath, oldCover);
					File.Delete(cover);
				}

				return game;
			}
			else
			{
				//return null and delete the new phtot as it is already updated in database
				var cover = Path.Combine(_imagesPath, game.Cover);
				File.Delete(cover);

				return null;
			}

		}
		public bool Delete(int id)
		{
			var isDeleted = false;

			var game = _context.Games.Find(id);

			if(game is null)
				return isDeleted;

			_context.Remove(game);

			var effectedRows = _context.SaveChanges();

			if (effectedRows > 0)
			{
				//deletion process is finished successfully
				isDeleted = true;

				var cover= Path.Combine(_imagesPath, game.Cover);

				File.Delete(cover);
			}

			return isDeleted;
		}

		private async Task<string> SaveCover(IFormFile cover)
		{
			var coverName = $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}";

			//this is the place where the cover is saved
			//   path.combine(place where image is saved, image name)
			var path = Path.Combine(_imagesPath, coverName);

			//to save the cover inside the server
			using var stream = File.Create(path);
			await cover.CopyToAsync(stream);

			return coverName;
		}

	}
}
