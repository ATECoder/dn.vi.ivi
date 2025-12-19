using System.Timers;
using cc.isr.VI.Pith.ExceptionExtensions;

namespace cc.isr.VI.Pith;

public partial class SessionBase
{
    /// <summary>   Restarts the keep alive timer. </summary>
    /// <remarks>   David, 2021-06-01. </remarks>
    protected void RestartKeepAliveTimer()
    {
        if ( this._lazyKeepAliveTimer.IsValueCreated )
        {
            this.KeepAliveTimer.Stop();
            this.KeepAliveTimer.Start();
        }
    }

    /// <summary>
    /// Gets the keep alive interval. Must be smaller or equal to half the communication timeout
    /// interval.
    /// </summary>
    /// <remarks> Required only with VISA Non-Standard. </remarks>
    /// <value> The keep alive interval. </value>
    public TimeSpan KeepAliveInterval
    {
        get;
        set
        {
            if ( value != this.KeepAliveInterval )
            {
                if ( value == TimeSpan.Zero )
                {
                    if ( this._lazyKeepAliveTimer.IsValueCreated )
                    {
                        this.KeepAliveTimer.Stop();
                    }
                }
                else
                {
                    this.KeepAliveTimer.Stop();
                    this.KeepAliveTimer.Interval = value.TotalMilliseconds;
                    this.KeepAliveTimer.Start();
                }
                field = value;
                this.OnPropertyChanged( new System.ComponentModel.PropertyChangedEventArgs( nameof( this.KeepAliveInterval ) ) );
            }
        }
    } = TimeSpan.Zero;

    private readonly Lazy<System.Timers.Timer> _lazyKeepAliveTimer = new();

    /// <summary>   Gets or sets the keep alive timer. </summary>
    /// <value> The keep alive timer. </value>
    private System.Timers.Timer KeepAliveTimer
    {
        get
        {
            if ( !this._lazyKeepAliveTimer.IsValueCreated )
                this._lazyKeepAliveTimer.Value.Elapsed += this.KeepAliveTimerElapsed;
            return this._lazyKeepAliveTimer.Value;
        }
    }

    /// <summary> Keep alive timer elapsed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Elapsed event information. </param>
    private void KeepAliveTimerElapsed( object? sender, ElapsedEventArgs e )
    {
        try
        {
            if ( sender is System.Timers.Timer tmr )
            {
                this.KeepAlive();
            }
        }
        catch ( Exception ex )
        {
            if ( sender is System.Timers.Timer tmr )
            {
                tmr.Stop();
            }
            _ = ex.AddExceptionData();
            this.OnEventHandlerError( ex );
        }
        finally
        {
        }
    }

    /// <summary>   Gets or sets the keep alive lock timeout. </summary>
    /// <value> The keep alive lock timeout. </value>
    public TimeSpan KeepAliveLockTimeout
    {
        get;
        set => _ = base.SetProperty( ref field, value );
    }

    /// <summary>   Keep alive. </summary>
    /// <remarks>   David, 2021-06-01. </remarks>
    public abstract void KeepAlive();

}
