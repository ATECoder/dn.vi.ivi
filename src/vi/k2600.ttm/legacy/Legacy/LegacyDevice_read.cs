using System.Diagnostics;
using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy;

public partial class LegacyDevice : CommunityToolkit.Mvvm.ComponentModel.ObservableObject, IDevice
{
    private string _lastReading = string.Empty;
    /// <summary> Returns the last reading. </summary>
    /// <value> The last reading. </value>
    public string LastReading
    {
        get => this._lastReading;
        set => _ = this.SetProperty( ref this._lastReading, value );
    }

    private string _lastOutcomeReading = string.Empty;
    /// <summary> Returns the last outcome reading. </summary>
    /// <value> The last outcome reading. </value>
    public string LastOutcomeReading
    {
        get => this._lastOutcomeReading;
        set => _ = this.SetProperty( ref this._lastOutcomeReading, value );
    }

    private string _lastStatusReading = string.Empty;
    /// <summary> Returns the last status reading. </summary>
    /// <value> The last status reading. </value>
    public string LastStatusReading
    {
        get => this._lastStatusReading;
        set => _ = this.SetProperty( ref this._lastStatusReading, value );
    }

    private string _lastOkayReading = string.Empty;
    /// <summary> Returns the last okay reading. </summary>
    /// <value> The last okay reading. </value>
    public string LastOkayReading
    {
        get => this._lastOkayReading;
        set => _ = this.SetProperty( ref this._lastOkayReading, value );
    }

    private string _lastOrphanMessages = string.Empty;
    /// <summary>   Gets or sets the last read orphan messages. </summary>
    /// <value> The last read orphan messages. </value>
    public string LastOrphanMessages
    {
        get => this._lastOrphanMessages;
        set => _ = this.SetProperty( ref this._lastOrphanMessages, value );
    }

    /// <summary> Reads the Final resistance and returns True if OK. Sets the last
    /// <see cref="LastReading">reading</see>,
    /// <see cref="LastOutcomeReading">outcome</see> and <see cref="LastStatusReading">status</see>
    /// The outcome is left empty if measurements were not made. </summary>
    /// <returns> The final resistance. </returns>
    public bool ReadFinalResistance()
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;

        this.OnMessageAvailable( TraceEventType.Verbose, "Reading Final resistance", "Instrument '{0}' reading Final Resistance", this.ResourceName );
        this.LastReading = string.Empty;
        this.LastOutcomeReading = string.Empty;
        this.LastStatusReading = string.Empty;
        this.LastOkayReading = string.Empty;

        if ( session.IsTrue( "_G.ttm.fr.outcome == nil " ) )
        {
            this.LastOutcomeReading = SessionBase.NilValue;
            this.LastStatusReading = SessionBase.NilValue;
            this.LastReading = SessionBase.NilValue;
            this.LastOkayReading = SessionBase.NilValue;
            this.OnMessageAvailable( TraceEventType.Verbose, "Final Resistance measurement not made", "Instrument '{0}' Final Resistance measurement not made", this.ResourceName );
            // return false if measurement was not made
            return false;
        }
        else
        {
            this.LastOkayReading = session.QueryPrintTrimEnd( "_G.ttm.fr:isOkay() " ) ?? string.Empty;
            this.OnMessageAvailable( TraceEventType.Verbose, "Checking Final Resistance okay", "Instrument '{0}' checking if Final Resistance measurement is okay", this.ResourceName );
            if ( session.IsTrue( "_G.ttm.fr:isOkay() " ) )
            {
                this.OnMessageAvailable( TraceEventType.Verbose, "Querying Final resistance", "Instrument '{0}' querying Final Resistance reading", this.ResourceName );
                this.LastReading = session.QueryPrintStringFormatTrimEnd( "%8.5f", "_G.ttm.fr.resistance" ) ?? string.Empty;
                this.OnMessageAvailable( TraceEventType.Verbose, "Received Final Resistance reading", "Instrument '{0}' received Final Resistance reading ='{1}'", this.ResourceName, this.LastReading );

                this.OnMessageAvailable( TraceEventType.Verbose, "Final Resistance okay--getting outcome", "Instrument '{0}' Final Resistance measurement okay--reading outcome", this.ResourceName );
                this.LastOutcomeReading = session.QueryPrintStringFormatTrimEnd( "%d", "_G.ttm.fr.outcome" ) ?? string.Empty;
                this.OnMessageAvailable( TraceEventType.Verbose, "Final Resistance outcome received", "Instrument '{0}' Final Resistance outcome = {1}", this.ResourceName, this.LastOutcomeReading );

                this.OnMessageAvailable( TraceEventType.Verbose, "Final Resistance okay--getting Status", "Instrument '{0}' Final Resistance measurement okay--reading status", this.ResourceName );
                this.LastStatusReading = session.QueryPrintStringFormatTrimEnd( "%d", "_G.ttm.fr.status" ) ?? string.Empty;
                this.OnMessageAvailable( TraceEventType.Verbose, "Final Resistance Status received", "Instrument '{0}' Final Resistance status= {1}", this.ResourceName, this.LastStatusReading );
                return true;
            }
            else
            {
                // if outcome failed, read and parse the outcome and status.
                this.OnMessageAvailable( TraceEventType.Verbose, "Final Resistance not okay--getting outcome", "Instrument '{0}' Final Resistance measurement failed--reading outcome", this.ResourceName );
                this.LastOutcomeReading = session.QueryPrintStringFormatTrimEnd( "%d", "_G.ttm.fr.outcome" ) ?? string.Empty;
                this.OnMessageAvailable( TraceEventType.Verbose, "Final Resistance outcome received", "Instrument '{0}' Final Resistance outcome = {1}", this.ResourceName, this.LastOutcomeReading );

                this.OnMessageAvailable( TraceEventType.Verbose, "Final Resistance not okay--getting Status", "Instrument '{0}' Final Resistance measurement failed--reading status", this.ResourceName );
                this.LastStatusReading = session.QueryPrintStringFormatTrimEnd( "%d", "_G.ttm.fr.status" ) ?? string.Empty;
                this.OnMessageAvailable( TraceEventType.Verbose, "Final Resistance Status received", "Instrument '{0}' Final Resistance status= {1}", this.ResourceName, this.LastStatusReading );

                return false;
            }
        }

    }

    /// <summary> Reads the Final resistance and returns True if OK. </summary>
    /// <param name="resistance"> The resistance. </param>
    /// <returns> The final resistance. </returns>
    private bool ReadFinalResistance( IColdResistance resistance )
    {
        if ( this.ReadFinalResistance() )
        {
            resistance.OkayReading = this.LastOkayReading;
            resistance.StatusReading = this.LastStatusReading;
            resistance.OutcomeReading = this.LastOutcomeReading;
            this.OnMessageAvailable( TraceEventType.Verbose, "Parsing good Final Resistance reading", "Instrument '{0}' parsing good final resistance reading", this.ResourceName );
            resistance.ParseReading( this.LastReading );
            return true;
        }
        else
        {
            resistance.OkayReading = this.LastOkayReading;
            resistance.StatusReading = this.LastStatusReading;
            resistance.OutcomeReading = this.LastOutcomeReading;
            this.OnMessageAvailable( TraceEventType.Verbose, "Parsing bad Final Resistance reading", "Instrument '{0}' parsing bad final resistance reading", this.ResourceName );
            resistance.ParseReading( this.LastReading );
            return false;
        }
    }

    /// <summary> Reads the Initial resistance and returns True if OK. Sets the last
    /// <see cref="LastReading">reading</see>,
    /// <see cref="LastOutcomeReading">outcome</see> and <see cref="LastStatusReading">status</see>
    /// The outcome is left empty if measurements were not made. </summary>
    /// <returns> The initial resistance. </returns>
    public bool ReadInitialResistance()
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;

        this.OnMessageAvailable( TraceEventType.Verbose, "Reading Initial resistance", "Instrument '{0}' reading Initial Resistance", this.ResourceName );

        this.LastReading = string.Empty;
        this.LastOutcomeReading = string.Empty;
        this.LastStatusReading = string.Empty;
        this.LastOkayReading = string.Empty;

        this.OnMessageAvailable( TraceEventType.Verbose, "Reading Initial resistance outcome", "Instrument '{0}' reading Initial Resistance outcome", this.ResourceName );

        if ( session.IsTrue( "_G.ttm.ir.outcome == nil " ) )
        {
            this.LastOutcomeReading = SessionBase.NilValue;
            this.LastStatusReading = SessionBase.NilValue;
            this.LastReading = SessionBase.NilValue;
            this.LastOkayReading = SessionBase.NilValue;

            this.OnMessageAvailable( TraceEventType.Verbose, "Initial resistance measurement not made", "Instrument '{0}' Initial Resistance measurement not made", this.ResourceName );
            // return false if measurement was not made
            return false;
        }
        else
        {
            this.LastOkayReading = session.QueryPrintTrimEnd( "_G.ttm.ir:isOkay() " ) ?? string.Empty;

            this.OnMessageAvailable( TraceEventType.Verbose, "Checking Initial resistance okay", "Instrument '{0}' checking if Initial Resistance measurement is okay", this.ResourceName );
            if ( session.IsTrue( "_G.ttm.ir:isOkay() " ) )
            {
                this.OnMessageAvailable( TraceEventType.Verbose, "Querying Initial resistance", "Instrument '{0}' querying Initial Resistance reading", this.ResourceName );
                this.LastReading = session.QueryPrintStringFormatTrimEnd( "%8.5f", "_G.ttm.ir.resistance" ) ?? string.Empty;
                this.OnMessageAvailable( TraceEventType.Verbose, "Received Initial resistance reading", "Instrument '{0}' received Initial Resistance reading ='{1}'", this.ResourceName, this.LastReading );

                this.OnMessageAvailable( TraceEventType.Verbose, "Initial Resistance okay--getting outcome", "Instrument '{0}' Initial Resistance measurement okay--reading outcome", this.ResourceName );
                this.LastOutcomeReading = session.QueryPrintStringFormatTrimEnd( "%d", "_G.ttm.ir.outcome" ) ?? string.Empty;
                this.OnMessageAvailable( TraceEventType.Verbose, "Initial Resistance outcome received", "Instrument '{0}' Initial Resistance outcome = {1}", this.ResourceName, this.LastOutcomeReading );

                this.OnMessageAvailable( TraceEventType.Verbose, "Initial resistance okay--getting Status", "Instrument '{0}' Initial Resistance measurement--reading status", this.ResourceName );
                this.LastStatusReading = session.QueryPrintStringFormatTrimEnd( "%d", "_G.ttm.ir.status" ) ?? string.Empty;
                this.OnMessageAvailable( TraceEventType.Verbose, "Initial resistance Status received", "Instrument '{0}' Initial Resistance status= {1}", this.ResourceName, this.LastStatusReading );
                return true;
            }
            else
            {
                // if outcome failed, read and parse the outcome and status.
                this.OnMessageAvailable( TraceEventType.Verbose, "Initial resistance not okay--getting outcome", "Instrument '{0}' Initial Resistance measurement failed--reading outcome", this.ResourceName );
                this.LastOutcomeReading = session.QueryPrintStringFormatTrimEnd( "%d", "_G.ttm.ir.outcome" ) ?? string.Empty;
                this.OnMessageAvailable( TraceEventType.Verbose, "Initial resistance outcome received", "Instrument '{0}' Initial Resistance outcome = {1}", this.ResourceName, this.LastOutcomeReading );

                this.OnMessageAvailable( TraceEventType.Verbose, "Initial resistance not okay--getting Status", "Instrument '{0}' Initial Resistance measurement failed--reading status", this.ResourceName );
                this.LastStatusReading = session.QueryPrintStringFormatTrimEnd( "%d", "_G.ttm.ir.status" ) ?? string.Empty;
                this.OnMessageAvailable( TraceEventType.Verbose, "Initial resistance Status received", "Instrument '{0}' Initial Resistance status= {1}", this.ResourceName, this.LastStatusReading );

                return false;
            }
        }

    }

    /// <summary> Meads the Initial resistance and returns True if OK. </summary>
    /// <param name="resistance"> The resistance. </param>
    /// <returns> The initial resistance. </returns>
    private bool ReadInitialResistance( IColdResistance resistance )
    {
        if ( this.ReadInitialResistance() )
        {
            resistance.OkayReading = this.LastOkayReading;
            resistance.StatusReading = this.LastStatusReading;
            resistance.OutcomeReading = this.LastOutcomeReading;
            this.OnMessageAvailable( TraceEventType.Verbose, "Parsing good Initial resistance reading", "Instrument '{0}' parsing good initial resistance reading", this.ResourceName );
            resistance.ParseReading( this.LastReading );
            return true;
        }
        else
        {
            resistance.OkayReading = this.LastOkayReading;
            resistance.StatusReading = this.LastStatusReading;
            resistance.OutcomeReading = this.LastOutcomeReading;
            this.OnMessageAvailable( TraceEventType.Verbose, "Parsing bad Initial resistance reading", "Instrument '{0}' parsing bad initial resistance reading", this.ResourceName );
            resistance.ParseReading( this.LastReading );
            return false;
        }
    }

    /// <summary> Measures the Thermal Transient and returns True if OK Sets the last
    /// <see cref="LastReading">reading</see>,
    /// <see cref="LastOutcomeReading">outcome</see> and <see cref="LastStatusReading">status</see>
    /// The outcome is left empty if measurements were not made. </summary>
    /// <returns> The thermal transient. </returns>
    public bool ReadThermalTransient()
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;

        this.OnMessageAvailable( TraceEventType.Verbose, "Reading Thermal Transient", "Instrument '{0}' reading thermal transient", this.ResourceName );

        this.LastReading = string.Empty;
        this.LastOutcomeReading = string.Empty;
        this.LastStatusReading = string.Empty;
        this.LastOkayReading = string.Empty;

        this.OnMessageAvailable( TraceEventType.Verbose, "Reading Thermal Transient outcome", "Instrument '{0}' reading thermal transient outcome", this.ResourceName );
        if ( session.IsTrue( "_G.ttm.tr.outcome == nil " ) )
        {
            this.LastOutcomeReading = SessionBase.NilValue;
            this.LastStatusReading = SessionBase.NilValue;
            this.LastReading = SessionBase.NilValue;
            this.LastOkayReading = SessionBase.NilValue;

            // return false if measurement was not made
            this.OnMessageAvailable( TraceEventType.Verbose, "Thermal Transient measurement not made", "Instrument '{0}' thermal transient measurement not made", this.ResourceName );
            return false;
        }
        else
        {
            this.LastOkayReading = session.QueryPrintTrimEnd( "_G.ttm.tr:isOkay() " ) ?? string.Empty;
            this.OnMessageAvailable( TraceEventType.Verbose, "Checking Thermal Transient okay", "Instrument '{0}' checking if thermal transient measurement is okay", this.ResourceName );
            if ( session.IsTrue( "_G.ttm.tr:isOkay() " ) )
            {
                this.OnMessageAvailable( TraceEventType.Verbose, "Querying Thermal Transient", "Instrument '{0}' querying thermal transient reading", this.ResourceName );
                this.LastReading = session.QueryPrintStringFormatTrimEnd( "%9.6f", "_G.ttm.est.voltageChange" ) ?? string.Empty;
                this.OnMessageAvailable( TraceEventType.Verbose, "Received Thermal Transient reading", "Instrument '{0}' received thermal transient reading ='{1}'", this.ResourceName, this.LastReading );
                this.LastOutcomeReading = "0";

                this.OnMessageAvailable( TraceEventType.Verbose, "Thermal Transient okay--getting Status", "Instrument '{0}' thermal transient measurement okay--reading status", this.ResourceName );
                this.LastStatusReading = session.QueryPrintStringFormatTrimEnd( "%d", "_G.ttm.tr.status" ) ?? string.Empty;
                this.OnMessageAvailable( TraceEventType.Verbose, "Thermal Transient Status received", "Instrument '{0}' thermal transient status= {1}", this.ResourceName, this.LastStatusReading );
                return true;
            }
            else
            {
                // if outcome failed, read and parse the outcome and status.
                this.OnMessageAvailable( TraceEventType.Verbose, "Thermal Transient not okay--getting outcome", "Instrument '{0}' thermal transient measurement failed--reading outcome", this.ResourceName );
                this.LastOutcomeReading = session.QueryPrintStringFormatTrimEnd( "%d", "_G.ttm.tr.outcome" ) ?? string.Empty;
                this.OnMessageAvailable( TraceEventType.Verbose, "Thermal Transient outcome received", "Instrument '{0}' thermal transient outcome = {1}", this.ResourceName, this.LastOutcomeReading );

                this.OnMessageAvailable( TraceEventType.Verbose, "Thermal Transient not okay--getting Status", "Instrument '{0}' thermal transient measurement failed--reading status", this.ResourceName );
                this.LastStatusReading = session.QueryPrintStringFormatTrimEnd( "%d", "_G.ttm.tr.status" ) ?? string.Empty;
                this.OnMessageAvailable( TraceEventType.Verbose, "Thermal Transient Status received", "Instrument '{0}' thermal transient status= {1}", this.ResourceName, this.LastStatusReading );

                return false;
            }
        }

    }

    /// <summary> Reads the thermal transient and returns True if OK. </summary>
    /// <param name="value">   The value. </param>
    /// <returns> The thermal transient. </returns>
    private bool ReadThermalTransient( IThermalTransient value )
    {
        if ( this.ReadThermalTransient() )
        {
            value.OkayReading = this.LastOkayReading;
            value.StatusReading = this.LastStatusReading;
            value.OutcomeReading = this.LastOutcomeReading;
            this.OnMessageAvailable( TraceEventType.Verbose, "Parsing good Thermal Transient reading", "Instrument '{0}' parsing good thermal transient reading", this.ResourceName );
            value.ParseReading( this.LastReading );
            return true;
        }
        else
        {
            value.OkayReading = this.LastOkayReading;
            value.StatusReading = this.LastStatusReading;
            value.OutcomeReading = this.LastOutcomeReading;
            this.OnMessageAvailable( TraceEventType.Verbose, "Parsing bad Thermal Transient reading", "Instrument '{0}' parsing bad thermal transient reading", this.ResourceName );
            value.ParseReading( this.LastReading );
            return false;
        }
    }
}
