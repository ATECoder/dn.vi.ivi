// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using System.ComponentModel;
using System.Diagnostics;
using cc.isr.Enums;
using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp;

/// <summary> Defines the contract that must be implemented by a Sense Subsystem. </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
/// <remarks> Initializes a new instance of the <see cref="SenseSubsystemBase" /> class. </remarks>
/// <param name="statusSubsystem"> A reference to a <see cref="VI.StatusSubsystemBase">TSP status
/// Subsystem</see>. </param>
public abstract class SenseSubsystemBase( Tsp.StatusSubsystemBase statusSubsystem ) : SourceMeasureUnitBase( statusSubsystem )
{
    #region " i presettable "

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    public override void DefineKnownResetState()
    {
        base.DefineKnownResetState();
        this.SenseMode = SenseActionMode.Local;
    }

    #endregion

    #region " sense mode "

    /// <summary> The Sense Action. </summary>

    /// <summary> Gets or sets the cached Sense Action. </summary>
    /// <value>
    /// The <see cref="SenseActionMode">Sense Action</see> or none if not set or unknown.
    /// </value>
    public SenseActionMode? SenseMode
    {
        get;
        protected set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Writes and reads back the Sense Action. </summary>
    /// <param name="value"> The  Sense Action. </param>
    /// <returns> The <see cref="SenseActionMode">Sense Action</see> or none if unknown. </returns>
    public SenseActionMode? ApplySenseMode( SenseActionMode value )
    {
        _ = this.WriteSenseMode( value );
        return this.QuerySenseMode();
    }

    /// <summary> Queries the Sense Action. </summary>
    /// <returns> The <see cref="SenseActionMode">Sense Action</see> or none if unknown. </returns>
    public SenseActionMode? QuerySenseMode()
    {
        string currentValue = this.SenseMode.ToString();
        this.Session.MakeEmulatedReplyIfEmpty( currentValue );
        currentValue = this.Session.QueryTrimEnd( $"_G.print({this.SourceMeasureUnitReference}.sense)" );
        if ( string.IsNullOrWhiteSpace( currentValue ) )
        {
            string message = "Failed fetching Sense Action";
            Debug.Assert( !Debugger.IsAttached, message );
            this.SenseMode = new SenseActionMode?();
        }
        else
        {
            //var se = new StringEnumerator<SenseActionMode>();
            //this.SenseMode = se.ParseContained( currentValue.BuildDelimitedValue() );
            this.SenseMode = SessionBase.ParseContained<SenseActionMode>( currentValue.BuildDelimitedValue() );
        }

        return this.SenseMode;
    }

    /// <summary> Writes the Sense Action without reading back the value from the device. </summary>
    /// <param name="value"> The Sense Action. </param>
    /// <returns> The <see cref="SenseActionMode">Sense Action</see> or none if unknown. </returns>
    public SenseActionMode? WriteSenseMode( SenseActionMode value )
    {
        _ = this.Session.WriteLine( "{0}.sense = {0}.{1}", this.SourceMeasureUnitReference, value.ExtractBetween() );
        this.SenseMode = value;
        return this.SenseMode;
    }

    #endregion
}
/// <summary> Specifies the sense modes. </summary>
public enum SenseActionMode
{
    /// <summary> An enum constant representing the none option. </summary>
    [Description( "None" )]
    None,

    /// <summary> An enum constant representing the remote option. </summary>
    [Description( "Remote (SENSE_REMOTE)" )]
    Remote,

    /// <summary> An enum constant representing the local option. </summary>
    [Description( "Local (SENSE_LOCAL)" )]
    Local
}
