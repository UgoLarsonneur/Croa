using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlatformData", menuName = "ScriptableObject/PlatformData", order = 0)]
public class PlatformData : ScriptableObject {

    [System.Serializable]
    struct WeightedPlatform {
        public GameObject platform;
        public float weight;
    }

    //Je n'ai pas eu le temps d'en ajouter, mais l'intêret est qu'il pourrait y avoir plusieurs plateforme de chaque type différentes...
    [SerializeField] private List<WeightedPlatform> safePlatform;

    [SerializeField] private List<WeightedPlatform> semiSafePlatform;

    [SerializeField] private List<WeightedPlatform> unsafePlatform; //Can disapear: are not part of the critical path, to ensure there is always a path forward


    public GameObject chooseSafePlatform()
    {
        return choosePlatform(safePlatform);
    }

    public GameObject chooseSemiSafePlatform()
    {
        return choosePlatform(semiSafePlatform);
    }

    public GameObject chooseUnsafePlatform()
    {
        return choosePlatform(unsafePlatform);
    }

    private GameObject choosePlatform(List<WeightedPlatform> list)
    {
        if(list.Count == 0)
        {
            Debug.LogError("No platform to select");
            return null;
        }

        float weightTotal = 0f;
        foreach (WeightedPlatform item in list)
        {
            weightTotal += item.weight;
        }

        float choice = Random.Range(0f, weightTotal);
        float selectThreshold = 0;
        foreach (WeightedPlatform item in list)
        {
            selectThreshold += item.weight;
            if(choice <= selectThreshold)
                return item.platform;
        }

        return list[0].platform; //devrait être unreachable, mais omnisharp grogne
    }
    
}