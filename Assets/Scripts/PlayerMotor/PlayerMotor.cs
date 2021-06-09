using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    // hidden in inspector but public to other scripts
    [HideInInspector] public Vector3 moveVector; // how far are we moving in this frame
    [HideInInspector] public float verticalVelocity; // my vertical velocity , jumping (positive) or falling (negative)  or floored ( should be 0ish) 
    [HideInInspector] public bool isGrounded; // is the character on ground
    [HideInInspector] public bool sprint; // is the character on ground
    [HideInInspector] public int currentLane; // which lane  ( -1 left , 0 middle , 1 right )

    // visible in editor 
    public float distanceInBetweenLanes = 3.0f; // how wide is each lane
    public float baseRunSpeed = 5.0f; // initial run speed
    private float actualRunSpeed; // initial run speed
    private float initRunSpeed; // initial run speed
    public float sprintSpeed = 10.0f; // initial run speed
    public float baseSidewaySpeed = 10.0f; // initial lane switching speed (left right)
    public float gravity = 14.0f; // how fast we will reach the floor when we jump/fall
    public float termivalVelocity = 20.0f; // max speed ? 
    
    [SerializeField] private float addedSpeed ;
    [SerializeField] private float timer = 1f;

    public CharacterController controller; // moves character with collission constraints without rigidbody or something 
    public Animator anim;
    private BaseState state; // gets the state
    private bool isPaused;

    [SerializeField] private AudioClip deathSFX;


    private void Start()
    {
        actualRunSpeed = baseRunSpeed;
        initRunSpeed = baseRunSpeed;
        controller = GetComponent<CharacterController>(); // gets the character controller ? 
        anim = GetComponent<Animator>();
        state = GetComponent<RunningState>(); // sets 
        state.Construct();
        isPaused = true;
        sprint = false;
        ChangeLane(0);
    }
    private void Update()
    {
        if (!isPaused)
            UpdateMotor();
        if (sprint)
        {
            baseRunSpeed = sprintSpeed;
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                sprint = false;
            }
        }
        else
        {
            baseRunSpeed = actualRunSpeed;
        }
    }
    private void UpdateMotor()
    {
        // check if grounded
        isGrounded = controller.isGrounded;

        // how should we be moving based on the state 
        moveVector = state.ProcessMotion();
        // are we trying to change state ?
        // 
        state.Transition();
        // increases speed over time
        if (actualRunSpeed < 8 )
            actualRunSpeed += Time.deltaTime * 0.03f;
        // feed state machine our animator some values 
        anim?.SetBool("IsGrounded", isGrounded);
        anim?.SetFloat("Speed", Mathf.Abs(moveVector.z));
        // Move the player
        controller.Move(moveVector * Time.deltaTime);
    }
    public float SnapToLane()
    {
        float r = 0.0f;
        if (transform.position.x != (currentLane * distanceInBetweenLanes)) // if not directly on top of a lane 
        {
            float deltaToDesiredPosition = (currentLane * distanceInBetweenLanes) - transform.position.x; // checks how far is the movement needed to snap to lane
            r = (deltaToDesiredPosition > 0) ? 1 : -1; // sets 'r' to 1 or -1 depending on the direction of the move (to multiply below)
            r *= baseSidewaySpeed; // will move in the necessary direction ( positive (right) negative (left)

            float actualDistance = r * Time.deltaTime; // calculates exactly how much to move

            if (Mathf.Abs(actualDistance) > Mathf.Abs(deltaToDesiredPosition)) // checks if actual distance will be farther than the desired position 
            {
                r = deltaToDesiredPosition * (1 / Time.deltaTime);
            }
        }
        else
        {
            r = 0;
        }
        return r;
    }
    public void ChangeLane(int direction)
    {
        currentLane = Mathf.Clamp(currentLane + direction, -1, 1); // clamps on both sides cause there are only 3 lanes 
    }
    public void ChangeState(BaseState s)
    {
        state.Destruct();
        state = s;
        state.Construct();
    }
    public void ApplyGravity()
    {
        verticalVelocity -= gravity * Time.deltaTime;
        if (verticalVelocity < -termivalVelocity)
        {
            verticalVelocity = -termivalVelocity; // 
        }
    }

    public void PausePlayer()
    {
        isPaused = true;
    }

    public void ResumePlayer()
    {
        isPaused = false;
    }
    public void RespawnPlayer()
    {
        Debug.Log("respawn");
        ChangeState(GetComponent<RespawnState>());
        GameManager.Instance.ChangeCamera(GameCamera.Respawn);
    }
    public void ResetPlayer()
    {
        actualRunSpeed = initRunSpeed;
        currentLane = 0;
        transform.position = Vector3.zero;
        anim?.SetTrigger("Idle");
        ChangeState(GetComponent<RunningState>());
        sprint = false;
        PausePlayer();
    }

    public void Sprint()
    {        
        timer = 1f;
        sprint = true;
    }
    public void OnControllerColliderHit(ControllerColliderHit hit) // built in PlayerController function that contains lots of collider hit info
    {
        string hitLayerName = LayerMask.LayerToName(hit.gameObject.layer); // gets colliding object layer name

        if (hitLayerName == "Death")
        {
            AudioManager.Instance.PlaySFX(deathSFX, 0.8f);
            ChangeState(GetComponent<DeathState>());

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sprint"))
        {
            Sprint();
            Debug.Log("sprint?");
        }
    }
}
