namespace cc.isr.VI.Tsp.K2600.Ttm.Tests.Firmware;

internal static partial class Asserts
{
    /// <summary>   Asserts that a current should be measures. </summary>
    /// <remarks>   2024-10-26. </remarks>
    /// <param name="session">  The session. </param>
    /// <returns>   The measured current <see cref="string" />. </returns>
    public static string AssertCurrentShouldBeMeasured( Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );

        _ = session.ReadStatusByte();
        _ = session.WriteLine( "print(smua.measure.i())" );
        string reading = session.ReadLineTrimEnd();
        Assert.IsFalse( string.IsNullOrEmpty( reading ), $"The current reading should not be null or empty." );
        return reading;
    }
}
