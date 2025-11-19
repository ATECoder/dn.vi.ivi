namespace cc.isr.VI;

/// <summary> Defines the Calculate Channel SCPI subsystem. </summary>
/// <remarks>
/// (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2016-07-06, 4.0.6031. </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="CalculateChannelSubsystemBase" /> class.
/// </remarks>
/// <param name="channelNumber">   The channel number. </param>
/// <param name="statusSubsystem"> The status subsystem. </param>
[CLSCompliant( false )]
public abstract class CalculateChannelSubsystemBase( int channelNumber, StatusSubsystemBase statusSubsystem ) : SubsystemBase( statusSubsystem )
{
    #region " channel "

    /// <summary> Gets or sets the channel number. </summary>
    /// <value> The channel number. </value>
    public int ChannelNumber { get; private set; } = channelNumber;

    #endregion

    #region " trace count "

    /// <summary> Number of traces. </summary>

    /// <summary> Gets or sets the cached Trace Count. </summary>
    /// <value> The Trace Count or none if not set or unknown. </value>
    public int? TraceCount
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.TraceCount, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Trace Count. </summary>
    /// <param name="value"> The current TraceCount. </param>
    /// <returns> The TraceCount or none if unknown. </returns>
    public int? ApplyTraceCount( int value )
    {
        _ = this.WriteTraceCount( value );
        return this.QueryTraceCount();
    }

    /// <summary> Gets or sets channel Trace Count query command. </summary>
    /// <remarks> SCPI: ":CALC{0}:PAR:COUN?". </remarks>
    /// <value> The Trace Count query command. </value>
    protected virtual string TraceCountQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current Trace Count. </summary>
    /// <returns> The Trace Count or none if unknown. </returns>
    public int? QueryTraceCount()
    {
        if ( !string.IsNullOrWhiteSpace( this.TraceCountQueryCommand ) )
        {
            this.TraceCount = this.Session.Query( 0, string.Format( this.TraceCountQueryCommand, this.ChannelNumber ) );
        }

        return this.TraceCount;
    }

    /// <summary> Gets or sets the channel Trace Count command format. </summary>
    /// <remarks> SCPI: ":CALC{0}:PAR:COUN {1}". </remarks>
    /// <value> The Trace Count command format. </value>
    protected virtual string TraceCountCommandFormat { get; set; } = string.Empty;

    /// <summary> WriteEnum the Trace Count without reading back the value from the device. </summary>
    /// <param name="value"> The current Trace Count. </param>
    /// <returns> The Trace Count or none if unknown. </returns>
    public int? WriteTraceCount( int value )
    {
        if ( !string.IsNullOrWhiteSpace( this.TraceCountCommandFormat ) )
        {
            _ = this.Session.WriteLine( this.TraceCountCommandFormat, this.ChannelNumber, value );
        }

        this.TraceCount = value;
        return this.TraceCount;
    }

    #endregion

    #region " average "

    /// <summary> Applies the average settings. </summary>
    /// <param name="enabled"> true to enable, false to disable. </param>
    /// <param name="count">   Number of. </param>
    public void ApplyAverageSettings( bool enabled, int count )
    {
        if ( !Nullable.Equals( this.AveragingEnabled, enabled ) )
        {
            _ = this.ApplyAveragingEnabled( enabled );
        }

        if ( !Nullable.Equals( this.AverageCount, count ) )
        {
            _ = this.ApplyAverageCount( count );
        }
    }

    #region " average clear "

    /// <summary> Gets or sets the clear command. </summary>
    /// <remarks> SCPI: ":CALC{0}:AVER:CLE". </remarks>
    /// <value> The clear command. </value>
    protected virtual string AverageClearCommand { get; set; } = string.Empty;

    /// <summary> Clears the average. </summary>
    public void ClearAverage()
    {
        _ = this.Session.WriteLine( string.Format( this.AverageClearCommand, this.ChannelNumber ) );
    }

    #endregion

    #region " average count "

    /// <summary> Number of averages. </summary>

    /// <summary> Gets or sets the cached Average Count. </summary>
    /// <value> The Average Count or none if not set or unknown. </value>
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

    /// <summary> Writes and reads back the Average Count. </summary>
    /// <param name="value"> The current AverageCount. </param>
    /// <returns> The AverageCount or none if unknown. </returns>
    public int? ApplyAverageCount( int value )
    {
        _ = this.WriteAverageCount( value );
        return this.QueryAverageCount();
    }

    /// <summary> Gets or sets Average Count query command. </summary>
    /// <remarks> SCPI: ":CAL{0}:AVER:COUN?". </remarks>
    /// <value> The Average Count query command. </value>
    protected virtual string AverageCountQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current Average Count. </summary>
    /// <returns> The Average Count or none if unknown. </returns>
    public int? QueryAverageCount()
    {
        if ( !string.IsNullOrWhiteSpace( this.AverageCountQueryCommand ) )
        {
            this.AverageCount = this.Session.Query( 0, string.Format( this.AverageCountQueryCommand, this.ChannelNumber ) );
        }

        return this.AverageCount;
    }

    /// <summary> Gets or sets Average Count command format. </summary>
    /// <remarks> SCPI: ":CALC{0}:AVER:COUN {1}". </remarks>
    /// <value> The Average Count command format. </value>
    protected virtual string AverageCountCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// WriteEnum the Average PointsAverageCount without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The current PointsAverageCount. </param>
    /// <returns> The PointsAverageCount or none if unknown. </returns>
    public int? WriteAverageCount( int value )
    {
        if ( !string.IsNullOrWhiteSpace( this.AverageCountCommandFormat ) )
        {
            _ = this.Session.WriteLine( this.AverageCountCommandFormat, this.ChannelNumber, value );
        }

        this.AverageCount = value;
        return this.AverageCount;
    }

    #endregion

    #region " averaging enabled "

    /// <summary> The averaging enabled. </summary>

    /// <summary> Gets or sets the cached Averaging Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Averaging Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? AveragingEnabled
    {
        get;

        protected set
        {
            if ( !Equals( this.AveragingEnabled, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Averaging Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyAveragingEnabled( bool value )
    {
        _ = this.WriteAveragingEnabled( value );
        return this.QueryAveragingEnabled();
    }

    /// <summary> Gets or sets the Averaging  enabled query command. </summary>
    /// <remarks> SCPI: ":CALC{0}:AVER?". </remarks>
    /// <value> The Averaging  enabled query command. </value>
    protected virtual string AveragingEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Averaging Enabled sentinel. Also sets the
    /// <see cref="AveragingEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryAveragingEnabled()
    {
        this.AveragingEnabled = this.Session.Query( this.AveragingEnabled, string.Format( this.AveragingEnabledQueryCommand, this.ChannelNumber ) );
        return this.AveragingEnabled;
    }

    /// <summary> Gets or sets the Averaging enabled command Format. </summary>
    /// <remarks> SCPI: ":CALC(0):AVER {1:1;1;0}". </remarks>
    /// <value> The Averaging enabled query command. </value>
    protected virtual string AveragingEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Averaging Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteAveragingEnabled( bool value )
    {
        _ = this.Session.WriteLine( string.Format( this.AveragingEnabledCommandFormat, this.ChannelNumber, value.GetHashCode() ) );
        this.AveragingEnabled = value;
        return this.AveragingEnabled;
    }

    #endregion

    #endregion
}
