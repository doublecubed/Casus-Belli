using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class HierarchySnapshotter : MonoBehaviour
{
    public static HierarchySnapshotter Instance;

    //public delegate void HierarchyChangeHandler();
    public event Action OnHierarchyChanged;

    private Dictionary<Transform, Transform> hierarchySnapshot = new Dictionary<Transform, Transform>();

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        TakeSnapshot();
    }

    private void TakeSnapshot()
    {
        var currentSnapshot = new Dictionary<Transform, Transform>();
        foreach (var go in FindObjectsOfType<Transform>())
        {
            currentSnapshot[go] = go.parent;
        }

        if (!DictionaryEquals(hierarchySnapshot, currentSnapshot))
        {
            hierarchySnapshot = currentSnapshot;
            OnHierarchyChanged?.Invoke();
        }
    }

    private bool DictionaryEquals(Dictionary<Transform, Transform> dict1, Dictionary<Transform, Transform> dict2)
    {
        return dict1.Count == dict2.Count && !dict1.Except(dict2).Any();
    }
}
