// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI;

/// <summary> Defines the Calculate Channel SCPI subsystem. </summary>
/// <remarks>
/// (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2016-07-06, 4.0.6031. </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="ChannelTriggerSubsystemBase" /> class.
/// </remarks>
/// <param name="channelNumber">   The channel number. </param>
/// <param name="statusSubsystem"> The status subsystem. </param>
[CLSCompliant( false )]
public abstract class ChannelTriggerSubsystemBase( int channelNumber, StatusSubsystemBase statusSubsystem ) : SubsystemBase( statusSubsystem )
{
    #region " channel "

    /// <summary> Gets or sets the channel number. </summary>
    /// <value> The channel number. </value>
    public int ChannelNumber { get; private set; } = channelNumber;

    #endregion

    #region " immediate "

    /// <summary> Gets or sets the initiate command. </summary>
    /// <remarks> SCPI: ":INIT&lt;c#&gt;:IMM". </remarks>
    /// <value> The initiate command. </value>
    protected virtual string InitiateCommand { get; set; } = string.Empty;

    /// <summary>
    /// Changes the state of the channel to the initiation state of the trigger system.
    /// </summary>
    public void Initiate()
    {
        _ = this.Session.WriteLine( this.InitiateCommand, this.ChannelNumber );
    }

    #endregion

    #region " continuous enabled "

    /// <summary> The continuous enabled. </summary>

    /// <summary> Gets or sets the cached Continuous Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Continuous Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? ContinuousEnabled
    {
        get;

        protected set
        {
            if ( !Equals( this.ContinuousEnabled, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Continuous Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyContinuousEnabled( bool value )
    {
        _ = this.WriteContinuousEnabled( value );
        return this.QueryContinuousEnabled();
    }

    /// <summary> Gets or sets the continuous trigger enabled query command. </summary>
    /// <remarks> SCPI: ":INIT{0}:CONT?". </remarks>
    /// <value> The continuous trigger enabled query command. </value>
    protected virtual string ContinuousEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Continuous Enabled sentinel. Also sets the
    /// <see cref="ContinuousEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryContinuousEnabled()
    {
        this.ContinuousEnabled = this.Session.Query( this.ContinuousEnabled, string.Format( this.ContinuousEnabledQueryCommand, this.ChannelNumber ) );
        return this.ContinuousEnabled;
    }

    /// <summary> Gets or sets the continuous trigger enabled command Format. </summary>
    /// <remarks> SCPI: ":INIT{0}:CONT {1:1;1;0}". </remarks>
    /// <value> The continuous trigger enabled query command. </value>
    protected virtual string ContinuousEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Continuous Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteContinuousEnabled( bool value )
    {
        _ = this.Session.WriteLine( string.Format( this.ContinuousEnabledCommandFormat, this.ChannelNumber, value.GetHashCode() ) );
        this.ContinuousEnabled = value;
        return this.ContinuousEnabled;
    }

    #endregion
}
