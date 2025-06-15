using UnityEngine;
using UnityEngine.UI;

public class RadialMenuView : MonoBehaviour
{
    [SerializeField] GameObject progressBar;
    [SerializeField] Image progressFill;
    [SerializeField] RadialMenu radialMenu;

    [SerializeField] float holdDuration;
    [SerializeField] float movementThreshold = 1f;

    float timer;

    float HoldPercentage => Mathf.Clamp01(timer / holdDuration);

    Vector3 startMousePos;

    bool canStartTimer = true;

    ViewManager viewManager => ViewManager.Instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        radialMenu.OnOpenMenu += () => viewManager.hudView.Hide();
        radialMenu.OnCloseMenu += () => viewManager.hudView.Show();
    }

    private void OnDestroy()
    {
        radialMenu.OnOpenMenu -= () => viewManager.hudView.Hide();
        radialMenu.OnCloseMenu -= () => viewManager.hudView.Show();
    }

    // Update is called once per frame
    void Update()
    {
        if (radialMenu.IsOpen)
            return;


        if(Input.GetMouseButtonDown(1))
        {
            if (!Utility.IsPointerOverUI())
            {
                startMousePos = Input.mousePosition;
                canStartTimer = true;
            }
        }

        if(Input.GetMouseButton(1))
        {                
            if (Vector2.Distance(Input.mousePosition, startMousePos) <= movementThreshold && canStartTimer)
            {
                timer += Time.deltaTime;
                if (timer > holdDuration)
                {
                    timer = 0;
                    radialMenu.Open();
                }
            }
            else
            {
                timer = 0;
            }
        }
        
        if(Input.GetMouseButtonUp(1))
        {
            timer = 0;
            canStartTimer = false;
        }

        UpdateProgressBar();
    }

    void UpdateProgressBar()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = 0;
        progressBar.transform.position = mousePos;
        progressBar.SetActive(HoldPercentage > 0.2f);
        progressFill.fillAmount = HoldPercentage;
    }
}
