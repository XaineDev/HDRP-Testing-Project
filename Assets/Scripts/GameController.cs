using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class GameController : MonoBehaviour
{

    private int _currentWave = 0;
    private int _zombiesSpawned = 0;
    
    [SerializeField]
    private float waveTimer = 5f;

    [SerializeField] 
    private float lastZombieSpawn = 0f;

    [SerializeField] private float zombieSpawnDelay = 1.25f;

    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject zombiePrefab;
    private List<GameObject> _zombies = new List<GameObject>();
    
    private TMP_Text _waveText;
    
    void Start()
    {
        _waveText = GameObject.Find("WaveText").GetComponent<TMP_Text>();
        _enemyContainer = GameObject.Find("Enemies");
        StartNewWave();
    }

    void Update()
    {
        
        if (_zombiesSpawned >= getZombiesPerWave())
        {
            var zombiesDead = checkZombiesDead();
            if (!zombiesDead) return;
            waveTimer += Time.deltaTime;
            if (waveTimer < 5) return;
            StartNewWave();
            return;
        }

        if (_waveText.IsActive()) return; // give use a grace period to prepare for the wave
        lastZombieSpawn += Time.deltaTime;
        if (lastZombieSpawn < zombieSpawnDelay) return;
        SpawnZombie();
    }

    private void StartNewWave()
    {
        _zombies.Clear();

        lastZombieSpawn = 0;
        waveTimer = 0;
        _zombiesSpawned = 0;
        _currentWave++;
        _waveText.text = "Wave " + _currentWave;
        _waveText.gameObject.SetActive(true);

        for (var i = 0; i < getZombiesPerWave(); i++)
        {
            var zombie = Instantiate(zombiePrefab, _enemyContainer.transform, true);
            zombie.SetActive(false);
            _zombies.Add(zombie);
        }
            
    }

    private void SpawnZombie()
    {
        var zombieToSpawn = _zombies[_zombiesSpawned];
        if (zombieToSpawn.IsDestroyed())
        {
            _zombiesSpawned++;
            return;
        }
        zombieToSpawn.SetActive(true);
        _zombiesSpawned++;
        lastZombieSpawn = 0;
    }

    private int getZombiesPerWave()
    {
        return 1 + (int)Math.Floor(_currentWave / 3f);
    }

    private bool checkZombiesDead()
    {
        return _zombies.All(zombie => zombie.IsDestroyed());
    }
    
    private bool isWaveComplete()
    {
        return _zombiesSpawned >= getZombiesPerWave() && checkZombiesDead();
    }
    
}
