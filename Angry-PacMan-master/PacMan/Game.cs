using System;
using System.Globalization;
using Android.Content;
using Java.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace pacmangame
{
    /* Основной класс "игра". Содержит все визуальные объекты и методы обработки игрового процесса. */

    partial class PacManGame : Game
    {
        // системные объекты графики
        private readonly GraphicsDeviceManager Graphics;
        private SpriteBatch SpriteBatch;
        // текстуры
        private Texture2D TextureBackground; // фон
        private Texture2D TextureBorder; // фон
        private Texture2D TextureBackgroundLose; // фон меню поражения
        private Texture2D TextureBackgroundWin; // фон меню победы
        private Texture2D TextureBackgroundLoad; // фон меню загрузки
        private Texture2D TexturePlayer; // игрок
        private Texture2D TexturePlayerBack; // игрок (отраженный)
        private Texture2D TextureTarget; // цель
        private Texture2D TextureTargetBad; // шестеренка
        private Texture2D TextureCross; // счетчик жизней
        private SpriteFont Font;
        // кнопки
        public Button ButtonUp = new Button();
        public Button ButtonDown = new Button();
        public Button ButtonRight = new Button();
        public Button ButtonLeft = new Button();
        public Button ButtonReplay = new Button();
        public Button ButtonNewGame = new Button();
        public Button ButtonPause = new Button();
        public Button ButtonResumeGame = new Button();
        public Button ButtonStartNewGame = new Button();
        public Button ButtonExit = new Button();
        public Button ButtonVisitSite = new Button();

        // используемые объекты
        private Player Player; // игрок (объект)
        private GameProcess GameProcess = new GameProcess(); // игровой процесс (объект)

        // настройки отрисовки
        public static float Dx = 1f;
        public static float Dy = 1f;
        private static int NominalWidth = 960;
        private static int NominalHeight = 540;
        private static float NominalWidthCounted;
        private static float NominalHeightCounted;
        private static int CurrentWidth;
        private static int CurrentHeigth;
        private static float deltaY = 0;
        private static float deltaY_1 = 0;
        public static float YTopBorder;
        public static float YBottomBorder;
        // строки
        public string Language = "en";
        public string strScore = "";
        public string strRecord = "";
        public string strScoreAmount = "";
        public string strRecordString = "";
        public string strRecordNotReached = "";
        public string strPacmanInjured = "";
        public string strNewRecord = "";
        // загрузочный экран
        public Texture2D Splash;
        public bool SplashShown = true;
        public bool SplashHide = false;

        // обновление параметров экрана
        public void UpdateScreenAttributies()
        {
            Dx = (float) CurrentWidth/NominalWidth;
            Dy = (float) CurrentHeigth/NominalHeight;

            NominalHeightCounted = CurrentHeigth/Dx;
            NominalWidthCounted = CurrentWidth/Dx;

            int check = Math.Abs(CurrentHeigth - CurrentWidth/16*9);
            if (check > 10)
                deltaY = (float) check/2; // недостающее расстояние до 16:9 по п оси Y (в абсолютных координатах)
            deltaY_1 = -(CurrentWidth/16*10 - CurrentWidth/16*9)/2f;

            YTopBorder = -deltaY/Dx; // координата точки в левом верхнем углу (в вируальных координатах)
            YBottomBorder = NominalHeight + (180); // координата точки в нижнем верхнем углу (в виртуальных координатах)
        }

        public void DrawRectangle(Rectangle coords, Color color)
        {
            var rect = new Texture2D(GraphicsDevice, 1, 1);
            rect.SetData(new[] {color});
            SpriteBatch.Draw(rect, coords, color);
        }

        // калибровка координаты X
        public static float AbsoluteX(float x)
        {
            return x*Dx;
        }

        // калибровка координаты Y
        public static float AbsoluteY(float y)
        {
            return y*Dx + deltaY;
        }

        // инициализация класса "игра"
        public PacManGame()
        {
            // инициализация графики
            Graphics = new GraphicsDeviceManager(this);
            var metric = new Android.Util.DisplayMetrics();
            Activity.WindowManager.DefaultDisplay.GetMetrics(metric);
            // установка параметров экрана

            Graphics.IsFullScreen = true;
            Graphics.PreferredBackBufferWidth = metric.WidthPixels;
            Graphics.PreferredBackBufferHeight = metric.HeightPixels;
            CurrentWidth = Graphics.PreferredBackBufferWidth;
            CurrentHeigth = Graphics.PreferredBackBufferHeight;
            Graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;

            UpdateScreenAttributies();

            string strlocale = Locale.Default.ToString();
            strlocale = strlocale.Substring(0, 2);
            if (strlocale.Equals("ru") || strlocale.Equals("be") || strlocale.Equals("uk") || strlocale.Equals("sr") ||
                strlocale.Equals("kz"))
            {
                Language = "ru";
                strScore = "Счет: ";
                strRecord = "Рекорд: ";
                strScoreAmount = "Число очков: ";
                strRecordString = "Рекорд ";
                strRecordNotReached = " не был побит.";
                strPacmanInjured = "Pacman был ранен о границы поля...";
                strNewRecord = "Поставлен рекорд ";
            }
            else
            {
                Language = "en";
                strScore = "Score: ";
                strRecord = "Record: ";
                strScoreAmount = "Reached score: ";
                strRecordString = "Record ";
                strRecordNotReached = " was not reached.";
                strPacmanInjured = "Pacman was injured by field border...";
                strNewRecord = "New record reached: ";
            }
            var locale = new Locale(Language); // languageIso is locale string
            Locale.Default = locale;
            var config = new Android.Content.Res.Configuration {Locale = locale};
            Activity.Resources.UpdateConfiguration(config, Activity.Resources.DisplayMetrics);

        }

        // загрузка контента
        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice); // инициализация графики и загрузка текстур
            Content.RootDirectory = "Content/" + Language;
            Splash = Content.Load<Texture2D>(Language + "_" + "splash");

        }

        public void LoadData(string locale)
        {
            Content.RootDirectory = "Content/";

            TextureBackground = Content.Load<Texture2D>("background_game");
            TextureBorder = Content.Load<Texture2D>("border");
            TextureBackgroundLose = Content.Load<Texture2D>("background_lose");
            TextureBackgroundWin = Content.Load<Texture2D>("background_win");
            TexturePlayer = Content.Load<Texture2D>("pacman");
            TexturePlayerBack = Content.Load<Texture2D>("pacman_back");
            TextureTarget = Content.Load<Texture2D>("target");
            TextureTargetBad = Content.Load<Texture2D>("target_bad");
            TextureCross = Content.Load<Texture2D>("cross");
            Font = Content.Load<SpriteFont>("Font");

            ButtonUp.TextureButton = Content.Load<Texture2D>("ButtonUp");
            ButtonUp.TextureButtonLight = Content.Load<Texture2D>("ButtonUpPressed");
            ButtonDown.TextureButton = Content.Load<Texture2D>("ButtonDown");
            ButtonDown.TextureButtonLight = Content.Load<Texture2D>("ButtonDownPressed");
            ButtonLeft.TextureButton = Content.Load<Texture2D>("ButtonLeft");
            ButtonLeft.TextureButtonLight = Content.Load<Texture2D>("ButtonLeftPressed");
            ButtonRight.TextureButton = Content.Load<Texture2D>("ButtonRight");
            ButtonRight.TextureButtonLight = Content.Load<Texture2D>("ButtonRightPressed");

            ButtonVisitSite.TextureButton = Content.Load<Texture2D>("ButtonSite");
            ButtonVisitSite.TextureButtonLight = Content.Load<Texture2D>("ButtonSitePressed");

            Content.RootDirectory = "Content/" + locale;

            TextureBackgroundLoad = Content.Load<Texture2D>(locale + "_" + "LoadingScreen");
            ButtonReplay.TextureButton = Content.Load<Texture2D>(locale + "_" + "ButtonPlayAgain");
            ButtonReplay.TextureButtonLight = Content.Load<Texture2D>(locale + "_" + "ButtonPlayAgainPressed");

            ButtonNewGame.TextureButton = Content.Load<Texture2D>(locale + "_" + "StartGame");
            ButtonNewGame.TextureButtonLight = Content.Load<Texture2D>(locale + "_" + "StartGamePressed");

            ButtonPause.TextureButton = Content.Load<Texture2D>(locale + "_" + "PauseButton");
            ButtonPause.TextureButtonLight = Content.Load<Texture2D>(locale + "_" + "PauseButtonPressed");

            ButtonResumeGame.TextureButton = Content.Load<Texture2D>(locale + "_" + "ButtonResume");
            ButtonResumeGame.TextureButtonLight = Content.Load<Texture2D>(locale + "_" + "ButtonResumePressed");
            ButtonStartNewGame.TextureButton = Content.Load<Texture2D>(locale + "_" + "ButtonNewGame");
            ButtonStartNewGame.TextureButtonLight = Content.Load<Texture2D>(locale + "_" + "ButtonNewGamePressed");
            ButtonExit.TextureButton = Content.Load<Texture2D>(locale + "_" + "ButtonExit");
            ButtonExit.TextureButtonLight = Content.Load<Texture2D>(locale + "_" + "ButtonExitPressed");
        }

        // обновление игрового процесса (выполняется в каждый момент времени)
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // обновить счетчик игрового времени
            // обработка нажатия кнопки "назад"
            if (GameProcess.IsGame)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) GameProcess.IsPause = true;
            }
            if (GameProcess.IsPause)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) GameProcess.IsPause = false;
            }
        }

        // отрисовка визуальных объектов (выполняется в каждый конкретный момент времени)
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.AliceBlue); // заполнить фон
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied); // установить последовательный порядок отрисовки объектов

            if (SplashShown)
            {
                if (SplashHide)
                {
                    LoadData(Language); // загрузка данных (текстур, звуков, шрифта)
                    SplashShown = false;
                    SplashHide = true;
                }
                else
                {
                    SplashHide = true;
                }
                SpriteBatch.Draw(Splash, new Vector2(AbsoluteX(0), AbsoluteY(0)),
                                 new Rectangle(0, 0, Splash.Width, Splash.Height),
                                 Color.White, 0,
                                 new Vector2(0, 0), Dx, SpriteEffects.None, 0);
            }
            else
            {
                if (GameProcess.IsGame)
                {
                    // если происходит игровой процесс - отрисовка игрового процесса
                    if (!GameProcess.IsPause)
                    {
                        // если пауза выключена
                        SpriteBatch.Draw(TextureBackground, new Vector2(AbsoluteX(0), AbsoluteY(0)),
                                         new Rectangle(0, 0, TextureBackground.Width, TextureBackground.Height),
                                         Color.White,
                                         0, new Vector2(0, 0), 1*Dx, SpriteEffects.None, 0); // отрисовка фона
                        foreach (var enemy in GameProcess.Enemies)
                        {
                            // отрисовка целей
                            SpriteBatch.Draw(TextureTarget,
                                             new Vector2(AbsoluteX(enemy.Screenpos.X), AbsoluteY(enemy.Screenpos.Y)),
                                             new Rectangle(0, 0, TextureTarget.Width, TextureTarget.Height), Color.White,
                                             0, new Vector2(TextureTarget.Width/2f, TextureTarget.Height/2f), 1*Dx,
                                             SpriteEffects.None, 0);

                        }
                        // отрисовка игрока
                        if (Player.Direction == 2)
                            SpriteBatch.Draw(TexturePlayerBack, new Vector2(AbsoluteX(Player.X), AbsoluteY(Player.Y)),
                                             new Rectangle(0, 0, TexturePlayerBack.Width, TexturePlayerBack.Height),
                                             Color.White,
                                             Player.Angle,
                                             new Vector2(TexturePlayerBack.Width/2f, TexturePlayerBack.Height/2f), 1*Dx,
                                             SpriteEffects.None, 0);
                            // отраженный вариант (движени влево)
                        else
                            SpriteBatch.Draw(TexturePlayer, new Vector2(AbsoluteX(Player.X), AbsoluteY(Player.Y)),
                                             new Rectangle(0, 0, TexturePlayer.Width, TexturePlayer.Height), Color.White,
                                             Player.Angle,
                                             new Vector2(TexturePlayerBack.Width/2f, TexturePlayerBack.Height/2f), 1*Dx,
                                             SpriteEffects.None, 0);
                        // обычный вариант (вправо/вверх/вниз)
                        foreach (var badenemy in GameProcess.BadEnemies)
                        {
                            SpriteBatch.Draw(TextureTargetBad,
                                             new Vector2(AbsoluteX(badenemy.Screenpos.X),
                                                         AbsoluteY(badenemy.Screenpos.Y)),
                                             new Rectangle(0, 0, TextureTargetBad.Width, TextureTargetBad.Height),
                                             Color.White,
                                             badenemy.Rotation, new Vector2(0, 0), 1*Dx, SpriteEffects.None, 0);
                        }
                        // отрисовка колючек по краям экрана
                        SpriteBatch.Draw(TextureBorder, new Vector2(AbsoluteX(0), AbsoluteY(0)),
                                         new Rectangle(0, 0, TextureBorder.Width, TextureBorder.Height), Color.White,
                                         0, new Vector2(0, 0), 1*Dx, SpriteEffects.None, 0);
                        // отрисовка числа очков
                        SpriteBatch.DrawString(Font,
                                               strRecord + GameProcess.MaxScore.ToString(CultureInfo.InvariantCulture),
                                               new Vector2(20, 60),
                                               Color.White, 0, new Vector2(0, 0), 0.8f*Dx, SpriteEffects.None, 0);
                        SpriteBatch.DrawString(Font,
                                               strScoreAmount + GameProcess.Score.ToString(CultureInfo.InvariantCulture),
                                               new Vector2(20, 10),
                                               GameProcess.Score <= GameProcess.MaxScore
                                                   ? Color.White
                                                   : new Color(66, 160, 208), 0,
                                               new Vector2(0, 0), 1*Dx, SpriteEffects.None, 0);
                        // отрисовка жизней
                        SpriteBatch.Draw(TextureCross, new Vector2(AbsoluteX(690), AbsoluteY(15)),
                                         new Rectangle(0, 0, TextureCross.Width, TextureCross.Height),
                                         Player.Lives >= 1 ? Color.White : Color.Red,
                                         0, new Vector2(0, 0), 1*Dx, SpriteEffects.None, 0);
                        SpriteBatch.Draw(TextureCross, new Vector2(AbsoluteX(743), AbsoluteY(15)),
                                         new Rectangle(0, 0, TextureCross.Width, TextureCross.Height),
                                         Player.Lives >= 2 ? Color.White : Color.Red,
                                         0, new Vector2(0, 0), 1*Dx, SpriteEffects.None, 0);
                        SpriteBatch.Draw(TextureCross, new Vector2(AbsoluteX(793), AbsoluteY(15)),
                                         new Rectangle(0, 0, TextureCross.Width, TextureCross.Height),
                                         Player.Lives >= 3 ? Color.White : Color.Red,
                                         0, new Vector2(0, 0), 1*Dx, SpriteEffects.None, 0);
                        SpriteBatch.Draw(TextureCross, new Vector2(AbsoluteX(845), AbsoluteY(15)),
                                         new Rectangle(0, 0, TextureCross.Width, TextureCross.Height),
                                         Player.Lives >= 4 ? Color.White : Color.Red,
                                         0, new Vector2(0, 0), 1*Dx, SpriteEffects.None, 0);
                        SpriteBatch.Draw(TextureCross, new Vector2(AbsoluteX(896), AbsoluteY(15)),
                                         new Rectangle(0, 0, TextureCross.Width, TextureCross.Height),
                                         Player.Lives >= 5 ? Color.White : Color.Red,
                                         0, new Vector2(0, 0), 1*Dx, SpriteEffects.None, 0);

                        // обработка действий целей ("false" означает, что ускорение выключено)
                        foreach (var enemy in GameProcess.Enemies)
                            enemy.Process(false, gameTime);
                        // обработка действий шестеренок ("true" означает, что включено ускорение)
                        foreach (var badenemy in GameProcess.BadEnemies)
                            badenemy.Process(true, gameTime);
                        Player.Process(gameTime); // обработка движения игрока
                        Player.WorkWithTarget(); // обработка работы с целями
                        // обработка управления движением
                        // вверх
                        ButtonUp.Process(SpriteBatch);
                        ButtonUp.Update(90, 205);
                        if (ButtonUp.IsEnabled)
                        {
                            ButtonUp.Reset();
                            Player.Direction = 3;
                        }
                        // вниз
                        ButtonDown.Process(SpriteBatch);
                        ButtonDown.Update(90, 430);
                        if (ButtonDown.IsEnabled)
                        {
                            ButtonDown.Reset();
                            Player.Direction = 4;
                        }
                        // влево
                        ButtonLeft.Process(SpriteBatch);
                        ButtonLeft.Update(18, 312);
                        if (ButtonLeft.IsEnabled)
                        {
                            ButtonLeft.Reset();
                            Player.Direction = 2;
                        }
                        // вправо
                        ButtonRight.Process(SpriteBatch);
                        ButtonRight.Update(175, 312);
                        if (ButtonRight.IsEnabled)
                        {
                            Player.Direction = 1;
                            ButtonRight.Reset();
                        }
                        // пауза
                        ButtonPause.Process(SpriteBatch);
                        ButtonPause.Update(830, 484);
                        if (ButtonPause.IsEnabled)
                        {
                            ButtonPause.Reset();
                            GameProcess.IsPause = true;
                        }
                    }
                    else
                    {
                        // если пауза включена
                        SpriteBatch.Draw(TextureBackgroundLoad, new Vector2(AbsoluteX(0), AbsoluteY(0)),
                                         new Rectangle(0, 0, TextureBackgroundLoad.Width, TextureBackgroundLoad.Height),
                                         Color.White,
                                         0, new Vector2(0, 0), 1*Dx, SpriteEffects.None, 0);

                        SpriteBatch.DrawString(Font,
                                               "Пауза",
                                               new Vector2(AbsoluteX(420), AbsoluteY(1175)),
                                               Color.White, 0, new Vector2(0, 0), 1.5f*Dx, SpriteEffects.None, 0);
                        // продолжить
                        ButtonResumeGame.Process(SpriteBatch);
                        ButtonResumeGame.Update(328, 351);
                        if (ButtonResumeGame.IsEnabled)
                        {
                            ButtonResumeGame.Reset();
                            GameProcess.IsPause = false;
                        }
                        // начать новую игру
                        ButtonStartNewGame.Process(SpriteBatch);
                        ButtonStartNewGame.Update(320, 420);
                        if (ButtonStartNewGame.IsEnabled)
                        {
                            ButtonStartNewGame.Reset();
                            GameProcess = new GameProcess();
                            GameProcess.IsGame = true;
                            Player = new Player(GameProcess);
                        }
                        // выход
                        ButtonExit.Process(SpriteBatch);
                        ButtonExit.Update(320, 483);
                        if (ButtonExit.IsEnabled)
                        {
                            ButtonExit.Reset();
                            Exit();
                        }
                        // посетить сайт
                        ButtonVisitSite.Process(SpriteBatch);
                        ButtonVisitSite.Update(741, 475);
                        if (ButtonVisitSite.IsEnabled)
                        {
                            ButtonVisitSite.Reset();
                            var uri = Android.Net.Uri.Parse("http://www.dageron.com/?cat=146");
                            var intent = new Intent(Intent.ActionView, uri);
                            Activity.StartActivity(intent);
                        }
                    }
                }
                else
                {
                    if (GameProcess.IsLose)
                    {
                        // отрисовка меню поражения
                        SpriteBatch.Draw(TextureBackgroundLose, new Vector2(AbsoluteX(0), AbsoluteY(0)),
                                         new Rectangle(0, 0, TextureBackgroundLose.Width, TextureBackgroundLose.Height),
                                         Color.White,
                                         0, new Vector2(0, 0), 1*Dx, SpriteEffects.None, 0);
                        SpriteBatch.DrawString(Font,
                                               strRecordString +
                                               GameProcess.MaxScore.ToString(CultureInfo.InvariantCulture) +
                                               strRecordNotReached,
                                               new Vector2(AbsoluteX(350), AbsoluteY(252)),
                                               Color.White, 0, new Vector2(0, 0), 1*Dx, SpriteEffects.None, 0);
                        if (Player.Lives == 5)
                            SpriteBatch.DrawString(Font, strPacmanInjured,
                                                   new Vector2(AbsoluteX(258), AbsoluteY(278)),
                                                   Color.White, 0, new Vector2(0, 0), 1.2f*Dx, SpriteEffects.None, 0);
                        // сыграть заново
                        ButtonReplay.Process(SpriteBatch);
                        ButtonReplay.Update(330, 355);
                        if (ButtonReplay.IsEnabled)
                        {
                            ButtonReplay.Reset();
                            GameProcess = new GameProcess();
                            GameProcess.IsGame = true;
                            Player = new Player(GameProcess);
                        }
                    }
                    else
                    {
                        if (GameProcess.IsWin)
                        {
                            // отрисовка меню победы
                            SpriteBatch.Draw(TextureBackgroundWin, new Vector2(AbsoluteX(0), AbsoluteY(0)),
                                             new Rectangle(0, 0, TextureBackgroundWin.Width, TextureBackgroundWin.Height),
                                             Color.White,
                                             0, new Vector2(0, 0), 1*Dx, SpriteEffects.None, 0);
                            SpriteBatch.DrawString(Font,
                                                   strNewRecord +
                                                   GameProcess.Score.ToString(CultureInfo.InvariantCulture) + "!",
                                                   new Vector2(AbsoluteX(350), AbsoluteY(252)),
                                                   Color.White, 0, new Vector2(0, 0), 1*Dx, SpriteEffects.None, 0);
                            // начать новую игру
                            ButtonStartNewGame.Process(SpriteBatch);
                            ButtonStartNewGame.Update(320, 320);
                            if (ButtonStartNewGame.IsEnabled)
                            {
                                ButtonStartNewGame.Reset();
                                GameProcess = new GameProcess();
                                GameProcess.IsGame = true;
                                Player = new Player(GameProcess);
                            }
                            // выйти
                            ButtonExit.Process(SpriteBatch);
                            ButtonExit.Update(332, 395);
                            if (ButtonExit.IsEnabled)
                            {
                                ButtonExit.Reset();
                                Exit();
                            }
                            // посетить сайт
                            ButtonVisitSite.Process(SpriteBatch);
                            ButtonVisitSite.Update(380, 180);
                            if (ButtonVisitSite.IsEnabled)
                            {
                                ButtonVisitSite.Reset();
                                var uri = Android.Net.Uri.Parse("http://www.dageron.com/?cat=146");
                                var intent = new Intent(Intent.ActionView, uri);
                                Activity.StartActivity(intent);
                            }
                        }
                        else
                        {
                            // отрисовать стартовый экран игры
                            SpriteBatch.Draw(TextureBackgroundLoad, new Vector2(AbsoluteX(0), AbsoluteY(0)),
                                             new Rectangle(0, 0, TextureBackgroundLoad.Width,
                                                           TextureBackgroundLoad.Height),
                                             Color.White,
                                             0, new Vector2(0, 0), 1*Dx, SpriteEffects.None, 0);
                            // начать новую игру
                            ButtonNewGame.Process(SpriteBatch);
                            ButtonNewGame.Update(330, 455);
                            if (ButtonNewGame.IsEnabled)
                            {
                                ButtonNewGame.Reset();
                                GameProcess = new GameProcess();
                                GameProcess.IsGame = true;
                                Player = new Player(GameProcess);
                            }
                            // посетить сайт
                            ButtonVisitSite.Process(SpriteBatch);
                            ButtonVisitSite.Update(741, 475);
                            if (ButtonVisitSite.IsEnabled)
                            {
                                ButtonVisitSite.Reset();
                                var uri = Android.Net.Uri.Parse("http://www.dageron.com/?cat=146");
                                var intent = new Intent(Intent.ActionView, uri);
                                Activity.StartActivity(intent);
                            }
                        }
                    }
                }
                // отрисовать рамки
                DrawRectangle(
                    new Rectangle(-100, -100, CurrentWidth + 100 + 100, 100 + (int) deltaY),
                    Color.Black);
                DrawRectangle(
                    new Rectangle(-100, CurrentHeigth - (int) deltaY, CurrentWidth + 100 + 100,
                                  (int) deltaY + (int) deltaY_1 + 100), Color.Black);
            }
            SpriteBatch.End(); // прервать отрисовку на данном этапе
            base.Draw(gameTime); // обновить счетчик игрового времени
        }
    }
}

