using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public InputField tWidth, tDepth, tHeight, tSeed;
    private int dWidth, dDepth, dHeight, dSeed;
    public Button saveButton, regenerateMapButton, regenerateSeedButton;

    void Start()
    {
        UpdateUI();
        saveButton.onClick.AddListener(UpdateMapDataFromUI);
        regenerateMapButton.onClick.AddListener(RegenerateMap);
        regenerateSeedButton.onClick.AddListener(RegenerateSeed);
    }

    void UpdateUI()
    {
        tWidth.text = MapData.Width.ToString();
        tDepth.text = MapData.Depth.ToString();
        tHeight.text = MapData.Height.ToString();
        tSeed.text = MapData.Seed.ToString();
    }

    void UpdateMapDataFromUI()
    {
        // Try to parse each text field; if parsing fails, keep the original value
        int temp;
        if (Int32.TryParse(tWidth.text, out temp)) dWidth = temp;
        if (Int32.TryParse(tDepth.text, out temp)) dDepth = temp;
        if (Int32.TryParse(tHeight.text, out temp)) dHeight = temp;
        if (Int32.TryParse(tSeed.text, out temp)) dSeed = temp;

        // Update MapData
        UpdateMapData();

        // Update UI
        UpdateUI();
    }

    void UpdateMapData()
    {
        MapData.Width = dWidth;
        MapData.Depth = dDepth;
        MapData.Height = dHeight;
        MapData.Seed = dSeed;
    }

    void RegenerateSeed()
    {
        MapData.Seed = MapData.GenerateRandomSeed();
        UpdateUI();
    }

    void RegenerateMap()
    {
        UpdateMapDataFromUI();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}