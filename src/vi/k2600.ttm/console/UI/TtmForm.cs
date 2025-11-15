namespace cc.isr.VI.Tsp.K2600.Ttm.Console.UI;

/// <summary> Form for viewing the ttm. </summary>
/// <remarks> David, 2020-10-11. </remarks>
public class TtmForm : Form
{
    /// <summary> Default constructor. </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    public TtmForm() : base()
    {
    }

    /// <summary> Gets or sets the meter view. </summary>
    /// <value> The meter view. </value>
    private cc.isr.VI.Tsp.K2600.Ttm.Controls.MeterView? MeterView { get; set; }

    /// <summary> Gets or sets the visa session. </summary>
    /// <value> The visa session. </value>
    private VisaSessionBase? VisaSession { get; set; }

    /// <summary>
    /// Disposes of the resources (other than memory) used by the
    /// <see cref="Form" />.
    /// </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    /// <param name="disposing"> true to release both managed and unmanaged resources; false to
    /// release only unmanaged resources. </param>
    protected override void Dispose( bool disposing )
    {
        try
        {
            if ( disposing )
            {
                this.VisaSession?.Dispose();
                this.VisaSession = null;

                this.MeterView?.Dispose();
                this.MeterView = null;
            }

        }
        finally
        {
            base.Dispose( disposing );
        }

    }

    /// <summary>
    /// Handles the form closing event. Releases all publishers.
    /// </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    protected void OnClosing()
    {
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            if ( this.VisaSession is not null )
            {
                if ( this.VisaSession.CandidateResourceNameValidated )
                    Properties.Settings.ConsoleSettings.K2600ResourceName = this.VisaSession.ValidatedResourceName;

                if ( !string.IsNullOrWhiteSpace( this.VisaSession.OpenResourceModel ) )
                    Properties.Settings.ConsoleSettings.K2600ResourceModel = this.VisaSession.OpenResourceModel;
            }
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// Raises the <see cref="System.Windows.Forms.Form.Closing" /> event. Releases all publishers.
    /// </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    /// <param name="e"> A <see cref="CancelEventArgs" /> that contains the
    /// event data. </param>
#if NET10_0_OR_GREATER
    protected override void OnFormClosing( System.Windows.Forms.FormClosingEventArgs e )
    {
        try
        {
            if ( e is not null && !e.Cancel )
                this.OnClosing();
        }
        catch
        {
            throw;
        }
        finally
        {
            base.OnFormClosing( e );
        }
    }
#else
    protected override void OnClosing( System.ComponentModel.CancelEventArgs e )
    {
        try
        {
            if ( e is not null && !e.Cancel )
                this.OnClosing();
        }
        catch
        {
            throw;
        }
        finally
        {
            base.OnClosing( e );
        }
    }
#endif

    /// <summary>
    /// Called upon receiving the <see cref="System.Windows.Forms.Form.Load" /> event.
    /// </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    /// <param name="e"> An <see cref="EventArgs" /> that contains the event data. </param>
    protected override void OnLoad( EventArgs e )
    {
        string activity = string.Empty;
        try
        {
            activity = $"{this.Name} adding Meter View";

            // the talker control publishes the device messages which thus get published to the form message box.
            this.MeterView = new cc.isr.VI.Tsp.K2600.Ttm.Controls.MeterView
            {
                // any form messages will be logged.
                ResourceName = Properties.Settings.ConsoleSettings.K2600ResourceName
            };

            if ( this.MeterView is null ) throw new InvalidOperationException( $"{nameof( TtmForm )}.{nameof( TtmForm.MeterView )} is null." );

            this.Controls.Add( this.MeterView );

            // this.AddTalkerControl( Properties.InstrumentSettings.Default.K2600ResourceModel, this.MeterView, false, false );
            // any form messages will be logged.
            // activity = $"{this.Name}; adding log listener";
            // this.AddListener( My.MyProject.Application.Logger );

            this.VisaSession = this.MeterView.Device;

            if ( this.VisaSession is null ) throw new InvalidOperationException( $"{nameof( TtmForm )}.{nameof( TtmForm.VisaSession )} is null." );

            this.VisaSession.CandidateResourceName = Properties.Settings.ConsoleSettings.K2600ResourceName;
            this.VisaSession.CandidateResourceModel = Properties.Settings.ConsoleSettings.K2600ResourceModel;
            if ( !string.IsNullOrWhiteSpace( this.VisaSession.CandidateResourceName ) )
            {
                activity = $"{this.Name}; starting {this.VisaSession.CandidateResourceName} selection task";
                _ = this.VisaSession.AsyncValidateResourceName( this.VisaSession.CandidateResourceName );
            }
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
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
    /// Called upon receiving the <see cref="System.Windows.Forms.Form.Shown" /> event.
    /// </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    /// <param name="e"> A <see cref="EventArgs" /> that contains the event data. </param>
    protected override void OnShown( EventArgs e )
    {
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            if ( this.MeterView is null ) throw new InvalidOperationException( $"{nameof( TtmForm )}.{nameof( TtmForm.MeterView )} is null." );
            this.MeterView.Cursor = Cursors.WaitCursor;
            activity = $"{this.Name} showing dialog";
            base.OnShown( e );
            if ( this.VisaSession is not null && this.VisaSession.IsValidatingResourceName() )
            {
                if ( this.AwaitResourceNameValidationTaskEnabled )
                {
                    activity = $"{this.Name}; awaiting {this.VisaSession.CandidateResourceName} validation";
                    this.VisaSession.AwaitResourceNameValidation( Properties.Settings.ConsoleSettings.ResourceNameSelectionTimeout );
                }
                else
                {
                    activity = $"{this.Name}; validating {this.VisaSession.CandidateResourceName}";
                }
            }
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
            _ = this.MeterView?.Cursor = Cursors.Default;
        }
    }

}
