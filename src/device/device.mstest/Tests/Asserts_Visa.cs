// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Device.Tests;

public sealed partial class Asserts
{
    #region " visa implementation checks "

    /// <summary>   Assert visa implementation should be loaded. </summary>
    /// <remarks>   2025-09-18. </remarks>
    /// <param name="verbose">  (Optional) True to verbose. </param>
    public static void AssertVisaImplementationShouldBeLoaded( bool verbose = true )
    {
        if ( typeof( Ivi.Visa.IMessageBasedSession ).Assembly is System.Reflection.Assembly iviVisaAssembly )
        {
            if ( verbose )
            {
                Console.WriteLine( $"\t{iviVisaAssembly.FullName}." );
                Console.WriteLine( $"\t{iviVisaAssembly.Location}." );
            }
        }
        else
            Assert.Fail( $"{nameof( Ivi.Visa.IMessageBasedSession )} assembly not found." );

        if ( typeof( cc.isr.Visa.Gac.Vendor ).Assembly is System.Reflection.Assembly vendorVisaAssembly )
        {
            if ( verbose )
            {
                Console.WriteLine( $"\t{vendorVisaAssembly.FullName}." );
                Console.WriteLine( $"\t{vendorVisaAssembly.Location}." );
            }
        }
        else
            Assert.Fail( $"{nameof( cc.isr.Visa.Gac.Vendor )} VISA assembly not found." );
    }

    #endregion

    #region " visa resource checks "

    /// <summary>   Assert visa resource manager should include a resource. </summary>
    /// <remarks>   2024-08-08. </remarks>
    /// <param name="session">  The device. </param>
    public static void AssertVisaResourceManagerShouldIncludeResource( cc.isr.VI.Pith.SessionBase session, Pith.Settings.ResourceSettings? resourceSettings )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );
        Assert.IsNotNull( resourceSettings, $"{nameof( Pith.Settings.ResourceSettings )} should not be null." );
        Assert.IsTrue( resourceSettings.Exists, $"{nameof( Pith.Settings.ResourceSettings )} should exist in the settings file." );

        string? resourcesFilter = SessionFactory.Instance.Factory.ResourcesProvider().ResourceFinder!.BuildMinimalResourcesFilter();
        Assert.IsNotNull( resourcesFilter );
        string[]? resources;
        using ( cc.isr.VI.Pith.ResourcesProviderBase rm = SessionFactory.Instance.Factory.ResourcesProvider() )
        {
            resources = [.. rm.FindResources( resourcesFilter )];
        }
        Assert.IsNotNull( resources );
        Assert.IsGreaterThan( 0, resources.Length, $"VISA Resources {(resources?.Length > 0 ? string.Empty : "not")} found among {resourcesFilter}" );
        Assert.IsTrue( resources!.Contains( resourceSettings.ResourceName ), $"Resource {resourceSettings.ResourceName} not found among {resourcesFilter}" );
    }

    /// <summary> Assert visa session base should find the resource. </summary>
    /// <param name="device">           The device. </param>
    /// <param name="resourceSettings"> The resource settings. </param>
    public static void AssertVisaSessionBaseShouldFindResource( VisaSessionBase? device, Pith.Settings.ResourceSettings? resourceSettings )
    {
        Assert.IsNotNull( device, nameof( device ) );
        Assert.IsNotNull( device.Session, nameof( device.Session ) );
        Assert.IsNotNull( resourceSettings, $"{nameof( Pith.Settings.ResourceSettings )} should not be null." );
        Assert.IsTrue( resourceSettings.Exists, $"{nameof( Pith.Settings.ResourceSettings )} should exist in the settings file." );

        Assert.IsTrue( VisaSessionBase.Find( resourceSettings.ResourceName, device.ResourcesFilter ),
            $"VISA Resource {resourceSettings.ResourceName} not found among {device.ResourcesFilter}" );
    }

    /// <summary> Assert visa session should find the resource. </summary>
    /// <param name="session">          The device. </param>
    /// <param name="resourceSettings"> The resource settings. </param>
    public static void AssertVisaSessionShouldFindResource( VisaSession? session, Pith.Settings.ResourceSettings? resourceSettings )
    {
        Assert.IsNotNull( session, nameof( session ) );
        Assert.IsNotNull( session.Session, nameof( session.Session ) );
        Assert.IsNotNull( resourceSettings, $"{nameof( Pith.Settings.ResourceSettings )} should not be null." );
        Assert.IsTrue( resourceSettings.Exists, $"{nameof( Pith.Settings.ResourceSettings )} should exist in the settings file." );
        Assert.IsTrue( VisaSessionBase.Find( resourceSettings.ResourceName, session.ResourcesFilter ),
            $"VISA Resource {resourceSettings.ResourceName} not found among {session.ResourcesFilter}" );
    }

    #endregion
}
