using System;
using System.Collections;
using UnityEngine;

public class InternetCheck : MonoBehaviour
{
    private event Action NoInternet;
    [SerializeField] float checkPeriod;
    [SerializeField] GameObject noInternetPanel;

    void Start() => StartCoroutine(ConnectingCheck());
    void Update() => NoInternet?.Invoke();
    IEnumerator ConnectingCheck()
    {
        WaitForSeconds wfs = new WaitForSeconds(checkPeriod);

        while (true)
        {
            NoInternet = null;
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                NoInternet += NotReachableInternet;
                Debug.Log("Error. Check internet connection!");
            }

            yield return wfs;
        }
    }
    private void NotReachableInternet()
    {
        Time.timeScale = 0f;
        noInternetPanel.SetActive(true);
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            ReachableInternet();
            NoInternet = null;
        }

    }
    private void ReachableInternet()
    {
        Time.timeScale = 1.0f;
        noInternetPanel.SetActive(false);
    }
}
