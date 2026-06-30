using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using static FilterOptionUI;
using AmongUs.GameOptions;
using System.Linq;

namespace DeepFriedDinoNuggets;

public class user_interface
{
    public void Update()
    {
        _animation_x = Mathf.Lerp( _animation_x, _animation_target_x, Time.deltaTime * ( ( _button_size + _button_spacing_x ) * 0.10f ) );
        
        for( int i = 0; i < _tabs.Count; i++ )
        {
            var key = _tabs.Keys.ElementAt( i );

            _tabs[ key ].animation_current = Mathf.Lerp( _tabs[ key ].animation_current, _tabs[ key ].animation_target, Time.deltaTime * 4f );
        }
    }

    public void OnGui()
    {
        plugins.global_style.InitializeStyles();
        
        if( Event.current != null && Event.current.isKey && Event.current.type == EventType.KeyDown )
        {
            if( Event.current.keyCode == KeyCode.F1 )
			{
                _animation_target_x = _animation_target_x == 0f ? ( _button_size + _button_spacing_x ) * 2 + 5f : 0f;
            }
        }

        for( int i = 0; i < _tabs.Count; i++ )
        {
            var key = _tabs.Keys.ElementAt( i );
            
            float tab_button_x = Screen.width - ( _button_size + _button_spacing_x ) * ( int )( 2 - i / 6 ) + _animation_x;
			float tab_button_y = ( Screen.height / 2f ) - ( 6f * _button_size + 5f * _button_spacing_y ) / 2f + i % 6 * ( _button_size + _button_spacing_y );
			Rect rect = new Rect( tab_button_x, tab_button_y, _button_size, _button_size );
            
            if( _current_role_in_edit == ( int )_tabs[ key ].target_role )
            {
                Rect hightlight_rect = rect;
                
                hightlight_rect.x -= 3f;
                hightlight_rect.y -= 3f;
                hightlight_rect.width += 6f;
                hightlight_rect.height += 6f;
                
                GUI.Box( hightlight_rect, "", plugins.global_style.tabbar_button_highlight );
            }

            GUI.Box( rect, "", plugins.global_style.tabbar_button_not_active );

            Color orignal_color = GUI.color;

            GUI.color = new Color( 1f, 1f, 1f, _tabs[ key ].animation_current );

            GUI.Box( rect, "", plugins.global_style.tabbar_button_active );
    
            GUI.color = orignal_color;
            
            float padding = rect.width * 0.18f;
            Rect rect_icon = new Rect( rect.x + padding, rect.y + padding, rect.width - ( padding * 2 ), rect.height - ( padding * 2 ) );
            
            GUI.Box( rect_icon, "", plugins.global_resources.GetStyleForIcon( key ) );

            if( GUI.Button( rect, "", GUIStyle.none ) )
            {
                _tabs[ key ].animation_target = _tabs[ key ].animation_target == 0f ? 1f : 0f;

                plugins.global_settings.esp_role_setting[ ( int )_tabs[ key ].target_role ].enabled = ! ( _tabs[ key ].animation_target == 0f );
            }
        }

        GUI.color = new Color( 1f, 1f, 1f, 1f - ( _animation_x / ( ( _button_size + _button_spacing_x ) * 2 ) ) );

        _page_settings.rect = GUI.Window( 0, _page_settings.rect, ( Action<int> )this.RenderPage, GUIContent.none, plugins.global_style.window );
    }
    private void RenderPage( int window_id )
    {
        GUI.color = new Color( 1f, 1f, 1f, 1f - ( _animation_x / ( ( _button_size + _button_spacing_x ) * 2 ) ) );

        GUI.Box( new Rect( 0, 0, _page_settings.rect.width, _page_settings.rect.height ), GUIContent.none, plugins.global_style.window );
        
        GUILayout.BeginArea( new Rect( 10, 10, _page_settings.rect.width - 20, _page_settings.rect.height - 20 ) );
        
        float row_height = 26f;

        GUILayout.BeginHorizontal( GUILayout.Height( row_height ) );
        
        if ( GUILayout.Button( "<", plugins.global_style.button, GUILayout.Width( row_height ), GUILayout.Height( row_height )  ) )
        {
            _current_tab_index--;
            if ( _current_tab_index < 0 )
            {
                _current_tab_index = _tabs.Count - 1;
            }
            _current_role_in_edit = ( int )_tabs.Values.ElementAt( _current_tab_index ).target_role;
        }

        GUILayout.Label( features.GetRoleName( _tabs.Values.ElementAt( _current_tab_index ).target_role ), plugins.global_style.centered_label, GUILayout.ExpandWidth( true ), GUILayout.Height( row_height ) );
        
        if ( GUILayout.Button( ">", plugins.global_style.button, GUILayout.Width( row_height ), GUILayout.Height( row_height )  ) )
        {
            _current_tab_index++;
            if ( _current_tab_index >= _tabs.Count )
            {
                _current_tab_index = 0;
            }
            _current_role_in_edit = ( int )_tabs.Values.ElementAt( _current_tab_index ).target_role;
        }

        GUILayout.EndHorizontal();
        
        plugins.global_style.RenderToggle( ref plugins.global_settings.esp_role_setting[ _current_role_in_edit ].visibile_all_time, "Render Model All Time" );
        plugins.global_style.RenderToggle( ref plugins.global_settings.esp_role_setting[ _current_role_in_edit ].esp_respects_visibility, "ESP Respect Visibility" );
        plugins.global_style.RenderToggle( ref plugins.global_settings.esp_role_setting[ _current_role_in_edit ].display_box, "Box" );
        plugins.global_style.RenderToggle( ref plugins.global_settings.esp_role_setting[ _current_role_in_edit ].display_role_name, "Role Text" );
        plugins.global_style.RenderToggle( ref plugins.global_settings.esp_role_setting[ _current_role_in_edit ].display_role_icon, "Role Icon" );
        plugins.global_style.RenderToggle( ref plugins.global_settings.esp_role_setting[ _current_role_in_edit ].display_role_pointer, "Role Pointer" );
        plugins.global_style.RenderColorPicker( ref plugins.global_settings.esp_role_setting[ _current_role_in_edit ].color, "Color" );
        
        if ( Event.current.type == EventType.Repaint )
        {
            _page_settings.rect.width = 300;
            _page_settings.rect.height = GUILayoutUtility.GetLastRect().yMax + 20;
            _page_settings.rect.x = Screen.width - ( _button_size + _button_spacing_x ) * 2f + _animation_x - _page_settings.rect.width - _button_spacing_x;
            _page_settings.rect.y = ( Screen.height / 2f ) - _page_settings.rect.height / 2;
        }

        GUILayout.EndArea();

        GUI.color = new Color( 1f, 1f, 1f, 1f );
    }
    
    private class tab
    {
        public float animation_current;
        public float animation_target;
        public RoleTypes target_role;

        public tab( RoleTypes role ) 
        { 
            this.animation_current = 0f;
            this.animation_target = 0f;
            this.target_role = role;    
        }
    }

    private Dictionary<string, tab> _tabs = new Dictionary<string, tab>
    {
        { "role_ghost.png", new tab( RoleTypes.CrewmateGhost ) },
        { "role_guardian_angle.png", new tab( RoleTypes.GuardianAngel ) },
        { "role_crewmate.png", new tab( RoleTypes.Crewmate ) },
        { "role_detective.png", new tab( RoleTypes.Detective ) },
        { "role_engineer.png", new tab( RoleTypes.Engineer ) },
        { "role_noise_maker.png", new tab( RoleTypes.Noisemaker ) },
        { "role_scientist.png", new tab( RoleTypes.Scientist ) },
        { "role_tracker.png", new tab( RoleTypes.Tracker ) },
        { "role_imposter.png", new tab( RoleTypes.Impostor ) },
        { "role_phantom.png", new tab( RoleTypes.Phantom ) },
        { "role_shape_shifter.png", new tab( RoleTypes.Shapeshifter ) },
        { "role_viper.png", new tab( RoleTypes.Viper ) },
    };
    
    private class page
    {
        public Rect rect = new Rect( 0, 0, 0, 0 );

        public page() { }
    }

    private page _page_settings = new page();

    private int _current_role_in_edit = ( int )RoleTypes.CrewmateGhost;
    private int _current_tab_index = 0;
    
    private float _button_size = 56f;
    private float _button_spacing_y = 10f;
    private float _button_spacing_x = 25f; 
    private float _animation_target_x = 0;
    private float _animation_x = 0;
}