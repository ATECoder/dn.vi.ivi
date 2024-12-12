using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.StatusSubsystemBaseExtensions;

public static partial class FirmwareManager
{
    /// <summary>   Loads a named script into the instrument. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
    /// <param name="statusSubsystem">  A reference to a
    ///                                 <see cref="VI.Tsp.StatusSubsystemBase">status
    ///                                 subsystem</see>. </param>
    /// <param name="scriptName">       Specifies the script name. </param>
    /// <param name="filePath">         The file path. </param>
    public static void LoadScriptFileSimple( this StatusSubsystemBase? statusSubsystem, string? scriptName, string? filePath )
    {
        if ( statusSubsystem is null ) throw new ArgumentNullException( nameof( statusSubsystem ) );
        if ( statusSubsystem.Session is null ) throw new ArgumentNullException( nameof( statusSubsystem.Session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( filePath is null || string.IsNullOrWhiteSpace( filePath ) ) throw new ArgumentNullException( nameof( filePath ) );

        Pith.SessionBase session = statusSubsystem.Session;

        using StreamReader? tspFile = FirmwareFileInfo.OpenScriptFile( filePath ) ?? throw new System.IO.FileNotFoundException( "Failed opening script file", filePath );

        string line;
        session.SetLastAction( $"load script '{scriptName}' from {filePath}" );
        session.LastNodeNumber = default;
        _ = session.WriteLine( "loadscript {0}", scriptName );
        while ( !tspFile.EndOfStream )
        {
            line = tspFile.ReadLine().Trim();
            if ( !string.IsNullOrWhiteSpace( line ) )
                _ = session.WriteLine( line );
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay );
        }
        _ = session.WriteLine( "endscript" );
    }

}
