// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI;

public partial class TraceSubsystemBase
{
    #region " feed source "

    /// <summary> Define feed source read writes. </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0028:Simplify collection initialization", Justification = "<Pending>" )]
    private void DefineFeedSourceReadWrites()
    {
        this.FeedSourceReadWrites = new();
        foreach ( FeedSources enumValue in Enum.GetValues( typeof( FeedSources ) ) )
            this.FeedSourceReadWrites.Add( enumValue );
    }

    /// <summary> Gets or sets a dictionary of Feed source parses. </summary>
    /// <value> A Dictionary of Feed source parses. </value>
    public Pith.EnumReadWriteCollection FeedSourceReadWrites { get; private set; }

    /// <summary> Gets or sets the supported Feed sources. </summary>
    /// <value> The supported Feed sources. </value>
    public FeedSources SupportedFeedSources
    {
        get;
        set
        {
            if ( !this.SupportedFeedSources.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the cached source FeedSource. </summary>
    /// <value>
    /// The <see cref="FeedSource">source Feed Source</see> or none if not set or unknown.
    /// </value>
    public FeedSources? FeedSource
    {
        get;

        protected set
        {
            if ( !this.FeedSource.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the source Feed Source. </summary>
    /// <param name="value"> The  Source Feed Source. </param>
    /// <returns> The <see cref="FeedSource">source Feed Source</see> or none if unknown. </returns>
    public FeedSources? ApplyFeedSource( FeedSources value )
    {
        _ = this.WriteFeedSource( value );
        return this.QueryFeedSource();
    }

    /// <summary> Gets or sets the Feed source query command. </summary>
    /// <remarks> SCPI: ":TRIG:SOUR?". </remarks>
    /// <value> The Feed source query command. </value>
    protected virtual string FeedSourceQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Feed source. </summary>
    /// <returns> The <see cref="FeedSource">Feed source</see> or none if unknown. </returns>
    public FeedSources? QueryFeedSource()
    {
        this.FeedSource = this.Session.Query( this.FeedSource.GetValueOrDefault( FeedSources.None ), this.FeedSourceReadWrites, this.FeedSourceQueryCommand );
        return this.FeedSource;
    }

    /// <summary> Gets or sets the Feed source command format. </summary>
    /// <remarks> SCPI: "{0}". </remarks>
    /// <value> The write Feed source command format. </value>
    protected virtual string FeedSourceCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Feed Source without reading back the value from the device. </summary>
    /// <param name="value"> The Feed Source. </param>
    /// <returns> The <see cref="FeedSource">Feed Source</see> or none if unknown. </returns>
    public FeedSources? WriteFeedSource( FeedSources value )
    {
        this.FeedSource = this.Session.Write( value, this.FeedSourceCommandFormat, this.FeedSourceReadWrites );
        return this.FeedSource;
    }

    #endregion

    #region " feed control "

    /// <summary> Define feed control read writes. </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0028:Simplify collection initialization", Justification = "<Pending>" )]
    private void DefineFeedControlReadWrites()
    {
        this.FeedControlReadWrites = new Pith.EnumReadWriteCollection();
        foreach ( FeedControls enumValue in Enum.GetValues( typeof( FeedControls ) ) )
            this.FeedControlReadWrites.Add( enumValue );
    }

    /// <summary> Gets or sets a dictionary of Feed Control parses. </summary>
    /// <value> A Dictionary of Feed Control parses. </value>
    public Pith.EnumReadWriteCollection FeedControlReadWrites { get; private set; }

    /// <summary> Gets or sets the supported Feed Controls. </summary>
    /// <value> The supported Feed Controls. </value>
    public FeedControls SupportedFeedControls
    {
        get;
        set
        {
            if ( !this.SupportedFeedControls.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the cached Control FeedControl. </summary>
    /// <value>
    /// The <see cref="FeedControl">Control Feed Control</see> or none if not set or unknown.
    /// </value>
    public FeedControls? FeedControl
    {
        get;

        protected set
        {
            if ( !this.FeedControl.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Control Feed Control. </summary>
    /// <param name="value"> The  Control Feed Control. </param>
    /// <returns> The <see cref="FeedControl">Control Feed Control</see> or none if unknown. </returns>
    public FeedControls? ApplyFeedControl( FeedControls value )
    {
        _ = this.WriteFeedControl( value );
        return this.QueryFeedControl();
    }

    /// <summary> Gets or sets the Feed Control query command. </summary>
    /// <remarks> SCPI: ":TRIG:SOUR?". </remarks>
    /// <value> The Feed Control query command. </value>
    protected virtual string FeedControlQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Feed Control. </summary>
    /// <returns> The <see cref="FeedControl">Feed Control</see> or none if unknown. </returns>
    public FeedControls? QueryFeedControl()
    {
        this.FeedControl = this.Session.Query( this.FeedControl.GetValueOrDefault( FeedControls.None ), this.FeedControlReadWrites, this.FeedControlQueryCommand );
        return this.FeedControl;
    }

    /// <summary> Gets or sets the Feed Control command format. </summary>
    /// <remarks> SCPI: "{0}". </remarks>
    /// <value> The write Feed Control command format. </value>
    protected virtual string FeedControlCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Feed Control without reading back the value from the device. </summary>
    /// <param name="value"> The Feed Control. </param>
    /// <returns> The <see cref="FeedControl">Feed Control</see> or none if unknown. </returns>
    public FeedControls? WriteFeedControl( FeedControls value )
    {
        this.FeedControl = this.Session.Write( value, this.FeedControlCommandFormat, this.FeedControlReadWrites );
        return this.FeedControl;
    }

    #endregion
}
/// <summary> Enumerates the trace feed control. </summary>
[Flags]
public enum FeedControls
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "Not Defined ()" )]
    None = 0,

    /// <summary> An enum constant representing the next] option. </summary>
    [System.ComponentModel.Description( "Sense (NEXT)" )]
    Next = 1,

    /// <summary> An enum constant representing the never] option. </summary>
    [System.ComponentModel.Description( "Never (NEV)" )]
    Never = 2,

    /// <summary> An enum constant representing the always option. </summary>
    [System.ComponentModel.Description( "Always (ALW)" )]
    Always = 3,

    /// <summary> An enum constant representing the pre trigger option. </summary>
    [System.ComponentModel.Description( "Pre-Trigger (PRET)" )]
    PreTrigger = 4
}
/// <summary> Enumerates the source of readings. </summary>
[Flags]
public enum FeedSources
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "None (NONE)" )]
    None = 0,

    /// <summary> An enum constant representing the sense option. </summary>
    [System.ComponentModel.Description( "Sense (SENS1)" )]
    Sense = 1,

    /// <summary> An enum constant representing the calculate 1 option. </summary>
    [System.ComponentModel.Description( "Calculate 1 (CALC)" )]
    Calculate1 = 2,

    /// <summary> An enum constant representing the calculate 2 option. </summary>
    [System.ComponentModel.Description( "Calculate 2 (CALC2)" )]
    Calculate2 = 4,

    /// <summary> An enum constant representing the current option. </summary>
    [System.ComponentModel.Description( "Current (CURR)" )]
    Current = 8,

    /// <summary> An enum constant representing the voltage option. </summary>
    [System.ComponentModel.Description( "Voltage (VOLT)" )]
    Voltage = 16,

    /// <summary> An enum constant representing the resistance option. </summary>
    [System.ComponentModel.Description( "Resistance (RES)" )]
    Resistance = 32
}
