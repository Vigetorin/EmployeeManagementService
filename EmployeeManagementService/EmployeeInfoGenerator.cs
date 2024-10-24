namespace EmployeeManagementService
{
    public class EmployeeInfoGenerator : IEmployeeInfoGenerator
    {
        readonly Random random = new();

        readonly string[] surnames;
        readonly string[] namesMale;
        readonly string[] namesFemale;
        readonly string[] patronymics;

        readonly DateOnly firstDate = new(1950, 1, 1);
        readonly DateOnly lastDate = new(2010, 1, 1);

        public EmployeeInfoGenerator()
        {
            surnames = File.ReadAllLines("surnames.txt");
            namesMale = File.ReadAllLines("names_male.txt");
            namesFemale = File.ReadAllLines("names_female.txt");
            patronymics = File.ReadAllLines("patronymics.txt");
        }

        string GetSurname(Gender gender)
        {
            return surnames[random.Next(surnames.Length)] + (gender == Gender.Male ? "" : "a");
        }
        string GetSurname(Gender gender, char letter)
        {
            List<string> availableSurnames = surnames.Where(surname => surname[0] == letter).ToList();
            return availableSurnames[random.Next(availableSurnames.Count)] + (gender == Gender.Male ? "" : "a");
        }
        string GetForename(Gender gender)
        {
            return gender == Gender.Male ? namesMale[random.Next(namesMale.Length)] : namesFemale[random.Next(namesFemale.Length)];
        }
        string GetPatronymic(Gender gender)
        {
            return patronymics[random.Next(patronymics.Length)] + (gender == Gender.Male ? "ich" : "na");
        }
        public string GetName(Gender gender)
        {
            return GetSurname(gender) + " " + GetForename(gender) + " " + GetPatronymic(gender);
        }
        public string GetName(Gender gender, char letter)
        {
            return GetSurname(gender, letter) + " " + GetForename(gender) + " " + GetPatronymic(gender);
        }
        public DateOnly GetBirthDate()
        {
            return firstDate.AddDays(random.Next(lastDate.DayNumber - firstDate.DayNumber));
        }
        public Gender GetGender()
        {
            return (Gender)random.Next(2);
        }
    }
}
