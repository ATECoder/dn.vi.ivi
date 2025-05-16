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

    /// <summary>   Gets or sets the status message. </summary>
    /// <value> The status message. </value>
    public string StatusMessage { get; set; } = string.Empty;

    /// <summary>   Gets or sets the firmware installed status. </summary>
    /// <value> The firmware installed status. </value>
    public string StatusDetails { get; set; } = string.Empty;

    /// <summary>   Gets or sets the firmware actually installed version. </summary>
    /// <value> The firmware actually installed version. </value>
    public string? InstalledVersion { get; set; }

    /// <summary>   Gets or sets the firmware latest version. </summary>
    /// <value> The firmware latest version. </value>
    public string? LatestVersion { get; set; }

    /// <summary>   Gets or sets the firmware released version. </summary>
    /// <value> The firmware released version. </value>
    public string? ReleasedVersion { get; set; }

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

    /// <summary>   Gets or sets a value indicating that not all loaded scripts were saved. </summary>
    /// <value> True if not all loaded scripts were saves, false if not. </value>
    public bool MustSave { get; set; }

    /// <summary>   Gets or sets a value indicating whether or not the instrument was registered. </summary>
    /// <value> True if the instrument was registered; otherwise, false. </value>
    public bool? Registered { get; set; }

    /// <summary>   Gets or sets a value indicating whether or not the instrument was certified. </summary>
    /// <value> True if the instrument was certified; false if not or unknow if null. </value>
    public bool? Certified { get; set; }

    /// <summary>   Gets or sets the firmware status. </summary>
    /// <value> The firmware status. </value>
    public FirmwareStatus FirmwareStatus { get; set; }

    /// <summary>   Builds the firmware info for the <paramref name="embeddedScripts"/> that are read from the instrument. </summary>
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
        string? installedVersion = null;
        string? releasedVersion = null;
        string? latestVersion = null;
        bool mayDelete = false;
        bool mustLoad = false;
        bool mustSave = false;
        bool? registered = false;
        bool? certified = null;
        string statusMessage = string.Empty;
        FirmwareStatus firmwareStatus = FirmwareStatus.Current;

        // Check if instrument serial number is listed in the instrument resource.
        string serialNumber = statusSubsystem.QuerySerialNumber()!;

        if ( statusSubsystem.SerialNumberReading is null || string.IsNullOrWhiteSpace( statusSubsystem.SerialNumberReading ) )
        {
            _ = statusBuilder.AppendLine( "Instrument serial number is empty. Contact developer--Error at Firmware Loader Control Base line 196." );
            statusMessage = "Instrument serial number is empty";
        }
        else
        {
            registered = accessSubsystem.IsRegistered( serialNumber, out string details );
            if ( !registered.GetValueOrDefault( false ) )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogWarning( details );
                _ = statusBuilder.AppendLine( details );
                statusMessage = "Register this instrument";
            }

            // must load if any firmware does not exist.
            mustLoad = embeddedScripts.MustLoad;

            // allow deleting if any firmware exists.
            mayDelete = embeddedScripts.MayDelete;

            // must saving if all script loaded but not all saved.
            mustSave = embeddedScripts.MustSave;

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
                certified = accessSubsystem.CertifyIfRegistered( out details );
                // if the Support Firmware Script exists, we can check for the certification.
                if ( !certified.GetValueOrDefault( false ) )
                {
                    _ = cc.isr.VI.SessionLogger.Instance.LogWarning( details );
                    _ = statusBuilder.AppendLine( details );
                    statusMessage = "Register this instrument";
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
                    _ = statusBuilder.AppendLine( "Autoexec script not found. Use this application to load new firmware." );
                    statusMessage = "Load Firmware";
                }
                else
                {
                    if ( ScriptStatuses.Loaded != (autoExecScript.ScriptStatus & ScriptStatuses.Loaded) )
                    {
                        // set version status to indicate that the program needs to be updated.
                        firmwareStatus = FirmwareStatus.UpdateRequired;
                        _ = statusBuilder.AppendLine( $"{autoExecScript.Title} script not found. Use this application to load new firmware." );
                        statusMessage = "Load Firmware";
                    }
                    else if ( ScriptStatuses.Activated != (autoExecScript.ScriptStatus & ScriptStatuses.Activated) )
                    {
                        _ = statusBuilder.AppendLine( $"{autoExecScript.Title} script not run. Report to the vendor." );
                        statusMessage = "Run Firmware";
                    }
                    else
                    {
                        installedVersion = autoExecScript.ActualVersion;
                        releasedVersion = autoExecScript.ReleaseVersion;
                        latestVersion = autoExecScript.LatestVersion;

                        // check if we have the most current firmware
                        int compareStatus = string.Compare( installedVersion, new Version( latestVersion ).ToString( 3 ), StringComparison.Ordinal );
                        if ( compareStatus == 0 )
                        {
                            firmwareStatus = FirmwareStatus.Current;
                            _ = statusBuilder.AppendLine( $"The installed firmware version {installedVersion} is up to date. No actions required." );
                        }
                        else if ( compareStatus < 0 )
                        {
                            firmwareStatus = FirmwareStatus.UpdateRequired;
                            _ = statusBuilder.AppendLine( $"The installed firmware version {installedVersion} needs updating. Load new firmware." );
                        }
                        else if ( compareStatus > 0 )
                        {
                            firmwareStatus = FirmwareStatus.NewVersionAvailable;
                            _ = statusBuilder.AppendLine( $"The installed firmware version {installedVersion} is newer than the version you are trying to install; Contact the Vendor for the latest release" );
                        }
                    }
                }
            }
            else
            {
                // set version status to indicate that the program needs to be loaded
                firmwareStatus = FirmwareStatus.LoadFirmware;
                _ = statusBuilder.AppendLine( "Firmware not found. Use this application to load new firmware." );
                statusMessage = "Load firmware";
            }

            string savedScriptNames = embeddedScripts.GetSavedScriptsNames();
            if ( !string.IsNullOrWhiteSpace( savedScriptNames ) )
            {
                _ = statusBuilder.AppendLine( $"The following scripts are in non-volatile memory:\t{savedScriptNames}" );
            }
        }

        return new FirmwareInfo()
        {
            StatusMessage = statusMessage.TrimEndNewLine(),
            StatusDetails = statusBuilder.ToString().TrimEndNewLine(),
            MustLoad = mustLoad,
            MayDelete = mayDelete,
            MustSave = mustSave,
            Registered = registered,
            Certified = certified,
            FirmwareStatus = firmwareStatus,
            InstalledVersion = installedVersion,
            ReleasedVersion = releasedVersion,
            LatestVersion = latestVersion
        };

    }

    /// <summary>   Returns a string that represents the current object. </summary>
    /// <remarks>   2024-08-27. </remarks>
    /// <returns>   A string that represents the current object. </returns>
    public override string ToString()
    {
        System.Text.StringBuilder sb = new();
        if ( !string.IsNullOrEmpty( this.StatusMessage ) )
            _ = sb.AppendLine( $"Status Message: {this.StatusMessage}" );
        if ( !string.IsNullOrEmpty( this.StatusDetails ) )
            _ = sb.AppendLine( $"Status Details: {this.StatusDetails}" );
        _ = sb.AppendLine( $"Versions:" );
        _ = sb.AppendLine( $"     Latest: {this.LatestVersion ?? "Unknown"}." );
        _ = sb.AppendLine( $"   Released: {this.ReleasedVersion ?? "Unknown"}." );
        _ = sb.AppendLine( $"  Installed: {this.InstalledVersion ?? "Unknown"}." );
        _ = sb.AppendLine( $"May delete, i.e., any script is loaded: {this.MayDelete}." );
        _ = sb.AppendLine( $" Must load, i.e., not all scripts loaded: {this.MustLoad}." );
        _ = sb.AppendLine( $" Must save, i.e., not all loaded scripts were saved: {this.MustSave}." );
        _ = sb.AppendLine( $"Registered: {(this.Registered.HasValue ? this.Registered.Value ? "True" : "False" : "Unknown")}." );
        _ = sb.AppendLine( $" Certified: {(this.Certified.HasValue ? this.Certified.Value ? "True" : "False" : "Unknown")}." );
        _ = sb.Append( $"Firmware status is '{this.FirmwareStatus}', i.e., {this.FirmwareStatus.Description()}" );
        return sb.ToString();
    }
}
