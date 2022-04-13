﻿using COMP3451Project.RIRRPackage.Behaviours;
using COMP3451Project.RIRRPackage.Entities;
using COMP3451Project.RIRRPackage.Entities.Interfaces;
using COMP3451Project.RIRRPackage.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using OrbitalEngine.Animation;
using OrbitalEngine.Animation.Interfaces;
using OrbitalEngine.Audio;
using OrbitalEngine.Audio.Interfaces;
using OrbitalEngine.Behaviours.Interfaces;
using OrbitalEngine.Camera;
using OrbitalEngine.Camera.Behaviours;
using OrbitalEngine.Camera.Interfaces;
using OrbitalEngine.CollisionManagement;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.EntityManagement;
using OrbitalEngine.EntityManagement.Interfaces;
using OrbitalEngine.Exceptions;
using OrbitalEngine.InputManagement;
using OrbitalEngine.InputManagement.Interfaces;
using OrbitalEngine.SceneManagement;
using OrbitalEngine.SceneManagement.Interfaces;
using OrbitalEngine.Services.Commands;
using OrbitalEngine.Services.Commands.Interfaces;
using OrbitalEngine.Services.Factories;
using OrbitalEngine.Services.Factories.Interfaces;
using OrbitalEngine.Services.Interfaces;
using OrbitalEngine.States;
using OrbitalEngine.States.Interfaces;
using OrbitalEngine.Tiles;
using OrbitalEngine.Tiles.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using TiledSharp;

namespace COMP3451Project
{
    /// <summary>
    /// This is the main type for your game.
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 12/04/22
    /// </summary>
    public partial class Kernel : Game, IInitialiseParam<IService>
    {
        #region FIELD VARIABLES

        // DECLARE a GraphicsDeviceManager, name it '_graphics':
        private GraphicsDeviceManager _graphics;

        // DECLARE a SpriteBatch, name it '_spriteBatch':
        private SpriteBatch _spriteBatch;

        // DECLARE an IRtnService, name it '_engineManager':
        private IRtnService _engineManager;

        // DECLARE a Random, name it '_rand':
        private Random _rand;

        // DECLARE a Vector2, name it 'screenSize':
        private Vector2 _screenSize;

        // DECLARE a float, name it '_viewZoom':
        private float _viewZoom;

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
            _screenSize = new Vector2(1600, 900);

            // SET screen width to _screenSize.X:
            _graphics.PreferredBackBufferWidth = (int)_screenSize.X;

            // SET screen height to _screenSize.Y:
            _graphics.PreferredBackBufferHeight = (int)_screenSize.Y;

            // INSTANTIATE _rand as new Random():
            _rand = new Random();

            // INITIALISE _viewZoom with a value of '4':
            _viewZoom = 4;
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

                // INITIALISE _createFloor's MethodRef Property with Factory<ICommand>.Create<CommandOneParam<string>>:
                (createCommand as IFuncCommandZeroParam<ICommand>).MethodRef = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<string>>;

                // INITIALISE entityManager with a reference to createCommand:
                (entityManager as IInitialiseParam<IFuncCommand<ICommand>>).Initialise(createCommand);

                #endregion


                #region SCENE MANAGER

                // DECLARE & GET an instance of SceneManager as an ISceneManager, name it 'sceneManager':
                ISceneManager sceneManager = _engineManager.GetService<SceneManager>() as ISceneManager;

                // INITIALISE engineManager with a new Dictionary<string, ISceneGraph>():
                (sceneManager as IInitialiseParam<IDictionary<string, ISceneGraph>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ISceneGraph>>() as IDictionary<string, ISceneGraph>);

                // INITIALISE engineManager with a new Dictionary<string, ICommand>():
                (sceneManager as IInitialiseParam<IDictionary<string, ICommand>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ICommand>>() as IDictionary<string, ICommand>);

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
                sceneManager.CreateScene("Level1", new CommandZeroParam());

                // INITIALISE sceneManager with a CollisionManager instance from _engineManager, a new Dictionary<string, IEntity>() and a reference to createCommand for scene "Level1":
                sceneManager.Initialise("Level1", _engineManager.GetService<CollisionManager>() as ICollisionManager,
                    (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, IEntity>>() as IDictionary<string, IEntity>, createCommand);


                #region DISPLAYABLE CREATION

                #region LAYER 1-5

                #region COMMANDS

                // DECLARE & INSTANTIATE an IFuncCommand<IEntity> as a new FuncCommandOneParam<string, IEntity>(), name it 'createFloor':
                IFuncCommand<IEntity> createFloor = (_engineManager.GetService<Factory<IFuncCommand<IEntity>>>() as IFactory<IFuncCommand<IEntity>>).Create<FuncCommandOneParam<string, IEntity>>();

                // INITIALISE createFloor's MethodRef Property with EntityManager.Create<DrawableRectangleEntity>:
                (createFloor as IFuncCommandOneParam<string, IEntity>).MethodRef = (_engineManager.GetService<EntityManager>() as IEntityManager).Create<DrawableRectangleEntity>;

                // DECLARE & INSTANTIATE an IFuncCommand<IEntity> as a new FuncCommandOneParam<string, IEntity>(), name it 'createSimpleCollidableEntity':
                IFuncCommand<IEntity> createSimpleCollidableEntity = (_engineManager.GetService<Factory<IFuncCommand<IEntity>>>() as IFactory<IFuncCommand<IEntity>>).Create<FuncCommandOneParam<string, IEntity>>();

                // INITIALISE createSimpleCollidableEntity's MethodRef Property with EntityManager.Create<SimpleCollidableEntity>:
                (createSimpleCollidableEntity as IFuncCommandOneParam<string, IEntity>).MethodRef = (_engineManager.GetService<EntityManager>() as IEntityManager).Create<SimpleCollidableEntity>;

                // DECLARE & INSTANTIATE an IFuncCommand<IEntity> as a new FuncCommandOneParam<string, IEntity>(), name it 'createDynamicCollidableEntity':
                IFuncCommand<IEntity> createDynamicCollidableEntity = (_engineManager.GetService<Factory<IFuncCommand<IEntity>>>() as IFactory<IFuncCommand<IEntity>>).Create<FuncCommandOneParam<string, IEntity>>();

                // INITIALISE createDynamicCollidableEntity's MethodRef Property with EntityManager.Create<DynamicCollidableEntity>:
                (createDynamicCollidableEntity as IFuncCommandOneParam<string, IEntity>).MethodRef = (_engineManager.GetService<EntityManager>() as IEntityManager).Create<DynamicCollidableEntity>;

                // DECLARE & INSTANTIATE an IFuncCommand<IEntity> as a new FuncCommandOneParam<string, IEntity>(), name it 'createPlayer':
                IFuncCommand<IEntity> createPlayer = (_engineManager.GetService<Factory<IFuncCommand<IEntity>>>() as IFactory<IFuncCommand<IEntity>>).Create<FuncCommandOneParam<string, IEntity>>();

                // INITIALISE createPlayer's MethodRef Property with EntityManager.Create<Player>:
                (createPlayer as IFuncCommandOneParam<string, IEntity>).MethodRef = (_engineManager.GetService<EntityManager>() as IEntityManager).Create<Player>;

                // DECLARE & INSTANTIATE an IFuncCommand<IEntity> as a new FuncCommandOneParam<string, IEntity>(), name it 'createNPC':
                IFuncCommand<IEntity> createNPC = (_engineManager.GetService<Factory<IFuncCommand<IEntity>>>() as IFactory<IFuncCommand<IEntity>>).Create<FuncCommandOneParam<string, IEntity>>();

                // INITIALISE createNPC's MethodRef Property with EntityManager.Create<NPC>:
                (createNPC as IFuncCommandOneParam<string, IEntity>).MethodRef = (_engineManager.GetService<EntityManager>() as IEntityManager).Create<NPC>;

                #endregion

                // DECLARE & INSTANTIATE an ILevelLayoutMaker as a new LevelLayoutMaker(), name it 'levelLM':
                ILevelLayoutMaker levelLM = _engineManager.GetService<LevelLayoutMaker>() as ILevelLayoutMaker;

                // INITIALISE levelLM with a new Dictionary<string, IEntity>():
                (levelLM as IInitialiseParam<IDictionary<string, IFuncCommand<IEntity>>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, IFuncCommand<IEntity>>>() as IDictionary<string, IFuncCommand<IEntity>>);

                // INITIALISE levelLM with "Floor" and createFloor as parameters:
                (levelLM as IInitialiseParam<string, IFuncCommand<IEntity>>).Initialise("Floor", createFloor);

                // INITIALISE levelLM with "SimpleCollidableEntity" and createSimpleCollidableEntity as parameters:
                (levelLM as IInitialiseParam<string, IFuncCommand<IEntity>>).Initialise("SimpleCollidableEntity", createSimpleCollidableEntity);

                // INITIALISE levelLM with "DynamicCollidableEntity" and createDynamicCollidableEntity as parameters:
                (levelLM as IInitialiseParam<string, IFuncCommand<IEntity>>).Initialise("DynamicCollidableEntity", createDynamicCollidableEntity);

                // INITIALISE levelLM with "Player" and createPlayer as parameters:
                (levelLM as IInitialiseParam<string, IFuncCommand<IEntity>>).Initialise("Player", createPlayer);

                // INITIALISE levelLM with "NPC" and createNPC as parameters:
                (levelLM as IInitialiseParam<string, IFuncCommand<IEntity>>).Initialise("NPC", createNPC);

                // DECLARE & INSTANTIATE a new TmxMap(), name it '_map', passing a .tmx file as a parameter:
                TmxMap map = new TmxMap("..\\..\\..\\..\\Content\\RIRR\\Levels\\Level1.tmx");

                // DECLARE & INITIALISE a Texture2D, name it '_tilesetTex', give value of _map's Tilesets[0]'s name:
                Texture2D tilesetTex = Content.Load<Texture2D>("RIRR\\Levels\\Tiles\\" + map.Tilesets[0].Name);

                // CALL CreateLevelLayout() on levelLM, passing "Level1", _map and _tilesetTex as parameters:
                levelLM.CreateLevelLayout("Level1", map, tilesetTex);


                #region LAYER 4 - ITEMS

                #region ARTEFACT

                #region STATES

                // DECLARE & INSTANTIATE an IState as a new CollidableState(), name it 'artefactState':
                IState artefactState = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<CollidableState>();

                // INITIALISE artefactState with a new Dictionary<string, ICommand>():
                (artefactState as IInitialiseParam<IDictionary<string, ICommand>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ICommand>>() as IDictionary<string, ICommand>);

                // INITIALISE artefactState with a new Dictionary<string, EventArgs>():
                (artefactState as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

                // INITIALISE artefactState with a new CollisionEventArgs():
                (artefactState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

                #endregion


                #region BEHAVIOUR

                // DECLARE & INSTANTIATE an IBehaviour as a new ArtefactBehaviour(), name it 'artefactBehaviour':
                IEventListener<CollisionEventArgs> artefactBehaviour = (_engineManager.GetService<Factory<IEventListener<CollisionEventArgs>>>() as IFactory<IEventListener<CollisionEventArgs>>).Create<ArtefactBehaviour>();

                // INITIALISE artefactState with a reference to artefactBehaviour:
                (artefactState as IInitialiseParam<IEventListener<CollisionEventArgs>>).Initialise(artefactBehaviour);

                #endregion


                #region ENTITY

                // INITIALISE "Item450" with a reference to artefactState:
                // NAMED 450 DUE TO PLACEMENT IN TILED FILE
                (entityManager.GetDictionary()["Item450"] as IInitialiseParam<IState>).Initialise(artefactState);

                // INITIALISE "Item450" with reference to artefactBehaviour:
                (entityManager.GetDictionary()["Item450"] as IInitialiseParam<IEventListener<CollisionEventArgs>>).Initialise(artefactBehaviour);

                #endregion

                #endregion

                #endregion


                #region LAYER 5 - LEVEL CHANGE

                #region LEVEL CHANGE

                #region STATES

                // DECLARE & INSTANTIATE an IState as a new CollidableState(), name it 'levelChangeState':
                IState levelChangeState = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<CollidableState>();

                // INITIALISE levelChangeState with a new Dictionary<string, ICommand>():
                (levelChangeState as IInitialiseParam<IDictionary<string, ICommand>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ICommand>>() as IDictionary<string, ICommand>);

                // INITIALISE levelChangeState with a new Dictionary<string, EventArgs>():
                (levelChangeState as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

                // INITIALISE levelChangeState with a new CollisionEventArgs():
                (levelChangeState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

                #endregion


                #region BEHAVIOUR

                // DECLARE & INSTANTIATE an IBehaviour as a new LevelChangeBehaviour(), name it 'levelChangeBehaviour':
                IEventListener<CollisionEventArgs> levelChangeBehaviour = (_engineManager.GetService<Factory<IEventListener<CollisionEventArgs>>>() as IFactory<IEventListener<CollisionEventArgs>>).Create<LevelChangeBehaviour>();

                // INITIALISE artefactState with a reference to levelChangeBehaviour:
                (levelChangeState as IInitialiseParam<IEventListener<CollisionEventArgs>>).Initialise(levelChangeBehaviour);

                // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam<string>, name it 'nextLevelCommand':
                ICommand nextLevelCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<string>>();

                // INITIALISE nextLevelCommand's MethodRef Property with a reference to sceneManager.ReturnCurrentScene().GoToNextScene:
                (nextLevelCommand as ICommandOneParam<string>).MethodRef = sceneManager.ReturnCurrentScene().GoToNextScene;

                // INITIALISE levelChangeBehaviour with a reference to levelChangeCommand:
                (levelChangeBehaviour as IInitialiseParam<ICommand>).Initialise(nextLevelCommand);

                #endregion


                #region ENTITY

                // INITIALISE "LevelChange38" with a reference to levelChangeState:
                // NAMED 38 DUE TO PLACEMENT IN TILED FILE
                (entityManager.GetDictionary()["LevelChange38"] as IInitialiseParam<IState>).Initialise(levelChangeState);

                // INITIALISE "LevelChange38" with reference to levelChangeBehaviour:
                (entityManager.GetDictionary()["LevelChange38"] as IInitialiseParam<IEventListener<CollisionEventArgs>>).Initialise(levelChangeBehaviour);

                #endregion

                #endregion

                #endregion

                #endregion


                #region LAYER 6 - PLAYER / NPC

                #region CAMERA

                #region STATES

                // DECLARE & INSTANTIATE an IState as a new UpdatableState(), name it 'camState':
                IState camState = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<UpdatableState>();

                // INITIALISE camState with a new Dictionary<string, EventArgs>():
                (camState as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

                // INITIALISE camState with a new UpdateEventArgs():
                (camState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

                #endregion


                #region BEHAVIOUR

                // DECLARE & INSTANTIATE an IEventListener<UpdateEventArgs> as a new CameraBehaviour(), name it 'camBehaviour':
                IEventListener<UpdateEventArgs> camBehaviour = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<CameraBehaviour>();

                // INITIALISE camBehaviour with a new MatrixEventArgs:
                (camBehaviour as IInitialiseParam<MatrixEventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<MatrixEventArgs>() as MatrixEventArgs);

                // INITIALISE camBehaviour with a reference to the current scene's MatrixEventArgs OnEvent() method:
                (camBehaviour as IInitialiseParam<EventHandler<MatrixEventArgs>>).Initialise((sceneManager.ReturnCurrentScene() as IEventListener<MatrixEventArgs>).OnEvent);

                // INITIALISE camState with a reference to camBehaviour:
                (camState as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(camBehaviour);

                #endregion


                #region ENTITY

                // DECLARE & INSTANTIATE an ICamera as a new Camera, name it 'camera':
                ICamera camera = (_engineManager.GetService<EntityManager>() as IEntityManager).Create<Camera>("Camera") as ICamera;

                // INITIALISE camera's WindowBorder Property with value of _screenSize:
                (camera as IContainBoundary).WindowBorder = _screenSize;

                // INITIALISE camera's Zoom Property with value of _viewZoom:
                (camera as IZoom).Zoom = _viewZoom;

                // INITIALISE _camera with a reference to camState:
                (camera as IInitialiseParam<IState>).Initialise(camState);

                // INITIALISE camera with reference to camBehaviour:
                (camera as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(camBehaviour);

                // DECLARE & INSTANTIATE an ICommand as a new CommandTwoParam<Vector2, Vector2>(), name it 'camPosChangeCommand':
                ICommand camPosChangeCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<Vector2, Vector2>>();

                // INITIALISE camPosChangeCommand's MethodRef Property with reference to camera.ChangeCamPos():
                (camPosChangeCommand as ICommandTwoParam<Vector2, Vector2>).MethodRef = camera.ChangeCamPos;

                #endregion

                #endregion


                #region PLAYER 1

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

                // INITIALISE tempStateStationary with a new CollisionEventArgs():
                (tempStateStationary as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

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

                // INITIALISE tempStateUp with a new CollisionEventArgs():
                (tempStateUp as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

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

                // INITIALISE tempStateDown with a new CollisionEventArgs():
                (tempStateDown as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

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

                // INITIALISE tempStateLeft with a new CollisionEventArgs():
                (tempStateLeft as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

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

                // INITIALISE tempStateRight with a new CollisionEventArgs():
                (tempStateRight as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

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

                // INITIALISE tempStateUpLeft with a new CollisionEventArgs():
                (tempStateUpLeft as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

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

                // INITIALISE tempStateUpRight with a new CollisionEventArgs():
                (tempStateUpRight as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

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

                // INITIALISE tempStateDownLeft with a new CollisionEventArgs():
                (tempStateDownLeft as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

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

                // INITIALISE tempStateDownRight with a new CollisionEventArgs():
                (tempStateDownRight as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

                // SET PlayerIndex of tempStateDownRight to PlayerIndex.One:
                (tempStateDownRight as IPlayer).PlayerNum = PlayerIndex.One;

                // SET ScheduleCommand Property of tempStateDownRight to reference of CommandScheduler.ScheduleCommand:
                (tempStateDownRight as ICommandSender).ScheduleCommand = (_engineManager.GetService<CommandScheduler>() as ICommandScheduler).ScheduleCommand;

                #endregion

                #endregion


                #region BEHAVIOURS

                #region INSTANTIATIONS

                // DECLARE & INSTANTIATE an IEventListener<UpdateEventArgs> as a new PlayerBehaviour(), name it 'behaviourStationary':
                IEventListener<UpdateEventArgs> behaviourStationary = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<PlayerBehaviour>();

                // DECLARE & INSTANTIATE an IEventListener<UpdateEventArgs> as a new PlayerBehaviour(), name it 'behaviourUp':
                IEventListener<UpdateEventArgs> behaviourUp = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<PlayerBehaviour>();

                // DECLARE & INSTANTIATE an IEventListener<UpdateEventArgs> as a new PlayerBehaviour(), name it 'behaviourDown':
                IEventListener<UpdateEventArgs> behaviourDown = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<PlayerBehaviour>();

                // DECLARE & INSTANTIATE an IEventListener<UpdateEventArgs> as a new PlayerBehaviour(), name it 'behaviourLeft':
                IEventListener<UpdateEventArgs> behaviourLeft = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<PlayerBehaviour>();

                // DECLARE & INSTANTIATE an IEventListener<UpdateEventArgs> as a new PlayerBehaviour(), name it 'behaviourRight':
                IEventListener<UpdateEventArgs> behaviourRight = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<PlayerBehaviour>();

                // DECLARE & INSTANTIATE an IEventListener<UpdateEventArgs> as a new PlayerBehaviour(), name it 'behaviourUpLeft':
                IEventListener<UpdateEventArgs> behaviourUpLeft = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<PlayerBehaviour>();

                // DECLARE & INSTANTIATE an IEventListener<UpdateEventArgs> as a new PlayerBehaviour(), name it 'behaviourUpRight':
                IEventListener<UpdateEventArgs> behaviourUpRight = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<PlayerBehaviour>();

                // DECLARE & INSTANTIATE an IEventListener<UpdateEventArgs> as a new PlayerBehaviour(), name it 'behaviourDownLeft':
                IEventListener<UpdateEventArgs> behaviourDownLeft = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<PlayerBehaviour>();

                // DECLARE & INSTANTIATE an IEventListener<UpdateEventArgs> as a new PlayerBehaviour(), name it 'behaviourDownRight':
                IEventListener<UpdateEventArgs> behaviourDownRight = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<PlayerBehaviour>();

                // DECLARE & INSTANTIATE an IEventListener<UpdateEventArgs> as a new PlayerBehaviour(), name it 'healthBehaviour':
                IEventListener<UpdateEventArgs> healthBehaviour = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<HealthBehaviour>();

                #endregion


                #region INITIALISATIONS

                // SET Direction Property value of behaviourStationary to '0':
                (behaviourStationary as IDirection).Direction = new Vector2(0);

                // INITIALISE behaviourStationary with reference to camPosChangeCommand:
                (behaviourStationary as IInitialiseParam<ICommand>).Initialise(camPosChangeCommand);

                // SET Direction Property value of behaviourUp to '0, -1':
                (behaviourUp as IDirection).Direction = new Vector2(0, -1);

                // INITIALISE behaviourUp with reference to camPosChangeCommand:
                (behaviourUp as IInitialiseParam<ICommand>).Initialise(camPosChangeCommand);

                // SET Direction Property value of behaviourDown to '0, 1':
                (behaviourDown as IDirection).Direction = new Vector2(0, 1);

                // INITIALISE behaviourDown with reference to camPosChangeCommand:
                (behaviourDown as IInitialiseParam<ICommand>).Initialise(camPosChangeCommand);

                // SET Direction Property value of behaviourLeft to '-1, 0':
                (behaviourLeft as IDirection).Direction = new Vector2(-1, 0);

                // INITIALISE behaviourLeft with reference to camPosChangeCommand:
                (behaviourLeft as IInitialiseParam<ICommand>).Initialise(camPosChangeCommand);

                // SET Direction Property value of behaviourRight to '1, 0':
                (behaviourRight as IDirection).Direction = new Vector2(1, 0);

                // INITIALISE behaviourRight with reference to camPosChangeCommand:
                (behaviourRight as IInitialiseParam<ICommand>).Initialise(camPosChangeCommand);

                // SET Direction Property value of behaviourUpLeft to '-1, -1':
                (behaviourUpLeft as IDirection).Direction = new Vector2(-1, -1);

                // INITIALISE behaviourUpLeft with reference to camPosChangeCommand:
                (behaviourUpLeft as IInitialiseParam<ICommand>).Initialise(camPosChangeCommand);

                // SET Direction Property value of behaviourUpLeft to '1, -1':
                (behaviourUpRight as IDirection).Direction = new Vector2(1, -1);

                // INITIALISE behaviourUpRight with reference to camPosChangeCommand:
                (behaviourUpRight as IInitialiseParam<ICommand>).Initialise(camPosChangeCommand);

                // SET Direction Property value of behaviourDownLeft to '-1, 1':
                (behaviourDownLeft as IDirection).Direction = new Vector2(-1, 1);

                // INITIALISE behaviourDownLeft with reference to camPosChangeCommand:
                (behaviourDownLeft as IInitialiseParam<ICommand>).Initialise(camPosChangeCommand);

                // SET Direction Property value of behaviourDownRight to '1, 1':
                (behaviourDownRight as IDirection).Direction = new Vector2(1, 1);

                // INITIALISE behaviourDownRight with reference to camPosChangeCommand:
                (behaviourDownRight as IInitialiseParam<ICommand>).Initialise(camPosChangeCommand);



                #endregion

                #endregion


                #region ANIMATIONS

                #region INSTANTIATIONS

                // DECLARE & INSTANTIATE an IAnimation as a new Animation(), name it 'animationStationary':
                IAnimation animationStationary = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

                // DECLARE & INSTANTIATE an IAnimation as a new Animation(), name it 'animationUp':
                IAnimation animationUp = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

                // DECLARE & INSTANTIATE an IAnimation as a new Animation(), name it 'animationDown':
                IAnimation animationDown = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

                // DECLARE & INSTANTIATE an IAnimation as a new Animation(), name it 'animationLeft':
                IAnimation animationLeft = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

                // DECLARE & INSTANTIATE an IAnimation as a new Animation(), name it 'animationRight':
                IAnimation animationRight = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

                #endregion


                #region INITIALISATIONS

                /// STATIONARY

                // SET Texture Property value of animationStationary to "Gerald":
                (animationStationary as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Gerald");

                // SET Row Property value of animationStationary to '15, 22':
                animationStationary.SpriteSize = new Point(14, 18);

                // SET Row Property value of animationStationary to '0':
                animationStationary.Row = 0;

                // SET MsPerFrame Property value of animationStationary to '175':
                animationStationary.MsPerFrame = 175;

                /// UP

                // SET Texture Property value of animationUp to "Gerald":
                (animationUp as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Gerald");

                // SET Row Property value of animationUp to '14, 18':
                animationUp.SpriteSize = new Point(14, 18);

                // SET Row Property value of animationUp to '2':
                animationUp.Row = 2;

                // SET MsPerFrame Property value of animationUp to '175':
                animationUp.MsPerFrame = 175;

                /// DOWN

                // SET Texture Property value of animationDown to "Gerald":
                (animationDown as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Gerald");

                // SET Row Property value of animationDown to '14, 18':
                animationDown.SpriteSize = new Point(14, 18);

                // SET Row Property value of animationDown to '1':
                animationDown.Row = 1;

                // SET MsPerFrame Property value of animationDown to '175':
                animationDown.MsPerFrame = 175;

                /// LEFT

                // SET Texture Property value of animationLeft to "Gerald":
                (animationLeft as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Gerald");

                // SET Row Property value of animationLeft to '14, 18':
                animationLeft.SpriteSize = new Point(14, 18);

                // SET Row Property value of animationLeft to '4':
                animationLeft.Row = 4;

                // SET MsPerFrame Property value of animationLeft to '175':
                animationLeft.MsPerFrame = 175;

                /// RIGHT

                // SET Texture Property value of animationRight to "Gerald":
                (animationRight as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Gerald");

                // SET Row Property value of animationRight to '14, 18':
                animationRight.SpriteSize = new Point(14, 18);

                // SET Row Property value of animationRight to '3':
                animationRight.Row = 3;

                // SET MsPerFrame Property value of animationRight to '175':
                animationRight.MsPerFrame = 175;

                #endregion

                #endregion


                #region FURTHER STATE INITIALISATION

                /// STATIONARY

                // INITIALISE tempStateStationary with reference to behaviourStationary:
                (tempStateStationary as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(behaviourStationary);

                // INITIALISE tempStateStationary with reference to animationStationary:
                (tempStateStationary as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationStationary as IEventListener<UpdateEventArgs>);

                // INITIALISE tempStateStationary with references to healthBehaviour Events for Update and Collision:
                (tempStateStationary as IInitialiseParam<EventHandler<UpdateEventArgs>, EventHandler<CollisionEventArgs>>).Initialise(healthBehaviour.OnEvent, (healthBehaviour as IEventListener<CollisionEventArgs>).OnEvent);

                /// UP

                // INITIALISE tempStateUp with reference to behaviourUp:
                (tempStateUp as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(behaviourUp);

                // INITIALISE tempStateUp with reference to animationUp:
                (tempStateUp as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationUp as IEventListener<UpdateEventArgs>);

                // INITIALISE tempStateUp with references to healthBehaviour Events for Update and Collision:
                (tempStateUp as IInitialiseParam<EventHandler<UpdateEventArgs>, EventHandler<CollisionEventArgs>>).Initialise(healthBehaviour.OnEvent, (healthBehaviour as IEventListener<CollisionEventArgs>).OnEvent);

                /// DOWN

                // INITIALISE tempStateDown with reference to behaviourDown:
                (tempStateDown as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(behaviourDown);

                // INITIALISE tempStateDown with reference to animationDown:
                (tempStateDown as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationDown as IEventListener<UpdateEventArgs>);

                // INITIALISE tempStateDown with references to healthBehaviour Events for Update and Collision:
                (tempStateDown as IInitialiseParam<EventHandler<UpdateEventArgs>, EventHandler<CollisionEventArgs>>).Initialise(healthBehaviour.OnEvent, (healthBehaviour as IEventListener<CollisionEventArgs>).OnEvent);

                /// LEFT

                // INITIALISE tempStateLeft with reference to behaviourLeft:
                (tempStateLeft as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(behaviourLeft);

                // INITIALISE tempStateLeft with reference to animationLeft:
                (tempStateLeft as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationLeft as IEventListener<UpdateEventArgs>);

                // INITIALISE tempStateLeft with references to healthBehaviour Events for Update and Collision:
                (tempStateLeft as IInitialiseParam<EventHandler<UpdateEventArgs>, EventHandler<CollisionEventArgs>>).Initialise(healthBehaviour.OnEvent, (healthBehaviour as IEventListener<CollisionEventArgs>).OnEvent);

                /// RIGHT

                // INITIALISE tempStateRight with reference to behaviourRight:
                (tempStateRight as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(behaviourRight);

                // INITIALISE tempStateRight with reference to animationDown:
                (tempStateRight as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationRight as IEventListener<UpdateEventArgs>);

                // INITIALISE tempStateRight with references to healthBehaviour Events for Update and Collision:
                (tempStateRight as IInitialiseParam<EventHandler<UpdateEventArgs>, EventHandler<CollisionEventArgs>>).Initialise(healthBehaviour.OnEvent, (healthBehaviour as IEventListener<CollisionEventArgs>).OnEvent);

                /// UP-LEFT

                // INITIALISE tempStateUpLeft with reference to behaviourUpLeft:
                (tempStateUpLeft as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(behaviourUpLeft);

                // INITIALISE tempStateUpLeft with reference to animationLeft:
                (tempStateUpLeft as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationLeft as IEventListener<UpdateEventArgs>);

                // INITIALISE tempStateUpLeft with references to healthBehaviour Events for Update and Collision:
                (tempStateUpLeft as IInitialiseParam<EventHandler<UpdateEventArgs>, EventHandler<CollisionEventArgs>>).Initialise(healthBehaviour.OnEvent, (healthBehaviour as IEventListener<CollisionEventArgs>).OnEvent);

                /// UP-RIGHT

                // INITIALISE tempStateUpRight with reference to behaviourUpRight:
                (tempStateUpRight as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(behaviourUpRight);

                // INITIALISE tempStateUpRight with reference to animationRight:
                (tempStateUpRight as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationRight as IEventListener<UpdateEventArgs>);

                // INITIALISE tempStateUpRight with references to healthBehaviour Events for Update and Collision:
                (tempStateUpRight as IInitialiseParam<EventHandler<UpdateEventArgs>, EventHandler<CollisionEventArgs>>).Initialise(healthBehaviour.OnEvent, (healthBehaviour as IEventListener<CollisionEventArgs>).OnEvent);

                /// DOWN-LEFT

                // INITIALISE tempStateDownLeft with reference to behaviourDownLeft:
                (tempStateDownLeft as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(behaviourDownLeft);

                // INITIALISE tempStateDownLeft with reference to animationLeft:
                (tempStateDownLeft as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationLeft as IEventListener<UpdateEventArgs>);

                // INITIALISE tempStateDownLeft with references to healthBehaviour Events for Update and Collision:
                (tempStateDownLeft as IInitialiseParam<EventHandler<UpdateEventArgs>, EventHandler<CollisionEventArgs>>).Initialise(healthBehaviour.OnEvent, (healthBehaviour as IEventListener<CollisionEventArgs>).OnEvent);

                /// DOWN-RIGHT

                // INITIALISE tempStateDownRight with reference to behaviourDownRight:
                (tempStateDownRight as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(behaviourDownRight);

                // INITIALISE tempStateDownRight with reference to animationRight:
                (tempStateDownRight as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationRight as IEventListener<UpdateEventArgs>);

                // INITIALISE tempStateDownRight with references to healthBehaviour Events for Update and Collision:
                (tempStateDownRight as IInitialiseParam<EventHandler<UpdateEventArgs>, EventHandler<CollisionEventArgs>>).Initialise(healthBehaviour.OnEvent, (healthBehaviour as IEventListener<CollisionEventArgs>).OnEvent);

                #endregion


                #region ENTITY

                #region INSTANTIATION

                // SUBSCRIBE "Player1" to returned KeyboardManager from _engineManager:
                (_engineManager.GetService<KeyboardManager>() as IKeyboardPublisher).Subscribe(entityManager.GetDictionary()["Player1"] as IKeyboardListener);

                // SET PlayerIndex of "Player1" to PlayerIndex.One:
                (entityManager.GetDictionary()["Player1"] as IPlayer).PlayerNum = PlayerIndex.One;

                #endregion


                #region INITIALISATION

                /// STATIONARY

                // INITIALISE "Player1" with tempStateStationary:
                (entityManager.GetDictionary()["Player1"] as IInitialiseParam<IState>).Initialise(tempStateStationary);

                // INITIALISE "Player1" with reference to behaviourStationary:
                (entityManager.GetDictionary()["Player1"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(behaviourStationary);

                // INITIALISE "Player1" with reference to animationStationary:
                (entityManager.GetDictionary()["Player1"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationStationary as IEventListener<UpdateEventArgs>);

                /// UP

                // INITIALISE "Player1" with reference to behaviourUp:
                (entityManager.GetDictionary()["Player1"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(behaviourUp);

                // INITIALISE "Player1" with reference to animationUp:
                (entityManager.GetDictionary()["Player1"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationUp as IEventListener<UpdateEventArgs>);

                /// DOWN

                // INITIALISE "Player1" with reference to behaviourDown:
                (entityManager.GetDictionary()["Player1"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(behaviourDown);

                // INITIALISE "Player1" with reference to animationDown:
                (entityManager.GetDictionary()["Player1"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationDown as IEventListener<UpdateEventArgs>);

                /// LEFT

                // INITIALISE "Player1" with reference to behaviourLeft:
                (entityManager.GetDictionary()["Player1"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(behaviourLeft);

                // INITIALISE "Player1" with reference to animationLeft:
                (entityManager.GetDictionary()["Player1"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationLeft as IEventListener<UpdateEventArgs>);

                /// RIGHT

                // INITIALISE "Player1" with reference to behaviourRight:
                (entityManager.GetDictionary()["Player1"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(behaviourRight);

                // INITIALISE "Player1" with reference to animationRight:
                (entityManager.GetDictionary()["Player1"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationRight as IEventListener<UpdateEventArgs>);

                /// UP-LEFT

                // INITIALISE "Player1" with reference to behaviourUpLeft:
                (entityManager.GetDictionary()["Player1"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(behaviourUpLeft);

                /// UP-RIGHT

                // INITIALISE "Player1" with reference to behaviourUpRight:
                (entityManager.GetDictionary()["Player1"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(behaviourUpRight);

                /// DOWN-LEFT

                // INITIALISE "Player1" with reference to behaviourDownLeft:
                (entityManager.GetDictionary()["Player1"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(behaviourDownLeft);

                /// DOWN-RIGHT

                // INITIALISE "Player1" with reference to behaviourDownRight:
                (entityManager.GetDictionary()["Player1"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(behaviourDownRight);

                /// HEALTH

                // INITIALISE "Player1" with reference to healthBehaviour:
                (entityManager.GetDictionary()["Player1"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(healthBehaviour);

                /// OTHER VALUES

                // SET TextureSize Property value of "Player1" to a new Point() passing animationStationary.SpriteSize as a parameter:
                (entityManager.GetDictionary()["Player1"] as ITexture).TextureSize = new Point(animationStationary.SpriteSize.X, animationStationary.SpriteSize.Y);

                // SET DrawOrigin of "Player1" to value of centre of animation.SpriteSize.X / 2:
                (entityManager.GetDictionary()["Player1"] as IRotation).DrawOrigin = new Vector2(animationStationary.SpriteSize.X / 2, animationStationary.SpriteSize.Y / 2);

                // SET WindowBorder of "Player1" to value of _screenSize:
                (entityManager.GetDictionary()["Player1"] as IContainBoundary).WindowBorder = _screenSize;

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

                // SET MethodRef Property value of stateStationaryChange to reference of "Player1"'s SetState() method:
                (stateStationaryChange as ICommandOneParam<IState>).MethodRef = (entityManager.GetDictionary()["Player1"] as IEntityInternal).SetState;

                // SET FirstParam Property value of stateStationaryChange to reference of tempStateStationary:
                (stateStationaryChange as ICommandOneParam<IState>).FirstParam = tempStateStationary;

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

                // SET MethodRef Property value of stateUpChange to reference of "Player1"'s SetState() method:
                (stateUpChange as ICommandOneParam<IState>).MethodRef = (entityManager.GetDictionary()["Player1"] as IEntityInternal).SetState;

                // SET FirstParam Property value of stateUpChange to reference of tempStateUp:
                (stateUpChange as ICommandOneParam<IState>).FirstParam = tempStateUp;

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

                // SET MethodRef Property value of stateDownChange to reference of "Player1"'s SetState() method:
                (stateDownChange as ICommandOneParam<IState>).MethodRef = (entityManager.GetDictionary()["Player1"] as IEntityInternal).SetState;

                // SET FirstParam Property value of stateDownChange to reference of tempStateDown:
                (stateDownChange as ICommandOneParam<IState>).FirstParam = tempStateDown;

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

                // SET MethodRef Property value of stateLeftChange to reference of "Player1"'s SetState() method:
                (stateLeftChange as ICommandOneParam<IState>).MethodRef = (entityManager.GetDictionary()["Player1"] as IEntityInternal).SetState;

                // SET FirstParam Property value of stateLeftChange to reference of tempStateLeft:
                (stateLeftChange as ICommandOneParam<IState>).FirstParam = tempStateLeft;

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

                // SET MethodRef Property value of stateRightChange to reference of "Player1"'s SetState() method:
                (stateRightChange as ICommandOneParam<IState>).MethodRef = (entityManager.GetDictionary()["Player1"] as IEntityInternal).SetState;

                // SET FirstParam Property value of stateRightChange to reference of tempStateRight:
                (stateRightChange as ICommandOneParam<IState>).FirstParam = tempStateRight;

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

                // SET MethodRef Property value of stateUpLeftChange to reference of "Player1"'s SetState() method:
                (stateUpLeftChange as ICommandOneParam<IState>).MethodRef = (entityManager.GetDictionary()["Player1"] as IEntityInternal).SetState;

                // SET FirstParam Property value of stateUpLeftChange to reference of tempStateUpLeft:
                (stateUpLeftChange as ICommandOneParam<IState>).FirstParam = tempStateUpLeft;

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

                // SET MethodRef Property value of stateUpRightChange to reference of "Player1"'s SetState() method:
                (stateUpRightChange as ICommandOneParam<IState>).MethodRef = (entityManager.GetDictionary()["Player1"] as IEntityInternal).SetState;

                // SET FirstParam Property value of stateUpRightChange to reference of tempStateUpRight:
                (stateUpRightChange as ICommandOneParam<IState>).FirstParam = tempStateUpRight;

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

                // SET MethodRef Property value of stateDownLeftChange to reference of "Player1"'s SetState() method:
                (stateDownLeftChange as ICommandOneParam<IState>).MethodRef = (entityManager.GetDictionary()["Player1"] as IEntityInternal).SetState;

                // SET FirstParam Property value of stateDownLeftChange to reference of tempStateDownLeft:
                (stateDownLeftChange as ICommandOneParam<IState>).FirstParam = tempStateDownLeft;

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

                // SET MethodRef Property value of stateDownRightChange to reference of "Player1"'s SetState() method:
                (stateDownRightChange as ICommandOneParam<IState>).MethodRef = (entityManager.GetDictionary()["Player1"] as IEntityInternal).SetState;

                // SET FirstParam Property value of stateDownRightChange to reference of tempStateDownRight:
                (stateDownRightChange as ICommandOneParam<IState>).FirstParam = tempStateDownRight;

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


                #region NPCS

                #region NPC 1

                #region STATES

                // DECLARE & INSTANTIATE an IState as a new UpdatableCollidableState(), name it 'npcState':
                IState npcState = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<UpdatableCollidableState>();

                // INITIALISE npcState with a new Dictionary<string, EventArgs>():
                (npcState as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

                // INITIALISE npcState with a new UpdateEventArgs():
                (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

                // INITIALISE npcState with a new CollisionEventArgs():
                (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

                #endregion


                #region BEHAVIOUR

                // DECLARE & INSTANTIATE an IEventListener<UpdateEventArgs> as a new NPCBehaviour(), name it 'npcBehaviour':
                IEventListener<UpdateEventArgs> npcBehaviour = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<NPCBehaviour>();

                // INITIALISE npcBehaviour with a new Dictionary<string, EventHandler<UpdateEventArgs>>():
                (npcBehaviour as IInitialiseParam<IDictionary<string, EventHandler<UpdateEventArgs>>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>)
                    .Create<Dictionary<string, EventHandler<UpdateEventArgs>>>() as IDictionary<string, EventHandler<UpdateEventArgs>>);

                // INITIALISE npcBehaviour Direction Property with a new Vector2(0, -0.1f):
                (npcBehaviour as IDirection).Direction = new Vector2(-1, 0);

                // INITIALISE npcState with a reference to npcBehaviour:
                (npcState as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

                #region ANIMATIONS

                /// UP

                // INSTANTIATE animationUp as a new Animation():
                animationUp = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

                // SET Texture Property value of animationUp to "Gerald":
                (animationUp as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03");

                // SET Row Property value of animationUp to '14, 18':
                animationUp.SpriteSize = new Point(14, 18);

                // SET Row Property value of animationUp to '2':
                animationUp.Row = 2;

                // SET MsPerFrame Property value of animationUp to '175':
                animationUp.MsPerFrame = 175;

                /// DOWN

                // INSTANTIATE animationDown as a new Animation():
                animationDown = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

                // SET Texture Property value of animationDown to "Gerald":
                (animationDown as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03");

                // SET Row Property value of animationDown to '14, 18':
                animationDown.SpriteSize = new Point(14, 18);

                // SET Row Property value of animationDown to '1':
                animationDown.Row = 1;

                // SET MsPerFrame Property value of animationDown to '175':
                animationDown.MsPerFrame = 175;

                #endregion

                #endregion


                #region ENTITY

                // INITIALISE "NPC1" with a reference to npcState:
                (entityManager.GetDictionary()["NPC1"] as IInitialiseParam<IState>).Initialise(npcState);

                // INITIALISE "NPC1" with a reference to npcBehaviour:
                (entityManager.GetDictionary()["NPC1"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

                // SET TextureSize Property value of "NPC1" to a new Point() passing animationStationary.SpriteSize as a parameter:
                (entityManager.GetDictionary()["NPC1"] as ITexture).TextureSize = new Point(animationStationary.SpriteSize.X, animationStationary.SpriteSize.Y);

                // SET DrawOrigin of "NPC1" to value of centre of animation.SpriteSize.X / 2:
                (entityManager.GetDictionary()["NPC1"] as IRotation).DrawOrigin = new Vector2(animationStationary.SpriteSize.X / 2, animationStationary.SpriteSize.Y / 2);

                // SET WindowBorder of "NPC1" to value of _screenSize:
                (entityManager.GetDictionary()["NPC1"] as IContainBoundary).WindowBorder = _screenSize;

                #endregion


                #endregion


                #endregion

                #endregion


                #region LAYER 7 - GUI

                #region HP BAR

                // DECLARE & INSTANTIATE an IEntity as a new HPBar(), name it 'hPBar':
                IEntity hPBar = entityManager.Create<HPBar>("HPBar");

                // INITIALISE hPBar's MaxHealthPoints Property with the value of "Player1"'s MaxHealthProperty:
                (hPBar as IHaveHealth).MaxHealthPoints = (entityManager.GetDictionary()["Player1"] as IHaveHealth).MaxHealthPoints;

                // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam<Vector2>(), name it 'hPBarPosChangeCommand':
                ICommand hPBarPosChangeCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<Vector2>>();

                // INITIALISE hPBarPosChangeCommand's MethodRef Property with reference to hpBar.ChangeCamPos():
                (hPBarPosChangeCommand as ICommandOneParam<Vector2>).MethodRef = (hPBar as IChangePosition).ChangePosition;

                // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam<int>, name it 'hPBarHealthChangeCommand':
                ICommand hPBarHealthChangeCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<int>>();

                // INITIALISE hPBarHealthChangeCommand's MethodRef Property with reference to hPBar.ChangeHealth():
                (hPBarHealthChangeCommand as ICommandOneParam<int>).MethodRef = (hPBar as IHPBar).ChangeHealth;

                // INITIALISE healthBehaviour with a reference to hPBarHealthChangeCommand:
                (healthBehaviour as IInitialiseParam<ICommand>).Initialise(hPBarHealthChangeCommand);

                #endregion


                #region HP BAR SHROUD

                #region STATE

                // DECLARE & INSTANTIATE an IState as a new UpdatableState(), name it 'hPBarShroudState':
                IState hPBarShroudState = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<UpdatableState>();

                // INITIALISE hPBarShroudState with a new Dictionary<string, ICommand>():
                (hPBarShroudState as IInitialiseParam<IDictionary<string, ICommand>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ICommand>>() as IDictionary<string, ICommand>);

                // INITIALISE hPBarShroudState with a new Dictionary<string, EventArgs>():
                (hPBarShroudState as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

                // INITIALISE hPBarShroudState with a new UpdateEventArgs():
                (hPBarShroudState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

                #endregion


                #region BEHAVIOUR

                // DECLARE & INSTANTIATE an IEventListener<UpdateEventArgs> as a new CameraBehaviour(), name it 'hPBarShroudBehaviour':
                IEventListener<UpdateEventArgs> hPBarShroudBehaviour = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<HPBarShroudBehaviour>();

                // INITIALISE hPBarShroudBehaviour with a reference to hPBarPosChangeCommand:
                (hPBarShroudBehaviour as IInitialiseParam<ICommand>).Initialise(hPBarPosChangeCommand);

                // INITIALISE hPBarShroudState with a reference to hPBarShroudBehaviour:
                (hPBarShroudState as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(hPBarShroudBehaviour);

                #endregion


                #region ENTITY

                // DECLARE & INSTANTIATE an IEntity as a new HPBarShroud(), name it 'hPBarShroud':
                IEntity hPBarShroud = entityManager.Create<HPBarShroud>("HPBarShroud");

                // INITIALISE hpBarShroud with reference to hPBarShroudState:
                (hPBarShroud as IInitialiseParam<IState>).Initialise(hPBarShroudState);

                // INITIALISE hpBarShroud with reference to hPBarShroudBehaviour:
                (hPBarShroud as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(hPBarShroudBehaviour);

                // INITIALISE hpBarShroud's Zoom Property with value of _screenSize:
                (hPBarShroud as IContainBoundary).WindowBorder = _screenSize;

                // INITIALISE hpBarShroud's Zoom Property with value of _viewZoom:
                (hPBarShroud as IZoom).Zoom = _viewZoom;

                // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam<Vector2>(), name it 'hPBarPosShroudChangeCommand':
                ICommand hPBarShroudPosChangeCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<Vector2>>();

                // INITIALISE hPBarShroudPosChangeCommand's MethodRef Property with reference to hPBarShroud.ChangeCamPos():
                (hPBarShroudPosChangeCommand as ICommandOneParam<Vector2>).MethodRef = (hPBarShroud as IChangePosition).ChangePosition;

                // INITIALISE camBehaviour with a reference to hPBarShroudPosChangeCommand:
                (camBehaviour as IInitialiseParam<ICommand>).Initialise(hPBarShroudPosChangeCommand);

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
            //songMgr.PlayAudio("MainTrack");

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

            #region LAYERS 1, 2, 3, 4 & 5

            // DECLARE & INITIALISE an IDictionary<string, IEntity>, name it 'tempEntityDict', give return value of EntityManager.GetDictionary():
            IDictionary<string, IEntity> tempEntityDict = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary();

            // FOREACH IEntity in tempEntityDict.Values:
            foreach (IEntity pEntity in tempEntityDict.Values)
            {
                // IF pEntity is on Layer 1, Layer 2, Layer 3, Layer 4 or Layer 5:
                if ((pEntity as ILayer).Layer == 1 || (pEntity as ILayer).Layer == 2 || (pEntity as ILayer).Layer == 3 || (pEntity as ILayer).Layer == 4 || (pEntity as ILayer).Layer == 5)
                {
                    // SPAWN pEntity in "Level1" at their specified DestinationRectangle:
                    (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level1", pEntity, new Vector2((pEntity as IDrawDestinationRectangle).DestinationRectangle.X, (pEntity as IDrawDestinationRectangle).DestinationRectangle.Y));
                }
            }

            #endregion

            #endregion


            #region LAYER 6 - PLAYER / NPC




            #region NPCS

            #region NPC 1

            // DECLARE & INITIALISE and IEntity with reference to "NPC1", name it 'tempEntity':
            IEntity tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["NPC1"];

            // LOAD "Thug_03" texture to "Player1":
            (tempEntity as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03");

            // SPAWN "NPC1" in "Level1" at position created from Tiled:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level1", tempEntity, tempEntity.Position);


            #endregion

            #endregion


            #region PLAYER

            // INITIALISE tempEntity with reference to "Player1":
            tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["Player1"];

            // LOAD "Gerald" texture to "Player1":
            (tempEntity as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Gerald");

            // SPAWN "Player1" in "Level1" at position created from Tiled:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level1", tempEntity, tempEntity.Position);

            #endregion


            #region CAMERA

            // LOAD "ViewRange" texture to "Camera":
            ((_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["Camera"] as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/ViewRange");

            // SPAWN "Camera" in "Level1" at position of tempEntity:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level1", (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["Camera"], (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["Player1"].Position);

            #endregion

            #endregion


            #region LAYER 7 - GUI

            // LOAD "HPBarShroud" texture to "HPBarShroud":
            ((_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["HPBar"] as ITexture).Texture = Content.Load<Texture2D>("RIRR/GUI/HPBar");

            // SPAWN "Camera" in "Level1" at position of tempEntity:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level1", (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["HPBar"], (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["Camera"].Position);

            // LOAD "HPBarShroud" texture to "HPBarShroud":
            ((_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["HPBarShroud"] as ITexture).Texture = Content.Load<Texture2D>("RIRR/GUI/HPBarShroud");

            // SPAWN "Camera" in "Level1" at position of tempEntity:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level1", (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["HPBarShroud"], (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["Camera"].Position);

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



        #endregion
    }
}