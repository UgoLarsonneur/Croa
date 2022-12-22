using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : Platform
{
    [SerializeField] PlatformData platformData;
    [SerializeField] GameObject initialPlatform;

    [Space]
    [SerializeField] float spawnDelay;
    [SerializeField] int rapidSpawnThreshold; //if the difference of the last platform reached and the last platform spawned is below this threshold, start spawning rapidly
    [SerializeField] int normalSpawnThreshold; //if the difference of the last platform reached and the last platform spawned is above this threshold, resume normal spawn speed
    [SerializeField] int stopSpawnThreshold;
    [SerializeField] float spawnAreaWidth;
    [SerializeField] float maxSpawnAngle;

    // Ces valeurs sont relative aux distances de sauts: 0 correspond à la distance de saut minimale, et 1 la distance de saut maximale
    [Space]
    [SerializeField] float minCriticalDistance;
    [SerializeField] float maxCriticalDistance;
    [SerializeField] float minNonCriticalDistance;
    [SerializeField] float maxNonCriticalDistance;
    
    [Space]
    [SerializeField] AnimationCurve unsafePlatformChanceBySpawnedCount;
    [SerializeField] AnimationCurve nonCtriticalPlatformChanceBySpawnedCount;
    //[SerializeField] float unsafePlatformChanceIncrease; //for each consecutive safe platform spawned, increases the chance to spawn an unsafe one

    private List<Platform> platforms;
    Platform _lastCriticalPlatform;
    private int spawnedCount;
    private int lastUnsafePlatform = 0;
    float spawnSpeedMultiplier = 1f;

    const int spawnMaxTryCount = 100;
    

    private void Awake() {
        platforms = new List<Platform>();
        _lastCriticalPlatform = initialPlatform.GetComponent<Platform>();
        platforms.Add(_lastCriticalPlatform);
        spawnedCount = 1;
    }


    private void Start() {
        StartCoroutine(SpawnCouroutine());
    }

    public void CheckForRapidMode()
    {   
        
        if(GetRemainingPlatformCount() <= rapidSpawnThreshold)
        {
            StopCoroutine("ActivateRapidSpawn");
            StartCoroutine(ActivateRapidSpawn());
        }
    }

    IEnumerator ActivateRapidSpawn()
    {
        spawnSpeedMultiplier = 5f;

        while(GetRemainingPlatformCount() <= normalSpawnThreshold)
        {
            yield return null;
        }

        spawnSpeedMultiplier = 1f;
        yield return null;
    }

    IEnumerator SpawnCouroutine()//TODO: remettre dans Update?
    {
        float _lastCycleSpawnTime = -spawnDelay;
        while(enabled)
        {
            float cycleDuration = spawnDelay / spawnSpeedMultiplier;
            if(Time.time - _lastCycleSpawnTime < cycleDuration || GetRemainingPlatformCount() >= stopSpawnThreshold)
            {
                yield return null;
                continue;
            }
            Debug.Log(GetRemainingPlatformCount());
            
            _lastCycleSpawnTime = Time.time;

            // Each cycle, spawn a critical platform and a non critical one at random times inside the cycle
            // Mieux que de faire spawner une critique et une non critique tour à tour,
            // car là le joueur pourrait finir par remarquer qu'une plateforme sur deux est forcément safe
            // TODO: meilleur façon de spawn (sans coroutines imbriquées)?
            
            StartCoroutine(SpawnCritical(Random.Range(0f, cycleDuration)));
            if(Random.Range(0f, 1f) < nonCtriticalPlatformChanceBySpawnedCount.Evaluate(spawnedCount))
                StartCoroutine(SpawnNonCritical(_lastCriticalPlatform.transform.position, Random.Range(0f, cycleDuration)));

            yield return null;
        }

        yield return null;
    }


    //Each critical platform is safe and jumpable from the previous one
    IEnumerator SpawnCritical(float delay)
    {
        yield return new WaitForSeconds(delay);
        Vector3 spawnPos = FindPos(_lastCriticalPlatform.transform.position, true);
        GameObject platformToSpawn = platformData.chooseSafePlatform();

        _lastCriticalPlatform = Spawn(spawnPos, platformToSpawn);
        //Debug.Log("Critical");
        yield return null;
    }

    IEnumerator SpawnNonCritical(Vector3 source, float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector3 spawnPos = FindPos(_lastCriticalPlatform.transform.position, true);

        GameObject platformToSpawn;
        if(Random.Range(0f, 1f) < unsafePlatformChanceBySpawnedCount.Evaluate(spawnedCount))
        {
            platformToSpawn = platformData.chooseUnsafePlatform();
            lastUnsafePlatform = spawnedCount;
        }
        else
            platformToSpawn = platformData.chooseSafePlatform();
        
        Spawn(spawnPos, platformToSpawn);
        //Debug.Log("NonCritical");
        yield return null;
    }


    Platform Spawn(Vector3 spawnPos, GameObject platformToSpawn)
    {
        Platform spawnedInstance = Instantiate(platformToSpawn, spawnPos, Quaternion.identity).GetComponent<Platform>();
        spawnedInstance.Number = spawnedCount;
        ++spawnedCount;
        platforms.Add(spawnedInstance);
        return spawnedInstance;
    }


    Vector3 FindPos(Vector3 source, bool critical = false)
    {
        Vector3 spawnPos = source + Vector3.forward * GameManager.Player.getJumpDistance(Random.Range(0f, 1f)); //default value
        for (int i = 0; i < spawnMaxTryCount; ++i)
        {
            float angle = Random.Range(-maxSpawnAngle, maxSpawnAngle);
            float dist = Random.Range(critical ? minCriticalDistance : minNonCriticalDistance,
                                      critical ? maxCriticalDistance : maxNonCriticalDistance);
            dist = GameManager.Player.getJumpDistance(dist); //convertion distance de saut ([0, 1]) en world distance
            Vector3 candidate = GetNextPos(source, dist, angle);
            if(IsValidSpawnPos(candidate))
            {
                spawnPos = candidate;
            }
        }
        return spawnPos;
    }


    Vector3 GetNextPos(Vector3 source, float dist, float angle)
    {
        return  source + Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward * dist;
    }


    bool IsValidSpawnPos(Vector3 pos)
    {
        //Is in spawn area
        if(Mathf.Abs(pos.x - transform.position.x) > spawnAreaWidth)
        {
            return false;
        }

        //Is not to close to other platforms
        float minDist = GameManager.Player.getJumpDistance(0.1f);
        foreach (Platform item in platforms)
        {
            if(Vector3.Distance(item.transform.position, pos) < minDist)
            {
                return false;
            }   
        }

        return true;
    }

    private int GetRemainingPlatformCount()
    {
        return (spawnedCount-1) - GameManager.LastPlatformReached;
    }


    public void RemovePlatform(Platform p)
    {
        platforms.Remove(p);
    }


    private void OnGUI() {
        if(GameManager.Instance.DebugEnabled)
        {
            GUILayout.Box("Platform spawn count: " + spawnedCount);
            GUILayout.Box("Last platform reached: " + GameManager.LastPlatformReached);
            GUILayout.Box("Current non critical platform spawn chance: " + nonCtriticalPlatformChanceBySpawnedCount.Evaluate(spawnedCount));
            GUILayout.Box("Current unsafe platform spawn chance: " + unsafePlatformChanceBySpawnedCount.Evaluate(spawnedCount));
            GUILayout.Space(1);

        }
            
    }
}
