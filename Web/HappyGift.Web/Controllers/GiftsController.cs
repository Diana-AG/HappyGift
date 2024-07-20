namespace HappyGift.Web.Controllers
{
    using HappyGift.Data.Models;
    using HappyGift.Services.Data;
    using Microsoft.AspNetCore.Mvc;

    public class GiftsController : BaseController
    {
        private readonly IGiftsService giftsService;

        public GiftsController(IGiftsService giftsService)
        {
            this.giftsService = giftsService;
        }

        public IActionResult Index()
        {
            var gifts = this.giftsService.GetAllAsync<Gift>();

            return this.View(gifts);
        }
    }
}
