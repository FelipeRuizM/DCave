using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SettingsPopup : BasePopup
{
    [SerializeField] private TextMeshProUGUI difficultyLabel;
    [SerializeField] private Slider difficultySlider;
    [SerializeField] private OptionsPopup optionsPopup;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    override public void Open()
    {
        base.Open();
        difficultySlider.value = PlayerPrefs.GetInt("difficulty", 1);
        UpdateDifficulty(difficultySlider.value);
        gameObject.SetActive(true);
    }
    override public void Close()
    {
        base.Close();
        gameObject.SetActive(false);
    }
    //public bool IsActive()
    //{
    //    return gameObject.activeSelf;
    //}

    public void OnOKButton()
    {
        PlayerPrefs.SetInt("difficulty", (int)difficultySlider.value);
        Close();
        optionsPopup.Open();
        Debug.Log("ok clicked");
    }
    public void OnCancelButton()
    {
        Close();
        optionsPopup.Open();
        Debug.Log("cancel");
        Application.Quit();
    }

    public void UpdateDifficulty(float difficulty)
    {
        difficultyLabel.text = ((int)difficulty).ToString();
    }
    public void OnDifficultyValueChanged(float difficulty)
    {
        Debug.Log("Difficulty: " + difficulty);
        UpdateDifficulty(difficulty);
        Messenger<int>.Broadcast(GameEvent.DIFFICULTY_CHANGED, (int)difficultySlider.value);
    }
}
