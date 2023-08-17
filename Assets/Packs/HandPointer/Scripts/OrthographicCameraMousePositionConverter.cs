using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthographicCameraMousePositionConverter : MousePositionConverter
{
    public OrthographicCameraMousePositionConverter(Camera camera) : base(camera) { }

    public override Vector3 GetCursorPosition(Vector2 mousePosition, float distance)
    {
        Vector2 cameraSize = new Vector2(_camera.orthographicSize * (float)((float)Screen.width / (float)Screen.height) * 2, _camera.orthographicSize * 2);
        float x = cameraSize.x * (mousePosition.x / Screen.width) - cameraSize.x / 2;
        float y = cameraSize.y * (mousePosition.y / Screen.height) - cameraSize.y / 2;

        return Camera.main.transform.TransformPoint(new Vector3(x, y, distance));
    }
}
