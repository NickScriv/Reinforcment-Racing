using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSelect : MonoBehaviour {
    [Header("Colors")]
    public Color[] Colors;    
    public int ColorIndex;
    public Color CurrentColor;
    [Header("ColorsPreview")]
    public Texture[] Textures;
    public int TextureIndex;
    public Texture CurrentTexture;
    [Header("Albendos")]
    public Texture[] AlbedoB;
    public Texture[] AlbedoD;
    public int AlbendoIndex;
    public Texture CurrentAlbedoB;
    public Texture CurrentAlbedoD;
    [Header("Materials")]
    public Material BodyColor;
    public Material DoorColor;
    [Header("Arrows")]
    public Texture LeftArrow;
    public Texture RightArrow;
    [Header("Miscelous")]
    public Texture ColorTexture;
    public Texture TextureTexture;

	void Start () {
        Colors = new Color[10];
        Colors[0] = Color.black;
        Colors[1] = new Color32(210,0,0,255); //red
        Colors[2] = new Color32(255,0,255,255); //magenta
        Colors[3] = Color.blue;
        Colors[4] = new Color32(19,94,0,255); //green
        Colors[5] = new Color32(255,235,0,255); //yellow
        Colors[6] = Color.white;
        Colors[7] = new Color32(73,73,73,255); //Grey
        Colors[8] = new Color32(0, 181, 255,255); //cyan
        Colors[9] = new Color32(255,47,0,255); //orange

        CurrentColor = Colors[ColorIndex];

        //Textures = new Texture[10];
        CurrentTexture = Textures[TextureIndex];
    }

	void Update () {
        BodyColor.color = CurrentColor;
        DoorColor.color = CurrentColor;
        CurrentColor = Colors[ColorIndex];

        CurrentTexture = Textures[TextureIndex];

        CurrentAlbedoB = AlbedoB[AlbendoIndex];
        CurrentAlbedoD = AlbedoD[AlbendoIndex];
        BodyColor.mainTexture = CurrentAlbedoB;
        DoorColor.mainTexture = CurrentAlbedoD;
        if(AlbendoIndex == 0)
        {
            BodyColor.mainTexture = null;
            DoorColor.mainTexture = null;
        }
    }

    private void OnGUI()
    {
        //Screen scaling
        Vector3 scale;
        float originalWidht = 1920;
        float originalHeight = 1080;

        scale.x = Screen.width / originalWidht;
        scale.y = Screen.height / originalHeight;
        scale.z = 1;
        Matrix4x4 svMat = GUI.matrix;
        GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, scale);

        GUI.backgroundColor = Color.clear;

        //Rects
        Rect ColorRect = new Rect(575, 900, 355, 45);
        Rect TextureRect = new Rect(950, 900, 355, 45);
        Rect CAlbendoRect = new Rect(1005, 955, 235, 45);
        Rect CTextureRect = new Rect(630, 955, 235, 45);
        Rect LeftAlbendoRect = new Rect(950, 955, 45, 45);
        Rect RightAlbendoRect = new Rect(1260, 955, 45, 45);
        Rect LefColorRect = new Rect(575, 955, 45, 45);
        Rect RightColorRect = new Rect(885, 955, 45, 45);

        //Content
        GUI.DrawTexture(CTextureRect, CurrentTexture);
        GUI.DrawTexture(CAlbendoRect, CurrentAlbedoB);
        GUI.DrawTexture(ColorRect, ColorTexture);
        GUI.DrawTexture(TextureRect, TextureTexture);

        if (ColorIndex > 0)
        {
            if (GUI.Button(LefColorRect, LeftArrow))
            {
                ColorIndex = ColorIndex - 1;
                TextureIndex = TextureIndex - 1;
            }
        }
        if (ColorIndex < 9)
        {
            if (GUI.Button(RightColorRect, RightArrow))
            {
                ColorIndex = ColorIndex + 1;
                TextureIndex = TextureIndex + 1;
            }
        }

        if (AlbendoIndex > 0)
        {
            if (GUI.Button(LeftAlbendoRect, LeftArrow))
            {
                ColorIndex = 6;
                TextureIndex = 6;
                AlbendoIndex = AlbendoIndex - 1;
            }
        }
        if (AlbendoIndex < 3)
        {
            if (GUI.Button(RightAlbendoRect, RightArrow))
            {
                ColorIndex = 6;
                TextureIndex = 6;
                AlbendoIndex = AlbendoIndex + 1;
            }
        }

    }
}
