using cc.isr.VI.Pith.ExceptionExtensions;
using cc.isr.VI.Pith.Settings;

namespace cc.isr.VI.Pith;

public partial class SessionBase
{
    #region " all settings "

    /// <summary>   Gets all settings. </summary>
    /// <value> all settings. </value>
    public AllSettings AllSettings { get; } = new AllSettings();

    /// <summary>   Define settings event handlers. </summary>
    /// <remarks>   2025-10-28. </remarks>
    private void DefineSettingsEventHandlers()
    {
        this.AllSettings.TimingSettings.PropertyChanged += this.HandleSettingsPropertyChanged;
        this.AllSettings.IOSettings.PropertyChanged += this.HandleSettingsPropertyChanged;
        this.AllSettings.ResourceSettings.PropertyChanged += this.HandleSettingsPropertyChanged;
    }

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-17. <para>
    /// Creates the scribe if null.</para>
    /// </remarks>
    /// <param name="declaringType">        The Type of the declaring object. </param>
    /// <param name="settingsFileSuffix">   (Optional) The suffix of the assembly settings file, e.g.,
    ///                                     '.session' in
    ///                                     'cc.isr.VI.Tsp.K2600.Device.MSTest.Session.json' where
    ///                                     cc.isr.VI.Tsp.K2600.Device.MSTest is the assembly name. </param>
    /// <param name="overrideAllUsersFile"> (Optional) [false] True to override all users settings file. </param>
    /// <param name="overrideThisUserFile"> (Optional) [false] True to override this user settings file. </param>
    public void ReadSettings( Type declaringType, string settingsFileSuffix = ".session", bool overrideAllUsersFile = false, bool overrideThisUserFile = false )
    {
        this.AllSettings.ReadSettings( declaringType, settingsFileSuffix, overrideAllUsersFile, overrideThisUserFile );
        this.ApplySettings( this.AllSettings.TimingSettings );
        this.ApplySettings( this.AllSettings.IOSettings );
        this.ApplySettings( this.AllSettings.ResourceSettings );
    }

    #endregion

    #region " settings event handlers"

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

    #region " IO settings event handlers "

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

    #region " Resource settings event handlers "

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

    #region " Timing settings event handlers "

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

    #endregion
}
