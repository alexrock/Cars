using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cars.Models;
using Microsoft.Extensions.Configuration;

namespace Cars.Controllers
{
    public class HomeController : Controller
    {
        private readonly CarsContext _context;

        public HomeController(CarsContext db)
        {
            this._context = db;
        }

        public IActionResult Index()
        {
            if (Request.HasFormContentType)
            {
                var consulta = Request.Form["consulta"].ToString();

                if (consulta.Length > 0)
                {
                    var carro = (
                        from c in _context.Carro
                        where c.Descricao.Contains(consulta) || c.Marca.Contains(consulta)
                        select c).FirstOrDefault();

                    if (carro != null)
                    {
                        return Redirect($"/Carros/Details/{carro.CarroId}");
                    }

                    ViewData["Message"] = "Nenhum registro encontrado para a consulta: " + consulta;

                    return View();
                }

                return View();
            }

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}