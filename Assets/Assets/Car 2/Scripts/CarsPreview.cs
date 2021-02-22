using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CarsPreview : MonoBehaviour {
    [Header("Cars")]
    public GameObject CurrentCar;
    public GameObject[] Cars;
    public int CarsIndex = 0;
    [Header("Arrows")]
    public Texture LeftArrow;
    public Texture RightArrow;
    [Header("TopFrame")]
    public Texture TopFrame;

    private Vector3 Mouse;

    private void Start()
    {
        foreach (GameObject Car in Cars)
        {
            Car.SetActive(false);
        }
        CarsIndex = 0;
        Cars[0].SetActive(true);
    }

    private void Update()
    {
        CurrentCar = Cars[CarsIndex];
        
        for(int i = 0; i == Cars.Length; i++)
        {
            if (i == CarsIndex) continue;
            Cars[i].SetActive(false);
        }

        Mouse = Input.mousePosition;
        Mouse.y = Screen.height - Mouse.y;
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
        Rect Left = new Rect(80, 480, 100, 100);
        Rect Right = new Rect(1740, 480, 100, 100);
        Rect TopFrameRect = new Rect(200, 80, 1520, 70);

        //Content
        GUI.DrawTexture(TopFrameRect, TopFrame);

        if (CarsIndex > 0)
        {
            if (GUI.Button(Left, LeftArrow))
            {
                Cars[CarsIndex].SetActive(false);
                CarsIndex = CarsIndex - 1;
                Cars[CarsIndex].SetActive(true);
            }
        }

        if (CarsIndex < 1)
        {
            if (GUI.Button(Right, RightArrow))
            {
                Cars[CarsIndex].SetActive(false);
                CarsIndex = CarsIndex + 1;
                Cars[CarsIndex].SetActive(true);
            }
        }
    }
}
