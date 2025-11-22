using cc.isr.Std.LineEndingExtensions;
using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseMethods
{
    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that fetches the raw script source, which has a Linux line endings. </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session to act on. </param>
    /// <param name="scriptName">   Name of the script. </param>
    public static string FetchRawScript( this SessionBase session, string scriptName )
    {
        if ( session == null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        session.SetLastAction( $"checking if the {scriptName} script exists;. " );
        session.LastNodeNumber = default;
        if ( session.IsNil( scriptName ) )
            throw new InvalidOperationException( $"The script {scriptName} cannot be fetched because it was not found." );

        session.SetLastAction( $"fetching the {scriptName} script;. " );
        _ = session.WriteLine( $"_G.print({scriptName}.source)" );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        string scriptSource = session.ReadFreeLineTrimEnd();
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        // throw if device error occurred
        session.ThrowDeviceExceptionIfError();

        // query and throw if operation complete query failed
        session.QueryAndThrowIfOperationIncomplete();
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        // throw if device error occurred
        session.ThrowDeviceExceptionIfError();

        if ( string.IsNullOrWhiteSpace( scriptSource ) )
            throw new InvalidOperationException( $"The script {scriptName} source is empty." );

        return scriptSource;
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that fetches a script and replaces the
    /// Linux with Windows line terminations including a terminating new line.
    /// </summary>
    /// <remarks>   2025-05-03. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session to act on. </param>
    /// <param name="scriptName">   Name of the script. </param>
    /// <param name="validate">     (Optional) True to validate the reader writer line ending
    ///                             transformation. </param>
    /// <returns>   The script with Windows style line terminations. </returns>
    public static string FetchScript( this SessionBase session, string scriptName, bool validate = true )
    {
        if ( session == null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        // fetch the raw script
        string rawScript = session.FetchRawScript( scriptName );

        if ( string.IsNullOrWhiteSpace( rawScript ) )
            return string.Empty;
        else
        {
            // terminate the raw script with a single line ending.
            rawScript = rawScript.TrimMultipleLineEndings();

            // if ( rawScript.Contains( @"\u00A0" ) )
            //     rawScript = rawScript.Replace( @"\u00A0", " " ); // replace non breaking space with space.

            // replace line endings with Windows new line validating with the LineEndingExtensions.ReplaceLineEnding method
            return rawScript.TerminateLines( validate );
        }
    }
}
