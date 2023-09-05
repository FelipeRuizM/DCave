using UnityEngine;

public class StartPopup : BasePopup
{
    public void OnExitGameButton2()
    {
        Debug.Log("exit game");
        Application.Quit();
    }

    public void OnPlayButton()
    {
        Debug.Log("play game");
        Close();
    }
}
