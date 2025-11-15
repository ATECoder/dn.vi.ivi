using cc.isr.Visa.WinControls;

namespace cc.isr.Visa.Console.UI;

/// <summary>   A visa i/o control. </summary>
/// <remarks>   David, 2021-07-26. </remarks>
public partial class VisaIoControl : UserControl
{
    /// <summary>   Default constructor. </summary>
    /// <remarks>   David, 2021-07-26. </remarks>
    public VisaIoControl() => this.InitializeComponent();

    private cc.isr.Logging.TraceLog.WinForms.MessagesBox? _messagesBox;
    private cc.isr.Tracing.WinForms.TextBoxTraceEventWriter? TextBoxTextWriter { get; set; }

    /// <summary>   Event handler. Called by VisaIoControl for load events. </summary>
    /// <remarks>   David, 2021-07-26. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
#if NET5_0_OR_GREATER
    [System.Runtime.Versioning.RequiresPreviewFeatures]
#endif
    private void VisaIoControl_Load( object? sender, EventArgs e )
    {
        _ = this.TreePanel.AddNode( "ResourceNameFinder", "Resource Name Finder", new ResourceNameSelector() );
        _ = this.TreePanel.AddNode( "SimpleReadWrite", "Simple I/O", new SimpleWriteRead() );
        _ = this.TreePanel.AddNode( "ServiceRequest", "Service Request", new ServiceRequest() );
        _ = this.TreePanel.AddNode( "Settings", "Settings Editor",
            new isr.Json.AppSettings.WinForms.JsonSettingsEditorControl( Properties.Settings.Instance.Scribe ) );

        this._messagesBox = new();
        TreeNode messagesNode = this.TreePanel.AddNode( "Message Box", "Log", this._messagesBox );

        this.TextBoxTextWriter = new( this._messagesBox )
        {
            ContainerTreeNode = messagesNode,
            HandleCreatedCheckEnabled = false,
            TabCaption = "Log",
            CaptionFormat = "{0} " + Convert.ToChar( 0x1C2 ),
            ResetCount = 1000,
            PresetCount = 500,
            TraceLevel = Properties.Settings.Instance.MessageDisplayLevel
        };
        cc.isr.Tracing.TracingPlatform.Instance.AddTraceEventWriter( this.TextBoxTextWriter );

        _ = Program.TraceLogger.LogVerbose( "VISA IO Control loaded" );
    }
}
