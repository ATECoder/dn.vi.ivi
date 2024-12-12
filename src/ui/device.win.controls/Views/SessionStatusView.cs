using System.ComponentModel;

namespace cc.isr.VI.DeviceWinControls;

/// <summary> ASession Status view. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-12-31 </para>
/// </remarks>
public partial class SessionStatusView : cc.isr.WinControls.ModelViewLoggerBase
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    public SessionStatusView() : base()
    {
        this.InitializingComponents = true;
        // This call is required by the Windows Form Designer.
        this.InitializeComponent();
        this.InitializingComponents = false;
    }

    /// <summary> Creates a new <see cref="SessionStatusView"/> </summary>
    /// <returns> A <see cref="SessionStatusView"/>. </returns>
    public static SessionStatusView Create()
    {
        SessionStatusView? view = null;
        try
        {
            view = new SessionStatusView();
            return view;
        }
        catch
        {
            view?.Dispose();
            throw;
        }
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="System.Windows.Forms.Control" />
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
                // todo: Doe we need this? Me.BindVisaSessionBase(Nothing)
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

    #region " visa session base (device base) "

    /// <summary> Gets the visa session base. </summary>
    /// <value> The visa session base. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public VisaSessionBase? VisaSessionBase { get; private set; }

    /// <summary> Binds the visa session to its controls. </summary>
    /// <param name="visaSessionBase"> The visa session view model. </param>
    public void BindVisaSessionBase( VisaSessionBase? visaSessionBase )
    {
        if ( this.VisaSessionBase is not null )
        {
            this._selectorOpener.AssignSelectorViewModel( null );
            this._selectorOpener.AssignOpenerViewModel( null );
            this.VisaSessionBase = null;
        }

        if ( visaSessionBase is not null )
        {
            this.VisaSessionBase = visaSessionBase;
            this._selectorOpener.AssignSelectorViewModel( visaSessionBase.SessionFactory );
            this._selectorOpener.AssignOpenerViewModel( visaSessionBase );
        }

        this.StatusView?.BindVisaSessionBase( visaSessionBase );
        this.SessionView?.BindVisaSessionBase( visaSessionBase );
    }

    #endregion

    #region " exposed views "

    /// <summary> Gets the status view. </summary>
    /// <value> The status view. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public StatusView? StatusView { get; private set; }

    /// <summary> Gets the session view. </summary>
    /// <value> The session view. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public SessionView? SessionView { get; private set; }

    #endregion
}
