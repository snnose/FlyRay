using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    private int clearQuestNum = 0;

    public List<Sprite> sprites = new List<Sprite>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        clearQuestNum = FlagCheck();
        
        switch(clearQuestNum)
        {
            case 0:
                this.GetComponent<Image>().sprite = sprites[0];
                break;
            case 1:
                this.GetComponent<Image>().sprite = sprites[1];
                break;
            case 2:
                this.GetComponent<Image>().sprite = sprites[1];
                break;
            case 3:
                this.GetComponent<Image>().sprite = sprites[2];
                break;
            case 4:
                this.GetComponent<Image>().sprite = sprites[3];
                break;
            default:
                break;
        }
    }

    private int FlagCheck()
    {
        int i = 0;

        if (DataManager.Instance.playerData.main1)
            i++;
        if (DataManager.Instance.playerData.main2)
            i++;
        if (DataManager.Instance.playerData.main3)
            i++;
        if (DataManager.Instance.playerData.main4)
            i++;

        return i;
    }
}
