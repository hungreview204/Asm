// Student.cs
public class Student
{
    public string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
}

// IStudentRepository.cs
public interface IStudentRepository
{
    void AddStudent(Student student);
    void RemoveStudent(string studentName);
    Student GetStudent(string studentName);
    List<Student> GetAllStudents();
}

// StudentRepository.cs
public class StudentRepository : IStudentRepository
{
    private List<Student> students = new List<Student>();

    public void AddStudent(Student student)
    {
        students.Add(student);
    }

    public void RemoveStudent(string studentName)
    {
        var studentToRemove = students.Find(s => s.Name == studentName);
        if (studentToRemove != null)
            students.Remove(studentToRemove);
    }

    public Student GetStudent(string studentName)
    {
        return students.Find(s => s.Name == studentName);
    }

    public List<Student> GetAllStudents()
    {
        return students;
    }
}

// IStudentManagement.cs
public interface IStudentManagement
{
    void AddNewStudent(Student student);
    void RemoveExistingStudent(string studentName);
    void UpdateStudentInfo(string studentName, Student updatedStudentInfo);
    Student GetStudentInfo(string studentName);
    List<Student> GetAllStudentsInfo();
}

// StudentManagement.cs
public class StudentManagement : IStudentManagement
{
    private readonly IStudentRepository studentRepository;

    public StudentManagement(IStudentRepository repository)
    {
        studentRepository = repository;
    }

    public void AddNewStudent(Student student)
    {
        if (student == null)
            throw new ArgumentNullException(nameof(student));

        studentRepository.AddStudent(student);
    }

    public void RemoveExistingStudent(string studentName)
    {
        if (string.IsNullOrWhiteSpace(studentName))
            throw new ArgumentException("Student name cannot be null or empty.", nameof(studentName));

        studentRepository.RemoveStudent(studentName);
    }

    public void UpdateStudentInfo(string studentName, Student updatedStudentInfo)
    {
        if (string.IsNullOrWhiteSpace(studentName))
            throw new ArgumentException("Student name cannot be null or empty.", nameof(studentName));

        var existingStudent = studentRepository.GetStudent(studentName);
        if (existingStudent != null)
        {
            existingStudent.Name = updatedStudentInfo.Name;
            existingStudent.DateOfBirth = updatedStudentInfo.DateOfBirth;
            existingStudent.Address = updatedStudentInfo.Address;
            existingStudent.PhoneNumber = updatedStudentInfo.PhoneNumber;
        }
    }

    public Student GetStudentInfo(string studentName)
    {
        if (string.IsNullOrWhiteSpace(studentName))
            throw new ArgumentException("Student name cannot be null or empty.", nameof(studentName));

        return studentRepository.GetStudent(studentName);
    }

    public List<Student> GetAllStudentsInfo()
    {
        return studentRepository.GetAllStudents();
    }
}

// Program.cs
class Program
{
    static void Main(string[] args)
    {
        // Initialization
        IStudentRepository repository = new StudentRepository();
        IStudentManagement studentManagement = new StudentManagement(repository);

        // Adding new student
        var newStudent = new Student
        {
            Name = "John Doe",
            DateOfBirth = new DateTime(2000, 1, 1),
            Address = "123 Main St, City",
            PhoneNumber = "123-456-7890"
        };
        studentManagement.AddNewStudent(newStudent);

        // Getting student info
        var studentInfo = studentManagement.GetStudentInfo("John Doe");
        if (studentInfo != null)
        {
            Console.WriteLine($"Student Name: {studentInfo.Name}");
            Console.WriteLine($"Date of Birth: {studentInfo.DateOfBirth.ToShortDateString()}");
            Console.WriteLine($"Address: {studentInfo.Address}");
            Console.WriteLine($"Phone Number: {studentInfo.PhoneNumber}");
        }
        else
        {
            Console.WriteLine("Student not found.");
        }

        // Removing student
        studentManagement.RemoveExistingStudent("John Doe");
    }
}
