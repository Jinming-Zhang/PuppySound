using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MazeGraphics : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer graphic;
    [SerializeField]
    Collider clder;

    [Header("Echos")]
    [SerializeField]
    List<Transform> tipLocations;
    [SerializeField]
    GameObject echosTemplate;

    float targetAlpha = 1;
    public abstract float TransitionSpeed { get; }
    private void Update()
    {
        float delta = 1 / TransitionSpeed * Time.deltaTime;
        if (graphic.color.a != targetAlpha)
        {
            Color c = graphic.color;
            c.a = targetAlpha - graphic.color.a > 0 ? c.a + delta : c.a - delta;
            c.a = Mathf.Clamp01(c.a);
            graphic.color = c;
        }
    }

    public void SetSprite(Sprite sprite)
    {
        graphic.sprite = sprite;
    }

    public void Show()
    {
        targetAlpha = 1;
    }
    public void Hide()
    {
        targetAlpha = 0;
    }
    public void SetCollider(bool activated)
    {
        clder.enabled = activated;
    }

    public void ShowTip(Color color, int dirIndex)
    {
        GameObject destroyLater = Instantiate(echosTemplate, tipLocations[dirIndex].transform);
        destroyLater.GetComponentInChildren<SpriteRenderer>().color = color;
        switch (dirIndex)
        {
            case 0:
                destroyLater.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case 1:
                destroyLater.transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
            case 2:
                destroyLater.transform.rotation = Quaternion.Euler(0, 0, -180);
                break;
            case 3:
                destroyLater.transform.rotation = Quaternion.Euler(0, 0, -270);
                break;
            default:
                break;
        }
        StartCoroutine(DestroyAfterSecCR(destroyLater.gameObject, 2));
    }

    IEnumerator DestroyAfterSecCR(GameObject target, float sec)
    {
        yield return new WaitForSeconds(sec);
        Destroy(target);
    }
}
