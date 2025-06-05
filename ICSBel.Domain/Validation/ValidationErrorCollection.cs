using System.Collections;

namespace ICSBel.Domain.Validation;

public class ValidationErrorCollection : IEnumerable<IReadOnlyList<string>>
{
    private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
    
    public bool HasErrors => _errors.Any(error => error.Value.Any());

    public bool HasPropertyErrors(string propertyName)
    {
        return _errors.ContainsKey(propertyName);
    }

    public IEnumerable<string> GetPropertyErrors(string propertyName)
    {
        if (_errors.TryGetValue(propertyName, out List<string> errors))
        {
            return errors;
        }
        else
        {
            return new List<string>();
        }
    }

    public bool TryAddPropertyErrors(string propertyName, ICollection<string> errorMessages)
    {
        if (!_errors.ContainsKey(propertyName))
        {
            _errors[propertyName] = new List<string>();
        }

        bool addedOnce = false;

        foreach (string errorMessage in errorMessages)
        {
            if (!_errors[propertyName].Contains(errorMessage))
            {
                _errors[propertyName].Add(errorMessage);
                addedOnce = true;
            }
        }
        
        return addedOnce;
    }

    public bool TryAddPropertyError(string propertyName, string errorMessage)
    {
        return TryAddPropertyErrors(propertyName, new string[] { errorMessage });
    }
    
    public bool TryClearPropertyErrors(string propertyName)
    {
        if (_errors.ContainsKey(propertyName))
        {
            _errors[propertyName].Clear();
            _errors.Remove(propertyName);
            return true;
        }

        return false;
    }

    public void ClearAllErrors()
    {
        _errors.Clear();
    }

    public IEnumerator<IReadOnlyList<string>> GetEnumerator()
    {
        foreach (var errorList in _errors.Values)
        {
            yield return errorList;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}