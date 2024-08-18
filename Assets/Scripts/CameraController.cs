using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : Singleton<CameraController>
{
    private const float PanSpeed = 20f;
    private new Camera camera;

    private const float ZoomSpeed = 2f;
    private const float MinZoom = 5f;
    private const float MaxZoom = 20f;

    [field: SerializeField] private Vector2 panLimit;

    protected override void Awake()
    {
        base.Awake();
        
        camera = Camera.main;
    }

    private void Update()
    {
        Pan();
        Zoom();
    }

    private void Pan()
    {
        var position = camera.transform.position;

        //TODO: Consider switching to Input System
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) position.y += PanSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) position.x -= PanSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) position.y -= PanSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) position.x += PanSpeed * Time.deltaTime;

        position.x = Mathf.Clamp(position.x, -panLimit.x, panLimit.x);
        position.y = Mathf.Clamp(position.y, -panLimit.y, panLimit.y);

        camera.transform.position = position;
    }

    private void Zoom()
    {
        //TODO: Consider switching to Input System
        var scroll = Input.GetAxis("Mouse ScrollWheel");

        camera.orthographicSize -= scroll * ZoomSpeed;
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, MinZoom, MaxZoom);
    }
}