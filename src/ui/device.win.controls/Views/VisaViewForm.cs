using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace cc.isr.VI.DeviceWinControls.Views;

/// <summary> Form for viewing the visa view. </summary>
/// <remarks>
/// (c) 2019 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2019-01-03 </para>
/// </remarks>
public class VisaViewForm : WinForms.ModelViewForm.ModelViewForm
{
    #region " construction and clean up "

    /// <summary> Default constructor. </summary>
    public VisaViewForm() : base() => this.Name = "Visa.View.Form";

    /// <summary> Creates a new VisaViewForm. </summary>
    /// <returns> A VisaViewForm. </returns>
    public static VisaViewForm Create()
    {
        VisaView? visaView = null;
        try
        {
            visaView = new VisaView();
            return Create( visaView );
        }
        catch
        {
            visaView?.Dispose();

            throw;
        }
    }

    /// <summary> Creates a new VisaViewForm. </summary>
    /// <param name="visaView"> The visa view. </param>
    /// <returns> A VisaViewForm. </returns>
    public static VisaViewForm Create( VisaView visaView )
    {
        VisaViewForm? result = null;
        try
        {
            result = new VisaViewForm() { VisaView = visaView };
        }
        catch
        {
            result?.Dispose();

            throw;
        }

        return result;
    }

    /// <summary>
    /// Disposes of the resources (other than memory) used by the
    /// <see cref="Form" />.
    /// </summary>
    /// <param name="disposing"> true to release both managed and unmanaged resources; false to
    /// release only unmanaged resources. </param>
    protected override void Dispose( bool disposing )
    {
        try
        {
            if ( !this.IsDisposed && disposing )
            {
                this.InitializingComponents = true;
                if ( this.VisaViewDisposeEnabled && this.VisaView is not null )
                    this.VisaView?.Dispose();
                if ( this.VisaSessionBase is not null )
                {
                    this.VisaSessionBase.Dispose();
                    this.VisaSessionBase = null;
                }

                this.VisaView = null;
            }
        }
        finally
        {
            base.Dispose( disposing );
        }
    }

    #endregion

    #region " form events "

    /// <summary>
    /// Called upon receiving the <see cref="Form.Load" /> event.
    /// </summary>
    /// <param name="e"> An <see cref="EventArgs" /> that contains the event data. </param>
    protected override void OnLoad( EventArgs e )
    {
        string activity = string.Empty;
        try
        {
            activity = $"{this.Name} adding Visa View";
            // any form messages will be logged.
            activity = $"{this.Name}; adding log listener";
            if ( !string.IsNullOrWhiteSpace( this.VisaView?.VisaSessionBase?.CandidateResourceName ) )
            {
                activity = $"{this.Name}; starting {this.VisaView?.VisaSessionBase?.CandidateResourceName} selection task";
                _ = this.VisaView?.VisaSessionBase?.AsyncValidateResourceName( this.VisaView.VisaSessionBase.CandidateResourceName );
            }
        }
        catch ( Exception ex )
        {
            _ = SessionLogger.Instance.LogException( ex, activity );
        }
        finally
        {
            base.OnLoad( e );
        }
    }

    /// <summary> Gets or sets the await the resource name validation task enabled. </summary>
    /// <value> The await the resource name validation task enabled. </value>
    protected bool AwaitResourceNameValidationTaskEnabled { get; set; }

    /// <summary>
    /// Called upon receiving the <see cref="Form.Shown" /> event.
    /// </summary>
    /// <param name="e"> A <see cref="EventArgs" /> that contains the event data. </param>
    protected override void OnShown( EventArgs e )
    {
        string activity = string.Empty;
        if ( this.VisaView is null || this.VisaView.VisaSessionBase is null ) return;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.VisaView.Cursor = Cursors.WaitCursor;
            activity = $"{this.Name} showing dialog";
            base.OnShown( e );
            if ( this.VisaView.VisaSessionBase.IsValidatingResourceName() )
                if ( this.AwaitResourceNameValidationTaskEnabled )
                {
                    activity = $"{this.Name}; awaiting {this.VisaView.VisaSessionBase.CandidateResourceName} validation";
                    this.VisaView.VisaSessionBase.AwaitResourceNameValidation( this.VisaSessionBase!.Session!.ResourceSettings.ResourceNameSelectionTimeout );
                }
                else
                    activity = $"{this.Name}; validating {this.VisaView.VisaSessionBase.CandidateResourceName}";
        }
        catch ( Exception ex )
        {
            _ = SessionLogger.Instance.LogException( ex, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
            _ = (this.VisaView?.Cursor = Cursors.Default);
        }
    }

    #endregion

    #region " visa view "

    /// <summary> Gets or sets the visa view. </summary>
    /// <value> The visa view. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public VisaView? VisaView { get; set; }

    /// <summary> Gets or sets the visa view dispose enabled. </summary>
    /// <value> The visa view dispose enabled. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public bool VisaViewDisposeEnabled { get; set; }

    /// <summary> Gets or sets the locally assigned visa session if any. </summary>
    /// <value> The visa session. </value>
    protected VisaSessionBase? VisaSessionBase { get; set; }

    /// <summary> Creates view. </summary>
    /// <param name="candidateResourceName">  Name of the candidate resource. </param>
    /// <param name="candidateResourceModel"> The candidate resource model. </param>
    private void CreateViewView( string candidateResourceName, string candidateResourceModel )
    {
        this.VisaSessionBase = new VisaSession();
        this.VisaView = new VisaView( this.VisaSessionBase );
        this.VisaView.VisaSessionBase!.CandidateResourceModel = candidateResourceModel;
        this.VisaView.VisaSessionBase.CandidateResourceName = candidateResourceName;
    }

    #endregion

    #region " show dialog "

    /// <summary> Shows the form dialog. </summary>
    /// <param name="owner">                  The owner. </param>
    /// <param name="candidateResourceName">  Name of the candidate resource. </param>
    /// <param name="candidateResourceModel"> The candidate resource model. </param>
    /// <returns> A Windows.Forms.DialogResult. </returns>
    public DialogResult ShowDialog( IWin32Window owner, string candidateResourceName, string candidateResourceModel )
    {
        VisaView? visaView = null;
        try
        {
            this.CreateViewView( candidateResourceName, candidateResourceModel );
            return visaView != null
                ? this.ShowDialog( owner, visaView )
                : DialogResult.OK;
        }
        catch
        {
            visaView?.Dispose();

            throw;
        }
    }

    /// <summary> Shows the form dialog. </summary>
    /// <param name="owner">    The owner. </param>
    /// <param name="visaView"> The visa view. </param>
    /// <returns> A Windows.Forms.DialogResult. </returns>
    public DialogResult ShowDialog( IWin32Window owner, VisaView visaView )
    {
        this.VisaView = visaView;
        return this.ShowDialog( owner );
    }

    /// <summary> Shows the form. </summary>
    /// <param name="owner">                  The owner. </param>
    /// <param name="candidateResourceName">  Name of the candidate resource. </param>
    /// <param name="candidateResourceModel"> The candidate resource model. </param>
    public void Show( IWin32Window owner, string candidateResourceName, string candidateResourceModel )
    {
        VisaView? visaView = null;
        try
        {
            this.CreateViewView( candidateResourceName, candidateResourceModel );
            if ( visaView is not null )
                this.Show( owner, visaView );
        }
        catch
        {
            visaView?.Dispose();

            throw;
        }
    }

    /// <summary> Shows the form. </summary>
    /// <param name="owner">    The owner. </param>
    /// <param name="visaView"> The visa view. </param>
    public void Show( IWin32Window owner, VisaView visaView )
    {
        this.VisaView = visaView;
        this.Show( owner );
    }

    #endregion
}
