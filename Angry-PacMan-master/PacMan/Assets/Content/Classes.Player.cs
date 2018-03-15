using System;

namespace XNAGame.Classes
{
    // класс "игрок"
    class Player
    {
        // поля класса
        public float X { get; private set; } //координата X
        public float Y { get; private set; } //координата Y
        public int Direction; // направление движения (1 - вправо, 2 - влево, 3 - вверх, 4 - вниз)
        public float Speed { get; private set; } // скорость движения
        public float Angle { get; private set; } // угол наклона
        public int Lives; // число жизней
        public GameProcess GameProcess; // игровой процесс (устанавливается обратная связь)
        private int IncreaseSpeedCount; // промежуточная переменная для хранения счетчика увеличения скорости
        // конструктор класса - действия, которые осуществляются при его создании (инициализации)
        public Player(GameProcess importGameProcess)
        {
            // координаты создания - в левой части экрана
            X = 50;
            Y = 200;
            Lives = 5;
            Direction = 1; // направление движения устанавливаем "вправо"
            Angle = 0; // начальный угол равен 0 градусов
            Speed = 3f; // начальная скорость (потом можно изменить)
            GameProcess = importGameProcess; // установка обратной связи с игровым процессом
        }
        // метод обработки взаимодействия игрока с целями
        public void WorkWithTarget()
        {
            foreach (var enemy in GameProcess.Enemies)
            {
                if ((Math.Abs(X - enemy.Screenpos.X) < 40) && (Math.Abs(Y - enemy.Screenpos.Y) < 40))
                {
                    GameProcess.Score++;
                    enemy.IsAlive = false;
                }
            }
            foreach (var badenemy in GameProcess.BadEnemies)
            {
                if ((Math.Abs(X - badenemy.Screenpos.X) < 40) && (Math.Abs(Y - badenemy.Screenpos.Y) < 40))
                {
                    Lives--;
                    badenemy.IsAlive = false;
                }
            }
        }
        // метод обработки игрока
        public void Process()
        {
            if (Lives < 0) // если число жизней меньше нуля
            {
                if (GameProcess.Score <= GameProcess.MaxScore) // если рекорд не был побит
                    GameProcess.LoseGame(); // поражение
                else GameProcess.WinGame(); // победа
            }
            //увеличение скорости до определенного предела
            IncreaseSpeedCount++;
            if (IncreaseSpeedCount == 4000) 
            {
                Speed += 0.3f;
                IncreaseSpeedCount = 0;
            }
            // выбор направления движения
            switch (Direction)
            {
                case 1: // движение вправо
                    {
                        Angle = 0;
                        X += Speed;
                        break;
                    }
                case 2:  // движение влево
                    {
                        Angle = 0;
                        X -= Speed;
                        break;
                    }
                case 3: // движение вверх
                    {
                        Angle = (float)(3.14 + 3.14 / 2);
                        Y -= Speed;
                        break;
                    }
                case 4: // движение вниз
                    {
                        Angle = (float)3.14 / 2;  
                        Y += Speed;
                        break;
                    }
            }
            // проверка позиции игрока (если выходит за границы экрана - установить поражение)
            if ((X < 32) || (Y < 32) || (X > 960-32) || (Y > 540-32))
            {
                Lives = 5; // число жизней равно максимальному (не важно, были они потрачены или нет)
                GameProcess.LoseGame(); // установка поражения
            }
        }
    }
}
