using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopItem : MonoBehaviour
{
    private RectTransform _rect;
    public RectTransform Rect
    {
        get
        {
            if (_rect == null)
                _rect = GetComponent<RectTransform>();
            return _rect;
        }
    }

    private Image _sprite;
    public Image Sprite
    {
        get
        {
            if (_sprite == null)
                _sprite =transform.Find("Image").GetComponent<Image>();
            return _sprite;
        }
    }

    private Text _text;
    public Text Text
    {
        get
        {
            if (_text == null)
                _text = transform.GetComponentInChildren<Text>();
            return _text;
        }
    }

    private Func<int, LoopItemData> _getData;
    private RectTransform content;

    
    private LoopItemData modelData = new LoopItemData();

    private int startId;
    private int endId;
    private float offset;
    private int showItemNum;
    private int selfId;
    private int rowCount;

    public void Init(int id, int showNum,float offsetY,int count)
    {
        selfId = -1;
        content = transform.parent.GetComponent<RectTransform>();
        offset = offsetY;
        showItemNum = showNum;
        rowCount = count;
        InitData(id);
    }
    private void InitData( int index)
    {
        if (JudgeIdValid(index))
        {
            modelData = _getData(index);
            selfId = index;
            transform.name = selfId.ToString();
            Text.text = modelData.content;
            Sprite.sprite = modelData.sprite;
        }
    }

    public void OnValueChange()
    {
        UpdateIdRange();
        JudgeSelfIsInRange();
    }
    public void GetDataAddListener(Func<int,LoopItemData> getData)
    {
        _getData = getData;
    }

    private void UpdateIdRange()
    {
        startId = Mathf.FloorToInt( content.anchoredPosition.y / (Rect.rect.height + offset));
        endId = startId + showItemNum/rowCount - 1;
    }
    

    private void JudgeSelfIsInRange()
    {
        //lineNumĿǰ����������������Ұ��Χ�ڣ�
        int currentLineNum = selfId/3;
        int off = 0;
        if (currentLineNum < startId)
        {
            off = startId - currentLineNum - 1;
            ChangeId(endId-off,endId);
        }else if (currentLineNum > endId)
        {
            off = currentLineNum - endId - 1;
            ChangeId(startId+off, startId);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="nextCurrentlineNum">������Ұ��Χ�ڵ� Ӧ���ڵڼ���</param>
    /// <param name="selfId">�����selfId���޸�֮ǰ�ϵ�id</param>
    /// <param name="lineNum"></param>
    private void ChangeId(int nextCurrentlineNum, int newLineNum)
    {
        int newId = newLineNum * rowCount + selfId % 3;//temp��ʵ�ʵ�id����Ӧ�����б��index
        if (newId != selfId && JudgeIdValid(selfId))
        {
            SetObjModel(newId);//
        }
        SetPos(nextCurrentlineNum);
    }
    /// <summary>
    /// ���������ͼƬ
    /// </summary>
    /// <param name="id"></param>
    /// <param name="index">��ʵ������id</param>
    private void SetObjModel(int index)
    {
        modelData = _getData(index);
        selfId = index;
        transform.name = selfId.ToString();
        Text.text = modelData.content;
        Sprite.sprite = modelData.sprite;
    }

    private void SetPos(int id)
    {
        Rect.anchoredPosition = new Vector2(Rect.anchoredPosition.x, -id * (Rect.sizeDelta.y+offset));
    }

    private bool JudgeIdValid(int id)
    {
        return !_getData(id).Equals(new LoopItemData());
    }

}
