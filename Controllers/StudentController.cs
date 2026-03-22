using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentProject.Models;

namespace StudentProject.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentController : Controller
{
    private readonly AppDbContext _context;

    public StudentController(AppDbContext context)
    {
        _context = context;
    }

    // =========================
    // GET ALL STUDENTS
    // =========================
    [HttpGet]
    public async Task<IActionResult> GetStudents()
    {
        var students = await _context.Students.ToListAsync();
        return Ok(students);
    }

    // =========================
    // GET STUDENT BY ID
    // =========================
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudentById(int id)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null)
            return NotFound(new { message = "Student not found" });

        return Ok(student);
    }

    // =========================
    // CREATE NEW STUDENT - Admin only
    // =========================
    [HttpPost]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddStudent([FromBody] Student student)
    {
        if (student == null)
            return BadRequest();

        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
    }

    // =========================
    // UPDATE STUDENT - Admin only
    // =========================
    [HttpPut("{id}")]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student updatedStudent)
    {
        if (id != updatedStudent.Id)
            return BadRequest(new { message = "ID mismatch" });

        var student = await _context.Students.FindAsync(id);

        if (student == null)
            return NotFound(new { message = "Student not found" });

        // Update fields
        student.Name = updatedStudent.Name;
        student.Course = updatedStudent.Course;

        await _context.SaveChangesAsync();

        return Ok(student);
    }

    // =========================
    // DELETE STUDENT - Admin only
    // =========================
    [HttpDelete("{id}")]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null)
            return NotFound(new { message = "Student not found" });

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Student deleted successfully" });
    }
}
