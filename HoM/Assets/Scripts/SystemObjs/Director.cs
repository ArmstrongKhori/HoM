using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : SystemObj {

    public bool isCutsceneMode = false;
    public int cutsceneID = -1;



    public float cutsceneTime = 0.0f;
    public void Cutscene_Begin(int id)
    {
        isCutsceneMode = true;
        cutsceneTime = 0.0f;
        cutsceneID = id;
        //
        GameManager gm = GameManager.Instance();
        gm.player.input = new DirectedInputter("player");
        gm.camera.lockedMode = true;
        //

    }
    public void Cutscene_Finish()
    {
        isCutsceneMode = false;
        //
        GameManager gm = GameManager.Instance();
        gm.player.input = new PlayerInputter();

        gm.camera.target = gm.player.transform;
        gm.camera.lockedMode = false;
        //

    }


    private void FixedUpdate()
    {
        if (isCutsceneMode)
        {
            cutsceneTime += Time.deltaTime;
            //
            RunCutscene(cutsceneID);
        }
    }


    public void RunCutscene(int id)
    {
        GameManager gm = GameManager.Instance();
        switch (id)
        {
            case 1:
                gm.camera.lockedMode = false;
                gm.camera.target = transform.Find("cutscene_001");

                if (cutsceneTime < 2.0f)
                {
                    GetActor("player").Direct("walk_forward");
                }

                if (cutsceneTime > 3.0f)
                {
                    Cutscene_Finish();
                }
                break;
            case 2:
                if (cutsceneTime < 2.0f)
                {
                    gm.gameWon = true;
                    gm.DoGameOver();
                    GetActor("player").Direct("walk_forward");
                }
                break;
        }
    }



    public DirectedInputter GetActor(string id)
    {
        foreach (PlayableCharacter a in FindObjectsOfType<PlayableCharacter>())
        {
            Debug.Log(a);
            Debug.Log(a.input);
            DirectedInputter di = (DirectedInputter)a.input;
            if (di != null)
            {
                if (di.inputID == id) { return di; }
            }
        }

        return null;
    }


    public static Director instance;
    public static Director Instance() { return instance; }
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
