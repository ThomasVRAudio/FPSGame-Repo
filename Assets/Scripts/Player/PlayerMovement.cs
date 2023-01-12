using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IOwner
{
    [Header("Movement")]
    [SerializeField] private float walkingSpeed = 10f;
    [SerializeField] private float runningSpeed = 15f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 2f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private bool _isAiming = false;
    public bool IsAiming { set { _isAiming = value; } }

    private float _speed = 10f;
    private CharacterController controller;
    private Vector3 _velocity;
    private bool _isGrounded;

    public event Action<PlayerMovement> OnLand;
    private CameraShake _cameraShake;

    void Start()
    {
        _cameraShake = FindObjectOfType<Camera>().GetComponent<CameraShake>();
        _speed = walkingSpeed;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
            OnLand?.Invoke(this);
        } 


        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = (transform.right * x + transform.forward * z).normalized;

        _speed = !Input.GetKey(KeyCode.LeftShift) || _isAiming ? walkingSpeed : runningSpeed;

        controller.Move(move * _speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && _isGrounded)
        {
            _cameraShake.OnJump();
            OnLand += _cameraShake.OnLanded;
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        _velocity.y += gravity * Time.deltaTime;
        controller.Move(_velocity * Time.deltaTime);
    }
}
