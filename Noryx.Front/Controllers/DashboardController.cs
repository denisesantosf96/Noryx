using Microsoft.AspNetCore.Mvc;
using Noryx.Front.Services;

namespace Noryx.Front.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ServiceApi _apiService;

        public DashboardController(ServiceApi apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var cotacao = await _apiService.GetCotacao("USD", "BRL");
            return View(cotacao);
        }
    }
}
