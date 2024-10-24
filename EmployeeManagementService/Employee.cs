
namespace EmployeeManagementService
{
    public class Employee(string name, DateOnly birthDate, Gender gender) : IEmployee
    {
        public string Name { get; } = name;
        public DateOnly BirthDate { get; } = birthDate;
        public Gender Gender { get; } = gender;

        public int GetAge() => DateTime.Now.Year - BirthDate.Year - (DateTime.Now.Month < BirthDate.Month || DateTime.Now.Month == BirthDate.Month && DateTime.Now.Day < BirthDate.Day ? 1 : 0);
        public void Save(IDatabase database) => database.Add(this);
    }
}
