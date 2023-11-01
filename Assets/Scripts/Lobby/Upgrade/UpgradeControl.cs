using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeControl : MonoBehaviour
{
    private GameObject upgradeUI;

    // Start is called before the first frame update
    void Start()
    {
        upgradeUI = GameObject.FindGameObjectWithTag("UpgradeUI");

        upgradeUI.transform.position = new Vector3(960, 540, 0);
        upgradeUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickUpgrade()
    {
        upgradeUI.SetActive(true);
    }

    public void OnClickUpgradeX()
    {
        upgradeUI.SetActive(false);
    }
}
