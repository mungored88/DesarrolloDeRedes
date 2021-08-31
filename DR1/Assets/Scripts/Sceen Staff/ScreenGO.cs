using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenGO : IScreen
{
    Dictionary<Behaviour, bool> _before;

    public Transform root;

    public ScreenGO(Transform root)
    {
        _before = new Dictionary<Behaviour, bool>();

        this.root = root;

    }

    public void Activate()
    {
        foreach (var keyvalue in _before)
        {
            keyvalue.Key.enabled = keyvalue.Value;
           
        }

        _before.Clear();
    }

    public void Deactivate()
    {
        foreach (var b in root.GetComponentsInChildren<Behaviour>())
        {
            _before[b] = b.enabled;
            b.enabled = false;
}
    }

    public string Free()
    {
        GameObject.Destroy(root.gameObject);
        return "Deletie una pantalla jugable";
    }
}
