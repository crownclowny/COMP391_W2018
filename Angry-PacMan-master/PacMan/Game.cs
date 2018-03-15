using System;
using System.Globalization;
using Android.Content;
using Java.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace pacmangame
{
    /* �������� ����� "����". �������� ��� ���������� ������� � ������ ��������� �������� ��������. */

    partial class PacManGame : Game
    {
        // ��������� ������� �������
        private readonly GraphicsDeviceManager Graphics;
        private SpriteBatch SpriteBatch;
        // ��������
        private Texture2D TextureBackground; // ���
        private Texture2D TextureBorder; // ���
        private Texture2D TextureBackgroundLose; // ��� ���� ���������
        private Texture2D TextureBackgroundWin; // ��� ���� ������
        private Texture2D TextureBackgroundLoad; // ��� ���� ��������
        private Texture2D TexturePlayer; // �����
        private Texture2D TexturePlayerBack; // ����� (����������)
        private Texture2D TextureTarget; // ����
        private Texture2D TextureTargetBad; // ����������
        private Texture2D TextureCross; // ������� ������
        private SpriteFont Font;
        // ������
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

        // ������������ �������
        private Player Player; // ����� (������)
        private GameProcess GameProcess = new GameProcess(); // ������� ������� (������)

        // ��������� ���������
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
        // ������
        public string Language = "en";
        public string strScore = "";
        public string strRecord = "";
        public string strScoreAmount = "";
        public string strRecordString = "";
        public string strRecordNotReached = "";
        public string strPacmanInjured = "";
        public string strNewRecord = "";
        // ����������� �����
        public Texture2D Splash;
        public bool SplashShown = true;
        public bool SplashHide = false;

        // ���������� ���������� ������
        public void UpdateScreenAttributies()
        {
            Dx = (float) CurrentWidth/NominalWidth;
            Dy = (float) CurrentHeigth/NominalHeight;

            NominalHeightCounted = CurrentHeigth/Dx;
            NominalWidthCounted = CurrentWidth/Dx;

            int check = Math.Abs(CurrentHeigth - CurrentWidth/16*9);
            if (check > 10)
                deltaY = (float) check/2; // ����������� ���������� �� 16:9 �� � ��� Y (� ���������� �����������)
            deltaY_1 = -(CurrentWidth/16*10 - CurrentWidth/16*9)/2f;

            YTopBorder = -deltaY/Dx; // ���������� ����� � ����� ������� ���� (� ���������� �����������)
            YBottomBorder = NominalHeight + (180); // ���������� ����� � ������ ������� ���� (� ����������� �����������)
        }

        public void DrawRectangle(Rectangle coords, Color color)
        {
            var rect = new Texture2D(GraphicsDevice, 1, 1);
            rect.SetData(new[] {color});
            SpriteBatch.Draw(rect, coords, color);
        }

        // ���������� ���������� X
        public static float AbsoluteX(float x)
        {
            return x*Dx;
        }

        // ���������� ���������� Y
        public static float AbsoluteY(float y)
        {
            return y*Dx + deltaY;
        }

        // ������������� ������ "����"
        public PacManGame()
        {
            // ������������� �������
            Graphics = new GraphicsDeviceManager(this);
            var metric = new Android.Util.DisplayMetrics();
            Activity.WindowManager.DefaultDisplay.GetMetrics(metric);
            // ��������� ���������� ������

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
                strScore = "����: ";
                strRecord = "������: ";
                strScoreAmount = "����� �����: ";
                strRecordString = "������ ";
                strRecordNotReached = " �� ��� �����.";
                strPacmanInjured = "Pacman ��� ����� � ������� ����...";
                strNewRecord = "��������� ������ ";
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

        // �������� ��������
        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice); // ������������� ������� � �������� �������
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

        // ���������� �������� �������� (����������� � ������ ������ �������)
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // �������� ������� �������� �������
            // ��������� ������� ������ "�����"
            if (GameProcess.IsGame)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) GameProcess.IsPause = true;
            }
            if (GameProcess.IsPause)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) GameProcess.IsPause = false;
            }
        }

        // ��������� ���������� �������� (����������� � ������ ���������� ������ �������)
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.AliceBlue); // ��������� ���
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied); // ���������� ���������������� ������� ��������� ��������

            if (SplashShown)
            {
                if (SplashHide)
                {
                    LoadData(Language); // �������� ������ (�������, ������, ������)
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
                    // ���� ���������� ������� ������� - ��������� �������� ��������
                    if (!GameProcess.IsPause)
                    {
                        // ���� ����� ���������
                        SpriteBatch.Draw(TextureBackground, new Vector2(AbsoluteX(0), AbsoluteY(0)),
                                         new Rectangle(0, 0, TextureBackground.Width, TextureBackground.Height),
                                         Color.White,
                                         0, new Vector2(0, 0), 1*Dx, SpriteEffects.None, 0); // ��������� ����
                        foreach (var enemy in GameProcess.Enemies)
                        {
                            // ��������� �����
                            SpriteBatch.Draw(TextureTarget,
                                             new Vector2(AbsoluteX(enemy.Screenpos.X), AbsoluteY(enemy.Screenpos.Y)),
                                             new Rectangle(0, 0, TextureTarget.Width, TextureTarget.Height), Color.White,
                                             0, new Vector2(TextureTarget.Width/2f, TextureTarget.Height/2f), 1*Dx,
                                             SpriteEffects.None, 0);

                        }
                        // ��������� ������
                        if (Player.Direction == 2)
                            SpriteBatch.Draw(TexturePlayerBack, new Vector2(AbsoluteX(Player.X), AbsoluteY(Player.Y)),
                                             new Rectangle(0, 0, TexturePlayerBack.Width, TexturePlayerBack.Height),
                                             Color.White,
                                             Player.Angle,
                                             new Vector2(TexturePlayerBack.Width/2f, TexturePlayerBack.Height/2f), 1*Dx,
                                             SpriteEffects.None, 0);
                            // ���������� ������� (������� �����)
                        else
                            SpriteBatch.Draw(TexturePlayer, new Vector2(AbsoluteX(Player.X), AbsoluteY(Player.Y)),
                                             new Rectangle(0, 0, TexturePlayer.Width, TexturePlayer.Height), Color.White,
                                             Player.Angle,
                                             new Vector2(TexturePlayerBack.Width/2f, TexturePlayerBack.Height/2f), 1*Dx,
                                             SpriteEffects.None, 0);
                        // ������� ������� (������/�����/����)
                        foreach (var badenemy in GameProcess.BadEnemies)
                        {
                            SpriteBatch.Draw(TextureTargetBad,
                                             new Vector2(AbsoluteX(badenemy.Screenpos.X),
                                                         AbsoluteY(badenemy.Screenpos.Y)),
                                             new Rectangle(0, 0, TextureTargetBad.Width, TextureTargetBad.Height),
                                             Color.White,
                                             badenemy.Rotation, new Vector2(0, 0), 1*Dx, SpriteEffects.None, 0);
                        }
                        // ��������� ������� �� ����� ������
                        SpriteBatch.Draw(TextureBorder, new Vector2(AbsoluteX(0), AbsoluteY(0)),
                                         new Rectangle(0, 0, TextureBorder.Width, TextureBorder.Height), Color.White,
                                         0, new Vector2(0, 0), 1*Dx, SpriteEffects.None, 0);
                        // ��������� ����� �����
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
                        // ��������� ������
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

                        // ��������� �������� ����� ("false" ��������, ��� ��������� ���������)
                        foreach (var enemy in GameProcess.Enemies)
                            enemy.Process(false, gameTime);
                        // ��������� �������� ���������� ("true" ��������, ��� �������� ���������)
                        foreach (var badenemy in GameProcess.BadEnemies)
                            badenemy.Process(true, gameTime);
                        Player.Process(gameTime); // ��������� �������� ������
                        Player.WorkWithTarget(); // ��������� ������ � ������
                        // ��������� ���������� ���������
                        // �����
                        ButtonUp.Process(SpriteBatch);
                        ButtonUp.Update(90, 205);
                        if (ButtonUp.IsEnabled)
                        {
                            ButtonUp.Reset();
                            Player.Direction = 3;
                        }
                        // ����
                        ButtonDown.Process(SpriteBatch);
                        ButtonDown.Update(90, 430);
                        if (ButtonDown.IsEnabled)
                        {
                            ButtonDown.Reset();
                            Player.Direction = 4;
                        }
                        // �����
                        ButtonLeft.Process(SpriteBatch);
                        ButtonLeft.Update(18, 312);
                        if (ButtonLeft.IsEnabled)
                        {
                            ButtonLeft.Reset();
                            Player.Direction = 2;
                        }
                        // ������
                        ButtonRight.Process(SpriteBatch);
                        ButtonRight.Update(175, 312);
                        if (ButtonRight.IsEnabled)
                        {
                            Player.Direction = 1;
                            ButtonRight.Reset();
                        }
                        // �����
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
                        // ���� ����� ��������
                        SpriteBatch.Draw(TextureBackgroundLoad, new Vector2(AbsoluteX(0), AbsoluteY(0)),
                                         new Rectangle(0, 0, TextureBackgroundLoad.Width, TextureBackgroundLoad.Height),
                                         Color.White,
                                         0, new Vector2(0, 0), 1*Dx, SpriteEffects.None, 0);

                        SpriteBatch.DrawString(Font,
                                               "�����",
                                               new Vector2(AbsoluteX(420), AbsoluteY(1175)),
                                               Color.White, 0, new Vector2(0, 0), 1.5f*Dx, SpriteEffects.None, 0);
                        // ����������
                        ButtonResumeGame.Process(SpriteBatch);
                        ButtonResumeGame.Update(328, 351);
                        if (ButtonResumeGame.IsEnabled)
                        {
                            ButtonResumeGame.Reset();
                            GameProcess.IsPause = false;
                        }
                        // ������ ����� ����
                        ButtonStartNewGame.Process(SpriteBatch);
                        ButtonStartNewGame.Update(320, 420);
                        if (ButtonStartNewGame.IsEnabled)
                        {
                            ButtonStartNewGame.Reset();
                            GameProcess = new GameProcess();
                            GameProcess.IsGame = true;
                            Player = new Player(GameProcess);
                        }
                        // �����
                        ButtonExit.Process(SpriteBatch);
                        ButtonExit.Update(320, 483);
                        if (ButtonExit.IsEnabled)
                        {
                            ButtonExit.Reset();
                            Exit();
                        }
                        // �������� ����
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
                        // ��������� ���� ���������
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
                        // ������� ������
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
                            // ��������� ���� ������
                            SpriteBatch.Draw(TextureBackgroundWin, new Vector2(AbsoluteX(0), AbsoluteY(0)),
                                             new Rectangle(0, 0, TextureBackgroundWin.Width, TextureBackgroundWin.Height),
                                             Color.White,
                                             0, new Vector2(0, 0), 1*Dx, SpriteEffects.None, 0);
                            SpriteBatch.DrawString(Font,
                                                   strNewRecord +
                                                   GameProcess.Score.ToString(CultureInfo.InvariantCulture) + "!",
                                                   new Vector2(AbsoluteX(350), AbsoluteY(252)),
                                                   Color.White, 0, new Vector2(0, 0), 1*Dx, SpriteEffects.None, 0);
                            // ������ ����� ����
                            ButtonStartNewGame.Process(SpriteBatch);
                            ButtonStartNewGame.Update(320, 320);
                            if (ButtonStartNewGame.IsEnabled)
                            {
                                ButtonStartNewGame.Reset();
                                GameProcess = new GameProcess();
                                GameProcess.IsGame = true;
                                Player = new Player(GameProcess);
                            }
                            // �����
                            ButtonExit.Process(SpriteBatch);
                            ButtonExit.Update(332, 395);
                            if (ButtonExit.IsEnabled)
                            {
                                ButtonExit.Reset();
                                Exit();
                            }
                            // �������� ����
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
                            // ���������� ��������� ����� ����
                            SpriteBatch.Draw(TextureBackgroundLoad, new Vector2(AbsoluteX(0), AbsoluteY(0)),
                                             new Rectangle(0, 0, TextureBackgroundLoad.Width,
                                                           TextureBackgroundLoad.Height),
                                             Color.White,
                                             0, new Vector2(0, 0), 1*Dx, SpriteEffects.None, 0);
                            // ������ ����� ����
                            ButtonNewGame.Process(SpriteBatch);
                            ButtonNewGame.Update(330, 455);
                            if (ButtonNewGame.IsEnabled)
                            {
                                ButtonNewGame.Reset();
                                GameProcess = new GameProcess();
                                GameProcess.IsGame = true;
                                Player = new Player(GameProcess);
                            }
                            // �������� ����
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
                // ���������� �����
                DrawRectangle(
                    new Rectangle(-100, -100, CurrentWidth + 100 + 100, 100 + (int) deltaY),
                    Color.Black);
                DrawRectangle(
                    new Rectangle(-100, CurrentHeigth - (int) deltaY, CurrentWidth + 100 + 100,
                                  (int) deltaY + (int) deltaY_1 + 100), Color.Black);
            }
            SpriteBatch.End(); // �������� ��������� �� ������ �����
            base.Draw(gameTime); // �������� ������� �������� �������
        }
    }
}

