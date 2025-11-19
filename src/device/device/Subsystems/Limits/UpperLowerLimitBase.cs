namespace cc.isr.VI;

/// <summary> Defines the SCPI Upper/Lower Limit subsystem. </summary>
/// <remarks>
/// (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-11-05. Created based on SCPI 5.1 library.  </para><para>
/// David, 2008-03-25, 5.0.3004. Port to new SCPI library. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract class UpperLowerLimitBase : NumericLimitBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="UpperLowerLimitBase" /> class.
    /// </summary>
    /// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
    /// subsystem</see>. </param>
    protected UpperLowerLimitBase( StatusSubsystemBase statusSubsystem ) : base( 2, statusSubsystem )
    {
    }

    /// <summary> Specialized constructor for use only by derived class. </summary>
    /// <param name="limitNumber">     The limit number. </param>
    /// <param name="statusSubsystem"> The status subsystem. </param>
    protected UpperLowerLimitBase( int limitNumber, StatusSubsystemBase statusSubsystem ) : base( limitNumber, statusSubsystem )
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
        this.UpperLimit = 1;
        this.UpperLimitFailureBits = 15;
        this.LowerLimit = -1;
        this.LowerLimitFailureBits = 15;
        this.PassBits = 15;
    }

    #endregion

    #region " pass bits "

    /// <summary> The pass bits. </summary>

    /// <summary> Gets or sets the cached Pass Bits. </summary>
    /// <value> The Pass Bits or none if not set or unknown. </value>
    public int? PassBits
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.PassBits, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Pass Bits. </summary>
    /// <param name="value"> The current Pass Bits. </param>
    /// <returns> The Pass Bits or none if unknown. </returns>
    public int? ApplyPassBits( int value )
    {
        _ = this.WritePassBits( value );
        return this.QueryPassBits();
    }

    /// <summary> Gets or sets trigger Pass Bits query command. </summary>
    /// <remarks> SCPI: ":CALC2:LIM2:SOUR2?". </remarks>
    /// <value> The trigger PassBits query command. </value>
    protected virtual string PassBitsQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current pass Bits. </summary>
    /// <returns> The Compliance Failure Bits or none if unknown. </returns>
    public int? QueryPassBits()
    {
        if ( !string.IsNullOrWhiteSpace( this.PassBitsQueryCommand ) )
        {
            this.PassBits = this.Session.Query( 0, this.BuildCommand( this.PassBitsQueryCommand ) );
        }

        return this.PassBits;
    }

    /// <summary> Gets or sets Pass Bits command format. </summary>
    /// <remarks> SCPI: ":CALC2:LIM2:SOUR2 {0}". </remarks>
    /// <value> The Pass Bits command format. </value>
    protected virtual string PassBitsCommandFormat { get; set; } = string.Empty;

    /// <summary> WriteEnum the Pass Bits without reading back the value from the device. </summary>
    /// <param name="value"> The current Pass Bits. </param>
    /// <returns> The Pass Bits or none if unknown. </returns>
    public int? WritePassBits( int value )
    {
        if ( !string.IsNullOrWhiteSpace( this.PassBitsCommandFormat ) )
        {
            _ = this.Session.WriteLine( this.BuildCommand( this.PassBitsCommandFormat ), value );
        }

        this.PassBits = value;
        return this.PassBits;
    }

    #endregion

    #region " lower limit "

    /// <summary> The lower limit. </summary>

    /// <summary> Gets or sets the cached Lower Limit. </summary>
    /// <value> The Lower Limit or none if not set or unknown. </value>
    public double? LowerLimit
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.LowerLimit, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Lower Limit. </summary>
    /// <param name="value"> The current Lower Limit. </param>
    /// <returns> The Lower Limit or none if unknown. </returns>
    public double? ApplyLowerLimit( double value )
    {
        _ = this.WriteLowerLimit( value );
        return this.QueryLowerLimit();
    }

    /// <summary> Gets or sets the Lower Limit query command. </summary>
    /// <remarks> SCPI: ":CALC2:LIM2:LOW?". </remarks>
    /// <value> The Limit enabled query command. </value>
    protected virtual string LowerLimitQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current Lower Limit. </summary>
    /// <returns> The Lower Limit or none if unknown. </returns>
    public double? QueryLowerLimit()
    {
        this.LowerLimit = this.Session.Query( this.LowerLimit, this.BuildCommand( this.LowerLimitQueryCommand ) );
        return this.LowerLimit;
    }

    /// <summary> Gets or sets the Lower Limit query command. </summary>
    /// <remarks> SCPI: ":CALC2:LIM2:LOW {0}". </remarks>
    /// <value> The Limit enabled query command. </value>
    protected virtual string LowerLimitCommandFormat { get; set; } = string.Empty;

    /// <summary> Sets the Lower Limit without reading back the value from the device. </summary>
    /// <param name="value"> The current Lower Limit. </param>
    /// <returns> The Lower Limit or none if unknown. </returns>
    public double? WriteLowerLimit( double value )
    {
        this.LowerLimit = this.Session.WriteLine( value, this.BuildCommand( this.LowerLimitCommandFormat ) );
        return this.LowerLimit;
    }

    #endregion

    #region " lower limit failure bits "

    /// <summary> The lower limit failure bits. </summary>

    /// <summary> Gets or sets the cached Lower Limit Failure Bits. </summary>
    /// <value> The Lower Limit Failure Bits or none if not set or unknown. </value>
    public int? LowerLimitFailureBits
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.LowerLimitFailureBits, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Lower Limit Failure Bits. </summary>
    /// <param name="value"> The current Lower Limit Failure Bits. </param>
    /// <returns> The Lower Limit Failure Bits or none if unknown. </returns>
    public int? ApplyLowerLimitFailureBits( int value )
    {
        _ = this.WriteLowerLimitFailureBits( value );
        return this.QueryLowerLimitFailureBits();
    }

    /// <summary> Gets or sets the Lower Limit failure Bits query command. </summary>
    /// <remarks> SCPI: ":CALC2:LIM2:LOW:SOUR2?". </remarks>
    /// <value> The Limit enabled query command. </value>
    protected virtual string LowerLimitFailureBitsQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current Lower Limit Failure Bits. </summary>
    /// <returns> The Lower Limit Failure Bits or none if unknown. </returns>
    public int? QueryLowerLimitFailureBits()
    {
        this.LowerLimitFailureBits = this.Session.Query( this.LowerLimitFailureBits, this.BuildCommand( this.LowerLimitFailureBitsQueryCommand ) );
        return this.LowerLimitFailureBits;
    }

    /// <summary> Gets or sets the Lower Limit Failure Bits query command. </summary>
    /// <remarks> SCPI: ":CALC2:LIM2:LOW:SOUR2 {0}". </remarks>
    /// <value> The Limit enabled query command. </value>
    protected virtual string LowerLimitFailureBitsCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Sets back the Lower Limit Failure Bits without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The current Lower Limit Failure Bits. </param>
    /// <returns> The Lower Limit Failure Bits or none if unknown. </returns>
    public int? WriteLowerLimitFailureBits( int value )
    {
        this.LowerLimitFailureBits = this.Session.WriteLine( value, this.BuildCommand( this.LowerLimitFailureBitsCommandFormat ) );
        return this.LowerLimitFailureBits;
    }

    #endregion

    #region " upper limit "

    /// <summary> The upper limit. </summary>

    /// <summary> Gets or sets the cached Upper Limit. </summary>
    /// <value> The Upper Limit or none if not set or unknown. </value>
    public double? UpperLimit
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.UpperLimit, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Upper Limit. </summary>
    /// <param name="value"> The current Upper Limit. </param>
    /// <returns> The Upper Limit or none if unknown. </returns>
    public double? ApplyUpperLimit( double value )
    {
        _ = this.WriteUpperLimit( value );
        return this.QueryUpperLimit();
    }

    /// <summary> Gets or sets the Upper Limit query command. </summary>
    /// <remarks> SCPI: ":CALC2:LIM2:UPP?". </remarks>
    /// <value> The Limit enabled query command. </value>
    protected virtual string UpperLimitQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current Upper Limit. </summary>
    /// <returns> The Upper Limit or none if unknown. </returns>
    public double? QueryUpperLimit()
    {
        this.UpperLimit = this.Session.Query( this.UpperLimit, this.BuildCommand( this.UpperLimitQueryCommand ) );
        return this.UpperLimit;
    }

    /// <summary> Gets or sets the Upper Limit query command. </summary>
    /// <remarks> SCPI: ":CALC2:LIM2:UPP {0}". </remarks>
    /// <value> The Limit enabled query command. </value>
    protected virtual string UpperLimitCommandFormat { get; set; } = string.Empty;

    /// <summary> Sets the Upper Limit without reading back the value from the device. </summary>
    /// <param name="value"> The current Upper Limit. </param>
    /// <returns> The Upper Limit or none if unknown. </returns>
    public double? WriteUpperLimit( double value )
    {
        this.UpperLimit = this.Session.WriteLine( value, this.BuildCommand( this.UpperLimitCommandFormat ) );
        return this.UpperLimit;
    }

    #endregion

    #region " upper limit failure bits "

    /// <summary> The upper limit failure bits. </summary>

    /// <summary> Gets or sets the cached Upper Limit Failure Bits. </summary>
    /// <value> The Upper Limit Failure Bits or none if not set or unknown. </value>
    public int? UpperLimitFailureBits
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.UpperLimitFailureBits, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Upper Limit Failure Bits. </summary>
    /// <param name="value"> The current Upper Limit Failure Bits. </param>
    /// <returns> The Upper Limit Failure Bits or none if unknown. </returns>
    public int? ApplyUpperLimitFailureBits( int value )
    {
        _ = this.WriteUpperLimitFailureBits( value );
        return this.QueryUpperLimitFailureBits();
    }

    /// <summary> Gets or sets the Upper Limit failure Bits query command. </summary>
    /// <remarks> SCPI: ":CALC2:LIM2:UPP:SOUR2?". </remarks>
    /// <value> The Limit enabled query command. </value>
    protected virtual string UpperLimitFailureBitsQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current Upper Limit Failure Bits. </summary>
    /// <returns> The Upper Limit Failure Bits or none if unknown. </returns>
    public int? QueryUpperLimitFailureBits()
    {
        this.UpperLimitFailureBits = this.Session.Query( this.UpperLimitFailureBits, this.BuildCommand( this.UpperLimitFailureBitsQueryCommand ) );
        return this.UpperLimitFailureBits;
    }

    /// <summary> Gets or sets the Upper Limit Failure Bits query command. </summary>
    /// <remarks> SCPI: ":CALC2:LIM2:UPP:SOUR2 {0}". </remarks>
    /// <value> The Limit enabled query command. </value>
    protected virtual string UpperLimitFailureBitsCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Sets back the Upper Limit Failure Bits without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The current Upper Limit Failure Bits. </param>
    /// <returns> The Upper Limit Failure Bits or none if unknown. </returns>
    public int? WriteUpperLimitFailureBits( int value )
    {
        this.UpperLimitFailureBits = this.Session.WriteLine( value, this.BuildCommand( this.UpperLimitFailureBitsCommandFormat ) );
        return this.UpperLimitFailureBits;
    }

    #endregion
}
