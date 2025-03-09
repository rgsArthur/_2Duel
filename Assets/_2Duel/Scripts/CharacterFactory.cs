using UnityEngine;

public class CharacterFactory : MonoBehaviour
{
    [SerializeField] private GameObject[] _prefabs;
    [SerializeField] private Transform[] _spawnPoints;

    public Character CreateRandom(int spawnIndex)
    {
        var prefab = _prefabs[Random.Range(0, _prefabs.Length)];
        var instance = Instantiate(prefab, _spawnPoints[spawnIndex].position, Quaternion.identity);
        return instance.GetComponent<Character>();
    }
}
