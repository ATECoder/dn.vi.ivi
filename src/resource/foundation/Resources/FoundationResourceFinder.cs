// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Foundation;

/// <summary> An IVI Foundation-based Resource Finder. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-09-10, 3.0.5001.x. </para>
/// </remarks>
[CLSCompliant( false )]
public class FoundationResourceFinder : Pith.ResourceFinderBase
{
    #region " construction and cleanup "

    /// <summary> Initializes a new instance of the <see cref="object" /> class. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    public FoundationResourceFinder() : base()
    {
    }

    #endregion

    #region " conversions "

    /// <summary> Converts the given value. </summary>
    /// <param name="value"> The detailed information derived when VISA.NET parses a resource
    /// descriptor. </param>
    /// <returns> A Ivi.Visa.HardwareInterfaceType. </returns>
    [CLSCompliant( false )]
    public static Pith.HardwareInterfaceType ConvertInterfaceType( Ivi.Visa.HardwareInterfaceType value )
    {
        return Enum.IsDefined( typeof( Pith.HardwareInterfaceType ), ( int ) value ) ? ( Pith.HardwareInterfaceType ) ( int ) value : Pith.HardwareInterfaceType.Gpib;
    }

    /// <summary> Converts the given value. </summary>
    /// <param name="value"> The detailed information derived when VISA.NET parses a resource
    /// descriptor. </param>
    /// <returns> A Ivi.Visa.HardwareInterfaceType. </returns>
    [CLSCompliant( false )]
    public static Ivi.Visa.HardwareInterfaceType ConvertInterfaceType( Pith.HardwareInterfaceType value )
    {
        return Enum.IsDefined( typeof( Ivi.Visa.HardwareInterfaceType ), ( int ) value )
            ? ( Ivi.Visa.HardwareInterfaceType ) ( int ) value
            : Ivi.Visa.HardwareInterfaceType.Gpib;
    }

    /// <summary> Convert parse result. </summary>
    /// <remarks> David, 2020-04-10. </remarks>
    /// <param name="value"> The detailed information derived when VISA.NET parses a resource
    /// descriptor. </param>
    /// <returns> The <see cref="VI.Pith.ResourceNameInfo"/>. </returns>
    [CLSCompliant( false )]
    public static Pith.ResourceNameInfo ConvertParseResult( Ivi.Visa.ParseResult value )
    {
        return new Pith.ResourceNameInfo( value.OriginalResourceName, ConvertInterfaceType( value.InterfaceType ), value.InterfaceNumber );
    }

    /// <summary> Converts a value to a resource open state. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <param name="value"> The detailed information derived when VISA.NET parses a resource
    /// descriptor. </param>
    /// <returns> Value as an cc.isr.VI.Pith.ResourceOpenState. </returns>
    [CLSCompliant( false )]
    public static Pith.ResourceOpenState ToResourceOpenState( Ivi.Visa.ResourceOpenStatus value )
    {
        switch ( value )
        {
            case Ivi.Visa.ResourceOpenStatus.Unknown:
                {
                    return Pith.ResourceOpenState.Unknown;
                }

            case Ivi.Visa.ResourceOpenStatus.Success:
                {
                    return Pith.ResourceOpenState.Success;
                }

            case Ivi.Visa.ResourceOpenStatus.DeviceNotResponding:
                {
                    return Pith.ResourceOpenState.DeviceNotResponding;
                }

            case Ivi.Visa.ResourceOpenStatus.ConfigurationNotLoaded:
                {
                    return Pith.ResourceOpenState.ConfigurationNotLoaded;
                }

            default:
                {
                    throw new InvalidOperationException( $"Unhandled case for {nameof( Ivi.Visa.ResourceOpenStatus )} {value}({( int ) value})" );
                }
        }
    }

    #endregion

    #region " parse resources "

    /// <summary> Parse resource. </summary>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    /// <exception cref="cc.isr.VI.Pith.NativeException">          Thrown when a Native error condition occurs. </exception>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <returns> <see cref="VI.Pith.ResourceNameInfo"/>. </returns>
    public override Pith.ResourceNameInfo ParseResource( string resourceName )
    {
        string activity = $"IVI.Visa {Ivi.Visa.GlobalResourceManager.ImplementationVersion}/{Ivi.Visa.GlobalResourceManager.SpecificationVersion} parsing resource {resourceName}";
        try
        {
            return ConvertParseResult( Ivi.Visa.GlobalResourceManager.Parse( resourceName ) );
        }
        catch ( MissingMethodException ex )
        {
            if ( Ivi.Visa.GlobalResourceManager.ImplementationVersion < new Version( cc.isr.Visa.Gac.Vendor.IVI_VISA_IMPLEMENTATION_VERSION ) )
                throw new InvalidOperationException(
                    $"Failed {activity}; Expecting {cc.isr.Visa.Gac.Vendor.IVI_VISA_IMPLEMENTATION_VERSION} implementation and {cc.isr.Visa.Gac.Vendor.IVI_VISA_SPECIFICATION_VERSION} specification versions.", ex );
            else
                throw new InvalidOperationException( $"Failed {activity}", ex );
        }
        catch ( Ivi.Visa.NativeVisaException ex )
        {
            NativeError nativeError = new( ex.ErrorCode, resourceName, "@DM", "Parsing resource" );
            throw new Pith.NativeException( nativeError, ex );
        }
        catch
        {
            throw;
        }
    }

    /// <summary> Gets or sets the using like pattern. </summary>
    /// <value> The using like pattern. </value>
    public override bool UsingLikePattern { get; } = false;

    #endregion

    #region " find resources "

    /// <summary>   Enumerates the items in this collection that meet given criteria. </summary>
    /// <remarks>   David, 2021-11-11. </remarks>
    /// <param name="filter">   A pattern specifying the search. </param>
    /// <returns>   An enumerator that allows foreach to be used to process the matched items. </returns>
    private IEnumerable<string> Find( string filter )
    {
        try
        {
            filter = VI.Pith.ResourceNamesManager.TranslatePattern( filter, this.UsingLikePattern );
            return Ivi.Visa.GlobalResourceManager.Find( filter );
        }
        catch ( Ivi.Visa.VisaException ex ) when ( ex.HResult == -2146233088 )
        {
            return [];
        }
    }

    /// <summary> Lists all resources. </summary>
    /// <exception cref="cc.isr.VI.Pith.NativeException"> Thrown when a Native error condition occurs. </exception>
    /// <returns> List of all resources. </returns>
    public override IEnumerable<string> FindResources()
    {
        string resources = string.Empty;
        try
        {
            return this.Find( Pith.ResourceNamesManager.AllResourcesFilter );
        }
        catch ( Ivi.Visa.NativeVisaException ex )
        {
            NativeError nativeError = new( ex.ErrorCode, resources, "@DM", "Finding resources" );
            throw new Pith.NativeException( nativeError, ex );
        }
    }

    /// <summary> Lists all resources. </summary>
    /// <remarks> David, 2020-04-10. </remarks>
    /// <exception cref="cc.isr.VI.Pith.NativeException"> Thrown when a Native error condition occurs. </exception>
    /// <param name="filter"> A pattern specifying the search. </param>
    /// <returns> List of all resources. </returns>
    public override IEnumerable<string> FindResources( string filter )
    {
        try
        {
            return this.Find( filter );
        }
        catch ( Ivi.Visa.NativeVisaException ex )
        {
            NativeError nativeError = new( ex.ErrorCode, string.Empty, "@DM", "Finding resources" );
            throw new Pith.NativeException( nativeError, ex );
        }
    }

    /// <summary> Searches for all interfaces. </summary>
    /// <exception cref="cc.isr.VI.Pith.NativeException"> Thrown when a Native error condition occurs. </exception>
    /// <returns> The found interface resource names. </returns>
    public override IEnumerable<string> FindInterfaces()
    {
        string filter = string.Empty;
        try
        {
            filter = Pith.ResourceNamesManager.BuildInterfaceFilter();
            return this.Find( filter );
        }
        catch ( Ivi.Visa.NativeVisaException ex )
        {
            NativeError nativeError = new( ex.ErrorCode, filter, "@DM", "Finding interfaces" );
            throw new Pith.NativeException( nativeError, ex );
        }
    }

    /// <summary> Searches for the interfaces. </summary>
    /// <remarks> David, 2020-04-10. </remarks>
    /// <exception cref="cc.isr.VI.Pith.NativeException"> Thrown when a Native error condition occurs. </exception>
    /// <param name="filter"> A pattern specifying the search. </param>
    /// <returns> The found interface resource names. </returns>
    public override IEnumerable<string> FindInterfaces( string filter )
    {
        try
        {
            return this.Find( filter );
        }
        catch ( Ivi.Visa.NativeVisaException ex )
        {
            NativeError nativeError = new( ex.ErrorCode, filter, "@DM", "Finding interfaces" );
            throw new Pith.NativeException( nativeError, ex );
        }
    }

    /// <summary> Searches for the interfaces. </summary>
    /// <exception cref="cc.isr.VI.Pith.NativeException"> Thrown when a Native error condition occurs. </exception>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <returns> The found interface resource names. </returns>
    public override (bool Success, string Details, IEnumerable<string> Resources) FindInterfaces( Pith.HardwareInterfaceType interfaceType )
    {
        string filter = string.Empty;
        try
        {
            filter = Pith.ResourceNamesManager.BuildInterfaceFilter( interfaceType );
            IEnumerable<string> resources = this.Find( filter );
            return (resources.Any(), "Resources not found", resources);
        }
        catch ( Ivi.Visa.NativeVisaException ex )
        {
            NativeError nativeError = new( ex.ErrorCode, filter, "@DM", "Finding interfaces" );
            throw new Pith.NativeException( nativeError, ex );
        }
    }

    /// <summary> Searches for instruments. </summary>
    /// <remarks> David, 2020-04-10. </remarks>
    /// <exception cref="cc.isr.VI.Pith.NativeException"> Thrown when a Native error condition occurs. </exception>
    /// <param name="filter"> A pattern specifying the search. </param>
    /// <returns> The found instrument resource names. </returns>
    public override IEnumerable<string> FindInstruments( string filter )
    {
        try
        {
            return this.Find( filter );
        }
        catch ( Ivi.Visa.NativeVisaException ex )
        {
            NativeError nativeError = new( ex.ErrorCode, filter, "@DM", "Finding instruments" );
            throw new Pith.NativeException( nativeError, ex );
        }
    }

    /// <summary> Searches for instruments. </summary>
    /// <exception cref="cc.isr.VI.Pith.NativeException"> Thrown when a Native error condition occurs. </exception>
    /// <returns> The found instrument resource names. </returns>
    public override IEnumerable<string> FindInstruments()
    {
        string filter = string.Empty;
        try
        {
            filter = Pith.ResourceNamesManager.BuildInstrumentFilter();
            return this.Find( filter );
        }
        catch ( Ivi.Visa.NativeVisaException ex )
        {
            NativeError nativeError = new( ex.ErrorCode, filter, "@DM", "Finding instruments" );
            throw new Pith.NativeException( nativeError, ex );
        }
    }

    /// <summary> Searches for instruments. </summary>
    /// <exception cref="cc.isr.VI.Pith.NativeException"> Thrown when a Native error condition occurs. </exception>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <returns> The found instrument resource names. </returns>
    public override IEnumerable<string> FindInstruments( Pith.HardwareInterfaceType interfaceType )
    {
        string filter = string.Empty;
        try
        {
            filter = Pith.ResourceNamesManager.BuildInstrumentFilter( interfaceType );
            return this.Find( filter );
        }
        catch ( Ivi.Visa.NativeVisaException ex )
        {
            NativeError nativeError = new( ex.ErrorCode, filter, "@DM", "Finding instruments" );
            throw new Pith.NativeException( nativeError, ex );
        }
    }

    /// <summary> Searches for instruments. </summary>
    /// <exception cref="cc.isr.VI.Pith.NativeException"> Thrown when a Native error condition occurs. </exception>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <param name="boardNumber">   The board number. </param>
    /// <returns> The found instrument resource names. </returns>
    public override IEnumerable<string> FindInstruments( Pith.HardwareInterfaceType interfaceType, int boardNumber )
    {
        string filter = string.Empty;
        try
        {
            filter = Pith.ResourceNamesManager.BuildInstrumentFilter( interfaceType, boardNumber );
            return this.Find( filter );
        }
        catch ( Ivi.Visa.NativeVisaException ex )
        {
            NativeError nativeError = new( ex.ErrorCode, filter, "@DM", "Finding instruments" );
            throw new Pith.NativeException( nativeError, ex );
        }
    }

    #endregion
}
