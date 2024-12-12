using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Ivi.Visa;

namespace cc.isr.Visa.WinControls;

/// <summary>   A resource name selector. </summary>
/// <remarks>   David, 2021-07-16. </remarks>
public partial class ResourceNamesSelector : UserControl
{
    #region " construction and cleanup "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   David, 2021-07-17. </remarks>
    public ResourceNamesSelector()
    {
        this.Load += this.OnLoad;
        this.InitializeComponent();

        this.PopulateFilterList();
        this.FilterComboBox.Text = this.ResourceNameFilter;
    }

    #endregion

    #region " control event handlers "

    /// <summary> Event handler. Called by MyBase for load events. </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information to send to registered event handlers. </param>
    private void OnLoad( object? sender, EventArgs e )
    {
        try
        {
            this.ListAvailableResourceNames();
        }
        catch ( Exception ex )
        {
            this.AvailableResourceNamesListBox.DataSource = new List<string>( [ex.Message] );
        }
    }

    /// <summary>   Event handler. Called by RefreshButton for click events. </summary>
    /// <remarks>   David, 2021-07-17. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void RefreshButton_Click( object? sender, EventArgs e )
    {
        try
        {
            this.ListAvailableResourceNames();
        }
        catch ( Exception ex )
        {
            this.AvailableResourceNamesListBox.DataSource = new List<string>( [ex.Message] );
        }
    }

    #endregion

    #region " resource names "

    /// <summary>   Raises the selected resource names changed event. </summary>
    /// <remarks>   David, 2021-07-17. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information to send to registered event handlers. </param>
    private void OnSelectedResourceNamesChanged( object? sender, EventArgs e )
    {
        this.OnValidated( e );
    }

    /// <summary>   Event handler. Called by AddResourceNameButton for click events. </summary>
    /// <remarks>   David, 2020-10-11. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void AddResourceNameButton_Click( object? sender, EventArgs e )
    {
        string? selectedString = this.AvailableResourceNamesListBox.SelectedItem?.ToString();
        if ( selectedString is not null )
        {
            _ = this.SelectedResourceNamesListBox.Items.Add( selectedString );
            this.OnSelectedResourceNamesChanged( this.SelectedResourceNamesListBox, e );
        }
    }

    /// <summary>   Event handler. Called by RemoveResourceNameButton for click events. </summary>
    /// <remarks>   David, 2020-10-11. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void RemoveResourceNameButton_Click( object? sender, EventArgs e )
    {
        this.SelectedResourceNamesListBox.Items.Remove( this.SelectedResourceNamesListBox.SelectedItem?.ToString() ?? string.Empty );
        this.OnSelectedResourceNamesChanged( this.SelectedResourceNamesListBox, e );
    }

    /// <summary> Gets a list of names of the resources. </summary>
    /// <value> A list of names of the resources. </value>
    [Browsable( false )]
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public IEnumerable<string> ResourceNames
    {
        get
        {
            List<string> l = [];
            foreach ( object item in this.SelectedResourceNamesListBox.Items )
                if ( item is not null )
                    l.Add( ( string ) item );
            return [.. l];
        }
    }

    #endregion

    #region " resource list "

    /// <summary>   Gets or sets the resource name filter. </summary>
    /// <value> The resource name filter. </value>
    [Bindable( true )]
    [Browsable( true )]
    [Category( "Appearance" )]
    [Description( "The GREP filter for filtering the resource names" )]
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Visible )]
    [DefaultValue( "(ASRL|GPIB|TCPIP|USB)?*" )]
    public string ResourceNameFilter { get; set; } = "(ASRL|GPIB|TCPIP|USB)?*";

    /// <summary>   Gets or sets the selected resource name filter. </summary>
    /// <value> The selected resource name filter. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public string SelectedResourceNameFilter
    {
        get => this.FilterComboBox.Text;
        set => this.FilterComboBox.Text = value;
    }

    /// <summary>   Populates the filter list. </summary>
    /// <remarks>   David, 2021-07-16. </remarks>
    private void PopulateFilterList()
    {
        this.FilterComboBox.Items.Clear();
        _ = this.FilterComboBox.Items.Add( "?*" );
        _ = this.FilterComboBox.Items.Add( "(ASRL|GPIB|TCPIP|USB)?*" );
        _ = this.FilterComboBox.Items.Add( "(ASRL|GPIB|TCPIP|USB)?*INSTR" );
        _ = this.FilterComboBox.Items.Add( "ASRL?*INSTR" );
        _ = this.FilterComboBox.Items.Add( "GPIB?*" );
        _ = this.FilterComboBox.Items.Add( "GPIB?*INSTR" );
        _ = this.FilterComboBox.Items.Add( "GPIB?*INTFC" );
        _ = this.FilterComboBox.Items.Add( "PXI?*" );
        _ = this.FilterComboBox.Items.Add( "PXI?*BACKPLANE" );
        _ = this.FilterComboBox.Items.Add( "PXI?*INSTR" );
        _ = this.FilterComboBox.Items.Add( "TCPIP?*" );
        _ = this.FilterComboBox.Items.Add( "TCPIP?*INSTR" );
        _ = this.FilterComboBox.Items.Add( "TCPIP?*SOCKET" );
        _ = this.FilterComboBox.Items.Add( "USB?*" );
        _ = this.FilterComboBox.Items.Add( "USB?*INSTR" );
        _ = this.FilterComboBox.Items.Add( "USB?*RAW" );
        _ = this.FilterComboBox.Items.Add( "VXI?*" );
        _ = this.FilterComboBox.Items.Add( "VXI?*BACKPLANE" );
        _ = this.FilterComboBox.Items.Add( "VXI?*INSTR" );
        this.FilterComboBox.SelectedIndex = 0;
    }

    /// <summary>   List available resource names. </summary>
    /// <remarks>   David, 2021-07-17. </remarks>
    public void ListAvailableResourceNames()
    {
        // A static method provided by the Ivi.Visa.ResourceManager class finds the available resource.
        // This Global Resource manager uses the resources defined by implementations, such as Keysight or NI.
        try
        {
            this.AvailableResourceNamesListBox.DataSource = new List<string>( GlobalResourceManager.Find( this.SelectedResourceNameFilter ) );
        }
        catch ( Ivi.Visa.VisaException ex )
        {
            this.AvailableResourceNamesListBox.DataSource = new List<string>( [ex.Message] );
        }
        catch
        { throw; }
    }

    #endregion
}
