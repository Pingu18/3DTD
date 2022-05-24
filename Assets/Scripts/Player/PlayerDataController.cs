using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataController : MonoBehaviour
{
    [SerializeField] CameraController cameraCon;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("playerSens"))
        {
            PlayerPrefs.SetFloat("playerSens", 5.0f);
        }

        cameraCon.setSens(PlayerPrefs.GetFloat("playerSens"));
    }

    public void saveSens(float newSens)
    {
        PlayerPrefs.SetFloat("playerSens", newSens);
        cameraCon.setSens(PlayerPrefs.GetFloat("playerSens"));
    }

    public float getSens()
    {
        return PlayerPrefs.GetFloat("playerSens");
    }
}
