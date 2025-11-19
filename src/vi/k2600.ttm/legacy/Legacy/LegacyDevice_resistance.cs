// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using System.Diagnostics;
using cc.isr.Std.StackTraceExtensions;
using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy;

public partial class LegacyDevice : CommunityToolkit.Mvvm.ComponentModel.ObservableObject, IDevice
{
    #region " apparatus "

    /// <summary>   Gets or sets the cold resistance configuration parameters. </summary>
    /// <value> The cold resistance configuration. </value>
    public IColdResistanceConfig ColdResistanceConfig { get; set; }

    /// <summary> Gets or sets reference to the <see cref="IColdResistance">Final cold resistance</see> </summary>
    /// <value> The final resistance. </value>
    public IColdResistance FinalResistance { get; set; }

    /// <summary> Gets or sets reference to the <see cref="IColdResistance">initial cold
    /// resistance</see> </summary>
    /// <value> The initial resistance. </value>
    public IColdResistance InitialResistance { get; set; }

    #endregion

    #region " cold resistance setters: current level "

    /// <summary> Gets the current level for measuring cold resistance. The system sets a limit on the
    /// cold resistance current. </summary>
    /// <value> The cold resistance current level. </value>
    public float ColdResistanceCurrentLevel { get; private set; } = 0;

    /// <summary> Gets the cold resistance current level. </summary>
    public bool ColdResistanceCurrentLevelGetter()
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;

        const float tolerance = 0.00001f;
        const string printFormat = "%9.6f";

        string synopsis = "Getting Cold Resistance Current Levels";
        this.OnMessageAvailable( TraceEventType.Verbose, synopsis, "Instrument '{0}' getting initial cold resistance current level", this.ResourceName );

        string reading = session.QueryPrintStringFormatTrimEnd( printFormat, "_G.ttm.ir.level" ) ?? string.Empty;

        if ( this.ReportVisaDeviceOperationOkay( synopsis, "getting initial Resistance current level using '{0}'", session.LastMessageSent ) )
        {
            if ( !float.TryParse( reading, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out float initialValue ) )
            {
                this.OnMessageAvailable( TraceEventType.Warning, synopsis, "Failed parsing initial cold resistance current level {0}.\n{1}.", reading, new StackTrace().ParseCallStack( 4, 10, CallStackType.UserCallStack ) );
                return false;
            }

            reading = session.QueryPrintStringFormatTrimEnd( printFormat, "_G.ttm.fr.level" ) ?? string.Empty;
            if ( !float.TryParse( reading, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out float finalValue ) )
            {
                this.OnMessageAvailable( TraceEventType.Warning, synopsis, "Failed parsing final cold resistance current level \n{1}.", reading, new StackTrace().ParseCallStack( 4, 10, CallStackType.UserCallStack ) );
                return false;
            }

            if ( this.ReportVisaDeviceOperationOkay( synopsis, "getting Final Resistance current level using '{0}'", session.LastMessageSent ) )
            {
                // set the default value.
                if ( Math.Abs( finalValue - initialValue ) < tolerance )
                {
                    this.ColdResistanceCurrentLevel = finalValue;
                    return true;
                }
                else
                {
                    this.OnMessageAvailable( TraceEventType.Warning, synopsis, "failed getting cold resistance current because the two values for initial {0} and final {1} levels are not the same.{2}{3}.", initialValue, finalValue, Environment.NewLine, new StackTrace().ParseCallStack( 4, 10, CallStackType.UserCallStack ) );
                    return false;
                }
            }
            else
                return false;
        }
        else
            return false;
    }

    /// <summary>   Sets the cold resistance current level. </summary>
    /// <remarks>   2024-11-27. </remarks>
    /// <param name="value">    The value. </param>
    /// <param name="force">    (Optional) )true) True to force setting the actual value. </param>
    /// <returns>   null if it fails, else. </returns>
    public bool ColdResistanceCurrentLevelSetter( float value, bool force = true )
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;

        if ( force || Math.Abs( this.ColdResistanceCurrentLevel - value ) > 10f * float.Epsilon )
        {
            string synopsis = "Setting Cold Resistance Current Levels";
            this.OnMessageAvailable( TraceEventType.Verbose, synopsis, "Instrument '{0}' setting cold resistance current level to {1}", this.ResourceName, value );

            string format = "_G.ttm.ir:levelSetter({0}) ";
            _ = session.WriteLine( format, value );
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

            if ( this.ReportVisaDeviceOperationOkay( synopsis, "setting initial resistance current level using '{0}'", session.LastMessageSent ) )
            {


                format = "_G.ttm.fr:levelSetter({0}) ";
                _ = session.WriteLine( format, value );
                _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

                if ( this.ReportVisaDeviceOperationOkay( synopsis, "setting final resistance current level using '{0}'", session.LastMessageSent ) )
                {

                    // set the default value.
                    this.ColdResistanceCurrentLevel = value;
                    return true;
                }
                else
                    return false;

            }
            else
                return false;
        }

        else
            return true;

    }

    #endregion

    #region " cold resistance setters: voltage limit "

    /// <summary> Gets the Voltage Limit for measuring cold resistance. </summary>
    /// <value> The cold resistance voltage limit. </value>
    public float ColdResistanceVoltageLimit { get; private set; }

    /// <summary> Gets the cold resistance voltage limit. </summary>
    public bool ColdResistanceVoltageLimitGetter()
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;

        const float tolerance = 0.0001f;
        const string printFormat = "%9.6f";

        string synopsis = "Getting Cold Resistance voltage limits";
        this.OnMessageAvailable( TraceEventType.Verbose, synopsis, "Instrument '{0}' getting initial cold resistance voltage limit", this.ResourceName );

        string reading = session.QueryPrintStringFormatTrimEnd( printFormat, "_G.ttm.ir.limit" ) ?? string.Empty;

        if ( this.ReportVisaDeviceOperationOkay( synopsis, "getting initial Resistance voltage limit using '{0}'", session.LastMessageSent ) )
        {

            if ( !float.TryParse( reading, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out float initialValue ) )
            {
                this.OnMessageAvailable( TraceEventType.Warning, synopsis, "Failed parsing initial cold resistance voltage limit \n{1}.", reading, new StackTrace().ParseCallStack( 4, 10, CallStackType.UserCallStack ) );
                return false;
            }

            reading = session.QueryPrintStringFormatTrimEnd( printFormat, "_G.ttm.fr.limit" ) ?? string.Empty;
            if ( !float.TryParse( reading, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out float finalValue ) )
            {
                this.OnMessageAvailable( TraceEventType.Warning, synopsis, "Failed parsing final cold resistance voltage limit \n{1}.", reading, new StackTrace().ParseCallStack( 4, 10, CallStackType.UserCallStack ) );
                return false;
            }

            if ( this.ReportVisaDeviceOperationOkay( synopsis, "getting Final Resistance voltage limit using '{0}'", session.LastMessageSent ) )
            {

                // set the default value.
                if ( Math.Abs( finalValue - initialValue ) < tolerance )
                {
                    this.ColdResistanceVoltageLimit = finalValue;
                    return true;
                }
                else
                {
                    this.OnMessageAvailable( TraceEventType.Warning, synopsis, "failed getting cold resistance voltage because the two values for initial {0} and final {1} limits are not the same.{2}{3}.", initialValue, finalValue, Environment.NewLine, new StackTrace().ParseCallStack( 4, 10, CallStackType.UserCallStack ) );
                    return false;
                }

            }
            else
                return false;
        }
        else
            return false;

    }

    /// <summary>   Sets the cold resistance voltage limit. </summary>
    /// <remarks>   2024-11-27. </remarks>
    /// <param name="value">    The value. </param>
    /// <param name="force">    (Optional) )true) True to force setting the actual value. </param>
    /// <returns>   null if it fails, else. </returns>
    public bool ColdResistanceVoltageLimitSetter( float value, bool force = true )
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;

        if ( force || Math.Abs( this.ColdResistanceVoltageLimit - value ) > 10f * float.Epsilon )
        {

            string synopsis = "Setting Cold Resistance Voltage Limits";
            this.OnMessageAvailable( TraceEventType.Verbose, synopsis, "Instrument '{0}' setting cold resistance Voltage Limit to {1}", this.ResourceName, value );

            string format = "_G.ttm.ir:limitSetter({0}) ";
            _ = session.WriteLine( format, value );
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

            if ( this.ReportVisaDeviceOperationOkay( synopsis, "setting initial Resistance Voltage Limit using '{0}'", session.LastMessageSent ) )
            {

                format = "_G.ttm.fr:limitSetter({0}) ";
                _ = session.WriteLine( format, value );
                _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

                if ( this.ReportVisaDeviceOperationOkay( synopsis, "setting Final Resistance Voltage Limit using '{0}'", session.LastMessageSent ) )
                {
                    // set the default value.
                    this.ColdResistanceVoltageLimit = value;
                    return true;
                }
                else
                    return false;

            }
            else
                return false;
        }
        else
            return true;
    }

    #endregion

    #region " high limit "

    /// <summary> The cold resistance high limit. </summary>

    /// <summary> Gets or sets the High Limit for measuring cold resistance. The system sets a limit on the
    /// cold resistance current. </summary>
    /// <value> The cold resistance high limit. </value>
    public float ColdResistanceHighLimit
    {
        get => this.InitialResistance.HighLimit;
        set
        {
            this.InitialResistance.HighLimit = value;
            this.FinalResistance.HighLimit = value;
        }
    }

    /// <summary> Gets the cold resistance High Limit. </summary>
    public bool ColdResistanceHighLimitGetter()
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;

        const float tolerance = 0.0001f;
        const string printFormat = "%9.6f";

        string synopsis = "Getting Cold Resistance High Limits";
        this.OnMessageAvailable( TraceEventType.Verbose, synopsis, "Instrument '{0}' getting initial cold resistance High Limit", this.ResourceName );

        string reading = session.QueryPrintStringFormatTrimEnd( printFormat, "_G.ttm.ir.highLimit" ) ?? string.Empty;

        if ( this.ReportVisaDeviceOperationOkay( synopsis, "getting initial Resistance High Limit using '{0}'", session.LastMessageSent ) )
        {
            if ( !float.TryParse( reading, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out float initialValue ) )
            {
                this.OnMessageAvailable( TraceEventType.Warning, synopsis, "Failed parsing initial cold resistance High Limit \n{1}.", reading, new StackTrace().ParseCallStack( 4, 10, CallStackType.UserCallStack ) );
                return false;
            }

            reading = session.QueryPrintStringFormatTrimEnd( printFormat, "_G.ttm.fr.highLimit" ) ?? string.Empty;
            if ( !float.TryParse( reading, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out float finalValue ) )
            {
                this.OnMessageAvailable( TraceEventType.Warning, synopsis, "Failed parsing final cold resistance High Limit \n{1}.", reading, new StackTrace().ParseCallStack( 4, 10, CallStackType.UserCallStack ) );
                return false;
            }

            if ( this.ReportVisaDeviceOperationOkay( synopsis, "getting Final Resistance High Limit using '{0}'", session.LastMessageSent ) )
            {

                // set the default value.
                if ( Math.Abs( finalValue - initialValue ) < tolerance )
                {
                    this.ColdResistanceHighLimit = finalValue;
                    return true;
                }
                else
                {
                    this.OnMessageAvailable( TraceEventType.Warning, synopsis, "failed getting cold resistance current because the two values for initial {0} and final {1} levels are not the same.{2}{3}.", initialValue, finalValue, Environment.NewLine, new StackTrace().ParseCallStack( 4, 10, CallStackType.UserCallStack ) );
                    return false;
                }

            }
            else
                return false;
        }
        else
            return false;

    }

    /// <summary> Sets the cold resistance High Limit. </summary>
    /// <param name="value"> The value. </param>
    /// <returns> null if it fails, else. </returns>
    public bool ColdResistanceHighLimitSetter( float value )
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;

        if ( Math.Abs( this.ColdResistanceHighLimit - value ) > 10f * float.Epsilon )
        {

            string synopsis = "Setting Cold Resistance High Limits";
            this.OnMessageAvailable( TraceEventType.Verbose, synopsis, "Instrument '{0}' setting cold resistance High Limit to {1}", this.ResourceName, value );

            string format = "_G.ttm.ir:highLimitSetter({0}) ";
            _ = session.WriteLine( format, value );
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

            if ( this.ReportVisaDeviceOperationOkay( synopsis, "setting initial Resistance High Limit using '{0}'", session.LastMessageSent ) )
            {

                format = "_G.ttm.fr:highLimitSetter({0}) ";
                _ = session.WriteLine( format, value );
                _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

                if ( this.ReportVisaDeviceOperationOkay( synopsis, "setting Final Resistance High Limit using '{0}'", session.LastMessageSent ) )
                {
                    // set the default value.
                    this.ColdResistanceHighLimit = value;
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
        else
            return true;
    }

    #endregion

    #region " low limit "

    /// <summary> Gets the Low Limit for measuring cold resistance. The system sets a limit on the cold
    /// resistance current. </summary>
    /// <value> The cold resistance low limit. </value>
    public float ColdResistanceLowLimit
    {
        get => this.InitialResistance.LowLimit;
        set
        {
            this.InitialResistance.LowLimit = value;
            this.FinalResistance.LowLimit = value;
        }
    }

    /// <summary> Gets the cold resistance Low Limit. </summary>
    public bool ColdResistanceLowLimitGetter()
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;

        const float tolerance = 0.0001f;
        const string printFormat = "%9.6f";

        string synopsis = "Getting Cold Resistance Low Limits";
        this.OnMessageAvailable( TraceEventType.Verbose, synopsis, "Instrument '{0}' getting initial cold resistance Low Limit", this.ResourceName );

        string reading = session.QueryPrintStringFormatTrimEnd( printFormat, "_G.ttm.ir.lowLimit" ) ?? string.Empty;

        if ( this.ReportVisaDeviceOperationOkay( synopsis, "getting initial Resistance Low Limit using '{0}'", session.LastMessageSent ) )
        {
            if ( !float.TryParse( reading, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out float initialValue ) )
            {
                this.OnMessageAvailable( TraceEventType.Warning, synopsis, "Failed parsing initial cold resistance Low Limit \n{1}.", reading, new StackTrace().ParseCallStack( 4, 10, CallStackType.UserCallStack ) );
                return false;
            }

            reading = session.QueryPrintStringFormatTrimEnd( printFormat, "_G.ttm.fr.lowLimit" ) ?? string.Empty;
            if ( !float.TryParse( reading, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out float finalValue ) )
            {
                this.OnMessageAvailable( TraceEventType.Warning, synopsis, "Failed parsing final cold resistance Low Limit \n{1}.", reading, new StackTrace().ParseCallStack( 4, 10, CallStackType.UserCallStack ) );
                return false;
            }

            if ( this.ReportVisaDeviceOperationOkay( synopsis, "getting Final Resistance Low Limit using '{0}'", session.LastMessageSent ) )
            {

                // set the default value.
                if ( Math.Abs( finalValue - initialValue ) < tolerance )
                {
                    this.ColdResistanceLowLimit = finalValue;
                    return true;
                }
                else
                {
                    this.OnMessageAvailable( TraceEventType.Warning, synopsis, "failed getting cold resistance current because the two values for initial {0} and final {1} levels are not the same.{2}{3}.", initialValue, finalValue, Environment.NewLine, new StackTrace().ParseCallStack( 4, 10, CallStackType.UserCallStack ) );
                    return false;
                }
            }
            else
                return false;
        }
        else
            return false;
    }

    /// <summary> Sets the cold resistance Low Limit. </summary>
    /// <param name="value"> The value. </param>
    /// <returns> null if it fails, else. </returns>
    public bool ColdResistanceLowLimitSetter( float value )
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;

        if ( Math.Abs( this.ColdResistanceLowLimit - value ) > 10f * float.Epsilon )
        {
            string synopsis = "Setting Cold Resistance Low Limits";
            this.OnMessageAvailable( TraceEventType.Verbose, synopsis, "Instrument '{0}' setting cold resistance Low Limit to {1}", this.ResourceName, value );

            string format = "_G.ttm.ir:lowLimitSetter({0}) ";
            _ = session.WriteLine( format, value );
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

            if ( this.ReportVisaDeviceOperationOkay( synopsis, "setting initial Resistance Low Limit using '{0}'", session.LastMessageSent ) )
            {
                format = "_G.ttm.fr:lowLimitSetter({0}) ";
                _ = session.WriteLine( format, value );
                _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

                if ( this.ReportVisaDeviceOperationOkay( synopsis, "setting Final Resistance Low Limit using '{0}'", session.LastMessageSent ) )
                {
                    // set the default value.
                    this.ColdResistanceLowLimit = value;
                    return true;
                }
                else
                    return false;

            }
            else
                return false;
        }
        else
            return true;
    }

    #endregion
}
