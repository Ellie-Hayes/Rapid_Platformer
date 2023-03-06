using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTileset : MonoBehaviour
{
    [SerializeField] GameObject nightTileset;
    [SerializeField] GameObject dayTileset;
    public void NightTilesetSwitch() { nightTileset.SetActive(!nightTileset.activeSelf); }
    public void DayTilesetSwitch() { dayTileset.SetActive(!dayTileset.activeSelf); }
   
}
