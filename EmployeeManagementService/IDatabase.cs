namespace EmployeeManagementService
{
    public interface IDatabase
    {
        void Create();
        void Add(IEmployee employee);
        void Add(IEmployee[] employees);
        IEmployee[] GetAll();
        IEmployee[] Select(char firstLetter, Gender gender);
    }
}
