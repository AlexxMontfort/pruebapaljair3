using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Intenteo6.Models.dbModels;
using Intenteo6.Models;

namespace Intenteo6.Controllers
{
    public class CarroesController : Controller
    {
        private readonly DriveDreamDbContext _context;

        public CarroesController(DriveDreamDbContext context)
        {
            _context = context;
        }

        // GET: Carroes
        public async Task<IActionResult> Index()
        {
            var driveDreamDbContext = _context.Carros.Include(c => c.IdmodeloNavigation).Include(c => c.MarcaNavigation);
            return View(await driveDreamDbContext.ToListAsync());
        }

        // GET: Carroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carro = await _context.Carros
                .Include(c => c.IdmodeloNavigation)
                .Include(c => c.MarcaNavigation)
                .FirstOrDefaultAsync(m => m.IdCar == id);
            if (carro == null)
            {
                return NotFound();
            }

            return View(carro);
        }

        // GET: Carroes/Create
        public IActionResult Create()
        {
            var marcas = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Toyota" },
                new SelectListItem { Value = "2", Text = "Honda" },
                new SelectListItem { Value = "3", Text = "Ford" },
                new SelectListItem { Value = "4", Text = "Chevrolet" },
                new SelectListItem { Value = "5", Text = "BMW" },
                new SelectListItem { Value = "6", Text = "Mercedes" },
                new SelectListItem { Value = "7", Text = "Tesla" },
                new SelectListItem { Value = "8", Text = "Volkswagen" },
                new SelectListItem { Value = "9", Text = "Porsche" },
                new SelectListItem { Value = "10", Text = "Subaru" }
            };

            ViewData["Marcas"] = new SelectList(marcas, "Value", "Text");
            ViewData["Idmodelo"] = new SelectList(_context.Modelos, "IdModelo", "IdModelo");
            return View();
        }

        // POST: Carroes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCar,Name,Año,Idmodelo,Precio,Marca")] CarroCreateDTO carro)
        {
            if (ModelState.IsValid)
            {
                var carro1 = new Carro
                {
                    Name = carro.Name,
                    Año = carro.Año,
                    Idmodelo = carro.Idmodelo,
                    Precio = carro.Precio,
                    Marca = carro.Marca
                };

                _context.Carros.Add(carro1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var marcas = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Toyota" },
                new SelectListItem { Value = "2", Text = "Honda" },
                new SelectListItem { Value = "3", Text = "Ford" },
                new SelectListItem { Value = "4", Text = "Chevrolet" },
                new SelectListItem { Value = "5", Text = "BMW" },
                new SelectListItem { Value = "6", Text = "Mercedes" },
                new SelectListItem { Value = "7", Text = "Tesla" },
                new SelectListItem { Value = "8", Text = "Volkswagen" },
                new SelectListItem { Value = "9", Text = "Porsche" },
                new SelectListItem { Value = "10", Text = "Subaru" }
            };

            ViewData["Marcas"] = new SelectList(marcas, "Value", "Text", carro.Marca);
            ViewData["Idmodelo"] = new SelectList(_context.Modelos, "IdModelo", "IdModelo", carro.Idmodelo);
            return View(carro);
        }

        // GET: Carroes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carro = await _context.Carros.FindAsync(id);
            if (carro == null)
            {
                return NotFound();
            }
            ViewData["Idmodelo"] = new SelectList(_context.Modelos, "IdModelo", "IdModelo", carro.Idmodelo);
            ViewData["Marca"] = new SelectList(_context.Marcas, "IdMarca", "IdMarca", carro.Marca);
            return View(carro);
        }

        // POST: Carroes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCar,Name,Año,Idmodelo,Precio,Marca")] Carro carro)
        {
            if (id != carro.IdCar)
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
                    if (!CarroExists(carro.IdCar))
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
            ViewData["Idmodelo"] = new SelectList(_context.Modelos, "IdModelo", "IdModelo", carro.Idmodelo);
            ViewData["Marca"] = new SelectList(_context.Marcas, "IdMarca", "IdMarca", carro.Marca);
            return View(carro);
        }

        // GET: Carroes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carro = await _context.Carros
                .Include(c => c.IdmodeloNavigation)
                .Include(c => c.MarcaNavigation)
                .FirstOrDefaultAsync(m => m.IdCar == id);
            if (carro == null)
            {
                return NotFound();
            }

            return View(carro);
        }

        // POST: Carroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carro = await _context.Carros.FindAsync(id);
            if (carro != null)
            {
                _context.Carros.Remove(carro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarroExists(int id)
        {
            return _context.Carros.Any(e => e.IdCar == id);
        }
    }
}
