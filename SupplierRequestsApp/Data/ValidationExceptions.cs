namespace SupplierRequestsApp.Data;

public class ValidationException( string message) : ArgumentException
{
    public override string Message { get; } = message;
}

public class ValidationConvertException<T>(T actual, Type expectedType,  string message)
    : ValidationException( message)
{
    public T Actual { get; } = actual;
    public Type ExpectedType { get; } = expectedType;
}

public class ValidationCheckException<T>(T actual,  string message)
    : ValidationException( message)
{
    public T Actual { get; } = actual;
}

public class ValidationLengthException<T>(T actual, int minLength, int maxLength,  string message)
    : ValidationException( message)
{
    public T Actual { get; } = actual;
    public int MinLength { get; } = minLength;
    public int MaxLength { get; } = maxLength;
}

public class ValidationNotBlankException(string actual,  string message) : ValidationException( message)
{
    public string Actual { get; } = actual;
}

public class ValidationNotCourseInException<T>(T actual, T? minValue, T? maxValue,  string message)
    : ValidationException( message) where T : struct
{
    public T Actual { get; } = actual;
    public T? MinValue { get; } = minValue;
    public T? MaxValue { get; } = maxValue;
}

public class ValidationEqualsException<T>(T actual, T expected,  string message)
    : ValidationException( message)
{
    public T Actual { get; } = actual;
    public T Expected { get; } = expected;
}

public class ValidationNullException( string message) : ValidationException( message);