// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using System.ComponentModel;

namespace cc.isr.VI.Primitives.BindingLists;

/// <summary> A cross-thread safe binding list. </summary>
/// <remarks>
/// (c) 2019 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2019-01-14.
/// <see href="https://StackOverflow.com/questions/10156991/iNotifyPropertyChanged-causes-cross-thread-error"/>
/// </para>
/// </remarks>
public class InvokingBindingList<T> : BindingList<T>
{
    /// <summary> Default constructor. </summary>
    /// <remarks> David, 2020-09-17. </remarks>
    public InvokingBindingList() : base()
    { }

    /// <summary> Constructor. </summary>
    /// <remarks> David, 2020-09-17. </remarks>
    /// <param name="list"> The list. </param>
    public InvokingBindingList( IList<T> list ) : base( list )
    { }

    /// <summary> Constructor. </summary>
    /// <remarks> David, 2020-09-17. </remarks>
    /// <param name="list">         The list. </param>
    /// <param name="synchronizer"> Provides a way to synchronously or asynchronously execute a
    /// delegate. </param>
    public InvokingBindingList( IList<T> list, ISynchronizeInvoke synchronizer ) : this( list ) => this.Synchronizer = synchronizer;

    /// <summary> Constructor. </summary>
    /// <remarks> David, 2020-09-17. </remarks>
    /// <param name="synchronizer"> Provides a way to synchronously or asynchronously execute a
    /// delegate. </param>
    public InvokingBindingList( ISynchronizeInvoke synchronizer ) : this() => this.Synchronizer = synchronizer;

    /// <summary>
    /// Gets or sets the synchronizer, which provides a way to synchronously or asynchronously
    /// execute a delegate.
    /// </summary>
    /// <remarks>
    /// This allows assigning the synchronizer to the class after it gets instantiated withing a non-
    /// UI class, such as a data entity.
    /// </remarks>
    /// <value> The synchronizer. </value>
    public ISynchronizeInvoke? Synchronizer { get; set; }

    /// <summary>
    /// Raises the <see cref="BindingList{T}.ListChanged" /> event.
    /// </summary>
    /// <remarks> David, 2020-09-17. </remarks>
    /// <param name="e"> A <see cref="ListChangedEventArgs" /> that contains
    /// the event data. </param>
    protected override void OnListChanged( ListChangedEventArgs e )
    {
        if ( this.Synchronizer?.InvokeRequired == true )
        {
            _ = this.Synchronizer?.BeginInvoke( new Action<ListChangedEventArgs>( this.OnListChanged ), [e] );
        }
        else
        {
            base.OnListChanged( e );
        }
    }

    /// <summary> Adds list. </summary>
    /// <remarks> David, 2020-07-20. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="list"> The list. </param>
    public void Add( IList<T> list )
    {
        if ( list is null )
        {
            throw new ArgumentNullException( nameof( list ) );
        }

        bool raiseListChangedEventsWasEnabled = this.RaiseListChangedEvents;
        try
        {
            this.RaiseListChangedEvents = false;
            foreach ( T value in list )
            {
                this.Add( value );
            }

            this.RaiseListChangedEvents = true;
            this.ResetBindings();
        }
        catch
        {
            throw;
        }
        finally
        {
            this.RaiseListChangedEvents = raiseListChangedEventsWasEnabled;
        }
    }
}
