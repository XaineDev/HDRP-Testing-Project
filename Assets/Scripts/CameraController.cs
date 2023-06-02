using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    private Camera _camera;
    private HealthController _healthController;
    
    
    // Start is called before the first frame update
    void Start()
    {
        // lock mouse
        Cursor.lockState = CursorLockMode.Locked;
        _camera = Camera.main;
        _healthController = GameObject.Find("HealthController").GetComponent<HealthController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (_healthController.IsDead())
        {
            return;
        }
        
        // get mouse movement
        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");
        
        // rotate camera
        _camera.transform.Rotate(new Vector3(0, mouseX, 0), Space.World);
        _camera.transform.Rotate(new Vector3(-mouseY, 0, 0), Space.Self);
    }
}
