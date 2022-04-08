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

                #region LAYER 1-5

                #region COMMANDS

                // DECLARE & INSTANTIATE an IFuncCommand<IEntity> as a new FuncCommandOneParam<string, IEntity>(), name it 'createFloor':
                IFuncCommand<IEntity> createFloor = (_engineManager.GetService<Factory<IFuncCommand<IEntity>>>() as IFactory<IFuncCommand<IEntity>>).Create<FuncCommandOneParam<string, IEntity>>();

                // INITIALISE _createFloor's MethodRef Property with EntityManager.Create<DrawableRectangleEntity>:
                (createFloor as IFuncCommandOneParam<string, IEntity>).MethodRef = (_engineManager.GetService<EntityManager>() as IEntityManager).Create<DrawableRectangleEntity>;

                // DECLARE & INSTANTIATE an IFuncCommand<IEntity> as a new FuncCommandOneParam<string, IEntity>(), name it 'createWall':
                IFuncCommand<IEntity> createWall = (_engineManager.GetService<Factory<IFuncCommand<IEntity>>>() as IFactory<IFuncCommand<IEntity>>).Create<FuncCommandOneParam<string, IEntity>>();

                // INITIALISE _createFloor's MethodRef Property with EntityManager.Create<Wall>:
                (createWall as IFuncCommandOneParam<string, IEntity>).MethodRef = (_engineManager.GetService<EntityManager>() as IEntityManager).Create<Wall>;

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


                #region LAYER 6

                #region PLAYER

                #region GERALD

                #region STATES

                #region INSTANTIATION

                // DECLARE & INSTANTIATE an IState as a new PlayerState(), name it 'tempStateStationary':
                IState tempStateStationary = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<PlayerState>();

                // DECLARE & INSTANTIATE an IState as a new PlayerState(), name it 'tempStateUp':
                IState tempStateUp = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<PlayerState>();

                // DECLARE & INSTANTIATE an IState as a new PlayerState(), name it 'tempStateDown':
                IState tempStateDown = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<PlayerState>();

                // DECLARE & INSTANTIATE an IState as a new PlayerState(), name it 'tempStateLeft':
                IState tempStateLeft = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<PlayerState>();

                // DECLARE & INSTANTIATE an IState as a new PlayerState(), name it 'tempStateRight':
                IState tempStateRight = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<PlayerState>();

                // DECLARE & INSTANTIATE an IState as a new PlayerState(), name it 'tempStateUpLeft':
                IState tempStateUpLeft = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<PlayerState>();

                // DECLARE & INSTANTIATE an IState as a new PlayerState(), name it 'tempStateUpRight':
                IState tempStateUpRight = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<PlayerState>();

                // DECLARE & INSTANTIATE an IState as a new PlayerState(), name it 'tempStateDownLeft':
                IState tempStateDownLeft = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<PlayerState>();

                // DECLARE & INSTANTIATE an IState as a new PlayerState(), name it 'tempStateDownRight':
                IState tempStateDownRight = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<PlayerState>();

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

                /// LEFT

                // SET Name Property value of tempStateLeft to "left":
                (tempStateLeft as IName).Name = "left";

                // INITIALISE tempStateLeft with a new Dictionary<string, ICommand>():
                (tempStateLeft as IInitialiseParam<IDictionary<string, ICommand>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ICommand>>() as IDictionary<string, ICommand>);

                // INITIALISE tempStateLeft with a new Dictionary<string, EventArgs>():
                (tempStateLeft as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

                // INITIALISE tempStateLeft with a new UpdateEventArgs():
                (tempStateLeft as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

                // SET PlayerIndex of tempStateLeft to PlayerIndex.One:
                (tempStateLeft as IPlayer).PlayerNum = PlayerIndex.One;

                // SET ScheduleCommand Property of tempStateLeft to reference of CommandScheduler.ScheduleCommand:
                (tempStateLeft as ICommandSender).ScheduleCommand = (_engineManager.GetService<CommandScheduler>() as ICommandScheduler).ScheduleCommand;

                /// RIGHT

                // SET Name Property value of tempStateRight to "right":
                (tempStateRight as IName).Name = "right";

                // INITIALISE tempStateRight with a new Dictionary<string, ICommand>():
                (tempStateRight as IInitialiseParam<IDictionary<string, ICommand>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ICommand>>() as IDictionary<string, ICommand>);

                // INITIALISE tempStateRight  with a new Dictionary<string, EventArgs>():
                (tempStateRight as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

                // INITIALISE tempStateRight with a new UpdateEventArgs():
                (tempStateRight as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

                // SET PlayerIndex of tempStateRight to PlayerIndex.One:
                (tempStateRight as IPlayer).PlayerNum = PlayerIndex.One;

                // SET ScheduleCommand Property of tempStateRight to reference of CommandScheduler.ScheduleCommand:
                (tempStateRight as ICommandSender).ScheduleCommand = (_engineManager.GetService<CommandScheduler>() as ICommandScheduler).ScheduleCommand;

                /// UP-LEFT

                // SET Name Property value of tempStateUpLeft to "up-left":
                (tempStateUpLeft as IName).Name = "up-left";

                // INITIALISE tempStateUpLeft with a new Dictionary<string, ICommand>():
                (tempStateUpLeft as IInitialiseParam<IDictionary<string, ICommand>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ICommand>>() as IDictionary<string, ICommand>);

                // INITIALISE tempStateUpLeft with a new Dictionary<string, EventArgs>():
                (tempStateUpLeft as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

                // INITIALISE tempStateUpLeft with a new UpdateEventArgs():
                (tempStateUpLeft as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

                // SET PlayerIndex of tempStateUpLeft to PlayerIndex.One:
                (tempStateUpLeft as IPlayer).PlayerNum = PlayerIndex.One;

                // SET ScheduleCommand Property of tempStateUpLeft to reference of CommandScheduler.ScheduleCommand:
                (tempStateUpLeft as ICommandSender).ScheduleCommand = (_engineManager.GetService<CommandScheduler>() as ICommandScheduler).ScheduleCommand;

                /// UP-RIGHT

                // SET Name Property value of tempStateUpRight to "up-right":
                (tempStateUpRight as IName).Name = "up-right";

                // INITIALISE tempStateUpRight with a new Dictionary<string, ICommand>():
                (tempStateUpRight as IInitialiseParam<IDictionary<string, ICommand>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ICommand>>() as IDictionary<string, ICommand>);

                // INITIALISE tempStateUpRight with a new Dictionary<string, EventArgs>():
                (tempStateUpRight as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

                // INITIALISE tempStateUpRight with a new UpdateEventArgs():
                (tempStateUpRight as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

                // SET PlayerIndex of tempStateUpRight to PlayerIndex.One:
                (tempStateUpRight as IPlayer).PlayerNum = PlayerIndex.One;

                // SET ScheduleCommand Property of tempStateUpRight to reference of CommandScheduler.ScheduleCommand:
                (tempStateUpRight as ICommandSender).ScheduleCommand = (_engineManager.GetService<CommandScheduler>() as ICommandScheduler).ScheduleCommand;

                /// DOWN-LEFT

                // SET Name Property value of tempStateDownLeft to "down-left":
                (tempStateDownLeft as IName).Name = "down-left";

                // INITIALISE tempStateUpLeft with a new Dictionary<string, ICommand>():
                (tempStateDownLeft as IInitialiseParam<IDictionary<string, ICommand>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ICommand>>() as IDictionary<string, ICommand>);

                // INITIALISE tempStateDownLeft with a new Dictionary<string, EventArgs>():
                (tempStateDownLeft as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

                // INITIALISE tempStateDownLeft with a new UpdateEventArgs():
                (tempStateDownLeft as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

                // SET PlayerIndex of tempStateDownLeft to PlayerIndex.One:
                (tempStateDownLeft as IPlayer).PlayerNum = PlayerIndex.One;

                // SET ScheduleCommand Property of tempStateDownLeft to reference of CommandScheduler.ScheduleCommand:
                (tempStateDownLeft as ICommandSender).ScheduleCommand = (_engineManager.GetService<CommandScheduler>() as ICommandScheduler).ScheduleCommand;

                /// DOWN-RIGHT

                // SET Name Property value of tempStateDownRight to "down-right":
                (tempStateDownRight as IName).Name = "down-right";

                // INITIALISE tempStateDownRight with a new Dictionary<string, ICommand>():
                (tempStateDownRight as IInitialiseParam<IDictionary<string, ICommand>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ICommand>>() as IDictionary<string, ICommand>);

                // INITIALISE tempStateDownRight with a new Dictionary<string, EventArgs>():
                (tempStateDownRight as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

                // INITIALISE tempStateDownRight with a new UpdateEventArgs():
                (tempStateDownRight as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

                // SET PlayerIndex of tempStateDownRight to PlayerIndex.One:
                (tempStateDownRight as IPlayer).PlayerNum = PlayerIndex.One;

                // SET ScheduleCommand Property of tempStateDownRight to reference of CommandScheduler.ScheduleCommand:
                (tempStateDownRight as ICommandSender).ScheduleCommand = (_engineManager.GetService<CommandScheduler>() as ICommandScheduler).ScheduleCommand;

                #endregion

                #endregion


                #region BEHAVIOURS

                #region INSTANTIATIONS

                // DECLARE & INSTANTIATE an IUpdateEventListener as a new PlayerBehaviour(), name it 'behaviourStationary':
                IUpdateEventListener behaviourStationary = (_engineManager.GetService<Factory<IUpdateEventListener>>() as IFactory<IUpdateEventListener>).Create<PlayerBehaviour>();

                // DECLARE & INSTANTIATE an IUpdateEventListener as a new PlayerBehaviour(), name it 'behaviourUp':
                IUpdateEventListener behaviourUp = (_engineManager.GetService<Factory<IUpdateEventListener>>() as IFactory<IUpdateEventListener>).Create<PlayerBehaviour>();

                // DECLARE & INSTANTIATE an IUpdateEventListener as a new PlayerBehaviour(), name it 'behaviourDown':
                IUpdateEventListener behaviourDown = (_engineManager.GetService<Factory<IUpdateEventListener>>() as IFactory<IUpdateEventListener>).Create<PlayerBehaviour>();

                // DECLARE & INSTANTIATE an IUpdateEventListener as a new PlayerBehaviour(), name it 'behaviourLeft':
                IUpdateEventListener behaviourLeft = (_engineManager.GetService<Factory<IUpdateEventListener>>() as IFactory<IUpdateEventListener>).Create<PlayerBehaviour>();

                // DECLARE & INSTANTIATE an IUpdateEventListener as a new PlayerBehaviour(), name it 'behaviourRight':
                IUpdateEventListener behaviourRight = (_engineManager.GetService<Factory<IUpdateEventListener>>() as IFactory<IUpdateEventListener>).Create<PlayerBehaviour>();

                // DECLARE & INSTANTIATE an IUpdateEventListener as a new PlayerBehaviour(), name it 'behaviourUpLeft':
                IUpdateEventListener behaviourUpLeft = (_engineManager.GetService<Factory<IUpdateEventListener>>() as IFactory<IUpdateEventListener>).Create<PlayerBehaviour>();

                // DECLARE & INSTANTIATE an IUpdateEventListener as a new PlayerBehaviour(), name it 'behaviourUpRight':
                IUpdateEventListener behaviourUpRight = (_engineManager.GetService<Factory<IUpdateEventListener>>() as IFactory<IUpdateEventListener>).Create<PlayerBehaviour>();

                // DECLARE & INSTANTIATE an IUpdateEventListener as a new PlayerBehaviour(), name it 'behaviourDownLeft':
                IUpdateEventListener behaviourDownLeft = (_engineManager.GetService<Factory<IUpdateEventListener>>() as IFactory<IUpdateEventListener>).Create<PlayerBehaviour>();

                // DECLARE & INSTANTIATE an IUpdateEventListener as a new PlayerBehaviour(), name it 'behaviourDownRight':
                IUpdateEventListener behaviourDownRight = (_engineManager.GetService<Factory<IUpdateEventListener>>() as IFactory<IUpdateEventListener>).Create<PlayerBehaviour>();

                #endregion


                #region INITIALISATIONS

                // SET Direction Property value of behaviourStationary to '0':
                (behaviourStationary as IDirection).Direction = new Vector2(0);

                // SET Direction Property value of behaviourUp to '0, -1':
                (behaviourUp as IDirection).Direction = new Vector2(0, -1);

                // SET Direction Property value of behaviourDown to '0, 1':
                (behaviourDown as IDirection).Direction = new Vector2(0, 1);

                // SET Direction Property value of behaviourLeft to '-1, 0':
                (behaviourLeft as IDirection).Direction = new Vector2(-1, 0);

                // SET Direction Property value of behaviourRight to '1, 0':
                (behaviourRight as IDirection).Direction = new Vector2(1, 0);

                // SET Direction Property value of behaviourUpLeft to '-1, -1':
                (behaviourUpLeft as IDirection).Direction = new Vector2(-1, -1);

                // SET Direction Property value of behaviourUpLeft to '1, -1':
                (behaviourUpRight as IDirection).Direction = new Vector2(1, -1);

                // SET Direction Property value of behaviourDownLeft to '-1, 1':
                (behaviourDownLeft as IDirection).Direction = new Vector2(-1, 1);

                // SET Direction Property value of behaviourDownRight to '1, 1':
                (behaviourDownRight as IDirection).Direction = new Vector2(1, 1);

                #endregion

                #endregion


                #region ANIMATIONS

                #region INSTANTIATIONS

                // DECLARE & INSTANTIATE an IUpdateEventListener as a new Animation(), name it 'animationStationary':
                IAnimation animationStationary = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

                // DECLARE & INSTANTIATE an IUpdateEventListener as a new Animation(), name it 'animationUp':
                IAnimation animationUp = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

                // DECLARE & INSTANTIATE an IUpdateEventListener as a new Animation(), name it 'animationDown':
                IAnimation animationDown = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

                // DECLARE & INSTANTIATE an IUpdateEventListener as a new Animation(), name it 'animationLeft':
                IAnimation animationLeft = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

                // DECLARE & INSTANTIATE an IUpdateEventListener as a new Animation(), name it 'animationRight':
                IAnimation animationRight = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

                #endregion


                #region INITIALISATIONS

                /// STATIONARY

                // SET Texture Property value of animationStationary to "Geoff":
                (animationStationary as ITexture).Texture = Content.Load<Texture2D>("RIRR/Geoff");

                // SET Row Property value of animationStationary to '15, 22':
                animationStationary.SpriteSize = new Point(15, 22);

                // SET Row Property value of animationStationary to '0':
                animationStationary.Row = 0;

                // SET MsPerFrame Property value of animationStationary to '200':
                animationStationary.MsPerFrame = 200;

                /// UP

                // SET Texture Property value of animationUp to "Geoff":
                (animationUp as ITexture).Texture = Content.Load<Texture2D>("RIRR/Geoff");

                // SET Row Property value of animationUp to '15, 22':
                animationUp.SpriteSize = new Point(15, 22);

                // SET Row Property value of animationUp to '2':
                animationUp.Row = 2;

                // SET MsPerFrame Property value of animationUp to '200':
                animationUp.MsPerFrame = 200;

                /// DOWN

                // SET Texture Property value of animationDown to "Geoff":
                (animationDown as ITexture).Texture = Content.Load<Texture2D>("RIRR/Geoff");

                // SET Row Property value of animationDown to '15, 22':
                animationDown.SpriteSize = new Point(15, 22);

                // SET Row Property value of animationDown to '1':
                animationDown.Row = 1;

                // SET MsPerFrame Property value of animationDown to '200':
                animationDown.MsPerFrame = 200;

                /// LEFT

                // SET Texture Property value of animationLeft to "Geoff":
                (animationLeft as ITexture).Texture = Content.Load<Texture2D>("RIRR/Geoff");

                // SET Row Property value of animationLeft to '15, 22':
                animationLeft.SpriteSize = new Point(15, 22);

                // SET Row Property value of animationLeft to '4':
                animationLeft.Row = 4;

                // SET MsPerFrame Property value of animationLeft to '200':
                animationLeft.MsPerFrame = 200;

                /// RIGHT

                // SET Texture Property value of animationRight to "Geoff":
                (animationRight as ITexture).Texture = Content.Load<Texture2D>("RIRR/Geoff");

                // SET Row Property value of animationRight to '15, 22':
                animationRight.SpriteSize = new Point(15, 22);

                // SET Row Property value of animationRight to '3':
                animationRight.Row = 3;

                // SET MsPerFrame Property value of animationRight to '200':
                animationRight.MsPerFrame = 200;

                #endregion

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

                /// LEFT

                // INITIALISE tempStateLeft with reference to behaviourLeft:
                (tempStateLeft as IInitialiseParam<IUpdateEventListener>).Initialise(behaviourLeft);

                // INITIALISE tempStateLeft with reference to animationLeft:
                (tempStateLeft as IInitialiseParam<IUpdateEventListener>).Initialise(animationLeft as IUpdateEventListener);

                /// RIGHT

                // INITIALISE tempStateRight with reference to behaviourRight:
                (tempStateRight as IInitialiseParam<IUpdateEventListener>).Initialise(behaviourRight);

                // INITIALISE tempStateRight with reference to animationDown:
                (tempStateRight as IInitialiseParam<IUpdateEventListener>).Initialise(animationRight as IUpdateEventListener);

                /// UP-LEFT

                // INITIALISE tempStateUpLeft with reference to behaviourUpLeft:
                (tempStateUpLeft as IInitialiseParam<IUpdateEventListener>).Initialise(behaviourUpLeft);

                // INITIALISE tempStateUpLeft with reference to animationLeft:
                (tempStateUpLeft as IInitialiseParam<IUpdateEventListener>).Initialise(animationLeft as IUpdateEventListener);

                /// UP-RIGHT

                // INITIALISE tempStateUpRight with reference to behaviourUpRight:
                (tempStateUpRight as IInitialiseParam<IUpdateEventListener>).Initialise(behaviourUpRight);

                // INITIALISE tempStateUpRight with reference to animationRight:
                (tempStateUpRight as IInitialiseParam<IUpdateEventListener>).Initialise(animationRight as IUpdateEventListener);

                /// DOWN-LEFT

                // INITIALISE tempStateDownLeft with reference to behaviourDownLeft:
                (tempStateDownLeft as IInitialiseParam<IUpdateEventListener>).Initialise(behaviourDownLeft);

                // INITIALISE tempStateDownLeft with reference to animationLeft:
                (tempStateDownLeft as IInitialiseParam<IUpdateEventListener>).Initialise(animationLeft as IUpdateEventListener);

                /// DOWN-RIGHT

                // INITIALISE tempStateDownRight with reference to behaviourDownRight:
                (tempStateDownRight as IInitialiseParam<IUpdateEventListener>).Initialise(behaviourDownRight);

                // INITIALISE tempStateDownRight with reference to animationRight:
                (tempStateDownRight as IInitialiseParam<IUpdateEventListener>).Initialise(animationRight as IUpdateEventListener);

                #endregion


                #region ENTITY

                #region INSTANTIATION

                // INSTANTIATE new Player(), name it "Gerald":
                entityManager.Create<Player>("Gerald");

                // SUBSCRIBE "Gerald" to returned KeyboardManager from _engineManager:
                (_engineManager.GetService<KeyboardManager>() as IKeyboardPublisher).Subscribe(entityManager.GetDictionary()["Gerald"] as IKeyboardListener);

                // SET PlayerIndex of "Gerald" to PlayerIndex.One:
                (entityManager.GetDictionary()["Gerald"] as IPlayer).PlayerNum = PlayerIndex.One;

                #endregion


                #region INITIALISATION

                /// STATIONARY

                // INITIALISE "Gerald" with tempStateStationary:
                (entityManager.GetDictionary()["Gerald"] as IInitialiseParam<IState>).Initialise(tempStateStationary);

                // INITIALISE "Gerald" with reference to behaviourStationary:
                (entityManager.GetDictionary()["Gerald"] as IInitialiseParam<IUpdateEventListener>).Initialise(behaviourStationary);

                // INITIALISE "Gerald" with reference to animationStationary:
                (entityManager.GetDictionary()["Gerald"] as IInitialiseParam<IUpdateEventListener>).Initialise(animationStationary as IUpdateEventListener);

                /// UP

                // INITIALISE "Gerald" with reference to behaviourUp:
                (entityManager.GetDictionary()["Gerald"] as IInitialiseParam<IUpdateEventListener>).Initialise(behaviourUp);

                // INITIALISE "Gerald" with reference to animationUp:
                (entityManager.GetDictionary()["Gerald"] as IInitialiseParam<IUpdateEventListener>).Initialise(animationUp as IUpdateEventListener);

                /// DOWN
                
                // INITIALISE "Gerald" with reference to behaviourDown:
                (entityManager.GetDictionary()["Gerald"] as IInitialiseParam<IUpdateEventListener>).Initialise(behaviourDown);

                // INITIALISE "Gerald" with reference to animationDown:
                (entityManager.GetDictionary()["Gerald"] as IInitialiseParam<IUpdateEventListener>).Initialise(animationDown as IUpdateEventListener);

                /// LEFT

                // INITIALISE "Gerald" with reference to behaviourLeft:
                (entityManager.GetDictionary()["Gerald"] as IInitialiseParam<IUpdateEventListener>).Initialise(behaviourLeft);

                // INITIALISE "Gerald" with reference to animationLeft:
                (entityManager.GetDictionary()["Gerald"] as IInitialiseParam<IUpdateEventListener>).Initialise(animationLeft as IUpdateEventListener);

                /// RIGHT

                // INITIALISE "Gerald" with reference to behaviourRight:
                (entityManager.GetDictionary()["Gerald"] as IInitialiseParam<IUpdateEventListener>).Initialise(behaviourRight);

                // INITIALISE "Gerald" with reference to animationRight:
                (entityManager.GetDictionary()["Gerald"] as IInitialiseParam<IUpdateEventListener>).Initialise(animationRight as IUpdateEventListener);

                /// UP-LEFT

                // INITIALISE "Gerald" with reference to behaviourUpLeft:
                (entityManager.GetDictionary()["Gerald"] as IInitialiseParam<IUpdateEventListener>).Initialise(behaviourUpLeft);

                /// UP-RIGHT

                // INITIALISE "Gerald" with reference to behaviourUpRight:
                (entityManager.GetDictionary()["Gerald"] as IInitialiseParam<IUpdateEventListener>).Initialise(behaviourUpRight);

                /// DOWN-LEFT

                // INITIALISE "Gerald" with reference to behaviourDownLeft:
                (entityManager.GetDictionary()["Gerald"] as IInitialiseParam<IUpdateEventListener>).Initialise(behaviourDownLeft);

                /// DOWN-RIGHT

                // INITIALISE "Gerald" with reference to behaviourDownRight:
                (entityManager.GetDictionary()["Gerald"] as IInitialiseParam<IUpdateEventListener>).Initialise(behaviourDownRight);

                /// OTHER VALUES

                // SET TextureSize Property value of "Gerald" to a new Point() passing animationStationary.SpriteSize as a parameter:
                (entityManager.GetDictionary()["Gerald"] as ITexture).TextureSize = new Point(animationStationary.SpriteSize.X, animationStationary.SpriteSize.Y);

                // SET DrawOrigin of "Gerald" to value of centre of animation.SpriteSize.X / 2:
                (entityManager.GetDictionary()["Gerald"] as IRotation).DrawOrigin = new Vector2(animationStationary.SpriteSize.X / 2, animationStationary.SpriteSize.Y / 2);

                // SET WindowBorder of "Gerald" to value of _screenSize:
                (entityManager.GetDictionary()["Gerald"] as IContainBoundary).WindowBorder = _screenSize;

                // SET Layer of "Gerald" to 6:
                (entityManager.GetDictionary()["Gerald"] as ILayer).Layer = 6;

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

                // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam(), name it 'stateLeftChange':
                ICommand stateLeftChange = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<IState>>();

                // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam(), name it 'stateRightChange':
                ICommand stateRightChange = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<IState>>();

                // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam(), name it 'stateUpLeftChange':
                ICommand stateUpLeftChange = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<IState>>();

                // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam(), name it 'stateUpRightChange':
                ICommand stateUpRightChange = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<IState>>();

                // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam(), name it 'stateDownLeftChange':
                ICommand stateDownLeftChange = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<IState>>();

                // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam(), name it 'stateDownRightChange':
                ICommand stateDownRightChange = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<IState>>();

                /// INITIALISATION

                /// STATIONARY

                // SET MethodRef Property value of stateStationaryChange to reference of "Gerald"'s SetState() method:
                (stateStationaryChange as ICommandOneParam<IState>).MethodRef = (entityManager.GetDictionary()["Gerald"] as IEntityInternal).SetState;

                // SET Data Property value of stateStationaryChange to reference of tempStateStationary:
                (stateStationaryChange as ICommandOneParam<IState>).Data = tempStateStationary;

                // INITIALISE tempStateStationary with tempStateUp.Name and stateUpChange as parameters:
                (tempStateStationary as IInitialiseParam<string, ICommand>).Initialise((tempStateUp as IName).Name, stateUpChange);

                // INITIALISE tempStateStationary with tempStateDown.Name and stateDownChange as parameters:
                (tempStateStationary as IInitialiseParam<string, ICommand>).Initialise((tempStateDown as IName).Name, stateDownChange);

                // INITIALISE tempStateStationary with tempStateLeft.Name and stateLeftChange as parameters:
                (tempStateStationary as IInitialiseParam<string, ICommand>).Initialise((tempStateLeft as IName).Name, stateLeftChange);

                // INITIALISE tempStateStationary with tempStateRight.Name and stateRightChange as parameters:
                (tempStateStationary as IInitialiseParam<string, ICommand>).Initialise((tempStateRight as IName).Name, stateRightChange);

                // INITIALISE tempStateStationary with tempStateUpLeft.Name and stateUpLeftChange as parameters:
                (tempStateStationary as IInitialiseParam<string, ICommand>).Initialise((tempStateUpLeft as IName).Name, stateUpLeftChange);

                // INITIALISE tempStateStationary with tempStateUpRight.Name and stateUpRightChange as parameters:
                (tempStateStationary as IInitialiseParam<string, ICommand>).Initialise((tempStateUpRight as IName).Name, stateUpRightChange);

                // INITIALISE tempStateStationary with tempStateDownLeft.Name and stateDownLeftChange as parameters:
                (tempStateStationary as IInitialiseParam<string, ICommand>).Initialise((tempStateDownLeft as IName).Name, stateDownLeftChange);

                // INITIALISE tempStateStationary with tempStateDownRight.Name and stateDownRightChange as parameters:
                (tempStateStationary as IInitialiseParam<string, ICommand>).Initialise((tempStateDownRight as IName).Name, stateDownRightChange);

                /// UP

                // SET MethodRef Property value of stateUpChange to reference of "Gerald"'s SetState() method:
                (stateUpChange as ICommandOneParam<IState>).MethodRef = (entityManager.GetDictionary()["Gerald"] as IEntityInternal).SetState;

                // SET Data Property value of stateUpChange to reference of tempStateUp:
                (stateUpChange as ICommandOneParam<IState>).Data = tempStateUp;

                // INITIALISE tempStateUp with tempStateStationary.Name and stateStationaryChange as parameters:
                (tempStateUp as IInitialiseParam<string, ICommand>).Initialise((tempStateStationary as IName).Name, stateStationaryChange);

                // INITIALISE tempStateUp with tempStateDown.Name and stateDownChange as parameters:
                (tempStateUp as IInitialiseParam<string, ICommand>).Initialise((tempStateDown as IName).Name, stateDownChange);

                // INITIALISE tempStateUp with tempStateLeft.Name and stateLeftChange as parameters:
                (tempStateUp as IInitialiseParam<string, ICommand>).Initialise((tempStateLeft as IName).Name, stateLeftChange);

                // INITIALISE tempStateUp with tempStateRight.Name and stateRightChange as parameters:
                (tempStateUp as IInitialiseParam<string, ICommand>).Initialise((tempStateRight as IName).Name, stateRightChange);

                // INITIALISE tempStateUp with tempStateUpLeft.Name and stateUpLeftChange as parameters:
                (tempStateUp as IInitialiseParam<string, ICommand>).Initialise((tempStateUpLeft as IName).Name, stateUpLeftChange);

                // INITIALISE tempStateUp with tempStateUpRight.Name and stateUpRightChange as parameters:
                (tempStateUp as IInitialiseParam<string, ICommand>).Initialise((tempStateUpRight as IName).Name, stateUpRightChange);

                // INITIALISE tempStateUp with tempStateDownLeft.Name and stateDownLeftChange as parameters:
                (tempStateUp as IInitialiseParam<string, ICommand>).Initialise((tempStateDownLeft as IName).Name, stateDownLeftChange);

                // INITIALISE tempStateUp with tempStateDownRight.Name and stateDownRightChange as parameters:
                (tempStateUp as IInitialiseParam<string, ICommand>).Initialise((tempStateDownRight as IName).Name, stateDownRightChange);

                /// DOWN

                // SET MethodRef Property value of stateDownChange to reference of "Gerald"'s SetState() method:
                (stateDownChange as ICommandOneParam<IState>).MethodRef = (entityManager.GetDictionary()["Gerald"] as IEntityInternal).SetState;

                // SET Data Property value of stateDownChange to reference of tempStateDown:
                (stateDownChange as ICommandOneParam<IState>).Data = tempStateDown;

                // INITIALISE tempStateDown with tempStateStationary.Name and stateStationaryChange as parameters:
                (tempStateDown as IInitialiseParam<string, ICommand>).Initialise((tempStateStationary as IName).Name, stateStationaryChange);

                // INITIALISE tempStateDown with tempStateUp.Name and stateUpChange as parameters:
                (tempStateDown as IInitialiseParam<string, ICommand>).Initialise((tempStateUp as IName).Name, stateUpChange);

                // INITIALISE tempStateDown with tempStateLeft.Name and stateLeftChange as parameters:
                (tempStateDown as IInitialiseParam<string, ICommand>).Initialise((tempStateLeft as IName).Name, stateLeftChange);

                // INITIALISE tempStateDown with tempStateRight.Name and stateRightChange as parameters:
                (tempStateDown as IInitialiseParam<string, ICommand>).Initialise((tempStateRight as IName).Name, stateRightChange);

                // INITIALISE tempStateDown with tempStateUpLeft.Name and stateUpLeftChange as parameters:
                (tempStateDown as IInitialiseParam<string, ICommand>).Initialise((tempStateUpLeft as IName).Name, stateUpLeftChange);

                // INITIALISE tempStateDown with tempStateUpRight.Name and stateUpRightChange as parameters:
                (tempStateDown as IInitialiseParam<string, ICommand>).Initialise((tempStateUpRight as IName).Name, stateUpRightChange);

                // INITIALISE tempStateDown with tempStateDownLeft.Name and stateDownLeftChange as parameters:
                (tempStateDown as IInitialiseParam<string, ICommand>).Initialise((tempStateDownLeft as IName).Name, stateDownLeftChange);

                // INITIALISE tempStateDown with tempStateDownRight.Name and stateDownRightChange as parameters:
                (tempStateDown as IInitialiseParam<string, ICommand>).Initialise((tempStateDownRight as IName).Name, stateDownRightChange);

                /// LEFT

                // SET MethodRef Property value of stateLeftChange to reference of "Gerald"'s SetState() method:
                (stateLeftChange as ICommandOneParam<IState>).MethodRef = (entityManager.GetDictionary()["Gerald"] as IEntityInternal).SetState;

                // SET Data Property value of stateLeftChange to reference of tempStateLeft:
                (stateLeftChange as ICommandOneParam<IState>).Data = tempStateLeft;

                // INITIALISE tempStateLeft with tempStateStationary.Name and stateStationaryChange as parameters:
                (tempStateLeft as IInitialiseParam<string, ICommand>).Initialise((tempStateStationary as IName).Name, stateStationaryChange);

                // INITIALISE tempStateLeft with tempStateUp.Name and stateUpChange as parameters:
                (tempStateLeft as IInitialiseParam<string, ICommand>).Initialise((tempStateUp as IName).Name, stateUpChange);

                // INITIALISE tempStateLeft with tempStateDown.Name and stateDownChange as parameters:
                (tempStateLeft as IInitialiseParam<string, ICommand>).Initialise((tempStateDown as IName).Name, stateDownChange);

                // INITIALISE tempStateLeft with tempStateRight.Name and stateRightChange as parameters:
                (tempStateLeft as IInitialiseParam<string, ICommand>).Initialise((tempStateRight as IName).Name, stateRightChange);

                // INITIALISE tempStateLeft with tempStateUpLeft.Name and stateUpLeftChange as parameters:
                (tempStateLeft as IInitialiseParam<string, ICommand>).Initialise((tempStateUpLeft as IName).Name, stateUpLeftChange);

                // INITIALISE tempStateLeft with tempStateUpRight.Name and stateUpRightChange as parameters:
                (tempStateLeft as IInitialiseParam<string, ICommand>).Initialise((tempStateUpRight as IName).Name, stateUpRightChange);

                // INITIALISE tempStateLeft with tempStateDownLeft.Name and stateDownLeftChange as parameters:
                (tempStateLeft as IInitialiseParam<string, ICommand>).Initialise((tempStateDownLeft as IName).Name, stateDownLeftChange);

                // INITIALISE tempStateLeft with tempStateDownRight.Name and stateDownRightChange as parameters:
                (tempStateLeft as IInitialiseParam<string, ICommand>).Initialise((tempStateDownRight as IName).Name, stateDownRightChange);

                /// RIGHT

                // SET MethodRef Property value of stateRightChange to reference of "Gerald"'s SetState() method:
                (stateRightChange as ICommandOneParam<IState>).MethodRef = (entityManager.GetDictionary()["Gerald"] as IEntityInternal).SetState;

                // SET Data Property value of stateRightChange to reference of tempStateRight:
                (stateRightChange as ICommandOneParam<IState>).Data = tempStateRight;

                // INITIALISE tempStateRight with tempStateStationary.Name and stateStationaryChange as parameters:
                (tempStateRight as IInitialiseParam<string, ICommand>).Initialise((tempStateStationary as IName).Name, stateStationaryChange);

                // INITIALISE tempStateRight with tempStateUp.Name and stateUpChange as parameters:
                (tempStateRight as IInitialiseParam<string, ICommand>).Initialise((tempStateUp as IName).Name, stateUpChange);

                // INITIALISE tempStateRight with tempStateDown.Name and stateDownChange as parameters:
                (tempStateRight as IInitialiseParam<string, ICommand>).Initialise((tempStateDown as IName).Name, stateDownChange);

                // INITIALISE tempStateRight with tempStateLeft.Name and stateLeftChange as parameters:
                (tempStateRight as IInitialiseParam<string, ICommand>).Initialise((tempStateLeft as IName).Name, stateLeftChange);

                // INITIALISE tempStateRight with tempStateUpLeft.Name and stateUpLeftChange as parameters:
                (tempStateRight as IInitialiseParam<string, ICommand>).Initialise((tempStateUpLeft as IName).Name, stateUpLeftChange);

                // INITIALISE tempStateRight with tempStateUpRight.Name and stateUpRightChange as parameters:
                (tempStateRight as IInitialiseParam<string, ICommand>).Initialise((tempStateUpRight as IName).Name, stateUpRightChange);

                // INITIALISE tempStateRight with tempStateDownLeft.Name and stateDownLeftChange as parameters:
                (tempStateRight as IInitialiseParam<string, ICommand>).Initialise((tempStateDownLeft as IName).Name, stateDownLeftChange);

                // INITIALISE tempStateRight with tempStateDownRight.Name and stateDownRightChange as parameters:
                (tempStateRight as IInitialiseParam<string, ICommand>).Initialise((tempStateDownRight as IName).Name, stateDownRightChange);

                /// UP-LEFT
                
                // SET MethodRef Property value of stateUpLeftChange to reference of "Gerald"'s SetState() method:
                (stateUpLeftChange as ICommandOneParam<IState>).MethodRef = (entityManager.GetDictionary()["Gerald"] as IEntityInternal).SetState;

                // SET Data Property value of stateUpLeftChange to reference of tempStateUpLeft:
                (stateUpLeftChange as ICommandOneParam<IState>).Data = tempStateUpLeft;

                // INITIALISE tempStateUpLeft with tempStateStationary.Name and stateStationaryChange as parameters:
                (tempStateUpLeft as IInitialiseParam<string, ICommand>).Initialise((tempStateStationary as IName).Name, stateStationaryChange);

                // INITIALISE tempStateUpLeft with tempStateUp.Name and stateUpChange as parameters:
                (tempStateUpLeft as IInitialiseParam<string, ICommand>).Initialise((tempStateUp as IName).Name, stateUpChange);

                // INITIALISE tempStateUpLeft with tempStateDown.Name and stateDownChange as parameters:
                (tempStateUpLeft as IInitialiseParam<string, ICommand>).Initialise((tempStateDown as IName).Name, stateDownChange);

                // INITIALISE tempStateUpLeft with tempStateLeft.Name and stateLeftChange as parameters:
                (tempStateUpLeft as IInitialiseParam<string, ICommand>).Initialise((tempStateLeft as IName).Name, stateLeftChange);

                // INITIALISE tempStateUpLeft with tempStateRight.Name and stateRightChange as parameters:
                (tempStateUpLeft as IInitialiseParam<string, ICommand>).Initialise((tempStateRight as IName).Name, stateRightChange);

                // INITIALISE tempStateUpLeft with tempStateUpRight.Name and stateUpRightChange as parameters:
                (tempStateUpLeft as IInitialiseParam<string, ICommand>).Initialise((tempStateUpRight as IName).Name, stateUpRightChange);

                // INITIALISE tempStateUpLeft with tempStateDownLeft.Name and stateDownLeftChange as parameters:
                (tempStateUpLeft as IInitialiseParam<string, ICommand>).Initialise((tempStateDownLeft as IName).Name, stateDownLeftChange);

                // INITIALISE tempStateUpLeft with tempStateDownRight.Name and stateDownRightChange as parameters:
                (tempStateUpLeft as IInitialiseParam<string, ICommand>).Initialise((tempStateDownRight as IName).Name, stateDownRightChange);

                /// UP-RIGHT

                // SET MethodRef Property value of stateUpRightChange to reference of "Gerald"'s SetState() method:
                (stateUpRightChange as ICommandOneParam<IState>).MethodRef = (entityManager.GetDictionary()["Gerald"] as IEntityInternal).SetState;

                // SET Data Property value of stateUpRightChange to reference of tempStateUpRight:
                (stateUpRightChange as ICommandOneParam<IState>).Data = tempStateUpRight;

                // INITIALISE tempStateUpRight with tempStateStationary.Name and stateStationaryChange as parameters:
                (tempStateUpRight as IInitialiseParam<string, ICommand>).Initialise((tempStateStationary as IName).Name, stateStationaryChange);

                // INITIALISE tempStateUpRight with tempStateUp.Name and stateUpChange as parameters:
                (tempStateUpRight as IInitialiseParam<string, ICommand>).Initialise((tempStateUp as IName).Name, stateUpChange);

                // INITIALISE tempStateUpRight with tempStateDown.Name and stateDownChange as parameters:
                (tempStateUpRight as IInitialiseParam<string, ICommand>).Initialise((tempStateDown as IName).Name, stateDownChange);

                // INITIALISE tempStateUpRight with tempStateLeft.Name and stateLeftChange as parameters:
                (tempStateUpRight as IInitialiseParam<string, ICommand>).Initialise((tempStateLeft as IName).Name, stateLeftChange);

                // INITIALISE tempStateUpRight with tempStateRight.Name and stateRightChange as parameters:
                (tempStateUpRight as IInitialiseParam<string, ICommand>).Initialise((tempStateRight as IName).Name, stateRightChange);

                // INITIALISE tempStateUpRight with tempStateUpLeft.Name and stateUpLeftChange as parameters:
                (tempStateUpRight as IInitialiseParam<string, ICommand>).Initialise((tempStateUpLeft as IName).Name, stateUpLeftChange);

                // INITIALISE tempStateUpRight with tempStateDownLeft.Name and stateDownLeftChange as parameters:
                (tempStateUpRight as IInitialiseParam<string, ICommand>).Initialise((tempStateDownLeft as IName).Name, stateDownLeftChange);

                // INITIALISE tempStateUpRight with tempStateDownRight.Name and stateDownRightChange as parameters:
                (tempStateUpRight as IInitialiseParam<string, ICommand>).Initialise((tempStateDownRight as IName).Name, stateDownRightChange);

                /// DOWN-LEFT

                // SET MethodRef Property value of stateDownLeftChange to reference of "Gerald"'s SetState() method:
                (stateDownLeftChange as ICommandOneParam<IState>).MethodRef = (entityManager.GetDictionary()["Gerald"] as IEntityInternal).SetState;

                // SET Data Property value of stateDownLeftChange to reference of tempStateDownLeft:
                (stateDownLeftChange as ICommandOneParam<IState>).Data = tempStateDownLeft;

                // INITIALISE tempStateDownLeft with tempStateStationary.Name and stateStationaryChange as parameters:
                (tempStateDownLeft as IInitialiseParam<string, ICommand>).Initialise((tempStateStationary as IName).Name, stateStationaryChange);

                // INITIALISE tempStateDownLeft with tempStateUp.Name and stateUpChange as parameters:
                (tempStateDownLeft as IInitialiseParam<string, ICommand>).Initialise((tempStateUp as IName).Name, stateUpChange);

                // INITIALISE tempStateDownLeft with tempStateDown.Name and stateDownChange as parameters:
                (tempStateDownLeft as IInitialiseParam<string, ICommand>).Initialise((tempStateDown as IName).Name, stateDownChange);

                // INITIALISE tempStateDownLeft with tempStateLeft.Name and stateLeftChange as parameters:
                (tempStateDownLeft as IInitialiseParam<string, ICommand>).Initialise((tempStateLeft as IName).Name, stateLeftChange);

                // INITIALISE tempStateDownLeft with tempStateRight.Name and stateRightChange as parameters:
                (tempStateDownLeft as IInitialiseParam<string, ICommand>).Initialise((tempStateRight as IName).Name, stateRightChange);

                // INITIALISE tempStateDownLeft with tempStateUpLeft.Name and stateUpLeftChange as parameters:
                (tempStateDownLeft as IInitialiseParam<string, ICommand>).Initialise((tempStateUpLeft as IName).Name, stateUpLeftChange);

                // INITIALISE tempStateDownLeft with tempStateDownLeft.Name and stateDownLeftChange as parameters:
                (tempStateDownLeft as IInitialiseParam<string, ICommand>).Initialise((tempStateUpRight as IName).Name, stateUpRightChange);

                // INITIALISE tempStateDownLeft with tempStateDownRight.Name and stateDownRightChange as parameters:
                (tempStateDownLeft as IInitialiseParam<string, ICommand>).Initialise((tempStateDownRight as IName).Name, stateDownRightChange);

                /// DOWN-RIGHT

                // SET MethodRef Property value of stateDownRightChange to reference of "Gerald"'s SetState() method:
                (stateDownRightChange as ICommandOneParam<IState>).MethodRef = (entityManager.GetDictionary()["Gerald"] as IEntityInternal).SetState;

                // SET Data Property value of stateDownRightChange to reference of tempStateDownRight:
                (stateDownRightChange as ICommandOneParam<IState>).Data = tempStateDownRight;

                // INITIALISE tempStateDownRight with tempStateStationary.Name and stateStationaryChange as parameters:
                (tempStateDownRight as IInitialiseParam<string, ICommand>).Initialise((tempStateStationary as IName).Name, stateStationaryChange);

                // INITIALISE tempStateDownRight with tempStateUp.Name and stateUpChange as parameters:
                (tempStateDownRight as IInitialiseParam<string, ICommand>).Initialise((tempStateUp as IName).Name, stateUpChange);

                // INITIALISE tempStateDownRight with tempStateDown.Name and stateDownChange as parameters:
                (tempStateDownRight as IInitialiseParam<string, ICommand>).Initialise((tempStateDown as IName).Name, stateDownChange);

                // INITIALISE tempStateDownRight with tempStateLeft.Name and stateLeftChange as parameters:
                (tempStateDownRight as IInitialiseParam<string, ICommand>).Initialise((tempStateLeft as IName).Name, stateLeftChange);

                // INITIALISE tempStateDownRight with tempStateRight.Name and stateRightChange as parameters:
                (tempStateDownRight as IInitialiseParam<string, ICommand>).Initialise((tempStateRight as IName).Name, stateRightChange);

                // INITIALISE tempStateDownRight with tempStateUpLeft.Name and stateUpLeftChange as parameters:
                (tempStateDownRight as IInitialiseParam<string, ICommand>).Initialise((tempStateUpLeft as IName).Name, stateUpLeftChange);

                // INITIALISE tempStateDownRight with tempStateDownRight.Name and stateDownRightChange as parameters:
                (tempStateDownRight as IInitialiseParam<string, ICommand>).Initialise((tempStateUpRight as IName).Name, stateUpRightChange);

                // INITIALISE tempStateDownRight with tempStateDownLeft.Name and stateDownLeftChange as parameters:
                (tempStateDownRight as IInitialiseParam<string, ICommand>).Initialise((tempStateDownLeft as IName).Name, stateDownLeftChange);


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

                // SUBSCRIBE "Gerald" to returned KeyboardManager from _engineManager:
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

            #region SOUND MANAGERS

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


            #region LAYER 6

            #region GERALD

            // INITIALISE tempEntity reference to "Gerald":
            IEntity tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["Gerald"];

            // LOAD "paddleSpriteSheet" texture to "Geoff":
            (tempEntity as ITexture).Texture = Content.Load<Texture2D>("RIRR/Geoff");

            // SPAWN "Gerald" in "Level1" at the far left on the X axis with a gap, and middle on the Y axis:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level1", tempEntity, new Vector2((tempEntity as IRotation).DrawOrigin.X * 2, _screenSize.Y / 2));

            #endregion


            #region GEOFF


            #endregion


            #region PADDLES

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
            // CreateBall();

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