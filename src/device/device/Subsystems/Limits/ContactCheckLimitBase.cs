namespace cc.isr.VI;

/// <summary> Defines the SCPI Contact Check Limit subsystem. </summary>
/// <remarks>
/// (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-11-05. Created based on SCPI 5.1 library.  </para><para>
/// David, 2008-03-25, 5.0.3004. Port to new SCPI library. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract class ContactCheckLimitBase : NumericLimitBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="ContactCheckLimitBase" /> class.
    /// </summary>
    /// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
    /// subsystem</see>. </param>
    protected ContactCheckLimitBase( StatusSubsystemBase statusSubsystem ) : base( 4, statusSubsystem )
    {
    }

    /// <summary> Specialized constructor for use only by derived class. </summary>
    /// <param name="limitNumber">     The limit number. </param>
    /// <param name="statusSubsystem"> The status subsystem. </param>
    protected ContactCheckLimitBase( int limitNumber, StatusSubsystemBase statusSubsystem ) : base( limitNumber, statusSubsystem )
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
}
