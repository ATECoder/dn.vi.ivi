using System;
using System.ComponentModel;
using System.Windows.Forms;
using cc.isr.Enums;
using cc.isr.Std.SplitExtensions;
using cc.isr.VI.SubsystemsWinControls.ComboBoxExtensions;

namespace cc.isr.VI.SubsystemsWinControls;

/// <summary> An Digital Output Line View. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-12-31 </para>
/// </remarks>
public partial class DigitalOutputLineView : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    public DigitalOutputLineView() : base()
    {
        this.InitializingComponents = true;
        // This call is required by the Windows Form Designer.
        this.InitializeComponent();
        this.InitializingComponents = false;
    }

    /// <summary> Creates a new DigitalOutputLineView. </summary>
    /// <returns> A DigitalOutputLineView. </returns>
    public static DigitalOutputLineView Create()
    {
        DigitalOutputLineView? view = null;
        try
        {
            view = new DigitalOutputLineView();
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

    /// <summary> Gets or sets the line identity. </summary>
    /// <value> The line identity. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public string LineIdentity
    {
        get;
        set
        {
            if ( !string.Equals( value, this.LineIdentity, StringComparison.Ordinal ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    } = string.Empty;

    /// <summary> Gets or sets the name of the line. </summary>
    /// <value> The name of the line. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public string LineName
    {
        get => this._subsystemSplitButton.Text ?? string.Empty;
        set
        {
            if ( !string.Equals( value, this.LineName, StringComparison.Ordinal ) )
            {
                this._subsystemSplitButton.Text = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the line number. </summary>
    /// <value> The line number. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public int LineNumber
    {
        get => this._lineNumberBox.ValueAsInt32;
        set
        {
            if ( value != this.LineNumber )
            {
                this._lineNumberBox.ValueAsInt32 = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the line level. </summary>
    /// <value> The line level. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public int LineLevel
    {
        get => this._lineLevelNumberBox.ValueAsInt32;
        set
        {
            if ( value != this.LineLevel )
            {
                this._lineLevelNumberBox.ValueAsInt32 = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the DigitalOutput source. </summary>
    /// <value> The DigitalOutput source. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public DigitalActiveLevels ActiveLevel
    {
        get => ( DigitalActiveLevels ) ( int ) (this._activeLevelComboBox.ComboBox.SelectedValue ?? 0);
        set
        {
            if ( value != this.ActiveLevel )
            {
                this._activeLevelComboBox.ComboBox.SelectedItem = value.ValueNamePair();
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the width of the pulse. </summary>
    /// <value> The width of the pulse. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public TimeSpan PulseWidth
    {
        get => TimeSpan.FromMilliseconds( this._pulseWidthNumberBox.ValueAsDouble );
        set
        {
            if ( value != this.PulseWidth )
            {
                this._pulseWidthNumberBox.ValueAsDouble = value.TotalMilliseconds;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Toggle line level. </summary>
    /// <remarks> David, 2020-11-12. </remarks>
    public void ToggleLineLevel()
    {
        this.ReadActiveLevel();
        this.WriteLineLevel( this.LineLevel == 0 ? 1 : 0 );
    }

    /// <summary> Reads line level. </summary>
    /// <remarks> David, 2020-11-12. </remarks>
    public void ReadLineLevel()
    {
        if ( this.Device is null || this.DigitalOutputSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing exception state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ClearExecutionState();
            this.Device.Session?.EnableServiceRequestOnOperationCompletion();
            activity = $"{this.Device?.ResourceNameCaption} reading {this._subsystemSplitButton.Text}{this.LineNumber} line level";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.DigitalOutputSubsystem.StartElapsedStopwatch();
            _ = this.DigitalOutputSubsystem.QueryLineLevel( this.LineNumber );
            this.DigitalOutputSubsystem.StopElapsedStopwatch();
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

    /// <summary> Writes the line level. </summary>
    /// <remarks> David, 2020-11-12. </remarks>
    public void WriteLineLevel( int value )
    {
        if ( this.InitializingComponents || this.Device is null || this.DigitalOutputSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing exception state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ClearExecutionState();
            this.Device.Session?.EnableServiceRequestOnOperationCompletion();
            activity = $"{this.Device.ResourceNameCaption} write {this._subsystemSplitButton.Text}{this.LineNumber}  line level";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.DigitalOutputSubsystem.StartElapsedStopwatch();
            _ = this.DigitalOutputSubsystem.ApplyLineLevel( this.LineNumber, value );
            this.DigitalOutputSubsystem.StopElapsedStopwatch();
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

    /// <summary> Pulse line level. </summary>
    /// <remarks> David, 2020-11-12. </remarks>
    /// <param name="value">    The value. </param>
    /// <param name="duration"> The duration. </param>
    public void PulseLineLevel( int value, TimeSpan duration )
    {
        if ( this.InitializingComponents || this.Device is null || this.DigitalOutputSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing exception state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ClearExecutionState();
            this.Device.Session?.EnableServiceRequestOnOperationCompletion();
            activity = $"{this.Device.ResourceNameCaption} output {this._subsystemSplitButton.Text}{this.LineNumber} pulse";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.DigitalOutputSubsystem.StartElapsedStopwatch();
            this.DigitalOutputSubsystem.Pulse( this.LineNumber, value, duration );
            this.DigitalOutputSubsystem.StopElapsedStopwatch();
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

    /// <summary> Reads active level. </summary>
    /// <remarks> David, 2020-11-12. </remarks>
    public void ReadActiveLevel()
    {
        if ( this.InitializingComponents || this.Device is null || this.DigitalOutputSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing exception state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ClearExecutionState();
            this.Device.Session?.EnableServiceRequestOnOperationCompletion();
            activity = $"{this.Device.ResourceNameCaption} read {this._subsystemSplitButton.Text}{this.LineNumber}  active level";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.DigitalOutputSubsystem.StartElapsedStopwatch();
            _ = this.DigitalOutputSubsystem.QueryDigitalActiveLevel( this.LineNumber );
            this.DigitalOutputSubsystem.StopElapsedStopwatch();
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

    /// <summary> Writes the active level. </summary>
    /// <remarks> David, 2020-11-12. </remarks>
    public void WriteActiveLevel()
    {
        if ( this.InitializingComponents || this.Device is null || this.DigitalOutputSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing exception state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ClearExecutionState();
            this.Device.Session?.EnableServiceRequestOnOperationCompletion();
            activity = $"{this.Device.ResourceNameCaption} write {this._subsystemSplitButton.Text}{this.LineNumber}  active level";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.DigitalOutputSubsystem.StartElapsedStopwatch();
            _ = this.DigitalOutputSubsystem.ApplyDigitalActiveLevel( this.LineNumber, this.ActiveLevel );
            this.DigitalOutputSubsystem.StopElapsedStopwatch();
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

    /// <summary> Applies the settings onto the instrument. </summary>
    public void ApplySettings()
    {
        if ( this.InitializingComponents || this.Device is null || this.DigitalOutputSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing exception state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ClearExecutionState();
            this.Device.Session?.EnableServiceRequestOnOperationCompletion();
            activity = $"{this.Device.ResourceNameCaption} applying {this._subsystemSplitButton.Text}{this.LineNumber}  settings";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.DigitalOutputSubsystem.StartElapsedStopwatch();
            _ = this.DigitalOutputSubsystem.ApplyDigitalActiveLevel( this.LineNumber, this.ActiveLevel );
            _ = this.DigitalOutputSubsystem.ApplyLineLevel( this.LineNumber, this.LineLevel );
            this.DigitalOutputSubsystem.StopElapsedStopwatch();
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

    /// <summary> Reads the settings from the instrument. </summary>
    public void ReadSettings()
    {
        if ( this.InitializingComponents || this.Device is null || this.DigitalOutputSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing exception state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ClearExecutionState();
            this.Device.Session?.EnableServiceRequestOnOperationCompletion();
            activity = $"{this.Device.ResourceNameCaption} reading  {this._subsystemSplitButton.Text}{this.LineNumber}  settings";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            _ = this.DigitalOutputSubsystem.QueryDigitalActiveLevel( this.LineNumber );
            _ = this.DigitalOutputSubsystem.QueryLineLevel( this.LineNumber );
            this.ApplyPropertyChanged( this.DigitalOutputSubsystem );
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

    /// <summary> The device. </summary>

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
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{value.ResourceNameCaption} assigned to {nameof( DigitalOutputLineView ).SplitWords()}" );
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

    #region " digital output subsystem "

    /// <summary> Gets or sets the DigitalOutput subsystem. </summary>
    /// <value> The DigitalOutput subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public DigitalOutputSubsystemBase? DigitalOutputSubsystem { get; private set; }

    /// <summary> Bind subsystem. </summary>
    /// <remarks> David, 2020-11-13. </remarks>
    /// <param name="subsystem">  The subsystem. </param>
    /// <param name="lineNumber"> The line number. </param>
    public void BindSubsystem( DigitalOutputSubsystemBase? subsystem, int lineNumber )
    {
        if ( this.DigitalOutputSubsystem is not null )
        {
            this.BindSubsystem( false, this.DigitalOutputSubsystem );
            this.DigitalOutputSubsystem = null;
        }

        this.LineNumber = lineNumber;
        this.DigitalOutputSubsystem = subsystem;
        if ( this.DigitalOutputSubsystem is not null )
        {
            this.BindSubsystem( true, this.DigitalOutputSubsystem );
        }
    }

    /// <summary> Bind subsystem. </summary>
    /// <param name="add">       True to add. </param>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindSubsystem( bool add, DigitalOutputSubsystemBase subsystem )
    {
        if ( add )
        {
            subsystem.PropertyChanged += this.DigitalOutputSubsystemPropertyChanged;
            this.HandlePropertyChanged( subsystem, nameof( DigitalOutputSubsystemBase.SupportedDigitalActiveLevels ) );
            // must not read setting when biding because the instrument may be locked or in a trigger mode
            // The bound values should be sent when binding or when applying property change.
            // ReadSettings( subsystem );
            this.ApplyPropertyChanged( subsystem );
        }
        else
        {
            subsystem.PropertyChanged -= this.DigitalOutputSubsystemPropertyChanged;
        }
    }

    /// <summary> Applies the property changed described by subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private void ApplyPropertyChanged( DigitalOutputSubsystemBase subsystem )
    {
        this.HandlePropertyChanged( subsystem, nameof( DigitalOutputSubsystemBase.DigitalOutputLines ) );
    }

    /// <summary> Reads the settings from the instrument. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0051:Remove unused private members", Justification = "<Pending>" )]
    private static void ReadSettings( DigitalOutputSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        subsystem.QueryDigitalOutputLines();
        subsystem.StopElapsedStopwatch();
    }

    private const int Not_Specified = -2;
    private const int Not_Set = -1;

    /// <summary> Handle the Calculate subsystem property changed event. </summary>
    /// <param name="subsystem">    The subsystem. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( DigitalOutputSubsystemBase subsystem, string? propertyName )
    {
        if ( subsystem is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( DigitalOutputSubsystemBase.DigitalOutputLines ):
                {
                    if ( subsystem.DigitalOutputLines.Contains( this.LineNumber ) )
                    {
                        this.LineLevel = subsystem.DigitalOutputLines[this.LineNumber].LineLevel.GetValueOrDefault( Not_Set );
                        this.ActiveLevel = subsystem.DigitalOutputLines[this.LineNumber].ActiveLevel;
                    }
                    else
                    {
                        this.LineLevel = Not_Specified;
                    }

                    break;
                }

            case nameof( DigitalOutputSubsystemBase.SupportedDigitalActiveLevels ):
                {
                    this.InitializingComponents = true;
                    this._activeLevelComboBox.ComboBox.ListDigitalActiveLevels( subsystem.SupportedDigitalActiveLevels );
                    this.InitializingComponents = false;
                    if ( subsystem.DigitalOutputLines.Contains( this.LineNumber ) )
                    {
                        this.ActiveLevel = subsystem.DigitalOutputLines[this.LineNumber].ActiveLevel;
                    }
                    else
                    {
                        this.LineLevel = Not_Specified;
                        this.ActiveLevel = DigitalActiveLevels.None;
                    }

                    break;
                }

            default:
                break;
                // If subsystem.ac.HasValue AndAlso Me._sourceComboBox.ComboBox.Items.Count > 0 Then
                // Me._sourceComboBox.ComboBox.SelectedItem = subsystem.TriggerSource.Value.ValueNamePair
                // End If
        }
    }

    /// <summary> DigitalOutput subsystem property changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void DigitalOutputSubsystemPropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = $"handling {nameof( DigitalOutputSubsystemBase )}.{e.PropertyName} change";
        try
        {
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.DigitalOutputSubsystemPropertyChanged ), [sender, e] );

            else if ( this._subsystemToolStrip.InvokeRequired )
                // Because ToolStripItems derive directly from Component instead of from Control, their containing ToolStrip's invoke should be used
                _ = this._subsystemToolStrip.Invoke( new Action<object, PropertyChangedEventArgs>( this.DigitalOutputSubsystemPropertyChanged ), [sender, e] );

            else if ( sender is DigitalOutputSubsystemBase s )
                this.HandlePropertyChanged( s, e.PropertyName );
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
    }

    #endregion

    #region " control event handlers "

    /// <summary> Read Button click. </summary>
    /// <remarks> David, 2020-11-12. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ReadButton_Click( object? sender, EventArgs e )
    {
        this.ReadLineLevel();
    }

    /// <summary> Write Button click. </summary>
    /// <remarks> David, 2020-11-12. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void WriteButton_Click( object? sender, EventArgs e )
    {
        this.WriteLineLevel( this.LineLevel );
    }

    /// <summary> Toggle button click. </summary>
    /// <remarks> David, 2020-11-12. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ToggleButton_Click( object? sender, EventArgs e )
    {
        this.ToggleLineLevel();
    }

    /// <summary> Pulse button click. </summary>
    /// <remarks> David, 2020-11-12. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void PulseButton_Click( object? sender, EventArgs e )
    {
        this.PulseLineLevel( this.LineLevel == 0 ? 1 : 0, this.PulseWidth );
    }

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
