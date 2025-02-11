using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public enum AllChanges
{
    Neutro,
    Primera,
    Segunda,
    Tercera,
    Cuarta,
    Quinta, 
    Sexta,
    Septima,
    Reversa
}

[System.Serializable]
public struct Changes
{
    public AllChanges changes;
    public float maxSpeed;
}

public enum WheelsType
{
    Back,
    Front
}

[System.Serializable]
public struct Wheels
{
    public WheelCollider wheel;
    public WheelsType wheelsType;
}

public abstract class CarFather : MonoBehaviour
{
    public Wheels[] allWheels;
    public Changes[] allChangesAvialable;

    [Header("Car Specs")]
    public float wheelBase;
    public float rearTrack;
    public float turnRadius;
    public CarController carControl;

    public float brakeTorque;
    public float brake;

    public float suspensionRestDist;
    public float suspensionStrenght;
    //public float suspensionDamper;

    [Range(0, 1)] [SerializeField] public int gripFactor;

    public AnimationCurve torque;
    public float maxSpeed;
    public float motorPower = 100f;
    public float steerPower = 100f;
    //public float traction = 1f;
    //public float turnSpeed;
    public Transform centerOfMass;
    public Rigidbody rb;

    private float _ackermanAngleLeft;
    private float _ackermanAngleRight;

    private float _currentSpeed;
    private Vector3 _moveForce;
    private Vector3 velocity;
    private int _changeIndex;

    private Model _model;
    private View _view;
    private Controller _controller;

    private void Start()
    {
        if (rb != null) rb.centerOfMass = centerOfMass.transform.localPosition;

        _moveForce = rb.velocity;

        _changeIndex = 1;
        maxSpeed = allChangesAvialable[_changeIndex].maxSpeed;

        carControl = new CarController();

        Debug.Log("Start");
        _view = new View();
        Debug.Log("Hay view");
        _model = new Model(_view, transform, rb).SetNumber(maxSpeed, motorPower, suspensionStrenght, suspensionRestDist, steerPower, turnRadius, brake, brakeTorque)
                                                .SetList(allChangesAvialable, allWheels);
        Debug.Log("Hay model");
        _controller = new Controller(_model, carControl);
        Debug.Log("Hay controller");
    }

    private void OnEnable()
    {
        carControl.Enable();
        Debug.Log("Enable");
    }

    private void OnDisable()
    {
        carControl.Disable();
    }

    private void Update()
    {
        _controller.ArtificialUpdate();
    }

    private void FixedUpdate()
    {
        _controller.ArtificialFixedUpdate();
    }


}
