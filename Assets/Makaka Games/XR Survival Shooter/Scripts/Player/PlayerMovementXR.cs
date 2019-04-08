using UnityEngine;

public class PlayerMovementXR : MonoBehaviour
{
    Animator anim;     
    
    void Awake ()
    {
        // Set up references.
        anim = GetComponent <Animator> ();
    }

    public void Animate (bool isAnimated)
    {
        // Tell the animator whether or not the player is walking.
        anim.SetBool ("IsWalking", isAnimated);
    }
}
