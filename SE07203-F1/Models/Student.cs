public class Student
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public DateTime BirthDate { get; set; }
    public string StudentId { get; internal set; }
    public string Name { get; internal set; }
    public string Class { get; internal set; }
    public string Major { get; internal set; }
    public string Status { get; internal set; }
    public string Teacher { get; internal set; }
    public string Note { get; internal set; }
}