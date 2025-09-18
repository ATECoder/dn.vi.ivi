# Casting a Keysight.Visa.TcpipSession to ITcpipSession fails with KeysightTechnologies.Visa 18.5.73 under .NET 9.0

Case Number: 01913757

I am getting the following exception when attempting to cast a open Tcpip session to the `Ivi.Visa.ITcpipSession` interface:
```
 System.InvalidCastException: Unable to cast object of type 'Keysight.Visa.TcpipSession' to type 'Ivi.Visa.ITcpipSession'.
```

To further investigate this issue, I modified a simple console application to identify the types of an open VISA session to a TCPIP resource. As the following code snippet shows, while identified as a `Keysight.Visa.TcpipSession' under both .NET 4.8 and 9.0 the session implements `Ivi.Visa.ITcpIpSession` under .NET 4.8 but not under .NET 9.0.

Please find below the project references that were used and the code snippet that demonstrates the issue along with the console outputs from both .NET 4.8 and .NET 9.0.

Note: I had to rebuild the project to target .NET 9.0 after testing under .NET 4.8 as otherwise, the issue does not show.

the relevant difference between the two outputs is:

__.NET 4.8 Output__
```
        is a 'Ivi.Visa.ITcpipSession'.
        is a 'Ivi.Visa.ITcpipSession2'.
```

__.NET 9.0 Output__
```
        is not a 'Ivi.Visa.ITcpipSession'.
        The type 'Ivi.Visa.ITcpipSession2' does not exist in the namespace 'Ivi.Visa'.
```

Perhaps a clue to the cause of this issue is in the fact that the type `Ivi.Visa.ITcpipSession2` does not exist in the namespace `Ivi.Visa` under .NET 9.0, which is not the case under .NET 4.8.

## Project references

The issue occurs with either this or the next project reference.

```
    <PackageReference Include="IviFoundation.Visa" Version="8.0.2" />
    <PackageReference Include="KeysightTechnologies.Visa" Version="18.5.73" />
```

```
    <PackageReference Include="KeysightTechnologies.Visa" Version="18.5.73" />
```

## Code Snippet

```csharp
    public static string IdentifyVisaSession( string resourceName )
    {
        if ( string.IsNullOrWhiteSpace( resourceName ) )
        {
            return $"{nameof( resourceName )} is null or empty or white space.";
        }

        if ( resourceName.StartsWith( "TCPIP", StringComparison.OrdinalIgnoreCase ) )
        {
            System.Text.StringBuilder sb = new();
            _ = sb.AppendLine( $"Visa Session for '{resourceName}': " );

            using Ivi.Visa.IVisaSession visaSession = Ivi.Visa.GlobalResourceManager.Open( resourceName, Ivi.Visa.AccessModes.ExclusiveLock, 2000 );

            // MessageBasedSession, ITcpipSession2, ITcpipSession, IMessageBasedSession, IVisaSession, IDisposable

            if ( visaSession is Ivi.Visa.IMessageBasedSession )
            {
                _ = sb.AppendLine( $"\tis a '{nameof( Ivi )}.{nameof( Ivi.Visa )}.{nameof( Ivi.Visa.IMessageBasedSession )}'." );

                _ = sb.Append( visaSession is Keysight.Visa.MessageBasedSession
                    ? $"\tis"
                    : $"\tis not" );
                _ = sb.AppendLine( $" a '{nameof( Keysight )}.{nameof( Keysight.Visa )}.{nameof( Keysight.Visa.MessageBasedSession )}'." );

                _ = sb.Append( visaSession is Keysight.Visa.TcpipSession
                    ? $"\tis"
                    : $"\tis not" );
                _ = sb.AppendLine( $" a '{nameof( Keysight )}.{nameof( Keysight.Visa )}.{nameof( Keysight.Visa.TcpipSession )}'." );

                _ = sb.Append( visaSession is Ivi.Visa.IVisaSession
                    ? $"\tis"
                    : $"\tis not" );
                _ = sb.AppendLine( $" a '{nameof( Ivi )}.{nameof( Ivi.Visa )}.{nameof( Ivi.Visa.IVisaSession )}'." );

                _ = sb.Append( visaSession is Ivi.Visa.ITcpipSession
                    ? $"\tis"
                    : $"\tis not" );
                _ = sb.AppendLine( $" a '{nameof( Ivi )}.{nameof( Ivi.Visa )}.{nameof( Ivi.Visa.ITcpipSession )}'." );

#if NET9_0_OR_GREATER
                string typeName = "Ivi.Visa.ITcpipSession2";
                Type? iTcpIpSession2 = Type.GetType( typeName, false, true );
                if ( iTcpIpSession2 == null )
                    _ = sb.AppendLine( $"\tThe type '{typeName}' does not exist in the namespace '{nameof( Ivi )}.{nameof( Ivi.Visa )}'." );
                else
                {
                    try
                    {
                        _ = sb.Append( iTcpIpSession2 is not null
                            ? $"\tis"
                            : $"\tis not" );
                        _ = sb.AppendLine( $" a 'typeName'." );
                    }
                    catch ( Exception ex )
                    {
                        _ = sb.AppendLine( $"\tException handling 'typeName':\n\t\t{ex.Message}." );
                    }
                }

#else
                try
                {
                    _ = sb.Append( visaSession is Ivi.Visa.ITcpipSession2
                        ? $"\tis"
                        : $"\tis not" );
                    _ = sb.AppendLine( $" a '{nameof( Ivi )}.{nameof( Ivi.Visa )}.{nameof( Ivi.Visa.ITcpipSession2 )}'." );
                }
                catch ( Exception ex )
                {
                    _ = sb.AppendLine( $" Exception handling '{nameof( Ivi )}.{nameof( Ivi.Visa )}.{nameof( Ivi.Visa.ITcpipSession2 )}'; {ex.Message}." );
                }
#endif
            }
            else
            {
                _ = sb.AppendLine( $"\tis not a '{nameof( Ivi )}.{nameof( Ivi.Visa )}.{nameof( Ivi.Visa.IMessageBasedSession )}'." );
            }
            return sb.ToString();
        }
        else
        {
            return $"Resource '{resourceName}' is not a TCPIP resource.";
        }

    }
```


## Output under .NET 4.8
```

VISA.NET Shared Components Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1.
        Version: 8.0.7511.0.
        visaConfMgr version 8.0.7331.0 detected.

Loaded Keysight.Visa, Version=18.5.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73, 18.5.48.0

Opening a VISA session to 'tcpip0::192.168.0.150::inst0::instr'...

Reading instrument identification string...

tcpip0::192.168.0.150::inst0::instr Identification string:

Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6

Visa Session for 'tcpip0::192.168.0.150::inst0::instr':
        is a 'Ivi.Visa.IMessageBasedSession'.
        is a 'Keysight.Visa.MessageBasedSession'.
        is a 'Keysight.Visa.TcpipSession'.
        is a 'Ivi.Visa.IVisaSession'.
        is a 'Ivi.Visa.ITcpipSession'.
        is a 'Ivi.Visa.ITcpipSession2'.
```

## Output under .NET 9.0
```
IviVisaNetSample, Version=8.0.1.9356, Culture=neutral, PublicKeyToken=null
        Running under .NETCoreApp,Version=v9.0 runtime .NET 9.0.8
Runtime Information:
        Framework Description: .NET 9.0.8
              OS Architecture: X64
               OS Description: Microsoft Windows 10.0.22631 (is Windows 11 if build >= 22000)
         Process Architecture: X64
           Runtime Identifier: win-x64

VISA.NET Shared Components Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1.
        Version: 8.0.7511.0.
        visaConfMgr version 8.0.7331.0 detected.

Loaded Keysight.Visa, Version=18.5.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73, 18.5.73.0

Opening a VISA session to 'tcpip0::192.168.0.150::inst0::instr'...

Reading instrument identification string...

tcpip0::192.168.0.150::inst0::instr Identification string:

Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6

Visa Session for 'tcpip0::192.168.0.150::inst0::instr':
        is a 'Ivi.Visa.IMessageBasedSession'.
        is a 'Keysight.Visa.MessageBasedSession'.
        is a 'Keysight.Visa.TcpipSession'.
        is a 'Ivi.Visa.IVisaSession'.
        is not a 'Ivi.Visa.ITcpipSession'.
        The type 'Ivi.Visa.ITcpipSession2' does not exist in the namespace 'Ivi.Visa'.
```

