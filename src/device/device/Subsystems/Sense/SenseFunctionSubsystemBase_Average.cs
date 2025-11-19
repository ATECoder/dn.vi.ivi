// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI;

public partial class SenseFunctionSubsystemBase
{
    #region " average count "

    /// <summary> The average count range. </summary>
    private Std.Primitives.RangeI _averageCountRange;

    /// <summary> The Average Count range in seconds. </summary>
    /// <value> The average count range. </value>
    public Std.Primitives.RangeI AverageCountRange
    {
        get => this._averageCountRange;
        set
        {
            if ( this.AverageCountRange != value )
            {
                this._averageCountRange = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the cached average count. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public int? AverageCount
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.AverageCount, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the average count. </summary>
    /// <param name="value"> The average count. </param>
    /// <returns> The average count. </returns>
    public int? ApplyAverageCount( int value )
    {
        _ = this.WriteAverageCount( value );
        return this.QueryAverageCount();
    }

    /// <summary> Gets or sets The average count query command. </summary>
    /// <value> The average count query command. </value>
    protected virtual string AverageCountQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries The average count. </summary>
    /// <returns> The average count or none if unknown. </returns>
    public int? QueryAverageCount()
    {
        this.AverageCount = this.Session.Query( this.AverageCount, this.AverageCountQueryCommand );
        return this.AverageCount;
    }

    /// <summary> Gets or sets The average count command format. </summary>
    /// <value> The average count command format. </value>
    protected virtual string AverageCountCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes The average count without reading back the value from the device. </summary>
    /// <remarks> This command sets The average count. </remarks>
    /// <param name="value"> The average count. </param>
    /// <returns> The average count. </returns>
    public int? WriteAverageCount( int value )
    {
        this.AverageCount = this.Session.WriteLine( value, this.AverageCountCommandFormat );
        return this.AverageCount;
    }

    #endregion

    #region " average enabled "

    /// <summary> Average enabled. </summary>

    /// <summary> Gets or sets the cached Average Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Average Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? AverageEnabled
    {
        get;

        protected set
        {
            if ( !Equals( this.AverageEnabled, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Average Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyAverageEnabled( bool value )
    {
        _ = this.WriteAverageEnabled( value );
        return this.QueryAverageEnabled();
    }

    /// <summary> Gets or sets the Average enabled query command. </summary>
    /// <remarks> SCPI: "CURR:AVER:STAT?". </remarks>
    /// <value> The Average enabled query command. </value>
    protected virtual string AverageEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Average Enabled sentinel. Also sets the
    /// <see cref="AverageEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryAverageEnabled()
    {
        this.AverageEnabled = this.Session.Query( this.AverageEnabled, this.AverageEnabledQueryCommand );
        return this.AverageEnabled;
    }

    /// <summary> Gets or sets the Average enabled command Format. </summary>
    /// <remarks> SCPI: "CURR:AVER:STAT {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The Average enabled query command. </value>
    protected virtual string AverageEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Average Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteAverageEnabled( bool value )
    {
        this.AverageEnabled = this.Session.WriteLine( value, this.AverageEnabledCommandFormat );
        return this.AverageEnabled;
    }

    #endregion

    #region " average filter type "

    /// <summary> Define average filter types read writes. </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0028:Simplify collection initialization", Justification = "<Pending>" )]
    private void DefineAverageFilterTypesReadWrites()
    {
        this.AverageFilterTypesReadWrites = new();
        foreach ( AverageFilterTypes enumValue in Enum.GetValues( typeof( AverageFilterTypes ) ) )
            this.AverageFilterTypesReadWrites.Add( enumValue );
    }

    /// <summary> Gets or sets a dictionary of Average Filter Type parses. </summary>
    /// <value> A Dictionary of Average Filter Type parses. </value>
    public Pith.EnumReadWriteCollection AverageFilterTypesReadWrites { get; private set; }

    /// <summary> Gets or sets the supported Average Filter Types. </summary>
    /// <value> The supported Average Filter Types. </value>
    public AverageFilterTypes SupportedAverageFilterTypes
    {
        get;
        set
        {
            if ( !this.SupportedAverageFilterTypes.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the cached source AverageFilterType. </summary>
    /// <value>
    /// The <see cref="AverageFilterType">source Average Filter Type</see> or none if not set or
    /// unknown.
    /// </value>
    public AverageFilterTypes? AverageFilterType
    {
        get;

        protected set
        {
            if ( !this.AverageFilterType.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
                this.MovingAverageFilterEnabled = Nullable.Equals( value, AverageFilterTypes.Moving );
            }
        }
    }

    /// <summary> Writes and reads back the source Average Filter Type. </summary>
    /// <param name="value"> The  Source Average Filter Type. </param>
    /// <returns>
    /// The <see cref="AverageFilterType">source Average Filter Type</see> or none if unknown.
    /// </returns>
    public AverageFilterTypes? ApplyAverageFilterType( AverageFilterTypes value )
    {
        _ = this.WriteAverageFilterType( value );
        return this.QueryAverageFilterType();
    }

    /// <summary> Gets or sets the Average Filter Type query command. </summary>
    /// <remarks> SCPI: SENS:CURR:DC:AVER:TCON?". </remarks>
    /// <value> The Average Filter Type query command. </value>
    protected virtual string AverageFilterTypeQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Average Filter Type. </summary>
    /// <returns>
    /// The <see cref="AverageFilterType">Average Filter Type</see> or none if unknown.
    /// </returns>
    public AverageFilterTypes? QueryAverageFilterType()
    {
        this.AverageFilterType = this.Session.Query( this.AverageFilterType.GetValueOrDefault( AverageFilterTypes.None ), this.AverageFilterTypesReadWrites,
            this.AverageFilterTypeQueryCommand );
        return this.AverageFilterType;
    }

    /// <summary> Gets or sets the Average Filter Type command format. </summary>
    /// <remarks> SCPI: SENS:CURR:DC:AVER:TCON {0}. </remarks>
    /// <value> The write Average Filter Type command format. </value>
    protected virtual string AverageFilterTypeCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Average Filter Type without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The Average Filter Type. </param>
    /// <returns>
    /// The <see cref="AverageFilterType">Average Filter Type</see> or none if unknown.
    /// </returns>
    public AverageFilterTypes? WriteAverageFilterType( AverageFilterTypes value )
    {
        this.AverageFilterType = this.Session.Write( value, this.AverageFilterTypeCommandFormat, this.AverageFilterTypesReadWrites );
        return this.AverageFilterType;
    }

    #endregion

    #region " average moving filter enabled "

    /// <summary> Moving Average Filter enabled. </summary>

    /// <summary> Gets or sets the cached Moving Average Filter Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Moving Average Filter Enabled is not known; <c>true</c> if output is on;
    /// otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? MovingAverageFilterEnabled
    {
        get;

        protected set
        {
            if ( !Equals( this.MovingAverageFilterEnabled, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Moving Average Filter Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyMovingAverageFilterEnabled( bool value )
    {
        _ = this.WriteMovingAverageFilterEnabled( value );
        return this.QueryMovingAverageFilterEnabled();
    }

    /// <summary>
    /// Queries the Moving Average Filter Enabled sentinel. Also sets the
    /// <see cref="MovingAverageFilterEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryMovingAverageFilterEnabled()
    {
        this.MovingAverageFilterEnabled = Nullable.Equals( AverageFilterTypes.Moving, this.QueryAverageFilterType() );
        return this.MovingAverageFilterEnabled;
    }

    /// <summary>
    /// Writes the Moving Average Filter Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteMovingAverageFilterEnabled( bool value )
    {
        this.MovingAverageFilterEnabled = Nullable.Equals( AverageFilterTypes.Moving, this.WriteAverageFilterType( value ? AverageFilterTypes.Moving : AverageFilterTypes.Repeat ) );
        return this.MovingAverageFilterEnabled;
    }

    #endregion

    #region " average percent window "

    /// <summary> The average percent window range. </summary>
    private Std.Primitives.RangeR _averagePercentWindowRange;

    /// <summary> The Average Percent Window range. </summary>
    /// <value> The average percent window range. </value>
    public Std.Primitives.RangeR AveragePercentWindowRange
    {
        get => this._averagePercentWindowRange;
        set
        {
            if ( this.AveragePercentWindowRange != value )
            {
                this._averagePercentWindowRange = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the cached Average Percent Window. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? AveragePercentWindow
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.AveragePercentWindow, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Average Percent Window. </summary>
    /// <param name="value"> The Average Percent Window. </param>
    /// <returns> The Average Percent Window. </returns>
    public double? ApplyAveragePercentWindow( double value )
    {
        _ = this.WriteAveragePercentWindow( value );
        return this.QueryAveragePercentWindow();
    }

    /// <summary> Gets or sets The Average Percent Window query command. </summary>
    /// <value> The Average Percent Window query command. </value>
    protected virtual string AveragePercentWindowQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries The Average Percent Window. </summary>
    /// <returns> The Average Percent Window or none if unknown. </returns>
    public double? QueryAveragePercentWindow()
    {
        this.AveragePercentWindow = this.Session.Query( this.AveragePercentWindow, this.AveragePercentWindowQueryCommand );
        return this.AveragePercentWindow;
    }

    /// <summary> Gets or sets The Average Percent Window command format. </summary>
    /// <value> The Average Percent Window command format. </value>
    protected virtual string AveragePercentWindowCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes The Average Percent Window without reading back the value from the device.
    /// </summary>
    /// <remarks> This command sets The Average Percent Window. </remarks>
    /// <param name="value"> The Average Percent Window. </param>
    /// <returns> The Average Percent Window. </returns>
    public double? WriteAveragePercentWindow( double value )
    {
        this.AveragePercentWindow = this.Session.WriteLine( value, this.AveragePercentWindowCommandFormat );
        return this.AveragePercentWindow;
    }

    #endregion
}
/// <summary> Values that represent average filter types. </summary>
[Flags]
public enum AverageFilterTypes
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "Not Defined ()" )]
    None = 0,

    /// <summary> An enum constant representing the repeat option. </summary>
    [System.ComponentModel.Description( "Repeat (REP)" )]
    Repeat = 1,

    /// <summary> An enum constant representing the moving option. </summary>
    [System.ComponentModel.Description( "Moving (MOV)" )]
    Moving = 2
}
