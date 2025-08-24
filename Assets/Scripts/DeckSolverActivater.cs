using System.Diagnostics;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;
public class DeckSolverActivater : MonoBehaviour
{
    private DrawType _drawType;
    private int _seed;
    private DeckSolver _deckSolver;

    [SerializeField] private Button _trySolveButton;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private Toggle _toggle;
    [SerializeField] private TMP_Text _statusText;
    private IDataService _dataService;
    void Awake()
    {
        _trySolveButton.onClick.AddListener(StartTest);
        _toggle.onValueChanged.AddListener(ChangeDrawType);
        _inputField.onValueChanged.AddListener(ChangeSeed);
        _dataService = new JsonDataService(new GameDataPathProvider());
        ChangeDrawType(true);
    }
    private async void StartTest()
    {
        _trySolveButton.interactable = false;

        if (_seed == 0)
        {
            _seed = Random.Range(1, 1000);
        }

        _deckSolver = new DeckSolver(new GameState(new Deck(_seed), _drawType));

        _statusText.text = "Solver running...";
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        bool result = await Task.Run(() => _deckSolver.IsSolvable());

        stopwatch.Stop();
        long elapsedMs = stopwatch.ElapsedMilliseconds;

        if (result)
        {
            _statusText.text = $"Seed: {_seed}: \n Solvable Deck \n Time: {elapsedMs} ms \n Visited States: {_deckSolver.VisitedStates.Count}";
        }
        else
        {
            _statusText.text = $"Seed: {_seed}:  \n Unsolvable Deck \n Time: {elapsedMs} ms \n Visited States: {_deckSolver.VisitedStates.Count}";
        }

        _trySolveButton.interactable = true;

        _dataService.SaveData(_seed.ToString() + "_" + _drawType.ToString(),
                            _statusText.text + " | " +
                            CardExtension.DeckToString(_deckSolver.CurrentGameState.Deck.DeckCards));
    }
    private void ChangeSeed(string seed)
    {
        if (!string.IsNullOrEmpty(seed))
        {
            _seed = int.Parse(seed);
        }
        else
        {
            _seed = Random.Range(1, 1000);
        }
    }
    private void ChangeDrawType(bool oneCardDraw)
    {
        _drawType = oneCardDraw ? DrawType.Single : DrawType.Three;
        _toggle.GetComponentInChildren<Text>().text = "Draw Type: " + _drawType.ToString();
    }
}
