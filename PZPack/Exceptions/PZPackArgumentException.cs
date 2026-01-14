namespace PZPack.Exceptions;

/// <summary>
/// Exception thrown when invalid arguments are provided to PZPack
/// </summary>
public class PZPackArgumentException : PZPackException
{
    /// <summary>
    /// Initializes PZPackArgumentException
    /// </summary>
    public PZPackArgumentException() : base() { }

    /// <summary>
    /// Initializes PZPackArgumentException
    /// </summary>
    /// <param name="message">The error message</param>
    public PZPackArgumentException(string message) : base(message) { }

    /// <summary>
    /// Initializes PZPackArgumentException
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="innerException">The inner exception</param>
    public PZPackArgumentException(string message, Exception innerException) : base(message, innerException) { }

    /// <summary>
    /// Initializes PZPackArgumentException
    /// </summary>
    /// <param name="paramName">The parameter name</param>
    /// <param name="message">The error message</param>
    public PZPackArgumentException(string paramName, string message) : base($"{message} (param name: {paramName})") { }
}