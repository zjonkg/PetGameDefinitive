using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class Inventory3DCarousel : MonoBehaviour
{
    public Transform modelHolder;
    public Button leftArrow;
    public Button rightArrow;
    public ItemDatabase itemDatabase;

    private List<FoodData> ownedItems = new List<FoodData>();
    private GameObject currentModel;
    private int currentIndex = 0;

    void Start()
    {
        leftArrow.onClick.AddListener(() => Navigate(-1));
        rightArrow.onClick.AddListener(() => Navigate(1));
        LoadDummyInventory();
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
                            }
                        });

                    currentModel = newModel;
                });
        }
        else
        {

            currentModel = Instantiate(newItem.itemPrefab, modelHolder);
            currentModel.transform.localPosition = targetPosition;


            MakeA3DObjectDraggable draggableScript = currentModel.GetComponent<MakeA3DObjectDraggable>();
            if (draggableScript != null)
            {
                draggableScript.setInitialPosition(currentModel.transform.localPosition);  
            }
        }
    }



    void SpawnModel(FoodData item)
    {
        currentModel = Instantiate(item.itemPrefab, modelHolder);

        Vector3 localPos = new Vector3(-1.31f, -0.613f, -1.872f);
        currentModel.transform.localPosition = localPos;

        MakeA3DObjectDraggable draggableScript = currentModel.GetComponent<MakeA3DObjectDraggable>();
        if (draggableScript != null)
        {
            draggableScript.setInitialPosition(localPos); 
        }
    }



}
