using UnityEngine;

public class PlayerShootingXR : MonoBehaviour {

    public int damagePerShot = 20;                  
    public float timeBetweenBullets = 0.15f;        
    public float range = 100f;						    

    public GameObject muzzleFlash;
    public Transform barrelTransform;
    public AudioSource gunAudio;            
    
    private float timer;                                    
    private Ray shootRay = new Ray();                       
    private RaycastHit shootHit;                            
    private int shootableMask;                              

    private LineRenderer gunLine;                           
    
    private float effectsDisplayTime = 0.2f;

    private void Awake() {
        // Create a layer mask for the Shootable layer.
        shootableMask = LayerMask.GetMask("Shootable");
        // Set up the references.
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();

    }


    private void Update() {

        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

#if !MOBILE_INPUT
        // If the Fire1 button is being press and it's time to fire...
        if(Input.GetButton("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0) {
            // ... shoot the gun.
            Shoot(); 
        }
#else
        // If there is input on the shoot direction stick and it's time to fire...
        if ((CrossPlatformInputManager.GetAxisRaw("Mouse X") != 0 || CrossPlatformInputManager.GetAxisRaw("Mouse Y") != 0) && timer >= timeBetweenBullets) {
            // ... shoot the gun
            var flash = Instantiate(muzzleFlash, barrelTransform);
            var bullet = Instantiate(projectile, barrelTransform);
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * 10f);
            Shoot();
            Destroy(flash, 0.1f);
        }
#endif
    }

    private void Shoot() {
        // Reset the timer.
        timer = 0f;

        var flash = Instantiate(muzzleFlash, barrelTransform);
        Destroy(flash, 0.1f);

        Vector3 rayOrigin = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

        // Play the gun shot audioclip.
        gunAudio.Play();


        // Enable the line renderer and set it's first position to be the end of the gun.
        gunLine.enabled = true;
        gunLine.SetPosition(0, barrelTransform.position);

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit)) {
           
            EnemyHealthXR target = hit.transform.GetComponent<EnemyHealthXR>();

            if(target != null) {
                target.TakeDamage(damagePerShot);
                target.gameObject.GetComponent<Rigidbody>().AddForce(-hit.normal * 300, ForceMode.Impulse);
            }
                                
            
        }
    }
}
