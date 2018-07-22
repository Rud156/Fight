using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.PostProcessing.Utilities;
using UnityEngine.UI;

public class AddScreenOverlay : MonoBehaviour
{

    [Header("Chromatic Aberration Texture")]
    public Texture2D overlayTexture;

    [Header("Post Processing Stack")]
    public PostProcessingController controller;

    public void TurnOnChromaticAberration()
    {
        controller.enableChromaticAberration = true;
        controller.chromaticAberration.spectralTexture = overlayTexture;
        controller.chromaticAberration.intensity = 1;
    }

    public void TurnOffChromaticAberration()
    {
        controller.enableChromaticAberration = false;
    }
}
