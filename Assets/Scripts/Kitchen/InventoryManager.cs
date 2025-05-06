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

        // Si ya tenemos un modelo actual
        if (currentModel != null)
        {
            GameObject oldModel = currentModel;

            // Guardamos la posici�n inicial local antes de la animaci�n
            MakeA3DObjectDraggable oldDraggableScript = oldModel.GetComponent<MakeA3DObjectDraggable>();
            if (oldDraggableScript != null)
            {
                oldDraggableScript.setInitialPosition(oldModel.transform.localPosition);  // Guardamos la posici�n local
            }

            // Animamos el viejo modelo fuera del �rea
            oldModel.transform.DOLocalMoveX(direction > 0 ? -5f : 5f, 0.3f)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    // Destruimos el viejo modelo
                    Destroy(oldModel);

                    // Instanciamos el nuevo modelo en la posici�n inicial
                    GameObject newModel = Instantiate(newItem.itemPrefab, modelHolder);
                    newModel.transform.localPosition = startOffset;

                    // Animamos el nuevo modelo hacia la posici�n de destino
                    newModel.transform.DOLocalMove(targetPosition, 0.3f).SetEase(Ease.OutBack)
                        .OnComplete(() =>
                        {
                            // Guardamos la posici�n inicial local del nuevo modelo
                            MakeA3DObjectDraggable newDraggableScript = newModel.GetComponent<MakeA3DObjectDraggable>();
                            if (newDraggableScript != null)
                            {
                                newDraggableScript.setInitialPosition(newModel.transform.localPosition);  // Guardamos la posici�n inicial local correctamente
                            }
                        });

                    currentModel = newModel;
                });
        }
        else
        {
            // Si no hay modelo actual, simplemente instanciamos el nuevo
            currentModel = Instantiate(newItem.itemPrefab, modelHolder);
            currentModel.transform.localPosition = targetPosition;

            // Guardamos la posici�n inicial local al crear el modelo
            MakeA3DObjectDraggable draggableScript = currentModel.GetComponent<MakeA3DObjectDraggable>();
            if (draggableScript != null)
            {
                draggableScript.setInitialPosition(currentModel.transform.localPosition);  // Aseguramos que se guarde la posici�n local
            }
        }
    }



    void SpawnModel(FoodData item)
    {
        currentModel = Instantiate(item.itemPrefab, modelHolder);

        // Posici�n inicial que no est� afectada por la animaci�n
        currentModel.transform.localPosition = new Vector3(-1.31f, -0.613f, -1.872f);  // Posici�n inicial predeterminada

        // Aseguramos que se guarde correctamente la posici�n inicial local
        MakeA3DObjectDraggable draggableScript = currentModel.GetComponent<MakeA3DObjectDraggable>();
        if (draggableScript != null)
        {
            draggableScript.setInitialPosition(currentModel.transform.localPosition);  // Usamos localPosition para asegurar que se guarde en el espacio local
        }
    }


}
