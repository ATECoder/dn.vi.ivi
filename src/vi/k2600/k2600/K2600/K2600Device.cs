using System.Diagnostics;
using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.K2600;

/// <summary> Implements a Keithley 2600 source meter. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2013-12-12 </para>
/// </remarks>
public class K2600Device : VisaSessionBase
{
    #region " construction and cleanup "

    /// <summary> Initializes a new instance of the <see cref="K2600Device" /> class. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public K2600Device() : this( StatusSubsystem.Create() )
    {
    }

    /// <summary> Specialized constructor for use only by derived class. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="statusSubsystem"> The Status Subsystem. </param>
    protected K2600Device( StatusSubsystem statusSubsystem ) : base( statusSubsystem )
    {
        statusSubsystem.ExpectedLanguage = Syntax.Ieee488Syntax.LanguageTsp;
        this.StatusSubsystem = statusSubsystem;
    }

    /// <summary> Creates a new Device. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> A Device. </returns>
    public static K2600Device Create()
    {
        K2600Device? device = null;
        try
        {
            device = new K2600Device();
        }
        catch
        {
            device?.Dispose();
            throw;
        }

        return device;
    }

    /// <summary> Define the session syntax. </summary>
    protected override void ApplyDefaultSyntax()
    {
        // the session instance exists at this point.
        this.Session?.ApplyDefaultSyntax( Syntax.CommandLanguage.Tsp );
    }

    /// <summary> Validated the given device. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="device"> The device. </param>
    /// <returns> A Device. </returns>
    public static K2600Device Validated( K2600Device device )
    {
        return device is null ? throw new ArgumentNullException( nameof( device ) ) : device;
    }

    #region " i disposable support "

    /// <summary>
    /// Releases the unmanaged resources used by the object and optionally releases the managed resources.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="disposing"> true to release both managed and unmanaged resources; false to
    ///                          release only unmanaged resources. </param>
    protected override void Dispose( bool disposing )
    {
        if ( this.IsDisposed )
            return;
        try
        {
            if ( disposing )
            {
                if ( this.IsDeviceOpen )
                {
                    this.OnClosing( new System.ComponentModel.CancelEventArgs() );
                    this.StatusSubsystem?.Dispose();
                    this.StatusSubsystem = null;
                }
            }
        }
        catch ( Exception ex )
        {
            Debug.Assert( !Debugger.IsAttached, $"Exception disposing {typeof( K2600Device )}", ex.ToString() );
        }
        finally
        {
            base.Dispose( disposing );
        }
    }

    #endregion

    #endregion

    #region " session "

    /// <summary>
    /// Allows the derived device to take actions before closing. Removes subsystems and event
    /// handlers.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="e"> Event information to send to registered event handlers. </param>
    protected override void OnClosing( System.ComponentModel.CancelEventArgs e )
    {
        if ( e is null ) throw new ArgumentNullException( nameof( e ) );
        base.OnClosing( e );
        if ( !e.Cancel && this.SubsystemSupportMode == SubsystemSupportMode.Full )
        {
            this.BindMeasureResistanceSubsystem( null );
            this.BindMeasureVoltageSubsystem( null );
            this.BindSourceSubsystem( null );
            this.BindSenseSubsystem( null );
            this.BindCurrentSourceSubsystem( null );
            this.BindDisplaySubsystem( null );
            this.BindContactSubsystem( null );
            this.BindLocalNodeSubsystem( null );
            this.BindSourceSubsystem( null );
            this.BindSystemSubsystem( null );
            this.StatusSubsystem?.CloseSession();
        }
    }

    /// <summary> Allows the derived device to take actions before opening. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="e"> Event information to send to registered event handlers. </param>
    protected override void OnOpening( System.ComponentModel.CancelEventArgs e )
    {
        if ( e is null ) throw new ArgumentNullException( nameof( e ) );
        if ( this.StatusSubsystem is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( this.StatusSubsystem )} is null." );
        base.OnOpening( e );
        if ( !e.Cancel && this.SubsystemSupportMode == SubsystemSupportMode.Full )
        {
            this.BindSystemSubsystem( new SystemSubsystem( this.StatusSubsystem ) );
            this.BindSourceMeasureUnit( new SourceMeasureUnit( this.StatusSubsystem ) );
            this.BindLocalNodeSubsystem( new LocalNodeSubsystem( this.StatusSubsystem ) );
            this.BindDisplaySubsystem( new DisplaySubsystem( this.StatusSubsystem ) );
            this.BindContactSubsystem( new ContactSubsystem( this.StatusSubsystem ) );
            this.BindCurrentSourceSubsystem( new CurrentSourceSubsystem( this.StatusSubsystem ) );
            this.BindSenseSubsystem( new SenseSubsystem( this.StatusSubsystem ) );
            this.BindSourceSubsystem( new SourceSubsystem( this.StatusSubsystem ) );
            this.BindMeasureVoltageSubsystem( new MeasureVoltageSubsystem( this.StatusSubsystem ) );
            this.BindMeasureResistanceSubsystem( new MeasureResistanceSubsystem( this.StatusSubsystem ) );
        }
    }

    /// <summary>
    /// Allows the derived device to take actions after opening. Adds subsystems and event handlers.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="e"> Event information to send to registered event handlers. </param>
    protected override void OnOpened( EventArgs e )
    {
        string activity = string.Empty;
        try
        {
            activity = $"{this.ResourceModelCaption} handling on opened event";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            base.OnOpened( e );

            if ( this.Session is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( this.Session )} is null." );
            if ( !this.IsSessionOpen ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( this.Session )} is not open." );

            // TODO: why not let the enable status which is set on Init known state stay?
            this.Session.ApplyStatusByteEnableBitmask( 0 );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, $"{activity}; closing..." );
            this.CloseSession();
        }
    }

    #endregion

    #region " subsystems "

    #region " collection "

    /// <summary> Adds a subsystem. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="subsystem"> The subsystem. </param>
    public void AddSubsystem( SubsystemBase subsystem )
    {
        this.Subsystems.Add( subsystem );
    }

    /// <summary> Removes the subsystem described by subsystem. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="subsystem"> The subsystem. </param>
    public void RemoveSubsystem( SubsystemBase subsystem )
    {
        _ = this.Subsystems.Remove( subsystem );
    }

    #endregion

    #region " status "

    /// <summary> Gets or sets the Status Subsystem. </summary>
    /// <value> The Status Subsystem. </value>
    public StatusSubsystem? StatusSubsystem { get; private set; }

    #endregion

    #region " system "

    /// <summary> Gets or sets the System Subsystem. </summary>
    /// <value> The System Subsystem. </value>
    public SystemSubsystem? SystemSubsystem { get; private set; }

    /// <summary> Bind the System subsystem. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindSystemSubsystem( SystemSubsystem? subsystem )
    {
        if ( this.SystemSubsystem is not null )
        {
            _ = this.Subsystems.Remove( this.SystemSubsystem );
            this.SystemSubsystem = null;
        }

        this.SystemSubsystem = subsystem;
        if ( this.SystemSubsystem is not null )
        {
            this.Subsystems.Add( this.SystemSubsystem );
        }
    }

    #endregion

    #region " current source "

    /// <summary> Gets or sets the Current Source Subsystem. </summary>
    /// <value> The Current Source Subsystem. </value>
    public CurrentSourceSubsystem? CurrentSourceSubsystem { get; private set; }

    /// <summary> Binds the Current Source subsystem. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindCurrentSourceSubsystem( CurrentSourceSubsystem? subsystem )
    {
        if ( this.CurrentSourceSubsystem is not null )
        {
            this.SourceMeasureUnit?.Remove( this.CurrentSourceSubsystem );
            _ = this.Subsystems.Remove( this.CurrentSourceSubsystem );
            this.CurrentSourceSubsystem = null;
        }

        this.CurrentSourceSubsystem = subsystem;
        if ( this.CurrentSourceSubsystem is not null )
        {
            this.Subsystems.Add( this.CurrentSourceSubsystem );
            this.SourceMeasureUnit?.Add( this.CurrentSourceSubsystem );
        }
    }

    #endregion

    #region " contact "

    /// <summary> Gets or sets the contact Subsystem. </summary>
    /// <value> The contact Subsystem. </value>
    public ContactSubsystem? ContactSubsystem { get; private set; }

    /// <summary> Binds the contact subsystem. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindContactSubsystem( ContactSubsystem? subsystem )
    {
        if ( this.ContactSubsystem is not null )
        {
            this.SourceMeasureUnit?.Remove( this.ContactSubsystem );
            _ = this.Subsystems.Remove( this.ContactSubsystem );
            this.ContactSubsystem = null;
        }

        this.ContactSubsystem = subsystem;
        if ( this.ContactSubsystem is not null )
        {
            this.Subsystems.Add( this.ContactSubsystem );
            this.SourceMeasureUnit?.Add( this.ContactSubsystem );
        }
    }

    #endregion

    #region " display "

    /// <summary> Gets or sets the Display Subsystem. </summary>
    /// <value> The Display Subsystem. </value>
    public DisplaySubsystem? DisplaySubsystem { get; private set; }

    /// <summary> Binds the Display subsystem. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindDisplaySubsystem( DisplaySubsystem? subsystem )
    {
        if ( this.DisplaySubsystem is not null )
        {
            _ = this.Subsystems.Remove( this.DisplaySubsystem );
            this.DisplaySubsystem = null;
        }

        this.DisplaySubsystem = subsystem;
        if ( this.DisplaySubsystem is not null )
        {
            this.Subsystems.Add( this.DisplaySubsystem );
        }
    }

    #endregion

    #region " source measure unit "

    /// <summary> Gets or sets the source measure unit Subsystem. </summary>
    /// <value> The source measure unit Subsystem. </value>
    public SourceMeasureUnit? SourceMeasureUnit { get; private set; }

    /// <summary> Binds the source measure unit subsystem. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindSourceMeasureUnit( SourceMeasureUnit? subsystem )
    {
        if ( this.SourceMeasureUnit is not null )
        {
            this.SourceMeasureUnit?.Remove( this.SourceMeasureUnit );
            _ = this.Subsystems.Remove( this.SourceMeasureUnit! );
            this.SourceMeasureUnit = null;
        }

        this.SourceMeasureUnit = subsystem;
        if ( this.SourceMeasureUnit is not null )
        {
            this.Subsystems.Add( this.SourceMeasureUnit );
            this.SourceMeasureUnit?.Add( this.SourceMeasureUnit );
        }
    }

    #endregion

    #region " local node "

    /// <summary> Gets or sets the Local Node Subsystem. </summary>
    /// <value> The Local Node Subsystem. </value>
    public LocalNodeSubsystem? LocalNodeSubsystem { get; private set; }

    /// <summary> Binds the Local Node subsystem. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindLocalNodeSubsystem( LocalNodeSubsystem? subsystem )
    {
        if ( this.LocalNodeSubsystem is not null )
        {
            _ = this.Subsystems.Remove( this.LocalNodeSubsystem );
            this.LocalNodeSubsystem = null;
        }

        this.LocalNodeSubsystem = subsystem;
        if ( this.LocalNodeSubsystem is not null )
        {
            this.Subsystems.Add( this.LocalNodeSubsystem );
        }
    }

    #endregion

    #region " sense "

    /// <summary> Gets or sets the Sense Subsystem. </summary>
    /// <value> The Sense Subsystem. </value>
    public SenseSubsystem? SenseSubsystem { get; private set; }

    /// <summary> Binds the Sense subsystem. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindSenseSubsystem( SenseSubsystem? subsystem )
    {
        if ( this.SenseSubsystem is not null )
        {
            this.SourceMeasureUnit?.Remove( this.SenseSubsystem );
            _ = this.Subsystems.Remove( this.SenseSubsystem );
            this.SenseSubsystem = null;
        }

        this.SenseSubsystem = subsystem;
        if ( this.SenseSubsystem is not null )
        {
            this.Subsystems.Add( this.SenseSubsystem );
            this.SourceMeasureUnit?.Add( this.SenseSubsystem );
        }
    }

    #endregion

    #region " source "

    /// <summary> Gets or sets the Source Subsystem. </summary>
    /// <value> The Source Subsystem. </value>
    public SourceSubsystem? SourceSubsystem { get; private set; }

    /// <summary> Binds the Source subsystem. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindSourceSubsystem( SourceSubsystem? subsystem )
    {
        if ( this.SourceSubsystem is not null )
        {
            this.SourceMeasureUnit?.Remove( this.SourceSubsystem );
            _ = this.Subsystems.Remove( this.SourceSubsystem );
            this.SourceSubsystem = null;
        }

        this.SourceSubsystem = subsystem;
        if ( this.SourceSubsystem is not null )
        {
            this.Subsystems.Add( this.SourceSubsystem );
            this.SourceMeasureUnit?.Add( this.SourceSubsystem );
        }
    }

    #endregion

    #region " measure resistance "

    /// <summary> Gets or sets the Measure Resistance Subsystem. </summary>
    /// <value> The Measure Resistance Subsystem. </value>
    public MeasureResistanceSubsystem? MeasureResistanceSubsystem { get; private set; }

    /// <summary> Binds the Measure Resistance subsystem. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindMeasureResistanceSubsystem( MeasureResistanceSubsystem? subsystem )
    {
        if ( this.MeasureResistanceSubsystem is not null )
        {
            this.SourceMeasureUnit?.Remove( this.MeasureResistanceSubsystem );
            _ = this.Subsystems.Remove( this.MeasureResistanceSubsystem );
            this.MeasureResistanceSubsystem = null;
        }

        this.MeasureResistanceSubsystem = subsystem;
        if ( this.MeasureResistanceSubsystem is not null )
        {
            this.Subsystems.Add( this.MeasureResistanceSubsystem );
            this.SourceMeasureUnit?.Add( this.MeasureResistanceSubsystem );
        }
    }

    #endregion

    #region " measure voltage "

    /// <summary> Gets or sets the Measure Voltage Subsystem. </summary>
    /// <value> The Measure Voltage Subsystem. </value>
    public MeasureVoltageSubsystem? MeasureVoltageSubsystem { get; private set; }

    /// <summary> Binds the Measure Voltage subsystem. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindMeasureVoltageSubsystem( MeasureVoltageSubsystem? subsystem )
    {
        if ( this.MeasureVoltageSubsystem is not null )
        {
            this.SourceMeasureUnit?.Remove( this.MeasureVoltageSubsystem );
            _ = this.Subsystems.Remove( this.MeasureVoltageSubsystem );
            this.MeasureVoltageSubsystem = null;
        }

        this.MeasureVoltageSubsystem = subsystem;
        if ( this.MeasureVoltageSubsystem is not null )
        {
            this.Subsystems.Add( this.MeasureVoltageSubsystem );
            this.SourceMeasureUnit?.Add( this.MeasureVoltageSubsystem );
        }
    }

    #endregion

    #endregion

    #region " service request "

    /// <summary> Processes the service request. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    protected override void ProcessServiceRequest()
    {
        // device errors will be read if the error available bit is set upon reading the status byte.
        // 20240830: changed to read but not apply the status byte so as to prevent
        // a query interrupt error.
        if ( this.Session is not null )
        {
            Pith.ServiceRequests statusByte = this.Session.ReadStatusByte();
            if ( this.Session.IsErrorBitSet( statusByte ) )

                // if an error occurred, let the status subsystem handle the error
                this.Session.ApplyStatusByte( statusByte );

            else if ( this.Session.IsMessageAvailableBitSet( statusByte ) )
            {
                // read the message after delay
                _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay );

                // result is also stored in the last message received.
                this.ServiceRequestReading = this.Session.ReadFreeLineTrimEnd();

                // at this point we can allow the status subsystem to process the
                // next status byte in case reading elicited an error.
                _ = SessionBase.AsyncDelay( this.Session.StatusReadDelay );
                this.Session.ApplyStatusByte( this.Session.ReadStatusByte() );
            }
            else

                // at this point we can allow the status subsystem to process
                // other status byte events
                this.Session.ApplyStatusByte( statusByte );
        }
    }

    #endregion
}
