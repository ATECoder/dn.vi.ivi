namespace cc.isr.VI.Tsp.K2600.Ttm.Tests.Firmware;

internal static partial class Asserts
{
    /// <summary>   Asserts that the session should open. </summary>
    /// <remarks>   2024-10-26. </remarks>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <returns>   A Pith.SessionBase. </returns>
    public static Pith.SessionBase AssetSessionShouldOpen( string? resourceName )
    {
        Assert.IsNotNull( resourceName, $"{nameof( resourceName )} should not be null." );
        Assert.IsFalse( string.IsNullOrEmpty( resourceName ), $"{nameof( resourceName )} should not be null or empty." );

        Pith.SessionBase? session = SessionFactory.Instance.Factory.Session();

        Assert.IsNotNull( session, $"{nameof( SessionFactory )} should provide a session." );

        // read settings and throw if not found.
        session.ReadSettings( typeof( Asserts ), ".Session", true, true );

        session.OpenSession( resourceName );

        Assert.AreEqual( resourceName, session.OpenResourceName, "The open session resource name should equal to the opening resource name." );
        Assert.IsTrue( session.IsDeviceOpen, $"{resourceName} should open" );

        // set the TSP session defaults
        session.ApplyDefaultSyntax( VI.Syntax.CommandLanguage.Tsp );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

        // reset and clear known state.

        // issues selective device clear.
        session.ClearActiveState();
        session.ThrowDeviceExceptionIfError();

        // reset device
        session.ResetKnownState();
        session.ThrowDeviceExceptionIfError();

        // Clear the device StatusReading and set more defaults
        session.ClearExecutionState();
        session.ThrowDeviceExceptionIfError();

        // initialize the device must be done after clear (5892)
        // session.InitKnownState();

        // set the MeterSubsystem legacy firmware
        MeterSubsystem.FirmwareVersion = new System.Version( session.QueryTrimEnd( Syntax.ThermalTransient.VersionQueryCommand ) );

        // detect the legacy firmware
        string ttmObjectName = Syntax.ThermalTransient.ThermalTransientBaseEntityName;
        Assert.IsFalse( session.IsNil( ttmObjectName ), $"{ttmObjectName} should not be nil." );
        ttmObjectName = Syntax.ThermalTransient.OutcomeBitsName;
        Assert.IsFalse( session.IsNil( ttmObjectName ), $"{ttmObjectName} should not be nil." );
        ttmObjectName = Syntax.ThermalTransient.OutcomeBitsOkayName;
        Assert.IsFalse( session.IsNil( ttmObjectName ), $"{ttmObjectName} should not be nil." );

        return session;
    }
}
