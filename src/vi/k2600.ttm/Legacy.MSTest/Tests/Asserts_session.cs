using System;

namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy.Tests;

internal static partial class Asserts
{
    /// <summary>   Assert orphan messages or device errors. </summary>
    /// <remarks>   2024-11-11. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="memberName">   (Optional) Name of the member. </param>
    public static void AssertOrphanMessagesOrDeviceErrors( Pith.SessionBase? session, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "" )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"VISA session to '{nameof( session.ResourceNameCaption )}' must be open." );
        string orphanMessages = session.ReadLines( session.StatusReadDelay, TimeSpan.FromMilliseconds( 100 ), false );

        Assert.IsTrue( string.IsNullOrWhiteSpace( orphanMessages ), $"The following messages were left on the output buffer after {memberName}:/n{orphanMessages}" );
        session.ThrowDeviceExceptionIfError( failureMessage: $"{nameof( Pith.SessionBase.ReadLines )} after {memberName} failed" );
    }

    /// <summary>   Asserts that the session should open. </summary>
    /// <remarks>   2024-11-08. </remarks>
    /// <param name="legacyDevice"> The legacy device. </param>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <returns>   A Pith.SessionBase. </returns>
    public static Pith.SessionBase AssetSessionShouldOpen( LegacyDevice? legacyDevice, string? resourceName )
    {
        Assert.IsNotNull( legacyDevice, $"{nameof( legacyDevice )} should not be null." );
        Assert.IsNotNull( resourceName, $"{nameof( resourceName )} should not be null." );
        Assert.IsFalse( string.IsNullOrEmpty( resourceName ), $"{nameof( resourceName )} should not be null or empty." );
        Assert.IsNotNull( legacyDevice.Meter.TspDevice, $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.Meter )}.{nameof( LegacyDevice.Meter.TspDevice )} should not be null." );
        Assert.IsNotNull( legacyDevice.Meter.TspDevice.Session, $"{nameof( Meter )}.{nameof( Meter.TspDevice )}.{nameof( Meter.TspDevice.Session )} should not be null." );
        Meter meter = legacyDevice.Meter;
        Pith.SessionBase session = legacyDevice.Meter.TspDevice.Session;

        // check for default syntax.
        Assert.AreEqual( cc.isr.VI.Syntax.Tsp.Lua.ClearExecutionStateCommand, session.ClearExecutionStateCommand, $"TSP syntax must be applied." );

        bool success = legacyDevice.Connect( resourceName );
        Assert.IsTrue( success, $"failed connection to {resourceName}" );

        Assert.AreEqual( resourceName, session.OpenResourceName, "The open session resource name should equal to the opening resource name." );
        Assert.IsTrue( session.IsDeviceOpen, $"{resourceName} should open" );

        Asserts.AssertOrphanMessagesOrDeviceErrors( session );

        // reset and clear known state.
        _ = legacyDevice.ResetAndClear();

        // initialize the device must be done after clear (5892)
        // session.InitKnownState();

        return session;
    }
}

