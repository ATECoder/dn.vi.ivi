using System.Diagnostics;
using System.Text;
using System;

namespace cc.isr.VI.Tsp.K2600.Ttm.Tests.Driver;

/// <summary>   An asserts. </summary>
/// <remarks>   2024-11-01. </remarks>
internal static partial class Asserts
{

    /// <summary>   Asserts that the session should open. </summary>
    /// <remarks>   2024-10-26. </remarks>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <returns>   A Pith.SessionBase. </returns>
    public static Pith.SessionBase AssetSessionShouldOpen( Meter? meter, string? resourceName, string? resourceModel )
    {
        Assert.IsNotNull( meter, $"{nameof( meter )} should not be null." );
        Assert.IsNotNull( meter.TspDevice, $"{nameof( Meter )}.{nameof( Meter.TspDevice )} should not be null." );
        Assert.IsNotNull( resourceName, $"{nameof( resourceName )} should not be null." );
        Assert.IsFalse( string.IsNullOrEmpty( resourceName ), $"{nameof( resourceName )} should not be null or empty." );
        Assert.IsNotNull( resourceModel, $"{nameof( resourceModel )} should not be null." );
        Assert.IsFalse( string.IsNullOrEmpty( resourceModel ), $"{nameof( resourceModel )} should not be null or empty." );
        Assert.IsNotNull( meter.TspDevice.Session, $"{nameof( Meter )}.{nameof( Meter.TspDevice )}.{nameof( Meter.TspDevice.Session )} should not be null." );
        Pith.SessionBase session = meter.TspDevice.Session;

        // check for default syntax.
        Assert.AreEqual( cc.isr.VI.Syntax.Tsp.Lua.ClearExecutionStateCommand, session.ClearExecutionStateCommand, $"TSP syntax must be applied." );

        (bool success, string details) = meter.TspDevice.TryOpenSession( resourceName, resourceModel );
        Assert.IsTrue( success, details );

        Assert.AreEqual( resourceName, session.OpenResourceName, "The open session resource name should equal to the opening resource name." );
        Assert.IsTrue( session.IsDeviceOpen, $"{resourceName} should open" );

        Asserts.AssertOrphanMessagesOrDeviceErrors( session );

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

        // detect the legacy firmware
        string ttmObjectName = "ttm";
        Assert.IsFalse( session.IsNil( ttmObjectName ), $"{ttmObjectName} should not be nil." );

        ttmObjectName = "ttm.Outcomes";
        Assert.IsFalse( session.IsNil( ttmObjectName ), $"{ttmObjectName} should not be nil." );

        ttmObjectName = "ttm.Outcomes.okay";
        Assert.IsFalse( session.IsNil( ttmObjectName ), $"{ttmObjectName} should not be nil." );

        // the legacy software has no definition for open leads.
        ttmObjectName = "ttm.Outcomes.openLeads";
        Asserts.LegacyFirmware = session.IsNil( ttmObjectName );

        // prepare the device under test.
        DeviceUnderTest deviceUnderTest = meter.ConfigInfo;

        // test DUT initialization
        deviceUnderTest.ClearPartInfo();
        deviceUnderTest.ClearMeasurements();

        deviceUnderTest.PartNumber = "part number";
        deviceUnderTest.LotId = "lot id";
        deviceUnderTest.OperatorId = "operator id";

        meter.Part = deviceUnderTest;

        Assert.IsNotNull( meter.MeasureSequencer, $"{nameof( Meter )}.{nameof( Meter.MeasureSequencer )} should not be null." );
        Assert.IsNotNull( meter.TriggerSequencer, $"{nameof( Meter )}.{nameof( Meter.TriggerSequencer )} should not be null." );

        return session;
    }

    /// <summary>   Asserts that orphan messaged should clear. </summary>
    /// <remarks>   2024-10-26. </remarks>
    /// <param name="session">  The session. </param>
    /// <param name="timeout">  The timeout. </param>
    public static string AssertOrphanMessagedShouldClear( Pith.SessionBase? session, TimeSpan timeout )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );
        string reading;
        bool messageAvailable = false;
        Stopwatch sw = Stopwatch.StartNew();
        StringBuilder builder = new();
        while ( !messageAvailable && sw.Elapsed < timeout )
        {
            _ = Pith.SessionBase.AsyncDelay( TimeSpan.FromMilliseconds( 10 ) );
            // was: messageAvailable = session.IsMeasurementEventBitSet( session.ReadStatusByte() );
            messageAvailable = session.IsMessageAvailableBitSet( session.ReadStatusByte() );
            if ( messageAvailable )
            {
                if ( builder.Length == 0 )
                {
                    _ = builder.AppendLine( "Orphan messages:" );
                }
                reading = session.ReadLineTrimEnd();
                _ = builder.AppendLine( reading );
            }
        }
        return builder.ToString();
    }

}


