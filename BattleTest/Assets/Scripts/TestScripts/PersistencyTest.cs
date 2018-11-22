using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PersistencyTest : MonoBehaviour
{
    public string stringToDisplay;
    public Text displayText;
    public Text inputText;

	// Use this for initialization
	void Start ()
	{
	    var loadString = GlobalInfo.Instance.testString;
	    if (loadString != "") stringToDisplay = loadString;
	    else loadString = "No string found!";
	    displayText.text = loadString;
	}

    public void SaveTemp()
    {
        GlobalInfo.Instance.testString = inputText.text;
    }

    public void LoadTemp()
    {
        displayText.text = GlobalInfo.Instance.testString;
    }


    //Scene Loading
    public void LoadScene1()
    {
        SceneManager.LoadScene("DataPersistencyTest_1");
    }
    public void LoadScene2()
    {
        SceneManager.LoadScene("DataPersistencyTest_2");
    }
    public void LoadScene3()
    {
        SceneManager.LoadScene("DataPersistencyTest_3");
    }
    public void LoadScene4()
    {
        SceneManager.LoadScene("DataPersistencyTest_4");
    }
}
