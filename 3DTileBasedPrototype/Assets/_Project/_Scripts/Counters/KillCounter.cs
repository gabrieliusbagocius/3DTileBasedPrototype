using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillCounter : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI killCounterText = null;
    private void Awake()
    {
        GetComponentInParent<BaseHero>().OnKCChanged += HandleKCChange;
    }

    public void HandleKCChange(int killCount)
    {
        killCounterText.gameObject.SetActive(true);
        killCounterText.text = killCount.ToString();
    }

    /*
private IEnumerator ChangeToPct(float pct)
{
   float preChangePct = foregroundImage.fillAmount;
   float elapsed = 0f;

   while(elapsed < updateSpeedSeconds)
   {
       elapsed += Time.deltaTime;
       foreground
   }    
}
*/
}
