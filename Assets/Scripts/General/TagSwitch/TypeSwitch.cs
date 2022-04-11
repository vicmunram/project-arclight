﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public abstract class TypeSwitch : MonoBehaviour
{
    public string tagA;
    public string tagB;
    public string layerA;
    public string layerB;
    public Sprite spriteA;
    public Sprite spriteB;

    public void Switch()
    {
        if (tag.Equals(tagA))
        {
            gameObject.tag = tagB;
            gameObject.layer = LayerMask.NameToLayer(layerB);
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteB;
            typeSwitchToB();
        }
        else
        {
            gameObject.tag = tagA;
            gameObject.layer = LayerMask.NameToLayer(layerA);
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteA;
            typeSwitchToA();
        }
    }

    public abstract void typeSwitchToA();
    public abstract void typeSwitchToB();
}
