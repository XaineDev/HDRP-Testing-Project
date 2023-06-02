using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponController : MonoBehaviour
{
    
    private CameraController _cameraController;
    private Transform _cameraTransform;
    private HealthController _healthController;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _cameraController = GameObject.Find("CameraController").GetComponent<CameraController>();
        if (_cameraTransform == null)
        {
            _cameraTransform = Camera.main!.transform;
        }
        _healthController = GameObject.Find("HealthController").GetComponent<HealthController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_healthController.IsDead()) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (!Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out var hit,
                    1000f)) return;
            Debug.Log("Hit: " + hit.collider.gameObject.name);
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
