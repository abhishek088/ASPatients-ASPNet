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
    // contains flow control logic for Medication Type
    //sends response when user makes a browser request for Medication Type
    public class ASMedicationTypeController : Controller
    {
        private readonly PatientsContext _context;

        // the context maintains the state of single module during the request life cycle
        public ASMedicationTypeController(PatientsContext context)
        {
            _context = context;
        }

        // this get method is used to request data for ASMedicationType
        // GET: ASMedicationType
        public async Task<IActionResult> Index()
        {
            return View(await _context.MedicationType.OrderBy(x => x.Name).ToListAsync());
        }

        //this is a get method for the details view, it requests data from the specified resource that is ASMedicationType
        //data to display details from the database
        // GET: ASMedicationType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationType = await _context.MedicationType
                .FirstOrDefaultAsync(m => m.MedicationTypeId == id);
            if (medicationType == null)
            {
                return NotFound();
            }

            return View(medicationType);
        }

        //this is a get method for the create view, it requests data from the specified resource that is ASMedicationType
        //data to display data for create
        // GET: ASMedicationType/Create
        public IActionResult Create()
        {
            return View();
        }

        //this is a post method for create mode
        //it submits data to be processed to a specified resource
        //here it is ASMedicationType
        //it holds the value the user has given on views
        // POST: ASMedicationType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MedicationTypeId,Name")] MedicationType medicationType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicationType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(medicationType);
        }

        //this is a get method for the edit view, it requests data from the specified resource that is ASMedicationType
        //data to display data for edit
        // GET: ASMedicationType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationType = await _context.MedicationType.FindAsync(id);
            if (medicationType == null)
            {
                return NotFound();
            }
            return View(medicationType);
        }

        //this is a post method for edit mode
        //it submits data to be processed to a specified resource
        //here it is ASMedicationType
        //it holds the value the user has given on views
        // POST: ASMedicationType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MedicationTypeId,Name")] MedicationType medicationType)
        {
            if (id != medicationType.MedicationTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicationType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicationTypeExists(medicationType.MedicationTypeId))
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
            return View(medicationType);
        }

        //this is a get method for the delete view, it requests data from the specified resource that is ASMedicationType
        //data to display data for delete
        // GET: ASMedicationType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationType = await _context.MedicationType
                .FirstOrDefaultAsync(m => m.MedicationTypeId == id);
            if (medicationType == null)
            {
                return NotFound();
            }

            return View(medicationType);
        }

        //this is a post method for delete mode
        //it submits data to be processed to a specified resource
        //here it is ASMedicationType
        //it holds the value the user has given on views
        //gives what happens when delete is requested by the user
        // POST: ASMedicationType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicationType = await _context.MedicationType.FindAsync(id);
            _context.MedicationType.Remove(medicationType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicationTypeExists(int id)
        {
            return _context.MedicationType.Any(e => e.MedicationTypeId == id);
        }
    }
}
