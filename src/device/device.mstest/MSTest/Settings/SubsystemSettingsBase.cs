using System;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace cc.isr.VI.Device.MSTest.Settings;

/// <summary>   Subsystem test settings base class. </summary>
/// <remarks>
/// <para>
/// David, 2018-02-12 </para>
/// </remarks>
[CLSCompliant( false )]
public class SubsystemSettingsBase() : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    #region " exists "

    private bool _exists;

    /// <summary>
    /// Gets or sets a value indicating whether this settings section exists and the values were thus
    /// fetched from the settings file.
    /// </summary>
    /// <value> True if this settings section exists in the settings file, false if not. </value>
	[Description( "True if this settings section exists and was read from the JSon settings file." )]
    public bool Exists
    {
        get => this._exists;
        set => _ = this.SetProperty( ref this._exists, value );
    }

    #endregion
}

