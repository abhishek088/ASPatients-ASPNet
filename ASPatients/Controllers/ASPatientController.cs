using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPatients.Models;

namespace ASPatients.Controllers
{
    public class ASPatientController : Controller
    {
        private readonly PatientsContext _context;

        public ASPatientController(PatientsContext context)
        {
            _context = context;
        }

        // GET: ASPatient
        public async Task<IActionResult> Index()
        {
            var patientsContext = _context.Patient.Include(p => p.ProvinceCodeNavigation)
                .OrderBy(a => a.LastName).ThenBy(a => a.FirstName);
            var deceased = _context.Patient.Where(p => p.Deceased == true).FirstOrDefault();
           
            return View(await patientsContext.ToListAsync());
        }

        // GET: ASPatient/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient
                .Include(p => p.ProvinceCodeNavigation)
                .FirstOrDefaultAsync(m => m.PatientId == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: ASPatient/Create
        public IActionResult Create()
        {
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "ProvinceCode");
            return View();
        }

        // POST: ASPatient/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientId,FirstName,LastName,Address,City,ProvinceCode,PostalCode,Ohip,DateOfBirth,Deceased,DateOfDeath,HomePhone,Gender")] Patient patient)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(patient);
                    await _context.SaveChangesAsync();
                    TempData["message"] = "New Patient Created Successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.InnerException.Message);
            }

            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "ProvinceCode", patient.ProvinceCode);
            return View(patient);
        }

        // GET: ASPatient/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            ViewData["ProvinceCode"] = new SelectList(_context.Province.OrderBy(a => a.Name), "ProvinceCode", "ProvinceCode", patient.ProvinceCode);
            return View(patient);
        }

        // POST: ASPatient/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientId,FirstName,LastName,Address,City,ProvinceCode,PostalCode,Ohip,DateOfBirth,Deceased,DateOfDeath,HomePhone,Gender")] Patient patient)
        {
            if (id != patient.PatientId)
            {
                return NotFound();
            }
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(patient);
                        await _context.SaveChangesAsync();
                        TempData["message"] = "Patient Edited Successfully";
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!PatientExists(patient.PatientId))
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
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.InnerException.Message);
            }
            
            ViewData["ProvinceCode"] = new SelectList(_context.Province.OrderBy(a => a.Name), "ProvinceCode", "ProvinceCode", patient.ProvinceCode);
            return View(patient);
        }

        // GET: ASPatient/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient
                .Include(p => p.ProvinceCodeNavigation)
                .FirstOrDefaultAsync(m => m.PatientId == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: ASPatient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patient.FindAsync(id);
            try
            {
                _context.Patient.Remove(patient);
                await _context.SaveChangesAsync();
                TempData["message"] = "Pateint Deleted Successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.InnerException.Message;
            }
            return View(patient);
            
        }

        private bool PatientExists(int id)
        {
            return _context.Patient.Any(e => e.PatientId == id);
        }
    }
}
