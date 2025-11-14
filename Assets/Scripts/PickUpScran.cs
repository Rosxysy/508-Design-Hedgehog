using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using System;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;

public class PickUpJewl : MonoBehaviour
{
    public static event Action OnJewl1Destroyed;
    public static event Action OnJewl2Destroyed;
    public static event Action OnBothJewlsDestroyed;
    private static bool jewl1Destroyed = false;
    private static bool jewl2Destroyed = false;

    [Header("Pickup Settings")]
    [SerializeField] Transform holdArea;
    private GameObject heldObj;
    private Rigidbody heldObjRB;

    [Header("Physics Parameters")]
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float pickupForce = 150.0f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObj == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                {
                    
                    PickupObject(hit.transform.gameObject); //calls on method pickupobject to execute if object not already held.
                }
            }


        }
        if (heldObj != null)
        {
            //moveobject
            MoveObject();
        }



    }

    void MoveObject()
    {
        if (heldObj == null) return;
        //distance between the held object and area below 0.1
        if (Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 moveDirection = (holdArea.position - heldObj.transform.position); //work out how far away it needs to move
            heldObjRB.AddForce(moveDirection * pickupForce);
        }
       
    }

    void PickupObject(GameObject pickObj)
{
    //check if have object
    if (pickObj.GetComponent<Rigidbody>())
    {
        //pick up method with set variables of rotation ect
        heldObjRB = pickObj.GetComponent<Rigidbody>();
        heldObjRB.useGravity = false;
        heldObjRB.linearDamping = 10;
        heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;

        heldObjRB.transform.parent = holdArea;
        heldObj = pickObj;
    }
}


    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter: heldObj={(heldObj != null ? heldObj.name : "null")}, heldObjTag={(heldObj != null ? heldObj.tag : "null")}, other={other.name}, otherTag={other.tag}");
        // Destroy held object if it has tag Jewl1 and on collision with Box1
        if (heldObj != null && heldObj.CompareTag("Jewl1") && other.CompareTag("Box1"))
        {
            Debug.Log("Destroy condition met: Destroying heldObj");
            Destroy(heldObj);
            heldObj = null;
            heldObjRB = null;
            jewl1Destroyed = true;
            OnJewl1Destroyed?.Invoke();
            CheckBothJewlsDestroyed();
            return;
        }
        else if (heldObj != null && heldObj.CompareTag("Jewl2") && other.CompareTag("Box2"))
        {
            Debug.Log("Destroy condition met: Destroying heldObj");
            Destroy(heldObj);
            heldObj = null;
            heldObjRB = null;
            jewl2Destroyed = true;
            OnJewl2Destroyed?.Invoke();
            CheckBothJewlsDestroyed();
            return;
        }
    }

    private void CheckBothJewlsDestroyed()
    {
        if (jewl1Destroyed && jewl2Destroyed)
        {
            OnBothJewlsDestroyed?.Invoke();
        }
    }

}

