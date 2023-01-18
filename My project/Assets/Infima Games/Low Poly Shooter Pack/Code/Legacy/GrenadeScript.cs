//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;
using System.Collections;
using UnityEngine.Assertions.Must;
using Photon.Pun;
using EZCameraShake;

public class GrenadeScript : MonoBehaviour
{

   	[Header("Timer")]
	//Time before the grenade explodes
	[Tooltip("Time before the grenade explodes")]
	public float grenadeTimer = 5.0f;

	[Header("Explosion Prefabs")]
	//Explosion prefab
	public Transform explosionPrefab;

	[Header("Explosion Options")]
	//Radius of the explosion
	[Tooltip("The radius of the explosion force")]
	public float explosionRadius = 25.0F;

	[Tooltip("VibeRadius")]
	public float vibeRadious = 9f;

	//Intensity of the explosion
	[Tooltip("The intensity of the explosion force")]
	public float power = 350.0F;

	[Header("Throw Force")]
	[Tooltip("Minimum throw force")]
	public float minimumForce = 1500.0f;

	[Tooltip("Maximum throw force")]
	public float maximumForce = 2500.0f;

	[Header("LayerMask")]
	public LayerMask hitLayerMask;

	[Header("VibeLayerMask")]
	public LayerMask vibeLayerMask;

	[Header("Audio")]
	public AudioSource impactSound;

	[Header("Damage")]
	public float damageRange;
	public float damage = 100.0f;

	private PhotonView PV;
	private CharacterBehaviour owerCharacterBehaviour;
	private int team;
	private int viewID;
    private float throwForce;
	private string playerName;
	private int index = -1;


    private void Awake()
	{
		PV = GetComponent<PhotonView>();	
		//Generate random throw force
		//based on min and max values
		throwForce = Random.Range
			(minimumForce, maximumForce);

		//Random rotation of the grenade
		GetComponent<Rigidbody>().AddRelativeTorque
		(Random.Range(500, 1500), //X Axis
			Random.Range(0, 0), //Y Axis
			Random.Range(0, 0) //Z Axis
			* Time.deltaTime * 5000);
	}

	private void Start()
	{
		//Launch the projectile forward by adding force to it at start
		GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * throwForce);

		//Start the explosion timer
		StartCoroutine(ExplosionTimer());
	}

	private void OnCollisionEnter(Collision collision)
	{
		//Play the impact sound on every collision
		impactSound.Play();
	}

    public void SetUp(CharacterBehaviour characterBehaviour, int team)
    {
        owerCharacterBehaviour = characterBehaviour;
        this.team = team;
        viewID = owerCharacterBehaviour.GetPhotonView().ViewID;
		playerName = owerCharacterBehaviour.GetPlayerName();
    }

    private void CheckAndVibe(Collider[] colliders)
    {
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.GetComponent<PhotonView>().IsMine)
                CameraShaker.Instance.ShakeOnce(5f, 2f, 1f, 1f);
        }
    }

    private IEnumerator ExplosionTimer()
	{
		//Wait set amount of time
		yield return new WaitForSeconds(grenadeTimer);

		//Raycast downwards to check ground
		RaycastHit checkGround;
		if (Physics.Raycast(transform.position, Vector3.down, out checkGround, 50))
		{
			//Instantiate metal explosion prefab on ground
			Instantiate(explosionPrefab, checkGround.point,
				Quaternion.FromToRotation(Vector3.forward, checkGround.normal));
		}

		//Explosion force
		Vector3 explosionPos = transform.position;
		//Use overlapshere to check for nearby colliders
		Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
		Collider[] vibeColliders = Physics.OverlapSphere(explosionPos, vibeRadious, vibeLayerMask);
		CheckAndVibe(vibeColliders);

        foreach (Collider hit in colliders)
		{
			//Ignore the player character.
			if (hit.GetComponent<Collider>().tag == "Player")
			{
				CharacterBehaviour hitPlayerCb = hit.GetComponent<CharacterBehaviour>();
				Vector3 grenadeToCharacterCam = hitPlayerCb.GetCameraWold().transform.position - transform.position;
				Vector3 direction = grenadeToCharacterCam.normalized;
				float dis = Vector3.Magnitude(grenadeToCharacterCam);
                float grenadeToCharacterdis = Vector3.Magnitude(hitPlayerCb.transform.position - transform.position);
                if (Physics.Raycast(transform.position,direction,dis,hitLayerMask))				
					continue;
				if(team == hitPlayerCb.GetPlayerTeam())
				{
					if (hitPlayerCb != owerCharacterBehaviour)
						continue;
				}

                if (hitPlayerCb.GetPhotonView().IsMine)
                    InGame.Instance.BlurCamera();

                if (grenadeToCharacterdis <= 3.0f)
					damage = 100.0f;
				else
					damage /= (grenadeToCharacterdis - damageRange);
				hit.GetComponent<Character>().TakeDamage((int)damage, transform.position, Quaternion.identity, team, viewID, false, playerName , index) ;

            }

            Rigidbody rb = hit.GetComponent<Rigidbody>();

			//Add force to nearby rigidbodies
			if (rb != null)
				rb.AddExplosionForce(power * 5, explosionPos, explosionRadius, 3.0F);

			//If the explosion hits "Target" tag and isHit is false
			//if (hit.GetComponent<Collider>().tag == "Target"
			//    && hit.gameObject.GetComponent<TargetScript>().isHit == false)
			//{
			//	//Toggle "isHit" on target object
			//	hit.gameObject.GetComponent<TargetScript>().isHit = true;
			//}

			////If the explosion hits "ExplosiveBarrel" tag
			//if (hit.GetComponent<Collider>().tag == "ExplosiveBarrel")
			//{
			//	//Toggle "explode" on explosive barrel object
			//	hit.gameObject.GetComponent<ExplosiveBarrelScript>().explode = true;
			//}

			////If the explosion hits "GasTank" tag
			//if (hit.GetComponent<Collider>().tag == "GasTank")
			//{
			//	//Toggle "isHit" on gas tank object
			//	hit.gameObject.GetComponent<GasTankScript>().isHit = true;
			//	//Reduce explosion timer on gas tank object to make it explode faster
			//	hit.gameObject.GetComponent<GasTankScript>().explosionTimer = 0.05f;
			//}
		}

		//Destroy the grenade object on explosion
		yield return new WaitForSeconds(0.1f);

		if(PV.IsMine)
			PhotonNetwork.Destroy(gameObject);
	}

}
