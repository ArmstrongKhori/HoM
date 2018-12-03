using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Actor {

    public override void Start()
    {
        base.Start();
        //
        // input = new EnemyInputter();
    }


    private void FixedUpdate()
    {
        if (invulnTime > 0)
        {
            invulnTime -= Time.deltaTime;
        }
    }


    internal float invulnTime = 0.0f;
    public override float TakeHit(float val)
    {
        if (invulnTime > 0) { return 0; }


        invulnTime = 1.0f;
        //
        return base.TakeHit(val);
    }
}
