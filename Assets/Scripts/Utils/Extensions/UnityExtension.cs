﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HauntedCity.Utils.Extensions
{
    public static class UnityExtension
    {
        public static void Clear(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        
        public static void ChangeVisibility(this Transform transform, bool isVisible)
        {
            if (isVisible)
            {
                transform.localScale = Vector3.one;
            }
            else
            {
                transform.localScale = Vector3.zero;
            }
        }
        
        public static List<GameObject> GetChildren(this GameObject go)
        {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform tran in go.transform)
            {
                children.Add(tran.gameObject);
            }
            return children;
        }
        

        public static void Set(this Text textObject, object value)
        {
            textObject.text = value.ToString();
        }

        public static GameObject AddChild(this Transform container, GameObject childPrefab)
        {
            var child = GameObject.Instantiate(childPrefab);
            child.transform.SetParent(container ,false);
            return child;
        }

        public static IEnumerable<T>  FindAllDescedantsOfType<T>(this Transform transform) where T : class
        {
            foreach (Transform child in transform)
            {
                foreach (var elem in child.FindAllDescedantsOfType<T>())
                {
                    yield return elem;
                }
            }
            var component = transform.GetComponent(typeof(T)) as T;
            if (component != null)
            {
                yield return component;
            }
        }
    }
}