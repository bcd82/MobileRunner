using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : BaseState // inheriting BaseState / give access to things inherited 
{
    public override void Construct()
    {
        motor.verticalVelocity = 0f;
    }
    public override Vector3 ProcessMotion()
    {
        Vector3 m = Vector3.zero;
        m.x = motor.SnapToLane(); // movement in the x  // gets horizontal movement from PlayerMotor - returns r.
        m.y = -1.0f;  // keeps player on floor 
        m.z = motor.baseRunSpeed; // gets base run speed from PlayerMotor

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
        if (InputManager.Instance.SwipeUp && motor.isGrounded)
        {
            // change to jump state if grounded
            motor.ChangeState(GetComponent<JumpingState>());
        }
        if (InputManager.Instance.SwipeDown)
        {
            motor.ChangeState(GetComponent<SlidingState>());
        }
        if (!motor.isGrounded)
        {
            motor.ChangeState(GetComponent<FallingState>());
        }

    }
}
