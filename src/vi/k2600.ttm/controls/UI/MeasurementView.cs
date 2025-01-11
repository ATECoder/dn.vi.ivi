using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace cc.isr.VI.Tsp.K2600.Ttm.Controls;

/// <summary> Panel for editing the measurement. </summary>
/// <remarks>
/// (c) 2014 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2014-03-17 </para>
/// </remarks>
public partial class MeasurementView : MeasurementViewBase
{
    /// <summary>   Default constructor. </summary>
    /// <remarks>   David, 2021-09-04. </remarks>
    public MeasurementView() => this.InitializeComponent();

    /// <summary> Releases the resources. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    protected override void ReleaseResources()
    {
        this.Meter = null;
        base.ReleaseResources();
    }

    /// <summary> Gets the is device open. </summary>
    /// <value> The is device open. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public override bool IsDeviceOpen => this.Meter is not null && this.Meter.IsDeviceOpen;

    /// <summary> Gets or sets the meter. </summary>
    /// <value> The meter. </value>
    [Browsable( false )]
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public Meter? Meter
    {
        get;
        set
        {
            field = value;
            if ( value is not null )
            {
                this.TspDevice = value.TspDevice;
                this.TriggerSequencer = value.TriggerSequencer;
                this.MeasureSequencer = value.MeasureSequencer;
                this.OnStateChanged();
            }
        }
    }

    /// <summary> Abort Trigger Sequence if started. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    protected override void AbortTriggerSequenceIf()
    {
        if ( this.Meter is null ) throw new InvalidOperationException( $"{nameof( MeasurementView )}.{nameof( this.Meter )} is null." );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Aborting measurements;. " );
        this.Meter.AbortTriggerSequenceIf();
    }

    /// <summary> Clears the part measurements. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    protected override void ClearPartMeasurements()
    {
        if ( this.Meter is null ) throw new InvalidOperationException( $"{nameof( MeasurementView )}.{nameof( this.Meter )} is null." );
        if ( this.Meter.Part is null ) throw new InvalidOperationException( $"{nameof( MeasurementView )}.{nameof( this.Meter )}.{nameof( this.Meter.Part )} is null." );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Clearing part measurements;. " );
        this.Meter.Part?.ClearMeasurements();
    }

    /// <summary> Measure initial resistance. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The measurement <see cref="MeasurementOutcomes">outcome</see>. </returns>
    protected override MeasurementOutcomes MeasureInitialResistance()
    {
        if ( this.Meter is null ) throw new InvalidOperationException( $"{nameof( MeasurementView )}.{nameof( this.Meter )} is null." );
        if ( this.Meter.Part is null ) throw new InvalidOperationException( $"{nameof( MeasurementView )}.{nameof( this.Meter )}.{nameof( this.Meter.Part )} is null." );
        if ( this.Meter.Part.InitialResistance is null )
            throw new InvalidOperationException( $"{nameof( MeasurementView )}.{nameof( this.Meter )}.{nameof( this.Meter.Part )}.{nameof( this.Meter.Part.InitialResistance )} is null." );
        this.Meter.MeasureInitialResistance( this.Meter.Part.InitialResistance );
        return this.Meter.Part.Outcome;
    }

    /// <summary> Measure thermal transient. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The measurement <see cref="MeasurementOutcomes">outcome</see>. </returns>
    protected override MeasurementOutcomes MeasureThermalTransient()
    {
        if ( this.Meter is null ) throw new InvalidOperationException( $"{nameof( MeasurementView )}.{nameof( this.Meter )} is null." );
        if ( this.Meter.Part is null ) throw new InvalidOperationException( $"{nameof( MeasurementView )}.{nameof( this.Meter )}.{nameof( this.Meter.Part )} is null." );
        if ( this.Meter.Part.ThermalTransient is null )
            throw new InvalidOperationException( $"{nameof( MeasurementView )}.{nameof( this.Meter )}.{nameof( this.Meter.Part )}.{nameof( this.Meter.Part.ThermalTransient )} is null." );
        this.Meter.MeasureThermalTransient( this.Meter.Part.ThermalTransient );
        return this.Meter.Part.Outcome;
    }

    /// <summary> Measure final resistance. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The measurement <see cref="MeasurementOutcomes">outcome</see>. </returns>
    protected override MeasurementOutcomes MeasureFinalResistance()
    {
        if ( this.Meter is null ) throw new InvalidOperationException( $"{nameof( MeasurementView )}.{nameof( this.Meter )} is null." );
        if ( this.Meter.Part is null ) throw new InvalidOperationException( $"{nameof( MeasurementView )}.{nameof( this.Meter )}.{nameof( this.Meter.Part )} is null." );
        if ( this.Meter.Part.FinalResistance is null )
            throw new InvalidOperationException( $"{nameof( MeasurementView )}.{nameof( this.Meter )}.{nameof( this.Meter.Part )}.{nameof( this.Meter.Part.FinalResistance )} is null." );
        this.Meter.MeasureFinalResistance( this.Meter.Part.FinalResistance );
        return this.Meter.Part.Outcome;
    }

    /// <summary> Displays a thermal transient trace described by chart. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="chart"> The chart. </param>
    /// <returns> A list of trace values. </returns>
    protected override IList<isr.Std.Cartesian.CartesianPoint<double>> DisplayThermalTransientTrace( Chart chart )
    {
        if ( this.Meter is null ) throw new InvalidOperationException( $"{nameof( MeasurementView )}.{nameof( this.Meter )} is null." );
        if ( this.Meter.ThermalTransient is null ) throw new InvalidOperationException( $"{nameof( MeasurementView )}.{nameof( this.Meter )}.{nameof( this.Meter.ThermalTransient )} is null." );
        if ( this.Meter.ThermalTransient.LastTimeSeries is null )
            throw new InvalidOperationException( $"{nameof( MeasurementView )}.{nameof( this.Meter )}.{nameof( this.Meter.ThermalTransient )}.{nameof( this.Meter.ThermalTransient.LastTimeSeries )} is null." );
        this.ChartThermalTransientTrace( chart );
        return this.Meter.ThermalTransient.LastTimeSeries;
    }

    /// <summary> Models the thermal transient trace. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="chart"> The chart. </param>
    protected override void ModelThermalTransientTrace( Chart chart )
    {
        if ( this.Meter is null ) throw new InvalidOperationException( $"{nameof( MeasurementView )}.{nameof( this.Meter )} is null." );
        if ( this.Meter.ThermalTransient is null ) throw new InvalidOperationException( $"{nameof( MeasurementView )}.{nameof( this.Meter )}.{nameof( this.Meter.ThermalTransient )} is null." );
        if ( this.Meter.Part is null ) throw new InvalidOperationException( $"{nameof( MeasurementView )}.{nameof( this.Meter )}.{nameof( this.Meter.Part )} is null." );
        if ( this.Meter.Part.ThermalTransient is null )
            throw new InvalidOperationException( $"{nameof( MeasurementView )}.{nameof( this.Meter )}.{nameof( this.Meter.Part )}.{nameof( this.Meter.Part.ThermalTransient )} is null." );
        this.Meter.ThermalTransient.ModelTransientResponse( this.Meter.Part.ThermalTransient );
        this.DisplayModel( chart );
    }

    #region " charting "

    /// <summary> Displays a thermal transient trace. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="chart"> The <see cref="System.Windows.Forms.DataVisualization.Charting.Chart">Chart</see>. </param>
    public void ChartThermalTransientTrace( Chart? chart )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( chart, nameof( chart ) );
#else
        if ( chart is null ) throw new ArgumentNullException( nameof( chart ) );
#endif

        if ( this.Meter is null ) throw new InvalidOperationException( $"{nameof( MeasurementView )}.{nameof( MeasurementView.Meter )} is null." );
        if ( this.Meter.ThermalTransient is null )
            throw new InvalidOperationException( $"{nameof( MeasurementView )}.{nameof( MeasurementView.Meter )}.{nameof( MeasurementView.Meter.ThermalTransient )} is null." );

        if ( this.Meter.ThermalTransient.NewTraceReadyToRead )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Reading trace;. " );
            Stopwatch st = Stopwatch.StartNew();
            this.Meter.ThermalTransient.ReadThermalTransientTrace();
            TimeSpan t = st.Elapsed;
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Read trace in {t.TotalMilliseconds} ms;. " );
        }
        else
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Displaying thermal transient trace;. " );
        }

        ConfigureTraceChart( chart );
        DisplayTrace( chart, this.Meter.ThermalTransient.LastTimeSeries );
    }

    /// <summary> Displays a trace described by timeSeries. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="chart"> The <see cref="System.Windows.Forms.DataVisualization.Charting.Chart">Chart</see>. </param>
    public void DisplayModel( Chart? chart )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( chart, nameof( chart ) );
#else
        if ( chart is null ) throw new ArgumentNullException( nameof( chart ) );
#endif

        if ( this.Meter is null ) throw new InvalidOperationException( $"{nameof( MeasurementView )}.{nameof( MeasurementView.Meter )} is null." );
        if ( this.Meter.ThermalTransient is null )
            throw new InvalidOperationException( $"{nameof( MeasurementView )}.{nameof( MeasurementView.Meter )}.{nameof( MeasurementView.Meter.ThermalTransient )} is null." );
        if ( this.Meter.ThermalTransient.Model is null )
            throw new InvalidOperationException( $"{nameof( MeasurementView )}.{nameof( MeasurementView.Meter )}.{nameof( MeasurementView.Meter.ThermalTransient )}.{nameof( MeasurementView.Meter.ThermalTransient.Model )} is null." );
        if ( this.Meter.ThermalTransient.LastTimeSeries is null )
            throw new InvalidOperationException( $"{nameof( MeasurementView )}.{nameof( MeasurementView.Meter )}.{nameof( MeasurementView.Meter.ThermalTransient )}.{nameof( MeasurementView.Meter.ThermalTransient.LastTimeSeries )} is null." );

        if ( chart.Series.Count == 1 )
        {
            _ = chart.Series.Add( "VersionInfoBase.Model" );
            chart.Series[1].ChartType = SeriesChartType.Line;
        }

        Series s = chart.Series[1];
        s.Points.SuspendUpdates();
        s.Points.Clear();
        s.Color = System.Drawing.Color.Chocolate;
        for ( int i = 0, loopTo = this.Meter.ThermalTransient.Model.FunctionValues().Count() - 1; i <= loopTo; i++ )
        {
            double x = this.Meter.ThermalTransient.LastTimeSeries[i].X;
            double y = this.Meter.ThermalTransient.Model.FunctionValues().ElementAtOrDefault( i );
            if ( x > 0d && y > 0d )
            {
                _ = s.Points.AddXY( x, y );
            }
        }

        s.Points.ResumeUpdates();
        s.Points.Invalidate();
    }

    /// <summary> Configure trace chart. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="chart"> The <see cref="System.Windows.Forms.DataVisualization.Charting.Chart">Chart</see>. </param>
    private static void ConfigureTraceChart( Chart? chart )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( chart, nameof( chart ) );
#else
        if ( chart is null ) throw new ArgumentNullException( nameof( chart ) );
#endif

        if ( chart.ChartAreas.Count == 0 )
        {
            ChartArea chartArea = new( "Area" );
            chart.ChartAreas.Add( chartArea );
            chartArea.AxisY.Title = "Voltage, mV";
            chartArea.AxisY.Minimum = 0d;
            chartArea.AxisY.TitleFont = new System.Drawing.Font( chart.Parent!.Font, System.Drawing.FontStyle.Bold );
            chartArea.AxisX.Title = "Time, ms";
            chartArea.AxisX.Minimum = 0d;
            chartArea.AxisX.TitleFont = new System.Drawing.Font( chart.Parent.Font, System.Drawing.FontStyle.Bold );
            chartArea.AxisX.IsLabelAutoFit = true;
            chartArea.AxisX.LabelStyle.Format = "#.0";
            chartArea.AxisX.LabelStyle.Enabled = true;
            chartArea.AxisX.LabelStyle.IsEndLabelVisible = true;
            chart.Location = new System.Drawing.Point( 82, 16 );
            chart.Series.Add( new Series( "Trace" ) );
            Series s = chart.Series["Trace"];
            s.SmartLabelStyle.Enabled = true;
            s.ChartArea = "Area";
            // s.Legend = "Legend"
            s.ChartType = SeriesChartType.Line;
            s.XAxisType = AxisType.Primary;
            s.YAxisType = AxisType.Primary;
            s.XValueType = ChartValueType.Double;
            s.YValueType = ChartValueType.Double;
            chart.Text = "Thermal Transient Trace";
            chart.Titles.Add( "Thermal Transient Trace" ).Font = new System.Drawing.Font( chart.Parent.Font.FontFamily, 11f, System.Drawing.FontStyle.Bold );
        }
    }

    /// <summary> Gets a simulate time series. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="asymptote">    The asymptote. </param>
    /// <param name="timeConstant"> The time constant. </param>
    /// <returns> null if it fails, else a list of. </returns>
    private static List<isr.Std.Cartesian.CartesianPoint<double>> Simulate( double asymptote, double timeConstant )
    {
        List<Std.Cartesian.CartesianPoint<double>> l = [];
        double vc = 0d;
        double deltaT = 0.1d;
        for ( int i = 0; i <= 99; i++ )
        {
            double deltaV = deltaT * (asymptote - vc) / timeConstant;
            l.Add( new cc.isr.Std.Cartesian.CartesianPoint<double>( ( float ) (deltaT * (i + 1)), ( float ) vc ) );
            vc += deltaV;
        }

        return l;
    }

    /// <summary> Displays a trace described by timeSeries. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="chart">      The <see cref="System.Windows.Forms.DataVisualization.Charting.Chart">Chart</see>. </param>
    /// <param name="timeSeries"> The time series in millivolts and milliseconds. </param>
    private static void DisplayTrace( Chart chart, IList<isr.Std.Cartesian.CartesianPoint<double>>? timeSeries )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( chart, nameof( chart ) );
        ArgumentNullException.ThrowIfNull( timeSeries, nameof( timeSeries ) );
#else
        if ( chart is null ) throw new ArgumentNullException( nameof( chart ) );
        if ( timeSeries is null ) throw new ArgumentNullException( nameof( timeSeries ) );
#endif

        Series s = chart.Series[0];
        s.Points.SuspendUpdates();
        s.Points.Clear();
        _ = s.Points.AddXY( 0d, 0d );
        double deltaT = 0d;
        if ( timeSeries.Count > 1 )
        {
            deltaT = timeSeries[1].X - timeSeries[0].X;
        }

        foreach ( cc.isr.Std.Cartesian.CartesianPoint<double> v in timeSeries )
            _ = s.Points.AddXY( v.X, v.Y );
        // Add one more point
        _ = s.Points.AddXY( timeSeries[timeSeries.Count - 1].X + deltaT, (2d * timeSeries[timeSeries.Count - 1].Y) - timeSeries[timeSeries.Count - 2].Y );
        s.Points.ResumeUpdates();
        s.Points.Invalidate();
    }

    /// <summary> Displays a thermal transient trace. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="chart"> The <see cref="System.Windows.Forms.DataVisualization.Charting.Chart">Chart</see>. </param>
    public static void EmulateThermalTransientTrace( Chart chart )
    {
        ConfigureTraceChart( chart );
        DisplayTrace( chart, Simulate( 100d, 5d ) );
    }

    #endregion
}
