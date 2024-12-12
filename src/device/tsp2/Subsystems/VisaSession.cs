using System;
using System.Diagnostics;
using cc.isr.Std.TimeSpanExtensions;
using cc.isr.VI.ExceptionExtensions;

namespace cc.isr.VI.Tsp2
{
    /// <summary> A TSP visa session. </summary>
    /// <remarks>
    /// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
    /// Licensed under The MIT License.</para><para>
    /// David, 2018-12-24 </para>
    /// </remarks>
    public class VisaSession : VisaSessionBase
    {
        #region " construction and cleanup "

        /// <summary> Initializes a new instance of the <see cref="VisaSession" /> class. </summary>
        public VisaSession() : base()
        {
            this.ApplyDefaultSyntax();
        }

        #region " i disposable support "

        /// <summary>
        /// Releases the unmanaged resources used by the object and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"> true to release both managed and unmanaged resources; false to
        /// release only unmanaged resources. </param>
        [DebuggerNonUserCode()]
        protected override void Dispose( bool disposing )
        {
            if ( this.IsDisposed ) return;
            try
            {
                if ( disposing )
                {
                }
            }
            // release unmanaged-only resources.
            catch ( Exception ex )
            {
                Debug.Assert( !Debugger.IsAttached, ex.BuildMessage() );
            }
            finally
            {
                base.Dispose( disposing );
            }
        }

        #endregion

        #endregion

        #region " syntax "

        /// <summary> Applies the default syntax. </summary>
        private void ApplyDefaultSyntax()
        {
            this.Session.ClearExecutionStateCommand = Syntax.Tsp.Status.ClearExecutionStateCommand;
            this.Session.OperationCompletedQueryCommand = Syntax.Tsp.Status.OperationEventConditionQueryCommand;
            this.Session.ResetKnownStateCommand = Syntax.Tsp.Lua.ResetKnownStateCommand;
            this.Session.ServiceRequestEnableCommandFormat = Syntax.Tsp.Status.ServiceRequestEnableCommandFormat;
            this.Session.ServiceRequestEnableQueryCommand = Pith.Ieee488.Syntax.ServiceRequestEnableQueryCommand;
            this.Session.StandardEventStatusQueryCommand = Syntax.Tsp.Status.StandardEventStatusQueryCommand;
            this.Session.StandardEventEnableQueryCommand = Syntax.Tsp.Status.StandardEventEnableQueryCommand;
            this.Session.StandardServiceEnableCommandFormat = Syntax.Tsp.Status.StandardServiceEnableCommandFormat;
            this.Session.WaitCommand = Syntax.Tsp.Lua.WaitCommand;
            this.Session.ErrorAvailableBitmask = Pith.ServiceRequests.ErrorAvailable;
            this.Session.MeasurementEventBitmask = Pith.ServiceRequests.MeasurementEvent;
            this.Session.MessageAvailableBitmask = Pith.ServiceRequests.MessageAvailable;
            this.Session.OperationEventSummaryBitmask = Pith.ServiceRequests.OperationEvent;
            this.Session.QuestionableEventBitmask = Pith.ServiceRequests.QuestionableEvent;
            this.Session.RequestingServiceBitmask = Pith.ServiceRequests.RequestingService;
            this.Session.StandardEventSummaryBitmask = Pith.ServiceRequests.StandardEvent;
            this.Session.SystemEventBitmask = Pith.ServiceRequests.SystemEvent;
        }

        #endregion

        #region " service request "

		/// <summary> Processes the service request. </summary>
		protected override void ProcessServiceRequest()
		{
			// device errors will be read if the error available bit is set upon reading the status byte.
			// 20240830: changed to read but not apply the status byte so as to prevent 
			// a query interrupt error.
			if ( this.Session is not null )
			{
				Pith.ServiceRequests statusByte = this.Session.ReadStatusByte();
				if ( this.Session.IsErrorBitSet( statusByte ) )

					// if an error occurred, let the status subsystem handle the error
					this.Session.ApplyStatusByte( statusByte );

				else if ( this.Session.IsMessageAvailableBitSet( statusByte ) )
				{
					// read the message after delay
					_ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay );

					// result is also stored in the last message received.
					this.ServiceRequestReading = this.Session.ReadFreeLineTrimEnd();

					// at this point we can allow the status subsystem to process the
					// next status byte in case reading elicited an error.
					_ = SessionBase.AsyncDelay( this.Session.StatusReadDelay );
					this.Session.ApplyStatusByte( this.Session.ReadStatusByte() );
				}
				else

					// at this point we can allow the status subsystem to process 
					// other status byte events
					this.Session.ApplyStatusByte( statusByte );
			}
		}

        #endregion

        #region " settings "

        /// <summary> Applies the settings. </summary>
        protected override void ApplySettings()
        {
        }

        #endregion

    }
}
