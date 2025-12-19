namespace cc.isr.VI.Pith;

/// <summary> An inheritable exception for use by framework classes and applications. </summary>
/// <remarks>
/// Inherits from System.Exception per design rule CA1958 which specifies that "Types do not
/// extend inheritance vulnerable types" and further explains that "This [Application Exception]
/// base exception type does not provide any additional value for framework classes.  <para>
/// (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. </para><para>
/// Licensed under The MIT License. </para><para>
/// David, 2014-01-21, x.x.5134. Based on legacy base exception. </para><para>
/// David, 2005-01-15, 1.0.1841 </para>
/// </remarks>
public abstract class ExceptionBase : Exception
{
    #region " construction "

    /// <summary> Initializes a new instance of the <see cref="ExceptionBase" /> class. </summary>
    /// <remarks> David, 2020-09-15. </remarks>
    protected ExceptionBase() : base() => this.ObtainEnvironmentInformation();

    /// <summary> Initializes a new instance of the <see cref="ExceptionBase" /> class. </summary>
    /// <remarks> David, 2020-09-15. </remarks>
    /// <param name="message"> The message. </param>
    protected ExceptionBase( string message ) : base( message ) => this.ObtainEnvironmentInformation();

    /// <summary> Initializes a new instance of the <see cref="ExceptionBase" /> class. </summary>
    /// <remarks> David, 2020-09-15. </remarks>
    /// <param name="message">        The message. </param>
    /// <param name="innerException"> The inner exception. </param>
    protected ExceptionBase( string message, Exception innerException ) : base( message, innerException ) => this.ObtainEnvironmentInformation();

    /// <summary> Initializes a new instance of the <see cref="ExceptionBase" /> class. </summary>
    /// <remarks> David, 2020-09-15. </remarks>
    /// <param name="format"> The format. </param>
    /// <param name="args">   The arguments. </param>
    protected ExceptionBase( string format, params object[] args ) : base( string.Format( System.Globalization.CultureInfo.CurrentCulture, format, args ) ) => this.ObtainEnvironmentInformation();

    /// <summary> Initializes a new instance of the <see cref="ExceptionBase" /> class. </summary>
    /// <remarks> David, 2020-09-15. </remarks>
    /// <param name="innerException"> Specifies the InnerException. </param>
    /// <param name="format">         Specifies the exception formatting. </param>
    /// <param name="args">           Specifies the message arguments. </param>
    protected ExceptionBase( Exception innerException, string format, params object[] args ) : base( string.Format( System.Globalization.CultureInfo.CurrentCulture, format, args ), innerException )
    {
    }

    #endregion

    #region " serialization "

    /// <summary> Initializes a new instance of the class with serialized data. </summary>
    /// <remarks> David, 2020-09-15. </remarks>
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
    protected ExceptionBase( System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context ) : base( info, context )
    {
        if ( info is null )
        {
            return;
        }

        this.MachineName = info.GetString( nameof( this.MachineName ) );
        this.CreatedDateTime = info.GetDateTime( nameof( this.CreatedDateTime ) );
        this.AppDomainName = info.GetString( nameof( this.AppDomainName ) );
        this.ThreadIdentityName = info.GetString( nameof( this.ThreadIdentityName ) );
        this.UserName = info.GetString( nameof( this.UserName ) );
        this.OSVersion = info.GetString( nameof( this.OSVersion ) );
        this.AdditionalInformation = ( System.Collections.Specialized.NameValueCollection ) info.GetValue( nameof( this.AdditionalInformation ), typeof( System.Collections.Specialized.NameValueCollection ) );
    }

    /// <summary>
    /// Overrides the <see cref="GetObjectData" /> method to serialize custom values.
    /// </summary>
    /// <remarks> David, 2020-09-15. </remarks>
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

        info.AddValue( nameof( this.MachineName ), this.MachineName, typeof( string ) );
        info.AddValue( nameof( this.CreatedDateTime ), this.CreatedDateTime );
        info.AddValue( nameof( this.AppDomainName ), this.AppDomainName, typeof( string ) );
        info.AddValue( nameof( this.ThreadIdentityName ), this.ThreadIdentityName, typeof( string ) );
        info.AddValue( nameof( this.UserName ), this.UserName, typeof( string ) );
        info.AddValue( nameof( this.OSVersion ), this.OSVersion, typeof( string ) );
        info.AddValue( nameof( this.AdditionalInformation ), this.AdditionalInformation, typeof( System.Collections.Specialized.NameValueCollection ) );
        base.GetObjectData( info, context );
    }

    #endregion

    #region " additional information "

    /// <summary> Information describing the additional. </summary>

    /// <summary> Collection allowing additional information to be added to the exception. </summary>
    /// <value> The additional information. </value>
    public System.Collections.Specialized.NameValueCollection AdditionalInformation
    {
        get
        {
            field ??= [];

            return field;
        }
    }

    /// <summary> AppDomain name where the Exception occurred. </summary>
    /// <value> The name of the application domain. </value>
    public string? AppDomainName { get; private set; }

    /// <summary> Date and Time <see cref="DateTimeOffset"/> the exception was created. </summary>
    /// <value> The created date time. </value>
    public DateTimeOffset CreatedDateTime { get; private set; } = DateTimeOffset.Now;

    /// <summary> Machine name where the Exception occurred. </summary>
    /// <value> The name of the machine. </value>
    public string? MachineName { get; private set; }

    /// <summary> Gets the OS version. </summary>
    /// <value> The OS version. </value>
    public string? OSVersion { get; private set; }

    /// <summary> Identity of the executing thread on which the exception was created. </summary>
    /// <value> The name of the thread identity. </value>
    public string? ThreadIdentityName { get; private set; }

    /// <summary> User name under which the code was running. </summary>
    /// <value> The User name. </value>
    public string? UserName { get; private set; }

    /// <summary> Gathers environment information safely. </summary>
    /// <remarks> David, 2020-09-15. </remarks>
    private void ObtainEnvironmentInformation()
    {
        const string unknown = "N/A";
        this.CreatedDateTime = DateTimeOffset.Now;
        this.MachineName = Environment.MachineName;
        if ( string.IsNullOrWhiteSpace( this.MachineName ) )
            this.MachineName = unknown;

        this.ThreadIdentityName = System.Threading.Thread.CurrentPrincipal?.Identity?.Name;
        if ( string.IsNullOrWhiteSpace( this.ThreadIdentityName ) )
            this.ThreadIdentityName = unknown;

        this.UserName = Environment.UserName;
        if ( string.IsNullOrWhiteSpace( this.UserName ) )
            this.UserName = unknown;

        this.AppDomainName = AppDomain.CurrentDomain.FriendlyName;
        if ( string.IsNullOrWhiteSpace( this.AppDomainName ) )
            this.AppDomainName = unknown;

        this.OSVersion = Environment.OSVersion.ToString();
        if ( string.IsNullOrWhiteSpace( this.OSVersion ) )
            this.OSVersion = unknown;
    }

    #endregion

    /// <summary> Specifies the contents of the additional information. </summary>
    /// <remarks> David, 2020-09-15. </remarks>
    private enum AdditionalInfoItem
    {
        /// <summary>
        /// The none
        /// </summary>
        None,

        /// <summary>
        /// The machine name
        /// </summary>
        MachineName,

        /// <summary>
        /// The timestamp
        /// </summary>
        Timestamp,

        /// <summary>
        /// The full name
        /// </summary>
        FullName,

        /// <summary>
        /// The application domain name
        /// </summary>
        AppDomainName,

        /// <summary>
        /// The thread identity
        /// </summary>
        ThreadIdentity,

        /// <summary>
        /// The user name
        /// </summary>
        UserName,

        /// <summary>
        /// The OS version
        /// </summary>
        OSVersion
    }
}
