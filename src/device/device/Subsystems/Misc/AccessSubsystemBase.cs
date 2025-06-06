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

    /// <summary>   Queries if the instrument is certified for API access. </summary>
    /// <remarks>   2025-05-26. </remarks>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   True if certified, false if not. </returns>
    public abstract bool IsCertified( out string details );

    /// <summary>   Queries if the instrument is registered and certified for API access. </summary>
    /// <remarks>   2025-05-26. </remarks>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   True if registered and certified, false if not. </returns>
    public abstract bool IsRegisteredAndCertified( out string details );

    /// <summary>   Queries if the instrument is registered and certified for API access. </summary>
    /// <remarks>   2025-06-03. </remarks>
    /// <param name="serialNumber"> The serial number. </param>
    /// <param name="details">      [out] The details. </param>
    /// <returns>   True if registered and certified, false if not. </returns>
    public bool IsRegisteredAndCertified( string serialNumber, out string details )
    {
        // If the serial number is not specified, use the connected instrument's serial number.
        if ( string.IsNullOrEmpty( serialNumber ) )
            serialNumber = this.StatusSubsystem.VersionInfoBase.SerialNumber;
        if ( string.IsNullOrEmpty( serialNumber ) )
        {
            details = "Serial number is not specified and the connected instrument does not have a serial number.";
            return false;
        }
        else if ( !serialNumber.Equals( this.StatusSubsystem.VersionInfoBase.SerialNumber, StringComparison.Ordinal ) )
        {
            details = $"Serial number '{serialNumber}' does not match the connected instrument's serial number '{this.StatusSubsystem.VersionInfoBase.SerialNumber}'.";
            return false;
        }
        return this.IsRegistered( serialNumber, out details ) && this.IsCertified( out details );
    }

    /// <summary>   Attempts to certifies the connected instrument for Api access if it is registered. </summary>
    /// <remarks>   2024-08-24. </remarks>
    /// <param name="details">          [out] The details. </param>
    /// <returns>   <c>true</c> if the instrument is certified; otherwise, false. </returns>
    public abstract bool TryCertifyIfRegistered( out string details );

    /// <summary>
    /// Attempts to register and certify a connected instrument.
    /// </summary>
    /// <remarks>   2025-05-26. </remarks>
    /// <param name="details">          [out] The details. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public abstract bool TryRegisterAndCertify( out string details );

    /// <summary>
    /// Tries to decertify the connected instrument. This does not remove the instrument from the
    /// registration dictionary.
    /// </summary>
    /// <remarks>   2024-12-09. </remarks>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public abstract bool TryDecertify( out string details );

    /// <summary>   Attempts to register an instrument with the specified serial number. </summary>
    /// <remarks>   2024-12-12. </remarks>
    /// <param name="serialNumber">     The serial number. </param>
    /// <param name="details">          [out] The details. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public abstract bool TryRegister( string serialNumber, out string details );

    /// <summary>
    /// Attempts to deregister an instrument with the specified serial number by removing this instrument from the
    /// registration dictionary but not removing the API access code from the current instrument.
    /// </summary>
    /// <remarks>   2024-12-12. </remarks>
    /// <param name="serialNumber"> The serial number. </param>
    /// <param name="details">      [out] The details. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public abstract bool TryDeregister( string serialNumber, out string details );

    /// <summary>   Queries an instrument with the specified serial number is registered. </summary>
    /// <remarks>   2024-08-24. </remarks>
    /// <param name="serialNumber"> The serial number. </param>
    /// <param name="details">      [out] The details. </param>
    /// <returns>   <c>true</c> if the instrument is certified; otherwise, false. </returns>
    public abstract bool IsRegistered( string serialNumber, out string details );

    /// <summary>   Validates the cached registration value. </summary>
    /// <remarks>   2025-06-02. </remarks>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   True if the registration is cached and is valid, otherwise, false. </returns>
    public abstract bool ValidateRegistration( out string details );

    /// <summary>   Queries if the connected instrument registration is cached in the connected instrument. </summary>
    /// <remarks>   2025-06-02. </remarks>
    /// <param name="details">      [out] The details. </param>
    /// <returns>   <c>true</c> if the instrument registration code is saved in the instrument user string. </returns>
    public abstract bool IsRegistrationCached( out string details );

    /// <summary>   Validates the cached certification value. </summary>
    /// <remarks>   2025-06-02. </remarks>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   True if the certification is cached and is valid, otherwise, false. </returns>
    public abstract bool ValidateCertification( out string details );

    /// <summary>   Queries if the connected instrument certification cached in the connected instrument. </summary>
    /// <remarks>   2025-06-02. </remarks>
    /// <returns>   <c>true</c> if the instrument certification code is saved in the instrument user string. </returns>
    public abstract bool IsCertificationCached( out string details );

    /// <summary>   Reads certification codes. </summary>
    /// <remarks>   2025-06-02. </remarks>
    /// <returns>   The certification codes. </returns>
    public abstract (string releaseCode, string versionCode) ReadCertificationCodes();

    /// <summary>   Export certification codes. </summary>
    /// <remarks>   2025-06-02. </remarks>
    /// <param name="filePath"> Full pathname of the file. </param>
    public abstract void ExportCertificationCodes( string filePath );

    /// <summary>   Restore certification codes. </summary>
    /// <remarks>   2025-06-02. </remarks>
    /// <param name="releaseCode">  The release code. </param>
    /// <param name="versionCode">  The version code. </param>
    public abstract void RestoreCertificationCodes( string releaseCode, string versionCode );

    /// <summary>   Import certification codes. </summary>
    /// <remarks>   2025-06-02. </remarks>
    /// <param name="filePath"> Full pathname of the file. </param>
    public abstract void ImportCertificationCodes( string filePath );

    #endregion

}
