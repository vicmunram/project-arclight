using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Text formattedTime;

    void Start()
    {
        formattedTime.text = Timer.enabled ? Timer.GetFormattedTime() : null;
    }

    void Update()
    {
        string time = Timer.GetFormattedTime();
        if (Timer.enabled && !time.Equals(formattedTime.text))
        {
            formattedTime.text = time;
        }
    }

}
