using cc.isr.VI.ExceptionExtensions;
using CommunityToolkit.Mvvm.ComponentModel;

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
public abstract partial class AccessSubsystemBase( StatusSubsystemBase statusSubsystem ) : SubsystemBase( statusSubsystem )
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

    #region " certification "

    /// <summary> Gets or sets the timeout time span allowed for certifying the device. </summary>
    /// <value> The certify timeout. </value>
    [ObservableProperty]
    public partial TimeSpan CertifyTimeout { get; set; }

    /// <summary> Gets or sets the cached certification sentinel. </summary>
    /// <value>
    /// <c>null</c> if not known; <c>true</c> if certified on; otherwise, <c>false</c>.
    /// </value>
    [ObservableProperty]
    public partial bool? Certified { get; protected set; }

    /// <summary>   Rerturns the certification state. </summary>
    /// <remarks>   2025-11-19. </remarks>
    /// <returns>   A string. </returns>
    public string CertificationState()
    {
        return this.Certified.HasValue
            ? this.Certified.Value
                ? "Certified for API access."
                : "Not certified for API access."
            : "Certification state is unknown.";
    }

    /// <summary>   Extended certification state. </summary>
    /// <remarks>   2025-11-19. </remarks>
    /// <returns>   A string. </returns>
    public string ExtendedCertificationState()
    {
        return this.Certified.HasValue
                ? this.Certified.Value
                   ? string.Empty
                   : $"The {this.Session.ResourceNameCaption} with serial number {this.StatusSubsystem.VersionInfoBase.SerialNumber} is not certified for API access."
                : "Unable to determine the API certification because the certification script is not loaded.";
    }

    /// <summary>   Checks the instrument certification for API access. </summary>
    /// <remarks>   2025-06-03. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    public abstract void CheckCertification();

    /// <summary>   Attempts to check if the instrument is certified for API access. </summary>
    /// <remarks>   2025-06-03. </remarks>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   True if certified, false if not. </returns>
    public bool TryCheckCertification( out string details )
    {
        try
        {
            this.CheckCertification();
            details = this.Certified.HasValue
                ? this.Certified.Value
                   ? string.Empty
                   : $"The {this.Session.ResourceNameCaption} with serial number {this.StatusSubsystem.VersionInfoBase.SerialNumber} is not certified for API access."
                : "Unable to determine the API certification because the certification script is not loaded.";
        }
        catch ( InvalidOperationException ex )
        {
            details = ex.Message;
        }
        catch ( Exception ex )
        {
            details = ex.BuildMessage();
        }
        return string.IsNullOrWhiteSpace( details );
    }

    /// <summary>   Validates the cached certification value. </summary>
    /// <remarks>   2025-06-02. </remarks>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   True if the certification is cached and is valid, otherwise, false. </returns>
    public abstract bool ValidateCertification( out string details );

    /// <summary>   Queries if the connected instrument certification cached in the connected instrument. </summary>
    /// <remarks>   2025-06-02. </remarks>
    /// <returns>   <c>true</c> if the instrument certification code is cached in the instrument user string. </returns>
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

    /// <summary>   Decertifies this object. </summary>
    /// <remarks>   2025-11-19. </remarks>
    public abstract void Decertify();

    /// <summary>
    /// Tries to decertify the connected instrument. This does not remove the instrument from the
    /// registration dictionary.
    /// </summary>
    /// <remarks>   2024-12-09. </remarks>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public bool TryDecertify( out string details )
    {
        details = string.Empty;
        try
        {
            this.Decertify();
        }
        catch ( InvalidOperationException ex )
        {
            details = ex.Message;
        }
        catch ( Exception ex )
        {
            details = ex.BuildMessage();
        }
        return string.IsNullOrWhiteSpace( details );
    }

    #endregion

    #region " registration "

    /// <summary>   Gets a value indicating whether we can register. </summary>
    /// <value> True if we can register, false if not. </value>
    public abstract bool CanRegister { get; }

    /// <summary>   Queries an instrument with the specified serial number is registered. </summary>
    /// <remarks>   2024-08-24. </remarks>
    /// <param name="serialNumber"> The serial number. </param>
    /// <param name="details">      [out] The details. </param>
    /// <returns>   <c>true</c> if the instrument is certified; otherwise, false. </returns>
    public abstract bool IsRegistered( string serialNumber, out string details );

    /// <summary>   Queries if the connected instrument is registered. </summary>
    /// <remarks>   2024-12-13. </remarks>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   <c>true</c> if the instrument is certified; otherwise, false. </returns>
    public bool IsRegistered( out string details )
    {
        return this.IsRegistered( this.Session.QueryTrimEnd( cc.isr.VI.Syntax.Tsp.LocalNode.SerialNumberFormattedQueryCommand ), out details );
    }

    /// <summary>   Attempts to register an instrument with the specified serial number. </summary>
    /// <remarks>   2024-12-12. </remarks>
    /// <param name="serialNumber">     The serial number. </param>
    /// <param name="details">          [out] The details. </param>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public abstract bool TryRegister( string serialNumber, out string details );

    /// <summary>   Attempts to register a connected instrument instrument. </summary>
    /// <remarks>   2024-12-13. </remarks>
    /// <param name="details">          [out] The details. </param>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public bool TryRegister( out string details )
    {
        return this.TryRegister( this.Session.QueryTrimEnd( cc.isr.VI.Syntax.Tsp.LocalNode.SerialNumberFormattedQueryCommand ), out details );
    }

    /// <summary>   Deregisters this object. </summary>
    /// <remarks>   2025-11-19. </remarks>
    /// <param name="serialNumber"> The serial number. </param>
    public abstract void Deregister( string serialNumber );

    /// <summary>
    /// Attempts to deregister an instrument with the specified serial number by removing this instrument from the
    /// registration dictionary but not removing the API access code from the current instrument.
    /// </summary>
    /// <remarks>   2024-12-12. </remarks>
    /// <param name="serialNumber"> The serial number. </param>
    /// <param name="details">      [out] The details. </param>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public bool TryDeregister( string serialNumber, out string details )
    {
        details = string.Empty;
        try
        {
            this.Deregister( serialNumber );
        }
        catch ( InvalidOperationException ex )
        {
            details = ex.Message;
        }
        catch ( Exception ex )
        {
            details = ex.BuildMessage();
        }
        return string.IsNullOrWhiteSpace( details );
    }

    /// <summary>
    /// Attempts to deregister the active instrument by removing it instrument from the
    /// registration dictionary but not removing the API access code from the current instrument.
    /// </summary>
    /// <remarks>   2024-12-13. </remarks>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public bool TryDeregister( out string details )
    {
        return this.TryDeregister( this.Session.QueryTrimEnd( cc.isr.VI.Syntax.Tsp.LocalNode.SerialNumberFormattedQueryCommand ), out details );
    }

    /// <summary>   Validates the cached registration value. </summary>
    /// <remarks>   2025-06-02. </remarks>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   True if the registration is cached and is valid, otherwise, false. </returns>
    public abstract bool ValidateRegistration( out string details );

    /// <summary>   Queries if the connected instrument registration is cached in the connected instrument. </summary>
    /// <remarks>   2025-06-02. </remarks>
    /// <param name="details">      [out] The details. </param>
    /// <returns>   <c>true</c> if the instrument registration code is cached in the instrument user string. </returns>
    public abstract bool IsRegistrationCached( out string details );

    #endregion

    #region " registration and certification "

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
        return this.IsRegistered( serialNumber, out details )
            && this.TryCheckCertification( out details );
    }

    /// <summary>   Certify if registered. </summary>
    /// <remarks>   2025-11-19. </remarks>
    /// <param name="serialNumber"> The serial number. </param>
    /// <param name="functionName"> Name of the function. </param>
    public abstract void CertifyIfRegistered( string serialNumber, string functionName );

    /// <summary>   Certify if registered. </summary>
    /// <remarks>   2025-11-19. </remarks>
    public abstract void CertifyIfRegistered();

    /// <summary>   Attempts to certifies the connected instrument for Api access if it is registered. </summary>
    /// <remarks>   2024-08-24. </remarks>
    /// <param name="details">          [out] The details. </param>
    /// <returns>   <c>true</c> if the instrument is certified; otherwise, false. </returns>
    public virtual bool TryCertifyIfRegistered( out string details )
    {
        try
        {
            this.CertifyIfRegistered();
            details = this.Certified.GetValueOrDefault( false )
                ? string.Empty
                : "Instrument is not certified for API access.";
        }
        catch ( InvalidOperationException ex )
        {
            details = ex.Message;
        }
        catch ( Exception ex )
        {
            details = ex.BuildMessage();
        }
        return string.IsNullOrWhiteSpace( details );
    }

    /// <summary>   Registers the controller instrument and certifies is for API access. </summary>
    /// <remarks>   2024-12-11. </remarks>
    public void RegisterAndCertify()
    {
        if ( this.TryRegister( this.Session.QueryTrimEnd( cc.isr.VI.Syntax.Tsp.LocalNode.SerialNumberFormattedQueryCommand ), out string details ) )
        {
            this.CertifyIfRegistered();
        }
        else
            throw new InvalidOperationException( details );
    }

    /// <summary>
    /// Attempts to register and certify a connected instrument.
    /// </summary>
    /// <remarks>   2025-05-26. </remarks>
    /// <param name="details">          [out] The details. </param>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public virtual bool TryRegisterAndCertify( out string details )
    {
        details = string.Empty;
        try
        {
            this.RegisterAndCertify();
        }
        catch ( InvalidOperationException ex )
        {
            details = ex.Message;
        }
        catch ( Exception ex )
        {
            details = ex.BuildMessage();
        }
        return string.IsNullOrWhiteSpace( details );
    }

    #endregion

}
