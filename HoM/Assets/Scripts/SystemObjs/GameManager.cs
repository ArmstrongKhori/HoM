using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SystemObj
{


    internal List<SystemObj> allSystems;
    //
    internal PlayableCharacter player;
    internal RPGCameraController camera;


    // Use this for initialization
    void Start () {

        allSystems = new List<SystemObj>();


        allSystems.Clear();
        //
        allSystems.Add(this); // *** Game Manager is always at the top of the list.
        //
        foreach (SystemObj so in FindObjectsOfType<SystemObj>())
        {
            if (!allSystems.Contains(so))
            {
                allSystems.Add(so);
            }
        }
        //
        foreach (SystemObj so in allSystems)
        {
            so.InitializeMe();
        }




        player = FindObjectOfType<PlayableCharacter>();
        camera = FindObjectOfType<RPGCameraController>();
        //
        //
        Director.Instance().Cutscene_Begin(1);
    }



    public bool gameOver = false;
    public float gameoverTime = 0.0f;
    //
    public bool gameWon = false;

    public void DoGameOver()
    {
        if (!gameOver)
        {
            gameOver = true;
            gameoverTime = 0.0f;

            camera.lockedMode = true;
        }
    }


    private void FixedUpdate()
    {
        if (camera.lockedMode)
        {
            camera.transform.LookAt(player.transform);
        }

        if (gameOver)
        {
            gameoverTime += Time.deltaTime;
        }
    }


    public static GameManager instance;
    public static GameManager Instance() { return instance; }
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
