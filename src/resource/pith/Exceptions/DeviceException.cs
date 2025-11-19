// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Pith;

/// <summary> Handles device exception including errors retrieved from the device. </summary>
/// <remarks>
/// (c) 2010 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-12-18, 3.0.5100. Updated from TSP Library.  </para><para>
/// David, 2010-12-25, 1.0.4011. </para>
/// </remarks>
public class DeviceException : ExceptionBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="VI.Pith.DeviceException"/> class.
    /// </summary>
    public DeviceException() : this( "Device Exception" )
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VI.Pith.DeviceException" /> class.
    /// </summary>
    /// <param name="message"> The message. </param>
    public DeviceException( string message ) : base( message )
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VI.Pith.DeviceException" /> class.
    /// </summary>
    /// <param name="message">        The message. </param>
    /// <param name="innerException"> The inner exception. </param>
    public DeviceException( string message, Exception innerException ) : base( message, innerException )
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Exception" /> class with serialized
    /// data.
    /// </summary>
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
    protected DeviceException( System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context ) : base( info, context )
    {
        this.ResourceName = info.GetString( "ResourceName" );
        this.LastAction = info.GetString( "LastAction" );
        this.DeviceErrors = info.GetString( "DeviceErrors" );
        this.NodeNumber = ( int? ) info.GetValue( "NodeNumber", typeof( int? ) );
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

        info.AddValue( "ResourceName", this.ResourceName, typeof( string ) );
        info.AddValue( "NodeNumber", this.NodeNumber, typeof( int? ) );
        info.AddValue( "LastAction", this.LastAction, typeof( string ) );
        info.AddValue( "DeviceErrors", this.DeviceErrors, typeof( string ) );
        base.GetObjectData( info, context );
    }

    #endregion

    #region " custom constructors "

    /// <summary> Constructor specifying the Message to be set. </summary>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <param name="format">       Specifies the exception message format. </param>
    /// <param name="args">         Specifies the report argument. </param>
    public DeviceException( string resourceName, string format, params object[] args )
        : base( $"{resourceName} had errors {string.Format( System.Globalization.CultureInfo.CurrentCulture, format, args )}" )
    {
    }

    /// <summary> Constructor specifying the Message to be set. </summary>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <param name="nodeNumber">   The node number. </param>
    /// <param name="format">       Specifies the exception message format. </param>
    /// <param name="args">         Specifies the report argument. </param>
    public DeviceException( string resourceName, int nodeNumber, string format, params object[] args )
        : base( $"{resourceName} node {nodeNumber} had errors {string.Format( System.Globalization.CultureInfo.CurrentCulture, format, args )}" )
    {
    }

    #endregion

    #region " custom properties "

    /// <summary> Gets or sets the name of the resource. </summary>
    /// <value> The name of the resource. </value>
    public string? ResourceName { get; set; }

    /// <summary> Gets or sets the node number. </summary>
    /// <value> The node number. </value>
    public int? NodeNumber { get; set; }

    /// <summary> Gets or sets the last action. </summary>
    /// <value> The last action. </value>
    public string? LastAction { get; set; }

    /// <summary> Gets or sets the device errors. </summary>
    /// <value> The device errors. </value>
    public string? DeviceErrors { get; set; }

    /// <summary> Creates and returns a string representation of the current exception. </summary>
    /// <returns> A <see cref="string" /> representation of the current exception. </returns>
    public override string ToString()
    {
        System.Text.StringBuilder builder = new( base.ToString() );
        _ = builder.AppendLine( "\nDevice error info:" );
        if ( !string.IsNullOrWhiteSpace( this.ResourceName ) )
            _ = builder.AppendLine( $"resource: {this.ResourceName};" );
        if ( this.NodeNumber.HasValue )
            _ = builder.AppendLine( $"node: {this.NodeNumber.Value};" );
        if ( !string.IsNullOrWhiteSpace( this.LastAction ) )
            _ = builder.AppendLine( $"action: {this.LastAction};" );
        if ( !string.IsNullOrWhiteSpace( this.DeviceErrors ) )
            _ = builder.AppendLine( $"errors: {this.DeviceErrors}" );
        return builder.ToString().TrimEnd( Environment.NewLine.ToCharArray() );
    }

    /// <summary> Adds an exception data. </summary>
    /// <param name="exception"> The exception receiving the added data. </param>
    public void AddExceptionData( Exception exception )
    {
        if ( exception is null ) return;

        if ( !string.IsNullOrWhiteSpace( this.ResourceName ) )
            exception.Data.Add( $"{exception.Data.Count}-ResourceName", this.ResourceName );

        if ( this.NodeNumber.HasValue )
            exception.Data.Add( $"{exception.Data.Count}-Node", this.NodeNumber );

        if ( !string.IsNullOrWhiteSpace( this.LastAction ) )
            exception.Data.Add( $"{exception.Data.Count}-LastAction", this.LastAction );

        if ( !string.IsNullOrWhiteSpace( this.DeviceErrors ) )
        {
            int errorIndex = 0;
            foreach ( string s in this.DeviceErrors!.Split( Environment.NewLine.ToCharArray() ) )
            {
                if ( !string.IsNullOrWhiteSpace( s ) )
                    exception.Data.Add( $"{exception.Data.Count}-DeviceError({errorIndex})", s );
                errorIndex += 1;
            }
        }
    }
    #endregion
}
