using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class GameController : MonoBehaviour
{
    private float posX;

    private float momment;

    public Transform bird;
	// Use this for initialization
	void Start ()
	{
        Advertisement.Initialize("48484");
        momment = -1.7f;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Time.time - momment > 1.7f)
	    {
	        momment = Time.time;
                posX = -10.5f;
            Instantiate(bird, new Vector3(posX, 6.4f, 0), Quaternion.identity);
        }
	}

    public void refreshScene(string sceneName)
    {
        Application.LoadLevel(sceneName);
    }

    public void showAds()
    {
        if (Advertisement.isReady())
        {
            Advertisement.Show();
        }
    }

}
