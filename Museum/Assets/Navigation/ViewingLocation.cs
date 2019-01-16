using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ViewingLocation : MonoBehaviour
{

    public ViewingLocationRuntimeSet RuntimeSet;
    public int Area;

    void OnEnable()
    {
        RuntimeSet.Add(this);
    }

    void OnDisable()
    {
        RuntimeSet.Remove(this);
    }
}
