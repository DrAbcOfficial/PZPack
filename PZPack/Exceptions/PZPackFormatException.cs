namespace PZPack.Exceptions;

/// <summary>
/// Exception thrown when PZPack format is invalid
/// </summary>
public class PZPackFormatException : PZPackException
{
    /// <summary>
    /// Initializes a new instance of PZPackFormatException
    /// </summary>
    public PZPackFormatException() : base() { }

    /// <summary>
    /// Initializes PZPackFormatException
    /// </summary>
    /// <param name="message">The error message</param>
    public PZPackFormatException(string message) : base(message) { }

    /// <summary>
    /// Initializes PZPackFormatException
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="innerException">The inner exception</param>
    public PZPackFormatException(string message, Exception innerException) : base(message, innerException) { }
}