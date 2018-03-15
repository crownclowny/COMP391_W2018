using System.Collections.Generic;

namespace XNAGame.Classes
{
    // класс "Игровой процесс"
    class GameProcess
    {
        public bool IsWin { get; private set; } // была ли победа (к полю можно обратиться извне, но нельзя изменить извне)
        public bool IsLose { get; private set; } // было ли поражение (к полю можно обратиться извне, но нельзя изменить извне)
        public bool IsGame { get; private set; } // идет ли игровой процесс (к полю можно обратиться извне, но нельзя изменить извне)
        public int Score = 0; // текущий счет (изначально равен нулю)
        public int MaxScore = 0; // наилучший счет (изначально равен нулю)
        public List<Enemy> Enemies = new List<Enemy>();
        public List<Enemy> BadEnemies = new List<Enemy>();
        // инициализация класса
        public GameProcess()
        {
            IsWin = false; // победы не было
            IsLose = false; // поражения не было
            IsGame = true; // игровой процесс идет
            // чтение рекорда из файла
            using (var file = new System.IO.StreamReader(@"score.txt"))
            {
                // ReSharper disable AssignNullToNotNullAttribute
                MaxScore = int.Parse(file.ReadLine());
                // ReSharper restore AssignNullToNotNullAttribute
            }
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
        }
        // установка победы
        public void WinGame()
        {
            IsWin = true;
            IsLose = false;
            IsGame = false;
            using (var file = new System.IO.StreamWriter(@"score.txt"))
            {
                file.WriteLine(Score);
            }
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
