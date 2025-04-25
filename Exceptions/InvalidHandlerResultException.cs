namespace SabiatR.Exceptions;

// Custom exception for invalid result from handler (when it doesn't return the expected Task<TResponse>)
public class InvalidHandlerResultException : Exception
{
    public Type? HandlerType { get; }
    public string MethodName { get; }
    public object? ReturnedValue { get; }

    public InvalidHandlerResultException(Type? handlerType, string methodName, object? returnedValue)
        : base($"Method '{methodName}' on handler '{handlerType?.Name}' returned an invalid result: {returnedValue?.GetType().Name ?? "null"}")
    {
        HandlerType = handlerType;
        MethodName = methodName;
        ReturnedValue = returnedValue;
    }
}