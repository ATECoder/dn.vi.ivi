// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI;

/// <summary> Defines the SCPI Composite Limit subsystem. </summary>
/// <remarks>
/// (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-11-05. Created based on SCPI 5.1 library.  </para><para>
/// David, 2008-03-25, 5.0.3004 Port to new SCPI library. </para>
/// </remarks>
/// <remarks> Initializes a new instance of the <see cref="CompositeLimitBase" /> class. </remarks>
/// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
/// subsystem</see>. </param>
[CLSCompliant( false )]
public abstract class CompositeLimitBase( StatusSubsystemBase statusSubsystem ) : SubsystemBase( statusSubsystem )
{
    #region " i presettable "

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    public override void DefineKnownResetState()
    {
        base.DefineKnownResetState();
        this.LimitMode = VI.LimitMode.Grading;
        this.BinningControl = VI.BinningControl.Immediate;
        this.FailureBits = 15;
        this.PassBits = 15;
        this.AutoClearEnabled = true;
    }

    #endregion

    #region " commands "

    /// <summary> Gets or sets the composite limits clear command. </summary>
    /// <remarks> SCPI: ":CLAC2:CLIM:CLE". </remarks>
    /// <value> The composite limits clear command. </value>
    protected virtual string ClearCommand { get; set; } = string.Empty;

    /// <summary>
    /// Clears composite limits. Returns the instrument output to the TTL settings per SOURC2:TTL.
    /// </summary>
    public void ClearLimits()
    {
        if ( !string.IsNullOrWhiteSpace( this.ClearCommand ) )
            _ = this.Session.WriteLine( this.ClearCommand );
    }

    #endregion

    #region " auto clear enabled "

    /// <summary> The automatic clear enabled. </summary>

    /// <summary> Gets or sets the cached Composite Limits Auto Clear enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Composite Limits Auto Clear enabled is not known; <c>true</c> if output is on;
    /// otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? AutoClearEnabled
    {
        get;

        protected set
        {
            if ( !Equals( this.AutoClearEnabled, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Composite Limits Auto Clear enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if Enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyAutoClearEnabled( bool value )
    {
        _ = this.WriteAutoClearEnabled( value );
        return this.QueryAutoClearEnabled();
    }

    /// <summary> Gets or sets the Composite Limits Auto Clear enabled query command. </summary>
    /// <remarks> SCPI: ":CALC2:CLIM:CLE:AUTO?". </remarks>
    /// <value> The Composite Limits Auto Clear enabled query command. </value>
    protected virtual string AutoClearEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Auto Delay Enabled sentinel. Also sets the
    /// <see cref="AutoClearEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryAutoClearEnabled()
    {
        this.AutoClearEnabled = this.Session.Query( this.AutoClearEnabled, this.AutoClearEnabledQueryCommand );
        return this.AutoClearEnabled;
    }

    /// <summary> Gets or sets the Composite Limits Auto Clear enabled command Format. </summary>
    /// <remarks> SCPI: ":CALC2:CLIM:CLE:AUTO {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The Composite Limits Auto Clear enabled query command. </value>
    protected virtual string AutoClearEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Auto Delay Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteAutoClearEnabled( bool value )
    {
        this.AutoClearEnabled = this.Session.WriteLine( value, this.AutoClearEnabledCommandFormat );
        return this.AutoClearEnabled;
    }

    #endregion

    #region " failure bits "

    /// <summary> The failure bits. </summary>

    /// <summary> Gets or sets the cached Composite Limits Failure Bits. </summary>
    /// <value> The Composite Limits Failure Bits or none if not set or unknown. </value>
    public int? FailureBits
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.FailureBits, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Composite Limits Failure Bits. </summary>
    /// <param name="value"> The current Composite Limits Failure Bits. </param>
    /// <returns> The Composite Limits Failure Bits or none if unknown. </returns>
    public int? ApplyFailureBits( int value )
    {
        _ = this.WriteFailureBits( value );
        return this.QueryFailureBits();
    }

    /// <summary> Gets or sets the Lower Limit failure Bits query command. </summary>
    /// <remarks> SCPI: ":CALC2:CLIM:FAIL:SOUR2?". </remarks>
    /// <value> The Limit enabled query command. </value>
    protected virtual string FailureBitsQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current Lower Limit Failure Bits. </summary>
    /// <returns> The Lower Limit Failure Bits or none if unknown. </returns>
    public int? QueryFailureBits()
    {
        this.FailureBits = this.Session.Query( this.FailureBits, this.FailureBitsQueryCommand );
        return this.FailureBits;
    }

    /// <summary> Gets or sets the Lower Limit Failure Bits query command. </summary>
    /// <remarks> SCPI: "::CALC2:CLIM:FAIL:SOUR2 {0}". </remarks>
    /// <value> The Limit enabled query command. </value>
    protected virtual string FailureBitsCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Sets back the Lower Limit Failure Bits without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The current Lower Limit Failure Bits. </param>
    /// <returns> The Lower Limit Failure Bits or none if unknown. </returns>
    public int? WriteFailureBits( int value )
    {
        this.FailureBits = this.Session.WriteLine( value, this.FailureBitsCommandFormat );
        return this.FailureBits;
    }

    #endregion

    #region " limits pass bits "

    /// <summary> The pass bits. </summary>

    /// <summary> Gets or sets the cached Composite Limits Pass Bits. </summary>
    /// <value> The Composite Limits Pass Bits or none if not set or unknown. </value>
    public int? PassBits
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.PassBits, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Composite Limits Pass Bits. </summary>
    /// <param name="value"> The current Composite Limits Pass Bits. </param>
    /// <returns> The Composite Limits Pass Bits or none if unknown. </returns>
    public int? ApplyPassBits( int value )
    {
        _ = this.WritePassBits( value );
        return this.QueryPassBits();
    }

    /// <summary> Gets or sets the Lower Limit Pass Bits query command. </summary>
    /// <remarks> SCPI: ":CALC2:CLIM:PASS:SOUR2?". </remarks>
    /// <value> The Limit enabled query command. </value>
    protected virtual string PassBitsQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current Lower Limit Pass Bits. </summary>
    /// <returns> The Lower Limit Pass Bits or none if unknown. </returns>
    public int? QueryPassBits()
    {
        this.PassBits = this.Session.Query( this.PassBits, this.PassBitsQueryCommand );
        return this.PassBits;
    }

    /// <summary> Gets or sets the Lower Limit Pass Bits query command. </summary>
    /// <remarks> SCPI: "::CALC2:CLIM:PASS:SOUR2 {0}". </remarks>
    /// <value> The Limit enabled query command. </value>
    protected virtual string PassBitsCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Sets back the Lower Limit Pass Bits without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The current Lower Limit Pass Bits. </param>
    /// <returns> The Lower Limit Pass Bits or none if unknown. </returns>
    public int? WritePassBits( int value )
    {
        this.PassBits = this.Session.WriteLine( value, this.PassBitsCommandFormat );
        return this.PassBits;
    }

    #endregion

    #region " binning control "

    /// <summary> The Binning Control. </summary>

    /// <summary> Gets or sets the cached Binning Control. </summary>
    /// <value>
    /// The <see cref="BinningControl">Binning Control</see> or none if not set or unknown.
    /// </value>
    public BinningControl? BinningControl
    {
        get;

        protected set
        {
            if ( !this.BinningControl.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Binning Control. </summary>
    /// <param name="value"> The Binning Control. </param>
    /// <returns>
    /// The <see cref="BinningControl">source  Binning Control</see> or none if unknown.
    /// </returns>
    public BinningControl? ApplyBinningControl( BinningControl value )
    {
        _ = this.WriteBinningControl( value );
        return this.QueryBinningControl();
    }

    /// <summary> Gets or sets the Binning Control query command. </summary>
    /// <remarks> SCPI: ":CALC2:CLIM:BCON". </remarks>
    /// <value> The Binning Control query command. </value>
    protected virtual string BinningControlQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Binning Control. </summary>
    /// <returns> The <see cref="BinningControl"> Binning Control</see> or none if unknown. </returns>
    public BinningControl? QueryBinningControl()
    {
        this.BinningControl = this.Session.QueryEnum( this.BinningControl, this.BinningControlQueryCommand );
        return this.BinningControl;
    }

    /// <summary> Gets or sets the Binning Control command format. </summary>
    /// <remarks> SCPI: ":CALC2:CLIM:BCON {0}". </remarks>
    /// <value> The Binning Control query command format. </value>
    protected virtual string BinningControlCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Binning Control without reading back the value from the device. </summary>
    /// <param name="value"> The Binning Control. </param>
    /// <returns> The <see cref="BinningControl"> Binning Control</see> or none if unknown. </returns>
    public BinningControl? WriteBinningControl( BinningControl value )
    {
        this.BinningControl = this.Session.WriteEnum( value, this.BinningControlCommandFormat );
        return this.BinningControl;
    }

    #endregion

    #region " limit mode "

    /// <summary> The Limit Mode. </summary>

    /// <summary> Gets or sets the cached Limit Mode. </summary>
    /// <value> The <see cref="LimitMode">Limit Mode</see> or none if not set or unknown. </value>
    public LimitMode? LimitMode
    {
        get;

        protected set
        {
            if ( !this.LimitMode.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Limit Mode. </summary>
    /// <param name="value"> The Limit Mode. </param>
    /// <returns> The <see cref="LimitMode">source  Limit Mode</see> or none if unknown. </returns>
    public LimitMode? ApplyLimitMode( LimitMode value )
    {
        _ = this.WriteLimitMode( value );
        return this.QueryLimitMode();
    }

    /// <summary> Gets or sets the Limit Mode query command. </summary>
    /// <remarks> SCPI: "CALC2:CLIM:MODE". </remarks>
    /// <value> The Limit Mode query command. </value>
    protected virtual string LimitModeQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Limit Mode. </summary>
    /// <returns> The <see cref="LimitMode"> Limit Mode</see> or none if unknown. </returns>
    public LimitMode? QueryLimitMode()
    {
        this.LimitMode = this.Session.QueryEnum( this.LimitMode, this.LimitModeQueryCommand );
        return this.LimitMode;
    }

    /// <summary> Gets or sets the Limit Mode command format. </summary>
    /// <remarks> SCPI: "CALC2:CLIM:MODE". </remarks>
    /// <value> The Limit Mode command format. </value>
    protected virtual string LimitModeCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Limit Mode without reading back the value from the device. </summary>
    /// <param name="value"> The Limit Mode. </param>
    /// <returns> The <see cref="LimitMode"> Limit Mode</see> or none if unknown. </returns>
    public LimitMode? WriteLimitMode( LimitMode value )
    {
        this.LimitMode = this.Session.WriteEnum( value, this.LimitModeCommandFormat );
        return this.LimitMode;
    }

    #endregion
}
/// <summary> Enumerates the binning control mode. </summary>
public enum BinningControl
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "Not Defined ()" )]
    None,

    /// <summary> An enum constant representing the immediate option. </summary>
    [System.ComponentModel.Description( "Immediate (IMM)" )]
    Immediate,

    /// <summary> An enum constant representing the end] option. </summary>
    [System.ComponentModel.Description( "End (END)" )]
    End
}
/// <summary> Enumerates the grading control mode. </summary>
public enum LimitMode
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "Not Defined ()" )]
    None,

    /// <summary> An enum constant representing the grading option. </summary>
    [System.ComponentModel.Description( "Grading (GRAD)" )]
    Grading,

    /// <summary> An enum constant representing the end] option. </summary>
    [System.ComponentModel.Description( "End (END)" )]
    End
}
