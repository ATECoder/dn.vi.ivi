namespace cc.isr.VI;

/// <summary> Defines a SCPI Instrument Subsystem. </summary>
/// <remarks>
/// (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2016-07-01, 4.0.6026. </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="InstrumentSubsystemBase" /> class.
/// </remarks>
/// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
/// subsystem</see>. </param>
[CLSCompliant( false )]
public abstract class InstrumentSubsystemBase( StatusSubsystemBase statusSubsystem ) : SubsystemBase( statusSubsystem )
{
    #region " dmm installed "

    /// <summary> DMM Installed. </summary>
    private bool? _dmmInstalled;

    /// <summary> Gets or sets the cached DMM Installed sentinel. </summary>
    /// <value>
    /// <c>null</c> if DMM Installed is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? DmmInstalled
    {
        get => this._dmmInstalled;

        protected set
        {
            if ( !Equals( this.DmmInstalled, value ) )
            {
                this._dmmInstalled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the DMM Installed query command. </summary>
    /// <value> The DMM Installed query command. </value>
    protected virtual string DmmInstalledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the DMM Installed sentinel. Also sets the
    /// <see cref="DmmInstalled">DMM Installed</see> sentinel.
    /// </summary>
    /// <returns>
    /// <c>null</c> Instrument status is not known; <c>true</c> if DmmInstalled; otherwise,
    /// <c>false</c>.
    /// </returns>
    public bool? QueryDmmInstalled()
    {
        this.Session.MakeEmulatedReplyIfEmpty( this.DmmInstalled.GetValueOrDefault( true ) );
        if ( !string.IsNullOrWhiteSpace( this.DmmInstalledQueryCommand ) )
        {
            this.DmmInstalled = this.Session.Query( this.DmmInstalled.GetValueOrDefault( true ), this.DmmInstalledQueryCommand );
        }

        return this.DmmInstalled;
    }

    #endregion
}
