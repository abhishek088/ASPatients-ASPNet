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
    public class ASPatientTreatmentController : Controller
    {
        private readonly PatientsContext _context;

        public ASPatientTreatmentController(PatientsContext context)
        {
            _context = context;
        }

        // GET: ASPatientTreatment
        public async Task<IActionResult> Index(string patientDiagnosisId)
        {
            //url check
            if (patientDiagnosisId != null)
            {
                Response.Cookies.Append("PatientDiagnosisId", patientDiagnosisId);
                HttpContext.Session.SetString("PatientDiagnosisId", patientDiagnosisId);
            }
            //querystring check
            else if (Request.Query["patientDiagnosisId"].Any())
            {
                Response.Cookies.Append("PatientDiagnosisId", Request.Query["patientDiagnosisId"]);
                HttpContext.Session.SetString("PatientDiagnosisId", Request.Query["patientDiagnosisId"]);
                patientDiagnosisId = Request.Query["patientDiagnosisId"];
            }
            //cookie
            else if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                patientDiagnosisId = Request.Cookies["PatientDiagnosisId"].ToString();
            }
            else if (HttpContext.Session.GetString("PatientDiagnosisId") != null)
            {
                patientDiagnosisId = HttpContext.Session.GetString("PatientDiagnosisId");
            }
            else
            {
                TempData["message"] = "Please Select Patient Diagnosis";
                return RedirectToAction("Index", "ASPatientDiagnosis");
            }
            //_context.PatientDiagnosis.Include(p => p.Diagnosis).Include(p => p.Patient).OrderBy(p => p.Patient.LastName).ThenBy(p => p.Patient.FirstName).ThenBy(p => p.Diagnosis.Name);
            var patientName = _context.PatientDiagnosis.Include(p => p.Diagnosis).Include(p => p.Patient)
                .Where(a => a.PatientDiagnosisId.ToString() == patientDiagnosisId).FirstOrDefault();

            var patientsContext = _context.PatientTreatment.Include(p => p.PatientDiagnosis).Include(p => p.Treatment)
                .Where(p => p.PatientDiagnosisId.ToString() == patientDiagnosisId)
                .OrderByDescending(p => p.DatePrescribed);

            ViewData["patientLastName"] = patientName.Patient.LastName;
            ViewData["patientFirstName"] = patientName.Patient.FirstName;
            ViewData["treatmentFor"] = patientName.Diagnosis.Name;
            return View(await patientsContext.ToListAsync());
        }

        // GET: ASPatientTreatment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            string diagnosisId = string.Empty;
            if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                diagnosisId = Request.Cookies["PatientDiagnosisId"].ToString();
            }
            else if (HttpContext.Session.GetString("PatientDiagnosisId") != null)
            {
                diagnosisId = HttpContext.Session.GetString("PatientDiagnosisId").ToString();
            }

            var patientName = _context.PatientDiagnosis.Include(p => p.Diagnosis).Include(p => p.Patient)
                .Where(a => a.PatientDiagnosisId.ToString() == diagnosisId).FirstOrDefault();

            ViewData["patientLastName"] = patientName.Patient.LastName;
            ViewData["patientFirstName"] = patientName.Patient.FirstName;
            ViewData["treatmentFor"] = patientName.Diagnosis.Name;

            if (id == null)
            {
                return NotFound();
            }

            var patientTreatment = await _context.PatientTreatment
                .Include(p => p.PatientDiagnosis)
                .Include(p => p.Treatment)
                .FirstOrDefaultAsync(m => m.PatientTreatmentId == id);
            if (patientTreatment == null)
            {
                return NotFound();
            }

            return View(patientTreatment);
        }

        // GET: ASPatientTreatment/Create
        public IActionResult Create()
        {
            string diagnosisId = string.Empty;
            if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                diagnosisId = Request.Cookies["PatientDiagnosisId"].ToString();
            }
            else if (HttpContext.Session.GetString("PatientDiagnosisId") != null)
            {
                diagnosisId = HttpContext.Session.GetString("PatientDiagnosisId").ToString();
            }

            var patientName = _context.PatientDiagnosis.Include(p => p.Diagnosis).Include(p => p.Patient)
                .Where(a => a.PatientDiagnosisId.ToString() == diagnosisId).FirstOrDefault();

            ViewData["patientLastName"] = patientName.Patient.LastName;
            ViewData["patientFirstName"] = patientName.Patient.FirstName;
            ViewData["treatmentFor"] = patientName.Diagnosis.Name;

            

            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnosis, "PatientDiagnosisId", "PatientDiagnosisId");
            ViewData["TreatmentId"] = new SelectList(_context.Treatment.Where(a => a.DiagnosisId.ToString() == diagnosisId), "TreatmentId", "Name");
            return View();
        }

        // POST: ASPatientTreatment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientTreatmentId,TreatmentId,DatePrescribed,Comments,PatientDiagnosisId")] PatientTreatment patientTreatment)
        {
            string diagnosisId = string.Empty;
            if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                diagnosisId = Request.Cookies["PatientDiagnosisId"].ToString();
            }
            else if (HttpContext.Session.GetString("PatientDiagnosisId") != null)
            {
                diagnosisId = HttpContext.Session.GetString("PatientDiagnosisId").ToString();
            }

            var patientName = _context.PatientDiagnosis.Include(p => p.Diagnosis).Include(p => p.Patient)
                .Where(a => a.PatientDiagnosisId.ToString() == diagnosisId).FirstOrDefault();

            ViewData["patientLastName"] = patientName.Patient.LastName;
            ViewData["patientFirstName"] = patientName.Patient.FirstName;
            ViewData["treatmentFor"] = patientName.Diagnosis.Name;

            if (ModelState.IsValid)
            {
                _context.Add(patientTreatment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnosis, "PatientDiagnosisId", "PatientDiagnosisId", patientTreatment.PatientDiagnosisId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatment.Where(a => a.DiagnosisId.ToString() == diagnosisId), "TreatmentId", "Name", patientTreatment.TreatmentId);
            return View(patientTreatment);
        }

        // GET: ASPatientTreatment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            string diagnosisId = string.Empty;
            if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                diagnosisId = Request.Cookies["PatientDiagnosisId"].ToString();
            }
            else if (HttpContext.Session.GetString("PatientDiagnosisId") != null)
            {
                diagnosisId = HttpContext.Session.GetString("PatientDiagnosisId").ToString();
            }

            var patientName = _context.PatientDiagnosis.Include(p => p.Diagnosis).Include(p => p.Patient)
                .Where(a => a.PatientDiagnosisId.ToString() == diagnosisId).FirstOrDefault();

            ViewData["patientLastName"] = patientName.Patient.LastName;
            ViewData["patientFirstName"] = patientName.Patient.FirstName;
            ViewData["treatmentFor"] = patientName.Diagnosis.Name;

            if (id == null)
            {
                return NotFound();
            }

            var patientTreatment = await _context.PatientTreatment.FindAsync(id);
            if (patientTreatment == null)
            {
                return NotFound();
            }
            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnosis, "PatientDiagnosisId", "PatientDiagnosisId", patientTreatment.PatientDiagnosisId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatment.Where(a => a.DiagnosisId.ToString() == diagnosisId), "TreatmentId", "Name", patientTreatment.TreatmentId);
            return View(patientTreatment);
        }

        // POST: ASPatientTreatment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientTreatmentId,TreatmentId,DatePrescribed,Comments,PatientDiagnosisId")] PatientTreatment patientTreatment)
        {
            string diagnosisId = string.Empty;
            if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                diagnosisId = Request.Cookies["PatientDiagnosisId"].ToString();
            }
            else if (HttpContext.Session.GetString("PatientDiagnosisId") != null)
            {
                diagnosisId = HttpContext.Session.GetString("PatientDiagnosisId").ToString();
            }

            var patientName = _context.PatientDiagnosis.Include(p => p.Diagnosis).Include(p => p.Patient)
                .Where(a => a.PatientDiagnosisId.ToString() == diagnosisId).FirstOrDefault();

            ViewData["patientLastName"] = patientName.Patient.LastName;
            ViewData["patientFirstName"] = patientName.Patient.FirstName;
            ViewData["treatmentFor"] = patientName.Diagnosis.Name;

            if (id != patientTreatment.PatientTreatmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patientTreatment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientTreatmentExists(patientTreatment.PatientTreatmentId))
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
            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnosis, "PatientDiagnosisId", "PatientDiagnosisId", patientTreatment.PatientDiagnosisId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatment.Where(a => a.DiagnosisId.ToString() == diagnosisId), "TreatmentId", "Name", patientTreatment.TreatmentId);
            return View(patientTreatment);
        }

        // GET: ASPatientTreatment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            string diagnosisId = string.Empty;
            if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                diagnosisId = Request.Cookies["PatientDiagnosisId"].ToString();
            }
            else if (HttpContext.Session.GetString("PatientDiagnosisId") != null)
            {
                diagnosisId = HttpContext.Session.GetString("PatientDiagnosisId").ToString();
            }

            var patientName = _context.PatientDiagnosis.Include(p => p.Diagnosis).Include(p => p.Patient)
                .Where(a => a.PatientDiagnosisId.ToString() == diagnosisId).FirstOrDefault();

            ViewData["patientLastName"] = patientName.Patient.LastName;
            ViewData["patientFirstName"] = patientName.Patient.FirstName;
            ViewData["treatmentFor"] = patientName.Diagnosis.Name;

            if (id == null)
            {
                return NotFound();
            }

            var patientTreatment = await _context.PatientTreatment
                .Include(p => p.PatientDiagnosis)
                .Include(p => p.Treatment)
                .FirstOrDefaultAsync(m => m.PatientTreatmentId == id);
            if (patientTreatment == null)
            {
                return NotFound();
            }

            return View(patientTreatment);
        }

        // POST: ASPatientTreatment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string diagnosisId = string.Empty;
            if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                diagnosisId = Request.Cookies["PatientDiagnosisId"].ToString();
            }
            else if (HttpContext.Session.GetString("PatientDiagnosisId") != null)
            {
                diagnosisId = HttpContext.Session.GetString("PatientDiagnosisId").ToString();
            }

            var patientName = _context.PatientDiagnosis.Include(p => p.Diagnosis).Include(p => p.Patient)
                .Where(a => a.PatientDiagnosisId.ToString() == diagnosisId).FirstOrDefault();

            ViewData["patientLastName"] = patientName.Patient.LastName;
            ViewData["patientFirstName"] = patientName.Patient.FirstName;
            ViewData["treatmentFor"] = patientName.Diagnosis.Name;

            var patientTreatment = await _context.PatientTreatment.FindAsync(id);
            _context.PatientTreatment.Remove(patientTreatment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientTreatmentExists(int id)
        {
            return _context.PatientTreatment.Any(e => e.PatientTreatmentId == id);
        }
    }
}
