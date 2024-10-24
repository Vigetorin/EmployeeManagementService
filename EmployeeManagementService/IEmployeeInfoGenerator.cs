namespace EmployeeManagementService
{
    public interface IEmployeeInfoGenerator
    {
        string GetName(Gender gender);
        string GetName(Gender gender, char letter);
        DateOnly GetBirthDate();
        Gender GetGender();
    }
}
