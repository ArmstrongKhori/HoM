using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour {

    public enum Types
    {
        None,
        /// <summary>
        /// Prevents fall damage and slows movement down
        /// </summary>
        Water,
        /// <summary>
        /// There are no other maps. Instead, this signals the end of the game's demo!
        /// </summary>
        Transition,
        /// <summary>
        /// In true 3D Sonic fashion: You watch yourself fall to the center of the earth.
        /// </summary>
        Abyss,
    }
    public Types type;
}
