namespace cc.isr.VI;

public partial class StatusSubsystemBase
{
    #region " device registers "

    /// <summary> Sets the known preset state. </summary>
    private void PresetRegistersKnownState()
    {
        this.OperationEventEnableBitmask = 0;
        this.QuestionableEventEnableBitmask = 0;
        this.OperationEventMap.Clear();
    }

    /// <summary> Sets the subsystem registers to their reset state. </summary>
    private void ResetRegistersKnownState()
    {
        this.MeasurementEventEnableBitmask = 0;
        this.MeasurementEventStatus = 0;
        this.OperationEventEnableBitmask = 0;
        this.OperationEventStatus = 0;
        this.QuestionableEventEnableBitmask = 0;
        this.QuestionableEventStatus = 0;
        this.Session.StandardEventStatus = new Pith.StandardEvents?();
    }

    /// <summary>
    /// Reads the standard event register and its related registers based on the service and standard
    /// event values.
    /// </summary>
    public virtual void ReadStandardEventRegisters()
    {
        if ( this.Session.HasStandardEvent )
            _ = this.Session.QueryStandardEventStatus();
        if ( this.Session.HasMeasurementEvent )
            _ = this.QueryMeasurementEventStatus();
        if ( (this.Session.ServiceRequestStatus & Pith.ServiceRequests.OperationEvent) != 0 )
            _ = this.QueryOperationEventStatus();
        if ( (this.Session.ServiceRequestStatus & Pith.ServiceRequests.QuestionableEvent) != 0 )
            _ = this.QueryQuestionableEventStatus();
    }

    /// <summary> Define event bitmasks. </summary>
    public virtual void DefineEventBitmasks()
    {
        this.DefineMeasurementEventsBitmasks();
        this.DefineOperationEventsBitmasks();
        this.DefineQuestionableEventsBitmasks();
    }

    #endregion

    #region " measurement register "

    #region " bit definitions "

    /// <summary> Gets or sets the measurement events bitmasks. </summary>
    /// <value> The measurement event bitmasks. </value>
    public MeasurementEventsBitmaskDictionary MeasurementEventsBitmasks { get; private set; }

    /// <summary> Define measurement events bitmasks. </summary>
    protected virtual void DefineMeasurementEventsBitmasks()
    {
        this.MeasurementEventsBitmasks = new MeasurementEventsBitmaskDictionary() { { MeasurementEventBitmaskKey.StatusOnly, 1 << StatusOnlyBit } };
    }

    /// <summary> The questionable bit. </summary>
    public const int QuestionableBit = 31;

    /// <summary> The status only bit. </summary>
    public const int StatusOnlyBit = 30;

    #endregion

    #region " enable bitmask "

    /// <summary> The Measurement event enable bitmask. </summary>

    /// <summary>
    /// Gets or sets the cached value of the Measurement register event enable bit mask.
    /// </summary>
    /// <remarks>
    /// The returned value could be cast to the Measurement events type that is specific to the
    /// instrument The enable register gates the corresponding events for registration by the Status
    /// Byte register. When an event bit is set and the corresponding enable bit is set, the output
    /// (summary) of the register will set to 1, which in turn sets the summary bit of the Status
    /// Byte Register.
    /// </remarks>
    /// <value> The mask to use for enabling the events. </value>
    public int? MeasurementEventEnableBitmask
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.MeasurementEventEnableBitmask, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Programs and reads back the Measurement register events enable bit mask. </summary>
    /// <param name="value"> The value. </param>
    /// <returns> The bit mask or nothing if not known. </returns>
    public int? ApplyMeasurementEventEnableBitmask( int value )
    {
        _ = this.WriteMeasurementEventEnableBitmask( value );
        return this.QueryMeasurementEventEnableBitmask();
    }

    /// <summary> Gets or sets the Measurement event enable query command. </summary>
    /// <remarks> SCPI: ":STAT:MEAS:ENAB?". </remarks>
    /// <value> The Measurement event enable command format. </value>
    protected virtual string MeasurementEventEnableQueryCommand { get; set; } = VI.Syntax.ScpiSyntax.MeasurementEventEnableQueryCommand;

    /// <summary> Queries the Measurement register event enable bit mask. </summary>
    /// <remarks>
    /// The returned value could be cast to the Measurement events type that is specific to the
    /// instrument The enable register gates the corresponding events for registration by the Status
    /// Byte register. When an event bit is set and the corresponding enable bit is set, the output
    /// (summary) of the register will set to 1, which in turn sets the summary bit of the Status
    /// Byte Register.
    /// </remarks>
    /// <returns> The mask to use for enabling the events; nothing if unknown. </returns>
    public int? QueryMeasurementEventEnableBitmask()
    {
        if ( !string.IsNullOrWhiteSpace( this.MeasurementEventEnableQueryCommand ) )
        {
            this.MeasurementEventEnableBitmask = this.Session.Query( 0, this.MeasurementEventEnableQueryCommand );
        }

        return this.MeasurementEventEnableBitmask;
    }

    /// <summary> Gets or sets the Measurement event enable command format. </summary>
    /// <remarks>
    /// SCPI: ":STAT:MEAS:ENAB {0:D}".
    /// <see cref="VI.Syntax.ScpiSyntax.MeasurementEventEnableCommandFormat"> </see>
    /// </remarks>
    /// <value> The Measurement event enable command format. </value>
    protected virtual string MeasurementEventEnableCommandFormat { get; set; } = VI.Syntax.ScpiSyntax.MeasurementEventEnableCommandFormat;

    /// <summary> Programs the Measurement register event enable bit mask. </summary>
    /// <remarks>
    /// When an event bit is set and the corresponding enable bit is set, the output (summary) of the
    /// register will set to 1, which in turn sets the summary bit of the Status Byte Register.
    /// </remarks>
    /// <param name="value"> The value. </param>
    /// <returns> The mask to use for enabling the events; nothing if unknown. </returns>
    public virtual int? WriteMeasurementEventEnableBitmask( int value )
    {
        if ( !string.IsNullOrWhiteSpace( this.MeasurementEventEnableCommandFormat ) )
        {
            _ = this.Session.WriteLine( this.MeasurementEventEnableCommandFormat, value );
        }

        this.MeasurementEventEnableBitmask = value;
        return this.MeasurementEventEnableBitmask;
    }

    #endregion

    #region " positive transition bitmask "

    /// <summary> The Measurement event Positive Transition bitmask. </summary>

    /// <summary>
    /// Gets or sets the cached value of the Measurement register event Positive Transition bit mask.
    /// </summary>
    /// <remarks>
    /// The returned value could be cast to the Measurement events type that is specific to the
    /// instrument The Positive Transition register gates the corresponding events for registration
    /// by the Status Byte register. When an event bit is set and the corresponding Positive
    /// Transition bit is set, the output (summary) of the register will set to 1, which in turn sets
    /// the summary bit of the Status Byte Register.
    /// </remarks>
    /// <value> The mask to use for enabling the events. </value>
    public int? MeasurementEventPositiveTransitionBitmask
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.MeasurementEventPositiveTransitionBitmask, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Programs and reads back the Measurement register events Positive Transition bit mask.
    /// </summary>
    /// <param name="value"> The value. </param>
    /// <returns> The bit mask or nothing if not known. </returns>
    public int? ApplyMeasurementEventPositiveTransitionBitmask( int value )
    {
        _ = this.WriteMeasurementEventPositiveTransitionBitmask( value );
        return this.QueryMeasurementEventPositiveTransitionBitmask();
    }

    /// <summary> Gets or sets the Measurement event Positive Transition query command. </summary>
    /// <remarks> SCPI: ":STAT:MEAS:PTR?". </remarks>
    /// <value> The Measurement event Positive Transition command format. </value>
    protected virtual string MeasurementEventPositiveTransitionQueryCommand { get; set; } = VI.Syntax.ScpiSyntax.MeasurementEventPositiveTransitionQueryCommand;

    /// <summary> Queries the Measurement register event Positive Transition bit mask. </summary>
    /// <remarks>
    /// The returned value could be cast to the Measurement events type that is specific to the
    /// instrument The Positive Transition register gates the corresponding events for registration
    /// by the Status Byte register. When an event bit is set and the corresponding Positive
    /// Transition bit is set, the output (summary) of the register will set to 1, which in turn sets
    /// the summary bit of the Status Byte Register.
    /// </remarks>
    /// <returns> The mask to use for enabling the events; nothing if unknown. </returns>
    public int? QueryMeasurementEventPositiveTransitionBitmask()
    {
        if ( !string.IsNullOrWhiteSpace( this.MeasurementEventPositiveTransitionQueryCommand ) )
        {
            this.MeasurementEventPositiveTransitionBitmask = this.Session.Query( 0, this.MeasurementEventPositiveTransitionQueryCommand );
        }

        return this.MeasurementEventPositiveTransitionBitmask;
    }

    /// <summary> Gets or sets the Measurement event Positive Transition command format. </summary>
    /// <remarks>
    /// SCPI: ":STAT:MEAS:PTR {0:D}".
    /// <see cref="VI.Syntax.ScpiSyntax.MeasurementEventPositiveTransitionCommandFormat"> </see>
    /// </remarks>
    /// <value> The Measurement event Positive Transition command format. </value>
    protected virtual string MeasurementEventPositiveTransitionCommandFormat { get; set; } = VI.Syntax.ScpiSyntax.MeasurementEventPositiveTransitionCommandFormat;

    /// <summary> Programs the Measurement register event Positive Transition bit mask. </summary>
    /// <remarks>
    /// When an event bit is set and the corresponding Positive Transition bit is set, the output
    /// (summary) of the register will set to 1, which in turn sets the summary bit of the Status
    /// Byte Register.
    /// </remarks>
    /// <param name="value"> The value. </param>
    /// <returns> The mask to use for enabling the events; nothing if unknown. </returns>
    public virtual int? WriteMeasurementEventPositiveTransitionBitmask( int value )
    {
        if ( !string.IsNullOrWhiteSpace( this.MeasurementEventPositiveTransitionCommandFormat ) )
        {
            _ = this.Session.WriteLine( this.MeasurementEventPositiveTransitionCommandFormat, value );
        }

        this.MeasurementEventPositiveTransitionBitmask = value;
        return this.MeasurementEventPositiveTransitionBitmask;
    }

    #endregion

    #region " negative transition bitmask "

    /// <summary> The Measurement event Negative Transition bitmask. </summary>

    /// <summary>
    /// Gets or sets the cached value of the Measurement register event Negative Transition bit mask.
    /// </summary>
    /// <remarks>
    /// The returned value could be cast to the Measurement events type that is specific to the
    /// instrument The Negative Transition register gates the corresponding events for registration
    /// by the Status Byte register. When an event bit is set and the corresponding Negative
    /// Transition bit is set, the output (summary) of the register will set to 1, which in turn sets
    /// the summary bit of the Status Byte Register.
    /// </remarks>
    /// <value> The mask to use for enabling the events. </value>
    public int? MeasurementEventNegativeTransitionBitmask
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.MeasurementEventNegativeTransitionBitmask, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Programs and reads back the Measurement register events Negative Transition bit mask.
    /// </summary>
    /// <param name="value"> The value. </param>
    /// <returns> The bit mask or nothing if not known. </returns>
    public int? ApplyMeasurementEventNegativeTransitionBitmask( int value )
    {
        _ = this.WriteMeasurementEventNegativeTransitionBitmask( value );
        return this.QueryMeasurementEventNegativeTransitionBitmask();
    }

    /// <summary> Gets or sets the Measurement event Negative Transition query command. </summary>
    /// <remarks> SCPI: ":STAT:MEAS:NTR?". </remarks>
    /// <value> The Measurement event Negative Transition command format. </value>
    protected virtual string MeasurementEventNegativeTransitionQueryCommand { get; set; } = VI.Syntax.ScpiSyntax.MeasurementEventNegativeTransitionQueryCommand;

    /// <summary> Queries the Measurement register event Negative Transition bit mask. </summary>
    /// <returns> The mask to use for enabling the events; nothing if unknown. </returns>
    public int? QueryMeasurementEventNegativeTransitionBitmask()
    {
        if ( !string.IsNullOrWhiteSpace( this.MeasurementEventNegativeTransitionQueryCommand ) )
        {
            this.MeasurementEventNegativeTransitionBitmask = this.Session.Query( 0, this.MeasurementEventNegativeTransitionQueryCommand );
        }

        return this.MeasurementEventNegativeTransitionBitmask;
    }

    /// <summary> Gets or sets the Measurement event Negative Transition command format. </summary>
    /// <remarks>
    /// SCPI: ":STAT:MEAS:NTR {0:D}".
    /// <see cref="VI.Syntax.ScpiSyntax.MeasurementEventNegativeTransitionCommandFormat"> </see>
    /// </remarks>
    /// <value> The Measurement event Negative Transition command format. </value>
    protected virtual string MeasurementEventNegativeTransitionCommandFormat { get; set; } = VI.Syntax.ScpiSyntax.MeasurementEventNegativeTransitionCommandFormat;

    /// <summary> Programs the Measurement register event Negative Transition bit mask. </summary>
    /// <remarks>
    /// When an event bit is set and the corresponding Negative Transition bit is set, the output
    /// (summary) of the register will set to 1, which in turn sets the summary bit of the Status
    /// Byte Register.
    /// </remarks>
    /// <param name="value"> The value. </param>
    /// <returns> The mask to use for enabling the events; nothing if unknown. </returns>
    public virtual int? WriteMeasurementEventNegativeTransitionBitmask( int value )
    {
        if ( !string.IsNullOrWhiteSpace( this.MeasurementEventNegativeTransitionCommandFormat ) )
        {
            _ = this.Session.WriteLine( this.MeasurementEventNegativeTransitionCommandFormat, value );
        }

        this.MeasurementEventNegativeTransitionBitmask = value;
        return this.MeasurementEventNegativeTransitionBitmask;
    }

    #endregion

    #region " condition "

    /// <summary> The measurement event Condition. </summary>

    /// <summary> Gets or sets the cached Condition of the measurement register events. </summary>
    /// <value> <c>null</c> if value is not known;. </value>
    public int? MeasurementEventCondition
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.MeasurementEventCondition, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the measurement event condition query command. </summary>
    /// <remarks>
    /// SCPI: ":STAT:MEAS:COND?".
    /// <see cref="VI.Syntax.ScpiSyntax.MeasurementEventConditionQueryCommand"> </see>
    /// </remarks>
    /// <value> The measurement event condition query command. </value>
    protected virtual string MeasurementEventConditionQueryCommand { get; set; } = VI.Syntax.ScpiSyntax.MeasurementEventConditionQueryCommand;

    /// <summary> Reads the condition of the measurement register event. </summary>
    /// <returns> System.Nullable{System.Int32}. </returns>
    public virtual int? QueryMeasurementEventCondition()
    {
        if ( !string.IsNullOrWhiteSpace( this.MeasurementEventConditionQueryCommand ) )
        {
            this.MeasurementEventCondition = this.Session.Query( 0, this.MeasurementEventConditionQueryCommand );
        }

        return this.MeasurementEventCondition;
    }

    #endregion

    #region " status "

    /// <summary> The measurement event status. </summary>

    /// <summary> Gets or sets the cached status of the measurement register events. </summary>
    /// <value> <c>null</c> if value is not known;. </value>
    public int? MeasurementEventStatus
    {
        get;

        protected set
        {
            field = value;
            this.NotifyPropertyChanged();
        }
    }

    /// <summary> Gets or sets the measurement event query command. </summary>
    /// <remarks>
    /// SCPI: ":STAT:MEAS:EVEN?".
    /// <see cref="VI.Syntax.ScpiSyntax.MeasurementEventQueryCommand"> </see>
    /// </remarks>
    /// <value> The measurement event query command. </value>
    protected virtual string MeasurementStatusQueryCommand { get; set; } = VI.Syntax.ScpiSyntax.MeasurementEventQueryCommand;

    /// <summary> Reads the status of the measurement register events. </summary>
    /// <returns> System.Nullable{System.Int32}. </returns>
    public virtual int? QueryMeasurementEventStatus()
    {
        if ( !string.IsNullOrWhiteSpace( this.MeasurementStatusQueryCommand ) )
        {
            this.MeasurementEventStatus = this.Session.Query( 0, this.MeasurementStatusQueryCommand );
        }

        return this.MeasurementEventStatus;
    }

    #endregion

    #endregion

    #region " map event numbers "

    /// <summary> Gets or sets the buffer 0% filled event number. </summary>
    /// <value> The buffer empty event number. </value>
    public int BufferEmptyEventNumber { get; set; } = 4916;

    /// <summary> Gets or sets the buffer 100% full event number. </summary>
    /// <value> The buffer full event number. </value>
    public int BufferFullEventNumber { get; set; } = 4917;

    #endregion

    #region " operation register "

    #region " bit definitions "

    /// <summary> Gets or sets the Operation events bitmasks. </summary>
    /// <value> The Operation event bitmasks. </value>
    public OperationEventsBitmaskDictionary OperationEventsBitmasks { get; private set; }

    /// <summary> Define operation events bitmasks. </summary>
    protected virtual void DefineOperationEventsBitmasks()
    {
        this.OperationEventsBitmasks = [];
    }

    #endregion

    #region " enable bitmask "

    /// <summary> The operation event enable bitmask. </summary>

    /// <summary>
    /// Gets or sets the cached value of the Operation register event enable bit mask.
    /// </summary>
    /// <remarks>
    /// The returned value could be cast to the Operation events type that is specific to the
    /// instrument The enable register gates the corresponding events for registration by the Status
    /// Byte register. When an event bit is set and the corresponding enable bit is set, the output
    /// (summary) of the register will set to 1, which in turn sets the summary bit of the Status
    /// Byte Register.
    /// </remarks>
    /// <value> The mask to use for enabling the events. </value>
    public int? OperationEventEnableBitmask
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.OperationEventEnableBitmask, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Programs and reads back the Operation register events enable bit mask. </summary>
    /// <param name="value"> The value. </param>
    /// <returns> The bit mask or nothing if not known. </returns>
    public int? ApplyOperationEventEnableBitmask( int value )
    {
        _ = this.WriteOperationEventEnableBitmask( value );
        return this.QueryOperationEventEnableBitmask();
    }

    /// <summary> Gets or sets the operation event enable query command. </summary>
    /// <remarks> SCPI: ":STAT:OPER:ENAB?". </remarks>
    /// <value> The operation event enable command format. </value>
    protected virtual string OperationEventEnableQueryCommand { get; set; } = VI.Syntax.ScpiSyntax.OperationEventEnableQueryCommand;

    /// <summary> Queries the Operation register event enable bit mask. </summary>
    /// <remarks>
    /// The returned value could be cast to the Operation events type that is specific to the
    /// instrument The enable register gates the corresponding events for registration by the Status
    /// Byte register. When an event bit is set and the corresponding enable bit is set, the output
    /// (summary) of the register will set to 1, which in turn sets the summary bit of the Status
    /// Byte Register.
    /// </remarks>
    /// <returns> The mask to use for enabling the events; nothing if unknown. </returns>
    public int? QueryOperationEventEnableBitmask()
    {
        if ( !string.IsNullOrWhiteSpace( this.OperationEventEnableQueryCommand ) )
        {
            this.OperationEventEnableBitmask = this.Session.Query( 0, this.OperationEventEnableQueryCommand );
        }

        return this.OperationEventEnableBitmask;
    }

    /// <summary> Gets or sets the operation event enable command format. </summary>
    /// <remarks>
    /// SCPI: ":STAT:OPER:ENAB {0:D}".
    /// <see cref="VI.Syntax.ScpiSyntax.OperationEventEnableCommandFormat"> </see>
    /// </remarks>
    /// <value> The operation event enable command format. </value>
    protected virtual string OperationEventEnableCommandFormat { get; set; } = VI.Syntax.ScpiSyntax.OperationEventEnableCommandFormat;

    /// <summary> Programs the Operation register event enable bit mask. </summary>
    /// <remarks>
    /// When an event bit is set and the corresponding enable bit is set, the output (summary) of the
    /// register will set to 1, which in turn sets the summary bit of the Status Byte Register.
    /// </remarks>
    /// <param name="value"> The value. </param>
    /// <returns> The mask to use for enabling the events; nothing if unknown. </returns>
    public virtual int? WriteOperationEventEnableBitmask( int value )
    {
        if ( !string.IsNullOrWhiteSpace( this.OperationEventEnableCommandFormat ) )
        {
            _ = this.Session.WriteLine( this.OperationEventEnableCommandFormat, value );
        }

        this.OperationEventEnableBitmask = value;
        return this.OperationEventEnableBitmask;
    }

    #endregion

    #region " negative transition event enable bitmask "

    /// <summary> The operation register negative transition events enable bitmask. </summary>

    /// <summary> Gets or sets the operation negative transition event request. </summary>
    /// <remarks>
    /// At this time this reads back the event status rather than the enable status. The returned
    /// value could be cast to the Operation Transition events type that is specific to the
    /// instrument.
    /// </remarks>
    /// <value> The Operation Transition Events mask to use for enabling the events. </value>
    public int? OperationNegativeTransitionEventEnableBitmask
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.OperationNegativeTransitionEventEnableBitmask, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Queries the operation register negative transition events enable bit mask. </summary>
    /// <remarks>
    /// The returned value could be cast to the Operation events type that is specific to the
    /// instrument The enable register gates the corresponding events for registration by the Status
    /// Byte register. When an event bit is set and the corresponding enable bit is set, the output
    /// (summary) of the register will set to 1, which in turn sets the summary bit of the Status
    /// Byte Register.
    /// </remarks>
    /// <returns> The mask to use for enabling the events; nothing if unknown. </returns>
    public virtual int? QueryOperationNegativeTransitionEventEnableBitmask()
    {
        this.OperationNegativeTransitionEventEnableBitmask = this.Session.Query( this.OperationNegativeTransitionEventEnableBitmask.GetValueOrDefault( 0 ), ":STAT:OPER:NTR?" );
        return this.OperationNegativeTransitionEventEnableBitmask;
    }

    /// <summary>
    /// Programs and reads back the operation register negative transition events enable bit mask.
    /// </summary>
    /// <param name="value"> The value. </param>
    /// <returns> The bit mask or nothing if not known. </returns>
    public int? ApplyOperationNegativeTransitionEventEnableBitmask( int value )
    {
        _ = this.WriteOperationNegativeTransitionEventEnableBitmask( value );
        return this.QueryOperationNegativeTransitionEventEnableBitmask();
    }

    /// <summary>
    /// Programs the operation register negative transition events enable bit mask.
    /// </summary>
    /// <remarks>
    /// When an event bit is set and the corresponding enable bit is set, the output (summary) of the
    /// register will set to 1, which in turn sets the summary bit of the Status Byte Register.
    /// </remarks>
    /// <param name="value"> The value. </param>
    /// <returns> The mask to use for enabling the events; nothing if unknown. </returns>
    public virtual int? WriteOperationNegativeTransitionEventEnableBitmask( int value )
    {
        _ = this.Session.WriteLine( ":STAT:OPER:NTR {0:D}", value );
        this.OperationNegativeTransitionEventEnableBitmask = value;
        return this.OperationNegativeTransitionEventEnableBitmask;
    }

    #endregion

    #region " positive transition event enable bitmask "

    /// <summary> The operation register Positive transition events enable bitmask. </summary>

    /// <summary>
    /// Gets or sets the cached value of the operation Positive transition event request.
    /// </summary>
    /// <remarks>
    /// At this time this reads back the event status rather than the enable status. The returned
    /// value could be cast to the Operation Transition events type that is specific to the
    /// instrument.
    /// </remarks>
    /// <value> The Operation Transition Events mask to use for enabling the events. </value>
    public int? OperationPositiveTransitionEventEnableBitmask
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.OperationPositiveTransitionEventEnableBitmask, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Queries the operation register Positive transition events enable bit mask. </summary>
    /// <remarks>
    /// The returned value could be cast to the Operation events type that is specific to the
    /// instrument The enable register gates the corresponding events for registration by the Status
    /// Byte register. When an event bit is set and the corresponding enable bit is set, the output
    /// (summary) of the register will set to 1, which in turn sets the summary bit of the Status
    /// Byte Register.
    /// </remarks>
    /// <returns> The mask to use for enabling the events; nothing if unknown. </returns>
    public virtual int? QueryOperationPositiveTransitionEventEnableBitmask()
    {
        this.OperationPositiveTransitionEventEnableBitmask = this.Session.Query( this.OperationPositiveTransitionEventEnableBitmask.GetValueOrDefault( 0 ), ":STAT:OPER:PTR?" );
        return this.OperationPositiveTransitionEventEnableBitmask;
    }

    /// <summary>
    /// Programs and reads back the operation register Positive transition events enable bit mask.
    /// </summary>
    /// <param name="value"> The value. </param>
    /// <returns> The bit mask or nothing if not known. </returns>
    public int? ApplyOperationPositiveTransitionEventEnableBitmask( int value )
    {
        _ = this.WriteOperationPositiveTransitionEventEnableBitmask( value );
        return this.QueryOperationPositiveTransitionEventEnableBitmask();
    }

    /// <summary>
    /// Programs the operation register Positive transition events enable bit mask.
    /// </summary>
    /// <remarks>
    /// When an event bit is set and the corresponding enable bit is set, the output (summary) of the
    /// register will set to 1, which in turn sets the summary bit of the Status Byte Register.
    /// </remarks>
    /// <param name="value"> The value. </param>
    /// <returns> The mask to use for enabling the events; nothing if unknown. </returns>
    public virtual int? WriteOperationPositiveTransitionEventEnableBitmask( int value )
    {
        _ = this.Session.WriteLine( ":STAT:OPER:PTR {0:D}", value );
        this.OperationPositiveTransitionEventEnableBitmask = value;
        return this.OperationPositiveTransitionEventEnableBitmask;
    }

    #endregion

    #region " condition "

    /// <summary> The operation event Condition. </summary>

    /// <summary>
    /// Gets or sets the Operation event register cached condition of the instrument.
    /// </summary>
    /// <remarks>
    /// This value reflects the cached condition of the instrument. The returned value could be cast
    /// to the Operation events type that is specific to the instrument The condition register is a
    /// real-time, read-only register that constantly updates to reflect the present operating
    /// conditions of the instrument.
    /// </remarks>
    /// <value> The operation event condition. </value>
    public int? OperationEventCondition
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.OperationEventCondition, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the operation event enable query command. </summary>
    /// <remarks> SCPI: ":STAT:OPER:ENAB?". </remarks>
    /// <value> The operation event enable command format. </value>
    protected virtual string OperationEventConditionQueryCommand { get; set; } = VI.Syntax.ScpiSyntax.OperationEventConditionQueryCommand;

    /// <summary> Queries the Operation event register condition from the instrument. </summary>
    /// <remarks>
    /// This value reflects the real time condition of the instrument. The returned value could be
    /// cast to the Operation events type that is specific to the instrument The condition register
    /// is a real-time, read-only register that constantly updates to reflect the present operating
    /// conditions of the instrument.
    /// </remarks>
    /// <returns> The operation event condition. </returns>
    public virtual int? QueryOperationEventCondition()
    {
        if ( !string.IsNullOrWhiteSpace( this.OperationEventConditionQueryCommand ) )
        {
            this.OperationEventCondition = this.Session.Query( this.OperationEventCondition.GetValueOrDefault( 0 ), this.OperationEventConditionQueryCommand );
        }

        return this.OperationEventCondition;
    }

    #endregion

    #region " status "

    /// <summary> The operation event status. </summary>

    /// <summary> Gets or sets the cached status of the Operation Register events. </summary>
    /// <remarks>
    /// This query commands Queries the contents of the status event register. This value indicates
    /// which bits are set. An event register bit is set to 1 when its event occurs. The bit remains
    /// latched to 1 until the register is reset. Reading an event register clears the bits of that
    /// register. *CLS resets all four event registers.
    /// </remarks>
    /// <value> <c>null</c> if value is not known;. </value>
    public int? OperationEventStatus
    {
        get;

        protected set
        {
            field = value;
            this.NotifyPropertyChanged();
        }
    }

    /// <summary> Gets or sets the operation event status query command. </summary>
    /// <remarks>
    /// SCPI: ":STAT:OPER:EVEN?".
    /// <see cref="VI.Syntax.ScpiSyntax.OperationEventQueryCommand"> </see>
    /// </remarks>
    /// <value> The operation event status query command. </value>
    protected virtual string OperationEventStatusQueryCommand { get; set; } = VI.Syntax.ScpiSyntax.OperationEventQueryCommand;

    /// <summary> Queries the status of the Operation Register events. </summary>
    /// <remarks>
    /// This query commands Queries the contents of the status event register. This value indicates
    /// which bits are set. An event register bit is set to 1 when its event occurs. The bit remains
    /// latched to 1 until the register is reset. Reading an event register clears the bits of that
    /// register. *CLS resets all four event registers.
    /// </remarks>
    /// <returns> <c>null</c> if value is not known;. </returns>
    public virtual int? QueryOperationEventStatus()
    {
        if ( !string.IsNullOrWhiteSpace( this.OperationEventStatusQueryCommand ) )
        {
            this.OperationEventStatus = this.Session.Query( 0, this.OperationEventStatusQueryCommand );
        }

        return this.OperationEventStatus;
    }
    #endregion

    #region " map "

    /// <summary> The operation event Map. </summary>
    /// <value> The operation event map. </value>
    private IDictionary<int, string> OperationEventMap { get; set; }

    /// <summary> Gets the cached Map of the Operation Register Map events. </summary>
    /// <remarks> David, 2020-11-09. </remarks>
    /// <param name="bitNumber"> The bit number. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string OperationEventMapGetter( int bitNumber )
    {
        return this.OperationEventMap.ContainsKey( bitNumber ) ? this.OperationEventMap[bitNumber] : string.Empty;
    }

    /// <summary> Sets the cached Map of the Operation Register Map events. </summary>
    /// <remarks> David, 2020-11-09. </remarks>
    /// <param name="bitNumber"> The bit number. </param>
    /// <param name="value">     The value. </param>
    public void OperationEventMapSetter( int bitNumber, string value )
    {
        if ( this.OperationEventMap.ContainsKey( bitNumber ) )
        {
            this.OperationEventMap[bitNumber] = value;
        }
        else
        {
            this.OperationEventMap.Add( bitNumber, value );
        }

        this.NotifyPropertyChanged();
    }

    /// <summary> Programs and reads back the Operation register events Map. </summary>
    /// <param name="bitNumber">        The bit number. </param>
    /// <param name="setEventNumber">   The set event number. </param>
    /// <param name="clearEventNumber"> The clear event number. </param>
    /// <returns> The event map or nothing if not known. </returns>
    public string ApplyOperationEventMap( int bitNumber, int setEventNumber, int clearEventNumber )
    {
        _ = this.WriteOperationEventMap( bitNumber, setEventNumber, clearEventNumber );
        return this.QueryOperationEventMap( bitNumber );
    }

    /// <summary> Gets or sets the operation event Map query command. </summary>
    /// <remarks>
    /// SCPI: ":STAT:OPER:MAP? {0:D}".
    /// <see cref="VI.Syntax.ScpiSyntax.OperationEventQueryCommand"> </see>
    /// </remarks>
    /// <value> The operation event Map query command. </value>
    protected virtual string OperationEventMapQueryCommand { get; set; } = VI.Syntax.ScpiSyntax.OperationEventMapQueryCommandFormat;

    /// <summary> Queries the Map of the Operation Register events. </summary>
    /// <remarks>
    /// This query commands Queries the contents of the Map event register. This value indicates
    /// which bits are set. An event register bit is set to 1 when its event occurs. The bit remains
    /// latched to 1 until the register is reset. Reading an event register clears the bits of that
    /// register. *CLS resets all four event registers.
    /// </remarks>
    /// <param name="bitNumber"> The bit number. </param>
    /// <returns> The event map or nothing if not known. </returns>
    public virtual string QueryOperationEventMap( int bitNumber )
    {
        if ( !string.IsNullOrWhiteSpace( this.OperationEventMapQueryCommand ) )
        {
            this.OperationEventMap[bitNumber] = this.Session.QueryTrimEnd( string.Format( this.OperationEventMapQueryCommand, bitNumber ) );
        }

        return this.OperationEventMap[bitNumber];
    }

    /// <summary> Gets or sets the operation event Map command format. </summary>
    /// <remarks>
    /// SCPI: ":STAT:OPER:MAP {0:D},{1:D},{2:D}".
    /// <see cref="VI.Syntax.ScpiSyntax.OperationEventMapCommandFormat"> </see>
    /// </remarks>
    /// <value> The operation event Map command format. </value>
    protected virtual string OperationEventMapCommandFormat { get; set; } = VI.Syntax.ScpiSyntax.OperationEventMapCommandFormat;

    /// <summary> Programs the Operation register event Map bit mask. </summary>
    /// <remarks>
    /// When an event bit is set and the corresponding Map bit is set, the output (summary) of the
    /// register will set to 1, which in turn sets the summary bit of the Status Byte Register.
    /// </remarks>
    /// <param name="bitNumber">        The bit number. </param>
    /// <param name="setEventNumber">   The set event number. </param>
    /// <param name="clearEventNumber"> The clear event number. </param>
    /// <returns> The event map or nothing if not known. </returns>
    public virtual string WriteOperationEventMap( int bitNumber, int setEventNumber, int clearEventNumber )
    {
        if ( !string.IsNullOrWhiteSpace( this.OperationEventMapCommandFormat ) )
        {
            _ = this.Session.WriteLine( this.OperationEventMapCommandFormat, bitNumber, setEventNumber, clearEventNumber );
        }

        this.OperationEventMap[bitNumber] = $"{setEventNumber},{clearEventNumber}";
        return this.OperationEventMap[bitNumber];
    }

    #endregion

    #endregion

    #region " questionable register "

    #region " bit definitions "

    /// <summary> Gets or sets the Questionable events bitmasks. </summary>
    /// <value> The Questionable event bitmasks. </value>
    public QuestionableEventsBitmaskDictionary QuestionableEventsBitmasks { get; private set; }

    /// <summary> Define Questionable events bitmasks. </summary>
    protected virtual void DefineQuestionableEventsBitmasks()
    {
        this.QuestionableEventsBitmasks = [];
    }

    #endregion

    #region " bitmask "

    /// <summary> The questionable event enable bitmask. </summary>

    /// <summary>
    /// Gets or sets the cached value of the Questionable register event enable bit mask.
    /// </summary>
    /// <remarks>
    /// The returned value could be cast to the Questionable events type that is specific to the
    /// instrument The enable register gates the corresponding events for registration by the Status
    /// Byte register. When an event bit is set and the corresponding enable bit is set, the output
    /// (summary) of the register will set to 1, which in turn sets the summary bit of the Status
    /// Byte Register.
    /// </remarks>
    /// <value> The mask to use for enabling the events. </value>
    public int? QuestionableEventEnableBitmask
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.QuestionableEventEnableBitmask, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Queries the status of the Questionable Register events. </summary>
    /// <remarks>
    /// This query commands Queries the contents of the status event register. This value indicates
    /// which bits are set. An event register bit is set to 1 when its event occurs. The bit remains
    /// latched to 1 until the register is reset. Reading an event register clears the bits of that
    /// register. *CLS resets all four event registers.
    /// </remarks>
    /// <returns> <c>null</c> if value is not known;. </returns>
    public virtual int? QueryQuestionableEventEnableBitmask()
    {
        this.QuestionableEventEnableBitmask = this.Session.Query( this.QuestionableEventEnableBitmask.GetValueOrDefault( 0 ), ":STAT:QUES:ENAB?" );
        return this.QuestionableEventEnableBitmask;
    }

    /// <summary> Programs and reads back the Questionable register events enable bit mask. </summary>
    /// <param name="value"> The value. </param>
    /// <returns> The bit mask or nothing if not known. </returns>
    public int? ApplyQuestionableEventEnableBitmask( int value )
    {
        _ = this.WriteQuestionableEventEnableBitmask( value );
        return this.QueryQuestionableEventEnableBitmask();
    }

    /// <summary>
    /// Writes the Questionable register events enable bit mask without updating the value from the
    /// device.
    /// </summary>
    /// <param name="value"> The value. </param>
    /// <returns> The bit mask or nothing if not known. </returns>
    public virtual int? WriteQuestionableEventEnableBitmask( int value )
    {
        _ = this.Session.WriteLine( ":STAT:QUES:ENAB {0:D}", value );
        this.QuestionableEventEnableBitmask = value;
        return this.QuestionableEventEnableBitmask;
    }

    #endregion

    #region " condition "

    /// <summary> The questionable event Condition. </summary>

    /// <summary>
    /// Gets or sets the cached value of the Questionable event register condition of the instrument.
    /// </summary>
    /// <remarks>
    /// This value reflects the real time condition of the instrument. The returned value could be
    /// cast to the Questionable events type that is specific to the instrument The condition
    /// register is a real-time, read-only register that constantly updates to reflect the present
    /// operating conditions of the instrument.
    /// </remarks>
    /// <value> The questionable event condition. </value>
    public int? QuestionableEventCondition
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.QuestionableEventCondition, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets the Questionable event register condition from the instrument. </summary>
    /// <remarks>
    /// This value reflects the real time condition of the instrument. The returned value could be
    /// cast to the Questionable events type that is specific to the instrument The condition
    /// register is a real-time, read-only register that constantly updates to reflect the present
    /// operating conditions of the instrument.
    /// </remarks>
    /// <returns> The questionable event condition. </returns>
    public virtual int? QueryQuestionableEventCondition()
    {
        this.QuestionableEventCondition = this.Session.Query( this.QuestionableEventCondition.GetValueOrDefault( 0 ), ":STAT:QUES:COND?" );
        return this.QuestionableEventCondition;
    }

    #endregion

    #region " status "

    /// <summary> The questionable event status. </summary>

    /// <summary> Gets or sets the status of the Questionable register events. </summary>
    /// <value> <c>null</c> if value is not known;. </value>
    public int? QuestionableEventStatus
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.QuestionableEventStatus, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the questionable status query command. </summary>
    /// <remarks>
    /// SCPI: ":STAT:QUES:EVEN?"
    /// <see cref="VI.Syntax.ScpiSyntax.QuestionableEventQueryCommand"> </see>
    /// </remarks>
    /// <value> The questionable status query command. </value>
    protected virtual string QuestionableStatusQueryCommand { get; set; } = VI.Syntax.ScpiSyntax.QuestionableEventQueryCommand;

    /// <summary> Reads the status of the Questionable register events. </summary>
    /// <returns> System.Nullable{System.Int32}. </returns>
    public virtual int? QueryQuestionableEventStatus()
    {
        if ( !string.IsNullOrWhiteSpace( this.QuestionableStatusQueryCommand ) )
        {
            this.QuestionableEventStatus = this.Session.Query( 0, this.QuestionableStatusQueryCommand );
        }

        return this.QuestionableEventStatus;
    }

    #endregion

    #region " map "

    /// <summary> The Questionable event Map. </summary>
    /// <value> The Questionable event map. </value>
    private IDictionary<int, string> QuestionableEventMap { get; set; }

    /// <summary> Gets the cached Map of the Questionable Register Map events. </summary>
    /// <remarks> David, 2020-11-09. </remarks>
    /// <param name="bitNumber"> The bit number. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string QuestionableEventMapGetter( int bitNumber )
    {
        return this.QuestionableEventMap.ContainsKey( bitNumber ) ? this.QuestionableEventMap[bitNumber] : string.Empty;
    }

    /// <summary> Sets the cached Map of the Questionable Register Map events. </summary>
    /// <remarks> David, 2020-11-09. </remarks>
    /// <param name="bitNumber"> The bit number. </param>
    /// <param name="value">     The value. </param>
    public void QuestionableEventMapSetter( int bitNumber, string value )
    {
        if ( this.QuestionableEventMap.ContainsKey( bitNumber ) )
        {
            this.QuestionableEventMap[bitNumber] = value;
        }
        else
        {
            this.QuestionableEventMap.Add( bitNumber, value );
        }

        this.NotifyPropertyChanged();
    }

    /// <summary> Programs and reads back the Questionable register events Map. </summary>
    /// <param name="bitNumber">        The bit number. </param>
    /// <param name="setEventNumber">   The set event number. </param>
    /// <param name="clearEventNumber"> The clear event number. </param>
    /// <returns> The event map or nothing if not known. </returns>
    public string ApplyQuestionableEventMap( int bitNumber, int setEventNumber, int clearEventNumber )
    {
        _ = this.WriteQuestionableEventMap( bitNumber, setEventNumber, clearEventNumber );
        return this.QueryQuestionableEventMap( bitNumber );
    }

    /// <summary> Gets or sets the Questionable event Map query command. </summary>
    /// <remarks>
    /// SCPI: ":STAT:QUES:MAP? {0:D}".
    /// <see cref="VI.Syntax.ScpiSyntax.QuestionableEventQueryCommand"> </see>
    /// </remarks>
    /// <value> The Questionable event Map query command. </value>
    protected virtual string QuestionableEventMapQueryCommand { get; set; } = VI.Syntax.ScpiSyntax.QuestionableEventMapQueryCommandFormat;

    /// <summary> Queries the Map of the Questionable Register events. </summary>
    /// <remarks>
    /// This query commands Queries the contents of the Map event register. This value indicates
    /// which bits are set. An event register bit is set to 1 when its event occurs. The bit remains
    /// latched to 1 until the register is reset. Reading an event register clears the bits of that
    /// register. *CLS resets all four event registers.
    /// </remarks>
    /// <param name="bitNumber"> The bit number. </param>
    /// <returns> The event map or nothing if not known. </returns>
    public virtual string QueryQuestionableEventMap( int bitNumber )
    {
        if ( !string.IsNullOrWhiteSpace( this.QuestionableEventMapQueryCommand ) )
        {
            this.QuestionableEventMap[bitNumber] = this.Session.QueryTrimEnd( string.Format( this.QuestionableEventMapQueryCommand, bitNumber ) );
        }

        return this.QuestionableEventMap[bitNumber];
    }

    /// <summary> Gets or sets the Questionable event Map command format. </summary>
    /// <remarks>
    /// SCPI: ":STAT:QUES:MAP {0:D},{1:D},{2:D}".
    /// <see cref="VI.Syntax.ScpiSyntax.QuestionableEventMapCommandFormat"> </see>
    /// </remarks>
    /// <value> The Questionable event Map command format. </value>
    protected virtual string QuestionableEventMapCommandFormat { get; set; } = VI.Syntax.ScpiSyntax.QuestionableEventMapCommandFormat;

    /// <summary> Programs the Questionable register event Map bit mask. </summary>
    /// <remarks>
    /// When an event bit is set and the corresponding Map bit is set, the output (summary) of the
    /// register will set to 1, which in turn sets the summary bit of the Status Byte Register.
    /// </remarks>
    /// <param name="bitNumber">        The bit number. </param>
    /// <param name="setEventNumber">   The set event number. </param>
    /// <param name="clearEventNumber"> The clear event number. </param>
    /// <returns> The event map or nothing if not known. </returns>
    public virtual string WriteQuestionableEventMap( int bitNumber, int setEventNumber, int clearEventNumber )
    {
        if ( !string.IsNullOrWhiteSpace( this.QuestionableEventMapCommandFormat ) )
        {
            _ = this.Session.WriteLine( this.QuestionableEventMapCommandFormat, bitNumber, setEventNumber, clearEventNumber );
        }

        this.QuestionableEventMap[bitNumber] = $"{setEventNumber},{clearEventNumber}";
        return this.QuestionableEventMap[bitNumber];
    }

    #endregion

    #endregion

    #region " service request "

    /// <summary>
    /// Enables or disables the operation service status registers requesting a service request upon
    /// a negative transition.
    /// </summary>
    /// <param name="isOn">                     if set to <c>true</c> [is on]. </param>
    /// <param name="usingNegativeTransitions"> True to using negative transitions. </param>
    public void ToggleEndOfScanService( bool isOn, bool usingNegativeTransitions )
    {
        this.ToggleEndOfScanService( isOn, usingNegativeTransitions, this.Session.OperationServiceRequestEnableBitmask );
    }

    /// <summary>
    /// Enables or disables the operation service status registers requesting a service request upon
    /// a negative transition.
    /// </summary>
    /// <param name="turnOn">                   True to turn on or false to turn off the service
    /// request. </param>
    /// <param name="usingNegativeTransitions"> True to using negative transitions. </param>
    /// <param name="serviceRequestMask">       Specifies the
    /// <see cref="VI.Pith.ServiceRequests">service request
    /// flags</see> </param>
    public void ToggleEndOfScanService( bool turnOn, bool usingNegativeTransitions, Pith.ServiceRequests serviceRequestMask )
    {
        if ( turnOn )
        {
            if ( usingNegativeTransitions )
            {
                _ = this.ApplyOperationNegativeTransitionEventEnableBitmask( this.OperationEventsBitmasks[( int ) OperationEventBitmaskKey.Setting] );
                _ = this.ApplyOperationPositiveTransitionEventEnableBitmask( 0 );
            }
            else
            {
                _ = this.ApplyOperationNegativeTransitionEventEnableBitmask( 0 );
                _ = this.ApplyOperationPositiveTransitionEventEnableBitmask( this.OperationEventsBitmasks[( int ) OperationEventBitmaskKey.Setting] );
            }

            _ = this.ApplyOperationEventEnableBitmask( this.OperationEventsBitmasks[( int ) OperationEventBitmaskKey.Setting] );
        }
        else
        {
            _ = this.ApplyOperationNegativeTransitionEventEnableBitmask( 0 );
            _ = this.ApplyOperationPositiveTransitionEventEnableBitmask( 0 );
            _ = this.ApplyOperationEventEnableBitmask( 0 );
        }

        this.ToggleServiceRequest( turnOn, serviceRequestMask );
    }

    /// <summary> Enables or disables service requests. </summary>
    /// <param name="turnOn">             True to turn on or false to turn off the service request. </param>
    /// <param name="serviceRequestMask"> Specifies the
    /// <see cref="VI.Pith.ServiceRequests">service request
    /// flags</see> </param>
    public void ToggleServiceRequest( bool turnOn, Pith.ServiceRequests serviceRequestMask )
    {
        this.Session.ApplyStatusByteEnableBitmask( ( int ) (turnOn ? serviceRequestMask : Pith.ServiceRequests.None) );
    }

    #endregion
}
