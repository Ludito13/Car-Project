using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using System;

public class Controller
{
    [SerializeField] private CarController carController;

    private Model _model;

    public Controller(Model m, CarController c)
    {
        _model = m;

        //carController = c;
        
        //carController = new CarController();

        carController.Car.Changesnegative.started += _ => _model.Change(false);

        carController.Car.ChangesPositive.started += _ => _model.Change(true);

        carController.Car.Break2.started += _ => _model.Break(true);
        carController.Car.Break2.canceled += _ => _model.Break(false);

        carController.Car.Acceleration.performed += _ => _model.Acceleration();
        carController.Car.Acceleration.canceled += _ => _model.NoAcceleration();
    }

    public void ArtificialUpdate()
    {
        if (carController.Car.Acceleration.IsPressed()) _model.Acceleration();

        if (!carController.Car.Acceleration.IsPressed()) _model.Stay();

        if (carController.Car.Break.IsPressed()) _model.NoAcceleration();

        //if (carController.Car.Break2.IsPressed()) _model.Break(true);
    }

    public void ArtificialFixedUpdate()
    {
        _model.MoveCar(carController.Car.Movement.ReadValue<Vector2>());
    }

    public void Enable()
    {
        carController.Enable();
    }
    
    public void Disable()
    {
        carController.Disable();
    }
}
