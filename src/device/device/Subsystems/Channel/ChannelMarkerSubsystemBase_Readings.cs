// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI;

public partial class ChannelMarkerSubsystemBase
{
    #region " reading status "

    /// <summary> The last reading. </summary>
    /// <summary> Gets or sets the last reading. </summary>
    /// <value> The last reading. </value>
    public string? LastReading
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
    }

    /// <summary> Gets or sets the reading caption. </summary>
    /// <value> The reading caption. </value>
    public string? ReadingCaption
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
    }

    /// <summary> Notifies that reading changed. </summary>
    public void NotifyReadingChanged()
    {
        this.NotifyPropertyChanged( nameof( this.ReadingCaption ) );
        this.NotifyPropertyChanged( nameof( this.LastReading ) );
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
        this.NotifyPropertyChanged( nameof( MeasureSubsystemBase.PrimaryReadingValue ) );
        return value;
    }

    /// <summary> Gets the reading amounts. </summary>
    /// <value> The reading amounts. </value>
    public ReadingAmounts ReadingAmounts { get; private set; } = readingAmounts;

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

        return result;
    }

    #endregion

    #region " primary reading "

    /// <summary> Gets the primary reading. </summary>
    /// <value> The primary reading. </value>
    public MeasuredAmount? PrimaryReading => this.ReadingAmounts.PrimaryReading;

    /// <summary>
    /// Gets the cached Primary Reading Value. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? PrimaryReadingValue => this.PrimaryReading?.Value;

    /// <summary> Parse primary reading. </summary>
    /// <remarks> David, 2020-08-12. </remarks>
    /// <param name="reading"> Specifies the measurement text to parse into the new reading. </param>
    /// <returns> A Double? </returns>
    public virtual double? ParsePrimaryReading( string reading )
    {
        if ( this.PrimaryReading is not null && this.PrimaryReading.TryApplyReading( reading ) )
        {
            this.ReadingCaption = this.PrimaryReading!.ToString();
            if ( string.IsNullOrWhiteSpace( reading ) )
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
            this.ClearMetaStatus();
            this.ReadingCaption = string.Empty;
        }

        this.NotifyPropertyChanged( nameof( SenseChannelSubsystemBase.PrimaryReadingValue ) );
        this.LastReading = reading;
        return this.PrimaryReadingValue;
    }

    #endregion

    #region " measure "

    /// <summary> Reads a value into the primary reading and converts it to Double. </summary>
    /// <param name="queryCommand"> The query command. </param>
    /// <returns> The measured value or none if unknown. </returns>
    public virtual double? MeasurePrimaryReading( string queryCommand )
    {
        return this.ParsePrimaryReading( this.Session.QueryTrimEnd( this.Session.EmulatedReply, queryCommand ) ?? this.Session.EmulatedReply );
    }

    /// <summary> Queries the reading into the reading amounts. </summary>
    /// <param name="queryCommand"> The query command. </param>
    /// <returns> The reading or none if unknown. </returns>
    public virtual double? MeasureReadingAmounts( string queryCommand )
    {
        return this.ParseReadingAmounts( this.Session.QueryTrimEnd( this.Session.EmulatedReply, queryCommand ) ?? this.Session.EmulatedReply );
    }

    #endregion
}
