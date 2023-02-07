using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{

    [SerializeField]
    private float pickUpRange = 1.5f;

    [SerializeField]
    private LayerMask pickUpLayerMask;

    private CharacterBehaviour characterBehaviour;

    private void Awake()
    {
        characterBehaviour = GetComponent<CharacterBehaviour>();
    }

    private void Update()
    {
        PickUp();
    }

    private void PickUp()
    {
        Camera camera = characterBehaviour.GetCameraWold();
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width,Screen.height)/2);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, pickUpRange,pickUpLayerMask))
        {
            hit.
        }
    }
}
