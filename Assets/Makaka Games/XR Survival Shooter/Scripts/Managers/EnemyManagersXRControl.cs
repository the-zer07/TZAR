using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class EnemyManagersXRControl : MonoBehaviour {

    public EnemyManagerXR[] enemyManagersXR;
    private EnemyManagerXR enemyManagerXRTemp;
    public UnityEvent OnInitialized;

    private int initedPools = 0;
    private int enemyManagersXRActiveCount = 0;

    private void Start() {

        Spawn();
        for(int i = 0; i < enemyManagersXR.Length; i++) {
            enemyManagerXRTemp = enemyManagersXR[i];

            if(enemyManagerXRTemp) {
                enemyManagerXRTemp.OnInitialized.AddListener(CountInitedEnemyManagerXR);
                enemyManagerXRTemp.enabled = true;

                enemyManagersXRActiveCount++;
            }
        }
    }

    public void CountInitedEnemyManagerXR() {
        initedPools++;

        //Debug.Log("Pool Inited: #" + initedPools);

        if(initedPools == enemyManagersXRActiveCount) {

            OnInitialized.Invoke();

            //Debug.Log("All Pools Inited");
        }
    }

    public void Spawn() {
        for(int i = 0; i < enemyManagersXR.Length; i++) {
            enemyManagerXRTemp = enemyManagersXR[i];

            if(enemyManagerXRTemp) {
                enemyManagerXRTemp.Spawn();
            }
        }
    }
}
