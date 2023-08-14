using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using RSG;

public class Net : MonoBehaviour
{
    [SerializeField] private Node _prefab;
    [SerializeField] private float _distance = 3;
    [SerializeField] private AnimalSpawner _spawner;
    [SerializeField] private HandPointer _pointer;
    [SerializeField] private Aviaries _aviaries;
    [SerializeField] private InGameInput _input;

    private List<Node> _nodes = new List<Node>();
    private List<Node> _selectedNodes = new List<Node>();
    private IPromiseTimer _timer = new PromiseTimer();

    public event UnityAction Selected;
    public event UnityAction Deselected;
    public event UnityAction<int> AnimalsChanged;
    public event UnityAction BadClick;
    public event UnityAction GoodClick;
    public event UnityAction AnimalsMoving;
    public event UnityAction BadTry;

    public void BuildLevel(int rows, int cols)
    {
        SpawnGrid(rows, cols);
        foreach (Node node in _nodes)
            node.SetConnected(FindConnected(node));

        foreach (var node in _nodes)
            SpawnAnimal(node);

        _nodes = _nodes.OrderBy(node => node.Index).ToList();
    }

    private void Update()
    {
        _timer.Update(Time.deltaTime);

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.B))
            CalcMove(true, 0);
#endif

        if (Input.GetKeyDown(KeyCode.W))
           StartCoroutine(ShowWave("spin", 0));

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (Time.timeScale == 1)
                Time.timeScale = 0.1f;
            else
                Time.timeScale = 1;
        }

        if ((_pointer == null || _pointer.gameObject.activeSelf == false) && Input.GetMouseButtonDown(0))
            if (_input.IsON)
                HandleClick(Input.mousePosition);
    }

    private void OnEnable()
    {
        _pointer.MouseDown += HandleClick;
        _aviaries.ReleasedAnimals += TakeAnimalsBack;
    }

    private void OnDisable()
    {
        _pointer.MouseDown -= HandleClick;
        _aviaries.ReleasedAnimals -= TakeAnimalsBack;
    }

    private void HandleClick(Vector2 mousePosition)
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition), out RaycastHit hit, 1000))
        {
            if (Time.timeScale == 0)
                return;

            if (hit.transform.TryGetComponent(out Node node) && node.IsBusy)
            {
                List<Node> nearAnimals = new List<Node>() { node };
                bool needLoop = true;
                while (needLoop)
                {
                    needLoop = false;
                    List<Node> newItems = new List<Node>();
                    foreach (Node item in nearAnimals)
                    {
                        List<Node> near = GetNearNodesWithSameAnimal(item);
                        foreach (Node nearItem in near)
                        {
                            if (nearAnimals.Contains(nearItem) == false)
                            {
                                newItems.Add(nearItem);
                                needLoop = true;
                            }
                        }
                    }
                    nearAnimals.AddRange(newItems);
                }

                if (nearAnimals.Count > 0)
                    Select(nearAnimals);
                else
                    Deselect();

                GoodClick?.Invoke();
            }
            else if (hit.transform.TryGetComponent(out Aviary aviary))
            {
                if (_selectedNodes.Count > 0)
                {
                    if (CanMove(_selectedNodes))
                    {
                        if (aviary.TryTakeGroup(_selectedNodes))
                            AnimalsChanged?.Invoke(GetAnimalsCount());

                        CalcMove(false, _selectedNodes.Count * 0.1f);
                        AnimalsMoving?.Invoke();
                        Deselect();


                        GoodClick?.Invoke();
                    }
                    else
                    {
                        BadTry?.Invoke();
                        foreach (Node item in _selectedNodes)
                            item.Animal.Shake();

                        BadClick?.Invoke();
                    }
                }

            }
        }
    }

    private IEnumerator ShowWave(string animation, float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (var item in _nodes)
            if (item.IsBusy)
                item.Animal.PlayAnimation(animation, 0.05f * item.Row);
    }

    private void TakeAnimalsBack(List<Animal> animals)
    {
        CalcMove(true, 0.2f);
        for (int i = 0; i < animals.Count; i++)
            GetNextFreeNode().MakeBusy(animals[i], 0, true);

        CalcMove(false, 0.2f);
        StartCoroutine(ShowWave("fear", 0.7f));
    }

    private Node GetNextFreeNode()
    {
        return _nodes.FirstOrDefault(node => node.IsBusy == false);
    }

    private int GetAnimalsCount()
    {
        int count = 0;
        foreach (var item in _nodes)
            if (item.IsBusy)
                count++;

        return count;
    }

    private void Select(List<Node> nodes)
    {
        _selectedNodes = nodes;

        foreach (Node item in _nodes)
            if (nodes.Contains(item))
                item.Select();
            else
                item.Deselect();

        Selected?.Invoke();
    }

    private void Deselect()
    {
        _selectedNodes.Clear();
        foreach (Node item in _nodes)
                item.Deselect();

        Deselected?.Invoke();
    }

    private bool CanMove(List<Node> nodes)
    {
        foreach (Node node in nodes)
            if (node.OnEdge)
                return true;

        return false;
    }

    private void CalcMove(bool back, float delay)
    {
        bool needUpdate = true;
        while (needUpdate)
        {
            needUpdate = false;
            foreach (var node in _nodes)
            {
                if (TryUpdateNode(node, back, delay))
                    needUpdate = true;
            }
        }
    }

    private List<Node> GetNearNodesWithSameAnimal(Node node)
    {
        List<Node> near = new List<Node>();
        foreach (Node item in node.Connected)
        {
            if (item.IsBusy && item.Animal.ID == node.Animal.ID)
            {
                near.Add(item);
            }
        }
        return near;
    }

    private Node[] FindConnected(Node node)
    {
        return _nodes.Where(item => item != node && Vector3.Distance(node.transform.position, item.transform.position) < _distance * 1.5f).ToArray();
    }

    private void SpawnGrid(int rows, int cols)
    {
        int midRow = rows / 2;
        float dX = _distance;
        float dZ = _distance;// Mathf.Sqrt(dX * dX - (dX * dX) / 4);
        float z0 = 0;
        for (int row = 0; row < rows; row++)
        {
            float z = row * dZ;
            int newCols = cols + row;
            if (row > midRow)
                newCols = cols + midRow + midRow - row;

            float x0 = (newCols - 1) * -dX / 2;
            for (int col = 0; col < newCols; col++)
            {
                float x = dX * col;
                bool isEdge = row == 0;// || row == rows - 1 || col == 0 || col == newCols - 1;
                Vector3 position = new Vector3(x0 + x, 0, z0 + z);
                int index = row * 10 + Mathf.Abs(col - newCols / 2);
                Spawn(position, index, row, isEdge);
            }
        }
    }

    private void Spawn(Vector3 position, int index, int row, bool edge)
    {
        Node node = Instantiate(_prefab, transform.position + position, Quaternion.identity);
        node.Init(index, row, edge);
        _nodes.Add(node);
    }

    private void SpawnAnimal(Node node)
    {
        node.MakeBusy(_spawner.Spawn(node.transform.position));
    }

    private bool TryUpdateNode(Node node, bool back = false, float delay = 0)
    {
        if (node.IsBusy)
        {
            if (back)
            {
                if (node.TryGetFarNode(out Node prefered))
                {
                    Animal animal = node.Animal;
                    node.Clear();
                    prefered.MakeBusy(animal, delay, false);
                    return true;
                }
            }
            else
            {
                if (node.TryGetPreferedNode(out Node prefered))
                {
                    Animal animal = node.Animal;
                    node.Clear();
                    prefered.MakeBusy(animal, delay, false);
                    return true;
                }
            }
        }

        return false;
    }
}
