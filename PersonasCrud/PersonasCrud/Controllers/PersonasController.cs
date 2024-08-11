using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using PersonasCrud.Models;

namespace PersonasCrud.Controllers
{
    public class PersonasController : Controller
    {
        private readonly PersonasContext _context;

        public PersonasController(PersonasContext context)
        {
            _context = context;
        }

        // GET: Personas
        public async Task<IActionResult> Index()
        {
            var personasContext = _context.Personas.Include(p => p.CodigoPaisNavigation);
            return View(await personasContext.ToListAsync());
        }

        // GET: Personas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Personas == null)
            {
                return NotFound();
            }

            var persona = await _context.Personas
                .Include(p => p.CodigoPaisNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // GET: Personas/Create
        public IActionResult Create()
        {
            ViewData["CodigoPais"] = new SelectList(_context.Pais, "CodigoPais", "CodigoPais");
            return View();
        }

        // POST: Personas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellidos,Documento,FechaNacimiento,Sexo,CodigoPais")] Persona persona)
        {
            if (ModelState.IsValid)
            {
                _context.Add(persona);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodigoPais"] = new SelectList(_context.Pais, "CodigoPais", "CodigoPais", persona.CodigoPais);
            return View(persona);
        }

        // GET: Personas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Personas == null)
            {
                return NotFound();
            }

            var persona = await _context.Personas.FindAsync(id);
            if (persona == null)
            {
                return NotFound();
            }
            ViewData["CodigoPais"] = new SelectList(_context.Pais, "CodigoPais", "CodigoPais", persona.CodigoPais);
            return View(persona);
        }

        // POST: Personas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellidos,Documento,FechaNacimiento,Sexo,CodigoPais")] Persona persona)
        {
            if (id != persona.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(persona);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonaExists(persona.Id))
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
            ViewData["CodigoPais"] = new SelectList(_context.Pais, "CodigoPais", "CodigoPais", persona.CodigoPais);
            return View(persona);
        }

        // GET: Personas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Personas == null)
            {
                return NotFound();
            }

            var persona = await _context.Personas
                .Include(p => p.CodigoPaisNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Personas == null)
            {
                return Problem("Entity set 'PersonasContext.Personas'  is null.");
            }
            var persona = await _context.Personas.FindAsync(id);
            if (persona != null)
            {
                _context.Personas.Remove(persona);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonaExists(int id)
        {
          return (_context.Personas?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public IActionResult DownloadExcel()
        {
            var userList = _context.Personas.ToList();  // Obtén la lista de usuarios desde tu base de datos

            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Personas");

                // Encabezados
                worksheet.Cells["A1"].Value = "Nombre";
                worksheet.Cells["B1"].Value = "Apellidos";
                worksheet.Cells["C1"].Value = "Documento";
                worksheet.Cells["D1"].Value = "Fecha de Nacimiento";
                worksheet.Cells["E1"].Value = "Sexo";
                worksheet.Cells["F1"].Value = "Pais";

                // Datos
                for (var i = 0; i < userList.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = userList[i].Nombre;
                    worksheet.Cells[i + 2, 2].Value = userList[i].Apellidos;
                    worksheet.Cells[i + 2, 3].Value = userList[i].Documento;
                    worksheet.Cells[i + 2, 4].Value = userList[i].FechaNacimiento;
                    worksheet.Cells[i + 2, 5].Value = userList[i].Sexo;
                    worksheet.Cells[i + 2, 6].Value = userList[i].CodigoPais;
                }

                var stream = new MemoryStream(package.GetAsByteArray());

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Personas.xlsx");
            }
        }
    }
}
