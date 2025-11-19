using cc.isr.Enums;
using cc.isr.VI.Tsp.Script;
using cc.isr.Std.TrimExtensions;
using cc.isr.VI.Tsp.Script.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.Firmware;
/// <summary>   Information about the installed firmware. </summary>
/// <remarks>   2024-08-27. </remarks>
public class FirmwareInfo
{
    /// <summary>   Default constructor. </summary>
    /// <remarks>   2024-08-27. </remarks>
    public FirmwareInfo()
    { }

    /// <summary>   Gets or sets the required action message. </summary>
    /// <value> The status message. </value>
    public string RequiredActionMessage { get; set; } = string.Empty;

    /// <summary>   Gets or sets the firmware installed status. </summary>
    /// <value> The firmware installed status. </value>
    public string RequiredActionDetails { get; set; } = string.Empty;

    /// <summary>   Gets or sets the version of the firmware actually installed (embedded) in the instrument. </summary>
    /// <value> The version of the firmware actually installed (embedded) in the instrument. </value>
    public string? EmbeddedVersion { get; set; }

    /// <summary>   Gets or sets the version of the new firmware slated to be installed. </summary>
    /// <value> The version of the new firmware slated to be installed. </value>
    public string? NextVersion { get; set; }

    /// <summary>   Gets or sets the version of the firmware that was previously release and might be installed in the instrument. </summary>
    /// <value> The version of the firmware that was previously release and might be installed in the instrument. </value>
    public string? PriorVersion { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether scripts exit on the instrument and thus may be
    /// deleted.
    /// </summary>
    /// <value> True if scripts may be deleted, false if not. </value>
    public bool MayDelete { get; set; }

    /// <summary>
    /// Gets or sets a value indicating that not all scripts were loaded.
    /// </summary>
    /// <value> True if scripts must be loaded, false if not. </value>
    public bool MustLoad { get; set; }

    /// <summary>   Gets or sets a value indicating that not all loaded scripts were embedded. </summary>
    /// <value> True if not all loaded scripts were embedded, false if not. </value>
    public bool MustEmbed { get; set; }

    /// <summary>   Gets or sets a value indicating whether or not the instrument was registered. </summary>
    /// <value> True if the instrument was registered; otherwise, false. </value>
    public bool? Registered { get; set; }

    /// <summary>   Gets or sets a value indicating whether or not the instrument was certified. </summary>
    /// <value> True if the instrument was certified; false if not or unknow if null. </value>
    public bool? Certified { get; set; }

    /// <summary>   Gets or sets the firmware status. </summary>
    /// <value> The firmware status. </value>
    public FirmwareStatus FirmwareStatus { get; set; }

    /// <summary>
    /// Builds the firmware info for the <paramref name="embeddedScripts"/> that are read from the
    /// instrument.
    /// </summary>
    /// <remarks>   2025-04-25. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="accessSubsystem">  The access subsystem. </param>
    /// <param name="embeddedScripts">  The embedded scripts. </param>
    /// <returns>   A <see cref="FirmwareInfo"/>. </returns>
    public static FirmwareInfo BuildFirmwareInfo( AccessSubsystemBase accessSubsystem, ScriptInfoCollection embeddedScripts )
    {
        if ( accessSubsystem is null ) throw new ArgumentNullException( nameof( accessSubsystem ) );
        if ( embeddedScripts is null ) throw new ArgumentNullException( nameof( embeddedScripts ) );

        if ( accessSubsystem.Session is null ) throw new InvalidOperationException( $"{nameof( AccessSubsystemBase )}.{nameof( AccessSubsystemBase.Session )}" );
        if ( !accessSubsystem.Session.IsDeviceOpen ) throw new InvalidOperationException( $"{nameof( AccessSubsystemBase )}.{nameof( AccessSubsystemBase.Session )} is not open." );


        Pith.SessionBase session = accessSubsystem.Session;
        Tsp.StatusSubsystemBase statusSubsystem = ( Tsp.StatusSubsystemBase ) accessSubsystem.StatusSubsystem;
        System.Text.StringBuilder statusBuilder = new();
        string? embeddedVersion = null;
        string? priorVersion = null;
        string? nextVersion = null;
        bool mayDelete = false;
        bool mustLoad = false;
        bool mustEmbed = false;
        bool? registered = false;
        bool? certified = null;
        string requiredAction = "None";
        FirmwareStatus firmwareStatus = FirmwareStatus.Current;

        // Check if instrument serial number is listed in the instrument resource.
        string serialNumber = statusSubsystem.QuerySerialNumber()!;

        if ( statusSubsystem.SerialNumberReading is null || string.IsNullOrWhiteSpace( statusSubsystem.SerialNumberReading ) )
        {
            _ = statusBuilder.AppendLine( "Instrument serial number reading is empty." );
            requiredAction = "Contact the developer";
        }
        else
        {
            registered = accessSubsystem.IsRegistered( serialNumber, out string details );
            if ( !registered.GetValueOrDefault( false ) )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogWarning( details );
                _ = statusBuilder.AppendLine( details );
                requiredAction = "Register this instrument";
            }

            // must load if any firmware does not exist.
            mustLoad = embeddedScripts.MustLoad;

            // allow deleting if any firmware exists.
            mayDelete = embeddedScripts.MayDelete;

            // must embed if all script loaded but not all are embedded.
            mustEmbed = embeddedScripts.MustEmbed;

            ScriptInfo? supportScriptInfo = null;
            foreach ( ScriptInfo scriptInfo in embeddedScripts )
            {
                if ( scriptInfo.IsSupport )
                {
                    supportScriptInfo = scriptInfo;
                    break;
                }
            }

            // check if the support firmware is embedded in the instrument
            if ( supportScriptInfo is not null && (ScriptStatuses.Loaded == (supportScriptInfo.ScriptStatus & ScriptStatuses.Loaded)) )
            {
                string scriptName = supportScriptInfo.Title;
                session.RunScript( scriptName );
                certified = accessSubsystem.TryCertifyIfRegistered( out details );
                // if the Support Firmware Script exists, we can check for the certification.
                if ( !certified.GetValueOrDefault( false ) )
                {
                    _ = cc.isr.VI.SessionLogger.Instance.LogWarning( details );
                    _ = statusBuilder.AppendLine( details );
                    requiredAction = "Register this instrument";
                }

                // read the firmware versions
                // session.QueryFirmwareVersion( this.NodeScriptsCollection );

                // use the autoexec firmware to report the version status.
                ScriptInfo? autoExecScript = null;
                foreach ( ScriptInfo scriptInfo in embeddedScripts )
                {
                    if ( scriptInfo.IsAutoexec )
                    {
                        autoExecScript = scriptInfo;
                        break;
                    }
                }

                if ( autoExecScript is null )
                {
                    _ = statusBuilder.AppendLine( "Autoexec script not found." );
                    requiredAction = "Use this application to load new firmware";
                }
                else
                {
                    if ( ScriptStatuses.Loaded != (autoExecScript.ScriptStatus & ScriptStatuses.Loaded) )
                    {
                        // set version status to indicate that the program needs to be updated.
                        firmwareStatus = FirmwareStatus.UpdateRequired;
                        _ = statusBuilder.AppendLine( $"The '{autoExecScript.Title}' firmware was not found." );
                        requiredAction = "Use this application to load new firmware";
                    }
                    else if ( ScriptStatuses.Activated != (autoExecScript.ScriptStatus & ScriptStatuses.Activated) )
                    {
                        _ = statusBuilder.AppendLine( $"The {autoExecScript.Title} firmware is inactive." );
                        requiredAction = "Contact the developer";
                    }
                    else
                    {
                        embeddedVersion = autoExecScript.EmbeddedVersion;
                        priorVersion = autoExecScript.PriorVersion;
                        nextVersion = autoExecScript.NextVersion;

                        // check if we have the most current firmware
                        int compareStatus = string.Compare( embeddedVersion, new Version( nextVersion ).ToString( 3 ), StringComparison.Ordinal );
                        if ( compareStatus == 0 )
                        {
                            firmwareStatus = FirmwareStatus.Current;
                            requiredAction = "None";
                            _ = statusBuilder.AppendLine( $"The installed (embedded) firmware version {embeddedVersion} is up to date." );
                        }
                        else if ( compareStatus < 0 )
                        {
                            firmwareStatus = FirmwareStatus.UpdateRequired;
                            requiredAction = "Use this application to load new firmware";
                            _ = statusBuilder.AppendLine( $"The installed (embedded) firmware version {embeddedVersion} needs updating." );
                        }
                        else if ( compareStatus > 0 )
                        {
                            requiredAction = "Get a new release of this application";
                            firmwareStatus = FirmwareStatus.NewVersionAvailable;
                            _ = statusBuilder.AppendLine( $"The installed (embedded) firmware version {embeddedVersion} is newer than the version you are trying to install." );
                        }
                    }
                }
            }
            else
            {
                // set version status to indicate that the program needs to be loaded
                firmwareStatus = FirmwareStatus.LoadFirmware;
                _ = statusBuilder.AppendLine( "Firmware not found. Use this application to load new firmware." );
                requiredAction = "Use this application to load new firmware";
            }

            string embeddedScriptNames = embeddedScripts.GetEmbeddedScriptsNames();
            if ( !string.IsNullOrWhiteSpace( embeddedScriptNames ) )
            {
                _ = statusBuilder.AppendLine( $"The following firmware is embedded in non-volatile memory:\n    {embeddedScriptNames}." );
            }
        }

        return new FirmwareInfo()
        {
            RequiredActionMessage = requiredAction.TrimEndNewLine(),
            RequiredActionDetails = statusBuilder.ToString().TrimEndNewLine(),
            MustLoad = mustLoad,
            MayDelete = mayDelete,
            MustEmbed = mustEmbed,
            Registered = registered,
            Certified = certified,
            FirmwareStatus = firmwareStatus,
            EmbeddedVersion = embeddedVersion,
            PriorVersion = priorVersion,
            NextVersion = nextVersion
        };

    }

    /// <summary>   Builds the report. </summary>
    /// <remarks>   2024-08-27. </remarks>
    /// <returns>   A string that represents the current object. </returns>
    public string BuildReport( string frameworkId )
    {
        System.Text.StringBuilder sb = new();
        _ = sb.AppendLine( $"{frameworkId} firmware status: '{this.FirmwareStatus}':" );
        _ = sb.AppendLine( $"\t{this.FirmwareStatus.Description()}" );
        _ = sb.AppendLine( $"Required action: {this.RequiredActionMessage}." );
        foreach ( string line in this.RequiredActionDetails.Split( ['\r', '\n'], StringSplitOptions.RemoveEmptyEntries ) )
        {
            _ = sb.AppendLine( $"\t{line}" );
        }
        _ = sb.AppendLine( $"Versions:" );
        _ = sb.AppendLine( $"      Next: {this.NextVersion ?? "Unknown"}." );
        _ = sb.AppendLine( $"     Prior: {this.PriorVersion ?? "Unknown"}." );
        _ = sb.AppendLine( $"  Embedded: {this.EmbeddedVersion ?? "Unknown"}." );
        _ = sb.AppendLine( $"Possible actions:" );
        _ = sb.AppendLine( $"  May delete, i.e., any script is loaded: {this.MayDelete}." );
        _ = sb.AppendLine( $"   Must load, i.e., not all scripts loaded: {this.MustLoad}." );
        _ = sb.AppendLine( $"  Must embed, i.e., not all loaded scripts were embedded: {this.MustEmbed}." );
        _ = sb.AppendLine( $"API access status:" );
        _ = sb.AppendLine( $"  Registered: {(this.Registered.HasValue ? this.Registered.Value ? "True" : "False" : "Unknown")}." );
        _ = sb.Append( $"   Certified: {(this.Certified.HasValue ? this.Certified.Value ? "True" : "False" : "Unknown")}." );
        return sb.ToString();
    }
}
