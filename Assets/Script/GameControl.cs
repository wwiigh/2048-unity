using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameControl : MonoBehaviour
{
    public GameObject board;
    public int win_cond = 2048;
    public int width = 4;
    public int height = 4;
    private GameState state;
    private Block[] blocks;
    public GameObject win;
    public GameObject lose;
    public GameObject leave_button;
    public GameObject tryagain_button;
    
    // Start is called before the first frame update
    void Start()
    {
        gamestate_change(GameState.init);
    }

    // Update is called once per frame
    void Update()
    {
        if(state!=GameState.wait_input)return;
        if(Input.GetKeyDown(KeyCode.UpArrow))move(Vector2.up);
        if(Input.GetKeyDown(KeyCode.DownArrow))move(Vector2.down);
        if(Input.GetKeyDown(KeyCode.LeftArrow))move(Vector2.left);
        if(Input.GetKeyDown(KeyCode.RightArrow))move(Vector2.right);
        bool islose = true;
        for(int i=0;i<width;i++)
        {
            for(int j=0;j<height;j++)
            {
                int index = width*i + j;
                int value = blocks[index].value;
                if(value==0)
                {
                    islose = false;
                    break;
                }
                if(index + width <height * width && value == blocks[index + width].value)
                {
                    islose = false;
                    break;
                }
                if(index - width >=0 && value == blocks[index - width].value)
                {
                    islose = false;
                    break;
                }
                if(index%4 != 3 && value == blocks[index + 1].value)
                {
                    islose = false;
                    break;
                }
                if(index%4 != 0 && value == blocks[index - 1].value)
                {
                    islose = false;
                    break;
                }
            }
        }
        if(islose==true)gamestate_change(GameState.lose);
    }
    private void init()
    {
        blocks = new Block[width * height];
        for(int i=0;i<width;i++)
        {
            for(int j=0;j<height;j++)
            {
                var new_block = Instantiate(board,new Vector2(i,j),Quaternion.identity);
                blocks[i*width + j] = new_block.GetComponentInChildren<Block>();
            }
        }
        spawn_block(2);
        gamestate_change(GameState.wait_input);
    }
    private void spawn_block(int count)
    {
        Block[] free_block;
        int[] random_array;
        int index = 0;
        free_block = new Block[width * height];
        random_array = new int[width * height];
        for(int i=0;i<width * height;i++)
        {
            random_array[i] = Random.Range(0,100);
            //Debug.Log(random_array[i]);
            if(blocks[i].free==true) free_block[index++] = blocks[i];
        }
        //Debug.Log(index);
        
        for(int i=0;i<index;i++)
        {
            for(int j=0;j<index-1 - i;j++)
            {
                if(random_array[j]>random_array[j+1])
                {
                    int tmp;
                    tmp = random_array[j];
                    random_array[j] = random_array[j+1];
                    random_array[j+1] = tmp;
                    Block tmp_block;
                    tmp_block = free_block[j];
                    free_block[j] = free_block[j+1];
                    free_block[j+1] = tmp_block;
                }
            }
        }


        for(int i=0;i<count;i++)
        {
            free_block[i].value = Random.Range(0.0f,1.0f) > 0.7f ? 4 : 2;
            free_block[i].change_color(free_block[i].value);
        }
    }
    private void move(Vector2 shift)
    {
        gamestate_change(GameState.move);
        
        for(int i=0;i<width * height;i++)blocks[i].ismerge = false;
        bool ismove = false;
        if(shift == Vector2.up)ismove = shift_up();
        else if(shift == Vector2.down)ismove = shift_down();
        else if(shift == Vector2.right)ismove = shift_right();
        else if(shift == Vector2.left)ismove = shift_left();
        
        if(ismove==true)
        spawn_block(1);
        
        if(state==GameState.move)
        gamestate_change(GameState.wait_input);
    }
    private bool shift_left()
    {
        bool ismove = false;
        for(int i=0;i<width * height;i++)
        {
            if(i - width <0 || blocks[i].value == 0)continue;
            int next = i - width;
            int now = i;
            do
            {
                if(blocks[next].free == true)
                {
                    blocks[next].value = blocks[now].value;
                    blocks[next].change_color(blocks[now].value);
                    blocks[now].set_default();
                    now = next;
                    next = next - width;
                    ismove = true;
                    if(next <0 )next = now;
                }
                else if(blocks[next].ismerge == false && blocks[next].free == false && blocks[next].value == blocks[now].value)
                {
                    blocks[next].value = blocks[now].value * 2;
                    blocks[next].change_color(blocks[now].value*2);
                    blocks[now].set_default();
                    if(blocks[next].value==win_cond)
                    {
                        gamestate_change(GameState.win);
                    }
                    blocks[next].ismerge = true;
                    ismove = true;
                    next = now;
                }
                else if(blocks[next].ismerge == true ||blocks[next].free == false ) next = now;
                
            } while (next != now);
        }
        return ismove;
    }
    private bool shift_right()
    {
        bool ismove = false;
        for(int i=width * height - 1;i>=0;i--)
        {
            if(i + width >= height*width || blocks[i].value == 0)continue;
            int next = i + width;
            int now = i;
            do
            {
                if(blocks[next].free == true)
                {
                    blocks[next].value = blocks[now].value;
                    blocks[next].change_color(blocks[now].value);
                    blocks[now].set_default();
                    now = next;
                    next = next + width;
                    ismove = true;
                    if(next >= height*width)next = now;
                }
                else if(blocks[next].ismerge == false &&blocks[next].free == false && blocks[next].value == blocks[now].value)
                {
                    blocks[next].value = blocks[now].value * 2;
                    if(blocks[next].value==win_cond)
                    {
                        gamestate_change(GameState.win);
                    }
                    blocks[next].change_color(blocks[now].value*2);
                    blocks[now].set_default();
                    blocks[next].ismerge = true;
                    ismove = true;
                    next = now;
                }
                else if(blocks[next].ismerge == true ||blocks[next].free == false) next = now;
            } while (next != now);
        }
        return ismove;
    }
    private bool shift_up()
    {
        bool ismove = false;
        for(int i=height-1;i>=0;i--)
        {
            for(int j=0;j<width;j++)
            {
                int index = j * width + i;
                if((index +1)%4==0  || blocks[index].value == 0)continue;
                int next = index + 1;
                int now = index;
                do
                {
                    if(blocks[next].free == true)
                    {
                        blocks[next].value = blocks[now].value;
                        blocks[next].change_color(blocks[now].value);
                        blocks[now].set_default();
                        now = next;
                        next = next + 1;
                        ismove = true;
                        if(next %4 ==0 )next = now;
                    }
                    else if(blocks[next].ismerge == false &&blocks[next].free == false && blocks[next].value == blocks[now].value)
                    {
                        blocks[next].value = blocks[now].value * 2;
                        if(blocks[next].value==win_cond)
                        {
                            gamestate_change(GameState.win);
                        }
                        blocks[next].change_color(blocks[now].value*2);
                        blocks[now].set_default();
                        blocks[next].ismerge = true;
                        next = now;
                        ismove = true;
                    }
                    else if(blocks[next].ismerge == true ||blocks[next].free == false) next = now;

                } while (next != now);
            }
        }
        return ismove;
    }
    private bool shift_down()
    {
        bool ismove = false;
        for(int i=0;i<height;i++)
        {
            for(int j=0;j<width;j++)
            {
                int index = j * width + i;
                if((index)%4==0  || blocks[index].value == 0)continue;
                int next = index - 1;
                int now = index;
                do
                {
                    if(blocks[next].free == true)
                    {
                        blocks[next].value = blocks[now].value;
                        blocks[next].change_color(blocks[now].value);
                        blocks[now].set_default();
                        now = next;
                        next = next - 1;
                        ismove = true;
                        if(now %4 ==0 )next = now;
                    }
                    else if(blocks[next].ismerge == false&&blocks[next].free == false && blocks[next].value == blocks[now].value)
                    {
                        blocks[next].value = blocks[now].value * 2;
                        blocks[next].change_color(blocks[now].value*2);
                        if(blocks[next].value==win_cond)
                        {
                            gamestate_change(GameState.win);
                        }
                        blocks[now].set_default();
                        blocks[next].ismerge = true;
                        ismove = true;
                        next = now;
                    }
                    else if(blocks[next].ismerge == true ||blocks[next].free == false) next = now;
                } while (next != now);
            }
        }
        return ismove;
    }
    private void gamestate_change(GameState _state)
    {
        state = _state;
        switch (_state)
        {
            case GameState.init:
                init();
                break;
            case GameState.spawn_block:
                break;
            case GameState.wait_input:
                break;
            case GameState.move:
                break;
            case GameState.win:
                win.SetActive(true);
                tryagain_button.SetActive(true);
                leave_button.SetActive(true);
                break;
            case GameState.lose:
                lose.SetActive(true);
                tryagain_button.SetActive(true);
                leave_button.SetActive(true);
                break;
            default:
                break;
        }
    }
}
enum GameState
{
    init,
    spawn_block,
    wait_input,
    move,
    win,
    lose

}