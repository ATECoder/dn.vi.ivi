using cc.isr.VI.Tsp.Script.SessionBaseExtensions;
using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script;

/// <summary>   Loads and runs TSP scripts. </summary>
/// <remarks>   David, 2007-03-12. </remarks>
/// <remarks>   Initializes a new instance of the <see cref="ScriptManagerBase" /> class. </remarks>
/// <param name="statusSubsystem">  The status subsystem. </param>
[CLSCompliant( false )]
public abstract class ScriptManagerBase( Tsp.StatusSubsystemBase statusSubsystem ) : SubsystemBase( statusSubsystem )
{
    #region " i presettable "

    /// <summary>   Sets the known initial post reset state. </summary>
    /// <remarks>   Customizes the reset state. </remarks>
    public override void InitKnownState()
    {
        base.InitKnownState();
    }

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    public override void DefineKnownResetState()
    {
        base.DefineKnownResetState();
        this.FirmwareExists = new bool?();
        this._supportScriptExists = new bool?();
        this.ScriptEntities?.DefineKnownResetState();
        foreach ( ScriptEntityCollection scripts in this.NodeScripts.Values )
            scripts.DefineKnownResetState();

        this.LegacyScripts?.DefineKnownResetState();
        foreach ( ScriptEntityCollection scripts in this.NodeLegacyScripts.Values )
            scripts.DefineKnownResetState();
    }
    #endregion

    #region " subsystems "

    /// <summary>   Gets or sets the VISA device. </summary>
    /// <value> The VISA device. </value>
    protected VisaSessionBase? VisaDevice { get; set; }

    /// <summary>   Gets or sets the display subsystem. </summary>
    /// <value> The display subsystem. </value>
    public DisplaySubsystemBase? DisplaySubsystem { get; set; }

    /// <summary>   Gets or sets the link subsystem. </summary>
    /// <value> The link subsystem. </value>
    public LinkSubsystemBase? LinkSubsystem { get; set; }

    /// <summary>   Gets or sets the interactive subsystem. </summary>
    /// <value> The interactive subsystem. </value>
    public Tsp.LocalNodeSubsystemBase? InteractiveSubsystem { get; set; }

    #endregion

    #region " identity "

    /// <summary>
    /// Gets the message indicating that registration is required for the instrument because this
    /// instrument is not included in the instrument list.
    /// </summary>
    /// <exception cref="NativeException">  Thrown when a Native error condition occurs. </exception>
    /// <value> The new program required. </value>
    public string NewProgramRequired
    {
        get
        {
            if ( string.IsNullOrWhiteSpace( this.StatusSubsystem.Identity ) )
                throw new NativeException( $"{nameof( this.StatusSubsystem )}.{nameof( this.StatusSubsystem.Identity )} is empty." );
            return $"{this.StatusSubsystem.Identity} is either unregistered or not included in the list of registered instruments.";
        }
    }

    #endregion

    #region " saved user script names "

    /// <summary>   Fetches the saved user script names from the non-volatile user script catalog. </summary>
    /// <remarks>   2024-09-10. </remarks>
    public int FetchSavedUserScriptNames()
    {
        string savedScriptNames;
        if ( this.ScriptEntities is null )
            savedScriptNames = this.Session.FetchSavedScriptsNames();
        else
        {
            this.ScriptEntities!.FetchSavedScriptsNames( this.Session );
            savedScriptNames = this.ScriptEntities.SavedScriptNames;
        }
        if ( string.IsNullOrWhiteSpace( savedScriptNames ) )
            this.SavedUserScriptNames = [];
        else
            this.SavedUserScriptNames = savedScriptNames.Split( ',' );

        return this.SavedUserScriptNames.Length;
    }

    /// <summary>   Returns the list of saved user script names. </summary>
    /// <value> the list of saved user script names. </value>
    public string[] SavedUserScriptNames { get; private set; } = [];

    /// <summary>   Fetches the loaded script names from the user script catalog. </summary>
    /// <remarks>   2024-09-10. </remarks>
    public int FetchLoadedUserScriptNames()
    {
        this.LoadedUserScriptNames = this.Session.FetchUserScriptNames();
        return this.LoadedUserScriptNames.Length;
    }

    /// <summary>   Returns the list of loaded user script names. </summary>
    /// <value> the list of loaded user script names. </value>
    public string[] LoadedUserScriptNames { get; private set; } = [];

    #endregion

    #region " add script "

    /// <summary>
    /// Adds a script to scripts collection if the instrument model matches the script model mask.
    /// </summary>
    /// <remarks>   2024-09-09. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="scripts">  Specifies the collection of scripts. </param>
    /// <param name="script">   Specifies reference to a valid <see cref="ScriptEntity">
    ///                         script</see> </param>
    protected virtual void AddScriptIfModelMatch( ScriptEntityCollection scripts, ScriptEntityBase script )
    {
        // make sure that the script matches its model.
        if ( script.IsModelMatch( script.Node.ModelNumber ) )
            _ = scripts.AddScriptItem( script );
    }

    /// <summary>   Throw and exception if the script instrument model does not match the script model mask. </summary>
    /// <remarks>   2024-09-20. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="script">   Specifies reference to a valid <see cref="ScriptEntity">
    ///                         script</see> </param>
    public static void ThrowIfModelMismatch( ScriptEntityBase script )
    {
        if ( !script.IsModelMatch( script.Node.ModelNumber ) )
            throw new InvalidOperationException( $"script '{script.Name}' model mask {script.ModelMask} does not match the specified model {script.Node.ModelNumber}" );
    }

    #endregion

    #region " find script "

    /// <summary>   Checks if the specified script allSaved as a saved script in the <see cref="ScriptEntityBaseCollection{ScriptEntityBase}.SavedScriptNames"/>. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="scriptName">           Specifies the script name. </param>
    /// <param name="refreshScriptCatalog"> (Optional) True to refresh the list of saved scripts. </param>
    /// <returns>
    /// <c>true</c> if the specified script allSaved as a saved script; otherwise, <c>false</c>.
    /// </returns>
    public bool LastSavedScriptExists( string scriptName, bool refreshScriptCatalog = false )
    {
        return this.NodeScripts[this.ScriptEntities!.ControllerNodeNumber].SavedScriptExists( this.Session, scriptName, refreshScriptCatalog );
    }

    /// <summary>
    /// Returns <c>true</c> if the specified script allSaved as a saved script in the remote <see cref="ScriptEntityBaseCollection{ScriptEntityBase}.SavedScriptNames"/>.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="scriptName">           Specifies the script name. </param>
    /// <param name="node">                 Specifies the node to validate. </param>
    /// <param name="refreshScriptCatalog"> (Optional) True to refresh the list of saved scripts. </param>
    /// <returns>
    /// <c>true</c> if the specified script allSaved as a saved script on the remote node;
    /// otherwise, <c>false</c>.
    /// </returns>
    public bool LastSavedScriptExists( string scriptName, NodeEntityBase node, bool refreshScriptCatalog = false )
    {
        ScriptEntityBaseCollection<ScriptEntityBase> scripts = this.NodeScripts[node.Number];
        return scripts.SavedScriptExists( this.Session, scriptName, refreshScriptCatalog );
    }

    /// <summary>   Returns <c>true</c> if the specified script allSaved as a saved script. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="scriptName">           Specifies the script name. </param>
    /// <param name="node">                 Specifies the node to validate. </param>
    /// <param name="refreshScriptCatalog"> True to refresh the list of saved scripts. </param>
    /// <returns>
    /// <c>true</c> if the specified script allSaved as a saved script; otherwise,
    /// <c>false</c>.
    /// </returns>
    public bool SavedScriptExists( string scriptName, NodeEntityBase node, bool refreshScriptCatalog )
    {
        return node is null
            ? throw new ArgumentNullException( nameof( node ) )
            : node.IsController
                ? this.LastSavedScriptExists( scriptName, refreshScriptCatalog )
                : this.LastSavedScriptExists( scriptName, node, refreshScriptCatalog );
    }

    #endregion

    #region " script collections "

    /// <summary>   Gets the list of legacy scripts. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <returns>   List of legacy scripts. </returns>
    public ScriptEntityCollection? LegacyScripts { get; protected set; }

    /// <summary>   Adds a new script to the list of legacy scripts. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="scriptName">   Specifies the name of the script. </param>
    /// <returns>   The added script. </returns>
    public ScriptEntityBase AddLegacyScript( string scriptName )
    {
        if ( this.LegacyScripts is null ) throw new NativeException( $"{nameof( this.LegacyScripts )} must not be null." );
        ScriptEntity script = new( new FirmwareScript( scriptName, string.Empty, new Version() ), this.LegacyScripts.Node );
        this.LegacyScripts.Add( script );
        return script;
    }

    /// <summary>   Gets or sets the node legacy scripts. </summary>
    /// <value> The node scripts. </value>
    public Dictionary<int, ScriptEntityCollection> NodeLegacyScripts { get; protected set; } = [];

    /// <summary>   Gets or sets the firmware scripts. </summary>
    /// <value> The firmware scripts. </value>
    public ScriptFirmwareCollection FirmwareScripts { get; protected set; } = [];

    /// <summary>   Adds a firmware script to 'modelMask'. </summary>
    /// <remarks>   2025-04-07. </remarks>
    /// <param name="scriptName">   Specifies the name of the script. </param>
    /// <param name="versionInfo">  Information describing the version. </param>
    /// <returns>   A ScriptFirmwareBase. </returns>
    public FirmwareScriptBase AddFirmwareScript( string scriptName, VersionInfoBase versionInfo )
    {
        FirmwareScript script = new( scriptName, versionInfo.ModelFamily, versionInfo.FirmwareVersion );
        this.FirmwareScripts.Add( script );
        return script;
    }

    /// <summary>   Adds a firmware script to 'modelMask'. </summary>
    /// <remarks>   2024-09-06. </remarks>
    /// <param name="scriptName">   Specifies the name of the script. </param>
    /// <param name="modelMask">    Specifies the family of instrument models for this script. </param>
    /// <param name="modelVersion"> The model version. </param>
    /// <returns>   A ScriptFirmwareBase. </returns>
    public FirmwareScriptBase AddFirmwareScript( string scriptName, string modelMask, Version modelVersion )
    {
        FirmwareScript script = new( scriptName, modelMask, modelVersion );
        this.FirmwareScripts.Add( script );
        return script;
    }

    /// <summary>
    /// Gets or sets the core list of script entities, which are initialized using the controller
    /// node.
    /// </summary>
    /// <value> List of script entities. </value>
    public ScriptEntityCollection? ScriptEntities { get; protected set; }

    /// <summary>   Adds a new script to the list of core scripts. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="firmwareScript">   Specifies the firmware script. </param>
    /// <returns>   The added script. </returns>
    public ScriptEntityBase AddScriptEntity( FirmwareScriptBase firmwareScript )
    {
        if ( this.ScriptEntities is null ) throw new NativeException( $"{nameof( this.ScriptEntities )} must not be null." );
        ScriptEntity script = new( firmwareScript, this.ScriptEntities.Node );
        this.AddScriptIfModelMatch( this.ScriptEntities, script );
        return script;
    }

    /// <summary>   Gets or sets the node scripts. </summary>
    /// <value> The node scripts. </value>
    public Dictionary<int, ScriptEntityCollection> NodeScripts { get; protected set; } = [];

    #endregion

    #region " scripts firmware "

    /// <summary>   Returns the released main firmware version. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <returns>   The released main firmware version. </returns>
    public string FirmwareReleasedVersionGetter()
    {
        if ( this.ScriptEntities is null ) throw new NativeException( $"{nameof( this.ScriptEntities )} must not be null." );
        return this.LinkSubsystem?.ControllerNode is not null
            ? this.FirmwareReleasedVersionGetter( this.LinkSubsystem.ControllerNode )
            : this.ScriptEntities[0].FirmwareScript.FirmwareVersion;
    }

    /// <summary>   Returns the released main firmware version. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="NativeException">          Thrown when a Native error condition occurs. </exception>
    /// <param name="node"> Specifies the node. </param>
    /// <returns>   The released main firmware version. </returns>
    public string FirmwareReleasedVersionGetter( NodeEntityBase? node )
    {
        if ( this.ScriptEntities is null ) throw new NativeException( $"{nameof( this.ScriptEntities )} must not be null." );
        return this.LinkSubsystem?.ControllerNode is null
            ? this.ScriptEntities[0].FirmwareScript.FirmwareVersion
            : node is null
                ? throw new ArgumentNullException( nameof( node ) )
                : this.ScriptEntities.SelectSerialNumberScript( node! )?.FirmwareScript.FirmwareVersion
                    ?? throw new NativeException( "Failed selecting serial number script." );
    }

    /// <summary>   Returns the embedded main firmware version. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <returns>   The actual embedded firmware version. </returns>
    public string FirmwareVersionGetter()
    {
        return this.LinkSubsystem?.ControllerNode is not null
            ? this.FirmwareVersionGetter( this.LinkSubsystem.ControllerNode )
            : "<unknown>";
    }

    /// <summary>   Returns the embedded main firmware version. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="NativeException">          Thrown when a Native error condition occurs. </exception>
    /// <param name="node"> Specifies the node. </param>
    /// <returns>   The released main firmware version. </returns>
    public string FirmwareVersionGetter( NodeEntityBase? node )
    {
        if ( this.ScriptEntities is null ) throw new NativeException( $"{nameof( this.ScriptEntities )} must not be null." );
        return node is null
            ? throw new ArgumentNullException( nameof( node ) )
            : this.ScriptEntities.SelectSerialNumberScript( node )?.EmbeddedFirmwareVersion
                    ?? throw new NativeException( "Failed selecting serial number script." );
    }

    /// <summary>   Returns the main firmware script name from the controller node. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="NativeException">  Thrown when a Native error condition occurs. </exception>
    /// <returns>   The firmware scriptName. </returns>
    public string FirmwareNameGetter()
    {
        if ( this.ScriptEntities is null ) throw new NativeException( $"{nameof( this.ScriptEntities )} must not be null." );
        return this.FirmwareNameGetter( this.ScriptEntities.Node );
    }

    /// <summary>   Returns the node firmware scriptName. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="NativeException">          Thrown when a Native error condition occurs. </exception>
    /// <param name="node"> Specifies the node. </param>
    /// <returns>   The node firmware scriptName. </returns>
    public string FirmwareNameGetter( NodeEntityBase? node )
    {
        if ( this.ScriptEntities is null ) throw new NativeException( $"{nameof( this.ScriptEntities )} must not be null." );
        return node is null
            ? throw new ArgumentNullException( nameof( node ) )
            : this.ScriptEntities.SelectSerialNumberScript( node )?.Name
                    ?? throw new NativeException( "Failed selecting serial number script." );
    }

    /// <summary> The firmware allSaved. </summary>
    private bool? _firmwareExists;

    /// <summary>   Gets or sets the firmware allSaved. </summary>
    /// <value> The firmware allSaved. </value>
    public bool? FirmwareExists
    {
        get => this._firmwareExists;
        set => _ = this.SetProperty( ref this._firmwareExists, value );
    }

    /// <summary>   Checks if firmware allSaved on the controller node. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="NativeException">  Thrown when a Native error condition occurs. </exception>
    /// <returns>   <c>true</c> if the firmware allSaved; otherwise, <c>false</c>. </returns>
    public bool FindFirmware()
    {
        if ( this.ScriptEntities is null ) throw new NativeException( $"{nameof( this.ScriptEntities )} must not be null." );
        return this.FindFirmware( this.ScriptEntities.Node );
    }

    /// <summary>   Checks if the main firmware allSaved . </summary>
    /// <remarks>   Value is cached in the <see cref="FirmwareExists">sentinel</see> </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="node"> Specifies the node. </param>
    /// <returns>   <c>true</c> if the firmware allSaved; otherwise, <c>false</c>. </returns>
    public bool FindFirmware( NodeEntityBase? node )
    {
        if ( node is null ) throw new ArgumentNullException( nameof( node ) );
        this._firmwareExists = !this.Session.IsNil( this.FirmwareNameGetter( node ) );
        return this._firmwareExists.Value;
    }

    /// <summary>   Returns the Support Firmware Script name. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="node"> Specifies the node. </param>
    /// <returns>   <c>true</c> if the Support Firmware Script allSaved; otherwise, <c>false</c>. </returns>
    public string? SupportScriptNameGetter( NodeEntityBase? node )
    {
        if ( this.ScriptEntities is null ) throw new NativeException( $"{nameof( this.ScriptEntities )} must not be null." );
        return node is null
            ? throw new ArgumentNullException( nameof( node ) )
            : this.ScriptEntities.SelectSupportScript( node )?.Name;
    }

    private bool? _supportScriptExists;

    /// <summary>   Gets or sets the firmware allSaved. </summary>
    /// <value> The Support Firmware Script allSaved 1. </value>
    public bool? SupportScriptExists1
    {
        get => this._supportScriptExists;
        set => _ = this.SetProperty( ref this._supportScriptExists, value );
    }

    /// <summary>   Checks if the Support Firmware Script allSaved on the controller node. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="NativeException">  Thrown when a Native error condition occurs. </exception>
    /// <returns>   <c>true</c> if the Support Firmware Script allSaved; otherwise, <c>false</c>. </returns>
    public bool FindSupportScript()
    {
        if ( this.ScriptEntities is null ) throw new NativeException( $"{nameof( this.ScriptEntities )} must not be null." );
        return this.FindSupportScript( this.ScriptEntities.Node );
    }

    /// <summary>   Checks if the Support Firmware Script allSaved. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="NativeException">          Thrown when a Native error condition occurs. </exception>
    /// <param name="node">     Specifies the node. </param>
    /// <returns>
    /// <c>true</c> if the Support Firmware Script allSaved; otherwise, <c>false</c>.
    /// </returns>
    public bool FindSupportScript( NodeEntityBase? node )
    {
        if ( node is null ) throw new ArgumentNullException( nameof( node ) );
        this._supportScriptExists = !this.Session.IsNil( this.SupportScriptNameGetter( node ) ??
            throw new NativeException( "Failed getting firmware scriptName." ) );
        return this._supportScriptExists.Value;
    }

    /// <summary>   Reads the firmware versions of the controller node. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="NativeException">  Thrown when a Native error condition occurs. </exception>
    /// <returns>   <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    public bool ReadFirmwareVersions()
    {
        if ( this.ScriptEntities is null ) throw new NativeException( $"{nameof( this.ScriptEntities )} must not be null." );
        return this.ScriptEntities.QueryFirmwareVersions( this.Session );
    }

    /// <summary>   Checks if all scripts were saved. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="NativeException">  Thrown when a Native error condition occurs. </exception>
    /// <param name="refreshScriptCatalog"> Specifies the condition for updating the catalog of saved
    ///                                     scripts before checking the status of these scripts. </param>
    /// <returns>   <c>true</c> if all scripts were saved; otherwise, <c>false</c>. </returns>
    public bool AllScriptsSaved( bool refreshScriptCatalog )
    {
        if ( this.NodeScripts is null ) throw new NativeException( $"{nameof( this.NodeScripts )} must not be null." );
        bool allSaved = true;
        foreach ( ScriptEntityCollection scripts in this.NodeScripts.Values )
        {
            if ( refreshScriptCatalog )
                scripts.ReadScriptsState( this.Session );
            allSaved = allSaved && scripts.AllSaved();
            if ( !allSaved ) break;
        }
        return allSaved;
    }

    /// <summary>   Reads scripts state. </summary>
    /// <remarks>   2024-10-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="NativeException">          Thrown when a Native error condition occurs. </exception>
    /// <param name="session">  The session. </param>
    public void ReadScriptsState( Pith.SessionBase? session )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( this.NodeScripts is null ) throw new NativeException( $"{nameof( this.NodeScripts )} must not be null." );
        foreach ( ScriptEntityCollection scripts in this.NodeScripts.Values )
        {
            // read the state of the current scripts.
            scripts.ReadScriptsState( session );
        }
    }

    /// <summary>   Checks if all scripts exist. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="NativeException">  Thrown when a Native error condition occurs. </exception>
    /// <returns>   <c>true</c> if all scripts exist; otherwise, <c>false</c>. </returns>
    public bool AllScriptsExist()
    {
        if ( this.NodeScripts is null ) throw new NativeException( $"{nameof( this.NodeScripts )} must not be null." );
        bool allExist = true;
        foreach ( ScriptEntityCollection scripts in this.NodeScripts.Values )
        {
            allExist = allExist && scripts.AllExist();
            if ( !allExist ) break;
        }
        return allExist;
    }

    /// <summary>   Checks if any script allSaved on the controller node. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="NativeException">  Thrown when a Native error condition occurs. </exception>
    /// <returns>   <c>true</c> if any script allSaved; otherwise, <c>false</c>. </returns>
    public bool AnyScriptExists()
    {
        if ( this.NodeScripts is null ) throw new NativeException( $"{nameof( this.NodeScripts )} must not be null." );
        bool exists = false;
        foreach ( ScriptEntityCollection scripts in this.NodeScripts.Values )
        {
            exists = exists || scripts.AnyExist();
            if ( exists ) break;
        }
        return exists;
    }

    /// <summary>   Returns <c>true</c> if any legacy scripts exist on the controller node. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="NativeException">  Thrown when a Native error condition occurs. </exception>
    /// <returns>   <c>true</c> if any legacy script allSaved; otherwise, <c>false</c>. </returns>
    public bool AnyLegacyScriptExists()
    {
        if ( this.NodeLegacyScripts is null ) throw new NativeException( $"{nameof( this.NodeLegacyScripts )} must not be null." );
        bool anyExists = false;
        foreach ( ScriptEntityCollection scripts in this.NodeLegacyScripts.Values )
        {
            anyExists = anyExists || scripts.AnyExist();
            if ( anyExists ) break;
        }
        return anyExists;
    }

    #endregion

    #region " framework script info "

    /// <summary>   Gets or sets the script directory info. </summary>
    /// <value> The script directory info. </value>
    public FirmwareDirectoryInfo? ScriptDirectoryInfo { get; set; }

    #endregion

    #region " framework scripts methods "

    /// <summary>   Define scripts. </summary>
    /// <remarks>   2024-08-16. </remarks>
    /// <param name="readSource">       (Optional) (true) True to read source. </param>
    public virtual void DefineScripts( bool readSource = true ) { }

    /// <summary>   Deletes the user scripts. </summary>
    /// <remarks>   David, 2021-12-03. </remarks>
    /// <param name="displaySubsystem"> A reference to a
    ///                                 <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                 subsystem</see>. </param>
    /// <param name="confirmFunction">  The confirm function; typically displays a dialog of Message,
    ///                                 Caption. </param>
    public virtual void DeleteUserScripts( DisplaySubsystemBase displaySubsystem, Func<string, string, bool> confirmFunction ) { }

    /// <summary>   Uploads the user scripts to the controller instrument. </summary>
    /// <remarks>   David, 2021-12-03. </remarks>
    /// <param name="displaySubsystem"> A reference to a
    ///                                 <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                 subsystem</see>. </param>
    /// <param name="accessSubsystem">  The access subsystem. </param>
    /// <param name="confirmFunction">  The confirm function; typically displays a dialog of Message,
    ///                                 Caption. </param>
    /// <returns>   <c>true</c> if uploaded; otherwise, <c>false</c>. </returns>
    public virtual (bool Success, string Details) UploadUserScripts( DisplaySubsystemBase displaySubsystem, AccessSubsystemBase accessSubsystem,
        Func<string, string, bool> confirmFunction )
    { return (false, string.Empty); }

    /// <summary>   Saves the user scripts applying the release scriptName. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="displaySubsystem"> A reference to a
    ///                                 <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                 subsystem</see>. </param>
    /// <param name="accessSubsystem">  The access subsystem. </param>
    /// <returns>   <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    public virtual (bool Success, string Details) SaveUserScripts( DisplaySubsystemBase displaySubsystem, AccessSubsystemBase accessSubsystem )
    { return (false, string.Empty); }

    #endregion

    #region " obsoleted "

    /// <summary>   Deletes the user scripts. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="statusSubsystem">  A reference to a
    ///                                 <see cref="VI.Tsp.StatusSubsystemBase">status
    ///                                 subsystem</see>. </param>
    /// <param name="displaySubsystem"> A reference to a
    ///                                 <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                 subsystem</see>. </param>
    [Obsolete( "Use DeleteSavedUserScripts( displaySubsystem, confirmFunction) )" )]
    public virtual void DeleteUserScripts( StatusSubsystemBase statusSubsystem, DisplaySubsystemBase displaySubsystem )
    {
    }

    /// <summary>   Deletes the user scripts. </summary>
    /// <remarks>   David, 2021-12-03. </remarks>
    /// <param name="statusSubsystem">  A reference to a
    ///                                 <see cref="VI.Tsp.StatusSubsystemBase">status
    ///                                 subsystem</see>. </param>
    /// <param name="displaySubsystem"> A reference to a
    ///                                 <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                 subsystem</see>. </param>
    /// <param name="confirmFunction">  The confirm function; typically displays a dialog of Message,
    ///                                 Caption. </param>
    [Obsolete( "Use DeleteSavedUserScripts( displaySubsystem, confirmFunction) )" )]
    public virtual void DeleteUserScripts( StatusSubsystemBase statusSubsystem, DisplaySubsystemBase displaySubsystem, Func<string, string, bool> confirmFunction )
    {
    }

    /// <summary>   Uploads the user scripts to the controller instrument. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="statusSubsystem">  A reference to a
    ///                                 <see cref="VI.Tsp.StatusSubsystemBase">status
    ///                                 subsystem</see>. </param>
    /// <param name="displaySubsystem"> A reference to a
    ///                                 <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                 subsystem</see>. </param>
    /// <param name="accessSubsystem">  The access subsystem. </param>
    /// <returns>   <c>true</c> if uploaded; otherwise, <c>false</c>. </returns>
    [Obsolete( "Use upload user scripts with option dialog." )]
    public virtual bool UploadUserScripts( StatusSubsystemBase statusSubsystem, DisplaySubsystemBase displaySubsystem, AccessSubsystemBase accessSubsystem )
    {
        return false;
    }

    /// <summary>   Uploads the user scripts to the controller instrument. </summary>
    /// <remarks>   David, 2021-12-03. </remarks>
    /// <param name="statusSubsystem">  A reference to a
    ///                                 <see cref="VI.Tsp.StatusSubsystemBase">status
    ///                                 subsystem</see>. </param>
    /// <param name="displaySubsystem"> A reference to a
    ///                                 <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                 subsystem</see>. </param>
    /// <param name="accessSubsystem">  The access subsystem. </param>
    /// <param name="confirmFunction">  The confirm function; typically displays a dialog of Message,
    ///                                 Caption. </param>
    /// <returns>   <c>true</c> if uploaded; otherwise, <c>false</c>. </returns>
    [Obsolete( "Use DeleteSavedUserScripts( displaySubsystem, accessSubsystem, confirmFunction) )" )]
    public virtual bool UploadUserScripts( StatusSubsystemBase statusSubsystem, DisplaySubsystemBase displaySubsystem,
        AccessSubsystemBase accessSubsystem, Func<string, string, bool> confirmFunction )
    {
        return false;
    }

    /// <summary>   Saves the user scripts applying the release scriptName. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="timeout">          The completionTimeout. </param>
    /// <param name="statusSubsystem">  A reference to a
    ///                                 <see cref="VI.Tsp.StatusSubsystemBase">status
    ///                                 subsystem</see>. </param>
    /// <param name="displaySubsystem"> A reference to a
    ///                                 <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                 subsystem</see>. </param>
    /// <param name="accessSubsystem">  The access subsystem. </param>
    /// <returns>   <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    [Obsolete( "Use SaveUserScripts( displaySubsystem, accessSubsystem, saveTimeout) )" )]
    public virtual bool SaveUserScripts( TimeSpan timeout, StatusSubsystemBase statusSubsystem, DisplaySubsystemBase displaySubsystem, AccessSubsystemBase accessSubsystem )
    {
        return false;
    }

    /// <summary>   Try read parse user scripts. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="statusSubsystem">  A reference to a
    ///                                 <see cref="VI.Tsp.StatusSubsystemBase">status
    ///                                 subsystem</see>. </param>
    /// <param name="displaySubsystem"> A reference to a
    ///                                 <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                 subsystem</see>. </param>
    /// <returns>   <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    [Obsolete( "Use TryReadParseUserScripts( displaySubsystem ) )" )]
    public virtual bool TryReadParseUserScripts( StatusSubsystemBase statusSubsystem, DisplaySubsystemBase displaySubsystem )
    {
        return false;
    }

    /// <summary>   Try read parse user scripts. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="displaySubsystem"> A reference to a
    ///                                 <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                 subsystem</see>. </param>
    /// <returns>   <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    [Obsolete( "no longer used" )]
    public virtual bool TryReadParseUserScripts( DisplaySubsystemBase displaySubsystem )
    {
        return false;
    }

    #endregion
}
