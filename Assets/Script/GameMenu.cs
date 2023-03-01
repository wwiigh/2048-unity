using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public string start_name;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void start_button()
    {
        SceneManager.LoadScene(start_name);
    }
    public void end_button()
    {
        Application.Quit();
    }

    public void try_button()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
