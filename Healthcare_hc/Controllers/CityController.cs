using HealtCare_Core.Managers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Healthcare_hc.Controllers
{
    public class CityController : Controller
    {

        private ICityManager _cityManager;
        private readonly ILogger<CityController> _logger;

        public CityController(ILogger<CityController> logger,
                              ICityManager cityManager)
        {
            _logger = logger;
            _cityManager = cityManager;
        }
        // GET: CityController
        [Route("api/city/GetCities")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult GetCities()
        { 
            var res = _cityManager.GetCities();
            return Ok(res);

        }

        //// GET: CityController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: CityController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: CityController/Create
        ////[HttpPost]
        ////[ValidateAntiForgeryToken]
        ////public ActionResult Create(IFormCollection collection)
        ////{
        ////    try
        ////    {
        ////        return RedirectToAction(nameof(Index));
        ////    }
        ////    catch
        ////    {
        ////        return View();
        ////    }
        ////}

        //// GET: CityController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: CityController/Edit/5
        ////[HttpPost]
        ////[ValidateAntiForgeryToken]
        ////public ActionResult Edit(int id, IFormCollection collection)
        ////{
        ////    try
        ////    {
        ////        return RedirectToAction(nameof(Index));
        ////    }
        ////    catch
        ////    {
        ////        return View();
        ////    }
        ////}

        //// GET: CityController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: CityController/Delete/5
        ////[HttpPost]
        ////[ValidateAntiForgeryToken]
        ////public ActionResult Delete(int id, IFormCollection collection)
        ////{
        ////    try
        ////    {
        ////        return RedirectToAction(nameof(Index));
        ////    }
        ////    catch
        ////    {
        ////        return View();
        ////    }
        ////}
    }
}
