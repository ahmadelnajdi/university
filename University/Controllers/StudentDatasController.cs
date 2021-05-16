using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using University.Areas.Identity.Data;
using University.Data;

namespace University.Controllers
{
    public class StudentDatasController : Controller
    {
        private readonly UniversityContext _context;

        public StudentDatasController(UniversityContext context)
        {
            _context = context;
        }

        // GET: StudentDatas
        public async Task<IActionResult> Index()
        {
            return View(await _context.studentDatas.ToListAsync());
        }

        // GET: StudentDatas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentData = await _context.studentDatas
                .FirstOrDefaultAsync(m => m.id == id);
            if (studentData == null)
            {
                return NotFound();
            }

            return View(studentData);
        }

        // GET: StudentDatas/Create
        public async Task<IActionResult> CreateAsync()
        {

            List<String> studentid = new List<string>();

            List<User> lists = new List<User>();

            lists = await _context.Users.ToListAsync();
            for (int i = 0; i < lists.Count(); i++)
            {
                if ( lists[i].Email != "admin@admin.com")
                studentid.Add(lists[i].StudentID);
            }

            ViewBag.lists = studentid ;

            return View();
        }

        // POST: StudentDatas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Studentid,Faculty,Course,Note")] StudentData studentData)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studentData);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(studentData);
        }

        // GET: StudentDatas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentData = await _context.studentDatas.FindAsync(id);
            if (studentData == null)
            {
                return NotFound();
            }
            return View(studentData);
        }

        // POST: StudentDatas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Studentid,Faculty,Course,Note")] StudentData studentData)
        {
            if (id != studentData.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentDataExists(studentData.id))
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
            return View(studentData);
        }

        // GET: StudentDatas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentData = await _context.studentDatas
                .FirstOrDefaultAsync(m => m.id == id);
            if (studentData == null)
            {
                return NotFound();
            }

            return View(studentData);
        }

        // POST: StudentDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentData = await _context.studentDatas.FindAsync(id);
            _context.studentDatas.Remove(studentData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentDataExists(int id)
        {
            return _context.studentDatas.Any(e => e.id == id);
        }
    }
}
