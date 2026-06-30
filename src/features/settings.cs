using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmongUs.GameOptions;
using HarmonyLib;
using UnityEngine;
using UnityEngine.ProBuilder;

namespace DeepFriedDinoNuggets;

public class settings
{
    public struct esp_role_settings
    {
        public bool enabled = false;
        public bool esp_respects_visibility = false;
        public bool display_box = false;
        public bool display_role_name = false;
        public bool display_role_icon = false;
        public bool display_role_pointer = false;
        public Color color = Color.white;
        public bool visibile_all_time = false;

        public esp_role_settings() { }
    };

    public esp_role_settings[] esp_role_setting = new esp_role_settings[ 20 ];

    public void InitalizeSettings()
    {
        for( int i = 0; i < 20; i++ )
        {
            esp_role_setting[ i ] = new esp_role_settings();
        }
    }
}
