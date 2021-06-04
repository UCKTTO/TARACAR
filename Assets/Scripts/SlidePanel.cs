using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlidePanel : MonoBehaviour
{
    public GameObject childPanel;
    public VerticalLayoutGroup parentVLG;
    
    public void ToggleChildPanel() {
        childPanel.SetActive(!childPanel.activeSelf);
        // parentVLG.childForceExpandHeight = !parentVLG.childForceExpandHeight;
        // parentVLG.childForceExpandHeight = false;
        StartCoroutine(ToggleVLG());
    }

    private IEnumerator ToggleVLG() {
        yield return new WaitForEndOfFrame();
        parentVLG.enabled = !parentVLG.enabled;
        parentVLG.enabled = !parentVLG.enabled;
    }
}
