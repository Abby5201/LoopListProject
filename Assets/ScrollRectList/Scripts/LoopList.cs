using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopList : MonoBehaviour
{
    public float itemHeight;
    float offsetY;
    private RectTransform contentRect;
    private List<LoopItem> loopItemList;
    private List<Sprite> spriteList;
    private List<LoopItemData> modelList;


    private int startId, endId;
    private RectTransform viewPort;
    private int rowCount = 3;
    

    // Start is called before the first frame update
    void Start()
    {
        loopItemList = new List<LoopItem>();
        spriteList = new List<Sprite>();
        modelList = new List<LoopItemData>();

        GetModel();
        viewPort = transform.Find("Viewport").GetComponent<RectTransform>();
        contentRect = viewPort.transform.Find("Content").GetComponent<RectTransform>();
        GameObject item = Resources.Load<GameObject>("item");
        float height = transform.GetComponent<RectTransform>().rect.height;
        itemHeight = contentRect.GetComponent<GridLayoutGroup>().cellSize.y;
        offsetY = contentRect.GetComponent<GridLayoutGroup>().spacing.y;
        SetContentSize();
        int showNum = GetItemNum(height, offsetY);
        SpanwItem(item,showNum);

        transform.GetComponent<ScrollRect>().onValueChanged.AddListener(OnValueChange);

    }

    private void OnValueChange(Vector2 vector2)
    {
        foreach (var item in loopItemList)
        {
            if (contentRect.anchoredPosition.y >= 0 && contentRect.anchoredPosition.y <= contentRect.rect.height-viewPort.rect.height-itemHeight)
            {
                item.OnValueChange();
            }
        }
    }

    private int GetItemNum(float height,float _offsetY)
    {
        
        int tempNum = Mathf.CeilToInt(height / (itemHeight + _offsetY)) + 1;
        int showNum = modelList.Count < tempNum * rowCount ? modelList.Count : tempNum * rowCount;
        return showNum;
    }

    void SpanwItem(GameObject item,int showNum)
    {
        for (int i = 0; i < showNum; i++)
        {
            var obj = GameObject.Instantiate(item, contentRect).AddComponent<LoopItem>();
            obj.name = i.ToString();
            loopItemList.Add(obj);
            obj.GetDataAddListener(GetData);
            obj.Init(i,showNum, offsetY,rowCount);
        }
    }

    private LoopItemData GetData(int index)
    {
        if (index < 0||index>=modelList.Count)
            return new LoopItemData();
        return modelList[index];
    }



    void GetModel()
    {
        foreach (var item in Resources.LoadAll<Sprite>("Sprites"))
        {
            modelList.Add(new LoopItemData(item, item.name));
        } 
    }

    private void SetContentSize()
    {
        int temp = Mathf.CeilToInt(modelList.Count / (float)rowCount);
        float height = (temp) * itemHeight +(temp-1)* offsetY;

        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, height);
        
    }
}
