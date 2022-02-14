using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using COMP3451Project.EnginePackage.Behaviours;
using COMP3451Project.EnginePackage.CollisionManagement;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.EntityManagement;
using COMP3451Project.EnginePackage.InputManagement;
using COMP3451Project.EnginePackage.SceneManagement;
using COMP3451Project.EnginePackage.Services;
using COMP3451Project.EnginePackage.Services.Factories;
using COMP3451Project.EnginePackage.Services.Commands;
using COMP3451Project.EnginePackage.States;
using COMP3451Project.PongPackage.Behaviours;
using COMP3451Project.PongPackage.Entities;

namespace COMP3451Project
{
    /// <summary>
    /// This is the main type for your game.
    /// Author: William Smith & Declan Kerby-Collins
    /// Date: 14/02/22
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

            #region LAYER 5

            #region PADDLES

            #region PADDLE 1

            #region STATES

            /// INSTANTIATION

            // DECLARE & INSTANTIATE an IState as a new PaddleState(), name it '_tempStateStationary':
            IState _tempStateStationary = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<PaddleState>();

            // DECLARE & INSTANTIATE an IState as a new PaddleState(), name it '_tempStateUp':
            IState _tempStateUp = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<PaddleState>();

            // DECLARE & INSTANTIATE an IState as a new PaddleState(), name it '_tempStateDown':
            IState _tempStateDown = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<PaddleState>();
            
            /// INITIALISATION

            // SET Name Property value of _tempStateStationary to "stationary":
            (_tempStateStationary as IName).Name = "stationary";

            // SET ScheduleCommand Property of _tempStateStationary to reference of CommandScheduler.ScheduleCommand:
            (_tempStateStationary as ICommandSender).ScheduleCommand = (_engineManager.GetService<CommandScheduler>() as ICommandScheduler).ScheduleCommand;

            // SET Name Property value of _tempStateUp to "up":
            (_tempStateUp as IName).Name = "up";

            // SET ScheduleCommand Property of _tempStateUp to reference of CommandScheduler.ScheduleCommand:
            (_tempStateUp as ICommandSender).ScheduleCommand = (_engineManager.GetService<CommandScheduler>() as ICommandScheduler).ScheduleCommand;

            // SET Name Property value of _tempStateDown to "down":
            (_tempStateDown as IName).Name = "down";

            // SET ScheduleCommand Property of _tempStateDown to reference of CommandScheduler.ScheduleCommand:
            (_tempStateDown as ICommandSender).ScheduleCommand = (_engineManager.GetService<CommandScheduler>() as ICommandScheduler).ScheduleCommand;

            #endregion


            #region BEHAVIOURS

            /// INSTANTIATION

            // DECLARE & INSTANTIATE an IUpdateEventListener as a new PaddleBehaviour(), name it '_behaviourStationary':
            IUpdateEventListener _behaviourStationary = (_engineManager.GetService<Factory<IUpdateEventListener>>() as IFactory<IUpdateEventListener>).Create<PaddleBehaviour>();

            // DECLARE & INSTANTIATE an IUpdateEventListener as a new PaddleBehaviour(), name it '_behaviourUp':
            IUpdateEventListener _behaviourUp = (_engineManager.GetService<Factory<IUpdateEventListener>>() as IFactory<IUpdateEventListener>).Create<PaddleBehaviour>();

            // DECLARE & INSTANTIATE an IUpdateEventListener as a new PaddleBehaviour(), name it '_behaviourDown':
            IUpdateEventListener _behaviourDown = (_engineManager.GetService<Factory<IUpdateEventListener>>() as IFactory<IUpdateEventListener>).Create<PaddleBehaviour>();
            
            /// INITIALISATION

            // SET Name Property value of _behaviourStationary to "stationary":
            (_behaviourStationary as IName).Name = "stationary";

            // SET Direction Property value of _behaviourStationary to '0':
            (_behaviourStationary as IDirection).Direction = new Vector2(0);

            // SET Name Property value of _behaviourUp to "up":
            (_behaviourUp as IName).Name = "up";

            // SET Direction Property value of _behaviourUp to '0, -1':
            (_behaviourUp as IDirection).Direction = new Vector2(0, -1);

            // SET Name Property value of _behaviourDown to "down":
            (_behaviourDown as IName).Name = "down";

            // SET Direction Property value of _behaviourDown to '0, 1':
            (_behaviourDown as IDirection).Direction = new Vector2(0, 1);

            // INITIALISE _tempStateStationary with reference to _behaviourStationary:
            (_tempStateStationary as IInitialiseParam<IUpdateEventListener>).Initialise(_behaviourStationary);

            // INITIALISE _tempStateUp with reference to _behaviourUp:
            (_tempStateUp as IInitialiseParam<IUpdateEventListener>).Initialise(_behaviourUp);

            // INITIALISE _tempStateDown with reference to _behaviourDown:
            (_tempStateDown as IInitialiseParam<IUpdateEventListener>).Initialise(_behaviourDown);

            #endregion


            #region ENTITY

            /// INSTANTIATION

            // INSTANTIATE new Paddle(), name it "Paddle1":
            _entityManager.Create<Paddle>("Paddle1");

            // SUBSCRIBE "Paddle1" to returned KeyboardManager from _engineManager:
            (_engineManager.GetService<KeyboardManager>() as IKeyboardPublisher).Subscribe(_entityManager.GetDictionary()["Paddle1"] as IKeyboardListener);

            // INITIALISE _behaviourStationary with reference to "Paddle1":
            (_behaviourStationary as IInitialiseParam<IEntity>).Initialise(_entityManager.GetDictionary()["Paddle1"]);

            // INITIALISE _behaviourup with reference to "Paddle1":
            (_behaviourUp as IInitialiseParam<IEntity>).Initialise(_entityManager.GetDictionary()["Paddle1"]);

            // INITIALISE _behaviourDown with reference to "Paddle1":
            (_behaviourDown as IInitialiseParam<IEntity>).Initialise(_entityManager.GetDictionary()["Paddle1"]);

            /// INITIALISATION

            // INITIALISE "Paddle1":
            _entityManager.GetDictionary()["Paddle1"].Initialise();

            // INITIALISE "Paddle1" with _tempStateStationary:
            (_entityManager.GetDictionary()["Paddle1"] as IInitialiseParam<IState>).Initialise(_tempStateStationary);

            // SET WindowBorder of "Paddle1" to value of _screenSize:
            (_entityManager.GetDictionary()["Paddle1"] as IContainBoundary).WindowBorder = _screenSize;

            // SET PlayerIndex of "Paddle1" to PlayerIndex.One:
            (_entityManager.GetDictionary()["Paddle1"] as IPlayer).PlayerNum = PlayerIndex.One;

            // SET Layer of "Paddle1" to 5:
            (_entityManager.GetDictionary()["Paddle1"] as ILayer).Layer = 5;

            #endregion


            #region COMMANDS

            /// INSTANTIATION

            // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam(), name it '_stateStationaryChange':
            ICommand _stateStationaryChange = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<IState>>();

            // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam(), name it '_stateUpChange':
            ICommand _stateUpChange = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<IState>>();

            // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam(), name it '_stateDownChange':
            ICommand _stateDownChange = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<IState>>();

            /// INITIALISATION

            // SET MethodRef Property value of _stateStationaryChange to reference of "Paddle1"'s SetState() method:
            (_stateStationaryChange as ICommandOneParam<IState>).MethodRef = (_entityManager.GetDictionary()["Paddle1"] as IEntityInternal).SetState;

            // SET Data Property value of _stateStationaryChange to reference of _tempStateStationary:
            (_stateStationaryChange as ICommandOneParam<IState>).Data = _tempStateStationary;

            // SET MethodRef Property value of _stateUpChange to reference of "Paddle1"'s SetState() method:
            (_stateUpChange as ICommandOneParam<IState>).MethodRef = (_entityManager.GetDictionary()["Paddle1"] as IEntityInternal).SetState;

            // SET Data Property value of _stateUpChange to reference of _tempStateUp:
            (_stateUpChange as ICommandOneParam<IState>).Data = _tempStateUp;

            // SET MethodRef Property value of _stateDownChange to reference of "Paddle1"'s SetState() method:
            (_stateDownChange as ICommandOneParam<IState>).MethodRef = (_entityManager.GetDictionary()["Paddle1"] as IEntityInternal).SetState;

            // SET Data Property value of _stateDownChange to reference of _tempStateDown:
            (_stateDownChange as ICommandOneParam<IState>).Data = _tempStateDown;

            // INITIALISE _tempStateStationary with _tempStateUp.Name and _stateUpChange as parameters:
            (_tempStateStationary as IInitialiseParam<string, ICommand>).Initialise((_tempStateUp as IName).Name, _stateUpChange);

            // INITIALISE _tempStateStationary with _tempStateDown.Name and _stateDownChange as parameters:
            (_tempStateStationary as IInitialiseParam<string, ICommand>).Initialise((_tempStateDown as IName).Name, _stateDownChange);

            // INITIALISE _tempStateUp with _tempStateStationary.Name and _stateStationaryChange as parameters:
            (_tempStateUp as IInitialiseParam<string, ICommand>).Initialise((_tempStateStationary as IName).Name, _stateStationaryChange);

            // INITIALISE _tempStateUp with _tempStateDown.Name and _stateDownChange as parameters:
            (_tempStateUp as IInitialiseParam<string, ICommand>).Initialise((_tempStateDown as IName).Name, _stateDownChange);

            // INITIALISE _tempStateDown with _tempStateStationary.Name and _stateStationaryChange as parameters:
            (_tempStateDown as IInitialiseParam<string, ICommand>).Initialise((_tempStateStationary as IName).Name, _stateStationaryChange);

            // INITIALISE _tempStateDown with _tempStateUp.Name and _stateUpChange as parameters:
            (_tempStateDown as IInitialiseParam<string, ICommand>).Initialise((_tempStateUp as IName).Name, _stateUpChange);


            #endregion

            #endregion

            #endregion

            #endregion

            /*
            #region PADDLE 2

            // INSTANTIATE new Paddle(), name it "paddle2":
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
            
            /// PADDLE 1

            // DECLARE & INITIALISE an IEntity with reference to "Paddle1":
            IEntity _tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["Paddle1"];

            // LOAD "paddle" texture to "Paddle1":
            (_tempEntity as ITexture).Texture = Content.Load<Texture2D>("paddle");

            // SPAWN "Paddle1" in "Level1" at the far left on the X axis with a gap, and middle on the Y axis:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level1", _tempEntity, new Vector2(0 + (_tempEntity as ITexture).Texture.Width, (_screenSize.Y / 2) - (_tempEntity as ITexture).Texture.Height / 2));

            /*
            /// PADDLE 2

            // INITIALISE _tempEntity with reference to "Paddle2":
            _tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["Paddle2"];

            // LOAD "paddle" texture to "Paddle2":
            (_tempEntity as ITexture).Texture = Content.Load<Texture2D>("paddle");

            // SPAWN "Paddle2" in "Level1" at the far left on the X axis with a gap, and middle on the Y axis:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level1", _tempEntity, new Vector2(_screenSize.X - ((_tempEntity as ITexture).Texture.Width * 2), (_screenSize.Y / 2) - (_tempEntity as ITexture).Texture.Height / 2));
            */
            #endregion
            

            #region BALL

            // CALL CreateBall(), creates, initialises and spawns on screen:
            //CreateBall();

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

            // GET Screen Width:
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