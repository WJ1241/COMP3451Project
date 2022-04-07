using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TiledSharp;
using OrbitalEngine.Animation;
using OrbitalEngine.Animation.Interfaces;
using OrbitalEngine.Audio;
using OrbitalEngine.Audio.Interfaces;
using OrbitalEngine.Behaviours.Interfaces;
using OrbitalEngine.CollisionManagement;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.EntityManagement;
using OrbitalEngine.EntityManagement.Interfaces;
using OrbitalEngine.Exceptions;
using OrbitalEngine.InputManagement;
using OrbitalEngine.InputManagement.Interfaces;
using OrbitalEngine.SceneManagement;
using OrbitalEngine.SceneManagement.Interfaces;
using OrbitalEngine.Services.Commands;
using OrbitalEngine.Services.Commands.Interfaces;
using OrbitalEngine.Services.Interfaces;
using OrbitalEngine.Services.Factories;
using OrbitalEngine.Services.Factories.Interfaces;
using OrbitalEngine.States.Interfaces;
using OrbitalEngine.Tiles;
using OrbitalEngine.Tiles.Interfaces;
using COMP3451Project.RIRRPackage;
using COMP3451Project.RIRRPackage.Behaviours;
using COMP3451Project.RIRRPackage.Behaviours.Interfaces;
using COMP3451Project.RIRRPackage.Entities;
using COMP3451Project.RIRRPackage.Interfaces;
using COMP3451Project.RIRRPackage.States;
using System.Collections;
using OrbitalEngine.CustomEventArgs;

namespace COMP3451Project
{
    /// <summary>
    /// This is the main type for your game.
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 07/04/22
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

        /// <summary>
        /// Constructor for objects of Kernel
        /// </summary>
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


        #region IMPLEMENTATION OF IINITIALISEPARAM<ISERVICE>

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

            // TRY checking if all engine creational classes as well as entity setup throw any exceptions:
            try
            {
                #region ENTITY MANAGER

                // DECLARE & GET an instance of EntityManager as an IEntityManager, name it 'entityManager':
                IEntityManager entityManager = _engineManager.GetService<EntityManager>() as IEntityManager;

                // INITIALISE entityManager with a new Dictionary<string, IEntity>():
                (entityManager as IInitialiseParam<IDictionary<string, IEntity>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, IEntity>>() as IDictionary<string, IEntity>);

                // INITIALISE entityManager with a Factory<IEntity> instance from _engineManager:
                (entityManager as IInitialiseParam<IFactory<IEntity>>).Initialise(_engineManager.GetService<Factory<IEntity>>() as IFactory<IEntity>);

                // INITIALISE entityManager with a CommandScheduler instance from _engineManager:
                (entityManager as IInitialiseParam<ICommandScheduler>).Initialise(_engineManager.GetService<CommandScheduler>() as ICommandScheduler);

                // INITIALISE entityManager with a KeyboardManager instance from _engineManager:
                (entityManager as IInitialiseParam<IKeyboardPublisher>).Initialise(_engineManager.GetService<KeyboardManager>() as IKeyboardPublisher);

                /// CREATE ICOMMAND FUNCCOMMAND

                // DECLARE & INSTANTIATE an IFuncCommand<ICommandOneParam<string>> as a new FuncCommand<ICommandOneParam<string>>(), name it 'createCommand':
                IFuncCommand<ICommand> createCommand = (_engineManager.GetService<Factory<IFuncCommand<ICommand>>>() as IFactory<IFuncCommand<ICommand>>).Create<FuncCommandZeroParam<ICommand>>();

                Console.WriteLine(_engineManager.GetService<Factory<IFuncCommand<ICommand>>>().GetType());


                // INITIALISE _createFloor's MethodRef Property with Factory<ICommand>.Create<CommandOneParam<string>>:
                (createCommand as IFuncCommandZeroParam<ICommand>).MethodRef = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<string>>;

                // INITIALISE entityManager with a reference to createCommand:
                (entityManager as IInitialiseParam<IFuncCommand<ICommand>>).Initialise(createCommand);

                #endregion


                #region SCENE MANAGER

                // DECLARE & GET an instance of SceneManager as an ISceneManager, name it 'sceneManager':
                ISceneManager sceneManager = _engineManager.GetService<SceneManager>() as ISceneManager;

                // INITIALISE engineManager with a new Dictionary<string, IService>():
                (sceneManager as IInitialiseParam<IDictionary<string, ISceneGraph>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ISceneGraph>>() as IDictionary<string, ISceneGraph>);

                // INITIALISE sceneManager with returned Factory<ISceneGraph> instance from _engineManager:
                (sceneManager as IInitialiseParam<IFactory<ISceneGraph>>).Initialise(_engineManager.GetService<Factory<ISceneGraph>>() as IFactory<ISceneGraph>);

                #endregion


                #region COLLISION MANAGER

                // INITIALISE returned CollisionManager from _engineManager.GetService() with a new List<ICollidable>():
                (_engineManager.GetService<CollisionManager>() as IInitialiseParam<IList<ICollidable>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<List<ICollidable>>() as IList<ICollidable>);


                #endregion


                #region KEYBOARD MANAGER

                // INITIALISE returned KeyboardManager from _engineManager.GetService() with a new Dictionary<string, IKeyboardListener>():
                (_engineManager.GetService<KeyboardManager>() as IInitialiseParam<IDictionary<string, IKeyboardListener>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, IKeyboardListener>>() as IDictionary<string, IKeyboardListener>);

                #endregion


                #region LEVEL 1 CREATION

                /// SCENE

                // CALL CreateScene() on sceneManager, passing "Level1" as a parameter:
                sceneManager.CreateScene("Level1");

                // INITIALISE sceneManager with a CollisionManager instance from _engineManager, a new Dictionary<string, IEntity>() and a reference to createCommand for scene "Level1":
                sceneManager.Initialise("Level1", _engineManager.GetService<CollisionManager>() as ICollisionManager,
                    (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, IEntity>>() as IDictionary<string, IEntity>, createCommand);

                /// REFEREE

                // DECLARE & INSTANTIATE an IPongReferee as a new PongReferee(), name it 'referee':
                IPongReferee referee = _engineManager.GetService<PongReferee>() as IPongReferee;

                // DECLARE & INSTANTIATE an ICommand as a new CommandZeroParam(), name it 'respawnBall':
                ICommand respawnBall = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandZeroParam>();

                // SET MethodRef Property value of respawnBall to reference of CreateBall():
                (respawnBall as ICommandZeroParam).MethodRef = CreateBall;

                // SET ScheduleCommand Property of referee to reference of respawnBall:
                referee.RespawnBall = respawnBall;


                #region DISPLAYABLE CREATION

                #region LAYER 2

                #region WALLS

                #region COMMANDS

                // DECLARE & INSTANTIATE an IFuncCommand<IEntity> as a new FuncCommandOneParam<string, IEntity>(), name it 'createWall':
                IFuncCommand<IEntity> createWall = (_engineManager.GetService<Factory<IFuncCommand<IEntity>>>() as IFactory<IFuncCommand<IEntity>>).Create<FuncCommandOneParam<string, IEntity>>();

                // INITIALISE _createFloor's MethodRef Property with EntityManager.Create<Wall>:
                (createWall as IFuncCommandOneParam<string, IEntity>).MethodRef = (_engineManager.GetService<EntityManager>() as IEntityManager).Create<Wall>;

                // DECLARE & INSTANTIATE an IFuncCommand<IEntity> as a new FuncCommandOneParam<string, IEntity>(), name it 'createFloor':
                IFuncCommand<IEntity> createFloor = new FuncCommandOneParam<string, IEntity>(); //(_engineManager.GetService<Factory<IFuncCommand<IEntity>>>() as IFactory<IFuncCommand<IEntity>>).Create<FuncCommandOneParam<string, IEntity>>();

                // INITIALISE _createFloor's MethodRef Property with EntityManager.Create<DrawableRectangleEntity>:
                (createFloor as IFuncCommandOneParam<string, IEntity>).MethodRef = (_engineManager.GetService<EntityManager>() as IEntityManager).Create<DrawableRectangleEntity>;

                #endregion

                // DECLARE & INSTANTIATE an ILevelLayoutMaker as a new LevelLayoutMaker(), name it 'levelLM':
                ILevelLayoutMaker levelLM = _engineManager.GetService<LevelLayoutMaker>() as ILevelLayoutMaker;

                // INITIALISE levelLM with a new Dictionary<string, IEntity>():
                (levelLM as IInitialiseParam<IDictionary<string, IFuncCommand<IEntity>>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, IFuncCommand<IEntity>>>() as IDictionary<string, IFuncCommand<IEntity>>);

                // INITIALISE levelLM with "Wall" and createWall as parameters:
                (levelLM as IInitialiseParam<string, IFuncCommand<IEntity>>).Initialise("Wall", createWall);

                // INITIALISE levelLM with "Floor" and createFloor as parameters:
                (levelLM as IInitialiseParam<string, IFuncCommand<IEntity>>).Initialise("Floor", createFloor);

                // DECLARE & INSTANTIATE a new TmxMap(), name it '_map', passing a .tmx file as a parameter:
                TmxMap map = new TmxMap("..\\..\\..\\..\\Content\\ExampleLevel\\ExampleLevel.tmx");

                // DECLARE & INITIALISE a Texture2D, name it '_tilesetTex', give value of _map's Tilesets[0]'s name:
                Texture2D tilesetTex = Content.Load<Texture2D>("ExampleLevel\\" + map.Tilesets[0].Name);

                // CALL CreateLevelLayout() on levelLM, passing "ExampleLevel", _map and _tilesetTex as parameters:
                levelLM.CreateLevelLayout("ExampleLevel", map, tilesetTex);

                #endregion

                #endregion


                #region LAYER 5

                #region PADDLES

                #region PADDLE 1

                #region STATES

                #region INSTANTIATION

                // DECLARE & INSTANTIATE an IState as a new PaddleState(), name it 'tempStateStationary':
                IState tempStateStationary = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<PaddleState>();

                // DECLARE & INSTANTIATE an IState as a new PaddleState(), name it 'tempStateUp':
                IState tempStateUp = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<PaddleState>();

                // DECLARE & INSTANTIATE an IState as a new PaddleState(), name it 'tempStateDown':
                IState tempStateDown = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<PaddleState>();

                #endregion


                #region INITIALISATION

                /// STATIONARY
                
                // SET Name Property value of tempStateStationary to "stationary":
                (tempStateStationary as IName).Name = "stationary";

                // INITIALISE tempStateStationary with a new Dictionary<string, ICommand>():
                (tempStateStationary as IInitialiseParam<IDictionary<string, ICommand>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ICommand>>() as IDictionary<string, ICommand>);

                // INITIALISE tempStateStationary with a new Dictionary<string, EventArgs>():
                (tempStateStationary as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

                // INITIALISE tempStateStationary with a new UpdateEventArgs():
                (tempStateStationary as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

                // SET PlayerIndex of tempStateStationary to PlayerIndex.One:
                (tempStateStationary as IPlayer).PlayerNum = PlayerIndex.One;

                // SET ScheduleCommand Property of tempStateStationary to reference of CommandScheduler.ScheduleCommand:
                (tempStateStationary as ICommandSender).ScheduleCommand = (_engineManager.GetService<CommandScheduler>() as ICommandScheduler).ScheduleCommand;

                /// UP

                // SET Name Property value of tempStateUp to "up":
                (tempStateUp as IName).Name = "up";

                // INITIALISE tempStateUp with a new Dictionary<string, ICommand>():
                (tempStateUp as IInitialiseParam<IDictionary<string, ICommand>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ICommand>>() as IDictionary<string, ICommand>);

                // INITIALISE tempStateUp with a new Dictionary<string, EventArgs>():
                (tempStateUp as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

                // INITIALISE tempStateUp with a new UpdateEventArgs():
                (tempStateUp as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

                // SET PlayerIndex of tempStateUp to PlayerIndex.One:
                (tempStateUp as IPlayer).PlayerNum = PlayerIndex.One;

                // SET ScheduleCommand Property of tempStateUp to reference of CommandScheduler.ScheduleCommand:
                (tempStateUp as ICommandSender).ScheduleCommand = (_engineManager.GetService<CommandScheduler>() as ICommandScheduler).ScheduleCommand;

                /// DOWN

                // SET Name Property value of tempStateDown to "down":
                (tempStateDown as IName).Name = "down";

                // INITIALISE tempStateDown with a new Dictionary<string, ICommand>():
                (tempStateDown as IInitialiseParam<IDictionary<string, ICommand>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ICommand>>() as IDictionary<string, ICommand>);

                // INITIALISE tempStateDown with a new Dictionary<string, EventArgs>():
                (tempStateDown as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

                // INITIALISE tempStateDown with a new UpdateEventArgs():
                (tempStateDown as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

                // SET PlayerIndex of tempStateDown to PlayerIndex.One:
                (tempStateDown as IPlayer).PlayerNum = PlayerIndex.One;

                // SET ScheduleCommand Property of tempStateDown to reference of CommandScheduler.ScheduleCommand:
                (tempStateDown as ICommandSender).ScheduleCommand = (_engineManager.GetService<CommandScheduler>() as ICommandScheduler).ScheduleCommand;

                #endregion

                #endregion


                #region BEHAVIOURS

                #region INSTANTIATIONS

                // DECLARE & INSTANTIATE an IUpdateEventListener as a new PaddleBehaviour(), name it 'behaviourStationary':
                IUpdateEventListener behaviourStationary = (_engineManager.GetService<Factory<IUpdateEventListener>>() as IFactory<IUpdateEventListener>).Create<PaddleBehaviour>();

                // DECLARE & INSTANTIATE an IUpdateEventListener as a new PaddleBehaviour(), name it 'behaviourUp':
                IUpdateEventListener behaviourUp = (_engineManager.GetService<Factory<IUpdateEventListener>>() as IFactory<IUpdateEventListener>).Create<PaddleBehaviour>();

                // DECLARE & INSTANTIATE an IUpdateEventListener as a new PaddleBehaviour(), name it 'behaviourDown':
                IUpdateEventListener behaviourDown = (_engineManager.GetService<Factory<IUpdateEventListener>>() as IFactory<IUpdateEventListener>).Create<PaddleBehaviour>();

                // DECLARE & INSTANTIATE an IUpdateEventListener as a new Animation(), name it 'animationStationary':
                IAnimation animationStationary = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

                // DECLARE & INSTANTIATE an IUpdateEventListener as a new Animation(), name it 'animationUp':
                IAnimation animationUp = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

                // DECLARE & INSTANTIATE an IUpdateEventListener as a new Animation(), name it 'animationDown':
                IAnimation animationDown = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

                #endregion


                #region INITIALISATION

                // SET Direction Property value of behaviourStationary to '0':
                (behaviourStationary as IDirection).Direction = new Vector2(0);

                // SET Direction Property value of behaviourUp to '0, -1':
                (behaviourUp as IDirection).Direction = new Vector2(0, -1);

                // SET Direction Property value of behaviourDown to '0, 1':
                (behaviourDown as IDirection).Direction = new Vector2(0, 1);

                #endregion

                #endregion


                #region ANIMATIONS

                /// STATIONARY

                // SET Texture Property value of animationStationary to "paddleSpriteSheet":
                (animationStationary as ITexture).Texture = Content.Load<Texture2D>("ExampleLevel/paddleSpriteSheet");

                // SET Row Property value of animationStationary to '50, 150':
                animationStationary.SpriteSize = new Point(50, 150);

                // SET Row Property value of animationStationary to '0':
                animationStationary.Row = 0;

                // SET MsPerFrame Property value of animationStationary to '200':
                animationStationary.MsPerFrame = 200;

                /// UP

                // SET Texture Property value of animationUp to "paddleSpriteSheet":
                (animationUp as ITexture).Texture = Content.Load<Texture2D>("ExampleLevel/paddleSpriteSheet");

                // SET Row Property value of animationUp to '50, 150':
                animationUp.SpriteSize = new Point(50, 150);

                // SET Row Property value of animationUp to '1':
                animationUp.Row = 1;

                // SET MsPerFrame Property value of animationUp to '200':
                animationUp.MsPerFrame = 200;

                /// DOWN

                // SET Texture Property value of animationDown to "paddleSpriteSheet":
                (animationDown as ITexture).Texture = Content.Load<Texture2D>("ExampleLevel/paddleSpriteSheet");

                // SET Row Property value of animationDown to '50, 150':
                animationDown.SpriteSize = new Point(50, 150);

                // SET Row Property value of animationDown to '2':
                animationDown.Row = 2;

                // SET MsPerFrame Property value of animationDown to '200':
                animationDown.MsPerFrame = 200;

                #endregion


                #region FURTHER STATE INITIALISATION

                /// STATIONARY

                // INITIALISE tempStateStationary with reference to behaviourStationary:
                (tempStateStationary as IInitialiseParam<IUpdateEventListener>).Initialise(behaviourStationary);

                // INITIALISE tempStateStationary with reference to animationStationary:
                (tempStateStationary as IInitialiseParam<IUpdateEventListener>).Initialise(animationStationary as IUpdateEventListener);

                /// UP

                // INITIALISE tempStateUp with reference to behaviourUp:
                (tempStateUp as IInitialiseParam<IUpdateEventListener>).Initialise(behaviourUp);

                // INITIALISE tempStateUp with reference to animationUp:
                (tempStateUp as IInitialiseParam<IUpdateEventListener>).Initialise(animationUp as IUpdateEventListener);

                /// DOWN

                // INITIALISE tempStateDown with reference to behaviourDown:
                (tempStateDown as IInitialiseParam<IUpdateEventListener>).Initialise(behaviourDown);

                // INITIALISE tempStateDown with reference to animationDown:
                (tempStateDown as IInitialiseParam<IUpdateEventListener>).Initialise(animationDown as IUpdateEventListener);

                #endregion


                #region ENTITY

                #region INSTANTIATION

                // INSTANTIATE new Paddle(), name it "Paddle1":
                entityManager.Create<Paddle>("Paddle1");

                // SUBSCRIBE "Paddle1" to returned KeyboardManager from _engineManager:
                (_engineManager.GetService<KeyboardManager>() as IKeyboardPublisher).Subscribe(entityManager.GetDictionary()["Paddle1"] as IKeyboardListener);

                // SET PlayerIndex of "Paddle1" to PlayerIndex.One:
                (entityManager.GetDictionary()["Paddle1"] as IPlayer).PlayerNum = PlayerIndex.One;

                #endregion


                #region INITIALISATION

                /// STATIONARY

                // INITIALISE "Paddle1" with tempStateStationary:
                (entityManager.GetDictionary()["Paddle1"] as IInitialiseParam<IState>).Initialise(tempStateStationary);

                // INITIALISE "Paddle1" with reference to behaviourStationary:
                (entityManager.GetDictionary()["Paddle1"] as IInitialiseParam<IUpdateEventListener>).Initialise(behaviourStationary);

                // INITIALISE "Paddle1" with reference to animationStationary:
                (entityManager.GetDictionary()["Paddle1"] as IInitialiseParam<IUpdateEventListener>).Initialise(animationStationary as IUpdateEventListener);

                /// UP

                // INITIALISE "Paddle1" with reference to behaviourUp:
                (entityManager.GetDictionary()["Paddle1"] as IInitialiseParam<IUpdateEventListener>).Initialise(behaviourUp);

                // INITIALISE "Paddle1" with reference to animationUp:
                (entityManager.GetDictionary()["Paddle1"] as IInitialiseParam<IUpdateEventListener>).Initialise(animationUp as IUpdateEventListener);

                /// DOWN
                
                // INITIALISE "Paddle1" with reference to behaviourDown:
                (entityManager.GetDictionary()["Paddle1"] as IInitialiseParam<IUpdateEventListener>).Initialise(behaviourDown);

                // INITIALISE "Paddle1" with reference to animationDown:
                (entityManager.GetDictionary()["Paddle1"] as IInitialiseParam<IUpdateEventListener>).Initialise(animationDown as IUpdateEventListener);

                /// OTHER VALUES

                // SET TextureSize Property value of "Paddle1" to a new Point() passing animationStationary.SpriteSize as a parameter:
                (entityManager.GetDictionary()["Paddle1"] as ITexture).TextureSize = new Point(animationStationary.SpriteSize.X, animationStationary.SpriteSize.Y);

                // SET DrawOrigin of "Paddle1" to value of centre of animation.SpriteSize.X / 2:
                (entityManager.GetDictionary()["Paddle1"] as IRotation).DrawOrigin = new Vector2(animationStationary.SpriteSize.X / 2, animationStationary.SpriteSize.Y / 2);

                // SET WindowBorder of "Paddle1" to value of _screenSize:
                (entityManager.GetDictionary()["Paddle1"] as IContainBoundary).WindowBorder = _screenSize;

                // SET Layer of "Paddle1" to 6:
                (entityManager.GetDictionary()["Paddle1"] as ILayer).Layer = 6;

                #endregion

                #endregion


                #region COMMANDS

                /// INSTANTIATION

                // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam(), name it 'stateStationaryChange':
                ICommand stateStationaryChange = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<IState>>();

                // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam(), name it 'stateUpChange':
                ICommand stateUpChange = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<IState>>();

                // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam(), name it 'stateDownChange':
                ICommand stateDownChange = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<IState>>();

                /// INITIALISATION

                // SET MethodRef Property value of stateStationaryChange to reference of "Paddle1"'s SetState() method:
                (stateStationaryChange as ICommandOneParam<IState>).MethodRef = (entityManager.GetDictionary()["Paddle1"] as IEntityInternal).SetState;

                // SET Data Property value of stateStationaryChange to reference of tempStateStationary:
                (stateStationaryChange as ICommandOneParam<IState>).Data = tempStateStationary;

                // SET MethodRef Property value of stateUpChange to reference of "Paddle1"'s SetState() method:
                (stateUpChange as ICommandOneParam<IState>).MethodRef = (entityManager.GetDictionary()["Paddle1"] as IEntityInternal).SetState;

                // SET Data Property value of stateUpChange to reference of tempStateUp:
                (stateUpChange as ICommandOneParam<IState>).Data = tempStateUp;

                // SET MethodRef Property value of stateDownChange to reference of "Paddle1"'s SetState() method:
                (stateDownChange as ICommandOneParam<IState>).MethodRef = (entityManager.GetDictionary()["Paddle1"] as IEntityInternal).SetState;

                // SET Data Property value of stateDownChange to reference of tempStateDown:
                (stateDownChange as ICommandOneParam<IState>).Data = tempStateDown;

                // INITIALISE tempStateStationary with tempStateUp.Name and stateUpChange as parameters:
                (tempStateStationary as IInitialiseParam<string, ICommand>).Initialise((tempStateUp as IName).Name, stateUpChange);

                // INITIALISE tempStateStationary with tempStateDown.Name and stateDownChange as parameters:
                (tempStateStationary as IInitialiseParam<string, ICommand>).Initialise((tempStateDown as IName).Name, stateDownChange);

                // INITIALISE tempStateUp with tempStateStationary.Name and stateStationaryChange as parameters:
                (tempStateUp as IInitialiseParam<string, ICommand>).Initialise((tempStateStationary as IName).Name, stateStationaryChange);

                // INITIALISE tempStateUp with tempStateDown.Name and stateDownChange as parameters:
                (tempStateUp as IInitialiseParam<string, ICommand>).Initialise((tempStateDown as IName).Name, stateDownChange);

                // INITIALISE tempStateDown with tempStateStationary.Name and stateStationaryChange as parameters:
                (tempStateDown as IInitialiseParam<string, ICommand>).Initialise((tempStateStationary as IName).Name, stateStationaryChange);

                // INITIALISE tempStateDown with tempStateUp.Name and stateUpChange as parameters:
                (tempStateDown as IInitialiseParam<string, ICommand>).Initialise((tempStateUp as IName).Name, stateUpChange);


                #endregion

                #endregion


                #region PADDLE 2

                #region STATES

                #region INSTANTIATION

                // INSTANTIATE tempStateStationary as a new PaddleState():
                tempStateStationary = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<PaddleState>();

                // INSTANTIATE tempStateUp as a new PaddleState():
                tempStateUp = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<PaddleState>();

                // INSTANTIATE tempStateDown as a new PaddleState():
                tempStateDown = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<PaddleState>();

                #endregion


                #region INITIALISATION

                /// STATIONARY

                // SET Name Property value of tempStateStationary to "stationary":
                (tempStateStationary as IName).Name = "stationary";

                // INITIALISE tempStateStationary with a new Dictionary<string, ICommand>():
                (tempStateStationary as IInitialiseParam<IDictionary<string, ICommand>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ICommand>>() as IDictionary<string, ICommand>);

                // INITIALISE tempStateStationary with a new Dictionary<string, EventArgs>():
                (tempStateStationary as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

                // INITIALISE tempStateStationary with a new UpdateEventArgs>():
                (tempStateStationary as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

                // SET PlayerIndex of tempStateStationary to PlayerIndex.Two:
                (tempStateStationary as IPlayer).PlayerNum = PlayerIndex.Two;

                // SET ScheduleCommand Property of tempStateStationary to reference of CommandScheduler.ScheduleCommand:
                (tempStateStationary as ICommandSender).ScheduleCommand = (_engineManager.GetService<CommandScheduler>() as ICommandScheduler).ScheduleCommand;

                /// UP

                // SET Name Property value of tempStateUp to "up":
                (tempStateUp as IName).Name = "up";

                // INITIALISE tempStateUp with a new Dictionary<string, ICommand>():
                (tempStateUp as IInitialiseParam<IDictionary<string, ICommand>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ICommand>>() as IDictionary<string, ICommand>);

                // INITIALISE tempStateUp with a new Dictionary<string, EventArgs>():
                (tempStateUp as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

                // INITIALISE tempStateUp with a new UpdateEventArgs>():
                (tempStateUp as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

                // SET PlayerIndex of tempStateUp to PlayerIndex.Two:
                (tempStateUp as IPlayer).PlayerNum = PlayerIndex.Two;

                // SET ScheduleCommand Property of tempStateUp to reference of CommandScheduler.ScheduleCommand:
                (tempStateUp as ICommandSender).ScheduleCommand = (_engineManager.GetService<CommandScheduler>() as ICommandScheduler).ScheduleCommand;

                /// DOWN

                // SET Name Property value of tempStateDown to "down":
                (tempStateDown as IName).Name = "down";

                // INITIALISE tempStateDown with a new Dictionary<string, ICommand>():
                (tempStateDown as IInitialiseParam<IDictionary<string, ICommand>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ICommand>>() as IDictionary<string, ICommand>);

                // INITIALISE tempStateDown with a new Dictionary<string, EventArgs>():
                (tempStateDown as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

                // INITIALISE tempStateDown with a new UpdateEventArgs():
                (tempStateDown as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

                // SET PlayerIndex of tempStateDown to PlayerIndex.Two:
                (tempStateDown as IPlayer).PlayerNum = PlayerIndex.Two;

                // SET ScheduleCommand Property of tempStateDown to reference of CommandScheduler.ScheduleCommand:
                (tempStateDown as ICommandSender).ScheduleCommand = (_engineManager.GetService<CommandScheduler>() as ICommandScheduler).ScheduleCommand;

                #endregion

                #endregion


                #region BEHAVIOURS

                #region INSTANTIATIONS

                // INSTANTIATE behaviourStationary as a new PaddleBehaviour():
                behaviourStationary = (_engineManager.GetService<Factory<IUpdateEventListener>>() as IFactory<IUpdateEventListener>).Create<PaddleBehaviour>();

                // INSTANTIATE behaviourUp as a new PaddleBehaviour():
                behaviourUp = (_engineManager.GetService<Factory<IUpdateEventListener>>() as IFactory<IUpdateEventListener>).Create<PaddleBehaviour>();

                // INSTANTIATE behaviourDown as a new PaddleBehaviour():
                behaviourDown = (_engineManager.GetService<Factory<IUpdateEventListener>>() as IFactory<IUpdateEventListener>).Create<PaddleBehaviour>();

                // INSTANTIATE animationStationary as a new Animation():
                animationStationary = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

                // INSTANTIATE animationUp as a new Animation():
                animationUp = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

                // INSTANTIATE animationDown as a new Animation():
                animationDown = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

                #endregion


                #region INITIALISATION

                // SET Direction Property value of behaviourStationary to '0':
                (behaviourStationary as IDirection).Direction = new Vector2(0);

                // SET Direction Property value of behaviourUp to '0, -1':
                (behaviourUp as IDirection).Direction = new Vector2(0, -1);

                // SET Direction Property value of behaviourDown to '0, 1':
                (behaviourDown as IDirection).Direction = new Vector2(0, 1);

                #endregion

                #endregion


                #region ANIMATIONS

                /// STATIONARY

                // SET Texture Property value of animationStationary to "paddleSpriteSheet":
                (animationStationary as ITexture).Texture = Content.Load<Texture2D>("ExampleLevel/paddleSpriteSheet");

                // SET Row Property value of animationStationary to '50, 150':
                animationStationary.SpriteSize = new Point(50, 150);

                // SET Row Property value of animationStationary to '0':
                animationStationary.Row = 0;

                // SET MsPerFrame Property value of animationStationary to '200':
                animationStationary.MsPerFrame = 200;

                /// UP

                // SET Texture Property value of animationUp to "paddleSpriteSheet":
                (animationUp as ITexture).Texture = Content.Load<Texture2D>("ExampleLevel/paddleSpriteSheet");

                // SET Row Property value of animationUp to '50, 150':
                animationUp.SpriteSize = new Point(50, 150);

                // SET Row Property value of animationUp to '1':
                animationUp.Row = 1;

                // SET MsPerFrame Property value of animationUp to '200':
                animationUp.MsPerFrame = 200;

                /// DOWN

                // SET Texture Property value of animationDown to "paddleSpriteSheet":
                (animationDown as ITexture).Texture = Content.Load<Texture2D>("ExampleLevel/paddleSpriteSheet");

                // SET Row Property value of animationDown to '50, 150':
                animationDown.SpriteSize = new Point(50, 150);

                // SET Row Property value of animationDown to '2':
                animationDown.Row = 2;

                // SET MsPerFrame Property value of animationDown to '200':
                animationDown.MsPerFrame = 200;

                #endregion


                #region FURTHER STATE INITIALISATION

                /// STATIONARY

                // INITIALISE tempStateStationary with reference to behaviourStationary:
                (tempStateStationary as IInitialiseParam<IUpdateEventListener>).Initialise(behaviourStationary);

                // INITIALISE tempStateStationary with reference to animationStationary:
                (tempStateStationary as IInitialiseParam<IUpdateEventListener>).Initialise(animationStationary as IUpdateEventListener);

                /// UP

                // INITIALISE tempStateUp with reference to behaviourUp:
                (tempStateUp as IInitialiseParam<IUpdateEventListener>).Initialise(behaviourUp);

                // INITIALISE tempStateUp with reference to animationUp:
                (tempStateUp as IInitialiseParam<IUpdateEventListener>).Initialise(animationUp as IUpdateEventListener);

                /// DOWN

                // INITIALISE tempStateDown with reference to behaviourDown:
                (tempStateDown as IInitialiseParam<IUpdateEventListener>).Initialise(behaviourDown);

                // INITIALISE tempStateDown with reference to animationDown:
                (tempStateDown as IInitialiseParam<IUpdateEventListener>).Initialise(animationDown as IUpdateEventListener);

                #endregion


                #region ENTITY

                #region INSTANTIATION

                // INSTANTIATE new Paddle(), name it "Paddle2":
                entityManager.Create<Paddle>("Paddle2");

                // SUBSCRIBE "Paddle1" to returned KeyboardManager from _engineManager:
                (_engineManager.GetService<KeyboardManager>() as IKeyboardPublisher).Subscribe(entityManager.GetDictionary()["Paddle2"] as IKeyboardListener);

                // SET PlayerIndex of "Paddle2" to PlayerIndex.Two:
                (entityManager.GetDictionary()["Paddle2"] as IPlayer).PlayerNum = PlayerIndex.Two;

                #endregion


                #region INITIALISATION

                /// STATIONARY

                // INITIALISE "Paddle2" with tempStateStationary:
                (entityManager.GetDictionary()["Paddle2"] as IInitialiseParam<IState>).Initialise(tempStateStationary);

                // INITIALISE "Paddle2" with reference to behaviourStationary:
                (entityManager.GetDictionary()["Paddle2"] as IInitialiseParam<IUpdateEventListener>).Initialise(behaviourStationary);

                // INITIALISE "Paddle2" with reference to animationStationary:
                (entityManager.GetDictionary()["Paddle2"] as IInitialiseParam<IUpdateEventListener>).Initialise(animationStationary as IUpdateEventListener);

                /// UP

                // INITIALISE "Paddle2" with reference to behaviourUp:
                (entityManager.GetDictionary()["Paddle2"] as IInitialiseParam<IUpdateEventListener>).Initialise(behaviourUp);

                // INITIALISE "Paddle2" with reference to animationUp:
                (entityManager.GetDictionary()["Paddle2"] as IInitialiseParam<IUpdateEventListener>).Initialise(animationUp as IUpdateEventListener);

                /// DOWN

                // INITIALISE "Paddle2" with reference to behaviourDown:
                (entityManager.GetDictionary()["Paddle2"] as IInitialiseParam<IUpdateEventListener>).Initialise(behaviourDown);

                // INITIALISE "Paddle2" with reference to animationDown:
                (entityManager.GetDictionary()["Paddle2"] as IInitialiseParam<IUpdateEventListener>).Initialise(animationDown as IUpdateEventListener);

                /// OTHER VALUES

                // SET TextureSize Property value of "Paddle2" to a new Point() passing animationStationary.SpriteSize as a parameter:
                (entityManager.GetDictionary()["Paddle2"] as ITexture).TextureSize = new Point(animationStationary.SpriteSize.X, animationStationary.SpriteSize.Y);

                // SET DrawOrigin of "Paddle2" to value of centre of animation.SpriteSize.X / 2:
                (entityManager.GetDictionary()["Paddle2"] as IRotation).DrawOrigin = new Vector2(animationStationary.SpriteSize.X / 2, animationStationary.SpriteSize.Y / 2);

                // SET WindowBorder of "Paddle2" to value of _screenSize:
                (entityManager.GetDictionary()["Paddle2"] as IContainBoundary).WindowBorder = _screenSize;

                // SET Layer of "Paddle2" to 6:
                (entityManager.GetDictionary()["Paddle2"] as ILayer).Layer = 6;

                #endregion

                #endregion


                #region COMMANDS

                /// INSTANTIATION

                // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam(), name it 'stateStationaryChange':
                stateStationaryChange = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<IState>>();

                // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam(), name it 'stateUpChange':
                stateUpChange = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<IState>>();

                // INSTANTIATE stateDownChange as a new CommandOneParam():
                stateDownChange = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<IState>>();

                /// INITIALISATION

                // SET MethodRef Property value of stateStationaryChange to reference of "Paddle2"'s SetState() method:
                (stateStationaryChange as ICommandOneParam<IState>).MethodRef = (entityManager.GetDictionary()["Paddle2"] as IEntityInternal).SetState;

                // SET Data Property value of stateStationaryChange to reference of tempStateStationary:
                (stateStationaryChange as ICommandOneParam<IState>).Data = tempStateStationary;

                // SET MethodRef Property value of stateUpChange to reference of "Paddle2"'s SetState() method:
                (stateUpChange as ICommandOneParam<IState>).MethodRef = (entityManager.GetDictionary()["Paddle2"] as IEntityInternal).SetState;

                // SET Data Property value of stateUpChange to reference of tempStateUp:
                (stateUpChange as ICommandOneParam<IState>).Data = tempStateUp;

                // SET MethodRef Property value of stateDownChange to reference of "Paddle2"'s SetState() method:
                (stateDownChange as ICommandOneParam<IState>).MethodRef = (entityManager.GetDictionary()["Paddle2"] as IEntityInternal).SetState;

                // SET Data Property value of stateDownChange to reference of tempStateDown:
                (stateDownChange as ICommandOneParam<IState>).Data = tempStateDown;

                // INITIALISE tempStateStationary with tempStateUp.Name and stateUpChange as parameters:
                (tempStateStationary as IInitialiseParam<string, ICommand>).Initialise((tempStateUp as IName).Name, stateUpChange);

                // INITIALISE tempStateStationary with tempStateDown.Name and stateDownChange as parameters:
                (tempStateStationary as IInitialiseParam<string, ICommand>).Initialise((tempStateDown as IName).Name, stateDownChange);

                // INITIALISE tempStateUp with tempStateStationary.Name and stateStationaryChange as parameters:
                (tempStateUp as IInitialiseParam<string, ICommand>).Initialise((tempStateStationary as IName).Name, stateStationaryChange);

                // INITIALISE tempStateUp with tempStateDown.Name and stateDownChange as parameters:
                (tempStateUp as IInitialiseParam<string, ICommand>).Initialise((tempStateDown as IName).Name, stateDownChange);

                // INITIALISE tempStateDown with tempStateStationary.Name and stateStationaryChange as parameters:
                (tempStateDown as IInitialiseParam<string, ICommand>).Initialise((tempStateStationary as IName).Name, stateStationaryChange);

                // INITIALISE tempStateDown with tempStateUp.Name and stateUpChange as parameters:
                (tempStateDown as IInitialiseParam<string, ICommand>).Initialise((tempStateUp as IName).Name, stateUpChange);


                #endregion

                #endregion

                #endregion

                #endregion

                #endregion

                #endregion
            }
            // CATCH ClassDoesNotExistException from Create():
            catch (ClassDoesNotExistException e)
            {
                // PRINT exception message to console:
                Console.WriteLine(e.Message);
            }
            // CATCH ClassDoesNotExistException from Create():
            catch (NullInstanceException e)
            {
                // PRINT exception message to console:
                Console.WriteLine(e.Message);
            }
            // CATCH ClassDoesNotExistException from Create():
            catch (NullValueException e)
            {
                // PRINT exception message to console:
                Console.WriteLine(e.Message);
            }
            // CATCH ValueAlreadyStoredException from Create():
            catch (ValueAlreadyStoredException e)
            {
                // PRINT exception message to console:
                Console.WriteLine(e.Message);
            }

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
            // INSTANTIATE _spriteBatch as new SpriteBatch(), passing GraphicsDevice as a parameter:
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            #region SOUND MANAGER

            /// SONG MANAGER

            // DECLARE & INSTANTIATE an IPlayAudio as a new SongManager(), name it 'songMgr':
            IPlayAudio songMgr = _engineManager.GetService<SongManager>() as IPlayAudio;

            // INITIALISE songMgr with a new Dictionary<string, Song>():
            (songMgr as IInitialiseParam<IDictionary<string, Song>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, Song>>() as IDictionary<string, Song>);

            // INITIALISE songMgr with "MainTrack" and a Song named "MainTrack":
            (songMgr as IInitialiseParam<string, Song>).Initialise("MainTrack", Content.Load<Song>("ExampleLevel/MainTrack"));

            // CALL PlayAudio on songMgr to play "MainTrack":
            songMgr.PlayAudio("MainTrack");

            /// SFX MANAGER

            // DECLARE & INSTANTIATE an IPlayAudio as a new SFXManager(), name it 'sfxMgr':
            IPlayAudio sfxMgr = _engineManager.GetService<SFXManager>() as IPlayAudio;

            // INITIALISE sfxMgr with a new Dictionary<string, SoundEffect>():
            (sfxMgr as IInitialiseParam<IDictionary<string, SoundEffect>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, SoundEffect>>() as IDictionary<string, SoundEffect>);

            // INITIALISE sfxMgr with "WallHit" and a SoundEffect named "WallHit":
            (sfxMgr as IInitialiseParam<string, SoundEffect>).Initialise("WallHit", Content.Load<SoundEffect>("ExampleLevel/WallHit"));

            // INITIALISE sfxMgr with "Score" and a SoundEffect named "Score":
            (sfxMgr as IInitialiseParam<string, SoundEffect>).Initialise("Score", Content.Load<SoundEffect>("ExampleLevel/Score"));

            // INITIALISE sfxMgr with "PaddleHit" and a SoundEffect named "PaddleHit":
            (sfxMgr as IInitialiseParam<string, SoundEffect>).Initialise("PaddleHit", Content.Load<SoundEffect>("ExampleLevel/PaddleHit"));

            #endregion


            #region TILES

            #region LAYER 1 & 2

            #region FLOORS & WALLS

            // DECLARE & INITIALISE an IDictionary<string, IEntity>, name it 'tempEntityDict', give return value of EntityManager.GetDictionary():
            IDictionary<string, IEntity> tempEntityDict = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary();

            // FOREACH IEntity in tempEntityDict.Values:
            foreach (IEntity pEntity in tempEntityDict.Values)
            {
                // IF pEntity is on Layer 1 or Layer 2:
                if ((pEntity as ILayer).Layer == 1 || (pEntity as ILayer).Layer == 2)
                {
                    // SPAWN pEntity in "Level1" at their specified DestinationRectangle:
                    (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level1", pEntity, new Vector2((pEntity as IDrawDestinationRectangle).DestinationRectangle.X, (pEntity as IDrawDestinationRectangle).DestinationRectangle.Y));
                }
            }

            #endregion

            #endregion

            #endregion


            #region LAYER 5

            #region GERALD

            //// DECLARE & INITIALISE an IEntity with reference to "Gerald":
            //IEntity tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["Gerald"];

            //// LOAD "paddleSpriteSheet" texture to "Gerald":
            //(tempEntity as ITexture).Texture = Content.Load<Texture2D>("ExampleLevel/paddleSpriteSheet");

            //// SPAWN "Paddle1" in "Level1" at the far left on the X axis with a gap, and middle on the Y axis:
            //(_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Gerald", tempEntity, new Vector2((tempEntity as IRotation).DrawOrigin.X * 2, _screenSize.Y / 2));

            #endregion


            #region PADDLES

            /// PADDLE 1

            // INITIALISE tempEntity reference to "Paddle1":
            IEntity tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["Paddle1"];

            // LOAD "paddleSpriteSheet" texture to "Paddle1":
            (tempEntity as ITexture).Texture = Content.Load<Texture2D>("ExampleLevel/paddleSpriteSheet");

            // SPAWN "Paddle1" in "Level1" at the far left on the X axis with a gap, and middle on the Y axis:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level1", tempEntity, new Vector2((tempEntity as IRotation).DrawOrigin.X * 2, _screenSize.Y / 2));

            /// PADDLE 2
            
            // INITIALISE tempEntity with reference to "Paddle2":
            tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["Paddle2"];

            // LOAD "paddleSpriteSheet" texture to "Paddle2":
            (tempEntity as ITexture).Texture = Content.Load<Texture2D>("ExampleLevel/paddleSpriteSheet");

            // SPAWN "Paddle2" in "Level1" at the far left on the X axis with a gap, and middle on the Y axis:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level1", tempEntity, new Vector2(_screenSize.X - (tempEntity as IRotation).DrawOrigin.X * 2, _screenSize.Y / 2));
            
            #endregion
            

            #region BALL

            // CALL CreateBall(), creates, initialises and spawns on screen:
            CreateBall();

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
        /// <param name="pGameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime pGameTime)
        {
            // SET colour of screen background as Black:
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // CALL Draw() method on returned SceneManager instance from _engineManager:
            (_engineManager.GetService<SceneManager>() as IDraw).Draw(_spriteBatch);

            // CALL Draw() method from base class:
            base.Draw(pGameTime);
        }

        #endregion


        #region PRIVATE METHODS

        /// <summary>
        /// Creates, initialises, and spawns a Ball object
        /// </summary>
        private void CreateBall()
        {
            // TRY checking if Ball creation, initialisation and spawning, throws any exception:
            try
            {
                #region STATE

                // DECLARE & INSTANTIATE an IState as a new BallState(), name it 'ballState':
                IState ballState = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<BallState>();

                // INITIALISE ballState with a new Dictionary<string, ICommand>():
                (ballState as IInitialiseParam<IDictionary<string, ICommand>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ICommand>>() as IDictionary<string, ICommand>);

                // INITIALISE ballState with a new Dictionary<string, EventArgs>():
                (ballState as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

                // INITIALISE ballState with a new UpdateEventArgs():
                (ballState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

                // INITIALISE ballState with a new CollisionEventArgs():
                (ballState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

                #endregion


                #region BEHAVIOUR

                // DECLARE & INSTANTIE an IUpdateEventListener as a new BallBehaviour(), name it 'ballBehaviour':
                IUpdateEventListener ballBehaviour = (_engineManager.GetService<Factory<IUpdateEventListener>>() as IFactory<IUpdateEventListener>).Create<BallBehaviour>();

                // INITIALISE ballState with reference to ballBehaviour:
                (ballState as IInitialiseParam<IUpdateEventListener>).Initialise(ballBehaviour);

                /// SCORE COMMAND

                // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam<int>(), name it 'tempCommand':
                ICommand tempCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<int>>();

                // SET MethodRef Property of tempCommand to reference of PongReferee.CheckWhoScored():
                (tempCommand as ICommandOneParam<int>).MethodRef = (_engineManager.GetService<PongReferee>() as IPongReferee).CheckWhoScored;

                // SET ScoreGoal Property of ballBehaviour to reference of tempCommand:
                (ballBehaviour as IScoreGoal).ScoreGoal = tempCommand;

                /// AUDIO COMMAND

                // INSTANTIATE tempCommand as a new CommandOneParam<string>():
                tempCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<string>>();

                // SET MethodRef Property of tempCommand to reference of SFXManager.PlayAudio():
                (tempCommand as ICommandOneParam<string>).MethodRef = (_engineManager.GetService<SFXManager>() as IPlayAudio).PlayAudio;

                // SET AudioCommand Property of ballBehaviour to reference of tempCommand:
                (ballBehaviour as IAudioCommand).AudioCommand = tempCommand;

                #endregion


                #region ENTITY

                #region INSTANTIATION

                // DECLARE & GET an instance of EntityManager as an IEntityManager, name it 'entityManager':
                IEntityManager entityManager = _engineManager.GetService<EntityManager>() as IEntityManager;

                // INCREMENT _ballCount by '1':
                _ballCount++;

                // INSTANTIATE new Ball():
                entityManager.Create<Ball>("Ball" + _ballCount);

                #endregion


                #region INITIALISATION

                // INITIALISE ("Ball" + _ballCount) with _rand:
                (entityManager.GetDictionary()["Ball" + _ballCount] as IInitialiseParam<Random>).Initialise(_rand);

                // CALL Reset() on ("Ball" + _ballCount):
                (entityManager.GetDictionary()["Ball" + _ballCount] as IReset).Reset();

                // INITIALISE '"Ball" + _ballCount' with reference to ballState:
                (entityManager.GetDictionary()["Ball" + _ballCount] as IInitialiseParam<IState>).Initialise(ballState);

                // INITIALISE '"Ball" + _ballCount' with reference to ballBehaviour:
                (entityManager.GetDictionary()["Ball" + _ballCount] as IInitialiseParam<IUpdateEventListener>).Initialise(ballBehaviour);

                // SET Layer of ("Ball" + _ballCount) to 6:
                (entityManager.GetDictionary()["Ball" + _ballCount] as ILayer).Layer = 6;

                // SET WindowBorder of "Ball" + _ballCount to value of _screenSize:
                (entityManager.GetDictionary()["Ball" + _ballCount] as IContainBoundary).WindowBorder = _screenSize;

                // SET DrawOrigin of '"Ball" + _ballCount' to value of centre of TextureSize / 2:
                (entityManager.GetDictionary()["Ball" + _ballCount] as IRotation).DrawOrigin = new Vector2((entityManager.GetDictionary()["Ball" + _ballCount] as ITexture).TextureSize.X / 2, (entityManager.GetDictionary()["Ball" + _ballCount] as ITexture).TextureSize.Y / 2);

                #endregion


                #region SPAWNING

                // LOAD "square" texture to '"Ball" + _ballCount':
                (entityManager.GetDictionary()["Ball" + _ballCount] as ITexture).Texture = Content.Load<Texture2D>("ExampleLevel/square");

                // SPAWN "Ball" + _ballCount in "Level1" in the centre of screen:
                (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level1", entityManager.GetDictionary()["Ball" + _ballCount], new Vector2(_screenSize.X / 2, _screenSize.Y / 2));

                #endregion

                #endregion
            }
            // CATCH ClassDoesNotExistException from Create():
            catch (ClassDoesNotExistException e)
            {
                // PRINT exception message to console:
                Console.WriteLine(e.Message);
            }
            // CATCH ClassDoesNotExistException from Create():
            catch (NullInstanceException e)
            {
                // PRINT exception message to console:
                Console.WriteLine(e.Message);
            }
            // CATCH ClassDoesNotExistException from Create():
            catch (NullValueException e)
            {
                // PRINT exception message to console:
                Console.WriteLine(e.Message);
            }
            // CATCH ValueAlreadyStoredException from Create():
            catch (ValueAlreadyStoredException e)
            {
                // PRINT exception message to console:
                Console.WriteLine(e.Message);
            }
        }

        #endregion
    }
}