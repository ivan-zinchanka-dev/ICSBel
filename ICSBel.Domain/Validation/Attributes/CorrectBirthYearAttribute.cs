using System.ComponentModel.DataAnnotations;

namespace ICSBel.Domain.Validation.Attributes;

public class CorrectBirthYearAttribute : ValidationAttribute
{
    private const int Century = 100;
    public static int MinYear => DateTime.Now.Year - Century;
    public static int MaxYear => DateTime.Now.Year;
    
    public override bool IsValid(object value)
    {
        if (value is int year)
        {
            return year >= MinYear && year <= MaxYear;
        }

        return false;
    }
    
    public override string FormatErrorMessage(string name)
    {
        return "Некорректный год рождения";
    }
}