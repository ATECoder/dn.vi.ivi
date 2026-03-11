using Ivi.Visa;
using Ivi.VisaNet.SessionExtensions;

namespace Ivi.VisaNet;

/// <summary>   A service request handler. This class cannot be inherited. </summary>
/// <remarks>   2026-03-09. </remarks>
internal sealed class ServiceRequestHandler
{
    /// <summary>   Default constructor. </summary>
    /// <remarks>   2026-03-09. </remarks>
    public ServiceRequestHandler()
    {
        this.CallCount = 0;
        this.PreviousCallCount = 0;
    }

    /// <summary>   Gets or sets the number of handler calls. </summary>
    /// <value> The number of handler calls. </value>
    public int CallCount { set; get; }

    /// <summary>   Gets or sets the number of handler previous calls. </summary>
    /// <value> The number of handler previous calls. </value>
    public int PreviousCallCount { set; get; }

    public bool HandlerCalled => this.CallCount != this.PreviousCallCount;

    /// <summary>   Resets the call count. </summary>
    /// <remarks>   2026-03-10. </remarks>
    public void ResetCallCount()
    {
        this.CallCount = 0;
        this.PreviousCallCount = 0;
    }

    /// <summary>   Increment the call count. </summary>
    /// <remarks>   2026-03-10. </remarks>
    /// <param name="handler">  The handler. </param>
    private void IncrementCallCount()
    {
        this.PreviousCallCount = this.CallCount;
        this.CallCount = 1 + (this.CallCount % (int.MaxValue - 1));
    }

    /// <summary>   Raises the visa event. </summary>
    /// <remarks>   2026-03-09. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information to send to registered event handlers. </param>
    public void OnServiceRequest( object? sender, VisaEventArgs e )
    {
        // This method is called asynchronously when an SRQ occurs if the status 
        // system is configured and the driver is enabled to do so.
        this.IncrementCallCount();

        Console.WriteLine( $"\tService Request Event Handler called #{this.CallCount}" );

        // The event arguments define two types:
        // e.CustomEventType of type integer
        // e.EvenType which should be EventType.ServiceRequest enum value.

        Console.WriteLine( "\tEvent arguments:" );
        Console.WriteLine( $"\t\tCustomEventType: {e.CustomEventType}" );
        Console.WriteLine( $"\t\t      EventType: {e.EventType}" );

        // The sender must be an instance of a message based session.

        if ( sender is Ivi.Visa.IMessageBasedSession mbs )
        {

            // Code to query additional status registers to determine the specific source of the SRQ, if needed, and to 
            // take appropriate action to service the SRQ.  Because this method is called asynchronously, any instrument
            // I/O done here may interfere with any I/O in progress from the main program execution.  Your code must 
            // manage this appropriately...

            Ivi.Visa.StatusByteFlags? sb = mbs.ReadStatusByte();
            Console.WriteLine( $"\tInitial Status Byte: 0x{( int ) sb:X2}" );

            if ( (sb & Ivi.Visa.StatusByteFlags.MessageAvailable) != 0 )
            {
                string textRead = mbs.ReadLineReplaceEnd();
                Console.WriteLine( $"\tPayload: {textRead}" );
            }

            sb = mbs.ReadStatusByte();
            Console.WriteLine( $"\tFinal Status Byte: 0x{( int ) sb:X2}" );

            // This method clears the status of the instrument and removes any errors that may have occurred before
            // the driver was initialized.  This improves the ability of the driver to initialize when the instrument
            // is already in an error state.  In 488.2-compliant devices, this method sends the *CLS command.
            // mbs.RawIO.Write( "*CLS\n" );

            // Performs an IEEE 488.1-style clear of the device and clears the input and output buffer
            // (both raw and formatted I/O buffers). It also may terminate pending operations on the device. 
            // mbs.Clear();

            // CppApiSystem.ClearIO();
            // Do nothing here as CppAPI/CMI constructor does this.
            // Driver.System.ClearIO();
            // Console.WriteLine( "  EventStatusRegister: " + Driver.Status.StandardEvent.ReadEventRegister());
            // Console.WriteLine();

            // mbs.DiscardEvents( EventType.AllEnabled );
            // mbs.ServiceRequest += this.OnServiceRequest;
            // mbs.EnableEvent( EventType.ServiceRequest );
        }
    }
}

