using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour { 

    public int whoseTurn; //0 = x and 1 = o
    public int turnCount; //counts the nummer of turns played
    public GameObject[] turnIcons; //displays whos turn it is
    public Sprite[] playerIcons; //0 = x icon and 1 = o icon
    public Button[] tictactoeSpaces; //playable space for our game
    public int[] markedSpaces; //ID's which space was marked by wich player
    public Text WinnerText; // Hold the text component of the winner text
    public GameObject[] winningLine; //Hold all the different line for show that ther is a winner
    public GameObject winnerPanel;
    public int xPlayersScore;
    public int oPlayersScore;
    public Text xPlayersScoreText;
    public Text oPlayersScoreText;
    public Button xPlayersButton;
    public Button oPlayersButton;
    public GameObject catImage;
    public AudioSource buttonClickAudio;
    public AudioSource buttonRestartAudio;
    public AudioSource buttonRematchAudio;
    public AudioSource WinAudio;
    public AudioSource LoseAudio;

    // Start is called before the first frame update
    void Start(){
        GameSetup();
    }

    //Deze functie word bij start van de game gelijkt uitgevoerd.
    void GameSetup(){ 
        whoseTurn = 0;//Hier zort hij dat 0 als eerst aan de beurt is.
        turnCount = 0;//Hier zet hij de turncounter op 0
        turnIcons[0].SetActive(true);//Hier zet hij de turnIcon op de goeie player
        turnIcons[1].SetActive(false);
        for (int i = 0; i <tictactoeSpaces.Length; i++){//Hier zorgt hij dat je op alle plekken kan klikken in het veld.
            tictactoeSpaces[i].interactable = true;
            tictactoeSpaces[i].GetComponent<Image>().sprite = null;
        }
        for(int i = 0; i < markedSpaces.Length; i++){
            markedSpaces[i] = -100;//Hier zet hij de markedSpaces op een heel overdreven getal anders krijg je 
                                    //foutmeldingen omdat je anders op getallen zoals 0 en 1 komt waar veel mee gebeurt in de game
        }
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void TicTacToeButton(int WhichNumber){
        xPlayersButton.interactable = false;
        oPlayersButton.interactable = false;
        tictactoeSpaces[WhichNumber].image.sprite = playerIcons[whoseTurn];
        tictactoeSpaces[WhichNumber].interactable = false;

        markedSpaces[WhichNumber] = whoseTurn + 1;
        turnCount++;
        if (turnCount > 4){
            bool isWinner = WinnerCheck();
            if (turnCount == 9 && isWinner == false)
            {
                Cat();
            }
        }

        if (whoseTurn == 0){
            whoseTurn = 1;
            turnIcons[0].SetActive(false);
            turnIcons[1].SetActive(true);
        }
        else{
            whoseTurn = 0;
            turnIcons[0].SetActive(true);
            turnIcons[1].SetActive(false);
        }
    }

    //Bij deze functie checkt hij of je gewonnen hebt en hoe.
    bool WinnerCheck(){
        int s1 = markedSpaces[0] + markedSpaces[1] + markedSpaces[2];//Het getal tussen de [] geeft aan welke plek op het speelveld het is. 
        int s2 = markedSpaces[3] + markedSpaces[4] + markedSpaces[5];
        int s3 = markedSpaces[6] + markedSpaces[7] + markedSpaces[8];
        int s4 = markedSpaces[0] + markedSpaces[3] + markedSpaces[6];
        int s5 = markedSpaces[1] + markedSpaces[4] + markedSpaces[7];
        int s6 = markedSpaces[2] + markedSpaces[5] + markedSpaces[8];
        int s7 = markedSpaces[0] + markedSpaces[4] + markedSpaces[8];
        int s8 = markedSpaces[2] + markedSpaces[4] + markedSpaces[6];
        var solutions = new int[] { s1, s2, s3, s4, s5, s6, s7, s8 };
        for (int i = 0; i < solutions.Length; i++){
            if(solutions[i] == 3 * (whoseTurn+1)){//Hier kijkt hij wie 1 van de solutions goed heeft.
                WinnerDisplay(i);
                return true;//Als je 1 van deze solutions hebt gebruikt dan gaat hij door naar winnerDisplay.
            }
        }
        return false;//Als je niet 1 van de solutions hebt dan gaat de game gewoon verder
    }

    //Door deze functie krijg je in beeld wie heeft gewonnen en ook hoe.
    void WinnerDisplay(int indexIn){
        winnerPanel.gameObject.SetActive(true);//Hierdoor weergeeft hij het winnerPannel.
        if(whoseTurn == 0){
            xPlayersScore++;//Hier voegt hij de punten toe voor player 0
            xPlayersScoreText.text = xPlayersScore.ToString();
            WinnerText.text = "Player X Wins!";//Hier past hij de tekst aan voor de gene wie heeft gewonnen.
        }else if(whoseTurn == 1){
            oPlayersScore++;
            oPlayersScoreText.text = oPlayersScore.ToString();
            WinnerText.text = "Player O Wins!";
        }
        winningLine[indexIn].SetActive(true);
        catImage.SetActive(false);// Hier zet hij het gelijkspel plaatje uit.
        PlayWin();//Hierdoor speeld hij de Win.mp3 af.
    }

    public void Rematch(){
        GameSetup();
        for(int i = 0; i <winningLine.Length; i++)
        {
            winningLine[i].SetActive(false);
        }
        winnerPanel.SetActive(false);
        xPlayersButton.interactable = true;
        oPlayersButton.interactable = true;
        catImage.SetActive(false);
    }

    public void Restart(){
        Rematch();
        xPlayersScore = 0;
        oPlayersScore = 0;
        xPlayersScoreText.text = "0";
        oPlayersScoreText.text = "0";
    }

    //Door deze functie kan je zien wie aan de beurt is.
    public void SwitchPlayer(int whichPlayer)
    {
        if(whichPlayer == 0){//Hier zegt hij gewoon als je player 1 is dan gebeurt dit
            whoseTurn = 0;
            turnIcons[0].SetActive(true);//Hier verrandert hij wie de turnicon heeft.
            turnIcons[1].SetActive(false);
        }else if(whichPlayer == 1){
            whoseTurn = 1;
            turnIcons[0].SetActive(false);
            turnIcons[1].SetActive(true);
        }
    }

    //Deze functie is voor als er gelijkspel is.
    void Cat()
    {
        winnerPanel.SetActive(true);
        catImage.SetActive(true);
        WinnerText.text = "CAT";
        PlayLose();
    }


    //De volgende functies zijn voor de geluiden in de game.
    public void PlayButtonClick()
    {
        buttonClickAudio.Play();
    }

    public void PlayRestartClick()
    {
        buttonRestartAudio.Play();
    }

    public void PlayRematchClick()
    {
        buttonRematchAudio.Play();
    }

    public void PlayWin()
    {
        WinAudio.Play();
    }

    public void PlayLose()
    {
        LoseAudio.Play();
    }

}
