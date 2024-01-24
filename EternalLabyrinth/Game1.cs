using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace EternalLabyrinth
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //Game universal variables
        public static readonly int FLOOR_HEIGHT = 578; // floor height for level
        public static int w, h; // screen w and h
        public static List<Object> entities; // Holds all objects on screen
        public static List<Keys> oldIn; // univarsal static List which holds old inputted key input
        public static List<Keys> presIn; // univarsal static List which holds newl inputted key input
        public static int Time;
        public static Random rand = new Random();

        public static Camera camera;
        public static Boolean pan;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteDraw;
        SpriteBatch UIElements;
        SpriteBatch screenEffects;

        //GameState Enums
        public enum GameState { START, LEVEL1, LEVEL2, LEVEL3, FINAL, END };
        public static GameState gState;

        //Textures (Loading still in development)
        public static Dictionary<T, Texture2D> textures;
        Texture2D textureBlank, crate, dirt, rock, stone, wall, back, grass, jail, cams, magmastone, TitleText;

        public static List<Rectangle> tileRects;
        public static Texture2D tileSheet;

        public static Texture2D white;

        //Level Loading (create functions)
        List<String> lines;
        Level level, backLevel;

        //input
        KeyboardState old = Keyboard.GetState(); // Used to initialize old key values
        MouseState oldm = Mouse.GetState();
        //Background
        Rectangle backgroundPosition; //Rectangle for the background
        int backgroundVelocity; //Speed of background movement
        Texture2D backgroundText; //Test background texture
        int rightEdge; //X-position of rightmost part of background

        //Player information
        int pX, pY;

        //UIElements
        public static SpriteFont font; //Main font 
        Texture2D startBackground, startForeground, startEffects; //Start screen background
        Rectangle startEffectControl;
        Color startColor = Color.White;
        double startEffectPos = h;
        int flickerVelocity = 1;
        Texture2D blockImage;
        Texture2D healthText;
        Rectangle healthRec;
        Rectangle healthBackRec;
        SpriteFont largerFont;
        Rectangle Title;
        Rectangle startbutton, leftbutton, rightbutton;
        Button start, quit, right;

        AttackAbility testAbility;
        DefenseAbility testShield;
        Texture2D fireText;
        Texture2D shieldText;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            // TODO: Add your initialization logic here

            //Setting Game Screen Dimaension
            graphics.PreferredBackBufferWidth = 120 * 32 / 3;
            graphics.PreferredBackBufferHeight = 20 * 32;
            graphics.ApplyChanges();

            graphics.ApplyChanges();

            w = GraphicsDevice.Viewport.Width;
            h = GraphicsDevice.Viewport.Height;

            Time = 0;

            //Initializing universal input vars
            oldIn = new List<Keys>();
            presIn = new List<Keys>();

            //level initialize
            tileRects = new List<Rectangle>();
            level = new Level();
            backLevel = new Level();
            level.tiles = new Tile[120, 20];
            backLevel.tiles = new Tile[120, 20];
            lines = new List<string>();

            InitializeTextures();

            //Initializing entities in entities List(ALLWAYS SET PLAYER TO INDEX 0)
            entities = new List<Object>();

            //Background rectangle intialization
            backgroundPosition = new Rectangle(0, 0, 140 * 32, 20 * 32);
            startEffectControl = new Rectangle(w / 2 - 375, h, 750, 1350);

            //Sets initial game state
            gState = GameState.START;
            rightEdge = -120 * 32;
            healthRec = new Rectangle(200, 50, 100, 40);
            healthBackRec = new Rectangle(200, 50, 100, 40);

            //Background rectangle intialization
            //backgroundPosition = new Rectangle(0, 0, 3000, 600);
            backgroundVelocity = 2;

            healthRec = new Rectangle(200, 50, 100, 40);
            healthBackRec = new Rectangle(200, 50, 100, 40);
            //Menu Buttons
            Title = new Rectangle(185, -20, 900, 695);
            startbutton = new Rectangle((w / 2) - 75, h / 2 + 200, 100, 50);
            leftbutton = new Rectangle((w / 2) - 75 - 300, h / 2 + 200, 100, 50);
            rightbutton = new Rectangle((w / 2) - 75 + 300, h / 2 + 200, 100, 50);
            start = new Button(startbutton, false, "Start");
            right = new Button(rightbutton, false, "Class");
            quit = new Button(leftbutton, false, "Quit");
            base.Initialize();
        }

        private void InitializeTextures()
        {
            textures = new Dictionary<T, Texture2D>();
            //Initializing textures (change to loading textures later)
            textures[T.Player] = this.Content.Load<Texture2D>("NORMPlayerSpriteSheet");
            textures[T.IceGlazier] = this.Content.Load<Texture2D>("CharacterSpriteSheets/IceAbilitySpriteSheet");
            textures[T.IceHold] = this.Content.Load<Texture2D>("CharacterSpriteSheets/IceHold");
            textures[T.ElectroCharge] = this.Content.Load<Texture2D>("CharacterSpriteSheets/LightningAbilitySpriteSheet");
            textures[T.LightningEffect] = this.Content.Load<Texture2D>("CharacterSpriteSheets/LightningEffectSpriteSheet");
            textures[T.RockWall] = this.Content.Load<Texture2D>("CharacterSpriteSheets/RockAbilitySpriteSheet");
            textures[T.FireBall] = this.Content.Load<Texture2D>("CharacterSpriteSheets/Fire");
            //text = this.Content.Load<Texture2D>("testSpriteSheetRight");
            textures[T.Skeltri] = this.Content.Load<Texture2D>("EnemySpriteSheets/Skeltri/SkeltriSpriteSheet");
            textures[T.SkeltriShard] = this.Content.Load<Texture2D>("EnemySpriteSheets/Skeltri/Abilities/IceShardAbility");
            textures[T.SkeltriStomp] = this.Content.Load<Texture2D>("EnemySpriteSheets/Skeltri/Abilities/IceStompAbility");

            textures[T.Flame] = this.Content.Load<Texture2D>("EnemySpriteSheets/Flame/FlameSpriteSheet");
            textures[T.FlamingBullet] = this.Content.Load<Texture2D>("EnemySpriteSheets/Flame/Abilities/FlamingBulletSpriteSheet");

            textures[T.Bouldren] = this.Content.Load<Texture2D>("EnemySpriteSheets/Bouldren/BouldrenSpriteSheet");

            textures[T.Skull] = this.Content.Load<Texture2D>("EnemySpriteSheets/Skull/SkullSpriteSheet");

            textures[T.Smauken] = this.Content.Load<Texture2D>("EnemySpriteSheets/Smauken/SmaukenSpriteSheet");
            textures[T.SmokeScreen] = this.Content.Load<Texture2D>("EnemySpriteSheets/Smauken/Abilities/SmokeScreenSpriteSheet");

            textures[T.Waterne] = this.Content.Load<Texture2D>("EnemySpriteSheets/Waterne/WaterneSpriteSheet");
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //Camera
            camera = new Camera(graphics.GraphicsDevice.Viewport);
            //Menu
            start.LoadContent(Content);
            right.LoadContent(Content);
            quit.LoadContent(Content);
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteDraw = new SpriteBatch(GraphicsDevice);
            UIElements = new SpriteBatch(GraphicsDevice);
            screenEffects = new SpriteBatch(GraphicsDevice);

            backgroundText = this.Content.Load<Texture2D>("CaveTexture");
            //backgroundText = this.Content.Load<Texture2D>("pxArt");
            // TODO: use this.Content to load your game content here

            font = this.Content.Load<SpriteFont>("font");
            textureBlank = Content.Load<Texture2D>("trans");

            //(turn into spritesheet for simplicity -> ask Paul)
            crate = Content.Load<Texture2D>("crate");
            dirt = Content.Load<Texture2D>("dirt");
            rock = Content.Load<Texture2D>("rock");
            stone = Content.Load<Texture2D>("stone");
            wall = Content.Load<Texture2D>("wall");
            back = Content.Load<Texture2D>("background");
            grass = Content.Load<Texture2D>("grass");
            jail = Content.Load<Texture2D>("bars");
            cams = Content.Load<Texture2D>("cams");
            fireText = this.Content.Load<Texture2D>("fir");
            shieldText = this.Content.Load<Texture2D>("shield");
            magmastone = Content.Load<Texture2D>("magmastone");
            tileSheet = Content.Load<Texture2D>("Tiles\\spritesheetTiles");

            //Preloads all rectangles for tiles

            for (int i = 0; i < tileSheet.Width; i += 32)
            {
                tileRects.Add(new Rectangle(i, 0, 32, 32));
            }

            white = Content.Load<Texture2D>("White");

            startBackground = this.Content.Load<Texture2D>("StartScreen/BackgroundLayer");
            startForeground = this.Content.Load<Texture2D>("StartScreen/ForegroundLayer");
            startEffects = this.Content.Load<Texture2D>("StartScreen/Flare");
            blockImage = this.Content.Load<Texture2D>("block");
            healthText = this.Content.Load<Texture2D>("rect");

            largerFont = this.Content.Load<SpriteFont>("largerFont");

            //LoadsLevel

            entities.Clear();

            Player player1 = new Player(textures[T.Player], new Rectangle(288, 256, 51, 63), Color.White, true);
            entities.Add(player1);

            testAbility = new AttackAbility(new Rectangle(0, FLOOR_HEIGHT - 50, 20, 20), Color.White, entities[0].isFacingRight, 2, 10);
            testShield = new DefenseAbility(shieldText, new Rectangle(0, FLOOR_HEIGHT - 50, 20, player1.rect.Height), Color.Green, entities[0].isFacingRight, 5, 0.5);
            //entities.Add(testAbility);
            //entities.Add(testShield);

            level = new Level();
            level.tiles = LoadLevel(level.tiles, true, "LevelMaps\\map1.txt");

            backLevel = new Level();
            backLevel.isBackground = true;
            backLevel.tiles = LoadLevel(backLevel.tiles, false, "LevelMaps\\backMap1.txt");
            //UI
            TitleText = Content.Load<Texture2D>("eternal");
            start.setTexture(magmastone);
            right.setTexture(magmastone);
            quit.setTexture(magmastone);

        }

        private Tile[,] LoadLevel(Tile[,] tiles, bool addEntities, string filename)
        {
            reset();


            tiles = new Tile[120, 20];
            lines = new List<string>();

            ReadFilesAsStrings(@"Content/" + filename);

            //loading levels (Ask Paul how it works) 
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    if (lines[j][i] != '.' && lines[j][i] != 'x')
                    {
                        int x = tiles.GetLength(0);
                        tiles[i, j] = new Tile(lines[j][i]);
                        tiles[i, j].rect = new Rectangle(i * 32, j * 32, 32, 32);
                        tiles[i, j].hitBox = tiles[i, j].rect;
                        switch (tiles[i, j].textString)
                        {
                            case "":
                                tiles[i, j].setRect(tileRects[25]);
                                break;
                            case "crate":
                                tiles[i, j].setRect(tileRects[4]);
                                break;
                            case "dirt":
                                tiles[i, j].setRect(tileRects[3]);
                                break;
                            case "jail":
                                tiles[i, j].setRect(tileRects[21]);
                                break;
                            case "cams":
                                tiles[i, j].setRect(tileRects[5]);
                                break;
                            case "grass":
                                tiles[i, j].setRect(tileRects[2]);
                                break;
                            case "rock":
                                tiles[i, j].setRect(tileRects[22]);
                                tiles[i, j].text = rock;
                                break;
                            case "stone":
                                tiles[i, j].setRect(tileRects[1]);
                                break;
                            case "wall":
                                tiles[i, j].setRect(tileRects[0]);
                                break;
                            case "player":
                                //if(gState == GameState.LEVEL1 || gState == GameState.START)
                                //    tiles[i, j].setRect(tileRects[21]);
                                //else
                                //    tiles[i, j].setRect(tileRects[25]);
                                tiles[i, j].setRect(tileRects[25]);
                                pX = tiles[i, j].rect.X;
                                pY = tiles[i, j].rect.Y + 65;
                                break;
                            case "skeltri":
                                Skeltri tempSkeltri = new Skeltri(tiles[i, j].rect.X, tiles[i, j].rect.Y + 50, true);
                                tiles[i, j].setRect(tileRects[25]);
                                entities.Add(tempSkeltri);
                                break;
                            case "bouldren":
                                Bouldren tempBouldren = new Bouldren(tiles[i, j].rect.X, tiles[i, j].rect.Y + 50, true);
                                tiles[i, j].setRect(tileRects[25]);
                                entities.Add(tempBouldren);
                                break;
                            case "flame":
                                Flame tempFlame = new Flame(tiles[i, j].rect.X, tiles[i, j].rect.Y + 50, true);
                                tiles[i, j].setRect(tileRects[25]);
                                entities.Add(tempFlame);
                                break;
                            case "skull":
                                Skull tempSkull = new Skull(tiles[i, j].rect.X, tiles[i, j].rect.Y);
                                tiles[i, j].setRect(tileRects[25]);
                                entities.Add(tempSkull);
                                break;
                            case "smauken":
                                Smauken tempSmauken = new Smauken(tiles[i, j].rect.X, tiles[i, j].rect.Y + 50, true);
                                tiles[i, j].setRect(tileRects[25]);
                                entities.Add(tempSmauken);
                                break;
                            case "waterne":
                                Waterne tempWaterne = new Waterne(tiles[i, j].rect.X, tiles[i, j].rect.Y + 50, true);
                                tiles[i, j].setRect(tileRects[25]);
                                entities.Add(tempWaterne);
                                break;
                            case "death":
                                tiles[i, j].setRect(tileRects[25]);
                                break;
                            case "nextLevel":
                                tiles[i, j].setRect(tileRects[25]);
                                break;
                            case "vine1":
                                tiles[i, j].setRect(tileRects[12]);
                                break;
                            case "vine2":
                                tiles[i, j].setRect(tileRects[13]);
                                break;
                            case "blueDirtVine":
                                tiles[i, j].setRect(tileRects[8]);
                                break;
                            case "blueStoneVine":
                                tiles[i, j].setRect(tileRects[19]);
                                break;
                            case "bluePattWall":
                                tiles[i, j].setRect(tileRects[9]);
                                break;
                            case "blueCrate":
                                tiles[i, j].setRect(tileRects[7]);
                                break;
                            case "blueStoneWall":
                                tiles[i, j].setRect(tileRects[10]);
                                break;
                            case "blueSpike":
                                tiles[i, j].setRect(tileRects[14]);
                                tiles[i, j].text = rock;
                                break;
                            case "redStoneWall":
                                tiles[i, j].setRect(tileRects[16]);
                                break;
                            case "redCrate":
                                tiles[i, j].setRect(tileRects[11]);
                                break;
                            case "redStoneVine":
                                tiles[i, j].setRect(tileRects[17]);
                                break;
                            case "redSpike":
                                tiles[i, j].setRect(tileRects[15]);
                                tiles[i, j].text = rock;
                                break;
                            case "yellowDirt":
                                tiles[i, j].setRect(tileRects[20]);
                                break;
                            case "yellowSpike":
                                tiles[i, j].setRect(tileRects[6]);
                                tiles[i, j].text = rock;
                                break;
                            case "yellowStoneWall":
                                tiles[i, j].setRect(tileRects[26]);
                                break;
                            case "yellowTree":
                                tiles[i, j].setRect(tileRects[33]);
                                break;
                            case "stalactite":
                                tiles[i, j].setRect(tileRects[32]);
                                break;
                            case "stalagmite":
                                tiles[i, j].setRect(tileRects[28]);
                                break;
                            case "stalactiteBlue":
                                tiles[i, j].setRect(tileRects[31]);
                                break;
                            case "stalagmiteBlue":
                                tiles[i, j].setRect(tileRects[29]);
                                break;
                            case "waterHalf":
                                tiles[i, j].setRect(tileRects[27]);
                                break;
                            case "waterFull":
                                tiles[i, j].setRect(tileRects[30]);
                                break;
                        }
                        if (addEntities)
                            entities.Add(tiles[i, j]);
                    }
                    else
                    {
                        if (j == 0 && i == 0)
                        {
                            tiles[i, j] = new Tile(lines[j][i]);
                            tiles[i, j].rect = new Rectangle(i * 32, j * 32, 32, 32);
                            tiles[i, j].hitBox = tiles[i, j].rect;
                            tiles[i, j].text = textureBlank;
                        }
                    }
                }
            }

            Player temp = (Player)entities[0];
            temp.motion.setPosition(pX, pY);
            temp.motion.stopMovement();
            pan = true;

            camera.moveRight = true;
            camera.panTimer = Time;
            camera.startX = w / 2;
            camera.endX = 120 * 32 - 640;
            camera.endPos = new Vector2(pX, pY);
            

            return tiles;
        }

        public void updateLevel()
        {
            for (int i = 0; i < entities.Count;)
            {
                if (entities[i] is Tile || entities[i] is Enemy)
                    entities.Remove(entities[i]);
                else
                    i++;
            }

            level = new Level();
            level.tiles = LoadLevel(level.tiles, true, "LevelMaps\\map" + ((int)gState) + ".txt");

            backLevel = new Level();
            backLevel.isBackground = true;
            backLevel.tiles = LoadLevel(backLevel.tiles, false, "LevelMaps\\backMap" + ((int)gState) + ".txt");

            reset();
        }



        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            //Retrieving and updating new key data
            KeyboardState pres = Keyboard.GetState();
            MouseState m = Mouse.GetState();
            presIn = pres.GetPressedKeys().ToList();

            Time++;

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || pres.IsKeyDown(Keys.Escape))
                this.Exit();

            //Updating all entities within game
            switch (gState)
            {
                case GameState.START:
                    IsMouseVisible = true;
                    //button
                    start.Update(m, oldm);
                    right.Update(m, oldm);
                    quit.Update(m, oldm);
                    if (pres.IsKeyDown(Keys.B) || start.isPressed())
                    {
                        gState = GameState.LEVEL1;
                        //graphics.PreferredBackBufferWidth = w;
                        //graphics.PreferredBackBufferHeight = h;
                        //graphics.ApplyChanges();
                    }

                    if (startEffectControl.Bottom == 0)
                    {
                        startEffectControl.Y = h;
                    }
                    else
                    {
                        startEffectPos -= 0.3;
                        startEffectControl.Y = (int)startEffectPos;
                    }


                    if (startColor.R < 200)
                    {
                        bool check = true;
                    }

                    if (startColor.R == 255 || startColor.R <= 211)
                    {
                        if (startColor.R < 211)
                        {
                            startColor = Color.LightGray;
                        }
                        flickerVelocity /= Math.Abs(flickerVelocity);
                        flickerVelocity *= -1;
                        flickerVelocity *= rand.Next(2, 20);
                    }

                    if (startColor.R + flickerVelocity > 255)
                    {
                        startColor = Color.White;
                    }
                    else
                    {
                        startColor.R += (byte)(flickerVelocity);
                        startColor.G += (byte)(flickerVelocity);
                        startColor.B += (byte)(flickerVelocity);
                    }
                    if (quit.isPressed())
                        this.Exit();
                    break;
                case GameState.LEVEL1:
                case GameState.LEVEL2:
                case GameState.LEVEL3:

                    //Grabbing Player Data
                    Player tempPlayer = (Player)entities[0];

                    if (pres.IsKeyDown(Keys.Enter))
                    {
                        pan = false;
                        camera.moveRight = true;
                        camera.X = tempPlayer.position.X;
                        camera.Y = tempPlayer.position.Y;
                        camera.Zoom = 1.8f;
                    }
                    if (pres.IsKeyDown(Keys.I))
                    {
                        ((Player)entities[0]).health = 20;
                    }

                    if (tempPlayer.advanceLevel)
                    {
                        tempPlayer.advanceLevel = false;
                        ((Player)entities[0]).advanceLevel = false;
                        updateLevel();
                    }

                    if (tempPlayer.health <= 0)
                    {
                        gState = GameState.END;
                    }

                    if (pres.IsKeyDown(Keys.A) && !testAbility.isCalled)
                    {
                        testAbility.callAbility(entities[0].rect.X, entities[0].rect.Y, entities[0].isFacingRight);
                    }


                    if (!pan)
                    {
                        for (int i = entities.Count - 1; i >= 0; i--)
                        {
                            //Checks if object is utilizes input, and does the logic accordingly
                            if (i < entities.Count && entities[i] is InputInteractable)
                            {
                                InputInteractable temp = (InputInteractable)entities[i];
                                temp.doInputLogic(gameTime);
                            }

                            if(i < entities.Count)
                            {
                                entities[i].Update();
                            }
                            //updates object
                        }
                    }


                    //updating level logic
                    level.Update(gameTime);
                    backLevel.Update(gameTime);

                    break;
                case GameState.END:
                case GameState.FINAL:

                    if (pres.IsKeyDown(Keys.R))
                    {
                        gState = GameState.LEVEL1;
                        entities.Clear();

                        Player player1 = new Player(textures[T.Player], new Rectangle(288, 256, 51, 63), Color.White, true);
                        entities.Add(player1);

                        level = new Level();
                        level.tiles = LoadLevel(level.tiles, true, "LevelMaps\\map" + ((int)gState) + ".txt");

                        backLevel.tiles = LoadLevel(backLevel.tiles, false, "LevelMaps\\backMap" + ((int)gState) + ".txt");
                    }

                    break;
            }


            //Retrieve and update old key data
            old = pres;
            oldIn = old.GetPressedKeys().ToList();

            camera.Update(((Player)entities[0]).position);

            base.Update(gameTime);
        }

        private void reset()
        {

            backgroundPosition = new Rectangle(0, 0, 140 * 32, 20 * 32);
            backgroundVelocity = 2;

            testAbility = new AttackAbility(new Rectangle(0, FLOOR_HEIGHT - 50, 20, 20), Color.White, entities[0].isFacingRight, 2, 10);
            testShield = new DefenseAbility(shieldText, new Rectangle(0, FLOOR_HEIGHT - 50, 20, entities[0].rect.Height), Color.Green, entities[0].isFacingRight, 5, 0.5);
           // entities.Add(testAbility);
           // entities.Add(testShield);

            rightEdge = -120 * 32 + 1280;

            healthRec = new Rectangle(200, 50, 100, 40);
            healthBackRec = new Rectangle(200, 50, 100, 40);

            graphics.PreferredBackBufferWidth = w;
            graphics.PreferredBackBufferHeight = h;
            graphics.ApplyChanges();
        }

        //Helper Method to check Key press events
        public static bool KeyPressed(Keys k)
        {
            if (presIn.Contains(k) && !oldIn.Contains(k))
            {
                //Console.WriteLine("KeyPress Registered");
                return true;
            }
            return false;
        }

        //Helper Method to check Key release events
        public static bool KeyReleased(Keys k)
        {
            if (!presIn.Contains(k) && oldIn.Contains(k))
            {
                //onsole.WriteLine("KeyReleased Registered");
                return true;
            }
            return false;
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            //spriteDraw.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, camera.Transform);
           // UIElements.Begin();
            
            switch (gState)
            {
                case GameState.START:
                    UIElements.Begin();

                    //GraphicsDevice.Clear(Color.Green);
                    UIElements.Draw(startBackground, new Rectangle(0, 0, w, h), startColor);
                    UIElements.Draw(startEffects, startEffectControl, Color.White);
                    UIElements.Draw(startForeground, new Rectangle(0, 0, w, h), startColor);
                    //spriteBatch.Draw(blockImage, new Rectangle(300, 250, 400, 100), Color.White);
                    //spriteBatch.DrawString(largerFont, "The Eternal Labyrinth", new Vector2(320, 275), Color.Black);
                    //spriteBatch.DrawString(largerFont, "Press B to begin!", new Vector2(375, 500), Color.Black);

                    start.Draw(UIElements);
                    right.Draw(UIElements);
                    quit.Draw(UIElements);
                    UIElements.Draw(TitleText, Title, Color.White);
                    UIElements.End();
                    break;

                case GameState.END:
                    UIElements.Begin();

                    GraphicsDevice.Clear(Color.Black);
                    UIElements.Draw(backgroundText, new Rectangle(0, 0, w, h), Color.White);
                    UIElements.DrawString(largerFont, "YOU HAVE DIED!", new Vector2(325, 55), Color.Red);
                    UIElements.DrawString(largerFont, "GAME OVER", new Vector2(400, 200), Color.Red);
                    UIElements.DrawString(largerFont, "Press R to Play Again", new Vector2(235, 400), Color.Orange);
                    UIElements.End();
                    break;
                case GameState.FINAL:
                    UIElements.Begin();

                    GraphicsDevice.Clear(Color.Black);
                    UIElements.Draw(backgroundText, new Rectangle(0, 0, w, h), Color.White);
                    UIElements.DrawString(largerFont, "YOU HAVE ESCAPED!", new Vector2(325, 55), Color.Red);
                    UIElements.DrawString(largerFont, "GAME COMPLETED", new Vector2(400, 200), Color.Red);
                    UIElements.DrawString(largerFont, "Press R to Play Again", new Vector2(235, 400), Color.Orange);
                    UIElements.End();
                    break;
                case GameState.LEVEL1:
                case GameState.LEVEL2:
                case GameState.LEVEL3:
                    spriteDraw.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, camera.Transform);
                    

                    //Temporary background
                    spriteDraw.Draw(backgroundText, backgroundPosition, Color.Gray);

                    //spriteBatch.Draw(back, entities[0].rect, Color.Black);

                    //Draws all entities in game

                    //Foreground Tiles / Environment
                    backLevel.Draw(gameTime, spriteDraw);
                    foreach (Object p in entities)
                    {
                        if (p is AttackAbility)
                        {
                            AttackAbility tempAbility = (AttackAbility)p;
                            if (tempAbility.isCalled)
                            {
                                p.Draw(spriteDraw);
                            }
                        }
                        if (p is DefenseAbility)
                        {
                            DefenseAbility tempAbility = (DefenseAbility)p;
                            if (tempAbility.isCalled)
                            {
                                p.Draw(spriteDraw);
                            }
                        }

                        if (!(p is Tile) && !pan)
                        {
                            p.Draw(spriteDraw);
                        }
                    }
                    level.Draw(gameTime, spriteDraw);

                    spriteDraw.End();
                    break;

            }

            if (gState == GameState.LEVEL1 || gState == GameState.LEVEL2 || gState == GameState.LEVEL3)
            {
                UIElements.Begin();

                screenEffects.Begin();
                foreach(Object p in entities)
                {
                    if (p is ScreenEffect)
                    {
                        p.Draw(screenEffects);
                    }
                }
                screenEffects.End();

                Player temp = (Player)entities[0];

                UIElements.DrawString(font, "Health:", new Vector2(100, 55), Color.DarkSalmon);
                UIElements.DrawString(font, gState + "", new Vector2(600, 55), Color.DarkSalmon);

                if(temp.timeStamps.ContainsKey("BOULDREN"))
                {
                    UIElements.DrawString(font, temp.timeStamps["BOULDREN"].ToString(), new Vector2(10, 250), Color.DarkSalmon);
                }
                UIElements.DrawString(font, HelperMethods.TotalSeconds().ToString(), new Vector2(10, 300), Color.DarkSalmon);

                UIElements.Draw(healthText, healthBackRec, Color.Gray);
                UIElements.Draw(healthText, new Rectangle(200, 50, (int)(100 * (temp.health / 20.0)), 40), Color.DarkRed);
                UIElements.DrawString(font, "" + temp.health, new Vector2(300, 55), Color.Orange);
                UIElements.End();
            }

            base.Draw(gameTime);
        }

        private void ReadFilesAsStrings(string path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        lines.Add(line);
                    }
                }
            }
            catch (Exception e)
            { }
        }
    }
}