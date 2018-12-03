using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roomgrid : SystemObj {

    public float zoneTimer = 0;
    // ??? <-- Hard-coded... For now.
    public string zoneName = "Eastern Highlands";

	// Update is called once per frame
	void FixedUpdate () {
        if (!Director.Instance().isCutsceneMode)
        {
            zoneTimer += Time.deltaTime;
        }
    }



    public static Roomgrid instance;
    public static Roomgrid Instance() { return instance; }
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
