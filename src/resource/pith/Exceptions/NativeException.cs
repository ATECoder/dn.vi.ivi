namespace cc.isr.VI.Pith;

/// <summary> VISA exception base. </summary>
/// <remarks>
/// (c) 2010 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-11-07; Updated from TSP Library. David, 2010-12-25, 1.0.4011.x.
/// </para>
/// </remarks>
public class NativeException : ExceptionBase
{
    #region " standard constructors "

    /// <summary>
    /// Initializes a new instance of the <see cref="Pith.NativeException"/> class.
    /// </summary>
    public NativeException() : this( "Native VISA exception" )
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Pith.NativeException" /> class.
    /// </summary>
    /// <param name="message"> The message. </param>
    public NativeException( string message ) : base( message ) => this.Timestamp = DateTimeOffset.Now;

    /// <summary>
    /// Initializes a new instance of the <see cref="Pith.NativeException" /> class.
    /// </summary>
    /// <param name="message">        The message. </param>
    /// <param name="innerException"> The inner exception. </param>
    public NativeException( string message, Exception innerException ) : base( message, innerException )
    {
    }

    /// <summary> Initializes a new instance of the class with serialized data. </summary>
    /// <param name="info">    The <see cref="System.Runtime.Serialization.SerializationInfo" />
    /// that holds the serialized object data about the exception being
    /// thrown. </param>
    /// <param name="context"> The <see cref="System.Runtime.Serialization.StreamingContext" />
    /// that contains contextual information about the source or destination.
    /// </param>
#if NET5_0_OR_GREATER
#pragma warning disable CA1041 // Provide ObsoleteAttribute message
[Obsolete( DiagnosticId = "SYSLIB0051" )] // add this attribute to the serialization ctor
#pragma warning restore CA1041 // Provide ObsoleteAttribute message
#endif
    protected NativeException( System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context ) : base( info, context )
    {
        if ( info is null )
        {
            return;
        }

        this.Timestamp = ( DateTimeOffset ) info.GetValue( "Timestamp", typeof( DateTimeOffset ) );
        this.InnerError = ( NativeErrorBase ) info.GetValue( "InnerError", typeof( NativeErrorBase ) );
    }

    /// <summary>
    /// Overrides the <see cref="GetObjectData" /> method to serialize custom values.
    /// </summary>
    /// <param name="info">    The <see cref="System.Runtime.Serialization.SerializationInfo">serialization
    /// information</see>. </param>
    /// <param name="context"> The <see cref="System.Runtime.Serialization.StreamingContext">streaming
    /// context</see> for the exception. </param>
    [System.Security.Permissions.SecurityPermission( System.Security.Permissions.SecurityAction.Demand, SerializationFormatter = true )]
#if NET5_0_OR_GREATER
#pragma warning disable CA1041 // Provide ObsoleteAttribute message
[Obsolete( DiagnosticId = "SYSLIB0051" )] // add this attribute to the serialization ctor
#pragma warning restore CA1041 // Provide ObsoleteAttribute message
#endif
    public override void GetObjectData( System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context )
    {
        if ( info is null )
        {
            return;
        }

        info.AddValue( "Timestamp", this.Timestamp, typeof( DateTimeOffset ) );
        info.AddValue( "InnerError", this.InnerError );
        base.GetObjectData( info, context );
    }

    #endregion

    #region " custom constructors "

    /// <summary> Constructor. </summary>
    /// <param name="innerError"> The inner error. </param>
    public NativeException( NativeErrorBase innerError ) : this( "Native VISA exception" ) => this.InnerError = innerError;

    /// <summary> Constructor. </summary>
    /// <param name="innerError">     The inner error. </param>
    /// <param name="innerException"> The inner exception. </param>
    public NativeException( NativeErrorBase innerError, Exception innerException ) : this( "Native VISA exception", innerException ) => this.InnerError = innerError;

    #endregion

    #region " custom properties "

    /// <summary> Gets or sets the timestamp. </summary>
    /// <value> The timestamp. </value>
    public DateTimeOffset? Timestamp { get; set; }

    /// <summary> Gets or sets the inner error. </summary>
    /// <value> The inner error. </value>
    public NativeErrorBase? InnerError { get; private set; }

    /// <summary> Convert this object into a string representation. </summary>
    /// <returns> A <see cref="string" /> that represents this object. </returns>
    public override string ToString()
    {
        System.Text.StringBuilder builder = new( base.ToString() );
        _ = builder.AppendLine( "Native Error Info:" );
        _ = builder.AppendLine( $"resource: {this.InnerError?.ResourceName};" );
        if ( this.InnerError is not null && this.InnerError.NodeNumber.HasValue )
            _ = builder.AppendLine( $"node: {this.InnerError.NodeNumber.Value};" );
        _ = builder.AppendLine( $"error: {this.InnerError!.BuildErrorCodeDetails()};" );
        _ = builder.AppendLine( $"action: {this.InnerError.LastAction};" );
        _ = builder.AppendLine( $"sent: {this.InnerError.LastMessageSent};" );
        return builder.ToString();
    }

    /// <summary> Adds an exception data. </summary>
    /// <param name="exception"> The exception receiving the added data. </param>
    public void AddExceptionData( Exception exception )
    {
        if ( exception is null ) return;
        this.InnerError?.AddExceptionData( exception );
    }

    #endregion
}
