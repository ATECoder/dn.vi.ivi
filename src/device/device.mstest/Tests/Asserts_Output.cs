using System.Collections.Generic;

namespace cc.isr.VI.Device.Tests;

public sealed partial class Asserts
{
    #region " output subsystem "

    /// <summary> Assert digital output signal polarity should toggle. </summary>
    /// <param name="digitalOutputLineNumber"> The digital output line number. </param>
    /// <param name="subsystem">               The subsystem. </param>
    public static void AssertDigitalOutputSignalPolarityShouldToggle( int digitalOutputLineNumber, DigitalOutputSubsystemBase subsystem )
    {
        Assert.IsNotNull( subsystem, $"{nameof( subsystem )} should not be null." );
        Assert.IsNotNull( subsystem.ResourceNameCaption );
        string deviceName = subsystem.ResourceNameCaption;

        string propertyName = $"{typeof( DigitalOutputSubsystemBase )}.{nameof( DigitalOutputSubsystemBase.CurrentDigitalActiveLevel )}";
        string activity = $"Reading [{deviceName}].[{propertyName}({digitalOutputLineNumber})] initial value";
        DigitalActiveLevels? initialPolarity = subsystem.QueryDigitalActiveLevel( digitalOutputLineNumber );
        Assert.IsTrue( initialPolarity.HasValue, $"{activity} should have a value" );

        DigitalActiveLevels expectedPolarity = initialPolarity.Value == DigitalActiveLevels.High ? DigitalActiveLevels.Low : DigitalActiveLevels.High;
        activity = $"Setting [{deviceName}].[{propertyName}({digitalOutputLineNumber})] to {expectedPolarity}";
        DigitalActiveLevels? actualPolarity = subsystem.ApplyDigitalActiveLevel( digitalOutputLineNumber, expectedPolarity );
        Assert.IsTrue( actualPolarity.HasValue, $"{activity} should have a value" );
        Assert.AreEqual( expectedPolarity, ( object ) actualPolarity, $"{activity} should equal actual value" );

        expectedPolarity = initialPolarity.Value;
        activity = $"Setting [{deviceName}].[{propertyName}({digitalOutputLineNumber})] to {expectedPolarity}";
        actualPolarity = subsystem.ApplyDigitalActiveLevel( digitalOutputLineNumber, expectedPolarity );
        Assert.IsTrue( actualPolarity.HasValue, $"{activity} should have a value" );
        Assert.AreEqual( expectedPolarity, ( object ) actualPolarity, $"{activity} should equal actual value" );
    }

    /// <summary> Assert digital output signal polarity should toggle. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    public static void AssertDigitalOutputSignalPolarityShouldToggle( DigitalOutputSubsystemBase subsystem )
    {
        List<DigitalActiveLevels> initialPolarities = [];
        List<int> inputLineNumbers = new( [1, 2, 3, 4] );
        System.Text.StringBuilder inputLineNumbersCaption = new();
        int inputLineNumber;
        foreach ( int currentInputLineNumber in inputLineNumbers )
        {
            inputLineNumber = currentInputLineNumber;
            if ( inputLineNumbersCaption.Length > 0 )
                _ = inputLineNumbersCaption.Append( ',' );

#if NET5_0_OR_GREATER
#pragma warning disable CA1305
#endif
            _ = inputLineNumbersCaption.Append( $"{inputLineNumber}" );
#if NET5_0_OR_GREATER
#pragma warning restore CA1305
#endif
            initialPolarities.Add( subsystem.QueryDigitalActiveLevel( inputLineNumber ).GetValueOrDefault( DigitalActiveLevels.Low ) );
        }

        foreach ( int currentInputLineNumber1 in inputLineNumbers )
        {
            inputLineNumber = currentInputLineNumber1;
            AssertDigitalOutputSignalPolarityShouldToggle( inputLineNumber, subsystem );
        }

        inputLineNumber = inputLineNumbers[0];
        string propertyName = $"{typeof( DigitalOutputSubsystemBase )}.{nameof( DigitalOutputSubsystemBase.CurrentDigitalActiveLevel )}";
        Assert.IsNotNull( subsystem.ResourceNameCaption );
        string deviceName = subsystem.ResourceNameCaption;
        string activity = $"Reading [{deviceName}].[{propertyName}({inputLineNumber})] initial value";
        DigitalActiveLevels? initialPolarity = subsystem.QueryDigitalActiveLevel( inputLineNumber );
        Assert.IsTrue( initialPolarity.HasValue, $"{activity} should have a value" );
        DigitalActiveLevels expectedPolarity = initialPolarity.Value == DigitalActiveLevels.High ? DigitalActiveLevels.Low : DigitalActiveLevels.High;
        activity = $"Setting [{deviceName}].[{propertyName}({inputLineNumbersCaption})] to {expectedPolarity}";
        DigitalActiveLevels? actualPolarity = subsystem.ApplyDigitalActiveLevel( inputLineNumbers, expectedPolarity );
        Assert.IsTrue( actualPolarity.HasValue, $"{activity} should have a value" );
        Assert.AreEqual( expectedPolarity, ( object ) actualPolarity, $"{activity} should equal actual value" );
        expectedPolarity = initialPolarity.Value;
        activity = $"Setting [{deviceName}].[{propertyName}({inputLineNumbersCaption})] to {expectedPolarity}";
        actualPolarity = subsystem.ApplyDigitalActiveLevel( inputLineNumbers, expectedPolarity );
        Assert.IsTrue( actualPolarity.HasValue, $"{activity} should have a value" );
        Assert.AreEqual( expectedPolarity, ( object ) actualPolarity, $"{activity} should equal actual value" );
        activity = $"Applying initial [{deviceName}].[{propertyName}s] to lines {inputLineNumbersCaption}";
        List<DigitalActiveLevels?> actualPolarities = new( subsystem.ApplyDigitalActiveLevels( inputLineNumbers, initialPolarities ) );
        foreach ( int currentInputLineNumber2 in inputLineNumbers )
        {
            inputLineNumber = currentInputLineNumber2;
            Assert.IsNotNull( actualPolarities[inputLineNumber - 1] );
            Assert.AreEqual( initialPolarities[inputLineNumber - 1],
                actualPolarities[inputLineNumber - 1]!.Value, $"{activity} [{deviceName}].[{propertyName}({inputLineNumber})] should equal actual value" );
        }
    }

    #endregion
}
