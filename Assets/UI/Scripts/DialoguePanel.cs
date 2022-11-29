using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePanel : MonoBehaviour
{
    [SerializeField]
    List<Sprite> sprites;
    private int spriteIndex = 0;

    private bool inProcess = false;

    // Start is called before the first frame update

    [SerializeField]
    private Image iconImage;

    [SerializeField]
    List<string> playerTexts = new List<string>() { "Where are you?", "[dogName]!", "Can you hear me, [dogName]?", "Come here, [dogName]" };

    [SerializeField]
    List<string> dogTexts = new List<string>() { "Woof woof!", "Bow-wow!", "Ruff arff!", "Bark!" };

    [SerializeField]
    string monsterText = "(Deep roaring from afar.)";

    [SerializeField]
    private TMPro.TextMeshProUGUI dialogueText;

    void Start()
    {
        gameObject.SetActive(false);   
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf && Input.GetKeyDown("space"))
        {
            spriteIndex = (spriteIndex + 1) % 3;
            setIcon();
            setText();

            if (spriteIndex == 0)
            {
                gameObject.SetActive(false);
                GameController.Instance.Calling();
                this.inProcess = false;
            }
        }
    }

    public void callingEvent()
    {
        if (!this.inProcess)
        {
            setText();
            setIcon();
            this.inProcess = true;
        }
    }

    private void setText()
    {
        if (spriteIndex == 0)
        {
            this.dialogueText.text = playerTexts[Random.Range(0, playerTexts.Count)];
        } else if (spriteIndex == 1)
        {
            this.dialogueText.text = dogTexts[Random.Range(0, dogTexts.Count)];
        } else if (spriteIndex == 2)
        {
            this.dialogueText.text = monsterText;
        }
    }
 
    private void setIcon()
    {
        this.iconImage.sprite = sprites[spriteIndex];
    }
    
}
