using System;
using cc.isr.VI.Device.MSTest.Base;
using cc.isr.VI.DeviceWinControls.Views;

namespace cc.isr.VI.DeviceWinControls.Tests;

public sealed partial class Asserts
{
    /// <summary>   Assert model should match. </summary>
    /// <remarks>   David, 2021-07-06. </remarks>
    /// <param name="subsystem">        The subsystem. </param>
    /// <param name="resourceSettings"> The resource settings. </param>
    public static void AssertModelShouldMatch( StatusSubsystemBase subsystem, cc.isr.VI.Device.MSTest.Settings.ResourceSettingsBase? resourceSettings )
    {
        Assert.IsNotNull( subsystem, $"{nameof( subsystem )} should not be null." );
        Assert.IsNotNull( resourceSettings, $"{nameof( resourceSettings )} should not be null." );
        Assert.IsTrue( resourceSettings.Exists, $"{nameof( resourceSettings )} should exist int he settings file." );
        Assert.IsTrue( string.Equals( resourceSettings.ResourceModel, subsystem.VersionInfoBase.Model, StringComparison.OrdinalIgnoreCase ),
            $"Resource settings model {resourceSettings.ResourceModel} should equal the model from {subsystem.ResourceNameCaption} Identity '{subsystem.VersionInfoBase.Identity}'" );
    }

    /// <summary>   Assert resource name should be selected. </summary>
    /// <remarks>
    /// Visa Tree View Timing:<para>
    /// Ping elapsed 0.119   </para><para>
    /// Create device 0.125   </para><para>
    /// Session Factory enumerating resource names 0.388   </para><para>
    /// Connector listing resource names 0.002   </para><para>
    /// Session factory listing resource names 0.000   </para><para>
    /// Session factory selecting TCPIP0:192.168.0.254:gpib0,22:INSTR selected  0.337   </para><para>
    /// Factory selected resource 0.000   </para><para>
    /// Check selected resource name 2.293   </para><para>
    /// </para>
    /// Visa View Timing:<para>
    /// Ping elapsed 0.123   </para><para>
    /// Create device 0.097   </para><para>
    /// Session Factory enumerating resource names 0.369   </para><para>
    /// Connector listing resource names 0.005   </para><para>
    /// Session factory listing resource names 0.000   </para><para>
    /// Session factory selecting TCPIP0:192.168.0.254:gpib0,22:INSTR selected  0.354   </para><para>
    /// Factory selected resource 0.000   </para><para>
    /// Check selected resource name 2.298   </para><para>
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="testInfo">         Information describing the test. </param>
    /// <param name="visaView">         The visa view control. </param>
    /// <param name="resourceSettings"> The resource settings. </param>
    public static void AssertResourceNameShouldBeSelected( IVisaView visaView, cc.isr.VI.Device.MSTest.Settings.ResourceSettingsBase? resourceSettings )
    {
        Assert.IsNotNull( visaView, $"{nameof( visaView )} should not be null." );
        Assert.IsNotNull( visaView.VisaSessionBase );
        Assert.IsNotNull( visaView.VisaSessionBase.Session );

        Assert.IsNotNull( resourceSettings, $"{nameof( resourceSettings )} should not be null." );
        Assert.IsTrue( resourceSettings.Exists, $"{nameof( resourceSettings )} should exist int he settings file." );

        Stopwatch sw = Stopwatch.StartNew();
        VisaSessionBase sessionBase = visaView.VisaSessionBase;
        Assert.IsNotNull( sessionBase.Session );
        Assert.IsFalse( string.IsNullOrEmpty( sessionBase.Session.ResourcesFilter ), "Resources filter should be defined" );
        string activity = "Session Factory enumerating resource names";
        Assert.IsNotNull( sessionBase.SessionFactory );
        SessionFactory factory = sessionBase.SessionFactory;
        // enumerate resources without filtering
        _ = factory.EnumerateResources( false );
        Console.Out.WriteLine( $"{activity} {sw.Elapsed:s\\.fff}" );
        sw.Restart();
        bool actualBoolean = factory.HasResources;
        bool expectedBoolean = true;
        Assert.AreEqual( expectedBoolean, actualBoolean, $"{activity} should succeed" );

        activity = "Connector listing resource names";
        actualBoolean = visaView.ResourceNamesCount > 0;
        expectedBoolean = true;
        Console.Out.WriteLine( $"{activity} {sw.Elapsed:s\\.fff}" );
        sw.Restart();
        Assert.AreEqual( expectedBoolean, actualBoolean, $"{activity} should succeed" );

        activity = "Session factory listing resource names";
        actualBoolean = sessionBase.SessionFactory.ResourceNames.Count > 0;
        expectedBoolean = true;
        Console.Out.WriteLine( $"{activity} {sw.Elapsed:s\\.fff}" );
        sw.Restart();
        Assert.AreEqual( expectedBoolean, actualBoolean, $"{activity} should succeed" );

        activity = $"Session factory selecting {resourceSettings.ResourceName} selected {factory.ValidatedResourceName}";
        (bool success, string details) = factory.TryValidateResource( resourceSettings.ResourceName );
        Console.Out.WriteLine( $"{activity} {sw.Elapsed:s\\.fff}" );
        sw.Restart();
        Assert.IsTrue( success, $"{activity} should succeed: {details}" );

        actualBoolean = !string.IsNullOrWhiteSpace( factory.ValidatedResourceName );
        expectedBoolean = true;
        Assert.AreEqual( expectedBoolean, actualBoolean, $"Resource {resourceSettings.ResourceName} not found" );

        activity = "Factory selected resource";
        string actualResource = factory.ValidatedResourceName;
        string expectedResource = resourceSettings.ResourceName;
        Console.Out.WriteLine( $"{activity} {sw.Elapsed:s\\.fff}" );
        sw.Restart();
        Assert.AreEqual( expectedResource, actualResource, $"{activity} should match" );
    }

    /// <summary>   Assert session resource names should match. </summary>
    /// <remarks>   David, 2021-07-06. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">          The session. </param>
    /// <param name="resourceSettings"> The resource settings. </param>
    public static void AssertSessionResourceNamesShouldMatch( Pith.SessionBase session, cc.isr.VI.Device.MSTest.Settings.ResourceSettingsBase? resourceSettings )
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
    public static void AssertVisaSessionBaseShouldOpen( int trialNumber, VisaSessionBase visaSessionBase, cc.isr.VI.Device.MSTest.Settings.ResourceSettingsBase? resourceSettings )
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

    /// <summary>   Assert Visa View session should open. </summary>
    /// <remarks>   David, 2021-07-06. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="testInfo">         Information describing the test. </param>
    /// <param name="trialNumber">      The trial number. </param>
    /// <param name="visaView">         The View control. </param>
    /// <param name="resourceSettings"> The resource settings. </param>
    public static void AssertVisaViewSessionShouldOpen( int trialNumber, IVisaView visaView, cc.isr.VI.Device.MSTest.Settings.ResourceSettingsBase? resourceSettings )
    {
        Assert.IsNotNull( visaView, $"{nameof( visaView )} should not be null." );
        Assert.IsNotNull( visaView.VisaSessionBase );
        Assert.IsNotNull( visaView.VisaSessionBase.Session );

        AssertVisaSessionBaseShouldOpen( trialNumber, visaView.VisaSessionBase, resourceSettings );
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

    /// <summary>   Assert visa view session should close. </summary>
    /// <remarks>   David, 2021-07-06. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="testInfo">     Information describing the test. </param>
    /// <param name="trialNumber">  The trial number. </param>
    /// <param name="visaView">     The control. </param>
    public static void AssertVisaViewSessionShouldClose( int trialNumber, IVisaView visaView )
    {
        Assert.IsNotNull( visaView, $"{nameof( visaView )} should not be null." );
        Assert.IsNotNull( visaView.VisaSessionBase );
        AssertVisaSessionBaseShouldClose( trialNumber, visaView.VisaSessionBase );
    }

    /// <summary>   Assert visa view session should open and close. </summary>
    /// <remarks>   David, 2021-07-06. </remarks>
    /// <param name="trialNumber">      The trial number. </param>
    /// <param name="visaView">         The control. </param>
    /// <param name="resourceSettings"> The resource settings. </param>
    public static void AssertVisaViewSessionShouldOpenAndClose( int trialNumber, IVisaView visaView, cc.isr.VI.Device.MSTest.Settings.ResourceSettingsBase? resourceSettings )
    {
        try
        {
            AssertVisaViewSessionShouldOpen( trialNumber, visaView, resourceSettings );
        }
        catch
        {
            throw;
        }
        finally
        {
            AssertVisaViewSessionShouldClose( trialNumber, visaView );
        }
    }
}
