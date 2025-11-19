// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using System.ComponentModel;
using System.Diagnostics;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary> Measure sequencer. </summary>
/// <remarks>
/// (c) 2014 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2014-03-14 </para>
/// </remarks>
public class MeasureSequencer : CommunityToolkit.Mvvm.ComponentModel.ObservableObject, IDisposable
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="isr.VI.Tsp.K2600.Ttm.MeasureSequencer" /> class.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public MeasureSequencer() : base()
    {
        this._measurementOnsetStopwatch = new Stopwatch();
        this._signalQueueSyncLocker = new object();
        this.LockedSignalQueue = new Queue<MeasurementSequenceSignal>();
    }

    #region " i disposable support "

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
    /// resources.
    /// </summary>
    /// <remarks>
    /// Do not make this method Overridable (virtual) because a derived class should not be able to
    /// override this method.
    /// </remarks>
    public void Dispose()
    {
        this.Dispose( true );
        // Take this object off the finalization(Queue) and prevent finalization code
        // from executing a second time.
        GC.SuppressFinalize( this );
    }

    /// <summary> Gets or sets the disposed status. </summary>
    /// <value> The is disposed. </value>
    public bool IsDisposed { get; private set; }

    /// <summary>
    /// Releases the unmanaged resources used by the object and optionally releases the managed resources.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="disposing"> true to release both managed and unmanaged resources; false to
    ///                          release only unmanaged resources. </param>
    [DebuggerNonUserCode()]
    protected virtual void Dispose( bool disposing )
    {
        if ( this.IsDisposed )
            return;
        try
        {
            if ( disposing )
            {
                // Free managed resources when explicitly called
                if ( this.SequencerTimer is not null )
                {
                    this.SequencerTimer.Enabled = false;
                    this.SequencerTimer.Dispose();
                }
            }
        }
        catch ( Exception ex )
        {
            Debug.Assert( !Debugger.IsAttached, ex.ToString() );
        }
        finally
        {
            this.IsDisposed = true;
        }
    }

    /// <summary> Finalizes this object. </summary>
    /// <remarks>
    /// David, 2015-11-21: Override because Dispose(disposing As Boolean) above has code to free
    /// unmanaged resources.
    /// </remarks>
    ~MeasureSequencer()
    {
        // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        this.Dispose( false );
    }

    #endregion

    #endregion

    #region " notify property change implementation "

    /// <summary>   Notifies a property changed. </summary>
    /// <remarks>   David, 2021-02-01. </remarks>
    /// <param name="propertyName"> (Optional) Name of the property. </param>
    protected void NotifyPropertyChanged( [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "" )
    {
        this.OnPropertyChanged( new PropertyChangedEventArgs( propertyName ) );
    }

    /// <summary>   Removes the property changed event handlers. </summary>
    /// <remarks>   David, 2021-06-28. </remarks>
    protected void RemovePropertyChangedEventHandlers()
    {
        MulticastDelegate? event_delegate = ( MulticastDelegate ) this.GetType().GetField( nameof( this.PropertyChanged ),
                                        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic
                                        | System.Reflection.BindingFlags.GetField ).GetValue( this );
        Delegate[]? delegates = event_delegate.GetInvocationList();
        if ( delegates is not null )
        {
            foreach ( Delegate? item in delegates )
            {
                this.PropertyChanged -= ( PropertyChangedEventHandler ) item;
            }
        }
    }

    #endregion

    #region " sequenced measurements "

    /// <summary> Returns the percent progress. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="state"> The state. </param>
    /// <returns> A value between 0 and 100 representing the progress. </returns>
    private static int PercentProgress( MeasurementSequenceState state )
    {
        return ( int ) Math.Round( 100d * Math.Min( 1d, Math.Max( 0d, (( int ) state - ( int ) MeasurementSequenceState.Starting) / ( double ) (( int ) MeasurementSequenceState.Completed - ( int ) MeasurementSequenceState.Starting) ) ) );
    }

    /// <summary> Returns the percent progress. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> A value between 0 and 100 representing the progress. </returns>
    public int PercentProgress()
    {
        return PercentProgress( this.MeasurementSequenceState );
    }

    /// <summary> Gets or sets the state of the measurement. </summary>
    /// <value> The measurement state. </value>
    public MeasurementSequenceState MeasurementSequenceState
    {
        get;

        protected set
        {
            if ( !value.Equals( this.MeasurementSequenceState ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The signal queue synchronization locker. </summary>
    private readonly object _signalQueueSyncLocker;

    /// <summary> Gets or sets a queue of signals. </summary>
    /// <value> A Queue of signals. </value>
    private Queue<MeasurementSequenceSignal> LockedSignalQueue { get; set; }

    /// <summary> Clears the signal queue. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public void ClearSignalQueue()
    {
        lock ( this._signalQueueSyncLocker )
            this.LockedSignalQueue.Clear();
    }

    /// <summary> Dequeues a  signal. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="signal"> [in,out] The signal. </param>
    /// <returns> <c>true</c> if a signal was dequeued. </returns>
    public bool Dequeue( ref MeasurementSequenceSignal signal )
    {
        lock ( this._signalQueueSyncLocker )
        {
            if ( this.LockedSignalQueue.Any() )
            {
                signal = this.LockedSignalQueue.Dequeue();
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary> Enqueues a  signal. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="signal"> The signal. </param>
    public void Enqueue( MeasurementSequenceSignal signal )
    {
        lock ( this._signalQueueSyncLocker )
            this.LockedSignalQueue.Enqueue( signal );
    }

    /// <summary> Starts a measurement sequence. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public void StartMeasurementSequence()
    {
        this.SequencerTimer ??= new System.Timers.Timer();

        this.SequencerTimer.Enabled = false;
        this.SequencerTimer.Interval = 300d;
        this.ClearSignalQueue();
        this.Enqueue( MeasurementSequenceSignal.Step );
        this.SequencerTimer.Enabled = true;
    }

    private System.Timers.Timer? SequencerTimer
    {
        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        get;

        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        set
        {
            if ( field is not null )
                field.Elapsed -= this.SequencerTimer_Elapsed;

            field = value;
            if ( field is not null )
                field.Elapsed += this.SequencerTimer_Elapsed;
        }
    }

    /// <summary> Executes the state sequence. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> Specifies the object where the call originated. </param>
    /// <param name="e">      Event information. </param>
    private void SequencerTimer_Elapsed( object? sender, EventArgs e )
    {
        if ( this.SequencerTimer is null ) return;
        try
        {
            this.SequencerTimer.Enabled = false;
            this.MeasurementSequenceState = this.ExecuteMeasurementSequence( this.MeasurementSequenceState );
        }
        catch ( Exception )
        {
            this.Enqueue( MeasurementSequenceSignal.Failure );
        }
        finally
        {
            this.SequencerTimer.Enabled = this.MeasurementSequenceState is not MeasurementSequenceState.Idle and not MeasurementSequenceState.None;
        }
    }

    /// <summary> The measurement onset stopwatch. </summary>
    private Stopwatch _measurementOnsetStopwatch;

    /// <summary> The measurement onset timespan. </summary>
    private TimeSpan _measurementOnsetTimespan;

    /// <summary> Starts final resistance time. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="pauseTime"> The pause time. </param>
    public void StartFinalResistanceTime( TimeSpan pauseTime )
    {
        this._measurementOnsetStopwatch = Stopwatch.StartNew();
        this._measurementOnsetTimespan = pauseTime;
    }

    /// <summary> Waits for end of post transient delay time. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> true when delay is done. </returns>
    private bool DonePostTransientPause()
    {
        return this._measurementOnsetStopwatch.Elapsed >= this._measurementOnsetTimespan;
    }

    /// <summary> Executes the measurement sequence returning the next state. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="currentState"> The current measurement sequence state. </param>
    /// <returns> The next state. </returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0010:Add missing cases", Justification = "<Pending>" )]
    private MeasurementSequenceState ExecuteMeasurementSequence( MeasurementSequenceState currentState )
    {
        MeasurementSequenceSignal signal = default;
        switch ( currentState )
        {
            case MeasurementSequenceState.Idle:
                {
                    // Waiting for the step signal to start.
                    if ( this.Dequeue( ref signal ) )
                    {
                        switch ( signal )
                        {
                            case MeasurementSequenceSignal.Abort:
                                {
                                    currentState = MeasurementSequenceState.Aborted;
                                    break;
                                }

                            case MeasurementSequenceSignal.None:
                                {
                                    break;
                                }

                            case MeasurementSequenceSignal.Step:
                                {
                                    currentState = MeasurementSequenceState.Starting;
                                    break;
                                }

                            default:
                                break;
                        }
                    }

                    break;
                }

            case MeasurementSequenceState.Aborted:
                {
                    // if failed, no action. The sequencer should stop here
                    // wait for the signal to move to the idle state
                    if ( this.Dequeue( ref signal ) )
                    {
                        switch ( signal )
                        {
                            case MeasurementSequenceSignal.Abort:
                                {
                                    break;
                                }

                            case MeasurementSequenceSignal.Failure:
                                {
                                    currentState = MeasurementSequenceState.Failed;
                                    break;
                                }

                            case MeasurementSequenceSignal.None:
                                {
                                    break;
                                }

                            case MeasurementSequenceSignal.Step:
                                {
                                    // clear the queue to start a fresh cycle.
                                    this.ClearSignalQueue();
                                    currentState = MeasurementSequenceState.Idle;
                                    break;
                                }
                        }
                    }

                    break;
                }

            case MeasurementSequenceState.Failed:
                {
                    if ( this.Dequeue( ref signal ) )
                    {
                        switch ( signal )
                        {
                            case MeasurementSequenceSignal.Abort:
                                {
                                    currentState = MeasurementSequenceState.Aborted;
                                    break;
                                }

                            case MeasurementSequenceSignal.None:
                                {
                                    break;
                                }

                            case MeasurementSequenceSignal.Step:
                                {
                                    currentState = MeasurementSequenceState.Aborted;
                                    break;
                                }
                        }
                    }

                    break;
                }

            case MeasurementSequenceState.Completed:
                {
                    if ( this.Dequeue( ref signal ) )
                    {
                        switch ( signal )
                        {
                            case MeasurementSequenceSignal.Abort:
                                {
                                    currentState = MeasurementSequenceState.Aborted;
                                    break;
                                }

                            case MeasurementSequenceSignal.Failure:
                                {
                                    currentState = MeasurementSequenceState.Failed;
                                    break;
                                }

                            case MeasurementSequenceSignal.None:
                                {
                                    break;
                                }

                            case MeasurementSequenceSignal.Step:
                                {
                                    currentState = MeasurementSequenceState.Idle;
                                    break;
                                }
                        }
                    }

                    break;
                }

            case MeasurementSequenceState.Starting:
                {
                    if ( this.Dequeue( ref signal ) )
                    {
                        switch ( signal )
                        {
                            case MeasurementSequenceSignal.Abort:
                                {
                                    currentState = MeasurementSequenceState.Aborted;
                                    break;
                                }

                            case MeasurementSequenceSignal.Failure:
                                {
                                    currentState = MeasurementSequenceState.Failed;
                                    break;
                                }

                            case MeasurementSequenceSignal.None:
                                {
                                    break;
                                }

                            case MeasurementSequenceSignal.Step:
                                {
                                    currentState = MeasurementSequenceState.MeasureInitialResistance;
                                    break;
                                }
                        }
                    }

                    break;
                }

            case MeasurementSequenceState.MeasureInitialResistance:
                {
                    if ( this.Dequeue( ref signal ) )
                    {
                        switch ( signal )
                        {
                            case MeasurementSequenceSignal.Abort:
                                {
                                    currentState = MeasurementSequenceState.Aborted;
                                    break;
                                }

                            case MeasurementSequenceSignal.Failure:
                                {
                                    currentState = MeasurementSequenceState.Failed;
                                    break;
                                }

                            case MeasurementSequenceSignal.None:
                                {
                                    break;
                                }

                            case MeasurementSequenceSignal.Step:
                                {
                                    currentState = MeasurementSequenceState.MeasureThermalTransient;
                                    break;
                                }
                        }
                    }

                    break;
                }

            case MeasurementSequenceState.MeasureThermalTransient:
                {
                    if ( this.Dequeue( ref signal ) )
                    {
                        switch ( signal )
                        {
                            case MeasurementSequenceSignal.Abort:
                                {
                                    currentState = MeasurementSequenceState.Aborted;
                                    break;
                                }

                            case MeasurementSequenceSignal.Failure:
                                {
                                    currentState = MeasurementSequenceState.Failed;
                                    break;
                                }

                            case MeasurementSequenceSignal.None:
                                {
                                    break;
                                }

                            case MeasurementSequenceSignal.Step:
                                {
                                    currentState = MeasurementSequenceState.PostTransientPause;
                                    break;
                                }
                        }
                    }

                    break;
                }

            case MeasurementSequenceState.PostTransientPause:
                {
                    if ( this.Dequeue( ref signal ) )
                    {
                        switch ( signal )
                        {
                            case MeasurementSequenceSignal.Abort:
                                {
                                    currentState = MeasurementSequenceState.Aborted;
                                    break;
                                }

                            case MeasurementSequenceSignal.Failure:
                                {
                                    currentState = MeasurementSequenceState.Failed;
                                    break;
                                }

                            case MeasurementSequenceSignal.None:
                                {
                                    break;
                                }

                            case MeasurementSequenceSignal.Step:
                                {
                                    currentState = MeasurementSequenceState.MeasureFinalResistance;
                                    break;
                                }
                        }
                    }
                    else if ( this.DonePostTransientPause() )
                    {
                        currentState = MeasurementSequenceState.MeasureFinalResistance;
                    }

                    break;
                }

            case MeasurementSequenceState.MeasureFinalResistance:
                {
                    if ( this.Dequeue( ref signal ) )
                    {
                        switch ( signal )
                        {
                            case MeasurementSequenceSignal.Abort:
                                {
                                    currentState = MeasurementSequenceState.Aborted;
                                    break;
                                }

                            case MeasurementSequenceSignal.Failure:
                                {
                                    currentState = MeasurementSequenceState.Failed;
                                    break;
                                }

                            case MeasurementSequenceSignal.None:
                                {
                                    break;
                                }

                            case MeasurementSequenceSignal.Step:
                                {
                                    currentState = MeasurementSequenceState.Completed;
                                    break;
                                }
                        }
                    }

                    break;
                }

            default:
                {
                    Debug.Assert( !Debugger.IsAttached, "Unhandled state: " + currentState.ToString() );
                    this.Enqueue( MeasurementSequenceSignal.Abort );
                    break;
                }
        }

        return currentState;
    }

    #endregion
}

/// <summary> Enumerates the measurement sequence. </summary>
/// <remarks> David, 2020-10-12. </remarks>
public enum MeasurementSequenceState
{
    /// <summary> An enum constant representing the none option. </summary>
    [Description( "Not Defined" )]
    None = 0,

    /// <summary> An enum constant representing the idle option. </summary>
    [Description( "Idle" )]
    Idle = 0,

    /// <summary> An enum constant representing the aborted option. </summary>
    [Description( "Measurement Sequence Aborted" )]
    Aborted,

    /// <summary> An enum constant representing the failed option. </summary>
    [Description( "Measurement Sequence Failed" )]
    Failed,

    /// <summary> An enum constant representing the starting option. </summary>
    [Description( "Measurement Sequence Starting" )]
    Starting,

    /// <summary> An enum constant representing the measure initial resistance option. </summary>
    [Description( "Measure Initial MeasuredValue" )]
    MeasureInitialResistance,

    /// <summary> An enum constant representing the measure thermal transient option. </summary>
    [Description( "Fetched Initial MeasuredValue" )]
    MeasureThermalTransient,

    /// <summary> An enum constant representing the post transient pause option. </summary>
    [Description( "Post Transient Pause" )]
    PostTransientPause,

    /// <summary> An enum constant representing the measure final resistance option. </summary>
    [Description( "Measure Final MeasuredValue" )]
    MeasureFinalResistance,

    /// <summary> An enum constant representing the completed option. </summary>
    [Description( "Measurement Sequence Completed" )]
    Completed
}

/// <summary> Enumerates the measurement signals. </summary>
/// <remarks> David, 2020-10-12. </remarks>
public enum MeasurementSequenceSignal
{
    /// <summary> An enum constant representing the none option. </summary>
    [Description( "Not Defined" )]
    None = 0,

    /// <summary> An enum constant representing the step] option. </summary>
    [Description( "Step Measurement" )]
    Step,

    /// <summary> An enum constant representing the abort] option. </summary>
    [Description( "Abort Measurement" )]
    Abort,

    /// <summary> An enum constant representing the failure] option. </summary>
    [Description( "Report Failure" )]
    Failure
}
