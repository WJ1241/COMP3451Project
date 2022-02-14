using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using COMP3451Project.EnginePackage.CollisionManagement;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.EntityManagement;
using COMP3451Project.EnginePackage.InputManagement;
using COMP3451Project.EnginePackage.SceneManagement;
using COMP3451Project.EnginePackage.Services;
using COMP3451Project.EnginePackage.Services.Factories;
using COMP3451Project.EnginePackage.Services.Commands;
using COMP3451Project.PongPackage.Entities;

namespace COMP3451Project
{
    /// <summary>
    /// This is the main type for your game.
    /// Author: William Smith & Declan Kerby-Collins
    /// Date: 12/02/22
    /// </summary>
    public class Kernel : Game, IInitialiseParam<IService>
    {
        #region FIELD VARIABLES

        // DECLARE a GraphicsDeviceManager, name it '_graphics':
        private GraphicsDeviceManager _graphics;

        // DECLARE a SpriteBatch, name it '_spriteBatch':
        private SpriteBatch _spriteBatch;

        // DECLARE a Random, name it '_rand':
        private Random _rand;

        // DECLARE an IRtnService, name it '_engineManager':
        private IRtnService _engineManager;

        // DECLARE a Point, name it 'screenSize':
        private Point _screenSize;

        // DECLARE an int, name it '_ballCount':
        // USED ONLY TO KEEP VALUE FROM RESETTING
        private int _ballCount;

        #endregion


        #region CONSTRUCTOR

        public Kernel()
        {
            // INSTANTIATE _graphics as a new GraphicsDeviceManager, passing Kernel as a parameter:
            _graphics = new GraphicsDeviceManager(this);

            // SET RootDirectory of Content to "Content":
            Content.RootDirectory = "Content";

            // SET IsMouseVisible to true:
            IsMouseVisible = true;

            // INSTANTIATE _screenSize as a new Point, with 1600 for X axis, and 900 for Y axis:
            _screenSize = new Point(1600, 900);

            // SET screen width to _screenSize.X:
            _graphics.PreferredBackBufferWidth = _screenSize.X;

            // SET screen height to _screenSize.Y:
            _graphics.PreferredBackBufferHeight = _screenSize.Y;
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEISERVICE

        /// <summary>
        /// Initialises an object with a reference to an IService instance
        /// </summary>
        /// <param name="pService"> IService instance </param>
        public void Initialise(IService pService)
        {
            // INITIALISE _engineManager with pService cast as IRtnService:
            _engineManager = pService as IRtnService;
        }

        #endregion


        #region PROTECTED METHODS

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            #region OBJECT INSTANTIATIONS & INITIALISATIONS

            // Get Screen Width:
            _screenSize.X = GraphicsDevice.Viewport.Width;

            // GET Screen Height:
            _screenSize.Y = GraphicsDevice.Viewport.Height;

            // INITIALISE _ballCount with value of '0':
            _ballCount = 0;

            // INSTANTIATE _rand as new Random():
            _rand = new Random();

            #region ENTITY MANAGER

            // DECLARE & GET an instance of EntityManager as an IEntityManager, name it '_entityManager':
            IEntityManager _entityManager = _engineManager.GetService<EntityManager>() as IEntityManager;

            // INITIALISE _entityManager with a Factory<IEntity> instance from _engineManager:
            (_entityManager as IInitialiseParam<IFactory<IEntity>>).Initialise(_engineManager.GetService<Factory<IEntity>>() as IFactory<IEntity>);

            // INITIALISE _entityManager with a CommandScheduler instance from _engineManager:
            (_entityManager as IInitialiseParam<ICommandScheduler>).Initialise(_engineManager.GetService<CommandScheduler>() as ICommandScheduler);

            // INITIALISE _entityManager with a KeyboardManager instance from _engineManager:
            (_entityManager as IInitialiseParam<IKeyboardPublisher>).Initialise(_engineManager.GetService<KeyboardManager>() as IKeyboardPublisher);

            #endregion


            #region SCENE MANAGER

            // DECLARE & GET an instance of SceneManager as an ISceneManager, name it '_sceneManager':
            ISceneManager _sceneManager = _engineManager.GetService<SceneManager>() as ISceneManager;

            // INITIALISE _sceneManager with returned Factory<ISceneGraph> instance from _engineManager:
            (_sceneManager as IInitialiseParam<IFactory<ISceneGraph>>).Initialise(_engineManager.GetService<Factory<ISceneGraph>>() as IFactory<ISceneGraph>);

            #endregion


            #endregion


            #region LEVEL 1 CREATION

            // CALL CreateScene() on _sceneManager, passing "Level1" as a parameter:
            _sceneManager.CreateScene("Level1");

            // INITIALISE _sceneManager with a CollisionManager instance from _engineManager for scene "Level1":
            _sceneManager.Initialise("Level1", _engineManager.GetService<CollisionManager>() as ICollisionManager);

            #region DISPLAYABLE CREATION

            #region LAYER 5 - PADDLE / BALL

            #region PADDLES
            /*
            #region PADDLE 1
            
            // INSTANTIATE new Paddle(), named "paddle1":
            _entityManager.Create<Paddle>("Paddle1");

            // SUBSCRIBE "Paddle1" to returned KeyboardManager from _engineManager:
            (_engineManager.GetService<KeyboardManager>() as IKeyboardPublisher).Subscribe(_entityManager.GetDictionary()["Paddle1"] as IKeyboardListener);

            // INITIALISE "Paddle1":
            _entityManager.GetDictionary()["Paddle1"].Initialise();

            // SET PlayerIndex of "Paddle1" to PlayerIndex.One:
            (_entityManager.GetDictionary()["Paddle1"] as IPlayer).PlayerNum = PlayerIndex.One;

            // SET Layer of "Paddle1" to 5:
            (_entityManager.GetDictionary()["Paddle1"] as ILayer).Layer = 5;

            #endregion


            #region PADDLE 2

            // INSTANTIATE new Paddle(), named "paddle2":
            _entityManager.Create<Paddle>("Paddle2");

            // SUBSCRIBE "Paddle2" to returned KeyboardManager from _engineManager:
            (_engineManager.GetService<KeyboardManager>() as IKeyboardPublisher).Subscribe(_entityManager.GetDictionary()["Paddle2"] as IKeyboardListener);

            // INITIALISE "Paddle2":
            _entityManager.GetDictionary()["Paddle2"].Initialise();

            // SET PlayerIndex of "Paddle2" to PlayerIndex.Two:
            (_entityManager.GetDictionary()["Paddle2"] as IPlayer).PlayerNum = PlayerIndex.Two;

            // SET Layer of "Paddle2" to 5:
            (_entityManager.GetDictionary()["Paddle1"] as ILayer).Layer = 5;
            
            #endregion
            */
            #endregion


            #region BALL

            // CALL CreateBall():
            //CreateBall();

            #endregion

            #endregion

            #endregion

            #endregion


            // INITIALISE base class:
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // INSTANTIATE _spriteBatch as new SpriteBatch:
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            /*

            //------------------------------------------
            //Dec's attempt at tile maps mrk1

            Tiles.Content = Content;

            //builds map.... make sure to add png's for tiles and make sure theyre called "Tile1" "Tile2" etc
            //where there are 1's there should populate with walls
            _map.BuildWalls(new int[,]
            {
                { 1,1,1,1,1,1,1,1,1,1,1 },
                { 1,0,0,0,0,0,0,0,0,0,1 },
                { 1,0,0,0,0,0,0,0,0,0,1 },
                { 1,0,0,0,0,0,0,0,0,0,1 },
                { 1,0,0,0,0,0,0,0,0,0,1 },
                { 1,1,1,1,1,1,1,1,1,1,1 }

            }, 50);


            //------------------------------------------

            */

            #region LAYER 5

            #region PADDLES
            /*
            /// PADDLE 1

            // DECLARE & INITIALISE an IEntity with reference to "Paddle1":
            IEntity _tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["Paddle1"];

            // LOAD "paddle" texture to "Paddle1":
            (_tempEntity as ITexture).Texture = Content.Load<Texture2D>("paddle");

            // SPAWN "Paddle1" in "Level1" at the far left on the X axis with a gap, and middle on the Y axis:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level1", _tempEntity, new Vector2(0 + (_tempEntity as ITexture).Texture.Width, (_screenSize.Y / 2) - (_tempEntity as ITexture).Texture.Height / 2));

            /// PADDLE 2

            // INITIALISE _tempEntity with reference to "Paddle2":
            _tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["Paddle2"];

            // LOAD "paddle" texture to "Paddle2":
            (_tempEntity as ITexture).Texture = Content.Load<Texture2D>("paddle");

            // SPAWN "Paddle2" in "Level1" at the far left on the X axis with a gap, and middle on the Y axis:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level1", _tempEntity, new Vector2(_screenSize.X - ((_tempEntity as ITexture).Texture.Width * 2), (_screenSize.Y / 2) - (_tempEntity as ITexture).Texture.Height / 2));

            */
            #endregion
            
            #endregion
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
        /// <param name="pGameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime pGameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // Get Screen Width:
            _screenSize.X = GraphicsDevice.Viewport.Width;

            // GET Screen Height:
            _screenSize.Y = GraphicsDevice.Viewport.Height;

            // CALL Update() on returned SceneManager instance from _engineManager, passing pGameTime as a parameter:
            (_engineManager.GetService<SceneManager>() as IUpdatable).Update(pGameTime);

            // CALL Update() on returned CollisionManager instance from _engineManager, passing pGameTime as a parameter:
            (_engineManager.GetService<CollisionManager>() as IUpdatable).Update(pGameTime);

            // CALL Update() on returned KeyboardManager instance from _engineManager, passing pGameTime as a parameter:
            (_engineManager.GetService<KeyboardManager>() as IUpdatable).Update(pGameTime);

            // CALL Update() on returned CommandScheduler instance from _engineManager, passing pGameTime as a parameter:
            (_engineManager.GetService<CommandScheduler>() as IUpdatable).Update(pGameTime);

            // CALL Update() on base class, passing pGameTime as a parameter:
            base.Update(pGameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // SET colour of screen background as Black:
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // CALL Draw() method on returned SceneManager instance from _engineManager:
            (_engineManager.GetService<SceneManager>() as IDraw).Draw(_spriteBatch);

            // CALL Draw() method from base class:
            base.Draw(gameTime);
        }

        #endregion


        #region PRIVATE METHODS

        /// <summary>
        /// Creates, initialises, and spawns a Ball object
        /// </summary>
        private void CreateBall()
        {
            #region CREATION & INITIALISATION

            // DECLARE & GET an instance of EntityManager as an IEntityManager, name it '_entityManager':
            IEntityManager _entityManager = _engineManager.GetService<EntityManager>() as IEntityManager;

            // INCREMENT _ballCount by '1':
            _ballCount++;

            // INSTANTIATE new Ball():
            _entityManager.Create<Ball>("Ball" + _ballCount);

            // INITIALISE ("Ball" + _ballCount):
            _entityManager.GetDictionary()["Ball" + _ballCount].Initialise();

            // INITIALISE ("Ball" + _ballCount) with _rand:
            (_entityManager.GetDictionary()["Ball" + _ballCount] as IInitialiseParam<Random>).Initialise(_rand);

            // SET Layer of ("Ball" + _ballCount) to 5:
            (_entityManager.GetDictionary()["Ball" + _ballCount] as ILayer).Layer = 5;

            // CALL Reset() on ("Ball" + _ballCount):
            (_entityManager.GetDictionary()["Ball" + _ballCount] as IReset).Reset();

            #endregion


            #region SPAWNING



            #endregion
        }

        #endregion
    }
}