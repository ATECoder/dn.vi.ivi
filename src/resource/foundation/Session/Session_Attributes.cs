namespace cc.isr.VI.Foundation;

public partial class Session
{
    #region " attributes "

    /// <summary> Query if 'value' is supported vendor attribute. </summary>
    /// <remarks> David, 2020-04-14. </remarks>
    /// <param name="value"> The value. </param>
    /// <returns> True if supported vendor attribute, false if not. </returns>
    private static bool IsSupportedVendorAttribute( Ivi.Visa.NativeVisaAttribute value )
    {
        return !VendorUnsupportedAttributes.Contains( value );
    }

    /// <summary> Gets or sets the timeout attribute value. </summary>
    /// <value> The timeout attribute value. </value>
    private int TimeoutAttributeValue { get; set; } = 2000;

    /// <summary> Gets or sets the timeout attribute. </summary>
    /// <value> The timeout attribute. </value>
    public int TimeoutAttribute
    {
        get
        {
            this.TimeoutAttributeValue = this.ReadAttribute( Ivi.Visa.NativeVisaAttribute.TimeoutValue, this.TimeoutAttributeValue );
            return this.TimeoutAttributeValue;
        }

        set
        {
            this.TimeoutAttributeValue = value;
            this.WriteAttribute( Ivi.Visa.NativeVisaAttribute.TimeoutValue, value );
        }
    }

    /// <summary> Gets or sets the default buffer size attribute. </summary>
    /// <value> The default buffer size attribute. </value>
    private int DefaultBufferSizeAttribute { get; set; } = 1024;

    /// <summary> Gets read buffer size. </summary>
    /// <remarks> David, 2020-04-14. </remarks>
    /// <returns> The read buffer size. </returns>
    private int ReadBufferSizeAttribute()
    {
        return this.VisaSession is Ivi.Visa.INativeVisaSession s ? s.GetAttributeInt32( Ivi.Visa.NativeVisaAttribute.ReadBufferSize ) : this.DefaultBufferSizeAttribute;
    }

    #endregion

    #region " attribute read write "

    /// <summary> Query if 'visaSession' 'Attribute' is supported. </summary>
    /// <param name="nativeVisaSession"> A native visa session. </param>
    /// <param name="attribute">         The attribute. </param>
    /// <returns> <c>true</c> if supported; otherwise <c>false</c> </returns>
    private static bool IsSupported( Ivi.Visa.INativeVisaSession nativeVisaSession, Ivi.Visa.NativeVisaAttribute attribute )
    {
        bool result = false;
        if ( nativeVisaSession is not null )
        {
            result = IsSupportedVendorAttribute( attribute );
            if ( result && nativeVisaSession.HardwareInterfaceType != Ivi.Visa.HardwareInterfaceType.Gpib && attribute.ToString().StartsWith( Ivi.Visa.HardwareInterfaceType.Gpib.ToString(), StringComparison.OrdinalIgnoreCase ) )
            {
                result = false;
            }

            if ( result && nativeVisaSession.HardwareInterfaceType != Ivi.Visa.HardwareInterfaceType.Pxi && attribute.ToString().StartsWith( Ivi.Visa.HardwareInterfaceType.Pxi.ToString(), StringComparison.OrdinalIgnoreCase ) )
            {
                result = false;
            }

            if ( result && nativeVisaSession.HardwareInterfaceType != Ivi.Visa.HardwareInterfaceType.Serial && attribute.ToString().StartsWith( Ivi.Visa.HardwareInterfaceType.Serial.ToString(), StringComparison.OrdinalIgnoreCase ) )
            {
                result = false;
            }

            if ( result && nativeVisaSession.HardwareInterfaceType != Ivi.Visa.HardwareInterfaceType.Tcp && attribute.ToString().StartsWith( Ivi.Visa.HardwareInterfaceType.Tcp.ToString(), StringComparison.OrdinalIgnoreCase ) )
            {
                result = false;
            }

            if ( result && nativeVisaSession.HardwareInterfaceType != Ivi.Visa.HardwareInterfaceType.Usb && attribute.ToString().StartsWith( Ivi.Visa.HardwareInterfaceType.Usb.ToString(), StringComparison.OrdinalIgnoreCase ) )
            {
                result = false;
            }

            if ( result && nativeVisaSession.HardwareInterfaceType != Ivi.Visa.HardwareInterfaceType.Vxi && attribute.ToString().StartsWith( Ivi.Visa.HardwareInterfaceType.Vxi.ToString(), StringComparison.OrdinalIgnoreCase ) )
            {
                result = false;
            }
        }

        return result;
    }

    /// <summary> Reads an attribute. </summary>
    /// <param name="attribute">    The attribute. </param>
    /// <param name="defaultValue"> The default value. </param>
    /// <returns> The attribute. </returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>" )]
    private bool ReadAttribute( Ivi.Visa.NativeVisaAttribute attribute, bool defaultValue )
    {
        return this.VisaSession is Ivi.Visa.INativeVisaSession s && IsSupported( s, attribute ) ? s.GetAttributeBoolean( attribute ) : defaultValue;
    }



    /// <summary> Writes an attribute. </summary>
    /// <param name="attribute"> The attribute. </param>
    /// <param name="value">     The value. </param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>" )]
    private void WriteAttribute( Ivi.Visa.NativeVisaAttribute attribute, bool value )
    {
        if ( this.VisaSession is Ivi.Visa.INativeVisaSession s && IsSupported( s, attribute ) )
        {
            s.SetAttributeBoolean( attribute, value );
        }
    }

    /// <summary> Reads an attribute. </summary>
    /// <param name="attribute">    The attribute. </param>
    /// <param name="defaultValue"> The default value. </param>
    /// <returns> The attribute. </returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>" )]
    private byte ReadAttribute( Ivi.Visa.NativeVisaAttribute attribute, byte defaultValue )
    {
        return this.VisaSession is Ivi.Visa.INativeVisaSession s && IsSupported( s, attribute ) ? s.GetAttributeByte( attribute ) : defaultValue;
    }

    /// <summary> Writes an attribute. </summary>
    /// <param name="attribute"> The attribute. </param>
    /// <param name="value">     The value. </param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>" )]
    private void WriteAttribute( Ivi.Visa.NativeVisaAttribute attribute, byte value )
    {
        if ( this.VisaSession is Ivi.Visa.INativeVisaSession s && IsSupported( s, attribute ) )
        {
            s.SetAttributeByte( attribute, value );
        }
    }

    /// <summary> Reads an attribute. </summary>
    /// <param name="attribute">    The attribute. </param>
    /// <param name="defaultValue"> The default value. </param>
    /// <returns> The attribute. </returns>
    private int ReadAttribute( Ivi.Visa.NativeVisaAttribute attribute, int defaultValue )
    {
        return this.VisaSession is Ivi.Visa.INativeVisaSession s && IsSupported( s, attribute ) ? s.GetAttributeInt32( attribute ) : defaultValue;
    }

    /// <summary> Writes an attribute. </summary>
    /// <param name="attribute"> The attribute. </param>
    /// <param name="value">     The value. </param>
    private void WriteAttribute( Ivi.Visa.NativeVisaAttribute attribute, int value )
    {
        if ( this.VisaSession is Ivi.Visa.INativeVisaSession s && IsSupported( s, attribute ) )
        {
            s.SetAttributeInt32( attribute, value );
        }
    }

    #endregion
}
