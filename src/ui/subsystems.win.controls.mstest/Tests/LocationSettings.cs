namespace cc.isr.VI.SubsystemsWinControls.Tests;

/// <summary>   Provides settings for all tests. </summary>
/// <remarks>   2023-04-24. </remarks>
internal sealed class LocationSettings : cc.isr.Json.AppSettings.Settings.LocationSettingsBase
{
    #region " scribe "

    /// <summary>   Creates the scribe. </summary>
    /// <remarks>   2025-10-30. </remarks>
    public override void CreateScribe()
    {
        this.Scribe = string.IsNullOrWhiteSpace( this.SectionName )
            ? new( [this] )
            : new( [this.SectionName], [this] );
    }

    #endregion

}
