using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
    public List<GameObject> goPanels = new List<GameObject>();

    public void OpenPanel(GameObject gP) {
        gP.SetActive(true);
        foreach (GameObject go in goPanels)
        {
            if (go != gP) {
                go.SetActive(false);
            }
        }
    }
}
