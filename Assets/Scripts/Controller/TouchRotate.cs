using UnityEngine;

public class TouchRotate : MonoBehaviour
{
    private void OnMouseDrag()
    {
        var YaxisRotation = Input.GetAxis("Mouse X") * 6 * Time.deltaTime;
        // select the axis by which you want to rotate the GameObject
        transform.RotateAround (Vector3.down, YaxisRotation);
    }
}
