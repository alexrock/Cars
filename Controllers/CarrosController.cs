using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cars.Models;

namespace Cars.Controllers
{
    public class CarrosController : Controller
    {
        private readonly CarsContext _context;

        public CarrosController(CarsContext context)
        {
            _context = context;
        }

        // GET: Carros
        public async Task<IActionResult> Index()
        {
            return View(await _context.Carro.ToListAsync());
        }

        private void FillCarroAcessorios(Carro carro)
        {
            carro.CarroAcessorios = (
                from ca in _context.CarroAcessorio
                where ca.CarroId == carro.CarroId
                from a in _context.Acessorio
                where a.AcessorioId == ca.AcessorioId
                select new CarroAcessorio
                {
                    CarroAcessorioId = ca.CarroAcessorioId,
                    CarroId = ca.CarroId,
                    AcessorioId = ca.AcessorioId,
                    Acessorio = a
                }).ToList();
        }

        // GET: Carros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carro = await _context.Carro
                .FirstOrDefaultAsync(m => m.CarroId == id);
            if (carro == null)
            {
                return NotFound();
            }

            FillCarroAcessorios(carro);

            return View(carro);
        }

        // GET: Carros/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Carros/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarroId,Marca,DataCompra,Descricao,Cor")] Carro carro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(carro);
        }

        // GET: Carros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carro = await _context.Carro.FindAsync(id);
            if (carro == null)
            {
                return NotFound();
            }

            FillCarroAcessorios(carro);

            return View(carro);
        }

        // POST: Carros/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarroId,Marca,DataCompra,Descricao,Cor")] Carro carro)
        {
            if (id != carro.CarroId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarroExists(carro.CarroId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(carro);
        }

        // GET: Carros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carro = await _context.Carro
                .FirstOrDefaultAsync(m => m.CarroId == id);
            if (carro == null)
            {
                return NotFound();
            }

            return View(carro);
        }

        // POST: Carros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Carro carro = null;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    carro = await _context.Carro.FindAsync(id);

                    var carroAcessorios = (
                        from ca in _context.CarroAcessorio
                        where ca.CarroId == carro.CarroId
                        select ca);

                    _context.CarroAcessorio.RemoveRange(carroAcessorios);
                    await _context.SaveChangesAsync();

                    _context.Carro.Remove(carro);
                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                }
            }

            ViewData["Message"] = "Erro ao tentar excluir";

            return View(carro);
        }

        public async Task<IActionResult> DeleteItem(int id)
        {
            var acessorio = await _context.CarroAcessorio.FindAsync(id);
            _context.CarroAcessorio.Remove(acessorio);
            await _context.SaveChangesAsync();
            return Redirect($"/Carros/Edit/{acessorio.CarroId}");
        }


        public async Task<IActionResult> AddItem(int id)
        {
            var carro = await _context.Carro.FindAsync(id);
            carro.Acessorios = await _context.Acessorio.ToListAsync();

            return View(carro);
        }

        // POST: Carros/AddItem/5
        [HttpPost, ActionName("AddItem")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int id)
        {
            var carroId = int.Parse(Request.Form["carroId"]);

            var carroAcessorio = new CarroAcessorio
            {
                CarroId = carroId,
                AcessorioId = id
            };

            _context.CarroAcessorio.Add(carroAcessorio);
            await _context.SaveChangesAsync();
            return Redirect($"/Carros/Edit/{carroId}");
        }

        private bool CarroExists(int id)
        {
            return _context.Carro.Any(e => e.CarroId == id);
        }
    }
}