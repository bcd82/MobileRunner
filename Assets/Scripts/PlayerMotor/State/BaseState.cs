using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : MonoBehaviour // will be the parent class for walking jumping falling moving etc . they will inherit this one .
                                                // - abstract means it has to be inherited . will not exist on its own, only as an inherited class !
{
    protected PlayerMotor motor; // protected - like declaring 'private' but , children of this object will also have access to it 
    // state machine 
    // Construct will usually be called when we first enter a state
    public virtual void Construct()
    {
    } // virtual - will allow to override these in inheriting objects
    // Destruct will usually be called when we leave the state

    public virtual void Destruct() { }
    // Transition will be called in the update loop constantly to know if we need to switch state
    public virtual void Transition() { }

    private void Awake()
    {
        motor = GetComponent<PlayerMotor>(); // assigns the playerMotor to the class
    }
    public virtual Vector3 ProcessMotion()
    {
        Debug.Log("process motion is not implented in " + this.ToString());
        return Vector3.zero;
    }
}
