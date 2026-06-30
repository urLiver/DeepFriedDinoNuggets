using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DeepFriedDinoNuggets;

public class resources
{
    private string resource_path;
    private string image_path;
    
    public resources()
    {
        resource_path = Path.Combine( Directory.GetCurrentDirectory(), "resources" );
        image_path = Path.Combine( resource_path, "images" );
    }

    private Dictionary<string, Texture2D> cached_image_textures = new Dictionary<string, Texture2D>();
    private Dictionary<string, GUIStyle> cached_image_styles = new Dictionary<string, GUIStyle>();

    private void SetupImage( string file_name )
    {
        string full_path = Path.Combine( image_path, file_name );

        byte[] data = File.ReadAllBytes( full_path );

        Texture2D temporary_texture = new Texture2D( 2, 2 );
        
        ImageConversion.LoadImage( temporary_texture, data );

        Texture2D texture = new Texture2D( temporary_texture.width, temporary_texture.height, TextureFormat.RGBA32, false );
        texture.hideFlags = HideFlags.HideAndDontSave;
        texture.hideFlags = HideFlags.HideAndDontSave;
        texture.filterMode = FilterMode.Bilinear;
        texture.wrapMode = TextureWrapMode.Clamp;

        Color[] pixels = temporary_texture.GetPixels();
        UnityEngine.Object.Destroy( temporary_texture );

        texture.SetPixels( pixels );
        texture.Apply();

        cached_image_textures[ file_name ] = texture;

        GUIStyle style = new GUIStyle();
        style.normal.background = texture;
        style.stretchWidth = true;
        style.stretchHeight = true;

        cached_image_styles[ file_name ] = style;
    }

    public Texture2D GetTextureForIcon( string file_name )
    {
        top:
        if( cached_image_textures.TryGetValue( file_name, out Texture2D existingTex ) && existingTex != null )
        {
            return existingTex;
        }

        SetupImage( file_name );

        goto top;
    }
    
    public GUIStyle GetStyleForIcon( string file_name )
    {
        top:

        if ( cached_image_styles.TryGetValue( file_name, out GUIStyle existingStyle ) && existingStyle != null )
        {
            return existingStyle;
        }

        SetupImage( file_name );

        goto top;
    }
}
