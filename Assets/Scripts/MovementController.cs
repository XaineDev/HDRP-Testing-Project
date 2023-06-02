using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
public class MovementController : MonoBehaviour
{
    private CharacterController _controller;
    private HealthController _healthController;
    private Camera _camera;
    private Vector3 _playerVelocity;
    private bool _groundedPlayer;
    
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    
    [SerializeField]
    private float gravityValue = -9.81f;

    private bool _isMoving = false;
    private int _movingTicks = 0;
    [SerializeField]
    private bool shouldDoBobbing = true;
    private bool _updatedCamera = false;
    private const float CameraHeight = 0.6f;

    private bool _hasJetpack = false;
    private TMP_Text _jetpackText;

    private void Start()
    {
        _controller = gameObject.GetComponentInParent<CharacterController>();
        _camera = Camera.main;
        _jetpackText = GameObject.Find("JetpackText").GetComponent<TMP_Text>();
        _jetpackText.enabled = false;

        _healthController = GameObject.Find("HealthController").GetComponentInParent<HealthController>();
    }

    private void Update()
    {

        if (_healthController.IsDead())
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            _healthController.TakeDamage(Random.Range(1, 3));
        }
        
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            _hasJetpack = !_hasJetpack;
            _jetpackText.enabled = _hasJetpack;
        }
        
        _groundedPlayer = _controller.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        
        var horizontal = Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
        var vertical = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        
        var yawAngleDegrees = _camera.transform.rotation.eulerAngles.y;
        Quaternion yawRotation = Quaternion.Euler(0f, yawAngleDegrees, 0f);
        Vector3 forwardMotion = new Vector3(horizontal, 0f, vertical);
        Vector3 rotatedMotion = yawRotation * forwardMotion;
        Vector3 finalMotion = rotatedMotion.normalized;
        
        var speed = playerSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed *= 2f;
        }
        _controller.Move(finalMotion * (speed * Time.deltaTime));
        if (finalMotion != Vector3.zero)
        {
            _isMoving = true;
            gameObject.transform.forward = finalMotion * (speed * Time.deltaTime);
        }
        else
        {
            _isMoving = false;
            _movingTicks = 0;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (_hasJetpack)
            {
                _playerVelocity.y = 2.5f;
            }
            else if (_groundedPlayer)
            {
                _playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }
        }

        _playerVelocity.y += gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (_groundedPlayer)
            ViewBobbing();
    }

    private void ViewBobbing()
    {
        if (shouldDoBobbing && _isMoving)
        {
            _movingTicks += Input.GetKey(KeyCode.LeftShift) ? 5 : 3;
            var bobbingAmount = (Math.Sin((_movingTicks % 1800) / 20f));
            var position = gameObject.transform.position;
            _camera.transform.position = new Vector3(position.x, position.y + CameraHeight + ((float)bobbingAmount/10f), position.z);
            _updatedCamera = true;
        } 
        else if (_updatedCamera)
        {
            var position = _controller.transform.position;
            _camera.transform.position = new Vector3(position.x, position.y + CameraHeight, position.z);
            _updatedCamera = false;
        }
    }
}
