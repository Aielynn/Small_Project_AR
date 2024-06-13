using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject hearts;
    private GameObject items;

    private void OnEnable()
    {
        GameManager.onStart += SetSprites;
        GameManager.onCollide += UpdateItemsUI;
        MoveProp.onPropOutOfRange += UpdateHealthUI;
        AnimationScript.onBombHit += UpdateHealthUI;
    }

    void OnDestroy()
    {
        GameManager.onStart -= SetSprites;
        GameManager.onCollide -= UpdateItemsUI;
        MoveProp.onPropOutOfRange -= UpdateHealthUI;
        AnimationScript.onBombHit -= UpdateHealthUI;
    }

    private void SetSprites(List<string> fileNames)
    {
        hearts = GameObject.FindWithTag("Hearts");
        items = GameObject.FindWithTag("Items");
        UnityEngine.Debug.Log(items);

        int ind = 0;
        foreach(var spriteName in fileNames)
        {
            Sprite sprite = Resources.Load<Sprite>("Sprites/"+spriteName);
            Image img = items.transform.GetChild(ind).gameObject.GetComponent<Image>();
            img.enabled = true;
            img.sprite = sprite;
            //items.transform.GetChild(ind).gameObject.GetComponent<Image>().sprite = sprite;
            UnityEngine.Debug.Log("Sprite set");
            ind++;
        }
    }

    private void UpdateHealthUI()
    {
        GameObject lastHeart = hearts.transform.GetChild(0).gameObject;
        Destroy(lastHeart);
    }

    private void UpdateItemsUI(int instanceIndex)
    {
        //must be in order
        GameObject itemToRemove = items.transform.GetChild(instanceIndex).gameObject;
        Destroy(itemToRemove);
    }
}
