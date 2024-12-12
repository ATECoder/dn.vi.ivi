using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace cc.isr.Visa.WinControls;

/// <summary>   Form for viewing the resource name selector. </summary>
/// <remarks>   David, 2021-07-17. </remarks>
public partial class ResourceNameSelectorForm : Form
{
    /// <summary>   Default constructor. </summary>
    /// <remarks>   David, 2021-07-16. </remarks>
    public ResourceNameSelectorForm()
    {
        this.InitializeComponent();
        this.Icon = Properties.Resources.favicon;
        this.ResourceNameSelector.Validated += this.OnResourceNameSelectorValidated;
    }

    /// <summary>   Raises the resource name selector validated event. </summary>
    /// <remarks>   David, 2021-07-16. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information to send to registered event handlers. </param>
    private void OnResourceNameSelectorValidated( object? sender, EventArgs e )
    {
        this.SelectedResourceName = this.ResourceNameSelector.ResourceName;
    }

    /// <summary>   Gets or sets the selected resource name. </summary>
    /// <value> The name of the selected resource. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public string? SelectedResourceName { get; private set; }

    /// <summary>   Gets or sets the name of the resource. </summary>
    /// <value> The name of the resource. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public string? ResourceName
    {
        get => this.ResourceNameSelector.ResourceName;
        set => this.ResourceNameSelector.ResourceName = value ?? string.Empty;
    }

}
