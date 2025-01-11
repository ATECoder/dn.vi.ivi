using System;
using System.ComponentModel;
using System.Windows.Forms;
using cc.isr.Std.SplitExtensions;

namespace cc.isr.VI.SubsystemsWinControls;

/// <summary> A Slot view. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-12-31 </para>
/// </remarks>
public partial class SlotView : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    public SlotView() : base()
    {
        this.InitializingComponents = true;
        // This call is required by the Windows Form Designer.
        this.InitializeComponent();
        this.InitializingComponents = false;
        this._settlingTimeNumeric.NumericUpDown.Minimum = 0m;
        this._settlingTimeNumeric.NumericUpDown.Maximum = 1000m;
        this._settlingTimeNumeric.NumericUpDown.DecimalPlaces = 0;
        this._settlingTimeNumeric.NumericUpDown.Increment = 10m;
        this._slotNumberNumeric.NumericUpDown.Minimum = 1m;
        this._slotNumberNumeric.NumericUpDown.Maximum = 10m;
        this._slotNumberNumeric.NumericUpDown.DecimalPlaces = 0;
        this._slotNumberNumeric.NumericUpDown.Increment = 1m;
    }

    /// <summary> Creates a new <see cref="SlotView"/> </summary>
    /// <returns> A <see cref="SlotView"/>. </returns>
    public static SlotView Create()
    {
        SlotView? view = null;
        try
        {
            view = new SlotView();
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
                this.InitializingComponents = true;
                // make sure the device is unbound in case the form is closed without closing the device.
                this.AssignDeviceThis( null );
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

    #region " public members "

    /// <summary> Gets or sets the slot card number. </summary>
    /// <value> The slot card number. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public int SlotNumber
    {
        get => ( int ) this._slotNumberNumeric.Value;
        set
        {
            if ( value != this.SlotNumber )
            {
                this._slotNumberNumeric.Value = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the settling time. </summary>
    /// <value> The settling time. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public TimeSpan SettlingTime
    {
        get => TimeSpan.FromMilliseconds( ( double ) this._settlingTimeNumeric.Value );
        set
        {
            if ( value != this.SettlingTime )
            {
                this._settlingTimeNumeric.Value = ( decimal ) value.TotalMilliseconds;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the type of the card. </summary>
    /// <value> The type of the card. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public string CardType
    {
        get => this._cardTypeTextBox.Text;
        set
        {
            if ( !string.Equals( value, this.CardType, StringComparison.Ordinal ) )
            {
                this._cardTypeTextBox.Text = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Reads the settings. </summary>
    private void ReadSettings()
    {
        if ( this.RouteSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device?.ResourceNameCaption} reading slot configuration";
            ReadSettings( this.RouteSubsystem, this.SlotNumber );
            this.ApplyPropertyChanged( this.RouteSubsystem );
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Applies the settings. </summary>
    public void ApplySettings()
    {
        if ( this.RouteSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device?.ResourceNameCaption} updating slot configuration";
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            if ( this.SlotNumber > 0 )
            {
                _ = this.RouteSubsystem.ApplySlotCardType( this.SlotNumber, this._cardTypeTextBox.Text );
                _ = this.RouteSubsystem.ApplySlotCardSettlingTime( this.SlotNumber, TimeSpan.FromMilliseconds( ( double ) this._settlingTimeNumeric.Value ) );
            }
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    #endregion

    #region " device "

    /// <summary> Gets the device. </summary>
    /// <value> The device. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public VisaSessionBase? Device { get; private set; }

    /// <summary> Assigns the device and binds the relevant subsystem values. </summary>
    /// <param name="value"> The value. </param>
    [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
    private void AssignDeviceThis( VisaSessionBase? value )
    {
        if ( this.Device is not null )
        {
            this.Device = null;
        }

        this.Device = value;
        if ( value is not null )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{value.ResourceNameCaption} assigned to {nameof( SlotView ).SplitWords()}" );
        }
    }

    /// <summary> Assigns a device. </summary>
    /// <param name="value"> True to show or False to hide the control. </param>
    public void AssignDevice( VisaSessionBase? value )
    {
        this.AssignDeviceThis( value );
    }

    /// <summary> Reads the status register and lets the session process the status byte. </summary>
    protected void ReadStatusRegister()
    {
        if ( this.Device is null || this.Device.Session is null ) return;
        string activity = $"{this.Device.ResourceNameCaption} reading service request";
        try
        {
            this.Device.Session.ApplyStatusByte( this.Device.Session.ReadStatusByte() );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " route subsystem "

    /// <summary> Gets or sets the Route subsystem. </summary>
    /// <value> The Route subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public RouteSubsystemBase? RouteSubsystem { get; private set; }

    /// <summary> Bind Route subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    public void BindSubsystem( RouteSubsystemBase subsystem )
    {
        if ( this.RouteSubsystem is not null )
        {
            this.BindSubsystem( false, this.RouteSubsystem );
            this.RouteSubsystem = null;
        }

        this.RouteSubsystem = subsystem;
        if ( this.RouteSubsystem is not null )
            this.BindSubsystem( true, this.RouteSubsystem );
    }

    /// <summary> Bind subsystem. </summary>
    /// <param name="add">       True to add. </param>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindSubsystem( bool add, RouteSubsystemBase subsystem )
    {
        if ( add )
        {
            subsystem.PropertyChanged += this.RouteSubsystemPropertyChanged;
            // must not read setting when biding because the instrument may be locked or in a trigger mode
            // The bound values should be sent when binding or when applying property change.
            // ReadSettings( subsystem );
            this.ApplyPropertyChanged( subsystem );
        }
        else
            subsystem.PropertyChanged -= this.RouteSubsystemPropertyChanged;
    }

    /// <summary> Applies the property changed described by subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private void ApplyPropertyChanged( RouteSubsystemBase subsystem )
    {
        this.HandlePropertyChanged( subsystem, nameof( RouteSubsystemBase.SlotCardType ) );
        this.HandlePropertyChanged( subsystem, nameof( RouteSubsystemBase.SlotCardSettlingTime ) );
    }

    /// <summary> Reads the settings. </summary>
    /// <param name="subsystem">  The subsystem. </param>
    /// <param name="slotNumber"> The slot card number. </param>
    private static void ReadSettings( RouteSubsystemBase subsystem, int slotNumber )
    {
        if ( slotNumber > 0 )
        {
            subsystem.StartElapsedStopwatch();
            _ = subsystem.QuerySlotCardType( slotNumber );
            _ = subsystem.QuerySlotCardSettlingTime( slotNumber );
            subsystem.StopElapsedStopwatch();
        }
    }

    /// <summary> Handle the Route subsystem property changed event. </summary>
    /// <param name="subsystem">    The subsystem. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( RouteSubsystemBase subsystem, string? propertyName )
    {
        if ( subsystem is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( RouteSubsystemBase.SlotCardType ):
                {
                    this._cardTypeTextBox.Text = subsystem.SlotCardType( this.SlotNumber );
                    break;
                }

            case nameof( RouteSubsystemBase.SlotCardSettlingTime ):
                {
                    this.SettlingTime = subsystem.SlotCardSettlingTime( this.SlotNumber );
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Route subsystem property changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void RouteSubsystemPropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = $"handling {nameof( RouteSubsystemBase )}.{e.PropertyName} change";
        try
        {
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.RouteSubsystemPropertyChanged ), [sender, e] );

            else if ( this._subsystemToolStrip.InvokeRequired )
                // Because ToolStripItems derive directly from Component instead of from Control, their containing ToolStrip's invoke should be used
                _ = this._subsystemToolStrip.Invoke( new Action<object, PropertyChangedEventArgs>( this.RouteSubsystemPropertyChanged ), [sender, e] );

            else if ( sender is RouteSubsystemBase s )
                this.HandlePropertyChanged( s, e.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " control event handlers "

    /// <summary> Applies the settings menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ApplySettingsMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.ApplySettings();
    }

    /// <summary> Reads settings menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ReadSettingsMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.ReadSettings();
    }

    #endregion


}
