using System.ComponentModel.DataAnnotations;

namespace ICSBel.Domain.Validation.Attributes;

public class CorrectSalaryAttribute : ValidationAttribute
{
    public const decimal MinSalary = 0;
    public const decimal MaxSalary = 1_000_000;
    public const int DecimalPlaces = 2;
    
    public override bool IsValid(object value)
    {
        if (value is decimal salary)
        {
            return salary >= MinSalary && salary <= MaxSalary;
        }

        return false;
    }
    
    public override string FormatErrorMessage(string name)
    {
        return $"{name} must be between {MinSalary} and {MaxSalary}.";
    }
}