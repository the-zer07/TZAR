using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class EnemyManagerXR : MonoBehaviour
{
    public RandomObjectPooler randomObjectPooler;
	public UnityEvent OnInitialized;

    [Header("Spawn")]    
    public PlayerHealthXR playerHealth;
    public float spawnTime = 3f;           
    public Transform[] spawnPoints; 
    private int spawnPointIndex;

    private GameObject gameObjectTemp; 
    private EnemyHealthXR enemyHealthXRTemp; 
    private System.Type enemyHealthXRType;   

    private void Awake()
    {
        enemyHealthXRType = typeof(EnemyHealthXR);
    }

    private void Start()
    {
        randomObjectPooler.OnInitialized.AddListener(Init);
        randomObjectPooler.enabled = true;
    }
    
    /// <summary>Call after pool initialisation.</summary>
    public void Init()
    {
		StartCoroutine(InitCoroutine());
	}

	private IEnumerator InitCoroutine()
	{
		if (randomObjectPooler)
		{
            randomObjectPooler.InitControlScripts(enemyHealthXRType);

            for (int i = 0; i < randomObjectPooler.pooledObjects.Count; i++)
            {
            	gameObjectTemp = randomObjectPooler.pooledObjects[i];

            	if (gameObjectTemp)
            	{
            		gameObjectTemp.SetActive(true);

                    enemyHealthXRTemp = randomObjectPooler.RegisterControlScript(gameObjectTemp) as EnemyHealthXR;
                    enemyHealthXRTemp.SetRenderersEnabled(false);
                    
            		yield return new WaitForFixedUpdate ();

                    gameObjectTemp.SetActive(false);

                    yield return new WaitForFixedUpdate ();
            	}
            }

            // OnInitialized.Invoke ();

            OnInitialized.Invoke();
		}
		else
		{
			Debug.LogError("Random Object Pooler is Null. Assign it in the Editor.");
		}
    }
    
    public void Spawn()
    {
        //Debug.Log("Spawn!");

        StartCoroutine(SpawnCoroutine(spawnTime));
    }

    IEnumerator SpawnCoroutine (float repeatRate)
    {
        while (playerHealth.currentHealth > 0f)
        {
            yield return new WaitForSeconds(spawnTime);

            gameObjectTemp = randomObjectPooler.GetPooledObject();

            if (gameObjectTemp)
            {
                gameObjectTemp.SetActive(true);

                enemyHealthXRTemp = randomObjectPooler.RegisterControlScript(gameObjectTemp) as EnemyHealthXR;

                spawnPointIndex = Random.Range (0, spawnPoints.Length);
                
                enemyHealthXRTemp.Reset(
                    spawnPoints[spawnPointIndex].position, 
                    spawnPoints[spawnPointIndex].rotation
                    );
            }
        }
    }
}