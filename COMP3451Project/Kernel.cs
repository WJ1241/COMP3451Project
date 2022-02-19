﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TiledSharp;
using OrbitalEngine.Behaviours.Interfaces;
using OrbitalEngine.CollisionManagement;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.EntityManagement;
using OrbitalEngine.Exceptions;
using OrbitalEngine.InputManagement;
using OrbitalEngine.InputManagement.Interfaces;
using OrbitalEngine.SceneManagement;
using OrbitalEngine.Services.Interfaces;
using OrbitalEngine.Services.Factories;
using OrbitalEngine.Services.Commands;
using OrbitalEngine.States;
using OrbitalEngine.Tiles;
using OrbitalEngine.Services.Commands.Interfaces;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.Tiles.Interfaces;
using OrbitalEngine.EntityManagement.Interfaces;
using OrbitalEngine.SceneManagement.Interfaces;
using OrbitalEngine.Services.Factories.Interfaces;
using OrbitalEngine.States.Interfaces;
using OrbitalEngine.Animation.Interfaces;
using OrbitalEngine.Animation;
using COMP3451Project.PongPackage.Behaviours;
using COMP3451Project.PongPackage.Entities;

namespace COMP3451Project
{
    /// <summary>
    /// This is the main type for your game.
    /// Author: William Smith & Declan Kerby-Collins
    /// Date: 19/02/22
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

            // TRY checking if all engine creational classes as well as entity setup throw any exceptions:
            try
            {
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

                #region LAYER 2

                #region WALLS

                #region COMMANDS

                // DECLARE & INSTANTIATE an IFuncCommand<IEntity> as a new FuncCommandOneParam<string, IEntity>(), name it '_createWall':
                IFuncCommand<IEntity> _createWall = new FuncCommandOneParam<string, IEntity>();

                // INITIALISE _createFloor's MethodRef Property with EntityManager.Create<Wall>:
                (_createWall as IFuncCommandOneParam<string, IEntity>).MethodRef = (_engineManager.GetService<EntityManager>() as IEntityManager).Create<Wall>;

                // DECLARE & INSTANTIATE an IFuncCommand<IEntity> as a new FuncCommandOneParam<string, IEntity>(), name it '_createFloor':
                IFuncCommand<IEntity> _createFloor = new FuncCommandOneParam<string, IEntity>();

                // INITIALISE _createFloor's MethodRef Property with EntityManager.Create<DrawableRectangleEntity>:
                (_createFloor as IFuncCommandOneParam<string, IEntity>).MethodRef = (_engineManager.GetService<EntityManager>() as IEntityManager).Create<DrawableRectangleEntity>;

                #endregion

                // DECLARE & INSTANTIATE an ILevelLayoutMaker as a new LevelLayoutMaker(), name it '_levelLM':
                ILevelLayoutMaker _levelLM = _engineManager.GetService<LevelLayoutMaker>() as ILevelLayoutMaker;

                // INITIALISE _levelLM with "Wall" and _createWall as parameters:
                (_levelLM as IInitialiseParam<string, IFuncCommand<IEntity>>).Initialise("Wall", _createWall);

                // INITIALISE _levelLM with "Floor" and _createFloor as parameters:
                (_levelLM as IInitialiseParam<string, IFuncCommand<IEntity>>).Initialise("Floor", _createFloor);

                // DECLARE & INSTANTIATE a new TmxMap(), name it '_map', passing a .tmx file as a parameter:
                TmxMap _map = new TmxMap("..\\..\\..\\..\\Content\\ExampleLevel\\ExampleLevel.tmx");

                // DECLARE & INITIALISE a Texture2D, name it '_tilesetTex', give value of _map's TileSet on Layer 0's name:
                Texture2D _tilesetTex = Content.Load<Texture2D>("ExampleLevel\\" + _map.Tilesets[0].Name);

                // CALL CreateLevelLayout() on _levelLM, passing "ExampleLevel", _map and _tilesetTex as parameters:
                _levelLM.CreateLevelLayout("ExampleLevel", _map, _tilesetTex);

                #endregion

                #endregion


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

                #region INSTANTIATIONS

                // DECLARE & INSTANTIATE an IUpdateEventListener as a new PaddleBehaviour(), name it '_behaviourStationary':
                IUpdateEventListener _behaviourStationary = (_engineManager.GetService<Factory<IUpdateEventListener>>() as IFactory<IUpdateEventListener>).Create<PaddleBehaviour>();

                // DECLARE & INSTANTIATE an IUpdateEventListener as a new PaddleBehaviour(), name it '_behaviourUp':
                IUpdateEventListener _behaviourUp = (_engineManager.GetService<Factory<IUpdateEventListener>>() as IFactory<IUpdateEventListener>).Create<PaddleBehaviour>();

                // DECLARE & INSTANTIATE an IUpdateEventListener as a new PaddleBehaviour(), name it '_behaviourDown':
                IUpdateEventListener _behaviourDown = (_engineManager.GetService<Factory<IUpdateEventListener>>() as IFactory<IUpdateEventListener>).Create<PaddleBehaviour>();

                // DECLARE & INSTANTIATE an IUpdateEventListener as a new Animation(), name it '_animationStationary':
                IAnimation _animationStationary = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

                // DECLARE & INSTANTIATE an IUpdateEventListener as a new Animation(), name it '_animationUp':
                IAnimation _animationUp = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

                // DECLARE & INSTANTIATE an IUpdateEventListener as a new Animation(), name it '_animationDown':
                IAnimation _animationDown = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

                #endregion


                #region INITIALISATION

                #region BEHAVIOURS

                // SET Direction Property value of _behaviourStationary to '0':
                (_behaviourStationary as IDirection).Direction = new Vector2(0);

                // SET Direction Property value of _behaviourUp to '0, -1':
                (_behaviourUp as IDirection).Direction = new Vector2(0, -1);

                // SET Direction Property value of _behaviourDown to '0, 1':
                (_behaviourDown as IDirection).Direction = new Vector2(0, 1);

                #endregion


                #region ANIMATIONS

                /// STATIONARY

                // SET Texture Property value of _animationStationary to "paddleSpriteSheet":
                (_animationStationary as ITexture).Texture = Content.Load<Texture2D>("ExampleLevel/paddleSpriteSheet");

                // SET Row Property value of _animationStationary to '50, 150':
                _animationStationary.SpriteSize = new Point(50, 150);

                // SET Row Property value of _animationStationary to '0':
                _animationStationary.Row = 0;

                // SET MsPerFrame Property value of _animationStationary to '200':
                _animationStationary.MsPerFrame = 200;

                /// UP

                // SET Texture Property value of _animationUp to "paddleSpriteSheet":
                (_animationUp as ITexture).Texture = Content.Load<Texture2D>("ExampleLevel/paddleSpriteSheet");

                // SET Row Property value of _animationUp to '50, 150':
                _animationUp.SpriteSize = new Point(50, 150);

                // SET Row Property value of _animationUp to '1':
                _animationUp.Row = 1;

                // SET MsPerFrame Property value of _animationUp to '200':
                _animationUp.MsPerFrame = 200;

                /// DOWN

                // SET Texture Property value of _animationDown to "paddleSpriteSheet":
                (_animationDown as ITexture).Texture = Content.Load<Texture2D>("ExampleLevel/paddleSpriteSheet");

                // SET Row Property value of _animationDown to '50, 150':
                _animationDown.SpriteSize = new Point(50, 150);

                // SET Row Property value of _animationDown to '2':
                _animationDown.Row = 2;

                // SET MsPerFrame Property value of _animationDown to '200':
                _animationDown.MsPerFrame = 200;

                #endregion


                #region STATE

                /// STATIONARY

                // INITIALISE _tempStateStationary with reference to _behaviourStationary:
                (_tempStateStationary as IInitialiseParam<IUpdateEventListener>).Initialise(_behaviourStationary);

                // INITIALISE _tempStateStationary with reference to _animationStationary:
                (_tempStateStationary as IInitialiseParam<IUpdateEventListener>).Initialise(_animationStationary as IUpdateEventListener);

                /// UP

                // INITIALISE _tempStateUp with reference to _behaviourUp:
                (_tempStateUp as IInitialiseParam<IUpdateEventListener>).Initialise(_behaviourUp);

                // INITIALISE _tempStateUp with reference to _animationUp:
                (_tempStateUp as IInitialiseParam<IUpdateEventListener>).Initialise(_animationUp as IUpdateEventListener);

                /// DOWN

                // INITIALISE _tempStateDown with reference to _behaviourDown:
                (_tempStateDown as IInitialiseParam<IUpdateEventListener>).Initialise(_behaviourDown);

                // INITIALISE _tempStateDown with reference to _animationDown:
                (_tempStateDown as IInitialiseParam<IUpdateEventListener>).Initialise(_animationDown as IUpdateEventListener);

                #endregion

                #endregion

                #endregion


                #region ENTITY

                #region INSTANTIATION

                // INSTANTIATE new Paddle(), name it "Paddle1":
                _entityManager.Create<Paddle>("Paddle1");

                // SUBSCRIBE "Paddle1" to returned KeyboardManager from _engineManager:
                (_engineManager.GetService<KeyboardManager>() as IKeyboardPublisher).Subscribe(_entityManager.GetDictionary()["Paddle1"] as IKeyboardListener);

                #endregion


                #region INITIALISATION
                
                /// STATIONARY

                // INITIALISE "Paddle1" with _tempStateStationary:
                (_entityManager.GetDictionary()["Paddle1"] as IInitialiseParam<IState>).Initialise(_tempStateStationary);

                // INITIALISE "Paddle1" with reference to _behaviourStationary:
                (_entityManager.GetDictionary()["Paddle1"] as IInitialiseParam<IUpdateEventListener>).Initialise(_behaviourStationary);

                // INITIALISE "Paddle1" with reference to _animationStationary:
                (_entityManager.GetDictionary()["Paddle1"] as IInitialiseParam<IUpdateEventListener>).Initialise(_animationStationary as IUpdateEventListener);

                /// UP

                // INITIALISE "Paddle1" with reference to _behaviourUp:
                (_entityManager.GetDictionary()["Paddle1"] as IInitialiseParam<IUpdateEventListener>).Initialise(_behaviourUp);

                // INITIALISE "Paddle1" with reference to _animationUp:
                (_entityManager.GetDictionary()["Paddle1"] as IInitialiseParam<IUpdateEventListener>).Initialise(_animationUp as IUpdateEventListener);

                /// DOWN
                
                // INITIALISE "Paddle1" with reference to _behaviourDown:
                (_entityManager.GetDictionary()["Paddle1"] as IInitialiseParam<IUpdateEventListener>).Initialise(_behaviourDown);

                // INITIALISE "Paddle1" with reference to _animationDown:
                (_entityManager.GetDictionary()["Paddle1"] as IInitialiseParam<IUpdateEventListener>).Initialise(_animationDown as IUpdateEventListener);

                /// OTHER VALUES

                // SET DrawOrigin of "Paddle1" to value of centre of _animation.SpriteSize.X / 2:
                (_entityManager.GetDictionary()["Paddle1"] as IRotation).DrawOrigin = new Vector2(_animationStationary.SpriteSize.X / 2, _animationStationary.SpriteSize.Y / 2);

                // SET WindowBorder of "Paddle1" to value of _screenSize:
                (_entityManager.GetDictionary()["Paddle1"] as IContainBoundary).WindowBorder = _screenSize;

                // SET PlayerIndex of "Paddle1" to PlayerIndex.One:
                (_entityManager.GetDictionary()["Paddle1"] as IPlayer).PlayerNum = PlayerIndex.One;

                // SET Layer of "Paddle1" to 5:
                (_entityManager.GetDictionary()["Paddle1"] as ILayer).Layer = 5;

                #endregion

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


            #region TILES

            #region LAYER 1 & 2

            #region FLOORS & WALLS

            // DECLARE & INITIALISE an IDictionary<string, IEntity>, name it '_tempEntityDict', give return value of EntityManager.GetDictionary():
            IDictionary<string, IEntity> _tempEntityDict = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary();

            // FOREACH IEntity in _tempEntityDict.Values:
            foreach (IEntity pEntity in _tempEntityDict.Values)
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

            #region PADDLES

            /// PADDLE 1

            // DECLARE & INITIALISE an IEntity with reference to "Paddle1":
            IEntity _tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["Paddle1"];

            // LOAD "paddle" texture to "Paddle1":
            (_tempEntity as ITexture).Texture = Content.Load<Texture2D>("ExampleLevel/paddleSpriteSheet");

            // SPAWN "Paddle1" in "Level1" at the far left on the X axis with a gap, and middle on the Y axis:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level1", _tempEntity, new Vector2(0 + (_tempEntity as IRotation).DrawOrigin.X, _screenSize.Y / 2));

            /*
            /// PADDLE 2

            // INITIALISE _tempEntity with reference to "Paddle2":
            _tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["Paddle2"];

            // LOAD "paddle" texture to "Paddle2":
            (_tempEntity as ITexture).Texture = Content.Load<Texture2D>("paddle");

            // SPAWN "Paddle2" in "Level1" at the far left on the X axis with a gap, and middle on the Y axis:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level1", _tempEntity, new Vector2(_screenSize.X - ((_tempEntity as ITexture).Texture.Width * 2), (_screenSize.Y / 2) - (_tempEntity as ITexture).#ght / 2));
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
            #region CREATION & INITIALISATION

            // DECLARE & GET an instance of EntityManager as an IEntityManager, name it '_entityManager':
            IEntityManager _entityManager = _engineManager.GetService<EntityManager>() as IEntityManager;

            // INCREMENT _ballCount by '1':
            _ballCount++;

            // INSTANTIATE new Ball():
            _entityManager.Create<Ball>("Ball" + _ballCount);

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