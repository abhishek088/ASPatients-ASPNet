using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPatients.Models;
using Microsoft.AspNetCore.Http;

namespace ASPatients.Controllers
{
    public class ASMedicationController : Controller
    {
        private readonly PatientsContext _context;

        public ASMedicationController(PatientsContext context)
        {
            _context = context;
        }

        // GET: ASMedication
        public async Task<IActionResult> Index(string medicationTypeId)
        {
            //url check
            if (medicationTypeId != null)
            {
                Response.Cookies.Append("MedicationTypeId", medicationTypeId);
                HttpContext.Session.SetString("MedicationTypeId", medicationTypeId);
            }
            //querystring check
            else if (Request.Query["medicationTypeId"].Any())
            {
                Response.Cookies.Append("MedicationTypeId", Request.Query["medicationTypeId"]);
                HttpContext.Session.SetString("MedicationTypeId", Request.Query["medicationTypeId"]);
                medicationTypeId = Request.Query["medicationTypeId"];
            }
            //cookie
            else if (Request.Cookies["MedicationTypeId"] != null)
            {
                medicationTypeId = Request.Cookies["MedicationTypeId"].ToString();
            }
            else if (HttpContext.Session.GetString("MedicationTypeId") != null)
            {
                medicationTypeId = HttpContext.Session.GetString("MedicationTypeId");
            }
            else
            {
                TempData["message"] = "Please Select Medication Type";
                return RedirectToAction("Index", "ASMedicationType");
            }

            var medName = _context.MedicationType.Where(a => a.MedicationTypeId.ToString() == medicationTypeId).FirstOrDefault();

            ViewData["MedicationTypeId"] = medicationTypeId;
            ViewData["MedicationType"] = medName.Name;

            var patientsContext = _context.Medication.Include(m => m.ConcentrationCodeNavigation)
                .Include(m => m.DispensingCodeNavigation).Include(m => m.MedicationType)
                .Where(a => a.MedicationTypeId.ToString() == medicationTypeId)
                .OrderBy(a => a.Name).ThenBy(a => a.Concentration);
            return View(await patientsContext.ToListAsync());
        }

        // GET: ASMedication/Details/5
        public async Task<IActionResult> Details(string id)
        {
            string medTName = string.Empty;
            if (Request.Cookies["MedicationTypeId"] != null)
            {
                medTName = Request.Cookies["MedicationTypeId"].ToString();
            }
            else if (HttpContext.Session.GetString("MedicationTypeId") != null)
            {
                medTName = HttpContext.Session.GetString("MedicationTypeId").ToString();
            }

            var medName = _context.MedicationType.Where(a => a.MedicationTypeId.ToString() == medTName).FirstOrDefault();

            ViewData["MedicationType"] = medName.Name;

            if (id == null)
            {
                return NotFound();
            }

            var medication = await _context.Medication
                .Include(m => m.ConcentrationCodeNavigation)
                .Include(m => m.DispensingCodeNavigation)
                .Include(m => m.MedicationType)
                .FirstOrDefaultAsync(m => m.Din == id);
            if (medication == null)
            {
                return NotFound();
            }

            return View(medication);
        }

        // GET: ASMedication/Create
        public IActionResult Create()
        {
            string medTName = string.Empty;
            if (Request.Cookies["MedicationTypeId"] != null)
            {
                medTName = Request.Cookies["MedicationTypeId"].ToString();
            }
            else if (HttpContext.Session.GetString("MedicationTypeId") != null)
            {
                medTName = HttpContext.Session.GetString("MedicationTypeId").ToString();
            }

            var medName = _context.MedicationType.Where(a => a.MedicationTypeId.ToString() == medTName).FirstOrDefault();

            ViewData["MedicationType"] = medName.Name;

            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnit.OrderBy(a => a.ConcentrationCode), "ConcentrationCode", "ConcentrationCode");
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnit.OrderBy(a => a.DispensingCode), "DispensingCode", "DispensingCode");
            ViewData["MedicationTypeId"] = new SelectList(_context.MedicationType, "MedicationTypeId", "Name");
            return View();
        }

        // POST: ASMedication/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Din,Name,Image,MedicationTypeId,DispensingCode,Concentration,ConcentrationCode")] Medication medication)
        {
            var isNameExists = _context.Medication.Any(a => a.Name == medication.Name);
            var isConcentrationExists = _context.Medication.Any(a => a.Concentration == medication.Concentration);
            var isConcentrationCodeExists = _context.Medication.Any(a => a.ConcentrationCode == medication.ConcentrationCode);

            if (isNameExists)
            {
                ModelState.AddModelError("", "Name already exists");
            }
            if (isConcentrationExists)
            {
                ModelState.AddModelError("", "Concentration already exists");
            }
            if (isConcentrationCodeExists)
            {
                ModelState.AddModelError("", "Concentration Code already exists");
            }

            string medTName = string.Empty;
            if (Request.Cookies["MedicationTypeId"] != null)
            {
                medTName = Request.Cookies["MedicationTypeId"].ToString();
            }
            else if (HttpContext.Session.GetString("MedicationTypeId") != null)
            {
                medTName = HttpContext.Session.GetString("MedicationTypeId").ToString();
            }

            var medName = _context.MedicationType.Where(a => a.MedicationTypeId.ToString() == medTName).FirstOrDefault();

            ViewData["MedicationType"] = medName.Name;

            if (ModelState.IsValid)
            {
                _context.Add(medication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnit.OrderBy(a => a.ConcentrationCode), "ConcentrationCode", "ConcentrationCode", medication.ConcentrationCode);
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnit.OrderBy(a => a.DispensingCode), "DispensingCode", "DispensingCode", medication.DispensingCode);
            ViewData["MedicationTypeId"] = new SelectList(_context.MedicationType, "MedicationTypeId", "Name", medication.MedicationTypeId);
            return View(medication);
        }

        // GET: ASMedication/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            string medTName = string.Empty;
            if (Request.Cookies["MedicationTypeId"] != null)
            {
                medTName = Request.Cookies["MedicationTypeId"].ToString();
            }
            else if (HttpContext.Session.GetString("MedicationTypeId") != null)
            {
                medTName = HttpContext.Session.GetString("MedicationTypeId").ToString();
            }

            var medName = _context.MedicationType.Where(a => a.MedicationTypeId.ToString() == medTName).FirstOrDefault();

            ViewData["MedicationType"] = medName.Name;

            if (id == null)
            {
                return NotFound();
            }

            var medication = await _context.Medication.FindAsync(id);
            if (medication == null)
            {
                return NotFound();
            }
            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnit, "ConcentrationCode", "ConcentrationCode", medication.ConcentrationCode);
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnit, "DispensingCode", "DispensingCode", medication.DispensingCode);
            ViewData["MedicationTypeId"] = new SelectList(_context.MedicationType, "MedicationTypeId", "Name", medication.MedicationTypeId);
            return View(medication);
        }

        // POST: ASMedication/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Din,Name,Image,MedicationTypeId,DispensingCode,Concentration,ConcentrationCode")] Medication medication)
        {
            string medTName = string.Empty;
            if (Request.Cookies["MedicationTypeId"] != null)
            {
                medTName = Request.Cookies["MedicationTypeId"].ToString();
            }
            else if (HttpContext.Session.GetString("MedicationTypeId") != null)
            {
                medTName = HttpContext.Session.GetString("MedicationTypeId").ToString();
            }

            var medName = _context.MedicationType.Where(a => a.MedicationTypeId.ToString() == medTName).FirstOrDefault();

            ViewData["MedicationType"] = medName.Name;

            if (id != medication.Din)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicationExists(medication.Din))
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
            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnit, "ConcentrationCode", "ConcentrationCode", medication.ConcentrationCode);
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnit, "DispensingCode", "DispensingCode", medication.DispensingCode);
            ViewData["MedicationTypeId"] = new SelectList(_context.MedicationType, "MedicationTypeId", "Name", medication.MedicationTypeId);
            return View(medication);
        }

        // GET: ASMedication/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            string medTName = string.Empty;
            if (Request.Cookies["MedicationTypeId"] != null)
            {
                medTName = Request.Cookies["MedicationTypeId"].ToString();
            }
            else if (HttpContext.Session.GetString("MedicationTypeId") != null)
            {
                medTName = HttpContext.Session.GetString("MedicationTypeId").ToString();
            }

            var medName = _context.MedicationType.Where(a => a.MedicationTypeId.ToString() == medTName).FirstOrDefault();

            ViewData["MedicationType"] = medName.Name;

            if (id == null)
            {
                return NotFound();
            }

            var medication = await _context.Medication
                .Include(m => m.ConcentrationCodeNavigation)
                .Include(m => m.DispensingCodeNavigation)
                .Include(m => m.MedicationType)
                .FirstOrDefaultAsync(m => m.Din == id);
            if (medication == null)
            {
                return NotFound();
            }

            return View(medication);
        }

        // POST: ASMedication/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            string medTName = string.Empty;
            if (Request.Cookies["MedicationTypeId"] != null)
            {
                medTName = Request.Cookies["MedicationTypeId"].ToString();
            }
            else if (HttpContext.Session.GetString("MedicationTypeId") != null)
            {
                medTName = HttpContext.Session.GetString("MedicationTypeId").ToString();
            }

            var medName = _context.MedicationType.Where(a => a.MedicationTypeId.ToString() == medTName).FirstOrDefault();

            ViewData["MedicationType"] = medName.Name;

            var medication = await _context.Medication.FindAsync(id);
            _context.Medication.Remove(medication);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicationExists(string id)
        {
            return _context.Medication.Any(e => e.Din == id);
        }
    }
}
