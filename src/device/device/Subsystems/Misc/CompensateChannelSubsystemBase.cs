namespace cc.isr.VI;

/// <summary> Defines the Compensation Channel subsystem. </summary>
/// <remarks>
/// (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2016-07-06, 4.0.6031. </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="CompensateChannelSubsystemBase" /> class.
/// </remarks>
/// <param name="channelNumber">   The channel number. </param>
/// <param name="statusSubsystem"> The status subsystem. </param>
[CLSCompliant( false )]
public abstract partial class CompensateChannelSubsystemBase( int channelNumber, StatusSubsystemBase statusSubsystem ) : SubsystemBase( statusSubsystem )
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="CompensateChannelSubsystemBase" /> class.
    /// </summary>
    /// <param name="compensationType"> Type of the compensation. </param>
    /// <param name="channelNumber">    A reference to a <see cref="StatusSubsystemBase">status
    /// subsystem</see>. </param>
    /// <param name="statusSubsystem">  The status subsystem. </param>
    protected CompensateChannelSubsystemBase( CompensationTypes compensationType, int channelNumber, StatusSubsystemBase statusSubsystem ) : this( channelNumber, statusSubsystem )
    {
        this.ApplyCompensationType( compensationType );
        this.DefineCompensationTypeReadWrites();
    }

    #endregion

    #region " channel "

    /// <summary> Gets or sets the channel number. </summary>
    /// <exception cref="ArgumentNullException">     Thrown when one or more required arguments are
    /// null. </exception>
    /// <exception cref="InvalidCastException">      Thrown when an object cannot be cast to a
    /// required type. </exception>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <value> The channel number. </value>
    public int ChannelNumber { get; private set; } = channelNumber;

    #endregion

    #region " array <> string "

    /// <summary> Parses an impedance array string. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <exception cref="InvalidCastException">  Thrown when an object cannot be cast to a required
    /// type. </exception>
    /// <param name="values"> The values. </param>
    /// <returns>
    /// An enumerator that allows for each to be used to process parse in this collection.
    /// </returns>
    public static IList<double> Parse( string values )
    {
        if ( string.IsNullOrWhiteSpace( values ) ) throw new ArgumentNullException( nameof( values ) );
        Queue<string> v = new( values.Split( ',' ) );
        List<double> data = [];
        while ( v.Any() )
        {
            string pop = v.Dequeue();
            if ( !double.TryParse( pop, out double value ) )
                throw new InvalidCastException( $"Parse failed for value {pop}" );

            data.Add( value );
        }

        return data;
    }

    /// <summary> Builds an impedance array string. </summary>
    /// <param name="values"> The values. </param>
    /// <returns> A <see cref="string" />. </returns>
    public static string Build( IList<double> values )
    {
        System.Text.StringBuilder builder = new();
        if ( values?.Any() == true )
        {
            foreach ( double v in values )
            {
                if ( builder.Length > 0 )
                    _ = builder.Append( "," );
                _ = builder.Append( v.ToString() );
            }
        }

        return builder.ToString();
    }

    /// <summary> Builds impedance array string. </summary>
    /// <param name="includesFrequency">      true if impedance array includes. </param>
    /// <param name="lowFrequencyImpedance">  The low frequency impedance values. </param>
    /// <param name="highFrequencyImpedance"> The high frequency values. </param>
    /// <returns> A <see cref="string" />. </returns>
    public static string Build( bool includesFrequency, IList<double> lowFrequencyImpedance, IList<double> highFrequencyImpedance )
    {
        return Build( Merge( includesFrequency, lowFrequencyImpedance, highFrequencyImpedance ) );
    }

    /// <summary> Merges the impedance arrays into a single array. </summary>
    /// <exception cref="ArgumentNullException">     Thrown when one or more required arguments are
    /// null. </exception>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <param name="includesFrequency">      true if impedance array includes. </param>
    /// <param name="lowFrequencyImpedance">  The low frequency impedance values. </param>
    /// <param name="highFrequencyImpedance"> The high frequency values. </param>
    /// <returns>
    /// An enumerator that allows for each to be used to process merge in this collection.
    /// </returns>
    public static IList<double> Merge( bool includesFrequency, IList<double> lowFrequencyImpedance, IList<double> highFrequencyImpedance )
    {
        if ( lowFrequencyImpedance is null ) throw new ArgumentNullException( nameof( lowFrequencyImpedance ) );
        if ( highFrequencyImpedance is null ) throw new ArgumentNullException( nameof( highFrequencyImpedance ) );
        int startIndex = includesFrequency ? 1 : 0;
        if ( lowFrequencyImpedance.Count != startIndex + 2 )
            throw new InvalidOperationException( $"Low frequency array has {lowFrequencyImpedance.Count} values instead of {startIndex + 2}" );
        if ( highFrequencyImpedance.Count != startIndex + 2 )
            throw new InvalidOperationException( $"High frequency array has {highFrequencyImpedance.Count} values instead of {startIndex + 2}" );
        List<double> l = [lowFrequencyImpedance[startIndex], lowFrequencyImpedance[startIndex + 1], highFrequencyImpedance[startIndex], highFrequencyImpedance[startIndex + 1]];
        return [.. l];
    }

    /// <summary> Merge frequency and impedance arrays. </summary>
    /// <exception cref="ArgumentNullException">     Thrown when one or more required arguments are
    /// null. </exception>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <param name="frequencies"> The frequencies. </param>
    /// <param name="impedances">  The impedances. </param>
    /// <returns> A <see cref="string" />. </returns>
    public static string Merge( string frequencies, string impedances )
    {
        if ( string.IsNullOrWhiteSpace( frequencies ) ) throw new ArgumentNullException( nameof( frequencies ) );
        if ( string.IsNullOrWhiteSpace( impedances ) ) throw new ArgumentNullException( nameof( impedances ) );
        System.Text.StringBuilder builder = new();
        Queue<string> f = new( frequencies.Split( ',' ) );
        Queue<string> v = new( impedances.Split( ',' ) );
        if ( 2 * f.Count != v.Count )
        {
            throw new InvalidOperationException( $"Number of values {v.Count} must be twice the number of frequencies {f.Count}" );
        }

        while ( f.Any() )
        {
            _ = builder.Length > 0 ? builder.AppendFormat( "{0}", f.Dequeue() ) : builder.AppendFormat( ",{0}", f.Dequeue() );

            if ( v.Any() )
                _ = builder.AppendFormat( ",{0}", v.Dequeue() );
            if ( v.Any() )
                _ = builder.AppendFormat( ",{0}", v.Dequeue() );
        }

        return builder.ToString();
    }

    #endregion

    #region " commands "

    /// <summary> Gets or sets the clear measurements command. </summary>
    /// <remarks> SCPI: ":SENS{0}:CORR2:{1}:COLL:CLE". </remarks>
    /// <value> The clear measurements command. </value>
    protected virtual string ClearMeasurementsCommand { get; set; } = string.Empty;

    /// <summary> Clears the measured data. </summary>
    public void ClearMeasurements()
    {
        _ = this.Session.WriteLine( this.ClearMeasurementsCommand, this.ChannelNumber, this.CompensationTypeCode );
        this.FrequencyArray = Array.Empty<double>();
        this.FrequencyArrayReading = string.Empty;
        this.ImpedanceArray = Array.Empty<double>();
        this.ImpedanceArrayReading = string.Empty;
        this.Enabled = false;
        this.FrequencyStimulusPoints = new int?();
    }

    /// <summary> Gets or sets the Acquire measurements command. </summary>
    /// <remarks> SCPI: ":SENS{0}:CORR2:COLL:ACQ:{1}". </remarks>
    /// <value> The clear measurements command. </value>
    protected virtual string AcquireMeasurementsCommand { get; set; } = string.Empty;

    /// <summary> Acquires the measured data. </summary>
    public void AcquireMeasurements()
    {
        _ = this.Session.WriteLine( this.ClearMeasurementsCommand, this.ChannelNumber, this.CompensationTypeCode );
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

    /// <summary> Gets or sets the compensation enabled query command. </summary>
    /// <remarks> SCPI: ":SENS{0}:CORR2:{1}:STAT?". </remarks>
    /// <value> The compensation enabled query command. </value>
    protected virtual string EnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the compensation Enabled sentinel. Also sets the
    /// <see cref="Enabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryEnabled()
    {
        this.Enabled = this.Session.Query( this.Enabled, string.Format( this.EnabledQueryCommand, this.ChannelNumber, this.CompensationTypeCode ) );
        return this.Enabled;
    }

    /// <summary> Gets or sets the enabled command Format. </summary>
    /// <remarks> SCPI: ":SENS{0}:CORR2:{1}:STAT {0:1;1;0}". </remarks>
    /// <value> The compensation enabled query command. </value>
    protected virtual string EnabledCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the  Enabled sentinel. Does not read back from the instrument. </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteEnabled( bool value )
    {
        _ = this.Session.WriteLine( this.EnabledCommandFormat, this.ChannelNumber, this.CompensationTypeCode, value.GetHashCode() );
        this.Enabled = value;
        return this.Enabled;
    }

    #endregion

    #region " frequency stimulus points "

    /// <summary> The Frequency Stimulus Points. </summary>
    private int? _frequencyStimulusPoints;

    /// <summary>
    /// Gets or sets the cached Frequency Stimulus Points. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public int? FrequencyStimulusPoints
    {
        get => this._frequencyStimulusPoints;

        protected set
        {
            if ( !Nullable.Equals( this.FrequencyStimulusPoints, value ) )
            {
                this._frequencyStimulusPoints = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets the Frequency Stimulus Points query command. </summary>
    /// <remarks> SCPI: ":SENS{0}:CORR2:ZME:{1}:POIN?". </remarks>
    /// <value> The Frequency Stimulus Points query command. </value>
    protected virtual string FrequencyStimulusPointsQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Frequency Stimulus Points. </summary>
    /// <returns> The Frequency Stimulus Points or none if unknown. </returns>
    public int? QueryFrequencyStimulusPoints()
    {
        this.FrequencyStimulusPoints = this.Session.Query( this.FrequencyStimulusPoints,
                                                   string.Format( this.FrequencyStimulusPointsQueryCommand, this.ChannelNumber, this.CompensationTypeCode ) );
        return this.FrequencyStimulusPoints;
    }

    #endregion

    #region " frequency array "

    /// <summary> Gets the has complete compensation values. </summary>
    /// <value> The has complete compensation values. </value>
    public bool HasCompleteCompensationValues => !(string.IsNullOrWhiteSpace( this.FrequencyArrayReading ) | string.IsNullOrWhiteSpace( this.ImpedanceArrayReading ));

    /// <summary> The frequency array reading. </summary>
    private string? _frequencyArrayReading;

    /// <summary> Gets or sets the frequency array reading. </summary>
    /// <value> The frequency array reading. </value>
    public string? FrequencyArrayReading
    {
        get => this._frequencyArrayReading;

        protected set
        {
            if ( !string.Equals( value, this.FrequencyArrayReading, StringComparison.Ordinal ) )
            {
                this._frequencyArrayReading = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged( nameof( this.HasCompleteCompensationValues ) );
            }
        }
    }

    /// <summary> The Frequency Array. </summary>
    private IList<double>? _frequencyArray;

    /// <summary>
    /// Gets or sets the cached Frequency Array. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public IList<double>? FrequencyArray
    {
        get => this._frequencyArray;

        protected set
        {
            if ( !Equals( this.FrequencyArray, value ) )
            {
                this._frequencyArray = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Frequency Array. </summary>
    /// <param name="value"> The Frequency Array. </param>
    /// <returns> The Frequency Array. </returns>
    public IList<double> ApplyFrequencyArray( IList<double> value )
    {
        _ = this.WriteFrequencyArray( value );
        return this.QueryFrequencyArray();
    }

    /// <summary> Gets or sets the Frequency Array query command. </summary>
    /// <remarks> SCPI: ":SENS{0}:CORR2:ZME:{1}:FREQ?". </remarks>
    /// <value> The Frequency Array query command. </value>
    protected virtual string FrequencyArrayQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Frequency Array. </summary>
    /// <returns> The Frequency Array or none if unknown. </returns>
    public IList<double> QueryFrequencyArray()
    {
        this.FrequencyArrayReading = this.Session.QueryTrimEnd( string.Empty, string.Format( this.FrequencyArrayQueryCommand, this.ChannelNumber, this.CompensationTypeCode ) );
        if ( this.FrequencyArrayReading is not null )
        {
            this.FrequencyArray = Parse( this._frequencyArrayReading! );
        }
        else
            this.FrequencyArray = [];

        return this.FrequencyArray;
    }

    /// <summary> Gets or sets the Frequency Array command format. </summary>
    /// <remarks> SCPI: ":SENS{0}:CORR2:ZME:{1}:FREQ {0}". </remarks>
    /// <value> The Frequency Array command format. </value>
    protected virtual string FrequencyArrayCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Frequency Array without reading back the value from the device. </summary>
    /// <remarks> This command sets the Frequency Array. </remarks>
    /// <param name="values"> The Frequency Array. </param>
    /// <returns> The Frequency Array. </returns>
    public IList<double>? WriteFrequencyArray( IList<double> values )
    {
        if ( values is not null )
        {
            this.FrequencyArrayReading = Build( values );
            _ = this.Session.WriteLine( this.FrequencyArrayCommandFormat, this._frequencyArrayReading! );
            this.FrequencyArray = [.. values!];
        }
        return this.FrequencyArray;
    }

    #endregion

    #region " impedance array "

    /// <summary> The impedance array reading. </summary>
    private string? _impedanceArrayReading;

    /// <summary> Gets or sets the impedance array reading. </summary>
    /// <value> The impedance array reading. </value>
    public string? ImpedanceArrayReading
    {
        get => this._impedanceArrayReading;

        protected set
        {
            if ( !string.Equals( value, this.ImpedanceArrayReading, StringComparison.Ordinal ) )
            {
                this._impedanceArrayReading = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged( nameof( this.HasCompleteCompensationValues ) );
            }
        }
    }

    /// <summary> The Impedance Array. </summary>
    private IList<double>? _impedanceArray;

    /// <summary>
    /// Gets or sets the cached Impedance Array. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public IList<double>? ImpedanceArray
    {
        get => this._impedanceArray;

        protected set
        {
            if ( !Equals( this.ImpedanceArray, value ) )
            {
                this._impedanceArray = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Impedance Array. </summary>
    /// <param name="reading"> The reading. </param>
    /// <returns> The Impedance Array. </returns>
    public IList<double>? ApplyImpedanceArray( string reading )
    {
        _ = this.WriteImpedanceArray( reading );
        return this.QueryImpedanceArray();
    }

    /// <summary> Writes and reads back the Impedance Array. </summary>
    /// <param name="value"> The Impedance Array. </param>
    /// <returns> The Impedance Array. </returns>
    public IList<double>? ApplyImpedanceArray( IList<double> value )
    {
        _ = this.WriteImpedanceArray( value );
        return this.QueryImpedanceArray();
    }

    /// <summary> Gets or sets the Impedance Array query command. </summary>
    /// <remarks> SCPI: ":SENS{0}:CORR2:ZME:{1}:DATA?". </remarks>
    /// <value> The Impedance Array query command. </value>
    protected virtual string ImpedanceArrayQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Impedance Array. </summary>
    /// <returns> The Impedance Array or none if unknown. </returns>
    public IList<double>? QueryImpedanceArray()
    {
        this.ImpedanceArrayReading = this.Session.QueryTrimEnd( "", string.Format( this.ImpedanceArrayQueryCommand, this.ChannelNumber, this.CompensationTypeCode ) );
        if ( this._impedanceArrayReading is not null )
            this.ImpedanceArray = Parse( this._impedanceArrayReading );
        return this.ImpedanceArray;
    }

    /// <summary> Gets or sets the Impedance Array command format. </summary>
    /// <remarks> SCPI: ":SENS{0}:CORR2:ZME:{1}:DATA {0}". </remarks>
    /// <value> The Impedance Array command format. </value>
    protected virtual string ImpedanceArrayCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Impedance Array without reading back the value from the device. </summary>
    /// <remarks> This command sets the Impedance Array. </remarks>
    /// <param name="reading"> The reading. </param>
    /// <returns> The Impedance Array. </returns>
    public IList<double>? WriteImpedanceArray( string reading )
    {
        if ( string.IsNullOrWhiteSpace( reading ) ) throw new ArgumentNullException( nameof( reading ) );
        this.ImpedanceArrayReading = reading;
        _ = this.Session.WriteLine( this.ImpedanceArrayCommandFormat, this.ChannelNumber, this.CompensationTypeCode, this._impedanceArrayReading! );
        this.ImpedanceArray = Parse( reading );
        return this.ImpedanceArray;
    }

    /// <summary> Writes the Impedance Array without reading back the value from the device. </summary>
    /// <remarks> This command sets the Impedance Array. </remarks>
    /// <param name="values"> The Impedance Array. </param>
    /// <returns> The Impedance Array. </returns>
    public IList<double>? WriteImpedanceArray( IList<double> values )
    {
        return this.WriteImpedanceArray( Build( values ) );
    }

    /// <summary> Writes the Impedance Array without reading back the value from the device. </summary>
    /// <remarks> This command sets the Impedance Array. </remarks>
    /// <param name="includesFrequency">      true if impedance array includes. </param>
    /// <param name="lowFrequencyImpedance">  The low frequency impedance values. </param>
    /// <param name="highFrequencyImpedance"> The high frequency values. </param>
    /// <returns> The Impedance Array. </returns>
    public IList<double>? WriteImpedanceArray( bool includesFrequency, IList<double> lowFrequencyImpedance, IList<double> highFrequencyImpedance )
    {
        return this.WriteImpedanceArray( Merge( includesFrequency, lowFrequencyImpedance, highFrequencyImpedance ) );
    }

    #endregion

    #region " model resistance "

    /// <summary> The Model Resistance. </summary>
    private double? _modelResistance;

    /// <summary>
    /// Gets or sets the cached Model Resistance. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? ModelResistance
    {
        get => this._modelResistance;

        protected set
        {
            if ( !Nullable.Equals( this.ModelResistance, value ) )
            {
                this._modelResistance = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Model Resistance. </summary>
    /// <param name="value"> The Model Resistance. </param>
    /// <returns> The Model Resistance. </returns>
    public double? ApplyModelResistance( double value )
    {
        _ = this.WriteModelResistance( value );
        return this.QueryModelResistance();
    }

    /// <summary> Gets or sets the Model Resistance query command. </summary>
    /// <remarks> SCPI: ":SENS{0}:CORR2:CKIT:{1}:R?". </remarks>
    /// <value> The Model Resistance query command. </value>
    protected virtual string ModelResistanceQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Model Resistance. </summary>
    /// <returns> The Model Resistance or none if unknown. </returns>
    public double? QueryModelResistance()
    {
        this.ModelResistance = this.Session.Query( this.ModelResistance, string.Format( this.ModelResistanceQueryCommand, this.ChannelNumber, this.CompensationTypeCode ) );
        return this.ModelResistance;
    }

    /// <summary> Gets or sets the Model Resistance command format. </summary>
    /// <remarks> SCPI: ":SENS{0}:CORR2:CKIT:{1}:R {0}". </remarks>
    /// <value> The Model Resistance command format. </value>
    protected virtual string ModelResistanceCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Model Resistance without reading back the value from the device.
    /// </summary>
    /// <remarks> This command sets the Model Resistance. </remarks>
    /// <param name="value"> The Model Resistance. </param>
    /// <returns> The Model Resistance. </returns>
    public double? WriteModelResistance( double value )
    {
        this.ModelResistance = this.Session.WriteLine( value, string.Format( this.ModelResistanceCommandFormat, this.ChannelNumber, this.CompensationTypeCode ) );
        return this.ModelResistance;
    }

    #endregion

    #region " model capacitance "

    /// <summary> The Model Capacitance. </summary>
    private double? _modelCapacitance;

    /// <summary>
    /// Gets or sets the cached Model Capacitance. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? ModelCapacitance
    {
        get => this._modelCapacitance;

        protected set
        {
            if ( !Nullable.Equals( this.ModelCapacitance, value ) )
            {
                this._modelCapacitance = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Model Capacitance. </summary>
    /// <param name="value"> The Model Capacitance. </param>
    /// <returns> The Model Capacitance. </returns>
    public double? ApplyModelCapacitance( double value )
    {
        _ = this.WriteModelCapacitance( value );
        return this.QueryModelCapacitance();
    }

    /// <summary> Gets or sets the Model Capacitance query command. </summary>
    /// <remarks> SCPI: ":SENS{0}:CORR2:CKIT:{1}:C?". </remarks>
    /// <value> The Model Capacitance query command. </value>
    protected virtual string ModelCapacitanceQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Model Capacitance. </summary>
    /// <returns> The Model Capacitance or none if unknown. </returns>
    public double? QueryModelCapacitance()
    {
        this.ModelCapacitance = this.Session.Query( this.ModelCapacitance, string.Format( this.ModelCapacitanceQueryCommand, this.ChannelNumber, this.CompensationTypeCode ) );
        return this.ModelCapacitance;
    }

    /// <summary> Gets or sets the Model Capacitance command format. </summary>
    /// <remarks> SCPI: ":SENS{0}:CORR2:CKIT:{1}:C {0}". </remarks>
    /// <value> The Model Capacitance command format. </value>
    protected virtual string ModelCapacitanceCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Model Capacitance without reading back the value from the device.
    /// </summary>
    /// <remarks> This command sets the Model Capacitance. </remarks>
    /// <param name="value"> The Model Capacitance. </param>
    /// <returns> The Model Capacitance. </returns>
    public double? WriteModelCapacitance( double value )
    {
        this.ModelCapacitance = this.Session.WriteLine( value, string.Format( this.ModelCapacitanceCommandFormat, this.ChannelNumber, this.CompensationTypeCode ) );
        return this.ModelCapacitance;
    }

    #endregion

    #region " model conductance "

    /// <summary> The Model Conductance. </summary>
    private double? _modelConductance;

    /// <summary>
    /// Gets or sets the cached Model Conductance. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? ModelConductance
    {
        get => this._modelConductance;

        protected set
        {
            if ( !Nullable.Equals( this.ModelConductance, value ) )
            {
                this._modelConductance = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Model Conductance. </summary>
    /// <param name="value"> The Model Conductance. </param>
    /// <returns> The Model Conductance. </returns>
    public double? ApplyModelConductance( double value )
    {
        _ = this.WriteModelConductance( value );
        return this.QueryModelConductance();
    }

    /// <summary> Gets or sets the Model Conductance query command. </summary>
    /// <remarks> SCPI: ":SENS{0}:CORR2:CKIT:{1}:G?". </remarks>
    /// <value> The Model Conductance query command. </value>
    protected virtual string ModelConductanceQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Model Conductance. </summary>
    /// <returns> The Model Conductance or none if unknown. </returns>
    public double? QueryModelConductance()
    {
        this.ModelConductance = this.Session.Query( this.ModelConductance, string.Format( this.ModelConductanceQueryCommand, this.ChannelNumber, this.CompensationTypeCode ) );
        return this.ModelConductance;
    }

    /// <summary> Gets or sets the Model Conductance command format. </summary>
    /// <remarks> SCPI: ":SENS{0}:CORR2:CKIT:{1}:G {0}". </remarks>
    /// <value> The Model Conductance command format. </value>
    protected virtual string ModelConductanceCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Model Conductance without reading back the value from the device.
    /// </summary>
    /// <remarks> This command sets the Model Conductance. </remarks>
    /// <param name="value"> The Model Conductance. </param>
    /// <returns> The Model Conductance. </returns>
    public double? WriteModelConductance( double value )
    {
        this.ModelConductance = this.Session.WriteLine( value, string.Format( this.ModelConductanceCommandFormat, this.ChannelNumber, this.CompensationTypeCode ) );
        return this.ModelConductance;
    }

    #endregion

    #region " model inductance "

    /// <summary> The Model Inductance. </summary>
    private double? _modelInductance;

    /// <summary>
    /// Gets or sets the cached Model Inductance. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? ModelInductance
    {
        get => this._modelInductance;

        protected set
        {
            if ( !Nullable.Equals( this.ModelInductance, value ) )
            {
                this._modelInductance = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Model Inductance. </summary>
    /// <param name="value"> The Model Inductance. </param>
    /// <returns> The Model Inductance. </returns>
    public double? ApplyModelInductance( double value )
    {
        _ = this.WriteModelInductance( value );
        return this.QueryModelInductance();
    }

    /// <summary> Gets or sets the Model Inductance query command. </summary>
    /// <remarks> SCPI: ":SENS{0}:CORR2:CKIT:{1}:L?". </remarks>
    /// <value> The Model Inductance query command. </value>
    protected virtual string ModelInductanceQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Model Inductance. </summary>
    /// <returns> The Model Inductance or none if unknown. </returns>
    public double? QueryModelInductance()
    {
        this.ModelInductance = this.Session.Query( this.ModelInductance, string.Format( this.ModelInductanceQueryCommand, this.ChannelNumber, this.CompensationTypeCode ) );
        return this.ModelInductance;
    }

    /// <summary> Gets or sets the Model Inductance command format. </summary>
    /// <remarks> SCPI: ":SENS{0}:CORR2:CKIT:{1}:L {0}". </remarks>
    /// <value> The Model Inductance command format. </value>
    protected virtual string ModelInductanceCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Model Inductance without reading back the value from the device.
    /// </summary>
    /// <remarks> This command sets the Model Inductance. </remarks>
    /// <param name="value"> The Model Inductance. </param>
    /// <returns> The Model Inductance. </returns>
    public double? WriteModelInductance( double value )
    {
        this.ModelInductance = this.Session.WriteLine( value, string.Format( this.ModelInductanceCommandFormat, this.ChannelNumber, this.CompensationTypeCode ) );
        return this.ModelInductance;
    }

    #endregion
}
