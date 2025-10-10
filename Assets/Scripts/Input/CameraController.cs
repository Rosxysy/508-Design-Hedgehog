using UnityEngine;

public class CameraController : MonoBehaviour
{
     public Transform cameraPosition;
        public void Update()
        {
            transform.position = cameraPosition.position;
        }
    

}
