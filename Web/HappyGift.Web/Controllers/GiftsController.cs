namespace HappyGift.Web.Controllers
{
    using System.Threading.Tasks;

    using HappyGift.Services.Data;
    using HappyGift.Web.ViewModels.Gifts;
    using Microsoft.AspNetCore.Mvc;

    public class GiftsController : BaseController
    {
        private readonly IGiftsService giftsService;

        public GiftsController(IGiftsService giftsService)
        {
            this.giftsService = giftsService;
        }

        public async Task<IActionResult> Index()
        {
            var gifts = await this.giftsService.GetAllAsync<GiftViewModel>();
            var model = new GiftsListViewModel { Gifts = gifts };
            return this.View(model);
        }
    }
}
