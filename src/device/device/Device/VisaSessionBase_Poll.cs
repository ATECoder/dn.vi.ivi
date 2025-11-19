// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using System.Diagnostics;
using cc.isr.VI.ExceptionExtensions;
using cc.isr.VI.Pith;

namespace cc.isr.VI;

public partial class VisaSessionBase
{
    /// <summary>   Gets or sets the poll timer internal. </summary>
    /// <value> The poll timer internal. </value>
    private System.Timers.Timer? PollTimerInternal
    {
        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        get;

        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        set
        {
            if ( field is not null )
            {
                field.Elapsed -= this.PollTimer_Elapsed;
            }

            field = value;
            if ( field is not null )
            {
                field.Elapsed += this.PollTimer_Elapsed;
            }
        }
    }

    /// <summary> Gets or sets the poll synchronizing object. </summary>
    /// <value> The poll synchronizing object. </value>
    public System.ComponentModel.ISynchronizeInvoke? PollSynchronizingObject
    {
        get => this.PollTimerInternal?.SynchronizingObject; set => this.PollTimerInternal?.SynchronizingObject = value;
    }

    /// <summary> Gets or sets the poll reading. </summary>
    /// <value> The poll reading. </value>
    public string? PollReading
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Gets or sets the automatic read polling option . </summary>
    /// <value> The automatic read status. </value>
    public bool PollAutoRead
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Gets or sets the poll Timespan. </summary>
    /// <value> The poll Timespan. </value>
    public TimeSpan PollTimespan
    {
        get => TimeSpan.FromMilliseconds( this.PollTimerInternal?.Interval ?? 0 );
        set
        {
            if ( this.PollTimerInternal is not null && this.PollTimespan != value )
            {
                this.PollTimerInternal.Interval = value.TotalMilliseconds;
                this.NotifyPropertyChanged();
            }
        }
    }

    private cc.isr.VI.Primitives.ConcurrentToken<bool>? _pollEnabled;

    /// <summary> Gets or sets the poll enabled. </summary>
    /// <value> The poll enabled. </value>
    public bool PollEnabled
    {
        get => this._pollEnabled?.Value ?? false;
        set
        {
            if ( this._pollEnabled is not null && this.PollEnabled != value )
            {
                if ( value )
                {
                    // turn off the poll message available sentinel.
                    this.PollMessageAvailable = false;
                    this.PollStatusByteErrorBitSet = false;

                    // enable only if service request is not enabled.
                    if ( this.PollTimerInternal is not null && this.Session is not null )
                        this.PollTimerInternal.Enabled = !this.Session.ServiceRequestEventEnabled;
                }

                this._pollEnabled.Value = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the poll message available bitmask. </summary>
    /// <value> The message available bitmask. </value>
    public int PollMessageAvailableBitmask
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = ( int ) ServiceRequests.MessageAvailable;

    /// <summary> Queries if a message is available. </summary>
    /// <param name="statusByte"> The status byte. </param>
    /// <returns> <c>true</c> if a message is available; otherwise <c>false</c> </returns>
    private bool IsPollMessageAvailable( int statusByte )
    {
        return SessionBase.IsFullyMasked( statusByte, this.PollMessageAvailableBitmask );
    }

    /// <summary> Gets or sets the poll message available status. </summary>
    /// <value> The poll message available. </value>
    public bool PollMessageAvailable
    {
        get;
        private set => _ = this.SetProperty( ref field, value );
    }

    /// <summary>
    /// Gets or sets a value indicating whether the poll status byte error bit set.
    /// </summary>
    /// <value> True if poll status byte error bit set, false if not. </value>
    public bool PollStatusByteErrorBitSet
    {
        get;
        private set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Event handler. Called by _pollTimer for tick events. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void PollTimer_Elapsed( object? sender, EventArgs e )
    {
        if ( sender is not System.Timers.Timer tmr || !this.IsSessionOpen )
            return;
        try
        {
            // disable the timer.
            tmr.Stop();
            tmr.Enabled = false;
            if ( !this.Session!.ServiceRequestEventEnabled )
            {
                _ = SessionBase.AsyncDelay( this.Session.StatusReadDelay );
                ServiceRequests statusByte = this.Session.ReadStatusByte();
                this.PollStatusByteErrorBitSet = this.Session.IsErrorBitSet( statusByte );
                if ( this.PollStatusByteErrorBitSet )
                {
                    // if error, disable polling
                    this.PollEnabled = false;

                    // let the status subsystem handle the error.
                    this.Session.ApplyStatusByte( this.Session.ErrorAvailableBitmask );
                }

                else if ( !this.PollStatusByteErrorBitSet && this.IsPollMessageAvailable( ( int ) statusByte ) )
                {
                    if ( this.PollAutoRead )
                    {
                        this.PollReading = string.Empty;

                        // allow other threads to execute
                        _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay );

                        // result is also stored in the last message received.
                        this.PollReading = this.Session.ReadFreeLineTrimEnd();

                        // read the status register after delay allowing the status to stabilize.
                        // 20240830: not sure why we are reading the status byte here and
                        // handling the service request. Possibly to handle an error reading,
                        // which is now handled as the code changed to read the status byte and handle
                        // an error if necessary.
                        // _ = this.Session.Delay Read Status Register;
                    }

                    this.PollMessageAvailable = true;
                }
            }
        }
        catch ( Exception ex )
        {
            _ = ex.AddExceptionData();
            Debug.Assert( !Debugger.IsAttached, ex.BuildMessage() );
        }
        finally
        {
            tmr.Enabled = this.PollEnabled && this.Session is not null && !this.Session.ServiceRequestEventEnabled;
        }
    }

    /// <summary> Starts awaiting poll reading task. </summary>
    /// <param name="timeout"> The timeout. </param>
    /// <returns> A Threading.Tasks.Task. </returns>
    public System.Threading.Tasks.Task StartAwaitingPollReadingTask( TimeSpan timeout )
    {
        return System.Threading.Tasks.Task.Factory.StartNew( () =>
        {
            long ticks = timeout.Ticks;
            Stopwatch sw = Stopwatch.StartNew();
            while ( this.PollEnabled && this.PollAutoRead && string.IsNullOrWhiteSpace( this.PollReading ) && sw.ElapsedTicks < ticks )
            {
                System.Threading.Thread.SpinWait( 1 );
                VI.Pith.SessionBase.DoEventsAction?.Invoke();
            }
        } );
    }

    /// <summary> Gets the poll timer enabled. </summary>
    /// <value> The poll timer enabled. </value>
    public bool PollTimerEnabled => this.PollTimerInternal?.Enabled ?? false;

    /// <summary> Starts awaiting poll timer disabled task. </summary>
    /// <param name="timeout"> The timeout. </param>
    /// <returns> A Threading.Tasks.Task. </returns>
    public System.Threading.Tasks.Task StartAwaitingPollTimerDisabledTask( TimeSpan timeout )
    {
        return System.Threading.Tasks.Task.Factory.StartNew( () =>
        {
            long ticks = timeout.Ticks;
            Stopwatch sw = Stopwatch.StartNew();
            while ( this.PollTimerEnabled && sw.ElapsedTicks < ticks )
            {
                System.Threading.Thread.SpinWait( 1 );
                VI.Pith.SessionBase.DoEventsAction?.Invoke();
            }
        } );
    }
}
