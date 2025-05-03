using cc.isr.VI.Pith;
using cc.isr.VI.Tsp.Script.ExportExtensions;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseExtensionMethods
{
    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that fetches and exports a script to file.
    /// </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session to act on. </param>
    /// <param name="scriptName">   Name of the script. </param>
    /// <param name="filePath">     Full pathname of the file. </param>
    /// <param name="overWrite">    (Optional) [false] True to over write. </param>
    /// <param name="validate">     (Optional) True to validate. </param>
    public static void ExportScript( this SessionBase session, string scriptName, string filePath, bool overWrite = false, bool validate = true )
    {
        if ( session == null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( string.IsNullOrWhiteSpace( filePath ) ) throw new ArgumentNullException( nameof( filePath ) );

        if ( !overWrite && System.IO.File.Exists( filePath ) )
            throw new InvalidOperationException( $"The script {scriptName} cannot be exported because the file '{filePath}' exists." );

        session.SetLastAction( $"checking if the {scriptName} script exists;. " );
        session.LastNodeNumber = default;
        if ( session.IsNil( scriptName ) )
            throw new InvalidOperationException( $"The script {scriptName} cannot be exported because it was not found." );

        string scriptSource = session.FetchScript( scriptName );
        if ( string.IsNullOrWhiteSpace( scriptSource ) )
            throw new InvalidOperationException( $"The script {scriptName} cannot be exported because it is empty." );

        // ensure that the source ends with a single line termination.
        scriptSource = scriptSource.TrimMultipleLineEndings();

        // write the source to file.
        scriptSource.ExportScript( filePath, overWrite, validate );
    }

    /// <summary>
    /// A <see cref="string"/> extension method that compress script source to a file.
    /// </summary>
    /// <remarks>   2025-04-16. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session to act on. </param>
    /// <param name="scriptName">   Name of the script. </param>
    /// <param name="filePath">     Full pathname of the destination file. </param>
    /// <param name="overWrite">    (Optional) [false] True to over write. </param>
    /// <param name="validate">     (Optional) True to validate. </param>
    public static void CompressScript( this SessionBase session, string scriptName, string filePath, bool overWrite = false, bool validate = true )
    {
        if ( session == null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( string.IsNullOrWhiteSpace( filePath ) ) throw new ArgumentNullException( nameof( filePath ) );

        if ( !overWrite && System.IO.File.Exists( filePath ) )
            throw new InvalidOperationException( $"The script {scriptName} cannot be exported because the file '{filePath}' exists." );

        session.SetLastAction( $"checking if the {scriptName} script exists;. " );
        session.LastNodeNumber = default;
        if ( session.IsNil( scriptName ) )
            throw new InvalidOperationException( $"The script {scriptName} cannot be exported because it was not found." );

        string scriptSource = session.FetchScript( scriptName );
        if ( string.IsNullOrWhiteSpace( scriptSource ) )
            throw new InvalidOperationException( $"The script {scriptName} cannot be exported because it is empty." );

        // ensure that the source ends with a single line termination.
        scriptSource = scriptSource.TrimMultipleLineEndings();

        // compress the source and write the source to file.
        scriptSource.CompressScript( filePath, overWrite, validate );
    }
}
