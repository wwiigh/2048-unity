using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Block : MonoBehaviour
{
    public int value;
    public Color color;
    public Color default_color;
    public bool free;
    public bool ismerge = false;
    public List<Color> colors;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void change_color(int value)
    {
        switch (value)
        {
            case 2:
                color = colors[0];
                break;
            case 4:
                color = colors[1];
                break;
            case 8:
                color = colors[2];
                break;
            case 16:
                color = colors[3];
                break;
            case 32:
                color = colors[4];
                break;
            case 64:
                color = colors[5];
                break;
            case 128:
                color = colors[6];
                break;
            case 256:
                color = colors[7];
                break;
            case 512:
                color = colors[8];
                break;
            case 1024:
                color = colors[9];
                break;
            case 2048:
                color = colors[10];
                break;
            default:
                break;
        }
        this.GetComponent<SpriteRenderer>().color = color;
        this.GetComponentInChildren<TextMeshPro>().text = value.ToString();
        this.free = false;
    }
    public void set_default()
    {
        this.GetComponent<SpriteRenderer>().color = default_color;
        this.GetComponentInChildren<TextMeshPro>().text = "";
        free = true;
        value = 0;
    }
}
