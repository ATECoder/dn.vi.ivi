using System.Diagnostics;

namespace cc.isr.VI;

public partial class VisaSessionBase
{
    /// <summary> Gets or sets the elapsed time stop watch. </summary>
    /// <value> The elapsed time stop watch. </value>
    private Stopwatch ElapsedTimeStopwatch { get; set; } = Stopwatch.StartNew();

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

    /// <summary> The elapsed time. </summary>
    private TimeSpan _elapsedTime;

    /// <summary> Gets or sets the elapsed time. </summary>
    /// <value> The elapsed time. </value>
    public TimeSpan ElapsedTime
    {
        get => this._elapsedTime;
        set
        {
            if ( value != this.ElapsedTime )
            {
                this._elapsedTime = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged( nameof( this.ElapsedTimeCaption ) );
            }
        }
    }

    private const string DefaultElapsedTimeFormat = @"s\.ffff";

    /// <summary> The elapsed time format. </summary>
    private string _elapsedTimeFormat = @"s\.ffff";

    /// <summary> Gets or sets the elapsed time format. </summary>
    /// <value> The elapsed time. </value>
    public string ElapsedTimeFormat
    {
        get => this._elapsedTimeFormat;
        set
        {
            if ( base.SetProperty( ref this._elapsedTimeFormat, value ?? DefaultElapsedTimeFormat ) )
                this.NotifyPropertyChanged( nameof( this.ElapsedTimeCaption ) );
        }
    }

    /// <summary> Gets the elapsed time caption. </summary>
    /// <value> The elapsed time caption. </value>
    public string ElapsedTimeCaption => this._elapsedTime.ToString( this.ElapsedTimeFormat );

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
        set
        {
            if ( value != this.ElapsedTimeCount )
            {
                this._elapsedTimeCount = value;
                this.NotifyPropertyChanged();
            }
        }
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
