using System.Diagnostics;
using cc.isr.Std.StackTraceExtensions;
using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy;

/// <summary>   A legacy device. </summary>
/// <remarks>   2024-11-07. </remarks>
public partial class LegacyDevice : CommunityToolkit.Mvvm.ComponentModel.ObservableObject, IDevice
{
    #region " aparatus "

    /// <summary> Gets or sets reference to the <see cref="IThermalTransient">thermal transient</see> </summary>
    /// <value> The thermal transient. </value>
    public IThermalTransient ThermalTransient { get; set; }

    /// <summary> Gets or sets the thermal transient configuration parameters. </summary>
    /// <value> The thermal transient configuration. </value>
    public IThermalTransientConfig ThermalTransientConfig { get; set; }

    #endregion

    #region " aperture "

    /// <summary> Gets the aperture for measuring Thermal Transient. </summary>
    /// <value> The thermal transient aperture. </value>
    public float ThermalTransientAperture { get; private set; }

    /// <summary> Gets the Aperture for measuring Thermal Transient. </summary>
    public bool ThermalTransientApertureGetter()
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;

        const string printFormat = "%7.4f";

        string synopsis = "Getting Thermal Transient Aperture";
        this.OnMessageAvailable( TraceEventType.Verbose, synopsis, "Instrument '{0}' Getting thermal transient Aperture", this.ResourceName );

        string reading = session.QueryPrintStringFormatTrimEnd( printFormat, "_G.ttm.tr.aperture" ) ?? string.Empty;

        if ( !float.TryParse( reading, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out float value ) )
        {
            this.OnMessageAvailable( TraceEventType.Warning, synopsis, "Failed parsing thermal transient Aperture \n{1}.", reading, new StackTrace().ParseCallStack( 4, 10, CallStackType.UserCallStack ) );
            return false;
        }

        if ( this.ReportVisaDeviceOperationOkay( synopsis, "getting thermal transient Aperture using '{0}'", session.LastMessageSent ) )
        {
            this.ThermalTransientAperture = value;
            return true;
        }
        else
            return false;
    }

    /// <summary> Sets the aperture for measuring Thermal Transient. Defaults to 4 for A type
    /// instruments and 2 for non-A instruments. </summary>
    /// <param name="value"> The value. </param>
    /// <returns> null if it fails, else. </returns>
    public bool ThermalTransientApertureSetter( float value )
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;

        if ( Math.Abs( this.ThermalTransientAperture - value ) > 10f * float.Epsilon )
        {

            string synopsis = "Setting Thermal Transient Aperture";
            this.OnMessageAvailable( TraceEventType.Verbose, synopsis, "Instrument '{0}' setting Thermal Transient Aperture to {1}", this.ResourceName, value );

            string format = "_G.ttm.tr:apertureSetter({0}) ";
            _ = session.WriteLine( format, value );
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

            if ( this.ReportVisaDeviceOperationOkay( synopsis, "setting Thermal Transient Aperture using '{0}'", session.LastMessageSent ) )
            {
                // set the default value.
                this.ThermalTransientAperture = value;
                return true;
            }
            else
                return false;
        }
        else
            return true;
    }

    #endregion

    #region " median filter size "

    /// <summary> Size of the thermal transient median filter. </summary>

    /// <summary> Gets the Median Filter Size for measuring Thermal Transient. </summary>
    /// <value> The size of the thermal transient median filter. </value>
    public int ThermalTransientMedianFilterSize { get; private set; }

    /// <summary> Gets the Median Filter Size for measuring Thermal Transient. </summary>
    public bool ThermalTransientMedianFilterSizeGetter()
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;

        const string printFormat = "%d";

        string synopsis = "Getting Thermal Transient Median Filter Size ";
        this.OnMessageAvailable( TraceEventType.Verbose, synopsis, "Instrument '{0}' Getting thermal transient Median Filter Size ", this.ResourceName );

        string reading = session.QueryPrintStringFormatTrimEnd( printFormat, "_G.ttm.tr.medianFilterSize " ) ?? string.Empty;

        if ( !int.TryParse( reading, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out int value ) )
        {
            this.OnMessageAvailable( TraceEventType.Warning, synopsis, "Failed parsing thermal transient Median Filter Size \n{1}.", reading, new StackTrace().ParseCallStack( 4, 10, CallStackType.UserCallStack ) );
            return false;
        }

        if ( this.ReportVisaDeviceOperationOkay( synopsis, "getting thermal transient Median Filter Size using '{0}'", session.LastMessageSent ) )
        {
            this.ThermalTransientMedianFilterSize = value;
            return true;
        }
        else
            return false;

    }

    /// <summary> Sets the Median Filter Size for measuring Thermal Transient. Defaults to 3. </summary>
    /// <param name="value"> The value. </param>
    public bool ThermalTransientMedianFilterSizeSetter( int value )
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;

        if ( this.ThermalTransientMedianFilterSize != value )
        {

            string synopsis = "Setting Thermal Transient MedianFilterSize ";
            this.OnMessageAvailable( TraceEventType.Verbose, synopsis, "Instrument '{0}' setting Thermal Transient Median Filter Size to {1}", this.ResourceName, value );

            string format = "_G.ttm.tr:medianFilterSizeSetter({0}) ";
            _ = session.WriteLine( format, value );
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

            if ( this.ReportVisaDeviceOperationOkay( synopsis, "setting Thermal Transient Median Filter Size using '{0}'", session.LastMessageSent ) )
            {
                // set the default value.
                this.ThermalTransientMedianFilterSize = value;
                return true;
            }
            else
                return false;
        }
        else
            return true;
    }

    #endregion

    #region " current level "

    /// <summary> The thermal transient current level. </summary>

    /// <summary> Gets the current level for measuring Thermal Transient. </summary>
    /// <value> The thermal transient current level. </value>
    public float ThermalTransientCurrentLevel { get; private set; }

    /// <summary> Gets the current level for measuring Thermal Transient. </summary>
    public bool ThermalTransientCurrentLevelGetter()
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;

        const string printFormat = "%9.6f";

        string synopsis = "Getting Thermal Transient Current Level";
        this.OnMessageAvailable( TraceEventType.Verbose, synopsis, "Instrument '{0}' Getting thermal transient current level", this.ResourceName );

        string reading = session.QueryPrintStringFormatTrimEnd( printFormat, "_G.ttm.tr.level" ) ?? string.Empty;

        if ( !float.TryParse( reading, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out float value ) )
        {
            this.OnMessageAvailable( TraceEventType.Warning, synopsis, "Failed parsing thermal transient current level \n{1}.", reading, new StackTrace().ParseCallStack( 4, 10, CallStackType.UserCallStack ) );
            return false;
        }

        if ( this.ReportVisaDeviceOperationOkay( synopsis, "getting thermal transient current level using '{0}'", session.LastMessageSent ) )
        {
            this.ThermalTransientCurrentLevel = value;
            return true;
        }
        else
            return false;
    }

    /// <summary> Sets the current level for measuring Thermal Transient. </summary>
    /// <param name="value"> The value. </param>
    /// <returns> null if it fails, else. </returns>
    public bool ThermalTransientCurrentLevelSetter( float value )
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;

        if ( Math.Abs( this.ThermalTransientCurrentLevel - value ) > 10f * float.Epsilon )
        {

            string synopsis = "Setting Thermal Transient Current Level";
            this.OnMessageAvailable( TraceEventType.Verbose, synopsis, "Instrument '{0}' setting Thermal Transient current level to {1}", this.ResourceName, value );

            string format = "_G.ttm.tr:levelSetter({0}) ";
            _ = session.WriteLine( format, value );
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

            if ( this.ReportVisaDeviceOperationOkay( synopsis, "setting Thermal Transient current level using '{0}'", session.LastMessageSent ) )
            {
                // set the default value.
                this.ThermalTransientCurrentLevel = value;
                return true;
            }
            else
                return false;
        }
        else
            return true;

    }

    #endregion

    #region " voltage change "

    /// <summary> Gets the maximum allowed voltage change measuring Thermal Transient. </summary>
    /// <value> The thermal transient voltage change. </value>
    public float ThermalTransientVoltageChange { get; private set; }

    /// <summary> Gets the voltage change for measuring Thermal Transient. </summary>
    public bool ThermalTransientVoltageChangeGetter()
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;

        const string printFormat = "%9.6f";

        string synopsis = "Getting Thermal Transient voltage change";
        this.OnMessageAvailable( TraceEventType.Verbose, synopsis, "Instrument '{0}' Getting thermal transient voltage change", this.ResourceName );

        string reading = session.QueryPrintStringFormatTrimEnd( printFormat, "_G.ttm.tr.maxVoltageChange" ) ?? string.Empty;

        if ( !float.TryParse( reading, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out float value ) )
        {
            this.OnMessageAvailable( TraceEventType.Warning, synopsis, "Failed parsing thermal transient voltage change \n{1}.", reading, new StackTrace().ParseCallStack( 4, 10, CallStackType.UserCallStack ) );
            return false;
        }

        if ( this.ReportVisaDeviceOperationOkay( synopsis, "getting thermal transient voltage change using '{0}'", session.LastMessageSent ) )
        {
            this.ThermalTransientVoltageChange = value;
            return true;
        }
        else
            return false;

    }

    /// <summary> Sets the voltage change measuring Thermal Transient. </summary>
    /// <param name="value"> The value. </param>
    /// <returns> null if it fails, else. </returns>
    public bool ThermalTransientVoltageChangeSetter( float value )
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;

        if ( Math.Abs( this.ThermalTransientVoltageChange - value ) > 10f * float.Epsilon )
        {

            string synopsis = "Setting Thermal Transient Voltage Change";
            this.OnMessageAvailable( TraceEventType.Verbose, synopsis, "Instrument '{0}' setting Thermal Transient Voltage Change to {1}", this.ResourceName, value );

            string format = "_G.ttm.tr:maxVoltageChangeSetter({0}) ";
            _ = session.WriteLine( format, value );
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

            if ( this.ReportVisaDeviceOperationOkay( synopsis, "setting Thermal Transient Voltage Change using '{0}'", session.LastMessageSent ) )
            {
                // set the default value.
                this.ThermalTransientVoltageChange = value;
                return true;
            }
            else
                return false;
        }

        else
            return true;
    }

    #endregion

    #region " high limit "

    /// <summary> Gets the maximum allowed voltage change measuring Thermal Transient. </summary>
    /// <value> The thermal transient high limit. </value>
    public float ThermalTransientHighLimit
    {
        get => this.ThermalTransient.HighLimit;
        set => this.ThermalTransient.HighLimit = value;
    }

    /// <summary> Gets the voltage change for measuring Thermal Transient. </summary>
    public bool ThermalTransientHighLimitGetter()
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;

        const string printFormat = "%9.6f";

        string synopsis = "Getting Thermal Transient voltage change";
        this.OnMessageAvailable( TraceEventType.Verbose, synopsis, "Instrument '{0}' Getting thermal transient high limit", this.ResourceName );

        string reading = session.QueryPrintStringFormatTrimEnd( printFormat, "_G.ttm.tr.highLimit" ) ?? string.Empty;

        if ( !float.TryParse( reading, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out float value ) )
        {
            this.OnMessageAvailable( TraceEventType.Warning, synopsis, "Failed parsing thermal transient high limit \n{1}.", reading, new StackTrace().ParseCallStack( 4, 10, CallStackType.UserCallStack ) );
            return false;
        }

        if ( this.ReportVisaDeviceOperationOkay( synopsis, "getting thermal transient high limit using '{0}'", session.LastMessageSent ) )
        {
            this.ThermalTransientHighLimit = value;
            return true;
        }
        else
            return false;

    }

    /// <summary> Sets the voltage change measuring Thermal Transient. </summary>
    /// <param name="value"> The value. </param>
    /// <returns> null if it fails, else. </returns>
    public bool ThermalTransientHighLimitSetter( float value )
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;

        if ( Math.Abs( this.ThermalTransientHighLimit - value ) > 10f * float.Epsilon )
        {

            string synopsis = "Setting Thermal Transient High Limit";
            this.OnMessageAvailable( TraceEventType.Verbose, synopsis, "Instrument '{0}' setting Thermal Transient High Limit to {1}", this.ResourceName, value );

            string format = "_G.ttm.tr:highLimitSetter({0}) ";
            _ = session.WriteLine( format, value );
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

            if ( this.ReportVisaDeviceOperationOkay( synopsis, "setting Thermal Transient High Limit using '{0}'", session.LastMessageSent ) )
            {
                // set the default value.
                this.ThermalTransientHighLimit = value;
                return true;
            }
            else
                return false;
        }
        else
            return true;
    }

    #endregion

    #region " low limit "

    /// <summary> Gets the maximum allowed voltage change measuring Thermal Transient. </summary>
    /// <value> The thermal transient low limit. </value>
    public float ThermalTransientLowLimit
    {
        get => this.ThermalTransient.LowLimit;
        set => this.ThermalTransient.LowLimit = value;
    }

    /// <summary> Gets the voltage change for measuring Thermal Transient. </summary>
    public bool ThermalTransientLowLimitGetter()
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;

        const string printFormat = "%9.6f";

        string synopsis = "Getting Thermal Transient voltage change";
        this.OnMessageAvailable( TraceEventType.Verbose, synopsis, "Instrument '{0}' Getting thermal transient low limit", this.ResourceName );

        string reading = session.QueryPrintStringFormatTrimEnd( printFormat, "_G.ttm.tr.lowLimit" ) ?? string.Empty;

        if ( !float.TryParse( reading, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out float value ) )
        {
            this.OnMessageAvailable( TraceEventType.Warning, synopsis, "Failed parsing thermal transient low limit \n{1}.", reading, new StackTrace().ParseCallStack( 4, 10, CallStackType.UserCallStack ) );
            return false;
        }

        if ( this.ReportVisaDeviceOperationOkay( synopsis, "getting thermal transient low limit using '{0}'", session.LastMessageSent ) )
        {
            this.ThermalTransientLowLimit = value;
            return true;
        }
        else
            return false;

    }

    /// <summary> Sets the voltage change measuring Thermal Transient. </summary>
    /// <param name="value"> The value. </param>
    /// <returns> null if it fails, else. </returns>
    public bool ThermalTransientLowLimitSetter( float value )
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;

        if ( Math.Abs( this.ThermalTransientLowLimit - value ) > 10f * float.Epsilon )
        {

            string synopsis = "Setting Thermal Transient Low Limit";
            this.OnMessageAvailable( TraceEventType.Verbose, synopsis, "Instrument '{0}' setting Thermal Transient Low Limit to {1}", this.ResourceName, value );

            string format = "_G.ttm.tr:lowLimitSetter({0}) ";
            _ = session.WriteLine( format, value );
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

            if ( this.ReportVisaDeviceOperationOkay( synopsis, "setting Thermal Transient Low Limit using '{0}'", session.LastMessageSent ) )
            {
                // set the default value.
                this.ThermalTransientLowLimit = value;
                return true;
            }
            else
                return false;
        }
        else
            return true;
    }

    #endregion
}
