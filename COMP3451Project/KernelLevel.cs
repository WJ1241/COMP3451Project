using COMP3451Project.RIRRPackage.Behaviours;
using COMP3451Project.RIRRPackage.Entities;
using COMP3451Project.RIRRPackage.Entities.Interfaces;
using COMP3451Project.RIRRPackage.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
using OrbitalEngine.InputManagement;
using OrbitalEngine.InputManagement.Interfaces;
using OrbitalEngine.SceneManagement;
using OrbitalEngine.SceneManagement.Interfaces;
using OrbitalEngine.Services.Commands;
using OrbitalEngine.Services.Commands.Interfaces;
using OrbitalEngine.Services.Factories;
using OrbitalEngine.Services.Factories.Interfaces;
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
    /// Kernel Class to hold all Level Creation Methods
    /// Authors: Declan Kerby-Collins & William Smith
    /// Date: 15/04/22
    /// </summary>
    public partial class Kernel
    {
        #region LEVEL 1

        /// <summary>
        /// Creates every dependency for the scene as well as all references for entities in Level 1
        /// </summary>
        private void CreateLevelOne()
        {
            #region MANAGER REFERENCES

            /// ENTITY MANAGER

            // DECLARE & GET an instance of EntityManager as an IEntityManager, name it 'entityManager':
            IEntityManager entityManager = _engineManager.GetService<EntityManager>() as IEntityManager;

            /// SCENE MANAGER

            // DECLARE & GET an instance of SceneManager as an ISceneManager, name it 'sceneManager':
            ISceneManager sceneManager = _engineManager.GetService<SceneManager>() as ISceneManager;

            /// CREATE ICOMMAND FUNCCOMMAND

            // DECLARE & INSTANTIATE an IFuncCommand<ICommandOneParam<string>> as a new FuncCommand<ICommandOneParam<string>>(), name it 'createCommand':
            IFuncCommand<ICommand> createCommand = (_engineManager.GetService<Factory<IFuncCommand<ICommand>>>() as IFactory<IFuncCommand<ICommand>>).Create<FuncCommandZeroParam<ICommand>>();

            // INITIALISE _createFloor's MethodRef Property with Factory<ICommand>.Create<CommandOneParam<string>>:
            (createCommand as IFuncCommandZeroParam<ICommand>).MethodRef = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<string>>;

            #endregion


            #region SFX COMMAND

            // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam<string>(), name it 'playSFXCommand':
            ICommand playSFXCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<string>>();

            // INITIALISE playSFXCommand's MethodRef Property with reference to SFXManager.PlayAudio:
            (playSFXCommand as ICommandOneParam<string>).MethodRef = (_engineManager.GetService<SFXManager>() as IPlayAudio).PlayAudio;

            #endregion


            #region LEVEL 1 CREATION

            /// SCENE

            // SET _bgColour to Black:
            _bgColour = Color.Black;

            // DECLARE & INSTANTIATE an ICommand as a new CommandZeroParam(), name it 'loadLevelOne':
            ICommand loadLevelOne = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandZeroParam>();

            // INITIALISE loadLevelOne with reference to this method:
            (loadLevelOne as ICommandZeroParam).MethodRef = CreateLevelOne;

            // DECLARE & INSTANTIATE an ICommand as a new CommandZeroParam(), name it 'loadVNTwo':
            ICommand loadVNTwo = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandZeroParam>();

            // INITIALISE loadVNTwo with reference to CreateVNTwo:
            (loadVNTwo as ICommandZeroParam).MethodRef = CreateVNTwo;

            // CALL CreateScene() on sceneManager, passing "Level1", a new Dictionary<string, ICommand>, and loadLevelOne as parameters:
            sceneManager.CreateScene("Level1", (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ICommand>>() as IDictionary<string, ICommand>, loadLevelOne);

            // CALL UploadNextScene() on sceneManager, passing "VN2", and loadVNTwo as parameters:
            sceneManager.UploadNextScene("VN2", loadVNTwo);

            // INITIALISE sceneManager with a CollisionManager instance from _engineManager, a new Dictionary<string, IEntity>() and a reference to createCommand for scene "Level1":
            sceneManager.Initialise("Level1", _engineManager.GetService<CollisionManager>() as ICollisionManager,
                (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, IEntity>>() as IDictionary<string, IEntity>, createCommand);

            /// SCENE GRAPH

            // DECLARE & INSTANTIATE an ICommand as a new CommandTwoParam<string, string>(), 'nextSceneCommand':
            ICommand nextSceneCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandTwoParam<string, string>>();

            // INITIALISE nextSceneCommand with a reference to SceneManager.LoadNextScene():
            (nextSceneCommand as ICommandTwoParam<string, string>).MethodRef = sceneManager.LoadNextScene;

            // DECLARE & INSTANTIATE an ICommand as a new CommandZeroParam(), name it 'stopAudioCommand':
            ICommand stopAudioCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandZeroParam>();

            // INITIALISE stopAudioCommand with a reference to SongManager.StopAudio():
            (stopAudioCommand as ICommandZeroParam).MethodRef = (_engineManager.GetService<SongManager>() as ISongManager).StopAudio;

            // INITIALISE the current scene with "NextScene" and nextSceneCommand as parameters:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string, ICommand>).Initialise("NextScene", nextSceneCommand);

            // INITIALISE the current scene with "StopAudio" and stopAudioCommand as parameters:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string, ICommand>).Initialise("StopAudio", stopAudioCommand);

            #region DISPLAYABLE CREATION

            #region LAYER 1-5 - TILES

            // DECLARE & INSTANTIATE a new TmxMap(), name it '_map', passing a .tmx file as a parameter:
            TmxMap map = new TmxMap("..\\..\\..\\..\\Content\\RIRR\\Levels\\Level1.tmx");

            // DECLARE & INITIALISE a Texture2D, name it '_tilesetTex', give value of _map's Tilesets[0]'s name:
            Texture2D tilesetTex = Content.Load<Texture2D>("RIRR\\Levels\\Tiles\\" + map.Tilesets[0].Name);

            // CALL CreateLevelLayout() on LevelLayoutMaker, passing "Level1", _map and _tilesetTex as parameters:
            (_engineManager.GetService<LevelLayoutMaker>() as ILevelLayoutMaker).CreateLevelLayout("Level1", map, tilesetTex);

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

            // INITIALISE artefactBehaviour with a reference to playSFXCommand:
            (artefactBehaviour as IInitialiseParam<ICommand>).Initialise(playSFXCommand);

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

            // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam<string>, name it 'nextLevelCommand':
            ICommand nextLevelCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<string>>();

            // INITIALISE nextLevelCommand's MethodRef Property with a reference to sceneManager.ReturnCurrentScene().GoToNextScene:
            (nextLevelCommand as ICommandOneParam<string>).MethodRef = sceneManager.ReturnCurrentScene().GoToNextScene;

            // INITIALISE levelChangeBehaviour with a reference to levelChangeCommand:
            (levelChangeBehaviour as IInitialiseParam<ICommand>).Initialise(nextLevelCommand);

            // INITIALISE levelChangeBehaviour with a value of "VN2":
            (levelChangeBehaviour as IInitialiseParam<string>).Initialise("VN2");

            // INITIALISE artefactState with a reference to levelChangeBehaviour:
            (levelChangeState as IInitialiseParam<IEventListener<CollisionEventArgs>>).Initialise(levelChangeBehaviour);

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
            ICommand camPosChangeCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandTwoParam<Vector2, Vector2>>();

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

            /// HEALTH BEHAVIOUR

            // INITIALISE healthBehaviour with a new Dictionary<string, ICommand>():
            (healthBehaviour as IInitialiseParam<IDictionary<string, ICommand>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ICommand>>() as IDictionary<string, ICommand>);

            // DECLARE & INSTANTIATE an ICommand as a new CommandZeroParam(), name it 'resetSceneCommand':
            ICommand resetSceneCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandZeroParam>();

            // INITIALISE resetSceneCommand's MethodRef Property to current scene's ResetScene() method:
            (resetSceneCommand as ICommandZeroParam).MethodRef = (sceneManager.ReturnCurrentScene() as IResetScene).ResetScene;

            // INITIALISE healthBehaviour with "ResetScene" and a reference to resetSceneCommand:
            (healthBehaviour as IInitialiseParam<string, ICommand>).Initialise("ResetScene", resetSceneCommand);


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

            // INITIALISE npcBehaviour with a reference to playSFXCommand:
            (npcBehaviour as IInitialiseParam<ICommand>).Initialise(playSFXCommand);

            // INITIALISE npcBehaviour Direction Property with a new Vector2(0, 0.8f):
            (npcBehaviour as IDirection).Direction = new Vector2(0, 0.8f);

            // INITIALISE npcState with a reference to npcBehaviour:
            (npcState as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            #region ANIMATIONS

            /// UP

            // INSTANTIATE animationUp as a new Animation():
            animationUp = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationUp to "Thug_04":
            (animationUp as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04");

            // SET Row Property value of animationUp to '14, 18':
            animationUp.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationUp to '2':
            animationUp.Row = 2;

            // SET MsPerFrame Property value of animationUp to '175':
            animationUp.MsPerFrame = 175;

            // INITIALISE npcBehaviour with a reference to animationUp.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Up", (animationUp as IEventListener<UpdateEventArgs>).OnEvent);

            /// DOWN

            // INSTANTIATE animationDown as a new Animation():
            animationDown = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationDown to "Thug_04":
            (animationDown as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04");

            // SET Row Property value of animationDown to '14, 18':
            animationDown.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationDown to '1':
            animationDown.Row = 1;

            // SET MsPerFrame Property value of animationDown to '175':
            animationDown.MsPerFrame = 175;

            // INITIALISE npcBehaviour with a reference to animationDown.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Down", (animationDown as IEventListener<UpdateEventArgs>).OnEvent);

            #endregion

            #endregion


            #region ENTITY

            // INITIALISE "NPC1" with a reference to npcState:
            (entityManager.GetDictionary()["NPC1"] as IInitialiseParam<IState>).Initialise(npcState);

            // INITIALISE "NPC1" with a reference to npcBehaviour:
            (entityManager.GetDictionary()["NPC1"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            // INITIALISE "NPC1" with a reference to animationUp:
            (entityManager.GetDictionary()["NPC1"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationUp as IEventListener<UpdateEventArgs>);

            // INITIALISE "NPC1" with a reference to animationDown:
            (entityManager.GetDictionary()["NPC1"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationDown as IEventListener<UpdateEventArgs>);

            // SET TextureSize Property value of "NPC1" to a new Point() passing animationUp.SpriteSize as a parameter:
            (entityManager.GetDictionary()["NPC1"] as ITexture).TextureSize = new Point(animationUp.SpriteSize.X, animationUp.SpriteSize.Y);

            // SET DrawOrigin of "NPC1" to value of centre of animationUp.SpriteSize.X / 2:
            (entityManager.GetDictionary()["NPC1"] as IRotation).DrawOrigin = new Vector2(animationUp.SpriteSize.X / 2, animationUp.SpriteSize.Y / 2);

            // SET WindowBorder of "NPC1" to value of _screenSize:
            (entityManager.GetDictionary()["NPC1"] as IContainBoundary).WindowBorder = _screenSize;

            #endregion


            #endregion


            #region NPC 2

            #region STATES

            // INSTANTIATE npcState as a new UpdatableCollidableState():
            npcState = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<UpdatableCollidableState>();

            // INITIALISE npcState with a new Dictionary<string, EventArgs>():
            (npcState as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

            // INITIALISE npcState with a new UpdateEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

            // INITIALISE npcState with a new CollisionEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

            #endregion


            #region BEHAVIOUR

            // INSTANTIATE npcBehaviour as a new NPCBehaviour():
            npcBehaviour = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<NPCBehaviour>();

            // INITIALISE npcBehaviour with a new Dictionary<string, EventHandler<UpdateEventArgs>>():
            (npcBehaviour as IInitialiseParam<IDictionary<string, EventHandler<UpdateEventArgs>>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>)
                .Create<Dictionary<string, EventHandler<UpdateEventArgs>>>() as IDictionary<string, EventHandler<UpdateEventArgs>>);

            // INITIALISE npcBehaviour with a reference to playSFXCommand:
            (npcBehaviour as IInitialiseParam<ICommand>).Initialise(playSFXCommand);

            // INITIALISE npcBehaviour Direction Property with a new Vector2(0.8, 0):
            (npcBehaviour as IDirection).Direction = new Vector2(0.8f, 0);

            // INITIALISE npcState with a reference to npcBehaviour:
            (npcState as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            #region ANIMATIONS

            /// LEFT

            // INSTANTIATE animationLeft as a new Animation():
            animationLeft = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationLeft to "Thug_03":
            (animationLeft as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03");

            // SET Row Property value of animationLeft to '14, 18':
            animationLeft.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationLeft to '4':
            animationLeft.Row = 4;

            // SET MsPerFrame Property value of animationLeft to '175':
            animationLeft.MsPerFrame = 175;

            // INITIALISE npcBehaviour with a reference to animationLeft.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Left", (animationLeft as IEventListener<UpdateEventArgs>).OnEvent);

            /// RIGHT

            // INSTANTIATE animationRight as a new Animation():
            animationRight = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationRight to "Thug_03":
            (animationRight as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03");

            // SET Row Property value of animationRight to '14, 18':
            animationRight.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationRight to '3':
            animationRight.Row = 3;

            // SET MsPerFrame Property value of animationRight to '175':
            animationRight.MsPerFrame = 175;

            // INITIALISE npcBehaviour with a reference to animationRight.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Right", (animationRight as IEventListener<UpdateEventArgs>).OnEvent);

            #endregion

            #endregion


            #region ENTITY

            // INITIALISE "NPC2" with a reference to npcState:
            (entityManager.GetDictionary()["NPC2"] as IInitialiseParam<IState>).Initialise(npcState);

            // INITIALISE "NPC2" with a reference to npcBehaviour:
            (entityManager.GetDictionary()["NPC2"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            // INITIALISE "NPC2" with a reference to animationLeft:
            (entityManager.GetDictionary()["NPC2"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationLeft as IEventListener<UpdateEventArgs>);

            // INITIALISE "NPC2" with a reference to animationRight:
            (entityManager.GetDictionary()["NPC2"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationRight as IEventListener<UpdateEventArgs>);

            // SET TextureSize Property value of "NPC2" to a new Point() passing animationLeft.SpriteSize as a parameter:
            (entityManager.GetDictionary()["NPC2"] as ITexture).TextureSize = new Point(animationLeft.SpriteSize.X, animationLeft.SpriteSize.Y);

            // SET DrawOrigin of "NPC2" to value of centre of animationLeft.SpriteSize.X / 2:
            (entityManager.GetDictionary()["NPC2"] as IRotation).DrawOrigin = new Vector2(animationLeft.SpriteSize.X / 2, animationLeft.SpriteSize.Y / 2);

            // SET WindowBorder of "NPC2" to value of _screenSize:
            (entityManager.GetDictionary()["NPC2"] as IContainBoundary).WindowBorder = _screenSize;

            #endregion

            #endregion


            #region NPC 3

            #region STATES

            // INSTANTIATE npcState as a new UpdatableCollidableState():
            npcState = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<UpdatableCollidableState>();

            // INITIALISE npcState with a new Dictionary<string, EventArgs>():
            (npcState as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

            // INITIALISE npcState with a new UpdateEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

            // INITIALISE npcState with a new CollisionEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

            #endregion


            #region BEHAVIOUR

            // INSTANTIATE npcBehaviour as a new NPCBehaviour():
            npcBehaviour = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<NPCBehaviour>();

            // INITIALISE npcBehaviour with a new Dictionary<string, EventHandler<UpdateEventArgs>>():
            (npcBehaviour as IInitialiseParam<IDictionary<string, EventHandler<UpdateEventArgs>>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>)
                .Create<Dictionary<string, EventHandler<UpdateEventArgs>>>() as IDictionary<string, EventHandler<UpdateEventArgs>>);

            // INITIALISE npcBehaviour with a reference to playSFXCommand:
            (npcBehaviour as IInitialiseParam<ICommand>).Initialise(playSFXCommand);

            // INITIALISE npcBehaviour Direction Property with a new Vector2(0, 0.8):
            (npcBehaviour as IDirection).Direction = new Vector2(0, 0.8f);

            // INITIALISE npcState with a reference to npcBehaviour:
            (npcState as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            #region ANIMATIONS

            /// UP

            // INSTANTIATE animationUp as a new Animation():
            animationUp = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationUp to "Thug_03":
            (animationUp as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03");

            // SET Row Property value of animationUp to '14, 18':
            animationUp.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationUp to '2':
            animationUp.Row = 2;

            // SET MsPerFrame Property value of animationUp to '175':
            animationUp.MsPerFrame = 175;

            // INITIALISE npcBehaviour with a reference to animationUp.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Up", (animationUp as IEventListener<UpdateEventArgs>).OnEvent);

            /// DOWN

            // INSTANTIATE animationDown as a new Animation():
            animationDown = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationDown to "Thug_03":
            (animationDown as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03");

            // SET Row Property value of animationDown to '14, 18':
            animationDown.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationDown to '1':
            animationDown.Row = 1;

            // SET MsPerFrame Property value of animationDown to '175':
            animationDown.MsPerFrame = 175;

            // INITIALISE npcBehaviour with a reference to animationDown.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Down", (animationDown as IEventListener<UpdateEventArgs>).OnEvent);

            #endregion

            #endregion


            #region ENTITY

            // INITIALISE "NPC3" with a reference to npcState:
            (entityManager.GetDictionary()["NPC3"] as IInitialiseParam<IState>).Initialise(npcState);

            // INITIALISE "NPC3" with a reference to npcBehaviour:
            (entityManager.GetDictionary()["NPC3"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            // INITIALISE "NPC3" with a reference to animationUp:
            (entityManager.GetDictionary()["NPC3"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationUp as IEventListener<UpdateEventArgs>);

            // INITIALISE "NPC3" with a reference to animationDown:
            (entityManager.GetDictionary()["NPC3"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationDown as IEventListener<UpdateEventArgs>);

            // SET TextureSize Property value of "NPC3" to a new Point() passing animationUp.SpriteSize as a parameter:
            (entityManager.GetDictionary()["NPC3"] as ITexture).TextureSize = new Point(animationUp.SpriteSize.X, animationUp.SpriteSize.Y);

            // SET DrawOrigin of "NPC3" to value of centre of animationUp.SpriteSize.X / 2:
            (entityManager.GetDictionary()["NPC3"] as IRotation).DrawOrigin = new Vector2(animationUp.SpriteSize.X / 2, animationUp.SpriteSize.Y / 2);

            // SET WindowBorder of "NPC3" to value of _screenSize:
            (entityManager.GetDictionary()["NPC3"] as IContainBoundary).WindowBorder = _screenSize;

            #endregion

            #endregion


            #region NPC 4

            #region STATES

            // INSTANTIATE npcState as a new UpdatableCollidableState():
            npcState = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<UpdatableCollidableState>();

            // INITIALISE npcState with a new Dictionary<string, EventArgs>():
            (npcState as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

            // INITIALISE npcState with a new UpdateEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

            // INITIALISE npcState with a new CollisionEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

            #endregion


            #region BEHAVIOUR

            // INSTANTIATE npcBehaviour as a new NPCBehaviour():
            npcBehaviour = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<NPCBehaviour>();

            // INITIALISE npcBehaviour with a new Dictionary<string, EventHandler<UpdateEventArgs>>():
            (npcBehaviour as IInitialiseParam<IDictionary<string, EventHandler<UpdateEventArgs>>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>)
                .Create<Dictionary<string, EventHandler<UpdateEventArgs>>>() as IDictionary<string, EventHandler<UpdateEventArgs>>);

            // INITIALISE npcBehaviour with a reference to playSFXCommand:
            (npcBehaviour as IInitialiseParam<ICommand>).Initialise(playSFXCommand);

            // INITIALISE npcBehaviour Direction Property with a new Vector2(0.8, 0):
            (npcBehaviour as IDirection).Direction = new Vector2(0.8f, 0);

            // INITIALISE npcState with a reference to npcBehaviour:
            (npcState as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            #region ANIMATIONS

            /// LEFT

            // INSTANTIATE animationLeft as a new Animation():
            animationLeft = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationLeft to "Thug_04":
            (animationLeft as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04");

            // SET Row Property value of animationLeft to '14, 18':
            animationLeft.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationLeft to '4':
            animationLeft.Row = 4;

            // SET MsPerFrame Property value of animationLeft to '175':
            animationLeft.MsPerFrame = 175;

            // INITIALISE npcBehaviour with a reference to animationUp.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Left", (animationLeft as IEventListener<UpdateEventArgs>).OnEvent);

            /// RIGHT

            // INSTANTIATE animationRight as a new Animation():
            animationRight = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationRight to "Thug_04":
            (animationRight as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04");

            // SET Row Property value of animationRight to '14, 18':
            animationRight.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationRight to '3':
            animationRight.Row = 3;

            // SET MsPerFrame Property value of animationRight to '175':
            animationRight.MsPerFrame = 175;

            // INITIALISE npcBehaviour with a reference to animationRight.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Right", (animationRight as IEventListener<UpdateEventArgs>).OnEvent);

            #endregion

            #endregion


            #region ENTITY

            // INITIALISE "NPC4" with a reference to npcState:
            (entityManager.GetDictionary()["NPC4"] as IInitialiseParam<IState>).Initialise(npcState);

            // INITIALISE "NPC4" with a reference to npcBehaviour:
            (entityManager.GetDictionary()["NPC4"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            // INITIALISE "NPC4" with a reference to animationLeft:
            (entityManager.GetDictionary()["NPC4"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationLeft as IEventListener<UpdateEventArgs>);

            // INITIALISE "NPC4" with a reference to animationRight:
            (entityManager.GetDictionary()["NPC4"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationRight as IEventListener<UpdateEventArgs>);

            // SET TextureSize Property value of "NPC4" to a new Point() passing animationUp.SpriteSize as a parameter:
            (entityManager.GetDictionary()["NPC4"] as ITexture).TextureSize = new Point(animationUp.SpriteSize.X, animationUp.SpriteSize.Y);

            // SET DrawOrigin of "NPC4" to value of centre of animationUp.SpriteSize.X / 2:
            (entityManager.GetDictionary()["NPC4"] as IRotation).DrawOrigin = new Vector2(animationUp.SpriteSize.X / 2, animationUp.SpriteSize.Y / 2);

            // SET WindowBorder of "NPC4" to value of _screenSize:
            (entityManager.GetDictionary()["NPC4"] as IContainBoundary).WindowBorder = _screenSize;

            #endregion

            #endregion


            #region NPC 5

            #region STATES

            // INSTANTIATE npcState as a new UpdatableCollidableState():
            npcState = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<UpdatableCollidableState>();

            // INITIALISE npcState with a new Dictionary<string, EventArgs>():
            (npcState as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

            // INITIALISE npcState with a new UpdateEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

            // INITIALISE npcState with a new CollisionEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

            #endregion


            #region BEHAVIOUR

            // INSTANTIATE npcBehaviour as a new NPCBehaviour():
            npcBehaviour = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<NPCBehaviour>();

            // INITIALISE npcBehaviour with a new Dictionary<string, EventHandler<UpdateEventArgs>>():
            (npcBehaviour as IInitialiseParam<IDictionary<string, EventHandler<UpdateEventArgs>>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>)
                .Create<Dictionary<string, EventHandler<UpdateEventArgs>>>() as IDictionary<string, EventHandler<UpdateEventArgs>>);

            // INITIALISE npcBehaviour with a reference to playSFXCommand:
            (npcBehaviour as IInitialiseParam<ICommand>).Initialise(playSFXCommand);

            // INITIALISE npcBehaviour Direction Property with a new Vector2(0.8, 0):
            (npcBehaviour as IDirection).Direction = new Vector2(0.8f, 0);

            // INITIALISE npcState with a reference to npcBehaviour:
            (npcState as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            #region ANIMATIONS

            /// LEFT

            // INSTANTIATE animationLeft as a new Animation():
            animationLeft = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationLeft to "Thug_03":
            (animationLeft as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03");

            // SET Row Property value of animationLeft to '14, 18':
            animationLeft.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationLeft to '4':
            animationLeft.Row = 4;

            // SET MsPerFrame Property value of animationLeft to '175':
            animationLeft.MsPerFrame = 175;

            // INITIALISE npcBehaviour with a reference to animationUp.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Left", (animationLeft as IEventListener<UpdateEventArgs>).OnEvent);

            /// RIGHT

            // INSTANTIATE animationRight as a new Animation():
            animationRight = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationRight to "Thug_03":
            (animationRight as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03");

            // SET Row Property value of animationRight to '14, 18':
            animationRight.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationRight to '3':
            animationRight.Row = 3;

            // SET MsPerFrame Property value of animationRight to '175':
            animationRight.MsPerFrame = 175;

            // INITIALISE npcBehaviour with a reference to animationRight.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Right", (animationRight as IEventListener<UpdateEventArgs>).OnEvent);

            #endregion

            #endregion


            #region ENTITY

            // INITIALISE "NPC5" with a reference to npcState:
            (entityManager.GetDictionary()["NPC5"] as IInitialiseParam<IState>).Initialise(npcState);

            // INITIALISE "NPC5" with a reference to npcBehaviour:
            (entityManager.GetDictionary()["NPC5"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            // INITIALISE "NPC5" with a reference to animationLeft:
            (entityManager.GetDictionary()["NPC5"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationLeft as IEventListener<UpdateEventArgs>);

            // INITIALISE "NPC5" with a reference to animationRight:
            (entityManager.GetDictionary()["NPC5"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationRight as IEventListener<UpdateEventArgs>);

            // SET TextureSize Property value of "NPC5" to a new Point() passing animationUp.SpriteSize as a parameter:
            (entityManager.GetDictionary()["NPC5"] as ITexture).TextureSize = new Point(animationUp.SpriteSize.X, animationUp.SpriteSize.Y);

            // SET DrawOrigin of "NPC5" to value of centre of animationUp.SpriteSize.X / 2:
            (entityManager.GetDictionary()["NPC5"] as IRotation).DrawOrigin = new Vector2(animationUp.SpriteSize.X / 2, animationUp.SpriteSize.Y / 2);

            // SET WindowBorder of "NPC5" to value of _screenSize:
            (entityManager.GetDictionary()["NPC5"] as IContainBoundary).WindowBorder = _screenSize;

            #endregion

            #endregion


            #region NPC 6

            #region STATES

            // INSTANTIATE npcState as a new UpdatableCollidableState():
            npcState = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<UpdatableCollidableState>();

            // INITIALISE npcState with a new Dictionary<string, EventArgs>():
            (npcState as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

            // INITIALISE npcState with a new UpdateEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

            // INITIALISE npcState with a new CollisionEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

            #endregion


            #region BEHAVIOUR

            // INSTANTIATE npcBehaviour as a new NPCBehaviour():
            npcBehaviour = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<NPCBehaviour>();

            // INITIALISE npcBehaviour with a new Dictionary<string, EventHandler<UpdateEventArgs>>():
            (npcBehaviour as IInitialiseParam<IDictionary<string, EventHandler<UpdateEventArgs>>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>)
                .Create<Dictionary<string, EventHandler<UpdateEventArgs>>>() as IDictionary<string, EventHandler<UpdateEventArgs>>);

            // INITIALISE npcBehaviour with a reference to playSFXCommand:
            (npcBehaviour as IInitialiseParam<ICommand>).Initialise(playSFXCommand);

            // INITIALISE npcBehaviour Direction Property with a new Vector2(0, 0.8):
            (npcBehaviour as IDirection).Direction = new Vector2(0, 0.8f);

            // INITIALISE npcState with a reference to npcBehaviour:
            (npcState as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            #region ANIMATIONS

            /// UP

            // INSTANTIATE animationUp as a new Animation():
            animationUp = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationUp to "Thug_04":
            (animationUp as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04");

            // SET Row Property value of animationUp to '14, 18':
            animationUp.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationUp to '2':
            animationUp.Row = 2;

            // SET MsPerFrame Property value of animationUp to '175':
            animationUp.MsPerFrame = 175;

            // INITIALISE npcBehaviour with a reference to animationUp.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Up", (animationUp as IEventListener<UpdateEventArgs>).OnEvent);

            /// DOWN

            // INSTANTIATE animationDown as a new Animation():
            animationDown = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationDown to "Thug_04":
            (animationDown as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04");

            // SET Row Property value of animationDown to '14, 18':
            animationDown.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationDown to '1':
            animationDown.Row = 1;

            // SET MsPerFrame Property value of animationDown to '175':
            animationDown.MsPerFrame = 175;

            // INITIALISE npcBehaviour with a reference to animationDown.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Down", (animationDown as IEventListener<UpdateEventArgs>).OnEvent);

            #endregion

            #endregion


            #region ENTITY

            // INITIALISE "NPC6" with a reference to npcState:
            (entityManager.GetDictionary()["NPC6"] as IInitialiseParam<IState>).Initialise(npcState);

            // INITIALISE "NPC6" with a reference to npcBehaviour:
            (entityManager.GetDictionary()["NPC6"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            // INITIALISE "NPC6" with a reference to animationUp:
            (entityManager.GetDictionary()["NPC6"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationUp as IEventListener<UpdateEventArgs>);

            // INITIALISE "NPC6" with a reference to animationDown:
            (entityManager.GetDictionary()["NPC6"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationDown as IEventListener<UpdateEventArgs>);

            // SET TextureSize Property value of "NPC6" to a new Point() passing animationUp.SpriteSize as a parameter:
            (entityManager.GetDictionary()["NPC6"] as ITexture).TextureSize = new Point(animationUp.SpriteSize.X, animationUp.SpriteSize.Y);

            // SET DrawOrigin of "NPC6" to value of centre of animationUp.SpriteSize.X / 2:
            (entityManager.GetDictionary()["NPC6"] as IRotation).DrawOrigin = new Vector2(animationUp.SpriteSize.X / 2, animationUp.SpriteSize.Y / 2);

            // SET WindowBorder of "NPC6" to value of _screenSize:
            (entityManager.GetDictionary()["NPC6"] as IContainBoundary).WindowBorder = _screenSize;

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

            // INITIALISE healthBehaviour with "UpdateHealthDisplay" and a reference to hPBarHealthChangeCommand:
            (healthBehaviour as IInitialiseParam<string, ICommand>).Initialise("UpdateHealthDisplay", hPBarHealthChangeCommand);

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


            #region SPAWNING

            #region TILES - LAYERS 1-5

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

            // FOREACH IEntity in tempEntityDict.Values:
            // NEEDS TO BE DONE TWICE DUE TO WALLS AND FLOORS OVERLAPPING OBSTACLES WHEN RESETTING
            foreach (IEntity pEntity in tempEntityDict.Values)
            {
                // IF pEntity is on Layer 3, Layer 4, or Layer 5:
                if ((pEntity as ILayer).Layer == 3 || (pEntity as ILayer).Layer == 4 || (pEntity as ILayer).Layer == 5)
                {
                    // SPAWN pEntity in "Level1" at their specified DestinationRectangle:
                    (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level1", pEntity, new Vector2((pEntity as IDrawDestinationRectangle).DestinationRectangle.X, (pEntity as IDrawDestinationRectangle).DestinationRectangle.Y));
                }
            }

            #endregion


            #region LAYER 6 - PLAYER / NPC

            #region NPCS

            #region NPC 1

            // DECLARE & INITIALISE and IEntity with reference to "NPC1", name it 'tempEntity':
            IEntity tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["NPC1"];

            // LOAD "Thug_04" texture to "NPC1":
            (tempEntity as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04");

            // SPAWN "NPC1" in "Level1" at position created from Tiled:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level1", tempEntity, tempEntity.Position);

            #endregion


            #region NPC 2

            // INITIALISE tempEntity with reference to "NPC2":
            tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["NPC2"];

            // LOAD "Thug_03" texture to "NPC2":
            (tempEntity as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03");

            // SPAWN "NPC2" in "Level1" at position created from Tiled:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level1", tempEntity, tempEntity.Position);

            #endregion


            #region NPC 3

            // INITIALISE tempEntity with reference to "NPC3":
            tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["NPC3"];

            // LOAD "Thug_03" texture to "NPC3":
            (tempEntity as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03");

            // SPAWN "NPC3" in "Level1" at position created from Tiled:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level1", tempEntity, tempEntity.Position);

            #endregion


            #region NPC 4

            // INITIALISE tempEntity with reference to "NPC4":
            tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["NPC4"];

            // LOAD "Thug_04" texture to "NPC4":
            (tempEntity as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04");

            // SPAWN "NPC4" in "Level1" at position created from Tiled:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level1", tempEntity, tempEntity.Position);

            #endregion


            #region NPC 5

            // INITIALISE tempEntity with reference to "NPC5":
            tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["NPC5"];

            // LOAD "Thug_03" texture to "NPC5":
            (tempEntity as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03");

            // SPAWN "NPC5" in "Level1" at position created from Tiled:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level1", tempEntity, tempEntity.Position);

            #endregion


            #region NPC 6

            // INITIALISE tempEntity with reference to "NPC6":
            tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["NPC6"];

            // LOAD "Thug_04" texture to "NPC6":
            (tempEntity as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04");

            // SPAWN "NPC6" in "Level1" at position created from Tiled:
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

            #endregion


            #region AUDIO

            // CALL PlayAudio on songMgr to play "LevelTrack":
            (_engineManager.GetService<SongManager>() as IPlayAudio).PlayAudio("LevelTrack");

            #endregion

            #endregion
        }

        #endregion


        #region LEVEL 2

        /// <summary>
        /// Creates every dependency for the scene as well as all references for entities in Level 2
        /// </summary>
        private void CreateLevelTwo()
        {
            #region MANAGER REFERENCES

            /// ENTITY MANAGER

            // DECLARE & GET an instance of EntityManager as an IEntityManager, name it 'entityManager':
            IEntityManager entityManager = _engineManager.GetService<EntityManager>() as IEntityManager;

            /// SCENE MANAGER

            // DECLARE & GET an instance of SceneManager as an ISceneManager, name it 'sceneManager':
            ISceneManager sceneManager = _engineManager.GetService<SceneManager>() as ISceneManager;

            /// CREATE ICOMMAND FUNCCOMMAND

            // DECLARE & INSTANTIATE an IFuncCommand<ICommandOneParam<string>> as a new FuncCommand<ICommandOneParam<string>>(), name it 'createCommand':
            IFuncCommand<ICommand> createCommand = (_engineManager.GetService<Factory<IFuncCommand<ICommand>>>() as IFactory<IFuncCommand<ICommand>>).Create<FuncCommandZeroParam<ICommand>>();

            // INITIALISE _createFloor's MethodRef Property with Factory<ICommand>.Create<CommandOneParam<string>>:
            (createCommand as IFuncCommandZeroParam<ICommand>).MethodRef = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<string>>;


            #endregion


            #region SFX COMMAND

            // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam<string>(), name it 'playSFXCommand':
            ICommand playSFXCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<string>>();

            // INITIALISE playSFXCommand's MethodRef Property with reference to SFXManager.PlayAudio:
            (playSFXCommand as ICommandOneParam<string>).MethodRef = (_engineManager.GetService<SFXManager>() as IPlayAudio).PlayAudio;

            #endregion


            #region LEVEL 2 CREATION

            /// SCENE

            // SET _bgColour to Black:
            _bgColour = Color.Black;

            // DECLARE & INSTANTIATE an ICommand as a new CommandZeroParam(), name it 'loadLevelTwo':
            ICommand loadLevelTwo = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandZeroParam>();

            // INITIALISE loadLevelTwo with reference to this method:
            (loadLevelTwo as ICommandZeroParam).MethodRef = CreateLevelTwo;

            // DECLARE & INSTANTIATE an ICommand as a new CommandZeroParam(), name it 'loadVNThree':
            ICommand loadVNThree = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandZeroParam>();

            // INITIALISE loadVNThree with reference to CreateVNThree:
            (loadVNThree as ICommandZeroParam).MethodRef = CreateVNThree;

            // CALL CreateScene() on sceneManager, passing "Level2", a new Dictionary<string, ICommand>, and loadLevelTwo as parameters:
            sceneManager.CreateScene("Level2", (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ICommand>>() as IDictionary<string, ICommand>, loadLevelTwo);

            // CALL UploadNextScene() on sceneManager, passing "VN3", and loadVNThree as parameters:
            sceneManager.UploadNextScene("VN3", loadVNThree);

            // INITIALISE sceneManager with a CollisionManager instance from _engineManager, a new Dictionary<string, IEntity>() and a reference to createCommand for scene "Level2":
            sceneManager.Initialise("Level2", _engineManager.GetService<CollisionManager>() as ICollisionManager,
                (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, IEntity>>() as IDictionary<string, IEntity>, createCommand);

            /// SCENE GRAPH

            // DECLARE & INSTANTIATE an ICommand as a new CommandTwoParam<string, string>(), 'nextSceneCommand':
            ICommand nextSceneCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandTwoParam<string, string>>();

            // INITIALISE nextSceneCommand with a reference to SceneManager.LoadNextScene():
            (nextSceneCommand as ICommandTwoParam<string, string>).MethodRef = sceneManager.LoadNextScene;

            // DECLARE & INSTANTIATE an ICommand as a new CommandZeroParam(), name it 'stopAudioCommand':
            ICommand stopAudioCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandZeroParam>();

            // INITIALISE stopAudioCommand with a reference to SongManager.StopAudio():
            (stopAudioCommand as ICommandZeroParam).MethodRef = (_engineManager.GetService<SongManager>() as ISongManager).StopAudio;

            // INITIALISE the current scene with "NextScene" and nextSceneCommand as parameters:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string, ICommand>).Initialise("NextScene", nextSceneCommand);

            // INITIALISE the current scene with "StopAudio" and stopAudioCommand as parameters:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string, ICommand>).Initialise("StopAudio", stopAudioCommand);

            #region DISPLAYABLE CREATION

            #region LAYER 1-5 - TILES

            // DECLARE & INSTANTIATE a new TmxMap(), name it '_map', passing a .tmx file as a parameter:
            TmxMap map = new TmxMap("..\\..\\..\\..\\Content\\RIRR\\Levels\\Level2.tmx");

            // DECLARE & INITIALISE a Texture2D, name it '_tilesetTex', give value of _map's Tilesets[0]'s name:
            Texture2D tilesetTex = Content.Load<Texture2D>("RIRR\\Levels\\Tiles\\" + map.Tilesets[0].Name);

            // CALL CreateLevelLayout() on LevelLayoutMaker, passing "Level2", _map and _tilesetTex as parameters:
            (_engineManager.GetService<LevelLayoutMaker>() as ILevelLayoutMaker).CreateLevelLayout("Level2", map, tilesetTex);


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

            // INITIALISE artefactBehaviour with a reference to playSFXCommand:
            (artefactBehaviour as IInitialiseParam<ICommand>).Initialise(playSFXCommand);

            // INITIALISE artefactState with a reference to artefactBehaviour:
            (artefactState as IInitialiseParam<IEventListener<CollisionEventArgs>>).Initialise(artefactBehaviour);

            #endregion


            #region ENTITY

            // INITIALISE "Item909" with a reference to artefactState:
            // NAMED 909 DUE TO PLACEMENT IN TILED FILE
            (entityManager.GetDictionary()["Item909"] as IInitialiseParam<IState>).Initialise(artefactState);

            // INITIALISE "Item450" with reference to artefactBehaviour:
            (entityManager.GetDictionary()["Item909"] as IInitialiseParam<IEventListener<CollisionEventArgs>>).Initialise(artefactBehaviour);

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

            // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam<string>, name it 'nextLevelCommand':
            ICommand nextLevelCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<string>>();

            // INITIALISE nextLevelCommand's MethodRef Property with a reference to sceneManager.ReturnCurrentScene().GoToNextScene:
            (nextLevelCommand as ICommandOneParam<string>).MethodRef = sceneManager.ReturnCurrentScene().GoToNextScene;

            // INITIALISE levelChangeBehaviour with a reference to levelChangeCommand:
            (levelChangeBehaviour as IInitialiseParam<ICommand>).Initialise(nextLevelCommand);

            // INITIALISE levelChangeBehaviour with a value of "VN3":
            (levelChangeBehaviour as IInitialiseParam<string>).Initialise("VN3");

            // INITIALISE artefactState with a reference to levelChangeBehaviour:
            (levelChangeState as IInitialiseParam<IEventListener<CollisionEventArgs>>).Initialise(levelChangeBehaviour);

            #endregion


            #region ENTITY

            // INITIALISE "LevelChange67" with a reference to levelChangeState:
            // NAMED 67 DUE TO PLACEMENT IN TILED FILE
            (entityManager.GetDictionary()["LevelChange67"] as IInitialiseParam<IState>).Initialise(levelChangeState);

            // INITIALISE "LevelChange67" with reference to levelChangeBehaviour:
            (entityManager.GetDictionary()["LevelChange67"] as IInitialiseParam<IEventListener<CollisionEventArgs>>).Initialise(levelChangeBehaviour);

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
            ICommand camPosChangeCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandTwoParam<Vector2, Vector2>>();

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

            /// HEALTH BEHAVIOUR

            // INITIALISE healthBehaviour with a new Dictionary<string, ICommand>():
            (healthBehaviour as IInitialiseParam<IDictionary<string, ICommand>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ICommand>>() as IDictionary<string, ICommand>);

            // DECLARE & INSTANTIATE an ICommand as a new CommandZeroParam(), name it 'resetSceneCommand':
            ICommand resetSceneCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandZeroParam>();

            // INITIALISE resetSceneCommand's MethodRef Property to current scene's ResetScene() method:
            (resetSceneCommand as ICommandZeroParam).MethodRef = (sceneManager.ReturnCurrentScene() as IResetScene).ResetScene;

            // INITIALISE healthBehaviour with "ResetScene" and a reference to resetSceneCommand:
            (healthBehaviour as IInitialiseParam<string, ICommand>).Initialise("ResetScene", resetSceneCommand);


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

            // INITIALISE npcBehaviour with a reference to playSFXCommand:
            (npcBehaviour as IInitialiseParam<ICommand>).Initialise(playSFXCommand);

            // INITIALISE npcBehaviour Direction Property with a new Vector2(0.8f, 0):
            (npcBehaviour as IDirection).Direction = new Vector2(0.8f, 0);

            // INITIALISE npcState with a reference to npcBehaviour:
            (npcState as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            #region ANIMATIONS

            /// LEFT

            // INSTANTIATE animationLeft as a new Animation():
            animationLeft = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationLeft to "Thug_04":
            (animationLeft as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04");

            // SET Row Property value of animationLeft to '14, 18':
            animationLeft.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationLeft to '4':
            animationLeft.Row = 4;

            // SET MsPerFrame Property value of animationLeft to '175':
            animationLeft.MsPerFrame = 175;

            // INITIALISE npcBehaviour with a reference to animationLeft.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Left", (animationLeft as IEventListener<UpdateEventArgs>).OnEvent);

            /// RIGHT

            // INSTANTIATE animationRight as a new Animation():
            animationRight = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationRight to "Thug_04":
            (animationRight as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04");

            // SET Row Property value of animationRight to '14, 18':
            animationRight.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationRight to '3':
            animationRight.Row = 3;

            // SET MsPerFrame Property value of animationRight to '175':
            animationRight.MsPerFrame = 175;

            // INITIALISE npcBehaviour with a reference to animationRight.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Right", (animationRight as IEventListener<UpdateEventArgs>).OnEvent);

            #endregion

            #endregion


            #region ENTITY

            // INITIALISE "NPC1" with a reference to npcState:
            (entityManager.GetDictionary()["NPC1"] as IInitialiseParam<IState>).Initialise(npcState);

            // INITIALISE "NPC1" with a reference to npcBehaviour:
            (entityManager.GetDictionary()["NPC1"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            // INITIALISE "NPC1" with a reference to animationLeft:
            (entityManager.GetDictionary()["NPC1"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationLeft as IEventListener<UpdateEventArgs>);

            // INITIALISE "NPC1" with a reference to animationRight:
            (entityManager.GetDictionary()["NPC1"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationRight as IEventListener<UpdateEventArgs>);

            // SET TextureSize Property value of "NPC1" to a new Point() passing animationUp.SpriteSize as a parameter:
            (entityManager.GetDictionary()["NPC1"] as ITexture).TextureSize = new Point(animationUp.SpriteSize.X, animationUp.SpriteSize.Y);

            // SET DrawOrigin of "NPC1" to value of centre of animationUp.SpriteSize.X / 2:
            (entityManager.GetDictionary()["NPC1"] as IRotation).DrawOrigin = new Vector2(animationUp.SpriteSize.X / 2, animationUp.SpriteSize.Y / 2);

            // SET WindowBorder of "NPC1" to value of _screenSize:
            (entityManager.GetDictionary()["NPC1"] as IContainBoundary).WindowBorder = _screenSize;

            #endregion


            #endregion


            #region NPC 2

            #region STATES

            // INSTANTIATE npcState as a new UpdatableCollidableState():
            npcState = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<UpdatableCollidableState>();

            // INITIALISE npcState with a new Dictionary<string, EventArgs>():
            (npcState as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

            // INITIALISE npcState with a new UpdateEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

            // INITIALISE npcState with a new CollisionEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

            #endregion


            #region BEHAVIOUR

            // INSTANTIATE npcBehaviour as a new NPCBehaviour():
            npcBehaviour = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<NPCBehaviour>();

            // INITIALISE npcBehaviour with a new Dictionary<string, EventHandler<UpdateEventArgs>>():
            (npcBehaviour as IInitialiseParam<IDictionary<string, EventHandler<UpdateEventArgs>>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>)
                .Create<Dictionary<string, EventHandler<UpdateEventArgs>>>() as IDictionary<string, EventHandler<UpdateEventArgs>>);

            // INITIALISE npcBehaviour with a reference to playSFXCommand:
            (npcBehaviour as IInitialiseParam<ICommand>).Initialise(playSFXCommand);

            // INITIALISE npcBehaviour Direction Property with a new Vector2(0, 1.2f):
            (npcBehaviour as IDirection).Direction = new Vector2(0f, 1.2f);

            // INITIALISE npcState with a reference to npcBehaviour:
            (npcState as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            #region ANIMATIONS

            /// UP

            // INSTANTIATE animationUp as a new Animation():
            animationUp = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationUp to "Thug_03_Infected":
            (animationUp as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03_Infected");

            // SET Row Property value of animationUp to '14, 18':
            animationUp.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationUp to '2':
            animationUp.Row = 2;

            // SET MsPerFrame Property value of animationUp to '200':
            animationUp.MsPerFrame = 200;

            // INITIALISE npcBehaviour with a reference to animationUp.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Up", (animationUp as IEventListener<UpdateEventArgs>).OnEvent);

            /// DOWN

            // INSTANTIATE animationDown as a new Animation():
            animationDown = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationDown to "Thug_03_Infected":
            (animationDown as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03_Infected");

            // SET Row Property value of animationDown to '14, 18':
            animationDown.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationDown to '1':
            animationDown.Row = 1;

            // SET MsPerFrame Property value of animationDown to '200':
            animationDown.MsPerFrame = 200;

            // INITIALISE npcBehaviour with a reference to animationDown.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Down", (animationDown as IEventListener<UpdateEventArgs>).OnEvent);

            #endregion

            #endregion


            #region ENTITY

            // INITIALISE "NPC2" with a reference to npcState:
            (entityManager.GetDictionary()["NPC2"] as IInitialiseParam<IState>).Initialise(npcState);

            // INITIALISE "NPC2" with a reference to npcBehaviour:
            (entityManager.GetDictionary()["NPC2"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            // INITIALISE "NPC2" with a reference to animationUp:
            (entityManager.GetDictionary()["NPC2"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationUp as IEventListener<UpdateEventArgs>);

            // INITIALISE "NPC2" with a reference to animationDown:
            (entityManager.GetDictionary()["NPC2"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationDown as IEventListener<UpdateEventArgs>);

            // SET TextureSize Property value of "NPC2" to a new Point() passing animationLeft.SpriteSize as a parameter:
            (entityManager.GetDictionary()["NPC2"] as ITexture).TextureSize = new Point(animationLeft.SpriteSize.X, animationLeft.SpriteSize.Y);

            // SET DrawOrigin of "NPC2" to value of centre of animationLeft.SpriteSize.X / 2:
            (entityManager.GetDictionary()["NPC2"] as IRotation).DrawOrigin = new Vector2(animationLeft.SpriteSize.X / 2, animationLeft.SpriteSize.Y / 2);

            // SET WindowBorder of "NPC2" to value of _screenSize:
            (entityManager.GetDictionary()["NPC2"] as IContainBoundary).WindowBorder = _screenSize;

            #endregion

            #endregion


            #region NPC 3

            #region STATES

            // INSTANTIATE npcState as a new UpdatableCollidableState():
            npcState = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<UpdatableCollidableState>();

            // INITIALISE npcState with a new Dictionary<string, EventArgs>():
            (npcState as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

            // INITIALISE npcState with a new UpdateEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

            // INITIALISE npcState with a new CollisionEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

            #endregion


            #region BEHAVIOUR

            // INSTANTIATE npcBehaviour as a new NPCBehaviour():
            npcBehaviour = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<NPCBehaviour>();

            // INITIALISE npcBehaviour with a new Dictionary<string, EventHandler<UpdateEventArgs>>():
            (npcBehaviour as IInitialiseParam<IDictionary<string, EventHandler<UpdateEventArgs>>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>)
                .Create<Dictionary<string, EventHandler<UpdateEventArgs>>>() as IDictionary<string, EventHandler<UpdateEventArgs>>);

            // INITIALISE npcBehaviour with a reference to playSFXCommand:
            (npcBehaviour as IInitialiseParam<ICommand>).Initialise(playSFXCommand);

            // INITIALISE npcBehaviour Direction Property with a new Vector2(0, 0.8f):
            (npcBehaviour as IDirection).Direction = new Vector2(0, 0.8f);

            // INITIALISE npcState with a reference to npcBehaviour:
            (npcState as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            #region ANIMATIONS

            /// UP

            // INSTANTIATE animationUp as a new Animation():
            animationUp = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationUp to "Thug_03":
            (animationUp as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03");

            // SET Row Property value of animationUp to '14, 18':
            animationUp.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationUp to '2':
            animationUp.Row = 2;

            // SET MsPerFrame Property value of animationUp to '175':
            animationUp.MsPerFrame = 175;

            // INITIALISE npcBehaviour with a reference to animationUp.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Up", (animationUp as IEventListener<UpdateEventArgs>).OnEvent);

            /// DOWN

            // INSTANTIATE animationDown as a new Animation():
            animationDown = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationDown to "Thug_03":
            (animationDown as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03");

            // SET Row Property value of animationDown to '14, 18':
            animationDown.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationDown to '1':
            animationDown.Row = 1;

            // SET MsPerFrame Property value of animationDown to '175':
            animationDown.MsPerFrame = 175;

            // INITIALISE npcBehaviour with a reference to animationDown.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Down", (animationDown as IEventListener<UpdateEventArgs>).OnEvent);

            #endregion

            #endregion


            #region ENTITY

            // INITIALISE "NPC3" with a reference to npcState:
            (entityManager.GetDictionary()["NPC3"] as IInitialiseParam<IState>).Initialise(npcState);

            // INITIALISE "NPC3" with a reference to npcBehaviour:
            (entityManager.GetDictionary()["NPC3"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            // INITIALISE "NPC3" with a reference to animationUp:
            (entityManager.GetDictionary()["NPC3"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationUp as IEventListener<UpdateEventArgs>);

            // INITIALISE "NPC3" with a reference to animationDown:
            (entityManager.GetDictionary()["NPC3"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationDown as IEventListener<UpdateEventArgs>);

            // SET TextureSize Property value of "NPC3" to a new Point() passing animationUp.SpriteSize as a parameter:
            (entityManager.GetDictionary()["NPC3"] as ITexture).TextureSize = new Point(animationUp.SpriteSize.X, animationUp.SpriteSize.Y);

            // SET DrawOrigin of "NPC3" to value of centre of animationUp.SpriteSize.X / 2:
            (entityManager.GetDictionary()["NPC3"] as IRotation).DrawOrigin = new Vector2(animationUp.SpriteSize.X / 2, animationUp.SpriteSize.Y / 2);

            // SET WindowBorder of "NPC3" to value of _screenSize:
            (entityManager.GetDictionary()["NPC3"] as IContainBoundary).WindowBorder = _screenSize;

            #endregion

            #endregion


            #region NPC 4

            #region STATES

            // INSTANTIATE npcState as a new UpdatableCollidableState():
            npcState = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<UpdatableCollidableState>();

            // INITIALISE npcState with a new Dictionary<string, EventArgs>():
            (npcState as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

            // INITIALISE npcState with a new UpdateEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

            // INITIALISE npcState with a new CollisionEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

            #endregion


            #region BEHAVIOUR

            // INSTANTIATE npcBehaviour as a new NPCBehaviour():
            npcBehaviour = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<NPCBehaviour>();

            // INITIALISE npcBehaviour with a new Dictionary<string, EventHandler<UpdateEventArgs>>():
            (npcBehaviour as IInitialiseParam<IDictionary<string, EventHandler<UpdateEventArgs>>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>)
                .Create<Dictionary<string, EventHandler<UpdateEventArgs>>>() as IDictionary<string, EventHandler<UpdateEventArgs>>);

            // INITIALISE npcBehaviour with a reference to playSFXCommand:
            (npcBehaviour as IInitialiseParam<ICommand>).Initialise(playSFXCommand);

            // INITIALISE npcBehaviour Direction Property with a new Vector2(1.2f, 0):
            (npcBehaviour as IDirection).Direction = new Vector2(1.2f, 0);

            // INITIALISE npcState with a reference to npcBehaviour:
            (npcState as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            #region ANIMATIONS

            /// LEFT

            // INSTANTIATE animationLeft as a new Animation():
            animationLeft = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationLeft to "Thug_04":
            (animationLeft as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04_Infected");

            // SET Row Property value of animationLeft to '14, 18':
            animationLeft.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationLeft to '4':
            animationLeft.Row = 4;

            // SET MsPerFrame Property value of animationLeft to '200':
            animationLeft.MsPerFrame = 200;

            // INITIALISE npcBehaviour with a reference to animationUp.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Left", (animationLeft as IEventListener<UpdateEventArgs>).OnEvent);

            /// RIGHT

            // INSTANTIATE animationRight as a new Animation():
            animationRight = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationRight to "Thug_04":
            (animationRight as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04_Infected");

            // SET Row Property value of animationRight to '14, 18':
            animationRight.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationRight to '3':
            animationRight.Row = 3;

            // SET MsPerFrame Property value of animationRight to '200':
            animationRight.MsPerFrame = 200;

            // INITIALISE npcBehaviour with a reference to animationRight.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Right", (animationRight as IEventListener<UpdateEventArgs>).OnEvent);

            #endregion

            #endregion


            #region ENTITY

            // INITIALISE "NPC4" with a reference to npcState:
            (entityManager.GetDictionary()["NPC4"] as IInitialiseParam<IState>).Initialise(npcState);

            // INITIALISE "NPC4" with a reference to npcBehaviour:
            (entityManager.GetDictionary()["NPC4"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            // INITIALISE "NPC4" with a reference to animationLeft:
            (entityManager.GetDictionary()["NPC4"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationLeft as IEventListener<UpdateEventArgs>);

            // INITIALISE "NPC4" with a reference to animationRight:
            (entityManager.GetDictionary()["NPC4"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationRight as IEventListener<UpdateEventArgs>);

            // SET TextureSize Property value of "NPC4" to a new Point() passing animationUp.SpriteSize as a parameter:
            (entityManager.GetDictionary()["NPC4"] as ITexture).TextureSize = new Point(animationUp.SpriteSize.X, animationUp.SpriteSize.Y);

            // SET DrawOrigin of "NPC4" to value of centre of animationUp.SpriteSize.X / 2:
            (entityManager.GetDictionary()["NPC4"] as IRotation).DrawOrigin = new Vector2(animationUp.SpriteSize.X / 2, animationUp.SpriteSize.Y / 2);

            // SET WindowBorder of "NPC4" to value of _screenSize:
            (entityManager.GetDictionary()["NPC4"] as IContainBoundary).WindowBorder = _screenSize;

            #endregion

            #endregion


            #region NPC 5

            #region STATES

            // INSTANTIATE npcState as a new UpdatableCollidableState():
            npcState = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<UpdatableCollidableState>();

            // INITIALISE npcState with a new Dictionary<string, EventArgs>():
            (npcState as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

            // INITIALISE npcState with a new UpdateEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

            // INITIALISE npcState with a new CollisionEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

            #endregion


            #region BEHAVIOUR

            // INSTANTIATE npcBehaviour as a new NPCBehaviour():
            npcBehaviour = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<NPCBehaviour>();

            // INITIALISE npcBehaviour with a new Dictionary<string, EventHandler<UpdateEventArgs>>():
            (npcBehaviour as IInitialiseParam<IDictionary<string, EventHandler<UpdateEventArgs>>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>)
                .Create<Dictionary<string, EventHandler<UpdateEventArgs>>>() as IDictionary<string, EventHandler<UpdateEventArgs>>);

            // INITIALISE npcBehaviour with a reference to playSFXCommand:
            (npcBehaviour as IInitialiseParam<ICommand>).Initialise(playSFXCommand);

            // INITIALISE npcBehaviour Direction Property with a new Vector2(0.8, 0):
            (npcBehaviour as IDirection).Direction = new Vector2(0.8f, 0);

            // INITIALISE npcState with a reference to npcBehaviour:
            (npcState as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            #region ANIMATIONS

            /// LEFT

            // INSTANTIATE animationLeft as a new Animation():
            animationLeft = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationLeft to "Thug_03":
            (animationLeft as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03");

            // SET Row Property value of animationLeft to '14, 18':
            animationLeft.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationLeft to '4':
            animationLeft.Row = 4;

            // SET MsPerFrame Property value of animationLeft to '175':
            animationLeft.MsPerFrame = 175;

            // INITIALISE npcBehaviour with a reference to animationUp.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Left", (animationLeft as IEventListener<UpdateEventArgs>).OnEvent);

            /// RIGHT

            // INSTANTIATE animationRight as a new Animation():
            animationRight = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationRight to "Thug_03":
            (animationRight as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03");

            // SET Row Property value of animationRight to '14, 18':
            animationRight.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationRight to '3':
            animationRight.Row = 3;

            // SET MsPerFrame Property value of animationRight to '175':
            animationRight.MsPerFrame = 175;

            // INITIALISE npcBehaviour with a reference to animationRight.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Right", (animationRight as IEventListener<UpdateEventArgs>).OnEvent);

            #endregion

            #endregion


            #region ENTITY

            // INITIALISE "NPC5" with a reference to npcState:
            (entityManager.GetDictionary()["NPC5"] as IInitialiseParam<IState>).Initialise(npcState);

            // INITIALISE "NPC5" with a reference to npcBehaviour:
            (entityManager.GetDictionary()["NPC5"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            // INITIALISE "NPC5" with a reference to animationLeft:
            (entityManager.GetDictionary()["NPC5"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationLeft as IEventListener<UpdateEventArgs>);

            // INITIALISE "NPC5" with a reference to animationRight:
            (entityManager.GetDictionary()["NPC5"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationRight as IEventListener<UpdateEventArgs>);

            // SET TextureSize Property value of "NPC5" to a new Point() passing animationUp.SpriteSize as a parameter:
            (entityManager.GetDictionary()["NPC5"] as ITexture).TextureSize = new Point(animationUp.SpriteSize.X, animationUp.SpriteSize.Y);

            // SET DrawOrigin of "NPC5" to value of centre of animationUp.SpriteSize.X / 2:
            (entityManager.GetDictionary()["NPC5"] as IRotation).DrawOrigin = new Vector2(animationUp.SpriteSize.X / 2, animationUp.SpriteSize.Y / 2);

            // SET WindowBorder of "NPC5" to value of _screenSize:
            (entityManager.GetDictionary()["NPC5"] as IContainBoundary).WindowBorder = _screenSize;

            #endregion

            #endregion


            #region NPC 6

            #region STATES

            // INSTANTIATE npcState as a new UpdatableCollidableState():
            npcState = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<UpdatableCollidableState>();

            // INITIALISE npcState with a new Dictionary<string, EventArgs>():
            (npcState as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

            // INITIALISE npcState with a new UpdateEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

            // INITIALISE npcState with a new CollisionEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

            #endregion


            #region BEHAVIOUR

            // INSTANTIATE npcBehaviour as a new NPCBehaviour():
            npcBehaviour = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<NPCBehaviour>();

            // INITIALISE npcBehaviour with a new Dictionary<string, EventHandler<UpdateEventArgs>>():
            (npcBehaviour as IInitialiseParam<IDictionary<string, EventHandler<UpdateEventArgs>>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>)
                .Create<Dictionary<string, EventHandler<UpdateEventArgs>>>() as IDictionary<string, EventHandler<UpdateEventArgs>>);

            // INITIALISE npcBehaviour with a reference to playSFXCommand:
            (npcBehaviour as IInitialiseParam<ICommand>).Initialise(playSFXCommand);

            // INITIALISE npcBehaviour Direction Property with a new Vector2(0, 1.2f):
            (npcBehaviour as IDirection).Direction = new Vector2(0, 1.2f);

            // INITIALISE npcState with a reference to npcBehaviour:
            (npcState as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            #region ANIMATIONS

            /// UP

            // INSTANTIATE animationUp as a new Animation():
            animationUp = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationUp to "Thug_04_Infected":
            (animationUp as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04_Infected");

            // SET Row Property value of animationUp to '14, 18':
            animationUp.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationUp to '2':
            animationUp.Row = 2;

            // SET MsPerFrame Property value of animationUp to '200':
            animationUp.MsPerFrame = 200;

            // INITIALISE npcBehaviour with a reference to animationUp.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Up", (animationUp as IEventListener<UpdateEventArgs>).OnEvent);

            /// DOWN

            // INSTANTIATE animationDown as a new Animation():
            animationDown = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationDown to "Thug_04_Infected":
            (animationDown as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04_Infected");

            // SET Row Property value of animationDown to '14, 18':
            animationDown.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationDown to '1':
            animationDown.Row = 1;

            // SET MsPerFrame Property value of animationDown to '200':
            animationDown.MsPerFrame = 200;

            // INITIALISE npcBehaviour with a reference to animationDown.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Down", (animationDown as IEventListener<UpdateEventArgs>).OnEvent);

            #endregion

            #endregion


            #region ENTITY

            // INITIALISE "NPC6" with a reference to npcState:
            (entityManager.GetDictionary()["NPC6"] as IInitialiseParam<IState>).Initialise(npcState);

            // INITIALISE "NPC6" with a reference to npcBehaviour:
            (entityManager.GetDictionary()["NPC6"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            // INITIALISE "NPC6" with a reference to animationUp:
            (entityManager.GetDictionary()["NPC6"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationUp as IEventListener<UpdateEventArgs>);

            // INITIALISE "NPC6" with a reference to animationDown:
            (entityManager.GetDictionary()["NPC6"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationDown as IEventListener<UpdateEventArgs>);

            // SET TextureSize Property value of "NPC6" to a new Point() passing animationUp.SpriteSize as a parameter:
            (entityManager.GetDictionary()["NPC6"] as ITexture).TextureSize = new Point(animationUp.SpriteSize.X, animationUp.SpriteSize.Y);

            // SET DrawOrigin of "NPC6" to value of centre of animationUp.SpriteSize.X / 2:
            (entityManager.GetDictionary()["NPC6"] as IRotation).DrawOrigin = new Vector2(animationUp.SpriteSize.X / 2, animationUp.SpriteSize.Y / 2);

            // SET WindowBorder of "NPC6" to value of _screenSize:
            (entityManager.GetDictionary()["NPC6"] as IContainBoundary).WindowBorder = _screenSize;

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

            // INITIALISE healthBehaviour with "UpdateHealthDisplay" and a reference to hPBarHealthChangeCommand:
            (healthBehaviour as IInitialiseParam<string, ICommand>).Initialise("UpdateHealthDisplay", hPBarHealthChangeCommand);

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


            #region SPAWNING

            #region TILES - LAYERS 1-5

            // DECLARE & INITIALISE an IDictionary<string, IEntity>, name it 'tempEntityDict', give return value of EntityManager.GetDictionary():
            IDictionary<string, IEntity> tempEntityDict = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary();

            // FOREACH IEntity in tempEntityDict.Values:
            foreach (IEntity pEntity in tempEntityDict.Values)
            {
                // IF pEntity is on Layer 1 or Layer 2:
                if ((pEntity as ILayer).Layer == 1 || (pEntity as ILayer).Layer == 2)
                {
                    // SPAWN pEntity in "Level2" at their specified DestinationRectangle:
                    (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level2", pEntity, new Vector2((pEntity as IDrawDestinationRectangle).DestinationRectangle.X, (pEntity as IDrawDestinationRectangle).DestinationRectangle.Y));
                }
            }

            // FOREACH IEntity in tempEntityDict.Values:
            // NEEDS TO BE DONE TWICE DUE TO WALLS AND FLOORS OVERLAPPING OBSTACLES WHEN RESETTING
            foreach (IEntity pEntity in tempEntityDict.Values)
            {
                // IF pEntity is on Layer 3, Layer 4, or Layer 5:
                if ((pEntity as ILayer).Layer == 3 || (pEntity as ILayer).Layer == 4 || (pEntity as ILayer).Layer == 5)
                {
                    // SPAWN pEntity in "Level2" at their specified DestinationRectangle:
                    (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level2", pEntity, new Vector2((pEntity as IDrawDestinationRectangle).DestinationRectangle.X, (pEntity as IDrawDestinationRectangle).DestinationRectangle.Y));
                }
            }

            #endregion


            #region LAYER 6 - PLAYER / NPC

            #region NPCS

            #region NPC 1

            // DECLARE & INITIALISE and IEntity with reference to "NPC1", name it 'tempEntity':
            IEntity tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["NPC1"];

            // LOAD "Thug_04" texture to "NPC1":
            (tempEntity as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04");

            // SPAWN "NPC1" in "Level2" at position created from Tiled:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level2", tempEntity, tempEntity.Position);

            #endregion


            #region NPC 2

            // INITIALISE tempEntity with reference to "NPC2":
            tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["NPC2"];

            // LOAD "Thug_03_Infected" texture to "NPC2":
            (tempEntity as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03_Infected");

            // SPAWN "NPC2" in "Level2" at position created from Tiled:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level2", tempEntity, tempEntity.Position);

            #endregion


            #region NPC 3

            // INITIALISE tempEntity with reference to "NPC3":
            tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["NPC3"];

            // LOAD "Thug_03" texture to "NPC3":
            (tempEntity as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03");

            // SPAWN "NPC3" in "Level2" at position created from Tiled:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level2", tempEntity, tempEntity.Position);

            #endregion


            #region NPC 4

            // INITIALISE tempEntity with reference to "NPC4":
            tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["NPC4"];

            // LOAD "Thug_04_Infected" texture to "NPC4":
            (tempEntity as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04_Infected");

            // SPAWN "NPC4" in "Level2" at position created from Tiled:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level2", tempEntity, tempEntity.Position);

            #endregion


            #region NPC 5

            // INITIALISE tempEntity with reference to "NPC5":
            tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["NPC5"];

            // LOAD "Thug_03" texture to "NPC5":
            (tempEntity as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03");

            // SPAWN "NPC5" in "Level2" at position created from Tiled:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level2", tempEntity, tempEntity.Position);

            #endregion


            #region NPC 6

            // INITIALISE tempEntity with reference to "NPC6":
            tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["NPC6"];

            // LOAD "Thug_04_Infected" texture to "NPC6":
            (tempEntity as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04_Infected");

            // SPAWN "NPC6" in "Level2" at position created from Tiled:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level2", tempEntity, tempEntity.Position);

            #endregion

            #endregion


            #region PLAYER

            // INITIALISE tempEntity with reference to "Player1":
            tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["Player1"];

            // LOAD "Gerald" texture to "Player1":
            (tempEntity as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Gerald");

            // SPAWN "Player1" in "Level2" at position created from Tiled:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level2", tempEntity, tempEntity.Position);

            #endregion


            #region CAMERA

            // LOAD "ViewRange" texture to "Camera":
            ((_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["Camera"] as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/ViewRange");

            // SPAWN "Camera" in "Level2" at position of tempEntity:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level2", (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["Camera"], (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["Player1"].Position);

            #endregion

            #endregion


            #region LAYER 7 - GUI

            // LOAD "HPBarShroud" texture to "HPBarShroud":
            ((_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["HPBar"] as ITexture).Texture = Content.Load<Texture2D>("RIRR/GUI/HPBar");

            // SPAWN "Camera" in "Level2" at position of tempEntity:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level2", (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["HPBar"], (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["Camera"].Position);

            // LOAD "HPBarShroud" texture to "HPBarShroud":
            ((_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["HPBarShroud"] as ITexture).Texture = Content.Load<Texture2D>("RIRR/GUI/HPBarShroud");

            // SPAWN "Camera" in "Level2" at position of tempEntity:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level2", (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["HPBarShroud"], (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["Camera"].Position);

            #endregion

            #endregion


            #region AUDIO

            // CALL PlayAudio() on SongManager to play "LevelTrack":
            (_engineManager.GetService<SongManager>() as IPlayAudio).PlayAudio("LevelTrack");

            #endregion

            #endregion        
        }

        #endregion


        #region LEVEL 3

        /// <summary>
        /// Creates every dependency for the scene as well as all references for entities in Level 3
        /// </summary>
        private void CreateLevelThree()
        {
            #region MANAGER REFERENCES

            /// ENTITY MANAGER

            // DECLARE & GET an instance of EntityManager as an IEntityManager, name it 'entityManager':
            IEntityManager entityManager = _engineManager.GetService<EntityManager>() as IEntityManager;

            /// SCENE MANAGER

            // DECLARE & GET an instance of SceneManager as an ISceneManager, name it 'sceneManager':
            ISceneManager sceneManager = _engineManager.GetService<SceneManager>() as ISceneManager;

            /// CREATE ICOMMAND FUNCCOMMAND

            // DECLARE & INSTANTIATE an IFuncCommand<ICommandOneParam<string>> as a new FuncCommand<ICommandOneParam<string>>(), name it 'createCommand':
            IFuncCommand<ICommand> createCommand = (_engineManager.GetService<Factory<IFuncCommand<ICommand>>>() as IFactory<IFuncCommand<ICommand>>).Create<FuncCommandZeroParam<ICommand>>();

            // INITIALISE _createFloor's MethodRef Property with Factory<ICommand>.Create<CommandOneParam<string>>:
            (createCommand as IFuncCommandZeroParam<ICommand>).MethodRef = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<string>>;


            #endregion


            #region SFX COMMAND

            // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam<string>(), name it 'playSFXCommand':
            ICommand playSFXCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<string>>();

            // INITIALISE playSFXCommand's MethodRef Property with reference to SFXManager.PlayAudio:
            (playSFXCommand as ICommandOneParam<string>).MethodRef = (_engineManager.GetService<SFXManager>() as IPlayAudio).PlayAudio;

            #endregion


            #region LEVEL 3 CREATION

            /// SCENE

            // SET _bgColour to Black:
            _bgColour = Color.Black;

            // DECLARE & INSTANTIATE an ICommand as a new CommandZeroParam(), name it 'loadLevelThree':
            ICommand loadLevelThree = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandZeroParam>();

            // INITIALISE loadLevelThree with reference to CreateLevelThree:
            (loadLevelThree as ICommandZeroParam).MethodRef = CreateLevelThree;

            // DECLARE & INSTANTIATE an ICommand as a new CommandZeroParam(), name it 'loadEpilogue':
            ICommand loadEpilogue = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandZeroParam>();

            // INITIALISE loadEpilogue with reference to CreateVNEpilogue:
            (loadEpilogue as ICommandZeroParam).MethodRef = CreateVNEpilogue;

            // CALL CreateScene() on sceneManager, passing "Level3", a new Dictionary<string, ICommand>, and loadLevelThree as parameters:
            sceneManager.CreateScene("Level3", (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ICommand>>() as IDictionary<string, ICommand>, loadLevelThree);

            // CALL UploadNextScene() on sceneManager, passing "Epilogue", and loadEpilogue as parameters:
            sceneManager.UploadNextScene("Epilogue", loadEpilogue);

            // INITIALISE sceneManager with a CollisionManager instance from _engineManager, a new Dictionary<string, IEntity>() and a reference to createCommand for scene "Level3":
            sceneManager.Initialise("Level3", _engineManager.GetService<CollisionManager>() as ICollisionManager,
                (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, IEntity>>() as IDictionary<string, IEntity>, createCommand);

            /// SCENE GRAPH

            // DECLARE & INSTANTIATE an ICommand as a new CommandTwoParam<string, string>(), 'nextSceneCommand':
            ICommand nextSceneCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandTwoParam<string, string>>();

            // INITIALISE nextSceneCommand with a reference to SceneManager.LoadNextScene():
            (nextSceneCommand as ICommandTwoParam<string, string>).MethodRef = sceneManager.LoadNextScene;

            // DECLARE & INSTANTIATE an ICommand as a new CommandZeroParam(), name it 'stopAudioCommand':
            ICommand stopAudioCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandZeroParam>();

            // INITIALISE stopAudioCommand with a reference to SongManager.StopAudio():
            (stopAudioCommand as ICommandZeroParam).MethodRef = (_engineManager.GetService<SongManager>() as ISongManager).StopAudio;

            // INITIALISE the current scene with "NextScene" and nextSceneCommand as parameters:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string, ICommand>).Initialise("NextScene", nextSceneCommand);

            // INITIALISE the current scene with "StopAudio" and stopAudioCommand as parameters:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string, ICommand>).Initialise("StopAudio", stopAudioCommand);

            #region DISPLAYABLE CREATION

            #region LAYER 1-5 - TILES

            // DECLARE & INSTANTIATE a new TmxMap(), name it '_map', passing a .tmx file as a parameter:
            TmxMap map = new TmxMap("..\\..\\..\\..\\Content\\RIRR\\Levels\\Level3.tmx");

            // DECLARE & INITIALISE a Texture2D, name it '_tilesetTex', give value of _map's Tilesets[0]'s name:
            Texture2D tilesetTex = Content.Load<Texture2D>("RIRR\\Levels\\Tiles\\" + map.Tilesets[0].Name);

            // CALL CreateLevelLayout() on LevelLayoutMaker, passing "Level3", map and tilesetTex as parameters:
            (_engineManager.GetService<LevelLayoutMaker>() as ILevelLayoutMaker).CreateLevelLayout("Level3", map, tilesetTex);


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

            // INITIALISE artefactBehaviour with a reference to playSFXCommand:
            (artefactBehaviour as IInitialiseParam<ICommand>).Initialise(playSFXCommand);

            // INITIALISE artefactState with a reference to artefactBehaviour:
            (artefactState as IInitialiseParam<IEventListener<CollisionEventArgs>>).Initialise(artefactBehaviour);

            #endregion


            #region ENTITY

            // INITIALISE "Item177" with a reference to artefactState:
            // NAMED 177 DUE TO PLACEMENT IN TILED FILE
            (entityManager.GetDictionary()["Item177"] as IInitialiseParam<IState>).Initialise(artefactState);

            // INITIALISE "Item177" with reference to artefactBehaviour:
            (entityManager.GetDictionary()["Item177"] as IInitialiseParam<IEventListener<CollisionEventArgs>>).Initialise(artefactBehaviour);

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

            // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam<string>, name it 'nextLevelCommand':
            ICommand nextLevelCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<string>>();

            // INITIALISE nextLevelCommand's MethodRef Property with a reference to sceneManager.ReturnCurrentScene().GoToNextScene:
            (nextLevelCommand as ICommandOneParam<string>).MethodRef = sceneManager.ReturnCurrentScene().GoToNextScene;

            // INITIALISE levelChangeBehaviour with a reference to levelChangeCommand:
            (levelChangeBehaviour as IInitialiseParam<ICommand>).Initialise(nextLevelCommand);

            // INITIALISE levelChangeBehaviour with a value of "Epilogue":
            (levelChangeBehaviour as IInitialiseParam<string>).Initialise("Epilogue");

            // INITIALISE artefactState with a reference to levelChangeBehaviour:
            (levelChangeState as IInitialiseParam<IEventListener<CollisionEventArgs>>).Initialise(levelChangeBehaviour);

            #endregion


            #region ENTITY

            // INITIALISE "LevelChange752" with a reference to levelChangeState:
            // NAMED 752 DUE TO PLACEMENT IN TILED FILE
            (entityManager.GetDictionary()["LevelChange752"] as IInitialiseParam<IState>).Initialise(levelChangeState);

            // INITIALISE "LevelChange752" with reference to levelChangeBehaviour:
            (entityManager.GetDictionary()["LevelChange752"] as IInitialiseParam<IEventListener<CollisionEventArgs>>).Initialise(levelChangeBehaviour);

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
            ICommand camPosChangeCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandTwoParam<Vector2, Vector2>>();

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

            /// HEALTH BEHAVIOUR

            // INITIALISE healthBehaviour with a new Dictionary<string, ICommand>():
            (healthBehaviour as IInitialiseParam<IDictionary<string, ICommand>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ICommand>>() as IDictionary<string, ICommand>);

            // DECLARE & INSTANTIATE an ICommand as a new CommandZeroParam(), name it 'resetSceneCommand':
            ICommand resetSceneCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandZeroParam>();

            // INITIALISE resetSceneCommand's MethodRef Property to current scene's ResetScene() method:
            (resetSceneCommand as ICommandZeroParam).MethodRef = (sceneManager.ReturnCurrentScene() as IResetScene).ResetScene;

            // INITIALISE healthBehaviour with "ResetScene" and a reference to resetSceneCommand:
            (healthBehaviour as IInitialiseParam<string, ICommand>).Initialise("ResetScene", resetSceneCommand);


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

            // INITIALISE npcBehaviour with a reference to playSFXCommand:
            (npcBehaviour as IInitialiseParam<ICommand>).Initialise(playSFXCommand);

            // INITIALISE npcBehaviour Direction Property with a new Vector2(0, 1.2f):
            (npcBehaviour as IDirection).Direction = new Vector2(0, 1.2f);

            // INITIALISE npcState with a reference to npcBehaviour:
            (npcState as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            #region ANIMATIONS

            /// UP

            // INSTANTIATE animationUp as a new Animation():
            animationUp = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationUp to "Thug_04_Infected":
            (animationUp as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04_Infected");

            // SET Row Property value of animationUp to '14, 18':
            animationUp.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationUp to '2':
            animationUp.Row = 2;

            // SET MsPerFrame Property value of animationUp to '200':
            animationUp.MsPerFrame = 200;

            // INITIALISE npcBehaviour with a reference to animationUp.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Up", (animationUp as IEventListener<UpdateEventArgs>).OnEvent);

            /// DOWN

            // INSTANTIATE animationDown as a new Animation():
            animationDown = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationDown to "Thug_04_Infected":
            (animationDown as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04_Infected");

            // SET Row Property value of animationDown to '14, 18':
            animationDown.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationDown to '1':
            animationDown.Row = 1;

            // SET MsPerFrame Property value of animationDown to '200':
            animationDown.MsPerFrame = 200;

            // INITIALISE npcBehaviour with a reference to animationDown.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Down", (animationDown as IEventListener<UpdateEventArgs>).OnEvent);

            #endregion

            #endregion


            #region ENTITY

            // INITIALISE "NPC1" with a reference to npcState:
            (entityManager.GetDictionary()["NPC1"] as IInitialiseParam<IState>).Initialise(npcState);

            // INITIALISE "NPC1" with a reference to npcBehaviour:
            (entityManager.GetDictionary()["NPC1"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            // INITIALISE "NPC1" with a reference to animationUp:
            (entityManager.GetDictionary()["NPC1"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationUp as IEventListener<UpdateEventArgs>);

            // INITIALISE "NPC1" with a reference to animationDown:
            (entityManager.GetDictionary()["NPC1"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationDown as IEventListener<UpdateEventArgs>);

            // SET TextureSize Property value of "NPC1" to a new Point() passing animationUp.SpriteSize as a parameter:
            (entityManager.GetDictionary()["NPC1"] as ITexture).TextureSize = new Point(animationUp.SpriteSize.X, animationUp.SpriteSize.Y);

            // SET DrawOrigin of "NPC1" to value of centre of animationUp.SpriteSize.X / 2:
            (entityManager.GetDictionary()["NPC1"] as IRotation).DrawOrigin = new Vector2(animationUp.SpriteSize.X / 2, animationUp.SpriteSize.Y / 2);

            // SET WindowBorder of "NPC1" to value of _screenSize:
            (entityManager.GetDictionary()["NPC1"] as IContainBoundary).WindowBorder = _screenSize;

            #endregion

            #endregion


            #region NPC 2

            #region STATES

            // INSTANTIATE npcState as a new UpdatableCollidableState():
            npcState = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<UpdatableCollidableState>();

            // INITIALISE npcState with a new Dictionary<string, EventArgs>():
            (npcState as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

            // INITIALISE npcState with a new UpdateEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

            // INITIALISE npcState with a new CollisionEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

            #endregion


            #region BEHAVIOUR

            // INSTANTIATE npcBehaviour as a new NPCBehaviour():
            npcBehaviour = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<NPCBehaviour>();

            // INITIALISE npcBehaviour with a new Dictionary<string, EventHandler<UpdateEventArgs>>():
            (npcBehaviour as IInitialiseParam<IDictionary<string, EventHandler<UpdateEventArgs>>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>)
                .Create<Dictionary<string, EventHandler<UpdateEventArgs>>>() as IDictionary<string, EventHandler<UpdateEventArgs>>);

            // INITIALISE npcBehaviour with a reference to playSFXCommand:
            (npcBehaviour as IInitialiseParam<ICommand>).Initialise(playSFXCommand);

            // INITIALISE npcBehaviour Direction Property with a new Vector2(1.2f, 0):
            (npcBehaviour as IDirection).Direction = new Vector2(1.2f, 0);

            // INITIALISE npcState with a reference to npcBehaviour:
            (npcState as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            #region ANIMATIONS

            /// LEFT

            // INSTANTIATE animationLeft as a new Animation():
            animationLeft = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationLeft to "Thug_03_Infected":
            (animationLeft as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03_Infected");

            // SET Row Property value of animationLeft to '14, 18':
            animationLeft.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationLeft to '4':
            animationLeft.Row = 4;

            // SET MsPerFrame Property value of animationLeft to '200':
            animationLeft.MsPerFrame = 200;

            // INITIALISE npcBehaviour with a reference to animationUp.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Left", (animationLeft as IEventListener<UpdateEventArgs>).OnEvent);

            /// RIGHT

            // INSTANTIATE animationRight as a new Animation():
            animationRight = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationRight to "Thug_03_Infected":
            (animationRight as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03_Infected");

            // SET Row Property value of animationRight to '14, 18':
            animationRight.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationRight to '3':
            animationRight.Row = 3;

            // SET MsPerFrame Property value of animationRight to '200':
            animationRight.MsPerFrame = 200;

            // INITIALISE npcBehaviour with a reference to animationRight.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Right", (animationRight as IEventListener<UpdateEventArgs>).OnEvent);

            #endregion

            #endregion


            #region ENTITY

            // INITIALISE "NPC2" with a reference to npcState:
            (entityManager.GetDictionary()["NPC2"] as IInitialiseParam<IState>).Initialise(npcState);

            // INITIALISE "NPC2" with a reference to npcBehaviour:
            (entityManager.GetDictionary()["NPC2"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            // INITIALISE "NPC2" with a reference to animationLeft:
            (entityManager.GetDictionary()["NPC2"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationLeft as IEventListener<UpdateEventArgs>);

            // INITIALISE "NPC2" with a reference to animationRight:
            (entityManager.GetDictionary()["NPC2"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationRight as IEventListener<UpdateEventArgs>);

            // SET TextureSize Property value of "NPC2" to a new Point() passing animationLeft.SpriteSize as a parameter:
            (entityManager.GetDictionary()["NPC2"] as ITexture).TextureSize = new Point(animationLeft.SpriteSize.X, animationLeft.SpriteSize.Y);

            // SET DrawOrigin of "NPC2" to value of centre of animationLeft.SpriteSize.X / 2:
            (entityManager.GetDictionary()["NPC2"] as IRotation).DrawOrigin = new Vector2(animationLeft.SpriteSize.X / 2, animationLeft.SpriteSize.Y / 2);

            // SET WindowBorder of "NPC2" to value of _screenSize:
            (entityManager.GetDictionary()["NPC2"] as IContainBoundary).WindowBorder = _screenSize;

            #endregion

            #endregion


            #region NPC 3

            #region STATES

            // INSTANTIATE npcState as a new UpdatableCollidableState():
            npcState = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<UpdatableCollidableState>();

            // INITIALISE npcState with a new Dictionary<string, EventArgs>():
            (npcState as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

            // INITIALISE npcState with a new UpdateEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

            // INITIALISE npcState with a new CollisionEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

            #endregion


            #region BEHAVIOUR

            // INSTANTIATE npcBehaviour as a new NPCBehaviour():
            npcBehaviour = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<NPCBehaviour>();

            // INITIALISE npcBehaviour with a new Dictionary<string, EventHandler<UpdateEventArgs>>():
            (npcBehaviour as IInitialiseParam<IDictionary<string, EventHandler<UpdateEventArgs>>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>)
                .Create<Dictionary<string, EventHandler<UpdateEventArgs>>>() as IDictionary<string, EventHandler<UpdateEventArgs>>);

            // INITIALISE npcBehaviour with a reference to playSFXCommand:
            (npcBehaviour as IInitialiseParam<ICommand>).Initialise(playSFXCommand);

            // INITIALISE npcBehaviour Direction Property with a new Vector2(1.2f, 0):
            (npcBehaviour as IDirection).Direction = new Vector2(1.2f, 0);

            // INITIALISE npcState with a reference to npcBehaviour:
            (npcState as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            #region ANIMATIONS

            /// LEFT

            // INSTANTIATE animationLeft as a new Animation():
            animationLeft = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationLeft to "Thug_03_Infected":
            (animationLeft as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03_Infected");

            // SET Row Property value of animationLeft to '14, 18':
            animationLeft.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationLeft to '4':
            animationLeft.Row = 4;

            // SET MsPerFrame Property value of animationLeft to '200':
            animationLeft.MsPerFrame = 200;

            // INITIALISE npcBehaviour with a reference to animationUp.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Left", (animationLeft as IEventListener<UpdateEventArgs>).OnEvent);

            /// RIGHT

            // INSTANTIATE animationRight as a new Animation():
            animationRight = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationRight to "Thug_03_Infected":
            (animationRight as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03_Infected");

            // SET Row Property value of animationRight to '14, 18':
            animationRight.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationRight to '3':
            animationRight.Row = 3;

            // SET MsPerFrame Property value of animationRight to '200':
            animationRight.MsPerFrame = 200;

            // INITIALISE npcBehaviour with a reference to animationRight.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Right", (animationRight as IEventListener<UpdateEventArgs>).OnEvent);

            #endregion

            #endregion


            #region ENTITY

            // INITIALISE "NPC3" with a reference to npcState:
            (entityManager.GetDictionary()["NPC3"] as IInitialiseParam<IState>).Initialise(npcState);

            // INITIALISE "NPC3" with a reference to npcBehaviour:
            (entityManager.GetDictionary()["NPC3"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            // INITIALISE "NPC3" with a reference to animationLeft:
            (entityManager.GetDictionary()["NPC3"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationLeft as IEventListener<UpdateEventArgs>);

            // INITIALISE "NPC3" with a reference to animationRight:
            (entityManager.GetDictionary()["NPC3"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationRight as IEventListener<UpdateEventArgs>);

            // SET TextureSize Property value of "NPC3" to a new Point() passing animationUp.SpriteSize as a parameter:
            (entityManager.GetDictionary()["NPC3"] as ITexture).TextureSize = new Point(animationUp.SpriteSize.X, animationUp.SpriteSize.Y);

            // SET DrawOrigin of "NPC3" to value of centre of animationUp.SpriteSize.X / 2:
            (entityManager.GetDictionary()["NPC3"] as IRotation).DrawOrigin = new Vector2(animationUp.SpriteSize.X / 2, animationUp.SpriteSize.Y / 2);

            // SET WindowBorder of "NPC3" to value of _screenSize:
            (entityManager.GetDictionary()["NPC3"] as IContainBoundary).WindowBorder = _screenSize;

            #endregion

            #endregion


            #region NPC 4

            #region STATES

            // INSTANTIATE npcState as a new UpdatableCollidableState():
            npcState = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<UpdatableCollidableState>();

            // INITIALISE npcState with a new Dictionary<string, EventArgs>():
            (npcState as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

            // INITIALISE npcState with a new UpdateEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

            // INITIALISE npcState with a new CollisionEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

            #endregion


            #region BEHAVIOUR

            // INSTANTIATE npcBehaviour as a new NPCBehaviour():
            npcBehaviour = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<NPCBehaviour>();

            // INITIALISE npcBehaviour with a new Dictionary<string, EventHandler<UpdateEventArgs>>():
            (npcBehaviour as IInitialiseParam<IDictionary<string, EventHandler<UpdateEventArgs>>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>)
                .Create<Dictionary<string, EventHandler<UpdateEventArgs>>>() as IDictionary<string, EventHandler<UpdateEventArgs>>);

            // INITIALISE npcBehaviour with a reference to playSFXCommand:
            (npcBehaviour as IInitialiseParam<ICommand>).Initialise(playSFXCommand);

            // INITIALISE npcBehaviour Direction Property with a new Vector2(0, 1.2f):
            (npcBehaviour as IDirection).Direction = new Vector2(0, 1.2f);

            // INITIALISE npcState with a reference to npcBehaviour:
            (npcState as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            #region ANIMATIONS

            /// UP

            // INSTANTIATE animationUp as a new Animation():
            animationUp = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationUp to "Thug_04_Infected":
            (animationUp as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04_Infected");

            // SET Row Property value of animationUp to '14, 18':
            animationUp.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationUp to '2':
            animationUp.Row = 2;

            // SET MsPerFrame Property value of animationUp to '200':
            animationUp.MsPerFrame = 200;

            // INITIALISE npcBehaviour with a reference to animationUp.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Up", (animationUp as IEventListener<UpdateEventArgs>).OnEvent);

            /// DOWN

            // INSTANTIATE animationDown as a new Animation():
            animationDown = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationDown to "Thug_04_Infected":
            (animationDown as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04_Infected");

            // SET Row Property value of animationDown to '14, 18':
            animationDown.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationDown to '1':
            animationDown.Row = 1;

            // SET MsPerFrame Property value of animationDown to '200':
            animationDown.MsPerFrame = 200;

            // INITIALISE npcBehaviour with a reference to animationDown.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Down", (animationDown as IEventListener<UpdateEventArgs>).OnEvent);

            #endregion

            #endregion


            #region ENTITY

            // INITIALISE "NPC4" with a reference to npcState:
            (entityManager.GetDictionary()["NPC4"] as IInitialiseParam<IState>).Initialise(npcState);

            // INITIALISE "NPC4" with a reference to npcBehaviour:
            (entityManager.GetDictionary()["NPC4"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            // INITIALISE "NPC4" with a reference to animationUp:
            (entityManager.GetDictionary()["NPC4"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationUp as IEventListener<UpdateEventArgs>);

            // INITIALISE "NPC4" with a reference to animationDown:
            (entityManager.GetDictionary()["NPC4"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationDown as IEventListener<UpdateEventArgs>);

            // SET TextureSize Property value of "NPC4" to a new Point() passing animationUp.SpriteSize as a parameter:
            (entityManager.GetDictionary()["NPC4"] as ITexture).TextureSize = new Point(animationUp.SpriteSize.X, animationUp.SpriteSize.Y);

            // SET DrawOrigin of "NPC4" to value of centre of animationUp.SpriteSize.X / 2:
            (entityManager.GetDictionary()["NPC4"] as IRotation).DrawOrigin = new Vector2(animationUp.SpriteSize.X / 2, animationUp.SpriteSize.Y / 2);

            // SET WindowBorder of "NPC4" to value of _screenSize:
            (entityManager.GetDictionary()["NPC4"] as IContainBoundary).WindowBorder = _screenSize;

            #endregion

            #endregion


            #region NPC 5

            #region STATES

            // INSTANTIATE npcState as a new UpdatableCollidableState():
            npcState = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<UpdatableCollidableState>();

            // INITIALISE npcState with a new Dictionary<string, EventArgs>():
            (npcState as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

            // INITIALISE npcState with a new UpdateEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

            // INITIALISE npcState with a new CollisionEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

            #endregion


            #region BEHAVIOUR

            // INSTANTIATE npcBehaviour as a new NPCBehaviour():
            npcBehaviour = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<NPCBehaviour>();

            // INITIALISE npcBehaviour with a new Dictionary<string, EventHandler<UpdateEventArgs>>():
            (npcBehaviour as IInitialiseParam<IDictionary<string, EventHandler<UpdateEventArgs>>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>)
                .Create<Dictionary<string, EventHandler<UpdateEventArgs>>>() as IDictionary<string, EventHandler<UpdateEventArgs>>);

            // INITIALISE npcBehaviour with a reference to playSFXCommand:
            (npcBehaviour as IInitialiseParam<ICommand>).Initialise(playSFXCommand);

            // INITIALISE npcBehaviour Direction Property with a new Vector2(1.2f, 0):
            (npcBehaviour as IDirection).Direction = new Vector2(1.2f, 0);

            // INITIALISE npcState with a reference to npcBehaviour:
            (npcState as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            #region ANIMATIONS

            /// LEFT

            // INSTANTIATE animationLeft as a new Animation():
            animationLeft = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationLeft to "Thug_03_Infected":
            (animationLeft as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03_Infected");

            // SET Row Property value of animationLeft to '14, 18':
            animationLeft.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationLeft to '4':
            animationLeft.Row = 4;

            // SET MsPerFrame Property value of animationLeft to '200':
            animationLeft.MsPerFrame = 200;

            // INITIALISE npcBehaviour with a reference to animationUp.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Left", (animationLeft as IEventListener<UpdateEventArgs>).OnEvent);

            /// RIGHT

            // INSTANTIATE animationRight as a new Animation():
            animationRight = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationRight to "Thug_03_Infected":
            (animationRight as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03_Infected");

            // SET Row Property value of animationRight to '14, 18':
            animationRight.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationRight to '3':
            animationRight.Row = 3;

            // SET MsPerFrame Property value of animationRight to '200':
            animationRight.MsPerFrame = 200;

            // INITIALISE npcBehaviour with a reference to animationRight.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Right", (animationRight as IEventListener<UpdateEventArgs>).OnEvent);

            #endregion

            #endregion


            #region ENTITY

            // INITIALISE "NPC5" with a reference to npcState:
            (entityManager.GetDictionary()["NPC5"] as IInitialiseParam<IState>).Initialise(npcState);

            // INITIALISE "NPC5" with a reference to npcBehaviour:
            (entityManager.GetDictionary()["NPC5"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            // INITIALISE "NPC5" with a reference to animationLeft:
            (entityManager.GetDictionary()["NPC5"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationLeft as IEventListener<UpdateEventArgs>);

            // INITIALISE "NPC5" with a reference to animationRight:
            (entityManager.GetDictionary()["NPC5"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationRight as IEventListener<UpdateEventArgs>);

            // SET TextureSize Property value of "NPC5" to a new Point() passing animationUp.SpriteSize as a parameter:
            (entityManager.GetDictionary()["NPC5"] as ITexture).TextureSize = new Point(animationUp.SpriteSize.X, animationUp.SpriteSize.Y);

            // SET DrawOrigin of "NPC5" to value of centre of animationUp.SpriteSize.X / 2:
            (entityManager.GetDictionary()["NPC5"] as IRotation).DrawOrigin = new Vector2(animationUp.SpriteSize.X / 2, animationUp.SpriteSize.Y / 2);

            // SET WindowBorder of "NPC5" to value of _screenSize:
            (entityManager.GetDictionary()["NPC5"] as IContainBoundary).WindowBorder = _screenSize;

            #endregion

            #endregion


            #region NPC 6

            #region STATES

            // INSTANTIATE npcState as a new UpdatableCollidableState():
            npcState = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<UpdatableCollidableState>();

            // INITIALISE npcState with a new Dictionary<string, EventArgs>():
            (npcState as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

            // INITIALISE npcState with a new UpdateEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

            // INITIALISE npcState with a new CollisionEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

            #endregion


            #region BEHAVIOUR

            // INSTANTIATE npcBehaviour as a new NPCBehaviour():
            npcBehaviour = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<NPCBehaviour>();

            // INITIALISE npcBehaviour with a new Dictionary<string, EventHandler<UpdateEventArgs>>():
            (npcBehaviour as IInitialiseParam<IDictionary<string, EventHandler<UpdateEventArgs>>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>)
                .Create<Dictionary<string, EventHandler<UpdateEventArgs>>>() as IDictionary<string, EventHandler<UpdateEventArgs>>);

            // INITIALISE npcBehaviour with a reference to playSFXCommand:
            (npcBehaviour as IInitialiseParam<ICommand>).Initialise(playSFXCommand);

            // INITIALISE npcBehaviour Direction Property with a new Vector2(0, 1.2f):
            (npcBehaviour as IDirection).Direction = new Vector2(0, 1.2f);

            // INITIALISE npcState with a reference to npcBehaviour:
            (npcState as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            #region ANIMATIONS

            /// UP

            // INSTANTIATE animationUp as a new Animation():
            animationUp = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationUp to "Thug_04_Infected":
            (animationUp as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04_Infected");

            // SET Row Property value of animationUp to '14, 18':
            animationUp.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationUp to '2':
            animationUp.Row = 2;

            // SET MsPerFrame Property value of animationUp to '200':
            animationUp.MsPerFrame = 200;

            // INITIALISE npcBehaviour with a reference to animationUp.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Up", (animationUp as IEventListener<UpdateEventArgs>).OnEvent);

            /// DOWN

            // INSTANTIATE animationDown as a new Animation():
            animationDown = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationDown to "Thug_04_Infected":
            (animationDown as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04_Infected");

            // SET Row Property value of animationDown to '14, 18':
            animationDown.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationDown to '1':
            animationDown.Row = 1;

            // SET MsPerFrame Property value of animationDown to '200':
            animationDown.MsPerFrame = 200;

            // INITIALISE npcBehaviour with a reference to animationDown.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Down", (animationDown as IEventListener<UpdateEventArgs>).OnEvent);

            #endregion

            #endregion


            #region ENTITY

            // INITIALISE "NPC6" with a reference to npcState:
            (entityManager.GetDictionary()["NPC6"] as IInitialiseParam<IState>).Initialise(npcState);

            // INITIALISE "NPC6" with a reference to npcBehaviour:
            (entityManager.GetDictionary()["NPC6"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            // INITIALISE "NPC6" with a reference to animationUp:
            (entityManager.GetDictionary()["NPC6"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationUp as IEventListener<UpdateEventArgs>);

            // INITIALISE "NPC6" with a reference to animationDown:
            (entityManager.GetDictionary()["NPC6"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationDown as IEventListener<UpdateEventArgs>);

            // SET TextureSize Property value of "NPC6" to a new Point() passing animationUp.SpriteSize as a parameter:
            (entityManager.GetDictionary()["NPC6"] as ITexture).TextureSize = new Point(animationUp.SpriteSize.X, animationUp.SpriteSize.Y);

            // SET DrawOrigin of "NPC6" to value of centre of animationUp.SpriteSize.X / 2:
            (entityManager.GetDictionary()["NPC6"] as IRotation).DrawOrigin = new Vector2(animationUp.SpriteSize.X / 2, animationUp.SpriteSize.Y / 2);

            // SET WindowBorder of "NPC6" to value of _screenSize:
            (entityManager.GetDictionary()["NPC6"] as IContainBoundary).WindowBorder = _screenSize;

            #endregion

            #endregion


            #region NPC 7

            #region STATES

            // INSTANTIATE npcState as a new UpdatableCollidableState():
            npcState = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<UpdatableCollidableState>();

            // INITIALISE npcState with a new Dictionary<string, EventArgs>():
            (npcState as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

            // INITIALISE npcState with a new UpdateEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

            // INITIALISE npcState with a new CollisionEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

            #endregion


            #region BEHAVIOUR

            // INSTANTIATE npcBehaviour as a new NPCBehaviour():
            npcBehaviour = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<NPCBehaviour>();

            // INITIALISE npcBehaviour with a new Dictionary<string, EventHandler<UpdateEventArgs>>():
            (npcBehaviour as IInitialiseParam<IDictionary<string, EventHandler<UpdateEventArgs>>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>)
                .Create<Dictionary<string, EventHandler<UpdateEventArgs>>>() as IDictionary<string, EventHandler<UpdateEventArgs>>);

            // INITIALISE npcBehaviour with a reference to playSFXCommand:
            (npcBehaviour as IInitialiseParam<ICommand>).Initialise(playSFXCommand);

            // INITIALISE npcBehaviour Direction Property with a new Vector2(1.2f, 0):
            (npcBehaviour as IDirection).Direction = new Vector2(1.2f, 0);

            // INITIALISE npcState with a reference to npcBehaviour:
            (npcState as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            #region ANIMATIONS

            /// LEFT

            // INSTANTIATE animationLeft as a new Animation():
            animationLeft = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationLeft to "Thug_03_Infected":
            (animationLeft as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03_Infected");

            // SET Row Property value of animationLeft to '14, 18':
            animationLeft.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationLeft to '4':
            animationLeft.Row = 4;

            // SET MsPerFrame Property value of animationLeft to '200':
            animationLeft.MsPerFrame = 200;

            // INITIALISE npcBehaviour with a reference to animationUp.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Left", (animationLeft as IEventListener<UpdateEventArgs>).OnEvent);

            /// RIGHT

            // INSTANTIATE animationRight as a new Animation():
            animationRight = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationRight to "Thug_03_Infected":
            (animationRight as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03_Infected");

            // SET Row Property value of animationRight to '14, 18':
            animationRight.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationRight to '3':
            animationRight.Row = 3;

            // SET MsPerFrame Property value of animationRight to '200':
            animationRight.MsPerFrame = 200;

            // INITIALISE npcBehaviour with a reference to animationRight.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Right", (animationRight as IEventListener<UpdateEventArgs>).OnEvent);

            #endregion

            #endregion


            #region ENTITY

            // INITIALISE "NPC7" with a reference to npcState:
            (entityManager.GetDictionary()["NPC7"] as IInitialiseParam<IState>).Initialise(npcState);

            // INITIALISE "NPC7" with a reference to npcBehaviour:
            (entityManager.GetDictionary()["NPC7"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            // INITIALISE "NPC7" with a reference to animationLeft:
            (entityManager.GetDictionary()["NPC7"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationLeft as IEventListener<UpdateEventArgs>);

            // INITIALISE "NPC7" with a reference to animationRight:
            (entityManager.GetDictionary()["NPC7"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationRight as IEventListener<UpdateEventArgs>);

            // SET TextureSize Property value of "NPC3" to a new Point() passing animationUp.SpriteSize as a parameter:
            (entityManager.GetDictionary()["NPC7"] as ITexture).TextureSize = new Point(animationUp.SpriteSize.X, animationUp.SpriteSize.Y);

            // SET DrawOrigin of "NPC7" to value of centre of animationUp.SpriteSize.X / 2:
            (entityManager.GetDictionary()["NPC7"] as IRotation).DrawOrigin = new Vector2(animationUp.SpriteSize.X / 2, animationUp.SpriteSize.Y / 2);

            // SET WindowBorder of "NPC7" to value of _screenSize:
            (entityManager.GetDictionary()["NPC7"] as IContainBoundary).WindowBorder = _screenSize;

            #endregion

            #endregion


            #region NPC 4

            #region STATES

            // INSTANTIATE npcState as a new UpdatableCollidableState():
            npcState = (_engineManager.GetService<Factory<IState>>() as IFactory<IState>).Create<UpdatableCollidableState>();

            // INITIALISE npcState with a new Dictionary<string, EventArgs>():
            (npcState as IInitialiseParam<IDictionary<string, EventArgs>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, EventArgs>>() as IDictionary<string, EventArgs>);

            // INITIALISE npcState with a new UpdateEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<UpdateEventArgs>());

            // INITIALISE npcState with a new CollisionEventArgs():
            (npcState as IInitialiseParam<EventArgs>).Initialise((_engineManager.GetService<Factory<EventArgs>>() as IFactory<EventArgs>).Create<CollisionEventArgs>());

            #endregion


            #region BEHAVIOUR

            // INSTANTIATE npcBehaviour as a new NPCBehaviour():
            npcBehaviour = (_engineManager.GetService<Factory<IEventListener<UpdateEventArgs>>>() as IFactory<IEventListener<UpdateEventArgs>>).Create<NPCBehaviour>();

            // INITIALISE npcBehaviour with a new Dictionary<string, EventHandler<UpdateEventArgs>>():
            (npcBehaviour as IInitialiseParam<IDictionary<string, EventHandler<UpdateEventArgs>>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>)
                .Create<Dictionary<string, EventHandler<UpdateEventArgs>>>() as IDictionary<string, EventHandler<UpdateEventArgs>>);

            // INITIALISE npcBehaviour with a reference to playSFXCommand:
            (npcBehaviour as IInitialiseParam<ICommand>).Initialise(playSFXCommand);

            // INITIALISE npcBehaviour Direction Property with a new Vector2(0, 1.2f):
            (npcBehaviour as IDirection).Direction = new Vector2(0, 1.2f);

            // INITIALISE npcState with a reference to npcBehaviour:
            (npcState as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            #region ANIMATIONS

            /// UP

            // INSTANTIATE animationUp as a new Animation():
            animationUp = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationUp to "Thug_04_Infected":
            (animationUp as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04_Infected");

            // SET Row Property value of animationUp to '14, 18':
            animationUp.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationUp to '2':
            animationUp.Row = 2;

            // SET MsPerFrame Property value of animationUp to '200':
            animationUp.MsPerFrame = 200;

            // INITIALISE npcBehaviour with a reference to animationUp.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Up", (animationUp as IEventListener<UpdateEventArgs>).OnEvent);

            /// DOWN

            // INSTANTIATE animationDown as a new Animation():
            animationDown = (_engineManager.GetService<Factory<IAnimation>>() as IFactory<IAnimation>).Create<Animation>();

            // SET Texture Property value of animationDown to "Thug_04_Infected":
            (animationDown as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04_Infected");

            // SET Row Property value of animationDown to '14, 18':
            animationDown.SpriteSize = new Point(14, 18);

            // SET Row Property value of animationDown to '1':
            animationDown.Row = 1;

            // SET MsPerFrame Property value of animationDown to '200':
            animationDown.MsPerFrame = 200;

            // INITIALISE npcBehaviour with a reference to animationDown.OnEvent:
            (npcBehaviour as IInitialiseParam<string, EventHandler<UpdateEventArgs>>).Initialise("Down", (animationDown as IEventListener<UpdateEventArgs>).OnEvent);

            #endregion

            #endregion


            #region ENTITY

            // INITIALISE "NPC8" with a reference to npcState:
            (entityManager.GetDictionary()["NPC8"] as IInitialiseParam<IState>).Initialise(npcState);

            // INITIALISE "NPC8" with a reference to npcBehaviour:
            (entityManager.GetDictionary()["NPC8"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(npcBehaviour);

            // INITIALISE "NPC8" with a reference to animationUp:
            (entityManager.GetDictionary()["NPC8"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationUp as IEventListener<UpdateEventArgs>);

            // INITIALISE "NPC8" with a reference to animationDown:
            (entityManager.GetDictionary()["NPC8"] as IInitialiseParam<IEventListener<UpdateEventArgs>>).Initialise(animationDown as IEventListener<UpdateEventArgs>);

            // SET TextureSize Property value of "NPC8" to a new Point() passing animationUp.SpriteSize as a parameter:
            (entityManager.GetDictionary()["NPC8"] as ITexture).TextureSize = new Point(animationUp.SpriteSize.X, animationUp.SpriteSize.Y);

            // SET DrawOrigin of "NPC8" to value of centre of animationUp.SpriteSize.X / 2:
            (entityManager.GetDictionary()["NPC8"] as IRotation).DrawOrigin = new Vector2(animationUp.SpriteSize.X / 2, animationUp.SpriteSize.Y / 2);

            // SET WindowBorder of "NPC8" to value of _screenSize:
            (entityManager.GetDictionary()["NPC8"] as IContainBoundary).WindowBorder = _screenSize;

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

            // INITIALISE healthBehaviour with "UpdateHealthDisplay" and a reference to hPBarHealthChangeCommand:
            (healthBehaviour as IInitialiseParam<string, ICommand>).Initialise("UpdateHealthDisplay", hPBarHealthChangeCommand);

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


            #region SPAWNING

            #region TILES - LAYERS 1-5

            // DECLARE & INITIALISE an IDictionary<string, IEntity>, name it 'tempEntityDict', give return value of EntityManager.GetDictionary():
            IDictionary<string, IEntity> tempEntityDict = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary();

            // FOREACH IEntity in tempEntityDict.Values:
            foreach (IEntity pEntity in tempEntityDict.Values)
            {
                // IF pEntity is on Layer 1 or Layer 2:
                if ((pEntity as ILayer).Layer == 1 || (pEntity as ILayer).Layer == 2)
                {
                    // SPAWN pEntity in "Level3" at their specified DestinationRectangle:
                    (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level3", pEntity, new Vector2((pEntity as IDrawDestinationRectangle).DestinationRectangle.X, (pEntity as IDrawDestinationRectangle).DestinationRectangle.Y));
                }
            }

            // FOREACH IEntity in tempEntityDict.Values:
            // NEEDS TO BE DONE TWICE DUE TO WALLS AND FLOORS OVERLAPPING OBSTACLES WHEN RESETTING
            foreach (IEntity pEntity in tempEntityDict.Values)
            {
                // IF pEntity is on Layer 3, Layer 4, or Layer 5:
                if ((pEntity as ILayer).Layer == 3 || (pEntity as ILayer).Layer == 4 || (pEntity as ILayer).Layer == 5)
                {
                    // SPAWN pEntity in "Level3" at their specified DestinationRectangle:
                    (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level3", pEntity, new Vector2((pEntity as IDrawDestinationRectangle).DestinationRectangle.X, (pEntity as IDrawDestinationRectangle).DestinationRectangle.Y));
                }
            }

            #endregion


            #region LAYER 6 - PLAYER / NPC

            #region NPCS

            #region NPC 1

            // DECLARE & INITIALISE and IEntity with reference to "NPC1", name it 'tempEntity':
            IEntity tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["NPC1"];

            // LOAD "Thug_04_Infected" texture to "NPC1":
            (tempEntity as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04_Infected");

            // SPAWN "NPC1" in "Level3" at position created from Tiled:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level3", tempEntity, tempEntity.Position);

            #endregion


            #region NPC 2

            // INITIALISE tempEntity with reference to "NPC2":
            tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["NPC2"];

            // LOAD "Thug_03_Infected" texture to "NPC2":
            (tempEntity as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03_Infected");

            // SPAWN "NPC2" in "Level3" at position created from Tiled:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level3", tempEntity, tempEntity.Position);

            #endregion


            #region NPC 3

            // INITIALISE tempEntity with reference to "NPC3":
            tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["NPC3"];

            // LOAD "Thug_03_Infected" texture to "NPC3":
            (tempEntity as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03_Infected");

            // SPAWN "NPC3" in "Level3" at position created from Tiled:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level3", tempEntity, tempEntity.Position);

            #endregion


            #region NPC 4

            // INITIALISE tempEntity with reference to "NPC4":
            tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["NPC4"];

            // LOAD "Thug_04_Infected" texture to "NPC4":
            (tempEntity as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04_Infected");

            // SPAWN "NPC4" in "Level3" at position created from Tiled:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level3", tempEntity, tempEntity.Position);

            #endregion


            #region NPC 5

            // INITIALISE tempEntity with reference to "NPC5":
            tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["NPC5"];

            // LOAD "Thug_03_Infected" texture to "NPC5":
            (tempEntity as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03_Infected");

            // SPAWN "NPC5" in "Level3" at position created from Tiled:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level3", tempEntity, tempEntity.Position);

            #endregion


            #region NPC 6

            // INITIALISE tempEntity with reference to "NPC6":
            tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["NPC6"];

            // LOAD "Thug_04_Infected" texture to "NPC6":
            (tempEntity as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04_Infected");

            // SPAWN "NPC6" in "Level3" at position created from Tiled:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level3", tempEntity, tempEntity.Position);

            #endregion


            #region NPC 7

            // INITIALISE tempEntity with reference to "NPC7":
            tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["NPC7"];

            // LOAD "Thug_03_Infected" texture to "NPC7":
            (tempEntity as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_03_Infected");

            // SPAWN "NPC7" in "Level3" at position created from Tiled:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level3", tempEntity, tempEntity.Position);

            #endregion


            #region NPC 8

            // INITIALISE tempEntity with reference to "NPC8":
            tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["NPC8"];

            // LOAD "Thug_04_Infected" texture to "NPC8":
            (tempEntity as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Thug_04_Infected");

            // SPAWN "NPC8" in "Level3" at position created from Tiled:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level3", tempEntity, tempEntity.Position);

            #endregion

            #endregion


            #region PLAYER

            // INITIALISE tempEntity with reference to "Player1":
            tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["Player1"];

            // LOAD "Gerald" texture to "Player1":
            (tempEntity as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/Gerald");

            // SPAWN "Player1" in "Level3" at position created from Tiled:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level3", tempEntity, tempEntity.Position);

            #endregion


            #region CAMERA

            // LOAD "ViewRange" texture to "Camera":
            ((_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["Camera"] as ITexture).Texture = Content.Load<Texture2D>("RIRR/GameSprites/ViewRange");

            // SPAWN "Camera" in "Level3" at position of tempEntity:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level3", (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["Camera"], (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["Player1"].Position);

            #endregion

            #endregion


            #region LAYER 7 - GUI

            // LOAD "HPBarShroud" texture to "HPBarShroud":
            ((_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["HPBar"] as ITexture).Texture = Content.Load<Texture2D>("RIRR/GUI/HPBar");

            // SPAWN "Camera" in "Level3" at position of tempEntity:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level3", (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["HPBar"], (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["Camera"].Position);

            // LOAD "HPBarShroud" texture to "HPBarShroud":
            ((_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["HPBarShroud"] as ITexture).Texture = Content.Load<Texture2D>("RIRR/GUI/HPBarShroud");

            // SPAWN "Camera" in "Level3" at position of tempEntity:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("Level3", (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["HPBarShroud"], (_engineManager.GetService<EntityManager>() as IEntityManager).GetDictionary()["Camera"].Position);

            #endregion

            #endregion


            #region AUDIO

            // CALL PlayAudio() on songMgr to play "LevelTrack":
            (_engineManager.GetService<SongManager>() as IPlayAudio).PlayAudio("LevelTrack");

            #endregion

            #endregion        
        }

        #endregion
    }
}
