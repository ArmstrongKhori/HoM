using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNPC : NPC
{

    public Actor myTarget;

    public enum Types
    {
        None,
        /// <summary>
        /// Moves towards the player when they get close. Presses the "Attack" button when they get close enough.
        /// </summary>
        Basic,
    }
    public Types type;

    public override void Start()
    {
        base.Start();
        //
        input = new EnemyInputter(type);
    }



    void Update()
    {
        input.Read(this);
        //
        //
        Act_Motion();
        //
        //
        if (input.fight)
        {
            weaponBox.gameObject.SetActive(true);
        }
        else
        {
            weaponBox.gameObject.SetActive(false);
        }


        bool isFalling = !isGrounded && groundTime > 0.1f;
        //
        isAttacking = (input.fight || animator.GetCurrentAnimatorStateInfo(0).IsName("attack"));
        isMoving = ((moveDirection.x != 0) || (moveDirection.z != 0)) && !isFalling;
        //
        //
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isFalling", isFalling);
        animator.SetBool("isAttacking", isAttacking);
    }



    public override bool CanBeFoughtBy(Actor target)
    {
        return target.pwrLevel >= pwrLevel;
    }

    public override void Die()
    {
        base.Die();
        //
        animator.SetTrigger("dead");

        // *** Turn off their darkness aura to make it clear they're dead!
        transform.Find("(particles)").Find("malice").gameObject.GetComponent<ParticleSystem>().Stop();
    }
}
