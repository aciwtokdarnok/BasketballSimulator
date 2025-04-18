# NBA Basketball Game Simulator

Welcome to the NBA Basketball Game Simulator! This repository provides a comprehensive framework for simulating NBA games using realistic player models and game logic. Whether you’re a developer, a basketball enthusiast, or a data scientist, this project offers tools to generate players, track statistics, and play out full games.

---

## 🏗️ Project Structure

```
.github/
BasketballSimulator/
├── Enums/
│   ├── ActionType.cs
│   ├── HybridPosition.cs
│   ├── PlayerRole.cs
│   └── Position.cs
├── Game/
│   ├── BasketballGame.cs
│   └── PlayerGenerator.cs
├── Models/
│   ├── Player.cs
│   ├── PlayerStats.cs
│   └── PlayerStatsTracker.cs
├── Utils/
├── BasketballSimulator.csproj
├── Program.cs
├── .gitattributes
├── .gitignore
├── BasketballSimulator.sln
├── LICENSE
└── README.md
```

This layout allows clear separation of concerns between enumerations, game logic, data models, and utility functions.

---

## 🚀 Tech Stack

- **Frontend:**
  - Vite
  - React
  - Tailwind CSS
  - shadcn/ui
- **Backend:**
  - ASP.NET Core Web API (C#)
- **Communication:**
  - REST API

This stack enables a modern, responsive frontend paired with a robust, scalable backend.

---

## ⚙️ Features

- **Player Generation:** Create custom players with unique attributes and roles.
- **Game Simulation:** Run full NBA-style games with turn-based logic and event handling.
- **Statistics Tracking:** Log per-player stats and generate game summaries.
- **Extensible Architecture:** Plug in new game rules, actions, or UI components with minimal effort.

---

## 📥 Installation & Setup

1. **Clone the repository:**

   ```bash
   git clone https://github.com/your-username/BasketballSimulator.git
   cd BasketballSimulator
   ```

2. **Backend Setup:**

   ```bash
   cd BasketballSimulator
   dotnet restore
   dotnet build
   dotnet run --project BasketballSimulator.csproj
   ```

   The API will be available at `https://localhost:5001`.

3. **Frontend Setup:**

   ```bash
   cd ../frontend
   npm install
   npm run dev
   ```

   The frontend will run at `http://localhost:5173` by default.

> Ensure your backend is running before launching the frontend to avoid CORS issues.

---

## 🏀 Usage

1. Open your browser to `http://localhost:5173`.
2. Use the UI to generate two teams of players.
3. Click **Simulate Game** to run the simulation.
4. View live play-by-play updates and final statistics.

Feel free to explore the code to customize simulation parameters, team strategies, and more.

---

## 🤝 Contributing

Contributions are welcome! To submit your improvements:

1. Fork the repository.
2. Create a new branch for your feature or bugfix:
   ```bash
   git checkout -b feature/my-new-feature
   ```
3. Commit your changes:
   ```bash
   git commit -m "Add some feature"
   ```
4. Push your branch to your fork:
   ```bash
   git push origin feature/my-new-feature
   ```
5. On GitHub, open a Pull Request targeting the `master` branch of the upstream repository.

**Note:** Direct pushes to `master` are disabled. Please use Pull Requests for all changes.

---

## 📄 License

This project is licensed under the [MIT License](LICENSE).

---

## 📬 Contact

For questions, suggestions, or feedback, feel free to open an issue or reach out to the maintainers.
