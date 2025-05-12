using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using System.Collections;
using TMPro;

public class Inventory3DCarousel : MonoBehaviour
{
    public Transform modelHolder;
    public Button leftArrow;
    public Button rightArrow;
    public ItemDatabase itemDatabase;

    public TextMeshProUGUI quantityText; 
    private List<FoodData> ownedItems = new List<FoodData>();
    private GameObject currentModel;
    private int currentIndex = 0;

    void Start()
    {
        leftArrow.onClick.AddListener(() => Navigate(-1));
        rightArrow.onClick.AddListener(() => Navigate(1));
        StartCoroutine(LoadUserInventory(int.Parse((PlayerPrefs.GetString("player_id")))));
    }

    IEnumerator LoadUserInventory(int userId)
    {
        string requestUrl = $"https://api-management-pet-production.up.railway.app/items/user/{userId}/items";

        yield return HttpService.Instance.SendRequest<List<UserItem>>(
            requestUrl,
            "GET",
            null,
            (userItems) =>
            {
                ownedItems.Clear();

                foreach (var userItem in userItems)
                {
                    FoodData itemData = itemDatabase.items.Find(i => i.itemId == userItem.item_id);
                    if (itemData != null)
                    {
                        FoodData instance = ScriptableObject.Instantiate(itemData);
                        instance.quantity = userItem.quantity;

                        ownedItems.Add(instance);
                    }
                }

                if (ownedItems.Count > 0)
                {
                    currentIndex = 0;
                    SpawnModel(ownedItems[currentIndex]);
                }
            },
            (error) =>
            {
                Debug.LogError("Error al cargar los ítems del usuario: " + error);
            }
        );
    }

    void LoadDummyInventory()
    {
        ownedItems = new List<FoodData>(itemDatabase.items);
        currentIndex = 0;
        SpawnModel(ownedItems[currentIndex]);
    }

    void Navigate(int direction)
    {
        if (ownedItems.Count == 0) return;

        int newIndex = (currentIndex + direction + ownedItems.Count) % ownedItems.Count;
        AnimateModelChange(ownedItems[newIndex], direction);
        currentIndex = newIndex;
    }

    void AnimateModelChange(FoodData newItem, int direction)
    {
        Vector3 targetPosition = new Vector3(-1.31f, -0.613f, -1.872f);
        Vector3 startOffset = new Vector3(direction > 0 ? 5f : -5f, targetPosition.y, targetPosition.z);

        if (currentModel != null)
        {
            GameObject oldModel = currentModel;

            MakeA3DObjectDraggable oldDraggableScript = oldModel.GetComponent<MakeA3DObjectDraggable>();
            if (oldDraggableScript != null)
            {
                oldDraggableScript.setInitialPosition(oldModel.transform.localPosition);
            }


            oldModel.transform.DOLocalMoveX(direction > 0 ? -5f : 5f, 0.3f)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {

                    Destroy(oldModel);


                    GameObject newModel = Instantiate(newItem.itemPrefab, modelHolder);
                    newModel.transform.localPosition = startOffset;


                    newModel.transform.DOLocalMove(targetPosition, 0.3f).SetEase(Ease.OutBack)
                        .OnComplete(() =>
                        {

                            MakeA3DObjectDraggable newDraggableScript = newModel.GetComponent<MakeA3DObjectDraggable>();
                            if (newDraggableScript != null)
                            {
                                newDraggableScript.setInitialPosition(newModel.transform.localPosition);
                                newDraggableScript.SetCarousel(this);
                            }
                        });

                    currentModel = newModel;

                    UpdateQuantityText(newItem.quantity);
                });
        }
        else
        {

            currentModel = Instantiate(newItem.itemPrefab, modelHolder);
            currentModel.transform.localPosition = targetPosition;

            UpdateQuantityText(newItem.quantity);
            MakeA3DObjectDraggable draggableScript = currentModel.GetComponent<MakeA3DObjectDraggable>();
            if (draggableScript != null)
            {
                draggableScript.setInitialPosition(currentModel.transform.localPosition);
                draggableScript.SetCarousel(this);
            }
        }
    }



    void SpawnModel(FoodData item)
    {
        currentModel = Instantiate(item.itemPrefab, modelHolder);

        Vector3 localPos = new Vector3(-1.31f, -0.613f, -1.872f);
        currentModel.transform.localPosition = localPos;

        UpdateQuantityText(item.quantity);

        MakeA3DObjectDraggable draggableScript = currentModel.GetComponent<MakeA3DObjectDraggable>();
        if (draggableScript != null)
        {
            draggableScript.setInitialPosition(localPos);
            draggableScript.SetCarousel(this); 
        }
    }

    void UpdateQuantityText(int quantity)
    {
        if (quantityText != null)
        {
            quantityText.text = $"{quantity}";
        }
        else
        {
            Debug.LogWarning("No se ha asignado un TextMeshProUGUI para la cantidad");
        }
    }

    public void EatCurrentItem()
    {
        if (ownedItems.Count == 0 || currentModel == null) return;

        int playerId = int.Parse(PlayerPrefs.GetString("player_id"));
        int itemId = ownedItems[currentIndex].itemId;

        StartCoroutine(EatItemRequest(playerId, itemId));
    }

    IEnumerator EatItemRequest(int playerId, int itemId)
    {
        string url = $"https://api-management-pet-production.up.railway.app/items/eat/{playerId}/{itemId}";

        yield return HttpService.Instance.SendRequest<ConsumeItemResponse>(
            url,
            "PUT",
            null,
            (response) =>
            {
                Debug.Log($"Ítem {itemId} consumido correctamente");

                ownedItems[currentIndex].quantity--;
                if (ownedItems[currentIndex].quantity <= 0)
                {
                    ownedItems.RemoveAt(currentIndex);
                    if (ownedItems.Count > 0)
                    {
                        currentIndex = currentIndex % ownedItems.Count;
                        AnimateModelChange(ownedItems[currentIndex], 1);
                    }
                    else
                    {
                        Destroy(currentModel);
                        quantityText.text = "0";
                    }
                }
                else
                {
                    UpdateQuantityText(ownedItems[currentIndex].quantity);
                }
            },
            (error) =>
            {
                Debug.LogError("Error al consumir el ítem: " + error);
            }
        );
    }




}
