using UnityEngine;

public class SlidingState : BaseState
{
    public float slideDuration = 1.0f; // time to slide

    // collider logic

    private Vector3 initialCenter; // the center of our player controller
    private float initialSize; // how big is the radius originally 
    private float slideStart; // when exactly does the slide start

    public override void Construct()
    {
        motor.anim?.SetTrigger("Slide"); // sets sliding trigger // motor.anim?. -> is like if (motor.anim) 
        Debug.Log("im sliding");
        slideStart = Time.time; // gives the timestamp for entering state

        initialSize = motor.controller.height;
        initialCenter = motor.controller.center;

        motor.controller.height = initialSize * 0.5f; // reduces collider height in half
        motor.controller.center = initialCenter * 0.5f; // moves the center of the collider down  
    }
    public override void Destruct()
    {
        motor.controller.height = initialSize;
        motor.controller.center = initialCenter;
        motor.anim?.SetTrigger("Running"); // goes back to running when slide is over

    }
    public override void Transition()
    {
        if (InputManager.Instance.SwipeLeft)
        {
            // change lanes - left
            motor.ChangeLane(-1);
        }
        if (InputManager.Instance.SwipeRight)
        {
            // change lanes - right
            motor.ChangeLane(1);

        }
        if (!motor.isGrounded) // if you fall while sliding
        {
            motor.ChangeState(GetComponent<FallingState>());
        }
        if (InputManager.Instance.SwipeUp && motor.isGrounded)
        {
            motor.ChangeState(GetComponent<JumpingState>());

        }
        if (Time.time - slideStart > slideDuration) // if slide duration is over
        {
            motor.ChangeState(GetComponent<RunningState>());

        }

    }
    public override Vector3 ProcessMotion()
    {
        Vector3 m = Vector3.zero;
        m.x = motor.SnapToLane(); // movement in the x  // gets horizontal movement from PlayerMotor - returns r.
        m.y = -1.0f;  // keeps player on floor 
        m.z = motor.baseRunSpeed; // gets base run speed from PlayerMotor

        return m;
    }
}
