using System;
using cc.isr.VI.Device.Tests.Base;

namespace cc.isr.VI.SubsystemsWinControls.Tests;

public sealed partial class Asserts
{
    /// <summary>   Assert model should match. </summary>
    /// <remarks>   David, 2021-07-06. </remarks>
    /// <param name="subsystem">        The subsystem. </param>
    /// <param name="resourceSettings"> The resource settings. </param>
    public static void AssertModelShouldMatch( StatusSubsystemBase subsystem, Pith.Settings.ResourceSettings? resourceSettings )
    {
        Assert.IsNotNull( subsystem, $"{nameof( subsystem )} should not be null." );
        Assert.IsNotNull( resourceSettings, $"{nameof( resourceSettings )} should not be null." );
        Assert.IsTrue( resourceSettings.Exists, $"{nameof( resourceSettings )} should exist int he settings file." );
        Assert.IsTrue( string.Equals( resourceSettings.ResourceModel, subsystem.VersionInfoBase.Model, StringComparison.OrdinalIgnoreCase ),
            $"Resource settings model {resourceSettings.ResourceModel} should equal the model from {subsystem.ResourceNameCaption} Identity '{subsystem.VersionInfoBase.Identity}'" );
    }

    /// <summary>   Assert session resource names should match. </summary>
    /// <remarks>   David, 2021-07-06. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">          The session. </param>
    /// <param name="resourceSettings"> The resource settings. </param>
    public static void AssertSessionResourceNamesShouldMatch( Pith.SessionBase session, Pith.Settings.ResourceSettings? resourceSettings )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );

        Assert.IsNotNull( resourceSettings, $"{nameof( resourceSettings )} should not be null." );
        Assert.IsTrue( resourceSettings.Exists, $"{nameof( resourceSettings )} should exist int he settings file." );

        int expectedErrorAvailableBits = ( int ) Pith.ServiceRequests.ErrorAvailable;
        int actualErrorAvailableBits = ( int ) session.ErrorAvailableBitmask;
        Assert.AreEqual( expectedErrorAvailableBits, actualErrorAvailableBits, $"Error available bits on creating device." );

        string actualResource = session.ValidatedResourceName;
        string expectedResource = resourceSettings.ResourceName;
        Assert.AreEqual( expectedResource, actualResource, $"Visa Session validated resource mismatch" );

        actualResource = session.CandidateResourceName;
        expectedResource = resourceSettings.ResourceName;
        Assert.AreEqual( expectedResource, actualResource, $"Visa Session candidate resource mismatch" );

        actualResource = session.OpenResourceName;
        expectedResource = resourceSettings.ResourceName;
        Assert.AreEqual( expectedResource, actualResource, $"Visa Session open resource mismatch" );

        string expectedResourceModel = resourceSettings.ResourceModel;
        string actualResourceModel = session.OpenResourceModel;
        Assert.AreEqual( expectedResourceModel, actualResourceModel, $"{session.ResourceNameCaption} candidate model names should match" );
        actualResourceModel = session.CandidateResourceModel;
        Assert.AreEqual( expectedResourceModel, actualResourceModel, $"{session.ResourceNameCaption} candidate model names should match" );
    }

    /// <summary>   Asserts Visa Session Base Should Open. </summary>
    /// <remarks>
    /// The session open overhead is due to the device initialization. Timing: <para>
    /// Ping elapsed 0.194     </para><para>
    /// Ping elapsed 0.000     </para><para>
    /// Test session opened 0.627     </para><para>
    /// Test session Closed 0.013     </para><para>
    /// Session opened 3.803     </para><para>
    /// Session open 6.021     </para><para>
    /// Session checked 0.001     </para><para>
    /// Session closed 0.019     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="testInfo">         Information describing the test. </param>
    /// <param name="trialNumber">      The trial number. </param>
    /// <param name="visaSessionBase">  The session base. </param>
    /// <param name="resourceSettings"> The resource settings. </param>
    public static void AssertVisaSessionBaseShouldOpen( int trialNumber, VisaSessionBase visaSessionBase, Pith.Settings.ResourceSettings? resourceSettings )
    {
        Assert.IsNotNull( visaSessionBase, $"{nameof( visaSessionBase )} should not be null." );
        Assert.IsNotNull( visaSessionBase.Session, $"{nameof( visaSessionBase )}.{nameof( visaSessionBase.Session )} should not be null." );
        Assert.IsNotNull( resourceSettings, $"{nameof( resourceSettings )} should not be null." );
        Assert.IsTrue( resourceSettings.Exists, $"{nameof( resourceSettings )} should exist int he settings file." );

        TestBase.AssertResourceNameShouldPing( visaSessionBase.Session, resourceSettings );

        Stopwatch sw = Stopwatch.StartNew();
        bool actualBoolean;
        sw.Restart();
        bool expectedBoolean;
        if ( !visaSessionBase.IsDeviceOpen )
        {
            (bool success, string details) = visaSessionBase.TryOpenSession( resourceSettings.ResourceName, resourceSettings.ResourceModel );
            actualBoolean = success;
            expectedBoolean = true;
            Assert.AreEqual( expectedBoolean, actualBoolean, $"{trialNumber} opening session {resourceSettings.ResourceName} canceled; {details}" );
        }

        actualBoolean = visaSessionBase.IsSessionOpen;
        expectedBoolean = true;
        Assert.AreEqual( expectedBoolean, actualBoolean, $"{trialNumber} opening session {resourceSettings.ResourceName} session not open" );
        Console.Out.WriteLine( $"Session opened {sw.Elapsed:s\\.fff}" );
        sw.Restart();
        actualBoolean = visaSessionBase.IsDeviceOpen;
        expectedBoolean = true;
        Assert.AreEqual( expectedBoolean, actualBoolean, $"{trialNumber} opening session {resourceSettings.ResourceName} device not open" );
        Assert.IsNotNull( visaSessionBase.StatusSubsystemBase );
        AssertModelShouldMatch( visaSessionBase.StatusSubsystemBase, resourceSettings );
        if ( visaSessionBase.IsSessionOpen )
            // report device errors if the error bit is set.
            _ = visaSessionBase.Session.TryQueryAndReportDeviceErrors( visaSessionBase.Session.ReadStatusByte() );
    }

    /// <summary>   Assert session should close. </summary>
    /// <remarks>   David, 2021-07-06. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="testInfo">         Information describing the test. </param>
    /// <param name="trialNumber">      The trial number. </param>
    /// <param name="visaSessionBase">  The session base. </param>
    public static void AssertVisaSessionBaseShouldClose( int trialNumber, VisaSessionBase visaSessionBase )
    {
        Assert.IsNotNull( visaSessionBase, $"{nameof( visaSessionBase )} should not be null." );
        Assert.IsNotNull( visaSessionBase.StatusSubsystemBase );
        Assert.IsNotNull( visaSessionBase.Session, $"{nameof( visaSessionBase )}.{nameof( visaSessionBase.Session )} should not be null." );
        bool actualBoolean;
        try
        {
            if ( visaSessionBase.IsSessionOpen )
                // report device errors if the error bit is set.
                _ = visaSessionBase.Session.TryQueryAndReportDeviceErrors( visaSessionBase.Session.ReadStatusByte() );
        }
        catch
        {
            throw;
        }
        finally
        {
            _ = visaSessionBase.TryCloseSession();
        }

        Assert.IsFalse( visaSessionBase.IsDeviceOpen, $"{visaSessionBase.ResourceNameCaption} failed closing session" );
        actualBoolean = visaSessionBase.IsDeviceOpen;
        bool expectedBoolean = false;
        Assert.AreEqual( expectedBoolean, actualBoolean, $"{trialNumber} closing session {visaSessionBase.OpenResourceName} control still connected" );
        actualBoolean = visaSessionBase.IsDeviceOpen;
        expectedBoolean = false;
        Assert.AreEqual( expectedBoolean, actualBoolean, $"{trialNumber} closing session {visaSessionBase.OpenResourceName} device still open" );
    }
}
