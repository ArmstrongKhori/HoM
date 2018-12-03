using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Artist : SystemObj {


    public Texture2D square;
    public Texture2D heart_full;
    public Texture2D heart_empty;
    public Texture2D sword;
    public Texture2D enemy;

    public Text screenText;

    GUIStyle swordFontStyle;
    GUIStyle gameoverStyle;
    private void Start()
    {
        swordFontStyle = new GUIStyle();
        swordFontStyle.fontSize = 72;
        swordFontStyle.normal.textColor = Color.yellow;


        gameoverStyle = new GUIStyle();
        gameoverStyle.fontSize = 72;
        gameoverStyle.normal.textColor = Color.white;
        gameoverStyle.alignment = TextAnchor.MiddleCenter;
    }

    private void OnGUI()
    {

        Director d = Director.Instance();
        GameManager gm = GameManager.Instance();
        Roomgrid rg = Roomgrid.Instance();
        if (!rg) { return; }

        if (d.isCutsceneMode)
        {
            GUI.color = Color.black;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, 100), square);
            GUI.color = Color.black;
            GUI.DrawTexture(new Rect(0, Screen.height - 100, Screen.width, 100), square);
        }
        else
        {
            float jist1 = Helper.Longevity(rg.zoneTimer, 1.0f, 0.3f);
            float jist2 = Helper.Longevity(rg.zoneTimer, 1.0f + 3.0f, 0.3f);
            // Debug.Log(screenText.rectTransform.localPosition);
            screenText.text = "Take Heart!\nNow Entering: " + rg.zoneName;
            screenText.rectTransform.localPosition = new Vector3(Screen.width * (1 - jist1) - Screen.width * (jist2), 0);


            for (int i = 0; i < gm.player.hitpoints_m; i++)
            {
                int p = i % 10;
                int n = i / 10;
                float xx, yy;
                xx = 16 + p * (80 - 8) + (p - 1) * (4);
                yy = 16 + n * (80 - 8) + (n - 1) * (4);
                GUI.DrawTexture(new Rect(xx, yy, 80, 80), (i < gm.player.hitpoints_c) ? heart_full : heart_empty);

            }


            
            GUI.DrawTexture(new Rect(16, 16 + 100, 80, 80), sword);
            GUI.Label(new Rect(16 + 80, 16 + 100, 200, 80), "Lv." + gm.player.pwrLevel, swordFontStyle);


            if (gm.player.myTarget != null)
            {
                float SCL = 2.0f;
                GUI.DrawTexture(new Rect(Screen.width - 16 - 80 * SCL, 16, 80 * SCL, 80 * SCL), enemy);
                //
                GUI.color = Color.magenta;
                GUI.DrawTexture(new Rect(Screen.width - 16 - 80 * SCL - 240 * SCL, 16 + 80 / 2 * SCL, 240 * SCL, 16 * SCL), square);
                GUI.color = Color.black;
                GUI.DrawTexture(new Rect(Screen.width - 16 - 80 * SCL - 240 * SCL + 1, 16 + 80 / 2 * SCL + 1, 240 * SCL - 1 * 2, 16 * SCL - 1 * 2), square);
                GUI.color = Color.red;
                GUI.DrawTexture(new Rect(Screen.width - 16 - 80 * SCL - 240 * SCL + 1, 16 + 80 / 2 * SCL + 1, (240 * Mathf.Max(0, gm.player.myTarget.hitpoints_c / gm.player.myTarget.hitpoints_m)) * SCL - 1 * 2, 16 * SCL - 1 * 2), square);
            }
        }


        if (gm.gameoverTime > 1.5f)
        {
            GUI.color = new Color(0, 0, 0, 1.0f * Helper.Longevity(gm.gameoverTime, 1.5f, 2.5f));
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), square);
            //
            if (gm.gameWon)
            {
                GUI.color = new Color(1, 1, 1, 1.0f * Helper.Longevity(gm.gameoverTime, 1.5f + 2.0f, 0.5f)); 
                GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "End of demo", gameoverStyle);
            }
            else
            {
                GUI.color = new Color(1, 1, 1, 1.0f * Helper.Longevity(gm.gameoverTime, 1.5f, 2.5f));
                GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "You are dead!\nGame Over", gameoverStyle);
            }
        }
    }




    public static Artist instance;
    public static Artist Instance() { return instance; }
    public override void InitializeMe()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        //
        DontDestroyOnLoad(this.gameObject);
    }
}
