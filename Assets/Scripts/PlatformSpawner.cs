using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] GameObject platform;

    [Space]
    [SerializeField] float spawnAreaWidth;

    [Space]
    [SerializeField] float minSpawnDelay;
    [SerializeField] float maxSpawnDelay;
    float _spawnDelay;
    [SerializeField] float maxSpawnAngle;

    // Ces valeurs sont relative aux distances de sauts: 0 correspond à la distance de saut minimale, et 1 la distance de saut maximale
    [SerializeField] float minSpawnDistFromPrevious;
    [SerializeField] float maxSpawnDistFromPrevious;
    [SerializeField] float maxSpawnDistFromBeforePrevious;

    int spawnMaxTryCount = 50;




    GameObject _lastPlatform;
    GameObject _beforeLastPlatform;

    float lastSpawnTime;

    void Awake()
    {
        _spawnDelay = minSpawnDelay;
        lastSpawnTime = -_spawnDelay;
        _lastPlatform = GameObject.FindObjectOfType<Lily>().gameObject;
    }

    void Update()
    {
        if(Time.time - lastSpawnTime > _spawnDelay) //TODO: coroutine
        {
            Spawn();
            lastSpawnTime = Time.time;
            _spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
        }
    }

    void Spawn()
    {
        Vector3 spawnPos = FindSpawnPos();

        _beforeLastPlatform = _lastPlatform;
        _lastPlatform = Instantiate(platform, spawnPos, Quaternion.identity);
    }

    Vector3 FindSpawnPos()
    {
        for (int i = 0; i < spawnMaxTryCount && _beforeLastPlatform != null; ++i)
        {
            Vector3 spawPosCandidate = GetNextPosCandidate();
            if(IsValidSpawnPos(spawPosCandidate)
               && Vector3.Distance(spawPosCandidate, _beforeLastPlatform.transform.position) <  GameManager.Player.getJumpDistance(maxSpawnDistFromBeforePrevious))
            {
                return spawPosCandidate;
            }
        }

        for (int i = 0; i < spawnMaxTryCount; ++i)
        {
            Vector3 spawPosCandidate = GetNextPosCandidate();
            if(IsValidSpawnPos(spawPosCandidate))
            {
                return spawPosCandidate;
            }
        }

        Debug.Log("Failed to find valid random pos");
        return _lastPlatform.transform.position + Vector3.forward * GameManager.Player.getJumpDistance(minSpawnDistFromPrevious); //valeur par défaut
    }

    Vector3 GetNextPosCandidate()
    {
        float angle = Random.Range(-maxSpawnAngle, maxSpawnAngle);
        float dist = Random.Range(minSpawnDistFromPrevious, maxSpawnDistFromBeforePrevious);
        dist = GameManager.Player.getJumpDistance(dist); //convertion distance de saut ([0, 1]) en world distance
        return  _lastPlatform.transform.position + Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward * dist;
    }

    bool IsValidSpawnPos(Vector3 pos)
    {
        return Mathf.Abs(pos.x - transform.position.x) < spawnAreaWidth;
    }
}
