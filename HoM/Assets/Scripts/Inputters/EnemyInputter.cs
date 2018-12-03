using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInputter : BaseInputter {

    public EnemyNPC.Types enemyType;
    public EnemyInputter(EnemyNPC.Types type)
    {
        enemyType = type;
    }

    public override void Read(Actor c)
    {
        base.Read(c);
        //
        EnemyNPC me = (EnemyNPC)c;
        GameManager gm = GameManager.Instance();

        if (!c.IsDead())
        {
            switch (enemyType)
            {
                case EnemyNPC.Types.Basic:
                    if (me.myTarget == null)
                    {
                        // ??? <-- A bit cheesy/cheaty, but the AI will always know about the player, per se.
                        RaycastHit ray = new RaycastHit();
                        Physics.Raycast(me.transform.position, (gm.player.transform.position - me.transform.position), out ray, 10f);
                        if (true || (ray.collider != null & ray.collider.gameObject == gm.player.gameObject)) // ??? <-- Just work for now.
                        {
                            me.myTarget = gm.player;
                        }
                    }
                    else
                    {
                        float dist = Vector3.Distance(me.transform.position, me.myTarget.transform.position);
                        if (dist < 1.3f)
                        {
                            fight = true;
                        }
                        else if (dist < 10)
                        {
                            float angDiff = Mathf.DeltaAngle(Helper.Direction(me.transform.position, me.myTarget.transform.position), Helper.Roundabout(180 + me.transform.eulerAngles.y, 360));
                            float limited = Mathf.Max(0, Mathf.Abs(angDiff) - 3); // *** 3 degree leeway for tracking (to prevent "swaying")
                            turning = Mathf.Sign(angDiff) * Mathf.Sign(limited);

                            // *** If you're getting close to seeing the target, move towards it!
                            if (angDiff < 30)
                            {
                                moving = 1;
                            }
                        }
                        else
                        {
                            me.myTarget = null;
                        }
                    }
                    break;
            }
        }
    }
}
