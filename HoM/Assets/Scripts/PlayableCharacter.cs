using UnityEngine;

public class PlayableCharacter : Actor 
{
    public Weapon weapon;

    public POI handle_rightHand;


    public bool isStunned = false;
    public float invulnTime = 0.0f;


    public Actor myTarget = null;
    public float targetTime = 0.0f;


    public override void Start()
    {
        /*
        RaycastHit ray;
        Physics.Raycast(transform.position+(new Vector3(0,1000,0)), new Vector3(0, -1, 0), out ray);
        //
        transform.position = (ray.point + (new Vector3(0, 1, 0)));
        */
    }

    void Update()
    {
        if (!isPerforming)
        {
            isAttacking = false;
            isLanding = false;
            isStunned = false;

            if (animator.GetCurrentAnimatorStateInfo(1).IsName("attack")) { isAttacking = true; }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("air_down")) { isLanding = true; }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("dead")) { isStunned = true; }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("splatDead")) { isStunned = true; }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("hurt")) { isStunned = true; }
        }
        //
        //
        input.Read(this);
        //
        // *** Lose control when hurt/dead/stunned
        if (isStunned)
        {
            input.fight = false;
            input.moving = 0.0f;
            input.strafing = 0.0f;
            input.turning = 0.0f;
            input.jump = false;
            input.duck = false;
            input.autoRun = false;
        }
        //
        //
        Act_Motion();
        //
        //



        if (isAttacking)
        {
            weaponBox.gameObject.SetActive(true);
        }
        else
        {
            weaponBox.gameObject.SetActive(false);
        }


        bool isFalling = !isGrounded && groundTime > 0.1f;
        //
        isMoving = ((moveDirection.x != 0) || (moveDirection.z != 0)) && !isFalling;


        animator.SetFloat("motionVal", inputDirection.z);
        animator.SetBool("isAttacking", isAttacking);
        animator.SetFloat("strafeVal", Input.GetAxis("Strafing"));
        if (isFalling && Mathf.Abs(lastAirSpeed) > 5)
        {
            animator.SetFloat("fallVal", Mathf.Abs(lastAirSpeed));
        }
        else
        {
            animator.SetFloat("fallVal", 0);
            
        }

        //
        animator.SetBool("isFalling", isFalling);

        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isRunning", isMoving && ((Input.GetAxis("Run") != 0) ^ !walkByDefault));
        animator.SetBool("isGrounded", (isGrounded));
        //
        animator.SetBool("isDucking", isDucking && isGrounded);
    }

    private void FixedUpdate()
    {
        if (invulnTime > 0)
        {
            invulnTime -= Time.deltaTime;
        }

        if (targetTime > 0)
        {
            targetTime -= Time.deltaTime;
            //
            if (targetTime <= 0)
            {
                myTarget = null;
            }
        }
    }

    private void LateUpdate()
    {
        if (weapon != null)
        {
            weapon.transform.parent = handle_rightHand.transform.parent;
            //
            weapon.transform.localEulerAngles = new Vector3(0, 0, -90);
            weapon.transform.position = handle_rightHand.transform.position - (weapon.handlePoint.transform.position - weapon.transform.position);
        }
    }


    public override float TakeHit(float val)
    {
        if (isStunned) { return 0; }
        if (invulnTime > 0) { return 0; }


        float ret = base.TakeHit(val);
        //
        if (isDead)
        {
            animator.SetTrigger("dead");
        }
        else
        {
            if (val > 0)
            {
                invulnTime = 3.0f;

                if (isGrounded)
                {
                    animator.SetTrigger("hurt");
                }
            }
        }

        return ret;
    }

    public bool isDead = false;
    public override void Die()
    {
        base.Die();
        //
        if (!isDead)
        {
            isDead = true;
            hitpoints_c = 0;
            

            GameManager.Instance().DoGameOver();
        }
    }


    public override void OnDamageCaused(Actor to, float howmuch)
    {
        base.OnDamageCaused(to, howmuch);
        //
        myTarget = to;
        targetTime = 5.0f;
    }

    public override bool CanBeFoughtBy(Actor target)
    {
        return true; // *** The player has NO immunities whatsoever!
    }
    public override void DoFighting(Actor target)
    {
        base.DoFighting(target);
        //
        if (!target.CanBeFoughtBy(this))
        {
            // ??? <-- DEFLECT!!
        }
    }

    public override void OnGroundingChanged(bool grounded)
    {
        base.OnGroundingChanged(grounded);
        //
        if (grounded)
        {
            if (isDead)
            {
                animator.SetTrigger("splatDead");
            }
            else if (IsInZone(Zone.Types.Water)) {
                // *** Breaks your fall (not that this works in real life...)
            }
            else if (Mathf.Abs(lastAirSpeed) > 40)
            {
                Die();
                animator.SetTrigger("splatDead");
            }
            else if (Mathf.Abs(lastAirSpeed) > 20)
            {
                TakeHit(1);
                animator.SetTrigger("signal_land");
            }
            else if (Mathf.Abs(lastAirSpeed) > 7)
            {
                animator.SetTrigger("signal_land");
            }
        }
    }


    public override void OnZoneEntered(Zone.Types type)
    {
        base.OnZoneEntered(type);
        //
        switch (type)
        {
            case Zone.Types.Abyss:
                Die();
                break;
            case Zone.Types.Transition:
                Director.Instance().Cutscene_Begin(2);
                break;
        }
    }
}
