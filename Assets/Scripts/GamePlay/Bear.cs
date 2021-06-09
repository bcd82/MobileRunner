using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private Transform cameraPos;
    [SerializeField] private float standDistance = 25f;

    private void Start()
    {
        cameraPos = Camera.main.transform;
    }
    private void OnEnable()
    {
        anim?.SetTrigger("Idle");

    }
    void Update()
    {
        if (transform.position.z - cameraPos.transform.position.z < standDistance) 
            anim?.SetTrigger("isCameraClose");
    }
}
