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
    // contains flow control logic for Diagnosis Category 
    //sends response when user makes a browser request for Diagnosis Category
    public class ASDiagnosisCategoryController : Controller
    {
        private readonly PatientsContext _context;

        // the context maintains the state of single module during the request life cycle
        public ASDiagnosisCategoryController(PatientsContext context)
        {
            _context = context;
        }

        // this get method is used to request data for ASDiagnosisCategory
        // GET: ASDiagnosisCategory
        public async Task<IActionResult> Index()
        {
            return View(await _context.DiagnosisCategory.ToListAsync());
        }

        //this is a get method for the details view, it requests data from the specified resource that is ASDiagnosisCategory
        //data to display details from the database
        // GET: ASDiagnosisCategory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diagnosisCategory = await _context.DiagnosisCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (diagnosisCategory == null)
            {
                return NotFound();
            }

            return View(diagnosisCategory);
        }

        //this is a get method for the create view, it requests data from the specified resource that is ASDiagnosisCategory
        //data to display data for create
        // GET: ASDiagnosisCategory/Create
        public IActionResult Create()
        {
            return View();
        }

        //this is a post method for create mode
        //it submits data to be processed to a specified resource
        //here it is ASDiagnosisCategory
        //it holds the value the user has given on views
        // POST: ASDiagnosisCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] DiagnosisCategory diagnosisCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(diagnosisCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(diagnosisCategory);
        }

        //this is a get method for the edit view, it requests data from the specified resource that is ASDiagnosisCategory
        //data to display data for edit
        // GET: ASDiagnosisCategory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diagnosisCategory = await _context.DiagnosisCategory.FindAsync(id);
            if (diagnosisCategory == null)
            {
                return NotFound();
            }
            return View(diagnosisCategory);
        }

        //this is a post method for edit mode
        //it submits data to be processed to a specified resource
        //here it is ASDiagnosisCategory
        //it holds the value the user has given on views
        // POST: ASDiagnosisCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] DiagnosisCategory diagnosisCategory)
        {
            if (id != diagnosisCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(diagnosisCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiagnosisCategoryExists(diagnosisCategory.Id))
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
            return View(diagnosisCategory);
        }

        //this is a get method for the delete view, it requests data from the specified resource that is ASDiagnosisCategory
        //data to display data for delete
        // GET: ASDiagnosisCategory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diagnosisCategory = await _context.DiagnosisCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (diagnosisCategory == null)
            {
                return NotFound();
            }

            return View(diagnosisCategory);
        }

        //this is a post method for delete mode
        //it submits data to be processed to a specified resource
        //here it is ASDiagnosisCategory
        //it holds the value the user has given on views
        //gives what happens when delete is requested by the user
        // POST: ASDiagnosisCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var diagnosisCategory = await _context.DiagnosisCategory.FindAsync(id);
            _context.DiagnosisCategory.Remove(diagnosisCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiagnosisCategoryExists(int id)
        {
            return _context.DiagnosisCategory.Any(e => e.Id == id);
        }
    }
}
