namespace cc.isr.VI.Pith;

/// <summary> An inner error base class. </summary>
/// <remarks>
/// (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2015-11-24 </para>
/// </remarks>
public class NativeErrorBase
{
    #region " construction  "

    /// <summary> Specialized constructor for use only by derived class. </summary>
    /// <param name="errorCode"> The error code. </param>
    protected NativeErrorBase( int errorCode ) : base()
    {
        this.ErrorCode = errorCode;
        this.ResourceName = NativeErrorBase.NotSpecified;
        this.NodeNumber = new int?();
        if ( errorCode == 0 )
        {
            this.ErrorCodeName = "Success";
            this.ErrorCodeDescription = "No Error";
        }
    }

    /// <summary> Specialized constructor for use only by derived class. </summary>
    /// <param name="errorCode">       The error code. </param>
    /// <param name="resourceName">    The name of the resource. </param>
    /// <param name="lastMessageSent"> The last message sent. </param>
    /// <param name="lastAction">      The last visa action. </param>
    protected NativeErrorBase( int errorCode, string? resourceName, string? lastMessageSent, string? lastAction ) : base()
    {
        this.ErrorCode = errorCode;
        this.ResourceName = resourceName ?? NativeErrorBase.NotSpecified;
        this.LastMessageSent = lastMessageSent;
        this.LastAction = lastAction;
    }

    /// <summary> Specialized constructor for use only by derived class. </summary>
    /// <param name="errorCode">       The error code. </param>
    /// <param name="resourceName">    The name of the resource. </param>
    /// <param name="nodeNumber">      The node number. </param>
    /// <param name="lastMessageSent"> The last message sent. </param>
    /// <param name="lastAction">      The last visa action. </param>
    protected NativeErrorBase( int errorCode, string? resourceName, int nodeNumber, string? lastMessageSent, string? lastAction ) : base()
    {
        this.ErrorCode = errorCode;
        this.ResourceName = resourceName ?? NativeErrorBase.NotSpecified;
        this.LastMessageSent = lastMessageSent;
        this.NodeNumber = nodeNumber;
        this.LastAction = lastAction;
    }

    #endregion

    #region " static "

    /// <summary> The success. </summary>

    /// <summary> Gets the success. </summary>
    /// <value> The success. </value>
    public static NativeErrorBase Success
    {
        get
        {
            field ??= new NativeErrorBase( 0 );

            return field;
        }
    }

    #endregion

    private const string NotSpecified = "n/a";

    /// <summary> Gets or sets the error code. </summary>
    /// <value> The error code. </value>
    public int ErrorCode { get; private set; }

    /// <summary> Gets or sets the last action. </summary>
    /// <value> The last action. </value>
    public string? LastAction { get; set; }

    /// <summary> Gets or sets the last message sent. </summary>
    /// <value> The last message sent. </value>
    public string? LastMessageSent { get; private set; }

    /// <summary> Gets or sets the name of the resource. </summary>
    /// <value> The name of the resource. </value>
    public string? ResourceName { get; set; }

    /// <summary> Gets or sets the node number. </summary>
    /// <value> The node number. </value>
    public int? NodeNumber { get; set; }

    /// <summary> Gets or sets the name of the error code. </summary>
    /// <value> The name of the error code. </value>
    public virtual string? ErrorCodeName { get; protected set; }

    /// <summary> Gets or sets information describing the error code. </summary>
    /// <value> Information describing the error code. </value>
    public virtual string? ErrorCodeDescription { get; protected set; }

    #region " error or status details "

    /// <summary> Adds an exception data. </summary>
    /// <param name="exception"> The exception receiving the added data. </param>
    public void AddExceptionData( Exception exception )
    {
        if ( exception is not null && this.ErrorCode != 0 )
        {
            int count = exception.Data.Count;
            if ( this.ErrorCode > 0 )
                exception.Data.Add( $"{count}-Warning", $"0x{this.ErrorCode:X}" );
            else
                exception.Data.Add( $"{count}-Error", $"0x{unchecked(( uint ) this.ErrorCode):X}" );

            if ( !string.IsNullOrWhiteSpace( this.ErrorCodeName ) )
                exception.Data.Add( $"{count}-Name", $"{this.ErrorCodeName}" );

            if ( !string.Equals( this.ErrorCodeName, this.ErrorCodeDescription, StringComparison.OrdinalIgnoreCase ) )
            {
                if ( !string.IsNullOrWhiteSpace( this.ErrorCodeDescription ) )
                    exception.Data.Add( $"{count}-Description", $"{this.ErrorCodeDescription}" );
            }

            if ( !string.IsNullOrWhiteSpace( this.ResourceName ) )
                exception.Data.Add( $"{count}-Resource", $"{this.ResourceName}" );

            if ( this.NodeNumber.HasValue )
                exception.Data.Add( $"{count}-Node", $"{this.NodeNumber}" );

            if ( !string.IsNullOrWhiteSpace( this.LastAction ) )
                exception.Data.Add( $"{count}-LastAction", $"{this.LastAction}" );

            if ( !string.IsNullOrWhiteSpace( this.LastMessageSent ) )
                exception.Data.Add( $"{count}-LastMessageSent", $"{this.LastMessageSent}" );
        }
    }

    /// <summary> Builds an error code or status message. </summary>
    /// <param name="lastAction"> The last visa action. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string BuildErrorCodeDetails( string lastAction )
    {
        return $"{lastAction} {this.BuildErrorCodeDetails()}.";
    }

    /// <summary> Builds an error code or status message. </summary>
    /// <returns> A <see cref="string" />. </returns>
    public string BuildErrorCodeDetails()
    {
        const string innerErrorConstructName = "Native I/O";
        if ( this.ErrorCode == 0 )
            return "OK";
        else
        {
            System.Text.StringBuilder visaMessage = new();
            _ = this.ErrorCode > 0
                ? visaMessage.AppendFormat( "{0} Warning {1:X}/{1}", innerErrorConstructName, this.ErrorCode )
                : visaMessage.AppendFormat( "{0} Error {1:X}/{1}", innerErrorConstructName, unchecked(( uint ) this.ErrorCode) );

            string? name = this.ErrorCodeName;
            string? description = this.ErrorCodeDescription;
            _ = string.Equals( name, description, StringComparison.CurrentCultureIgnoreCase )
                ? visaMessage.AppendFormat( System.Globalization.CultureInfo.CurrentCulture, ": {0}.", name )
                : visaMessage.AppendFormat( System.Globalization.CultureInfo.CurrentCulture, " {0}: {1}.", name, description );

            return visaMessage.ToString();
        }
    }

    #endregion
}
