namespace cc.isr.VI.Pith;

/// <summary> Information about the resource name. </summary>
/// <remarks>
/// David, 2020-06-06. (c) 2020 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para>
/// </remarks>
public class ResourceNameInfo
{
    #region " construction "

    /// <summary> Initializes a new instance of the <see cref="object" /> class. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <param name="resourceName"> The name of the resource. </param>
    public ResourceNameInfo( string resourceName ) : base()
    {
        this.ResourceName = resourceName;
        this.ParseThis( resourceName );
    }

    /// <summary> Constructor. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <param name="fields"> The fields. </param>
    public ResourceNameInfo( IEnumerable<string> fields ) : base()
    {
        if ( fields?.Any() == true )
        {
            Queue<string> queue = new( fields );
            if ( queue.Any() )
                this.ResourceName = queue.Dequeue();
            if ( this.ResourceName is not null )
                this.ParseThis( this.ResourceName );
            else
                throw new NativeException( $"The first resource name in the resource name list {nameof( fields )} is null." );
        }
    }

    /// <summary> Constructor. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <param name="resourceName">    The name of the resource. </param>
    /// <param name="interfaceType">   The type of the interface. </param>
    /// <param name="interfaceNumber"> The interface number. </param>
    public ResourceNameInfo( string resourceName, HardwareInterfaceType interfaceType, int interfaceNumber ) : base()
    {
        this.ResourceName = resourceName;
        this.InterfaceType = interfaceType;
        this.InterfaceNumber = interfaceNumber;
        this.InterfaceBaseName = ResourceNamesManager.InterfaceResourceBaseName( this.InterfaceType );
        this.InterfaceResourceName = ResourceNamesManager.BuildInterfaceResourceName( this.InterfaceBaseName, this.InterfaceNumber );
        this.ResourceType = ResourceNamesManager.ParseResourceType( resourceName );
        this.ResourceAddress = string.Empty;
        if ( this.ResourceType == ResourceType.Instrument )
        {
            this.ResourceAddress = ResourceNamesManager.ParseAddress( resourceName );
        }

        if ( this.InterfaceType == HardwareInterfaceType.Gpib )
        {
            if ( !int.TryParse( this.ResourceAddress, out int address ) )
            {
                address = 0;
            }
            this.GpibAddress = address;
        }
    }

    /// <summary> Parses. </summary>
    /// <param name="resourceName"> The name of the resource. </param>
    private void ParseThis( string resourceName )
    {
        this.ResourceName = resourceName;
        this.InterfaceType = ResourceNamesManager.ParseHardwareInterfaceType( resourceName );
        this.InterfaceNumber = ResourceNamesManager.ParseInterfaceNumber( resourceName );
        this.InterfaceBaseName = ResourceNamesManager.InterfaceResourceBaseName( this.InterfaceType );
        this.InterfaceResourceName = ResourceNamesManager.BuildInterfaceResourceName( this.InterfaceBaseName, this.InterfaceNumber );
        this.ResourceType = ResourceNamesManager.ParseResourceType( resourceName );
        this.ResourceAddress = string.Empty;
        if ( this.ResourceType == ResourceType.Instrument )
        {
            this.ResourceAddress = ResourceNamesManager.ParseAddress( resourceName );
        }

        if ( this.InterfaceType == HardwareInterfaceType.Gpib )
        {
            if ( !int.TryParse( this.ResourceAddress, out int address ) )
            {
                address = 0;
            }
            this.GpibAddress = address;
        }

        this.UsingLanController = System.Globalization.CultureInfo.InvariantCulture.CompareInfo.IndexOf( resourceName, "gpib0,", System.Globalization.CompareOptions.OrdinalIgnoreCase ) >= 0;
    }

    /// <summary> Parses resource name. </summary>
    /// <param name="resourceName"> The name of the resource. </param>
    public void Parse( string resourceName )
    {
        this.ParseThis( resourceName );
    }

    #endregion

    #region " fields "

    /// <summary> Gets the name of the resource. </summary>
    /// <value> The name of the resource. </value>
    public string? ResourceName { get; set; }

    /// <summary> Gets the type of the resource. </summary>
    /// <value> The type of the resource. </value>
    public ResourceType ResourceType { get; set; }

    /// <summary> Gets the type of the interface. </summary>
    /// <value> The type of the interface. </value>
    public HardwareInterfaceType InterfaceType { get; set; }

    /// <summary> Gets the interface number. </summary>
    /// <value> The interface number. </value>
    public int InterfaceNumber { get; set; }

    /// <summary> Gets the resource address. </summary>
    /// <value> The resource address. </value>
    public string? ResourceAddress { get; set; }

    /// <summary> Gets the gpib address. </summary>
    /// <value> The gpib address. </value>
    public int GpibAddress { get; set; }

    /// <summary> Gets the sentinel indicating if the resource uses a LAN controller. </summary>
    /// <value> The sentinel indicating if the resource uses a LAN controller. </value>
    public bool UsingLanController { get; set; }

    /// <summary> Gets the is parsed. </summary>
    /// <value> The is parsed. </value>
    public bool IsParsed => !string.IsNullOrWhiteSpace( this.ResourceName );

    /// <summary> Gets the name of the interface base. </summary>
    /// <value> The name of the interface base. </value>
    public string? InterfaceBaseName { get; private set; }

    /// <summary> Gets the name of the interface resource. </summary>
    /// <value> The name of the interface resource. </value>
    public string? InterfaceResourceName { get; private set; }

    #endregion

    #region " storage "

    /// <summary> Builds header record. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <param name="delimiter"> The delimiter. </param>
    /// <returns> A <see cref="string" />. </returns>
    public static string BuildHeaderRecord( char delimiter )
    {
        System.Text.StringBuilder builder = new();
        _ = builder.Append( nameof( ResourceName ) );
        _ = builder.Append( delimiter );
        _ = builder.Append( nameof( InterfaceBaseName ) );
        return builder.ToString();
    }

    /// <summary> Builds data record. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <param name="delimiter"> The delimiter. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string BuildDataRecord( char delimiter )
    {
        System.Text.StringBuilder builder = new();
        _ = builder.Append( this.ResourceName );
        _ = builder.Append( delimiter );
        _ = builder.Append( this.InterfaceBaseName );
        return builder.ToString();
    }

    #endregion
}

/// <summary> Collection of resource name information. </summary>
/// <remarks>
/// David, 2020-06-06. (c) 2020 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para>
/// </remarks>
public class ResourceNameInfoCollection : System.Collections.ObjectModel.KeyedCollection<string, ResourceNameInfo>
{
    #region " construction "

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="System.Collections.ObjectModel.KeyedCollection{String, ResourceNameInfo}" /> class that uses the default
    /// equality comparer.
    /// </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    public ResourceNameInfoCollection() : base()
    {
        this.Keys = [];
        this.DefaultFileName = "VisaResources.txt";
        this.BackupFileName = "VisaResourcesBackup.txt";
        this.DefaultFolderName = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.CommonDocuments ), "cc.isr.visa" );
    }

    /// <summary>
    /// When implemented in a derived class, extracts the key from the specified element.
    /// </summary>
    /// <param name="item"> The element from which to extract the key. </param>
    /// <returns> The key for the specified element. </returns>
    protected override string GetKeyForItem( ResourceNameInfo item )
    {
        if ( item is null ) throw new ArgumentNullException( nameof( item ) );
        if ( item.ResourceName is null ) throw new NativeException( "Resource name info resource name is null." );
        return item.ResourceName;
    }

    #endregion

    #region " item management "

    /// <summary>
    /// Adds or replaces an object to the end of the
    /// <see cref="System.Collections.ObjectModel.KeyedCollection{String, ResourceNameInfo}" />.
    /// </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <param name="item"> The object to be added to the end of the
    /// <see cref="System.Collections.ObjectModel.KeyedCollection{String, ResourceNameInfo}" />. The value
    /// can be <see langword="null" /> for reference types. </param>
    public new void Add( ResourceNameInfo item )
    {
        if ( item is null ) throw new ArgumentNullException( nameof( item ) );
        if ( item.ResourceName is null ) throw new NativeException( "Resource name info resource name is null." );

        if ( this.Contains( item.ResourceName ) )
            this.Remove( item.ResourceName );
        base.Add( item );
        this.Keys.Add( item.ResourceName );
    }

    /// <summary>
    /// Adds an object to the end of the <see cref="System.Collections.ObjectModel.KeyedCollection{String, ResourceNameInfo}" />.
    /// </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <param name="resourceName"> The resource name to remove. </param>
    public void Add( string resourceName )
    {
        this.Add( new ResourceNameInfo( resourceName ) );
    }

    /// <summary> Removes the given resourceName. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <param name="resourceName"> The resource name to remove. </param>
    public new void Remove( string resourceName )
    {
        _ = base.Remove( resourceName );
        _ = this.Keys.Remove( resourceName );
    }

    /// <summary>
    /// Removes all elements from the <see cref="System.Collections.ObjectModel.KeyedCollection{String, ResourceNameInfo}" />.
    /// </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    public new void Clear()
    {
        base.Clear();
        this.Keys.Clear();
    }

    /// <summary> Gets the keys. </summary>
    /// <value> The keys. </value>
    public IList<string> Keys { get; private set; }

    /// <summary> Gets a list of names of the resources. </summary>
    /// <value> A list of names of the resources. </value>
    public IList<string> ResourceNames => this.Keys;

    /// <summary> Populates the given items. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <param name="items"> The items. </param>
    /// <returns> An Integer. </returns>
    public int Populate( IList<ResourceNameInfo> items )
    {
        foreach ( ResourceNameInfo item in items )
            this.Add( item );
        return this.Count;
    }

    /// <summary> Adds a new resource. </summary>
    /// <remarks> David, 2020-06-08. </remarks>
    /// <param name="resourceName"> The resource name to remove. </param>
    public static void AddNewResource( string? resourceName )
    {
        if ( string.IsNullOrWhiteSpace( resourceName ) ) throw new ArgumentNullException( nameof( resourceName ) );

        ResourceNameInfoCollection rm = [];
        rm.ReadResources();
        rm.Add( resourceName! );
        rm.WriteResources();
        rm.BackupResources();
    }
    #endregion

    #region " find "

    /// <summary> Enumerates the items in this collection that meet given criteria. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <param name="pattern"> Specifies the pattern. </param>
    /// <returns> An enumerator that allows foreach to be used to process the matched items. </returns>
    public IList<string> Find( string? pattern )
    {
        if ( string.IsNullOrWhiteSpace( pattern ) ) throw new ArgumentNullException( nameof( pattern ) );
        ResourceNameInfoCollection coll = [];
        _ = coll.Populate( this.FindResourceNamesInfo( pattern! ) );
        return coll.ResourceNames;
    }

    /// <summary> Query if <see cref="ResourceNameInfo.ResourceName"/> matches the pattern. </summary>
    /// <remarks> David, 2020-06-07. </remarks>
    /// <param name="resourceNameInfo"> Information describing the resource name. </param>
    /// <param name="pattern">          Specifies the pattern. </param>
    /// <returns> True if match, false if not. </returns>
    public static bool IsMatch( ResourceNameInfo resourceNameInfo, string? pattern )
    {
        if ( resourceNameInfo is null ) throw new ArgumentNullException( nameof( resourceNameInfo ) );
        if ( string.IsNullOrWhiteSpace( pattern ) ) throw new ArgumentNullException( nameof( pattern ) );
        if ( string.IsNullOrWhiteSpace( resourceNameInfo.ResourceName ) ) throw new ArgumentException( $"{nameof( resourceNameInfo )}.ResourceName is null or empty." );
        return cc.isr.VI.Pith.LikeOperator.IsLike( resourceNameInfo.ResourceName!, pattern! );
    }

    /// <summary> Finds the resource names information in this collection. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <param name="pattern"> Specifies the pattern. </param>
    /// <returns>
    /// An enumerator that allows foreach to be used to process the resource names information in
    /// this collection.
    /// </returns>
    public IList<ResourceNameInfo> FindResourceNamesInfo( string? pattern )
    {
        if ( string.IsNullOrWhiteSpace( pattern ) ) throw new ArgumentNullException( nameof( pattern ) );
        return (from v in this
                where (v is not null) && !string.IsNullOrWhiteSpace( v.ResourceName ) && cc.isr.VI.Pith.LikeOperator.IsLike( v.ResourceName!, pattern! )
                select v).ToList();
    }

    #endregion

    #region " storage "

    /// <summary> Query if this  is file exists. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <returns> True if file exists, false if not. </returns>
    public bool IsFileExists()
    {
        return new FileInfo( this.DefaultFullFileName ).Exists;
    }

    /// <summary> The delimiter. </summary>
    private const char Delimiter = '|';

    /// <summary> Reads resources from file. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <param name="filename"> The filename to read. </param>
    public void ReadResources( string filename )
    {
        this.Clear();
        string[] fields;
        FileInfo fi = new( filename );
        // file has to be written before it is read.
        if ( !fi.Exists )
            return;
        try
        {
            using StreamReader reader = File.OpenText( filename );
            bool isHeaderRow = true;
            while ( !reader.EndOfStream )
            {
                fields = reader.ReadLine().Split( ResourceNameInfoCollection.Delimiter );
                if ( !isHeaderRow && fields is not null && fields.Length > 0 )
                {
                    this.Add( new ResourceNameInfo( fields ) );
                }
                isHeaderRow = false;
            }

        }
        catch
        {
            throw;
        }
        finally
        {
        }
    }

    /// <summary> Reads resources from default file. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    public void ReadResources()
    {
        this.ReadResources( this.DefaultFullFileName );
        FileInfo fi = new( this.BackupFullFileName );
        if ( !fi.Exists )
            this.WriteResources( this.BackupFullFileName );
    }

    /// <summary> Restore resources. </summary>
    /// <remarks> David, 2020-06-07. </remarks>
    public void RestoreResources()
    {
        FileInfo fi = new( this.BackupFullFileName );
        if ( !fi.Exists )
            this.WriteResources( this.BackupFullFileName );
        this.ReadResources( this.BackupFullFileName );
    }

    /// <summary> Writes resources to file. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <param name="filename"> The filename to read. </param>
    public void WriteResources( string filename )
    {
        try
        {
            FileInfo fi = new( filename );
            if ( !fi.Directory.Exists )
            {
                fi.Directory.Create();
            }

            using StreamWriter writer = new( filename );
            writer.WriteLine( ResourceNameInfo.BuildHeaderRecord( Delimiter ) );
            foreach ( ResourceNameInfo resourceNameInfo in this )
                writer.WriteLine( resourceNameInfo.BuildDataRecord( Delimiter ) );
        }
        catch
        {
            throw;
        }
        finally
        {
        }
    }

    /// <summary> Writes resources to default file. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    public void WriteResources()
    {
        this.WriteResources( this.DefaultFullFileName );
        FileInfo fi = new( this.BackupFullFileName );
        if ( !fi.Exists )
            this.WriteResources( this.BackupFullFileName );
    }

    /// <summary> Backup resources. </summary>
    /// <remarks> David, 2020-06-07. </remarks>
    public void BackupResources()
    {
        this.WriteResources( this.BackupFullFileName );
    }

    /// <summary> Gets the default folder name. </summary>
    /// <value> The default folder name. </value>
    public string DefaultFolderName { get; set; }

    /// <summary> Gets the default file name. </summary>
    /// <value> The default file name. </value>
    public string DefaultFileName { get; set; }

    /// <summary> Gets the default full file name. </summary>
    /// <value> The default full file name. </value>
    public string DefaultFullFileName => Path.Combine( this.DefaultFolderName, this.DefaultFileName );

    /// <summary> Gets the filename of the backup file. </summary>
    /// <value> The filename of the backup file. </value>
    public string BackupFileName { get; set; }

    /// <summary> Gets the full filename of the backup file. </summary>
    /// <value> The full filename of the backup file. </value>
    public string BackupFullFileName => Path.Combine( this.DefaultFolderName, this.BackupFileName );

    #endregion
}
