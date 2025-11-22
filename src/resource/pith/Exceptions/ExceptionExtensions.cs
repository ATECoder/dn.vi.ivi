namespace cc.isr.VI.Pith.ExceptionExtensions;
/// <summary>
/// Exception methods for adding exception data and building a detailed exception message.
/// </summary>
public static class ExceptionMethods
{
    /// <summary> Adds an exception data to 'exception'. </summary>
    /// <param name="value">     The value. </param>
    /// <param name="exception"> The exception. </param>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    private static bool AddExceptionData( Exception value, DeviceException? exception )
    {
        exception?.AddExceptionData( value );

        return exception is not null;
    }

    /// <summary> Adds an exception data to 'exception'. </summary>
    /// <param name="value">     The value. </param>
    /// <param name="exception"> The exception. </param>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    private static bool AddExceptionData( Exception value, NativeException? exception )
    {
        if ( exception is not null && exception.InnerError is not null )
        {
            exception.AddExceptionData( value );
        }

        return exception is not null;
    }

    /// <summary>
    /// Adds the <paramref name="exception"/> data to <paramref name="value"/> exception.
    /// </summary>
    /// <remarks>
    /// For more info on the external exceptions see:
    /// <see href="http://MSDN.Microsoft.com/en-us/library/system.runtime.InteropServices.SEHException.ASPX"/>.
    /// </remarks>
    /// <param name="value">     The value. </param>
    /// <param name="exception"> The exception. </param>
    /// <returns>
    /// <c>true</c> if it <see cref="Exception"/> is not nothing; otherwise <c>false</c>
    /// </returns>
    private static bool AddExceptionData( Exception value, System.Runtime.InteropServices.ExternalException? exception )
    {
        if ( value is not null && exception is not null )
            value.Data.Add( $"{value.Data.Count}-External.Error.Code", $"{exception.ErrorCode}" );

        return exception is not null;
    }

    /// <summary>
    /// Adds the <paramref name="exception"/> data to <paramref name="value"/> exception.
    /// </summary>
    /// <remarks> David, 2020-09-17. </remarks>
    /// <param name="value">     The value. </param>
    /// <param name="exception"> The exception. </param>
    /// <returns>
    /// <c>true</c> if it <see cref="Exception"/> is not nothing; otherwise <c>false</c>
    /// </returns>
    private static bool AddExceptionData( Exception value, ArgumentOutOfRangeException? exception )
    {
        if ( value is not null && exception is not null )
            value.Data.Add( $"{value.Data.Count}-Name+Value", $"{exception.ParamName}={exception.ActualValue}" );

        return exception is not null;
    }

    /// <summary>
    /// Adds the <paramref name="exception"/> data to <paramref name="value"/> exception.
    /// </summary>
    /// <remarks> David, 2020-09-17. </remarks>
    /// <param name="value">     The value. </param>
    /// <param name="exception"> The exception. </param>
    /// <returns>
    /// <c>true</c> if it <see cref="Exception"/> is not nothing; otherwise <c>false</c>
    /// </returns>
    private static bool AddExceptionData( Exception value, ArgumentException? exception )
    {
        if ( value is not null && exception is not null )
            value.Data.Add( $"{value.Data.Count}-Name", exception.ParamName );
        return exception is not null;
    }

    /// <summary> Adds exception data from the specified exception. </summary>
    /// <remarks> David, 2020-09-17. </remarks>
    /// <param name="exception"> The exception. </param>
    /// <returns> <c>true</c> if exception was added; otherwise <c>false</c> </returns>
    public static bool AddExceptionData( this Exception exception )
    {
        return AddExceptionData( exception, exception as ArgumentOutOfRangeException ) ||
               AddExceptionData( exception, exception as ArgumentException ) ||
               AddExceptionData( exception, exception as System.Runtime.InteropServices.ExternalException ) ||
               AddExceptionData( exception, exception as NativeException ) ||
               AddExceptionData( exception, exception as DeviceException );
    }

    /// <summary>
    /// Builds a detailed exception message including stack trace and exception data.
    /// </summary>
    /// <remarks>   David, 2020-09-15. </remarks>
    /// <param name="exception">            The exception. </param>
    /// <param name="includeStackTrace">    (Optional) [true] True to include, false to exclude the stack
    ///                                     trace. </param>
    /// <returns>   An exception message including stack trace and exception data. </returns>
    public static string BuildMessage( this Exception exception, bool includeStackTrace = true )
    {
        System.Text.StringBuilder builder = new();
        if ( includeStackTrace )
        {
            string stackTrace = exception.StackTrace;
            if ( !string.IsNullOrWhiteSpace( stackTrace ) ) _ = builder.AppendLine( stackTrace );
        }

        int counter = 1;
        _ = AppendExceptionInfo( builder, exception, counter );
        return builder.ToString().TrimEnd( Environment.NewLine.ToCharArray() );
    }

    /// <summary>   Appends an exception information. </summary>
    /// <remarks>   David, 2021-02-17. </remarks>
    /// <param name="builder">      The builder. </param>
    /// <param name="exception">    The exception. </param>
    /// <param name="counter">      The counter. </param>
    /// <returns>   An int. </returns>
    private static int AppendExceptionInfo( System.Text.StringBuilder builder, Exception exception, int counter )
    {
        AppendExceptionInfo( builder, exception, $"{counter}->" );
        counter += 1;
        if ( exception is AggregateException aggEx )
        {
            foreach ( Exception? ex in aggEx.InnerExceptions )
                counter = AppendExceptionInfo( builder, exception, counter );
        }
        if ( exception.InnerException is not null )
            counter = AppendExceptionInfo( builder, exception.InnerException, counter );

        return counter;
    }

    /// <summary>   Appends an exception information. </summary>
    /// <remarks>   David, 2020-09-15. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="builder">      The builder. </param>
    /// <param name="exception">    The exception. </param>
    /// <param name="prefix">       The prefix. </param>
    private static void AppendExceptionInfo( System.Text.StringBuilder builder, Exception exception, string prefix )
    {
        if ( exception is null ) throw new ArgumentNullException( nameof( exception ) );

        if ( builder is null ) throw new ArgumentNullException( nameof( builder ) );

        const int width = 8;
        _ = builder.AppendLine( $"{prefix}{nameof( System.Type ),width}: {exception.GetType()}" );
        if ( !string.IsNullOrWhiteSpace( exception.Message ) )
            _ = builder.AppendLine( $"{prefix}{nameof( Exception.Message ),width}: {exception.Message}" );

        if ( !string.IsNullOrWhiteSpace( exception.Source ) )
            _ = builder.AppendLine( $"{prefix}{nameof( Exception.Source ),width}: {exception.Source}" );

        if ( exception.TargetSite is not null )
            _ = builder.AppendLine( $"{prefix}  Method: {exception.TargetSite}" );

        if ( exception.HResult != 0 )
            _ = builder.AppendLine( $"{prefix}{nameof( Exception.HResult ),width}: {exception.HResult} ({exception.HResult:X})" );

        if ( exception.Data is not null )
        {
            foreach ( System.Collections.DictionaryEntry keyValuePair in exception.Data )
                _ = builder.AppendLine( $"{prefix}    Data: {keyValuePair.Key}: {keyValuePair.Value}" );
        }
    }
}
