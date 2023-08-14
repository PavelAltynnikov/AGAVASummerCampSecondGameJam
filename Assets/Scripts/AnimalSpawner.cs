using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnAnimals
{

}

public class AnimalSpawner : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] [Range(0,1)] private float _AnimalRatio;

    private AnimalSet _set;
    private List<Animal> _animals = new List<Animal>();
    private int[] _spawned;

    private void OnEnable()
    {
        _game.LevelStarted += ChangeAnimalSet;
    }

    private void OnDisable()
    {
        _game.LevelStarted -= ChangeAnimalSet;
    }

    public Animal Spawn(Vector3 position)
    {
        var index = NewAnimalIndex();
        Animal animal = Instantiate(_set.GetAnimalTemplate(index), position, Quaternion.LookRotation(Vector3.back, Vector3.up));
        _animals.Add(animal);
        _spawned[index]++;
        return animal;
    }

    private void ChangeAnimalSet(int level, LevelType type)
    {
        _set = type.AnimalSet;
        _spawned = new int[_set.Size];
    }

    private int NewAnimalIndex()
    {
        var animalsCount = 0;
        foreach (var count in _spawned)
            animalsCount += count;
        var ratios = new float[_spawned.Length];
        for (var i=0; i< _set.Size; i++)
        {
            if (animalsCount - _spawned[i] > 0)
            {
                ratios[i] = (float)_spawned[i] / ((animalsCount - _spawned[i])/(_set.Size -1));
            }
        }
        var minRatioIndex = Random.Range(0, _set.Size);
        for (var i = 0; i < _set.Size; i++)
        {
            if (ratios[i] < _AnimalRatio && ratios[i] < ratios[minRatioIndex])
            {
                minRatioIndex = i;
            }
        }
        return minRatioIndex;
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        Debug.Log(s);
    //    }
    //}

    //private void WriteString(string data)
    //{
    //    string path = "Assets/test.json";

    //    StreamWriter writer = new StreamWriter(path, false);
    //    writer.Write(data);
    //    writer.Close();
    //}

    //private string ReadString()
    //{
    //    string path = "Assets/test.json";

    //    StreamReader reader = new StreamReader(path);
    //    string result = reader.ReadToEnd();
    //    reader.Close();

    //    return result;
    //}
}
