using cc.isr.VI.Pith.ExceptionExtensions;

namespace cc.isr.VI.Pith;

public partial class SessionBase
{
    #region " scribe "

    /// <summary>   Gets or sets the scribe. </summary>
    /// <value> The scribe. </value>
    public AppSettingsScribe? Scribe { get; set; }

    /// <summary>   Gets or sets the timing settings. </summary>
    /// <value> The timing settings. </value>
    public Settings.TimingSettings TimingSettings { get; private set; } = new Settings.TimingSettings();

    /// <summary>   Gets or sets the commands settings. </summary>
    /// <value> The commands settings. </value>
    public Settings.CommandsSettings CommandsSettings { get; private set; } = new Settings.CommandsSettings();

    /// <summary>   Gets or sets the i/o settings. </summary>
    /// <value> The i/o settings. </value>
    public Settings.IOSettings IOSettings { get; private set; } = new Settings.IOSettings();

    /// <summary>   Gets or sets the registers bitmasks settings. </summary>
    /// <value> The register settings. </value>
    public Settings.RegistersBitmasksSettings RegistersBitmasksSettings { get; private set; } = new Settings.RegistersBitmasksSettings();

    /// <summary>   Gets or sets the SCPI exceptions settings. </summary>
    /// <value> The SCPI exceptions settings. </value>
    public Settings.ScpiExceptionsSettings ScpiExceptionsSettings { get; private set; } = new Settings.ScpiExceptionsSettings();

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-05. </remarks>
    /// <param name="callingEntity">        The calling entity. </param>
    /// <param name="settingsFileSuffix">   The suffix of the assembly settings file, e.g.,
    ///                                     '.session' in
    ///                                     'cc.isr.VI.Tsp.K2600.Device.MSTest.Session.JSon' where
    ///                                     cc.isr.VI.Tsp.K2600.Device.MSTest is the assembly name. </param>
    public void ReadSettings( System.Type callingEntity, string settingsFileSuffix )
    {
        AssemblyFileInfo ai = new( callingEntity.Assembly, null, settingsFileSuffix, ".json" );

        // must copy application context settings here to clear any bad settings files.

        AppSettingsScribe.CopySettings( ai.AppContextAssemblyFilePath!, ai.AllUsersAssemblyFilePath! );
        AppSettingsScribe.CopySettings( ai.AppContextAssemblyFilePath!, ai.ThisUserAssemblyFilePath! );

        this.Scribe = new( [this.TimingSettings, this.CommandsSettings, this.IOSettings,
            this.RegistersBitmasksSettings, this.ScpiExceptionsSettings],
            ai.AppContextAssemblyFilePath!, ai.AllUsersAssemblyFilePath! )
        {
            AllUsersSettingsPath = ai.AllUsersAssemblyFilePath,
            ThisUserSettingsPath = ai.ThisUserAssemblyFilePath
        };
        this.Scribe.ReadSettings();

        this.ApplySettings( this.TimingSettings );
    }

    #endregion

    #region " Timing settings "

    /// <summary>   Applies the settings. </summary>
    /// <remarks>   2024-08-17. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="settings"> Options for controlling the operation. </param>
    private void ApplySettings( Settings.TimingSettings settings )
    {
        if ( settings is null ) throw new ArgumentNullException( nameof( settings ) );
        System.Reflection.PropertyInfo[] properties = settings.GetType().GetProperties( System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public );
        foreach ( System.Reflection.PropertyInfo? item in properties )
        {
            this.HandlePropertyChanged( settings, item.Name );
        }
    }

    /// <summary> Handles the settings property changed event. </summary>
    /// <param name="sender">       Source of the event. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( Settings.TimingSettings sender, string? propertyName )
    {
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( Settings.TimingSettings.ClearRefractoryPeriod ):
                {
                    this.ClearRefractoryPeriod = sender.ClearRefractoryPeriod;
                    break;
                }

            case nameof( Settings.TimingSettings.CommunicationTimeout ):
                {
                    this.TimeoutCandidate = sender.CommunicationTimeout;
                    break;
                }

            case nameof( Settings.TimingSettings.DeviceClearRefractoryPeriod ):
                {
                    this.DeviceClearRefractoryPeriod = sender.DeviceClearRefractoryPeriod;
                    break;
                }

            case nameof( Settings.TimingSettings.InitKnownStateTimeout ):
                {
                    this.InitKnownStateTimeout = sender.InitKnownStateTimeout;
                    break;
                }

            case nameof( Settings.TimingSettings.InterfaceClearRefractoryPeriod ):
                {
                    this.InterfaceClearRefractoryPeriod = sender.InterfaceClearRefractoryPeriod;
                    break;
                }

            case nameof( Settings.TimingSettings.OpenSessionTimeout ):
                {
                    this.OpenTimeout = sender.OpenSessionTimeout;
                    break;
                }

            case nameof( Settings.TimingSettings.OperationCompletionRefractoryPeriod ):
                {
                    this.OperationCompletionRefractoryPeriod = sender.OperationCompletionRefractoryPeriod;
                    break;
                }

            case nameof( Settings.TimingSettings.ResetRefractoryPeriod ):
                {
                    this.ResetRefractoryPeriod = sender.ResetRefractoryPeriod;
                    break;
                }

            case nameof( Settings.TimingSettings.StatusReadDelay ):
                {
                    this.StatusReadDelay = sender.StatusReadDelay;
                    break;
                }

            case nameof( Settings.TimingSettings.StatusReadTurnaroundTime ):
                {
                    this.StatusReadTurnaroundTime = sender.StatusReadTurnaroundTime;
                    break;
                }

            case nameof( Settings.TimingSettings.ReadAfterWriteDelay ):
                {
                    this.ReadAfterWriteDelay = sender.ReadAfterWriteDelay;
                    break;
                }

            case nameof( Settings.TimingSettings.SessionMessageNotificationModes ):
                {
                    this.MessageNotificationModes = sender.SessionMessageNotificationModes;
                    break;
                }
            case nameof( Settings.TimingSettings.PingHops ):
                {
                    cc.isr.VI.Pith.ResourceNamesManager.PingHops = sender.PingHops;
                    break;
                }

            case nameof( Settings.TimingSettings.PingTimeout ):
                {
                    cc.isr.VI.Pith.ResourceNamesManager.PingTimeout = sender.PingTimeout;
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Handles the settings property changed event. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void HandleSettingsPropertyChanged( object? sender, System.ComponentModel.PropertyChangedEventArgs e )
    {
        if ( this.IsDisposed || sender is null || e is null )
            return;
        try
        {
            if ( sender is Settings.TimingSettings s )
                this.HandlePropertyChanged( s, e.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = ex.AddExceptionData();
            this.OnEventHandlerError( ex );
        }
    }

    #endregion
}
