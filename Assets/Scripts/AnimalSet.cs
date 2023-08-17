using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAnimalSet", menuName = "Custom/AnimalSet", order = 51)]
public class AnimalSet : ScriptableObject
{
    [SerializeField] private List<Animal> _animals;

    public int Size => _animals.Count;

    public Animal GetAnimalTemplate(int index)
    {
        return _animals[index];
    }
}
