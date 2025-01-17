using System;
using System.Linq;
using cc.isr.Std.SplitExtensions;

namespace cc.isr.VI.Device.MSTest;

public sealed partial class Asserts
{
    #region " session "

    /// <summary>   Assert session initial values should match. </summary>
    /// <remarks>   2024-10-23. </remarks>
    /// <param name="session">          The session. </param>
    /// <param name="resourceSettings"> Information describing the resource. </param>
    public static void AssertSessionInitialValuesShouldMatch( cc.isr.VI.Pith.SessionBase? session, Pith.Settings.ResourceSettings? resourceSettings )
    {
        System.Reflection.MethodBase? methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
        string methodFullName = $"{methodInfo?.DeclaringType?.Name}.{methodInfo?.Name}";
        Console.WriteLine( $"@{methodFullName}" );

        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );
        Assert.IsTrue( session.TimingSettings.Exists );
        Assert.IsNotNull( resourceSettings, $"{nameof( Pith.Settings.ResourceSettings )} should not be null." );
        Assert.IsTrue( resourceSettings.Exists, $"{nameof( Pith.Settings.ResourceSettings )} should exist in the settings file." );
        string propertyName = nameof( cc.isr.VI.Pith.ServiceRequests.ErrorAvailable ).SplitWords();
        int expectedErrorAvailableBitmask = session.RegistersBitmasksSettings.ErrorAvailableBitmask;
        int actualErrorAvailableBitmask = ( int ) session.ErrorAvailableBitmask;
        Assert.AreEqual( expectedErrorAvailableBitmask, actualErrorAvailableBitmask,
                    $"{session.ResourceNameCaption} {propertyName} bitmask set upon on creating device should match" );
    }

    /// <summary> Assert session open values should match. </summary>
    /// <param name="session">      The session. </param>
    /// <param name="resourceInfo"> Information describing the resource. </param>
    public static void AssertSessionOpenValuesShouldMatch( cc.isr.VI.Pith.SessionBase? session, Pith.Settings.ResourceSettings? resourceSettings )
    {
        System.Reflection.MethodBase? methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
        string methodFullName = $"{methodInfo?.DeclaringType?.Name}.{methodInfo?.Name}";
        Console.WriteLine( $"@{methodFullName}" );

        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );
        Assert.IsTrue( session.TimingSettings.Exists );
        Assert.IsNotNull( resourceSettings, $"{nameof( Pith.Settings.ResourceSettings )} should not be null." );
        Assert.IsTrue( resourceSettings.Exists, $"{nameof( Pith.Settings.ResourceSettings )} should exist in the settings file." );

        string propertyName = nameof( cc.isr.VI.Pith.SessionBase.ValidatedResourceName ).SplitWords();
        string actualResource = session.ValidatedResourceName;
        string expectedResource = resourceSettings.ResourceName;
        Assert.AreEqual( expectedResource, actualResource, $"{session.ResourceNameCaption} {propertyName} should match" );
        propertyName = nameof( cc.isr.VI.Pith.SessionBase.CandidateResourceName ).SplitWords();
        actualResource = session.CandidateResourceName;
        expectedResource = resourceSettings.ResourceName;
        Assert.AreEqual( expectedResource, actualResource, $"{session.ResourceNameCaption} {propertyName} should match" );
        propertyName = nameof( cc.isr.VI.Pith.SessionBase.OpenResourceName ).SplitWords();
        actualResource = session.OpenResourceName;
        expectedResource = resourceSettings.ResourceName;
        Assert.AreEqual( expectedResource, actualResource, $"{session.ResourceNameCaption} {propertyName} should match" );
        propertyName = nameof( cc.isr.VI.Pith.SessionBase.OpenResourceModel ).SplitWords();
        string actualResourceModel = session.OpenResourceModel;
        string expectedResourceModel = resourceSettings.ResourceModel;
        Assert.AreEqual( expectedResourceModel, actualResourceModel, $"{session.ResourceNameCaption} {propertyName} should match" );
        propertyName = nameof( cc.isr.VI.Pith.SessionBase.CandidateResourceModel ).SplitWords();
        actualResourceModel = session.CandidateResourceModel;
        Assert.AreEqual( expectedResourceModel, actualResourceModel, $"{session.ResourceNameCaption} {propertyName} should match" );
    }

    /// <summary>   Assert termination values should match. </summary>
    /// <remarks>   2025-01-16. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="ioSettings">   Information describing the Session IO. </param>
    public static void AssertTerminationValuesShouldMatch( cc.isr.VI.Pith.SessionBase? session, Pith.Settings.IOSettings? ioSettings )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );
        Assert.IsNotNull( ioSettings, $"{nameof( ioSettings )} should not be null." );

        System.Reflection.MethodBase? methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
        string methodFullName = $"{methodInfo?.DeclaringType?.Name}.{methodInfo?.Name}";
        Console.WriteLine( $"@{methodFullName}" );

        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );
        Assert.IsNotNull( ioSettings, $"{nameof( ioSettings )} should not be null." );
        // read termination is enabled from the status subsystem. It is disabled at the IVI Visa level. 
        bool actualReadTerminationEnabled = session.ReadTerminationCharacterEnabled;
        bool expectedReadTerminationEnabled = ioSettings.ReadTerminationEnabled;
        string propertyName = nameof( cc.isr.VI.Pith.SessionBase.ReadTerminationCharacterEnabled ).SplitWords();
        Assert.AreEqual( expectedReadTerminationEnabled, actualReadTerminationEnabled, $"{session.ResourceNameCaption} initial {propertyName} should match" );

        propertyName = nameof( cc.isr.VI.Pith.SessionBase.ReadTerminationCharacter ).SplitWords();
        int actualTermination = session.ReadTerminationCharacter;
        int expectedTermination = ioSettings.ReadTerminationCharacter;
        Assert.AreEqual( expectedTermination, actualTermination, $"{session.ResourceNameCaption} initial {propertyName} value should match" );

        propertyName = nameof( cc.isr.VI.Pith.SessionBase.ReadTerminationCharacterEnabled ).SplitWords();
        expectedReadTerminationEnabled = !ioSettings.ReadTerminationEnabled;
        session.ReadTerminationCharacterEnabled = expectedReadTerminationEnabled;
        actualReadTerminationEnabled = session.ReadTerminationCharacterEnabled;
        Assert.AreEqual( expectedReadTerminationEnabled, actualReadTerminationEnabled, $"{session.ResourceNameCaption} toggled {propertyName} should match" );

        expectedReadTerminationEnabled = ioSettings.ReadTerminationEnabled;
        session.ReadTerminationCharacterEnabled = expectedReadTerminationEnabled;
        actualReadTerminationEnabled = session.ReadTerminationCharacterEnabled;
        Assert.AreEqual( expectedReadTerminationEnabled, actualReadTerminationEnabled, $"{session.ResourceNameCaption} restored {propertyName} should match" );

        propertyName = nameof( cc.isr.VI.Pith.SessionBase.ReadTerminationCharacter ).SplitWords();
        actualTermination = session.ReadTerminationCharacter;
        expectedTermination = ioSettings.ReadTerminationCharacter;
        Assert.AreEqual( expectedTermination, actualTermination, $"{session.ResourceNameCaption} {propertyName} value should match" );
        Assert.AreEqual( ( byte ) session.TerminationCharacters().ElementAtOrDefault( 0 ), session.ReadTerminationCharacter, $"{session.ResourceNameCaption} first termination character value should match" );
    }

    /// <summary>   Assert session should collect garbage. </summary>
    /// <remarks>   2024-09-17. </remarks>
    /// <param name="session">                  The session. </param>
    /// <param name="deviceErrorTraceEnabled">  True to enable, false to disable the device error
    ///                                         trace. </param>
    public static void AssertSessionShouldCollectGarbage( cc.isr.VI.Pith.SessionBase? session, bool deviceErrorTraceEnabled )
    {
        Assert.IsNotNull( session, "session should not be null" );
        Assert.IsTrue( session.IsDeviceOpen, "session should be open" );
        if ( string.IsNullOrWhiteSpace( session.CollectGarbageOperationCompleteCommand ) ) return;

        Assert.IsTrue( session.TimingSettings.CollectGarbageTimeout > System.TimeSpan.Zero,
            $"{nameof( session.TimingSettings.CollectGarbageTimeout )} should be larger than zero." );

        if ( deviceErrorTraceEnabled )
            Trace.WriteLine( "session.CollectGarbageQueryComplete;. ",
            $"{nameof( Asserts )}.{nameof( Asserts.AssertSessionShouldCollectGarbage )}." );

        Assert.IsTrue( session.CollectGarbageQueryComplete(), "collecting garbage incomplete (reply not '1')." );

        if ( deviceErrorTraceEnabled )
            cc.isr.VI.Device.MSTest.Asserts.AssertMessageQueue();
        if ( deviceErrorTraceEnabled )
            cc.isr.VI.Device.MSTest.Asserts.AssertSessionLastError( session, session.ReadStatusByte() );
        else if ( session.IsErrorBitSet( session.ReadStatusByte() ) )
            Assert.Fail( "failed after collecting garbage" );
    }

    #endregion
}
