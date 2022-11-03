using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FieldController : MonoBehaviour
{
    [Header("Some Header")] [Tooltip("Это грид")]
    public GridLayoutGroup gridLayoutGroup;

    public ModeController modeController;
    public GridButton buttonPref;
    public int size;
    [Space] public TextMeshProUGUI textBomb;
    public TextMeshProUGUI textFlag;
    public List<GridButton> button;
    private bool[,] _bombs;
    private bool[,] _opened;
    private bool[,] _flagged;
    public int quantityBombs;
    public int quantityFlag;


    public void Build() //создание поля
    {
        //заполнение поля кнопками
        gridLayoutGroup.cellSize = Vector2.one * 230 / size;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                GridButton createButton = Instantiate(buttonPref, transform);
                createButton.Construct(i, j, this);
                button.Add(createButton);
            }
        }

        _flagged = new bool[size, size];
        _opened = new bool[size, size];
        quantityFlag = 0;
        textFlag.text = quantityFlag.ToString();
        //растановка бомб в зависимости от размера
        _bombs = new bool[size, size];
        quantityBombs = Mathf.RoundToInt(size * 0.75f);
        textBomb.text = quantityBombs.ToString(); //вывод кол-ва бомб на меню над полем
        while (quantityBombs > 0)
        {
            int x = Random.Range(0, size);
            int y = Random.Range(0, size);
            if (!_bombs[x, y])
            {
                _bombs[x, y] = true;
                quantityBombs--;
            }
        }

    }

    public void OnClick(int x, int y, GridButton button) //проверка поля при нажатии
    {
        Debug.unityLogger.Log("FieldController", $"OnClick in fieldController {x}, {y}");
        if (modeController.currentActionMode == ActionMode.Shovel)
        {
            if (_flagged[x, y])
                return;
            
            if (_opened[x,y])
            {
                CheckButton(x,y,button);
            } else
            {
                SimpleClick(x, y, button);
            }
        }
        else
        {
            if (_opened[x, y])
            {
                return;
            }
            else if (_flagged[x,y])
            {
                button.SetMode(ButtonMode.Empty);
                _flagged[x, y] = false;
                quantityFlag--;
                textFlag.text = quantityFlag.ToString();
            } else
            {
                button.SetMode(ButtonMode.Flag);
                _flagged[x, y] = true;
                quantityFlag++;
                textFlag.text = quantityFlag.ToString();
            }
        }
    }

    public void SimpleClick(int x, int y, GridButton button)
    {
        if (_bombs[x, y])
        {
            button.SetMode(ButtonMode.Mine);
        }
        else //если поле не бомба (подсчёт бомб вокруг)
        {
            int quantityAround = SearchBomb(x, y);
            if (quantityAround == 0)
            {
                button.SetMode(ButtonMode.Number, quantityAround);
                _opened[x, y] = true;
                ZeroButton(x,y);
            }
            else
            {
                button.SetMode(ButtonMode.Number, quantityAround);
                _opened[x, y] = true;
            }
        }
    }

    public int SearchBomb(int x, int y)
    {
        int quantityAround = 0;
        for (int i = x - 1; i < x + 2; i++)
        {
            for (int j = y - 1; j < y + 2; j++)
            {
                if (i >= 0 && i < size && j >= 0 && j < size)
                {
                    if (_bombs[i, j])
                    {
                        quantityAround++;
                    }
                }
            }
        }

        return quantityAround;
    }

    public void ZeroButton(int x, int y)
    {
        
        for (int i = x - 1; i < x + 2; i++)
        {
            for (int j = y-1; j < y+2; j++)
            {
                if (i >= 0 && i < size && j >= 0 && j < size)
                {
                    if (!_opened[i,j])
                    {
                        int qBombs = SearchBomb(i, j);
                        if (qBombs == 0)
                        {
                            button[size * i + j].SetMode(ButtonMode.Number, qBombs);
                            _opened[i, j] = true;
                            ZeroButton(i,j);
                        }
                        else
                        {
                            button[size * i + j].SetMode(ButtonMode.Number, qBombs);
                            _opened[i, j] = true;
                        } 
                    }
                }
            }
        }
    }

    public void CheckButton(int x, int y, GridButton button)
    {
        int flagAround = 0;
        for (int i = x - 1; i < x + 2; i++)
        {
            for (int j = y - 1; j < y + 2; j++)
            {
                if (i >= 0 && i < size && j >= 0 && j < size)
                {
                    if (_flagged[i, j])
                    {
                        flagAround++;
                    }
                }
            }
        }
        int quantityAround = 0;
        for (int i = x - 1; i < x + 2; i++)
        {
            for (int j = y - 1; j < y + 2; j++)
            {
                if (i >= 0 && i < size && j >= 0 && j < size)
                {
                    if (_bombs[i, j])
                    {
                        quantityAround++;
                    }
                }
            }
        }

        if (flagAround != quantityAround)
            return;
        
        for (int i = x - 1; i < x + 2; i++)
        for (int j = y-1; j < y+2; j++)
        {
            if (i >= 0 && i < size && j >= 0 && j < size)
            {
                if (!_opened[i,j] && !_flagged[i,j])
                {
                    SimpleClick(i, j, this.button[size * i + j]);
                }
            }
        }
        
        
    }

    public void Start()
    {
        Build();
    }
}
