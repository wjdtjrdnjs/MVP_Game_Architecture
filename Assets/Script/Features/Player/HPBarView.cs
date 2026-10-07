using UnityEngine;
using UnityEngine.UI;

public class HPBarView : MonoBehaviour
{


    [SerializeField] Image currentBar;
    [SerializeField] Image ghostBar;

    float ghostDelay = 2f; //잔상 유지 시간
    float ghostSpeed = 2f; //잔상 줄어드는 속도 
    float ghostTimer = 0f; //잔상 줄어드는 속도 

    private void Start()
    {
        //
        GetComponent<RectTransform>().anchoredPosition = new Vector2(-671, 299);
    }
    public void UpdateHP(float current, float max)
    {
        currentBar.fillAmount = current / max;
        ghostTimer = ghostDelay;
    }

    private void Update()
    {
        if (ghostTimer > 0f)
        {
            ghostTimer -= Time.deltaTime;
        }
        else
        {
            ghostBar.fillAmount = Mathf.Lerp(
                ghostBar.fillAmount,
                currentBar.fillAmount,
                Time.deltaTime * ghostSpeed
            );
        }
    }
}
   
