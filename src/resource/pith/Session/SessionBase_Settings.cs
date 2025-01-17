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

    /// <summary>   Gets or sets the resource settings. </summary>
    /// <value> The resource settings. </value>
    public Settings.ResourceSettings ResourceSettings { get; private set; } = new Settings.ResourceSettings();

    /// <summary>   Gets or sets the SCPI exceptions settings. </summary>
    /// <value> The SCPI exceptions settings. </value>
    public Settings.ScpiExceptionsSettings ScpiExceptionsSettings { get; private set; } = new Settings.ScpiExceptionsSettings();

    /// <summary>   Creates a scribe. </summary>
    /// <remarks>   2025-01-13. </remarks>
    /// <param name="settingsAssembly">     The settings assembly. </param>
    /// <param name="settingsFileSuffix">   (Optional) The suffix of the assembly settings file, e.g.,
    ///                                     '.session' in
    ///                                     'cc.isr.VI.Tsp.K2600.Device.MSTest.Session.JSon' where
    ///                                     cc.isr.VI.Tsp.K2600.Device.MSTest is the assembly name. </param>
    public void CreateScribe( System.Reflection.Assembly settingsAssembly, string settingsFileSuffix = ".session" )
    {
        AssemblyFileInfo ai = new( settingsAssembly, null, settingsFileSuffix, ".json" );

        // must copy application context settings here to clear any bad settings files.

        AppSettingsScribe.CopySettings( ai.AppContextAssemblyFilePath!, ai.AllUsersAssemblyFilePath! );
        AppSettingsScribe.CopySettings( ai.AppContextAssemblyFilePath!, ai.ThisUserAssemblyFilePath! );

        this.Scribe = new( [this.TimingSettings, this.CommandsSettings, this.IOSettings,
            this.RegistersBitmasksSettings, this.ScpiExceptionsSettings, this.ResourceSettings],
            ai.AppContextAssemblyFilePath!, ai.AllUsersAssemblyFilePath! )
        {
            AllUsersSettingsPath = ai.AllUsersAssemblyFilePath,
            ThisUserSettingsPath = ai.ThisUserAssemblyFilePath
        };
    }

    /// <summary>   Creates a scribe. </summary>
    /// <remarks>   2025-01-13. </remarks>
    /// <param name="callingEntity">        The calling entity. </param>
    /// <param name="settingsFileSuffix">   The suffix of the assembly settings file, e.g.,
    ///                                     '.session' in
    ///                                     'cc.isr.VI.Tsp.K2600.Device.MSTest.Session.JSon' where
    ///                                     cc.isr.VI.Tsp.K2600.Device.MSTest is the assembly name. </param>
    public void CreateScribe( System.Type callingEntity, string settingsFileSuffix = ".session" )
    {
        this.CreateScribe( callingEntity.Assembly, settingsFileSuffix );
    }

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-17. <para>
    /// Creates the scribe if null.</para>
    /// </remarks>
    /// <param name="settingsAssembly">     The assembly where the settings fille is located.. </param>
    /// <param name="settingsFileSuffix">   (Optional) The suffix of the assembly settings file, e.g.,
    ///                                     '.session' in
    ///                                     'cc.isr.VI.Tsp.K2600.Device.MSTest.Session.JSon' where
    ///                                     cc.isr.VI.Tsp.K2600.Device.MSTest is the assembly name. </param>
    public void ReadSettings( System.Reflection.Assembly settingsAssembly, string settingsFileSuffix = ".session" )
    {
        if ( this.Scribe is null )
            this.CreateScribe( settingsAssembly, settingsFileSuffix );

        this.Scribe!.ReadSettings();

        this.ApplySettings( this.TimingSettings );
        this.ApplySettings( this.IOSettings );
        this.ApplySettings( this.ResourceSettings );
    }

    /// <summary>   Reads the settings. </summary>
    /// <remarks>
    /// 2024-08-05. <para>
    /// Creates the scribe if null.</para>
    /// </remarks>
    /// <param name="callingEntity">        The calling entity. </param>
    /// <param name="settingsFileSuffix">   (Optional) The suffix of the assembly settings file, e.g.,
    ///                                     '.session' in
    ///                                     'cc.isr.VI.Tsp.K2600.Device.MSTest.Session.JSon' where
    ///                                     cc.isr.VI.Tsp.K2600.Device.MSTest is the assembly name. </param>
    public void ReadSettings( System.Type callingEntity, string settingsFileSuffix = ".session" )
    {
        this.ReadSettings( callingEntity.Assembly, settingsFileSuffix );
    }

    #endregion

    #region " All settings "

    /// <summary> Handles the settings property changed event. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void HandleSettingsPropertyChanged( object? sender, System.ComponentModel.PropertyChangedEventArgs e )
    {
        if ( this.IsDisposed || sender is null || e is null )
            return;
        try
        {
            if ( sender is Settings.TimingSettings timingSettings )
                this.HandlePropertyChanged( timingSettings, e.PropertyName );
            else if ( sender is Settings.IOSettings ioSettings )
                this.HandlePropertyChanged( ioSettings, e.PropertyName );
            else if ( sender is Settings.ResourceSettings resourceSettings )
                this.HandlePropertyChanged( resourceSettings, e.PropertyName );

        }
        catch ( Exception ex )
        {
            _ = ex.AddExceptionData();
            this.OnEventHandlerError( ex );
        }
    }

    #endregion

    #region " IO settings "

    /// <summary>   Applies the <see cref="IOSettings"/>. </summary>
    /// <remarks>   2024-08-17. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="settings"> Options for controlling the operation. </param>
    private void ApplySettings( Settings.IOSettings settings )
    {
        if ( settings is null ) throw new ArgumentNullException( nameof( settings ) );
        System.Reflection.PropertyInfo[] properties = settings.GetType().GetProperties( System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public );
        foreach ( System.Reflection.PropertyInfo? item in properties )
        {
            this.HandlePropertyChanged( settings, item.Name );
        }
    }

    /// <summary> Handles the <see cref="IOSettings"/> settings property changed event. </summary>
    /// <param name="sender">       Source of the event. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( Settings.IOSettings sender, string? propertyName )
    {
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( Settings.IOSettings.SessionMessageNotificationModes ):
                {
                    this.MessageNotificationModes = sender.SessionMessageNotificationModes;
                    break;
                }
            case nameof( Settings.IOSettings.ReadTerminationCharacter ):
                {
                    this.ReadTerminationCharacter = sender.ReadTerminationCharacter;
                    break;
                }
            case nameof( Settings.IOSettings.ReadTerminationEnabled ):
                {
                    this.ReadTerminationCharacterEnabled = sender.ReadTerminationEnabled;
                    break;
                }
            default:
                break;
        }
    }

    #endregion

    #region " IO settings "

    /// <summary>   Applies the <see cref="ResourceSettings"/>. </summary>
    /// <remarks>   2024-08-17. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="settings"> Options for controlling the operation. </param>
    private void ApplySettings( Settings.ResourceSettings settings )
    {
        if ( settings is null ) throw new ArgumentNullException( nameof( settings ) );
        System.Reflection.PropertyInfo[] properties = settings.GetType().GetProperties( System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public );
        foreach ( System.Reflection.PropertyInfo? item in properties )
        {
            this.HandlePropertyChanged( settings, item.Name );
        }
    }

    /// <summary> Handles the <see cref="ResourceSettings"/> settings property changed event. </summary>
    /// <param name="sender">       Source of the event. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( Settings.ResourceSettings sender, string? propertyName )
    {
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( Settings.ResourceSettings.PingHops ):
                {
                    cc.isr.VI.Pith.ResourceNamesManager.PingHops = sender.PingHops;
                    break;
                }

            case nameof( Settings.ResourceSettings.PingTimeout ):
                {
                    cc.isr.VI.Pith.ResourceNamesManager.PingTimeout = sender.PingTimeout;
                    break;
                }

            default:
                break;
        }
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

            default:
                break;
        }
    }

    #endregion
}
