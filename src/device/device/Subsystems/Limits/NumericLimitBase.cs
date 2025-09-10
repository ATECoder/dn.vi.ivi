namespace cc.isr.VI;

/// <summary> Defines the SCPI numeric limit subsystem base. </summary>
/// <remarks>
/// (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-11-05. Created based on SCPI 5.1 library.  </para><para>
/// David, 2008-03-25, 5.0.3004. Port to new SCPI library. </para>
/// </remarks>
/// <remarks> Initializes a new instance of the <see cref="NumericLimitBase" /> class. </remarks>
/// <param name="limitNumber">     The limit number. </param>
/// <param name="statusSubsystem"> The status subsystem. </param>
[CLSCompliant( false )]
public abstract class NumericLimitBase( int limitNumber, StatusSubsystemBase statusSubsystem ) : SubsystemBase( statusSubsystem )
{
    #region " i presettable "

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    public override void DefineKnownResetState()
    {
        base.DefineKnownResetState();
        this.Enabled = false;
    }

    #endregion

    #region " numeric command builder "

    /// <summary> Gets or sets the limit number. </summary>
    /// <value> The limit number. </value>
    protected int LimitNumber { get; private set; } = limitNumber;

    /// <summary> Builds a command. </summary>
    /// <param name="baseCommand"> The base command. </param>
    /// <returns> A <see cref="string" />. </returns>
    protected string BuildCommand( string baseCommand )
    {
        return string.Format( System.Globalization.CultureInfo.InvariantCulture, baseCommand, this.LimitNumber );
    }

    #endregion

    #region " limit failed "

    /// <summary> The failed. </summary>

    /// <summary> Gets or sets the cached In Limit Failed Condition sentinel. </summary>
    /// <value>
    /// <c>null</c> if In Limit Failed Condition is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? Failed
    {
        get;

        protected set
        {
            if ( !Equals( this.Failed, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the Limit Failed query command. </summary>
    /// <remarks> SCPI: ":CALC2:LIM[#]:FAIL?". </remarks>
    /// <value> The Limit Failed query command. </value>
    protected virtual string FailedQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Auto Delay Failed sentinel. Also sets the
    /// <see cref="Failed">Failed</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if Failed; otherwise <c>false</c>. </returns>
    public bool? QueryFailed()
    {
        this.Failed = this.Session.Query( this.Failed, this.BuildCommand( this.FailedQueryCommand ) );
        return this.Failed;
    }

    #endregion

    #region " limit enabled "

    /// <summary> The enabled. </summary>

    /// <summary> Gets or sets the cached Limit Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Limit Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? Enabled
    {
        get;

        protected set
        {
            if ( !Equals( this.Enabled, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Limit Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if Enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyEnabled( bool value )
    {
        _ = this.WriteEnabled( value );
        return this.QueryEnabled();
    }

    /// <summary> Gets or sets the Limit enabled query command. </summary>
    /// <remarks> SCPI: "CALC2:LIM[#]:STAT?". </remarks>
    /// <value> The Limit enabled query command. </value>
    protected virtual string EnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Auto Delay Enabled sentinel. Also sets the
    /// <see cref="Enabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryEnabled()
    {
        this.Enabled = this.Session.Query( this.Enabled, this.BuildCommand( this.EnabledQueryCommand ) );
        return this.Enabled;
    }

    /// <summary> Gets or sets the Limit enabled command Format. </summary>
    /// <remarks> SCPI: "CALC2:LIM[#]:STAT {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The Limit enabled query command. </value>
    protected virtual string EnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Auto Delay Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteEnabled( bool value )
    {
        this.Enabled = this.Session.WriteLine( value, this.BuildCommand( this.EnabledCommandFormat ) );
        return this.Enabled;
    }

    #endregion
}
