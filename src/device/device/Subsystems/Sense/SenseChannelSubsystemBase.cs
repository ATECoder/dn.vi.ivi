using System.Diagnostics;
using cc.isr.Enums;

namespace cc.isr.VI;
/// <summary>
/// Defines the contract that must be implemented by a Sense Channel Subsystem.
/// </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific ReSenses, Inc.<para>
/// Licensed under The MIT License. </para><para>
/// David, 2016-07-06, 4.0.6031. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract class SenseChannelSubsystemBase : SenseFunctionSubsystemBase
{
    #region " construction and cleanup "

    /// <summary> Initializes a new instance of the <see cref="SenseSubsystemBase" /> class. </summary>
    /// <remarks> David, 2020-07-28. </remarks>
    /// <param name="channelNumber">   The channel number. </param>
    /// <param name="statusSubsystem"> The status subsystem. </param>
    /// <param name="readingAmounts">  The reading amounts. </param>
    protected SenseChannelSubsystemBase( int channelNumber, StatusSubsystemBase statusSubsystem, ReadingAmounts readingAmounts ) : base( statusSubsystem, readingAmounts )
    {
        this.ChannelNumber = channelNumber;
        this.ApertureRange = Ranges.StandardApertureRange;
    }

    #endregion

    #region " channel "

    /// <summary> Gets or sets the channel number. </summary>
    /// <value> The channel number. </value>
    public int ChannelNumber { get; private set; }

    #endregion

    #region " commands "

    /// <summary> Gets or sets the clear command. </summary>
    /// <remarks> SCPI: ":SENS{0}:CORR2:CLE". </remarks>
    /// <value> The clear compensations command. </value>
    protected virtual string ClearCompensationsCommand { get; set; } = string.Empty;

    /// <summary> Clears the Compensations. </summary>
    public void ClearCompensations()
    {
        _ = this.Session.WriteLine( this.ClearCompensationsCommand );
    }

    #endregion

    #region " sweep points "

    /// <summary> The sweep points. </summary>

    /// <summary> Gets or sets the cached Sweep Points. </summary>
    /// <value> The Sweep Points or none if not set or unknown. </value>
    public int? SweepPoints
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.SweepPoints, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Sweep Points. </summary>
    /// <param name="value"> The current Sweep Points. </param>
    /// <returns> The SweepPoints or none if unknown. </returns>
    public int? ApplySweepPoints( int value )
    {
        _ = this.WriteSweepPoints( value );
        return this.QuerySweepPoints();
    }

    /// <summary> Gets or sets Sweep Points query command. </summary>
    /// <remarks> SCPI: ":SENS{0}:SWE:POIN?". </remarks>
    /// <value> The Sweep Points query command. </value>
    protected virtual string SweepPointsQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current Sweep Points. </summary>
    /// <returns> The Sweep Points or none if unknown. </returns>
    public int? QuerySweepPoints()
    {
        if ( !string.IsNullOrWhiteSpace( this.SweepPointsQueryCommand ) )
        {
            this.SweepPoints = this.Session.Query( 0, this.SweepPointsQueryCommand );
        }

        return this.SweepPoints;
    }

    /// <summary> Gets or sets Sweep Points command format. </summary>
    /// <remarks> SCPI: ":SENS{0}:SWE:POIN {0}". </remarks>
    /// <value> The Sweep Points command format. </value>
    protected virtual string SweepPointsCommandFormat { get; set; } = string.Empty;

    /// <summary> WriteEnum the Sweep Points without reading back the value from the device. </summary>
    /// <param name="value"> The current PointsSweepPoints. </param>
    /// <returns> The PointsSweepPoints or none if unknown. </returns>
    public int? WriteSweepPoints( int value )
    {
        if ( !string.IsNullOrWhiteSpace( this.SweepPointsCommandFormat ) )
        {
            _ = this.Session.WriteLine( this.SweepPointsCommandFormat, value );
        }

        this.SweepPoints = value;
        return this.SweepPoints;
    }

    #endregion

    #region " sweep start "

    /// <summary> The sweep start. </summary>

    /// <summary> Gets or sets the cached Sweep Start. </summary>
    /// <value> The Sweep Start or none if not set or unknown. </value>
    public double? SweepStart
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.SweepStart, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Sweep Start. </summary>
    /// <param name="value"> The current Sweep Start. </param>
    /// <returns> The SweepStart or none if unknown. </returns>
    public double? ApplySweepStart( double value )
    {
        _ = this.WriteSweepStart( value );
        return this.QuerySweepStart();
    }

    /// <summary> Gets or sets Sweep Start query command. </summary>
    /// <remarks> SCPI: ":SENS{0}:FREQ:STAR?". </remarks>
    /// <value> The Sweep Start query command. </value>
    protected virtual string SweepStartQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current Sweep Start. </summary>
    /// <returns> The Sweep Start or none if unknown. </returns>
    public double? QuerySweepStart()
    {
        if ( !string.IsNullOrWhiteSpace( this.SweepStartQueryCommand ) )
        {
            this.SweepStart = this.Session.Query( 0, this.SweepStartQueryCommand );
        }

        return this.SweepStart;
    }

    /// <summary> Gets or sets Sweep Start command format. </summary>
    /// <remarks> SCPI: ":SENS{0}:FREQ:STAR {0}". </remarks>
    /// <value> The Sweep Start command format. </value>
    protected virtual string SweepStartCommandFormat { get; set; } = string.Empty;

    /// <summary> WriteEnum the Sweep Start without reading back the value from the device. </summary>
    /// <param name="value"> The current StartSweepStart. </param>
    /// <returns> The StartSweepStart or none if unknown. </returns>
    public double? WriteSweepStart( double value )
    {
        if ( !string.IsNullOrWhiteSpace( this.SweepStartCommandFormat ) )
        {
            _ = this.Session.WriteLine( this.SweepStartCommandFormat, value );
        }

        this.SweepStart = value;
        return this.SweepStart;
    }

    #endregion

    #region " sweep stop "

    /// <summary> The sweep stop. </summary>

    /// <summary> Gets or sets the cached Sweep Stop. </summary>
    /// <value> The Sweep Stop or none if not set or unknown. </value>
    public double? SweepStop
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.SweepStop, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Sweep Stop. </summary>
    /// <param name="value"> The current Sweep Stop. </param>
    /// <returns> The SweepStop or none if unknown. </returns>
    public double? ApplySweepStop( double value )
    {
        _ = this.WriteSweepStop( value );
        return this.QuerySweepStop();
    }

    /// <summary> Gets or sets Sweep Stop query command. </summary>
    /// <remarks> SCPI: ":SENS{0}:FREQ:STOP?". </remarks>
    /// <value> The Sweep Stop query command. </value>
    protected virtual string SweepStopQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current Sweep Stop. </summary>
    /// <returns> The Sweep Stop or none if unknown. </returns>
    public double? QuerySweepStop()
    {
        if ( !string.IsNullOrWhiteSpace( this.SweepStopQueryCommand ) )
        {
            this.SweepStop = this.Session.Query( 0, this.SweepStopQueryCommand );
        }

        return this.SweepStop;
    }

    /// <summary> Gets or sets Sweep Stop command format. </summary>
    /// <remarks> SCPI: ":SENS{0}:FREQ:STOP {0}". </remarks>
    /// <value> The Sweep Stop command format. </value>
    protected virtual string SweepStopCommandFormat { get; set; } = string.Empty;

    /// <summary> WriteEnum the Sweep Stop without reading back the value from the device. </summary>
    /// <param name="value"> The current StopSweepStop. </param>
    /// <returns> The StopSweepStop or none if unknown. </returns>
    public double? WriteSweepStop( double value )
    {
        if ( !string.IsNullOrWhiteSpace( this.SweepStopCommandFormat ) )
        {
            _ = this.Session.WriteLine( this.SweepStopCommandFormat, value );
        }

        this.SweepStop = value;
        return this.SweepStop;
    }

    #endregion

    #region " adapter type "

    /// <summary> List of types of the supported adapters. </summary>

    /// <summary>
    /// Gets or sets the supported Adapter Type. This is a subset of the functions supported by the
    /// instrument.
    /// </summary>
    /// <value> A list of types of the supported adapters. </value>
    public AdapterTypes SupportedAdapterTypes
    {
        get;
        set
        {
            if ( !this.SupportedAdapterTypes.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The Adapter Type. </summary>
    private AdapterTypes? _adapterType;

    /// <summary> Gets or sets the cached Adapter Type. </summary>
    /// <value> The <see cref="AdapterType">Adapter Type</see> or none if not set or unknown. </value>
    public virtual AdapterTypes? AdapterType
    {
        get => this._adapterType;

        protected set
        {
            if ( !Nullable.Equals( this.AdapterType, value ) )
            {
                this._adapterType = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Adapter Type. </summary>
    /// <param name="value"> The Adapter Type. </param>
    /// <returns> The <see cref="AdapterType">Adapter Type</see> or none if unknown. </returns>
    public AdapterTypes? ApplyAdapterType( AdapterTypes value )
    {
        _ = this.WriteAdapterType( value );
        return this.QueryAdapterType();
    }

    /// <summary> Gets or sets the Adapter Type query command. </summary>
    /// <value> The Adapter Type query command, e.g., :SENS:ADAPT? </value>
    protected virtual string AdapterTypeQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Adapter Type. </summary>
    /// <returns> The <see cref="AdapterType">Adapter Type</see> or none if unknown. </returns>
    public AdapterTypes? QueryAdapterType()
    {
        if ( string.IsNullOrWhiteSpace( this.AdapterTypeQueryCommand ) )
        {
            this.AdapterType = new AdapterTypes?();
        }
        else
        {
            string mode = this.AdapterType.ToString();
            this.Session.MakeEmulatedReplyIfEmpty( mode );
            mode = this.Session.QueryTrimEnd( this.AdapterTypeQueryCommand );
            if ( string.IsNullOrWhiteSpace( mode ) )
            {
                string message = "Failed fetching Adapter Type";
                Debug.Assert( !Debugger.IsAttached, message );
                this.AdapterType = new AdapterTypes?();
            }
            else
            {
                this.AdapterType = Pith.SessionBase.ParseContained<AdapterTypes>( mode.BuildDelimitedValue() );
            }
        }

        return this.AdapterType;
    }

    /// <summary> Gets or sets the Adapter Type command. </summary>
    /// <value> The Adapter Type command, e.g., :SENS:ADAPT {0}. </value>
    protected virtual string AdapterTypeCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Adapter Type without reading back the value from the device. </summary>
    /// <param name="value"> The Adapter Type. </param>
    /// <returns> The <see cref="AdapterType">Adapter Type</see> or none if unknown. </returns>
    public AdapterTypes? WriteAdapterType( AdapterTypes value )
    {
        if ( !string.IsNullOrWhiteSpace( this.AdapterTypeCommandFormat ) )
        {
            _ = this.Session.WriteLine( this.AdapterTypeCommandFormat, value.ExtractBetween() );
        }

        this.AdapterType = value;
        return this.AdapterType;
    }

    #endregion

    #region " frequency points type "

    /// <summary> The Frequency Points Type. </summary>
    private FrequencyPointsTypes? _frequencyPointsType;

    /// <summary> Gets or sets the cached Frequency Points Type. </summary>
    /// <value>
    /// The <see cref="FrequencyPointsType">Frequency Points Type</see> or none if not set or unknown.
    /// </value>
    public virtual FrequencyPointsTypes? FrequencyPointsType
    {
        get => this._frequencyPointsType;

        protected set
        {
            if ( !Nullable.Equals( this.FrequencyPointsType, value ) )
            {
                this._frequencyPointsType = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Frequency Points Type. </summary>
    /// <param name="value"> The Frequency Points Type. </param>
    /// <returns>
    /// The <see cref="FrequencyPointsType">Frequency Points Type</see> or none if unknown.
    /// </returns>
    public FrequencyPointsTypes? ApplyFrequencyPointsType( FrequencyPointsTypes value )
    {
        _ = this.WriteFrequencyPointsType( value );
        return this.QueryFrequencyPointsType();
    }

    /// <summary> Gets or sets the Frequency Points Type query command. </summary>
    /// <remarks> SCPI: ":SENS{0}:COLL:FPO?". </remarks>
    /// <value> The 'frequency points type query' command. </value>
    protected virtual string FrequencyPointsTypeQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Frequency Points Type. </summary>
    /// <returns>
    /// The <see cref="FrequencyPointsType">Frequency Points Type</see> or none if unknown.
    /// </returns>
    public FrequencyPointsTypes? QueryFrequencyPointsType()
    {
        if ( string.IsNullOrWhiteSpace( this.FrequencyPointsTypeQueryCommand ) )
        {
            this.FrequencyPointsType = new FrequencyPointsTypes?();
        }
        else
        {
            string mode = this.FrequencyPointsType.ToString();
            this.Session.MakeEmulatedReplyIfEmpty( mode );
            mode = this.Session.QueryTrimEnd( this.FrequencyPointsTypeQueryCommand );
            if ( string.IsNullOrWhiteSpace( mode ) )
            {
                string message = "Failed fetching Frequency Points Type";
                Debug.Assert( !Debugger.IsAttached, message );
                this.FrequencyPointsType = new FrequencyPointsTypes?();
            }
            else
            {
                this.FrequencyPointsType = Pith.SessionBase.ParseContained<FrequencyPointsTypes>( mode.BuildDelimitedValue() );
            }
        }

        return this.FrequencyPointsType;
    }

    /// <summary> Gets or sets the Frequency Points Type command. </summary>
    /// <remarks> SCPI: ":SENS{0}:COLL:FPO {0}". </remarks>
    /// <value> The frequency points type command format. </value>
    protected virtual string FrequencyPointsTypeCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Frequency Points Type without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The Frequency Points Type. </param>
    /// <returns>
    /// The <see cref="FrequencyPointsType">Frequency Points Type</see> or none if unknown.
    /// </returns>
    public FrequencyPointsTypes? WriteFrequencyPointsType( FrequencyPointsTypes value )
    {
        if ( !string.IsNullOrWhiteSpace( this.FrequencyPointsTypeCommandFormat ) )
        {
            _ = this.Session.WriteLine( this.FrequencyPointsTypeCommandFormat, value.ExtractBetween() );
        }

        this.FrequencyPointsType = value;
        return this.FrequencyPointsType;
    }

    #endregion

    #region " sweep type "

    /// <summary> List of types of the supported sweeps. </summary>

    /// <summary>
    /// Gets or sets the supported Sweep Type. This is a subset of the functions supported by the
    /// instrument.
    /// </summary>
    /// <value> A list of types of the supported sweeps. </value>
    public SweepTypes SupportedSweepTypes
    {
        get;
        set
        {
            if ( !this.SupportedSweepTypes.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The Sweep Type. </summary>
    private SweepTypes? _sweepType;

    /// <summary> Gets or sets the cached Sweep Type. </summary>
    /// <value> The <see cref="SweepType">Sweep Type</see> or none if not set or unknown. </value>
    public virtual SweepTypes? SweepType
    {
        get => this._sweepType;

        protected set
        {
            if ( !Nullable.Equals( this.SweepType, value ) )
            {
                this._sweepType = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Sweep Type. </summary>
    /// <param name="value"> The Sweep Type. </param>
    /// <returns> The <see cref="SweepType">Sweep Type</see> or none if unknown. </returns>
    public SweepTypes? ApplySweepType( SweepTypes value )
    {
        _ = this.WriteSweepType( value );
        return this.QuerySweepType();
    }

    /// <summary> Gets or sets the Sweep Type query command. </summary>
    /// <remarks> SCPI: ":SENS{0}:SWE:TYPE? </remarks>
    /// <value> The 'sweep type query' command. </value>
    protected virtual string SweepTypeQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Sweep Type. </summary>
    /// <returns> The <see cref="SweepType">Sweep Type</see> or none if unknown. </returns>
    public SweepTypes? QuerySweepType()
    {
        if ( string.IsNullOrWhiteSpace( this.SweepTypeQueryCommand ) )
        {
            this.SweepType = new SweepTypes?();
        }
        else
        {
            string mode = this.SweepType.ToString();
            this.Session.MakeEmulatedReplyIfEmpty( mode );
            mode = this.Session.QueryTrimEnd( this.SweepTypeQueryCommand );
            if ( string.IsNullOrWhiteSpace( mode ) )
            {
                string message = "Failed fetching Sweep Type";
                Debug.Assert( !Debugger.IsAttached, message );
                this.SweepType = new SweepTypes?();
            }
            else
            {
                this.SweepType = Pith.SessionBase.ParseContained<SweepTypes>( mode.BuildDelimitedValue() );
            }
        }

        return this.SweepType;
    }

    /// <summary> Gets or sets the Sweep Type command. </summary>
    /// <remarks> SCPI: ":SENS{0}:SWE:TYPE {0} </remarks>
    /// <value> The sweep type command format. </value>
    protected virtual string SweepTypeCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Sweep Type without reading back the value from the device. </summary>
    /// <param name="value"> The Sweep Type. </param>
    /// <returns> The <see cref="SweepType">Sweep Type</see> or none if unknown. </returns>
    public SweepTypes? WriteSweepType( SweepTypes value )
    {
        if ( !string.IsNullOrWhiteSpace( this.SweepTypeCommandFormat ) )
        {
            _ = this.Session.WriteLine( this.SweepTypeCommandFormat, value.ExtractBetween() );
        }

        this.SweepType = value;
        return this.SweepType;
    }

    #endregion
}
/// <summary> Specifies the adapter types. </summary>
[Flags]
public enum AdapterTypes
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "None (NONE)" )]
    None = 0,

    /// <summary> An enum constant representing the 4 m 1 option. </summary>
    [System.ComponentModel.Description( "4TP 1m (E4M1)" )]
    E4M1 = 1,

    /// <summary> An enum constant representing the 4 m 2 option. </summary>
    [System.ComponentModel.Description( "4TP 2m (E4M2)" )]
    E4M2 = 2
}
/// <summary> A bit-field of flags for specifying sweep types. </summary>
[Flags]
public enum SweepTypes
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "Not set ()" )]
    None = 0,

    /// <summary> An enum constant representing the linear option. </summary>
    [System.ComponentModel.Description( "Linear (LIN)" )]
    Linear = 1,

    /// <summary> An enum constant representing the logarithmic option. </summary>
    [System.ComponentModel.Description( "Logarithmic (LOG)" )]
    Logarithmic = 2,

    /// <summary> An enum constant representing the segment option. </summary>
    [System.ComponentModel.Description( "Segment (SEGM)" )]
    Segment = 4,

    /// <summary> An enum constant representing the power option. </summary>
    [System.ComponentModel.Description( "Power (POW)" )]
    Power = 8,

    /// <summary> An enum constant representing the linear bias option. </summary>
    [System.ComponentModel.Description( "Linear Bias (BIAS)" )]
    LinearBias = 16,

    /// <summary> An enum constant representing the logarithmic bias option. </summary>
    [System.ComponentModel.Description( "Logarithmic Bias (LBI)" )]
    LogarithmicBias = 32
}
/// <summary> A bit-field of flags for specifying frequency points types. </summary>
[Flags]
public enum FrequencyPointsTypes
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "Not set ()" )]
    None = 0,

    /// <summary> An enum constant representing the fixed option. </summary>
    [System.ComponentModel.Description( "Fixed (FIX)" )]
    Fixed = 1,

    /// <summary> An enum constant representing the user option. </summary>
    [System.ComponentModel.Description( "User (USER)" )]
    User = 2
}
