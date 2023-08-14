using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveCameraMousePositionConverter : MousePositionConverter
{
    public PerspectiveCameraMousePositionConverter(Camera camera) : base(camera) { }

    public override Vector3 GetCursorPosition(Vector2 mousePosition, float distance)
    {
        Ray ray = _camera.ScreenPointToRay(mousePosition);
        return _camera.transform.position + ray.direction * distance;
    }
}
