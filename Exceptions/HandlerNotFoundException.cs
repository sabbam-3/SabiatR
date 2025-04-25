namespace SabiatR.Exceptions;

// Custom exception for when handler method is not found
public class HandlerNotFoundException : Exception
{
    private Type? RequestType { get; set; }
    private string MethodName { get; set; }

    public HandlerNotFoundException(Type? requestType, string methodName)
        : base($"Handler method '{methodName}' not found for request type '{requestType?.Name}'")
    {
        RequestType = requestType;
        MethodName = methodName;
    }
}