using System;

namespace cc.isr.VI.WinControls.Tests;

public sealed partial class Asserts
{
    /// <summary> An opener. </summary>
    private sealed class Opener : cc.isr.Std.Notifiers.OpenResourceBase
    {
        /// <summary> True if is open, false if not. </summary>
        private bool _isOpen;

        /// <summary> Gets the Open status. </summary>
        /// <value> The Open status. </value>
        public override bool IsOpen => this._isOpen;

        /// <summary> Opens a resource. </summary>
        /// <exception cref="OperationCanceledException"> Thrown when an Operation Canceled error condition
        /// occurs. </exception>
        /// <param name="resourceName">  The name of the resource. </param>
        /// <param name="ResourceModel"> The resource model. </param>
        public override void OpenResource( string resourceName, string ResourceModel )
        {
            this._isOpen = true;
            base.OpenResource( resourceName, ResourceModel );
            System.ComponentModel.CancelEventArgs e = new();
            this.OnOpening( e );
            if ( e.Cancel )
            {
                throw new OperationCanceledException( $"Opening {ResourceModel}:{resourceName} status canceled;. " );
            }

            this.OnOpened( EventArgs.Empty );
        }

        /// <summary> Closes the resource. </summary>
        public override void CloseResource()
        {
            this._isOpen = false;
            this.OnClosing( new System.ComponentModel.CancelEventArgs() );
            this.OnClosed( EventArgs.Empty );
        }

        /// <summary> Gets or sets the active state cleared. </summary>
        /// <value> The active state cleared. </value>
        public bool ActiveStateCleared { get; set; }

        /// <summary> Applies default settings and clears the resource active state. </summary>
        public override void ClearActiveState()
        {
            this.ActiveStateCleared = true;
        }

        /// <summary>   Attempts to open resource from the given data. </summary>
        /// <remarks>   David, 2021-12-07. </remarks>
        /// <param name="resourceName">     The name of the resource. </param>
        /// <param name="ResourceModel">    The resource model. </param>
        /// <returns>   <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
        public override (bool success, string Details) TryOpen( string resourceName, string ResourceModel )
        {
            try
            {
                this.OpenResource();
                return (true, string.Empty);
            }
            catch ( Exception ex )
            {
                return (false, ex.ToString());
                throw;
            }
        }

        /// <summary>   Attempts to close resource from the given data. </summary>
        /// <remarks>   David, 2021-12-07. </remarks>
        /// <returns>   <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
        public override (bool success, string Details) TryClose()
        {
            try
            {
                this.CloseResource();
                return (true, string.Empty);
            }
            catch ( Exception ex )
            {
                return (false, ex.ToString());
                throw;
            }
        }

        /// <summary>   Attempts to clear active state. </summary>
        /// <remarks>   David, 2021-12-07. </remarks>
        /// <returns>   (Success: True if success; false otherwise, Details) </returns>
        public override (bool success, string Details) TryClearActiveState()
        {
            try
            {
                this.ClearActiveState();
                return (true, string.Empty);
            }
            catch ( Exception ex )
            {
                return (false, ex.ToString());
                throw;
            }
        }
    }

    /// <summary>   Assert selector should enumerate resources. </summary>
    /// <remarks>   David, 2021-07-06. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="selectorOpener">   The selector opener. </param>
    /// <param name="selector">         The selector. </param>
    public static void AssertSelectorShouldEnumerateResources( cc.isr.WinControls.SelectorOpener selectorOpener, cc.isr.Std.Notifiers.SelectResourceBase selector )
    {
        Assert.IsNotNull( selectorOpener, nameof( selectorOpener ) );
        Assert.IsNotNull( selector, nameof( selector ) );
        selectorOpener.AssignSelectorViewModel( selector );
        _ = selector.EnumerateResources( false );
        Assert.AreEqual( selector.ResourceNames.Count, selectorOpener.ResourceNamesCount, $"Resource names count should match internal resource names count" );
    }

    /// <summary>   Assert resource should open and close. </summary>
    /// <remarks>   David, 2021-07-06. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="selectorOpener">   The control. </param>
    /// <param name="selector">         The selector. </param>
    /// <param name="opener">           The opener. </param>
    public static void AssertResourceShouldOpenAndClose( cc.isr.WinControls.SelectorOpener selectorOpener,
                                                         cc.isr.Std.Notifiers.SelectResourceBase selector,
                                                         cc.isr.Std.Notifiers.OpenResourceBase opener )
    {
        Assert.IsNotNull( selectorOpener, nameof( selectorOpener ) );
        Assert.IsNotNull( selector, nameof( selector ) );
        Assert.IsNotNull( opener, nameof( opener ) );

        using System.Windows.Forms.Form form = new();
        form.Controls.Add( selectorOpener );
        form.Show();
        selectorOpener.Visible = true;
        selectorOpener.AssignSelectorViewModel( selector );
        selectorOpener.AssignOpenerViewModel( opener );
        _ = selector.EnumerateResources( true );
        Assert.AreEqual( selector.ResourceNames.Count, selectorOpener.ResourceNamesCount, $"Resource names count should match internal resource names count" );
        Assert.IsGreaterThan( 0, selectorOpener.SelectedValueChangeCount, $"Selected value count {selectorOpener.SelectedValueChangeCount} should exceed zero" );
        Assert.IsFalse( string.IsNullOrWhiteSpace( selector.ValidatedResourceName ), "Validated resource name is not empty" );
        Assert.IsTrue( opener.OpenEnabled, $"Opener open enabled after validating the resource" );
        Std.ActionEventArgs e = new();
        (bool success, string details) = opener.TryOpen();
        Assert.IsTrue( success, $"open failed {details}" );
        Assert.AreEqual( selector.ValidatedResourceName, opener.OpenResourceName, "validated resource name assigned to resource name" );
        Assert.AreEqual( opener.CandidateResourceModel, opener.OpenResourceModel, "title assigned to resource model" );
        (success, details) = opener.TryClose();
        Assert.IsTrue( success, $"close failed {details}" );
    }

    /// <summary>   Assert resource should open and close. </summary>
    /// <remarks>   David, 2021-07-06. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="testInfo">         Information describing the test. </param>
    /// <param name="resourceSettings"> The resource settings. </param>
    public static void AssertResourceShouldOpenAndClose( string resourceModel, string resourceName )
    {
        Assert.IsNotNull( resourceModel, nameof( resourceModel ) );
        Assert.IsNotNull( resourceName, nameof( resourceName ) );
        Assert.IsNotNull( SessionFactory.Instance.Factory.ResourcesProvider().ResourceFinder, "ResourceFinder" );
        using isr.WinControls.SelectorOpener control = new();
        cc.isr.Std.Notifiers.SelectResourceBase selector = new SessionFactory()
        {
            Searchable = true,
            ResourcesFilter = SessionFactory.Instance.Factory.ResourcesProvider().ResourceFinder!.BuildMinimalResourcesFilter()
        };
        Opener opener = new()
        {
            CandidateResourceModel = resourceModel,
            CandidateResourceName = resourceName
        };
        AssertResourceShouldOpenAndClose( control, selector, opener );
    }
}
