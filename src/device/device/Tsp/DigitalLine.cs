using System.ComponentModel;
using System.Diagnostics;
using cc.isr.Enums;

namespace cc.isr.VI;

/// <summary> A digital line. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-06-29 </para>
/// </remarks>
[CLSCompliant( false )]
public class DigitalLine : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    #region " construction and cleanup "

    /// <summary> Constructor. </summary>
    /// <param name="lineNumber">        The line number. </param>
    /// <param name="supportsReadWrite"> True to supports read write. </param>
    public DigitalLine( int lineNumber, bool supportsReadWrite ) : base()
    {
        this.LineNumber = lineNumber;
        this.DigitalLineReadWriteEnabled = supportsReadWrite;
        this.ResetKnownStateThis();
    }

    /// <summary> Gets or sets the line number. </summary>
    /// <value> The line number. </value>
    public int LineNumber { get; private set; }

    #endregion

    #region " i presettable "

    /// <summary> Resets the known state this. </summary>
    private void ResetKnownStateThis()
    {
        this._digitalLineMode = VI.DigitalLineMode.DigitalInput;
    }

    /// <summary> Sets the known reset (default) state. </summary>
    public void ResetKnownState()
    {
        this.ResetKnownStateThis();
    }

    #endregion

    #region " notify property change implementation "

    /// <summary>   Notifies a property changed. </summary>
    /// <remarks>   David, 2021-02-01. </remarks>
    /// <param name="propertyName"> (Optional) Name of the property. </param>
    protected void NotifyPropertyChanged( [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "" )
    {
        base.OnPropertyChanged( new PropertyChangedEventArgs( propertyName ) );
    }

    #endregion

    #region " digital line mode "

    /// <summary> The digital line mode. </summary>
    private DigitalLineMode? _digitalLineMode;

    /// <summary> Gets or sets the digital line mode. </summary>
    /// <value> The digital line mode. </value>
    public DigitalLineMode? DigitalLineMode
    {
        get => this._digitalLineMode;

        protected set
        {
            if ( !Nullable.Equals( this.DigitalLineMode, value ) )
            {
                this._digitalLineMode = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the digital line Mode. </summary>
    /// <param name="session">       The session. </param>
    /// <param name="commandFormat"> The command format. </param>
    /// <param name="queryCommand">  The query command. </param>
    /// <param name="value">         The  digital line Mode. </param>
    /// <returns>
    /// The <see cref="DigitalLineMode">digital line Mode</see> or none if unknown.
    /// </returns>
    public DigitalLineMode? ApplyDigitalLineMode( Pith.SessionBase session, string commandFormat, string queryCommand, DigitalLineMode value )
    {
        _ = this.WriteDigitalLineMode( session, commandFormat, value );
        _ = this.QueryDigitalLineMode( session, queryCommand );
        return this.DigitalLineMode;
    }

    /// <summary> Queries the digital line Mode. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="queryCommand"> The query command. </param>
    /// <returns>
    /// The <see cref="DigitalLineMode">digital line Mode</see> or none if unknown.
    /// </returns>
    public virtual DigitalLineMode? QueryDigitalLineMode( Pith.SessionBase session, string queryCommand )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !string.IsNullOrWhiteSpace( queryCommand ) )
        {
            string mode = this.DigitalLineMode.ToString();
            session.MakeEmulatedReplyIfEmpty( mode );
            mode = session.QueryTrimEnd( queryCommand );
            if ( string.IsNullOrWhiteSpace( mode ) )
            {
                string message = $"Failed fetching {nameof( DigitalLine )}({this.LineNumber}).{nameof( this.DigitalLineMode )}";
                Debug.Assert( !Debugger.IsAttached, message );
                this.DigitalLineMode = new DigitalLineMode?();
            }
            else
            {
                this.DigitalLineMode = Pith.SessionBase.ParseContained<DigitalLineMode>( mode.BuildDelimitedValue() );
            }
        }

        return this.DigitalLineMode;
    }

    /// <summary>
    /// Writes the digital line Mode without reading back the value from the device.
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="session">       The session. </param>
    /// <param name="commandFormat"> The command format. </param>
    /// <param name="value">         The digital line Mode. </param>
    /// <returns>
    /// The <see cref="DigitalLineMode">digital line Mode</see> or none if unknown.
    /// </returns>
    public virtual DigitalLineMode? WriteDigitalLineMode( Pith.SessionBase session, string commandFormat, DigitalLineMode value )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !string.IsNullOrWhiteSpace( commandFormat ) )
        {
            _ = session.WriteLine( commandFormat, value.ExtractBetween() );
        }

        this.DigitalLineMode = value;
        return this.DigitalLineMode;
    }

    #endregion

    #region " digital line reset "

    /// <summary> Resets the digital line. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="session">       The session. </param>
    /// <param name="commandFormat"> The command format. </param>
    public virtual void ResetDigitalLine( Pith.SessionBase session, string commandFormat )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !string.IsNullOrWhiteSpace( commandFormat ) )
            _ = session.WriteLine( commandFormat );

        this.DigitalLineMode = VI.DigitalLineMode.DigitalInput;
    }

    #endregion

    #region " digital line state "

    /// <summary> True to enable, false to disable the digital line read write. </summary>

    /// <summary> Gets or sets the digital line read write enabled. </summary>
    /// <value>
    /// The digital line read write enabled. <c>true</c> if both read and write are supported and
    /// enabled; <c>false</c> otherwise.
    /// </value>
    public bool DigitalLineReadWriteEnabled
    {
        get;

        protected set
        {
            if ( !Equals( this.DigitalLineReadWriteEnabled, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the digital line lineState. </summary>
    /// <value> The digital line lineState. </value>
    public DigitalLineState? DigitalLineState
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.DigitalLineState, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the digital line lineState. </summary>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <param name="session">       The session. </param>
    /// <param name="commandFormat"> The command format. </param>
    /// <param name="queryCommand">  The query command. </param>
    /// <param name="value">         The  digital line lineState. </param>
    /// <returns>
    /// The <see cref="DigitalLineState">digital line lineState</see> or none if unknown.
    /// </returns>
    public DigitalLineState? ApplyDigitalLineState( Pith.SessionBase session, string commandFormat, string queryCommand, DigitalLineState value )
    {
        if ( this.DigitalLineReadWriteEnabled )
        {
            _ = this.WriteDigitalLineState( session, commandFormat, value );
            _ = this.QueryDigitalLineState( session, queryCommand );
        }
        else
        {
            throw new InvalidOperationException( $"Digital line #{this.LineNumber} does not support Read and WriteEnum" );
        }

        return this.DigitalLineState;
    }

    /// <summary> Queries the digital line lineState. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="queryCommand"> The query command. </param>
    /// <returns>
    /// The <see cref="DigitalLineState">digital line lineState</see> or none if unknown.
    /// </returns>
    public virtual DigitalLineState? QueryDigitalLineState( Pith.SessionBase session, string queryCommand )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !string.IsNullOrWhiteSpace( queryCommand ) )
        {
            string lineState = this.DigitalLineState.ToString();
            session.MakeEmulatedReplyIfEmpty( lineState );
            lineState = session.QueryTrimEnd( queryCommand );
            if ( string.IsNullOrWhiteSpace( lineState ) )
            {
                string message = $"Failed fetching {nameof( DigitalLine )}({this.LineNumber}).{nameof( this.DigitalLineState )}";
                Debug.Assert( !Debugger.IsAttached, message );
                this.DigitalLineState = new DigitalLineState?();
            }
            else
            {
                this.DigitalLineState = Pith.SessionBase.ParseContained<DigitalLineState>( lineState.BuildDelimitedValue() );
            }
        }

        return this.DigitalLineState;
    }

    /// <summary>
    /// Writes the digital line lineState without reading back the value from the device.
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="session">       The session. </param>
    /// <param name="commandFormat"> The command format. </param>
    /// <param name="value">         The digital line lineState. </param>
    /// <returns>
    /// The <see cref="DigitalLineState">digital line lineState</see> or none if unknown.
    /// </returns>
    public virtual DigitalLineState? WriteDigitalLineState( Pith.SessionBase session, string commandFormat, DigitalLineState value )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !string.IsNullOrWhiteSpace( commandFormat ) )
        {
            _ = session.WriteLine( commandFormat, value.ExtractBetween() );
        }

        this.DigitalLineState = value;
        return this.DigitalLineState;
    }

    #endregion
}

/// <summary> Collection of digital lines. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-06-29 </para>
/// </remarks>
[CLSCompliant( false )]
public class DigitalLineCollection : System.Collections.ObjectModel.KeyedCollection<int, DigitalLine>
{
    /// <summary> Constructor. </summary>
    /// <param name="count">            Number of. </param>
    /// <param name="supportReadWrite"> True to support read write. </param>
    public DigitalLineCollection( int count, bool supportReadWrite ) : base() => this.Populate( count, supportReadWrite );

    /// <summary>
    /// When implemented in a derived class, extracts the key from the specified element.
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="item"> The element from which to extract the key. </param>
    /// <returns> The key for the specified element. </returns>
    protected override int GetKeyForItem( DigitalLine item )
    {
        return item is null ? throw new ArgumentNullException( nameof( item ) ) : item.LineNumber;
    }

    /// <summary> Populates. </summary>
    /// <param name="count">            Number of. </param>
    /// <param name="supportReadWrite"> True to support read write. </param>
    private void Populate( int count, bool supportReadWrite )
    {
        for ( int i = 1, loopTo = count; i <= loopTo; i++ )
        {
            try
            {
                DigitalLine line = new( i, supportReadWrite );
                this.Add( line );
            }
            catch
            {
                throw;
            }
            finally
            {
            }
        }
    }
}
/// <summary> Values that represent digital line states. </summary>
public enum DigitalLineState
{
    /// <summary> An enum constant representing the low option. </summary>
    [System.ComponentModel.Description( "Low (digio.STATE_LOW)" )]
    Low,

    /// <summary> An enum constant representing the high option. </summary>
    [System.ComponentModel.Description( "High (digio.STATE_HIGH)" )]
    High
}
/// <summary> Values that represent digital line modes. </summary>
public enum DigitalLineMode
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "None" )]
    None,

    /// <summary> An enum constant representing the digital input option.
    /// The instrument automatically detects externally generated logic
    /// levels. You can read an input line, but you cannot write To it. </summary>
    [System.ComponentModel.Description( "Digital control, input (digio.MODE_DIGITAL_IN)" )]
    DigitalInput,

    /// <summary> An enum constant representing the digital output option.
    /// You can set the line as logic high (+5 V) or as logic low (0 V).
    /// The Default level Is logic low (0 V). When the instrument Is In output mode,
    /// the line Is actively driven high Or low. </summary>
    [System.ComponentModel.Description( "Digital control, output (digio.MODE_DIGITAL_OUT)" )]
    DigitalOutput,

    /// <summary> An enum constant representing the digital open drain option.
    /// The line can serve as an input, an output Or both. When a digital
    /// I/O line is used as an input in open-drain mode, you must write a 1 To it.</summary>
    [System.ComponentModel.Description( "Digital control, open-drain (digio.MODE_DIGITAL_OPEN_DRAIN)" )]
    DigitalOpenDrain,

    /// <summary> An enum constant representing the trigger input option.
    /// The line automatically responds to and detects externally generated
    /// triggers. It detects falling-edge, rising-edge, Or either-edge triggers As input.
    /// This line state uses the edge setting specified by the trigger.digin[N].edge attribute.</summary>
    [System.ComponentModel.Description( "Trigger control, input (digio.MODE_TRIGGER_IN)" )]
    TriggerInput,

    /// <summary> An enum constant representing the trigger output option.
    /// The line is automatically set high or low depending on the output
    /// logic setting. Use the negative logic setting When you want
    /// to generate a falling edge trigger and use the positive logic
    /// setting When you want To generate a rising edge trigger. </summary>
    [System.ComponentModel.Description( "Trigger control, output (digio.MODE_TRIGGER_OUT)" )]
    TriggerOutput,

    /// <summary> An enum constant representing the trigger open drain option.
    /// Configures the line to be an open-drain signal. You can
    /// use the line To detect input triggers Or generate output triggers.
    /// This line state uses the edge setting specified by the trigger.digin[N].edge attribute. </summary>
    [System.ComponentModel.Description( "Trigger control, open-drain (digio.MODE_TRIGGER_OPEN_DRAIN)" )]
    TriggerOpenDrain,

    /// <summary> An enum constant representing the synchronous main option.
    /// When the line Is set as a synchronous main, the line detects
    /// rising-edge triggers as input. For output, the line asserts a TTL-low pulse. </summary>
    [System.ComponentModel.Description( "Synchronous main (digio.MODE_SYNCHRONOUS_MASTER)" )]
    SynchronousMain,

    /// <summary> An enum constant representing the synchronous acceptor option.
    /// When the line is set as a synchronous acceptor, the line detects
    /// the falling-edge input triggers and automatically latches And drives the trigger line low.
    /// Asserting an output trigger releases the latched line. </summary>
    [System.ComponentModel.Description( "Synchronous acceptor (digio.MODE_SYNCHRONOUS_ACCEPTOR)" )]
    SynchronousAcceptor
}
