using System.Diagnostics;
using cc.isr.Std.StackTraceExtensions;
using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy;

public partial class LegacyDevice : CommunityToolkit.Mvvm.ComponentModel.ObservableObject, IDevice
{
    #region " post transient delay " 

    /// <summary> Gets or sets the delay time in seconds between the end of the thermal transient and
    /// the start of the final cold resistance measurement. </summary>
    /// <value> The post transient delay. </value>
    public float PostTransientDelay { get; set; }

    /// <summary> Gets the delay time in seconds between the end of the thermal transient and the start
    /// of the final cold resistance measurement. </summary>
    /// <value> The post transient delay configuration. </value>
    public float PostTransientDelayConfig { get; set; }

    /// <summary> Gets the time between the thermal transient measurement and the final cold resistance
    /// measurement. </summary>
    public bool PostTransientDelayGetter()
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;

        // increase resolution for unit test equals comparison at the 0.1% level.
        const string printFormat = "%9.6f";

        string synopsis = "Getting Post Transient Delay";
        this.OnMessageAvailable( TraceEventType.Verbose, synopsis, "Instrument '{0}' getting post transient delay", this.ResourceName );

        string reading = session.QueryPrintStringFormatTrimEnd( printFormat, "_G.ttm.postTransientDelayGetter()" ) ?? string.Empty;

        if ( !float.TryParse( reading, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out float value ) )
        {
            this.OnMessageAvailable( TraceEventType.Warning, synopsis, "Failed parsing post transient delay \n{1}.", reading, new StackTrace().ParseCallStack( 4, 10, CallStackType.UserCallStack ) );
            return false;
        }

        if ( this.ReportVisaDeviceOperationOkay( synopsis, "getting post transient delay using '{0}'", session.LastMessageSent ) )
        {
            this.PostTransientDelay = value;
            return true;
        }
        else
            return false;
    }

    /// <summary> Sets the time between the thermal transient measurement and the final cold resistance
    /// measurement. </summary>
    /// <param name="value"> The value. </param>
    /// <returns> null if it fails, else. </returns>
    public bool PostTransientDelaySetter( float value )
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;

        if ( Math.Abs( this.PostTransientDelay - value ) > 10f * float.Epsilon )
        {

            string synopsis = "Setting Post Transient Delay";
            this.OnMessageAvailable( TraceEventType.Verbose, synopsis, "Instrument '{0}' setting post transient delay to {1}", this.ResourceName, value );

            string format = "_G.ttm.postTransientDelaySetter({0}) ";
            _ = session.WriteLine( format, value );
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

            if ( this.ReportVisaDeviceOperationOkay( synopsis, "setting post transient delay using '{0}'", session.LastMessageSent ) )
            {
                // set the default value.
                this.PostTransientDelay = value;
                return true;
            }
            else
                return false;
        }
        else
            return true;
    }

    #endregion

    #region " common to all entities "

    /// <summary> Initializes known state. </summary>
    /// <remarks> This erases the last reading. </remarks>
    public void InitializeKnownState()
    {
        this.FinalResistance.InitializeKnownState();
        this.InitialResistance.InitializeKnownState();
        this.ThermalTransient.InitializeKnownState();
    }

    #endregion
}
