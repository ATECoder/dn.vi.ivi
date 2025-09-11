using cc.isr.VI.Tsp;

namespace cc.isr.VI.Device.Tsp.Tests;

public sealed partial class Asserts
{
    #region " source subsystem "

    /// <summary>
    /// Assert that the source function, e.g., <see cref="SourceFunctionMode.CurrentDC"/> should apply.
    /// </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <param name="device">               The device. </param>
    /// <param name="expectedFunctionMode"> The expected function mode. </param>
    public static void AssertSourceFunctionShouldApply( VI.Tsp.SourceSubsystemBase? sourceSubsystem, SourceFunctionMode expectedFunctionMode )
    {
        Assert.IsNotNull( sourceSubsystem );
        SourceFunctionMode? sourceFunction = sourceSubsystem.ApplySourceFunction( expectedFunctionMode ).GetValueOrDefault( SourceFunctionMode.None );
        Assert.IsNotNull( sourceFunction );
        Assert.AreEqual( expectedFunctionMode, sourceFunction,
            $"{typeof( VI.Tsp.SourceSubsystemBase )}.{nameof( VI.Tsp.SourceSubsystemBase.SourceFunction )} is {sourceFunction} ; expected {expectedFunctionMode}" );
    }


    #endregion
}
