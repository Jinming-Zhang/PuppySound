using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationPanel : MonoBehaviour
{
    [SerializeField]
    Transform contentRoot;
    [SerializeField]
    NotificationText notificationTemplate;
    public void PushNotification(string text)
    {
        NotificationText noti = Instantiate(notificationTemplate);
        noti.transform.SetParent(contentRoot);
        noti.Action(text);
    }
}
