namespace cc.isr.VI;

public partial class RouteSubsystemBase
{
    /// <summary> Define terminals mode read writes. </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0028:Simplify collection initialization", Justification = "<Pending>" )]
    private void DefineTerminalsModeReadWrites()
    {
        this.TerminalsModeReadWrites = new();
        foreach ( RouteTerminalsModes enumValue in Enum.GetValues( typeof( RouteTerminalsModes ) ) )
            this.TerminalsModeReadWrites.Add( enumValue );
    }

    /// <summary>
    /// Gets or sets a cached value indicating whether Front terminals are selected.
    /// </summary>
    /// <value>
    /// <c>true</c> if Front is Switched; <c>false</c> if not or none if not set or unknown.
    /// </value>
    public bool? FrontTerminalsSelected
    {
        get;

        protected set
        {
            if ( !Equals( this.FrontTerminalsSelected, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets a dictionary of output Terminals Mode parses. </summary>
    /// <value> A Dictionary of output Terminals Mode parses. </value>
    public Pith.EnumReadWriteCollection TerminalsModeReadWrites { get; private set; }

    /// <summary> Gets or sets the supported Terminals Modes. </summary>
    /// <value> The supported Terminals Modes. </value>
    public RouteTerminalsModes SupportedTerminalsModes
    {
        get;
        set
        {
            if ( !this.SupportedTerminalsModes.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the cached Route Terminals mode. </summary>
    /// <value> The Route Terminals mode or null if unknown. </value>
    public RouteTerminalsModes? TerminalsMode
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.TerminalsMode, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
                this.FrontTerminalsSelected = RouteTerminalsModes.Front == value.GetValueOrDefault( RouteTerminalsModes.None );
            }
        }
    }

    /// <summary> Writes and reads back the Route Terminals mode. </summary>
    /// <param name="value"> The <see cref="RouteTerminalsModes">Route Terminals mode</see>. </param>
    /// <returns> The Route Terminals mode or null if unknown. </returns>
    public RouteTerminalsModes? ApplyTerminalsMode( RouteTerminalsModes value )
    {
        _ = this.WriteTerminalsMode( value );
        return this.QueryTerminalsMode();
    }

    /// <summary> Gets or sets the terminals mode query command. </summary>
    /// <value> The terminals mode command. </value>
    protected virtual string TerminalsModeQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Route Terminals Mode. Also sets the <see cref="TerminalsMode">output on</see>
    /// sentinel.
    /// </summary>
    /// <returns> The Route Terminals mode or null if unknown. </returns>
    public RouteTerminalsModes? QueryTerminalsMode()
    {
        this.TerminalsMode = this.Session.QueryEnum( this.TerminalsMode, this.TerminalsModeQueryCommand );
        return this.TerminalsMode;
    }

    /// <summary> Gets or sets the terminals mode command format. </summary>
    /// <value> The terminals mode command format. </value>
    protected virtual string TerminalsModeCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Route Terminals mode. Does not read back from the instrument. </summary>
    /// <param name="value"> The Terminal mode. </param>
    /// <returns> The Route Terminals mode or null if unknown. </returns>
    public RouteTerminalsModes? WriteTerminalsMode( RouteTerminalsModes value )
    {
        this.TerminalsMode = this.Session.WriteEnum( value, this.TerminalsModeCommandFormat );
        return this.TerminalsMode;
    }
}
/// <summary> Specifies the route terminals mode. </summary>
[Flags]
public enum RouteTerminalsModes
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "Not set ()" )]
    None = 0,

    /// <summary> An enum constant representing the front option. </summary>
    [System.ComponentModel.Description( "Front (FRON)" )]
    Front = 1,

    /// <summary> An enum constant representing the rear option. </summary>
    [System.ComponentModel.Description( "Rear (REAR)" )]
    Rear = 2
}
