using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : Platform
{
    [SerializeField] PlatformData platformData;

    [Space]
    [SerializeField] float spawnAreaWidth;
    [SerializeField] float minSpawnDelay;
    [SerializeField] float maxSpawnDelay;
    float _spawnDelay;
    [SerializeField] float maxSpawnAngle;

    // Ces valeurs sont relative aux distances de sauts: 0 correspond à la distance de saut minimale, et 1 la distance de saut maximale
    [SerializeField] float minSpawnDistFromPrevious;
    [SerializeField] float maxSpawnDistFromPrevious;
    
    [Space]
    [SerializeField] AnimationCurve unsafePlatformChanceBySpawnCount;

    float lastSpawnTime;
    int spawnMaxTryCount = 100;

    bool lastWasUnsafe = false;
    GameObject _lastPlatform;
    GameObject _beforeLastPlatform;

    public static int lastPlatformNumber {get; set;} = 0;

    protected override void OnAwake()
    {
        base.OnAwake();
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

        Vector3 spawnPos;
        GameObject toSpawn;
        if(lastWasUnsafe)
        {
            //Get a platform jumpable from the before last platform, beacause the last one was unsafe
            spawnPos = FindSpawnPos(_beforeLastPlatform, _lastPlatform);
            toSpawn = platformData.chooseSafePlatform();
            lastWasUnsafe = false;
        }
        else
        {
            //TODO: varier la façon de spawner quand la dérnière plateforme est safe de temps en temps
            //(ne pas tenir compte de l'avant dérnière ? forcer un long saut ? ...)
            spawnPos = FindSpawnPos(_lastPlatform, _beforeLastPlatform);

            if(Random.Range(0f, 1f) < unsafePlatformChanceBySpawnCount.Evaluate(lastPlatformNumber))
            {
                toSpawn = platformData.chooseUnsafePlatform();
                lastWasUnsafe = true;
            }
            else
            {
                toSpawn = platformData.chooseSafePlatform();
                lastWasUnsafe = false;
            }
        }

        _beforeLastPlatform = _lastPlatform;
        _lastPlatform = Instantiate(toSpawn, spawnPos, Quaternion.identity);
    }

    /// <summary>
    /// Search a position to spawn the next platform, that is:
    ///     - jumpable from source and in jump range of other (and further than minSpawnDistFromPrevious from other)
    ///         - the position may not be jumpable from other
    ///     - if that position still can't be found, only tries to find a position jumpable from source
    /// This methods search randomly, therefore it can fail to find an existing valid position 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="other"></param>
    /// <returns>A position that is jumpable from source and probably from other</returns>
    //TODO: chercher une meileur façon de faire
    Vector3 FindSpawnPos(GameObject source, GameObject other)
    {

        //Search for pos jumpable from source and in range of other
        for (int i = 0; i < spawnMaxTryCount && other != null; ++i)
        {
            Vector3 spawPosCandidate = GetNextPosCandidate(source.transform.position);
            float distanceToOther = Vector3.Distance(spawPosCandidate, other.transform.position);

            if(IsValidSpawnPos(spawPosCandidate) && distanceToOther < GameManager.Player.getJumpDistance(0.95f)
                                                 && distanceToOther >= GameManager.Player.getJumpDistance(minSpawnDistFromPrevious))
            {
                return spawPosCandidate;
            }
        }
        Debug.Log("Failed to find pos reachable by other");


        //Search for pos jumpable from source and not too close from other
        for (int i = 0; i < spawnMaxTryCount && other != null; ++i)
        {
            Vector3 spawPosCandidate = GetNextPosCandidate(source.transform.position);
            float distanceToOther = Vector3.Distance(spawPosCandidate, other.transform.position);

            if(IsValidSpawnPos(spawPosCandidate) && distanceToOther >= GameManager.Player.getJumpDistance(minSpawnDistFromPrevious))
                return spawPosCandidate;
        }
        Debug.Log("Failed to find pos away from other");

        //Search for pos jumpable from sourcer
        for (int i = 0; i < spawnMaxTryCount; ++i)
        {
            Vector3 spawPosCandidate = GetNextPosCandidate(source.transform.position);
            if(IsValidSpawnPos(spawPosCandidate))
                return spawPosCandidate;
        }
        Debug.Log("Failed to find pos jumpable from source"); //à priori impossible

        return source.transform.position + Vector3.forward * GameManager.Player.getJumpDistance(minSpawnDistFromPrevious); //default value
    }

    


    Vector3 GetNextPosCandidate(Vector3 source)
    {
        float angle = Random.Range(-maxSpawnAngle, maxSpawnAngle);
        float dist = Random.Range(minSpawnDistFromPrevious, maxSpawnDistFromPrevious);
        dist = GameManager.Player.getJumpDistance(dist); //convertion distance de saut ([0, 1]) en world distance
        return  GetNextPos(source, dist, angle);
    }

    Vector3 GetNextPos(Vector3 source, float dist, float angle)
    {
        return  source + Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward * dist;
    }


    bool IsValidSpawnPos(Vector3 pos)
    {
        return Mathf.Abs(pos.x - transform.position.x) < spawnAreaWidth;
    }
}
