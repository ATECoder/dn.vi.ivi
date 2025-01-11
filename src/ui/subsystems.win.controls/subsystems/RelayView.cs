using System;
using System.Windows.Forms;

namespace cc.isr.VI.SubsystemsWinControls;

/// <summary> A relay view. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-12-31 </para>
/// </remarks>
public partial class RelayView : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    public RelayView() : base()
    {
        this.InitializingComponents = true;
        // This call is required by the Windows Form Designer.
        this.InitializeComponent();
        this.InitializingComponents = false;
        this._channelListBuilder = new ChannelListBuilder();
        this._memoryLocationNumeric.NumericUpDown.Minimum = 1m;
        this._memoryLocationNumeric.NumericUpDown.Maximum = 100m;
        this._memoryLocationNumeric.NumericUpDown.DecimalPlaces = 0;
        this._memoryLocationNumeric.NumericUpDown.Value = 1m;
        this._slotNumberNumeric.NumericUpDown.Minimum = 1m;
        this._slotNumberNumeric.NumericUpDown.Maximum = 10m;
        this._slotNumberNumeric.NumericUpDown.DecimalPlaces = 0;
        this._slotNumberNumeric.NumericUpDown.Value = 1m;
        this._relayNumberNumeric.NumericUpDown.Minimum = 1m;
        this._relayNumberNumeric.NumericUpDown.Maximum = 100m;
        this._relayNumberNumeric.NumericUpDown.DecimalPlaces = 0;
        this._relayNumberNumeric.NumericUpDown.Value = 1m;
    }

    /// <summary> Creates a new <see cref="RelayView"/> </summary>
    /// <returns> A <see cref="RelayView"/>. </returns>
    public static RelayView Create()
    {
        RelayView? view = null;
        try
        {
            view = new RelayView();
            return view;
        }
        catch
        {
            view?.Dispose();
            throw;
        }
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="Control" />
    /// and its child controls and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing"> <c>true</c> to release both managed and unmanaged resources;
    /// <c>false</c> to release only unmanaged
    /// resources when called from the runtime
    /// finalize. </param>
    protected override void Dispose( bool disposing )
    {
        try
        {
            if ( !this.IsDisposed && disposing )
            {
                this.InitializingComponents = true;
                // make sure the device is unbound in case the form is closed without closing the device.
                if ( this.components is not null )
                {
                    this.components?.Dispose();
                    this.components = null;
                }
            }
        }
        finally
        {
            base.Dispose( disposing );
        }
    }

    #endregion

    #region " public members "

    /// <summary>Gets or sets the channel list.</summary>
    private ChannelListBuilder? _channelListBuilder;

    /// <summary> Gets a list of channels. </summary>
    /// <value> A List of channels. </value>
    public string ChannelList => this._channelListBuilder?.ToString() ?? string.Empty;

    /// <summary> Adds channel to list. </summary>
    public void AddChannelToList()
    {
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"adding channel [{( int ) this._slotNumberNumeric.Value},{( int ) this._relayNumberNumeric.Value}] to the list";
            this._channelListBuilder ??= new ChannelListBuilder();

            this._channelListBuilder.AddChannel( ( int ) this._slotNumberNumeric.Value, ( int ) this._relayNumberNumeric.Value );
            this.NotifyPropertyChanged( nameof( this.ChannelList ) );
        }
        catch ( Exception ex )
        {
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Clears the channel list. </summary>
    public void ClearChannelList()
    {
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"clearing channel list";
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            this._channelListBuilder = new ChannelListBuilder();
            this.NotifyPropertyChanged( nameof( this.ChannelList ) );
        }
        catch ( Exception ex )
        {
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Adds memory location to list. </summary>
    public void AddMemoryLocationToList()
    {
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"adding memory location {( int ) this._memoryLocationNumeric.Value}";
            this._channelListBuilder ??= new ChannelListBuilder();

            this._channelListBuilder.AddChannel( ( int ) this._memoryLocationNumeric.Value );
            this.NotifyPropertyChanged( nameof( this.ChannelList ) );
        }
        catch ( Exception ex )
        {
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    #endregion

    #region " control event handlers "

    /// <summary> Adds a channel to list click to 'e'. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void AddChannelToList_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.AddChannelToList();
    }

    /// <summary> Adds a memory location button click to 'e'. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void AddMemoryLocationButton_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.AddMemoryLocationToList();
    }

    /// <summary> Clears the channel ist menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ClearChannelLIstMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.ClearChannelList();
    }

    #endregion


}
