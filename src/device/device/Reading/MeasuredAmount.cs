namespace cc.isr.VI;

/// <summary> Implements a measured value amount. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-11-01 </para>
/// </remarks>
public class MeasuredAmount : ReadingAmount
{
    #region " construction and cleanup "

    /// <summary>
    /// Constructs a measured value without specifying the value or its validity, which must be
    /// specified for the value to be made valid.
    /// </summary>
    /// <param name="readingType"> Type of the reading. </param>
    /// <param name="unit">        The unit. </param>
    public MeasuredAmount( ReadingElementTypes readingType, cc.isr.UnitsAmounts.Unit unit ) : base( readingType, unit )
    {
        this.MetaStatus = new MetaStatus();
        this.ComplianceLimitMargin = 0.001d;
        this.HighLimit = VI.Syntax.ScpiSyntax.Infinity;
        this.LowLimit = VI.Syntax.ScpiSyntax.NegativeInfinity;
        this.ComplianceLimit = 0d;
        this.InfinityUncertainty = 1d;
    }

    /// <summary> Constructs a copy of an existing value. </summary>
    /// <param name="model"> The model. </param>
    public MeasuredAmount( MeasuredAmount model ) : base( model )
    {
        if ( model is not null )
        {
            this.HighLimit = model.HighLimit;
            this.LowLimit = model.LowLimit;
            this.MetaStatus = new MetaStatus( model.MetaStatus );
            this.ComplianceLimit = model.ComplianceLimit;
            this.ComplianceLimitMargin = model.ComplianceLimitMargin;
        }
        this.MetaStatus = new MetaStatus();
    }

    /// <summary> Constructs a copy of an existing value. </summary>
    /// <param name="model"> The model. </param>
    public MeasuredAmount( BufferReading model ) : base( BufferReading.Validated( model ).MeasuredElementType, model.Amount.Unit )
    {
        this.Value = model.HasReading ? model.Value : new double?();
        this.MetaStatus = new MetaStatus();
    }

    #endregion

    #region " amount "

    /// <summary> Resets measured value to nothing. </summary>
    public override void Reset()
    {
        base.Reset();
        this.MetaStatus.Reset();
        this.Generator.Min = this.LowLimit;
        this.Generator.Max = this.HighLimit;
    }

    /// <summary> Returns the default string representation of the value. </summary>
    /// <param name="format"> The format string. </param>
    /// <returns> A <see cref="string" /> that represents this object. </returns>
    public string ToString( string format )
    {
        return this.MetaStatus.IsValid ? this.Amount.ToString( format ) : this.MetaStatus.ToShortDescription( "p" );
    }

    #endregion

    #region " meta status "

    /// <summary>
    /// Gets the infinity uncertainty. Allows the value to be close infinity for triggering the
    /// infinity status.
    /// </summary>
    /// <value> The infinity uncertainty. </value>
    public double InfinityUncertainty { get; set; }

    /// <summary> Gets the measured value meta status. </summary>
    /// <value> The measured value status. </value>
    public MetaStatus MetaStatus { get; set; }

    /// <summary> Attempts to evaluate using the applied reading and given status. </summary>
    /// <param name="reading"> The reading. </param>
    /// <returns> <c>true</c> if evaluated. </returns>
    public override bool TryEvaluate( double reading )
    {
        _ = base.TryEvaluate( reading );
        this.MetaStatus.HasValue = true;
        this.MetaStatus.Infinity = Math.Abs( reading - VI.Syntax.ScpiSyntax.Infinity ) < this.InfinityUncertainty;
        if ( this.MetaStatus.Infinity )
            return this.MetaStatus.IsValid;
        this.MetaStatus.NegativeInfinity = Math.Abs( reading - VI.Syntax.ScpiSyntax.NegativeInfinity ) < this.InfinityUncertainty;
        if ( this.MetaStatus.NegativeInfinity )
            return this.MetaStatus.IsValid;
        this.MetaStatus.NotANumber = Math.Abs( reading - VI.Syntax.ScpiSyntax.NotANumber ) < this.InfinityUncertainty;
        if ( this.MetaStatus.NotANumber )
            return this.MetaStatus.IsValid;
        this.MetaStatus.HitLevelCompliance = !(reading >= this.ComplianceLimitLevel) ^ this.ComplianceLimit > 0d;
        if ( this.MetaStatus.HitLevelCompliance )
            return this.MetaStatus.IsValid;
        if ( this.Value is not null )
        {
            this.MetaStatus.IsHigh = this.Value.Value.CompareTo( this.HighLimit ) > 0;
            this.MetaStatus.IsLow = this.Value.Value.CompareTo( this.LowLimit ) < 0;
        }
        return this.MetaStatus.IsValid;
    }

    /// <summary> Attempts to evaluate using the applied reading and given status. </summary>
    /// <param name="status"> The status. </param>
    /// <returns> <c>true</c> if evaluated. </returns>
    public override bool TryEvaluate( long status )
    {
        // update the status to preserve the validity state.
        this.MetaStatus.Preset( this.MetaStatus.StatusValue | status );
        if ( this.MetaStatus.IsValid && this.Value is not null )
            _ = this.TryEvaluate( this.Value.Value );
        return this.MetaStatus.IsValid;
    }

    #endregion

    #region " status "

    /// <summary> Gets the high limit. </summary>
    /// <value> The high limit. </value>
    public double HighLimit { get; set; }

    /// <summary> Gets the low limit. </summary>
    /// <value> The low limit. </value>
    public double LowLimit { get; set; }

    /// <summary>
    /// Gets the compliance limit for testing if the reading exceeded the compliance level.
    /// </summary>
    /// <value> A <see cref="double">Double</see> value. </value>
    public double ComplianceLimit { get; set; }

    /// <summary>
    /// Gets the margin of how close will allow the measured value to the compliance limit.  For
    /// instance, if the margin is 0.001, the measured value must not exceed 99.9% of the compliance
    /// limit. The default is 0.001.
    /// </summary>
    /// <value> A <see cref="double">Double</see> value. </value>
    public double ComplianceLimitMargin { get; set; }

    /// <summary>
    /// Gets the compliance limit deviation. The factor by which to reduce the compliance limit for
    /// checking compliance.
    /// </summary>
    /// <value> The compliance limit deviation. </value>
    public double ComplianceLimitDeviation { get; set; }

    /// <summary> Gets the compliance limit level. </summary>
    /// <value> The compliance limit level. </value>
    public double ComplianceLimitLevel => this.ComplianceLimit * (1d - this.ComplianceLimitDeviation);

    #endregion

    #region " amount "

    /// <summary>
    /// Applies the reading to create the specific reading type in the inherited class.
    /// </summary>
    /// <remarks> Assumes that reading is a number. </remarks>
    /// <param name="rawValueReading"> The raw the value reading. </param>
    /// <param name="rawUnitsReading"> The raw units reading. </param>
    /// <returns> <c>true</c> if parsed; Otherwise, <c>false</c>. </returns>
    public override bool TryApplyReading( string rawValueReading, string rawUnitsReading )
    {
        this.MetaStatus.Reset();
        this.MetaStatus.IsValid = base.TryApplyReading( rawValueReading, rawUnitsReading );
        return this.MetaStatus.IsValid;
    }

    /// <summary>
    /// Applies the reading to create the specific reading type in the inherited class.
    /// </summary>
    /// <remarks> Assumes that reading is a number. </remarks>
    /// <param name="rawValueReading"> Specifies the value reading. </param>
    /// <returns> <c>true</c> if parsed; Otherwise, <c>false</c>. </returns>
    public override bool TryApplyReading( string rawValueReading )
    {
        this.MetaStatus.Reset();
        this.MetaStatus.IsValid = base.TryApplyReading( rawValueReading );
        if ( this.MetaStatus.IsValid && this.Value.HasValue )
            _ = this.TryEvaluate( this.Value.Value );
        return this.MetaStatus.IsValid;
    }

    /// <summary> Applies the unit described by unit. </summary>
    /// <param name="unit"> The unit. </param>
    public override void ApplyUnit( cc.isr.UnitsAmounts.Unit unit )
    {
        if ( this.Amount.Unit != unit )
        {
            this.MetaStatus.Reset();
            this.MetaStatus.IsValid = false;
        }

        base.ApplyUnit( unit );
    }

    #endregion
}
