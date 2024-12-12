using System;

namespace cc.isr.VI.Tsp2
{
    /// <summary> Manages TSP SMU subsystem. </summary>
    /// <remarks>
    /// (c) 2007 Integrated Scientific Resources, Inc. All rights reserved. <para>
    /// Licensed under The MIT License. </para><para>
    /// David, 2013-11-07. Uses new core. </para><para>
    /// David, 2008-01-28, 2.0.2949. Use .NET Framework.  </para><para>
    /// David, 2007-03-12, 1.15.2627.x. </para>
    /// </remarks>
    public abstract class SourceMeasureUnitBase : SubsystemBase
    {
        #region " construction and cleanup "

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceMeasureUnitBase" /> class.
        /// </summary>
        /// <remarks>
        /// Note that the local node status clear command only clears the SMU status.  So, issue a CLS
        /// and RST as necessary when adding an SMU.
        /// </remarks>
        /// <param name="statusSubsystem"> A reference to a <see cref="VI.StatusSubsystemBase">TSP status
        /// Subsystem</see>. </param>
        protected SourceMeasureUnitBase( VI.StatusSubsystemBase statusSubsystem ) : this( statusSubsystem, 0, Syntax.Tsp.SourceMeasureUnit.SourceMeasureUnitNumberA )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceMeasureUnitBase" /> class.
        /// </summary>
        /// <remarks>
        /// Note that the local node status clear command only clears the SMU status.  So, issue a CLS
        /// and RST as necessary when adding an SMU.
        /// </remarks>
        /// <param name="statusSubsystem"> A reference to a <see cref="VI.StatusSubsystemBase">TSP status
        /// Subsystem</see>. </param>
        /// <param name="nodeNumber">      Specifies the node number. </param>
        /// <param name="smuNumber">       Specifies the SMU (either 'a' or 'b'. </param>
        protected SourceMeasureUnitBase( VI.StatusSubsystemBase statusSubsystem, int nodeNumber, string smuNumber ) : base( statusSubsystem )
        {
            this._nodeNumber = nodeNumber;
            this.UnitNumber = smuNumber;
            this.SourceMeasureBasedSubsystems = new SourceMeasureUnitSubsystemCollection() {
                NodeNumber = nodeNumber,
                UnitNumber = smuNumber
            };
        }

        /// <summary> Validated the given source measure unit. </summary>
        /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        /// <param name="sourceMeasureUnit"> Source measure unit. </param>
        /// <returns> A SourceMeasureUnitBase. </returns>
        public static SourceMeasureUnitBase Validated( SourceMeasureUnitBase sourceMeasureUnit )
        {
            return sourceMeasureUnit is null ? throw new ArgumentNullException( nameof( sourceMeasureUnit ) ) : sourceMeasureUnit;
        }

        #endregion

        #region " smu node "

        /// <summary>
        /// Queries the <see cref="SourceMeasureUnitReference">source measure unit</see> exists.
        /// </summary>
        /// <returns> <c>true</c> if the source measure unit exists; otherwise <c>false</c> </returns>
        public bool SourceMeasureUnitExists()
        {
            this.Session.MakeTrueFalseReplyIfEmpty( false );
            return !this.Session.IsNil( this.SourceMeasureUnitReference );
        }

        /// <summary> The local node number. </summary>
        private int _localNodeNumber;

        /// <summary> Gets or sets the local node number. </summary>
        /// <value> The local node number. </value>
        public int LocalNodeNumber
        {
            get => this._localNodeNumber;
            set
            {
                if ( !value.Equals( this.LocalNodeNumber ) )
                {
                    this._localNodeNumber = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        /// <summary> The node number. </summary>
        private int _nodeNumber;

        /// <summary> Gets or sets the one-based node number. </summary>
        /// <value> The node number. </value>
        public int NodeNumber
        {
            get => this._nodeNumber;
            set
            {
                if ( !value.Equals( this.NodeNumber ) )
                {
                    this._nodeNumber = value;
                    this.NotifyPropertyChanged();
                    if ( !string.IsNullOrWhiteSpace( this.UnitNumber ) )
                    {
                        // update the smu reference
                        this.UnitNumber = this.UnitNumber;
                    }
                }
            }
        }

        /// <summary> Source measure unit reference. </summary>
        private string _sourceMeasureUnitReference;

        /// <summary> Gets or sets the full SMU reference string, e.g., '_G.node[ 1 ].smua'. </summary>
        /// <value> The SMU reference. </value>
        public string SourceMeasureUnitReference
        {
            get => this._sourceMeasureUnitReference;
            protected set
            {
                if ( string.IsNullOrWhiteSpace( value ) )
                    value = string.Empty;
                if ( !value.Equals( this.SourceMeasureUnitReference ) )
                {
                    this._sourceMeasureUnitReference = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        /// <summary> Name of the source measure unit. </summary>
        private string _sourceMeasureUnitName;

        /// <summary> Gets or sets the SMU name string, e.g., 'smua'. </summary>
        /// <value> The SMU reference. </value>
        public string SourceMeasureUnitName
        {
            get => this._sourceMeasureUnitName;
            protected set
            {
                if ( string.IsNullOrWhiteSpace( value ) )
                    value = string.Empty;
                if ( !value.Equals( this.SourceMeasureUnitName ) )
                {
                    this._sourceMeasureUnitName = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        /// <summary> Gets or sets the SMU Unit number. </summary>
        private string _unitNumber;

        /// <summary> Gets or sets the SMU unit number (a or b). </summary>
        /// <value> The unit number. </value>
        public string UnitNumber
        {
            get => this._unitNumber;
            set
            {
                if ( string.IsNullOrWhiteSpace( value ) )
                    value = string.Empty;
                if ( !value.Equals( this.UnitNumber ) )
                {
                    this._unitNumber = value;
                    this.NotifyPropertyChanged();
                }

                if ( string.IsNullOrWhiteSpace( this.UnitNumber ) )
                {
                    this.SourceMeasureUnitName = string.Empty;
                    this.SourceMeasureUnitReference = string.Empty;
                }
                else
                {
                    this.SourceMeasureUnitName = Syntax.Tsp.SourceMeasureUnit.BuildSmuName( this.UnitNumber );
                    this.SourceMeasureUnitReference = this.NodeNumber <= 0 || this.LocalNodeNumber <= 0 ? Syntax.Tsp.SourceMeasureUnit.BuildSmuReference( this.UnitNumber ) : Syntax.Tsp.SourceMeasureUnit.BuildSmuReference( this.NodeNumber, this.LocalNodeNumber, this.UnitNumber );
                }
            }
        }

        /// <summary> Gets the unique key. </summary>
        /// <value> The unique key. </value>
        public string UniqueKey => this.SourceMeasureUnitReference;

        /// <summary> Gets or sets source measure based subsystems. </summary>
        /// <value> The source measure based subsystems. </value>
        public SourceMeasureUnitSubsystemCollection SourceMeasureBasedSubsystems { get; private set; }

        /// <summary> Adds a subsystem. </summary>
        /// <param name="item"> The item. </param>
        public void Add( SourceMeasureUnitBase item )
        {
            this.SourceMeasureBasedSubsystems.Add( item );
        }

        /// <summary> Removes the subsystem described by item. </summary>
        /// <param name="item"> The item. </param>
        public void Remove( SourceMeasureUnitBase item )
        {
            _ = this.SourceMeasureBasedSubsystems.Remove( item );
        }

        #endregion

        #region " maximum output power "

        /// <summary> The Maximum Output Power. </summary>
        private double _maximumOutputPower;

        /// <summary>
        /// Gets or sets the source measure unit maximum output power. Set to
        /// <see cref="VI.Syntax.Syntax.Infinity">infinity</see> to set to maximum or to
        /// <see cref="VI.Syntax.Syntax.NegativeInfinity">negative infinity</see> for minimum.
        /// </summary>
        /// <value> <c>null</c> if value is not known. </value>
        public double MaximumOutputPower
        {
            get => this._maximumOutputPower;
            set
            {
                if ( !Equals( this.MaximumOutputPower, value ) )
                {
                    this._maximumOutputPower = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        /// <summary> Maximum current. </summary>
        /// <param name="sourceVoltage"> Source voltage. </param>
        /// <returns> A Double. </returns>
        public double MaximumCurrent( double sourceVoltage )
        {
            return this.MaximumOutputPower / sourceVoltage;
        }

        /// <summary> Maximum voltage. </summary>
        /// <param name="sourceCurrent"> Source current. </param>
        /// <returns> A Double. </returns>
        public double MaximumVoltage( double sourceCurrent )
        {
            return this.MaximumOutputPower / sourceCurrent;
        }

        #endregion

    }

    /// <summary>
    /// A <see cref="Collections.ObjectModel.KeyedCollection{TKey, TItem}">collection</see> of
    /// <see cref="SourceMeasureUnitBase">Source Measure Unit</see>
    /// items keyed by the <see cref="SourceMeasureUnitBase.UniqueKey">unique key.</see>
    /// </summary>
    public class SourceMeasureUnitCollection : SourceMeasureUnitBaseCollection<SourceMeasureUnitBase>
    {
        /// <summary> Gets key for item. </summary>
        /// <param name="item"> The item. </param>
        /// <returns> The key for item. </returns>
        protected override string GetKeyForItem( SourceMeasureUnitBase item )
        {
            return base.GetKeyForItem( item );
        }
    }

    /// <summary>
    /// A <see cref="Collections.ObjectModel.KeyedCollection{TKey, TItem}">collection</see> of
    /// <see cref="SourceMeasureUnitBase">Source Measure Unit (SMU)</see>
    /// items keyed by the <see cref="SourceMeasureUnitBase.UniqueKey">unique key.</see>
    /// </summary>
    public class SourceMeasureUnitBaseCollection<TItem> : System.Collections.ObjectModel.KeyedCollection<string, TItem> where TItem : SourceMeasureUnitBase
    {
        /// <summary> Gets key for item. </summary>
        /// <param name="item"> The item. </param>
        /// <returns> The key for item. </returns>
        protected override string GetKeyForItem( TItem item )
        {
            return item.UniqueKey;
        }
    }

    /// <summary> Collection of source measure unit subsystems. </summary>
    public class SourceMeasureUnitSubsystemCollection : System.Collections.ObjectModel.Collection<SourceMeasureUnitBase>
    {
        /// <summary> The unit number. </summary>
        private string _unitNumber;

        /// <summary> Gets or sets the unit number. </summary>
        /// <value> The unit number. </value>
        public string UnitNumber
        {
            get => this._unitNumber;
            set
            {
                 this._unitNumber = value;
                foreach ( SourceMeasureUnitBase smu in this )
                    smu.UnitNumber = value;
            }
        }

        /// <summary> The node number. </summary>
        private int _nodeNumber;

        /// <summary> Gets or sets the node number. </summary>
        /// <value> The node number. </value>
        public int NodeNumber
        {
            get => this._nodeNumber;
            set
            {
                 this._nodeNumber = value;
                foreach ( SourceMeasureUnitBase smu in this )
                    smu.NodeNumber = value;
            }
        }

        /// <summary>
        /// Adds an item to the <see cref="System.Collections.ICollection" />.
        /// </summary>
        /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        /// <param name="item"> The object to add to the
        /// <see cref="System.Collections.ICollection" />. </param>
        public new void Add( SourceMeasureUnitBase item )
        {
            if ( item is null )
                throw new ArgumentNullException( nameof( item ) );
            item.NodeNumber = this.NodeNumber;
            item.UnitNumber = this.UnitNumber;
            base.Add( item );
        }
    }
}
