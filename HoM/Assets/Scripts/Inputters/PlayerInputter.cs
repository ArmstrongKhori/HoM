using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputter : BaseInputter {
    public override void Read(Actor c)
    {
        base.Read(c);
        //
        turning = Input.GetAxis("Horizontal");
        moving = Input.GetAxis("Vertical");
        strafing = Input.GetAxis("Strafing");
        fight = Input.GetAxis("Fire1") != 0;
        jump = Input.GetButton("Jump");
        run = Input.GetButton("Run");
        duck = Input.GetButton("Duck");
        autoRun = Input.GetButton("Toggle Move");
    }
}
