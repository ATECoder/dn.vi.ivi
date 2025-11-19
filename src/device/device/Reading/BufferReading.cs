// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using System.ComponentModel;

namespace cc.isr.VI;

/// <summary> A buffer reading. </summary>
/// <remarks>
/// (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2016-07-23 </para>
/// </remarks>
public class BufferReading
{
    #region " construction "

    /// <summary> Constructor. </summary>
    public BufferReading() : base()
    {
        this.ClearThis();
        this.BuildTimestamp = DateTimeOffset.Now;
    }

    /// <summary> Constructor. </summary>
    /// <param name="data">                       The value. </param>
    /// <param name="orderedReadingElementTypes"> List of types of the reading elements. </param>
    public BufferReading( Queue<string> data, IEnumerable<ReadingElementTypes> orderedReadingElementTypes ) : this()
    {
        this.OrderedReadingElementTypes = orderedReadingElementTypes;
        this.ReadingElementTypes = JoinReadingElementTypes( orderedReadingElementTypes ) ?? ReadingElementTypes.None;
        this.ParseThis( data, orderedReadingElementTypes );
    }

    /// <summary> Constructor. </summary>
    /// <param name="measuredElementType">        The type of the measured element. </param>
    /// <param name="data">                       The value. </param>
    /// <param name="orderedReadingElementTypes"> The ordered list of types of the readings. </param>
    public BufferReading( ReadingElementTypes measuredElementType, Queue<string> data, IEnumerable<ReadingElementTypes> orderedReadingElementTypes )
        : this( data, orderedReadingElementTypes ) => this.MeasuredElementType = measuredElementType;

    /// <summary> Constructor. </summary>
    /// <param name="data">                       The value. </param>
    /// <param name="firstReading">               The first buffer reading. </param>
    /// <param name="orderedReadingElementTypes"> The ordered list of types of the readings. </param>
    public BufferReading( Queue<string> data, BufferReading firstReading, IEnumerable<ReadingElementTypes> orderedReadingElementTypes ) : this()
    {
        this.OrderedReadingElementTypes = orderedReadingElementTypes;
        this.ReadingElementTypes = JoinReadingElementTypes( this.OrderedReadingElementTypes ) ?? ReadingElementTypes.None;
        this.ParseThis( data, firstReading );
    }

    /// <summary>   Constructor. </summary>
    /// <remarks>   David, 2021-05-22. </remarks>
    /// <param name="measuredAmount">   The measured amount. </param>
    public BufferReading( MeasuredAmount measuredAmount ) : this()
    {
        this.OrderedReadingElementTypes = [measuredAmount.ReadingType];
        this.ReadingElementTypes = measuredAmount.ReadingType;
        this.Reading = measuredAmount.RawValueReading;
        this.Value = measuredAmount.Amount.Value;
        this.StatusReading = measuredAmount.MetaStatus.StatusValue.ToString();
        // change status word to long
        this.StatusWord = ( int ) (measuredAmount.MetaStatus.StatusValue & int.MaxValue);
        this.TimestampReading = string.Empty;
        this.Timestamp = DateTimeOffset.UtcNow;
        this.BuildTimestamp = this.Timestamp;
        this.FractionalSecond = 0;
        this.FractionalTimespan = TimeSpan.Zero;
        this.RelativeTimespan = TimeSpan.Zero;
        this.UnitReading = measuredAmount.Amount.Unit.ToString();
        this.Amount = new cc.isr.UnitsAmounts.Amount( measuredAmount.Amount );
    }

    /// <summary> Constructor. </summary>
    /// <param name="reading"> The reading. </param>
    public BufferReading( BufferReading reading ) : this()
    {
        if ( reading is not null )
        {
            this.OrderedReadingElementTypes = reading.OrderedReadingElementTypes;
            this.ReadingElementTypes = reading.ReadingElementTypes;
            this.Reading = reading.Reading;
            this.Value = reading.Value;
            this.StatusReading = reading.StatusReading;
            this.StatusWord = reading.StatusWord;
            this.TimestampReading = reading.TimestampReading;
            this.Timestamp = reading.Timestamp;
            this.BuildTimestamp = reading.BuildTimestamp;
            this.FractionalSecond = reading.FractionalSecond;
            this.FractionalTimespan = reading.FractionalTimespan;
            this.RelativeTimespan = reading.RelativeTimespan;
            this.UnitReading = reading.UnitReading;
            if ( reading.Amount is null )
            {
                if ( !string.IsNullOrWhiteSpace( this.UnitReading ) )
                {
                    this.Amount = new UnitsAmounts.Amount( reading.Value, reading.UnitReading );
                }
            }
            else
            {
                this.Amount = new cc.isr.UnitsAmounts.Amount( reading.Amount );
            }
        }
    }

    /// <summary> Validated the given buffer reading. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="bufferReading"> The buffer reading. </param>
    /// <returns> A BufferReading. </returns>
    public static BufferReading Validated( BufferReading bufferReading )
    {
        return bufferReading is null ? throw new ArgumentNullException( nameof( bufferReading ) ) : bufferReading;
    }

    /// <summary> Clears this object to its blank/initial state. </summary>
    private void ClearThis()
    {
        this.Reading = string.Empty;
        this.StatusReading = string.Empty;
        this.StatusWord = 0;
        this.TimestampReading = string.Empty;
        this.Timestamp = DateTimeOffset.MinValue;
        this.BuildTimestamp = DateTimeOffset.MinValue;
        this.FractionalSecond = 0d;
        this.FractionalTimespan = TimeSpan.Zero;
        this.RelativeTimespan = TimeSpan.Zero;
        this.UnitReading = string.Empty;
        this.Amount = new();
    }

    /// <summary> Gets a list of types of the readings. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> A list of types of the readings. </value>
    public IEnumerable<ReadingElementTypes>? OrderedReadingElementTypes { get; private set; }

    /// <summary> Join reading element types. </summary>
    /// <param name="values"> The values. </param>
    /// <returns> The Reading Element Types. </returns>
    private static ReadingElementTypes? JoinReadingElementTypes( IEnumerable<ReadingElementTypes> values )
    {
        ReadingElementTypes result = default;
        foreach ( ReadingElementTypes v in values )
            result |= v;
        return result;
    }

    /// <summary> Gets the type of the measured element. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The type of the measured element. </value>
    public ReadingElementTypes MeasuredElementType { get; set; } = ReadingElementTypes.None;

    /// <summary> Gets a list of types of the reading elements. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> A list of types of the reading elements. </value>
    public ReadingElementTypes ReadingElementTypes { get; private set; } = ReadingElementTypes.None;

    /// <summary> Gets the reading. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The reading. </value>
    public string? Reading { get; private set; }

    /// <summary> Gets the timestamp reading. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The timestamp reading. </value>
    public string? TimestampReading { get; private set; }

    /// <summary> Parses the given value. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="data">                       The value. </param>
    /// <param name="orderedReadingElementTypes"> The ordered list of types of the readings elements. </param>
    private void ParseThis( Queue<string> data, IEnumerable<ReadingElementTypes> orderedReadingElementTypes )
    {
        if ( data is null ) throw new ArgumentNullException( nameof( data ) );
        this.ClearThis();
        this.BuildTimestamp = DateTimeOffset.UtcNow;
        foreach ( ReadingElementTypes readingElementType in orderedReadingElementTypes )
        {
            switch ( readingElementType )
            {
                case ReadingElementTypes.Reading:
                    {
                        if ( data.Any() )
                        {
                            this.Reading = data.Dequeue();
                        }

                        break;
                    }

                case ReadingElementTypes.Channel:
                    {
                        if ( data.Any() )
                        {
                            this.ParseChannel( data.Dequeue() );
                        }

                        break;
                    }

                case ReadingElementTypes.Status:
                    {
                        if ( data.Any() )
                        {
                            this.ParseStatus( data.Dequeue() );
                        }

                        break;
                    }

                case ReadingElementTypes.Timestamp:
                    {
                        if ( data.Any() )
                        {
                            this.ParseTimestamp( data.Dequeue() );
                        }

                        break;
                    }

                case ReadingElementTypes.Time:
                    {
                        if ( data.Any() )
                        {
                            this.ParseTimestamp( data.Dequeue() );
                        }

                        break;
                    }

                case ReadingElementTypes.Units:
                    {
                        if ( data.Any() )
                        {
                            this.BuildAmount( data.Dequeue() );
                        }

                        break;
                    }

                case ReadingElementTypes.None:
                    break;
                case ReadingElementTypes.ReadingNumber:
                    break;
                case ReadingElementTypes.Source:
                    break;
                case ReadingElementTypes.Compliance:
                    break;
                case ReadingElementTypes.AverageVoltage:
                    break;
                case ReadingElementTypes.Voltage:
                    break;
                case ReadingElementTypes.Current:
                    break;
                case ReadingElementTypes.Resistance:
                    break;
                case ReadingElementTypes.Limits:
                    break;
                case ReadingElementTypes.Seconds:
                    break;
                case ReadingElementTypes.Primary:
                    break;
                case ReadingElementTypes.Secondary:
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary> Parses the given value. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="data">         The value. </param>
    /// <param name="firstReading"> The first buffer reading. </param>
    private void ParseThis( Queue<string> data, BufferReading firstReading )
    {
        if ( data is null ) throw new ArgumentNullException( nameof( data ) );
        if ( firstReading is null ) throw new ArgumentNullException( nameof( firstReading ) );
        if ( firstReading.OrderedReadingElementTypes is null ) throw new ArgumentNullException( $"{nameof( firstReading )}.{nameof( BufferReading.OrderedReadingElementTypes )}" );

        this.ParseThis( data, firstReading.OrderedReadingElementTypes );

        if ( !string.IsNullOrWhiteSpace( this.TimestampReading ) )
            this.AdjustRelativeTimespanThis( firstReading );
    }

    #endregion

    #region " status "

    /// <summary> Gets the status reading. </summary>
    /// <value> The status reading. </value>
    public string StatusReading { get; private set; } = string.Empty;

    /// <summary> Gets the has status. </summary>
    /// <value> The has status. </value>
    public bool HasStatus => !string.IsNullOrWhiteSpace( this.StatusReading );

    /// <summary> Gets the status word. </summary>
    /// <value> The status word. </value>
    public int StatusWord { get; private set; }

    /// <summary> Parse status. </summary>
    /// <param name="reading"> The reading. </param>
    public void ParseStatus( string reading )
    {
        this.StatusReading = reading;
        if ( string.IsNullOrWhiteSpace( reading ) )
        {
            this.StatusWord = 0;
        }
        else
        {
            double value = double.Parse( reading, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowExponent );
            this.StatusWord = ( int ) value;
        }
    }

    /// <summary> Applies the status described by value. </summary>
    /// <param name="value"> The value. </param>
    public void ApplyStatus( int value )
    {
        this.StatusReading = value.ToString();
        this.StatusWord = value;
    }

    #endregion

    #region " channel "

    /// <summary> Gets the has channel reading. </summary>
    /// <value> The has channel reading. </value>
    public bool HasChannelReading => !string.IsNullOrWhiteSpace( this.ChannelReading );

    /// <summary> Gets the Channel reading. </summary>
    /// <value> The Channel reading. </value>
    public string ChannelReading { get; private set; } = string.Empty;

    /// <summary> Gets the Channel number. </summary>
    /// <value> The Channel number. </value>
    public int ChannelNumber { get; private set; }

    /// <summary> Parse Channel. </summary>
    /// <param name="reading"> The reading. </param>
    private void ParseChannel( string reading )
    {
        this.ChannelReading = reading;
        double value = double.Parse( reading, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowExponent );
        this.ChannelNumber = ( int ) value;
    }

    #endregion

    #region " amount "

    /// <summary>
    /// Gets the indication that the buffer reading has a reading.  The buffer reading could consist
    /// of status values alone.
    /// </summary>
    /// <value> The has reading. </value>
    public bool HasReading => !string.IsNullOrWhiteSpace( this.Reading );

    /// <summary> Gets the reading caption. </summary>
    /// <value> The reading caption. </value>
    public string? ReadingCaption => this.Amount is null
        ? string.IsNullOrWhiteSpace( this.UnitReading ) ? $"-.---- " : $"-.---- {this.UnitReading}"
        : $"{this.Amount} {this.Amount.Unit}";

    /// <summary> Gets the unit reading. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The unit reading. </value>
    public string UnitReading { get; private set; } = string.Empty;

    /// <summary> The amount. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The amount. </value>
    public cc.isr.UnitsAmounts.Amount Amount { get; private set; } = new();

    /// <summary> Gets the value. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The value. </value>
    public double Value { get; private set; }

    /// <summary>   Builds an amount. </summary>
    /// <remarks>   2024-07-06. </remarks>
    public void BuildAmount()
    {
        if ( this.HasReading )
        {
            if ( !double.TryParse( this.Reading, out double v ) )
            {
                v = double.NaN;
            }
            this.Value = v;
        }
        else
        {
            this.Value = 0d;
        }

        this.Amount = VI.Syntax.ScpiSyntax.BuildAmount( this.Value, this.UnitReading );
    }

    /// <summary>   Builds an amount. </summary>
    /// <remarks>   2024-07-06. </remarks>
    /// <param name="reading">  The reading. </param>
    public void BuildAmount( double reading )
    {
        this.Reading = reading.ToString();
        if ( this.HasReading )
        {
            if ( !double.TryParse( this.Reading, out double v ) )
            {
                v = double.NaN;
            }
            this.Value = v;
        }
        else
        {
            this.Value = 0d;
        }

        this.Amount = VI.Syntax.ScpiSyntax.BuildAmount( this.Value, this.UnitReading );
    }

    /// <summary> Builds an amount. </summary>
    /// <param name="unit"> The reading. </param>
    public void BuildAmount( string unit )
    {
        this.UnitReading = unit;
        if ( this.HasReading )
        {
            if ( !double.TryParse( this.Reading, out double v ) )
            {
                v = double.NaN;
            }
            this.Value = v;
        }
        else
        {
            this.Value = 0d;
        }

        this.Amount = VI.Syntax.ScpiSyntax.BuildAmount( this.Value, this.UnitReading );
    }

    /// <summary> Builds an amount. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="unit"> The reading. </param>
    public void BuildAmount( cc.isr.UnitsAmounts.Unit unit )
    {
        if ( unit is null ) throw new ArgumentNullException( nameof( unit ) );
        this.UnitReading = unit.ToString();
        if ( this.HasReading )
        {
            if ( !double.TryParse( this.Reading, out double v ) )
            {
                v = double.NaN;
            }
            this.Value = v;
        }
        else
        {
            this.Value = 0d;
        }

        this.Amount = new cc.isr.UnitsAmounts.Amount( this.Value, unit );
    }

    #endregion

    #region " timestamp "

    /// <summary> Gets the has timestamp. </summary>
    /// <value> The has timestamp. </value>
    public bool HasTimestamp => !string.IsNullOrWhiteSpace( this.TimestampReading );

    /// <summary> Parse timestamp. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="timestamp"> The time stamp rounded down to the second. </param>
    private void ParseTimestamp( string timestamp )
    {
        if ( string.IsNullOrWhiteSpace( timestamp ) ) throw new ArgumentNullException( nameof( timestamp ) );
        this.TimestampReading = timestamp;
        Queue<string> q = new( timestamp.Split( '.' ) );
        this.Timestamp = DateTimeOffset.Parse( q.Dequeue() );
        this.FractionalSecond = double.Parse( $".{q.Dequeue()}" );
        this.FractionalTimespan = TimeSpan.FromTicks( ( long ) (TimeSpan.TicksPerSecond * this.FractionalSecond) );
        this.MeterTime = this.Timestamp.Add( this.FractionalTimespan );
    }

    /// <summary> Parse timestamp. </summary>
    /// <param name="firstTimestamp">  The first time stamp. </param>
    /// <param name="elapsedTimespan"> The elapsed timespan. </param>
    public void ParseTimestamp( DateTimeOffset firstTimestamp, TimeSpan elapsedTimespan )
    {
        this.BuildTimestamp = firstTimestamp.Add( elapsedTimespan );
        this.Timestamp = this.BuildTimestamp;
        this.TimestampReading = this.Timestamp.ToString( "O" );
        this.FractionalSecond = elapsedTimespan.Ticks / ( double ) TimeSpan.TicksPerSecond;
        this.FractionalTimespan = elapsedTimespan;
        this.MeterTime = this.Timestamp;
    }

    /// <summary> Gets or sets the local date time offset when the reading was constructed. </summary>
    /// <value> The local date time offset  when the reading was constructed. </value>
    public DateTimeOffset BuildTimestamp { get; private set; }

    /// <summary> Gets or sets the meter time. </summary>
    /// <value> The meter time. </value>
    public DateTimeOffset MeterTime { get; private set; }

    /// <summary> Gets or sets the time stamp rounded down to the second. </summary>
    /// <value> The time stamp rounded down to the second. </value>
    public DateTimeOffset Timestamp { get; private set; }

    /// <summary> Gets or sets the fractional second. </summary>
    /// <value> The fractional second. </value>
    public double FractionalSecond { get; private set; }

    /// <summary> Gets or sets the fractional timestamp. </summary>
    /// <remarks> Converted from the fractional second of the instrument timestamp f. </remarks>
    /// <value> The fractional timestamp. </value>
    public TimeSpan FractionalTimespan { get; private set; }

    /// <summary> Gets or sets the timespan relative to the first reading. </summary>
    /// <value> The relative timespan. </value>
    public TimeSpan RelativeTimespan { get; private set; }

    /// <summary> Parses the given value. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="data">         The value. </param>
    /// <param name="firstReading"> The first buffer reading. </param>
    public void Parse( Queue<string> data, BufferReading firstReading )
    {
        if ( data is null ) throw new ArgumentNullException( nameof( data ) );
        if ( firstReading is null ) throw new ArgumentNullException( nameof( firstReading ) );
        if ( firstReading.OrderedReadingElementTypes is null ) throw new ArgumentNullException( $"{nameof( firstReading )}.{nameof( BufferReading.OrderedReadingElementTypes )}" );
        this.ParseThis( data, firstReading.OrderedReadingElementTypes );
        this.AdjustRelativeTimespanThis( firstReading );
    }

    /// <summary> Adjust relative timespan. </summary>
    /// <param name="firstReading"> The first buffer reading. </param>
    private void AdjustRelativeTimespanThis( BufferReading firstReading )
    {
        this.RelativeTimespan = firstReading is null
            ? TimeSpan.Zero
            : this.Timestamp.Subtract( firstReading.Timestamp ).Add( this.FractionalTimespan ).Subtract( firstReading.FractionalTimespan );
    }

    /// <summary> Adjust relative timespan. </summary>
    /// <param name="firstReading"> The first buffer reading. </param>
    public void AdjustRelativeTimespan( BufferReading firstReading )
    {
        this.AdjustRelativeTimespanThis( firstReading );
    }

    #endregion
}
/// <summary> A buffer readings binding list. </summary>
/// <remarks>
/// (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2016-07-23 </para>
/// </remarks>
public class BufferReadingBindingList : VI.Primitives.BindingLists.InvokingBindingList<BufferReading>
{
    #region " construction "

    /// <summary> Default constructor. </summary>
    /// <remarks> David, 2020-07-20. </remarks>
    public BufferReadingBindingList() : base() { }

    /// <summary> Constructor. </summary>
    /// <param name="synchronizer"> Provides a way to synchronously or asynchronously execute a
    /// delegate. </param>
    public BufferReadingBindingList( ISynchronizeInvoke synchronizer ) : base( synchronizer ) { }

    #endregion

    #region " add "

    /// <summary> Parses the reading and adds values to the collection. </summary>
    /// <remarks> David, 2020-07-20. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="data">             The data. </param>
    /// <param name="referenceReading"> The reference reading, which is required for parsing the
    /// data. </param>
    public void Add( string data, BufferReading referenceReading )
    {
        if ( string.IsNullOrWhiteSpace( data ) ) throw new ArgumentNullException( nameof( data ) );
        bool raiseListChangedEventsWasEnabled = this.RaiseListChangedEvents;
        try
        {
            Queue<string> q = new( data.Split( ',' ) );
            this.RaiseListChangedEvents = false;
            while ( q.Any() )
                this.Add( q, referenceReading );
            this.RaiseListChangedEvents = true;
            this.ResetBindings();
        }
        catch
        {
            throw;
        }
        finally
        {
            this.RaiseListChangedEvents = raiseListChangedEventsWasEnabled;
        }
    }

    /// <summary> Parses the reading and adds values to the collection. </summary>
    /// <remarks> David, 2020-07-20. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="readingTimestampQueue"> The reading plus timestamp pair of values to add. </param>
    /// <param name="referenceReading">      The reference reading, which is required for parsing the
    /// data. </param>
    public void Add( Queue<string> readingTimestampQueue, BufferReading referenceReading )
    {
        if ( readingTimestampQueue is null ) throw new ArgumentNullException( nameof( readingTimestampQueue ) );
        this.Add( new BufferReading( readingTimestampQueue, referenceReading,
                    [ReadingElementTypes.Reading, ReadingElementTypes.Timestamp] ) );
    }

    /// <summary>
    /// Adds an item to the <see cref="System.Collections.ICollection" />
    /// This does not notify of list changes.
    /// </summary>
    /// <remarks> David, 2020-07-20. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="item"> The object to be added to the end of the
    /// <see cref="System.Collections.ICollection" />. The value
    /// can be <see langword="null" /> for reference types. </param>
    public new void Add( BufferReading item )
    {
        if ( item is null ) throw new ArgumentNullException( nameof( item ) );
        if ( !this.Any() )
            this.FirstReading = item;
        this.LastReading = item;
        base.Add( item );
    }

    /// <summary> Adds and notify of binding change. </summary>
    /// <remarks> David, 2020-07-20. </remarks>
    /// <param name="item"> The object to be added to the end of the
    /// <see cref="System.Collections.ICollection" />. The value
    /// can be <see langword="null" /> for reference types. </param>
    public void AddAndResetBinding( BufferReading item )
    {
        bool raiseListChangedEventsWasEnabled = this.RaiseListChangedEvents;
        try
        {
            this.RaiseListChangedEvents = false;
            this.Add( item );
            this.RaiseListChangedEvents = true;
            this.ResetBindings();
        }
        catch
        {
            throw;
        }
        finally
        {
            this.RaiseListChangedEvents = raiseListChangedEventsWasEnabled;
        }
    }

    /// <summary> Parses the reading and adds values to the collection. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="values"> The values to add. </param>
    public void Add( BufferReading[] values )
    {
        if ( values is null ) throw new ArgumentNullException( nameof( values ) );
        bool raiseListChangedEventsWasEnabled = this.RaiseListChangedEvents;
        try
        {
            this.RaiseListChangedEvents = false;
            foreach ( BufferReading br in values )
                this.Add( new BufferReading( br ) );
            this.RaiseListChangedEvents = true;
            this.ResetBindings();
        }
        catch
        {
            throw;
        }
        finally
        {
            this.RaiseListChangedEvents = raiseListChangedEventsWasEnabled;
        }
    }

    /// <summary>
    /// Removes all elements from the <see cref="System.Collections.ICollection" />.
    /// </summary>
    /// <remarks> David, 2020-07-20. </remarks>
    public new void Clear()
    {
        bool raiseListChangedEventsWasEnabled = this.RaiseListChangedEvents;
        try
        {
            this.RaiseListChangedEvents = false;
            base.Clear();
            this.FirstReading = new BufferReading();
            this.LastReading = new BufferReading();
            this.RaiseListChangedEvents = true;
            this.ResetBindings();
        }
        catch
        {
            throw;
        }
        finally
        {
            this.RaiseListChangedEvents = raiseListChangedEventsWasEnabled;
        }
    }

    #endregion

    #region " first and last values "

    /// <summary> Gets or sets the first reading. </summary>
    /// <value> The first reading. </value>
    public BufferReading FirstReading { get; private set; } = new BufferReading();

    /// <summary> Gets or sets the last reading. </summary>
    /// <value> The last reading. </value>
    public BufferReading LastReading { get; private set; } = new BufferReading();

    #endregion
}
