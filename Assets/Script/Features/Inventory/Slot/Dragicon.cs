using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Dragicon : MonoBehaviour
{
    public Image iconImage;

    void Awake()
    {
        iconImage.gameObject.SetActive(false);
    }

    public void Show(Sprite icon)
    {
        iconImage.sprite = icon;
        iconImage.gameObject.SetActive(true);
    }
    public void Hide()
    {
        iconImage.gameObject.SetActive(false);
    }

    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
