// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI;

public partial class OutputSubsystemBase
{
    #region " output off mode "

    /// <summary> Define output off mode read writes. </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0028:Simplify collection initialization", Justification = "<Pending>" )]
    private void DefineOutputOffModeReadWrites()
    {
        this.OutputOffModeReadWrites = new();
        foreach ( OutputOffModes enumValue in Enum.GetValues( typeof( OutputOffModes ) ) )
            this.OutputOffModeReadWrites.Add( enumValue );
    }

    /// <summary> Gets or sets a dictionary of output Off Mode parses. </summary>
    /// <value> A Dictionary of output Off Mode parses. </value>
    public Pith.EnumReadWriteCollection OutputOffModeReadWrites { get; private set; }

    /// <summary> Gets or sets the supported Off Modes. </summary>
    /// <value> The supported Off Modes. </value>
    public OutputOffModes SupportedOutputOffModes
    {
        get;
        set
        {
            if ( !this.SupportedOutputOffModes.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the cached output off mode. </summary>
    /// <value>
    /// The <see cref="OutputOffMode">output off mode</see> or none if not set or unknown.
    /// </value>
    public OutputOffModes? OutputOffMode
    {
        get;

        protected set
        {
            if ( !this.OutputOffMode.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the output off mode. </summary>
    /// <param name="value"> The  output off mode. </param>
    /// <returns> The <see cref="OutputOffMode">output off mode</see> or none if unknown. </returns>
    public OutputOffModes? ApplyOutputOffMode( OutputOffModes value )
    {
        _ = this.WriteOutputOffMode( value );
        return this.QueryOutputOffMode();
    }

    /// <summary> Gets or sets the Output Off Mode query command. </summary>
    /// <remarks> SCPI: "?". </remarks>
    /// <value> The Off Mode query command. </value>
    protected virtual string OutputOffModeQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the output Off Mode. </summary>
    /// <returns> The <see cref="OutputOffMode">Off Mode</see> or none if unknown. </returns>
    public OutputOffModes? QueryOutputOffMode()
    {
        this.OutputOffMode = this.Session.Query( this.OutputOffMode.GetValueOrDefault( OutputOffModes.None ), this.OutputOffModeReadWrites, this.OutputOffModeQueryCommand );
        return this.OutputOffMode;
    }

    /// <summary> Gets or sets the output Off Mode command format. </summary>
    /// <remarks> SCPI: " {0}". </remarks>
    /// <value> The write Off Mode command format. </value>
    protected virtual string OutputOffModeCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the output Off Mode without reading back the value from the device. </summary>
    /// <param name="value"> The Off Mode. </param>
    /// <returns> The <see cref="OutputOffMode">Off Mode</see> or none if unknown. </returns>
    public OutputOffModes? WriteOutputOffMode( OutputOffModes value )
    {
        this.OutputOffMode = this.Session.Write( value, this.OutputOffModeCommandFormat, this.OutputOffModeReadWrites );
        return this.OutputOffMode;
    }

    #endregion

    #region " output terminals mode "

    /// <summary> Define output terminals mode read writes. </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0028:Simplify collection initialization", Justification = "<Pending>" )]
    private void DefineOutputTerminalsModeReadWrites()
    {
        this.OutputTerminalsModeReadWrites = new();
        foreach ( OutputTerminalsModes enumValue in Enum.GetValues( typeof( OutputTerminalsModes ) ) )
            this.OutputTerminalsModeReadWrites.Add( enumValue );
    }

    /// <summary> Gets or sets a dictionary of output Terminals Mode parses. </summary>
    /// <value> A Dictionary of output Terminals Mode parses. </value>
    public Pith.EnumReadWriteCollection OutputTerminalsModeReadWrites { get; private set; }

    /// <summary> Gets or sets the supported Terminals Modes. </summary>
    /// <value> The supported Terminals Modes. </value>
    public OutputTerminalsModes SupportedOutputTerminalsModes
    {
        get;
        set
        {
            if ( !this.SupportedOutputTerminalsModes.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the cached output Terminals mode. </summary>
    /// <value>
    /// The <see cref="OutputTerminalsMode">output Terminals mode</see> or none if not set or unknown.
    /// </value>
    public OutputTerminalsModes? OutputTerminalsMode
    {
        get;

        protected set
        {
            if ( !this.OutputTerminalsMode.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the output Terminals mode. </summary>
    /// <param name="value"> The  output Terminals mode. </param>
    /// <returns>
    /// The <see cref="OutputTerminalsMode">output Terminals mode</see> or none if unknown.
    /// </returns>
    public OutputTerminalsModes? ApplyOutputTerminalsMode( OutputTerminalsModes value )
    {
        _ = this.WriteOutputTerminalsMode( value );
        return this.QueryOutputTerminalsMode();
    }

    /// <summary> Gets or sets the Output Terminals Mode query command. </summary>
    /// <remarks> SCPI: ":OUTP:ROUT:TERM?". </remarks>
    /// <value> The Terminals Mode query command. </value>
    protected virtual string OutputTerminalsModeQueryCommand { get; set; } = ":OUTP:ROUT:TERM?";

    /// <summary> Queries the output Terminals Mode. </summary>
    /// <returns>
    /// The <see cref="OutputTerminalsMode">Terminals Mode</see> or none if unknown.
    /// </returns>
    public OutputTerminalsModes? QueryOutputTerminalsMode()
    {
        this.OutputTerminalsMode = this.Session.Query( this.OutputTerminalsMode.GetValueOrDefault( OutputTerminalsModes.None ), this.OutputTerminalsModeReadWrites,
            this.OutputTerminalsModeQueryCommand );
        return this.OutputTerminalsMode;
    }

    /// <summary> Gets or sets the output Terminals Mode command format. </summary>
    /// <remarks> SCPI: ":OUTP:ROUT:TERM {0}". </remarks>
    /// <value> The write Terminals Mode command format. </value>
    protected virtual string OutputTerminalsModeCommandFormat { get; set; } = ":OUTP:ROUT:TERM {0}";

    /// <summary>
    /// Writes the output Terminals Mode without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The Terminals Mode. </param>
    /// <returns>
    /// The <see cref="OutputTerminalsMode">Terminals Mode</see> or none if unknown.
    /// </returns>
    public OutputTerminalsModes? WriteOutputTerminalsMode( OutputTerminalsModes value )
    {
        this.OutputTerminalsMode = this.Session.Write( value, this.OutputTerminalsModeCommandFormat, this.OutputTerminalsModeReadWrites );
        return this.OutputTerminalsMode;
    }

    #endregion
}
/// <summary> Specifies the output terminals mode. </summary>
[Flags]
public enum OutputTerminalsModes
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
/// <summary> Specifies the output off mode. </summary>
[Flags]
public enum OutputOffModes
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "Not set ()" )]
    None = 0,

    /// <summary> An enum constant representing the guard option. </summary>
    [System.ComponentModel.Description( "Guard (GUAR)" )]
    Guard = 1,

    /// <summary> An enum constant representing the high impedance option. </summary>
    [System.ComponentModel.Description( "High Impedance (HIMP)" )]
    HighImpedance = 2,

    /// <summary> An enum constant representing the normal option. </summary>
    [System.ComponentModel.Description( "Normal (NORM)" )]
    Normal = 4,

    /// <summary> An enum constant representing the zero option. </summary>
    [System.ComponentModel.Description( "Zero (ZERO)" )]
    Zero = 8
}
