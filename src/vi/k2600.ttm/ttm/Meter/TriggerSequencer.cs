using System.ComponentModel;
using System.Diagnostics;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary> Triggered Measurement sequencer. </summary>
/// <remarks>
/// (c) 2014 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2014-03-14 </para>
/// </remarks>
public class TriggerSequencer : CommunityToolkit.Mvvm.ComponentModel.ObservableObject, IDisposable
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="isr.VI.Tsp.K2600.Ttm.TriggerSequencer" /> class.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public TriggerSequencer() : base()
    {
        this._triggerStopwatch = new Stopwatch();
        this._signalQueueSyncLocker = new object();
        this.LockedSignalQueue = new Queue<TriggerSequenceSignal>();
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
    ~TriggerSequencer()
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

    /// <summary> Gets or sets the assert requested. </summary>
    /// <value> The assert requested. </value>
    public bool AssertRequested { get; set; }

    /// <summary> Asserts a trigger to emulate triggering for timing measurements. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public void AssertTrigger()
    {
        this.AssertRequested = true;
        this.NotifyPropertyChanged();
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
    }

    /// <summary> Next status bar. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="lastStatusBar"> The last status bar. </param>
    /// <returns> The next status bar. </returns>
    public static string NextStatusBar( string lastStatusBar )
    {
        lastStatusBar = string.IsNullOrEmpty( lastStatusBar )
            ? "|" : lastStatusBar == "|" ? "/" : lastStatusBar == "/" ? "-" : lastStatusBar == "-" ? @"\" : lastStatusBar == @"\" ? "|" : "|";

        return lastStatusBar;
    }

    /// <summary> The trigger stopwatch. </summary>
    private Stopwatch _triggerStopwatch;

    /// <summary> Returns the percent progress. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="lastStatusBar"> The last status bar. </param>
    /// <returns> The status message. </returns>
    public string ProgressMessage( string lastStatusBar )
    {
        string message = NextStatusBar( lastStatusBar );
        switch ( this.TriggerSequenceState )
        {
            case TriggerSequenceState.Aborted:
                {
                    message = "ABORTED";
                    break;
                }

            case TriggerSequenceState.Failed:
                {
                    message = "FAILED";
                    break;
                }

            case TriggerSequenceState.Idle:
                {
                    message = "Inactive";
                    break;
                }

            case TriggerSequenceState.MeasurementCompleted:
                {
                    message = "DATA AVAILABLE";
                    break;
                }

            case var @case when @case == TriggerSequenceState.None:
                {
                    break;
                }

            case TriggerSequenceState.ReadingValues:
                {
                    message = "Reading...";
                    break;
                }

            case TriggerSequenceState.Starting:
                {
                    message = "PREPARING";
                    break;
                }

            case TriggerSequenceState.Stopped:
                {
                    message = "STOPPED";
                    break;
                }

            case TriggerSequenceState.WaitingForTrigger:
                {
                    message = string.Format( @"Waiting for trigger {0:s\.f} s", this._triggerStopwatch.Elapsed );
                    break;
                }

            default:
                break;
        }

        return message;
    }

    /// <summary> Gets or sets the state of the measurement. </summary>
    /// <value> The measurement state. </value>
    public TriggerSequenceState TriggerSequenceState
    {
        get;

        protected set
        {
            if ( TriggerSequenceState.WaitingForTrigger == value || !value.Equals( this.TriggerSequenceState ) )
            {
                field = value;
                this.NotifyPropertyChanged();
                cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
            }
        }
    }

    /// <summary> The signal queue synchronize locker. </summary>
    private readonly object _signalQueueSyncLocker;

    /// <summary> Gets or sets a queue of signals. </summary>
    /// <value> A Queue of signals. </value>
    private Queue<TriggerSequenceSignal> LockedSignalQueue { get; set; }

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
    public bool Dequeue( ref TriggerSequenceSignal signal )
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
    public void Enqueue( TriggerSequenceSignal signal )
    {
        lock ( this._signalQueueSyncLocker )
            this.LockedSignalQueue.Enqueue( signal );
    }

    /// <summary> Starts a measurement sequence. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public void StartMeasurementSequence()
    {
        this.RestartSignal = TriggerSequenceSignal.None;
        this.SequencerTimer ??= new System.Timers.Timer();

        this.SequencerTimer.Enabled = false;
        this.SequencerTimer.Interval = 100d;
        this.ClearSignalQueue();
        this.Enqueue( TriggerSequenceSignal.Step );
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
            cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
            this.TriggerSequenceState = this.ExecuteMeasurementSequence( this.TriggerSequenceState );
        }
        catch ( Exception )
        {
            this.Enqueue( TriggerSequenceSignal.Failure );
        }
        finally
        {
            cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
            this.SequencerTimer.Enabled = this.TriggerSequenceState is not TriggerSequenceState.Idle and not TriggerSequenceState.None;
        }
    }

    /// <summary> Gets or sets the restart signal. </summary>
    /// <value> The restart signal. </value>
    public TriggerSequenceSignal RestartSignal { get; set; }

    /// <summary> Executes the measurement sequence returning the next state. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="currentState"> The current measurement sequence state. </param>
    /// <returns> The next state. </returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0010:Add missing cases", Justification = "<Pending>" )]
    private TriggerSequenceState ExecuteMeasurementSequence( TriggerSequenceState currentState )
    {
        TriggerSequenceSignal signal = default;
        switch ( currentState )
        {
            case TriggerSequenceState.Idle:
                {
                    // Waiting for the step signal to start.
                    if ( this.Dequeue( ref signal ) )
                    {
                        switch ( signal )
                        {
                            case TriggerSequenceSignal.Abort:
                                {
                                    currentState = TriggerSequenceState.Aborted;
                                    break;
                                }

                            case TriggerSequenceSignal.None:
                                {
                                    break;
                                }

                            case TriggerSequenceSignal.Stop:
                                {
                                    break;
                                }

                            case TriggerSequenceSignal.Step:
                                {
                                    currentState = TriggerSequenceState.Starting;
                                    break;
                                }

                            default:
                                break;
                        }
                    }

                    break;
                }

            case TriggerSequenceState.Aborted:
                {
                    // if failed, no action. The sequencer should stop here
                    // wait for the signal to move to the idle state
                    if ( this.Dequeue( ref signal ) )
                    {
                        switch ( signal )
                        {
                            case TriggerSequenceSignal.Abort:
                                {
                                    break;
                                }

                            case TriggerSequenceSignal.Failure:
                                {
                                    currentState = TriggerSequenceState.Failed;
                                    break;
                                }

                            case TriggerSequenceSignal.None:
                                {
                                    break;
                                }

                            case TriggerSequenceSignal.Stop:
                                {
                                    // clear the queue to start a fresh cycle.
                                    this.ClearSignalQueue();
                                    currentState = TriggerSequenceState.Idle;
                                    break;
                                }

                            case TriggerSequenceSignal.Step:
                                {
                                    // clear the queue to start a fresh cycle.
                                    this.ClearSignalQueue();
                                    currentState = TriggerSequenceState.Idle;
                                    break;
                                }

                            default:
                                break;
                        }
                    }

                    break;
                }

            case TriggerSequenceState.Failed:
                {
                    if ( this.Dequeue( ref signal ) )
                    {
                        switch ( signal )
                        {
                            case TriggerSequenceSignal.Abort:
                                {
                                    currentState = TriggerSequenceState.Aborted;
                                    break;
                                }

                            case TriggerSequenceSignal.None:
                                {
                                    break;
                                }

                            case TriggerSequenceSignal.Stop:
                                {
                                    currentState = TriggerSequenceState.Aborted;
                                    break;
                                }

                            case TriggerSequenceSignal.Step:
                                {
                                    currentState = TriggerSequenceState.Aborted;
                                    break;
                                }

                            default:
                                break;
                        }
                    }

                    break;
                }

            case TriggerSequenceState.Stopped:
                {
                    if ( this.Dequeue( ref signal ) )
                    {
                        switch ( signal )
                        {
                            case TriggerSequenceSignal.Abort:
                                {
                                    currentState = TriggerSequenceState.Aborted;
                                    break;
                                }

                            case TriggerSequenceSignal.Failure:
                                {
                                    currentState = TriggerSequenceState.Failed;
                                    break;
                                }

                            case TriggerSequenceSignal.None:
                                {
                                    break;
                                }

                            case TriggerSequenceSignal.Stop:
                                {
                                    currentState = TriggerSequenceState.Idle;
                                    break;
                                }

                            case TriggerSequenceSignal.Step:
                                {
                                    currentState = TriggerSequenceState.Idle;
                                    break;
                                }

                            default:
                                break;
                        }
                    }

                    break;
                }

            case TriggerSequenceState.Starting:
                {
                    if ( this.Dequeue( ref signal ) )
                    {
                        switch ( signal )
                        {
                            case TriggerSequenceSignal.Abort:
                                {
                                    currentState = TriggerSequenceState.Aborted;
                                    break;
                                }

                            case TriggerSequenceSignal.Failure:
                                {
                                    currentState = TriggerSequenceState.Failed;
                                    break;
                                }

                            case TriggerSequenceSignal.None:
                                {
                                    break;
                                }

                            case TriggerSequenceSignal.Stop:
                                {
                                    currentState = TriggerSequenceState.Idle;
                                    break;
                                }

                            case TriggerSequenceSignal.Step:
                                {
                                    this._triggerStopwatch = Stopwatch.StartNew();
                                    currentState = TriggerSequenceState.WaitingForTrigger;
                                    break;
                                }

                            default:
                                break;
                        }
                    }

                    break;
                }

            case TriggerSequenceState.WaitingForTrigger:
                {
                    if ( this.Dequeue( ref signal ) )
                    {
                        switch ( signal )
                        {
                            case TriggerSequenceSignal.Abort:
                            case TriggerSequenceSignal.Failure:
                            case TriggerSequenceSignal.Stop:
                                {
                                    this.RestartSignal = signal;
                                    // request a trigger.
                                    this.AssertRequested = true;
                                    break;
                                }

                            case TriggerSequenceSignal.Step:
                                {
                                    currentState = TriggerSequenceState.MeasurementCompleted;
                                    break;
                                }

                            default:
                                break;
                        }
                    }

                    break;
                }

            case TriggerSequenceState.MeasurementCompleted:
                {
                    if ( this.Dequeue( ref signal ) )
                    {
                        switch ( signal )
                        {
                            case TriggerSequenceSignal.Abort:
                                {
                                    currentState = TriggerSequenceState.Aborted;
                                    break;
                                }

                            case TriggerSequenceSignal.Failure:
                                {
                                    currentState = TriggerSequenceState.Failed;
                                    break;
                                }

                            case TriggerSequenceSignal.None:
                                {
                                    break;
                                }

                            case TriggerSequenceSignal.Stop:
                                {
                                    currentState = TriggerSequenceState.Idle;
                                    break;
                                }

                            case TriggerSequenceSignal.Step:
                                {
                                    if ( this.RestartSignal is TriggerSequenceSignal.None or TriggerSequenceSignal.Step )
                                    {
                                        currentState = TriggerSequenceState.ReadingValues;
                                    }
                                    else
                                    {
                                        switch ( this.RestartSignal )
                                        {
                                            case TriggerSequenceSignal.Abort:
                                                {
                                                    currentState = TriggerSequenceState.Aborted;
                                                    break;
                                                }

                                            case TriggerSequenceSignal.Failure:
                                                {
                                                    currentState = TriggerSequenceState.Failed;
                                                    break;
                                                }

                                            case TriggerSequenceSignal.None:
                                                {
                                                    break;
                                                }

                                            case TriggerSequenceSignal.Stop:
                                                {
                                                    currentState = TriggerSequenceState.Idle;
                                                    break;
                                                }

                                            default:
                                                break;
                                        }

                                        this.RestartSignal = TriggerSequenceSignal.None;
                                    }

                                    break;
                                }

                            default:
                                break;
                        }
                    }

                    break;
                }

            case TriggerSequenceState.ReadingValues:
                {
                    if ( this.Dequeue( ref signal ) )
                    {
                        switch ( signal )
                        {
                            case TriggerSequenceSignal.Abort:
                                {
                                    currentState = TriggerSequenceState.Aborted;
                                    break;
                                }

                            case TriggerSequenceSignal.Failure:
                                {
                                    currentState = TriggerSequenceState.Failed;
                                    break;
                                }

                            case TriggerSequenceSignal.None:
                                {
                                    break;
                                }

                            case TriggerSequenceSignal.Stop:
                                {
                                    currentState = TriggerSequenceState.Idle;
                                    break;
                                }

                            case TriggerSequenceSignal.Step:
                                {
                                    if ( this.RestartSignal is TriggerSequenceSignal.None or TriggerSequenceSignal.Step )
                                    {
                                        currentState = TriggerSequenceState.Starting;
                                    }
                                    else
                                    {
                                        switch ( this.RestartSignal )
                                        {
                                            case TriggerSequenceSignal.Abort:
                                                {
                                                    currentState = TriggerSequenceState.Aborted;
                                                    break;
                                                }

                                            case TriggerSequenceSignal.Failure:
                                                {
                                                    currentState = TriggerSequenceState.Failed;
                                                    break;
                                                }

                                            case TriggerSequenceSignal.None:
                                                {
                                                    break;
                                                }

                                            case TriggerSequenceSignal.Stop:
                                                {
                                                    currentState = TriggerSequenceState.Idle;
                                                    break;
                                                }
                                        }

                                        this.RestartSignal = TriggerSequenceSignal.None;
                                    }

                                    break;
                                }

                            default:
                                break;
                        }
                    }

                    break;
                }

            default:
                {
                    Debug.Assert( !Debugger.IsAttached, "Unhandled state: " + currentState.ToString() );
                    this.Enqueue( TriggerSequenceSignal.Abort );
                    break;
                }
        }

        return currentState;
    }

    #endregion
}

// #Disable Warning CA1027 ' Mark enums with FlagsAttribute
/// <summary> Enumerates the measurement sequence. </summary>
/// <remarks> David, 2020-10-12. </remarks>
public enum TriggerSequenceState
{
    // #Enable Warning CA1027 ' Mark enums with FlagsAttribute

    /// <summary> An enum constant representing the none option. </summary>
    [Description( "Not Defined" )]
    None = 0,

    /// <summary> An enum constant representing the idle option. </summary>
    [Description( "Idle" )]
    Idle = 0,

    /// <summary> An enum constant representing the aborted option. </summary>
    [Description( "Triggered Measurement Sequence Aborted" )]
    Aborted,

    /// <summary> An enum constant representing the failed option. </summary>
    [Description( "Triggered Measurement Sequence Failed" )]
    Failed,

    /// <summary> An enum constant representing the starting option. </summary>
    [Description( "Triggered Measurement Sequence Starting" )]
    Starting,

    /// <summary> An enum constant representing the waiting for trigger option. </summary>
    [Description( "Waiting for Trigger" )]
    WaitingForTrigger,

    /// <summary> An enum constant representing the measurement completed option. </summary>
    [Description( "Measurement Completed" )]
    MeasurementCompleted,

    /// <summary> An enum constant representing the reading values option. </summary>
    [Description( "Reading Values" )]
    ReadingValues,

    /// <summary> An enum constant representing the stopped option. </summary>
    [Description( "Triggered Measurement Sequence Stopped" )]
    Stopped
}
/// <summary> Enumerates the measurement signals. </summary>
/// <remarks> David, 2020-10-12. </remarks>
public enum TriggerSequenceSignal
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

    /// <summary> An enum constant representing the stop] option. </summary>
    [Description( "Stop Measurement" )]
    Stop,

    /// <summary> An enum constant representing the failure] option. </summary>
    [Description( "Report Failure" )]
    Failure
}
