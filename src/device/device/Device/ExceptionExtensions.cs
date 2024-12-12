namespace cc.isr.VI.ExceptionExtensions;
/// <summary>
/// Exception methods for adding exception data and building a detailed exception message.
/// </summary>
public static class ExceptionExtensionMethods
{
    /// <summary> Adds exception data from the specified exception. </summary>
    /// <param name="exception"> The exception. </param>
    /// <returns> <c>true</c> if exception was added; otherwise <c>false</c> </returns>
    public static bool AddExceptionData( this Exception exception )
    {
        return cc.isr.VI.Foundation.ExceptionExtensions.ExceptionExtensionMethods.AddExceptionData( exception );
    }

    /// <summary>   An Exception extension method that builds a message. </summary>
    /// <remarks>   David, 2021-06-28. </remarks>
    /// <param name="exception">    The exception. </param>
    /// <returns>   A <see cref="string" />. </returns>
    public static string BuildMessage( this Exception exception )
    {
        return cc.isr.VI.Pith.ExceptionExtensions.ExceptionExtensionMethods.BuildMessage( exception );
    }

}
