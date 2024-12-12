namespace cc.isr.VI;

public partial class HarmonicsMeasureSubsystemBase
{
    /// <summary> Information describing the failure short. </summary>
    private string _failureShortDescription;

    /// <summary> Gets or sets the failure Short Description. </summary>
    /// <value> The failure Short Description. </value>
    public string FailureShortDescription
    {
        get => this._failureShortDescription;
        set
        {
            if ( !string.Equals( value, this.FailureShortDescription, StringComparison.OrdinalIgnoreCase ) )
            {
                this._failureShortDescription = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The failure color. </summary>
    private System.Drawing.Color _failureColor;

    /// <summary> Gets or sets the color of the failure. </summary>
    /// <value> The color of the failure. </value>
    public System.Drawing.Color FailureColor
    {
        get => this._failureColor;
        set
        {
            if ( !Equals( value, this.FailureCode ) )
            {
                this._failureColor = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The failure code. </summary>
    private string _failureCode;

    /// <summary> Gets or sets the failure Code. </summary>
    /// <value> The failure Code. </value>
    public string FailureCode
    {
        get => this._failureCode;
        set
        {
            if ( !string.Equals( value, this.FailureCode, StringComparison.OrdinalIgnoreCase ) )
            {
                this._failureCode = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Information describing the failure long. </summary>
    private string _failureLongDescription;

    /// <summary> Gets or sets the failure long description. </summary>
    /// <value> The failure long description. </value>
    public string FailureLongDescription
    {
        get => this._failureLongDescription;
        set
        {
            if ( !string.Equals( value, this.FailureLongDescription, StringComparison.OrdinalIgnoreCase ) )
            {
                this._failureLongDescription = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Notifies of failure information. </summary>
    public void NotifyFailureInfo()
    {
        this.NotifyPropertyChanged( nameof( MultimeterSubsystemBase.FailureLongDescription ) );
        this.NotifyPropertyChanged( nameof( MultimeterSubsystemBase.FailureShortDescription ) );
        this.NotifyPropertyChanged( nameof( MultimeterSubsystemBase.FailureCode ) );
    }

    /// <summary> Updates the meta status described by metaStatus. </summary>
    /// <remarks> David, 2020-08-12. </remarks>
    /// <param name="metaStatus"> The meta status. </param>
    private void UpdateMetaStatus( MetaStatus metaStatus )
    {
        System.Drawing.Color failureColor = System.Drawing.Color.Black;
        string failureCode = string.Empty;
        string failureShortDescription = string.Empty;
        string failureLongDescription = string.Empty;
        if ( metaStatus.HasValue )
        {
            failureColor = metaStatus.ToColor();
            failureCode = $"{metaStatus.TwoCharDescription( "" ),2}";
            failureShortDescription = $"{metaStatus.ToShortDescription( "" ),4}";
            failureLongDescription = metaStatus.ToLongDescription( "" );
        }

        this.FailureLongDescription = failureLongDescription;
        this.FailureShortDescription = failureShortDescription;
        this.FailureCode = failureCode;
        this.FailureColor = failureColor;
    }

    /// <summary> Parse measured value. </summary>
    private void ClearMetaStatus()
    {
        this.FailureCode = string.Empty;
        this.FailureShortDescription = string.Empty;
        this.FailureLongDescription = string.Empty;
    }
}
