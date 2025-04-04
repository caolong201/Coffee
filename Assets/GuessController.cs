using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Random = UnityEngine.Random;

public class GuessController : MonoBehaviour
{
    public int Cost;
    public int Index;
    public Transform ModelHost;

    public ParticleSystem SellFx;
    public GameObject ReadyFx;
    //public GameObject  UnlockButton;

    public Transform[] ItemPosition;

    //public TextMeshProUGUI CoinLbl;

    public GameObject ItemFrame, Checkmark;

    private GameObject _go;

    private List<Order> _order = new List<Order>();
    private List<GameObject> _obj = new List<GameObject>();

    private bool _unlocked = false;

    private Transform _model;

    private int _money;

    [SerializeField] private SpriteRenderer _sprite;

    private float _timer = 0f;
    private bool _isRunning = false;

    private bool _isMainGame = true;

    private ParticleSystem _particleEmotion = null;

    public void LoadSkinMainGame()
    {
        _isMainGame = true;
        gameObject.SetActive(true);
        ModelHost.gameObject.SetActive(true);

        _unlocked = true;

        LoadSkin();
    }

    public void LoadSkinMiniGame()
    {
        _isMainGame = false;
        int index = Random.Range(1, 101);
        Sprite mySprite = Resources.Load<Sprite>("Guess/CityCharacters" + index);

        if (mySprite != null)
        {
            _sprite.sprite = mySprite;
        }
        else
        {
            Debug.LogError("Failed to load sprite!");
        }

        ModelHost.gameObject.SetActive(true);
        ModelHost.DOLocalMoveY(0, 0.5f).OnComplete(() =>
        {
            _timer = 0;
            _isRunning = true;
        });
    }

    private void LoadSkin()
    {
        int index = Random.Range(1, 101);
        Sprite mySprite = Resources.Load<Sprite>("Guess/CityCharacters" + index);

        if (mySprite != null)
        {
            _sprite.sprite = mySprite;
        }
        else
        {
            Debug.LogError("Failed to load sprite!");
        }

        ModelHost.gameObject.SetActive(true);
        _unlocked = true;

        ModelHost.DOLocalMoveY(0, 0.5f).OnComplete(() =>
        {
            ItemFrame.SetActive(true);
            ItemFrame.transform.localScale = Vector3.zero;
            ItemFrame.transform.DOScale(new Vector3(-0.35f, 0.4f, 0.4f), 0.3f).SetEase(Ease.OutBounce);
            ItemFrame.GetComponent<SpriteRenderer>().DOFade(1, 0.1f);
            GenerateOrder();
            _timer = 0;
            _isRunning = true;
        });
    }

    void Update()
    {
        if (!_isRunning)
            return;

        _timer += Time.deltaTime;
        if (_timer >= 6f)
        {
            _timer = 0;

            if (GameUI.Instance.ActiveCount > 1 || (_isMainGame && GameController.Instance.GameEnd) || (_isMainGame && UserData.LevelIndex == 0))
            {
                return;
            }

            if (_particleEmotion != null) _particleEmotion.gameObject.SetActive(false);

            Debug.Log("play effect angry random");
            int index = Random.Range(3, 7);
            var effType = (EEffectType)index;
            if (_isMainGame)
            {
                _particleEmotion = EffectManager.Instance.PlayEffect<ParticleSystem>(effType,
                    new Vector3(ModelHost.position.x, ModelHost.position.y, ModelHost.position.z + 5),
                    Quaternion.Euler(Vector3.zero));

                _particleEmotion.gameObject.layer = LayerMask.NameToLayer("Top");
                var main = _particleEmotion.main;
                main.startSize = 1f;
            }
            else
            {
                _particleEmotion = EffectManager.Instance.PlayEffect<ParticleSystem>(effType, new Vector3(-1f, 1.8f, -5),
                    Quaternion.Euler(new Vector3(-90, 0, 0)));

                _particleEmotion.gameObject.layer = LayerMask.NameToLayer("Default");
                var main = _particleEmotion.main;
                main.startSize = 0.3f;
            }
        }
    }

    public void Unlock()
    {
        LoadSkinMainGame();
    }

    void GenerateOrder()
    {
        _money = 0;

        {
            int type = GameController.Instance.ItemList[Random.Range(0, GameController.Instance.ItemList.Count)].Type;

            int count = 0;

            bool dup = false;

            do
            {
                dup = false;
                type = GameController.Instance.ItemList[Random.Range(0, GameController.Instance.ItemList.Count)].Type;
                count = 0;

                foreach (var item in GameController.Instance.OrderList)
                {
                    if (item.Type == type)
                    {
                        dup = true;

                        break;
                    }
                }

                if (!dup)
                {
                    foreach (var item in GameController.Instance.ItemList)
                    {
                        if (item.Type == type)
                        {
                            count++;

                            if (count >= 3)
                                break;
                        }
                    }
                }
            } while (count < 3 || dup); // Check for count and duplicate order

            {
                _go = Instantiate(Resources.Load<GameObject>($"Icon3D/Item_{type}_{0}"));

                _go.transform.parent = ItemPosition[0];

                _go.transform.localScale = Vector3.one * 1.75f;
                _go.transform.localPosition = Vector3.zero;
                _go.transform.localEulerAngles = Vector3.zero;

                //_go.transform.DOScale(1, 0.15f);

                _obj.Add(_go);

                var order = new Order(type, 0);

                GameController.Instance.OrderList.Add(order);

                _order.Add(order);

                _money += (0 + 1) * 5 + Random.Range(0, 10);

                if (UserData.LevelIndex == 0 && ETutorialType.HighlightItem == UITutorialMainGame.Instance.TutorialType)
                {
                    Debug.LogError("show highlight");
                    //tutorial
                    GameController.Instance.TutrialHighlight(type);
                    UITutorialMainGame.Instance.SetStep(ETutorialType.Step1);
                }
            }
        }

        GameController.Instance.UpdateOrderType();

        // CoinLbl.transform.parent.gameObject.SetActive(true);
        // CoinLbl.text = _money.ToString();
    }

    public bool CheckForOrder(int type)
    {
        foreach (var order in _order)
        {
            if (type == order.Type)
            {
                return true;
            }
        }

        return false;
    }

    public void Serve(FoodItem foodItem)
    {
        foodItem.transform.DOScale(foodItem.transform.localScale * 0.5f, 0.3f);
        foodItem.transform.DOMove(ItemFrame.transform.position, 0.3f).OnComplete(() =>
        {
            Checkmark.SetActive(true);
            Checkmark.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f)
                .OnComplete(() => { Checkmark.SetActive(false); });

            foreach (var item in _obj)
            {
                Destroy(item);
            }

            _obj.Clear();

            UserData.TotalMoneyInGame += _money;
            // CoinLbl.transform.parent.gameObject.SetActive(false);


            UserData.OnNewObject.Invoke();

            GameController.Instance.ResetTimerTutorial();


            {
                DOTween.Sequence()
                    .AppendInterval(0.25f)
                    .AppendCallback(() =>
                    {
                        SellFx.Play();

                        UIController.Instance.ShowStar(Index);
                    })
                    .AppendInterval(1f)
                    .AppendCallback(() =>
                    {
                        _order.Clear();
                        _obj.Clear();

                        //ItemFrame.SetActive(false);
                        ItemFrame.GetComponent<SpriteRenderer>().DOFade(0, 0.2f);
                        ItemFrame.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f);

                        MoveOut(0.5f);
                    })
                    .AppendInterval(Random.Range(1.5f, 3f))
                    .AppendCallback(() =>
                    {
                        if (!GameController.Instance.GameEnd)
                            LoadSkin();
                    });
            }

            foreach (var item in _order)
            {
                GameController.Instance.OrderList.Remove(item);
            }

            Destroy(foodItem.gameObject);
        });
    }

    public void MoveOut(float delay = 1f)
    {
        _timer = 0;
        _isRunning = false;

        if (_particleEmotion != null) _particleEmotion.gameObject.SetActive(false);

        if (_isMainGame)
        {
            _particleEmotion = EffectManager.Instance.PlayEffect<ParticleSystem>(EEffectType.Correct1,
                new Vector3(ModelHost.position.x, ModelHost.position.y, ModelHost.position.z + 3),
                Quaternion.Euler(new Vector3(65, 0, 0)));

            _particleEmotion.gameObject.layer = LayerMask.NameToLayer("Top");
            var main = _particleEmotion.main;
            main.startSize = 1f;
        }
        else
        {
            _particleEmotion = EffectManager.Instance.PlayEffect<ParticleSystem>(EEffectType.Correct1,
                new Vector3(0, 2.2f, -4),
                Quaternion.Euler(new Vector3(0, 180, 0)));

            _particleEmotion.gameObject.layer = LayerMask.NameToLayer("Default");
            var main = _particleEmotion.main;
            main.startSize = 0.3f;
        }

        DOVirtual.DelayedCall(delay, () => { ModelHost.DOLocalMoveY(-5, 0.5f); });
    }

    private void OnDisable()
    {
        _timer = 0;
        _isRunning = false;
    }
}


public class Order
{
    public int Type, Level;
    public int Number;

    public Order(int type, int level)
    {
        Type = type;
        Level = level;
        Number = 1;
    }
}