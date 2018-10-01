using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class Quit : MonoBehaviour, IPointerClickHandler {

    public void OnPointerClick(PointerEventData eventData)
    {
        MyOwnEventTriggered();
        //throw new System.NotImplementedException();
    }


    [Serializable]
    public class MyOwnEvent : UnityEvent
    {
        public void quitGame()
        {
            Application.Quit();
        }
    }

    [SerializeField]
    private MyOwnEvent myOwnEvent = new MyOwnEvent();
    public MyOwnEvent onMyOwnEvent { get { return myOwnEvent; } set { myOwnEvent = value; } }

    public void MyOwnEventTriggered()
    {
        onMyOwnEvent.Invoke();
    }
}
