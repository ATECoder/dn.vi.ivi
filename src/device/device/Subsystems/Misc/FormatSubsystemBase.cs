// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using cc.isr.Enums;

namespace cc.isr.VI;

/// <summary> Defines the contract that must be implemented by a Format Subsystem. </summary>
/// <remarks>
/// (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2005-01-15, 1.0.1841.x. </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="FormatSubsystemBase" /> class.
/// </remarks>
/// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
/// subsystem</see>. </param>
[CLSCompliant( false )]
public abstract class FormatSubsystemBase( StatusSubsystemBase statusSubsystem ) : SubsystemBase( statusSubsystem )
{
    #region " i presettable "

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    /// <remarks> Clears the queues and sets all registers to zero. </remarks>
    public override void DefineKnownResetState()
    {
        base.DefineKnownResetState();
        this.Elements = ReadingElementTypes.Reading;
    }

    #endregion

    #region " elements "

    /// <summary> The ordered supported elements. </summary>
    private readonly List<ReadingElementTypes> _orderedSupportedElements = [];

    /// <summary> Gets or sets the ordered supported elements. </summary>
    /// <value> The ordered supported elements. </value>
    public IList<ReadingElementTypes> OrderedSupportedElements
    {
        get =>
            // supported elements for the 2002 are: Reading channel, reading number, units, timestamp and status.
            this._orderedSupportedElements;
        set
        {
            if ( !Equals( value, this.OrderedSupportedElements ) )
            {
                this._orderedSupportedElements.Clear();
                this._orderedSupportedElements.AddRange( value );
                this.NotifyPropertyChanged();
            }

            ReadingElementTypes supported = ReadingElementTypes.None;
            foreach ( ReadingElementTypes element in this.OrderedSupportedElements )
                supported |= element;
            this.SupportedElements = supported;
        }
    }

    /// <summary> Gets or sets the supported elements. </summary>
    /// <value> The supported elements. </value>
    public ReadingElementTypes SupportedElements
    {
        get;
        set
        {
            if ( value != this.SupportedElements )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Builds the elements record for the specified reading elements types. </summary>
    /// <param name="readingElementType"> Reading Element types. </param>
    /// <returns> The record. </returns>
    public static string BuildRecord( ReadingElementTypes readingElementType )
    {
        if ( readingElementType == ReadingElementTypes.None )
        {
            return string.Empty;
        }
        else
        {
            System.Text.StringBuilder reply = new();
            foreach ( int code in Enum.GetValues( typeof( ReadingElementTypes ) ) )
            {
                if ( (( int ) readingElementType & code) != 0 )
                {
                    string value = (( ReadingElementTypes ) code).ExtractBetween();
                    if ( !string.IsNullOrWhiteSpace( value ) )
                    {
                        if ( reply.Length > 0 )
                            _ = reply.Append( ',' );
                        _ = reply.Append( value );
                    }
                }
            }

            return reply.ToString();
        }
    }

    /// <summary>
    /// Returns the <see cref="ReadingElementTypes"></see> from the specified value.
    /// </summary>
    /// <param name="value"> The Elements. </param>
    /// <returns> The reading elements. </returns>
    public static ReadingElementTypes ParseReadingElement( string value )
    {
        return string.IsNullOrWhiteSpace( value )
            ? ReadingElementTypes.None
            : Pith.SessionBase.ParseContained<ReadingElementTypes>( value.BuildDelimitedValue() );
    }

    /// <summary>
    /// Get the composite reading elements based on the message from the instrument.
    /// </summary>
    /// <param name="record"> Specifies the comma delimited elements record. </param>
    /// <returns> The reading elements. </returns>
    public static ReadingElementTypes ParseReadingElements( string record )
    {
        ReadingElementTypes parsed = ReadingElementTypes.None;
        if ( !string.IsNullOrWhiteSpace( record ) )
        {
            foreach ( string elementValue in record.Split( ',' ) )
                parsed |= ParseReadingElement( elementValue );
        }

        return parsed;
    }

    /// <summary> Gets or sets the cached Elements. </summary>
    /// <value> A List of scans. </value>
    public ReadingElementTypes Elements
    {
        get;

        protected set
        {
            if ( !this.Elements.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Elements. </summary>
    /// <param name="value"> The Elements. </param>
    /// <returns> A List of scans. </returns>
    public ReadingElementTypes ApplyElements( ReadingElementTypes value )
    {
        _ = this.WriteElements( value );
        return this.QueryElements();
    }

    /// <summary> Gets the supports elements. </summary>
    /// <value> The supports elements. </value>
    public bool SupportsElements => !string.IsNullOrWhiteSpace( this.ElementsQueryCommand );

    /// <summary> Gets or sets the elements query command. </summary>
    /// <remarks> SCPI Base Command: ":FORM:ELEM". </remarks>
    /// <value> The elements query command. </value>
    protected virtual string ElementsQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Elements. Also sets the <see cref="Elements">Format on</see> sentinel.
    /// </summary>
    /// <returns> A List of scans. </returns>
    public ReadingElementTypes QueryElements()
    {
        string record = BuildRecord( this.Elements );
        string? reply = this.Session.QueryTrimEnd( record, this.ElementsQueryCommand );
        this.Elements = reply != null ? ParseReadingElements( reply ) : ReadingElementTypes.None;
        return this.Elements;
    }

    /// <summary>
    /// Queries the Elements for a single element. Also sets the <see cref="Elements">Format on</see>
    /// sentinel.
    /// </summary>
    /// <returns> A List of scans. </returns>
    public ReadingElementTypes QueryElement()
    {
        string record = BuildRecord( this.Elements );
        string? reply = this.Session.QueryTrimEnd( record, this.ElementsQueryCommand );
        this.Elements = reply != null ? ParseReadingElements( reply ) : ReadingElementTypes.None;
        return this.Elements;
    }

    /// <summary> Gets or sets the elements command format. </summary>
    /// <remarks> SCPI Base Command: ":FORM:ELEM {0}". </remarks>
    /// <value> The elements command format. </value>
    protected virtual string ElementsCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Elements for a single element. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> The Elements. </param>
    /// <returns> A List of scans. </returns>
    public ReadingElementTypes WriteElement( ReadingElementTypes value )
    {
        string record = BuildRecord( value );
        _ = this.Session.WriteLine( this.ElementsCommandFormat, record );
        this.Elements = ParseReadingElement( record );
        return this.Elements;
    }

    /// <summary> Writes the Elements. Does not read back from the instrument. </summary>
    /// <param name="value"> The Elements. </param>
    /// <returns> A List of scans. </returns>
    public ReadingElementTypes WriteElements( ReadingElementTypes value )
    {
        value &= this.SupportedElements;
        string record = BuildRecord( value );
        _ = this.Session.WriteLine( this.ElementsCommandFormat, record );
        this.Elements = ParseReadingElements( record );
        return this.Elements;
    }

    #endregion
}
