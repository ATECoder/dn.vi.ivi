using cc.isr.Std.TrimExtensions;

namespace cc.isr.VI.Pith;

/// <summary> Encapsulates handling a device reported error. </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
public class DeviceError
{
    #region " constructor "

    /// <summary>
    /// Initializes a new instance of the <see cref="DeviceError" /> class specifying no error.
    /// </summary>
    public DeviceError() : this( VI.Syntax.ScpiSyntax.NoErrorCompoundMessage )
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeviceError" /> class specifying no error.
    /// </summary>
    /// <param name="noErrorCompoundMessage"> Message describing the no error compound. </param>
    public DeviceError( string noErrorCompoundMessage ) : base()
    {
        this.NoErrorCompoundMessage = noErrorCompoundMessage;
        this.CompoundErrorMessage = noErrorCompoundMessage;
        this.ErrorNumber = 0;
        this._errorLevel = 0;
        this.ErrorMessage = VI.Syntax.ScpiSyntax.NoErrorMessage;
        this.Severity = TraceEventType.Verbose;
        this.Timestamp = DateTimeOffset.Now;
    }

    /// <summary> Initializes a new instance of the <see cref="DeviceError" /> class. </summary>
    /// <param name="value"> The value. </param>
    public DeviceError( DeviceError value ) : base()
    {
        if ( value is null )
        {
            this.NoErrorCompoundMessage = VI.Syntax.ScpiSyntax.NoErrorCompoundMessage;
            this.CompoundErrorMessage = VI.Syntax.ScpiSyntax.NoErrorCompoundMessage;
            this.ErrorMessage = VI.Syntax.ScpiSyntax.NoErrorMessage;
            this.ErrorNumber = 0;
        }
        else
        {
            this.NoErrorCompoundMessage = value.NoErrorCompoundMessage;
            this.CompoundErrorMessage = value.CompoundErrorMessage;
            this.ErrorMessage = value.ErrorMessage;
            this.ErrorNumber = value.ErrorNumber;
            this._errorLevel = value.ErrorLevel;
            this.Timestamp = value.Timestamp;
        }
    }

    /// <summary> Gets the no error. </summary>
    /// <value> The no error. </value>
    public static DeviceError NoError => new( VI.Syntax.ScpiSyntax.NoErrorCompoundMessage );

    #endregion

    #region " parse "

    /// <summary> True if error level parsed. </summary>
    private bool _errorLevelParsed;

    /// <summary> Parses the error message. </summary>
    /// <remarks>
    /// TSP2 error: -285,TSP Ieee488Syntax error at line 1: unexpected symbol near `*',level=1 TSP error: -
    /// 285,TSP Ieee488Syntax Error at line 1: unexpected symbol near `*',level=20 SCPI Error: -113,
    /// "Undefined header;1;2018/05/26 14:00:14.871".
    /// </remarks>
    /// <param name="compoundError"> The compound error. </param>
    public virtual void Parse( string compoundError )
    {
        if ( string.IsNullOrWhiteSpace( compoundError ) )
        {
            this.CompoundErrorMessage = string.Empty;
            this.ErrorNumber = 0;
            this.ErrorMessage = string.Empty;
            this.ErrorLevel = 0;
            this.Severity = TraceEventType.Verbose;
            this.Timestamp = DateTimeOffset.Now;
        }
        else
        {
            this.CompoundErrorMessage = compoundError;
            Queue<string> parts = new( compoundError.Split( ',' ) );
            bool localTryParse1()
            {
                _ = this.ErrorNumber;
                bool ret = int.TryParse( compoundError, out int parseResult ); this.ErrorNumber = parseResult; return ret;
            }

            if ( parts.Any() )
            {
                bool localTryParse()
                {
                    _ = this.ErrorNumber;
                    bool ret = int.TryParse( parts.Dequeue(), out int parseResult ); this.ErrorNumber = parseResult; return ret;
                }

                if ( localTryParse() )
                {
                    this.Severity = this.ErrorNumber < 0 ? TraceEventType.Error : this.ErrorNumber > 0 ? TraceEventType.Warning : TraceEventType.Verbose;
                }
                else
                {
                    this.ErrorNumber = int.MinValue;
                    this.Severity = TraceEventType.Error;
                }

                if ( parts.Any() )
                {
                    this.ErrorMessage = parts.Dequeue().Trim().Trim( '"' ).Trim();
                    if ( parts.Any() )
                    {
                        this.ParseErrorLevel( parts.Dequeue() );
                    }
                }
                else
                {
                    this.ErrorMessage = string.Empty;
                }
            }
            else if ( localTryParse1() )
            {
                this.ErrorMessage = compoundError;
            }
            else
            {
                this.ErrorNumber = 0;
                this.ErrorMessage = compoundError;
            }

            this.ParseErrorMessage( this.ErrorMessage );
        }
    }

    /// <summary> Parse error level. </summary>
    /// <param name="message"> The message. </param>
    private void ParseErrorLevel( string message )
    {
        this._errorLevelParsed = false;
        char levelDelimiter = '=';
        string levelPrefix = "level";
        Queue<string> parts = new( message.Split( levelDelimiter ) );
        if ( parts.Any() )
        {
            if ( string.Equals( levelPrefix, parts.Dequeue(), StringComparison.Ordinal ) && parts.Any() )
            {
                bool localTryParse()
                {
                    _ = this.ErrorLevel;
                    bool ret = int.TryParse( parts.Dequeue(), out int parseResult ); this.ErrorLevel = parseResult; return ret;
                }

                if ( localTryParse() )
                {
                    this._errorLevelParsed = true;
                }
                else
                {
                    this.ErrorLevel = 0;
                }
            }
        }
        else
        {
            this.ErrorLevel = 0;
        }
    }

    /// <summary> Parse error message. </summary>
    /// <param name="message"> The message. </param>
    private void ParseErrorMessage( string message )
    {
        char errorDelimiter = ';';
        if ( !string.IsNullOrWhiteSpace( message ) && message.Contains( errorDelimiter.ToString() ) )
        {
            Queue<string> parts = new( message.Split( errorDelimiter ) );
            if ( parts.Any() )
                this.ErrorMessage = parts.Dequeue();
            if ( parts.Any() && int.TryParse( parts.Dequeue(), out int level ) && !this._errorLevelParsed )
            {
                this.ErrorLevel = level;
            }

            bool localTryParse()
            {
                _ = this.Timestamp;
                bool ret = DateTimeOffset.TryParse( parts.Dequeue(), out DateTimeOffset parseResult ); this.Timestamp = parseResult; return ret;
            }

            if ( !(parts.Any() && localTryParse()) )
            {
                this.Timestamp = DateTimeOffset.Now;
            }
        }
    }

    #endregion

    #region " error info "

    /// <summary> Builds error message. </summary>
    /// <returns> A <see cref="string" />. </returns>
    public virtual string BuildErrorMessage()
    {
        return $"{this.ErrorNumber},{this.ErrorMessage}";
    }

    /// <summary> Gets or sets a message describing the no error compound message. </summary>
    /// <value> A message describing the no error compound. </value>
    public string NoErrorCompoundMessage { get; protected set; }

    /// <summary> Gets a value indicating whether the error number represent and error. </summary>
    /// <value> The is error. </value>
    public bool IsError => this.ErrorNumber != 0;

    /// <summary> The error Level. </summary>
    private int _errorLevel;

    /// <summary> Gets or sets (protected) the error Level. </summary>
    /// <value> The error Level. </value>
    public virtual int ErrorLevel
    {
        get => this._errorLevel;

        protected set => this._errorLevel = value;
    }

    /// <summary> Gets or sets (protected) the error number. </summary>
    /// <value> The error number. </value>
    public int ErrorNumber { get; protected set; }

    /// <summary> Gets or sets (protected) the error message. </summary>
    /// <value> A message describing the error. </value>
    public string ErrorMessage { get; protected set; }

    /// <summary> Gets or sets (protected) the compound error message. </summary>
    /// <value> A message describing the compound error. </value>
    public string CompoundErrorMessage { get; protected set; }

    /// <summary> Gets or sets the severity. </summary>
    /// <value> The severity. </value>
    public TraceEventType Severity { get; protected set; }

    /// <summary> Returns a string that represents the current object. </summary>
    /// <returns> A <see cref="string" /> that represents the current object. </returns>
    public override string ToString()
    {
        return this.CompoundErrorMessage;
    }

    /// <summary> Gets or sets the timestamp. </summary>
    /// <value> The timestamp. </value>
    public DateTimeOffset Timestamp { get; protected set; }

    #endregion

    #region " equals "

    /// <summary>
    /// Indicates whether the current <see cref="DeviceError"></see> value is equal to a specified
    /// object.
    /// </summary>
    /// <param name="obj"> An object. </param>
    /// <returns>
    /// <c>true</c> if <paramref name="obj" /> and this instance are the same type and represent the
    /// same value; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals( object obj )
    {
        return this.Equals( obj as DeviceError );
    }

    /// <summary>
    /// Indicates whether the current <see cref="DeviceError"></see> value is equal to a specified
    /// object.
    /// </summary>
    /// <remarks>
    /// The two Parameters are the same if they have the same actual and cached values.
    /// </remarks>
    /// <param name="value"> The value to compare. </param>
    /// <returns>
    /// <c>true</c> if the other parameter is equal to the current
    /// <see cref="DeviceError"></see> value;
    /// otherwise, <c>false</c>.
    /// </returns>
    public bool Equals( DeviceError? value )
    {
        return value is not null && (this.CompoundErrorMessage ?? "") == (value.CompoundErrorMessage ?? "");
    }

    /// <summary> Returns a hash code for this instance. </summary>
    /// <returns> A hash code for this object. </returns>
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    /// <summary> Implements the operator =. </summary>
    /// <param name="left">  The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> The result of the operation. </returns>
    public static bool operator ==( DeviceError left, DeviceError right )
    {
        return ReferenceEquals( left, right ) || (left is not null && left.Equals( right ));
    }

    /// <summary> Implements the not equal operator. </summary>
    /// <param name="left">  The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> The result of the operation. </returns>
    public static bool operator !=( DeviceError left, DeviceError right )
    {
        return !ReferenceEquals( left, right ) && (left is null || !left.Equals( right ));
    }

    #endregion
}

/// <summary> Queue of device errors. </summary>
/// <remarks>
/// (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2016-01-12 </para>
/// </remarks>
/// <remarks> Constructor. </remarks>
/// <param name="noErrorCompoundMessage"> A message describing the empty error message. </param>
public class DeviceErrorQueue( string noErrorCompoundMessage ) : Queue<DeviceError>()
{
    /// <summary> Gets a message describing the no error compound message. </summary>
    /// <value> A message describing the no error compound. </value>
    public string NoErrorCompoundMessage { get; private set; } = noErrorCompoundMessage;

    /// <summary> Gets the last error. </summary>
    /// <value> The last error. </value>
    public DeviceError LastError => this.Count == 0 ? new DeviceError( this.NoErrorCompoundMessage ) : this.ElementAtOrDefault( this.Count - 1 );

    /// <summary>   Returns a string that concatenates all the <see cref="DeviceError"/> items.. </summary>
    /// <remarks>   2024-09-02. </remarks>
    /// <returns>   A string that represents the current object. </returns>
    public override string ToString()
    {
        System.Text.StringBuilder message = new();
        if ( this.Count > 0 )
        {
            foreach ( DeviceError deviceError in this )
                _ = message.AppendLine( deviceError.CompoundErrorMessage );
        }
        return message.ToString().TrimEndNewLine();
    }
}
