using System.Collections;
using System.Collections.Generic;
using CandyCrush.Enums;
using CandyCrush.Services;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject focusTarget;
    private CameraService _service;

    void Start()
    {
        _service = new CameraService();
    }

    void Update()
    {
        if (focusTarget != null)
            _service.CenterCamera(this.gameObject, focusTarget);
    }
}
