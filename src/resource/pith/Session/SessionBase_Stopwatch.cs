namespace cc.isr.VI.Pith;

public partial class SessionBase
{
    /// <summary> Gets or sets the elapsed time stop watch. </summary>
    /// <value> The elapsed time stop watch. </value>
    private Stopwatch ElapsedTimeStopwatch { get; set; } = new Stopwatch();

    /// <summary> Reads elapsed time. </summary>
    /// <returns> The elapsed time. </returns>
    public TimeSpan ReadElapsedTime()
    {
        TimeSpan result = this.ElapsedTimeStopwatch.Elapsed;
        this.ElapsedTimeStopwatch.Stop();
        return result;
    }

    /// <summary> Reads elapsed time. </summary>
    /// <param name="stopRequested"> True if stop requested. </param>
    /// <returns> The elapsed time. </returns>
    public TimeSpan ReadElapsedTime( bool stopRequested )
    {
        if ( stopRequested && this.ElapsedTimeStopwatch.IsRunning )
        {
            this._elapsedTimeCount -= 1;
            if ( this.ElapsedTimeCount <= 0 )
                this.ElapsedTimeStopwatch.Stop();
        }

        return this.ElapsedTimeStopwatch.Elapsed;
    }

    /// <summary> Gets or sets the elapsed time. </summary>
    /// <value> The elapsed time. </value>
    public TimeSpan ElapsedTime
    {
        get;
        set
        {
            if ( _ = base.SetProperty( ref field, value ) )
            {
                base.OnPropertyChanged( new System.ComponentModel.PropertyChangedEventArgs( nameof( this.ElapsedTimeCaption ) ) );
            }
        }
    }

    private const string Elapsed_Time_Format_Default = @"s\.ffff";

    /// <summary> Gets or sets the elapsed time format. </summary>
    /// <value> The elapsed time. </value>
    public string ElapsedTimeFormat
    {
        get;
        set
        {
            if ( _ = base.SetProperty( ref field, value ) )
            {
                base.OnPropertyChanged( new System.ComponentModel.PropertyChangedEventArgs( nameof( this.ElapsedTimeCaption ) ) );
            }
        }
    } = @"s\.ffff";

    /// <summary> Gets the elapsed time caption. </summary>
    /// <value> The elapsed time caption. </value>
    public string ElapsedTimeCaption => this.ElapsedTime.ToString( this.ElapsedTimeFormat );

    /// <summary> Number of elapsed times. </summary>
    private int _elapsedTimeCount;

    /// <summary>
    /// Gets or sets the number of elapsed times. Some actions require two cycles to get the full
    /// elapsed time.
    /// </summary>
    /// <value> The number of elapsed times. </value>
    public int ElapsedTimeCount
    {
        get => this._elapsedTimeCount;
        set => _ = base.SetProperty( ref this._elapsedTimeCount, value );
    }

    /// <summary> Starts elapsed stopwatch. </summary>
    public void StartElapsedStopwatch()
    {
        this.StartElapsedStopwatch( 0 );
    }

    /// <summary> Starts elapsed stopwatch. </summary>
    /// <param name="count"> Number of. </param>
    public void StartElapsedStopwatch( int count )
    {
        this._elapsedTimeCount = count;
        this.ElapsedTimeStopwatch.Restart();
    }

}
