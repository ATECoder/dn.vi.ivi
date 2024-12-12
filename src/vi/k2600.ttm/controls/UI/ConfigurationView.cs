using System;
using System.ComponentModel;

namespace cc.isr.VI.Tsp.K2600.Ttm.Controls;

/// <summary> Panel for editing the TTM configuration. </summary>
/// <remarks>
/// (c) 2014 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2014-02-25 </para>
/// </remarks>
public partial class ConfigurationView : ConfigurationViewBase
{
    /// <summary>   Default constructor. </summary>
    /// <remarks>   David, 2021-09-04. </remarks>
    public ConfigurationView() => this.InitializeComponent();

    /// <summary>
    /// Releases the unmanaged resources used by the object and optionally releases the managed resources.
    /// </summary>
    /// <remarks>   David, 2021-09-08. </remarks>
    /// <param name="disposing">    true to release both managed and unmanaged resources; false to
    ///                             release only unmanaged resources. </param>
    protected override void Dispose( bool disposing )
    {
        try
        {
            if ( disposing )
            {
            }
        }
        finally
        {
            base.Dispose( disposing );
        }
    }

    #region " configure "

    /// <summary> Releases the resources. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    protected override void ReleaseResources()
    {
        base.ReleaseResources();
        this._meter = null;
    }

    private Meter? _meter;

    /// <summary> Gets or sets reference to the thermal transient meter device. </summary>
    /// <value> The meter. </value>
    [Browsable( false )]
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public Meter? Meter
    {
        get => this._meter;
        set
        {
            if ( this.SetProperty( ref this._meter, value ) )
                this.DeviceUnderTest = value is null ? null : this.Meter!.ConfigInfo;
        }
    }

    /// <summary> Configure changed. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="part"> The part. </param>
    protected override void ConfigureChanged( DeviceUnderTest part )
    {
        if ( this.Meter is null ) throw new InvalidOperationException( $"{nameof( ConfigurationView )}.{nameof( ConfigurationView.Meter )} is null." );
        this.Meter?.ConfigureChanged( part );
    }

    /// <summary> Configures the given part. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="part"> The part. </param>
    protected override void Configure( DeviceUnderTest part )
    {
        if ( this.Part is null ) throw new InvalidOperationException( $"{nameof( ConfigurationView )}.{nameof( ConfigurationView.Part )} is null." );
        if ( this.Meter is null ) throw new InvalidOperationException( $"{nameof( ConfigurationView )}.{nameof( ConfigurationView.Meter )} is null." );
        this.Meter.Configure( this.Part );
    }

    #endregion
}
