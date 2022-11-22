using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class UIWeaponSlotManager : MonoBehaviour
{
    public static UIWeaponSlotManager I { get; private set; }

    public List<UIWeaponSlot> slotList;

    SlotListData slotListData;

    [SerializeField] UIWeaponSlot Prefab_Slot;

    public RectTransform rect;

    private void Awake()
    {
        I = this;
        print(Application.persistentDataPath);

        InitSlotData();
        InitSlot();
    }

    void InitSlot()
    {
        foreach (SlotData sd in slotListData.slotDataList)
        {
            UIWeaponSlot slot = Instantiate<UIWeaponSlot>(Prefab_Slot, transform);
            slot.slotData = sd;
            slotList.Add(slot);
        }
    }

    void InitSlotData()
    {
        string dataPath = $"/SlotData.json";

        print(JsonData.isFileExist(dataPath));

        if (JsonData.isFileExist(dataPath) == false)
        {
            slotListData = new SlotListData();
            slotListData.slotDataList = new List<SlotData>(50);

            slotListData.slotDataList.Add(new SlotData(CreateNewId(), 0));
            slotListData.slotDataList.Add(new SlotData(CreateNewId(), 1));

            JsonData.SaveObj(slotListData, dataPath);
        }
        else
        {
            string text = JsonData.LoadJson(dataPath);
            slotListData = JsonUtility.FromJson<SlotListData>(text);
        }
    }

    int CreateNewId()
    {
        int newId = 0;

        while (true)
        {
            bool isExist = false;
            foreach (SlotData sd in slotListData.slotDataList)
            {
                if (newId == sd.id)
                {
                    isExist = true;
                    break;
                }
            }

            if (isExist == false) return newId;

            newId++;
        }
    }
}

[System.Serializable]
public class SlotListData
{
    public List<SlotData> slotDataList;
}

[System.Serializable]
public class SlotData
{
    public int id;
    public int order;

    public SlotData(int _id, int _order)
    {
        id = _id;
        order = _order;
    }
}