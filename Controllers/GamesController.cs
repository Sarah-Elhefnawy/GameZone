namespace GameZone.Controllers
{
    public class GamesController : Controller
    {
        // 1       in order to talk with the database we are trying to figure if there really is a problem to change data from domain model to SelectListitem
        //private readonly ApplicationDbContext _context;
        //no need as servise is dealing with database not controller

        //injecting my new service
        private readonly ICategoriesService _categoriesService;
		//then control dot to choose ---> add parameters

		//injecting my new service
		private readonly IDevicesService _devicesService;
		//then control dot to choose ---> add parameters

        //to save cover file inside the server
		private readonly IGamesService _gamesService;

		public GamesController(ICategoriesService categoriesService,
		IDevicesService devicesService,
		IGamesService gamesService)
		{
			_categoriesService = categoriesService;
			_devicesService = devicesService;
			_gamesService = gamesService;
		}

        public IActionResult Index()
        {
            var games = _gamesService.GetAll();
            return View(games);
        }

		public IActionResult Details(int id)
		{
			var game = _gamesService.GetById(id);
			if(game is null)
				return NotFound();

			return View(game);
		}

        //if httpGet not written then it is by default Get
        [HttpGet]
        public IActionResult Create()
        {
            // 2      in order to talk with the database we are trying to figure if there really is a problem to change data from domain model to SelectListitem
            //var categories = _context.Categories.ToList();

            CreateGameFormViewModel viewModel = new()
            {
				// 3      in order to talk with the database we are trying to figure if there really is a problem to change data from domain model to SelectListitem
				//Categories = categories;        the same 500 error occured so that will not fix it


				Categories = _categoriesService.GetSelectList(),
				Devices = _devicesService.GetSelectList()
			};
            return View(viewModel);
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateGameFormViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.Categories = _categoriesService.GetSelectList();
				model.Devices = _devicesService.GetSelectList();

				//if not valid he returns to the form
				return View(model);
            }

			/*
             *        if valid (18 in playlist)
             * 1) Save Game to the database
             * 2) Save cover to server
             * return to games form
            */

			/* you need to remember that create in an ASYNCRUS so we need to change the whoe function to be ASYNCRUS
             * function will be "async Task<IActionResult>" instead of "IActionResult"
             * don't forget to validate the cover size and extention
             */
			await _gamesService.Create(model);

			//return RedirectToAction(actionName:"Index");
			//if written like this in type string ---> if action name changed will be a problem to figure the change where

			return RedirectToAction(nameof(Index));
			//if written like this in type string ---> if action name changed will send warning saying that action name changed 

		}

		[HttpGet]
		public IActionResult Edit(int id)
		{
			var game = _gamesService.GetById(id);
			if (game is null)
				return NotFound();

			//i need to make a form for the edit process

			//initialize the values that the viewmodel needs
			// could be done by the automapper or any packages you can doenload but we are doing manual mapping
			// of these values between domain model(game) to the viewmodel(EditGameFormViewModel)
			EditGameFormViewModel viewModel = new()
			{
				Id = id,
				Name = game.Name,
				Description = game.Description,
				CategoryId = game.CategoryId,
				SelectedDevices = game.Devices.Select(d => d.DeviceId).ToList(),
				Categories = _categoriesService.GetSelectList(),
				Devices = _devicesService.GetSelectList(),
				CurrentCover = game.Cover
			};

			return View(viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(EditGameFormViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.Categories = _categoriesService.GetSelectList();
				model.Devices = _devicesService.GetSelectList();

				//if not valid he returns to the form
				return View(model);
			}

			var game = await _gamesService.Update(model);

			if (game is null)
				return BadRequest();
			
			return RedirectToAction(nameof(Index));
			//if written like this in type string ---> if action name changed will send warning saying that action name changed 

		}

		[HttpDelete]
		public IActionResult Delete(int id)
		{

			var isDeleted = _gamesService.Delete(id);

			return isDeleted ? Ok() : BadRequest();
		}
	}
}
