namespace PZPack.Exceptions;

/// <summary>
/// Base exception class for PZPack
/// </summary>
public class PZPackException : Exception
{
    /// <summary>
    /// Initializes a new instance of PZPackException
    /// </summary>
    public PZPackException() : base() { }

    /// <summary>
    /// Initializes a new instance of PZPackException
    /// </summary>
    /// <param name="message">The error message</param>
    public PZPackException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of PZPackException
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="innerException">The inner exception</param>
    public PZPackException(string message, Exception innerException) : base(message, innerException) { }
}
