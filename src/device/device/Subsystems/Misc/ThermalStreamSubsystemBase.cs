// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI;
/// <summary>
/// Defines the contract that must be implemented by a Thermal Stream Subsystem.
/// </summary>
/// <remarks>
/// (c) 2015 Integrated Scientific ReSenses, Inc.<para>
/// Licensed under The MIT License. </para><para>
/// David, 2015-03-17, 3.0.5554. </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="ThermalStreamSubsystemBase" /> class.
/// </remarks>
/// <param name="statusSubsystem"> The status subsystem. </param>
[CLSCompliant( false )]
public abstract class ThermalStreamSubsystemBase( StatusSubsystemBase statusSubsystem ) : SubsystemBase( statusSubsystem )
{
    #region " i presettable "

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    public override void DefineKnownResetState()
    {
        base.DefineKnownResetState();
        this.HeadDown = new bool?();
        this.MaximumTestTime = new double?();
        this.RampRate = new double?();
        this.Setpoint = new double?();
        this.SetpointNumber = new int?();
        this.SetpointWindow = new double?();
        this.SoakTime = new double?();
        this.Temperature = new double?();
        this.TemperatureEventStatus = new int?();
        this.RampRateUnit = cc.isr.UnitsAmounts.StandardUnits.TemperatureUnits.DegreesCelsiusPerMinute;
    }

    #endregion

    #region " cycle count "

    /// <summary> The cycle count range. </summary>

    /// <summary> The Range of the CycleCount. </summary>
    /// <value> The cycle count range. </value>
    public Std.Primitives.RangeI CycleCountRange
    {
        get;
        set
        {
            if ( this.CycleCountRange != value )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    } = Std.Primitives.RangeI.FullNonnegative;

    /// <summary> Gets or sets the cached CycleCount. </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public int? CycleCount
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.CycleCount, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Cycle Count. </summary>
    /// <param name="value"> the Cycle Count. </param>
    /// <returns> the Cycle Count. </returns>
    public int? ApplyCycleCount( int value )
    {
        _ = this.WriteCycleCount( value );
        return this.QueryCycleCount();
    }

    /// <summary> Gets or sets the Cycle Count query command. </summary>
    /// <value> the Cycle Count query command. </value>
    protected virtual string CycleCountQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Cycle Count. </summary>
    /// <returns> the Cycle Count or none if unknown. </returns>
    public int? QueryCycleCount()
    {
        this.CycleCount = this.Session.Query( this.CycleCount, this.CycleCountQueryCommand );
        return this.CycleCount;
    }

    /// <summary> Gets or sets the Cycle Count command format. </summary>
    /// <value> the Cycle Count command format. </value>
    protected virtual string CycleCountCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Cycle Count without reading back the value from the device. </summary>
    /// <remarks> This command sets the Cycle Count. </remarks>
    /// <param name="value"> the Cycle Count. </param>
    /// <returns> the Cycle Count. </returns>
    public int? WriteCycleCount( int value )
    {
        this.CycleCount = this.Session.WriteLine( value, this.CycleCountCommandFormat );
        return this.CycleCount;
    }

    #endregion

    #region " cycle number "

    /// <summary> the Cycle Number. </summary>

    /// <summary> Gets or sets the cached CycleNumber. </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public int? CycleNumber
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.CycleNumber, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the Cycle Number query command. </summary>
    /// <value> the Cycle Number query command. </value>
    protected virtual string CycleNumberQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Cycle Number. </summary>
    /// <returns> the Cycle Number or none if unknown. </returns>
    public int? QueryCycleNumber()
    {
        this.CycleNumber = this.Session.Query( this.CycleNumber, this.CycleNumberQueryCommand );
        return this.CycleNumber;
    }

    #endregion

    #region " cycling: start / stop "

    /// <summary> Gets or sets the command execution refractory time span. </summary>
    /// <value> The command execution refractory time span. </value>
    public abstract TimeSpan CommandRefractoryTimeSpan { get; set; }

    /// <summary> Gets or sets the start cycling command. </summary>
    /// <value> The start cycling command. </value>
    protected virtual string StartCyclingCommand { get; set; } = string.Empty;

    /// <summary> Starts a cycling. </summary>
    public void StartCycling()
    {
        if ( !string.IsNullOrWhiteSpace( this.StartCyclingCommand ) )
            _ = this.Session.WriteLine( this.StartCyclingCommand );
    }

    /// <summary> Gets or sets the stop cycling command. </summary>
    /// <value> The stop cycling command. </value>
    protected virtual string StopCyclingCommand { get; set; } = string.Empty;

    /// <summary> Stops a cycling. </summary>
    public void StopCycling()
    {
        if ( !string.IsNullOrWhiteSpace( this.StopCyclingCommand ) )
            _ = this.Session.WriteLine( this.StopCyclingCommand );
    }

    #endregion

    #region " device error "

    /// <summary> The DeviceError. </summary>

    /// <summary> Gets or sets the cached DeviceError. </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public string? DeviceError
    {
        get;

        protected set
        {
            if ( !Equals( this.DeviceError, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the DeviceError query command. </summary>
    /// <value> The DeviceError query command. </value>
    protected virtual string DeviceErrorQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the DeviceError. </summary>
    /// <returns> The DeviceError or none if unknown. </returns>
    public string? QueryDeviceError()
    {
        this.DeviceError = this.Session.QueryTrimEnd( this.DeviceError, this.DeviceErrorQueryCommand );
        return this.DeviceError;
    }

    #endregion

    #region " device control "

    /// <summary> Device Control. </summary>

    /// <summary> Gets or sets the cached Device Control sentinel. </summary>
    /// <value>
    /// <c>null</c> if Device Control is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? DeviceControl
    {
        get;

        protected set
        {
            if ( !Equals( this.DeviceControl, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Device Control sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyDeviceControl( bool value )
    {
        _ = this.WriteDeviceControl( value );
        return this.QueryDeviceControl();
    }

    /// <summary> Gets or sets the automatic Range enabled query command. </summary>
    /// <remarks> SCPI: "?". </remarks>
    /// <value> The automatic Range enabled query command. </value>
    protected virtual string DeviceControlQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Device Control sentinel. Also sets the
    /// <see cref="DeviceControl">Device Control</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryDeviceControl()
    {
        this.DeviceControl = this.Session.Query( this.DeviceControl, this.DeviceControlQueryCommand );
        return this.DeviceControl;
    }

    /// <summary> Gets or sets the automatic Range enabled command Format. </summary>
    /// <remarks> SCPI: "". </remarks>
    /// <value> The automatic Range enabled query command. </value>
    protected virtual string DeviceControlCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Device Control sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteDeviceControl( bool value )
    {
        this.DeviceControl = this.Session.WriteLine( value, this.DeviceControlCommandFormat );
        return this.DeviceControl;
    }

    #endregion

    #region " device sensor type "

    /// <summary> the Device Sensor Type. </summary>

    /// <summary> Gets or sets the cached DeviceSensorType. </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public DeviceSensorType? DeviceSensorType
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.DeviceSensorType, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Device Sensor Type. </summary>
    /// <param name="value"> the Device Sensor Type. </param>
    /// <returns> the Device Sensor Type. </returns>
    public DeviceSensorType? ApplyDeviceSensorType( DeviceSensorType value )
    {
        _ = this.WriteDeviceSensorType( value );
        return this.QueryDeviceSensorType();
    }

    /// <summary> Gets or sets the Device Sensor Type query command. </summary>
    /// <value> the Device Sensor Type query command. </value>
    protected virtual string DeviceSensorTypeQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Device Sensor Type. </summary>
    /// <returns> the Device Sensor Type or none if unknown. </returns>
    public DeviceSensorType? QueryDeviceSensorType()
    {
        this.DeviceSensorType = this.Session.QueryEnum( this.DeviceSensorType, this.DeviceSensorTypeQueryCommand );
        return this.DeviceSensorType;
    }

    /// <summary> Gets or sets the Device Sensor Type command format. </summary>
    /// <value> the Device Sensor Type command format. </value>
    protected virtual string DeviceSensorTypeCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Device Sensor Type without reading back the value from the device.
    /// </summary>
    /// <remarks> This command sets the Device Sensor Type. </remarks>
    /// <param name="value"> the Device Sensor Type. </param>
    /// <returns> the Device Sensor Type. </returns>
    public DeviceSensorType? WriteDeviceSensorType( DeviceSensorType value )
    {
        this.DeviceSensorType = this.Session.WriteEnumValue( value, this.DeviceSensorTypeCommandFormat );
        return this.DeviceSensorType;
    }

    #endregion

    #region " device thermal constant "

    /// <summary> The device thermal constant range. </summary>

    /// <summary> The Device Thermal Constant range. </summary>
    /// <value> The device thermal constant range. </value>
    public Std.Primitives.RangeI DeviceThermalConstantRange
    {
        get;
        set
        {
            if ( this.DeviceThermalConstantRange != value )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    } = Std.Primitives.RangeI.FullNonnegative;

    /// <summary> Gets or sets the cached DeviceThermalConstant. </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public int? DeviceThermalConstant
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.DeviceThermalConstant, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Device Thermal Constant. </summary>
    /// <param name="value"> the Device Thermal Constant. </param>
    /// <returns> the Device Thermal Constant. </returns>
    public int? ApplyDeviceThermalConstant( int value )
    {
        _ = this.WriteDeviceThermalConstant( value );
        return this.QueryDeviceThermalConstant();
    }

    /// <summary> Gets or sets the Device Thermal Constant query command. </summary>
    /// <value> the Device Thermal Constant query command. </value>
    protected virtual string DeviceThermalConstantQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Device Thermal Constant. </summary>
    /// <returns> the Device Thermal Constant or none if unknown. </returns>
    public int? QueryDeviceThermalConstant()
    {
        this.DeviceThermalConstant = this.Session.Query( this.DeviceThermalConstant, this.DeviceThermalConstantQueryCommand );
        return this.DeviceThermalConstant;
    }

    /// <summary> Gets or sets the Device Thermal Constant command format. </summary>
    /// <value> the Device Thermal Constant command format. </value>
    protected virtual string DeviceThermalConstantCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Device Thermal Constant without reading back the value from the device.
    /// </summary>
    /// <remarks> This command sets the Device Thermal Constant. </remarks>
    /// <param name="value"> the Device Thermal Constant. </param>
    /// <returns> the Device Thermal Constant. </returns>
    public int? WriteDeviceThermalConstant( int value )
    {
        this.DeviceThermalConstant = this.Session.WriteLine( value, this.DeviceThermalConstantCommandFormat );
        return this.DeviceThermalConstant;
    }

    #endregion

    #region " head down "

    /// <summary> Head Down. </summary>

    /// <summary> Gets or sets the cached Head Down sentinel. </summary>
    /// <value>
    /// <c>null</c> if Head Down is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? HeadDown
    {
        get;

        protected set
        {
            if ( !Equals( this.HeadDown, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Head Down sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyHeadDown( bool value )
    {
        _ = this.WriteHeadDown( value );
        return this.QueryHeadDown();
    }

    /// <summary> Gets or sets the automatic Range enabled query command. </summary>
    /// <remarks> SCPI: "HEAD?". </remarks>
    /// <value> The automatic Range enabled query command. </value>
    protected virtual string HeadDownQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Head Down sentinel. Also sets the
    /// <see cref="HeadDown">Head down</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryHeadDown()
    {
        this.HeadDown = this.Session.Query( this.HeadDown, this.HeadDownQueryCommand );
        return this.HeadDown;
    }

    /// <summary> Gets or sets the automatic Range enabled command Format. </summary>
    /// <remarks> SCPI: "HEAD {0:'1';'1';'0'}". </remarks>
    /// <value> The automatic Range enabled query command. </value>
    protected virtual string HeadDownCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Head Down sentinel. Does not read back from the instrument. </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteHeadDown( bool value )
    {
        this.HeadDown = this.Session.WriteLine( value, this.HeadDownCommandFormat );
        return this.HeadDown;
    }

    #endregion

    #region " operator / cycle mode "

    /// <summary> Gets or sets the Reset Operator Screen command. </summary>
    /// <value> The Reset Operator Screen command. </value>
    protected virtual string ResetOperatorScreenCommand { get; set; } = string.Empty;

    /// <summary> Gets or sets the refractory time span for resetting to Operator Mode. </summary>
    /// <value> The command execution refractory time span. </value>
    public abstract TimeSpan ResetOperatorScreenRefractoryTimeSpan { get; set; }

    /// <summary> Go to Reset Operator Screen. </summary>
    public void ResetOperatorScreen()
    {
        if ( !string.IsNullOrWhiteSpace( this.ResetOperatorScreenCommand ) )
            _ = this.Session.WriteLine( this.ResetOperatorScreenCommand );
    }

    /// <summary> Gets or sets the Reset Cycle Screen command. </summary>
    /// <value> The Reset Cycle Screen command. </value>
    protected virtual string ResetCycleScreenCommand { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the refractory time span for resetting to Cycle (manual) mode.
    /// </summary>
    /// <value> The command execution refractory time span. </value>
    public abstract TimeSpan ResetCycleScreenRefractoryTimeSpan { get; set; }

    /// <summary> Go to Reset Cycle Screen. </summary>
    public void ResetCycleScreen()
    {
        if ( !string.IsNullOrWhiteSpace( this.ResetCycleScreenCommand ) )
            _ = this.Session.WriteLine( this.ResetCycleScreenCommand );
    }

    #endregion

    #region " ramp rate "

    /// <summary> Gets or sets the ramp rate unit. </summary>
    /// <value> The ramp rate unit. </value>
    public cc.isr.UnitsAmounts.Unit RampRateUnit { get; set; } = cc.isr.UnitsAmounts.StandardUnits.TemperatureUnits.DegreesCelsiusPerMinute;

    /// <summary> Gets or sets the cached Ramp Rate. </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? RampRate
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.RampRate, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Ramp Rate. </summary>
    /// <param name="value"> the Ramp Rate. </param>
    /// <returns> the Ramp Rate. </returns>
    public double? ApplyRampRate( double value )
    {
        _ = this.WriteRampRate( value );
        return this.QueryRampRate();
    }

    /// <summary> Gets or sets the Ramp Rate query command. </summary>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <value> the Ramp Rate query command. </value>
    protected virtual string RampRateQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Ramp Rate. </summary>
    /// <remarks>
    /// Thermal stream ramp rate query of rates lower than 100 gives incorrect values. ramp rate,
    /// Ramp?<para>
    /// 700,  700\r\n</para><para>
    /// 1.1,  0.1\r\n</para><para>
    /// 99.9,  9.9\r\n</para><para>
    /// 53.5,  5.3\r\n</para><para>
    /// 5.3, 0.5\r\n</para><para>
    /// 1.0, 0.1\r\n</para><para>
    /// Sending RAMP 1.1, gives 1.1 ramp on the Thermal stream, reading using Ramp?\n returns 0.1\r\
    /// n</para>
    /// </remarks>
    /// <returns> the Ramp Rate or none if unknown. </returns>
    public double? QueryRampRate()
    {
        double? value = this.Session.Query( this.RampRate, this.RampRateQueryCommand );
        if ( !this.RampRate.HasValue )
        {
            // if this is the first reading, set value even if low range.
            this.RampRate = value;
        }
        else if ( !value.HasValue )
        {
            // if failed to read, set value to indicated failure to read.
            this.RampRate = value;
        }
        else if ( this.HighRampRateRange.Contains( value.Value ) )
        {
            // if high ramp rate, than value is correct.
            this.RampRate = value;
        }
        else
        {
            // if read low ramp rate, value is x10 too low. Leave value as written -- open loop.
        }

        return this.RampRate;
    }

    /// <summary> Writes the Ramp Rate without reading back the value from the device. </summary>
    /// <remarks> This command sets the Ramp Rate. </remarks>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <param name="value"> the Ramp Rate. </param>
    /// <returns> the Ramp Rate. </returns>
    public double? WriteRampRate( double value )
    {
        this.RampRate = this.LowRampRateRange.Contains( value )
            ? this.Session.WriteLine( value, this.LowRampRateCommandFormat )
            : this.HighRampRateRange.Contains( value )
                ? this.Session.WriteLine( value, this.HighRampRateCommandFormat )
                : throw new InvalidOperationException( $"Ramp range {value} is outside both the low {this.LowRampRateRange} and high {this.HighRampRateRange} ranges" );

        return this.RampRate;
    }

    #region " low ramp rate "

    /// <summary> The low ramp rate range. </summary>

    /// <summary> The Low Ramp Rate range in degrees per minute. </summary>
    /// <value> The low ramp rate range. </value>
    public Std.Primitives.RangeR LowRampRateRange
    {
        get;
        set
        {
            if ( this.LowRampRateRange != value )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    } = Std.Primitives.RangeR.FullNonnegative;

    /// <summary> Gets or sets the Low Ramp Rate command format. </summary>
    /// <value> the Low Ramp Rate command format. </value>
    protected virtual string LowRampRateCommandFormat { get; set; } = string.Empty;

    #endregion

    #region " high ramp rate "

    /// <summary> The high ramp rate range. </summary>

    /// <summary> The High Ramp Rate range in degrees per minute. </summary>
    /// <value> The high ramp rate range. </value>
    public Std.Primitives.RangeR HighRampRateRange
    {
        get;
        set
        {
            if ( this.HighRampRateRange != value )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    } = Std.Primitives.RangeR.FullNonnegative;

    /// <summary> Gets or sets the High Ramp Rate command format. </summary>
    /// <value> the High Ramp Rate command format. </value>
    protected virtual string HighRampRateCommandFormat { get; set; } = string.Empty;

    #endregion

    #endregion

    #region " set point "

    /// <summary> The setpoint range. </summary>

    /// <summary> The Setpoint range. </summary>
    /// <value> The setpoint range. </value>
    public Std.Primitives.RangeR SetpointRange
    {
        get;
        set
        {
            if ( this.SetpointRange != value )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    } = Std.Primitives.RangeR.Full;

    /// <summary> Gets or sets the cached sense Setpoint. </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? Setpoint
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.Setpoint, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the sense Setpoint. </summary>
    /// <param name="value"> The Set Point. </param>
    /// <returns> The Set Point. </returns>
    public double? ApplySetpoint( double value )
    {
        _ = this.WriteSetpoint( value );
        return this.QuerySetpoint();
    }

    /// <summary> Gets or sets The Set Point query command. </summary>
    /// <value> The Set Point query command. </value>
    protected virtual string SetpointQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries The Set Point. </summary>
    /// <returns> The Set Point or none if unknown. </returns>
    public double? QuerySetpoint()
    {
        this.Setpoint = this.Session.Query( this.Setpoint, this.SetpointQueryCommand );
        return this.Setpoint;
    }

    /// <summary> Gets or sets The Set Point command format. </summary>
    /// <value> The Set Point command format. </value>
    protected virtual string SetpointCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes The Set Point without reading back the value from the device. </summary>
    /// <remarks> This command sets The Set Point. </remarks>
    /// <param name="value"> The Set Point. </param>
    /// <returns> The Set Point. </returns>
    public double? WriteSetpoint( double value )
    {
        this.Setpoint = this.Session.WriteLine( value, this.SetpointCommandFormat );
        return this.Setpoint;
    }

    #endregion

    #region " set point number "

    /// <summary> The setpoint number range. </summary>

    /// <summary> The Setpoint Number range. </summary>
    /// <value> The setpoint number range. </value>
    public Std.Primitives.RangeI SetpointNumberRange
    {
        get;
        set
        {
            if ( this.SetpointNumberRange != value )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    } = Std.Primitives.RangeI.FullNonnegative;

    /// <summary> Gets or sets the cached SetpointNumber. </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public int? SetpointNumber
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.SetpointNumber, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Set Point Number. </summary>
    /// <param name="value"> the Set Point Number. </param>
    /// <returns> the Set Point Number. </returns>
    public int? ApplySetpointNumber( int value )
    {
        _ = this.WriteSetpointNumber( value );
        return this.QuerySetpointNumber();
    }

    /// <summary> Gets or sets the Set Point Number query command. </summary>
    /// <value> the Set Point Number query command. </value>
    protected virtual string SetpointNumberQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Set Point Number. </summary>
    /// <returns> the Set Point Number or none if unknown. </returns>
    public int? QuerySetpointNumber()
    {
        this.SetpointNumber = this.Session.Query( this.SetpointNumber, this.SetpointNumberQueryCommand );
        return this.SetpointNumber;
    }

    /// <summary> Gets or sets the Set Point Number command format. </summary>
    /// <value> the Set Point Number command format. </value>
    protected virtual string SetpointNumberCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Set Point Number without reading back the value from the device.
    /// </summary>
    /// <remarks> This command sets the Set Point Number. </remarks>
    /// <param name="value"> the Set Point Number. </param>
    /// <returns> the Set Point Number. </returns>
    public int? WriteSetpointNumber( int value )
    {
        this.SetpointNumber = this.Session.WriteLine( value, this.SetpointNumberCommandFormat );
        return this.SetpointNumber;
    }

    #endregion

    #region " set point: next "

    /// <summary> Gets or sets the next setpoint refractory time span. </summary>
    /// <value> The next setpoint refractory time span. </value>
    public abstract TimeSpan NextSetpointRefractoryTimeSpan { get; set; }

    /// <summary> Gets or sets the Next Set Point command. </summary>
    /// <value> The Next Set Point command. </value>
    protected virtual string NextSetpointCommand { get; set; } = string.Empty;

    /// <summary> Advances to the next setpoint. </summary>
    public void NextSetpoint()
    {
        if ( !string.IsNullOrWhiteSpace( this.NextSetpointCommand ) )
            _ = this.Session.WriteLine( this.NextSetpointCommand );
    }

    #endregion

    #region " set point window "

    /// <summary> The setpoint window range. </summary>

    /// <summary> The Setpoint Window range in degrees centigrade. </summary>
    /// <value> The setpoint window range. </value>
    public Std.Primitives.RangeR SetpointWindowRange
    {
        get;
        set
        {
            if ( this.SetpointWindowRange != value )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    } = Std.Primitives.RangeR.FullNonnegative;

    /// <summary> Gets or sets the cached SetpointWindow. </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? SetpointWindow
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.SetpointWindow, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Set Point Window. </summary>
    /// <param name="value"> the Set Point Window. </param>
    /// <returns> the Set Point Window. </returns>
    public double? ApplySetpointWindow( double value )
    {
        _ = this.WriteSetpointWindow( value );
        return this.QuerySetpointWindow();
    }

    /// <summary> Gets or sets the Set Point Window query command. </summary>
    /// <value> the Set Point Window query command. </value>
    protected virtual string SetpointWindowQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Set Point Window. </summary>
    /// <returns> the Set Point Window or none if unknown. </returns>
    public double? QuerySetpointWindow()
    {
        this.SetpointWindow = this.Session.Query( this.SetpointWindow, this.SetpointWindowQueryCommand );
        return this.SetpointWindow;
    }

    /// <summary> Gets or sets the Set Point Window command format. </summary>
    /// <value> the Set Point Window command format. </value>
    protected virtual string SetpointWindowCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Set Point Window without reading back the value from the device.
    /// </summary>
    /// <remarks> This command sets the Set Point Window. </remarks>
    /// <param name="value"> the Set Point Window. </param>
    /// <returns> the Set Point Window. </returns>
    public double? WriteSetpointWindow( double value )
    {
        this.SetpointWindow = this.Session.WriteLine( value, this.SetpointWindowCommandFormat );
        return this.SetpointWindow;
    }

    #endregion

    #region " soak time "

    /// <summary> The soak time range. </summary>

    /// <summary> The Soak Time range in seconds. </summary>
    /// <value> The soak time range. </value>
    public Std.Primitives.RangeI SoakTimeRange
    {
        get;
        set
        {
            if ( this.SoakTimeRange != value )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    } = Std.Primitives.RangeI.FullNonnegative;

    /// <summary> Gets or sets the cached Soak Time. </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? SoakTime
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.SoakTime, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Soak Time. </summary>
    /// <param name="value"> the Soak Time. </param>
    /// <returns> the Soak Time. </returns>
    public double? ApplySoakTime( double value )
    {
        _ = this.WriteSoakTime( value );
        return this.QuerySoakTime();
    }

    /// <summary> Gets or sets the Soak Time query command. </summary>
    /// <value> the Soak Time query command. </value>
    protected virtual string SoakTimeQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Soak Time. </summary>
    /// <returns> the Soak Time or none if unknown. </returns>
    public double? QuerySoakTime()
    {
        this.SoakTime = this.Session.Query( this.SoakTime, this.SoakTimeQueryCommand );
        return this.SoakTime;
    }

    /// <summary> Gets or sets the Soak Time command format. </summary>
    /// <value> the Soak Time command format. </value>
    protected virtual string SoakTimeCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Soak Time without reading back the value from the device. </summary>
    /// <remarks> This command sets the Soak Time. </remarks>
    /// <param name="value"> the Soak Time. </param>
    /// <returns> the Soak Time. </returns>
    public double? WriteSoakTime( double value )
    {
        this.SoakTime = this.Session.WriteLine( value, this.SoakTimeCommandFormat );
        return this.SoakTime;
    }

    #endregion

    #region " temperature "

    /// <summary> The Temperature. </summary>

    /// <summary> Gets or sets the cached Temperature. </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? Temperature
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.Temperature, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the Temperature query command. </summary>
    /// <value> The Temperature query command. </value>
    protected virtual string TemperatureQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Temperature. </summary>
    /// <returns> The Temperature or none if unknown. </returns>
    public double? QueryTemperature()
    {
        this.Temperature = this.Session.Query( this.Temperature, this.TemperatureQueryCommand );
        return this.Temperature;
    }

    #endregion

    #region " maximum test time "

    /// <summary> The maximum test time range. </summary>

    /// <summary> The maximum Test Time range in seconds. </summary>
    /// <value> The maximum test time range. </value>
    public Std.Primitives.RangeI MaximumTestTimeRange
    {
        get;
        set
        {
            if ( this.MaximumTestTimeRange != value )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    } = Std.Primitives.RangeI.FullNonnegative;

    /// <summary> Gets or sets the cached Maximum Test Time. </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? MaximumTestTime
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.MaximumTestTime, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Maximum Test Time. </summary>
    /// <param name="value"> the Maximum Test Time. </param>
    /// <returns> the Maximum Test Time. </returns>
    public double? ApplyMaximumTestTime( double value )
    {
        _ = this.WriteMaximumTestTime( value );
        return this.QueryMaximumTestTime();
    }

    /// <summary> Gets or sets the Maximum Test Time query command. </summary>
    /// <value> the Maximum Test Time query command. </value>
    protected virtual string MaximumTestTimeQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Maximum Test Time. </summary>
    /// <returns> the Maximum Test Time or none if unknown. </returns>
    public double? QueryMaximumTestTime()
    {
        this.MaximumTestTime = this.Session.Query( this.MaximumTestTime, this.MaximumTestTimeQueryCommand );
        return this.MaximumTestTime;
    }

    /// <summary> Gets or sets the Maximum Test Time command format. </summary>
    /// <value> the Maximum Test Time command format. </value>
    protected virtual string MaximumTestTimeCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Maximum Test Time without reading back the value from the device.
    /// </summary>
    /// <remarks> This command sets the Maximum Test Time. </remarks>
    /// <param name="value"> the Maximum Test Time. </param>
    /// <returns> the Maximum Test Time. </returns>
    public double? WriteMaximumTestTime( double value )
    {
        this.MaximumTestTime = this.Session.WriteLine( value, this.MaximumTestTimeCommandFormat );
        return this.MaximumTestTime;
    }

    #endregion

    #region " system screen "

    /// <summary> The System Screen. </summary>

    /// <summary> Gets or sets the cached System Screen. </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public int? SystemScreen
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.SystemScreen, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the System Screen query command. </summary>
    /// <value> The System Screen query command. </value>
    protected virtual string SystemScreenQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the System Screen. </summary>
    /// <returns> The System Screen or none if unknown. </returns>
    public virtual int? QuerySystemScreen()
    {
        this.SystemScreen = this.Session.Query( this.SystemScreen, this.SystemScreenQueryCommand );
        return this.SystemScreen;
    }

    #endregion

    #region " registers "

    #region " temperature event register "

    #region " bit mask"

    /// <summary> The Temperature event enable bitmask. </summary>

    /// <summary>
    /// Gets or sets the cached value of the Temperature register event enable bit mask.
    /// </summary>
    /// <remarks>
    /// The returned value could be cast to the Temperature events type that is specific to the
    /// instrument The enable register gates the corresponding events for registration by the Status
    /// Byte register. When an event bit is set and the corresponding enable bit is set, the output
    /// (summary) of the register will set to 1, which in turn sets the summary bit of the Status
    /// Byte Register.
    /// </remarks>
    /// <value> The mask to use for enabling the events. </value>
    public int? TemperatureEventEnableBitmask
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.TemperatureEventEnableBitmask, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Programs and reads back the Temperature register events enable bit mask. </summary>
    /// <param name="value"> The value. </param>
    /// <returns> The bit mask or nothing if not known. </returns>
    public int? ApplyTemperatureEventEnableBitmask( int value )
    {
        _ = this.WriteTemperatureEventEnableBitmask( value );
        return this.QueryTemperatureEventEnableBitmask();
    }

    /// <summary> Gets or sets the temperature event enable mask query command. </summary>
    /// <remarks> 'TESE?'. </remarks>
    /// <value> The temperature event enable mask query command. </value>
    protected virtual string TemperatureEventEnableMaskQueryCommand { get; set; } = string.Empty;

    /// <summary> Reads back the Temperature register event enable bit mask. </summary>
    /// <remarks>
    /// The returned value could be cast to the Temperature events type that is specific to the
    /// instrument The enable register gates the corresponding events for registration by the Status
    /// Byte register. When an event bit is set and the corresponding enable bit is set, the output
    /// (summary) of the register will set to 1, which in turn sets the summary bit of the Status
    /// Byte Register.
    /// </remarks>
    /// <returns> The mask used for enabling the events. </returns>
    public int? QueryTemperatureEventEnableBitmask()
    {
        this.TemperatureEventEnableBitmask = this.Session.Query( this.TemperatureEventEnableBitmask.GetValueOrDefault( 0 ), this.TemperatureEventEnableMaskQueryCommand );
        return this.TemperatureEventEnableBitmask;
    }

    /// <summary> Gets or sets the temperature event enable mask command format. </summary>
    /// <remarks> 'TESE {0:D}'. </remarks>
    /// <value> The temperature event enable mask command format. </value>
    protected virtual string TemperatureEventEnableMaskCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Programs the Temperature register events enable bit mask without updating the value from the
    /// device.
    /// </summary>
    /// <param name="value"> The value. </param>
    /// <returns> The bit mask or nothing if not known. </returns>
    public int? WriteTemperatureEventEnableBitmask( int value )
    {
        _ = this.Session.WriteLine( this.TemperatureEventEnableMaskCommandFormat, value );
        this.TemperatureEventEnableBitmask = value;
        return this.TemperatureEventEnableBitmask;
    }

    #endregion

    #region " condition "

    /// <summary> The Temperature event Condition. </summary>

    /// <summary> Gets or sets the cached Condition of the Temperature register events. </summary>
    /// <value> <c>null</c> if value is not known;. </value>
    public int? TemperatureEventCondition
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.TemperatureEventCondition, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the Temperature event condition query command. </summary>
    /// <value> The Temperature event condition query command. </value>
    protected virtual string TemperatureEventConditionQueryCommand { get; set; } = string.Empty;

    /// <summary> Reads the condition of the Temperature register event. </summary>
    /// <returns> System.Nullable{System.Int32}. </returns>
    public virtual int? QueryTemperatureEventCondition()
    {
        if ( !string.IsNullOrWhiteSpace( this.TemperatureEventConditionQueryCommand ) )
        {
            this.TemperatureEventCondition = this.Session.Query( 0, this.TemperatureEventConditionQueryCommand );
        }

        return this.TemperatureEventCondition;
    }

    #endregion

    #region " status "

    /// <summary> The Temperature event status. </summary>
    private int? _temperatureEventStatus;

    /// <summary> Gets or sets the cached status of the Temperature register events. </summary>
    /// <value> <c>null</c> if value is not known;. </value>
    public virtual int? TemperatureEventStatus
    {
        get => this._temperatureEventStatus;

        protected set
        {
            this._temperatureEventStatus = value;
            this.NotifyPropertyChanged();
        }
    }

    /// <summary> Gets or sets the Temperature Event status query command. </summary>
    /// <remarks> 'TESR?'. </remarks>
    /// <value> The Temperature status query command. </value>
    protected virtual string TemperatureEventStatusQueryCommand { get; set; } = string.Empty;

    /// <summary> Reads the status of the Temperature register events. </summary>
    /// <returns> System.Nullable{System.Int32}. </returns>
    public virtual int? QueryTemperatureEventStatus()
    {
        if ( !string.IsNullOrWhiteSpace( this.TemperatureEventStatusQueryCommand ) )
        {
            this.TemperatureEventStatus = this.Session.Query( this.TemperatureEventStatus.GetValueOrDefault( 0 ), this.TemperatureEventStatusQueryCommand );
        }

        return this.TemperatureEventStatus;
    }
    #endregion

    #endregion

    #region " auxiliary register "

    #region " status "

    /// <summary> The Auxiliary event status. </summary>
    private int? _auxiliaryEventStatus;

    /// <summary> Gets or sets the cached status of the Auxiliary register events. </summary>
    /// <value> <c>null</c> if value is not known;. </value>
    public virtual int? AuxiliaryEventStatus
    {
        get => this._auxiliaryEventStatus;

        protected set
        {
            this._auxiliaryEventStatus = value;
            this.NotifyPropertyChanged();
        }
    }

    /// <summary> Gets or sets the Auxiliary Event status query command. </summary>
    /// <remarks> 'AUXC?'. </remarks>
    /// <value> The Auxiliary status query command. </value>
    protected virtual string AuxiliaryEventStatusQueryCommand { get; set; } = string.Empty;

    /// <summary> Reads the status of the Auxiliary register events. </summary>
    /// <returns> System.Nullable{System.Int32}. </returns>
    public virtual int? QueryAuxiliaryEventStatus()
    {
        if ( !string.IsNullOrWhiteSpace( this.AuxiliaryEventStatusQueryCommand ) )
        {
            this.AuxiliaryEventStatus = this.Session.Query( 0, this.AuxiliaryEventStatusQueryCommand );
        }

        return this.AuxiliaryEventStatus;
    }
    #endregion

    #endregion

    #endregion
}
/// <summary> Values that represent device sensor types. </summary>
public enum DeviceSensorType
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "Air Control" )]
    None = 0,

    /// <summary> An enum constant representing the thermocouple option. </summary>
    [System.ComponentModel.Description( "T Thermocouple" )]
    TThermocouple = 1,

    /// <summary> An enum constant representing the thermocouple option. </summary>
    [System.ComponentModel.Description( "K Thermocouple" )]
    KThermocouple = 2
}
