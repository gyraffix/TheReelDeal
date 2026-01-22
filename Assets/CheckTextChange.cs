using TMPro;
using UnityEngine;

 public class CheckTextChange : MonoBehaviour
 {
    private string lastText;
    private string currentText;


    private void Update()
    {
        currentText = gameObject.GetComponent<TMP_Text>().text;

        if  (!lastText.Equals(currentText))
        {
            Debug.Log("Play Noise");
        }

        lastText = currentText;

    }

}

