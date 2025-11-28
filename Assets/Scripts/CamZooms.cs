using Unity.Cinemachine;
using UnityEngine;

public class CamZooms : MonoBehaviour
{


    



    public float zoomedOutFOV = 70f;
    public float currentFOV;
    public float zoomspeed = 5f;
 public Camera camera ;
    

    public bool zoomingOut = false;



    void Start()
    {
       

      currentFOV = camera.fieldOfView;
    }
    void Update()
    {
        if (zoomingOut == true)
        {
            currentFOV = zoomedOutFOV;
        }

        if(zoomingOut == false)
        {
            currentFOV = 50f;
        }
      
       
        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, currentFOV, Time.deltaTime * zoomspeed);

    }


    public void ToggleZoomOut()
    {
        zoomingOut = true;
    }

    public void ToggleZoomIn()
    {
        zoomingOut = false;
    }
}

