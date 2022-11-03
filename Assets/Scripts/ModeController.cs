using UnityEngine;
using UnityEngine.UI;
public class ModeController : MonoBehaviour
{
    public Image imageModeController;
    public Sprite flag;
    public Sprite shovel;


    private ActionMode _currentActionMode = ActionMode.Shovel;
    public ActionMode currentActionMode => _currentActionMode;
    
    public void OnClick() //смена режима после нажания на панель + смена картинки
    {
        if (_currentActionMode == ActionMode.Shovel)
        {
            _currentActionMode = ActionMode.Flag;
            imageModeController.sprite = flag;
        }
        else
        {
            _currentActionMode = ActionMode.Shovel;
            imageModeController.sprite = shovel;
        }
    }

    
}
