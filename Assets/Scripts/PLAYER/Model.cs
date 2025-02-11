using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class Model 
{
    private View _view;
    private Transform _transform;

    private Rigidbody _rb;

    private float _currentSpeed;
    private float _maxSpeed;

    private float _motorTorque;
    private float _suspensionStrenght;
    private float _suspensionRestDist;
    private float _steerPower;
    private float _turnRadius;
    private float _break;
    private float _breakTorque;

    private int _changeIndex;

    private Changes[] _allChangesAvialable;
    private Wheels[] _allWheels;

    private Dictionary<WheelsType, WheelCollider> _wheelsDic = new Dictionary<WheelsType, WheelCollider>();

    public Model(View v, Transform t, Rigidbody r)
    {
        _view = v;
        _transform = t;
        _rb = r;
    }

    public Model SetNumber(float mxS, float mt, float sS, float sR, float stP, float tR, float br, float brT)
    {
        _maxSpeed = mxS;
        _motorTorque = mt;
        _suspensionStrenght = sS;
        _suspensionRestDist = sR;
        _steerPower = stP;
        _turnRadius = tR;
        _break = br;
        _breakTorque = brT;

        return this;
    }

    public Model SetList(Changes[] a, Wheels[] w)
    {
        _allChangesAvialable = a;
        _allWheels = w;


        foreach (var item in _allWheels)
        {
            if (!_wheelsDic.ContainsKey(item.wheelsType)) _wheelsDic.Add(item.wheelsType, item.wheel);
        }

        return this;
    }

    public void Change(bool isPositive)
    {
        if (isPositive)
        {
            _changeIndex++;
            _changeIndex = Mathf.Clamp(_changeIndex, 0, _allChangesAvialable.Length - 1);
            _maxSpeed = _allChangesAvialable[_changeIndex].maxSpeed;
        }
        else
        {
            _changeIndex--;
            _changeIndex = Mathf.Clamp(_changeIndex, 0, _allChangesAvialable.Length - 1);
            _maxSpeed = _allChangesAvialable[_changeIndex].maxSpeed;
        }
    }

    public void Stay()
    {
        _currentSpeed -= Time.deltaTime * .3f;
        _currentSpeed = Mathf.Clamp(_currentSpeed, -_maxSpeed, _maxSpeed);
    }

    public void NoAcceleration()
    {
        _currentSpeed -= Time.deltaTime * 10f;
        _currentSpeed = Mathf.Clamp(_currentSpeed, -_maxSpeed, _maxSpeed);

    }

    public void Acceleration()
    {
        _currentSpeed += Time.deltaTime * 10f;
        _currentSpeed = Mathf.Clamp(_currentSpeed, 0, _maxSpeed);
    }

    public void Break(bool condition)
    {
        if(condition)
        {
            _wheelsDic[WheelsType.Back].brakeTorque = _break;
            _wheelsDic[WheelsType.Front].brakeTorque = _break;
        }
        else
        {
            _wheelsDic[WheelsType.Back].brakeTorque = 0f;
            _wheelsDic[WheelsType.Front].brakeTorque = 0f;
        }
    }

    public void MoveCar(Vector2 input)
    {
        _transform.rotation = new Quaternion(0f, _transform.rotation.y, 0f, _transform.rotation.w);
        
        _wheelsDic[WheelsType.Back].motorTorque = _currentSpeed * _motorTorque * Time.deltaTime;

        var _steerAngle = input.x * _steerPower * _turnRadius;
        _wheelsDic[WheelsType.Front].steerAngle = Mathf.Lerp(_wheelsDic[WheelsType.Front].steerAngle, _steerAngle, 0.6f);

        foreach (var item in _allWheels.Select(x => x.wheel))
        {
            #region Suspention

            Vector3 springDir = item.transform.up;

            Vector3 tireWorldVel = _rb.GetPointVelocity(item.transform.position);

            float offset = _suspensionRestDist - item.suspensionDistance;

            float vel = Vector3.Dot(springDir, tireWorldVel);

            float force = (offset * _suspensionStrenght) - (vel * item.suspensionSpring.damper);

            _rb.AddForceAtPosition(springDir * force, item.transform.position);
            #endregion
        }
    }
}
