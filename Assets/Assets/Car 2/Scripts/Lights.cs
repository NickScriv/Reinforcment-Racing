using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : MonoBehaviour {

    private bool on = false;
    private bool on2 = false;
    private bool on3 = false;

    private bool BrakeL = false;
    private bool ReverseL = false;
    private bool FrontL = false;
    private bool TailL = false;
    private bool IndicatorL = false;
    private bool IndicatorR = false;
    private bool IAIcon = false;
    [Header("Lights Objects")]
    public GameObject FL;
    public GameObject BL;
    public GameObject TL;
    public GameObject RL;
    public GameObject IL;
    public GameObject IR;
    [Header("Materials")]
    public Material FLM;
    public Material TLM;
    public Material RLM;
    public Material ILM;
    public Material IRM;

    private float timer = 0.5f;
    private float timer2 = 0.5f;
    private float btimer = 0.5f;
    private float btimer2 = 0.5f;
    [Header("Lights Styles")]
    public GUIContent FLB;
    public GUIContent TLB;
    public GUIContent BLB;
    public GUIContent ILB;
    public GUIContent IRB;
    public GUIContent RLB;
    public GUIContent IAB;
    [Header("Normal Light Buttons")]
    public Texture FLN;
    public Texture BLN;
    public Texture TLN;
    public Texture ILN;
    public Texture IRN;
    public Texture RLN;
    public Texture IAN;
    [Header("Active Light Buttons")]
    public Texture FLA;
    public Texture BLA;
    public Texture TLA;
    public Texture ILA;
    public Texture IRA;
    public Texture RLA;
    public Texture IAA;

    private void Start()
    {
        FL = GameObject.Find("FrontLights");
        BL = GameObject.Find("BrakeLights");
        TL = GameObject.Find("TailLights");
        RL = GameObject.Find("ReverseLights");
        IL = GameObject.Find("LeftIndicators");
        IR = GameObject.Find("RightIndicators");

        FL.SetActive(false);
        BL.SetActive(false);
        TL.SetActive(false);
        RL.SetActive(false);
        IL.SetActive(false);
        IR.SetActive(false);

        FLM.DisableKeyword("_EMISSION");
        TLM.DisableKeyword("_EMISSION");
        RLM.DisableKeyword("_EMISSION");
        ILM.DisableKeyword("_EMISSION");
        IRM.DisableKeyword("_EMISSION");
    }

    private void Update()
    {
        if (IndicatorL)
        {
            if (timer >= 0f)
            {
                timer -= Time.deltaTime;
                IL.SetActive(true);
                if (IAIcon)
                {
                    IAB.image = IAA;
                    ILB.image = ILN;
                }
                else
                {
                    ILB.image = ILA;
                    IAB.image = IAN;
                }
                ILM.EnableKeyword("_EMISSION");
                timer2 = 0.5f;
            }
            if(timer <= 0f)
            {
                if (!IAIcon) ILB.image = ILN;
                if (IAIcon) IAB.image = IAN;
                IL.SetActive(false);
                ILM.DisableKeyword("_EMISSION");
                timer2 -= Time.deltaTime;
                if (timer2 <= 0f) timer = 0.5f;
            }
        }
        else
        {
            IL.SetActive(false);
            if (!IAIcon) ILB.image = ILN;
            if (IAIcon) IAB.image = IAN;
            ILM.DisableKeyword("_EMISSION");
        }

        if (IndicatorR)
        {
            if (btimer >= 0f)
            {
                btimer -= Time.deltaTime;
                IR.SetActive(true);
                if (IAIcon)
                {
                    IAB.image = IAA;
                    IRB.image = IRN;
                }
                else
                {
                    IRB.image = IRA;
                    IAB.image = IAN;
                }
                IRM.EnableKeyword("_EMISSION");
                btimer2 = 0.5f;
            }
            if (btimer <= 0f)
            {
                IR.SetActive(false);
                if (!IAIcon) IRB.image = IRN;
                if (IAIcon) IAB.image = IAN;
                IRM.DisableKeyword("_EMISSION");
                btimer2 -= Time.deltaTime;
                if (btimer2 <= 0f) btimer = 0.5f;
            }
        }
        else
        {
            IR.SetActive(false);
            if(!IAIcon) IRB.image = IRN;
            if (IAIcon) IAB.image = IAN;
            IRM.DisableKeyword("_EMISSION");
        }
            
    }

    private void OnGUI()
    {
        //Rects
        Rect Rect1 = new Rect(200, 170, 80, 80);
        Rect Rect2 = new Rect(440, 170, 80, 80);
        Rect Rect3 = new Rect(680, 170, 80, 80);
        Rect Rect4 = new Rect(920, 170, 80, 80);
        Rect Rect5 = new Rect(1160, 170, 80, 80);
        Rect Rect6 = new Rect(1400, 170, 80, 80);
        Rect Rect7 = new Rect(1640, 170, 80, 80);

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

        //Buttons
        if (GUI.Button(Rect1, FLB))
        {
            if (FrontL == false)
            {
                FLB.image = FLA;
                FrontL = true;
                FL.SetActive(true);
                FLM.EnableKeyword("_EMISSION");
            }
            else {
                FLB.image = FLN;
                FrontL = false;
                FL.SetActive(false);
                FLM.DisableKeyword("_EMISSION");
            }
        }
        if (GUI.Button(Rect2, TLB))
        {
            if (TailL == false)
            {
                TLB.image = TLA;
                TailL = true;
                TL.SetActive(true);
                TLM.EnableKeyword("_EMISSION");
            }
            else {
                TLB.image = TLN;
                TailL = false;
                TL.SetActive(false);
                TLM.DisableKeyword("_EMISSION");
            }
        }
        if (TailL) {
            if (GUI.Button(Rect3, BLB))
            {
                if (BrakeL == false)
                {
                    BLB.image = BLA;
                    BrakeL = true;
                    BL.SetActive(true);
                }
                else
                {
                    BLB.image = BLN;
                    BrakeL = false;
                    BL.SetActive(false);
                }
            }
        }
        if (GUI.Button(Rect4, ILB))
        {
            IAIcon = false;
            timer = 0.5f;
            IndicatorL = false;
            IndicatorR = false;
            if (on2 == false)
            {
                ILB.image = ILA;
                on2 = true;
                on = false;
                on3 = false;
                if (IndicatorL == false)
                {
                    IndicatorL = true;
                    IndicatorR = false;
                    /*IL.active = true;
                    ILM.EnableKeyword("_EMISSION");*/
                }
            }
            else
            {
                ILB.image = ILN;
                on2 = false;
                IndicatorL = false;
                /*IL.active = false;
                ILM.DisableKeyword("_EMISSION");*/
            }
        }
        if (GUI.Button(Rect5, IRB))
        {
            IAIcon = false;
            btimer = 0.5f;
            IndicatorL = false;
            IndicatorR = false;
            if (on == false)
            {
                IRB.image = IRA;
                on = true;
                on2 = false;
                on3 = false;
                if (IndicatorR == false)
                {
                    IndicatorR = true;
                    IndicatorL = false;
                    //IR.active = true;
                    //IRM.EnableKeyword("_EMISSION");
                }
            }
            else
            {
                IRB.image = IRN;
                on = false;
                IndicatorR = false;
                //IR.active = false;
                //IRM.DisableKeyword("_EMISSION");
            }
        }
        if (GUI.Button(Rect6, RLB))
        {
            if (ReverseL == false)
            {
                RLB.image = RLA;
                ReverseL = true;
                RL.SetActive(true);
                RLM.EnableKeyword("_EMISSION");
            }
            else
            {
                RLB.image = RLN;
                ReverseL = false;
                RL.SetActive(false);
                RLM.DisableKeyword("_EMISSION");
            }
        }
            if (GUI.Button(Rect7, IAB))
        {
            IAIcon = true;
            btimer = 0.5f;
            timer = 0.5f;
            IndicatorL = false;
            IndicatorR = false;
            if (on3 == false)
            {
                IAB.image = IAA;
                on3 = true;
                on = false;
                on2 = false;
                if (IndicatorL == false && IndicatorR == false)
                {
                    IndicatorL = true;
                    IndicatorR = true;
                }
            }
            else
            {
                IAIcon = false;
                IAB.image = IAN;
                on3 = false;
                IndicatorL = false;
                IndicatorR = false;
            }
        }
    }

}
