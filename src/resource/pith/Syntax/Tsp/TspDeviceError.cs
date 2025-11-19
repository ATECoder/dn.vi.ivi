namespace cc.isr.VI.Syntax.Tsp;

/// <summary> Encapsulates handling a TSP device error. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-11-08 </para>
/// </remarks>
public class TspDeviceError : Pith.DeviceError
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2025-09-09. </remarks>
    public TspDeviceError() : base( Syntax.Tsp.EventLog.NoErrorCompoundMessage )
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TspDeviceError" /> class specifying no error.
    /// </summary>
    public TspDeviceError( string noErrorCompoundMessage ) : base( noErrorCompoundMessage )
    {
    }

    /// <summary> Initializes a new instance of the <see cref="TspDeviceError" /> class. </summary>
    /// <param name="value"> The value. </param>
    public TspDeviceError( TspDeviceError value ) : base( value )
    {
        if ( value is null )
        {
            this.CompoundErrorMessage = Syntax.Tsp.EventLog.NoErrorCompoundMessage;
            this.ErrorMessage = Syntax.Tsp.EventLog.NoErrorMessage;
            base.ErrorLevel = ( int ) TspErrorLevel.None;
            this.NodeNumber = 0;
        }
        else
        {
            base.ErrorLevel = value.ErrorLevel;
            this.NodeNumber = value.NodeNumber;
        }
    }

    #endregion

    #region " parse "

    /// <summary> Initializes a new instance of the <see cref="TspDeviceError" /> class. </summary>
    /// <remarks>
    /// Error messages are formatted as follows:<para>
    /// error#,message,level#,node#.</para>
    /// <list type="bullet">
    /// <listheader><description>Error levels are:</description>
    /// </listheader>
    /// <item><description>0 - Informational.</description></item>
    /// <item><description>10 - Informational.</description></item>
    /// <item><description>30 - Serious.</description></item>
    /// <item><description>40 - Critical.</description></item>
    /// </list>
    /// </remarks>
    /// <param name="compoundError"> The compound error. </param>
    public override void Parse( string compoundError )
    {
        base.Parse( compoundError );
        if ( string.IsNullOrWhiteSpace( compoundError ) )
        {
            this.ErrorLevel = ( int ) TspErrorLevel.None;
            this.NodeNumber = 0;
        }
        else
        {
            string[] parts = compoundError.Split( ',' );
            int value;
            if ( parts.Length > 2 )
            {
                if ( int.TryParse( parts[2], out value ) )
                {
                    this.ErrorLevel = value;
                }
            }

            if ( parts.Length > 3 )
            {
                if ( int.TryParse( parts[3], out value ) )
                {
                    this.NodeNumber = value;
                }
            }
        }

        switch ( this.TspErrorLevel )
        {
            case TspErrorLevel.Fatal:
                {
                    this.Severity = TraceEventType.Critical;
                    break;
                }

            case TspErrorLevel.Informational:
                {
                    this.Severity = TraceEventType.Information;
                    break;
                }

            case TspErrorLevel.None:
                {
                    this.Severity = TraceEventType.Verbose;
                    break;
                }

            case TspErrorLevel.Recoverable:
                {
                    this.Severity = TraceEventType.Warning;
                    break;
                }

            case TspErrorLevel.Serious:
                {
                    this.Severity = TraceEventType.Error;
                    break;
                }

            default:
                {
                    this.Severity = TraceEventType.Verbose;
                    break;
                }
        }
    }

    #endregion

    #region " tsp error "

    /// <summary> Gets or sets (protected) the error Level. </summary>
    /// <value> The error Level. </value>
    public override int ErrorLevel
    {
        get => base.ErrorLevel;

        protected set
        {
            if ( value != this.ErrorLevel )
            {
                base.ErrorLevel = value;
                this.TspErrorLevel = Enum.IsDefined( typeof( TspErrorLevel ), this.ErrorLevel ) ? ( TspErrorLevel ) this.ErrorLevel : this.ErrorLevel > 0 ? TspErrorLevel.Serious : TspErrorLevel.None;
            }
        }
    }

    /// <summary> Gets the error level. </summary>
    /// <value> The error level. </value>
    public TspErrorLevel TspErrorLevel { get; private set; }

    /// <summary> Gets the node number. </summary>
    /// <value> The node number. </value>
    public int NodeNumber { get; private set; }

    /// <summary> Builds error message. </summary>
    /// <returns> A <see cref="string" />. </returns>
    public override string BuildErrorMessage()
    {
        return BuildErrorMessage( this.ErrorNumber, this.ErrorMessage, this.ErrorLevel, this.NodeNumber );
    }

    /// <summary> Builds error message. </summary>
    /// <param name="errorNumber">  The error number. </param>
    /// <param name="errorMessage"> Message describing the error. </param>
    /// <param name="errorLevel">   The error Level. </param>
    /// <param name="nodeNumber">   The node number. </param>
    /// <returns> A <see cref="string" />. </returns>
    public static string BuildErrorMessage( int errorNumber, string errorMessage, int errorLevel, int nodeNumber )
    {
        return string.Format( System.Globalization.CultureInfo.CurrentCulture, $"{errorNumber},{errorMessage},{errorLevel},{nodeNumber}" );
    }

    #endregion
}
/// <summary> Enumerates the TSP error levels. </summary>
public enum TspErrorLevel
{
    /// <summary> Indicates no error: “Queue is Empty”. </summary>
    [System.ComponentModel.Description( "None" )]
    None = 0,

    /// <summary> Indicates an event or a minor error.
    /// Examples: “Reading Available” and “Reading Overflow”. </summary>
    [System.ComponentModel.Description( "Informational" )]
    Informational = 10,

    /// <summary> Indicates possible invalid user input.
    /// Operation will continue but action should be taken to correct the error.
    /// Examples: “Exponent Too Large” and “Numeric Data Not Allowed”. </summary>
    [System.ComponentModel.Description( "Recoverable" )]
    Recoverable = 20,

    /// <summary> Indicates a serious error and may require technical assistance.
    /// Example: “Saved calibration constants corrupted”. </summary>
    [System.ComponentModel.Description( "Serious" )]
    Serious = 30,

    /// <summary> Indicates that the Series 2600 is non-operational and will
    /// require service. Contact information for service is provided in Section 1.
    /// Examples: “Bad SMU A FPGA image size”, “SMU is unresponsive” and
    /// “Communication Timeout with D FPGA”. </summary>
    [System.ComponentModel.Description( "Fatal" )]
    Fatal = 40
}
