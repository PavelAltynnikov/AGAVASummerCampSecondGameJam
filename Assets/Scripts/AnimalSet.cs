using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAnimalSet", menuName = "Custom/AnimalSet", order = 51)]
public class AnimalSet : ScriptableObject
{
    [SerializeField] private List<Animal> _animals;

    public int Size => _animals.Count;

    private void Awake()
    {
        for (var i=0; i< _animals.Count; i++)
        {
            _animals[i].SetID(i);
        }
    }

    public Animal GetAnimalTemplate(int index)
    {
        return _animals[index];
    }
}
