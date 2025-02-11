using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public enum CameraPosition
{
    Reverse,
    Forward
}

public struct CameraPos
{
    public CameraPosition positionState;
    public Transform father;
}

public class CameraControl : MonoBehaviour
{
    [SerializeField] public Transform cam;
    [SerializeField] public float maxRotationSpeed;
    [SerializeField] public float maxRotAngle;
    [SerializeField] public CameraPos[] allPositions;

    [SerializeField] public CarController car;

    private float _currentRotationSpeed;

    private Quaternion _positivePosition;
    private Quaternion _negativePosition;
    private Quaternion _neutralPosition;
    private Quaternion _actualQuad;

    //private void OnEnable()
    //{
    //    car.Enable();
    //}

    //private void OnDisable()
    //{
    //    car.Disable();
    //}

    void Start()
    {
        _currentRotationSpeed = maxRotationSpeed;

        car = new CarController();

        _neutralPosition = Quaternion.identity;
    }

    private void Update()
    {
        _positivePosition = new Quaternion(cam.localRotation.x, Mathf.Atan(maxRotAngle), cam.localRotation.z, cam.localRotation.w);
        _negativePosition = new Quaternion(cam.localRotation.x, Mathf.Atan(-maxRotAngle), cam.localRotation.z, cam.localRotation.w);

        Rotation(car.Car.Move.ReadValue<Vector2>());
    }

    public void Rotation(Vector2 input)
    {
        if(input.x > 0)
        {
            _actualQuad = _positivePosition;
        }
        else
        {
            _actualQuad = _negativePosition;
        }
         
        if(input.x == 0)
        {
            _actualQuad = _neutralPosition;
        }

        cam.localRotation = Quaternion.Lerp(cam.localRotation, _actualQuad, _currentRotationSpeed * Time.deltaTime);
    }
}
