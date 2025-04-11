using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

/// <summary>   A session base methods. </summary>
/// <remarks>   2025-04-10. </remarks>
public static partial class Methods
{
    /// <summary>   A <see cref="SessionBase"/> extension method that import script. </summary>
    /// <remarks>   2024-09-05. &lt;remarks&gt; </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="FileNotFoundException">        Thrown when the requested file is not
    ///                                                 present. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">          The session. </param>
    /// <param name="scriptName">       Specifies the script name. Empty for an anonymous script. </param>
    /// <param name="filePath">         The file path. </param>
    /// <param name="deleteExisting">   (Optional) [false] True to delete an existing script if it
    ///                                 exists. </param>
    /// <param name="ignoreExisting">   (Optional) [false] True to ignore existing script. </param>
    public static void ImportScript( this SessionBase session, string? scriptName, string? filePath, bool deleteExisting = false, bool ignoreExisting = false )
    {
        if ( session == null )
            throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen )
            throw new InvalidOperationException( $"{nameof( session )} is not open." );

        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( filePath is null || string.IsNullOrWhiteSpace( filePath ) ) throw new ArgumentNullException( nameof( filePath ) );

        using StreamReader? reader = (string.IsNullOrWhiteSpace( filePath ) || !System.IO.File.Exists( filePath )
            ? null
            : new System.IO.StreamReader( filePath )) ?? throw new System.IO.FileNotFoundException( "Failed opening script file", filePath );

        session.SetLastAction( $"checking if the {scriptName} script exists;. " );
        session.LastNodeNumber = default;
        bool scriptExists = !session.IsNil( scriptName );
        if ( scriptExists && deleteExisting )
            session.DeleteScript( scriptName );
        else if ( scriptExists && !ignoreExisting )
            throw new InvalidOperationException( $"The script {scriptName} cannot be imported over an existing script." );

        if ( string.IsNullOrWhiteSpace( scriptName ) )
        {
            session.SetLastAction( $"import script an anonymous script from {filePath}" );
            _ = session.WriteLine( $"{Syntax.Tsp.Script.LoadScriptCommand}" );
        }
        else
        {
            session.SetLastAction( $"import script '{scriptName}' from {filePath}" );
            _ = session.WriteLine( $"{Syntax.Tsp.Script.LoadScriptCommand} {scriptName}" );
        }
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        // use the text reader to load the script.
        session.LoadScript( reader, scriptName, deleteExisting, ignoreExisting );
    }

}
