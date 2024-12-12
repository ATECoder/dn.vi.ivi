using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace cc.isr.Visa.WinControls;

/// <summary>   Form for viewing the resource names selector. </summary>
/// <remarks>   David, 2021-07-17. </remarks>
public partial class ResourceNamesSelectorForm : Form
{
    /// <summary>   Default constructor. </summary>
    /// <remarks>   David, 2021-07-16. </remarks>
    public ResourceNamesSelectorForm()
    {
        this.InitializeComponent();
        this.Icon = Properties.Resources.favicon;
        this.ResourceNamesSelector.Validated += this.OnResourceNameSelectorValidated;
    }

    /// <summary>   Raises the resource names selector validated event. </summary>
    /// <remarks>   David, 2021-07-16. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information to send to registered event handlers. </param>
    private void OnResourceNameSelectorValidated( object? sender, EventArgs e )
    {
        this.SelectedResourceNames = this.ResourceNamesSelector.ResourceNames;
    }

    /// <summary>   Gets or sets the selected resource names. </summary>
    /// <value> The name of the selected resource. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public IEnumerable<string>? SelectedResourceNames { get; private set; }

}
