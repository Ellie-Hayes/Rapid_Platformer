using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialText : MonoBehaviour
{
    PlayerCont cont;
    public List<TextMeshProUGUI> tutorialTexts = new List<TextMeshProUGUI>();
    private void Start()
    {
        cont = GetComponent<PlayerCont>();
    }

    private void Update()
    {
        if (cont.IsDay) { }
    }
}
