namespace cc.isr.VI;
/// <summary>
/// Holds and processes an <see cref="ReadingAmount">base class</see> to a single set of
/// instrument readings.
/// </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-11-02 </para>
/// </remarks>
public abstract class ReadingAmounts
{
    #region " construction and cleanup "

    /// <summary> Constructs this class. </summary>
    /// <remarks> Use this constructor to instantiate this class and set its properties. </remarks>
    protected ReadingAmounts() : base()
    {
        this.Readings = [];
        this.RawReading = Empty;
        this.BaseReadings = [];
    }

    /// <summary> Constructs this class. </summary>
    /// <param name="model"> The value. </param>
    protected ReadingAmounts( ReadingAmounts model ) : this()
    {
        if ( model is not null )
        {
            this.Readings = new ReadingEntityCollection( model.Readings );
            this.BaseReadings = new ReadingEntityCollection( model.BaseReadings );
            this.RawReading = model.RawReading;
            this.ActiveReadingType = model.ActiveReadingType;
            this.Elements = model.Elements;
        }
    }

    /// <summary> Makes a deep copy of this object. </summary>
    /// <remarks> David, 2020-07-27. </remarks>
    /// <param name="model"> The value. </param>
    /// <returns> A copy of this object. </returns>
    public abstract ReadingAmounts Clone( ReadingAmounts model );

    #endregion

    #region " parse "

    /// <summary> Gets the default delimiter. </summary>
    public const string DefaultDelimiter = ",";

    /// <summary> Applies the measured data. </summary>
    /// <param name="values">    A record of one or more reading values or an empty string to clear
    /// the current readings. </param>
    /// <param name="delimiter"> The delimiter. </param>
    /// <returns> <c>true</c> if parsed; <c>false</c> otherwise. </returns>
    public virtual bool TryApplyReadings( string values, string delimiter )
    {
        if ( values is null )
        {
            this.RawReading = Empty;
            return false;
        }
        else if ( values.Length == 0 )
        {
            // indicate that we do not have a valid value
            this.Reset();
            this.RawReading = Empty;
            return true;
        }
        else
        {
            this.RawReading = values;
            return this.TryApplyReadings( new Queue<string>( values.Split( delimiter.ToCharArray() ) ) );
        }
    }

    /// <summary> Applies the measured data. </summary>
    /// <param name="values"> A record of one or more readings. </param>
    /// <returns> <c>true</c> if parsed; <c>false</c> otherwise. </returns>
    public virtual bool TryApplyReadings( string values )
    {
        return this.TryApplyReadings( values, DefaultDelimiter );
    }

    /// <summary> Applies the measured data. </summary>
    /// <param name="values"> The reading values. </param>
    /// <returns> <c>true</c> if parsed; <c>false</c> otherwise. </returns>
    public virtual bool TryApplyReadings( string[] values )
    {
        return this.TryApplyReadings( new Queue<string>( values ) );
    }

    /// <summary> Applies  the measured data. </summary>
    /// <param name="values"> Specifies the values. </param>
    /// <returns> <c>true</c> if applies; <c>false</c> otherwise. </returns>
    public virtual bool TryApplyReadings( Queue<string> values )
    {
        bool affirmative = false;
        if ( values is null )
        {
            this.RawReading = Empty;
        }
        else if ( values.Count < this.Readings.Count )
        {
            // if the queue has fewer values than expected, reset to 
            // indicate that the value is invalid
            this.Reset();
            this.RawReading = Empty;
        }
        else
        {
            System.Text.StringBuilder builder = new();
            affirmative = true;
            foreach ( ReadingEntity readingItem in this.Readings )
            {
                string valueReading = values.Dequeue();
                if ( builder.Length > 0 )
                    _ = builder.Append( DefaultDelimiter );
                _ = builder.Append( valueReading );
                if ( readingItem.IncludesUnitsSuffix )
                {
                    string unitsSuffix = VI.Syntax.ScpiSyntax.ParseUnitSuffix( valueReading );
                    valueReading = VI.Syntax.ScpiSyntax.TrimUnits( valueReading, unitsSuffix );
                    affirmative &= readingItem.TryApplyReading( valueReading, unitsSuffix );
                }
                else
                {
                    affirmative &= readingItem.TryApplyReading( valueReading );
                }
            }

            this.RawReading = builder.ToString();
        }

        return affirmative;
    }

    /// <summary>
    /// Parses all <see cref="ReadingAmount">reading elements</see> in a
    /// <see cref="ReadingElementTypes">set of reading amounts</see>.
    /// Use for parsing reading elements that were set before limits were set.
    /// </summary>
    /// <param name="values"> Specifies a reading element. </param>
    /// <returns> <c>true</c> if parsed; <c>false</c> otherwise. </returns>
    public virtual bool TryApplyReadings( ReadingAmounts values )
    {
        if ( values is null )
        {
            this.RawReading = Empty;
            return false;
        }

        // clear all as we start from a fresh slate.
        this.Reset();
        return this.TryApplyReadings( values.Readings.ToRawReadings() );
    }

    /// <summary> Builds meta status. </summary>
    /// <remarks> David, 2020-07-28. </remarks>
    /// <param name="status"> The status. </param>
    /// <returns> The <see cref="MetaStatus"/> <see cref="MetaStatus.StatusValue"/> . </returns>
    protected abstract long BuildMetaStatus( long status );

    /// <summary> Attempts to evaluate using the applied reading and given status. </summary>
    /// <param name="status"> The status. </param>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    public bool TryEvaluate( long status )
    {
        status = this.BuildMetaStatus( status );
        bool affirmative = true;
        foreach ( ReadingEntity readingItem in this.Readings )
            affirmative &= readingItem.TryEvaluate( status );
        return affirmative;
    }

    /// <summary> Resets the measured outcomes. </summary>
    public virtual void Reset()
    {
        this.Readings.Reset();
    }

    /// <summary> Attempts to parse from the given data. </summary>
    /// <remarks>
    /// Parsing takes two steps. First all values are assigned. Then the status is used to evaluate
    /// the measured amounts.
    /// </remarks>
    /// <param name="readings"> The readings. </param>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    public bool TryParse( string readings )
    {
        bool affirmative;
        if ( this.TryApplyReadings( readings ) )
        {
            long statusValue = 0L;
            affirmative = this.TryEvaluate( statusValue );
        }
        else
        {
            affirmative = false;
        }

        return affirmative;
    }

    /// <summary> Attempts to parse from the given data. </summary>
    /// <param name="values"> A queue of reading values. </param>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    public bool TryParse( Queue<string> values )
    {
        bool affirmative;
        if ( this.TryApplyReadings( values ) )
        {
            long statusValue = 0L;
            affirmative = this.TryEvaluate( statusValue );
        }
        else
        {
            affirmative = false;
        }

        return affirmative;
    }

    /// <summary> Enumerates parse many in this collection. </summary>
    /// <remarks> David, 2020-07-27. </remarks>
    /// <exception cref="ArgumentNullException">     Thrown when one or more required arguments are
    /// null. </exception>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <param name="baseReading">    Specifies the base reading which includes the limits for all
    /// reading elements. </param>
    /// <param name="readingRecords"> The reading records. </param>
    /// <returns>
    /// An enumerator that allows foreach to be used to process parse many in this collection.
    /// </returns>
    protected virtual IList<ReadingAmounts> ParseMany( ReadingAmounts baseReading, string? readingRecords )
    {
        List<ReadingAmounts> readingsArray = [];
        if ( readingRecords is null )
        {
            throw new ArgumentNullException( nameof( readingRecords ) );
        }
        else if ( baseReading is null )
        {
            throw new ArgumentNullException( nameof( baseReading ) );
        }
        else if ( baseReading.Readings is null )
        {
            throw new InvalidOperationException( "Base reading readings not defined" );
        }
        else if ( baseReading.Readings.Count == 0 )
        {
            throw new InvalidOperationException( "Base reading has not readings" );
        }
        else if ( readingRecords.Length > 0 )
        {
            Queue<string> values = new( readingRecords.Split( DefaultDelimiter.ToCharArray() ) );
            if ( values.Count >= baseReading.Readings.Count )
            {
                for ( int j = 0, loopTo = (values.Count / baseReading.Readings.Count) - 1; j <= loopTo; j++ )
                {
                    ReadingAmounts reading = this.Clone( baseReading );
                    _ = reading.TryParse( values );
                    readingsArray.Add( reading );
                }
            }
        }

        return [.. readingsArray];
    }

    /// <summary> Parses reading data into a readings array. </summary>
    /// <remarks> David, 2020-09-04. </remarks>
    /// <param name="baseReading">    Specifies the base reading which includes the limits for all
    /// reading elements. </param>
    /// <param name="readingRecords"> The reading records. </param>
    /// <returns>
    /// An enumerator that allows for each to be used to process parse multi in this collection.
    /// </returns>
    public virtual IList<ReadingAmounts> Parse( ReadingAmounts baseReading, string? readingRecords )
    {
        return this.ParseMany( baseReading, readingRecords );
    }

    #endregion

    #region " readings "

    /// <summary> The empty reading string. </summary>
    public const string Empty = "nil";

    /// <summary> Gets the raw reading. </summary>
    /// <value> The raw reading. </value>
    public string RawReading { get; private set; }

    /// <summary> Gets the is empty. </summary>
    /// <value> The is empty. </value>
    public bool IsEmpty => string.IsNullOrWhiteSpace( this.RawReading ) || string.Equals( this.RawReading, Empty, StringComparison.Ordinal );

    /// <summary>
    /// Gets or sets a collection of readings that implements the <see cref="ReadingAmount">reading
    /// base</see>.
    /// </summary>
    /// <value> The readings. </value>
    public ReadingEntityCollection Readings { get; private set; }

    /// <summary> Gets or sets the base readings. </summary>
    /// <value> The base readings. </value>
    public ReadingEntityCollection BaseReadings { get; private set; }

    #endregion

    #region " active reading "

    /// <summary> QueryEnum if this object has reading elements. </summary>
    /// <returns> <c>true</c> if reading elements; otherwise <c>false</c> </returns>
    public bool HasReadingElements()
    {
        return this.Readings.Any();
    }

    /// <summary> Gets or sets the reading elements. </summary>
    /// <value> The elements. </value>
    public ReadingElementTypes Elements { get; private set; }

    /// <summary> Gets or sets the reading type of the active reading entity. </summary>
    /// <value> The active element. </value>
    public ReadingElementTypes ActiveReadingType { get; set; }

    /// <summary> Returns the meta status of the active reading. </summary>
    /// <returns> The MetaStatus. </returns>
    public MetaStatus ActiveMetaStatus()
    {
        return this.Readings.MetaStatus( this.ActiveReadingType );
    }

    /// <summary> Active reading amount. </summary>
    /// <returns> A ReadingAmount. </returns>
    public ReadingAmount ActiveReadingAmount()
    {
        return this.Readings.ReadingAmount( this.ActiveReadingType );
    }

    /// <summary> Active reading unit symbol. </summary>
    /// <returns> A <see cref="string" />. </returns>
    public string ActiveReadingUnitSymbol()
    {
        return this.ActiveReadingAmount() is null ? string.Empty : this.ActiveReadingAmount().Symbol;
    }

    /// <summary> Returns the caption value of the active reading. </summary>
    /// <returns> A <see cref="string" />. </returns>
    public string ActiveAmountCaption()
    {
        ReadingAmount? amount = this.ActiveReadingAmount();
        string result;
        if ( amount is null )
        {
            if ( this.IsEmpty )
            {
                result = "0x------- ";
            }
            else
            {
                ReadingStatus? value = this.Readings.ReadingStatus( this.ActiveReadingType );
                result = value is null ? "-.------- :(" : value.ToString();
            }
        }
        else
        {
            result = this.IsEmpty ? $"-.------- {amount.Symbol}" : amount.ToString();
        }

        return result;
    }

    #endregion

    #region " custom reading elements "

    /// <summary> Adds a reading entity. </summary>
    /// <remarks> David, 2020-07-27. </remarks>
    /// <param name="elementType"> Type of the element. </param>
    private void AddReadingEntity( ReadingElementTypes elementType )
    {
        this.Readings.AddIf( elementType, this.ChannelNumber );
        this.Readings.AddIf( elementType, this.CurrentReading );
        this.Readings.AddIf( elementType, this.Limits );
        this.Readings.AddIf( elementType, this.LimitStatus );
        this.Readings.AddIf( elementType, this.PrimaryReading );
        this.Readings.AddIf( elementType, this.ReadingNumber );
        this.Readings.AddIf( elementType, this.ResistanceReading );
        this.Readings.AddIf( elementType, this.Seconds );
        this.Readings.AddIf( elementType, this.SecondaryReading );
        this.Readings.AddIf( elementType, this.StatusReading );
        this.Readings.AddIf( elementType, this.Timestamp );
        this.Readings.AddIf( elementType, this.VoltageReading );
    }

    /// <summary> Adds a reading entities. </summary>
    /// <remarks> David, 2020-07-28. </remarks>
    /// <param name="elementTypes"> List of types of the elements. </param>
    private void AddReadingEntities( ReadingElementTypes elementTypes )
    {
        foreach ( ReadingElementTypes elementType in this.BaseReadings.ElementTypes )
            this.AddReadingEntity( elementType & elementTypes );
    }

    /// <summary> Initializes this object. </summary>
    /// <remarks>
    /// Adds reading elements in the order they are returned by the instrument so as to automate
    /// parsing of these data.
    /// </remarks>
    /// <param name="value"> The value. </param>
    public void Initialize( ReadingElementTypes value )
    {
        this.Elements = value;
        this.Readings.Clear();
        this.AddReadingEntities( value );
        this.Readings.IncludeUnitSuffixIf( value );
    }

    /// <summary> Gets or sets the <see cref="cc.isr.VI.ReadingAmount">channel number</see>. </summary>
    /// <value> The channel number. </value>
    public ReadingValue? ChannelNumber { get; set; }

    /// <summary>
    /// Gets or sets the source meter <see cref="cc.isr.VI.MeasuredAmount">current reading</see>.
    /// </summary>
    /// <value> The current reading. </value>
    public MeasuredAmount? CurrentReading { get; set; }

    /// <summary> Gets or sets the <see cref="cc.isr.VI.ReadingAmount">limits</see>. </summary>
    /// <value> The limits. </value>
    public ReadingValue? Limits { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="cc.isr.VI.ReadingValue">alarm limits threshold state</see>.
    /// </summary>
    /// <value> The limit status. </value>
    public ReadingValue? LimitStatus { get; set; }

    /// <summary> Gets or sets the <see cref="cc.isr.VI.ReadingAmount">reading number</see>. </summary>
    /// <value> The reading number. </value>
    public ReadingValue? ReadingNumber { get; set; }

    /// <summary> Gets or sets the <see cref="cc.isr.VI.MeasuredAmount">primary reading</see>. </summary>
    /// <value> The primary reading. </value>
    public MeasuredAmount? PrimaryReading { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="cc.isr.VI.MeasuredAmount">Secondary reading</see>.
    /// </summary>
    /// <value> The secondary reading. </value>
    public MeasuredAmount? SecondaryReading { get; set; }

    /// <summary>
    /// Gets or sets the source meter <see cref="cc.isr.VI.MeasuredAmount">resistance reading</see>.
    /// </summary>
    /// <value> The resistance reading. </value>
    public MeasuredAmount? ResistanceReading { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="cc.isr.VI.ReadingValue">time span in seconds</see>.
    /// </summary>
    /// <value> The seconds. </value>
    public ReadingValue? Seconds { get; set; }

    /// <summary>
    /// Gets or sets the source meter <see cref="cc.isr.VI.MeasuredAmount">status reading</see>.
    /// </summary>
    /// <value> The status reading. </value>
    public ReadingStatus? StatusReading { get; set; }

    /// <summary> Gets or sets the timestamp <see cref="cc.isr.VI.ReadingAmount">reading</see>. </summary>
    /// <value> The timestamp. </value>
    public ReadingAmount? Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the source meter <see cref="cc.isr.VI.MeasuredAmount">voltage reading</see>.
    /// </summary>
    /// <value> The voltage reading. </value>
    public MeasuredAmount? VoltageReading { get; set; }

    #endregion
}
