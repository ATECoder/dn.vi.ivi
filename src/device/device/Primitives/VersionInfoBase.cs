namespace cc.isr.VI;

/// <summary> Parses and holds the instrument version information. </summary>
/// <remarks>
/// (c) 2008 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2008-01-15, 2.0.2936. Derived from previous SCPI Instrument implementation. </para>
/// </remarks>
public abstract class VersionInfoBase
{
    #region " construction and cleanup "

    /// <summary> Specialized default constructor for use only by derived classes. </summary>
    protected VersionInfoBase() : base() => this.ClearThis();

    #endregion

    #region " information properties "

    /// <summary>
    /// Gets or sets the identity information that was used to parse the version information.
    /// </summary>
    /// <value> The identity. </value>
    public string Identity { get; set; } = string.Empty;

    /// <summary> Gets or sets the instrument manufacturer name . </summary>
    /// <value> The name of the manufacturer. </value>
    public string ManufacturerName { get; set; } = string.Empty;

    /// <summary> Gets or sets the instrument model number. </summary>
    /// <value>
    /// A <see cref="string" /> property that may include additional precursors such as 'Model' to the relevant
    /// information.
    /// </value>
    public string Model { get; set; } = string.Empty;

    /// <summary> Gets or sets the serial number. </summary>
    /// <value> The serial number. </value>
    public string SerialNumber { get; set; } = string.Empty;

    /// <summary> Gets or sets the list of revision elements. </summary>
    /// <value> The firmware revision elements. </value>
    public System.Collections.Specialized.StringDictionary FirmwareRevisionElements { get; set; } = [];

    #endregion

    #region " parse "

    /// <summary> Clears this object to its blank/initial state. </summary>
    private void ClearThis()
    {
        this.FirmwareRevisionElements = [];
        this.Identity = string.Empty;
        this.FirmwareRevision = string.Empty;
        this.ManufacturerName = string.Empty;
        this.Model = string.Empty;
        this.SerialNumber = string.Empty;
        this.FirmwareVersion = new();
    }

    /// <summary> Clears this object to its blank/initial state. </summary>
    public virtual void Clear()
    {
        this.ClearThis();
    }

    /// <summary> Builds the identity. </summary>
    /// <returns> A <see cref="string" />. </returns>
    public string BuildIdentity()
    {
        System.Text.StringBuilder builder = new();
        _ = string.IsNullOrWhiteSpace( this.ManufacturerName )
            ? builder.Append( "Manufacturer," )
            : builder.Append( $"{this.ManufacturerName}," );

        _ = string.IsNullOrWhiteSpace( this.Model ) ? builder.Append( "Model," ) : builder.Append( $"{this.Model}," );

        _ = string.IsNullOrWhiteSpace( this.SerialNumber ) ? builder.Append( "1," ) : builder.Append( $"{this.SerialNumber}," );

        _ = string.IsNullOrWhiteSpace( this.FirmwareRevision )
            ? builder.Append( "Oct 10 1997" )
            : builder.Append( $"{this.FirmwareRevision}" );

        return builder.ToString();
    }

    /// <summary> Parses the instrument identity string. </summary>
    /// <remarks> The firmware revision can be further interpreted by the child instruments. <para>
    /// KEITHLEY INSTRUMENTS INC.,MODEL 2420,0669977,C11 Oct 10 1997 09:51:36/A02 D/B/E. </para><para>
    /// Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6\n </para></remarks>
    /// <param name="value"> Specifies the instrument identity string, which includes at a minimum the
    /// following information:
    /// <see cref="ManufacturerName">manufacturer</see>,
    /// <see cref="Model">model</see>,
    /// <see cref="SerialNumber">serial number</see> </param>
    public virtual void Parse( string value )
    {
        if ( string.IsNullOrWhiteSpace( value ) )
        {
            this.Clear();
        }
        else
        {
            // save the identity.
            this.Identity = value.Trim();

            // Parse the id to get the revision number
            Queue<string> idItems = new( value.Split( ',' ) );

            // company, e.g., KEITHLEY INSTRUMENTS,
            this.ManufacturerName = idItems.Dequeue().Trim();

            // 24xx: MODEL 2420
            // 7510: MODEL DMM7510
            // 2601A: Model 2601A
            this.Model = idItems.Dequeue().Trim();
            string stripCandidate = "MODEL ";
            if ( this.Model.StartsWith( stripCandidate, StringComparison.OrdinalIgnoreCase ) )
            {
                this.Model = this.Model[stripCandidate.Length..];
            }

            // Serial Number: 0669977
            this.SerialNumber = idItems.Dequeue().Trim();

            // 2002: C11 Oct 10 1997 09:51:36/A02 /D/B/E
            // 7510: 1.0.0i
            this.FirmwareRevision = idItems.Dequeue().Trim(); ;

            // parse thee firmware revision
            this.ParseFirmwareRevision( this.FirmwareRevision );

            // parsed the model families
            (this.ModelFamily, this.ModelFamilyMask) = VersionInfoBase.ParseModelFamily( this.Model, VersionInfoBase.ModelFamilyMasks, VersionInfoBase.ModelFamilies );
        }
    }

    #endregion

    #region " firmware revision "

    /// <summary> Gets or sets the extended version information. </summary>
    /// <value> The firmware revision. </value>
    public string FirmwareRevision { get; set; } = string.Empty;

    /// <summary> Gets or sets the firmware version. </summary>
    /// <value> The firmware version. </value>
    public Version FirmwareVersion { get; set; } = new();

    /// <summary> Parses the instrument firmware revision. </summary>
    /// <exception cref="ArgumentNullException" guarantee="strong"> . </exception>
    /// <param name="revision"> Specifies the instrument revision
    /// e.g., <c>2.1.6</c>. The source meter identity includes no board
    /// specs. </param>
    protected virtual void ParseFirmwareRevision( string revision )
    {
        this.FirmwareRevisionElements = [];
        if ( revision is null )
            throw new ArgumentNullException( nameof( revision ) );
        else if ( string.IsNullOrWhiteSpace( revision ) )
            this.FirmwareVersion = new Version();
        else
        {
            _ = new Version();
            if ( !Version.TryParse( revision, out Version rev ) )
            {
                _ = new Version();
                // 3700 revision is 1.53c.
                string[] values = revision.Split( '.' );
                System.Text.StringBuilder builder = new();
                foreach ( string v in values )
                {
                    string vv = GetNumbers( v );
                    if ( int.TryParse( vv, out int iv ) )
                    {
                        if ( builder.Length > 0 )
                            _ = builder.Append( "." );
                        _ = builder.Append( vv );
                    }

                    if ( Version.TryParse( builder.ToString(), out Version tempRev ) )
                    {
                        rev = Version.Parse( builder.ToString() );
                    }

                    vv = GetNotNumbers( v );
                    if ( !string.IsNullOrWhiteSpace( vv ) )
                    {
                        foreach ( char c in vv.ToUpperInvariant().ToCharArray() )
                        {
                            int value = Convert.ToInt16( c ) - Convert.ToInt16( 'A' ) + 1;
                            if ( builder.Length > 0 )
                                _ = builder.Append( "." );
                            _ = builder.Append( value.ToString() );
                            if ( Version.TryParse( builder.ToString(), out tempRev ) )
                            {
                                rev = Version.Parse( builder.ToString() );
                            }
                        }
                    }
                }
            }

            this.FirmwareVersion = rev;
        }
    }

    /// <summary> Gets the numbers. </summary>
    /// <param name="input"> The input. </param>
    /// <returns> The numbers. </returns>
    private static string GetNumbers( string input )
    {
        return new string( input.Where( char.IsDigit ).ToArray() );
    }

    /// <summary> Gets not numbers. </summary>
    /// <param name="input"> The input. </param>
    /// <returns> The not numbers. </returns>
    private static string GetNotNumbers( string input )
    {
        return new string( input.Where( c => !char.IsDigit( c ) ).ToArray() );
    }

    #endregion

    #region " model family "

    /// <summary>   Gets or sets the model family. </summary>
    /// <value> The model family. </value>
    public string ModelFamily { get; set; } = string.Empty;

    /// <summary>   Gets or sets the model family mask. </summary>
    /// <value> The model family mask. </value>
    public string ModelFamilyMask { get; set; } = string.Empty;

    /// <summary>   Gets or sets the model family masks. </summary>
    /// <value> The model family masks. </value>
    public static string[] ModelFamilyMasks { get; set; } = ["26%%", "26%%A", "26%%B", "37%%", "245%", "65%%", "75%%"];

    /// <summary>   Gets or sets the model families. </summary>
    /// <value> The model families. </value>
    public static string[] ModelFamilies { get; set; } = ["2600", "2600A", "2600B", "3700", "2450", "6500", "7500"];

    /// <summary>
    /// Returns <c>true</c> if the <paramref name="model">model</paramref> matches the mask.
    /// </summary>
    /// <param name="model"> Actual mode. </param>
    /// <param name="mask">  Model mask using '%', e,.g., 26%%, to signify ignored characters and * to specify
    /// wildcard suffix, e.g., 37*. </param>
    /// <returns>
    /// Returns <c>true</c> if the <paramref name="model">model</paramref> matches the mask.
    /// </returns>
    public static bool IsModelMatch( string model, string mask )
    {
        char wildcard = '*';
        char ignore = '%';
        if ( string.IsNullOrWhiteSpace( mask ) )
        {
            return true;
        }
        else if ( string.IsNullOrWhiteSpace( model ) )
        {
            return false;
        }
        else if ( mask.Contains( wildcard.ToString() ) )
        {
            int length = mask.IndexOf( wildcard );
            char[] m = mask[..length].ToCharArray();
            char[] candidate = model[..length].ToCharArray();
            for ( int i = 0, loopTo1 = m.Length - 1; i <= loopTo1; i++ )
            {
                char c = m[i];
                if ( c != ignore && c != candidate[i] )
                {
                    return false;
                }
            }
        }
        else if ( mask.Length != model.Length )
        {
            return false;
        }
        else
        {
            char[] m = mask.ToCharArray();
            char[] candidate = model.ToCharArray();
            for ( int i = 0, loopTo = m.Length - 1; i <= loopTo; i++ )
            {
                char c = m[i];
                if ( c != ignore && c != candidate[i] )
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>   Parses the model number to get the model family. </summary>
    /// <remarks>   2024-11-16. </remarks>
    /// <param name="model">    The model. </param>
    /// <param name="masks">    The masks. </param>
    /// <param name="families"> The families. </param>
    /// <returns>   A Tuple. Returns empty values if the model does not match any of the masks. </returns>
    public static (string familty, string mask) ParseModelFamily( string model, string[] masks, string[] families )
    {
        int modelFamilyIndex = -1;
        foreach ( string item in masks )
        {
            modelFamilyIndex += 1;
            if ( !string.IsNullOrWhiteSpace( item ) && IsModelMatch( model, item ) )
            {
                // verify the iteration is ordered :)
                if ( !string.Equals( item, masks[modelFamilyIndex], StringComparison.Ordinal ) )
                    throw new InvalidOperationException( $"The item '{item}' selected by iteration does not match the indexed item '{masks[modelFamilyIndex]}'." );
                return (families[modelFamilyIndex], masks[modelFamilyIndex]);
            }
        }
        return (string.Empty, string.Empty);
    }

    #endregion

}
/// <summary>
/// Enumerates the instrument board types as defined by the instrument identity.
/// </summary>
public enum FirmwareRevisionElement
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "None" )]
    None,

    /// <summary> An enum constant representing the analog option. </summary>
    [System.ComponentModel.Description( "Analog" )]
    Analog,

    /// <summary> An enum constant representing the digital option. </summary>
    [System.ComponentModel.Description( "Digital" )]
    Digital,

    /// <summary> An enum constant representing the display option. </summary>
    [System.ComponentModel.Description( "Display" )]
    Display,

    /// <summary> An enum constant representing the contact check option. </summary>
    [System.ComponentModel.Description( "Contact Check" )]
    ContactCheck,

    /// <summary> An enum constant representing the LED display option. </summary>
    [System.ComponentModel.Description( "LED Display" )]
    LedDisplay,

    /// <summary> An enum constant representing the primary option. </summary>
    [System.ComponentModel.Description( "Primary" )]
    Primary,

    /// <summary> An enum constant representing the secondary option. </summary>
    [System.ComponentModel.Description( "Secondary" )]
    Secondary,

    /// <summary> An enum constant representing the ternary option. </summary>
    [System.ComponentModel.Description( "Ternary" )]
    Ternary,

    /// <summary> An enum constant representing the quaternary option. </summary>
    [System.ComponentModel.Description( "Quaternary" )]
    Quaternary,

    /// <summary> An enum constant representing the mainframe option. </summary>
    [System.ComponentModel.Description( "Mainframe" )]
    Mainframe,

    /// <summary> An enum constant representing the boot code option. </summary>
    [System.ComponentModel.Description( "Boot code" )]
    BootCode,

    /// <summary> An enum constant representing the front panel option. </summary>
    [System.ComponentModel.Description( "Front panel" )]
    FrontPanel,

    /// <summary> An enum constant representing the internal meter option. </summary>
    [System.ComponentModel.Description( "Internal Meter" )]
    InternalMeter
}
