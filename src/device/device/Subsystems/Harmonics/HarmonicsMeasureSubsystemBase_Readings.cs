namespace cc.isr.VI;

public partial class HarmonicsMeasureSubsystemBase
{
    #region " measured amount "

    /// <summary> The last reading. </summary>
    /// <summary> Gets or sets the last reading. </summary>
    /// <value> The last reading. </value>
    public string LastReading
    {
        get;
        set
        {
            if ( !string.Equals( value, this.LastReading, StringComparison.Ordinal ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    } = string.Empty;

    /// <summary> Gets or sets the reading caption. </summary>
    /// <value> The reading caption. </value>
    public string ReadingCaption
    {
        get;
        set
        {
            if ( !string.Equals( value, this.ReadingCaption, StringComparison.Ordinal ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    } = string.Empty;

    /// <summary> Notifies that reading changed. </summary>
    public void NotifyReadingChanged()
    {
        this.NotifyPropertyChanged( nameof( ChannelMarkerSubsystemBase.ReadingCaption ) );
        this.NotifyPropertyChanged( nameof( ChannelMarkerSubsystemBase.LastReading ) );
        this.NotifyFailureInfo();
    }

    #endregion

    #region " reading amounts "

    /// <summary> Parse the active reading. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="readingAmounts"> The readings. </param>
    /// <returns> A Double? </returns>
    protected virtual double? ParseActiveReading( ReadingAmounts readingAmounts )
    {
        if ( readingAmounts is null ) throw new ArgumentNullException( nameof( readingAmounts ) );

        string reading = string.Empty;
        double? value = new double?();
        string caption;
        if ( readingAmounts is null || readingAmounts.ActiveReadingType == ReadingElementTypes.None )
        {
            caption = this.PrimaryReading!.ToString();
        }
        else if ( readingAmounts.IsEmpty )
        {
            caption = readingAmounts.ActiveAmountCaption();
        }
        else
        {
            value = readingAmounts.ActiveReadingAmount().Value;
            caption = readingAmounts.ActiveAmountCaption();
            reading = readingAmounts.ActiveReadingAmount().RawValueReading;
            this.UpdateMetaStatus( readingAmounts.ActiveMetaStatus() );
        }

        _ = string.IsNullOrWhiteSpace( this.FailureLongDescription )
            ? cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.ResourceModelCaption}={caption}" )
            : cc.isr.VI.SessionLogger.Instance.LogInformation( this.FailureLongDescription );

        this.ReadingCaption = caption;
        this.LastReading = reading;
        // this notifies of available reading and must be the last set value
        this.NotifyPropertyChanged( nameof( this.PrimaryReadingValue ) );
        return value;
    }

    /// <summary> Gets the reading amounts. </summary>
    /// <value> The reading amounts. </value>
    public ReadingAmounts ReadingAmounts { get; private set; }

    /// <summary> Assign reading amounts. </summary>
    /// <param name="readingAmounts"> The reading amounts. </param>
    protected void AssignReadingAmounts( ReadingAmounts readingAmounts )
    {
        this.ReadingAmounts = readingAmounts;
    }

    /// <summary> Select active reading. </summary>
    /// <param name="readingType"> Type of the reading. </param>
    public void SelectActiveReading( ReadingElementTypes readingType )
    {
        this.ReadingAmounts.ActiveReadingType = readingType;
        _ = this.ParseActiveReading( this.ReadingAmounts );
    }

    /// <summary> Parses a new set of reading elements. </summary>
    /// <param name="reading"> Specifies the measurement text to parse into the new reading. </param>
    /// <returns> A Double? </returns>
    protected double? ParseReadingAmounts( string reading )
    {
        double? result = new double?();

        // check if we have units suffixes.
        if ( (this.ReadingAmounts.Elements & ReadingElementTypes.Units) != 0 )
            reading = ReadingEntity.TrimUnits( reading );
        if ( this.ReadingAmounts.TryParse( reading ) )
        {
            result = this.ParseActiveReading( this.ReadingAmounts );
        }
        else
        {
            this.PrimaryReading.Value = new double?();
            this.NotifyPropertyChanged( nameof( this.PrimaryReadingValue ) );
        }

        return result;
    }

    /// <summary> Gets the reading element Types. </summary>
    /// <value> The reading element types. </value>
    public ReadingElementTypes ReadingElementTypes => this.ReadingAmounts.Elements;

    #endregion

    #region " primary reading "

    /// <summary> Gets the primary reading. </summary>
    /// <value> The primary reading. </value>
    public MeasuredAmount PrimaryReading => this.ReadingAmounts.PrimaryReading!;

    /// <summary>
    /// Gets the cached Primary Reading Value. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? PrimaryReadingValue => this.PrimaryReading?.Value;

    /// <summary>   Parse primary reading full. </summary>
    /// <remarks>   David, 2021-04-05. </remarks>
    /// <param name="rawReading">   Specifies the measurement text to parse into the new reading. </param>
    /// <returns>   A Tuple: (bool Success, double? ParsedReading, string RawReading) </returns>
    public virtual (bool Success, double? ParsedReading, string RawReading) ParsePrimaryReadingFull( string rawReading )
    {
        bool success = this.TryParse( rawReading, "VM=", ["mV", "uV", "dB"], [0.001, 0.000001, 1], out double reading );
        if ( this.PrimaryReading is not null && success )
        {
            this.PrimaryReading.ReadingCaption = rawReading.TrimEnd( this.Session.TerminationCharacters() );

            if ( this.PrimaryReading.TryApplyReading( reading.ToString() ) )
            {
                this.ReadingCaption = this.PrimaryReading!.ToString();
                if ( string.IsNullOrWhiteSpace( rawReading ) )
                {
                    this.ClearMetaStatus();
                }
                else
                {
                    this.UpdateMetaStatus( this.PrimaryReading.MetaStatus );
                }

                _ = string.IsNullOrWhiteSpace( this.FailureLongDescription )
                    ? cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.ResourceModelCaption}={this.ReadingCaption}" )
                    : cc.isr.VI.SessionLogger.Instance.LogInformation( this.FailureLongDescription );
            }
            else
            {
                this.PrimaryReading.Reset();
                this.ClearMetaStatus();
                this.ReadingCaption = rawReading;
            }
        }
        else
        {
            this.PrimaryReading?.Reset();
            this.ClearMetaStatus();
            this.ReadingCaption = rawReading;
        }

        this.NotifyPropertyChanged( nameof( SenseChannelSubsystemBase.PrimaryReadingValue ) );
        this.LastReading = rawReading;
        return (success, this.PrimaryReadingValue, rawReading);
    }

    /// <summary> Parse primary reading. </summary>
    /// <remarks> David, 2020-08-12. </remarks>
    /// <param name="rawReading"> Specifies the measurement text to parse into the new reading. </param>
    /// <returns> A Double? </returns>
    public virtual double? ParsePrimaryReading( string rawReading )
    {
        return this.ParsePrimaryReadingFull( rawReading ).ParsedReading;
    }

    /// <summary>   parse reading and units. </summary>
    /// <remarks>   David, 2021-03-28. </remarks>
    /// <param name="rawReading">   The raw reading. </param>
    /// <param name="prefix">       The prefix. </param>
    /// <param name="units">        The units. </param>
    /// <param name="scales">       The scales. </param>
    /// <returns> A tuple: (bool Success, string Reading, string Units, double Scale). </returns>
    internal static (bool Success, string Reading, string Units, double Scale) ParseReadingUnit( string rawReading, string prefix, string[] units, double[] scales )
    {
        return ParseReadingUnit( rawReading.TrimStart( prefix.ToCharArray() ), units, scales );
    }

    /// <summary>   parse reading and units. </summary>
    /// <remarks>   David, 2021-04-06. </remarks>
    /// <param name="reading">  The reading. </param>
    /// <param name="units">    The units. </param>
    /// <param name="scales">   The scales. </param>
    /// <returns> A tuple: (bool Success, string Reading, string Units, double Scale). </returns>
    internal static (bool Success, string Reading, string Units, double Scale) ParseReadingUnit( string reading, string[] units, double[] scales )
    {
        int unitIndex = reading.LastIndexOfAny( "0123456789.".ToCharArray() ) + 1;
        string readingValue = reading[..unitIndex];
        string readingUnit = reading[unitIndex..];
#if true
        for ( int i = 0; i < units.Length; i++ )
        {
            if ( string.Equals( readingUnit, units[i], StringComparison.Ordinal ) )
                return (true, readingValue, readingUnit, scales[i]);
        }
#else
        for ( int i = 0; i < units.Length; i++ )
        {
            string unit = units[i];
            if ( reading.EndsWith( unit ) )
                return (true, reading.TrimEnd( units[i].ToCharArray() ), unit, scales[i]);
        }
#endif
        return (false, reading, string.Empty, 0);
    }

    /// <summary>   Try parse. </summary>
    /// <remarks>   David, 2021-03-28. </remarks>
    /// <param name="rawReading">   The raw reading. </param>
    /// <param name="prefix">       The prefix. </param>
    /// <param name="units">        The units. </param>
    /// <param name="scales">       The scales. </param>
    /// <param name="value">        [out] The value. </param>
    /// <returns>   A Tuple. </returns>
    internal bool TryParse( string rawReading, string prefix, string[] units, double[] scales, out double value )
    {
        (bool success, string reading, _, double scale) = ParseReadingUnit( rawReading.TrimEnd( this.Session.TerminationCharacters() ), prefix, units, scales );
        if ( success && Pith.SessionBase.TryParse( reading, out double unscaledValue ) )
        {
            value = unscaledValue * scale;
            return true;
        }
        else
        {
            value = 0;
            return false;
        }
    }

    #endregion

    #region " measure "

    /// <summary> Reads a value into the primary reading and converts it to Double. </summary>
    /// <param name="queryCommand"> The query command. </param>
    /// <returns> The measured value or none if unknown. </returns>
    public virtual double? QueryParsePrimaryReading( string queryCommand )
    {
        return this.ParsePrimaryReading( this.FetchReading( queryCommand ).ReceivedMessage.TrimEnd( this.Session.TerminationCharacters() ) );
    }

    /// <summary> Queries The reading. </summary>
    /// <param name="queryCommand"> The query command. </param>
    /// <returns> The reading or none if unknown. </returns>
    public virtual double? QueryParseReadingAmounts( string queryCommand )
    {
        return this.ParseReadingAmounts( this.FetchReading( queryCommand ).ReceivedMessage.TrimEnd( this.Session.TerminationCharacters() ) );
    }

    #endregion

    #region " fetch reading "

    /// <summary> Fetches a reading with status awareness. </summary>
    /// <param name="queryCommand"> The query command. </param>
    /// <returns> A tuple: QueryInfo. </returns>
    public virtual QueryInfo FetchReading( string queryCommand )
    {
        this.Session.MakeEmulatedReplyIfEmpty( this.ReadingAmounts.PrimaryReading!.Generator.Value.ToString() );
        return this.Session.QueryElapsed( this.Session.EmulatedReply, queryCommand );
    }

    #endregion
}
