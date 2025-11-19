using System.Diagnostics;
using cc.isr.Enums;
using cc.isr.Std.ParseExtensions;
using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp;

/// <summary> Defines the contract that must be implemented by a Contact Subsystem. </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="SourceSubsystemBase" /> class.
/// </remarks>
/// <param name="statusSubsystem"> A reference to a <see cref="VI.StatusSubsystemBase">TSP status
/// Subsystem</see>. </param>
public class ContactSubsystemBase( Tsp.StatusSubsystemBase statusSubsystem ) : SourceMeasureUnitBase( statusSubsystem )
{
    #region " i presettable "

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    public override void DefineKnownResetState()
    {
        base.DefineKnownResetState();
        this.ContactCheckOkay = new bool?();
        this.ContactCheckThreshold = new int?();
        this.ContactCheckSpeedMode = new ContactCheckSpeedMode?();
        this.ContactResistances = string.Empty;
    }

    #endregion

    #region " contact check speed mode "

    /// <summary> The Contact Check Speed Mode. </summary>

    /// <summary> Gets or sets the cached Contact Check Speed Mode. </summary>
    /// <value>
    /// The <see cref="ContactCheckSpeedMode">Contact Check Speed Mode</see> or none if not set or
    /// unknown.
    /// </value>
    public ContactCheckSpeedMode? ContactCheckSpeedMode
    {
        get;
        protected set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Writes and reads back the Contact Check Speed Mode. </summary>
    /// <param name="value"> The  Contact Check Speed Mode. </param>
    /// <returns>
    /// The <see cref="ContactCheckSpeedMode">Contact Check Speed Mode</see> or none if unknown.
    /// </returns>
    public ContactCheckSpeedMode? ApplyContactCheckSpeedMode( ContactCheckSpeedMode value )
    {
        _ = this.WriteContactCheckSpeedMode( value );
        return this.QueryContactCheckSpeedMode();
    }

    /// <summary> Queries the Contact Check Speed Mode. </summary>
    /// <returns>
    /// The <see cref="ContactCheckSpeedMode">Contact Check Speed Mode</see> or none if unknown.
    /// </returns>
    public ContactCheckSpeedMode? QueryContactCheckSpeedMode()
    {
        string currentValue = this.ContactCheckSpeedMode.ToString();
        this.Session.MakeEmulatedReplyIfEmpty( currentValue );
        currentValue = this.Session.QueryTrimEnd( $"_G.print({this.SourceMeasureUnitReference}.contact.speed)" );
        if ( string.IsNullOrWhiteSpace( currentValue ) )
        {
            string message = "Failed fetching Contact Check Speed Mode";
            Debug.Assert( !Debugger.IsAttached, message );
            this.ContactCheckSpeedMode = new ContactCheckSpeedMode?();
        }
        else if ( currentValue.TryParseNumber( out double enumValue ) )
        {
            this.ContactCheckSpeedMode = ( cc.isr.VI.Tsp.ContactCheckSpeedMode ) (( int ) enumValue + 1);
        }
        else
        {
            // var se = new StringEnumerator<ContactCheckSpeedMode>();
            // strip the SMU reference.
            // currentValue = currentValue.Substring( currentValue.LastIndexOf( ".", StringComparison.OrdinalIgnoreCase ) + 1 ).Trim( '.' );
            this.ContactCheckSpeedMode = SessionBase.ParseContained<ContactCheckSpeedMode>( currentValue[4..].BuildDelimitedValue() );
            currentValue = currentValue[(currentValue.LastIndexOf( ".", StringComparison.OrdinalIgnoreCase ) + 1)..].Trim( '.' );
            string enumText = currentValue.BuildDelimitedValue();
            // this.ContactCheckSpeedMode = se.ParseContained( currentValue.Substring( 4 ) );
            this.ContactCheckSpeedMode = SessionBase.ParseContained<ContactCheckSpeedMode>( enumText );
        }

        return this.ContactCheckSpeedMode;
    }

    /// <summary>
    /// Writes the Contact Check Speed Mode without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The Contact Check Speed Mode. </param>
    /// <returns>
    /// The <see cref="ContactCheckSpeedMode">Contact Check Speed Mode</see> or none if unknown.
    /// </returns>
    public ContactCheckSpeedMode? WriteContactCheckSpeedMode( ContactCheckSpeedMode value )
    {
        _ = this.Session.WriteLine( "{0}.contact.speed={0}.{1}", this.SourceMeasureUnitReference, value.ExtractBetween() );
        this.ContactCheckSpeedMode = value;
        return this.ContactCheckSpeedMode;
    }

    #endregion

    #region " threshold "

    /// <summary> Gets or sets (Protected) the contact check threshold. </summary>
    /// <value> The contact check threshold. </value>
    public int? ContactCheckThreshold
    {
        get;
        protected set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Programs and reads back the Contact Check Threshold Level. </summary>
    /// <param name="value"> The value. </param>
    /// <returns>
    /// The <see cref="ContactCheckThreshold">Contact Check Threshold</see> or nothing if not known.
    /// </returns>
    public int? ApplyContactCheckThreshold( int value )
    {
        _ = this.WriteContactCheckThreshold( value );
        return this.QueryContactCheckThreshold();
    }

    /// <summary> Reads back the Contact Check Threshold Level. </summary>
    /// <returns>
    /// The <see cref="ContactCheckThreshold">Contact Check Threshold</see> or nothing if not known.
    /// </returns>
    public virtual int? QueryContactCheckThreshold()
    {
        this.ContactCheckThreshold = this.Session.QueryPrint( this.ContactCheckThreshold.GetValueOrDefault( 15 ),
                                                                "{0}.contact.threshold", this.SourceMeasureUnitReference );
        return this.ContactCheckThreshold;
    }

    /// <summary>
    /// Programs the Contact Check Threshold Level without updating the value from the device.
    /// </summary>
    /// <param name="value"> The value. </param>
    /// <returns>
    /// The <see cref="ContactCheckThreshold">Contact Check Threshold</see> or nothing if not known.
    /// </returns>
    public virtual int? WriteContactCheckThreshold( int value )
    {
        _ = this.Session.WriteLine( "{0}.contact.threshold={1}", this.SourceMeasureUnitReference, value );
        this.ContactCheckThreshold = value;
        return this.ContactCheckThreshold;
    }

    #endregion

    #region " resistances "

    /// <summary> The contact resistances. </summary>

    /// <summary> Gets or sets (Protected) the contact resistances. </summary>
    /// <value> The contact resistances. </value>
    public string? ContactResistances
    {
        get;
        protected set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Reads the Contact Resistances. </summary>
    /// <returns>
    /// The <see cref="ContactResistances">Contact Resistances</see> as (high tab low) or nothing if not known.
    /// </returns>
    public virtual string QueryContactResistances()
    {
        this.Session.MakeEmulatedReplyIfEmpty( this.ContactResistances ?? string.Empty );
        this.ContactResistances = this.Session.QueryTrimEnd( $"_G.print({this.SourceMeasureUnitReference}.contact.r())" );
        return this.ContactResistances;
    }

    /// <summary>   Parse contact resistances. </summary>
    /// <remarks>   2025-01-23. </remarks>
    /// <returns>   A Tuple. </returns>
    public (double? highContactResistance, double? lowContactResistance) ParseContactResistances()
    {
        string? contactResistances = this.ContactResistances ?? this.QueryContactResistances();
        if ( string.IsNullOrWhiteSpace( contactResistances ) )
            return (null, null);

        string[] values = contactResistances!.Split( '\t' );
        return (values[0].ExtractNumber(), values[1].ExtractNumber());
    }

    #endregion

    #region " contact check okay "

    /// <summary> ContactCheckOkay. </summary>

    /// <summary> Gets or sets the cached Contact Check Okay sentinel. </summary>
    /// <value>
    /// <c>null</c> if Contact Check Okay is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? ContactCheckOkay
    {
        get;
        protected set => _ = this.SetProperty( ref field, value );
    }

    /// <summary>
    /// Queries the Contact Check status. Also sets the <see cref="ContactCheckOkay">contact
    /// check</see> sentinel.
    /// </summary>
    /// <returns>
    /// <c>null</c> if not known; <c>true</c> if ContactCheckOkay; otherwise, <c>false</c>.
    /// </returns>
    public bool? QueryContactCheckOkay()
    {
        this.Session.MakeTrueFalseReplyIfEmpty( true );
        this.ContactCheckOkay = this.Session.IsStatementTrue( "{0}.contact.check()", this.SourceMeasureUnitReference );
        return this.ContactCheckOkay;
    }

    #region " contact check "

    /// <summary> Determines whether contact resistances are below the specified threshold. </summary>
    /// <param name="threshold"> The threshold. </param>
    /// <returns>
    /// <c>true</c> if passed, <c>false</c> if failed, <c>true</c> if passed. Exception is thrown if
    /// failed configuring contact check.
    /// </returns>
    public bool? CheckContacts( int threshold )
    {
        this.Session.LastNodeNumber = default;
        this.ContactResistances = "-1,-1";
        if ( !threshold.Equals( this.ContactCheckThreshold ) )
        {
            this.Session.SetLastAction( $"writing contact check limit {threshold}" );
            _ = this.WriteContactCheckThreshold( threshold );
            this.Session.ThrowDeviceExceptionIfError();
        }

        if ( !this.ContactCheckSpeedMode.Equals( Tsp.ContactCheckSpeedMode.Fast ) )
        {
            this.Session.SetLastAction( $"writing contact mode {Tsp.ContactCheckSpeedMode.Fast}" );
            _ = this.WriteContactCheckSpeedMode( Tsp.ContactCheckSpeedMode.Fast );
            this.Session.ThrowDeviceExceptionIfError();
        }

        this.Session.SetLastAction( "querying contact check" );
        _ = this.QueryContactCheckOkay();
        this.Session.ThrowDeviceExceptionIfError();
        if ( this.ContactCheckOkay.HasValue && !this.ContactCheckOkay.Value )
        {
            this.Session.SetLastAction( "reading contact check resistance" );
            _ = this.QueryContactResistances();
            this.Session.ThrowDeviceExceptionIfError();
            _ = string.IsNullOrWhiteSpace( this.ContactResistances )
                ? cc.isr.VI.SessionLogger.Instance.LogError( $"Contact check failed;. Failed fetching contact resistances using  '{this.Session.LastMessageSent}'" )
                : cc.isr.VI.SessionLogger.Instance.LogWarning( $"Contact check failed;. Contact resistance {this.ContactResistances} exceeded the limit {this.ContactCheckThreshold}" );
        }

        return this.ContactCheckOkay ?? new bool?();
    }

    #endregion

    #endregion
}
/// <summary> Specifies the contact check speed modes. </summary>
public enum ContactCheckSpeedMode
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "None" )]
    None,

    /// <summary> An enum constant representing the fast option. </summary>
    [System.ComponentModel.Description( "Fast (CONTACT_FAST)" )]
    Fast,

    /// <summary> An enum constant representing the medium option. </summary>
    [System.ComponentModel.Description( "Medium (CONTACT_MEDIUM)" )]
    Medium,

    /// <summary> An enum constant representing the slow option. </summary>
    [System.ComponentModel.Description( "Slow (CONTACT_SLOW)" )]
    Slow
}
