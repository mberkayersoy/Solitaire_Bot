
# Solitaire Deck Solver

Solitaire Bot is a Unity project that analyzes and solves Klondike Solitaire games automatically. It features a bot that interprets the game state, makes optimal moves, and attempts to solve decks using a recursive Depth-First Search (DFS) algorithm. The project is designed for reproducible experiments, testing, and easy Unity integration.

## Features

- **Automated Solitaire Bot:** Interprets the current game state and plays Solitaire autonomously.
- **Deterministic Decision-Making:** The bot's logic is fully deterministic, ensuring reproducible results for the same deck and state.
- **Seeded Random Deck Generation:** Generate decks randomly with a configurable seed for repeatable experiments.
- **Deck Analysis:** Evaluates all possible moves and selects the best strategy to solve the deck.
- **Unity Integration:** Built as a Unity project for visualization and user interaction.
- **Comprehensive Testing:** Includes test projects to validate solver performance and correctness.
- **DFS-Based Solver:** Uses a recursive Depth-First Search algorithm to explore possible solutions.
- **Logging:** Logs solvability results, seed, execution time, and deck string for reproducibility in `Assets/StreamingAssets/`.

## Project Structure

- `Assets/Scripts/`: Core bot, solver, and game logic scripts
- `Assets/Scenes/`: Unity scenes for visualization
- `Assets/StreamingAssets/`: Output logs and reproducibility data

## Getting Started

1. Open the project in Unity.
2. Open `SampleScene.unity` in the `Assets/Scenes/` folder.
3. Play the scene to watch the bot solve a randomly generated or seeded deck.
4. Check `Assets/StreamingAssets/` for logs and deck data.