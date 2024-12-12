using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using cc.isr.Enums;

namespace cc.isr.VI.Tsp.K2600.Ttm.Controls;

/// <summary> Measurement panel base. </summary>
/// <remarks>
/// (c) 2014 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2014-04-12 </para><para>
/// David, 2014-03-15 </para>
/// </remarks>
public partial class MeasurementViewBase : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary>
    /// A private constructor for this class making it not publicly creatable. This ensure using the
    /// class as a singleton.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    protected MeasurementViewBase() : base()
    {
        this.InitializeComponent();
        this._ttmMeasureControlsToolStrip.Enabled = false;
    }

    /// <summary>
    /// Releases the unmanaged resources used by the object and optionally releases the managed resources.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="disposing"> true to release both managed and unmanaged resources; false to
    ///                          release only unmanaged resources. </param>
    [DebuggerNonUserCode()]
    protected override void Dispose( bool disposing )
    {
        try
        {
            if ( disposing )
            {
                this.ReleaseResources();
                this.components?.Dispose();
            }
        }
        finally
        {
            base.Dispose( disposing );
        }
    }

    #endregion

    #region " on control events "

    /// <summary> Releases the resources. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    protected virtual void ReleaseResources()
    {
        this.TriggerSequencerInternal = null;
        this.MeasureSequencerInternal = null;
        this.LastTimeSeries = null;
    }

    /// <summary> Gets or sets the is device open. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The is device open. </value>
    [Browsable( false )]
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public virtual bool IsDeviceOpen { get; private set; }

    /// <summary> True if trace is available, false if not. </summary>
    private bool _isTraceAvailable;

    /// <summary> Updates the availability of the controls. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    protected void OnStateChanged()
    {
        MeasurementSequenceState measurementSequenceState = MeasurementSequenceState.None;
        if ( this.MeasureSequencer is not null )
        {
            measurementSequenceState = this.MeasureSequencer.MeasurementSequenceState;
        }

        TriggerSequenceState triggerSequenceState = TriggerSequenceState.None;
        if ( this.TriggerSequencer is not null )
        {
            triggerSequenceState = this.TriggerSequencer.TriggerSequenceState;
        }

        this._ttmMeasureControlsToolStrip.Enabled = this.IsDeviceOpen;
        this._triggerToolStripDropDownButton.Enabled = this.IsDeviceOpen && MeasurementSequenceState.Idle == measurementSequenceState;
        if ( this._triggerToolStripDropDownButton.Enabled )
        {
            this._waitForTriggerToolStripMenuItem.Enabled = this.IsDeviceOpen && (MeasurementSequenceState.Idle == measurementSequenceState || TriggerSequenceState.WaitingForTrigger == triggerSequenceState);
            this._assertTriggerToolStripMenuItem.Enabled = TriggerSequenceState.WaitingForTrigger == triggerSequenceState;
        }

        this._measureToolStripDropDownButton.Enabled = this.IsDeviceOpen && TriggerSequenceState.Idle == triggerSequenceState;
        if ( this._ttmMeasureControlsToolStrip.Enabled )
        {
            this._initialResistanceToolStripMenuItem.Enabled = MeasurementSequenceState.Idle == measurementSequenceState;
            this._thermalTransientToolStripMenuItem.Enabled = MeasurementSequenceState.Idle == measurementSequenceState;
            this._finalResistanceToolStripMenuItem.Enabled = MeasurementSequenceState.Idle == measurementSequenceState;
            this._measureAllToolStripMenuItem.Enabled = MeasurementSequenceState.Idle == measurementSequenceState;
            this._abortSequenceToolStripMenuItem.Enabled = MeasurementSequenceState.Idle != measurementSequenceState;
        }

        this._traceToolStripDropDownButton.Enabled = this.IsDeviceOpen && TriggerSequenceState.Idle == triggerSequenceState && MeasurementSequenceState.Idle == measurementSequenceState;
        if ( this._traceToolStripDropDownButton.Enabled )
        {
            this._readTraceToolStripMenuItem.Enabled = this._isTraceAvailable;
            bool enabled = this.LastTimeSeries is object && this.LastTimeSeries.Any();
            this._saveTraceToolStripMenuItem.Enabled = enabled;
            this._clearTraceToolStripMenuItem.Enabled = enabled;
            this._modelTraceToolStripMenuItem.Enabled = enabled;
        }
    }

    #endregion

    #region " trace tool strip "

    #region " trace list view "

    /// <summary> Displays a part described by grid. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="grid">   The grid. </param>
    /// <param name="values"> The values. </param>
    private static void DisplayTrace( DataGridView grid, IList<isr.Std.Cartesian.CartesianPoint<double>>? values )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( grid, nameof( grid ) );
#else
        if ( grid is null ) throw new ArgumentNullException( nameof( grid ) );
#endif
        grid.Enabled = false;
        grid.Columns.Clear();
        grid.DataSource = new List<PointF>();
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightSteelBlue;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        grid.RowHeadersVisible = false;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

        if ( values is not null ) grid.DataSource = values;

        grid.Refresh();
        if ( grid.Columns is not null && grid.Columns.Count > 0 )
        {
            DataGridViewColumn column;
            foreach ( DataGridViewColumn currentColumn in grid.Columns )
            {
                column = currentColumn;
                column.Visible = false;
            }

            int displayIndex = 0;
            column = grid.Columns["X"]!;
            column.Visible = true;
            column.HeaderText = " Time, ms ";
            column.DisplayIndex = displayIndex;
            displayIndex += 1;
            column = grid.Columns["Y"]!;
            column.Visible = true;
            column.HeaderText = " Voltage, mV ";
            column.DisplayIndex = displayIndex;
            grid.Enabled = true;
        }
    }

    #endregion

    /// <summary> Gets or sets the last time series. </summary>
    /// <value> The last time series. </value>
    private IList<isr.Std.Cartesian.CartesianPoint<double>>? LastTimeSeries { get; set; }

    /// <summary> Displays the thermal transient trace. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="chart"> The chart. </param>
    /// <returns> The trace values. </returns>
    protected virtual IList<isr.Std.Cartesian.CartesianPoint<double>> DisplayThermalTransientTrace( Chart chart )
    {
        return new List<isr.Std.Cartesian.CartesianPoint<double>>( [] );
    }

    /// <summary> Event handler. Called by _readTraceToolStripMenuItem for click events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ReadTraceToolStripMenuItem_Click( object? sender, EventArgs e )
    {
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            activity = $"reading and displaying the trace";
            this.InfoProvider?.SetError( this._ttmMeasureControlsToolStrip, "" );
            this.InfoProvider?.SetIconPadding( this._ttmMeasureControlsToolStrip, -15 );
            // MeterThermalTransient.EmulateThermalTransientTrace(Me._chart)
            this.LastTimeSeries = this.DisplayThermalTransientTrace( this._chart );
            // list the trace values
            DisplayTrace( this._traceDataGridView, this.LastTimeSeries );
            bool enabled = this.LastTimeSeries is object && this.LastTimeSeries.Any();
            this._saveTraceToolStripMenuItem.Enabled = enabled;
            this._clearTraceToolStripMenuItem.Enabled = enabled;
            this._modelTraceToolStripMenuItem.Enabled = enabled;
        }
        catch ( Exception ex )
        {
            this.InfoProvider?.SetError( this._ttmMeasureControlsToolStrip, "Failed reading and displaying the thermal transient trace." );
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Clears the trace list. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> Specifies the object where the call originated. </param>
    /// <param name="e">      Event information. </param>
    private void ClearTraceToolStripMenuItem_Click( object? sender, EventArgs e )
    {
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"User action @{System.Reflection.MethodBase.GetCurrentMethod()?.Name};. " );
        DisplayTrace( this._traceDataGridView, null );
    }

    /// <summary> Determines whether the specified folder path is writable. </summary>
    /// <remarks>
    /// Uses a temporary random file name to test if the file can be created. The file is deleted
    /// thereafter.
    /// </remarks>
    /// <param name="path"> The path. </param>
    /// <returns> <c>true</c> if the specified path is writable; otherwise, <c>false</c>. </returns>
    public static bool IsFolderWritable( string path )
    {
        string filePath = string.Empty;
        bool affirmative = false;
        try
        {
            filePath = System.IO.Path.Combine( path, System.IO.Path.GetRandomFileName() );
            using ( System.IO.FileStream s = System.IO.File.Open( filePath, System.IO.FileMode.OpenOrCreate ) )
            {
            }

            affirmative = true;
        }
        catch
        {
        }
        finally
        {
            // SS reported an exception from this test possibly indicating that Windows allowed writing the file 
            // by failed report deletion. Or else, Windows raised another exception type.
            try
            {
                if ( System.IO.File.Exists( filePath ) )
                    System.IO.File.Delete( filePath );
            }
            catch
            {
            }
        }

        return affirmative;
    }

    /// <summary> Sets the default file path in case the data folder does not exist. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="filePath"> The file path. </param>
    /// <returns>   A <see cref="string" />. </returns>
    private static string UpdateDefaultFilePath( string filePath )
    {
        // check validity of data folder.
        string? dataFolder = System.IO.Path.GetDirectoryName( filePath );
        if ( string.IsNullOrWhiteSpace( dataFolder ) || !System.IO.Directory.Exists( dataFolder ) )
        {
            dataFolder = Ttm.Controls.Settings.Instance.LotSettings.MeasurementsFolder;
            if ( IsFolderWritable( dataFolder ) )
            {
                dataFolder = System.IO.Directory.Exists( dataFolder )
                    ? System.IO.Path.GetDirectoryName( dataFolder )
                    : System.IO.Directory.CreateDirectory( dataFolder ).FullName;
            }
            else
            {
                // in run time use the documents folder.
                dataFolder = Environment.GetFolderPath( Environment.SpecialFolder.CommonDocuments );
                dataFolder = System.IO.Path.Combine( dataFolder, "TTM" );
                if ( !System.IO.Directory.Exists( dataFolder ) )
                {
                    _ = System.IO.Directory.CreateDirectory( dataFolder );
                }

                dataFolder = System.IO.Path.Combine( dataFolder, "Measurements" );
                if ( !System.IO.Directory.Exists( dataFolder ) )
                {
                    _ = System.IO.Directory.CreateDirectory( dataFolder );
                }
            }
        }
        if ( string.IsNullOrWhiteSpace( dataFolder ) ) throw new InvalidOperationException( $"{nameof( dataFolder )} fail to create." );
        return System.IO.Path.Combine( dataFolder, System.IO.Path.GetFileName( filePath ) );
    }

    /// <summary> Gets a new file for storing the data. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="filePath"> The file path. </param>
    /// <returns> A file name or empty if error. </returns>
    private static string BrowseForFile( string filePath )
    {
        // make sure the default data file name is valid.
        filePath = UpdateDefaultFilePath( filePath );
        using SaveFileDialog dialog = new();
        dialog.CheckFileExists = false;
        dialog.CheckPathExists = true;
        dialog.DefaultExt = ".xls";
        dialog.FileName = filePath;
        dialog.Filter = "Comma Separated Values (*.csv)|*.csv|All Files (*.*)|*.*";
        dialog.FilterIndex = 0;
        dialog.InitialDirectory = System.IO.Path.GetDirectoryName( filePath );
        dialog.RestoreDirectory = true;
        dialog.Title = "Select a Comma-Separated File";

        // Open the Open dialog
        if ( dialog.ShowDialog() == DialogResult.OK )
        {
            // if file selected,
            return dialog.FileName;
        }
        else
        {
            // if some error, just ignore
            return string.Empty;
        }
    }

    /// <summary> Saves the trace to file. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> Specifies the object where the call originated. </param>
    /// <param name="e">      Event information. </param>
    private void SaveTraceToolStripMenuItem_Click( object? sender, EventArgs e )
    {
        this.InfoProvider?.SetError( this._ttmMeasureControlsToolStrip, "" );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"User action @{System.Reflection.MethodBase.GetCurrentMethod()?.Name};. " );
        if ( this.LastTimeSeries is object && this.LastTimeSeries.Any() )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Browsing for file;. " );
            string filePath = BrowseForFile( Ttm.Controls.Settings.Instance.LotSettings.TraceFilePath );
            if ( string.IsNullOrWhiteSpace( filePath ) ) return;

            System.IO.FileInfo fi = new( filePath );
            if ( fi.DirectoryName is null || string.IsNullOrWhiteSpace( fi.DirectoryName ) ) return;

            Ttm.Controls.Settings.Instance.LotSettings.MeasurementsFolderName = fi.DirectoryName.Split( ['\\'] ).Last();
            int len = fi.DirectoryName.Length - Ttm.Controls.Settings.Instance.LotSettings.MeasurementsFolderName.Length - 1;
            Ttm.Controls.Settings.Instance.LotSettings.DataFolder = fi.DirectoryName[..len];
            Ttm.Controls.Settings.Instance.LotSettings.TraceFileName = fi.Name;


            string activity = string.Empty;
            try
            {
                activity = $"Saving trace;. to '{filePath}' ";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( activity );
                using ( System.IO.StreamWriter writer = new( filePath, false ) )
                {
                    string record = string.Empty;
                    foreach ( cc.isr.Std.Cartesian.CartesianPoint<double> point in this.LastTimeSeries )
                    {
                        if ( string.IsNullOrWhiteSpace( record ) )
                        {
                            record = "Trace time Series";
                            writer.WriteLine( record );
                            record = "Time,Voltage";
                            writer.WriteLine( record );
                            record = "ms,mV";
                            writer.WriteLine( record );
                        }

                        record = $"{point.X},{point.Y}";
                        writer.WriteLine( record );
                    }

                    writer.Flush();
                }

                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Done saving;. " );
            }
            catch ( Exception ex )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
                this.InfoProvider?.SetError( this._ttmMeasureControlsToolStrip, "Exception occurred saving trace" );
            }
            finally
            {
            }
        }
    }

    /// <summary> Creates a model of the thermal transient trace. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="chart"> The chart. </param>
    protected virtual void ModelThermalTransientTrace( Chart chart )
    {
    }

    /// <summary> Event handler. Called by _modelTraceToolStripMenuItem for click events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ModelTraceToolStripMenuItem_Click( object? sender, EventArgs e )
    {
        string activity = string.Empty;
        try
        {
            activity = $"modeling the trace";
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.SetError( this._ttmMeasureControlsToolStrip, "" );
            this.InfoProvider?.SetIconPadding( this._ttmMeasureControlsToolStrip, -15 );
            // MeterThermalTransient.EmulateThermalTransientTrace(Me._chart)
            this.ModelThermalTransientTrace( this._chart );
        }
        catch ( Exception ex )
        {
            this.InfoProvider?.SetError( this._ttmMeasureControlsToolStrip, "Failed modeling the thermal transient trace." );
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    #endregion

    #region " ttm measure tool strip "

    /// <summary> Event handler. Called by  for  events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    protected virtual void ClearPartMeasurements()
    {
    }

    /// <summary> Event handler. Called by _clearToolStripMenuItem for click events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ClearToolStripMenuItem_Click( object? sender, EventArgs e )
    {
        this.InfoProvider?.SetError( this._ttmMeasureControlsToolStrip, "" );
        this.ClearPartMeasurements();
    }

    /// <summary> Event handler. Called by  for  events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The MeasurementOutcomes. </returns>
    protected virtual MeasurementOutcomes MeasureInitialResistance()
    {
        return MeasurementOutcomes.None;
    }

    /// <summary> Measures initial resistance. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void InitialResistanceToolStripMenuItem_Click( object? sender, EventArgs e )
    {
        string activity = string.Empty;
        try
        {
            activity = $"Measuring Initial MeasuredValue";
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.SetError( this._ttmMeasureControlsToolStrip, "" );
            this.InfoProvider?.SetIconPadding( this._ttmMeasureControlsToolStrip, -15 );
            _ = this.MeasureInitialResistance();
            MeasurementOutcomes outcome = this.MeasureInitialResistance();
            if ( outcome is MeasurementOutcomes.None or MeasurementOutcomes.PartPassed )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Initial MeasuredValue measured;. " );
            }
            else if ( (outcome & MeasurementOutcomes.FailedContactCheck) != 0 )
            {
                this.InfoProvider?.SetError( this._ttmMeasureControlsToolStrip, "Failed contact check." );
                _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"Contact check failed;. " );
            }
            else
            {
                this.InfoProvider?.SetError( this._ttmMeasureControlsToolStrip, "Failed initial resistance." );
                _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"Initial MeasuredValue failed with outcome = {( int ) outcome}:{outcome};. " );
            }
        }
        catch ( Exception ex )
        {
            this.InfoProvider?.SetError( this._ttmMeasureControlsToolStrip, "Failed Measuring Initial MeasuredValue" );
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Event handler. Called by  for  events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The MeasurementOutcomes. </returns>
    protected virtual MeasurementOutcomes MeasureThermalTransient()
    {
        return MeasurementOutcomes.None;
    }

    /// <summary> Measures the thermal transient voltage. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ThermalTransientToolStripMenuItem_Click( object? sender, EventArgs e )
    {
        string activity = string.Empty;
        try
        {
            activity = $"Measuring thermal MeasuredValue";
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.SetError( this._ttmMeasureControlsToolStrip, "" );
            this.InfoProvider?.SetIconPadding( this._ttmMeasureControlsToolStrip, -15 );
            MeasurementOutcomes outcome = this.MeasureThermalTransient();
            if ( outcome is MeasurementOutcomes.None or MeasurementOutcomes.PartPassed )
            {
                this._isTraceAvailable = true;
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Thermal Transient measured;. " );
            }
            else
            {
                this._isTraceAvailable = false;
                this.InfoProvider?.SetError( this._ttmMeasureControlsToolStrip, "Thermal transient failed." );
                _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"Thermal transient failed with outcome = {( int ) outcome}:{outcome};. " );
            }

            this.OnStateChanged();
        }
        catch ( Exception ex )
        {
            this.InfoProvider?.SetError( this._ttmMeasureControlsToolStrip, "Failed Measuring Thermal Transient" );
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Event handler. Called by  for  events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The MeasurementOutcomes. </returns>
    protected virtual MeasurementOutcomes MeasureFinalResistance()
    {
        return MeasurementOutcomes.None;
    }

    /// <summary> Measures the final resistance. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void FinalResistanceToolStripMenuItem_Click( object? sender, EventArgs e )
    {
        string activity = string.Empty;
        try
        {
            activity = $"Measuring final MeasuredValue";
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.SetError( this._ttmMeasureControlsToolStrip, "" );
            this.InfoProvider?.SetIconPadding( this._ttmMeasureControlsToolStrip, -15 );
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Measuring Final MeasuredValue...;. " );
            MeasurementOutcomes outcome = this.MeasureFinalResistance();
            if ( outcome is MeasurementOutcomes.None or MeasurementOutcomes.PartPassed )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Final MeasuredValue measured;. " );
            }
            else
            {
                this.InfoProvider?.SetError( this._ttmMeasureControlsToolStrip, "Failed final resistance." );
                _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"Final MeasuredValue failed with outcome = {( int ) outcome}:{outcome};. " );
            }
        }
        catch ( Exception ex )
        {
            this.InfoProvider?.SetError( this._ttmMeasureControlsToolStrip, "Failed Measuring Final MeasuredValue" );
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    #endregion

    #region " sequenced measurements "

#if NET9_0_OR_GREATER
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0032:Use auto property", Justification = "preview; not preferred time." )]
#endif
    private MeasureSequencer? _measureSequencerInternal;

    private MeasureSequencer? MeasureSequencerInternal
    {
        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        get => this._measureSequencerInternal;

        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        set
        {
            if ( this._measureSequencerInternal is not null )
                this._measureSequencerInternal.PropertyChanged -= this.MeasureSequencer_PropertyChanged;

            this._measureSequencerInternal = value;
            if ( this._measureSequencerInternal is not null )
                this._measureSequencerInternal.PropertyChanged += this.MeasureSequencer_PropertyChanged;
        }
    }

    /// <summary> Gets or sets the sequencer. </summary>
    /// <value> The sequencer. </value>
    [Browsable( false )]
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public MeasureSequencer? MeasureSequencer
    {
        get => this.MeasureSequencerInternal;
        set => this.MeasureSequencerInternal = value;
    }

    /// <summary> Handles the measure sequencer property changed event. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender">       The source of the event. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void OnPropertyChanged( MeasureSequencer sender, string? propertyName )
    {
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName )
        {
            case nameof( Ttm.MeasureSequencer.MeasurementSequenceState ):
                {
                    this.OnMeasurementSequenceStateChanged( sender.MeasurementSequenceState );
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Event handler. Called by _measureSequencer for property changed events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void MeasureSequencer_PropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( sender is null || e is null ) return;
        string activity = string.Empty;
        try
        {
            activity = $"handler measure sequencer {e?.PropertyName} property changed event";
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.MeasureSequencer_PropertyChanged ), [sender, e] );
            else if ( sender is MeasureSequencer s )
                this.OnPropertyChanged( s, e?.PropertyName! );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary> Updates the progress bar. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="state"> The state. </param>
    private void UpdateProgressBar( MeasurementSequenceState state )
    {
        if ( this.MeasureSequencer is null ) throw new InvalidOperationException( $"{nameof( this.MeasureSequencer )} is null." );
        // unhide the progress bar.
        this._ttmToolStripProgressBar.Visible = true;
        if ( state is MeasurementSequenceState.Failed or MeasurementSequenceState.Starting or MeasurementSequenceState.Idle )
        {
            this._ttmToolStripProgressBar.Value = this._ttmToolStripProgressBar.Minimum;
            this._ttmToolStripProgressBar.ToolTipText = "Failed";
        }
        else if ( state == MeasurementSequenceState.Completed )
        {
            this._ttmToolStripProgressBar.Value = this._ttmToolStripProgressBar.Maximum;
            this._ttmToolStripProgressBar.ToolTipText = "Completed";
        }
        else
        {
            this._ttmToolStripProgressBar.Value = this.MeasureSequencer.PercentProgress();
        }
    }

    /// <summary> Handles the change in measurement state. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="state"> The state. </param>
    private void OnMeasurementSequenceStateChanged( MeasurementSequenceState state )
    {
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Processing the {( int ) state}:{state.Description()} state;. " );
        this.UpdateProgressBar( state );
        switch ( state )
        {
            case MeasurementSequenceState.Aborted:
                {
                    break;
                }

            case MeasurementSequenceState.Completed:
                {
                    break;
                }

            case MeasurementSequenceState.Failed:
                {
                    break;
                }

            case MeasurementSequenceState.MeasureInitialResistance:
                {
                    break;
                }

            case MeasurementSequenceState.MeasureThermalTransient:
                {
                    break;
                }

            case MeasurementSequenceState.Idle:
                {
                    this.OnStateChanged();
                    break;
                }

            case var @case when @case == MeasurementSequenceState.None:
                {
                    break;
                }

            case MeasurementSequenceState.PostTransientPause:
                {
                    this._isTraceAvailable = true;
                    break;
                }

            case MeasurementSequenceState.MeasureFinalResistance:
                {
                    break;
                }

            case MeasurementSequenceState.Starting:
                {
                    this._isTraceAvailable = false;
                    this.OnStateChanged();
                    break;
                }

            default:
                {
                    Debug.Assert( !Debugger.IsAttached, "Unhandled state: " + state.ToString() );
                    break;
                }
        }
    }

    /// <summary> Aborts the measurement sequence. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> Specifies the object where the call originated. </param>
    /// <param name="e">      Event information. </param>
    private void AbortSequenceToolStripMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.MeasureSequencer is null ) throw new InvalidOperationException( $"{nameof( this.MeasureSequencer )} is null." );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"User action @{System.Reflection.MethodBase.GetCurrentMethod()?.Name};. " );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Abort requested;. " );
        this.MeasureSequencer.Enqueue( MeasurementSequenceSignal.Abort );
    }

    /// <summary> Starts measurement sequence. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> Specifies the object where the call originated. </param>
    /// <param name="e">      Event information. </param>
    private void MeasureAllToolStripMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.MeasureSequencer is null ) throw new InvalidOperationException( $"{nameof( this.MeasureSequencer )} is null." );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"User action @{System.Reflection.MethodBase.GetCurrentMethod()?.Name};. " );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Start requested;. " );
        this.InfoProvider?.SetError( this._ttmMeasureControlsToolStrip, "" );
        this.InfoProvider?.SetIconPadding( this._ttmMeasureControlsToolStrip, -15 );
        this.MeasureSequencer.StartMeasurementSequence();
    }

    #endregion

    #region " triggered measurements "

    /// <summary> Abort triggered measurement. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    protected virtual void AbortTriggerSequenceIf()
    {
    }

    /// <summary> Event handler. Called by _abortToolStripMenuItem for click events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void AbortToolStripMenuItem_Click( object? sender, EventArgs e )
    {
        this.InfoProvider?.SetError( this._ttmMeasureControlsToolStrip, "" );
        this.AbortTriggerSequenceIf();
    }

#if NET9_0_OR_GREATER
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0032:Use auto property", Justification = "preview; not preferred time." )]
#endif
    private TriggerSequencer? _triggerSequencerInternal;

    /// <summary>   Gets or sets the trigger sequencer internal. </summary>
    /// <value> The trigger sequencer internal. </value>
    private TriggerSequencer? TriggerSequencerInternal
    {
        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        get => this._triggerSequencerInternal;

        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        set
        {
            if ( this._triggerSequencerInternal is not null )
                this._triggerSequencerInternal.PropertyChanged -= this.TriggerSequencer_PropertyChanged;

            this._triggerSequencerInternal = value;
            if ( this._triggerSequencerInternal is not null )
                this._triggerSequencerInternal.PropertyChanged += this.TriggerSequencer_PropertyChanged;
        }
    }

    /// <summary> Gets or sets the trigger sequencer. </summary>
    /// <value> The sequencer. </value>
    [Browsable( false )]
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public TriggerSequencer? TriggerSequencer
    {
        get => this.TriggerSequencerInternal;
        set => this.TriggerSequencerInternal = value;
    }

    /// <summary> Handles the trigger sequencer property changed event. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender">       The source of the event. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void OnPropertyChanged( TriggerSequencer sender, string? propertyName )
    {
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName )
        {
            case nameof( Ttm.TriggerSequencer.TriggerSequenceState ):
                {
                    this.OnTriggerSequenceStateChanged( sender.TriggerSequenceState );
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Event handler. Called by _triggerSequencer for property changed events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void TriggerSequencer_PropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( sender is null || e is null ) return;
        string activity = string.Empty;
        try
        {
            activity = $"handler trigger sequencer {e?.PropertyName} property changed event";
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.TriggerSequencer_PropertyChanged ), [sender, e] );
            else if ( sender is TriggerSequencer s )
                this.OnPropertyChanged( s, e?.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary> The last status bar message. </summary>
    protected string? LastStatusBarMessage { get; set; }

    /// <summary> Handles the change in measurement state. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="state"> The state. </param>
    private void OnTriggerSequenceStateChanged( TriggerSequenceState state )
    {
        if ( this.TriggerSequencer is null ) throw new InvalidOperationException( $"{nameof( MeasurementViewBase )}.{nameof( MeasurementViewBase.TriggerSequencer )} is null." );
        this._triggerActionToolStripLabel.Text = this.TriggerSequencer.ProgressMessage( this.LastStatusBarMessage ?? string.Empty );
        switch ( state )
        {
            case TriggerSequenceState.Aborted:
                {
                    this.OnStateChanged();
                    break;
                }

            case TriggerSequenceState.Stopped:
                {
                    break;
                }

            case TriggerSequenceState.Failed:
                {
                    break;
                }

            case TriggerSequenceState.WaitingForTrigger:
                {
                    break;
                }

            case TriggerSequenceState.MeasurementCompleted:
                {
                    this._isTraceAvailable = true;
                    break;
                }

            case TriggerSequenceState.ReadingValues:
                {
                    break;
                }

            case TriggerSequenceState.Idle:
                {
                    this.OnStateChanged();
                    this._waitForTriggerToolStripMenuItem.Image = MeasurementsHeader.GetResourceImage( nameof( Ttm.Controls.Properties.Resources.view_refresh_7 ), new Bitmap( 1, 1 ) );
                    break;
                }

            case var @case when @case == TriggerSequenceState.None:
                {
                    break;
                }

            case TriggerSequenceState.Starting:
                {
                    this._isTraceAvailable = false;
                    this.OnStateChanged();
                    this._assertTriggerToolStripMenuItem.Enabled = true;
                    this._waitForTriggerToolStripMenuItem.Image =
                        MeasurementsHeader.GetResourceImage( nameof( Ttm.Controls.Properties.Resources.media_playback_stop_2 ), new Bitmap( 1, 1 ) );
                    break;
                }

            default:
                {
                    Debug.Assert( !Debugger.IsAttached, "Unhandled state: " + state.ToString() );
                    break;
                }
        }
    }

    /// <summary> Event handler. Called by  for  events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    private void AssertTrigger()
    {
        if ( this.TriggerSequencer is null ) throw new InvalidOperationException( $"{nameof( MeasurementViewBase )}.{nameof( MeasurementViewBase.TriggerSequencer )} is null." );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Assert requested;. " );
        this.TriggerSequencer.AssertTrigger();
    }

    /// <summary> Asserts a trigger to emulate triggering for timing measurements. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void AssertTriggerToolStripMenuItem_Click( object? sender, EventArgs e )
    {
        this.AssertTrigger();
    }

    /// <summary>
    /// Event handler. Called by _waitForTriggerToolStripMenuItem for click events.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void WaitForTriggerToolStripMenuItem_CheckChanged( object? sender, EventArgs e )
    {
        if ( this.TriggerSequencerInternal is null ) throw new InvalidOperationException( $"{nameof( MeasurementViewBase )}.{nameof( MeasurementViewBase.TriggerSequencerInternal )} is null." );
        if ( this._waitForTriggerToolStripMenuItem.Enabled )
        {
            if ( this.TriggerSequencerInternal.TriggerSequenceState == TriggerSequenceState.Idle )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Starting triggered measurements;. " );
                this.TriggerSequencerInternal.StartMeasurementSequence();
            }
            else if ( this.TriggerSequencerInternal.TriggerSequenceState == TriggerSequenceState.WaitingForTrigger )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Stopping triggered measurements;. " );
                this.TriggerSequencerInternal.Enqueue( TriggerSequenceSignal.Stop );
            }
        }
    }

    #endregion

    #region " Tsp Device (K2600) "

#if NET9_0_OR_GREATER
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0032:Use auto property", Justification = "preview; not preferred time." )]
#endif
    private K2600Device? _tspDevice;

    /// <summary> Gets or sets the Tsp Device. </summary>
    /// <value> The Tsp Device. </value>
    [Browsable( false )]
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public K2600Device? TspDevice
    {
        get => this._tspDevice;
        set
        {
            this._tspDevice = value;
            if ( value is null )
                this.DisableTraceLevelControls();
            else
                this.EnableTraceLevelControls();
        }
    }

    /// <summary> Disables the trace level controls. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    private void DisableTraceLevelControls()
    {
        this._logTraceLevelComboBox.ComboBox.SelectedValueChanged -= this.LogTraceLevelComboBox_SelectedIndexChanged;
        this._displayTraceLevelComboBox.ComboBox.SelectedValueChanged -= this.DisplayTraceLevelComboBox_SelectedIndexChanged;
    }

    /// <summary> Enables the trace level controls. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    private void EnableTraceLevelControls()
    {
        cc.isr.WinControls.ComboBoxExtensions.ComboBoxExtensionsMethods.ListTraceEventLevels( this._logTraceLevelComboBox.ComboBox, cc.isr.Logging.TraceLog.TraceEventSelector.TraceEventValueNamePairs );
        this._logTraceLevelComboBox.ComboBox.SelectedValueChanged += this.LogTraceLevelComboBox_SelectedIndexChanged;
        cc.isr.WinControls.ComboBoxExtensions.ComboBoxExtensionsMethods.ListTraceEventLevels( this._displayTraceLevelComboBox.ComboBox, cc.isr.Logging.TraceLog.TraceEventSelector.TraceEventValueNamePairs );
        this._displayTraceLevelComboBox.ComboBox.SelectedValueChanged += this.DisplayTraceLevelComboBox_SelectedIndexChanged;
        cc.isr.WinControls.ComboBoxExtensions.ComboBoxExtensionsMethods.SelectItem( this._logTraceLevelComboBox, cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmSessionSettings.TraceLogLevel );
        cc.isr.WinControls.ComboBoxExtensions.ComboBoxExtensionsMethods.SelectItem( this._displayTraceLevelComboBox, cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmSessionSettings.TraceShowLevel );
    }

    /// <summary>
    /// Event handler. Called by _logTraceLevelComboBox for selected value changed events.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> <see cref="object"/> instance of this
    ///                                             <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void LogTraceLevelComboBox_SelectedIndexChanged( object? sender, EventArgs e )
    {
        string activity = string.Empty;
        try
        {
            activity = $"selecting log trace level on this instrument only";
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
        }
        catch ( Exception ex )
        {
            _ = this.InfoProvider?.Annunciate( sender, cc.isr.WinControls.InfoProviderLevel.Error, ex.Message );
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// Event handler. Called by _displayTraceLevelComboBox for selected value changed events.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> <see cref="object"/> instance of this
    ///                                             <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void DisplayTraceLevelComboBox_SelectedIndexChanged( object? sender, EventArgs e )
    {
        string activity = string.Empty;
        try
        {
            activity = $"selecting display trace level on this instrument only";
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
        }
        catch ( Exception ex )
        {
            _ = this.InfoProvider?.Annunciate( sender, cc.isr.WinControls.InfoProviderLevel.Error, ex.Message );
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    #endregion
}
