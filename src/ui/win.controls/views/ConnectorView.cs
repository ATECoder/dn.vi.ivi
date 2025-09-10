using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace cc.isr.VI.WinControls;

/// <summary> A connector view. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-12-31 </para>
/// </remarks>
public partial class ConnectorView : UserControl
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    public ConnectorView() : base() => this.InitializeComponent();

    /// <summary> Creates a new <see cref="ConnectorView"/> </summary>
    /// <returns> A <see cref="ConnectorView"/>. </returns>
    public static ConnectorView Create()
    {
        ConnectorView? view = null;
        try
        {
            view = new ConnectorView();
            return view;
        }
        catch
        {
            view?.Dispose();
            throw;
        }
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="Control" />
    /// and its child controls and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing"> <c>true</c> to release both managed and unmanaged resources;
    /// <c>false</c> to release only unmanaged
    /// resources when called from the runtime
    /// finalize. </param>
    protected override void Dispose( bool disposing )
    {
        try
        {
            if ( !this.IsDisposed && disposing )
            {
                // make sure the session is unbound in case the form is closed without closing the device.
                this.BindVisaSessionBaseThis( null );
                if ( this.components is not null )
                {
                    this.components?.Dispose();
                    this.components = null;
                }
            }
        }
        finally
        {
            base.Dispose( disposing );
        }
    }

    #endregion

    #region " visa session base (device base) "

    /// <summary> Gets or sets the visa session base. </summary>
    /// <value> The visa session base. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public VisaSessionBase? VisaSessionBase { get; private set; }

    /// <summary> Bind visa session base. </summary>
    /// <param name="value"> The value. </param>
    public void BindVisaSessionBase( VisaSessionBase? value )
    {
        this.BindVisaSessionBaseThis( value );
    }

    /// <summary> Bind visa session base. </summary>
    /// <param name="value"> The value. </param>
    private void BindVisaSessionBaseThis( VisaSessionBase? value )
    {
        if ( this.VisaSessionBase is not null )
        {
            this.VisaSessionBase.Opening -= this.DeviceOpening;
            this.VisaSessionBase.Opened -= this.DeviceOpened;
            this.VisaSessionBase.Closing -= this.DeviceClosing;
            this.VisaSessionBase.Closed -= this.DeviceClosed;
            this.VisaSessionBase.Initialized -= this.DeviceInitialized;
            this.VisaSessionBase.Initializing -= this.DeviceInitializing;
            this._resourceSelectorConnector.Enabled = false;
            this._resourceSelectorConnector.AssignSelectorViewModel( null );
            this._resourceSelectorConnector.AssignOpenerViewModel( null );
        }

        this.VisaSessionBase = value;
        if ( this.VisaSessionBase is not null )
        {
            this.VisaSessionBase.Opening += this.DeviceOpening;
            this.VisaSessionBase.Opened += this.DeviceOpened;
            this.VisaSessionBase.Closing += this.DeviceClosing;
            this.VisaSessionBase.Closed += this.DeviceClosed;
            this.VisaSessionBase.Initialized += this.DeviceInitialized;
            this.VisaSessionBase.Initializing += this.DeviceInitializing;
            this._resourceSelectorConnector.Enabled = true;
            this._resourceSelectorConnector.Openable = true;
            this._resourceSelectorConnector.Clearable = true;
            this._resourceSelectorConnector.Searchable = true;
            this._resourceSelectorConnector.AssignSelectorViewModel( this.VisaSessionBase.SessionFactory );
            this._resourceSelectorConnector.AssignOpenerViewModel( value );
        }

        this.BindSessionFactory( value );
    }

    /// <summary> Reads the status register and lets the session process the status byte. </summary>
    protected void ReadStatusRegister()
    {
        if ( this.VisaSessionBase is null ) throw new InvalidOperationException( $"{nameof( ConnectorView )}.{nameof( ConnectorView.VisaSessionBase )} is null." );
        if ( this.VisaSessionBase.Session is null ) throw new InvalidOperationException( $"{nameof( ConnectorView )}.{nameof( ConnectorView.VisaSessionBase )}.{nameof( ConnectorView.VisaSessionBase.Session )} is null." );
        string activity = $"{this.VisaSessionBase.ResourceNameCaption} reading service request";
        try
        {
            this.VisaSessionBase.Session.ApplyStatusByte( this.VisaSessionBase.Session.ReadStatusByte() );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " device events "

    /// <summary>
    /// Event handler. Called upon device opening so as to instantiated all subsystems.
    /// </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    protected void DeviceOpening( object? sender, CancelEventArgs e )
    {
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Opening access to {this.ResourceName};. " );
        this._identityTextBox.Text = "connecting...";
    }

    /// <summary>
    /// Event handler. Called after the device opened and all subsystems were defined.
    /// </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    protected void DeviceOpened( object? sender, EventArgs e )
    {
        if ( this.VisaSessionBase is null )
            throw new InvalidOperationException( $"{nameof( ConnectorView )}.{nameof( ConnectorView.VisaSessionBase )} is null." );
        this.VisaSessionBase.LogSessionOpenAndEnabled();

        string activity = string.Empty;
        try
        {
            this._identityTextBox.Text = "identifying...";
            Application.DoEvents();
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary>
    /// Attempts to open a session to the device using the specified resource name.
    /// </summary>
    /// <remarks> David, 2020-06-08. </remarks>
    /// <param name="resourceName">  The name of the resource. </param>
    /// <param name="resourceModel"> The model of the resource. </param>
    /// <returns> The (Success As Boolean, Details As String) </returns>
    public (bool Success, string Details) TryOpenDeviceSession( string resourceName, string resourceModel )
    {
        if ( this.VisaSessionBase is null ) throw new InvalidOperationException( $"{nameof( ConnectorView )}.{nameof( ConnectorView.VisaSessionBase )} is null." );

        string activity = $"opening {resourceName} VISA session";
        try
        {
            (bool success, string details) = this.VisaSessionBase.TryOpenSession( resourceName, resourceModel );
            return success ? (success, details) : (success, $"failed {activity};. {details}");
        }
        catch ( Exception ex )
        {
            return (false, $"Exception {activity};. {ex}");
        }
        finally
        {
            Cursor.Current = Cursors.Default;
        }
    }

    /// <summary> Device initializing. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Cancel event information. </param>
    protected virtual void DeviceInitializing( object? sender, CancelEventArgs e )
    {
    }

    /// <summary> Device initialized. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    protected virtual void DeviceInitialized( object? sender, EventArgs e )
    {
        if ( this.VisaSessionBase is null ) throw new InvalidOperationException( $"{nameof( ConnectorView )}.{nameof( ConnectorView.VisaSessionBase )} is null." );
        if ( this.VisaSessionBase.StatusSubsystemBase is null ) throw new InvalidOperationException( $"{nameof( ConnectorView )}.{nameof( ConnectorView.VisaSessionBase )}.{nameof( ConnectorView.VisaSessionBase.StatusSubsystemBase )} is null." );

        this._identityTextBox.Text = this.VisaSessionBase.StatusSubsystemBase.Identity;
    }

    /// <summary> Event handler. Called when device is closing. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    protected virtual void DeviceClosing( object? sender, CancelEventArgs e )
    {
        if ( this.VisaSessionBase is not null )
        {
            string activity = string.Empty;
            try
            {
                activity = $"Disconnecting from {this.ResourceName}";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.VisaSessionBase.Session?.DisableServiceRequestEventHandler();
            }
            catch ( Exception ex )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            }
        }
    }

    /// <summary> Event handler. Called when device is closed. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    protected virtual void DeviceClosed( object? sender, EventArgs e )
    {
        if ( this.VisaSessionBase is not null )
        {
            string activity = string.Empty;
            try
            {
                activity = $"Disconnected from {this.ResourceName}";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                if ( this.VisaSessionBase is not null && this.VisaSessionBase.Session is not null && this.VisaSessionBase.Session.IsSessionOpen )
                {
                    _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"{this.VisaSessionBase.Session.ResourceNameCaption} closed but session still open;. " );
                    this._identityTextBox.Text = "<failed disconnecting>";
                }
                else if ( this.VisaSessionBase is not null && this.VisaSessionBase.Session is not null && this.VisaSessionBase.Session.IsDeviceOpen )
                {
                    _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"{this.VisaSessionBase.Session.ResourceNameCaption} closed but emulated session still open;. " );
                    this._identityTextBox.Text = "<failed disconnecting>";
                }
                else
                {
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( "Disconnected; Device access closed." );
                    this._identityTextBox.Text = "<disconnected>";
                }
            }
            catch ( Exception ex )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            }
        }
        else
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( "Disconnected; Device disposed." );
        }
    }

    #endregion

    #region " session factory "

    /// <summary> Gets or sets the session factory. </summary>
    /// <value> The session factory. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public SessionFactory? SessionFactory { get; private set; }

    /// <summary> Bind session factory. </summary>
    /// <param name="value"> The value. </param>
    public void BindSessionFactory( VisaSessionBase? value )
    {
        if ( value is null )
        {
            this.BindSessionFactoryThis( null );
        }
        else
        {
            this.BindSessionFactoryThis( value.SessionFactory );
        }
    }

    /// <summary> Bind session factory. </summary>
    /// <param name="value"> The value. </param>
    private void BindSessionFactoryThis( SessionFactory? value )
    {
        if ( this.SessionFactory is not null )
            this.SessionFactory.PropertyChanged -= this.SessionFactoryPropertyChanged;

        this.SessionFactory = value;
        if ( this.SessionFactory is not null )
        {
            this.SessionFactory.PropertyChanged += this.SessionFactoryPropertyChanged;
            this.HandlePropertyChanged( this.SessionFactory, nameof( VI.SessionFactory.IsOpen ) );
        }
    }

    /// <summary> Gets or sets the name of the resource. </summary>
    /// <value> The name of the resource. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public string? ResourceName
    {
        get;
        set
        {
            if ( value is null || string.IsNullOrWhiteSpace( value ) ) value = string.Empty;
            if ( this.VisaSessionBase is not null && this.VisaSessionBase.SessionFactory is not null
                && !string.Equals( value, this.ResourceName, StringComparison.Ordinal ) )
            {
                field = value;
                this.VisaSessionBase.SessionFactory.CandidateResourceName = value;
            }
        }
    }

    /// <summary> Gets or sets the Search Pattern of the resource. </summary>
    /// <value> The Search Pattern of the resource. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public string? ResourceFilter
    {
        get => this.VisaSessionBase?.SessionFactory?.ResourcesFilter;
        set
        {
            if ( value is null || string.IsNullOrWhiteSpace( value ) ) value = string.Empty;
            if ( this.VisaSessionBase is not null && this.VisaSessionBase.SessionFactory is not null
                && !string.Equals( value, this.ResourceFilter, StringComparison.Ordinal ) )
            {
                this.VisaSessionBase.SessionFactory.ResourcesFilter = value;
            }
        }
    }

    /// <summary> Executes the session factory property changed action. </summary>
    /// <param name="sender">       Specifies the object where the call originated. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( SessionFactory sender, string? propertyName )
    {
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( this.SessionFactory.ValidatedResourceName ):
                {
                    this.ResourceName = sender.ValidatedResourceName;
                    break;
                }

            case nameof( this.SessionFactory.CandidateResourceName ):
                {
                    this.ResourceName = sender.CandidateResourceName;
                    break;
                }

            case nameof( this.SessionFactory.IsOpen ):
                {
                    if ( sender.IsOpen )
                    {
                        this.ResourceName = sender.ValidatedResourceName;
                        this._identityTextBox.Text = $"Resource {this.ResourceName} connected";
                        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Resource connected;. {this.ResourceName}" );
                    }
                    else
                    {
                        this.ResourceName = sender.CandidateResourceName;
                        this._identityTextBox.Text = $"Resource {this.ResourceName} not open";
                        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Resource not open;. {this.ResourceName}" );
                    }

                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Session factory property changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void SessionFactoryPropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        string activity = string.Empty;
        try
        {
            activity = $"handling session factory {e?.PropertyName} property changed event";
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.SessionFactoryPropertyChanged ), [sender, e] );
            else if ( sender is SessionFactory s )
                this.HandlePropertyChanged( s, e?.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion
}
