using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace cc.isr.VI.Tsp.K2600.Ttm.Controls;

/// <summary> Panel for editing the parts. </summary>
/// <remarks>
/// (c) 2014 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2014-04-12 </para>
/// </remarks>
public partial class PartsView : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public PartsView() : base()
    {
        // This call is required by the Windows Form Designer.
        this.InitializeComponent();
        this.OnPartOutcomeChanged();
        this.PartsInternal = [];
        this.ConfigurePartsDisplay( this._partsDataGridView );
        this.OnPartsChanged( this.PartsInternal, EventArgs.Empty );
#if designMode
        Debug.Assert( False, "Illegal call; reset design mode" )
#endif
    }

    /// <summary> Release resources. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    private void ReleaseResources()
    {
        this.PartsInternal?.Clear();
        this.PartsInternal = null;
    }

    #endregion

    #region " device under test "

    private DeviceUnderTest? PartInternal
    {
        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        get;

        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        set
        {
            if ( field is not null )
                field.PropertyChanged -= this.Part_PropertyChanged;

            field = value;
            if ( field is not null )
                field.PropertyChanged += this.Part_PropertyChanged;
        }
    }

    /// <summary> Gets or sets the part. </summary>
    /// <value> The part. </value>
    [Browsable( false )]
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public DeviceUnderTest? Part
    {
        get => this.PartInternal;
        set => this.PartInternal = value;
    }

    /// <summary> Update outcome display and enable adding part. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    private void OnPartOutcomeChanged()
    {
        if ( this.Part is null )
        {
            this.OutcomeSetter( MeasurementOutcomes.None );
            this._clearMeasurementsToolStripMenuItem.Enabled = false;
            this._addPartToolStripButton.Enabled = false;
        }
        else
        {
            this.OutcomeSetter( this.Part.Outcome );
            this._addPartToolStripButton.Enabled = this.Part.AllMeasurementsAvailable && (this.Part.Outcome & MeasurementOutcomes.FailedContactCheck) == 0 && (this.Part.Outcome & MeasurementOutcomes.MeasurementFailed) == 0 && (this.Part.Outcome & MeasurementOutcomes.MeasurementNotMade) == 0;
            this._clearMeasurementsToolStripMenuItem.Enabled = this.Part.AnyMeasurementsAvailable;
        }
    }

    /// <summary> Executes the part property changed action. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender">       Specifies the object where the call originated. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void OnPartPropertyChanged( DeviceUnderTest sender, string? propertyName )
    {
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName )
        {
            case nameof( DeviceUnderTest.AnyMeasurementsAvailable ):
                {
                    this.OnPartOutcomeChanged();
                    break;
                }

            case nameof( DeviceUnderTest.AllMeasurementsAvailable ):
                {
                    this.OnPartOutcomeChanged();
                    break;
                }

            case nameof( DeviceUnderTest.LotId ):
                {
                    this._lotToolStripTextBox.Text = this.Part?.LotId;
                    break;
                }

            case nameof( DeviceUnderTest.Outcome ):
                {
                    this.OnPartOutcomeChanged();
                    break;
                }

            case nameof( DeviceUnderTest.PartNumber ):
                {
                    this._partNumberToolStripTextBox.Text = $"{this.Part?.PartNumber}";
                    break;
                }

            case nameof( DeviceUnderTest.OperatorId ):
                {
                    this._operatorToolStripTextBox.Text = $"{this.Part?.OperatorId}";
                    break;
                }

            case nameof( DeviceUnderTest.SerialNumber ):
                {
                    this._serialNumberToolStripTextBox.Text = $"{this.Part?.SerialNumber}";
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Event handler. Called by _part for property changed events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> Specifies the object where the call originated. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void Part_PropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( sender is null || e is null ) return;
        string activity = string.Empty;
        try
        {
            activity = $"handling part {e?.PropertyName} property changed event";
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.Part_PropertyChanged ), [sender, e] );
            else if ( sender is DeviceUnderTest s )
                this.OnPartPropertyChanged( s, e?.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary> Gets or sets a message describing the measurement. </summary>
    /// <value> A message describing the measurement. </value>
    [Browsable( false )]
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public string? MeasurementMessage
    {
        get;
        set
        {
            if ( string.IsNullOrWhiteSpace( value ) )
                value = string.Empty;
            else if ( (value ?? "") != (this.MeasurementMessage ?? "") )
                this._outcomeToolStripLabel.Text = value;

            field = value;
        }
    }

    /// <summary> Gets or sets the test <see cref="MeasurementOutcomes">outcome</see>. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The value. </param>
    public void OutcomeSetter( MeasurementOutcomes value )
    {
        if ( value == MeasurementOutcomes.None )
        {
            this.MeasurementMessage = string.Empty;
            this._passFailToolStripButton.Visible = false;
        }
        else if ( (value & MeasurementOutcomes.MeasurementFailed) == 0 )
        {
            this.MeasurementMessage = "OKAY";
        }
        else if ( (value & MeasurementOutcomes.FailedContactCheck) != 0 )
        {
            this.MeasurementMessage = "CONTACTS";
        }
        else if ( (value & MeasurementOutcomes.HitCompliance) != 0 )
        {
            this.MeasurementMessage = "COMPLIANCE";
        }
        else if ( (value & MeasurementOutcomes.UnexpectedReadingFormat) != 0 )
        {
            this.MeasurementMessage = "READING FORMAT";
        }
        else if ( (value & MeasurementOutcomes.UnexpectedOutcomeFormat) != 0 )
        {
            this.MeasurementMessage = "OUTCOME FORMAT";
        }
        else if ( (value & MeasurementOutcomes.UnspecifiedFirmwareOutcome) != 0 )
        {
            this.MeasurementMessage = "DEVICE";
        }
        else if ( (value & MeasurementOutcomes.UnspecifiedProgramFailure) != 0 )
        {
            this.MeasurementMessage = "PROGRAM";
        }
        else if ( (value & MeasurementOutcomes.MeasurementNotMade) != 0 )
        {
            this.MeasurementMessage = "PARTIAL";
        }
        else
        {
            this.MeasurementMessage = "FAILED";
            if ( (value & MeasurementOutcomes.UnknownOutcome) != 0 )
            {
                Debug.Assert( !Debugger.IsAttached, "Unknown outcome" );
            }
        }

        if ( (value & MeasurementOutcomes.PartPassed) != 0 )
        {
            this._passFailToolStripButton.Visible = true;
            this._passFailToolStripButton.Image = ( System.Drawing.Bitmap ) (Ttm.Controls.Properties.Resources.ResourceManager.GetObject( nameof( Ttm.Controls.Properties.Resources.Good ),
                System.Globalization.CultureInfo.CurrentUICulture ) ?? new Bitmap( 1, 1 ));
        }
        else if ( (value & MeasurementOutcomes.PartFailed) != 0 )
        {
            this._passFailToolStripButton.Visible = true;
            this._passFailToolStripButton.Image = ( System.Drawing.Bitmap ) (Ttm.Controls.Properties.Resources.ResourceManager.GetObject( nameof( Ttm.Controls.Properties.Resources.Bad ),
                System.Globalization.CultureInfo.CurrentUICulture ) ?? new Bitmap( 1, 1 ));
        }
    }

    #region " settings and binding "

    /// <summary> Copy panel values to the settings store. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public void CopySettings()
    {
        Ttm.Controls.Settings.Instance.LotSettings.AutomaticallyAddPart = this._autoAddToolStripMenuItem.Checked;
        string value = this._partNumberToolStripTextBox.Text.Trim();
        if ( !string.IsNullOrWhiteSpace( value ) )
        {
            Ttm.Controls.Settings.Instance.LotSettings.PartNumber = value;
        }

        value = this._lotToolStripTextBox.Text.Trim();
        if ( !string.IsNullOrWhiteSpace( value ) )
        {
            Ttm.Controls.Settings.Instance.LotSettings.LotNumber = value;
        }

        value = this._operatorToolStripTextBox.Text.Trim();
        if ( !string.IsNullOrWhiteSpace( value ) )
        {
            Ttm.Controls.Settings.Instance.LotSettings.OperatorId = value;
        }
    }

    /// <summary> Applies settings to the controls. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public void ApplySettings()
    {
        if ( this.Part is null ) throw new InvalidOperationException( $"{nameof( PartsView )}.{nameof( PartsView.Part )} is null." );
        this._autoAddToolStripMenuItem.Checked = Ttm.Controls.Settings.Instance.LotSettings.AutomaticallyAddPart;
        this.Part.PartNumber = Ttm.Controls.Settings.Instance.LotSettings.PartNumber;
        this._partNumberToolStripTextBox.Text = Ttm.Controls.Settings.Instance.LotSettings.PartNumber;
        this.Part.LotId = Ttm.Controls.Settings.Instance.LotSettings.LotNumber;
        this._lotToolStripTextBox.Text = Ttm.Controls.Settings.Instance.LotSettings.LotNumber;
        this.Part.OperatorId = Ttm.Controls.Settings.Instance.LotSettings.OperatorId;
        this._operatorToolStripTextBox.Text = Ttm.Controls.Settings.Instance.LotSettings.OperatorId;
        this._serialNumberToolStripTextBox.Text = $"{this.Part.SerialNumber}";
    }

    #endregion

    #endregion

    #region " parts list management "


    private DeviceUnderTestCollection? PartsInternal
    {
        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        get;

        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        set
        {
            if ( field is not null )
                field.CollectionChanged -= this.OnPartsChanged;

            field = value;
            if ( field is not null )
                field.CollectionChanged += this.OnPartsChanged;
        }
    }

    /// <summary> Gets the parts. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> A DeviceUnderTestCollection. </returns>
    public DeviceUnderTestCollection? Parts()
    {
        return this.PartsInternal;
    }

    /// <summary>   Raises the parts changed event. </summary>
    /// <remarks>   David, 2021-09-04. </remarks>
    /// <param name="sender">   Specifies the object where the call originated. </param>
    /// <param name="e">        Event information to send to registered event handlers. </param>
    protected void OnPartsChanged( object? sender, EventArgs e )
    {
        if ( this._partsBindingSource is null ) throw new InvalidOperationException( $"{nameof( PartsView )}._partsBindingSource is null." );
        // Me.ConfigurePartsDisplay(Me._partsDataGridView)
        this._partsBindingSource.ResetBindings( false );
        this._addPartToolStripButton.Enabled = this._addPartToolStripButton.Enabled && this.PartsInternal is not null && this.PartsInternal.Any();
        this._clearPartListToolStripButton.Enabled = this.PartsInternal is not null && this.PartsInternal.Any();
        this._savePartsToolStripButton.Enabled = this.PartsInternal is not null && this.PartsInternal.Any();
    }

    /// <summary> Clears the list of measurements. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public void ClearParts()
    {
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Clearing parts;. " );
        this.PartsInternal ??= [];
        this.PartsInternal.Clear();
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Part list cleared;. " );
    }

    /// <summary> Gets the automatic add enabled. </summary>
    /// <value> The automatic add enabled. </value>
    public bool AutoAddEnabled => this._addPartToolStripButton.Enabled && this._autoAddToolStripMenuItem.Checked;

    /// <summary> Adds the current part to the list of measurements. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public void AddPart()
    {
        if ( this.Part is null ) throw new InvalidOperationException( $"{nameof( PartsView )}.{nameof( PartsView.Part )} is null." );
        if ( this.PartsInternal is null ) throw new InvalidOperationException( $"{nameof( PartsView )}.{nameof( PartsView.PartsInternal )} is null." );

        if ( this.Part.AllMeasurementsMade() )
        {
            if ( string.IsNullOrEmpty( this._partNumberToolStripTextBox.Text.Trim() ) )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"Enter part number;. " );
            }
            else if ( string.IsNullOrEmpty( this._lotToolStripTextBox.Text.Trim() ) )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"Enter lot number;. " );
            }
            else if ( string.IsNullOrEmpty( this._operatorToolStripTextBox.Text.Trim() ) )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"Enter operator id;. " );
            }
            else
            {
                this.CopySettings();
                this.Part.PartNumber = this._partNumberToolStripTextBox.Text.Trim();
                this.Part.LotId = this._lotToolStripTextBox.Text.Trim();
                this.Part.OperatorId = this._operatorToolStripTextBox.Text.Trim();
                bool localTryParse()
                {
                    _ = this.Part.SerialNumber;
                    bool ret = int.TryParse( this._serialNumberToolStripTextBox.Text.Trim(), out int result ); this.Part.SerialNumber = result; return ret;
                }

                if ( !localTryParse() )
                {
                    this.Part.SerialNumber += 1;
                }

                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting part sample number to '{this.PartsInternal.Count + 1}';. " );
                this.Part.SampleNumber = this.PartsInternal.Count + 1;
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Adding part '{this.Part.UniqueKey}';. " );
                this.PartsInternal.Add( new DeviceUnderTest( this.Part ) );
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Part '{this.Part.UniqueKey}' added;. " );
            }
        }
        else
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogInformation( $"Part has incomplete data;. MeasurementOutcome: {( int ) this.Part.Outcome}:{this.Part.Outcome}" );
        }
    }

    /// <summary> Sets the default file path in case the data folder does not exist. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="filePath"> The file path. </param>
    /// <returns>   A <see cref="string" />. </returns>
    private static string UpdateDefaultFilePath( string filePath )
    {
        // check validity of data folder.
        string? dataFolder = System.IO.Path.GetDirectoryName( filePath );
        if ( !System.IO.Directory.Exists( dataFolder ) )
        {
            if ( Debugger.IsAttached )
            {
                // in design mode use the application folder.
                dataFolder = Ttm.Controls.Settings.Instance.LotSettings.PartsFilePath;
                dataFolder = System.IO.Directory.Exists( dataFolder )
                    ? System.IO.Path.GetDirectoryName( dataFolder )
                    : System.IO.Directory.CreateDirectory( dataFolder ).FullName;

                System.IO.FileInfo fi = new( filePath );

                if ( fi.DirectoryName is null || string.IsNullOrWhiteSpace( fi.DirectoryName ) ) throw new InvalidOperationException( $"Unable to get a directory folder for {filePath}." );

                Ttm.Controls.Settings.Instance.LotSettings.MeasurementsFolderName = fi.DirectoryName.Split( ['\\'] ).Last();
                int len = fi.DirectoryName.Length - Ttm.Controls.Settings.Instance.LotSettings.MeasurementsFolderName.Length - 1;
                Ttm.Controls.Settings.Instance.LotSettings.DataFolder = fi.DirectoryName[..len];
                // Ttm.Properties.ConfigurationViewBase.InstrumentSettings.DataFolder = fi.DirectoryName.Substring( len );
                Ttm.Controls.Settings.Instance.LotSettings.PartsFileName = fi.Name;

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

        if ( string.IsNullOrWhiteSpace( dataFolder ) ) throw new InvalidOperationException( $"Unable to get a directory folder for {filePath}." );
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

    /// <summary> Gets the save parts data enabled condition. </summary>
    /// <value> The save parts data enabled condition. </value>
    public bool SaveEnabled => this._savePartsToolStripButton.Enabled;

    /// <summary> Saves data for all parts. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public void SaveParts()
    {
        if ( this.PartsInternal is null || !this.PartsInternal.Any() ) return;

        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Browsing for file;. " );
        string filePath = PartsView.BrowseForFile( Ttm.Controls.Settings.Instance.LotSettings.PartsFilePath );

        if ( string.IsNullOrWhiteSpace( filePath ) ) return;

        System.IO.FileInfo fi = new( filePath );

        if ( fi.DirectoryName is null || string.IsNullOrWhiteSpace( fi.DirectoryName ) ) throw new InvalidOperationException( $"Unable to get a directory folder for {filePath}." );

        Ttm.Controls.Settings.Instance.LotSettings.MeasurementsFolderName = fi.DirectoryName.Split( ['\\'] ).Last();
        Ttm.Controls.Settings.Instance.LotSettings.DataFolder = fi.DirectoryName[..(fi.DirectoryName.Length - Ttm.Controls.Settings.Instance.LotSettings.MeasurementsFolderName.Length - 1)];
        Ttm.Controls.Settings.Instance.LotSettings.PartsFileName = fi.Name;
        string activity = string.Empty;
        try
        {
            activity = $"Saving...;. data to '{filePath}'";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( activity );
            using ( System.IO.StreamWriter writer = new( filePath, false ) )
            {
                foreach ( DeviceUnderTest part in this.PartsInternal )
                {
                    if ( part.SampleNumber == 1 )
                    {
                        writer.WriteLine( "\"TTM measurements\"" );
                        writer.WriteLine( "\"Part number\",\"{0}\"", part.PartNumber );
                        writer.WriteLine( "\"Lot number\",\"{0}\"", part.LotId );
                        writer.WriteLine( "\"Operator Id\",\"{0}\"", part.OperatorId );
                        writer.WriteLine( DeviceUnderTest.DataHeader );
                    }

                    writer.WriteLine( part.DataRecord );
                }

                writer.Flush();
            }

            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Done saving part number {this.Part?.PartNumber};. " );
            this._savePartsToolStripButton.Enabled = false;
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            this.InfoProvider?.SetError( this._partsListToolStrip, $"Exception {activity}" );
        }
        finally
        {
        }
    }

    /// <summary> Event handler. Called by _addPartToolStripButton for click events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> Specifies the object where the call originated. </param>
    /// <param name="e">      Event information. </param>
    private void AddPartToolStripButton_Click( object? sender, EventArgs e )
    {
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"User action @{System.Reflection.MethodBase.GetCurrentMethod()?.Name};. " );
        this.InfoProvider?.SetError( this._partsListToolStrip, "" );
        this.AddPart();
    }

    /// <summary> Event handler. Called by _cLearPartListToolStripButton for click events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> Specifies the object where the call originated. </param>
    /// <param name="e">      Event information. </param>
    private void ClearPartListToolStripButton_Click( object? sender, EventArgs e )
    {
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"User action @{System.Reflection.MethodBase.GetCurrentMethod()?.Name};. " );
        this.InfoProvider?.SetError( this._partsListToolStrip, "" );
        this.ClearParts();
    }

    /// <summary> Event handler. Called by _savePartsToolStripButton for click events.3. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> Specifies the object where the call originated. </param>
    /// <param name="e">      Event information. </param>
    private void SavePartsToolStripButton_Click( object? sender, EventArgs e )
    {
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"User action @{System.Reflection.MethodBase.GetCurrentMethod()?.Name};. " );
        this.InfoProvider?.SetError( this._partsListToolStrip, "" );
        this.SaveParts();
    }

    #endregion

    #region " parts display "

    /// <summary> Event handler. Called by _partsDataGridView for data error events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> Specifies the object where the call originated. </param>
    /// <param name="e">      Data grid view data error event information. </param>
    private void PartsDataGridView_DataError( object? sender, DataGridViewDataErrorEventArgs e )
    {
        if ( e is not null )
        {
            e.Cancel = false;
        }
    }

    /// <summary> The binder. </summary>
    private BindingSource? _partsBindingSource;

    /// <summary> Configures parts display. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="grid"> The grid. </param>
    public void ConfigurePartsDisplay( System.Windows.Forms.DataGridView grid )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( grid, nameof( grid ) );
#else
        if ( grid is null ) throw new ArgumentNullException( nameof( grid ) );
#endif

        this._partsBindingSource = new BindingSource() { DataSource = this.PartsInternal };
        grid.Enabled = false;
        grid.Columns.Clear();
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightSalmon;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        grid.AllowUserToAddRows = false;
        grid.RowHeadersVisible = false;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        grid.AutoGenerateColumns = false;
        grid.ReadOnly = true;
        grid.DataSource = this._partsBindingSource;
        grid.Refresh();
        int displayIndex = 0;
        DataGridViewTextBoxColumn column;
        column = new DataGridViewTextBoxColumn()
        {
            DataPropertyName = "SampleNumber",
            Name = "SampleNumber",
            Visible = true,
            HeaderText = " # ",
            DisplayIndex = displayIndex
        };
        displayIndex += 1;
        column.Width = 30;
        _ = grid.Columns.Add( column );
        column = new DataGridViewTextBoxColumn()
        {
            DataPropertyName = "SerialNumber",
            Name = "SerialNumber",
            Visible = true,
            HeaderText = " S/N ",
            DisplayIndex = displayIndex
        };
        displayIndex += 1;
        _ = grid.Columns.Add( column );
        column = new DataGridViewTextBoxColumn()
        {
            DataPropertyName = "Timestamp",
            Name = "Timestamp",
            Visible = true,
            HeaderText = " Time ",
            DisplayIndex = displayIndex
        };
        displayIndex += 1;
        _ = grid.Columns.Add( column );
        column = new DataGridViewTextBoxColumn()
        {
            DataPropertyName = "InitialResistanceCaption",
            Name = "InitialResistanceCaption",
            Visible = true,
            HeaderText = " Initial ",
            DisplayIndex = displayIndex
        };
        displayIndex += 1;
        _ = grid.Columns.Add( column );
        column = new DataGridViewTextBoxColumn()
        {
            DataPropertyName = "FinalResistanceCaption",
            Name = "FinalResistanceCaption",
            Visible = true,
            HeaderText = " Final ",
            DisplayIndex = displayIndex
        };
        displayIndex += 1;
        _ = grid.Columns.Add( column );
        column = new DataGridViewTextBoxColumn()
        {
            DataPropertyName = "ThermalTransientVoltageCaption",
            Name = "ThermalTransientVoltageCaption",
            Visible = true,
            HeaderText = " Transient ",
            DisplayIndex = displayIndex
        };
        _ = grid.Columns.Add( column );
        grid.Enabled = true;
    }

    #endregion
}
