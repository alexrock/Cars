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
    public class AcessoriosController : Controller
    {
        private readonly CarsContext _context;

        public AcessoriosController(CarsContext context)
        {
            _context = context;
        }

        // GET: Acessorios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Acessorio.ToListAsync());
        }

        // GET: Acessorios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var acessorio = await _context.Acessorio
                .FirstOrDefaultAsync(m => m.AcessorioId == id);
            if (acessorio == null)
            {
                return NotFound();
            }

            return View(acessorio);
        }

        // GET: Acessorios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Acessorios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AcessorioId,Nome")] Acessorio acessorio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(acessorio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(acessorio);
        }

        // GET: Acessorios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var acessorio = await _context.Acessorio.FindAsync(id);
            if (acessorio == null)
            {
                return NotFound();
            }
            return View(acessorio);
        }

        // POST: Acessorios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AcessorioId,Nome")] Acessorio acessorio)
        {
            if (id != acessorio.AcessorioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(acessorio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AcessorioExists(acessorio.AcessorioId))
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
            return View(acessorio);
        }

        // GET: Acessorios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var acessorio = await _context.Acessorio
                .FirstOrDefaultAsync(m => m.AcessorioId == id);
            if (acessorio == null)
            {
                return NotFound();
            }

            return View(acessorio);
        }

        // POST: Acessorios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Acessorio acessorio = null;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    acessorio = await _context.Acessorio.FindAsync(id);

                    var count = await (
                        from ca in _context.CarroAcessorio
                        where ca.AcessorioId == acessorio.AcessorioId
                        select ca).CountAsync();

                    if (count == 0)
                    {
                        _context.Acessorio.Remove(acessorio);
                        await _context.SaveChangesAsync();

                        transaction.Commit();

                        return RedirectToAction(nameof(Index));
                    }
                }
                catch
                {
                }
            }

            ViewData["Message"] = "Acessório em uso";

            return View(acessorio);
        }

        private bool AcessorioExists(int id)
        {
            return _context.Acessorio.Any(e => e.AcessorioId == id);
        }
    }
}