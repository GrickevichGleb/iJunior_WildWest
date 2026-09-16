using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    [SerializeField] private List<Transform> _points = new List<Transform>();

    public bool TryGetPoint(out Transform point)
    {
        point = null;
        
        if (_points.Count == 0)
            return false;

        point = GetRandomSpawnPoint();
        return true;
    }
    
    private Transform GetRandomSpawnPoint()
    {
        int index = UtilsRandom.GetRandomNumber(0, _points.Count - 1);

        return _points[index];
    }
}
