using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CustomCursor : MonoBehaviour
{
    [SerializeField] GameObject normalCursor;
    [SerializeField] GameObject harvestCursor;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Follow Mouse
        transform.position = Input.mousePosition;

        if(Input.GetMouseButton(0) && !Utility.IsPointerOverUI())
        {
            if (normalCursor.activeInHierarchy)
                normalCursor.SetActive(false);
            if (!harvestCursor.activeInHierarchy)
                harvestCursor.SetActive(true);
        }
        else
        {
            if (!normalCursor.activeInHierarchy)
                normalCursor.SetActive(true);
            if (harvestCursor.activeInHierarchy)
                harvestCursor.SetActive(false);
        }
    }

}
