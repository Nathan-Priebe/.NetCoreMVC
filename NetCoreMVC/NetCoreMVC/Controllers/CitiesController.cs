using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetCoreMVC.Entities;
using NetCoreMVC.Models;

namespace NetCoreMVC.Controllers
{
    public class CitiesController : Controller
    {
        private readonly CityInfoContext _context;

        public CitiesController(CityInfoContext context)
        {
            _context = context;
        }

        // GET: Cities
        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "My Cities";
            var cityEntities = await _context.Cities.ToListAsync();
            return View(Mapper.Map<IEnumerable<CityDto>>(cityEntities));
        }

        // GET: Cities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cityEntity = await _context.Cities
                .SingleOrDefaultAsync(m => m.Id == id);
            var city = Mapper.Map<CityDetailsDto>(cityEntity);
            if (city == null)
            {
                return NotFound();
            }

            ViewBag.Title = $@"{city.Name} Details";

            return View(city);
        }

        // GET: Cities/Create
        public IActionResult Create()
        {
            ViewBag.Title = "Create City";
            return View();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(CityCreationDto city)
        {
            if (ModelState.IsValid)
            {
                var finalCity = Mapper.Map<City>(city);
                _context.Add(finalCity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(city);
        }

        // GET: Cities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cityEntity = await _context.Cities.SingleOrDefaultAsync(m => m.Id == id);
            var city = Mapper.Map<CityEditDto>(cityEntity);
            if (city == null)
            {
                return NotFound();
            }
            ViewBag.Title = $@"Edit {city.Name}";

            return View(city);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(int id, CityEditDto city)
        {
            if (id != city.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var finalCity = Mapper.Map<City>(city);
                    _context.Update(finalCity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.Id))
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
            return View(city);
        }

        // GET: Cities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cityEntity = await _context.Cities
                .SingleOrDefaultAsync(m => m.Id == id);
            var city = Mapper.Map<CityDeleteDto>(cityEntity);
            if (city == null)
            {
                return NotFound();
            }

            ViewBag.Title = $@"Delete {city.Name}";

            return View(city);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var city = await _context.Cities.SingleOrDefaultAsync(m => m.Id == id);
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CityExists(int id)
        {
            return _context.Cities.Any(e => e.Id == id);
        }
    }
}
