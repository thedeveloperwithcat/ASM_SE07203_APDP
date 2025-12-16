// Replace only the Create POST method with this version
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(StudentInputModel input)
{
    if (!ModelState.IsValid)
    {
        var students = await _context.Students.OrderBy(s => s.Id).ToListAsync();
        return View("Index", students);
    }

    var student = new Student
    {
        StudentId = input.StudentId,
        Name = input.Name,
        Class = input.Class,
        Major = input.Major,
        Status = input.Status,
        Teacher = input.Teacher,
        Note = input.Note,
        // set server-side defaults
        AccountId = HttpContext.Session.GetInt32("id") ?? 0, // or your logic
        FullName = input.Name, // optional mapping
        Email = null,
        BirthDate = null
    };

    _context.Students.Add(student);
    await _context.SaveChangesAsync();
    return RedirectToAction(nameof(Index));
}