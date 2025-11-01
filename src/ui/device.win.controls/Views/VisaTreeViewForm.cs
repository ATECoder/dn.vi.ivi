using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace cc.isr.VI.DeviceWinControls.Views;

/// <summary> Form for viewing the visa tree view. </summary>
/// <remarks>
/// (c) 2019 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2019-01-03 </para>
/// </remarks>
public class VisaTreeViewForm : WinForms.ModelViewForm.ModelViewForm
{
    #region " construction and clean up "

    /// <summary> Default constructor. </summary>
    public VisaTreeViewForm() : base() => this.Name = "Visa.View.Form";

    /// <summary> Creates a new VisaTreeViewForm. </summary>
    /// <returns> A VisaTreeViewForm. </returns>
    public static VisaTreeViewForm Create()
    {
        VisaTreeView? visaTreeView = null;
        try
        {
            visaTreeView = new VisaTreeView();
            return Create( visaTreeView );
        }
        catch
        {
            visaTreeView?.Dispose();

            throw;
        }
    }

    /// <summary> Creates a new VisaTreeViewForm. </summary>
    /// <param name="visaTreeView"> The visa view. </param>
    /// <returns> A VisaTreeViewForm. </returns>
    public static VisaTreeViewForm Create( VisaTreeView visaTreeView )
    {
        VisaTreeViewForm? result = null;
        try
        {
            result = new VisaTreeViewForm() { VisaTreeView = visaTreeView };
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
                if ( this.VisaTreeViewDisposeEnabled && this.VisaTreeView is not null )
                {
                    VisaSessionBase? session = this.VisaTreeView.VisaSessionBase;
                    this.VisaTreeView?.Dispose();
                    session?.Dispose();
                }

                this.VisaTreeView = null;
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
            if ( !string.IsNullOrWhiteSpace( this.VisaTreeView?.VisaSessionBase?.CandidateResourceName ) )
            {
                activity = $"{this.Name}; starting {this.VisaTreeView?.VisaSessionBase?.CandidateResourceName} selection task";
                _ = this.VisaTreeView?.VisaSessionBase?.AsyncValidateResourceName( this.VisaTreeView.VisaSessionBase.CandidateResourceName );
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

    /// <summary> Gets the await the resource name validation task enabled. </summary>
    /// <value> The await the resource name validation task enabled. </value>
    protected bool AwaitResourceNameValidationTaskEnabled { get; set; }

    /// <summary>
    /// Called upon receiving the <see cref="Form.Shown" /> event.
    /// </summary>
    /// <param name="e"> A <see cref="EventArgs" /> that contains the event data. </param>
    protected override void OnShown( EventArgs e )
    {
        string activity = string.Empty;
        if ( this.VisaTreeView is null || this.VisaTreeView.VisaSessionBase is null ) return;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.VisaTreeView.Cursor = Cursors.WaitCursor;
            activity = $"{this.Name} showing dialog";
            base.OnShown( e );
            if ( this.VisaTreeView.VisaSessionBase.IsValidatingResourceName() )
                if ( this.AwaitResourceNameValidationTaskEnabled )
                {
                    activity = $"{this.Name}; awaiting {this.VisaSessionBase?.CandidateResourceName} validation";
                    this.VisaTreeView.VisaSessionBase.AwaitResourceNameValidation( this.VisaSessionBase!.Session!.AllSettings.ResourceSettings.ResourceNameSelectionTimeout );
                }
                else
                    activity = $"{this.Name}; validating {this.VisaSessionBase?.CandidateResourceName}";
        }
        catch ( Exception ex )
        {
            _ = SessionLogger.Instance.LogException( ex, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
            _ = (this.VisaTreeView?.Cursor = Cursors.Default);
        }
    }

    #endregion

    #region " visa tree view "

    /// <summary> Gets the visa view. </summary>
    /// <value> The visa view. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public VisaTreeView? VisaTreeView { get; set; }

    /// <summary> Gets the visa view dispose enabled. </summary>
    /// <value> The visa view dispose enabled. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public bool VisaTreeViewDisposeEnabled { get; set; }

    /// <summary> Gets the locally assigned visa session if any. </summary>
    /// <value> The visa session. </value>
    protected VisaSessionBase? VisaSessionBase => this.VisaTreeView?.VisaSessionBase;

    /// <summary> Creates view. </summary>
    /// <param name="candidateResourceName">  Name of the candidate resource. </param>
    /// <param name="candidateResourceModel"> The candidate resource model. </param>
    private void CreateViewView( string candidateResourceName, string candidateResourceModel )
    {
        if ( this.VisaSessionBase is null ) return;
        try
        {
            this.VisaTreeView = new VisaTreeView( new VisaSession() );
            this.VisaSessionBase.CandidateResourceModel = candidateResourceModel;
            this.VisaSessionBase.CandidateResourceName = candidateResourceName;
        }
        catch
        {
            this.VisaTreeView?.Dispose();
            throw;
        }
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
        VisaTreeView? visaTreeView = null;
        try
        {
            this.CreateViewView( candidateResourceName, candidateResourceModel );
            return visaTreeView is null
                ? DialogResult.OK
                : this.ShowDialog( owner, visaTreeView );
        }
        catch
        {
            visaTreeView?.Dispose();

            throw;
        }
    }

    /// <summary> Shows the form dialog. </summary>
    /// <param name="owner">        The owner. </param>
    /// <param name="visaTreeView"> The visa view. </param>
    /// <returns> A Windows.Forms.DialogResult. </returns>
    public DialogResult ShowDialog( IWin32Window owner, VisaTreeView visaTreeView )
    {
        this.VisaTreeView = visaTreeView;
        return this.ShowDialog( owner );
    }

    /// <summary> Shows the form. </summary>
    /// <param name="owner">                  The owner. </param>
    /// <param name="candidateResourceName">  Name of the candidate resource. </param>
    /// <param name="candidateResourceModel"> The candidate resource model. </param>
    public void Show( IWin32Window owner, string candidateResourceName, string candidateResourceModel )
    {
        VisaTreeView? visaTreeView = null;
        try
        {
            this.CreateViewView( candidateResourceName, candidateResourceModel );
            if ( visaTreeView is not null )
                this.Show( owner, visaTreeView );
        }
        catch
        {
            visaTreeView?.Dispose();

            throw;
        }
    }

    /// <summary> Shows the form. </summary>
    /// <param name="owner">        The owner. </param>
    /// <param name="visaTreeView"> The visa view. </param>
    public void Show( IWin32Window owner, VisaTreeView visaTreeView )
    {
        this.VisaTreeView = visaTreeView;
        this.Show( owner );
    }

    #endregion
}
