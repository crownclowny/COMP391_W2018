using System.Collections.Generic;

namespace pacmangame
{
    // класс "Игровой процесс"
    class GameProcess
    {
        public bool IsWin; // была ли победа (к полю можно обратиться извне, но нельзя изменить извне)
        public bool IsLose; // было ли поражение (к полю можно обратиться извне, но нельзя изменить извне)
        public bool IsGame; // идет ли игровой процесс (к полю можно обратиться извне, но нельзя изменить извне)
        public bool IsPause; // включена ли пауза
		public int Score; // текущий счет 
        public int MaxScore = 0; // наилучший счет (изначально равен нулю)
		ClassScoreManager ClassScoreManager = new ClassScoreManager(); // менеджер счета
		public List<Enemy> Enemies = new List<Enemy>(); // привидения
		public List<Enemy> BadEnemies = new List<Enemy>(); // желтые шестеренки
        // инициализация класса
		public GameProcess()
        {
            IsWin = false; // победы не было
            IsLose = false; // поражения не было
            IsGame = false; // игровой процесс еще не идет
            IsGame = false; // пауза не включена

			// установка рекорда
			ClassScoreManager = ClassScoreManager.ReadScores();
			MaxScore = ClassScoreManager.Score.Value;

            // добавление врагов
            Enemies.Add(new Enemy(100, 300));
            Enemies.Add(new Enemy(500, 400));
            Enemies.Add(new Enemy(300, 400));
            Enemies.Add(new Enemy(200, 350));
            Enemies.Add(new Enemy(600, 100));
            Enemies.Add(new Enemy(150, 200));
            Enemies.Add(new Enemy(400, 150));
            // добавление шестеренок
            BadEnemies.Add(new Enemy(200, 500));
            BadEnemies.Add(new Enemy(610, 200));
            BadEnemies.Add(new Enemy(200, 510));
            BadEnemies.Add(new Enemy(500, 350));
            BadEnemies.Add(new Enemy(100, 420));
        }
        // установка новой игры (подразумевает отрисовку начального экрана)
        public void NewGame()
        {
            IsWin = false;
            IsLose = false;
            IsGame = false;     
			ClassScoreManager.ReadScores();
        }
        // установка победы
        public void WinGame()
        {
            IsWin = true;
            IsLose = false;
            IsGame = false;
			ClassScoreManager.Score.Value = Score;
			ClassScoreManager.WriteScores();
        }
        // установка поражения
        public void LoseGame()
        {
            IsWin = false;
            IsLose = true;
            IsGame = false;
        }
    }
}
