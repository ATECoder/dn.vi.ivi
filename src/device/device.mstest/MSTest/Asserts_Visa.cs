using System;
using System.Linq;

namespace cc.isr.VI.Device.MSTest;

public sealed partial class Asserts
{
    #region " visa resource checks "

    /// <summary>   Assert visa resource manager should include a resource. </summary>
    /// <remarks>   2024-08-08. </remarks>
    /// <param name="session">  The device. </param>
    public static void AssertVisaResourceManagerShouldIncludeResource( cc.isr.VI.Pith.SessionBase session, Settings.ResourceSettingsBase? resourceSettings )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );
        Assert.IsNotNull( resourceSettings, $"{nameof( Settings.ResourceSettingsBase )} should not be null." );
        Assert.IsTrue( resourceSettings.Exists, $"{nameof( Settings.ResourceSettingsBase )} should exist in the settings file." );

        string? resourcesFilter = SessionFactory.Instance.Factory.ResourcesProvider().ResourceFinder!.BuildMinimalResourcesFilter();
        Assert.IsNotNull( resourcesFilter );
        string[]? resources;
        using ( cc.isr.VI.Pith.ResourcesProviderBase rm = SessionFactory.Instance.Factory.ResourcesProvider() )
        {
            resources = rm.FindResources( resourcesFilter ).ToArray();
        }
        Assert.IsNotNull( resources );
        Assert.IsTrue( resources.Length > 0, $"VISA Resources {(resources?.Length > 0 ? string.Empty : "not")} found among {resourcesFilter}" );
        Assert.IsTrue( resources!.Contains( resourceSettings.ResourceName ), $"Resource {resourceSettings.ResourceName} not found among {resourcesFilter}" );
    }

    /// <summary> Assert visa session base should find the resource. </summary>
    /// <param name="device">           The device. </param>
    /// <param name="resourceSettings"> The resource settings. </param>
    public static void AssertVisaSessionBaseShouldFindResource( VisaSessionBase? device, Settings.ResourceSettingsBase? resourceSettings )
    {
        Assert.IsNotNull( device, nameof( device ) );
        Assert.IsNotNull( device.Session, nameof( device.Session ) );
        Assert.IsNotNull( resourceSettings, $"{nameof( Settings.ResourceSettingsBase )} should not be null." );
        Assert.IsTrue( resourceSettings.Exists, $"{nameof( Settings.ResourceSettingsBase )} should exist in the settings file." );

        Assert.IsTrue( VisaSessionBase.Find( resourceSettings.ResourceName, device.ResourcesFilter ),
            $"VISA Resource {resourceSettings.ResourceName} not found among {device.ResourcesFilter}" );
    }

    /// <summary> Assert visa session should find the resource. </summary>
    /// <param name="session">          The device. </param>
    /// <param name="resourceSettings"> The resource settings. </param>
    public static void AssertVisaSessionShouldFindResource( VisaSession? session, Settings.ResourceSettingsBase? resourceSettings )
    {
        Assert.IsNotNull( session, nameof( session ) );
        Assert.IsNotNull( session.Session, nameof( session.Session ) );
        Assert.IsNotNull( resourceSettings, $"{nameof( Settings.ResourceSettingsBase )} should not be null." );
        Assert.IsTrue( resourceSettings.Exists, $"{nameof( Settings.ResourceSettingsBase )} should exist in the settings file." );
        Assert.IsTrue( VisaSessionBase.Find( resourceSettings.ResourceName, session.ResourcesFilter ),
            $"VISA Resource {resourceSettings.ResourceName} not found among {session.ResourcesFilter}" );
    }

    #endregion
}
