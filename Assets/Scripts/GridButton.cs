using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridButton : MonoBehaviour
{
    public Button cell;
    public TextMeshProUGUI text;
    public Image imageButton;
    public Sprite bombCell;
    public Sprite flagCell;
    public Sprite empty;
    public Image buttonImage;
    private int _xCell, _yCell;
    public int xCell => _xCell;
    public int yCell => _yCell;
    private FieldController _fieldController;
    
    
    public void OnClick() //свзять кнопки и поля контроля
    {
        _fieldController.OnClick(_xCell,_yCell,this);
    }

    public void SetMode(ButtonMode mode, int minesAround = 0) //вывод на кнопку её режима
    {
        if (mode == ButtonMode.Flag)
        {
            imageButton.sprite = flagCell;
        } else if (mode == ButtonMode.Mine)
        {
            imageButton.sprite = bombCell;
        } else if (mode == ButtonMode.Empty)
        {
            imageButton.sprite = empty;
        } else
        {
            if (minesAround == 0)
            {
                text.text = "";
                buttonImage.enabled = false;
            }
            else
            {
                text.text = minesAround.ToString();
            }
            
        }
    }

    public void Construct(int x, int y, FieldController fC) //информация о кнопке, координаты и привязка к полю
    {
        _xCell = x;
        _yCell = y;
        _fieldController = fC;
    }
}
