using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInputter
{ // : MonoBehaviour 

    internal float turning;
    internal float moving;
    internal float strafing;
    internal bool fight;
    internal bool jump;
    internal bool run;
    internal bool duck;
    internal bool autoRun;


    public virtual void Read(Actor c)
    {
        turning = 0;
        moving = 0;
        strafing = 0;
        fight = false;
        jump = false;
        run = false;
        duck = false;
        autoRun = false;
    }
}
