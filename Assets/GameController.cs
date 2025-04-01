using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameController : SingletonMonoBehaviour<GameController>
{
    public int[] CommonOrder;
    public List<OrderNumber> OrderNumber;
    public StageData[] Level;
    public Transform[] Slots;

    public Transform SpawnPoint, InitPoint;
    public ParticleSystem[] MergeFx;

    public GameObject Tutorial;

    public bool DestroyeMode { set; get; } = false;

    public bool Moveable { set; get; } = true;
    public bool GameEnd { set; get; } = false;

    //public FoodItem Selected { set; get; } = null;

    public List<FoodItem> SelectedItems = new List<FoodItem>();

    public List<Order> OrderList = new List<Order>();

    public List<FoodItem> ItemList = new List<FoodItem>();
    public List<FoodItem> HintList = new List<FoodItem>();

    public GuessController[] Guess;

    private GameObject _go;

    private List<int> _orderType = new List<int>();

    public static int MAX_ITEM = 35;

    private string _objectPath = "";

    private void Start()
    {
        if (!UserData.NewMode)
            _objectPath = "";
        else
            _objectPath = "Mode2/";

        UpdateOrderType();

        {
            // Spawn guess

            Guess[0].LoadSkinMainGame();

            DOTween.Sequence().AppendInterval(Random.Range(2.5f, 5f)).AppendCallback(() =>
                {
                    Guess[1].LoadSkinMainGame();
                    ;
                })
                .AppendInterval(Random.Range(2.5f, 5f)).AppendCallback(() =>
                {
                    Guess[2].LoadSkinMainGame();
                    ;
                });
        }

        float x = 0, y = 0, z = 0;

        List<int> typeList = new List<int>();

        if (UserData.LevelIndex < 5)
        {
            for (int i = 0; i < Level[UserData.LevelIndex].ItemType.Length; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    typeList.Add(Level[UserData.LevelIndex].ItemType[i]);
                }
            }
        }
        else // Random stage
        {
            List<int> id = new List<int>();

            for (int i = 0; i < 8; i++)
            {
                int r = Random.Range(0, 21);

                while (id.Contains(r))
                    r = Random.Range(0, 21);

                id.Add(r);
            }

            foreach (var item in id)
            {
                for (int j = 0; j < 9; j++)
                {
                    typeList.Add(item);
                }
            }
        }

        typeList = Ext.Shuffle(typeList);

        int count = 0;

        for (int i = 0; i < typeList.Count; i++)
        {
            _go = Instantiate(Resources.Load<GameObject>($"{_objectPath}Item_{typeList[i]}_{Random.Range(0, 0)}"),
                new Vector3(InitPoint.position.x + x, InitPoint.position.y + z, InitPoint.position.z + y),
                Quaternion.Euler(0, 0, 0), transform);

            _go.transform.eulerAngles = new Vector3(0, Random.value * 360, 0);

            var body = _go.GetComponent<Rigidbody>();

            body.isKinematic = false;
            body.useGravity = true;

            x += 2;

            count++;

            if (count % 5 == 0)
            {
                x = 0;
                y -= 1.5f;
            }

            if (count % 35 == 0)
            {
                x = 0;
                y = 0;
                z = 1;
            }
        }

        DOVirtual.DelayedCall(1, () =>
        {
            if (UserData.LevelIndex <= 1)
            {
                GameAnalyticsManager.Instance.TrackEvent(string.Format("Stage1:WholeProgress:Tutorial:start"));
            }
            else
            {
                GameAnalyticsManager.Instance.TrackEvent(string.Format("Stage{0}:WholeProgress:Level:start",
                    UserData.LevelIndex));
            }
        });
    }

    public void UpdateOrderType()
    {
        _orderType.Clear();

        foreach (var item in OrderList)
        {
            if (!_orderType.Contains(item.Type))
                _orderType.Add(item.Type);
        }
    }


    float _timer = 0;
    public int TutorialIndex { set; get; } = 0;

    public void ResetTimerTutorial()
    {
        _timer = 0;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        //List<string> checklist = new List<string>();
        Dictionary<string, int> type = new Dictionary<string, int>();

        for (int i = 0; i < ItemList.Count; i++)
        {
            string key = $"{ItemList[i].Type}_{ItemList[i].Level}";

            foreach (var item in Instance.OrderList)
            {
                if (item.Type == ItemList[i].Type
                    && item.Level == ItemList[i].Level
                    && type.GetValueOrDefault(key, 0) < item.Number)
                {
                    int value = type.GetValueOrDefault(key, 0);
                    type.Remove(key);

                    type.Add(key, value + 1);

                    //checklist.Add(key);

                    ItemList[i].CheckMark.SetActive(true);

                    break;
                }
                else
                {
                    ItemList[i].CheckMark.SetActive(false);
                }
            }

            ItemList[i].CheckMark.transform.eulerAngles = new Vector3(0, 0, 0);
            //ItemList[i].transform.eulerAngles = new Vector3(0, 0, ItemList[i].transform.eulerAngles.z);
            //ItemList[i].transform.position = new Vector3(ItemList[i].transform.position.x, ItemList[i].transform.position.y, 0);
        }

        if (_timer > 2f && UserData.LevelIndex == 0 && UserData.OrderIndex == 0 &&
            !UIController.Instance.Hand.activeSelf)
        {
            foreach (var item in ItemList)
            {
                if (!SelectedItems.Contains(item))
                {
                    Tutorial.transform.position = new Vector3(item.transform.position.x + 1.0f,
                        item.transform.position.y - 1.0f, Tutorial.transform.position.z);

                    Tutorial.SetActive(true);

                    break;
                }
                else
                {
                    Tutorial.SetActive(false);
                }
            }
        }
        else if (_timer > 2f && UserData.LevelIndex == 1 && UserData.OrderIndex == 1 &&
                 !UIController.Instance.Hand.activeSelf)
        {
            int count = 0;
            int target = 0;

            foreach (var item in ItemList)
            {
                if (item.Level == 1)
                    count++;
            }

            if (count >= 3)
            {
                target = 1;
            }

            foreach (var item in ItemList)
            {
                if (!SelectedItems.Contains(item) && item.Level == target)
                {
                    Tutorial.transform.position = new Vector3(item.transform.position.x + 1.0f,
                        item.transform.position.y - 1.0f, Tutorial.transform.position.z);

                    Tutorial.SetActive(true);

                    break;
                }
                else
                {
                    Tutorial.SetActive(false);
                }
            }
        }
        else
        {
            Tutorial.SetActive(false);
        }
    }

    public void ResetHint()
    {
        foreach (var hint in HintList)
        {
            if (hint)
                hint.Deselect();
        }

        HintList.Clear();
    }

    Sequence _seq;

    public void Select(FoodItem item)
    {
        SelectedItems.Add(item);

        item.transform.parent = Slots[SelectedItems.Count - 1];

        item.MoveToSlot();

        _seq.Kill();
        _seq = DOTween.Sequence().AppendInterval(0.35f).AppendCallback(() =>
        {
            if (SelectedItems.Count >= 3)
            {
                Merge();
            }
        });
    }

    public void DeselectAll()
    {
        //Selected = null;

        foreach (var i in SelectedItems)
        {
            i?.Deselect();
        }

        SelectedItems.Clear();
    }

    public void Deselect(FoodItem item)
    {
        if (SelectedItems.Contains(item))
            SelectedItems.Remove(item);

        //Selected = null;
    }

    public void Merge()
    {
        bool resetList = false;
        List<FoodItem> list = new List<FoodItem>();
        for (int i = 0; i < SelectedItems.Count; i++)
        {
            list.Clear();
            // Find match 3 items
            for (int j = 0; j < SelectedItems.Count; j++)
            {
                if (SelectedItems[j].Type == SelectedItems[i].Type && !list.Contains(SelectedItems[j]))
                {
                    list.Add(SelectedItems[j]);
                    if (list.Count >= 3)
                        break;
                }
            }

            //doto
        }

        if (list.Count >= 3)
        {
            resetList = true;

            int type = list[0].Type;

            // Merge
            MergeAnimation(list, () =>
            {
                for (int j = 0; j < list.Count; j++)
                {
                    MergeFx[j].transform.position = new Vector3(list[j].gameObject.transform.position.x,
                        2, list[j].gameObject.transform.position.z);
                    MergeFx[j].Play();

                    SelectedItems.Remove(list[j]);
                    if (j != 1) DestroyImmediate(list[j].gameObject);
                }

                if (resetList)
                {
                    for (int i = 0; i < SelectedItems.Count; i++)
                    {
                        SelectedItems[i].transform.parent = Slots[i];
                        SelectedItems[i].MoveToSlot(true);
                    }
                }

                // Serve
                foreach (var item in Guess)
                {
                    if (item.CheckForOrder(type))
                    {
                        item.Serve(list[1]);

                        break;
                    }
                }
            });
        }

        if (SelectedItems.Count >= 7)
        {
            Moveable = false;
            UIController.Instance.UIOver.Show();
        }
    }

    public void Suffle()
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            int r = Random.Range(0, ItemList.Count);
            Vector3 p = ItemList[i].OriginPos;

            ItemList[i].OriginPos = ItemList[r].OriginPos;
            ItemList[r].OriginPos = p;
        }

        foreach (var item in ItemList)
        {
            item.ResetPosition();
        }

        SelectedItems.Clear();
    }

    public void Hint()
    {
        for (int i = 3; i >= 0; i--)
        {
            for (int j = 0; j < 9; j++)
            {
                List<FoodItem> list = new List<FoodItem>();

                foreach (var item in ItemList)
                {
                    if (item.Type == j && item.Level == i)
                    {
                        list.Add(item);

                        if (list.Count >= 3)
                        {
                            break;
                        }
                    }
                }

                if (list.Count >= 3)
                {
                    foreach (var item in list)
                    {
                        item.Select();
                    }

                    HintList = list;

                    return;
                }
            }
        }
    }

    public void Destroy(FoodItem item)
    {
        Vector3 pos = item.transform.position;

        MergeFx[0].transform.position = pos;
        MergeFx[0].Play();

        UserData.OnNewObject.Invoke();

        UIController.Instance.BombLabel.SetActive(false);
        DestroyeMode = false;


        DestroyImmediate(item.gameObject);
    }

    private void MergeAnimation(List<FoodItem> listMerge, System.Action onComplete)
    {
        DOVirtual.DelayedCall(0.2f, () =>
        {
            Vector3 posMerge = Vector3.zero;

            //anim up
            for (int i = 0; i < listMerge.Count; i++)
            {
                listMerge[i].transform.DOScale(Vector3.one * 0.3f, 0.2f);
                if (i == 1) posMerge = listMerge[i].transform.position;
            }

            //amim move
            for (int i = 0; i < listMerge.Count; i++)
            {
                listMerge[i].transform.DOMove(new Vector3(posMerge.x, posMerge.y, posMerge.z + 1), 0.4f)
                    .SetDelay(0.2f).SetEase(Ease.InOutElastic);
            }

            DOVirtual.DelayedCall(0.7f, () => { onComplete?.Invoke(); });
        });
    }
}

[System.Serializable]
public class OrderNumber
{
    public int Order;
}