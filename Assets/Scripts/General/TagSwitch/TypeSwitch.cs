using UnityEngine;
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
            SwitchLayer(layerB);
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteB;
            typeSwitchToB();
        }
        else
        {
            gameObject.tag = tagA;
            SwitchLayer(layerA);
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteA;
            typeSwitchToA();
        }
    }

    private void SwitchLayer(string layer)
    {
        gameObject.layer = LayerMask.NameToLayer(layer);
        if (layer.Equals(Layers.triggered))
        {
            gameObject.GetComponent<Collider2D>().isTrigger = true;
        }
    }

    public abstract void typeSwitchToA();
    public abstract void typeSwitchToB();
}
