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

    /// <summary>   Returns the certification state. </summary>
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

    /// <summary>   Validates the embedded certification value. </summary>
    /// <remarks>   2025-06-02. </remarks>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   True if the certification is embedded and is valid, otherwise, false. </returns>
    public abstract bool ValidateCertification( out string details );

    /// <summary>   Queries if the connected instrument certification code is embedded in the connected instrument. </summary>
    /// <remarks>   2025-06-02. </remarks>
    /// <returns>   <c>true</c> if the instrument certification code is embedded the instrument user string. </returns>
    public abstract bool IsCertificationEmbedded( out string details );

    /// <summary>   Reads certification codes. </summary>
    /// <remarks>   2025-06-02. </remarks>
    /// <returns>   A tuple of the enrollment (release) and certification (version) codes. </returns>
    public abstract (string releaseCode, string versionCode) ReadCertificationCodes();

    /// <summary>   Export certification codes and verify by reading back. </summary>
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

    /// <summary>   Decertifies this instrument. </summary>
    /// <remarks>   2025-11-19. </remarks>
    public abstract void Decertify();

    /// <summary>
    /// Tries to decertify the connected instrument. This does not remove the instrument from the
    /// API roster.
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

    #region " enrollement "

    /// <summary>   Queries if an instrument with the specified serial number is enrolled in the roster. </summary>
    /// <remarks>   2024-08-24. </remarks>
    /// <param name="serialNumber"> The serial number. </param>
    /// <param name="details">      [out] The details. </param>
    /// <returns>   <c>true</c> if the instrument is enrolled; otherwise, false. </returns>
    public abstract bool IsEnrolled( string serialNumber, out string details );

    /// <summary>   Queries if the connected instrument is enrolled in the API roster. </summary>
    /// <remarks>   2024-12-13. </remarks>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   <c>true</c> if the instrument is enrolled in the API roster; otherwise, false. </returns>
    public bool IsEnrolled( out string details )
    {
        return this.IsEnrolled( this.Session.QueryTrimEnd( cc.isr.VI.Syntax.Tsp.LocalNode.SerialNumberFormattedQueryCommand ), out details );
    }

    /// <summary>   Attempts to enroll an instrument with the specified serial number in the API roster. </summary>
    /// <remarks>   2024-12-12. </remarks>
    /// <param name="serialNumber">     The serial number. </param>
    /// <param name="details">          [out] The details. </param>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public abstract bool TryEnroll( string serialNumber, out string details );

    /// <summary>   Attempts to enroll a connected instrument with the specified serial number in the API roster. </summary>
    /// <remarks>   2024-12-13. </remarks>
    /// <param name="details">          [out] The details. </param>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public bool TryEnroll( out string details )
    {
        return this.TryEnroll( this.Session.QueryTrimEnd( cc.isr.VI.Syntax.Tsp.LocalNode.SerialNumberFormattedQueryCommand ), out details );
    }

    /// <summary>   Removes the instrument from the API roster. </summary>
    /// <remarks>   2025-11-19. </remarks>
    /// <param name="serialNumber"> The serial number. </param>
    public abstract void Disenroll( string serialNumber );

    /// <summary>
    /// Attempts to remove an instrument with the specified serial number from the ApI roster without
    /// removing the instrument certification.
    /// </summary>
    /// <remarks>   2024-12-12. </remarks>
    /// <param name="serialNumber"> The serial number. </param>
    /// <param name="details">      [out] The details. </param>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public bool TryDisenroll( string serialNumber, out string details )
    {
        details = string.Empty;
        try
        {
            this.Disenroll( serialNumber );
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
    /// Attempts to remove the current instrument with the specified serial number from the ApI roster without
    /// removing the instrument certification.
    /// </summary>
    /// <remarks>   2024-12-13. </remarks>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public bool TryDisenroll( out string details )
    {
        return this.TryDisenroll( this.Session.QueryTrimEnd( cc.isr.VI.Syntax.Tsp.LocalNode.SerialNumberFormattedQueryCommand ), out details );
    }

    /// <summary>   Validates the enrollment code embedded in the instrument. </summary>
    /// <remarks>   2025-06-02. </remarks>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   True if the enrollment code is embedded and is valid, otherwise, false. </returns>
    public abstract bool ValidateEnrollment( out string details );

    /// <summary>   Queries if an enrollment code is embedded in the connected instrument. </summary>
    /// <remarks>   2025-06-02. </remarks>
    /// <param name="details">      [out] The details. </param>
    /// <returns>   <c>true</c> if the an enrollment code is embedded in the instrument user string. </returns>
    public abstract bool IsEnrollmentEmbedded( out string details );

    #endregion

    #region " registration and certification "

    /// <summary>   Gets a value indicating whether we can enroll in the roster and certify . </summary>
    /// <value> True if we can enroll in the roster and the certify, false if not. </value>
    public abstract bool CanEnrollAndCertify { get; }

    /// <summary>   Queries if the instrument is enrolled in the API roster and certified for API access. </summary>
    /// <remarks>   2025-05-26. </remarks>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   True if enrolled and certified, false if not. </returns>
    public abstract bool IsEnrolledAndCertified( out string details );

    /// <summary>   Queries if the instrument is enrolled in the API roster and certified for API access. </summary>
    /// <remarks>   2025-06-03. </remarks>
    /// <param name="serialNumber"> The serial number. </param>
    /// <param name="details">      [out] The details. </param>
    /// <returns>   True if enrolled and certified, false if not. </returns>
    public bool IsEnrolledAndCertified( string serialNumber, out string details )
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
        return this.IsEnrolled( serialNumber, out details )
            && this.TryCheckCertification( out details );
    }

    /// <summary>   Certify if enrolled. </summary>
    /// <remarks>   2025-11-19. </remarks>
    /// <param name="serialNumber"> The serial number. </param>
    /// <param name="functionName"> Name of the function. </param>
    public abstract void CertifyIfEnrolled( string serialNumber, string functionName );

    /// <summary>   Certify if enrolled. </summary>
    /// <remarks>   2025-11-19. </remarks>
    public abstract void CertifyIfEnrolled();

    /// <summary>   Attempts to certifies the connected instrument for Api access if it is enrolled in the API roster. </summary>
    /// <remarks>   2024-08-24. </remarks>
    /// <param name="details">          [out] The details. </param>
    /// <returns>   <c>true</c> if the instrument is certified; otherwise, false. </returns>
    public virtual bool TryCertifyIfEnrolled( out string details )
    {
        try
        {
            this.CertifyIfEnrolled();
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

    /// <summary>   Enroll the instrument in the API roster and certify it for API access. </summary>
    /// <remarks>   2024-12-11. </remarks>
    public void EnrollAndCertify()
    {
        if ( this.TryEnroll( this.Session.QueryTrimEnd( cc.isr.VI.Syntax.Tsp.LocalNode.SerialNumberFormattedQueryCommand ), out string details ) )
        {
            this.CertifyIfEnrolled();
        }
        else
            throw new InvalidOperationException( details );
    }

    /// <summary>
    /// Attempts to enroll the connected instrument in the API roster and embed its certification code.
    /// </summary>
    /// <remarks>   2025-05-26. </remarks>
    /// <param name="details">          [out] The details. </param>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public virtual bool TryEnrollAndCertify( out string details )
    {
        details = string.Empty;
        try
        {
            this.EnrollAndCertify();
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
