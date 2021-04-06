using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarColor : MonoBehaviour
{
    public Color color;

    public Texture albedoB;
    public Texture albedoD;

    public Material BodyColor;
    public Material DoorColor;

    public GameObject body;
    public GameObject door;

    MeshRenderer bodyMesh;
    MeshRenderer doorMesh;

    Material newBody;
    Material newDoor;
    // Start is called before the first frame update
    void Start()
    {
        newBody = new Material(BodyColor);
        newDoor = new Material(DoorColor);

        newBody.color = color;
        newDoor.color = color;
        

        newBody.mainTexture = albedoB;
        newDoor.mainTexture = albedoD;

        bodyMesh = body.GetComponent<MeshRenderer>();
        doorMesh = door.GetComponent<MeshRenderer>();

        bodyMesh.material = newBody;
        doorMesh.material = newDoor;
    }

    // Update is called once per frame
    void Update()
    {
        newBody.color = color;
        newDoor.color = color;


        newBody.mainTexture = albedoB;
        newDoor.mainTexture = albedoD;

        bodyMesh.material = newBody;
        doorMesh.material = newDoor;
    }
}
