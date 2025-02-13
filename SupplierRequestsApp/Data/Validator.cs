using System.Text.RegularExpressions;

namespace SupplierRequestsApp.Data;

public static partial class Validator
{
    public static T RequireGreaterThan<T>(T value, T min) where T : struct, IComparable<T>
    {
        if (value.CompareTo(min) <= 0)
            throw new ValidationNotCourseInException<T>(value, min, null,
                $"Value ({value}) must be greater than {min}");
        return value;
    }

    public static T RequireCourseIn<T>(T value, T min, T max) where T : struct, IComparable<T>
    {
        if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
        {
            throw new ValidationNotCourseInException<T>(value, min, max,
                $"Value ({value}) must be greater than {min} and less than {max}");
        }

        return value;
    }

    public static T RequireGreaterOrEqualsThan<T>(T value, T min) where T : struct, IComparable<T>
    {
        if (value.CompareTo(min) < 0)
            throw new ValidationNotCourseInException<T>(value, min, null,
                $"Value ({value}) must be greater or equals than {min}");
        return value;
    }

    public static T RequireLessOrEqualsThan<T>(T value, T max) where T : struct, IComparable<T>
    {
        if (value.CompareTo(max) > 0)
        {
            throw new ValidationNotCourseInException<T>(value, null, max,
                $"Value ({value}) must be less or equals than {max}");
        }

        return value;
    }

    public static T RequireLessThan<T>(T value, T max) where T : struct, IComparable<T>
    {
        if (value.CompareTo(max) >= 0)
        {
            throw new ValidationNotCourseInException<T>(value, null, max,
                $"Value ({value}) must be less than {max}");
        }

        return value;
    }

    public static T RequireEquals<T>(T value, T requireValue) where T : struct, IComparable<T>
    {
        if (value.CompareTo(requireValue) != 0)
            throw new ValidationEqualsException<T>(value, requireValue,
                $"Value ({value}) must be equals to {requireValue}");
        return value;
    }

    public static string RequireNotEmpty(string value)
    {
        if (value == "")
            throw new ValidationLengthException<string>(value, 1, int.MaxValue, $"Value must be not empty");
        return value;
    }

    public static T RequireNotEmpty<T>(T value) where T : IEnumerable<object>
    {
        if (!value.Any())
            throw new ValidationLengthException<T>(value, 1, int.MaxValue,
                $"Value must be not empty");
        return value;
    }

    public static string RequireNotBlank(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationNotBlankException(value, $"Value ({value}) must be not blank");
        return value;
    }
    public static Guid RequireGuid(Guid value)
    {
        if (string.IsNullOrWhiteSpace(value.ToString()))
            throw new ValidationNotBlankException(value.ToString(), $"Value ({value}) must be not blank");
        return value;
    }

    public static T RequireNotNull<T>(T? value)
    {
        if (value == null) throw new ValidationNullException($"Value must be not null");
        return value;
    }

    public static T RequireNotNull<T>(T? value) where T : struct
    {
        if (value == null) throw new ValidationNullException($"Value must be not null");
        return value.Value;
    }

    public static int RequireInt(string value)
    {
        if (!int.TryParse(value, out var result))
            throw new ValidationConvertException<string>(value, typeof(int),
                $"Value ({value}) must be int");
        return result;
    }

    public static decimal RequireDecimal(string value)
    {
        if (!decimal.TryParse(value, out var result))
            throw new ValidationConvertException<string>(value, typeof(int),
                $"Value ({value}) must be int");
        return result;
    }


    public static T RequireEnum<T>(int value) where T : Enum
    {
        if (!Enum.IsDefined(typeof(T), value))
            throw new ValidationConvertException<int>(value, typeof(T),
                $"Value ({value}) must be defined in {typeof(T)}");
        return (T)Enum.ToObject(typeof(T), value);
    }

    public static int RequireLong(string value)
    {
        if (!long.TryParse(value, out var result))
            throw new ValidationConvertException<string>(value, typeof(long),
                $"Value ({value}) must be int");
        return (int)result;
    }

    public static bool RequireBool(string value)
    {
        if (!bool.TryParse(value, out var result))
            throw new ValidationConvertException<string>(value, typeof(bool),
                $"Value ({value}) must be boolean");
        return result;
    }

    public static string RequireNumeric(string value)
    {
        var isNumber = NumericRegex().IsMatch(value);
        if (!isNumber)
            throw new ValidationCheckException<string>(value, $"Value ({value}) must be numeric");
        return value;
    }


    [GeneratedRegex(@"^\d+$")]
    private static partial Regex NumericRegex();
}