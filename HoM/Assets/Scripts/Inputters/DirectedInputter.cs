using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectedInputter : BaseInputter {

    internal string inputID;
    public DirectedInputter(string id)
    {
        inputID = id;
    }

    internal string command;
    internal float howlong;

    public override void Read(Actor c)
    {
        base.Read(c);
        //
        Director d = Director.Instance();


        if (howlong > 0)
        {
            switch (command)
            {
                case "walk_forward":
                    moving = 1;
                    break;
            }
            //
            howlong -= Time.deltaTime;
            //
            if (howlong <= 0)
            {
                command = "";
                howlong = 0;
            }
        }
        
    }


    public void Direct(string action, float duration = 999f)
    {
        command = action;
        howlong = duration;
    }
}
