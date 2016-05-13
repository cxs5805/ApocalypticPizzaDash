using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ApocalypticPizzaDash
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        //hi-score attributes
        char[] initials = new char[3];
        int[] scores = new int[3]; string[] names = new string[3];
        bool isLoaded = false, isWrittem = false;
        int currentScore;
        int timesEntered;
        int currentLetter;
        int[] index = new int[3];
        int myScorePos;

        //alphabet for user letter choosing
        List<char> alphabet;

        //level loading pizza
        Texture2D loadingBkd;
        Texture2D pizza;
        int pizzaFramesElapsed;
        int numPizzaFrames = 7;
        int pizzaFrame;
        const int PIZZA_HEIGHT = 72;
        const int PIZZA_WIDTH = 120;
        double timePerPizzaFrame = 100;
        Vector2 pizzaPos;

        // general game attributes
        enum GameState { Menu, Game, GameOver, Loading }
        GameState gState, gStatePrev;
        KeyboardState kState, kStatePrev;
        double timer;
        double maxTime = 6000;
        double minTime = 1200;
        int score, loop;
        bool isPaused, isLoading;
        int elTimer = 0;
        Texture2D backdrop, sky, pause, gameover;

        //player attributes
        Player player;
        int playerDeliveryAnimationOffset;
        int playerAttackAnimationOffset;
        const int PLAYER_HEIGHT = 46;
        const int PLAYER_WIDTH = 30;

        //animate player attacking 
        const int PLAYER_ATTACK_HEIGHT = 46;
        const int PLAYER_ATTACK_WIDTH = 42;
        int playerAttackFrame;
        int numPlayerAttackFrames = 3;
        int playerAttackFramesElapsed;
        double timePerPlayerAttackFrame = 100;

        // animate player climbing
        const int PLAYER_CLIMB_HEIGHT = 46;
        const int PLAYER_CLIMB_WIDTH = 28;
        int playerClimbFrame;
        int numPlayerClimbFrames = 3;
        int playerClimbFramesElapsed;
        double timePerPlayerClimbFrame = 100;

        // animate delivery
        const int PLAYER_DELIVERY_HEIGHT = 46;
        const int PLAYER_DELIVERY_WIDTH = 42;
        int playerDeliveryFrame;
        int numPlayerDeliveryFrames = 10;
        int playerDeliveryFramesElapsed;
        double timePerPlayerDeliveryFrame = 100;

        //animating player movement
        int numPlayerFrames = 7;
        int playerFrame;
        int playerFramesElapsed;


        double timePerPlayerFrame = 100;

        // zombie attributes
        List<Zombie> zombies;
        List<BossZombie> bosses;
        Texture2D zombie1;
        Texture2D zombie2;
        Texture2D bZombie;
        int zombieSpeed;
        int maxZombieSpeed = 2;
        int bossSpeed;
        int maxBossspeed = 2;
        int zombieHealth;
        int bossHealth;
        int maxZombieHealth = 10;
        int maxBossHealth = 20;
        const int ZOMBIE_HEIGHT = 42;
        const int ZOMBIE_WIDTH = 26;
        const int BOSS_HEIGHT = 52;
        const int BOSS_WIDTH = 40;
        int zombieFrame;
        int numZombieFrames = 5;
        int zombieFramesElapsed;
        double timePerZombieFrame = 100;
        int bossFrame;
        int numBossFrames = 3;
        int bossFramesElapsed;
        double timePerBossFrame = 100;


        // buildings
        List<Building> buildings;
        bool buildingsLeft;
        Texture2D building1;
        Texture2D building2;
        Texture2D building3;
        Texture2D building4;
        Texture2D building5;
        Texture2D building6;
        Texture2D delivery2;
        int currentBuildings;

        // level stuff
        int currentLevel;
        List<int> levelData;
        List<Rectangle> levelRects;
        LevelReader reader;
        int levelWidth;

        //indicator stuff
        List<Rectangle> indicator;
        Rectangle charStandee;
        Rectangle indBar;
        Texture2D indicatorBar;
        Texture2D playerStandee;
        Texture2D delivery;


        SpriteFont font;
        private Texture2D background, playerImage, playerAttack, playerClimb, playerDeliver;
        //List<Texture2D> UI = new List<Texture2D>();
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont testFont;
        Rectangle screen;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 450;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //hi score stuff
            initials[0] = 'A';
            initials[1] = 'A';
            initials[2] = 'A';
            index[0] = 0;
            index[1] = 0;
            index[2] = 0;
            alphabet = new List<char>();

            //adds each letter of the alphabet to the list
            alphabet.Add('A');
            alphabet.Add('B');
            alphabet.Add('C');
            alphabet.Add('D');
            alphabet.Add('E');
            alphabet.Add('F');
            alphabet.Add('G');
            alphabet.Add('H');
            alphabet.Add('I');
            alphabet.Add('J');
            alphabet.Add('K');
            alphabet.Add('L');
            alphabet.Add('M');
            alphabet.Add('N');
            alphabet.Add('O');
            alphabet.Add('P');
            alphabet.Add('Q');
            alphabet.Add('R');
            alphabet.Add('S');
            alphabet.Add('T');
            alphabet.Add('U');
            alphabet.Add('V');
            alphabet.Add('W');
            alphabet.Add('X');
            alphabet.Add('Y');
            alphabet.Add('Z');

            // start game out unpaused
            isPaused = false;

            // initi the player object
            player = new Player(null, new Rectangle(0, GraphicsDevice.Viewport.Height - 75,
            PLAYER_WIDTH, PLAYER_HEIGHT), 3, 3);

            // init Zombie list and add one zombie for testing purposes
            zombies = new List<Zombie>();
            zombies.Add(new Zombie(player, null, new Rectangle(GraphicsDevice.Viewport.Width,
            GraphicsDevice.Viewport.Height - 75, ZOMBIE_WIDTH, ZOMBIE_HEIGHT), 3, 1));
            zombieSpeed = 1;
            zombieHealth = 3;

            //also a boss
            bosses = new List<BossZombie>();
            bosses.Add(new BossZombie(player, null, new Rectangle(GraphicsDevice.Viewport.Width,
            GraphicsDevice.Viewport.Height - 100, BOSS_WIDTH, BOSS_HEIGHT), 3, 2));
            bossSpeed = 1;
            bossHealth = 5;

            // init buildings list
            buildings = new List<Building>();

            // init level stuff
            reader = new LevelReader();
            levelData = new List<int>();
            levelRects = new List<Rectangle>();
            levelWidth = 0;

            // set up indicator stuff
            indicator = new List<Rectangle>();
            charStandee = new Rectangle(0, 20, 8, 12);
            indBar = new Rectangle(((GraphicsDevice.Viewport.Width / 2) - (101)), 32, 202, 12);

            // init the screen's position
            screen = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            // by default, start game at the menu
            gState = GameState.Menu;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // this background image is only used at the menu
            background = Content.Load<Texture2D>("title screen");

            // basic backdrop for each level
            backdrop = Content.Load<Texture2D>("BG-1");
            sky = Content.Load<Texture2D>("BG-2");

            gameover = Content.Load<Texture2D>("gameover");

            pause = Content.Load<Texture2D>("pausebox2");

            //loading screen pizza
            pizza = Content.Load<Texture2D>("pizza");

            // now giving the player and zombie their respective sprites
            player.Image = Content.Load<Texture2D>("spritesheet");
            playerAttack = Content.Load<Texture2D>("attack2");
            playerClimb = Content.Load<Texture2D>("climb1");
            playerDeliver = Content.Load<Texture2D>("PZG_PZZx2");

            zombie1 = Content.Load<Texture2D>("Zombie1");
            zombie2 = Content.Load<Texture2D>("Zombie2");
            bZombie = Content.Load<Texture2D>("BossZombieWalking");

            // buildings
            building1 = Content.Load<Texture2D>("Building1");
            building2 = Content.Load<Texture2D>("Building2");
            building3 = Content.Load<Texture2D>("Building3");
            building4 = Content.Load<Texture2D>("Building4");
            building5 = Content.Load<Texture2D>("Building5");
            building6 = Content.Load<Texture2D>("Building6");
            delivery2 = Content.Load<Texture2D>("Delivery2");

            //indicator
            playerStandee = Content.Load<Texture2D>("Pizzaperson");
            indicatorBar = Content.Load<Texture2D>("IndicatorBar");
            delivery = Content.Load<Texture2D>("Delivery");

            testFont = Content.Load<SpriteFont>("Arial14Bold");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            player.Attack(kState);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (gState)
            {
                case GameState.Menu:

                    // respawn the player
                    if(player.Die())
                    {
                      player = new Player(player.Image, new Rectangle(0, GraphicsDevice.Viewport.Height - 75,
                      PLAYER_WIDTH, PLAYER_HEIGHT), player.TotalHealth, 3);
                    }

                    // respawn the zombie(s)
                    for (int i = 0; i < zombies.Count; i++)
                    {
                        if (zombies[i].Die())
                        {
                            zombies[i] = new Zombie(player, zombies[i].Image, new Rectangle(GraphicsDevice.Viewport.Width,
                                GraphicsDevice.Viewport.Height - 75, PLAYER_WIDTH, PLAYER_HEIGHT), 3, 1);
                        }
                    }

                    // when the user hits "enter", the game begins
                    kState = Keyboard.GetState();
                    if(kState.IsKeyDown(Keys.Enter) && kStatePrev.IsKeyUp(Keys.Enter))
                    {
                        gState = GameState.Game;

                        // each level lasts 100 seconds (1 min 40)
                        bossSpeed = 2;
                        bossHealth = 5; 
                        zombieSpeed = 1;
                        zombieHealth = 3;
                        currentLevel = 1;


                        loop = 1;
                        timer = maxTime - ((loop - 1) * 1200);
                        if (timer < minTime)
                        {
                            timer = minTime;
                        }

                        // by default, player has no points
                        currentScore = 0;
                        buildingsLeft = true;

                        // clear any leftover in-game assets 
                        levelData.Clear();
                        buildings.Clear();
                        zombies.Clear();
                        bosses.Clear();
                        indicator.Clear();

                        // read in level file and set
                        levelData = reader.readIn("Content/Levels/level" + currentLevel.ToString() + ".dat");
                        levelWidth = levelData[0] * 2;
                        levelData.RemoveAt(0);
                        levelRects = reader.makeRect(levelData);
                        int currentZombies = 0;
                        int currentbosses = 0;
                        currentBuildings = 0;
                        for(int i = 0; i < levelData.Count; i += 3)
                        {
                            switch(levelData[i])
                            {
                                case 0:
                                    if (currentBuildings < buildings.Count)
                                    {
                                        buildings[currentBuildings] = new Building(0, levelRects[i / 3], building1);
                                        buildings[currentBuildings].SetHitboxes();
                                        indicator.Add(new Rectangle(indBar.X + ((indBar.Width * buildings[currentBuildings].Hitboxes["door1"].X) / levelWidth) - 8, 45, 22, 20));
                                        currentBuildings++;
                                    }
                                    else
                                    {
                                        buildings.Add(new Building(0, levelRects[i / 3], building1));
                                        buildings[currentBuildings].SetHitboxes();
                                        indicator.Add(new Rectangle(indBar.X + ((indBar.Width * buildings[currentBuildings].Hitboxes["door1"].X) / levelWidth) - 8, 45, 22, 20));
                                        currentBuildings++;
                                    }
                                    break;
                                case 1:
                                    if(currentBuildings < buildings.Count)
                                    {
                                        buildings[currentBuildings] = new Building(1, levelRects[i / 3], building2);
                                        buildings[currentBuildings].SetHitboxes();
                                        indicator.Add(new Rectangle(indBar.X + ((indBar.Width * buildings[currentBuildings].Hitboxes["door1"].X) / levelWidth) - 8, 45, 22, 20));
                                        currentBuildings++;
                                    }
                                    else
                                    {
                                        buildings.Add(new Building(1, levelRects[i / 3], building2));
                                        buildings[currentBuildings].SetHitboxes();
                                        indicator.Add(new Rectangle(indBar.X + ((indBar.Width * buildings[currentBuildings].Hitboxes["door1"].X) / levelWidth) - 8, 45, 22, 20));
                                        currentBuildings++;
                                    }
                                    break;
                                case 2:
                                    player = new Player(player.Image, levelRects[i / 3], player.TotalHealth, 3);
                                    player.AllDelivered = false;
                                    break;
                                case 3:
                                    if (currentZombies < zombies.Count)
                                    {
                                        zombies[currentZombies] = new Zombie(player, zombie1, levelRects[i / 3], zombieHealth, zombieSpeed);
                                        currentZombies++;
                                    }
                                    else
                                    {
                                        zombies.Add(new Zombie(player, zombie1, levelRects[i / 3], zombieHealth, zombieSpeed));
                                        currentZombies++;
                                    }
                                    if (zombies[currentZombies - 1].Rect.Y != 360)
                                    {
                                        zombies[currentZombies - 1].Rect = new Rectangle(zombies[currentZombies - 1].Rect.X, 360, zombies[currentZombies - 1].Rect.Width,
                                            zombies[currentZombies - 1].Rect.Height);
                                    }
                                    break;
                                case 4:
                                    if (currentZombies < zombies.Count)
                                    {
                                        zombies[currentZombies] = new Zombie(player, zombie2, levelRects[i / 3], zombieHealth, zombieSpeed);
                                        currentZombies++;
                                    }
                                    else
                                    {
                                        zombies.Add(new Zombie(player, zombie2, levelRects[i / 3], zombieHealth, zombieSpeed));
                                        currentZombies++;
                                    }
                                    if (zombies[currentZombies - 1].Rect.Y != 360)
                                    {
                                        zombies[currentZombies - 1].Rect = new Rectangle(zombies[currentZombies - 1].Rect.X, 360, zombies[currentZombies - 1].Rect.Width,
                                            zombies[currentZombies - 1].Rect.Height);
                                    }
                                    break;
                                case 5:
                                    if (currentBuildings < buildings.Count)
                                    {
                                        buildings[currentBuildings] = new Building(2, levelRects[i / 3], building3);
                                        buildings[currentBuildings].SetHitboxes();
                                        indicator.Add(new Rectangle(indBar.X + ((indBar.Width * buildings[currentBuildings].Hitboxes["door1"].X) / levelWidth) - 8, 45, 22, 20));
                                        currentBuildings++;
                                    }
                                    else
                                    {
                                        buildings.Add(new Building(2, levelRects[i / 3], building3));
                                        buildings[currentBuildings].SetHitboxes();
                                        indicator.Add(new Rectangle(indBar.X + ((indBar.Width * buildings[currentBuildings].Hitboxes["door1"].X) / levelWidth) - 8, 45, 22, 20));
                                        currentBuildings++;
                                    }
                                    break;
                                case 6:
                                    if (currentBuildings < buildings.Count)
                                    {
                                        buildings[currentBuildings] = new Building(3, levelRects[i / 3], building4);
                                        buildings[currentBuildings].SetHitboxes();
                                        indicator.Add(new Rectangle(indBar.X + ((indBar.Width * buildings[currentBuildings].Hitboxes["door1"].X) / levelWidth) - 8, 45, 22, 20));
                                        currentBuildings++;
                                    }
                                    else
                                    {
                                        buildings.Add(new Building(3, levelRects[i / 3], building4));
                                        buildings[currentBuildings].SetHitboxes();
                                        indicator.Add(new Rectangle(indBar.X + ((indBar.Width * buildings[currentBuildings].Hitboxes["door1"].X) / levelWidth) - 8, 45, 22, 20));
                                        currentBuildings++;
                                    }
                                    break;
                                case 7:
                                    if (currentBuildings < buildings.Count)
                                    {
                                        buildings[currentBuildings] = new Building(4, levelRects[i / 3], building5);
                                        buildings[currentBuildings].SetHitboxes();
                                        indicator.Add(new Rectangle(indBar.X + ((indBar.Width * buildings[currentBuildings].Hitboxes["door1"].X) / levelWidth) - 8, 45, 22, 20));
                                        currentBuildings++;
                                    }
                                    else
                                    {
                                        buildings.Add(new Building(4, levelRects[i / 3], building5));
                                        buildings[currentBuildings].SetHitboxes();
                                        indicator.Add(new Rectangle(indBar.X + ((indBar.Width * buildings[currentBuildings].Hitboxes["door1"].X) / levelWidth) - 8, 45, 22, 20));
                                        currentBuildings++;
                                    }
                                    break;
                                case 8:
                                    if (currentBuildings < buildings.Count)
                                    {
                                        buildings[currentBuildings] = new Building(5, levelRects[i / 3], building6);
                                        buildings[currentBuildings].SetHitboxes();
                                        indicator.Add(new Rectangle(indBar.X + ((indBar.Width * buildings[currentBuildings].Hitboxes["door1"].X) / levelWidth) - 8, 45, 22, 20));
                                        currentBuildings++;
                                    }
                                    else
                                    {
                                        buildings.Add(new Building(5, levelRects[i / 3], building6));
                                        buildings[currentBuildings].SetHitboxes();
                                        indicator.Add(new Rectangle(indBar.X + ((indBar.Width * buildings[currentBuildings].Hitboxes["door1"].X) / levelWidth) - 8, 45, 22, 20));
                                        currentBuildings++;
                                    }
                                    break;
                                case 9:
                                    bosses.Add(new BossZombie(player, bZombie, levelRects[i / 3], bossHealth, bossSpeed));
                                    currentbosses++;
                                    if (bosses[currentbosses - 1].Rect.Y + bosses[currentbosses - 1].Rect.Height != 402)
                                    {
                                        bosses[currentbosses - 1].Rect = new Rectangle(bosses[currentbosses - 1].Rect.X, 402 - bosses[currentbosses - 1].Rect.Height,
                                            bosses[currentbosses - 1].Rect.Height, bosses[currentbosses - 1].Rect.Width);
                                    }
                                    break;
                            }
                        }
                    }
                    kStatePrev = kState;
                    break;

                case GameState.Game:

                    // cause game over if timer runs out
                    if(timer < 60)
                    {
                        if (player.Lives == 0)
                        {
                            gState = GameState.GameOver;
                            isWrittem = false;
                            currentLetter = 0;
                            LoadScores();
                        }
                        else
                        {
                            player.Lives--;
                            player.IsDelivering = false;
                            timer = maxTime - ((loop - 1) * 1200);
                            if (timer < minTime)
                            {
                                timer = minTime;
                            }
                            int currentZombies = 0;
                            int currentBosses = 0;
                            for (int i = 0; i < levelData.Count; i += 3)
                            {
                                switch (levelData[i])
                                {
                                    case 2:
                                        player = new Player(player.Image, new Rectangle(levelRects[i / 3].X, levelRects[i / 3].Y, PLAYER_WIDTH, PLAYER_HEIGHT), player.TotalHealth, player.Lives);
                                        break;
                                    case 3:
                                        zombies[currentZombies] = new Zombie(player, zombie1, levelRects[i / 3], zombieHealth, zombieSpeed);
                                        currentZombies++;
                                        if (zombies[currentZombies - 1].Rect.Y != 360)
                                        {
                                            zombies[currentZombies - 1].Rect = new Rectangle(zombies[currentZombies - 1].Rect.X, 360, zombies[currentZombies - 1].Rect.Width,
                                                zombies[currentZombies - 1].Rect.Height);
                                        }
                                        break;
                                    case 4:
                                        zombies[currentZombies] = new Zombie(player, zombie2, levelRects[i / 3], zombieHealth, zombieSpeed);
                                        currentZombies++;
                                        if (zombies[currentZombies - 1].Rect.Y != 360)
                                        {
                                            zombies[currentZombies - 1].Rect = new Rectangle(zombies[currentZombies - 1].Rect.X, 360, zombies[currentZombies - 1].Rect.Width,
                                                zombies[currentZombies - 1].Rect.Height);
                                        }
                                        break;
                                    case 9:
                                        if (bosses.Count > 0)
                                        {
                                            if (bosses[currentBosses].CurrentHealth > 0)
                                            {
                                                bosses[currentBosses] = new BossZombie(player, bZombie, levelRects[i / 3], bossHealth, bossSpeed);
                                                if (bosses[currentBosses].Rect.Y + bosses[currentBosses].Rect.Height != 402)
                                                {
                                                    bosses[currentBosses].Rect = new Rectangle(bosses[currentBosses].Rect.X, 402 - bosses[currentBosses].Rect.Height,
                                                        bosses[currentBosses].Rect.Width, bosses[currentBosses].Rect.Height);
                                                }
                                            }
                                            currentBosses++;
                                        }
                                        break;
                                }
                            }
                        }
                    }

                    // get current keyboard state
                    kState = Keyboard.GetState();

                    // pause and unpause the game
                    if(kState.IsKeyDown(Keys.Enter) && kStatePrev.IsKeyUp(Keys.Enter))
                    {
                        if(!isPaused)
                        {
                            isPaused = true;
                        }
                        else
                        {
                            isPaused = false;
                        }
                    }

                    // set previous keyboard state
                    kStatePrev = kState;

                    // normally, run the game when it isn't paused
                    if (!isPaused)
                    {
                        // by default, no objects are colliding
                        player.IsColliding = false;
                        for (int i = 0; i < zombies.Count; i++)
                        {
                            zombies[i].IsColliding = false;
                        }
                        for (int i = 0; i < bosses.Count; i++)
                        {
                            bosses[i].IsColliding = false;
                        }
                        for (int i = 0; i < zombies.Count; i++)
                        {
                            // if player's attack box collides with zombie, zombie takes damage
                            if (player.AttackBox.Intersects(zombies[i].Rect) && zombies[i].CurrentHealth > 0)
                            {
                                // zombie gets pushed back
                                if (zombies[i].Rect.X - player.AttackBox.X > 0)
                                {
                                    zombies[i].Rect = new Rectangle(zombies[i].Rect.X + 13, zombies[i].Rect.Y, zombies[i].Rect.Width, zombies[i].Rect.Height);
                                }
                                else if (zombies[i].Rect.X - player.AttackBox.X <= 0)
                                {
                                    zombies[i].Rect = new Rectangle(zombies[i].Rect.X - 13, zombies[i].Rect.Y, zombies[i].Rect.Width, zombies[i].Rect.Height);
                                }

                                zombies[i].IsColliding = true;
                                zombies[i].Collision();

                                // Increment score by 50 points upon kill
                                if(zombies[i].CurrentHealth == 0 && zombies[i].Rect != Rectangle.Empty)
                                {
                                    currentScore += 50;
                                }
                            }
                            // if zombie collides with player, player takes damage
                            if (zombies[i].Rect.Intersects(player.Rect) && player.CurrentHealth > 0 && player.Invincible == 0)
                            {
                                player.IsColliding = true;
                                player.Collision();
                                player.Invincible = 120;
                            }
                        }

                        for (int i = 0; i < bosses.Count; i++)
                        {
                            // if player's attack box collides with boss, it takes damage
                            if (player.AttackBox.Intersects(bosses[i].Rect) && bosses[i].CurrentHealth > 0)
                            {
                                // boss gets pushed back (may remove)
                                if (bosses[i].Rect.X - player.AttackBox.X > 0)
                                {
                                    bosses[i].Rect = new Rectangle(bosses[i].Rect.X + 13, bosses[i].Rect.Y, bosses[i].Rect.Width, bosses[i].Rect.Height);
                                }
                                else if (bosses[i].Rect.X - player.AttackBox.X <= 0)
                                {
                                    bosses[i].Rect = new Rectangle(bosses[i].Rect.X - 13, bosses[i].Rect.Y, bosses[i].Rect.Width, bosses[i].Rect.Height);
                                }

                                bosses[i].IsColliding = true;
                                bosses[i].Collision();

                                // Increment score by 100 points upon kill
                                if (bosses[i].CurrentHealth == 0 && bosses[i].Rect != Rectangle.Empty)
                                {
                                    score += 100;
                                }
                            }
                            // if boss collides with player, player takes damage
                            if (bosses[i].Rect.Intersects(player.Rect) && player.CurrentHealth > 0 && player.Invincible == 0)
                            {
                                player.IsColliding = true;
                                player.Collision();
                                player.Invincible = 120;
                            }
                            //if boss is aggro and collides with zombie, knockback
                            for (int j = 0; j < zombies.Count; j++)
                            {
                                if (zombies[j].Rect.Intersects(bosses[i].Rect) && bosses[i].isAngry)
                                {
                                    zombies[j].Fall();
                                    //zombies[j].CurrentHealth = zombies[i].CurrentHealth - 1;
                                }
                            }
                        }
                        if (player.Invincible > 0)
                        {
                            player.Invincible--;
                        }
                        player.WasColliding = player.IsColliding;
                        for (int i = 0; i < zombies.Count; i++)
                        {
                            zombies[i].WasColliding = zombies[i].IsColliding;
                        }

                        // getting total number of frames elapsed thus far in the existence of each object 
                        playerAttackFramesElapsed = (int)(gameTime.TotalGameTime.TotalMilliseconds / timePerPlayerAttackFrame);
                        playerClimbFramesElapsed = (int)(gameTime.TotalGameTime.TotalMilliseconds / timePerPlayerClimbFrame);
                        playerDeliveryFramesElapsed = (int)(gameTime.TotalGameTime.TotalMilliseconds / timePerPlayerDeliveryFrame);
                        playerFramesElapsed = (int)(gameTime.TotalGameTime.TotalMilliseconds / timePerPlayerFrame);
                        zombieFramesElapsed = (int)(gameTime.TotalGameTime.TotalMilliseconds / timePerZombieFrame);
                        bossFramesElapsed = (int)(gameTime.TotalGameTime.TotalMilliseconds / timePerBossFrame);


                        // when the player runs out of health, the game ends
                        if (player.Die())
                        {
                            if (player.Lives == 0)
                            {
                                isLoaded = false;
                                isWrittem = false;
                                gState = GameState.GameOver;
                                currentLetter = 0;
                                LoadScores();
                            }
                            else
                            {
                                player.Lives--;
                                player.IsDelivering = false;
                                timer = maxTime - ((loop - 1) * 1200);
                                if (timer < minTime)
                                {
                                    timer = minTime;
                                }
                                int currentZombies = 0;
                                int currentBosses = 0;
                                for (int i = 0; i < levelData.Count; i += 3)
                                {
                                    switch(levelData[i])
                                    {
                                        case 2:
                                            player = new Player(player.Image, new Rectangle(levelRects[i / 3].X, levelRects[i / 3].Y, PLAYER_WIDTH, PLAYER_HEIGHT), player.TotalHealth, player.Lives);
                                            break;
                                        case 3:
                                            zombies[currentZombies] = new Zombie(player, zombie1, levelRects[i / 3], zombieHealth, zombieSpeed);
                                            currentZombies++;
                                            if (zombies[currentZombies - 1].Rect.Y != 360)
                                            {
                                                zombies[currentZombies - 1].Rect = new Rectangle(zombies[currentZombies - 1].Rect.X, 360, zombies[currentZombies - 1].Rect.Width,
                                                    zombies[currentZombies - 1].Rect.Height);
                                            }
                                            break;
                                        case 4:
                                            zombies[currentZombies] = new Zombie(player, zombie2, levelRects[i / 3], zombieHealth, zombieSpeed);
                                            currentZombies++;
                                            if (zombies[currentZombies - 1].Rect.Y != 360)
                                            {
                                                zombies[currentZombies - 1].Rect = new Rectangle(zombies[currentZombies - 1].Rect.X, 360, zombies[currentZombies - 1].Rect.Width,
                                                    zombies[currentZombies - 1].Rect.Height);
                                            }
                                            break;
                                        case 9:
                                            if (bosses.Count > 0)
                                            {
                                                if (bosses[currentBosses].CurrentHealth > 0)
                                                {
                                                    bosses[currentBosses] = new BossZombie(player, bZombie, levelRects[i / 3], bossHealth, bossSpeed);
                                                    if (bosses[currentBosses].Rect.Y + bosses[currentBosses].Rect.Height != 402)
                                                    {
                                                        bosses[currentBosses].Rect = new Rectangle(bosses[currentBosses].Rect.X, 402 - bosses[currentBosses].Rect.Height,
                                                            bosses[currentBosses].Rect.Width, bosses[currentBosses].Rect.Height);
                                                    }
                                                }
                                                currentBosses++;
                                            }

                                            break;
                                    }
                                }
                            }
                        }

                        // when the zombie runs out of health, it dies
                        for (int i = 0; i < zombies.Count; i++)
                        {
                            if (zombies[i].CurrentHealth <= 0)
                            {
                                zombies[i].Rect = Rectangle.Empty;
                            }
                        }

                        // as does the boss
                        for (int i = 0; i < bosses.Count; i++)
                        {
                            if (bosses[i].CurrentHealth <= 0)
                            {
                                bosses[i].Rect = Rectangle.Empty;
                            }
                        }

                        // move player normally if not attacking
                        if (!player.Attack(kState) && !player.IsDelivering && !player.IsAttacking)
                        {
                            // disable attack box
                            player.AttackBox = Rectangle.Empty;
                            playerAttackAnimationOffset = (int)(gameTime.TotalGameTime.TotalMilliseconds / timePerPlayerAttackFrame);

                            // handling input to move player
                            if (!player.IsClimbing)
                            {
                                player.Move(kState, levelWidth, 356, GraphicsDevice.Viewport.Height - 120, buildings);
                            }

                            // If the player is colliding with the right hitbox, enable climbing
                            bool canClimb = false;
                            Rectangle ladderRect = new Rectangle();
                            Rectangle collision = new Rectangle(player.Rect.X, player.Rect.Y, player.Rect.Width, player.Rect.Height + 2);
                            for(int i = 0; i < buildings.Count && !canClimb && !player.IsUp; i++)
                            {
                                for (int ladder = 1; buildings[i].Hitboxes.ContainsKey("ladder" + ladder.ToString()) && !canClimb; ladder++)
                                {
                                    if (collision.Intersects(buildings[i].Hitboxes["ladder" + ladder.ToString()]))
                                    {
                                        canClimb = true;
                                        ladderRect = buildings[i].Hitboxes["ladder" + ladder.ToString()];
                                    }
                                }
                            }
                            if (canClimb)
                            {
                                player.Climb(kState, ladderRect);
                                if (player.IsClimbing == true && (kState.IsKeyDown(Keys.W) || kState.IsKeyDown(Keys.S)))
                                {
                                    playerClimbFrame = playerClimbFramesElapsed % numPlayerClimbFrames + 1;
                                }
                            }
                            else
                            {
                                player.IsClimbing = false;
                            }

                            // Delivery!
                            Rectangle doorRect = new Rectangle();
                            for(int i = 0; i < buildings.Count && !player.IsUp; i++)
                            {
                                for(int door = 1; buildings[i].Hitboxes.ContainsKey("door" + door.ToString()); door++)
                                {
                                    if (player.Rect.Intersects(buildings[i].Hitboxes["door1"]) && !buildings[i].HasPizza)
                                    {
                                        doorRect = buildings[i].Hitboxes["door1"];
                                        if(player.Deliver(kState, doorRect))
                                        {
                                            player.IsDelivering = true;
                                            playerDeliveryAnimationOffset = (int)(gameTime.TotalGameTime.TotalMilliseconds / timePerPlayerDeliveryFrame);
                                            buildings[i].HasPizza = true;
                                            buildingsLeft = false;
                                            for(int k = 0; k < currentBuildings; k++)
                                            {
                                                if(!buildings[k].HasPizza)
                                                {
                                                    buildingsLeft = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (!buildingsLeft && !player.IsDelivering)
                            {
                                isLoading = true;
                                currentLevel++;
                                // If we've reached the end of the week, it's time to loop!
                                if (currentLevel > 5)
                                {
                                    currentLevel = 1;
                                    loop++;
                                    if(loop == 3)
                                    {
                                        zombieSpeed++;
                                        bossSpeed++;
                                    }
                                    zombieHealth += loop % 2;
                                    bossHealth += loop % 2;
                                    if (zombieHealth > maxZombieHealth)
                                    {
                                        zombieHealth = maxZombieHealth;
                                    }
                                    if (bossHealth > maxBossHealth)
                                    {
                                        bossHealth = maxBossHealth;
                                    }
                                }
                                gState = GameState.Loading;
                            }

                            // animating player

                            switch (player.Dir)
                            {
                                // when the player is standing still, only frame 0 (standing idle) gets drawn
                                case Direction.FaceLeft:
                                    playerFrame = 0;
                                    break;
                                case Direction.FaceRight:
                                    playerFrame = 0;
                                    break;
                                // otherwise, loop through the walk cycle
                                default:
                                    playerFrame = playerFramesElapsed % numPlayerFrames + 1;
                                    break;
                            }                       
                        }
                        else if(!player.IsDelivering)
                        {
                            // If the player is attacking, update their animation frame each frame
                            if(player.IsAttacking)
                            {
                                playerAttackFrame = (playerAttackFramesElapsed - playerAttackAnimationOffset) % (numPlayerAttackFrames + 2);
                            }
                            // If the last frame is reached, the attack is over.
                            if(playerAttackFrame == numPlayerAttackFrames + 1)
                            {
                                player.IsAttacking = false;
                            }
                        }
                        else if(player.IsDelivering)
                        {
                            // animate the delivery
                            playerDeliveryFrame = (playerDeliveryFramesElapsed - playerAttackAnimationOffset) % (numPlayerDeliveryFrames + 2);
                            if(playerDeliveryFrame == numPlayerDeliveryFrames + 1)
                            {
                                player.IsDelivering = false;
                                playerDeliveryFrame = 0;

                                // player earns 100 points for delivery
                                currentScore += 100;
                            }
                        }

                        // moving and animating the zombie
                        for (int i = 0; i < zombies.Count; i++)
                        {
                            zombies[i].Move(levelWidth);
                        }
                        zombieFrame = zombieFramesElapsed % numZombieFrames + 1;

                        // and the bosses
                        for (int i = 0; i < bosses.Count; i++)
                        {
                            bosses[i].Move(levelWidth);
                        }
                        bossFrame = bossFramesElapsed % numBossFrames + 1;

                        // decrement the timer
                        timer--;
                    }

                    

                    // Update screen position
                    screen = new Rectangle(player.Rect.X - (screen.Width / 2), 0, screen.Width, screen.Height);
                    if(screen.X < 0)
                    {
                        screen = new Rectangle(0, 0, screen.Width, screen.Height);
                    }
                    else if(screen.X + screen.Width > levelWidth)
                    {
                        screen = new Rectangle(levelWidth - screen.Width, 0, screen.Width, screen.Height);
                    }

                    //set up the indicators
                    charStandee.X = (indBar.X + ((indBar.Width * player.Rect.X) / levelWidth));

                    break;

                case GameState.GameOver:

                    //hi score stuff
                    kState = Keyboard.GetState();
                    char temp = GetLetter();
                    initials[currentLetter] = temp;
                    if (!isWrittem)
                    {
                        myScorePos = WriteScores();
                    }
                    

                    // user can return to menu by hitting "enter"
                    if(kState.IsKeyDown(Keys.Enter) && kStatePrev.IsKeyUp(Keys.Enter))
                    {
                        StreamWriter scoreWriter = new StreamWriter("Content/scores.txt");
                        if (myScorePos != -1)
                        {
                            names[myScorePos] = initials[0].ToString() + initials[1].ToString() + initials[2].ToString();
                        }
                        for (int l = 0; l < scores.Length; l++)
                        {
                            scoreWriter.WriteLine(names[l] + " " + scores[l]);
                        }
                        scoreWriter.Close();
                        gState = GameState.Menu;
                        timer = 0;
                    }
                    kStatePrev = kState;
                    break;

                case GameState.Loading:
                    LoadLevel(gameTime.ElapsedGameTime.Milliseconds);

                    //animating el pizza
                    pizzaFramesElapsed = (int)(gameTime.TotalGameTime.TotalMilliseconds / timePerPizzaFrame);
                    pizzaFrame = pizzaFramesElapsed % (numPizzaFrames + 1);

                    break;
            }

            // setting the previous game state
            gStatePrev = gState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

                switch (gState)
                {
                    case GameState.Menu:

                    // drawing menu graphic
                    spriteBatch.Draw(background, new Rectangle(0, 0, 800, 450), Color.White);
                    break;

                    case GameState.Game:

                    // draw the in-game backdrop
                    if ((int)(timer / 3600) == 1 || (int)((timer / 60) % 60) >= 50)
                    {
                        spriteBatch.Draw(sky, new Rectangle(0, 0, 800, 450), new Rectangle(0, 0, 800, 450), Color.White);
                    }
                    else if ((int)((timer / 60) % 60) < 50)
                    {
                        spriteBatch.Draw(sky, new Rectangle(0, 0, 800, 450), new Rectangle(0, 750 - (int)(15*((timer / 60) % 60)), 800, 450), Color.White);
                    }

                    spriteBatch.Draw(backdrop, new Rectangle(0, 0, 800, 450), new Rectangle(0, 0, 800, 450), Color.White);

                    // format time to display in minutes and seconds
                    int minutes = (int)(timer / 3600);
                    int seconds = (int)((timer / 60) % 60);
                    string timeDisplay = "Time: " + minutes + ":" + seconds;
                    if(seconds < 10)
                    {
                        timeDisplay = "Time: " + minutes + ":0" + seconds;
                    }

                    //draw indicators
                    spriteBatch.Draw(indicatorBar, indBar, Color.White);
                    spriteBatch.Draw(playerStandee, charStandee, Color.White);
                    for(int i = 0; i < currentBuildings; i++)
                    {
                        if (!buildings[i].HasPizza)
                        {
                            spriteBatch.Draw(delivery, indicator[i], Color.White);
                        }
                    }

                    // draw buildings
                    for (int i = 0; i < buildings.Count; i++)
                    {
                        if (buildings[i].HasPizza)
                        {
                            spriteBatch.Draw(buildings[i].Image, new Rectangle(buildings[i].Rect.X - screen.X, buildings[i].Rect.Y, buildings[i].Rect.Width, buildings[i].Rect.Height), Color.White);
                        }
                        else
                        {
                            spriteBatch.Draw(buildings[i].Image, new Rectangle(buildings[i].Rect.X - screen.X, buildings[i].Rect.Y, buildings[i].Rect.Width, buildings[i].Rect.Height), Color.White);
                            spriteBatch.Draw(delivery2, new Rectangle(buildings[i].Hitboxes["door1"].X - 4 - screen.X, buildings[i].Hitboxes["door1"].Y - 60, 42, 38), Color.White);
                        }
                    }

                    // draw the UI (player's health, the timer, and the zombie's health)
                    spriteBatch.DrawString(testFont, "Player health: " + player.CurrentHealth, new Vector2(0, 0), Color.Black);
                    spriteBatch.DrawString(testFont, "Day " + currentLevel.ToString() + " Week " + loop.ToString(), new Vector2(0, 32), Color.Black);
                    spriteBatch.DrawString(testFont, "Spare Lives: " + player.Lives.ToString(), new Vector2(0, 64), Color.Black);
                    spriteBatch.DrawString(testFont, timeDisplay, new Vector2(325, 0), Color.Black);
                    spriteBatch.DrawString(testFont, "Score: " + currentScore.ToString(), new Vector2(GraphicsDevice.Viewport.Width - 200, 0), Color.Black);

                    // drawing the objects. when a collision occurs, both the player and the zombie turn red
                    // testing if player is colliding with the zombie on the current frame
                    for (int i = 0; i < zombies.Count; i++)
                    {
                        // when attack box collides with zombie, turn zombie red
                        if (player.AttackBox.Intersects(zombies[i].Rect))
                        {
                            zombies[i].Color = Color.Red;
                        }
                        else
                        {
                            zombies[i].Color = Color.White;
                        }
                    }


                    // draw the attack box (for debugging purposes, TO BE REMOVED IN FINAL GAME)
                    /*if(player.IsAttacking && !player.IsDelivering)
                    {
                        spriteBatch.Draw(gameover, new Rectangle(player.AttackBox.X - screen.X, player.AttackBox.Y, player.AttackBox.Width, player.AttackBox.Height), Color.White);
                    }
                    // draw feet collision box (ALSO FOR DEBUGGING ONLY)
                    Rectangle collision = new Rectangle(player.Rect.X + 10 - screen.X, player.Rect.Y + 40, 14, 8); ;

                    if (player.Dir == Direction.FaceLeft || player.Dir == Direction.MoveLeft)
                    {
                        collision = new Rectangle(player.Rect.X + 6 - screen.X, player.Rect.Y + 40, 14, 8);
                    }
                    
                    spriteBatch.Draw(gameover, collision, Color.White);*/

                    // drawing the player
                    if (player.Invincible == 0 || player.Invincible % 30 <= 15)
                    {
                        if (player.CurrentHealth > 0)
                        {
                            //draws the player's attack
                            if (player.IsAttacking && !player.IsDelivering)
                            {
                                if (player.Dir == Direction.FaceLeft || player.Dir == Direction.MoveLeft)
                                {
                                    spriteBatch.Draw(playerAttack, new Vector2((player.Rect.X - 12) - screen.X, player.Rect.Y), new Rectangle(playerAttackFrame * PLAYER_ATTACK_WIDTH, 0, PLAYER_ATTACK_WIDTH, PLAYER_ATTACK_HEIGHT), player.Color, 0, Vector2.Zero, 1,
                                        SpriteEffects.FlipHorizontally, 0);
                                }
                                else if (player.Dir == Direction.FaceRight || player.Dir == Direction.MoveRight)
                                {
                                    spriteBatch.Draw(playerAttack, new Vector2(player.Rect.X - screen.X, player.Rect.Y), new Rectangle(playerAttackFrame * PLAYER_ATTACK_WIDTH, 0, PLAYER_ATTACK_WIDTH, PLAYER_ATTACK_HEIGHT), player.Color);
                                }
                            }
                            // draw the delivery
                            else if (player.IsDelivering)
                            {
                                if (player.Dir == Direction.FaceLeft || player.Dir == Direction.MoveLeft)
                                {
                                    spriteBatch.Draw(playerDeliver, new Vector2((player.Rect.X - 12) - screen.X, player.Rect.Y), new Rectangle(playerDeliveryFrame * PLAYER_DELIVERY_WIDTH, 0, PLAYER_DELIVERY_WIDTH, PLAYER_DELIVERY_HEIGHT), player.Color, 0, Vector2.Zero, 1,
                                        SpriteEffects.FlipHorizontally, 0);
                                }
                                else if (player.Dir == Direction.FaceRight || player.Dir == Direction.MoveRight)
                                {
                                    spriteBatch.Draw(playerDeliver, new Vector2(player.Rect.X - screen.X, player.Rect.Y), new Rectangle(playerDeliveryFrame * PLAYER_DELIVERY_WIDTH, 0, PLAYER_DELIVERY_WIDTH, PLAYER_DELIVERY_HEIGHT), player.Color);
                                }
                            }
                            // draw the player moving
                            else
                            {
                                if (player.IsClimbing == false)
                                {
                                    if (player.Dir == Direction.FaceLeft || player.Dir == Direction.MoveLeft)
                                    {
                                        spriteBatch.Draw(player.Image, new Vector2(player.Rect.X - screen.X, player.Rect.Y), new Rectangle(playerFrame * PLAYER_WIDTH, 0, PLAYER_WIDTH, PLAYER_HEIGHT), player.Color, 0, Vector2.Zero, 1,
                                            SpriteEffects.FlipHorizontally, 0);
                                    }
                                    else if (player.Dir == Direction.FaceRight || player.Dir == Direction.MoveRight)
                                    {
                                        spriteBatch.Draw(player.Image, new Vector2(player.Rect.X - screen.X, player.Rect.Y), new Rectangle(playerFrame * PLAYER_WIDTH, 0, PLAYER_WIDTH, PLAYER_HEIGHT), player.Color);
                                    }
                                }
                                else
                                {
                                    spriteBatch.Draw(playerClimb, new Vector2(player.Rect.X - screen.X, player.Rect.Y), new Rectangle(playerClimbFrame * PLAYER_CLIMB_WIDTH, 0, PLAYER_CLIMB_WIDTH, PLAYER_CLIMB_HEIGHT), player.Color, 0, Vector2.Zero, 1,
                                            SpriteEffects.FlipHorizontally, 0);
                                }
                            }
                        }
                    }

                    // draw the zombies
                    for (int i = 0; i < zombies.Count; i++)
                    {
                        if (zombies[i].CurrentHealth > 0)
                        {
                            if (zombies[i].Dir == Direction.MoveLeft)
                            {

                                if (!zombies[i].isFalling)
                                {
                                    spriteBatch.Draw(zombies[i].Image, new Vector2(zombies[i].Rect.X - screen.X, zombies[i].Rect.Y), new Rectangle(zombieFrame * ZOMBIE_WIDTH, 0, ZOMBIE_WIDTH, ZOMBIE_HEIGHT), zombies[i].Color, 0, Vector2.Zero, 1,
                                        SpriteEffects.FlipHorizontally, 0);
                                }
                                else
                                {
                                    spriteBatch.Draw(zombies[i].Image, new Vector2(zombies[i].Rect.X - screen.X, zombies[i].Rect.Y), new Rectangle(6 * ZOMBIE_WIDTH, 0, ZOMBIE_WIDTH, ZOMBIE_HEIGHT), zombies[i].Color);
                                }
                            }
                            else if (zombies[i].Dir == Direction.MoveRight)
                            {
                                if (!zombies[i].isFalling)
                                {
                                    spriteBatch.Draw(zombies[i].Image, new Vector2(zombies[i].Rect.X - screen.X, zombies[i].Rect.Y), new Rectangle(zombieFrame * ZOMBIE_WIDTH, 0, ZOMBIE_WIDTH, ZOMBIE_HEIGHT), zombies[i].Color);
                                }
                                else
                                {
                                    spriteBatch.Draw(zombies[i].Image, new Vector2(zombies[i].Rect.X - screen.X, zombies[i].Rect.Y), new Rectangle(6 * ZOMBIE_WIDTH, 0, ZOMBIE_WIDTH, ZOMBIE_HEIGHT), zombies[i].Color, 0, Vector2.Zero, 1,
                                    SpriteEffects.FlipHorizontally, 0);
                                }
                                spriteBatch.Draw(zombies[i].Image, new Vector2(zombies[i].Rect.X - screen.X, zombies[i].Rect.Y), new Rectangle(zombieFrame * ZOMBIE_WIDTH, 0, ZOMBIE_WIDTH, ZOMBIE_HEIGHT), zombies[i].Color);
                            }

                        }
                    }

                    for (int i = 0; i < bosses.Count; i++)
                    {
                        if (bosses[i].CurrentHealth > 0)
                        {
                            if (bosses[i].Dir == Direction.MoveLeft)
                            {
                                spriteBatch.Draw(bZombie, new Vector2(bosses[i].Rect.X - screen.X, bosses[i].Rect.Y), new Rectangle(bossFrame * BOSS_WIDTH, 0, BOSS_WIDTH, BOSS_HEIGHT), bosses[i].Color, 0, Vector2.Zero, 1,
                                SpriteEffects.FlipHorizontally, 0);
                            }
                            else if (bosses[i].Dir == Direction.MoveRight)
                            {
                                spriteBatch.Draw(bZombie, new Vector2(bosses[i].Rect.X - screen.X, bosses[i].Rect.Y), new Rectangle(bossFrame * BOSS_WIDTH, 0, BOSS_WIDTH, BOSS_HEIGHT), bosses[i].Color);
                            }
                        }
                    }

                    // draw the pause screen
                    if (isPaused)
                    {
                        spriteBatch.Draw(pause, new Rectangle(0, 0, 800, 450), Color.White);
                    }

                    break;

                    case GameState.GameOver:
                    
                    // drawing the game over screen and prompting player to try again
                    spriteBatch.Draw(gameover, new Rectangle(0, 0, 800, 450), Color.White);
                    bool myScore = false;
                    for (int i = 0; i < scores.Length; i++ )
                    {
                        if (scores[i] == currentScore && !myScore)
                        {
                            for (int k = 0; k < initials.Length; k++)
                            {
                                if (k == currentLetter)
                                {
                                    spriteBatch.DrawString(testFont, initials[k].ToString(), new Vector2((GraphicsDevice.Viewport.Width / 2) - 64 + (k * 18), GraphicsDevice.Viewport.Height / 2 + (i * 32)), Color.Red);
                                }
                                else
                                {
                                    spriteBatch.DrawString(testFont, initials[k].ToString(), new Vector2((GraphicsDevice.Viewport.Width / 2) - 64 + (k * 18), GraphicsDevice.Viewport.Height / 2 + (i * 32)), Color.White);
                                }
                            }
                            myScore = true;
                        }
                        else
                        {
                            spriteBatch.DrawString(testFont, names[i], new Vector2((GraphicsDevice.Viewport.Width / 2) - 64, GraphicsDevice.Viewport.Height / 2 + (i * 32)), Color.White);
                        }
                        spriteBatch.DrawString(testFont, scores[i].ToString(), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 + (i * 32)), Color.White);
                    }
                        break;
                
                //draws the pizza
                case GameState.Loading:
                    GraphicsDevice.Clear(Color.Black);
                    spriteBatch.DrawString(testFont, "Dawn of Day " + currentLevel.ToString(), new Vector2(336, 128), Color.White);
                    spriteBatch.DrawString(testFont, "Week " + loop.ToString(), new Vector2(364, 160), Color.White);
                    spriteBatch.Draw(pizza, new Vector2(GraphicsDevice.Viewport.Width / 2 - PIZZA_WIDTH / 2, GraphicsDevice.Viewport.Height / 2 - PIZZA_HEIGHT / 2), new Rectangle(pizzaFrame * PIZZA_WIDTH, 0, PIZZA_WIDTH, PIZZA_HEIGHT), Color.White);
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void LoadLevel(int time)
        {
            //sets the pizza's position
            pizzaPos = new Vector2((800 / 2) - (PIZZA_WIDTH / 2), (450 / 2) - (PIZZA_HEIGHT / 2));

            if (isLoading)
            {
                elTimer += time;
            }
            if (elTimer >= 4000)
            {
                isLoading = false;
                elTimer = 0;
                gState = GameState.Game;
                player.IsDelivering = false;
                player.AllDelivered = true;
                //sets the game to a loading state
                isLoading = true;
                // Time to load the new level
                buildingsLeft = true;
                timer = maxTime - ((loop - 1) * 1200);
                if (timer < minTime)
                {
                    timer = minTime;
                }
                levelData.Clear();
                buildings.Clear();
                zombies.Clear();
                bosses.Clear();
                indicator.Clear();
                levelData = reader.readIn("Content/Levels/level" + currentLevel.ToString() + ".dat");
                levelWidth = levelData[0] * 2;
                levelData.RemoveAt(0);
                levelRects = reader.makeRect(levelData);
                int currentZombies = 0;
                int currentBosses = 0;
                currentBuildings = 0;
                for (int l = 0; l < levelData.Count; l += 3)
                {
                    switch (levelData[l])
                    {
                        case 0:
                            if (currentBuildings < buildings.Count)
                            {
                                buildings[currentBuildings] = new Building(0, levelRects[l / 3], building1);
                                buildings[currentBuildings].SetHitboxes();
                                indicator.Add(new Rectangle(indBar.X + ((indBar.Width * buildings[currentBuildings].Hitboxes["door1"].X) / levelWidth) - 8, 45, 22, 20));
                                currentBuildings++;
                            }
                            else
                            {
                                buildings.Add(new Building(0, levelRects[l / 3], building1));
                                buildings[currentBuildings].SetHitboxes();
                                indicator.Add(new Rectangle(indBar.X + ((indBar.Width * buildings[currentBuildings].Hitboxes["door1"].X) / levelWidth) - 8, 45, 22, 20));
                                currentBuildings++;
                            }
                            break;
                        case 1:
                            if (currentBuildings < buildings.Count)
                            {
                                buildings[currentBuildings] = new Building(1, levelRects[l / 3], building2);
                                buildings[currentBuildings].SetHitboxes();
                                indicator.Add(new Rectangle(indBar.X + ((indBar.Width * buildings[currentBuildings].Hitboxes["door1"].X) / levelWidth) - 8, 45, 22, 20));
                                currentBuildings++;
                            }
                            else
                            {
                                buildings.Add(new Building(1, levelRects[l / 3], building2));
                                buildings[currentBuildings].SetHitboxes();
                                indicator.Add(new Rectangle(indBar.X + ((indBar.Width * buildings[currentBuildings].Hitboxes["door1"].X) / levelWidth) - 8, 45, 22, 20));
                                currentBuildings++;
                            }
                            break;
                        case 2:
                            player = new Player(player.Image, new Rectangle(levelRects[l / 3].X, levelRects[l / 3].Y, PLAYER_WIDTH, PLAYER_HEIGHT), player.TotalHealth, player.Lives);
                            break;
                        case 3:
                            if (currentZombies < zombies.Count)
                            {
                                zombies[currentZombies] = new Zombie(player, zombie1, levelRects[l / 3], zombieHealth, zombieSpeed);
                                currentZombies++;
                            }
                            else
                            {
                                zombies.Add(new Zombie(player, zombie1, levelRects[l / 3], zombieHealth, zombieSpeed));
                                currentZombies++;
                            }
                            if (zombies[currentZombies - 1].Rect.Y != 360)
                            {
                                zombies[currentZombies - 1].Rect = new Rectangle(zombies[currentZombies - 1].Rect.X, 360, zombies[currentZombies - 1].Rect.Width, zombies[currentZombies - 1].Rect.Height);
                            }
                            break;
                        case 4:
                            if (currentZombies < zombies.Count)
                            {
                                zombies[currentZombies] = new Zombie(player, zombie2, levelRects[l / 3], zombieHealth, zombieSpeed);
                                currentZombies++;
                            }
                            else
                            {
                                zombies.Add(new Zombie(player, zombie2, levelRects[l / 3], zombieHealth, zombieSpeed));
                                currentZombies++;
                            }
                            if (zombies[currentZombies - 1].Rect.Y != 360)
                            {
                                zombies[currentZombies - 1].Rect = new Rectangle(zombies[currentZombies - 1].Rect.X, 360, zombies[currentZombies - 1].Rect.Width, zombies[currentZombies - 1].Rect.Height);
                            }
                            break;
                        case 5:
                            if (currentBuildings < buildings.Count)
                            {
                                buildings[currentBuildings] = new Building(2, levelRects[l / 3], building3);
                                buildings[currentBuildings].SetHitboxes();
                                indicator.Add(new Rectangle(indBar.X + ((indBar.Width * buildings[currentBuildings].Hitboxes["door1"].X) / levelWidth) - 8, 45, 22, 20));
                                currentBuildings++;
                            }
                            else
                            {
                                buildings.Add(new Building(2, levelRects[l / 3], building3));
                                buildings[currentBuildings].SetHitboxes();
                                indicator.Add(new Rectangle(indBar.X + ((indBar.Width * buildings[currentBuildings].Hitboxes["door1"].X) / levelWidth) - 8, 45, 22, 20));
                                currentBuildings++;
                            }
                            break;
                        case 6:
                            if (currentBuildings < buildings.Count)
                            {
                                buildings[currentBuildings] = new Building(3, levelRects[l / 3], building4);
                                buildings[currentBuildings].SetHitboxes();
                                indicator.Add(new Rectangle(indBar.X + ((indBar.Width * buildings[currentBuildings].Hitboxes["door1"].X) / levelWidth) - 8, 45, 22, 20));
                                currentBuildings++;
                            }
                            else
                            {
                                buildings.Add(new Building(3, levelRects[l / 3], building4));
                                buildings[currentBuildings].SetHitboxes();
                                indicator.Add(new Rectangle(indBar.X + ((indBar.Width * buildings[currentBuildings].Hitboxes["door1"].X) / levelWidth) - 8, 45, 22, 20));
                                currentBuildings++;
                            }
                            break;
                        case 7:
                            if (currentBuildings < buildings.Count)
                            {
                                buildings[currentBuildings] = new Building(4, levelRects[l / 3], building5);
                                buildings[currentBuildings].SetHitboxes();
                                indicator.Add(new Rectangle(indBar.X + ((indBar.Width * buildings[currentBuildings].Hitboxes["door1"].X) / levelWidth) - 8, 45, 22, 20));
                                currentBuildings++;
                            }
                            else
                            {
                                buildings.Add(new Building(4, levelRects[l / 3], building5));
                                buildings[currentBuildings].SetHitboxes();
                                indicator.Add(new Rectangle(indBar.X + ((indBar.Width * buildings[currentBuildings].Hitboxes["door1"].X) / levelWidth) - 8, 45, 22, 20));
                                currentBuildings++;
                            }
                            break;
                        case 8:
                            if (currentBuildings < buildings.Count)
                            {
                                buildings[currentBuildings] = new Building(5, levelRects[l / 3], building6);
                                buildings[currentBuildings].SetHitboxes();
                                indicator.Add(new Rectangle(indBar.X + ((indBar.Width * buildings[currentBuildings].Hitboxes["door1"].X) / levelWidth) - 8, 45, 22, 20));
                                currentBuildings++;
                            }
                            else
                            {
                                buildings.Add(new Building(5, levelRects[l / 3], building6));
                                buildings[currentBuildings].SetHitboxes();
                                indicator.Add(new Rectangle(indBar.X + ((indBar.Width * buildings[currentBuildings].Hitboxes["door1"].X) / levelWidth) - 8, 45, 22, 20));
                                currentBuildings++;
                            }
                            break;
                        case 9:
                            bosses.Add(new BossZombie(player, bZombie, levelRects[l / 3], bossHealth, bossSpeed));
                            currentBosses++;
                            if (bosses[currentBosses - 1].Rect.Y + bosses[currentBosses - 1].Rect.Height != 402)
                            {
                                bosses[currentBosses - 1].Rect = new Rectangle(bosses[currentBosses - 1].Rect.X, 402 - bosses[currentBosses - 1].Rect.Height,
                                    bosses[currentBosses - 1].Rect.Height, bosses[currentBosses - 1].Rect.Width);
                            }
                            break;
                    }
                }
                // break here
            }
        }

        public void LoadScores()
        {
            StreamReader scoreReader = new StreamReader("Content/scores.txt");
            
            string currentLine = "";
            int count = 0;

            while((currentLine = scoreReader.ReadLine()) != null)
            {
                names[count] = currentLine[0].ToString() + currentLine[1].ToString() + currentLine[2].ToString();
                currentLine = currentLine.Substring(3);
                currentLine.Trim();
                scores[count] = int.Parse(currentLine);
                count++;
            }

            scoreReader.Close();

            //isLoaded = true;
        }

        public int WriteScores()
        {
            int myScorePos = -1;

            for (int i = 0; i < scores.Length; i++)
            {
                if (currentScore >= scores[i])
                {
                    int[] temp = new int[scores.Length];
                    string[] tempName = new string[names.Length];
                    for (int k = 0; k < i; k++)
                    {
                        temp[k] = scores[k];
                        tempName[k] = names[k];
                    }

                    for (int j = i + 1; j < temp.Length; j++)
                    {
                        temp[j] = scores[j - 1];
                        tempName[j] = names[j - 1];
                    }
                    temp[i] = currentScore;
                    tempName[i] = initials[0].ToString() + initials[1].ToString() + initials[2].ToString();
                    myScorePos = i;
                    scores = temp;
                    names = tempName;
                    break;
                }
            }

            isWrittem = true;
            return myScorePos;
        }

        //reusing single key press for the hi score table
        public bool SingleKeyPress(Keys key)
        {
            // only true if it's the first frame in which the param key is pressed
            if (kState.IsKeyDown(key) && kStatePrev.IsKeyUp(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// scrolls through the alphabet so that the user can enter their name, returns the letter that the player has selected
        /// </summary>
        public char GetLetter()
        {
            //will start at the lowest index of the list, and add to that as the down key is pressed
            if(SingleKeyPress(Keys.Down))
            {
                index[currentLetter]++;
                if(index[currentLetter] == alphabet.Count)
                {
                    index[currentLetter] = 0;
                }
            }
            if(SingleKeyPress(Keys.Up))
            {
                index[currentLetter]--;
                if(index[currentLetter] < 0)
                {
                    index[currentLetter] = alphabet.Count - 1;
                }
            }
            if(SingleKeyPress(Keys.Left))
            {
                currentLetter--;
                if(currentLetter < 0)
                {
                    currentLetter = 2;
                }
            }
            if(SingleKeyPress(Keys.Right))
            {
                currentLetter++;
                if(currentLetter > 2)
                {
                    currentLetter = 0;
                }
            }

            return alphabet[index[currentLetter]];
        }
    }
}

