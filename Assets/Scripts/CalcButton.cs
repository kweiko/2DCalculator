using UnityEngine;
using UnityEngine.UI;

public class CalcButton : MonoBehaviour
{
    public Text label;

    /* Get the cache copy of the text's rect transform if set,
    otherwise fetch it, cache it, and then return it. */
    public RectTransform rectTransform{
        get{
            if(_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();
            return _rectTransform;
        }
    }
    RectTransform _rectTransform;

    /* Access the Manager script. Use static variable to cache the reference
    as it's a same value for all buttons */
    public Manager calcManager{
        get{
            if(_calcManager == null)
                _calcManager = GetComponentInParent<Manager>();
            return _calcManager;
        }
    }
    static Manager _calcManager;
    
    // Start is called before the first frame update 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onTapped(){
        Debug.Log("Tapped: " + label.text);
        calcManager.buttonTapped(label.text[0]);
    }
}
