namespace PZPack.Exceptions;

/// <summary>
/// Exception thrown when IO operations fail for PZPack
/// </summary>
public class PZPackIOException : PZPackException
{
    /// <summary>
    /// Initializes PZPackIOException
    /// </summary>
    public PZPackIOException() : base() { }

    /// <summary>
    /// Initializes PZPackIOException
    /// </summary>
    /// <param name="message">The error message</param>
    public PZPackIOException(string message) : base(message) { }

    /// <summary>
    /// Initializes PZPackIOException
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="innerException">The inner exception</param>
    public PZPackIOException(string message, Exception innerException) : base(message, innerException) { }
}
