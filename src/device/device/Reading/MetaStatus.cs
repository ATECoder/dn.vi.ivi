using System.Diagnostics;
using cc.isr.Std.NumericExtensions;

namespace cc.isr.VI;

/// <summary> Holds the measurand meta status and derived properties. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-11-01 </para>
/// </remarks>
public class MetaStatus
{
    #region " construction and cleanup "

    /// <summary> Contracts this class setting the meta status to 0. </summary>
    public MetaStatus() : base()
    {
        this.ApplyDefaultFailStatusBitmaskThis();
        this.ApplyDefaultInvalidStatusBitmaskThis();
        this.ResetThis();
    }

    /// <summary> Constructs a copy of an existing value. </summary>
    /// <param name="model"> The model. </param>
    public MetaStatus( MetaStatus model ) : this()
    {
        if ( model is not null )
        {
            this.StatusValue = model.StatusValue;
            this.FailStatusBitmask = model.FailStatusBitmask;
            this.InvalidStatusBitmask = model.InvalidStatusBitmask;
        }
    }

    #endregion

    #region " methods "

    /// <summary> Clears the meta status. </summary>
    private void ResetThis()
    {
        this.StatusValue = 0L;
    }

    /// <summary> Clears the meta status. </summary>
    public void Reset()
    {
        this.ResetThis();
    }

    /// <summary> Presets the meta status. </summary>
    /// <param name="value"> The value. </param>
    public void Preset( long value )
    {
        this.StatusValue = value;
    }

    /// <summary> Appends a value. </summary>
    /// <param name="value"> The value. </param>
    public void Append( long value )
    {
        this.StatusValue |= value;
    }

    /// <summary> Appends a value. </summary>
    /// <param name="status"> The status to append. </param>
    public void Append( MetaStatus status )
    {
        if ( status is not null && status.StatusValue != 0L )
        {
            this.StatusValue |= status.StatusValue;
        }
    }

    #endregion

    #region " properties "

    /// <summary> Converts this object to a color. </summary>
    /// <returns> This object as a Drawing.Color. </returns>
    public System.Drawing.Color ToColor()
    {
        return this.Passed() ? System.Drawing.Color.LimeGreen : System.Drawing.Color.Red;
    }

    /// <summary>
    /// Returns a two-character description representing the measurand meta status.
    /// </summary>
    /// <param name="passedValue"> The passed value. </param>
    /// <returns> The short meta status description. </returns>
    public string TwoCharDescription( string passedValue )
    {
        string result;
        if ( this.Passed() )
        {
            result = passedValue;
        }
        else if ( !this.IsSet() )
        {
            // if not set then return NO
            result = "no";
        }
        else if ( !this.HasValue )
        {
            // if no value we are out of luck
            result = "nv";
        }
        else if ( !this.IsValid )
        {
            result = "iv";
        }
        else if ( this.IsHigh )
        {
            result = "hi";
        }
        else if ( this.IsLow )
        {
            result = "lo";
        }
        else if ( this.FailedContactCheck )
        {
            result = "cc";
        }
        else if ( this.HitCompliance )
        {
            result = "sc"; // status compliance
        }
        else if ( this.HitRangeCompliance )
        {
            result = "rc";
        }
        else if ( this.HitLevelCompliance )
        {
            result = "lc";
        }
        else if ( this.HitOverRange )
        {
            result = "or";
        }
        else if ( this.HitVoltageProtection )
        {
            result = "vp";
        }
        else if ( this.Infinity )
        {
            result = Convert.ToChar( 0x221E ).ToString(); // 236)
        }
        else if ( this.NegativeInfinity )
        {
            result = $"-{Convert.ToChar( 0x221E )}";
        }
        else
        {
            // if we do not return a value, raise an exception
            Debug.Assert( !Debugger.IsAttached, "Unhandled case in determining two-char code." );
            result = "na";
        }

        return result;
    }

    /// <summary> Returns a short description representing the measurand meta status. </summary>
    /// <param name="passedValue"> The passed value. </param>
    /// <returns> The short meta status description. </returns>
    public string ToShortDescription( string passedValue )
    {
        string result;
        if ( this.Passed() )
        {
            result = passedValue;
        }
        else if ( !this.IsSet() )
        {
            // if not set then return NO
            result = "~set";
        }
        else if ( !this.HasValue )
        {
            // if no value we are out of luck
            result = "null";
        }
        else if ( !this.IsValid )
        {
            result = "~valid";
        }
        else if ( this.IsHigh )
        {
            result = "high";
        }
        else if ( this.IsLow )
        {
            result = "low";
        }
        else if ( this.FailedContactCheck )
        {
            result = "open";
        }
        else if ( this.HitCompliance )
        {
            result = "~comply"; // status compliance
        }
        else if ( this.HitRangeCompliance )
        {
            result = "~range";
        }
        else if ( this.HitLevelCompliance )
        {
            result = "~level";
        }
        else if ( this.HitOverRange )
        {
            result = "over";
        }
        else if ( this.HitVoltageProtection )
        {
            result = "volt";
        }
        else if ( this.Infinity )
        {
            result = "inf";
        }
        else if ( this.NegativeInfinity )
        {
            result = "-inf";
        }
        else
        {
            // if we do not return a value, raise an exception
            Debug.Assert( !Debugger.IsAttached, "Unhandled case in determining code." );
            result = "na";
        }

        return result;
    }

    /// <summary> Returns a long description representing the measurand meta status. </summary>
    /// <param name="passedValue"> The passed value. </param>
    /// <returns> The long  meta status description. </returns>
    public string ToLongDescription( string passedValue )
    {
        string result;
        if ( this.Passed() )
        {
            result = passedValue;
        }
        else if ( !this.IsSet() )
        {
            result = "Not Set";
        }
        else if ( !this.HasValue )
        {
            // if no value we are out of luck
            result = "No Value";
        }
        else if ( !this.IsValid )
        {
            result = "Invalid";
        }
        else if ( this.IsHigh )
        {
            result = "high";
        }
        else if ( this.IsLow )
        {
            result = "low";
        }
        else if ( this.FailedContactCheck )
        {
            result = "Failed contact check";
        }
        else if ( this.HitCompliance )
        {
            result = "Status Compliance";
        }
        else if ( this.HitRangeCompliance )
        {
            result = "Range compliance";
        }
        else if ( this.HitLevelCompliance )
        {
            result = "Level compliance";
        }
        else if ( this.HitOverRange )
        {
            result = "Over range";
        }
        else if ( this.HitVoltageProtection )
        {
            result = "Voltage protection";
        }
        else if ( this.Infinity )
        {
            result = "infinity";
        }
        else if ( this.NegativeInfinity )
        {
            result = "-infinity";
        }
        else
        {
            // if we do not return a value, raise an exception
            Debug.Assert( !Debugger.IsAttached, "Unhandled case in determining long outcome." );
            result = "not handled";
        }

        return result;
    }

    #endregion

    #region " conditions "

    /// <summary>
    /// Gets or sets the condition determining if the outcome indicates a failed contact check.
    /// </summary>
    /// <value> The failed contact check. </value>
    public bool FailedContactCheck
    {
        get => this.IsBit( MetaStatusBit.FailedContactCheck );
        set
        {
            this.ToggleBit( MetaStatusBit.FailedContactCheck, value );
            this.ToggleBit( MetaStatusBit.Valid, !value );
        }
    }

    /// <summary> Returns true if the <see cref="MetaStatus"/> was set. </summary>
    /// <returns>
    /// <c>true</c> if the <see cref="MetaStatus"/> was set; Otherwise, <c>false</c>.
    /// </returns>
    public bool IsSet()
    {
        return this.StatusValue - (1L << Measurand.MetaStatusBitBase) >= 0L;
    }

    /// <summary>
    /// Gets or sets the condition determining if the outcome indicates a having a value, namely if a
    /// value was set.
    /// </summary>
    /// <value>
    /// <c>true</c> if the <see cref="MetaStatusBit.HasValue"/> was set; Otherwise, <c>false</c>.
    /// </value>
    public bool HasValue
    {
        get => this.IsBit( MetaStatusBit.HasValue );
        set => this.ToggleBit( MetaStatusBit.HasValue, value );
    }

    /// <summary>
    /// Gets or sets the condition determining if the outcome indicates a high value.
    /// </summary>
    /// <value>
    /// <c>true</c> if the <see cref="MetaStatusBit.High"/> was set; Otherwise, <c>false</c>.
    /// </value>
    public bool IsHigh
    {
        get => this.IsBit( MetaStatusBit.High );
        set => this.ToggleBit( MetaStatusBit.High, value );
    }

    /// <summary>
    /// Gets the condition determining if the outcome indicates a measurement that Hit Compliance.
    /// The instrument sensed an output hit the compliance limit before being able to make a
    /// measurement.
    /// </summary>
    /// <value> <c>true</c> if hit compliance; Otherwise, <c>false</c>. </value>
    public bool HitCompliance => this.HitLevelCompliance || this.HitStatusCompliance;

    /// <summary>
    /// Gets or sets the condition determining if the outcome indicates a measurement that hit level
    /// compliance. Level compliance is hit if the measured level absolute value is outside the
    /// compliance limits.  This is a calculated value designed to address cases where the compliance
    /// bit is in correct.
    /// </summary>
    /// <value>
    /// <c>true</c> if the <see cref="MetaStatusBit.HitLevelCompliance"/> was set; Otherwise,
    /// <c>false</c>.
    /// </value>
    public bool HitLevelCompliance
    {
        get => this.IsBit( MetaStatusBit.HitLevelCompliance );
        set
        {
            this.ToggleBit( MetaStatusBit.HitLevelCompliance, value );
            this.ToggleBit( MetaStatusBit.Valid, !value );
        }
    }

    /// <summary>
    /// Gets or sets the condition determining if the outcome indicates a measurement that hit Status
    /// compliance.
    /// </summary>
    /// <remarks>
    /// When in real compliance, the source clamps at the displayed compliance value. <para>
    /// For example, if the compliance voltage Is set to 1V And the measurement range Is 2V, output
    /// voltage will clamp at 1V. In this Case, the “CMPL” annunciator will flash. </para><para>
    /// Status compliance is reported by the instrument in the status word.</para>
    /// </remarks>
    /// <value>
    /// <c>true</c> if the <see cref="MetaStatusBit.HitStatusCompliance"/> was set; Otherwise,
    /// <c>false</c>.
    /// </value>
    public bool HitStatusCompliance
    {
        get => this.IsBit( MetaStatusBit.HitStatusCompliance );
        set
        {
            this.ToggleBit( MetaStatusBit.HitStatusCompliance, value );
            this.ToggleBit( MetaStatusBit.Valid, value );
        }
    }

    /// <summary>
    /// Gets or sets the condition determining if the outcome indicates a measurement that Hit Range
    /// Compliance.
    /// </summary>
    /// <remarks>
    /// Range compliance is the maximum possible compliance value for the fixed measurement range.
    /// Note that range compliance cannot occur if the AUTO measurement range Is selected. Thus, To
    /// avoid range compliance, use AUTO range. <para>
    /// When in range compliance, the source output clamps at the maximum compliance value
    /// For the fixed measurement range (Not the compliance value). For example, if compliance Is
    /// Set To 1V And the measurement range Is 200mV, output voltage will clamp at 210mV. In this
    /// situation, the units In the compliance display field will flash. For example, With the
    /// following display : VCMPL: 10mA, the “mA” units indication will flash.</para>
    /// </remarks>
    /// <value>
    /// <c>true</c> if the <see cref="MetaStatusBit.HitRangeCompliance"/> was set; Otherwise,
    /// <c>false</c>.
    /// </value>
    public bool HitRangeCompliance
    {
        get => this.IsBit( MetaStatusBit.HitRangeCompliance );
        set
        {
            this.ToggleBit( MetaStatusBit.HitRangeCompliance, value );
            this.ToggleBit( MetaStatusBit.Valid, !value );
        }
    }

    /// <summary>
    /// Gets or sets the condition determining if the outcome indicates a measurement that Hit
    /// Voltage Protection.
    /// </summary>
    /// <value>
    /// <c>true</c> if the <see cref="MetaStatusBit.HitVoltageProtection"/> was set; Otherwise,
    /// <c>false</c>.
    /// </value>
    public bool HitVoltageProtection
    {
        get => this.IsBit( MetaStatusBit.HitVoltageProtection );
        set
        {
            this.ToggleBit( MetaStatusBit.HitVoltageProtection, value );
            this.ToggleBit( MetaStatusBit.Valid, !value );
        }
    }

    /// <summary>
    /// Gets or sets the condition determining if the outcome indicates a measurement that was made
    /// while over range.
    /// </summary>
    /// <value>
    /// <c>true</c> if the <see cref="MetaStatusBit.HitOverRange"/> was set; Otherwise, <c>false</c>.
    /// </value>
    public bool HitOverRange
    {
        get => this.IsBit( MetaStatusBit.HitOverRange );
        set
        {
            this.ToggleBit( MetaStatusBit.HitOverRange, value );
            this.ToggleBit( MetaStatusBit.Valid, !value );
        }
    }

    /// <summary>
    /// Gets or sets the condition determining if the outcome indicates a measurement that is low.
    /// </summary>
    /// <value>
    /// <c>true</c> if the <see cref="MetaStatusBit.Low"/> was set; Otherwise, <c>false</c>.
    /// </value>
    public bool IsLow
    {
        get => this.IsBit( MetaStatusBit.Low );
        set => this.ToggleBit( MetaStatusBit.Low, value );
    }

    /// <summary> Is affirmative. </summary>
    /// <param name="value"> The value. </param>
    /// <returns>
    /// <c>true</c> if the <paramref name="value"/> is set in the
    /// <see cref="MetaStatus">meta status</see>; Otherwise, <c>false</c>.
    /// </returns>
    public bool IsBit( MetaStatusBit value )
    {
        return this.StatusValue.IsBit( ( int ) value );
    }

    /// <summary>
    /// Toggles the bits specified in the <paramref name="value"/> based on the
    /// <paramref name="switch"/>.
    /// </summary>
    /// <param name="value">  The value. </param>
    /// <param name="switch"> True to set; otherwise, clear. </param>
    public void ToggleBit( MetaStatusBit value, bool @switch )
    {
        this.StatusValue = this.StatusValue.Toggle( ( int ) value, @switch );
        if ( value is not (MetaStatusBit.Pass or MetaStatusBit.HasValue) )
        {
            this.StatusValue = this.StatusValue.Toggle( ( int ) MetaStatusBit.Pass, this.Passed() );
            this.StatusValue = this.StatusValue.Toggle( ( int ) MetaStatusBit.HasValue, true );
        }
    }

    /// <summary> Gets or sets the condition determining if the outcome is Pass. </summary>
    /// <value>
    /// <c>true</c> if the <see cref="MetaStatusBit.Pass"/> was set; Otherwise, <c>false</c>.
    /// </value>
    public bool IsPass
    {
        get => this.IsBit( MetaStatusBit.Pass );
        set => this.ToggleBit( MetaStatusBit.Pass, value );
    }

    /// <summary>
    /// Gets or sets the condition determining if the outcome is Valid. The measured value is valid
    /// when its value or pass/fail outcomes or compliance or contact check conditions are set.
    /// </summary>
    /// <value>
    /// <c>true</c> if the <see cref="MetaStatusBit.Valid"/> was set; Otherwise, <c>false</c>.
    /// </value>
    public bool IsValid
    {
        get => this.IsBit( MetaStatusBit.Valid );
        set => this.ToggleBit( MetaStatusBit.Valid, value );
    }

    /// <summary>
    /// Gets or sets the condition determining if the outcome indicates a measurement that is
    /// infinity.
    /// </summary>
    /// <value>
    /// <c>true</c> if the <see cref="MetaStatusBit.Infinity"/> was set; Otherwise, <c>false</c>.
    /// </value>
    public bool Infinity
    {
        get => this.IsBit( MetaStatusBit.Infinity );
        set => this.ToggleBit( MetaStatusBit.Infinity, value );
    }

    /// <summary>
    /// Gets or sets the condition determining if the outcome indicates a measurement that is not a
    /// number.
    /// </summary>
    /// <value>
    /// <c>true</c> if the <see cref="MetaStatusBit.NotANumber"/> was set; Otherwise, <c>false</c>.
    /// </value>
    public bool NotANumber
    {
        get => this.IsBit( MetaStatusBit.NotANumber );
        set => this.ToggleBit( MetaStatusBit.NotANumber, value );
    }

    /// <summary>
    /// Gets or sets the condition determining if the outcome indicates a measurement that is
    /// negative infinity.
    /// </summary>
    /// <value>
    /// <c>true</c> if the <see cref="MetaStatusBit.NegativeInfinity"/> was set; Otherwise,
    /// <c>false</c>.
    /// </value>
    public bool NegativeInfinity
    {
        get => this.IsBit( MetaStatusBit.NegativeInfinity );
        set => this.ToggleBit( MetaStatusBit.NegativeInfinity, value );
    }

    /// <summary> Gets or sets the fail status bitmask. </summary>
    /// <value> The fail status bitmask. </value>
    public long FailStatusBitmask { get; set; }

    /// <summary> Applies the default fail status bitmask. </summary>
    private void ApplyDefaultFailStatusBitmaskThis()
    {
        this.FailStatusBitmask = (1L << ( int ) MetaStatusBit.Low) | (1L << ( int ) MetaStatusBit.High);
    }

    /// <summary> Applies the default fail status bitmask. </summary>
    public void ApplyDefaultFailStatusBitmask()
    {
        this.ApplyDefaultFailStatusBitmaskThis();
    }

    /// <summary> Returns True if valid and passed. </summary>
    /// <returns> True if it succeeds; otherwise, false. </returns>
    public bool Passed()
    {
        return this.Valid() && (this.StatusValue & this.FailStatusBitmask) == 0L;
    }

    /// <summary> Gets or sets the invalid status bitmask. </summary>
    /// <value> The invalid status bitmask. </value>
    public long InvalidStatusBitmask { get; set; }

    /// <summary> Applies the default invalid status bitmask. </summary>
    private void ApplyDefaultInvalidStatusBitmaskThis()
    {
        this.InvalidStatusBitmask = (1L << ( int ) MetaStatusBit.NotANumber) | (1L << ( int ) MetaStatusBit.Infinity) | (1L << ( int ) MetaStatusBit.NegativeInfinity) | (1L << ( int ) MetaStatusBit.FailedContactCheck) | (1L << ( int ) MetaStatusBit.HitLevelCompliance) | (1L << ( int ) MetaStatusBit.HitRangeCompliance) | (1L << ( int ) MetaStatusBit.HitVoltageProtection) | (1L << ( int ) MetaStatusBit.HitOverRange);
    }

    /// <summary> Applies the default invalid status bitmask. </summary>
    public void ApplyDefaultInvalidStatusBitmask()
    {
        this.ApplyDefaultInvalidStatusBitmaskThis();
    }

    /// <summary> Returns true if the value is valid, not out of range and not failed. </summary>
    /// <returns>
    /// <c>true</c> if the value is valid, not out of range and not failed; Otherwise, <c>false</c>
    /// </returns>
    public bool Valid()
    {
        return this.IsValid && (this.StatusValue & this.InvalidStatusBitmask) == 0L;
    }

    /// <summary> Holds the meta status value. </summary>
    /// <value> The meta status value. </value>
    public long StatusValue { get; private set; }

    #endregion
}
