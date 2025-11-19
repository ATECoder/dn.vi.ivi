// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using System.Diagnostics;
using cc.isr.VI.ExceptionExtensions;
using cc.isr.VI.Subsystem;

namespace cc.isr.VI;

/// <summary> Collection of <see cref="IPresettable"/> elements that might be disposable. </summary>
/// <remarks>
/// The synchronization context is captured as part of the property change and other event
/// handlers and is no longer needed here. <para>
/// (c) 2015 Integrated Scientific Resources, Inc. All rights reserved., Inc. All rights
/// reserved. </para><para>
/// Licensed under The MIT License.</para><para>
/// David, 2015-12-21 </para>
/// </remarks>
[CLSCompliant( false )]
public partial class PresettableCollection : System.Collections.ObjectModel.Collection<IPresettable>
{
    #region " construction "

    /// <summary> Default constructor. </summary>
    public PresettableCollection() : base()
    {
    }

    #endregion

    #region " i presettable "

    /// <summary>
    /// Defines the clear execution state (CLS) by setting system properties to the their Clear
    /// Execution (CLS) default values.
    /// </summary>
    /// <remarks> Clears the queues and sets all registers to zero. </remarks>
    public void DefineClearExecutionState()
    {
        foreach ( IPresettable element in this.Items )
            element.DefineClearExecutionState();
    }

    /// <summary>
    /// Performs a reset and additional custom setting for the subsystem:<para>
    /// </para>
    /// </summary>
    public void InitKnownState()
    {
        foreach ( IPresettable element in this.Items )
            element.InitKnownState();
    }

    /// <summary>
    /// Gets subsystem to the following default system preset values:<para>
    /// </para>
    /// </summary>
    public void PresetKnownState()
    {
        foreach ( IPresettable element in this.Items )
            element.PresetKnownState();
    }

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    /// <remarks> Clears the queues and sets all registers to zero. </remarks>
    public void DefineKnownResetState()
    {
        foreach ( IPresettable element in this.Items )
            element.DefineKnownResetState();
    }

    #endregion

    #region " add "

    /// <summary>
    /// Adds an item to the <see cref="System.Collections.ICollection" />.
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="item"> The object to add to the
    /// <see cref="System.Collections.ICollection" />. </param>
    public new void Add( IPresettable item )
    {
        if ( item is null ) throw new ArgumentNullException( nameof( item ) );
        base.Add( item );
    }

    #endregion

    #region " clear/dispose "

    /// <summary> Disposes items and clear. </summary>
    public new void Clear()
    {
        foreach ( IPresettable element in this.Items )
        {
            try
            {
                (element as IDisposable)?.Dispose();
            }
            catch ( Exception ex )
            {
                Debug.Assert( !Debugger.IsAttached, ex.BuildMessage() );
            }
        }

        base.Clear();
    }

    #endregion
}
