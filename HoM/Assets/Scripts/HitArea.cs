using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitArea : MonoBehaviour {


    public enum Types
    {
        None,
        /// <summary>
        /// For interacting with actor bodies (as to "hit" them)
        /// </summary>
        Fighting,
        /// <summary>
        /// For interacting with zones (such as water or screen transitions)
        /// </summary>
        Zoner,
    }
    public Types type;

    internal Actor owner;
    internal Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        if (rb == null) { rb = gameObject.AddComponent<Rigidbody>(); }
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        owner = gameObject.transform.parent.parent.GetComponent<Actor>(); // *** <this> --> "(hitboxes)" --> <whatever it is> ... "Actor" component
	}

    private void OnTriggerEnter(Collider other)
    {
        Zone z = other.gameObject.GetComponent<Zone>();
        if (z != null)
        {
            owner._ZoneInteraction(z.type, +1);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Zone z = other.gameObject.GetComponent<Zone>();
        if (z != null)
        {
            owner._ZoneInteraction(z.type, -1);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Actor a = other.gameObject.GetComponent<Actor>();
        if (a != null)
        {
            if (a.gameObject != owner.gameObject)
            {
                if (type == Types.Fighting)
                {
                    owner.DoFighting(a);
                }
            }
        }
    }
}
