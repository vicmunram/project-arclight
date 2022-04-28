using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class TypeSwitchGroup : MonoBehaviour
{
    public TypeSwitch[] auxTypeSwitches;
    public void Switch()
    {
        TypeSwitch[] typeSwitches = gameObject.GetComponentsInChildren<TypeSwitch>();
        if (typeSwitches.Length == 0)
        {
            typeSwitches = auxTypeSwitches;
        }

        foreach (TypeSwitch typeSwitch in typeSwitches)
        {
            typeSwitch.Switch();
        }
    }

}
