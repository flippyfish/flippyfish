﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMove : MonoBehaviour
{
	// https://answers.unity.com/questions/773911/move-object-to-mouse-click-position.html
     Vector3 newPosition;
     void Start () {
         newPosition = transform.position;
     }
     void Update()
     {
         if (Input.GetMouseButtonDown(0))
         {
             RaycastHit hit;
             Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
             if (Physics.Raycast(ray, out hit))
             {
                 newPosition = hit.point;
                 transform.position = newPosition;
             }
         }
     }
 }