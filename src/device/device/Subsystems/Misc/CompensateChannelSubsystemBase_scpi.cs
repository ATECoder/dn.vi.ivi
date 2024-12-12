using cc.isr.Enums;

namespace cc.isr.VI;
public partial class CompensateChannelSubsystemBase
{
    #region " compensation type "

    /// <summary> Define compensation type read writes. </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0028:Simplify collection initialization", Justification = "<Pending>" )]
    private void DefineCompensationTypeReadWrites()
    {
        this.CompensationTypeReadWrites = new();
        foreach ( CompensationTypes enumValue in Enum.GetValues( typeof( CompensationTypes ) ) )
            this.CompensationTypeReadWrites.Add( enumValue );
    }

    /// <summary> Gets or sets the compensation type read writes. </summary>
    /// <value> The compensation type read writes. </value>
    public Pith.EnumReadWriteCollection CompensationTypeReadWrites { get; private set; } = [];

    /// <summary> List of types of the supported compensations. </summary>
    private CompensationTypes _supportedCompensationTypes;

    /// <summary>
    /// Gets or sets the supported Compensation Type. This is a subset of the functions supported by
    /// the instrument.
    /// </summary>
    /// <value> A list of types of the supported compensations. </value>
    public CompensationTypes SupportedCompensationTypes
    {
        get => this._supportedCompensationTypes;
        set
        {
            if ( !this.SupportedCompensationTypes.Equals( value ) )
            {
                this._supportedCompensationTypes = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the compensation type code. </summary>
    /// <value> The compensation type code. </value>
    protected string CompensationTypeCode { get; private set; } = string.Empty;

    /// <summary> The Compensation Type. </summary>
    private CompensationTypes? _compensationType;

    /// <summary> Applies the compensation type described by value. </summary>
    /// <param name="value"> The value. </param>
    private void ApplyCompensationType( CompensationTypes value )
    {
        this._compensationType = value;
        this.CompensationTypeCode = value.ExtractBetween();
    }

    /// <summary> Gets or sets the cached source CompensationType. </summary>
    /// <value>
    /// The <see cref="CompensationTypes">source Compensation Type</see> or none if not set or
    /// unknown.
    /// </value>
    public virtual CompensationTypes? CompensationType
    {
        get => this._compensationType;

        protected set
        {
            if ( !Nullable.Equals( this.CompensationType, value ) )
            {
                this._compensationType = value;
                if ( value.HasValue )
                {
                    this.ApplyCompensationType( value.Value );
                }
                else
                {
                    this.CompensationTypeCode = string.Empty;
                }

                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion
}
/// <summary> A bit-field of flags for specifying compensation types. </summary>
[Flags]
public enum CompensationTypes
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "None" )]
    None = 0,

    /// <summary> An enum constant representing the open circuit option. </summary>
    [System.ComponentModel.Description( "Open (OPEN)" )]
    OpenCircuit = 1,

    /// <summary> An enum constant representing the short circuit option. </summary>
    [System.ComponentModel.Description( "Short (SHOR)" )]
    ShortCircuit = 2,

    /// <summary> An enum constant representing the load option. </summary>
    [System.ComponentModel.Description( "Load (LOAD)" )]
    Load = 4
}
