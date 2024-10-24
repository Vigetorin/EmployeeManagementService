namespace EmployeeManagementService
{
    public enum Gender
    {
        Male,
        Female
    }

    public interface IEmployee
    {
        string Name { get; }
        DateOnly BirthDate { get; }
        Gender Gender { get; }

        int GetAge();
        void Save(IDatabase database);
    }
}
