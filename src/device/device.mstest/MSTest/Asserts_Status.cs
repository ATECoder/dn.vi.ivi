using System;
using cc.isr.Std.SplitExtensions;

namespace cc.isr.VI.Device.MSTest;

public sealed partial class Asserts
{
    #region " status subsystem "

    /// <summary> Assert line frequency should match. </summary>
    /// <param name="subsystem">      The subsystem. </param>
    /// <param name="subsystemsInfo"> Information describing the subsystems. </param>
    public static void AssertLineFrequencyShouldMatch( StatusSubsystemBase subsystem, VI.Settings.SubsystemsSettings? subsystemsInfo )
    {
        System.Reflection.MethodBase? methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
        string methodFullName = $"{methodInfo?.DeclaringType?.Name}.{methodInfo?.Name}";
        Console.WriteLine( $"@{methodFullName}" );

        Assert.IsNotNull( subsystem, $"{nameof( subsystem )} should not be null." );
        Assert.IsNotNull( subsystemsInfo, $"{nameof( subsystemsInfo )} should not be null." );
        string propertyName = nameof( StatusSubsystemBase.LineFrequency ).SplitWords();
        double actualFrequency = subsystem.LineFrequency.GetValueOrDefault( 0d );
        Assert.AreEqual( subsystemsInfo.LineFrequency, actualFrequency, $"{subsystem.ResourceNameCaption} {propertyName} should match" );
    }

    /// <summary> Assert integration period should match. </summary>
    /// <param name="subsystem">      The subsystem. </param>
    /// <param name="subsystemsInfo"> Information describing the subsystems. </param>
    public static void AssertIntegrationPeriodShouldMatch( StatusSubsystemBase subsystem, VI.Settings.SubsystemsSettings? subsystemsInfo )
    {
        System.Reflection.MethodBase? methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
        string methodFullName = $"{methodInfo?.DeclaringType?.Name}.{methodInfo?.Name}";
        Console.WriteLine( $"@{methodFullName}" );

        Assert.IsNotNull( subsystem, $"{nameof( subsystem )} should not be null." );
        Assert.IsNotNull( subsystemsInfo, $"{nameof( subsystemsInfo )} should not be null." );
        string propertyName = "Integration Period";
        double expectedPowerLineCycles = subsystemsInfo.InitialPowerLineCycles;
        TimeSpan expectedIntegrationPeriod = StatusSubsystemBase.FromSecondsPrecise( expectedPowerLineCycles / subsystemsInfo.LineFrequency );
        TimeSpan actualIntegrationPeriod = StatusSubsystemBase.FromPowerLineCycles( expectedPowerLineCycles );
        Assert.AreEqual( expectedIntegrationPeriod, actualIntegrationPeriod, $"{propertyName} for {expectedPowerLineCycles} power line cycles is {actualIntegrationPeriod}; expected {expectedIntegrationPeriod}" );
        propertyName = "Power line cycles";
        double actualPowerLineCycles = StatusSubsystemBase.ToPowerLineCycles( actualIntegrationPeriod );
        Assert.AreEqual( expectedPowerLineCycles, actualPowerLineCycles, subsystemsInfo.LineFrequency / TimeSpan.TicksPerSecond, $"{propertyName} is {actualPowerLineCycles:G5}; expected {expectedPowerLineCycles:G5}" );
    }

    /// <summary> Assert device model should match. </summary>
    /// <param name="subsystem">    The subsystem. </param>
    /// <param name="resourceInfo"> Information describing the resource. </param>
    public static void AssertDeviceModelShouldMatch( StatusSubsystemBase subsystem, Pith.Settings.ResourceSettings? resourceSettings )
    {
        System.Reflection.MethodBase? methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
        string methodFullName = $"{methodInfo?.DeclaringType?.Name}.{methodInfo?.Name}";
        Console.WriteLine( $"@{methodFullName}" );

        Assert.IsNotNull( subsystem, $"{nameof( subsystem )} should not be null." );
        Assert.IsNotNull( resourceSettings, $"{nameof( Pith.Settings.ResourceSettings )} should not be null." );
        Assert.IsTrue( resourceSettings.Exists, $"{nameof( Pith.Settings.ResourceSettings )} should exist in the settings file." );

        string propertyName = nameof( VersionInfoBase.Model ).SplitWords();
        Assert.AreEqual( resourceSettings.ResourceModel, subsystem.VersionInfoBase.Model,
            $"Resource settings model {resourceSettings.ResourceModel} should equal Version Info {propertyName} from {subsystem.ResourceNameCaption} Identity '{subsystem.VersionInfoBase.Identity}'" );
    }

    /// <summary> Assert orphan messages (discarded data) should be empty. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    public static void AssertOrphanMessagesShouldBeEmpty( StatusSubsystemBase subsystem )
    {
        System.Reflection.MethodBase? methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
        string methodFullName = $"{methodInfo?.DeclaringType?.Name}.{methodInfo?.Name}";
        Console.WriteLine( $"@{methodFullName}" );

        Assert.IsNotNull( subsystem, $"{nameof( subsystem )} should not be null." );
        string orphanMessages = subsystem.Session.DiscardedData;
        Assert.IsTrue( string.IsNullOrWhiteSpace( orphanMessages ), $"{subsystem.ResourceNameCaption} orphan messages {orphanMessages} should be empty" );
    }

    /// <summary>   Assert keep alive message could be sent. </summary>
    /// <remarks>   David, 2021-06-01. </remarks>
    /// <param name="subsystem">    The subsystem. </param>
    public static void AssertKeepAliveMessageCouldBeSent( StatusSubsystemBase subsystem )
    {
        Assert.IsNotNull( subsystem, $"{nameof( subsystem )} should not be null." );
        subsystem.Session.KeepAliveLockTimeout = TimeSpan.FromMilliseconds( 12 );
        System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
        subsystem.Session.KeepAlive();
        sw.Stop();
        Console.Out.WriteLine( $"Keep alive took {sw.ElapsedMilliseconds:F0}ms" );
        _ = subsystem.ReadIdentity();
    }

    #endregion
}
