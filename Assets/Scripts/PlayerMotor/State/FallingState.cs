using UnityEngine;

public class FallingState : BaseState
{
    public override void Construct()
    {
        motor.anim?.SetTrigger("Fall");
        Debug.Log("am i falling?");
    }
    public override Vector3 ProcessMotion()
    {
        //apply gravity 
        motor.ApplyGravity();

        // create return vector
        Vector3 m = Vector3.zero;

        m.x = motor.SnapToLane();
        m.y = motor.verticalVelocity;
        m.z = motor.baseRunSpeed;

        return m;
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
        if (motor.isGrounded)
        {
            motor.ChangeState(GetComponent<RunningState>());
        }
    }
}
