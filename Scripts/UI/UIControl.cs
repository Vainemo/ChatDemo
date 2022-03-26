using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ns
{
    ///<summary>
    ///
    ///<summary>
public class UIControl : MonoBehaviour
{
        public void Server()
        {
            transform.FindChildByName("TabClient").GetComponent<RectTransform>().localScale = Vector3.zero;
            transform.FindChildByName("TabServer").GetComponent<RectTransform>().localScale = Vector3.one;
        }
        public void Client()
        {
            transform.FindChildByName("TabServer").GetComponent<RectTransform>().localScale = Vector3.zero;
            transform.FindChildByName("TabClient").GetComponent<RectTransform>().localScale = Vector3.one;
        }
    }
}
