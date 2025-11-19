// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using System.Diagnostics;
using cc.isr.VI.ExceptionExtensions;
using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp;

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
    public VisaSession() : base() => this.ApplyDefaultSyntax();

    /// <summary> Define the session syntax. </summary>
    protected override void ApplyDefaultSyntax()
    {
        // the session instance exists at this point.
        this.Session?.ApplyDefaultSyntax( Syntax.CommandLanguage.Tsp );
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
}
