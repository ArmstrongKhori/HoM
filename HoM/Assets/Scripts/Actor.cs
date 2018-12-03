using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {

    internal float hitpoints_c = 3;
    public float hitpoints_m = 3;
    public int pwrLevel = 12;




    //Movement speeds
    public float jumpSpeed = 8.0f;
    public float runSpeed = 10.0f;
    public float walkSpeed = 4.0f;
    public float turnSpeed = 250.0f;
    public float moveBackwardsMultiplier = 0.75f;

    //Internal vars to work with
    private float speedMultiplier = 0.0f;
    private bool mouseSideButton = false;

    public bool walkByDefault = true;
    public float gravity = 20.0f;
    private bool isWalking = false;


    public bool isGrounded = false;
    public float groundTime = 0.0f;
    public bool isMoving = false;
    public bool isDucking = false;
    public bool isAttacking = false;
    public bool isLanding = false;

    internal Vector3 inputDirection = Vector3.zero;
    internal Vector3 moveDirection = Vector3.zero;
    /// <summary>
    /// Should the character's animation parameters automatically change?
    /// </summary>
    public bool isPerforming = false;

    public float lastAirSpeed = 0;
    public bool lastGrounded = false;

    public BaseInputter input;

    public Animator animator;
    private CharacterController controller;



    internal HitArea weaponBox;

    void Awake()
    {
        _zoneInteraction = new Dictionary<Zone.Types, int>();
        foreach (Zone.Types z in System.Enum.GetValues(typeof(Zone.Types)))
        {
            _zoneInteraction.Add(z, 0);
        }


        animator = transform.Find("model").GetComponent<Animator>();


        //Get CharacterController
        controller = GetComponent<CharacterController>();
        if (controller == null) {
            controller = gameObject.AddComponent<CharacterController>();
            controller.slopeLimit = 45;
            controller.stepOffset = 0.3f;
            controller.skinWidth = 0.08f;
            controller.minMoveDistance = 0.001f;
            controller.center = new Vector3(0, 0, 0);
            controller.radius = 0.5f;
            controller.height = 1;
        }


        weaponBox = transform.Find("(hitboxes)").Find("attack").GetComponent<HitArea>();
    }

    public virtual void Start()
    {
        hitpoints_c = hitpoints_m;

        input = new PlayerInputter();
    }


    public void Act_Motion()
    {
        if (!isPerforming)
        {
            isDucking = input.duck;
            isAttacking = input.fight;
        }


        //Set idle animation
        isWalking = walkByDefault;
        //


        // Hold "Run" to run
        if (input.run)
        {
            isWalking = !walkByDefault;
        }

        // Only allow movement and jumps while grounded
        if (isGrounded)
        {
            if (isLanding)
            {
                inputDirection = Vector3.zero;
            }
            else
            {
                //if the player is steering with the right mouse button, A/D strafe instead of turn.
                if (Input.GetMouseButton(1))
                {
                    inputDirection = new Vector3(input.turning, 0, input.moving);
                }
                else
                {
                    inputDirection = new Vector3(0, 0, input.moving);
                }

                //Auto-move button pressed
                if (input.autoRun)
                {
                    mouseSideButton = !mouseSideButton;
                }

                //player moved or otherwise interrupted the auto-move.
                if (mouseSideButton && (input.moving != 0 || input.jump) || (Input.GetMouseButton(0) && Input.GetMouseButton(1)))
                {
                    mouseSideButton = false;
                }

                //L+R MouseButton Movement
                if ((Input.GetMouseButton(0) && Input.GetMouseButton(1)) || mouseSideButton)
                {
                    inputDirection.z = 1;
                }

                //If not strafing with right mouse and horizontal, check for strafe keys
                if (!(Input.GetMouseButton(1) && input.turning != 0))
                {
                    inputDirection.x -= input.strafing;
                }

                //if moving forward/backward and sideways at the same time, compensate for distance
                if (((Input.GetMouseButton(1) && input.turning != 0) || input.strafing != 0) && input.moving != 0)
                {
                    inputDirection *= 0.707f;
                }

                //apply the move backwards multiplier if not moving forwards only.
                if ((Input.GetMouseButton(1) && input.turning != 0) || input.strafing != 0 || input.moving < 0)
                {
                    speedMultiplier = moveBackwardsMultiplier;
                }
                else
                {
                    speedMultiplier = 1f;
                }

                //Use run or walkspeed
                inputDirection *= isWalking ? walkSpeed * speedMultiplier : runSpeed * speedMultiplier;

                // Jump!
                if (input.jump)
                {
                    inputDirection.y = jumpSpeed;
                }
            }
        }


        if (isGrounded)
        {
            //transform direction
            moveDirection = transform.TransformDirection(inputDirection);
            //
            if (IsInZone(Zone.Types.Water))
            {
                moveDirection *= 0.65f; // ??? <-- A bit arbitrary.
            }
        }



        //Character must face the same direction as the Camera when the right mouse button is down.
        if (Input.GetMouseButton(1))
        {
            // transform.rotation = Quaternion.Euler(0,Camera.main.transform.eulerAngles.y,0);
        }
        else
        {
            transform.Rotate(0, input.turning * turnSpeed * Time.deltaTime, 0);
        }

        //Apply gravity
        if (IsInZone(Zone.Types.Water))
        {
            moveDirection.y -= 0.35f * gravity * Time.deltaTime;
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        //
        //
        //Move Charactercontroller and check if grounded
        isGrounded = ((controller.Move(moveDirection * Time.deltaTime)) & CollisionFlags.Below) != 0;





        //movestatus jump/swimup (for animations)      

        if (!isGrounded)
        {
            lastAirSpeed = moveDirection.y;
        }



        if (lastGrounded != isGrounded)
        {
            OnGroundingChanged(isGrounded);
            //
            groundTime = 0;
        }
        //
        groundTime += Time.deltaTime;
        lastGrounded = isGrounded;
    }



    public virtual bool CanBeFoughtBy(Actor target)
    {
        return true;
    }
    public virtual void DoFighting(Actor target)
    {
        if (target.CanBeFoughtBy(this))
        {
            OnDamageCaused(target, target.TakeHit(1));
        }
    }

    public virtual float TakeHit(float val)
    {
        hitpoints_c -= val;
        //
        if (hitpoints_c <= 0)
        {
            Die();
        }


        return val;
    }
    public virtual void Die()
    {

    }
    public virtual bool IsDead() { return hitpoints_c <= 0; }


    public virtual void OnDamageCaused(Actor to, float howmuch)
    {
    }
    public virtual void OnGroundingChanged(bool grounded)
    {
    }


    internal Dictionary<Zone.Types, int> _zoneInteraction;
    internal void _ZoneInteraction(Zone.Types type, int val)
    {
        int prevVal = _zoneInteraction[type];
        _zoneInteraction[type] += val;
        //
        if (prevVal <= 0 && _zoneInteraction[type] > 0) { OnZoneEntered(type); }
        if (_zoneInteraction[type] <= 0 && prevVal > 0) { OnZoneExited(type); }
    }

    public virtual void OnZoneEntered(Zone.Types type)
    {

    }
    public virtual void OnZoneExited(Zone.Types type)
    {

    }
    public bool IsInZone(Zone.Types type) { return _zoneInteraction[type] > 0; }
}
