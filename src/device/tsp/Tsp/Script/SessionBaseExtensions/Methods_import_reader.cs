using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

/// <summary>   A session base methods. </summary>
/// <remarks>   2025-04-10. </remarks>
public static partial class SessionBaseExtensionMethods
{
    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that import script. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <exception cref="FileNotFoundException">        Thrown when the requested file is not
    ///                                                 present. </exception>
    /// <param name="session">                  The session. </param>
    /// <param name="scriptName">               Specifies the script name. Empty for an anonymous
    ///                                         script. </param>
    /// <param name="filePath">                 The file path. </param>
    /// <param name="runScriptAfterLoading">    (Optional) [false] True to run script after loading. </param>
    /// <param name="deleteExisting">           (Optional) [false] True to delete an existing script
    ///                                         if it exists. </param>
    /// <param name="ignoreExisting">           (Optional) [false] True to ignore existing script. </param>
    [Obsolete( "Stream Reader failed reading the Keithley binary file and might fail interpreting backslashes in other cases." )]
    public static void ImportScript( this SessionBase session, string? scriptName, string? filePath, bool runScriptAfterLoading = false, bool deleteExisting = false, bool ignoreExisting = false )
    {
        if ( session == null )
            throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen )
            throw new InvalidOperationException( $"{nameof( session )} is not open." );

        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( filePath is null || string.IsNullOrWhiteSpace( filePath ) ) throw new ArgumentNullException( nameof( filePath ) );

        using System.IO.StreamReader reader = (string.IsNullOrWhiteSpace( filePath ) || !System.IO.File.Exists( filePath )
                   ? null
                   : new System.IO.StreamReader( filePath )) ?? throw new System.IO.FileNotFoundException( "Failed opening script file", filePath );

        // use the text reader to load the script.
        session.LoadScript( reader, scriptName, runScriptAfterLoading, deleteExisting, ignoreExisting );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that imports a script using a <see cref="TextReader"/>.
    /// </summary>
    /// <remarks>
    /// 2025-04-16. <para>
    /// If you are attempting to import a compressed scripts with a <see cref="StringReader"/> you
    /// must decompress the script first because the <see cref="StringReader"/> cannot be rewound.  </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">                  The session. </param>
    /// <param name="reader">                   The reader. </param>
    /// <param name="scriptName">               Specifies the script name. Empty for an anonymous
    ///                                         script. </param>
    /// <param name="runScriptAfterLoading">    (Optional) [false] True to run script after loading. </param>
    /// <param name="deleteExisting">           (Optional) [false] True to delete an existing script
    ///                                         if it exists. </param>
    /// <param name="ignoreExisting">           (Optional) [false] True to ignore existing script. </param>
    [Obsolete( "Stream Reader failed reading the Keithley binary file and might fail interpreting backslashes in other cases." )]
    public static void ImportScript( this SessionBase session, TextReader reader, string? scriptName, bool runScriptAfterLoading = false, bool deleteExisting = false, bool ignoreExisting = false )
    {
        if ( session == null )
            throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen )
            throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( reader is null )
            throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        // use the text reader to load the script.
        session.LoadScript( reader, scriptName, runScriptAfterLoading, deleteExisting, ignoreExisting );
    }
}
