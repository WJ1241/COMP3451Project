﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using COMP3451Project.EnginePackage.Camera;
using COMP3451Project.EnginePackage.CollisionManagement;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.EntityManagement;
using COMP3451Project.EnginePackage.InputManagement;
using COMP3451Project.EnginePackage.SceneManagement;
using COMP3451Project.EnginePackage.Services;
using COMP3451Project.PongPackage.Entities;

namespace COMP3451Project
{
    /// <summary>
    /// This is the main type for your game.
    /// Author: William Smith & Declan Kerby-Collins
    /// Date: 07/02/22
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

        // DECLARE an IDictionary, name it '_entityDictionary':
        private IDictionary<string, IEntity> _entityDictionary;

        // DECLARE a Vector2, name it 'screenSize':
        private Vector2 _screenSize;

        #endregion


        #region CONSTRUCTOR

        public Kernel()
        {
            // INSTANTIATE _graphics as new GraphicsDeviceManager, passing Kernel as a parameter:
            _graphics = new GraphicsDeviceManager(this);

            // SET RootDirectory of Content as "Content":
            Content.RootDirectory = "Content";

            // SET IsMouseVisible to true:
            IsMouseVisible = true;

            // SET screen width to 1600:
            _graphics.PreferredBackBufferWidth = 1600;

            // SET screen height to 900:
            _graphics.PreferredBackBufferHeight = 900;

            // INITIALISE _screenSize with Viewport Width and Height:
            _screenSize = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
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

            // INSTANTIATE _rand as new Random():
            _rand = new Random();

            #region ENTITY MANAGER

            // DECLARE an IEntityManager, name it '_entityManager':
            IEntityManager _entityManager;

            // INSTANTIATE _engineManager as a new EntityManager():
            _entityManager = _engineManager.GetService<EntityManager>() as IEntityManager;


            //_entityManager.;

            #endregion


            #region SCENE MANAGER

            // DECLARE an ISceneManager, name it '_sceneManager':
            ISceneManager _sceneManager;

            #endregion



            // INSTANTIATE _entityManager as new EntityManager():
            _entityManager = new EntityManager();

            // INSTANTIATE _sceneManager as new SceneManager():
            _sceneManager = new SceneManager();

            // INSTANTIATE _sceneGraph as new SceneGraph():
            _sceneGraph = new SceneGraph();

            // INSTANTIATE _collisionManager as new SceneManager():
            _collisionManager = new CollisionManager();

            // INSTANTIATE _kBManager, call it '_kBManager':
            _kBManager = new KeyboardManager();

            // ASSIGNMENT, set value of '_entityDictionary' the same as _entityManager Dictionary:
            _entityDictionary = _entityManager.GetDictionary;

            // INITIALISE _entityManager, passing _sceneManager as a parameter:
            _entityManager.Initialise(_kBManager);

            // INITIALISE _sceneManager, passing _collisionManager as a parameter:
            _sceneManager.Initialise(_collisionManager);

            // INITIALISE _sceneManager, passing _sceneGraph as a parameter:
            _sceneManager.Initialise(_sceneGraph);

            #endregion


            #region DISPLAYABLE CREATION

            #region LAYER 5 - PADDLE / BALL

            #region PADDLES

            #region PADDLE 1

            // INSTANTIATE new Paddle():
            _entityManager.Create<Paddle>("paddle1");

            // SUBSCRIBE "paddle1" to Keyboard Manager:
            _kBManager.Subscribe(_entityDictionary["paddle1"] as IKeyboardListener);

            // INITIALISE "paddle1":
            _entityDictionary["paddle1"].Initialise();

            // SET PlayerIndex of "paddle1" to PlayerIndex.One:
            (_entityDictionary["paddle1"] as IPlayer).PlayerNum = PlayerIndex.One;

            // SET Layer of "paddle1" to 5:
            (_entityDictionary["paddle1"] as ILayer).Layer = 5;

            #endregion


            #region PADDLE 2

            // INSTANTIATE new Paddle():
            _entityManager.Create<Paddle>("paddle2");

            // SUBSCRIBE "paddle2" to Keyboard Manager:
            _kBManager.Subscribe(_entityDictionary["paddle1"] as IKeyboardListener);

            // INITIALISE "paddle2":
            _entityDictionary["paddle1"].Initialise();

            // SET PlayerIndex of "paddle2" to PlayerIndex.One:
            (_entityDictionary["paddle1"] as IPlayer).PlayerNum = PlayerIndex.One;

            // SET Layer of "paddle2" to 5:
            (_entityDictionary["paddle1"] as ILayer).Layer = 5;

            #endregion


            #region BALL

            // INSTANTIATE new Ball():
            _entityManager.Create<Ball>("ball");

            // INITIALISE "ball":
            _entityDictionary["ball"].Initialise();

            // SET Layer of "ball" to 5:
            (_entityDictionary["ball"] as ILayer).Layer = 5;

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
            //decs attempt at tile maps mrk1

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

            #region LAYER 1 - WALLS

            #region WALLS

            // CANNOT USE FOR LOOP, INDIVIDUAL TEXTURING & POSITIONING IS REQUIRED

            /// WALLS 1-10

            (_entityDictionary["wall1"] as ITexture).Texture = Content.Load<Texture2D>("Brick_11Y"); // LOAD "Brick_11Y" texture to "wall1"       
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall1"], new Vector2(0, 0)); // SPAWN "wall1" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall2"] as ITexture).Texture = Content.Load<Texture2D>("Brick_6X"); // LOAD "Brick_6X" texture to "wall2"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall2"], new Vector2(16, 0)); // SPAWN "wall2" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall3"] as ITexture).Texture = Content.Load<Texture2D>("Brick_6X"); // LOAD "Brick_6X" texture to "wall3"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall3"], new Vector2(16, 160)); // SPAWN "wall3" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall4"] as ITexture).Texture = Content.Load<Texture2D>("Brick_3Y"); // LOAD "Brick_3Y" texture to "wall4"       
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall4"], new Vector2(112, 0)); // SPAWN "wall4" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall5"] as ITexture).Texture = Content.Load<Texture2D>("Brick_6Y"); // LOAD "Brick_6Y" texture to "wall5"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall5"], new Vector2(112, 80)); // SPAWN "wall5" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall6"] as ITexture).Texture = Content.Load<Texture2D>("Brick_32X"); // LOAD "Brick_32X" texture to "wall6"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall6"], new Vector2(128, 32)); // SPAWN "wall6" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall7"] as ITexture).Texture = Content.Load<Texture2D>("Brick_10X"); // LOAD "Brick_10X" texture to "wall7"       
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall7"], new Vector2(128, 80)); // SPAWN "wall7" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall8"] as ITexture).Texture = Content.Load<Texture2D>("Brick_14Y"); // LOAD "Brick_14Y" texture to "wall8"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall8"], new Vector2(288, 80)); // SPAWN "wall8" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall9"] as ITexture).Texture = Content.Load<Texture2D>("Brick_4X"); // LOAD "Brick_4X" texture to "wall9"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall9"], new Vector2(304, 80)); // SPAWN "wall9" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall10"] as ITexture).Texture = Content.Load<Texture2D>("Brick_2Y"); // LOAD "Brick_2Y" texture to "wall10"       
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall10"], new Vector2(368, 80)); // SPAWN "wall10" on screen by adding to SceneManager Dictionary


            /// WALLS 11-20

            (_entityDictionary["wall11"] as ITexture).Texture = Content.Load<Texture2D>("Brick_4X"); // LOAD "Brick_4X" texture to "wall11"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall11"], new Vector2(304, 160)); // SPAWN "wall11" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall12"] as ITexture).Texture = Content.Load<Texture2D>("Brick_3Y"); // LOAD "Brick_3Y" texture to "wall12"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall12"], new Vector2(368, 144)); // SPAWN "wall12" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall13"] as ITexture).Texture = Content.Load<Texture2D>("Brick_4X"); // LOAD "Brick_4X" texture to "wall13"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall13"], new Vector2(304, 240)); // SPAWN "wall13" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall14"] as ITexture).Texture = Content.Load<Texture2D>("Brick_2Y"); // LOAD "Brick_2Y" texture to "wall14"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall14"], new Vector2(368, 224)); // SPAWN "wall14" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall15"] as ITexture).Texture = Content.Load<Texture2D>("Brick_10X"); // LOAD "Brick_10X" texture to "wall15"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall15"], new Vector2(304, 288)); // SPAWN "wall15" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall16"] as ITexture).Texture = Content.Load<Texture2D>("Brick_14Y"); // LOAD "Brick_14Y" texture to "wall16"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall16"], new Vector2(464, 80)); // SPAWN "wall16" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall17"] as ITexture).Texture = Content.Load<Texture2D>("Brick_4X"); // LOAD "Brick_4X" texture to "wall17"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall17"], new Vector2(480, 80)); // SPAWN "wall17" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall18"] as ITexture).Texture = Content.Load<Texture2D>("Brick_2Y"); // LOAD "Brick_2Y" texture to "wall18"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall18"], new Vector2(544, 80)); // SPAWN "wall18" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall19"] as ITexture).Texture = Content.Load<Texture2D>("Brick_4X"); // LOAD "Brick_4X" texture to "wall19"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall19"], new Vector2(480, 160)); // SPAWN "wall19" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall20"] as ITexture).Texture = Content.Load<Texture2D>("Brick_3Y"); // LOAD "Brick_3Y" texture to "wall20"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall20"], new Vector2(544, 144)); // SPAWN "wall20" on screen by adding to SceneManager Dictionary


            /// WALLS 21-30

            (_entityDictionary["wall21"] as ITexture).Texture = Content.Load<Texture2D>("Brick_4X"); // LOAD "Brick_4X" texture to "wall21"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall21"], new Vector2(480, 240)); // SPAWN "wall21" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall22"] as ITexture).Texture = Content.Load<Texture2D>("Brick_2Y"); // LOAD "Brick_2Y" texture to "wall22"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall22"], new Vector2(544, 224)); // SPAWN "wall22" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall23"] as ITexture).Texture = Content.Load<Texture2D>("Brick_7Y"); // LOAD "Brick_7Y" texture to "wall23"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall23"], new Vector2(640, 32)); // SPAWN "wall23" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall24"] as ITexture).Texture = Content.Load<Texture2D>("Brick_6X"); // LOAD "Brick_6X" texture to "wall24"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall24"], new Vector2(656, 32)); // SPAWN "wall24" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall25"] as ITexture).Texture = Content.Load<Texture2D>("Brick_17Y"); // LOAD "Brick_17Y" texture to "wall25"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall25"], new Vector2(752, 32)); // SPAWN "wall25" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall26"] as ITexture).Texture = Content.Load<Texture2D>("Brick_7Y"); // LOAD "Brick_7Y" texture to "wall26"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall26"], new Vector2(640, 192)); // SPAWN "wall26" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall27"] as ITexture).Texture = Content.Load<Texture2D>("Brick_2X"); // LOAD "Brick_2X" texture to "wall27"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall27"], new Vector2(656, 240)); // SPAWN "wall27" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall28"] as ITexture).Texture = Content.Load<Texture2D>("Brick_2X"); // LOAD "Brick_2X" texture to "wall28"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall28"], new Vector2(720, 240)); // SPAWN "wall28" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall29"] as ITexture).Texture = Content.Load<Texture2D>("Brick_6X"); // LOAD "Brick_6X" texture to "wall29"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall29"], new Vector2(656, 288)); // SPAWN "wall29" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall30"] as ITexture).Texture = Content.Load<Texture2D>("Brick_5X"); // LOAD "Brick_5X" texture to "wall30"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall30"], new Vector2(560, 288)); // SPAWN "wall30" on screen by adding to SceneManager Dictionary


            /// WALLS 30-38

            (_entityDictionary["wall31"] as ITexture).Texture = Content.Load<Texture2D>("Brick_4Y"); // LOAD "Brick_4Y" texture to "wall31"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall31"], new Vector2(544, 288)); // SPAWN "wall31" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall32"] as ITexture).Texture = Content.Load<Texture2D>("Brick_18X"); // LOAD "Brick_18X" texture to "wall32"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall32"], new Vector2(256, 336)); // SPAWN "wall32" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall33"] as ITexture).Texture = Content.Load<Texture2D>("Brick_2X"); // LOAD "Brick_2X" texture to "wall33"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall33"], new Vector2(256, 288)); // SPAWN "wall33" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall34"] as ITexture).Texture = Content.Load<Texture2D>("Brick_2Y"); // LOAD "Brick_2Y" texture to "wall34"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall34"], new Vector2(240, 272)); // SPAWN "wall34" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall35"] as ITexture).Texture = Content.Load<Texture2D>("Brick_6X"); // LOAD "Brick_6X" texture to "wall35"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall35"], new Vector2(144, 272)); // SPAWN "wall35" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall36"] as ITexture).Texture = Content.Load<Texture2D>("Brick_6Y"); // LOAD "Brick_6Y" texture to "wall36"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall36"], new Vector2(128, 272)); // SPAWN "wall36" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall37"] as ITexture).Texture = Content.Load<Texture2D>("Brick_6X"); // LOAD "Brick_6X" texture to "wall37"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall37"], new Vector2(144, 352)); // SPAWN "wall37" on screen by adding to SceneManager Dictionary

            (_entityDictionary["wall38"] as ITexture).Texture = Content.Load<Texture2D>("Brick_2Y"); // LOAD "Brick_2Y" texture to "wall38"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["wall38"], new Vector2(240, 336)); // SPAWN "wall38" on screen by adding to SceneManager Dictionary

            #endregion

            #endregion


            #region LAYER 2 - FLOORS

            #region FLOORS

            /// LEVEL FLOOR

            (_entityDictionary["floor"] as ITexture).Texture = Content.Load<Texture2D>("floor"); // LOAD "floor" texture to "player"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["floor"], new Vector2(16, 16)); // SPAWN "floor" on screen by adding to SceneManager Dictionary

            #endregion

            #endregion


            #region LAYER 3 - STATIC OBSTACLES

            #region CUPBOARDS

            /// CUPBOARD 1

            (_entityDictionary["cupboard1"] as ITexture).Texture = Content.Load<Texture2D>("cupboard"); // LOAD "cupboard" texture to "cupboard1"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["cupboard1"], new Vector2(412, 48)); // SPAWN "cupboard1" on screen by adding to SceneManager Dictionary


            /// CUPBOARD 2

            (_entityDictionary["cupboard2"] as ITexture).Texture = Content.Load<Texture2D>("cupboard"); // LOAD "cupboard" texture to "cupboard2"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["cupboard2"], new Vector2(692, 48)); // SPAWN "cupboard2" on screen by adding to SceneManager Dictionary


            /// CUPBOARD 3

            (_entityDictionary["cupboard3"] as ITexture).Texture = Content.Load<Texture2D>("cupboard"); // LOAD "cupboard" texture to "cupboard3"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["cupboard3"], new Vector2(492, 256)); // SPAWN "cupboard3" on screen by adding to SceneManager Dictionary

            #endregion


            #region BEDS

            /// PLAYER SPAWN BED

            (_entityDictionary["playerspawn"] as ITexture).Texture = Content.Load<Texture2D>("playerspawn"); // LOAD "playerspawn" texture to "playerspawn"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["playerspawn"], new Vector2(80, 144)); // SPAWN "playerspawn" on screen by adding to SceneManager Dictionary


            /// CELL BED 1

            (_entityDictionary["cellbed1"] as ITexture).Texture = Content.Load<Texture2D>("dfltbed"); // LOAD "dfltbed" texture to "cellbed1"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["cellbed1"], new Vector2(336, 144)); // SPAWN "cellbed1" on screen by adding to SceneManager Dictionary


            /// CELL BED 2

            (_entityDictionary["cellbed2"] as ITexture).Texture = Content.Load<Texture2D>("dfltbed"); // LOAD "dfltbed" texture to "cellbed2"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["cellbed2"], new Vector2(336, 224)); // SPAWN "cellbed2" on screen by adding to SceneManager Dictionary


            /// CELL BED 3

            (_entityDictionary["cellbed3"] as ITexture).Texture = Content.Load<Texture2D>("dfltbed"); // LOAD "dfltbed" texture to "cellbed3"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["cellbed3"], new Vector2(512, 144)); // SPAWN "cellbed3" on screen by adding to SceneManager Dictionary


            /// CELL BED 4

            (_entityDictionary["cellbed4"] as ITexture).Texture = Content.Load<Texture2D>("dfltbed"); // LOAD "dfltbed" texture to "cellbed4"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["cellbed4"], new Vector2(512, 224)); // SPAWN "cellbed4" on screen by adding to SceneManager Dictionary

            #endregion


            #region TABLES

            /// OFFICE TABLE

            (_entityDictionary["officetable"] as ITexture).Texture = Content.Load<Texture2D>("chairtable"); // LOAD "chairtable" texture to "officetable"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["officetable"], new Vector2(166, 310)); // SPAWN "officetable" on screen by adding to SceneManager Dictionary


            #endregion


            #region BOOKSHELVES

            /// CUPBOARD 1

            (_entityDictionary["bookshelf1"] as ITexture).Texture = Content.Load<Texture2D>("bookshelf"); // LOAD "bookshelf" texture to "bookshelf1"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["bookshelf1"], new Vector2(196, 288)); // SPAWN "bookshelf1" on screen by adding to SceneManager Dictionary


            /// CUPBOARD 2

            (_entityDictionary["bookshelf2"] as ITexture).Texture = Content.Load<Texture2D>("bookshelf"); // LOAD "bookshelf" texture to "bookshelf2"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["bookshelf2"], new Vector2(196, 344)); // SPAWN "bookshelf2" on screen by adding to SceneManager Dictionary


            /// CUPBOARD 3

            (_entityDictionary["bookshelf3"] as ITexture).Texture = Content.Load<Texture2D>("bookshelf"); // LOAD "bookshelf" texture to "bookshelf3"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["bookshelf3"], new Vector2(164, 344)); // SPAWN "bookshelf3" on screen by adding to SceneManager Dictionary

            #endregion

            #endregion


            #region LAYER 4 - ITEMS

            #region HEALTH PICKUPS

            /// HEALTH PICKUP 1

            (_entityDictionary["hppickup1"] as ITexture).Texture = Content.Load<Texture2D>("hp"); // LOAD "hp" texture to "hppickup1"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["hppickup1"], new Vector2(304, 96)); // SPAWN "hppickup1" on screen by adding to SceneManager Dictionary


            /// HEALTH PICKUP 2

            (_entityDictionary["hppickup2"] as ITexture).Texture = Content.Load<Texture2D>("hp"); // LOAD "hp" texture to "hppickup2"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["hppickup2"], new Vector2(628, 276)); // SPAWN "hppickup2" on screen by adding to SceneManager Dictionary

            #endregion

            #endregion


            #region LAYER 5 - PLAYER / ENEMY

            #region PLAYER

            /// PLAYER 1

            (_entityDictionary["player"] as ITexture).Texture = Content.Load<Texture2D>("player"); // LOAD "player" texture to "player"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["player"], new Vector2(64, 112)); // SPAWN "player" on screen by adding to SceneManager Dictionary

            #endregion


            #region ENEMIES

            /// ENEMY 1

            (_entityDictionary["enemy1"] as ITexture).Texture = Content.Load<Texture2D>("enemy1"); // LOAD "enemy1" texture to "enemy1"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["enemy1"], new Vector2(506, 60)); // SPAWN "enemy1" on screen by adding to SceneManager Dictionary


            /// ENEMY 2

            (_entityDictionary["enemy2"] as ITexture).Texture = Content.Load<Texture2D>("enemy1"); // LOAD "enemy1" texture to "enemy2"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["enemy2"], new Vector2(330, 270)); // SPAWN "enemy2" on screen by adding to SceneManager Dictionary


            /// ENEMY 3

            (_entityDictionary["enemy3"] as ITexture).Texture = Content.Load<Texture2D>("enemy1"); // LOAD "enemy1" texture to "enemy3"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["enemy3"], new Vector2(716, 96)); // SPAWN "enemy3" on screen by adding to SceneManager Dictionary

            #endregion

            #endregion


            #region LAYER 6 - GUI

            #region HEALTH BAR

            /// PLAYER HEALTH BAR SHROUD

            (_entityDictionary["hpbarshroud"] as ITexture).Texture = Content.Load<Texture2D>("healthbarshroud"); // LOAD "healthbarshroud" texture to "hpbarshroud"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["hpbarshroud"], new Vector2(0, 0)); // SPAWN "hpbarshroud" on screen by adding to SceneManager Dictionary, 0,0 is default position

            /// PLAYER HEALTH BAR

            (_entityDictionary["hpbar"] as ITexture).Texture = Content.Load<Texture2D>("healthbar"); // LOAD "healthbarshroud" texture to "hpbar"
            (_sceneManager as ISpawn).Spawn(_entityDictionary["hpbar"], new Vector2(0, 0)); // SPAWN "hpbar" on screen by adding to SceneManager Dictionary, 0,0 is default position

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
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
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

            // CALL Update() on EntityManager:
            (_entityManager as IUpdatable).Update(gameTime);

            // CALL Update() on SceneManager:
            (_sceneManager as IUpdatable).Update(gameTime);

            // CALL Update() on CollisionManager:
            (_collisionManager as IUpdatable).Update(gameTime);

            // CALL Update() on KeyboardManager:
            (_kBManager as IUpdatable).Update(gameTime);

            // CALL method from base Game class, uses gameTime as parameter:
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // SET colour of screen background as Black:
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // CALL Draw() method in _sceneManager,
            (_sceneManager as IDraw).Draw(_spriteBatch);

            // CALL Draw() method from base class:
            base.Draw(gameTime);
        }

        #endregion
    }
}