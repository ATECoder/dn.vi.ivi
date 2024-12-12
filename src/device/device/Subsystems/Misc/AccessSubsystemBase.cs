namespace cc.isr.VI;

/// <summary> Defines the contract that must be implemented by an Output Subsystem. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-12-12, 3.0.5093. </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="OutputSubsystemBase" /> class.
/// </remarks>
/// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
/// subsystem</see>. </param>
[CLSCompliant( false )]
public abstract class AccessSubsystemBase( StatusSubsystemBase statusSubsystem ) : SubsystemBase( statusSubsystem )
{
    #region " i presettable "

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    public override void DefineKnownResetState()
    {
        base.DefineKnownResetState();
        this.CertifyTimeout = TimeSpan.FromMilliseconds( 20000d );
        this.Certified = new bool?();
    }

    #endregion

    #region " api access management "

    private TimeSpan _certifyTimeout;

    /// <summary> Gets or sets the timeout time span allowed for certifying the device. </summary>
    /// <value> The certify timeout. </value>
    public TimeSpan CertifyTimeout
    {
        get => this._certifyTimeout;
        set => this.SetProperty( ref this._certifyTimeout, value );
    }

    private bool? _certified;

    /// <summary> Gets or sets the cached certification sentinel. </summary>
    /// <value>
    /// <c>null</c> if not known; <c>true</c> if certified on; otherwise, <c>false</c>.
    /// </value>
    public bool? Certified
    {
        get => this._certified;
        protected set => this.SetProperty( ref this._certified, value );
    }

    /// <summary>   Certifies. </summary>
    /// <remarks>   2024-11-21. </remarks>
    /// <param name="value">    The value. </param>
    /// <param name="details">  [out] The details. </param>
    /// <returns>
    /// <c>null</c> if not known; <c>true</c> if certified on; otherwise, <c>false</c>.
    /// </returns>
    public abstract bool Certify( string value, out string details );

    /// <summary>   Queries if the instrument is certified for Api access. </summary>
    /// <remarks>   2024-08-24. </remarks>
    /// <param name="serialNumber"> The serial number. </param>
    /// <param name="details">      [out] The details. </param>
    /// <returns>   <c>true</c> if the instrument is certified; otherwise, false. </returns>
    public abstract bool IsCertified( string serialNumber, out string details );

    /// <summary>   Registers the controller instrument. </summary>
    /// <remarks>   2024-08-24. </remarks>
    /// <param name="serialNumber"> The serial number. </param>
    /// <param name="details">      [out] The details. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public abstract bool Register( string serialNumber, out string details );

    /// <summary>   Queries if the controller instrument is registered. </summary>
    /// <remarks>   2024-08-24. </remarks>
    /// <param name="serialNumber"> The serial number. </param>
    /// <param name="details">      [out] The details. </param>
    /// <returns>   <c>true</c> if the instrument is certified; otherwise, false. </returns>
    public abstract bool IsRegistered( string serialNumber, out string details );

    /// <summary> Checks if the custom scripts were loaded successfully. </summary>
    /// <returns> <c>true</c> if loaded; otherwise, <c>false</c>. </returns>
    public abstract bool Loaded();

    #endregion

}
