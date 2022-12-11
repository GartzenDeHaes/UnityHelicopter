using UnityEngine;

namespace Portland.Helicopter
{
	public class HeliHealth : MonoBehaviour
	{
		public new AudioSource audio;
		public AudioClip sparksound;
		public float FallImpactVel = -40;//The negative velocity below which to detect whether player falls from too much height
		public int health = 100;
		public bool state = false;
		public GameObject rotor;
		public float DamageFactor = 1;
		public int mainrotor_axis = 1;
		public GameObject refr;
		public float distanceToGround;
		//public GameObject particlefire;
		//public GameObject explosion;
		//public GameObject detonator;
		private float altitude;
		private HeliController helifin;
		//public Texture textureburn;
		//public Texture normaltexture;
		//public Material mainmaterial;
		//public GameObject scorchMark;
		//public GameObject fire;
		//public GameObject hitParticles;
		private float deathtimer = 0;
		private float Dieingtimer = 0;

		private new Rigidbody rigidbody;

		void Awake()
		{
			helifin = gameObject.GetComponent<HeliController>();
			rigidbody = gameObject.GetComponent<Rigidbody>();
		}

		// Use this for initialization
		void falling()
		{
			deathtimer += Time.deltaTime;
			if (deathtimer > 10)
			{
				explode();
			}
			switch (mainrotor_axis)
			{
				case 1:
					rotor.transform.Rotate(1000 * Random.value, 0, 0);
					break;
				case 2:
					rotor.transform.Rotate(0, 1000 * Random.value, 0);
					break;
				case 3:
					rotor.transform.Rotate(0, 0, 1000 * Random.value);
					break;

			}

			audio.pitch = Random.Range(0.1f, 0.6f);
			RaycastHit groundHit;
			Physics.Raycast(refr.transform.position, -Vector3.up, out groundHit);
			altitude = groundHit.distance;

			if (altitude < 2)
			{
				explode();
			}
			if (Vector3.Angle(refr.transform.up, Vector3.down) < 30)
			{
				explode();
			}
		}

		void explode() //exploding the helicopter and creating d4ecals
		{
			print("exploded" + distanceToGround);

			audio.Stop();

			rigidbody.AddExplosionForce(427600, transform.position, 100);

			//Instantiate(detonator, explosion.transform.position, transform.rotation);
			//mainmaterial.mainTexture = textureburn;
			//scorchMark = Instantiate(scorchMark, Vector3.zero, Quaternion.identity);
			//Vector3 scorchPosition = refr.GetComponent<Collider>().ClosestPointOnBounds(transform.position - Vector3.up * 100);
			//scorchMark.transform.position = scorchPosition + Vector3.up * 1.1f;

			//scorchMark.transform.eulerAngles = new Vector3(scorchMark.transform.eulerAngles.x, Random.Range(0.0f, 90.0f), scorchMark.transform.eulerAngles.z);

			Destroy(this);
		}

		void Start()
		{
			//particlefire.SetActive(false);
			//mainmaterial.mainTexture = normaltexture;
		}

		// Update is called once per frame
		void Update()
		{
			if (rigidbody.velocity.y < FallImpactVel)
			{
				if (helifin.altitude < 2)
				{
					health += (int)(1 * rigidbody.velocity.y * DamageFactor);
				}
			}

			if (health < 0)
			{
				state = true;
				falling();
			}
			if (health < 20)
			{
				//fire.SetActive(true);
			}

			if (state == true)
			{
				//particlefire.SetActive(true);
			}
		}

		void OnCollisionEnter(Collision other)
		{ //If colliding above ground
			if (helifin.altitude > 2.4)
			{
				//audio.PlayOneShot(sparksound);
				//health -= (int)(20 * DamageFactor);
				//var contact = other.contacts[0];
				//var rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
				//var pos = contact.point;

				//var mark = Instantiate(hitParticles, pos, rot);
				//mark.transform.parent = other.collider.transform;
				//Dieingtimer = 0;
			}
		}

		void OnCollisionStay(Collision collision)
		{
			if (helifin.altitude > 2.4)
			{

				Dieingtimer += Time.deltaTime;
				if (Dieingtimer > 1)
				{
					health -= (int)(10 * DamageFactor);
					Dieingtimer = 0;
				}

			}
		}

		void FixedUpdate()
		{

			if (state == true)
			{

				rigidbody.AddRelativeTorque(0, 30000, 0);
				rigidbody.AddForce(0, -15000, 0);

			}
		}
	}
}
