using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class Methods
{
    /// <summary>   A <see cref="SessionBase"/> extension method that fetches and exports a script to file. </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session to act on. </param>
    /// <param name="scriptName">   Name of the script. </param>
    /// <param name="filePath">     Full pathname of the file. </param>
    /// <param name="overWrite">    (Optional) [false] True to over write. </param>
    public static void ExportScript( this SessionBase session, string scriptName, string filePath, bool overWrite = false )
    {
        if ( session == null )
            throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen )
            throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( string.IsNullOrWhiteSpace( scriptName ) )
            throw new ArgumentNullException( nameof( scriptName ) );
        if ( string.IsNullOrWhiteSpace( filePath ) )
            throw new ArgumentNullException( nameof( filePath ) );

        if ( !overWrite && System.IO.File.Exists( filePath ) )
            throw new InvalidOperationException( $"The script {scriptName} cannot be exported because the file '{filePath}' exists." );

        session.SetLastAction( $"checking if the {scriptName} script exists;. " );
        session.LastNodeNumber = default;
        if ( session.IsNil( scriptName ) )
            throw new InvalidOperationException( $"The script {scriptName} cannot be exported because it was not found." );

        string scriptSource = session.FetchScript( scriptName );
        if ( string.IsNullOrWhiteSpace( scriptSource ) )
            throw new InvalidOperationException( $"The script {scriptName} cannot be exported because it is empty." );

        // write the source to file.
        session.ExportScript( scriptName, scriptSource, filePath, overWrite );
    }

    /// <summary>
    /// A <see cref="SessionBase"/> extension method that exports a script to file.
    /// </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session to act on. </param>
    /// <param name="scriptName">   Name of the script. </param>
    /// <param name="scriptSource"> The script source. </param>
    /// <param name="filePath">     Full pathname of the file. </param>
    /// <param name="overWrite">    (Optional) [false] True to over write. </param>
    public static void ExportScript( this SessionBase session, string scriptName, string scriptSource, string filePath, bool overWrite = false )
    {
        if ( session == null )
            throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen )
            throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( string.IsNullOrWhiteSpace( scriptName ) )
            throw new ArgumentNullException( nameof( scriptName ) );
        if ( string.IsNullOrWhiteSpace( scriptSource ) )
            throw new ArgumentNullException( nameof( scriptSource ) );
        if ( string.IsNullOrWhiteSpace( filePath ) )
            throw new ArgumentNullException( nameof( filePath ) );

        if ( !overWrite && System.IO.File.Exists( filePath ) )
            throw new InvalidOperationException( $"The script {scriptName} cannot be exported because the file '{filePath}' exists." );

        // export the source to the file.
        System.IO.File.WriteAllText( filePath, scriptSource );

        // check tat the file size matches the source size.
        FileInfo fileInfo = new( filePath );
        if ( fileInfo.Length < scriptSource.Length )
            throw new InvalidOperationException(
                $"The exported script {scriptName} file '{filePath}' length {fileInfo.Length} is smaller that the script source length {scriptSource.Length}." );
    }

}
