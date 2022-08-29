using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
	int playerCount = 0;
	int counter = 1;

	int currentQuestion = 0;
	int currentPlayer = 0;

	int questionsAnswered = 0;

	List<string> shuffledQuestions = new List<string>();
	List<string> shuffledPlayerNames = new List<string>();
	List<string> playerNames = new List<string>();

	[SerializeField]
	GameObject setupScreen;
	[SerializeField]
	GameObject gameScreen;
	[SerializeField]
	GameObject endScreen;

	[SerializeField]
	TMPro.TextMeshProUGUI instructionText;
	[SerializeField]
	TMPro.TMP_InputField playerCountInput;
	[SerializeField]
	TMPro.TMP_InputField playerNameInput;
	[SerializeField]
	TMPro.TextMeshProUGUI questionText;
	[SerializeField]
	TMPro.TextMeshProUGUI playerNameText;

	[SerializeField]
	List<string> questions = new List<string>();

	private void Start()
	{
		ShuffleQuestions();
	}

	void ShuffleQuestions()
	{
		int count = questions.Count;
		while (count != 0)
		{
			int rand = Random.Range(0, questions.Count - 1);
			shuffledQuestions.Add(questions[rand]);
			questions.Remove(questions[rand]);
			count = questions.Count;
		}
	}

	public void SetPlayerCount()
	{
		if (!int.TryParse(playerCountInput.text, out this.playerCount))
			return;

		UpdateInstructionText("Enter Player " + (counter) + "'s Name");
		
		playerCountInput.text = "";
	}

	public void AddPlayer()
	{
		playerNames.Add(playerNameInput.text);

		if (counter < playerCount)
		{
			counter++;
			UpdateInstructionText("Enter Player " + (counter) + "'s Name");

			playerNameInput.text = "Player Name...";
			playerNameInput.Select();
		}
		else
		{
			EndSetup();
		}
	}

	void UpdateInstructionText(string newText)
	{
		instructionText.text = newText;
	}

	void EndSetup()
	{
		int count = playerNames.Count;
		while (count !=0)
		{
			int rand = Random.Range(0, playerNames.Count - 1);
			shuffledPlayerNames.Add(playerNames[rand]);
			playerNames.Remove(playerNames[rand]);
			count = playerNames.Count;
		}

		playerNameText.text = "This one's for " + shuffledPlayerNames[currentPlayer];
		questionText.text = shuffledQuestions[currentQuestion];

		setupScreen.SetActive(false);
		gameScreen.SetActive(true);
	}

	public void NextQuestion()
	{
		if (currentPlayer > shuffledPlayerNames.Count - 1)
			EndGame();

		questionsAnswered++;
		if(questionsAnswered > 1)
		{
			currentPlayer++;
			if (currentPlayer == shuffledPlayerNames.Count)
			{
				EndGame();
				return;
			}
			
			playerNameText.text = "This one's for " + shuffledPlayerNames[currentPlayer];
			questionsAnswered = 0;
		}

		if(currentQuestion < shuffledQuestions.Count)
		{
			currentQuestion++;
		}
		else
		{
			ShuffleQuestions();
			currentQuestion = 0;
		}

		questionText.text = shuffledQuestions[currentQuestion];
	}

	void EndGame()
	{
		gameScreen.SetActive(false);
		endScreen.SetActive(true);
	}

	public void ResetGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
