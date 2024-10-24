using EmployeeManagementService;
using System.Diagnostics;

class Program
{
    const string connectionString = "Data Source=TERMINATOR;Initial Catalog=EmployeeManagementService;Integrated Security=True;Encrypt=False";

    static void Main(string[] args)
    {
        if (args.Length > 0)
            switch (args[0])
            {
                case "1":
                    Create();
                    break;
                case "2":
                    Add(args[1], DateOnly.Parse(args[2]), Enum.Parse<Gender>(args[3]));
                    break;
                case "3":
                    GetAll();
                    break;
                case "4":
                    Fill();
                    break;
                case "5":
                    Select('F', Gender.Male);
                    break;
            }
    }

    static void Create() => new Database(connectionString).Create();
    static void Add(string name, DateOnly birthDate, Gender gender)
    {
        Employee employee = new(name, birthDate, gender);
        employee.Save(new Database(connectionString));
    }
    static void GetAll()
    {
        IEmployee[] employees = new Database(connectionString).GetAll();
        foreach (IEmployee employee in employees)
            Console.WriteLine($"{employee.Name}\t{employee.BirthDate}\t{employee.Gender}\t{employee.GetAge()}");
        Console.ReadKey();
    }
    static void Fill()
    {
        EmployeeInfoGenerator employeeInfoGenerator = new();
        const int count = 1000000;
        const int additionalCount = 100;
        IEmployee[] employees = new IEmployee[count + additionalCount];
        for (int i = 0; i < count; i++)
        {
            Gender gender = employeeInfoGenerator.GetGender();
            employees[i] = new Employee(employeeInfoGenerator.GetName(gender), employeeInfoGenerator.GetBirthDate(), gender);
        }
        for (int i = 0; i < additionalCount; i++)
            employees[count + i] = new Employee(employeeInfoGenerator.GetName(Gender.Male, 'F'), employeeInfoGenerator.GetBirthDate(), Gender.Male);
        new Database(connectionString).Add(employees);
    }
    static void Select(char firstLetter, Gender gender)
    {
        Stopwatch watch = Stopwatch.StartNew();
        IEmployee[] employees = new Database(connectionString).Select(firstLetter, gender);
        foreach (IEmployee employee in employees)
            Console.WriteLine(employee.Name + "\t" + employee.BirthDate + "\t" + employee.Gender + "\t" + employee.GetAge());
        watch.Stop();
        Console.WriteLine($"Execution time: {watch.ElapsedMilliseconds} ms");
        Console.ReadKey();
    }
}