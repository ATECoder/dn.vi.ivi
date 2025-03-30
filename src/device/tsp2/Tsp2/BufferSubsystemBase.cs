namespace cc.isr.VI.Tsp2;

/// <summary> Defines the contract that must be implemented by a SCPI Trace Subsystem. </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract class BufferSubsystemBase : VI.BufferSubsystemBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="BufferSubsystemBase" /> class.
    /// </summary>
    /// <param name="statusSubsystem"> A reference to a
    /// <see cref="isr.VI.StatusSubsystemBase">status
    /// subsystem</see>. </param>
    protected BufferSubsystemBase( VI.StatusSubsystemBase statusSubsystem ) : this( DefaultBuffer1Name, statusSubsystem )
    {
    }

    /// <summary> Specialized constructor for use only by derived class. </summary>
    /// <param name="bufferName">      The name of the buffer. </param>
    /// <param name="statusSubsystem"> The status subsystem. </param>
    protected BufferSubsystemBase( string bufferName, VI.StatusSubsystemBase statusSubsystem ) : base( statusSubsystem )
    {
        this.BufferName = bufferName;
    }

    #endregion

    #region " buffer name "

    /// <summary> The default buffer 1 name. </summary>
    public const string DefaultBuffer1Name = "defbuffer1";

    /// <summary> The default buffer 2 name. </summary>
    public const string DefaultBuffer2Name = "defbuffer2";

    /// <summary> Name of the buffer. </summary>
    private string _bufferName;

    /// <summary> Builds the commands. </summary>
    /// <remarks> David, 2020-11-30. </remarks>
    /// <param name="bufferName"> The name of the buffer. </param>
    private void BuildCommands( string bufferName )
    {
        this.ClearBufferCommand = $"{bufferName}.clear";
        this.CapacityQueryCommand = $"_G.print(string.format('%d',{this.BufferName}.capacity))";
        this.CapacityCommandFormat = $"{this.BufferName}.capacity={{0}}";
        this.FillOnceEnabledQueryCommand = $"_G.print({this.BufferName}.fillmode==buffer.FILL_ONCE)";
        this.FillOnceEnabledCommandFormat = $"{this.BufferName}.fillmode={0:'buffer.FILL_ONCE';'buffer.FILL_ONCE';'buffer.FILL_CONTINUOUS'}";
        this.ActualPointCountQueryCommand = $"_G.print(string.format('%d',{this.BufferName}.n))";
        this.FirstPointNumberQueryCommand = $"_G.print(string.format('%d',{this.BufferName}.startindex))";
        this.LastPointNumberQueryCommand = $"_G.print(string.format('%d',{this.BufferName}.endindex))";
        this.BufferReadCommandFormat = $"_G.printbuffer({{0}},{{1}},{this.BufferName}.readings,{this.BufferName}.relativetimestampts,{this.BufferName}.statuses,{this.BufferName}.units";
    }

    /// <summary> Gets or sets the name of the buffer. </summary>
    /// <value> The name of the buffer. </value>
    public string BufferName
    {
        get => this._bufferName;
        set
        {
            if ( !string.Equals( value, this.BufferName, StringComparison.Ordinal ) )
            {
                this._bufferName = string.IsNullOrWhiteSpace( value ) ? DefaultBuffer1Name : value;
                if ( !string.IsNullOrWhiteSpace( value ) )
                    this.BuildCommands( value );
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion

    #region " command syntax "

    #region " clear "

    /// <summary> Gets the Clear Buffer command. </summary>
    /// <value> The ClearBuffer command. </value>
    protected override string ClearBufferCommand { get; set; }

    #endregion

    #region " capacity "

    /// <summary> Gets the points count query command. </summary>
    /// <value> The points count query command. </value>
    protected override string CapacityQueryCommand { get; set; }

    /// <summary> Gets the points count command format. </summary>
    /// <remarks> SCPI: ":TRAC:POIN:COUN {0}". </remarks>
    /// <value> The points count query command format. </value>
    protected override string CapacityCommandFormat { get; set; }

    #endregion

    #region " fill once enabled  "

    /// <summary> Gets the automatic Delay enabled query command. </summary>
    /// <value> The automatic Delay enabled query command. </value>
    protected override string FillOnceEnabledQueryCommand { get; set; }

    /// <summary> Gets the automatic Delay enabled command Format. </summary>
    /// <value> The automatic Delay enabled query command. </value>
    protected override string FillOnceEnabledCommandFormat { get; set; }

    #endregion

    #region " actual points "

    /// <summary> Gets the ActualPoint count query command. </summary>
    /// <value> The ActualPoint count query command. </value>
    protected override string ActualPointCountQueryCommand { get; set; }

    /// <summary> Gets The First Point Number (1-based) query command. </summary>
    /// <value> The First Point Number query command. </value>
    protected override string FirstPointNumberQueryCommand { get; set; }

    /// <summary> Gets The Last Point Number (1-based) query command. </summary>
    /// <value> The Last Point Number query command. </value>
    protected override string LastPointNumberQueryCommand { get; set; }

    #endregion

    #region " buffer streaming "

    /// <summary> Gets the buffer read command format. </summary>
    /// <value> The buffer read command format. </value>
    public override string BufferReadCommandFormat { get; set; }

    #endregion

    #endregion

    #region " data "

    /// <summary> Gets or sets the data query command. </summary>
    /// <value> The points count query command. </value>
    protected override string DataQueryCommand { get; set; } = string.Empty;

    #endregion

}
