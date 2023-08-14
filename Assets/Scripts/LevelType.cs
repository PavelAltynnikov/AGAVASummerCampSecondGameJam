using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Custom/Level", order = 51)]
public class LevelType : ScriptableObject
{
    [SerializeField] private AnimalSet _set;
    [SerializeField] private GameObject _enviroment;

    public AnimalSet AnimalSet => _set;
    public GameObject Enviroment => _enviroment;
}
