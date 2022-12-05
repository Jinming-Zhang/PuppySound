using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationPanel : MonoBehaviour
{
    [SerializeField]
    Transform contentRoot;
    [SerializeField]
    NotificationText notificationTemplate;
    public void PushNotification(NotificationInfo info)
    {
        NotificationText noti = Instantiate(notificationTemplate);
        noti.transform.SetParent(contentRoot);
        noti.Action(info.text);
    }
}
public class NotificationInfo
{
    public string text;
    public AudioClip sfx;
    public NotificationInfo(string text = "", AudioClip sfx = null)
    {
        this.text = text;
        this.sfx = sfx;
    }
}
