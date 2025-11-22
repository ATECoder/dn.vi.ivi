namespace cc.isr.VI.Foundation.ExceptionExtensions;
/// <summary>
/// Exception methods for adding exception data and building a detailed exception message.
/// </summary>
public static class ExceptionMethods
{
    /// <summary> Adds an exception data to 'exception'. </summary>
    /// <param name="value">     The value. </param>
    /// <param name="exception"> The exception. </param>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    private static bool AddExceptionData( Exception value, Ivi.Visa.NativeVisaException? exception )
    {
        if ( exception is not null )
        {
            if ( exception.ErrorCode > 0 )
                value.Data.Add( $"{value.Data.Count}-Warning", $"0x{exception.ErrorCode:X}" );
            else if ( exception.ErrorCode < 0 )
                value.Data.Add( $"{value.Data.Count}-Error", $"-0x{-exception.ErrorCode:X}" );
        }

        return exception is not null;
    }

    /// <summary> Adds an exception data to 'Exception'. </summary>
    /// <param name="exception"> The exception. </param>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    public static bool AddExceptionData( this Exception exception )
    {
        return AddExceptionData( exception, exception as Ivi.Visa.NativeVisaException ) ||
              cc.isr.VI.Pith.ExceptionExtensions.ExceptionMethods.AddExceptionData( exception );
    }

    /// <summary>   An Exception extension method that builds a message. </summary>
    /// <remarks>   David, 2021-06-28. </remarks>
    /// <param name="exception">    The exception. </param>
    /// <returns>   A <see cref="string" />. </returns>
    public static string BuildMessage( this Exception exception )
    {
        return cc.isr.VI.Pith.ExceptionExtensions.ExceptionMethods.BuildMessage( exception );
    }
}
