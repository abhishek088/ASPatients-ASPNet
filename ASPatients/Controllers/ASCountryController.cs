using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPatients.Models;
using Microsoft.AspNetCore.Authorization;

namespace ASPatients.Controllers
{
    // contains flow control logic for Country 
    //sends response when user makes a browser request for Country
    
    public class ASCountryController : Controller
    {
        private readonly PatientsContext _context;

        // the context maintains the state of single module during the request life cycle
        public ASCountryController(PatientsContext context)
        {
            _context = context;
        }

        // this get method is used to request data for ASCountry
        // GET: ASCountry
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Country.ToListAsync());
        }

        //this is a get method for the details view, it requests data from the specified resource that is ASCountry
        //data to display details from the database
        // GET: ASCountry/Details/5
        [Authorize(Roles = "members, administrators, medicalStaff")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Country
                .FirstOrDefaultAsync(m => m.CountryCode == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        //this is a get method for the create view, it requests data from the specified resource that is ASCountry
        //data to display data for create
        // GET: ASCountry/Create
        [Authorize(Roles = "members, administrators, medicalStaff")]
        public IActionResult Create()
        {
            return View();
        }

        //this is a post method for create mode
        //it submits data to be processed to a specified resource
        //here it is ASCountry
        //it holds the value the user has given on views
        // POST: ASCountry/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "members, administrators, medicalStaff")]
        public async Task<IActionResult> Create([Bind("CountryCode,Name,PostalPattern,PhonePattern,FederalSalesTax")] Country country)
        {
            if (ModelState.IsValid)
            {
                _context.Add(country);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        //this is a get method for the edit view, it requests data from the specified resource that is ASCountry
        //data to display data for edit
        // GET: ASCountry/Edit/5
        [Authorize(Roles = "members, administrators, medicalStaff")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Country.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        //this is a post method for edit mode
        //it submits data to be processed to a specified resource
        //here it is ASCountry
        //it holds the value the user has given on views
        // POST: ASCountry/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "members, administrators, medicalStaff")]
        public async Task<IActionResult> Edit(string id, [Bind("CountryCode,Name,PostalPattern,PhonePattern,FederalSalesTax")] Country country)
        {
            if (id != country.CountryCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.CountryCode))
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
            return View(country);
        }

        //this is a get method for the delete view, it requests data from the specified resource that is ASCountry
        //data to display data for delete
        // GET: ASCountry/Delete/5
        [Authorize(Roles = "members, administrators, medicalStaff")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Country
                .FirstOrDefaultAsync(m => m.CountryCode == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        //this is a post method for delete mode
        //it submits data to be processed to a specified resource
        //here it is ASCountry
        //it holds the value the user has given on views
        //gives what happens when delete is requested by the user
        // POST: ASCountry/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "members, administrators, medicalStaff")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var country = await _context.Country.FindAsync(id);
            _context.Country.Remove(country);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CountryExists(string id)
        {
            return _context.Country.Any(e => e.CountryCode == id);
        }
    }
}
