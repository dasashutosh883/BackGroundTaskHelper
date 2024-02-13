using BackgroundTaskHelper.ActionFilters;
using BackgroundTaskHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BackgroundTaskHelper.Controllers
{
    
    public class HomeController : BaseController
    {

        public HomeController(GlobalListManager<LogModel> listManager) :base(listManager)
        {
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Show( int a,int b)
        {
            try
            {
                int c = a / b;
                return Json(new { sucess = true, responseMessage = c.ToString(), responseText = "Sucess", data = c });
            }
            catch (Exception ex)
            {
                return Json(new { sucess = true, responseMessage = ex.Message, responseText = "Sucess", data = ex.Message }); 
            }
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}