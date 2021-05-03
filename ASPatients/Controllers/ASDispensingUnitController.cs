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
    // contains flow control logic for Dispensing Unit 
    //sends response when user makes a browser request for Dispensing unit
    public class ASDispensingUnitController : Controller
    {
        private readonly PatientsContext _context;

        // the context maintains the state of single module during the request life cycle
        public ASDispensingUnitController(PatientsContext context)
        {
            _context = context;
        }

        // this get method is used to request data for ASConcentrationUnit
        // GET: ASDispensingUnit
        public async Task<IActionResult> Index()
        {
            return View(await _context.DispensingUnit.ToListAsync());
        }

        //this is a get method for the details view, it requests data from the specified resource that is ASDispensingUnit
        //data to display details from the database
        // GET: ASDispensingUnit/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dispensingUnit = await _context.DispensingUnit
                .FirstOrDefaultAsync(m => m.DispensingCode == id);
            if (dispensingUnit == null)
            {
                return NotFound();
            }

            return View(dispensingUnit);
        }

        //this is a get method for the create view, it requests data from the specified resource that is ASDispensingUnit
        //data to display data for create
        // GET: ASDispensingUnit/Create
        public IActionResult Create()
        {
            return View();
        }

        //this is a post method for create mode
        //it submits data to be processed to a specified resource
        //here it is ASDispensingUnit
        //it holds the value the user has given on views
        // POST: ASDispensingUnit/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DispensingCode")] DispensingUnit dispensingUnit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dispensingUnit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dispensingUnit);
        }

        //this is a get method for the edit view, it requests data from the specified resource that is ASDispensingUnit
        //data to display data for edit
        // GET: ASDispensingUnit/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dispensingUnit = await _context.DispensingUnit.FindAsync(id);
            if (dispensingUnit == null)
            {
                return NotFound();
            }
            return View(dispensingUnit);
        }

        //this is a post method for edit mode
        //it submits data to be processed to a specified resource
        //here it is ASDispensingUnit
        //it holds the value the user has given on views
        // POST: ASDispensingUnit/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("DispensingCode")] DispensingUnit dispensingUnit)
        {
            if (id != dispensingUnit.DispensingCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dispensingUnit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DispensingUnitExists(dispensingUnit.DispensingCode))
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
            return View(dispensingUnit);
        }

        //this is a get method for the delete view, it requests data from the specified resource that is ASDispensingUnit
        //data to display data for delete
        // GET: ASDispensingUnit/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dispensingUnit = await _context.DispensingUnit
                .FirstOrDefaultAsync(m => m.DispensingCode == id);
            if (dispensingUnit == null)
            {
                return NotFound();
            }

            return View(dispensingUnit);
        }

        //this is a post method for delete mode
        //it submits data to be processed to a specified resource
        //here it is ASDispensingUnit
        //it holds the value the user has given on views
        //gives what happens when delete is requested by the user
        // POST: ASDispensingUnit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var dispensingUnit = await _context.DispensingUnit.FindAsync(id);
            _context.DispensingUnit.Remove(dispensingUnit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DispensingUnitExists(string id)
        {
            return _context.DispensingUnit.Any(e => e.DispensingCode == id);
        }
    }
}
