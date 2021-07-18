using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HackingPuzzleManager : MonoBehaviour
{    
    [SerializeField] HackingPuzzle[] puzzleArray;
    [TextArea] [SerializeField] string successInstructions = "1234";
    [SerializeField] SoundArray soundPlayer;

    [Header("Tuning")]
    [SerializeField] int maxEnergy = 10;
    [SerializeField] int baseWinCost = 100;
    [SerializeField] int winCostIncreasePerLevel = 10;
    [SerializeField] int scanCost = 2;
    [Space]
    [SerializeField] int bruteForceCost = 1;
    [SerializeField] int bruteForcePower = 5;
    [Space]
    [SerializeField] int keylogCost = 3;
    [SerializeField] int keylogPower = 30;
    [Space]
    [SerializeField] int sqlInjectCost = 5;
    [SerializeField] int sqlInjectPower = 50;
    [Space]
    [SerializeField] int rootkitCost = 9;
    [SerializeField] int rootkitPower = 90;
    [Space]
    [SerializeField] float weakAgainstModifier = 2f;
    [SerializeField] float strongAgainstModifier = 0.5f;
    [Space]
    [SerializeField] float typingDelay = 0.05f;
    [SerializeField] float appearDelay = 0.1f;

    [Header("Text Fields")]    
    [SerializeField] TextMeshProUGUI deviceNameText;
    [SerializeField] TextMeshProUGUI difficultyText;
    [SerializeField] TextMeshProUGUI typeText;
    [SerializeField] TextMeshProUGUI weaknessText;
    [SerializeField] TextMeshProUGUI strongAgainstText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI energyText;

    [Header("Progress Bar")]
    [SerializeField] Slider progressBar;

    [Header("Buttons")]
    [SerializeField] GameObject scanButton;
    [SerializeField] GameObject bruteForceButton;
    [SerializeField] GameObject keyloggerButton;
    [SerializeField] GameObject sqlInjectionButton;
    [SerializeField] GameObject installRootkiButton;

    [Header("Hacking Canvas")]
    [SerializeField] GameObject hackingCanvas;

    [Header("Win Canvas")]
    [SerializeField] GameObject winCanvas;
    [SerializeField] TextMeshProUGUI successInstructionsText;

    [Header("No Energy Canvas")]
    [SerializeField] GameObject noEnergyCanvas;    

    HackingPuzzle currentPuzzle;
    int currentEnergy = 10;
    TextMeshProUGUI scanButtonText;
    TextMeshProUGUI bruteForceButtonText;
    TextMeshProUGUI keyloggerButtonText;
    TextMeshProUGUI sqlInjectButtonText;
    TextMeshProUGUI rootkitButtonText;
    bool isScanned = false;

    private void Awake()
    {
        currentPuzzle = ChooseRandomPuzzle();
        scanButtonText = scanButton.GetComponentInChildren<TextMeshProUGUI>();
        bruteForceButtonText = bruteForceButton.GetComponentInChildren<TextMeshProUGUI>();
        keyloggerButtonText = keyloggerButton.GetComponentInChildren<TextMeshProUGUI>();
        sqlInjectButtonText = sqlInjectionButton.GetComponentInChildren<TextMeshProUGUI>();
        rootkitButtonText = installRootkiButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        InitializeTerminal();
    }

    public void ScanPuzzle()
    {
        soundPlayer.PlayRandomSound(soundPlayer.typingClips, 0.5f);
        if (isScanned) return;
        if(!SpendEnergy(scanCost)) return;
        isScanned = true;
        UpdateButtons();
        RevealDetails();        
    }

    public void BruteForce()
    {
        if (!SpendEnergy(bruteForceCost)) { return; }
        AttackLock(HackingMethod.Brute_Force, bruteForcePower);
    }

    public void Keylogger()
    {
        if (!SpendEnergy(keylogCost)) { return; }
        AttackLock(HackingMethod.Keylogger, keylogPower);
    }

    public void SQLInject()
    {
        if (!SpendEnergy(sqlInjectCost)) { return; }
        AttackLock(HackingMethod.SQL_Injection, sqlInjectPower);
    }

    public void InstallRootKit()
    {
        if (!SpendEnergy(rootkitCost)) { return; }
        AttackLock(HackingMethod.Install_Rootkit, rootkitPower);
    }

    public void LogOut()
    {
        SceneManager.LoadScene("Skills");
    }

    public void ContinueHacking()
    {
        currentEnergy = maxEnergy;
        noEnergyCanvas.SetActive(false);
        UpdateEnergy();
        UpdateButtons();
    }

    void AttackLock(HackingMethod attackType, int power)
    {
        int damage = power;
        damage *= (int)Mathf.Max(1, (IsWeakAgainst(currentPuzzle, attackType) * weakAgainstModifier));
        damage *= (int)Mathf.Max(1, (IsStrongAgainst(currentPuzzle, attackType) * strongAgainstModifier));

        progressBar.value = Mathf.Clamp((progressBar.value + damage), 0, progressBar.maxValue);
        soundPlayer.PlayRandomSound(soundPlayer.compSFX, 0.4f);
        WinTest();
    }    

    void WinTest()
    {        
        if (!Mathf.Approximately(progressBar.value, progressBar.maxValue)) { return; }

        soundPlayer.PlaySingleSound(soundPlayer.powerDown, 0.5f);
        winCanvas.SetActive(true);
        successInstructionsText.text = successInstructions;
    }
    
    int IsWeakAgainst(HackingPuzzle puzzle, HackingMethod attackType)
    {
        if(puzzle.weakAgainst.Length == 0) { return 0; }
        foreach (var weakness in puzzle.weakAgainst)
        {
            if(weakness == attackType) { return 1; }
        }
        return 0;
    }

    int IsStrongAgainst(HackingPuzzle puzzle, HackingMethod attackType)
    {
        if (puzzle.strongAgainst.Length == 0) { return 0; }
        foreach (var strength in puzzle.strongAgainst)
        {
            if (strength == attackType) { return 1; }
        }
        return 0;
    }

    bool SpendEnergy(int amount)
    {
        if(currentEnergy<amount) { return false; }

        currentEnergy -= amount;
        UpdateEnergy();
        UpdateButtons();
        NoEnergyTest();
        return true;
    }

    void NoEnergyTest()
    {
        if (!(currentEnergy < bruteForceCost)) return;        

        noEnergyCanvas.SetActive(true);        
    }

    void UpdateButtons()
    {
        scanButton.GetComponent<Button>().interactable = (currentEnergy >= scanCost && !isScanned);
        bruteForceButton.GetComponent<Button>().interactable = (currentEnergy >= bruteForceCost);
        keyloggerButton.GetComponent<Button>().interactable = (currentEnergy >= keylogCost);
        sqlInjectionButton.GetComponent<Button>().interactable = (currentEnergy >= sqlInjectCost);
        installRootkiButton.GetComponent<Button>().interactable = (currentEnergy >= rootkitCost);

    }

    void UpdateEnergy()
    {
        energyText.text = $"Energy {currentEnergy}/{maxEnergy}";
    }

    HackingPuzzle ChooseRandomPuzzle()
    {
        if (puzzleArray == null) return null;

        return puzzleArray[Random.Range(0, puzzleArray.Length)];
    }

    IEnumerator TypeText(TextMeshProUGUI textMesh, string text)
    {
        textMesh.text = "";
        for (int i = 0; i < text.Length; i++)
        {
            textMesh.text += text[i];
            yield return new WaitForSeconds(typingDelay);
        }
    }    

    IEnumerator AppearCanvas(GameObject canvas)
    {
        canvas.SetActive(true);
        yield return new WaitForSeconds(Random.Range(0.8f * appearDelay, 1.2f * appearDelay));
        foreach (Transform child in canvas.transform)
        {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(Random.Range(0.8f * appearDelay, 1.2f * appearDelay));
        }
    }

    void InitializeTerminal()
    {
        progressBar.maxValue = baseWinCost + ((currentPuzzle.difficulty - 1) * winCostIncreasePerLevel);
        currentEnergy = maxEnergy;
        StartCoroutine(TypeText(deviceNameText, currentPuzzle.deviceName));
        //deviceNameText.text = currentPuzzle.deviceName;

        StartCoroutine(TypeText(descriptionText, currentPuzzle.description));
        //descriptionText.text = currentPuzzle.description;
        UpdateEnergy();

        StartCoroutine(AppearCanvas(hackingCanvas));
        StartCoroutine(TypeText(difficultyText, "Difficulty: ??"));
        //difficultyText.text = $"Difficulty: ??";

        StartCoroutine(TypeText(typeText, "Type: ???"));
        //typeText.text = $"Type: ???";

        StartCoroutine(TypeText(weaknessText, "Weakness: ???"));
        //weaknessText.text = $"Weakness: ???";

        StartCoroutine(TypeText(strongAgainstText, "Strong Against: ???"));
        //strongAgainstText.text = $"Strong Against: ???";

        progressBar.value = 0;

        scanButtonText.text = $"Scan - {scanCost}";
        bruteForceButtonText.text = $"Brute Force - {bruteForceCost}";
        keyloggerButtonText.text = $"Keylogger - {keylogCost}";
        sqlInjectButtonText.text = $"SQL Inject - {sqlInjectCost}";
        rootkitButtonText.text = $"Install Rootkit - {rootkitCost}";

        UpdateButtons();
    }

    void RevealDetails()
    {
        StartCoroutine(TypeText(difficultyText, $"Difficulty: {currentPuzzle.difficulty}"));
        //difficultyText.text = $"Difficulty: {currentPuzzle.difficulty}";

        StartCoroutine(TypeText(typeText, $"Type: {currentPuzzle.type}"));
        //typeText.text = $"Type: {currentPuzzle.type}";

        StartCoroutine(TypeText(weaknessText, $"Weakness: {GetWeaknessList(currentPuzzle)}"));
        //weaknessText.text = $"Weakness: {GetWeaknessList(currentPuzzle)}";

        StartCoroutine(TypeText(strongAgainstText, $"Strong Against: {GetStrengthList(currentPuzzle)}"));
        //strongAgainstText.text = $"Strong Against: {GetStrengthList(currentPuzzle)}";
    }

    string GetWeaknessList(HackingPuzzle puzzle)
    {
        string weaknessList = "";
        if(puzzle.weakAgainst.Length == 0) { return weaknessList; }

        for (int i = 0; i < puzzle.weakAgainst.Length; i++)
        {
            if(i == puzzle.weakAgainst.Length-1) 
            { 
                weaknessList += puzzle.weakAgainst[i].ToString(); 
            }
            else
            {
                weaknessList += $"{puzzle.weakAgainst[i]}, ";
            }
        }
        return weaknessList;
    }

    string GetStrengthList(HackingPuzzle puzzle)
    {
        string strengthList = "";
        if (puzzle.strongAgainst.Length == 0) { return strengthList; }

        for (int i = 0; i < puzzle.strongAgainst.Length; i++)
        {
            if (i == puzzle.strongAgainst.Length - 1)
            {
                strengthList += puzzle.strongAgainst[i].ToString();
            }
            else
            {
                strengthList += $"{puzzle.strongAgainst[i]}, ";
            }
        }
        return strengthList;
    }
}
