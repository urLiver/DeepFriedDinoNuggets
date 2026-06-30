using System;
using InnerNet;
using UnityEngine;

namespace DeepFriedDinoNuggets;

public class events : MonoBehaviour
{
    public void Update()
    {
        plugins.global_user_interface.Update();
    }

    public void OnGUI()
    {
        plugins.global_style.InitializeStyles();

        plugins.global_user_interface.OnGui();
    }
}