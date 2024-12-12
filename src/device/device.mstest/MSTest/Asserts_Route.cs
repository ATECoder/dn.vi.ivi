using System;
using System.Linq;
using cc.isr.Std.SplitExtensions;
using cc.isr.VI.Device.MSTest.Settings;

namespace cc.isr.VI.Device.MSTest;

public sealed partial class Asserts
{
    #region " route subsystem "

    /// <summary> Assert initial subsystem values should match. </summary>
    /// <param name="subsystem">      The subsystem. </param>
    /// <param name="subsystemsInfo"> Information describing the subsystems. </param>
    public static void AssertSubsystemInitialValuesShouldMatch( RouteSubsystemBase? subsystem, SubsystemsSettingsBase? subsystemsInfo )
    {
        Assert.IsNotNull( subsystem, $"{nameof( subsystem )} should not be null." );
        Assert.IsNotNull( subsystemsInfo, $"{nameof( subsystemsInfo )} should not be null." );
        string propertyName = nameof( RouteSubsystemBase.ClosedChannels ).SplitWords();
        string expectedClosedChannels = subsystemsInfo.InitialClosedChannels;
        if ( subsystem.SupportsClosedChannelsQuery )
        {
            string? actualClosedChannels = subsystem.QueryClosedChannels();
            Assert.AreEqual( expectedClosedChannels, actualClosedChannels, $"{subsystem.ResourceNameCaption} initial {propertyName} should be expected" );
        }

        if ( !subsystem.ScanListPersists )
        {
            propertyName = nameof( RouteSubsystemBase.ScanList ).SplitWords();
            string expectedScanList = subsystemsInfo.InitialScanList;
            string? actualScanList = subsystem.QueryScanList();
            Assert.AreEqual( expectedScanList, actualScanList, $"{subsystem.ResourceNameCaption} initial {propertyName} should be expected" );
        }
    }

    /// <summary> Assert slot card information should match. </summary>
    /// <param name="subsystem">    The subsystem. </param>
    /// <param name="cardNumber">   The card number. </param>
    /// <param name="expectedName"> Name of the expected. </param>
    public static void AssertSlotCardInfoShouldMatch( RouteSubsystemBase? subsystem, int cardNumber, string expectedName )
    {
        Assert.IsNotNull( subsystem, $"{nameof( subsystem )} should not be null." );
        string propertyName = nameof( RouteSubsystemBase.SlotCardType ).SplitWords();
        string? actualName = subsystem.QuerySlotCardType( cardNumber );
        Assert.AreEqual( expectedName, actualName, $"{subsystem.ResourceNameCaption} slot card #{cardNumber} {propertyName} should be expected" );
        propertyName = nameof( RouteSubsystemBase.SlotCardSettlingTime ).SplitWords();
        TimeSpan existingSettlingTime = subsystem.QuerySlotCardSettlingTime( cardNumber );
        TimeSpan expectedSettlingTime = TimeSpan.FromMilliseconds( 11d );
        TimeSpan actualSettlingTime = subsystem.ApplySlotCardSettlingTime( cardNumber, expectedSettlingTime );
        Assert.AreEqual( expectedSettlingTime, actualSettlingTime, $"{subsystem.ResourceNameCaption} slot card #{cardNumber} {propertyName} should be expected" );
        actualSettlingTime = subsystem.ApplySlotCardSettlingTime( cardNumber, existingSettlingTime );
        Assert.AreEqual( existingSettlingTime, actualSettlingTime, $"{subsystem.ResourceNameCaption} slot card #{cardNumber} restored {propertyName} should be expected" );
    }

    /// <summary> Assert scan card installed should match. </summary>
    /// <remarks> David, 2020-04-04. </remarks>
    /// <param name="subsystem">      The subsystem. </param>
    /// <param name="subsystemsInfo"> Information describing the subsystems. </param>
    public static void AssertScanCardInstalledShouldMatch( SystemSubsystemBase? subsystem, SubsystemsSettingsBase? subsystemsInfo )
    {
        Assert.IsNotNull( subsystem, $"{nameof( subsystem )} should not be null." );
        Assert.IsNotNull( subsystemsInfo, $"{nameof( subsystemsInfo )} should not be null." );
        Assert.AreEqual( subsystemsInfo.ScanCardInstalled, subsystem.InstalledScanCards.Any(), $"Scan card should {(subsystemsInfo.ScanCardInstalled ? string.Empty : "not")} be installed" );
    }

    /// <summary> Assert scan card internal scan should set. </summary>
    /// <remarks> David, 2020-04-04. </remarks>
    /// <param name="routeSubsystem">  The route subsystem. </param>
    /// <param name="systemSubsystem"> The system subsystem. </param>
    /// <param name="subsystemsInfo">  Information describing the subsystems. </param>
    public static void AssertScanCardInternalScanShouldSet( RouteSubsystemBase? routeSubsystem, SystemSubsystemBase? systemSubsystem, SubsystemsSettingsBase? subsystemsInfo )
    {
        if ( systemSubsystem is null || !systemSubsystem.InstalledScanCards.Any() )
            return;
        Assert.IsNotNull( routeSubsystem, $"{nameof( routeSubsystem )} should not be null." );
        Assert.IsNotNull( systemSubsystem, $"{nameof( systemSubsystem )} should not be null." );
        Assert.IsNotNull( subsystemsInfo, $"{nameof( subsystemsInfo )} should not be null." );

        string expectedScanList = subsystemsInfo.InitialScanList;
        string? actualScanList = routeSubsystem.QueryScanList();
        Assert.AreEqual( expectedScanList, actualScanList, $"Scan list should be '{expectedScanList}'" );
        expectedScanList = "(@1,2)";
        actualScanList = routeSubsystem.ApplyScanList( expectedScanList );
        Assert.AreEqual( expectedScanList, actualScanList, $"Scan list should be '{expectedScanList}'" );
        ScanListType expectedScanListType = ScanListType.None;
        string expectedSelectedScanListType = routeSubsystem.ScanListTypeReadWrites.SelectItem( ( long ) expectedScanListType ).ReadValue;
        string? actualSelectedScanListType = routeSubsystem.ApplySelectedScanListType( expectedSelectedScanListType );
        ScanListType actualScanListType = routeSubsystem.ScanListType;
        Assert.AreEqual( expectedSelectedScanListType, actualSelectedScanListType, $"Scan list type should be '{expectedSelectedScanListType}'" );
        Assert.AreEqual( expectedScanListType, actualScanListType, $"Scan list type should be '{expectedScanListType}'" );
        expectedScanListType = ScanListType.Internal;
        actualScanListType = routeSubsystem.ApplySelectedScanListType( expectedScanListType );
        Assert.AreEqual( expectedScanListType, actualScanListType, $"Applied scan list type should be '{expectedScanListType}'" );
        expectedScanListType = ScanListType.None;
        actualScanListType = routeSubsystem.ApplySelectedScanListType( expectedScanListType );
        Assert.AreEqual( expectedScanListType, actualScanListType, $"Cleared scan list type should be '{expectedScanListType}'" );
        expectedScanList = "(@1,2)";
        actualScanList = routeSubsystem.QueryScanList();
        Assert.AreEqual( expectedScanList, actualScanList, $"Scan list should be '{expectedScanList}'" );

        // Dim candidateScanListType As String = routeSubsystem.ScanListTypeReadWrites.SelectItem(expectedScanListType).WriteValue
        // expectedSelectedScanListType = "INT"
        // actualScanList = routeSubsystem.QueryScanList
        // Assert.AreEqual(expectedSelectedScanListType, actualSelectedScanListType, $"Scan list type should be '{expectedSelectedScanListType}'")
        // expectedSelectedScanListType = "NONE"
        // actualSelectedScanListType = routeSubsystem.ApplySelectedScanListType(expectedSelectedScanListType)
        // Assert.AreEqual(expectedSelectedScanListType, actualSelectedScanListType, $"Scan list type should be '{expectedSelectedScanListType}'")
    }

    #endregion
}
