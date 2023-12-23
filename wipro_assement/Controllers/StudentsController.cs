using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using wipro_assement.Data;
using wipro_assement.Models;
using X.PagedList;

namespace wipro_assement.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string searchby, string search, string? sortBy, int? page)
        {
            ViewBag.SortbyNameParameter = string.IsNullOrEmpty(sortBy) ? "Sname desc" : "";
            ViewBag.SortbyStdparameter = sortBy == "Std" ? "Std desc" : "Std";

            var studentList = _context.Students.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
               switch (searchby)
                {
                    case "Name":
                        studentList = studentList.Where(x => x.Sname.Contains(search) || search == null);
                        break;
                    case "Std":
                        studentList = studentList.Where(x => x.Std.Contains(search) || search == null);
                        break;
                }
            }
            switch (sortBy)
            {
                case "Sname desc":
                    studentList = studentList.OrderByDescending(x => x.Sname); 
                    break;

                case "Std desc":
                    studentList = studentList.OrderByDescending(x => x.Std);
                    break;

                case "Std":
                    studentList = studentList.OrderBy(x => x.Std);
                    break;


                default:
                    studentList = studentList.OrderBy(x => x.Sname);
                    break;
            }

            var result = await studentList.ToListAsync();
            return View(result.ToPagedList(page ?? 1, 3));
        }


        //public async Task<IActionResult> Index()
        //{
        //    return _context.Students != null ?
        //                View(await _context.Students.ToListAsync()) :
        //                Problem("Entity set 'ApplicationDbContext.Students'  is null.");
        //}

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Rollno == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Rollno,Sname,Std")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Rollno,Sname,Std")] Student student)
        {
            if (id != student.Rollno)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Rollno))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Rollno == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Students == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Students'  is null.");
            }
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
          return (_context.Students?.Any(e => e.Rollno == id)).GetValueOrDefault();
        }
    }
}
