using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using RSG;

public class Node : MonoBehaviour
{
    private int _index;
    private bool _onEdge = false;
    private Node[] _connectedNodes;
    private Animal _animal;
    private IPromiseTimer _timer = new PromiseTimer();

    public int Index => _index;
    public bool IsBusy => _animal != null;
    public bool OnEdge => _onEdge;
    public Animal Animal => _animal;
    public Node[] Connected => _connectedNodes;
    public int Row { get; private set; } = 0;

    public void Init(int index, int row, bool edge)
    {
        _index = index;
        _onEdge = edge;
        Row = row;
    }

    public void SetConnected(Node[] nodes)
    {
        _connectedNodes = nodes;
    }

    public void MakeBusy(Animal animal)
    {
        _animal = animal;
    }

    public void MakeBusy(Animal animal, float delay, bool fromAviary)
    {
        _animal = animal;
        if (fromAviary)
        {
            _animal.MoveFromAviary(transform.position);
        }
        else
        {
            _timer.WaitFor(delay).Then(() =>
            {
                _animal.Go(transform.position, 0.5f);
            });
        }
    }

    public void Clear()
    {
        _animal = null;
    }

    public bool TryGetPreferedNode(out Node prefered)
    {
        int min = _index;
        prefered = null;
        foreach (Node node in _connectedNodes)
        {
            if (!node.IsBusy && node.Index < min)
            {
                min = node.Index;
                prefered = node;
            }
        }

        return prefered != null;
    }

    public bool TryGetFarNode(out Node farNode)
    {
        int max = _index;
        farNode = null;
        foreach (Node node in _connectedNodes)
        {
            if (!node.IsBusy && node.Index > max)
            {
                max = node.Index;
                farNode = node;
            }
        }

        return farNode != null;
    }

    public void Select()
    {
        _animal?.Select();
    }

    public void Deselect()
    {
        _animal?.Unselect();
    }

    private void Update()
    {
        _timer.Update(Time.deltaTime);
    }
}
