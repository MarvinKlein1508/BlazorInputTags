namespace SamplesShared.SampleData;

public record Person(int PersonId, string CountryCode, string FirstName, string LastName, DateOnly BirthDate, string Picture)
{
    public override string ToString() => $"{FirstName} {LastName} ({BirthDate}, {CountryCode})";

    public string GetName() => $"{FirstName} {LastName}";
}

public class SimplePerson
{
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public int Age { get; set; }
}
