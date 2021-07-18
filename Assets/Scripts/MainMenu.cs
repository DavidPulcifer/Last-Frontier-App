using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OpenPassport()
    {
        SceneManager.LoadScene("Passport");
    }

    public void OpenBanking()
    {
        SceneManager.LoadScene("Banking");
    }

    public void OpenMap()
    {
        SceneManager.LoadScene("Map");
    }

    public void OpenSkills()
    {
        SceneManager.LoadScene("Skills");
    }

    public void OpenSponsors()
    {
        SceneManager.LoadScene("Sponsors");
    }

    public void OpenShopping()
    {
        SceneManager.LoadScene("Shopping");
    }

    public void OpenAllegiance()
    {
        SceneManager.LoadScene("Allegiance");
    }
}
