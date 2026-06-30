using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmongUs.GameOptions;
using HarmonyLib;
using InnerNet;
using UnityEngine;
using UnityEngine.ProBuilder;

namespace DeepFriedDinoNuggets;

public class features
{

    [ HarmonyPatch( typeof( PlayerPhysics ), nameof( PlayerPhysics.LateUpdate ) ) ]
    public static class Patch_PlayerPhysics
    {
        public static void Postfix( PlayerPhysics __instance )
        {
            if ( __instance == null || __instance.myPlayer == null || __instance.myPlayer.Data == null )
            {
                 return;
            }
            
            try
            {
                if( __instance.myPlayer == PlayerControl.LocalPlayer )
                {
                    return;
                }

                settings.esp_role_settings settings = plugins.global_settings.esp_role_setting[ ( int )( __instance.myPlayer.Data.Role.Role == RoleTypes.ImpostorGhost ? RoleTypes.CrewmateGhost : __instance.myPlayer.Data.Role.Role ) ];
                
                RenderPlayerVisuals( ref __instance, settings );
                                
                if( IsEspOptionVisibile( settings, settings.visibile_all_time, __instance ) )
                {
                    __instance.myPlayer.Visible = true;
                }
            }
            catch
            {
            
            }
        }
    }

    private static void RenderPlayerVisuals( ref PlayerPhysics __instance, settings.esp_role_settings settings )
    {
        LineRenderer line_renderer = __instance.myPlayer.GetComponent<LineRenderer>();
        LineRenderer arrow_renderer = null!;
       
        Transform arrow_transform = __instance.myPlayer.transform.Find( "esp_arrow_renderer" );
        Transform transform = __instance.myPlayer.transform.Find( "esp_transform" );
        TMPro.TextMeshPro text_mesh = null;
        SpriteRenderer sprite_renderer = null;

        if( line_renderer == null )
        {
            line_renderer =  __instance.myPlayer.gameObject.AddComponent<LineRenderer>();
            line_renderer.SetVertexCount( 2 );
            line_renderer.SetWidth( 0.01f, 0.01f );
                
            try 
            { 
                if( HatManager.Instance != null )
                {
                    line_renderer.material = HatManager.Instance.PlayerMaterial; 
                }
            } 
            catch 
            { 
                        
            }
        }

        if( arrow_transform == null )
        {
            GameObject arrow_obj = new GameObject( "esp_arrow_renderer" );
            arrow_obj.transform.SetParent( __instance.myPlayer.transform, false );
            arrow_renderer = arrow_obj.AddComponent<LineRenderer>();
            arrow_renderer.SetVertexCount( 4 );
            arrow_renderer.SetWidth( 0.01f, 0.01f );
            
            if ( HatManager.Instance != null )
            {
                arrow_renderer.material = HatManager.Instance.PlayerMaterial;
            }
        }
        else
        {
            arrow_renderer = arrow_transform.GetComponent<LineRenderer>();
        }

        if( transform == null )
        {
            GameObject text_container = new GameObject( "esp_transform" );
            
            text_container.transform.SetParent( __instance.myPlayer.transform, false );
                    
            text_mesh = text_container.AddComponent<TMPro.TextMeshPro>();
            text_mesh.alignment = TMPro.TextAlignmentOptions.TopGeoAligned;
            text_mesh.fontSize = 2.5f;
                  
            UnityEngine.Object[] fonts = UnityEngine.Resources.FindObjectsOfTypeAll( Il2CppInterop.Runtime.Il2CppType.Of<TMPro.TMP_FontAsset>() );
                    
            if( fonts != null && fonts.Length > 0 )
            {
                text_mesh.font = fonts[ 0 ].Cast<TMPro.TMP_FontAsset>();
            }

            if( text_mesh.fontMaterial != null )
            {
                text_mesh.fontMaterial.EnableKeyword( "OUTLINE_ON" );
                text_mesh.fontMaterial.SetColor( "_OutlineColor", Color.black );
                text_mesh.fontMaterial.SetFloat( "_OutlineWidth", 0.2f );
            }

            GameObject icon_container = new GameObject( "icon_container" );
            icon_container.transform.SetParent( text_container.transform, false );
            sprite_renderer = icon_container.AddComponent<SpriteRenderer>();
        }
        else
        {
            text_mesh = transform.GetComponent<TMPro.TextMeshPro>();
          
            Transform icon_transform = transform.Find( "icon_container" );
            if ( icon_transform == null )
            {
                GameObject icon_container = new GameObject( "icon_container" );
                icon_container.transform.SetParent( transform, false );
                sprite_renderer = icon_container.AddComponent<SpriteRenderer>();
            }
            else
            {
                sprite_renderer = icon_transform.GetComponent<SpriteRenderer>();
            }
        }
        
        Texture2D icon_texture = plugins.global_resources.GetTextureForIcon( GetRoleIcon( __instance.myPlayer.Data.Role.Role ) );
        
        Vector3 bounds = __instance.myPlayer.transform.localScale / 2f;
        Vector3 bottom_left = new Vector3( __instance.myPlayer.transform.position.x - bounds.x, __instance.myPlayer.transform.position.y - bounds.y, __instance.myPlayer.transform.position.z );
        Vector3 bottom_right = new Vector3( __instance.myPlayer.transform.position.x + bounds.x, __instance.myPlayer.transform.position.y - bounds.y, __instance.myPlayer.transform.position.z );
        Vector3 top_right = new Vector3( __instance.myPlayer.transform.position.x + bounds.x, __instance.myPlayer.transform.position.y + bounds.y, __instance.myPlayer.transform.position.z );
        Vector3 top_left = new Vector3( __instance.myPlayer.transform.position.x - bounds.x, __instance.myPlayer.transform.position.y + bounds.y, __instance.myPlayer.transform.position.z );
        Vector3 direction = ( __instance.myPlayer.transform.position - PlayerControl.LocalPlayer.transform.position ).normalized;
        Vector3 arrow_center = PlayerControl.LocalPlayer.transform.position + ( direction * 0.55f );
        arrow_center.z = __instance.myPlayer.transform.position.z;
        Vector3 perp_direction = new Vector3( -direction.y, direction.x, 0f );
        Vector3 tip = arrow_center + ( direction * 0.05f );
        Vector3 base_left = arrow_center - ( direction * 0.05f ) + ( perp_direction * 0.035f );
        Vector3 base_right = arrow_center - ( direction * 0.05f ) - ( perp_direction * 0.035f );

        line_renderer.enabled = IsEspOptionVisibile( settings, settings.display_box, __instance );
        line_renderer.positionCount = 5;
        line_renderer.SetColors( settings.color, settings.color );
        line_renderer.SetPosition( 0, bottom_left );
        line_renderer.SetPosition( 1, bottom_right );
        line_renderer.SetPosition( 2, top_right );
        line_renderer.SetPosition( 3, top_left );
        line_renderer.SetPosition( 4, bottom_left );

        text_mesh.enabled = IsEspOptionVisibile( settings, settings.display_role_name, __instance );
        text_mesh.text = GetRoleName( __instance.myPlayer.Data.Role.Role );
        text_mesh.color = Color.white;
        text_mesh.transform.position = __instance.myPlayer.transform.position - new Vector3( 0, __instance.myPlayer.transform.localScale.y * 3, 0 );

        sprite_renderer.enabled = IsEspOptionVisibile( settings, settings.display_role_icon, __instance );

        if ( icon_texture != null )
        {
            sprite_renderer.sprite = Sprite.Create( icon_texture, new Rect( 0f, 0f, icon_texture.width, icon_texture.height ), new Vector2( 0.5f, 0.5f ), icon_texture.width / ( 40f / 100f ) );
            
            sprite_renderer.transform.position = __instance.myPlayer.transform.position - new Vector3( 0f, __instance.myPlayer.transform.localScale.y - ( text_mesh.enabled ? 0f : 0.175f ), 0f );
        }
        
        arrow_renderer.enabled = IsEspOptionVisibile( settings, settings.display_role_pointer, __instance );
        arrow_renderer.SetColors( settings.color, settings.color );
        arrow_renderer.positionCount = 4;
        arrow_renderer.SetPosition( 0, tip );
        arrow_renderer.SetPosition( 1, base_left );
        arrow_renderer.SetPosition( 2, base_right );
        arrow_renderer.SetPosition( 3, tip );
    }
    
    private static bool IsEspOptionVisibile( settings.esp_role_settings settings, bool other, PlayerPhysics __instance )
    {
        return ( settings.enabled && ( ! settings.esp_respects_visibility || settings.esp_respects_visibility && __instance.myPlayer.Visible ) && other );
    }

    public static string GetRoleIcon( RoleTypes role )
    {
        switch( role )
        {
            case RoleTypes.Crewmate: 
                return "role_crewmate.png";
            case RoleTypes.Impostor: 
                return "role_imposter.png";
            case RoleTypes.Scientist: 
                return "role_scientist.png";
            case RoleTypes.Engineer: 
                return "role_engineer.png";
            case RoleTypes.GuardianAngel: 
                return "role_guardian_angle.png";
            case RoleTypes.Shapeshifter: 
                return "role_shape_shifter.png";
            case RoleTypes.CrewmateGhost: 
                return "role_ghost.png";
            case RoleTypes.ImpostorGhost: 
                return "role_ghost.png";
            case RoleTypes.Noisemaker: 
                return "role_noise_maker.png";
            case RoleTypes.Phantom: 
                return "role_phantom.png";
            case RoleTypes.Tracker: 
                return "role_tracker.png";
            case RoleTypes.Detective: 
                return "role_detective.png";
            case RoleTypes.Viper: 
                return "role_viper.png";
            default:
                return "role_crewmate.png";
        }
    }

    public static string GetRoleName( RoleTypes role )
    {
        switch( role )
        {
            case RoleTypes.Crewmate: 
                return "Crewmate";
            case RoleTypes.Impostor: 
                return "Impostor";
            case RoleTypes.Scientist: 
                return "Scientist";
            case RoleTypes.Engineer: 
                return "Engineer";
            case RoleTypes.GuardianAngel: 
                return "Guardian Angel";
            case RoleTypes.Shapeshifter: 
                return "Shapeshifter";
            case RoleTypes.CrewmateGhost: 
                return "Ghost";
            case RoleTypes.ImpostorGhost: 
                return "Ghost";
            case RoleTypes.Noisemaker: 
                return "Noisemaker";
            case RoleTypes.Phantom: 
                return "Phantom";
            case RoleTypes.Tracker: 
                return "Tracker";
            case RoleTypes.Detective: 
                return "Detective";
            case RoleTypes.Viper: 
                return "Viper";
            default:
                return "Unknown";
        }
    }
}