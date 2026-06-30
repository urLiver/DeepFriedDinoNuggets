using System;
using Il2CppSystem;
using UnityEngine;
using UnityEngine.UI;
using static Il2CppSystem.Xml.XmlWellFormedWriter.AttributeValueCache;

namespace DeepFriedDinoNuggets;

public class style
{
    public GUIStyle centered_label = null!;
    public GUIStyle button = null!;
    public GUIStyle tabbar_button_not_active = null!;
    public GUIStyle tabbar_button_active = null!;
    public GUIStyle tabbar_button_highlight = null!;
    public GUIStyle window = null!;
    public GUIStyle toggle_text = null!;
    public GUIStyle toggle_box_off = null!;   
    public GUIStyle toggle_box_on = null!;
    public GUIStyle color_picker_label = null!;
    public GUIStyle color_preset_btn = null!;
    
    public Texture2D CreateRoundedSquare( int width, int height, int radius, Color fill_color, Color border_color, int border_width )
    {
        Texture2D tex = new Texture2D( width, height, TextureFormat.RGBA32, false );
        tex.hideFlags = HideFlags.HideAndDontSave;

        for( int y = 0; y < height; y++ )
        {
            for( int x = 0; x < width; x++ )
            {
                int dx = Mathf.Min( x, width - 1 - x );
                int dy = Mathf.Min( y, height - 1 - y );

                if( dx < radius && dy < radius )
                {
                    int cx = radius - dx;
                    int cy = radius - dy;
                    float dist = Mathf.Sqrt( cx * cx + cy * cy );
                  
                    if( dist > radius )
                    {
                        tex.SetPixel( x, y, Color.clear );
                        continue;
                    }
                    else if( dist > radius - border_width )
                    {
                        tex.SetPixel( x, y, border_color );
                        continue;
                    }
                }
                else if( Mathf.Min( x, width - 1 - x ) < border_width || Mathf.Min( y, height - 1 - y ) < border_width )
                {
                    tex.SetPixel( x, y, border_color );
                    continue;
                }

                tex.SetPixel( x, y, fill_color );
            }
        }
        
        tex.Apply();
        return tex;
    }

    public void InitializeStyles()
    {
        if( _initialized )
        {
            return;
        }

        _initialized = true;
        
        Color not_active = new Color( 0.16f, 0.51f, 0.43f, 1.0f );
        Color active = new Color( 0.38f, 0.91f, 0.73f, 1.0f);
        Color black_outline = new Color( 0.05f, 0.08f, 0.07f, 1.0f );
        Color black_transparent = new Color( 0.04f, 0.04f, 0.06f, 0.88f ); 
        Color lighter_black_transparent = new Color( 0.08f, 0.08f, 0.12f, 0.88f ); 
        Color even_lighter_black_transparent = new Color( 0.12f, 0.12f, 0.18f, 0.88f ); 
        
        tabbar_button_not_active = new GUIStyle();
        tabbar_button_not_active.normal.background = CreateRoundedSquare( 64, 64, 14, not_active, black_outline, 3 );
        tabbar_button_not_active.alignment = TextAnchor.MiddleCenter;

        tabbar_button_active = new GUIStyle();
        tabbar_button_active.normal.background = CreateRoundedSquare( 64, 64, 14, active, black_outline, 3 );
        tabbar_button_active.alignment = TextAnchor.MiddleCenter;

        tabbar_button_highlight = new GUIStyle();
        tabbar_button_highlight.normal.background = CreateRoundedSquare( 64, 64, 14, Color.clear, Color.red, 5 );
        tabbar_button_highlight.alignment = TextAnchor.MiddleCenter;

        button = new GUIStyle();
        button.normal.background = CreateRoundedSquare( 64, 64, 14, not_active, black_outline, 3 );
        button.alignment = TextAnchor.MiddleCenter;
        button.normal.textColor = Color.white;
        button.fontSize = 14;
        button.fontStyle = FontStyle.Bold;
        
        centered_label = new GUIStyle( GUI.skin.label );
        centered_label.alignment = TextAnchor.MiddleCenter;
        centered_label.fontStyle = FontStyle.Bold;
        centered_label.fontSize = 14;
        centered_label.margin = new RectOffset();
        centered_label.padding = new RectOffset();

        window = new GUIStyle();
        window.normal.background = CreateRoundedSquare( 300, 300, 4, black_transparent, not_active, 2 );
        window.padding = new RectOffset();
        window.padding.left = 10;
        window.padding.right = 10;
        window.padding.top = 10;
        window.padding.bottom = 10;

        toggle_text = new GUIStyle();
        toggle_text.normal.textColor = Color.white;
        toggle_text.fontSize = 14;
        toggle_text.alignment = TextAnchor.MiddleLeft;
        toggle_text.margin = new RectOffset();
        toggle_text.margin.top = 1; 
        toggle_text.margin.bottom = 0;
        
        toggle_box_off = new GUIStyle();
        toggle_box_off.normal.background = CreateRoundedSquare( 18, 18, 5, lighter_black_transparent, even_lighter_black_transparent, 1 );
        toggle_box_off.fixedWidth = 18;
        toggle_box_off.fixedHeight = 18;
        toggle_box_off.margin = new RectOffset();
        toggle_box_off.margin.top = 0;
        toggle_box_off.margin.bottom = 0;
        
        toggle_box_on = new GUIStyle();
        toggle_box_on.normal.background = CreateRoundedSquare( 18, 18, 5, not_active, even_lighter_black_transparent, 1 );
        toggle_box_on.fixedWidth = 18;
        toggle_box_on.fixedHeight = 18;
        toggle_box_on.margin = new RectOffset();
        toggle_box_on.margin.top = 0;
        toggle_box_on.margin.bottom = 0;
        
        color_picker_label = new GUIStyle();
        color_picker_label.normal.textColor = Color.white;
        color_picker_label.fontSize = 14;
        color_picker_label.alignment = TextAnchor.MiddleLeft;
        color_picker_label.margin = new RectOffset();
        color_picker_label.margin.top = 2;
        color_picker_label.margin.bottom = 2;

        color_preset_btn = new GUIStyle();
        color_preset_btn.margin = new RectOffset();
        color_preset_btn.margin.top = 2;
        color_preset_btn.margin.bottom = 2;
        color_preset_btn.margin.left = 2;
        color_preset_btn.margin.right = 2;

        for( int i = 0; i < _color_picker_styles.Length; i++ )
        {
            _color_picker_styles[ i ] = new GUIStyle( color_preset_btn );
            _color_picker_styles[ i ].normal.background = CreateRoundedSquare( 32, 24, 4, _color_picker_colors[ i ], new Color( 0.05f, 0.08f, 0.07f, 1.0f ), 1 );
        }
    }

    public void RenderToggle( ref bool value, string text )
    {
        GUILayout.Space( 12 );

        GUILayout.BeginHorizontal();
        
        if ( GUILayout.Button( GUIContent.none, value ? toggle_box_on : toggle_box_off ) )
        {
            value = ! value;
        }

        GUILayout.Space( 8 );

        if ( GUILayout.Button( text, toggle_text ) )
        {
            value = ! value;
        }

        GUILayout.EndHorizontal();
    }

    public void RenderColorPicker( ref Color color, string text )
    {
        GUILayout.Space( 12 );

        GUILayout.BeginVertical();
        
        GUILayout.BeginHorizontal();

        GUIStyle preview = new GUIStyle();
        preview.normal.background = CreateRoundedSquare( 18, 18, 5, color, new Color( 0.05f, 0.08f, 0.07f, 1.0f ), 1 );

        GUILayout.Box( GUIContent.none, preview, GUILayout.Width( 18 ), GUILayout.Height( 18 ) );
        GUILayout.Space( 8 );
        GUILayout.Label( text, toggle_text );
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        GUILayout.Space( 8 );

        int columns = 8;
        for ( int i = 0; i < _color_picker_colors.Length; i++ )
        {
            if ( i % columns == 0 )
            {
                GUILayout.BeginHorizontal();
            }

            if ( GUILayout.Button( GUIContent.none, _color_picker_styles[ i ], GUILayout.ExpandWidth( true ), GUILayout.Height( 24 ) ) )
            {
                color = _color_picker_colors[ i ];
            }

            if ( i % columns == columns - 1 || i == _color_picker_colors.Length - 1 )
            {
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.EndVertical();
    }
    
    private bool _initialized = false;
    
	private readonly GUIStyle[] _color_picker_styles = new GUIStyle[ 32 ];

	private readonly Color32[] _color_picker_colors = new Color32[ 32 ]
    {
        new Color32( 255, 0, 0, 255 ),
        new Color32( 0, 0, 128, 255 ),
        new Color32( 127, 255, 212, 255 ),
        new Color32( 176, 196, 222, 255 ),
        new Color32( 255, 140, 0, 255 ),
        new Color32( 255, 105, 180, 255 ),
        new Color32( 139, 0, 0, 255 ),
        new Color32( 106, 90, 205, 255 ),
        new Color32( 189, 183, 107, 255 ),
        new Color32( 173, 255, 47, 255 ),
        new Color32( 0, 255, 0, 255 ),
        new Color32( 0, 255, 255, 255 ),
        new Color32( 255, 255, 0, 255 ),
        new Color32( 255, 0, 255, 255 ),
        new Color32( 128, 0, 128, 255 ),
        new Color32( 165, 42, 42, 255 ),
        new Color32( 255, 255, 255, 255 ),
        new Color32( 128, 128, 128, 255 ),
        new Color32( 0, 128, 0, 255 ),
        new Color32( 218, 165, 32, 255 ),
        new Color32( 255, 192, 203, 255 ),
        new Color32( 240, 230, 140, 255 ),
        new Color32( 230, 230, 250, 255 ),
        new Color32( 75, 0, 130, 255 ),
        new Color32( 255, 99, 71, 255 ),
        new Color32( 32, 178, 170, 255 ),
        new Color32( 135, 206, 235, 255 ),
        new Color32( 245, 222, 179, 255 ),
        new Color32( 255, 215, 0, 255 ),
        new Color32( 47, 79, 79, 255 ),
        new Color32( 0, 0, 0, 255 ),
        new Color32( 79, 79, 47, 255 )
    };
}