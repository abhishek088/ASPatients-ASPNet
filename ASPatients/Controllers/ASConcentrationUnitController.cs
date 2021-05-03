using ASPatients.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ASPatients.Controllers
{
    // contains flow control logic for Concentration Unit 
    //sends response when user makes a browser request for concentration unit
    public class ASConcentrationUnitController : Controller
    {
        private readonly PatientsContext _context;

        // the context maintains the state of single module during the request life cycle
        public ASConcentrationUnitController(PatientsContext context)
        {
            _context = context;
        }

        // GET: ASConcentrationUnit
        // this get method is used to request data for ASConcentrationUnit
        public async Task<IActionResult> Index()
        {
            return View(await _context.ConcentrationUnit.ToListAsync());
        }

        //this is a get method for the details view, it requests data from the specified resource that is ASConcentrationUnit
        //data to display details from the database
        // GET: ASConcentrationUnit/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concentrationUnit = await _context.ConcentrationUnit
                .FirstOrDefaultAsync(m => m.ConcentrationCode == id);
            if (concentrationUnit == null)
            {
                return NotFound();
            }

            return View(concentrationUnit);
        }

        //this is a get method for the create view, it requests data from the specified resource that is ASConcentrationUnit
        //data to display data for create
        // GET: ASConcentrationUnit/Create
        public IActionResult Create()
        {
            return View();
        }

        //this is a post method for create mode
        //it submits data to be processed to a specified resource
        //here it is ASConcentrationUnit
        //it holds the value the user has given on views
        // POST: ASConcentrationUnit/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConcentrationCode")] ConcentrationUnit concentrationUnit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(concentrationUnit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(concentrationUnit);
        }

        //this is a get method for the edit view, it requests data from the specified resource that is ASConcentrationUnit
        //data to display data for edit
        // GET: ASConcentrationUnit/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concentrationUnit = await _context.ConcentrationUnit.FindAsync(id);
            if (concentrationUnit == null)
            {
                return NotFound();
            }
            return View(concentrationUnit);
        }

        //this is a post method for edit mode
        //it submits data to be processed to a specified resource
        //here it is ASConcentrationUnit
        //it holds the value the user has given on views
        // POST: ASConcentrationUnit/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ConcentrationCode")] ConcentrationUnit concentrationUnit)
        {
            if (id != concentrationUnit.ConcentrationCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(concentrationUnit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConcentrationUnitExists(concentrationUnit.ConcentrationCode))
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
            return View(concentrationUnit);
        }

        //this is a get method for the delete view, it requests data from the specified resource that is ASConcentrationUnit
        //data to display data for delete
        // GET: ASConcentrationUnit/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concentrationUnit = await _context.ConcentrationUnit
                .FirstOrDefaultAsync(m => m.ConcentrationCode == id);
            if (concentrationUnit == null)
            {
                return NotFound();
            }

            return View(concentrationUnit);
        }

        //this is a post method for delete mode
        //it submits data to be processed to a specified resource
        //here it is ASConcentrationUnit
        //it holds the value the user has given on views
        //gives what happens when delete is requested by the user
        // POST: ASConcentrationUnit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var concentrationUnit = await _context.ConcentrationUnit.FindAsync(id);
            _context.ConcentrationUnit.Remove(concentrationUnit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConcentrationUnitExists(string id)
        {
            return _context.ConcentrationUnit.Any(e => e.ConcentrationCode == id);
        }
    }
}
