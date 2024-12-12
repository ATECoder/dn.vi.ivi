namespace cc.isr.VI;

/// <summary> Defines the SCPI Compliance Limit subsystem. </summary>
/// <remarks>
/// (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-11-05. Created based on SCPI 5.1 library.  </para><para>
/// David, 2008-03-25, 5.0.3004. Port to new SCPI library. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract class ComplianceLimitBase : NumericLimitBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="ComplianceLimitBase" /> class.
    /// </summary>
    /// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
    /// subsystem</see>. </param>
    protected ComplianceLimitBase( StatusSubsystemBase statusSubsystem ) : base( 1, statusSubsystem )
    {
    }

    /// <summary> Specialized constructor for use only by derived class. </summary>
    /// <param name="limitNumber">     The limit number. </param>
    /// <param name="statusSubsystem"> The status subsystem. </param>
    protected ComplianceLimitBase( int limitNumber, StatusSubsystemBase statusSubsystem ) : base( limitNumber, statusSubsystem )
    {
    }

    #endregion

    #region " i presettable "

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    public override void DefineKnownResetState()
    {
        base.DefineKnownResetState();
        this.Enabled = false;
        this.IncomplianceCondition = true;
        this.FailureBits = 15;
    }

    #endregion

    #region " failure bits "

    /// <summary> The failure bits. </summary>
    private int? _failureBits;

    /// <summary> Gets or sets the cached Failure Bits. </summary>
    /// <value> The Failure Bits or none if not set or unknown. </value>
    public int? FailureBits
    {
        get => this._failureBits;

        protected set
        {
            if ( !Nullable.Equals( this.FailureBits, value ) )
            {
                this._failureBits = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Failure Bits. </summary>
    /// <param name="value"> The current Failure Bits. </param>
    /// <returns> The Failure Bits or none if unknown. </returns>
    public int? ApplyFailureBits( int value )
    {
        _ = this.WriteFailureBits( value );
        return this.QueryFailureBits();
    }

    /// <summary> Gets or sets Failure Bits query command. </summary>
    /// <remarks> SCPI: ":CALC2:LIM:COMP:SOUR2?". </remarks>
    /// <value> The Failure Bits query command. </value>
    protected virtual string FailureBitsQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current Failure Bits. </summary>
    /// <returns> The Failure Bits or none if unknown. </returns>
    public int? QueryFailureBits()
    {
        if ( !string.IsNullOrWhiteSpace( this.FailureBitsQueryCommand ) )
        {
            this.FailureBits = this.Session.Query( 0, this.BuildCommand( this.FailureBitsQueryCommand ) );
        }

        return this.FailureBits;
    }

    /// <summary> Gets or sets Failure Bits command format. </summary>
    /// <remarks> SCPI: ":CALC2:LIM:COMP:SOUR2 {0}". </remarks>
    /// <value> The Failure Bits command format. </value>
    protected virtual string FailureBitsCommandFormat { get; set; } = string.Empty;

    /// <summary> WriteEnum the Failure Bits without reading back the value from the device. </summary>
    /// <param name="value"> The current Failure Bits. </param>
    /// <returns> The Failure Bits or none if unknown. </returns>
    public int? WriteFailureBits( int value )
    {
        if ( !string.IsNullOrWhiteSpace( this.FailureBitsCommandFormat ) )
        {
            _ = this.Session.WriteLine( this.BuildCommand( this.FailureBitsCommandFormat ), value );
        }

        this.FailureBits = value;
        return this.FailureBits;
    }

    #endregion

    #region " in compliance failure condition "

    /// <summary> The incompliance condition. </summary>
    private bool? _incomplianceCondition;

    /// <summary> Gets or sets the cached In Compliance Condition sentinel. </summary>
    /// <value>
    /// <c>null</c> if In Compliance Condition is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? IncomplianceCondition
    {
        get => this._incomplianceCondition;

        protected set
        {
            if ( !Equals( this.IncomplianceCondition, value ) )
            {
                this._incomplianceCondition = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the In Compliance Condition sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if Condition; otherwise <c>false</c>. </returns>
    public bool? ApplyIncomplianceCondition( bool value )
    {
        _ = this.WriteIncomplianceCondition( value );
        return this.QueryIncomplianceCondition();
    }

    /// <summary> Gets or sets the In compliance Condition query command. </summary>
    /// <remarks> SCPI: ":CALC2:LIM:COMP:FAIL?". </remarks>
    /// <value> The In-compliance Condition query command. </value>
    protected virtual string IncomplianceConditionQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Auto Delay Enabled sentinel. Also sets the
    /// <see cref="VI.ComplianceLimitBase.IncomplianceCondition">Condition</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if in compliance; otherwise <c>false</c>. </returns>
    public bool? QueryIncomplianceCondition()
    {
        this.IncomplianceCondition = this.Session.Query( this.IncomplianceCondition, this.BuildCommand( this.IncomplianceConditionQueryCommand ) );
        return this.IncomplianceCondition;
    }

    /// <summary>
    /// Gets or sets the In-compliance Condition command Format.
    /// <see cref="VI.ComplianceLimitBase.IncomplianceCondition">Condition</see> sentinel.
    /// </summary>
    /// <remarks> SCPI: ":CALC2:LIM:COMP:FAIL {0:'IN';'IN';'OUT'}". </remarks>
    /// <value> The incompliance condition command format. </value>
    protected virtual string IncomplianceConditionCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Auto Delay Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if in compliance; otherwise <c>false</c>. </returns>
    public bool? WriteIncomplianceCondition( bool value )
    {
        this.IncomplianceCondition = this.Session.WriteLine( value, this.BuildCommand( this.IncomplianceConditionCommandFormat ) );
        return this.IncomplianceCondition;
    }

    #endregion
}
