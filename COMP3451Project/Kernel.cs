using COMP3451Project.RIRRPackage.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using OrbitalEngine.Audio;
using OrbitalEngine.Audio.Interfaces;
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
using OrbitalEngine.Services.Factories;
using OrbitalEngine.Services.Factories.Interfaces;
using OrbitalEngine.Services.Interfaces;
using OrbitalEngine.Tiles;
using OrbitalEngine.Tiles.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

namespace COMP3451Project
{
    /// <summary>
    /// Kernel Class to hold all Engine Setup
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 14/04/22
    /// </summary>
    public partial class Kernel : Game, IInitialiseParam<IService>
    {
        #region FIELD VARIABLES

        // DECLARE an IRtnService, name it '_engineManager':
        private IRtnService _engineManager;

        // DECLARE a GraphicsDeviceManager, name it '_graphics':
        private GraphicsDeviceManager _graphics;

        // DECLARE a SpriteBatch, name it '_spriteBatch':
        private SpriteBatch _spriteBatch;

        // DECLARE a Random, name it '_rand':
        private Random _rand;

        // DECLARE a Color, name it '_bgColour':
        private Color _bgColour;

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

                // INITIALISE sceneManager with a reference to createCommand:
                (sceneManager as IInitialiseParam<IFuncCommand<ICommand>>).Initialise(createCommand);

                #endregion


                #region COLLISION MANAGER

                // INITIALISE returned CollisionManager from _engineManager.GetService() with a new List<ICollidable>():
                (_engineManager.GetService<CollisionManager>() as IInitialiseParam<IList<ICollidable>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<List<ICollidable>>() as IList<ICollidable>);

                #endregion


                #region KEYBOARD MANAGER

                // INITIALISE returned KeyboardManager from _engineManager.GetService() with a new Dictionary<string, IKeyboardListener>():
                (_engineManager.GetService<KeyboardManager>() as IInitialiseParam<IDictionary<string, IKeyboardListener>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, IKeyboardListener>>() as IDictionary<string, IKeyboardListener>);

                #endregion


                #region LEVEL LAYOUT MAKER

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

                #endregion


                #region SOUND MANAGERS

                /// SONG MANAGER

                // DECLARE & INSTANTIATE an IPlayAudio as a new SongManager(), name it 'songMgr':
                IPlayAudio songMgr = _engineManager.GetService<SongManager>() as IPlayAudio;

                // INITIALISE songMgr with a new Dictionary<string, Song>():
                (songMgr as IInitialiseParam<IDictionary<string, Song>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, Song>>() as IDictionary<string, Song>);

                // INITIALISE songMgr with "LevelTrack" and a Song named "LevelTrack":
                (songMgr as IInitialiseParam<string, Song>).Initialise("LevelTrack", Content.Load<Song>("RIRR/Songs/LevelTrack"));

                // INITIALISE songMgr with "VNTrack" and a Song named "VNTrack":
                (songMgr as IInitialiseParam<string, Song>).Initialise("VNTrack", Content.Load<Song>("RIRR/Songs/VNTrack"));

                /// SFX MANAGER

                // DECLARE & INSTANTIATE an IPlayAudio as a new SFXManager(), name it 'sfxMgr':
                IPlayAudio sfxMgr = _engineManager.GetService<SFXManager>() as IPlayAudio;

                // INITIALISE sfxMgr with a new Dictionary<string, SoundEffect>():
                (sfxMgr as IInitialiseParam<IDictionary<string, SoundEffect>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, SoundEffect>>() as IDictionary<string, SoundEffect>);

                // INITIALISE sfxMgr with "Attack" and a SoundEffect named "Attack":
                (sfxMgr as IInitialiseParam<string, SoundEffect>).Initialise("Attack", Content.Load<SoundEffect>("RIRR/SFX/Attack"));

                // INITIALISE sfxMgr with "Pickup" and a SoundEffect named "Pickup":
                (sfxMgr as IInitialiseParam<string, SoundEffect>).Initialise("Pickup", Content.Load<SoundEffect>("RIRR/SFX/Pickup"));

                #endregion


                // CALL CreateLevelOne():
                CreateVNOne();
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
            // SET colour of screen background to value of _bgColour:
            GraphicsDevice.Clear(_bgColour);

            // CALL Draw() method on returned SceneManager instance from _engineManager:
            (_engineManager.GetService<SceneManager>() as IDraw).Draw(_spriteBatch);

            // CALL Draw() method from base class:
            base.Draw(pGameTime);
        }

        #endregion
    }
}