using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grua : MonoBehaviour
{
    #region
    public float maxSpeed;
    public float maxForce;
    public Transform pivot;
    #endregion


    private float _speed;
    private float _force;

    private void Start()
    {
        _speed = maxSpeed;
        _force = maxForce;
    }

    private void Update()
    {
        RotateVertical();
    }


    public void RotateHorizontal()
    {

    }

    public void RotateVertical()
    {
         
    }
}
