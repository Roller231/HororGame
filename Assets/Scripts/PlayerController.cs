using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float AnimBlendSpeed = 8.9f;
    [SerializeField] private Transform CameraRoot;
    [SerializeField] private Transform Camera;
    [SerializeField] private float UpperLimit = -40f;
    [SerializeField] private float BottomLimit = 70f;
    [SerializeField] private float MouseSensitivity = 21.9f;
    [SerializeField] private float JumpFactor = 100f;
    [SerializeField] private float Dis2Ground = 0.8f;
    [SerializeField] private LayerMask GroundCheck;






    private Rigidbody _playerRigidbody;
    private InputManager _inputManager;
    private Animator _animator;
    private bool _hasAnimator;
    private int _xVelHash;
    private int _yVelHash;
    private int _jumpHash;
    private int _groundHash;
    private int _fallingHash;
    private bool _grounded;
    private int _crouchHash;
    private float _xRotation;


    private const float _walkSpeed = 2.5f;

    private const float _runSpeed = 6f;

    private Vector2 _currentVelocity;

    public Transform _airTargetPosition;

    [SerializeField] private AudioSource _legsSource;




    private void Start()
    {
        _hasAnimator = TryGetComponent<Animator>(out _animator);
        _playerRigidbody = GetComponent<Rigidbody>();
        _inputManager = GetComponent<InputManager>();

        _xVelHash = Animator.StringToHash("X_Velocity");
        _yVelHash = Animator.StringToHash("Y_Velocity");
        _jumpHash = Animator.StringToHash("Jump");
        _groundHash = Animator.StringToHash("Grounded");
        _fallingHash = Animator.StringToHash("Falling");
        _crouchHash = Animator.StringToHash("Crouch");

    }
    void Update()
    {
        Ray desiredTargetRay = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenPointToRay(new Vector2(Screen.width/2, Screen.height/1.85f));
        Vector3 desiredTargetPosition =desiredTargetRay.origin + desiredTargetRay.direction * 0.5f;
        _airTargetPosition.position = desiredTargetPosition;
        HandleLight();

    }

    private void FixedUpdate()
    {
        Move();
        HandleJump();
        HandleCrouch();
        SampleGround();


    }
    private void LateUpdate()
    {
        CamMovements();
    }

    private void Move()
    {
        if (!_hasAnimator) return;

        float targetSpeed = _inputManager.Run ? _runSpeed : _walkSpeed;
        if (_inputManager.Crouch) targetSpeed = 1.5f;
        if(_inputManager.Move == Vector2.zero) targetSpeed = 0f;

        _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, _inputManager.Move.x * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);
        _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, _inputManager.Move.y * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);

        var xVelDifference = _currentVelocity.x - _playerRigidbody.velocity.x;
        var zVelDifference = _currentVelocity.y - _playerRigidbody.velocity.z;

        _playerRigidbody.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0, zVelDifference)), ForceMode.VelocityChange); 

        _animator.SetFloat(_xVelHash, _currentVelocity.x);
        _animator.SetFloat(_yVelHash, _currentVelocity.y);
        PlaySoundOnStep(targetSpeed);



    }

    private void CamMovements()
    {
        if (!_hasAnimator) return;

        var Mouse_X = _inputManager.Look.x;
        var Mouse_Y = _inputManager.Look.y;
        Camera.position = CameraRoot.position;

        _xRotation -= Mouse_Y * MouseSensitivity * Time.smoothDeltaTime;
        _xRotation = Mathf.Clamp(_xRotation, UpperLimit, BottomLimit);

        Camera.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        _playerRigidbody.MoveRotation(_playerRigidbody.rotation * Quaternion.Euler(0, Mouse_X * MouseSensitivity * Time.smoothDeltaTime, 0));
    }

    private void HandleCrouch() => _animator.SetBool(_crouchHash, _inputManager.Crouch);
    private void HandleJump()
    {
        if (!_hasAnimator) return;
        if(!_inputManager.Jump) return;
        _animator.SetTrigger(_jumpHash);
    }

    private void HandleLight()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            gameObject.GetComponentInChildren<Light>().FlipFLopLight();
        }
    }

    public void JumpAddForce()
    {
        _playerRigidbody.AddForce(-_playerRigidbody.velocity.y * Vector3.up, ForceMode.VelocityChange); ;
        _playerRigidbody.AddForce(Vector3.up * JumpFactor , ForceMode.Impulse);
        _animator.ResetTrigger(_jumpHash);
    }

    private void SampleGround()
    {
        if (!_hasAnimator) return;

        RaycastHit hitInfo;
        if (Physics.Raycast(_playerRigidbody.worldCenterOfMass, Vector3.down, out hitInfo, Dis2Ground + 0.1f, GroundCheck)){
            _grounded = true;
            SetAnimationGrounding();
            return;

        }

        _grounded = false;
        SetAnimationGrounding();
        return;

    }

    private void SetAnimationGrounding()
    {
        _animator.SetBool(_fallingHash, !_grounded);
        _animator.SetBool(_groundHash, _grounded);

    }

    public void PlaySoundOnStep(float targetSpeed)
    {
        if (targetSpeed > 0f)
        {
            if (_inputManager.Run) { _legsSource.pitch = 1f; _legsSource.volume = 1f; }
            else if (_inputManager.Crouch) { _legsSource.pitch = 0.7f; _legsSource.volume = 0.2f; }
            else if (!_inputManager.Run) { _legsSource.pitch = 0.7f; _legsSource.volume = 0.4f; }

        }
        else {
            _legsSource.volume = 0f;
        }

    }
}
