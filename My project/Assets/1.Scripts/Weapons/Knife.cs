using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{


    [Title(label: "Effect Prefabs")]

    public Transform[] metalImpactPrefabs;
    public Transform[] dirtImpactPrefabs;
    public Transform[] concreteImpactPrefabs;

    [Title(label:"Knife Attack Settings")]
    public float knifeDistance = 3.0f;
    public LayerMask layerMask;


    private CharacterBehaviour characterBehaviour;
    private PhotonView PV;
    private int damage = 20;
    private int team;
    private int weaponIndex = -2;
    private string playerName;
    private Camera playerCamera;

    
    private void Awake()
    {
        characterBehaviour = transform.root.GetComponent<CharacterBehaviour>();
        playerCamera = characterBehaviour.GetCameraWold();
        PV = transform.root.GetComponent<PhotonView>();
        team = characterBehaviour.GetPlayerTeam();
        playerName = characterBehaviour.GetPlayerName();
        
    }

    private void CheckCollider(RaycastHit raycastHit)
    {
        Transform ts = raycastHit.transform;
        if(ts.tag == "Blood")
        {
            if(team == ts.root.GetComponent<CharacterBehaviour>().GetPlayerTeam())
                return;
            float totalDamage = damage * ts.transform.GetComponent<HitBox>().GetDamagePercent();
            characterBehaviour.CreateHitEffect();
            ts.transform.root.GetComponent<Character>().TakeDamage((int)totalDamage, raycastHit.point, Quaternion.LookRotation(playerCamera.transform.position), team, PV.ViewID, false, playerName, weaponIndex);
            
        }

        else if(ts.tag == "Metal")
        {
            Instantiate(metalImpactPrefabs[Random.Range
                    (0, metalImpactPrefabs.Length)], raycastHit.point,
              Quaternion.FromToRotation(Vector3.back,raycastHit.normal));

        }
        else if(ts.tag == "Dirt")
        {
            Instantiate(dirtImpactPrefabs[Random.Range
                  (0, dirtImpactPrefabs.Length)], raycastHit.point,
              Quaternion.FromToRotation(Vector3.back, raycastHit.normal));
     
        }
        else//"Concrete"
        {
            Instantiate(concreteImpactPrefabs[Random.Range
                  (0, concreteImpactPrefabs.Length)], raycastHit.point,
               Quaternion.FromToRotation(Vector3.back, raycastHit.normal));
        }
    }

    private void AttackKnife()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray,out raycastHit,knifeDistance,layerMask))
        {
            CheckCollider(raycastHit);
        }

    }

    private void OnEnable()
    {
        //StartCoroutine(EnableCollider());
        AttackKnife();
    }

    IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.1f);
        AttackKnife();
    }
}
