using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using UnityEngine;

namespace DeepFriedDinoNuggets;

[ BepInPlugin( "com.object.menu", "DFDN", "1.0.0" ) ]

public partial class plugins : BasePlugin
{
    public static plugins Instance { get; private set; } = null!;

    public static user_interface global_user_interface = new user_interface();

    public static style global_style = new style();

    public static resources global_resources = new resources();

    public static settings global_settings = new settings();
    
    public static features global_features = new features();

    public override void Load()
    {
        Instance = this;
     
        global_settings.InitalizeSettings();

        ClassInjector.RegisterTypeInIl2Cpp<events>();

        var user_interface_object = new GameObject( "Objects_Object" );
        
        UnityEngine.Object.DontDestroyOnLoad( user_interface_object);
        
        user_interface_object.hideFlags = HideFlags.HideAndDontSave;
     
        user_interface_object.AddComponent<events>();

        var harmony = new Harmony( "com.object.menu" );

        harmony.PatchAll();
    }
}