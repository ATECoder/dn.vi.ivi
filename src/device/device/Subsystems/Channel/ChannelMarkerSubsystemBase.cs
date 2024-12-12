namespace cc.isr.VI;

/// <summary> Defines the Channel Marker subsystem. </summary>
/// <remarks>
/// (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2016-07-06, 4.0.6031. </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="ChannelMarkerSubsystemBase" /> class.
/// </remarks>
/// <remarks> David, 2020-08-12. </remarks>
/// <param name="markerNumber">    The Marker number. </param>
/// <param name="channelNumber">   The channel number. </param>
/// <param name="statusSubsystem"> The status subsystem. </param>
/// <param name="readingAmounts">  The reading amounts. </param>
[CLSCompliant( false )]
public abstract partial class ChannelMarkerSubsystemBase( int markerNumber, int channelNumber,
    StatusSubsystemBase statusSubsystem, ReadingAmounts readingAmounts ) : SubsystemBase( statusSubsystem )
{
    #region " channel "

    /// <summary> Gets or sets the channel number. </summary>
    /// <value> The channel number. </value>
    public int ChannelNumber { get; private set; } = channelNumber;

    #endregion

    #region " marker "

    /// <summary> Gets or sets the Marker number. </summary>
    /// <value> The Marker number. </value>
    public int MarkerNumber { get; private set; } = markerNumber;

    #endregion

    #region " latest data "

    /// <summary> Gets or sets the latest data query command. </summary>
    /// <remarks> SCPI: ":CALC{0}:MARK{1}:Y?". </remarks>
    /// <value> The latest data query command. </value>
    protected virtual string LatestDataQueryCommand { get; set; } = string.Empty;

    /// <summary> Fetches the latest data and parses it. </summary>
    /// <remarks>
    /// Issues the ':SENSE:DATA:LAT?' query, which reads data stored in the Sample Buffer.
    /// </remarks>
    /// <returns> The latest data. </returns>
    public virtual double? FetchLatestData()
    {
        return this.MeasureReadingAmounts( string.Format( this.LatestDataQueryCommand, this.ChannelNumber, this.MarkerNumber ) );
    }

    #endregion

    #region " abscissa "

    /// <summary> The abscissa. </summary>
    private double? _abscissa;

    /// <summary> Gets or sets the cached Trigger Abscissa. </summary>
    /// <remarks>
    /// The Abscissa is used to Abscissa operation in the trigger layer. After the programmed trigger
    /// event occurs, the instrument waits until the Abscissa period expires before performing the
    /// Device Action.
    /// </remarks>
    /// <value> The Trigger Abscissa or none if not set or unknown. </value>
    public double? Abscissa
    {
        get => this._abscissa;

        protected set
        {
            if ( !Nullable.Equals( this.Abscissa, value ) )
            {
                this._abscissa = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Trigger Abscissa. </summary>
    /// <param name="value"> The current Abscissa. </param>
    /// <returns> The Trigger Abscissa or none if unknown. </returns>
    public double? ApplyAbscissa( double value )
    {
        _ = this.WriteAbscissa( value );
        return this.QueryAbscissa();
    }

    /// <summary> Gets or sets the Abscissa query command. </summary>
    /// <remarks> SCPI: ":CALC{0}:MARK{1}:X?". </remarks>
    /// <value> The Abscissa query command. </value>
    protected virtual string AbscissaQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Abscissa. </summary>
    /// <returns> The Abscissa or none if unknown. </returns>
    public double? QueryAbscissa()
    {
        this.Abscissa = this.Session.Query( this.Abscissa, string.Format( this.AbscissaQueryCommand, this.ChannelNumber, this.MarkerNumber ) );
        return this.Abscissa;
    }

    /// <summary> Gets or sets the Abscissa command format. </summary>
    /// <remarks> SCPI: ":CALC{0}:MARK{1}:X {2}". </remarks>
    /// <value> The Abscissa command format. </value>
    protected virtual string AbscissaCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Trigger Abscissa without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The current Abscissa. </param>
    /// <returns> The Trigger Abscissa or none if unknown. </returns>
    public double? WriteAbscissa( double value )
    {
        _ = this.Session.WriteLine( string.Format( this.AbscissaCommandFormat, this.ChannelNumber, this.MarkerNumber, value ) );
        this.Abscissa = value;
        return this.Abscissa;
    }

    #endregion

    #region " enabled "

    /// <summary> The enabled. </summary>
    private bool? _enabled;

    /// <summary> Gets or sets the cached Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if  Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? Enabled
    {
        get => this._enabled;

        protected set
        {
            if ( !Equals( this.Enabled, value ) )
            {
                this._enabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the  Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyEnabled( bool value )
    {
        _ = this.WriteEnabled( value );
        return this.QueryEnabled();
    }

    /// <summary> Gets or sets the marker enabled query command. </summary>
    /// <remarks> SCPI: ":CALC{0}:MARK{1}:STAT?". </remarks>
    /// <value> The marker enabled query command. </value>
    protected virtual string EnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the  Enabled sentinel. Also sets the
    /// <see cref="Enabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryEnabled()
    {
        this.Enabled = this.Session.Query( this.Enabled, string.Format( this.EnabledQueryCommand, this.ChannelNumber, this.MarkerNumber ) );
        return this.Enabled;
    }

    /// <summary> Gets or sets the marker enabled command Format. </summary>
    /// <remarks> SCPI: "":CALC{0}:MARK{1}:STAT {2:1;1;0}". </remarks>
    /// <value> The marker enabled query command. </value>
    protected virtual string EnabledCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the  Enabled sentinel. Does not read back from the instrument. </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteEnabled( bool value )
    {
        _ = this.Session.WriteLine( string.Format( this.EnabledCommandFormat, this.ChannelNumber, this.MarkerNumber, value.GetHashCode() ) );
        this.Enabled = value;
        return this.Enabled;
    }

    #endregion
}
