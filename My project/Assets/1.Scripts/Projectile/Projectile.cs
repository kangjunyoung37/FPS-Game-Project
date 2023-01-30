using UnityEngine;
using System.Collections;
using InfimaGames.LowPolyShooterPack.Legacy;
using Random = UnityEngine.Random;
using Photon.Pun;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    [Title(label:"Settings")]
    [Range(5, 100)]
    [Tooltip("After how long time should the bullet prefab be destroyed?")]
    public float destroyAfter;

    [Tooltip("If enabled the bullet destroys on impact")]
    public bool destroyOnImpact = false;

    [Tooltip("Minimum time after impact that the bullet is destroyed")]
    public float minDestroyTime;

    [Tooltip("Maximum time after impact that the bullet is destroyed")]
    public float maxDestroyTime;

    //public ParticleSystem particleSystem;

    [Title(label: "Effect Prefabs")]
    public Transform[] bloodImpactPrefabs;
    public Transform[] metalImpactPrefabs;
    public Transform[] dirtImpactPrefabs;
    public Transform[] concreteImpactPrefabs;

    #region FIELDS


    private CharacterBehaviour chbehaviour;
    private PhotonView PV;
    private bool IsLocalPlayer;
    private int team;
    private int damage;
    private float totalDamage;
    private string playerName;
    private int index;
    private Coroutine coroutine;
    public ParticleSystem particle;

    private Collider[] colliders;
    #endregion

    #region SERIALZED FIELDS



    #endregion

    #region SETUP


    public void SetIgnoreCollision(Collider[] colliders)
    {
        if (colliders == null)
            return;
        foreach (Collider col in colliders)
        {
            Physics.IgnoreCollision(col, GetComponent<Collider>(), false);
        }
    }

    public void Setup(CharacterBehaviour characterBehaviour, int damage, int index, Collider[] colliders)
    {
        chbehaviour = characterBehaviour;
        PV = characterBehaviour.GetPhotonView();
        IsLocalPlayer = PV.IsMine;
        team = characterBehaviour.GetPlayerTeam();
        this.damage = damage;
        this.index = index;
        playerName = characterBehaviour.GetPlayerName();
        this.colliders = colliders;
        
    }

    #endregion

    //If the bullet collides with anything
    private void OnCollisionEnter(Collision collision)
    {
        //Ignore collisions with other projectiles.
        if (collision.gameObject.GetComponent<Projectile>() != null)
            return;

        ////If destroy on impact is false, start 
        ////coroutine with random destroy timer
        if (!destroyOnImpact)
            StartCoroutine(DestroyTimer());

        //Otherwise, destroy bullet on impact
        else       
            InGame.Instance.DeactivatePoolItem(gameObject);


        //If bullet collides with "Blood" tag
        if (collision.transform.tag == "Blood")
        {
            if(!IsLocalPlayer || team == collision.transform.root.GetComponent<CharacterBehaviour>().GetPlayerTeam())
            {
                
                InGame.Instance.DeactivatePoolItem(gameObject);
                return;
            }

            totalDamage = damage * collision.transform.GetComponent<HitBox>().GetDamagePercent();
            chbehaviour.CreateHitEffect();
            collision.transform.root.GetComponent<Character>().TakeDamage((int)totalDamage, transform.position, Quaternion.LookRotation(collision.contacts[0].normal),team, PV.ViewID,true, playerName , index);
            
            //Destroy bullet object
            InGame.Instance.DeactivatePoolItem(gameObject);
           
        }

        //If bullet collides with "Metal" tag
        if (collision.transform.tag == "Metal")
        {
            //Instantiate random impact prefab from array
            Instantiate(metalImpactPrefabs[Random.Range
                    (0, bloodImpactPrefabs.Length)], transform.position,
                Quaternion.LookRotation(collision.contacts[0].normal));
            //Destroy bullet object
            InGame.Instance.DeactivatePoolItem(gameObject);
           
        }

        //If bullet collides with "Dirt" tag
        if (collision.transform.tag == "Dirt")
        {
            //Instantiate random impact prefab from array
            Instantiate(dirtImpactPrefabs[Random.Range
                    (0, bloodImpactPrefabs.Length)], transform.position,
                Quaternion.LookRotation(collision.contacts[0].normal));
            //Destroy bullet object
            InGame.Instance.DeactivatePoolItem(gameObject);
          
        }

        //If bullet collides with "Concrete" tag
        if (collision.transform.tag == "Concrete")
        {
            //Instantiate random impact prefab from array
            Instantiate(concreteImpactPrefabs[Random.Range
                    (0, bloodImpactPrefabs.Length)], transform.position,
                Quaternion.LookRotation(collision.contacts[0].normal));
            //Destroy bullet object
            InGame.Instance.DeactivatePoolItem(gameObject);
        }

        //If bullet collides with "Target" tag
        if (collision.transform.tag == "Target")
        {
            //Toggle "isHit" on target object
            collision.transform.gameObject.GetComponent
                <TargetScript>().isHit = true;
            //Destroy bullet object
            InGame.Instance.DeactivatePoolItem(gameObject);
        }

        //If bullet collides with "ExplosiveBarrel" tag
        if (collision.transform.tag == "ExplosiveBarrel")
        {
            //Toggle "explode" on explosive barrel object
            collision.transform.gameObject.GetComponent
                <ExplosiveBarrelScript>().explode = true;
            //Destroy bullet object
            InGame.Instance.DeactivatePoolItem(gameObject);
        }

        //If bullet collides with "GasTank" tag
        if (collision.transform.tag == "GasTank")
        {
            //Toggle "isHit" on gas tank object
            collision.transform.gameObject.GetComponent
                <GasTankScript>().isHit = true;
            //Destroy bullet object
            InGame.Instance.DeactivatePoolItem(gameObject);

        }

    }

    private void OnEnable()
    {
        coroutine = StartCoroutine(DestroyAfter());
        particle.Play();
    }

    private void OnDisable()
    {
        SetIgnoreCollision(colliders);
        if (coroutine != null)
            StopCoroutine(coroutine);
        particle.Stop();
    }

    private IEnumerator DestroyTimer()
    {
        //Wait random time based on min and max values
        yield return new WaitForSeconds
            (Random.Range(minDestroyTime, maxDestroyTime));
        //Destroy bullet object
        InGame.Instance.DeactivatePoolItem(gameObject);
    }

    private IEnumerator DestroyAfter()
    {
        //Wait for set amount of time
        yield return new WaitForSeconds(destroyAfter);
        //Destroy bullet object
        InGame.Instance.DeactivatePoolItem(gameObject);
    }
}

