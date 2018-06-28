using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RemoveGridObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    MainSceneManager manager;
    public bool removable;

    void Start() {
        manager = FindObjectOfType<MainSceneManager>();
        removable = false;
    }

    void Update() {

      /*  if (Input.GetMouseButtonUp(0) && removable) {
            //removable = false;
            // manager.isRemovableObj = true;
            manager.objectToMove.GetComponent<SpriteRenderer>().color = new Vector4(1f, 0.2f, 0.2f, 0.2f);
            manager.RemoveObject();

        } *///Llamada a la funcion eliminar objeto

        ManageRemovalColors(); //Cambios en color papelera y objeto eliminable.
    }


    void ManageRemovalColors() {
        if (manager.objectToMove)
        {
            if (removable)
            {
                GetComponent<Image>().color = Color.red;
                manager.objectToMove.GetComponent<SpriteRenderer>().color = new Vector4(1f, 0.2f, 0.2f, 0.8f);
            }
            else
            {
                GetComponent<Image>().color = Color.white;
                manager.objectToMove.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }        
    }

   

    public void OnPointerEnter(PointerEventData eventData)
    {
        removable = true;
        //manager.isRemovableObj = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        removable = false;
       // manager.isRemovableObj = false;
    }
}
