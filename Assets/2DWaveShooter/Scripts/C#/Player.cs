using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public static float moveSpeed;			//The speed at which the player will move at
	private Rigidbody2D rig;				//The player's rigidbody2D component
	public LayerMask mask;					//LayerMask defining which layer can be affected by the player's attacks
	public Camera cam;						//The camera in the scene
	
	public static int money;				//The player's money
	public static int curHp;				//The player's current health
	public static int maxHp;				//The player's maximum health
	
	//Weapon
	public static int ammo;					//The player's current ammunition
	public static int startAmmo;			//The ammunition which the player will start each round with
	public static int shootDist;			//The distance of how far the player's bullets will travel
	public int damage;						//The damage that the player will deal
	public GameObject muzzleFlash;			//The muzzle flash effect that will be instantiated once the gun is shot
	public GameObject muzzle;				//The point of origin where the end of the player's gun muzzle is
	public GameObject laser;				//The laser
	
	public static bool hasSight;			//Does the player have the Sight upgrade
	public static bool hasExtendedMag;		//Does the player have the Extended Mag upgrade
	public static bool hasLaser;			//Does the player have the Laser upgrade
	
	//Audio
	public AudioSource asource; 			//The players AudioSource that sounds will be played through
	public AudioClip shootSound;			//The audio clip that will be played when the player fires a bullet
	public AudioClip dryFireSound;          //The audio clip that will be played when the player has no more ammo but continues to click

	[Tooltip("bullet to instantiate on shoot")]
	public GameObject BulletPrefab;
	public GameObject Shoot_Start_Location;

	GameObject SpawnedBullet;
	GameObject ObjectHolder;
	GameObject Bac_Image;
	SpriteRenderer Bac_Image_Sprite_Renderer;
	Vector3 Original_Scale;

	public Sprite Idle_Sprite;
	public Sprite Get_Hit_Sprite;

	void Start ()
	{
		rig = transform.GetComponent<Rigidbody2D>();
		startAmmo = 100;
		ammo = startAmmo;
		moveSpeed = 10000;
		shootDist = 100;
		
		curHp = 100;
		maxHp = 100;
		
		hasSight = false;
		hasExtendedMag = false;
		hasLaser = false;
		
		money = 5;
		
		laser.GetComponent<LineRenderer>().sortingLayerName = "Player";
		
		asource.volume = PlayerPrefs.GetFloat("Volume");

		ObjectHolder = GameObject.Find("Objects");


		if (transform.GetChild(3))
        {
			Bac_Image = transform.GetChild(3).gameObject;
			Bac_Image_Sprite_Renderer = Bac_Image.GetComponent<SpriteRenderer>();
			Original_Scale = Bac_Image_Sprite_Renderer.transform.localScale;
		}
	}
	
	void Update ()
	{
		Move();
		//CameraFollow();
		
		if(Input.GetMouseButtonDown(0) && !UI.isPaused){
			if(ammo > 0){
				Shoot();
			}else{
				asource.PlayOneShot(dryFireSound);
			}
		}
		
		if(curHp <= 0){
			GameOver();
		}
		
		if(curHp > maxHp){
			curHp = maxHp;
		}
		
		if(hasLaser){
			laser.active = true;
		}
	}
	
	void Move ()
	{
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");

		rig.velocity = new Vector2(x, y) * Time.deltaTime * moveSpeed;
		
		//Player Rotation
		if(!UI.isPaused){
			Vector3 objectPos = new Vector3(0,0,0);
			Vector3 dir = new Vector3(0,0,0);

			objectPos = Camera.main.WorldToScreenPoint(transform.position);
			dir = Input.mousePosition - objectPos; 
		
			transform.rotation = Quaternion.Euler(new Vector3(0,0,Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg - 90));
		}
	}
	
	void Shoot ()
	{
		ammo --;
		Update_Image_Scale();
		
		RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, shootDist, mask.value);
		
		if(hit.collider != null){
			hit.collider.gameObject.SendMessage("Damaged", damage);
		}
		
		asource.PlayOneShot(shootSound);
		
		GameObject mf = Instantiate(muzzleFlash, muzzle.transform.position, transform.rotation) as GameObject;
		mf.transform.parent = transform;

		GameObject.Destroy(mf, 0.1f);


		SpawnedBullet = Instantiate(BulletPrefab, Shoot_Start_Location.transform.position, Quaternion.identity);
		Vector3 objectPos = new Vector3(0, 0, 0);
		Vector3 dir = new Vector3(0, 0, 0);
		objectPos = Camera.main.WorldToScreenPoint(transform.position);
		dir = Input.mousePosition - objectPos;
		SpawnedBullet.GetComponent<Bullet>().RBVelocity = new Vector2(dir.x, dir.y).normalized * 100;
	}
	
	public void Damaged (int dmg)
	{
		curHp -= dmg;
		Blink();
	}
	
	void CameraFollow ()
	{
		cam.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
	}
	
	void GameOver ()
	{
		if(PlayerPrefs.GetInt("Highscore") < Game.score){
			PlayerPrefs.SetInt("Highscore", Game.score);
		}	
		if(PlayerPrefs.GetInt("Highest Wave") < Game.curWave - 1){
			PlayerPrefs.SetInt("Highest Wave", Game.curWave - 1);
		}
		
		Application.LoadLevel(0);
	}

	public void Blink()
	{
		StartCoroutine(Change_Color());
	}

	public IEnumerator Change_Color()
	{
		if (Bac_Image_Sprite_Renderer)
		{
			//Bac_Image_Sprite_Renderer.color = new Color(1, 0, 0, 1);

			Bac_Image_Sprite_Renderer.sprite = Get_Hit_Sprite;

			yield return new WaitForSeconds(0.1f);

			//Bac_Image_Sprite_Renderer.color = new Color(1, 1, 1, 1);

			Bac_Image_Sprite_Renderer.sprite = Idle_Sprite;
		}
	}

	public void Update_Image_Scale()
    {
		Bac_Image_Sprite_Renderer.transform.localScale = Original_Scale * Mathf.Clamp(((float)ammo / startAmmo), 0.5f, 1.5f);
	}
}
