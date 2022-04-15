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

namespace COMP3451Project
{
    /// <summary>
    /// Kernel Class to hold all VN Scene Creation Methods
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 14/04/22
    /// </summary>
    public partial class Kernel
    {
        #region PRE LEVEL 1 VN

        /// <summary>
        /// Creates every dependency for the scene as well as all references for entities in VN Scene One
        /// </summary>
        private void CreateVNOne()
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


            #region VN SCENE ONE CREATION

            /// SCENE

            // SET _bgColour to Grey:
            _bgColour = Color.Gray;

            // DECLARE & INSTANTIATE an ICommand as a new CommandZeroParam(), name it 'loadVNOne':
            ICommand loadVNOne = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandZeroParam>();

            // INITIALISE loadVNOne with reference to this method:
            (loadVNOne as ICommandZeroParam).MethodRef = CreateVNOne;

            // DECLARE & INSTANTIATE an ICommand as a new CommandZeroParam(), name it 'loadLevelOne':
            ICommand loadLevelOne = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandZeroParam>();

            // INITIALISE loadLevelOne with reference to CreateLevelOne:
            (loadLevelOne as ICommandZeroParam).MethodRef = CreateLevelOne;

            // CALL CreateScene() on sceneManager, passing "VN1", a new Dictionary<string, ICommand>, and loadVNOne as parameters:
            sceneManager.CreateCutscene("VN1", (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ICommand>>() as IDictionary<string, ICommand>, loadVNOne);

            // CALL UploadNextScene() on sceneManager, passing "Level1", and loadLevelOne as parameters:
            sceneManager.UploadNextScene("Level1", loadLevelOne);

            // INITIALISE sceneManager with a new Dictionary<string, IEntity>() and a reference to createCommand for scene "VN1":
            sceneManager.Initialise("VN1", (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, IEntity>>() as IDictionary<string, IEntity>, createCommand);

            /// SCENE GRAPH

            #region COMMANDS

            // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam<IKeyboardListener>(), 'unsubscribeKBCommand':
            ICommand unsubscribeKBCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<string>>();

            // INITIALISE unsubscribeKBCommand with a reference to KeyboardManager.Unsubscribe:
            (unsubscribeKBCommand as ICommandOneParam<string>).MethodRef = (_engineManager.GetService<KeyboardManager>() as IKeyboardPublisher).Unsubscribe;

            // DECLARE & INSTANTIATE an ICommand as a new CommandTwoParam<string, string>(), 'nextSceneCommand':
            ICommand nextSceneCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandTwoParam<string, string>>();

            // INITIALISE nextSceneCommand with a reference to SceneManager.LoadNextScene():
            (nextSceneCommand as ICommandTwoParam<string, string>).MethodRef = sceneManager.LoadNextScene;

            // DECLARE & INSTANTIATE an ICommand as a new CommandZeroParam(), name it 'stopAudioCommand':
            ICommand stopAudioCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandZeroParam>();

            // INITIALISE stopAudioCommand with a reference to SongManager.StopAudio():
            (stopAudioCommand as ICommandZeroParam).MethodRef = (_engineManager.GetService<SongManager>() as ISongManager).StopAudio;

            #endregion


            #region INITIALISATIONS

            // SUBSCRIBE current scene to the KeyboardManager:
            (_engineManager.GetService<KeyboardManager>() as IKeyboardPublisher).Subscribe(sceneManager.ReturnCurrentScene() as IKeyboardListener);

            // INITIALISE the current scene with a new Dictionary<string, IDictionary<int, Texture2D>>:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<IDictionary<string, IDictionary<int, Texture2D>>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, IDictionary<int, Texture2D>>>() as IDictionary<string, IDictionary<int, Texture2D>>);

            // INITIALISE the current scene with "UnsubscribeKB" and unsubscribeKBCommand as parameters:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string, ICommand>).Initialise("UnsubscribeKB", unsubscribeKBCommand);

            // INITIALISE the current scene with "NextScene" and nextSceneCommand as parameters:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string, ICommand>).Initialise("NextScene", nextSceneCommand);

            // INITIALISE the current scene with "StopAudio" and stopAudioCommand as parameters:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string, ICommand>).Initialise("StopAudio", stopAudioCommand);

            // INITIALISE the current scene with "Level1" as a parameter:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string>).Initialise("Level1");

            // INITIALISE the current scene with "ComicSansMS" for text font:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<SpriteFont>).Initialise(Content.Load<SpriteFont>("RIRR/Fonts/ComicSansMS"));

            // INITIALISE the current scene with a new Vector2() for text positioning:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<Vector2>).Initialise(new Vector2(100, 600));

            #endregion


            #region ITERATION SETUP

            // DECLARE & INSTANTIATE an IDictionary<int, string> as a new Dictionary<int, string>(), name it 'quoteDict':
            IDictionary<int, string> quoteDict = (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<int, string>>() as IDictionary<int, string>;

            // DECLARE & INSTANTIATE an IDictionary<int, Texture2D> as a new Dictionary<int, Texture2D>(), name it 'geraldTexDict':
            IDictionary<int, Texture2D> geraldTexDict = (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<int, Texture2D>>() as IDictionary<int, Texture2D>;

            // DECLARE & INSTANTIATE an IDictionary<int, string> as a new Dictionary<int, Texture2D>(), name it 'geoffTexDict':
            IDictionary<int, Texture2D> geoffTexDict = (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<int, Texture2D>>() as IDictionary<int, Texture2D>;

            // DECLARE & INSTANTIATE an IEntity as a new DrawableEntity(), name it 'gerald':
            IEntity gerald = (_engineManager.GetService<EntityManager>() as IEntityManager).Create<DrawableEntity>("Gerald");

            // DECLARE & INSTANTIATE an IEntity as a new DrawableEntity(), name it 'geoff':
            IEntity geoff = (_engineManager.GetService<EntityManager>() as IEntityManager).Create<DrawableEntity>("Geoff");

            // DECLARE & INSTANTIATE an IEntity as a new DrawableEntity(), name it 'textBox':
            IEntity textBox = (_engineManager.GetService<EntityManager>() as IEntityManager).Create<DrawableEntity>("TextBox");


            #region ITERATION 0

            // ADD blank string for Iteration 0:
            quoteDict.Add(0, "");

            // ADD Gerald_Dialogue_Normal" at address '0' in geraldTexDict:
            geraldTexDict.Add(0, Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Normal"));

            #endregion


            #region ITERATION 1

            // ADD Gerald quote for Iteration 1:
            quoteDict.Add(1, "GERALD: Ah well... Looks like 'nother boring night...");

            #endregion


            #region ITERATION 2

            // ADD Gerald quote string for Iteration 2:
            quoteDict.Add(2, "GERALD: ...");

            // ADD Gerald_Dialogue_Thinking" at address '2' in geraldTexDict:
            geraldTexDict.Add(2, Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Thinking"));

            #endregion


            #region ITERATION 3

            // ADD Gerald quote string for Iteration 3:
            quoteDict.Add(3, "GERALD: Think I'll go with Whisky toni...");

            // ADD Gerald_Dialogue_Normal" at address '3' in geraldTexDict:
            geraldTexDict.Add(3, Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Normal"));

            #endregion


            #region ITERATION 4

            // ADD Gerald quote string for Iteration 4:
            quoteDict.Add(4, "GERALD: What the actual?!");

            // ADD Gerald_Dialogue_Shocked" at address '4' in geraldTexDict:
            geraldTexDict.Add(4, Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Shocked"));

            #endregion


            #region ITERATION 5

            // ADD Gerald quote string for Iteration 5:
            quoteDict.Add(5, "GERALD: Geoff!! They're gone!");

            // ADD Gerald_Dialogue_Angry" at address '5' in geraldTexDict:
            geraldTexDict.Add(5, Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Angry"));

            #endregion


            #region ITERATION 6

            // ADD Geoff quote string for Iteration 6:
            quoteDict.Add(6, "GEOFF: Eh? You what?!");

            // ADD Geoff_Dialogue_Normal" at address '6' in geoffTexDict:
            geoffTexDict.Add(6, Content.Load<Texture2D>("RIRR/VNSprites/Geoff_Dialogue_Normal"));

            #endregion


            #region ITERATION 7

            // ADD Geoff quote string for Iteration 7:
            quoteDict.Add(7, "GEOFF: *chuckles* The 'eck do ya mean they're gone?!");

            // ADD Geoff_Dialogue_Evil" at address '7' in geoffTexDict:
            geoffTexDict.Add(7, Content.Load<Texture2D>("RIRR/VNSprites/Geoff_Dialogue_Evil"));

            #endregion


            #region ITERATION 8

            // ADD GERALD quote string for Iteration 8:
            quoteDict.Add(8, "GERALD: The artefacts! They're gone!");

            // ADD Gerald_Dialogue_Annoyed" at address '8' in geraldTexDict:
            geraldTexDict.Add(8, Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Annoyed"));

            #endregion


            #region ITERATION 9

            // ADD Geoff quote string for Iteration 9:
            quoteDict.Add(9, "GEOFF: Ah... So 'ats what them miscreants were up ta");

            // ADD Geoff_Dialogue_Evil" at address '9' in geoffTexDict:
            geoffTexDict.Add(9, Content.Load<Texture2D>("RIRR/VNSprites/Geoff_Dialogue_Evil"));

            #endregion


            #region ITERATION 10

            // ADD Gerald quote string for Iteration 10:
            quoteDict.Add(10, "GERALD: What do ya mean by miscreants?!");

            // ADD Gerald_Dialogue_Angry" at address '10' in geraldTexDict:
            geraldTexDict.Add(10, Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Angry"));

            #endregion


            #region ITERATION 11

            // ADD Geoff quote string for Iteration 11:
            quoteDict.Add(11, "GEOFF: Saw a load runnin off 'bout an hour ago and they was 'eading toward the other campus.");

            // ADD Geoff_Dialogue_Normal" at address '11' in geoffTexDict:
            geoffTexDict.Add(11, Content.Load<Texture2D>("RIRR/VNSprites/Geoff_Dialogue_Normal"));

            #endregion


            #region ITERATION 12

            // ADD Gerald quote string for Iteration 12:
            quoteDict.Add(12, "GERALD: Ah great, that hole in the ground...");

            // ADD "Gerald_Dialogue_Annoyed" at address '12' in geraldTexDict:
            geraldTexDict.Add(13, Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Annoyed"));

            #endregion



            #region ITERATION 13

            // ADD Geoff quote string for Iteration 13:
            quoteDict.Add(13, "GEOFF: Well then, shake a leg or you'll be out on your backside *snickers*");

            // ADD "Geoff_Dialogue_Evil" at address '13' in geoffTexDict:
            geoffTexDict.Add(13, Content.Load<Texture2D>("RIRR/VNSprites/Geoff_Dialogue_Evil"));

            #endregion


            #region ITERATION 14

            // ADD Gerald quote string for Iteration 14:
            quoteDict.Add(14, "GERALD: For crying out loud! How will I afford my booze!!??");

            // ADD "Gerald_Dialogue_Angry" at address '14' in geraldTexDict:
            geraldTexDict.Add(14, Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Angry"));

            #endregion


            #region ITERATION 15

            // ADD Geoff quote string for Iteration 15:
            quoteDict.Add(15, "GEOFF: Never mind that! Just grab the artefact and make ya way back to the lift!");

            // ADD "Geoff_Dialogue_Normal" at address '15' in geoffTexDict:
            geoffTexDict.Add(15, Content.Load<Texture2D>("RIRR/VNSprites/Geoff_Dialogue_Normal"));

            #endregion


            #region ITERATION 16

            // ADD Gerald quote string for Iteration 16:
            quoteDict.Add(16, "GERALD: Well if I don't find 'em tonight, I'll be applying for another job in t' morning...");

            // ADD "Gerald_Dialogue_Angry" at address '16' in geraldTexDict:
            geraldTexDict.Add(16, Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Angry"));

            #endregion


            #endregion


            #region DICTIONARY INITIALISATION

            // INITIALISE the current scene with "Gerald" and a reference to geraldTexDict:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string, IDictionary<int, Texture2D>>).Initialise("Gerald", geraldTexDict);

            // INITIALISE the current scene with "Geoff" and a reference to geoffTexDict:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string, IDictionary<int, Texture2D>>).Initialise("Geoff", geoffTexDict);

            // INITIALISE the current scene with a reference to quoteDict:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<IDictionary<int, string>>).Initialise(quoteDict);

            #endregion


            #region SPAWNING

            /// GERALD

            // INITIALISE gerald's Texture Property with "Gerald_Dialogue_Shocked":
            (gerald as ITexture).Texture = Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Shocked");
            
            // SPAWN gerald in "VN1" at (40, 0):
            sceneManager.Spawn("VN1", gerald, new Vector2(40, 0));

            /// GEOFF

            // INITIALISE geoff's Texture Property with "Blank":
            (geoff as ITexture).Texture = Content.Load<Texture2D>("RIRR/VNSprites/Blank");

            // SPAWN geoff in "VN1" at (840, 0):
            sceneManager.Spawn("VN1", geoff, new Vector2(840, 0));

            /// TEXT BOX

            // INITIALISE textBox's Texture Property with "Text_Bubble":
            (textBox as ITexture).Texture = Content.Load<Texture2D>("RIRR/VNSprites/Text_Bubble");

            // SPAWN textBox in "VN1" at (-40, 500):
            sceneManager.Spawn("VN1", textBox, new Vector2(-40, 500));


            #endregion


            #region AUDIO

            // CALL PlayAudio on songMgr to play "VNTrack":
            (_engineManager.GetService<SongManager>() as IPlayAudio).PlayAudio("VNTrack");

            #endregion

            #endregion
        }

        #endregion


        #region PRE LEVEL 2 VN

        /// <summary>
        /// Creates every dependency for the scene as well as all references for entities in VN Scene Two
        /// </summary>
        private void CreateVNTwo()
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


            #region VN SCENE TWO CREATION

            /// SCENE

            // SET _bgColour to Grey:
            _bgColour = Color.Gray;

            // DECLARE & INSTANTIATE an ICommand as a new CommandZeroParam(), name it 'loadVNTwo':
            ICommand loadVNTwo = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandZeroParam>();

            // INITIALISE CreateVNTwo with reference to this method:
            (loadVNTwo as ICommandZeroParam).MethodRef = CreateVNTwo;

            // DECLARE & INSTANTIATE an ICommand as a new CommandZeroParam(), name it 'loadLevelTwo':
            ICommand loadLevelTwo = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandZeroParam>();

            // INITIALISE loadLevelTwo with reference to CreateLevelTwo:
            (loadLevelTwo as ICommandZeroParam).MethodRef = CreateLevelTwo;

            // CALL CreateScene() on sceneManager, passing "VN2", a new Dictionary<string, ICommand>, and loadVNTwo as parameters:
            sceneManager.CreateCutscene("VN2", (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ICommand>>() as IDictionary<string, ICommand>, loadVNTwo);

            // CALL UploadNextScene() on sceneManager, passing "Level2", and loadLevelTwo as parameters:
            sceneManager.UploadNextScene("Level2", loadLevelTwo);

            // INITIALISE sceneManager with a new Dictionary<string, IEntity>() and a reference to createCommand for scene "VN2":
            sceneManager.Initialise("VN2", (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, IEntity>>() as IDictionary<string, IEntity>, createCommand);

            /// SCENE GRAPH

            #region COMMANDS

            // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam<IKeyboardListener>(), 'unsubscribeKBCommand':
            ICommand unsubscribeKBCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<string>>();

            // INITIALISE unsubscribeKBCommand with a reference to KeyboardManager.Unsubscribe:
            (unsubscribeKBCommand as ICommandOneParam<string>).MethodRef = (_engineManager.GetService<KeyboardManager>() as IKeyboardPublisher).Unsubscribe;

            // DECLARE & INSTANTIATE an ICommand as a new CommandTwoParam<string, string>(), 'nextSceneCommand':
            ICommand nextSceneCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandTwoParam<string, string>>();

            // INITIALISE nextSceneCommand with a reference to SceneManager.LoadNextScene():
            (nextSceneCommand as ICommandTwoParam<string, string>).MethodRef = sceneManager.LoadNextScene;

            // DECLARE & INSTANTIATE an ICommand as a new CommandZeroParam(), name it 'stopAudioCommand':
            ICommand stopAudioCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandZeroParam>();

            // INITIALISE stopAudioCommand with a reference to SongManager.StopAudio():
            (stopAudioCommand as ICommandZeroParam).MethodRef = (_engineManager.GetService<SongManager>() as ISongManager).StopAudio;

            #endregion


            #region INITIALISATIONS

            // SUBSCRIBE current scene to the KeyboardManager:
            (_engineManager.GetService<KeyboardManager>() as IKeyboardPublisher).Subscribe(sceneManager.ReturnCurrentScene() as IKeyboardListener);

            // INITIALISE the current scene with a new Dictionary<string, IDictionary<int, Texture2D>>:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<IDictionary<string, IDictionary<int, Texture2D>>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, IDictionary<int, Texture2D>>>() as IDictionary<string, IDictionary<int, Texture2D>>);

            // INITIALISE the current scene with "UnsubscribeKB" and unsubscribeKBCommand as parameters:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string, ICommand>).Initialise("UnsubscribeKB", unsubscribeKBCommand);

            // INITIALISE the current scene with "NextScene" and nextSceneCommand as parameters:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string, ICommand>).Initialise("NextScene", nextSceneCommand);

            // INITIALISE the current scene with "StopAudio" and stopAudioCommand as parameters:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string, ICommand>).Initialise("StopAudio", stopAudioCommand);

            // INITIALISE the current scene with "Level2" as a parameter:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string>).Initialise("Level2");

            // INITIALISE the current scene with "ComicSansMS" for text font:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<SpriteFont>).Initialise(Content.Load<SpriteFont>("RIRR/Fonts/ComicSansMS"));

            // INITIALISE the current scene with a new Vector2() for text positioning:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<Vector2>).Initialise(new Vector2(100, 600));

            #endregion


            #region ITERATION SETUP

            // DECLARE & INSTANTIATE an IDictionary<int, string> as a new Dictionary<int, string>(), name it 'quoteDict':
            IDictionary<int, string> quoteDict = (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<int, string>>() as IDictionary<int, string>;

            // DECLARE & INSTANTIATE an IDictionary<int, Texture2D> as a new Dictionary<int, Texture2D>(), name it 'geraldTexDict':
            IDictionary<int, Texture2D> geraldTexDict = (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<int, Texture2D>>() as IDictionary<int, Texture2D>;

            // DECLARE & INSTANTIATE an IDictionary<int, string> as a new Dictionary<int, Texture2D>(), name it 'geoffTexDict':
            IDictionary<int, Texture2D> geoffTexDict = (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<int, Texture2D>>() as IDictionary<int, Texture2D>;

            // DECLARE & INSTANTIATE an IEntity as a new DrawableEntity(), name it 'gerald':
            IEntity gerald = (_engineManager.GetService<EntityManager>() as IEntityManager).Create<DrawableEntity>("Gerald");

            // DECLARE & INSTANTIATE an IEntity as a new DrawableEntity(), name it 'geoff':
            IEntity geoff = (_engineManager.GetService<EntityManager>() as IEntityManager).Create<DrawableEntity>("Geoff");

            // DECLARE & INSTANTIATE an IEntity as a new DrawableEntity(), name it 'textBox':
            IEntity textBox = (_engineManager.GetService<EntityManager>() as IEntityManager).Create<DrawableEntity>("TextBox");


            #region ITERATION 0

            // ADD blank string for Iteration 0:
            quoteDict.Add(0, "");

            // ADD Gerald_Dialogue_Normal" at address '0' in geraldTexDict:
            geraldTexDict.Add(0, Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Normal"));

            #endregion


            #region ITERATION 1

            // ADD Gerald quote for Iteration 1:
            quoteDict.Add(1, "GERALD: So this is that tourniquet right?");

            #endregion


            #region ITERATION 2

            // ADD Geoff quote string for Iteration 2:
            quoteDict.Add(2, "GEOFF: Yeah, they use 'em to stop form bleedin' out, that ones from the 1800's!");

            // ADD Geoff_Dialogue_Normal" at address '2' in geoffTexDict:
            geoffTexDict.Add(2, Content.Load<Texture2D>("RIRR/VNSprites/Geoff_Dialogue_Normal"));

            #endregion


            #region ITERATION 3

            // ADD Geoff quote string for Iteration 3:
            quoteDict.Add(3, "GEOFF: You stick that on ya leg or arm to stop the blood flow n' keep yourself from bleedin' out.");

            // ADD Geoff_Dialogue_Normal_Stern" at address '3' in geoffTexDict:
            geoffTexDict.Add(3, Content.Load<Texture2D>("RIRR/VNSprites/Geoff_Dialogue_Normal_Stern"));

            #endregion


            #region ITERATION 4

            // ADD GEOFF quote string for Iteration 4:
            quoteDict.Add(4, "GEOFF: Might not want to use it on your neck though, pretty obvious but knowing you...");

            // ADD Geoff_Dialogue_Evil" at address '4' in geoffTexDict:
            geoffTexDict.Add(4, Content.Load<Texture2D>("RIRR/VNSprites/Geoff_Dialogue_Evil"));

            #endregion


            #region ITERATION 5

            // ADD Gerald quote string for Iteration 5:
            quoteDict.Add(5, "GERALD: 'ight then, well I'd better get outta here.");

            // ADD Gerald_Dialogue_Normal" at address '5' in geraldTexDict:
            geraldTexDict.Add(5, Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Normal"));

            #endregion


            #region ITERATION 6

            // ADD GERALD quote string for Iteration 6:
            quoteDict.Add(6, "GERALD: Hang on a sec, these 'miscreants' look a bit off if ya ask me?");

            // ADD Gerald_Dialogue_Thinking" at address '6' in geraldTexDict:
            geraldTexDict.Add(6, Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Thinking"));

            #endregion


            #region ITERATION 7

            // ADD Geoff quote string for Iteration 7:
            quoteDict.Add(7, "GEOFF: *whispers* Stupid portal...");

            // ADD Geoff_Dialogue_Angry" at address '7' in geoffTexDict:
            geoffTexDict.Add(7, Content.Load<Texture2D>("RIRR/VNSprites/Geoff_Dialogue_Angry"));

            #endregion


            #region ITERATION 8

            // ADD Gerald quote string for Iteration 8:
            quoteDict.Add(8, "GERALD: Ok, right that's one down. What's next?? ");

            // ADD Gerald_Dialogue_Normal" at address '8' in geraldTexDict:
            geraldTexDict.Add(8, Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Normal"));

            #endregion


            #region ITERATION 9

            // ADD Geoff quote string for Iteration 9:
            quoteDict.Add(9, "GEOFF: Oh... ah well I think I saw the lot with the porcelain set");

            // ADD Geoff_Dialogue_Evil" at address '9' in geoffTexDict:
            geoffTexDict.Add(9, Content.Load<Texture2D>("RIRR/VNSprites/Geoff_Dialogue_Evil"));

            #endregion


            #region ITERATION 10

            // ADD Gerald quote string for Iteration 10:
            quoteDict.Add(10, "GERALD: I can drink my whiskey from that then, cleaner than the bottle! ");

            // ADD Gerald_Dialogue_Normal" at address '10' in geraldTexDict:
            geraldTexDict.Add(10, Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Normal"));

            #endregion


            #region ITERATION 11

            // ADD Geoff quote string for Iteration 11:
            quoteDict.Add(11, "GEOFF: Don't you dare! That's a priceless set from when the NHS was established in 1948!");

            // ADD Geoff_Dialogue_Angry" at address '11' in geoffTexDict:
            geoffTexDict.Add(11, Content.Load<Texture2D>("RIRR/VNSprites/Geoff_Dialogue_Angry"));

            #endregion


            #region ITERATION 12

            // ADD Gerald quote string for Iteration 12:
            quoteDict.Add(12, "GERALD: To be honest, those mugs are a bit posh for whiskey ain't they...");

            // ADD "Gerald_Dialogue_Shocked" at address '12' in geraldTexDict:
            geraldTexDict.Add(13, Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Shocked"));

            #endregion


            #endregion


            #region DICTIONARY INITIALISATION

            // INITIALISE the current scene with "Gerald" and a reference to geraldTexDict:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string, IDictionary<int, Texture2D>>).Initialise("Gerald", geraldTexDict);

            // INITIALISE the current scene with "Geoff" and a reference to geoffTexDict:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string, IDictionary<int, Texture2D>>).Initialise("Geoff", geoffTexDict);

            // INITIALISE the current scene with a reference to quoteDict:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<IDictionary<int, string>>).Initialise(quoteDict);

            #endregion


            #region SPAWNING

            /// GERALD

            // INITIALISE gerald's Texture Property with "Gerald_Dialogue_Shocked":
            (gerald as ITexture).Texture = Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Shocked");

            // SPAWN gerald in "VN2" at (40, 0):
            sceneManager.Spawn("VN2", gerald, new Vector2(40, 0));

            /// GEOFF

            // INITIALISE geoff's Texture Property with "Blank":
            (geoff as ITexture).Texture = Content.Load<Texture2D>("RIRR/VNSprites/Blank");

            // SPAWN geoff in "VN2" at (840, 0):
            sceneManager.Spawn("VN2", geoff, new Vector2(840, 0));

            /// TEXT BOX

            // INITIALISE textBox's Texture Property with "Text_Bubble":
            (textBox as ITexture).Texture = Content.Load<Texture2D>("RIRR/VNSprites/Text_Bubble");

            // SPAWN textBox in "VN2" at (-40, 500):
            sceneManager.Spawn("VN2", textBox, new Vector2(-40, 500));


            #endregion


            #region AUDIO

            // CALL PlayAudio on songMgr to play "VNTrack":
            (_engineManager.GetService<SongManager>() as IPlayAudio).PlayAudio("VNTrack");

            #endregion

            #endregion
        }


        #endregion


        #region PRE LEVEL 3 VN
        /// <summary>
        /// Creates every dependency for the scene as well as all references for entities in VN Scene Three
        /// </summary>
        private void CreateVNThree()
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


            #region VN SCENE THREE CREATION

            /// SCENE

            // SET _bgColour to Grey:
            _bgColour = Color.Gray;

            // DECLARE & INSTANTIATE an ICommand as a new CommandZeroParam(), name it 'loadVNThree':
            ICommand loadVNThree = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandZeroParam>();

            // INITIALISE CreateVNThree with reference to this method:
            (loadVNThree as ICommandZeroParam).MethodRef = CreateVNThree;

            // DECLARE & INSTANTIATE an ICommand as a new CommandZeroParam(), name it 'loadLevelThree':
            ICommand loadLevelThree = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandZeroParam>();

            // INITIALISE loadLevelThree with reference to CreateLevelThree:
            (loadLevelThree as ICommandZeroParam).MethodRef = CreateLevelThree;

            // CALL CreateScene() on sceneManager, passing "VN3", a new Dictionary<string, ICommand>, and loadVNThree as parameters:
            sceneManager.CreateCutscene("VN3", (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ICommand>>() as IDictionary<string, ICommand>, loadVNThree);

            // CALL UploadNextScene() on sceneManager, passing "Level3", and loadLevelThree as parameters:
            sceneManager.UploadNextScene("Level3", loadLevelThree);

            // INITIALISE sceneManager with a new Dictionary<string, IEntity>() and a reference to createCommand for scene "VN3":
            sceneManager.Initialise("VN3", (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, IEntity>>() as IDictionary<string, IEntity>, createCommand);

            /// SCENE GRAPH

            #region COMMANDS

            // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam<IKeyboardListener>(), 'unsubscribeKBCommand':
            ICommand unsubscribeKBCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<string>>();

            // INITIALISE unsubscribeKBCommand with a reference to KeyboardManager.Unsubscribe:
            (unsubscribeKBCommand as ICommandOneParam<string>).MethodRef = (_engineManager.GetService<KeyboardManager>() as IKeyboardPublisher).Unsubscribe;

            // DECLARE & INSTANTIATE an ICommand as a new CommandTwoParam<string, string>(), 'nextSceneCommand':
            ICommand nextSceneCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandTwoParam<string, string>>();

            // INITIALISE nextSceneCommand with a reference to SceneManager.LoadNextScene():
            (nextSceneCommand as ICommandTwoParam<string, string>).MethodRef = sceneManager.LoadNextScene;

            // DECLARE & INSTANTIATE an ICommand as a new CommandZeroParam(), name it 'stopAudioCommand':
            ICommand stopAudioCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandZeroParam>();

            // INITIALISE stopAudioCommand with a reference to SongManager.StopAudio():
            (stopAudioCommand as ICommandZeroParam).MethodRef = (_engineManager.GetService<SongManager>() as ISongManager).StopAudio;

            #endregion


            #region INITIALISATIONS

            // SUBSCRIBE current scene to the KeyboardManager:
            (_engineManager.GetService<KeyboardManager>() as IKeyboardPublisher).Subscribe(sceneManager.ReturnCurrentScene() as IKeyboardListener);

            // INITIALISE the current scene with a new Dictionary<string, IDictionary<int, Texture2D>>:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<IDictionary<string, IDictionary<int, Texture2D>>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, IDictionary<int, Texture2D>>>() as IDictionary<string, IDictionary<int, Texture2D>>);

            // INITIALISE the current scene with "UnsubscribeKB" and unsubscribeKBCommand as parameters:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string, ICommand>).Initialise("UnsubscribeKB", unsubscribeKBCommand);

            // INITIALISE the current scene with "NextScene" and nextSceneCommand as parameters:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string, ICommand>).Initialise("NextScene", nextSceneCommand);

            // INITIALISE the current scene with "StopAudio" and stopAudioCommand as parameters:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string, ICommand>).Initialise("StopAudio", stopAudioCommand);

            // INITIALISE the current scene with "Level3" as a parameter:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string>).Initialise("Level3");

            // INITIALISE the current scene with "ComicSansMS" for text font:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<SpriteFont>).Initialise(Content.Load<SpriteFont>("RIRR/Fonts/ComicSansMS"));

            // INITIALISE the current scene with a new Vector2() for text positioning:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<Vector2>).Initialise(new Vector2(100, 600));

            #endregion


            #region ITERATION SETUP

            // DECLARE & INSTANTIATE an IDictionary<int, string> as a new Dictionary<int, string>(), name it 'quoteDict':
            IDictionary<int, string> quoteDict = (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<int, string>>() as IDictionary<int, string>;

            // DECLARE & INSTANTIATE an IDictionary<int, Texture2D> as a new Dictionary<int, Texture2D>(), name it 'geraldTexDict':
            IDictionary<int, Texture2D> geraldTexDict = (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<int, Texture2D>>() as IDictionary<int, Texture2D>;

            // DECLARE & INSTANTIATE an IDictionary<int, string> as a new Dictionary<int, Texture2D>(), name it 'geoffTexDict':
            IDictionary<int, Texture2D> geoffTexDict = (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<int, Texture2D>>() as IDictionary<int, Texture2D>;

            // DECLARE & INSTANTIATE an IEntity as a new DrawableEntity(), name it 'gerald':
            IEntity gerald = (_engineManager.GetService<EntityManager>() as IEntityManager).Create<DrawableEntity>("Gerald");

            // DECLARE & INSTANTIATE an IEntity as a new DrawableEntity(), name it 'geoff':
            IEntity geoff = (_engineManager.GetService<EntityManager>() as IEntityManager).Create<DrawableEntity>("Geoff");

            // DECLARE & INSTANTIATE an IEntity as a new DrawableEntity(), name it 'textBox':
            IEntity textBox = (_engineManager.GetService<EntityManager>() as IEntityManager).Create<DrawableEntity>("TextBox");


            #region ITERATION 0

            // ADD blank string for Iteration 0:
            quoteDict.Add(0, "");

            // ADD Gerald_Dialogue_Thinking" at address '0' in geraldTexDict:
            geraldTexDict.Add(0, Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Thinking"));

            #endregion


            #region ITERATION 1

            // ADD Gerald quote for Iteration 1:
            quoteDict.Add(1, "GERALD: Just the bone saw ta fetch now, right?");

            #endregion


            #region ITERATION 2

            // ADD Geoff quote string for Iteration 2:
            quoteDict.Add(2, "GEOFF: Yup, did ya know surgeons could adjust the tension of the blade for different amputations?");

            // ADD Geoff_Dialogue_Evil" at address '2' in geoffTexDict:
            geoffTexDict.Add(2, Content.Load<Texture2D>("RIRR/VNSprites/Geoff_Dialogue_Evil"));

            #endregion


            #region ITERATION 3

            // ADD Gerald quote string for Iteration 3:
            quoteDict.Add(3, "GERALD: Not really bothered with ya lecture, I need to rescue ma job!");

            // ADD Gerald_Dialogue_Angry" at address '3' in geraldTexDict:
            geraldTexDict.Add(3, Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Angry"));

            #endregion


            #region ITERATION 4

            // ADD GEOFF quote string for Iteration 4:
            quoteDict.Add(4, "GEOFF: Well speed up then! Get on with it before the mornin' guard shift.");

            // ADD Geoff_Dialogue_Evil" at address '4' in geoffTexDict:
            geoffTexDict.Add(4, Content.Load<Texture2D>("RIRR/VNSprites/Geoff_Dialogue_Evil"));

            #endregion


            #region ITERATION 5

            // ADD Gerald quote string for Iteration 5:
            quoteDict.Add(5, "GERALD: Even more of this lot look weird now!");

            // ADD Gerald_Dialogue_Thinking" at address '5' in geraldTexDict:
            geraldTexDict.Add(5, Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Thinking"));

            #endregion


            #region ITERATION 6

            // ADD GEOFF quote string for Iteration 6:
            quoteDict.Add(6, "GEOFF: What?");

            // ADD Geoff_Dialogue_Shocked" at address '6' in geoffTexDict:
            geoffTexDict.Add(6, Content.Load<Texture2D>("RIRR/VNSprites/Geoff_Dialogue_Shocked"));

            #endregion


            #region ITERATION 7

            // ADD Geoff quote string for Iteration 7:
            quoteDict.Add(7, "GEOFF: That damned portal has ruined everything!");

            // ADD Geoff_Dialogue_Angry" at address '7' in geoffTexDict:
            geoffTexDict.Add(7, Content.Load<Texture2D>("RIRR/VNSprites/Geoff_Dialogue_Angry"));

            #endregion


            #region ITERATION 8

            // ADD Gerald quote string for Iteration 8:
            quoteDict.Add(8, "GERALD: Ya what? ");

            // ADD Gerald_Dialogue_Shocked" at address '8' in geraldTexDict:
            geraldTexDict.Add(8, Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Shocked"));

            #endregion


            #region ITERATION 9

            // ADD Geoff quote string for Iteration 9:
            quoteDict.Add(9, "GEOFF: *stutters* Uh ah uh nothin' just get that saw before we lose our jobs alright!");

            // ADD Geoff_Dialogue_Shocked" at address '9' in geoffTexDict:
            geoffTexDict.Add(9, Content.Load<Texture2D>("RIRR/VNSprites/Geoff_Dialogue_Shocked"));

            #endregion


            #endregion


            #region DICTIONARY INITIALISATION

            // INITIALISE the current scene with "Gerald" and a reference to geraldTexDict:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string, IDictionary<int, Texture2D>>).Initialise("Gerald", geraldTexDict);

            // INITIALISE the current scene with "Geoff" and a reference to geoffTexDict:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string, IDictionary<int, Texture2D>>).Initialise("Geoff", geoffTexDict);

            // INITIALISE the current scene with a reference to quoteDict:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<IDictionary<int, string>>).Initialise(quoteDict);

            #endregion


            #region SPAWNING

            /// GERALD

            // INITIALISE gerald's Texture Property with "Gerald_Dialogue_Shocked":
            (gerald as ITexture).Texture = Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Shocked");

            // SPAWN gerald in "VN3" at (40, 0):
            sceneManager.Spawn("VN3", gerald, new Vector2(40, 0));

            /// GEOFF

            // INITIALISE geoff's Texture Property with "Blank":
            (geoff as ITexture).Texture = Content.Load<Texture2D>("RIRR/VNSprites/Blank");

            // SPAWN geoff in "VN3" at (840, 0):
            sceneManager.Spawn("VN3", geoff, new Vector2(840, 0));

            /// TEXT BOX

            // INITIALISE textBox's Texture Property with "Text_Bubble":
            (textBox as ITexture).Texture = Content.Load<Texture2D>("RIRR/VNSprites/Text_Bubble");

            // SPAWN textBox in "VN3" at (-40, 500):
            sceneManager.Spawn("VN3", textBox, new Vector2(-40, 500));


            #endregion


            #region AUDIO

            // CALL PlayAudio on songMgr to play "VNTrack":
            (_engineManager.GetService<SongManager>() as IPlayAudio).PlayAudio("VNTrack");

            #endregion

            #endregion
        }

        #endregion


        #region EPILOGUE
        /// <summary>
        /// Creates every dependency for the scene as well as all references for entities in VN Scene Epilogue
        /// </summary>
        private void CreateVNEpilogue()
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


            #region VN SCENE EPILOGUE CREATION

            /// SCENE

            // SET _bgColour to Grey:
            _bgColour = Color.Gray;

            // DECLARE & INSTANTIATE an ICommand as a new CommandZeroParam(), name it 'loadVNEpilogue':
            ICommand loadVNEpilogue = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandZeroParam>();

            // INITIALISE CreateVNEpilogue with reference to this method:
            (loadVNEpilogue as ICommandZeroParam).MethodRef = CreateVNEpilogue;

            // DECLARE & INSTANTIATE an ICommand as a new CommandZeroParam(), name it 'loadThankYou':
            ICommand loadTYScene = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandZeroParam>();

            // INITIALISE loadThankYou with reference to CreateThankYouScene:
            (loadTYScene as ICommandZeroParam).MethodRef = CreateThankYouScene;

            // CALL CreateScene() on sceneManager, passing "VN4", a new Dictionary<string, ICommand>, and loadVNEpilogue as parameters:
            sceneManager.CreateCutscene("VN4", (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ICommand>>() as IDictionary<string, ICommand>, loadVNEpilogue);

            // CALL UploadNextScene() on sceneManager, passing "TYScene", and loadTYScene as parameters:
            sceneManager.UploadNextScene("TYScene", loadTYScene);

            // INITIALISE sceneManager with a new Dictionary<string, IEntity>() and a reference to createCommand for scene "VN4":
            sceneManager.Initialise("VN4", (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, IEntity>>() as IDictionary<string, IEntity>, createCommand);

            /// SCENE GRAPH

            #region COMMANDS

            // DECLARE & INSTANTIATE an ICommand as a new CommandOneParam<IKeyboardListener>(), 'unsubscribeKBCommand':
            ICommand unsubscribeKBCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandOneParam<string>>();

            // INITIALISE unsubscribeKBCommand with a reference to KeyboardManager.Unsubscribe:
            (unsubscribeKBCommand as ICommandOneParam<string>).MethodRef = (_engineManager.GetService<KeyboardManager>() as IKeyboardPublisher).Unsubscribe;

            // DECLARE & INSTANTIATE an ICommand as a new CommandTwoParam<string, string>(), 'nextSceneCommand':
            ICommand nextSceneCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandTwoParam<string, string>>();

            // INITIALISE nextSceneCommand with a reference to SceneManager.LoadNextScene():
            (nextSceneCommand as ICommandTwoParam<string, string>).MethodRef = sceneManager.LoadNextScene;

            // DECLARE & INSTANTIATE an ICommand as a new CommandZeroParam(), name it 'stopAudioCommand':
            ICommand stopAudioCommand = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandZeroParam>();

            // INITIALISE stopAudioCommand with a reference to SongManager.StopAudio():
            (stopAudioCommand as ICommandZeroParam).MethodRef = (_engineManager.GetService<SongManager>() as ISongManager).StopAudio;

            #endregion


            #region INITIALISATIONS

            // SUBSCRIBE current scene to the KeyboardManager:
            (_engineManager.GetService<KeyboardManager>() as IKeyboardPublisher).Subscribe(sceneManager.ReturnCurrentScene() as IKeyboardListener);

            // INITIALISE the current scene with a new Dictionary<string, IDictionary<int, Texture2D>>:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<IDictionary<string, IDictionary<int, Texture2D>>>).Initialise((_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, IDictionary<int, Texture2D>>>() as IDictionary<string, IDictionary<int, Texture2D>>);

            // INITIALISE the current scene with "UnsubscribeKB" and unsubscribeKBCommand as parameters:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string, ICommand>).Initialise("UnsubscribeKB", unsubscribeKBCommand);

            // INITIALISE the current scene with "NextScene" and nextSceneCommand as parameters:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string, ICommand>).Initialise("NextScene", nextSceneCommand);

            // INITIALISE the current scene with "StopAudio" and stopAudioCommand as parameters:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string, ICommand>).Initialise("StopAudio", stopAudioCommand);

            // INITIALISE the current scene with "TYScene" as a parameter:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string>).Initialise("TYScene");

            // INITIALISE the current scene with "ComicSansMS" for text font:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<SpriteFont>).Initialise(Content.Load<SpriteFont>("RIRR/Fonts/ComicSansMS"));

            // INITIALISE the current scene with a new Vector2() for text positioning:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<Vector2>).Initialise(new Vector2(100, 600));

            #endregion


            #region ITERATION SETUP

            // DECLARE & INSTANTIATE an IDictionary<int, string> as a new Dictionary<int, string>(), name it 'quoteDict':
            IDictionary<int, string> quoteDict = (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<int, string>>() as IDictionary<int, string>;

            // DECLARE & INSTANTIATE an IDictionary<int, Texture2D> as a new Dictionary<int, Texture2D>(), name it 'geraldTexDict':
            IDictionary<int, Texture2D> geraldTexDict = (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<int, Texture2D>>() as IDictionary<int, Texture2D>;
            
            // DECLARE & INSTANTIATE an IDictionary<int, string> as a new Dictionary<int, Texture2D>(), name it 'portalTexDict':
            IDictionary<int, Texture2D> portalTexDict = (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<int, Texture2D>>() as IDictionary<int, Texture2D>;

            // DECLARE & INSTANTIATE an IDictionary<int, string> as a new Dictionary<int, Texture2D>(), name it 'geoffTexDict':
            IDictionary<int, Texture2D> geoffTexDict = (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<int, Texture2D>>() as IDictionary<int, Texture2D>;

            

            // DECLARE & INSTANTIATE an IEntity as a new DrawableEntity(), name it 'gerald':
            IEntity gerald = (_engineManager.GetService<EntityManager>() as IEntityManager).Create<DrawableEntity>("Gerald");

            // DECLARE & INSTANTIATE an IEntity as a new DrawableEntity(), name it 'portal':
            IEntity portal = (_engineManager.GetService<EntityManager>() as IEntityManager).Create<DrawableEntity>("Portal");

            // DECLARE & INSTANTIATE an IEntity as a new DrawableEntity(), name it 'geoff':
            IEntity geoff = (_engineManager.GetService<EntityManager>() as IEntityManager).Create<DrawableEntity>("Geoff");
            
            // DECLARE & INSTANTIATE an IEntity as a new DrawableEntity(), name it 'textBox':
            IEntity textBox = (_engineManager.GetService<EntityManager>() as IEntityManager).Create<DrawableEntity>("TextBox");


            #region ITERATION 0

            // ADD blank string for Iteration 0:
            quoteDict.Add(0, "");

            // ADD Gerald_Dialogue_Normal" at address '0' in geraldTexDict:
            geraldTexDict.Add(0, Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Normal"));

            #endregion


            #region ITERATION 1

            // ADD Gerald quote for Iteration 1:
            quoteDict.Add(1, "GERALD: Why're you on the roof? An' whats 'at red ting? ");

            #endregion


            #region ITERATION 2

            // ADD Geoff quote string for Iteration 2:
            quoteDict.Add(2, "GEOFF: It was me, Geoff! I did it, it didn't exactly go to plan... But I did it!");

            // ADD Geoff_Dialogue_Evil" at address '2' in geoffTexDict:
            geoffTexDict.Add(2, Content.Load<Texture2D>("RIRR/VNSprites/Geoff_Dialogue_Evil"));

            #endregion


            #region ITERATION 3

            // ADD blank quote string for Iteration 3:
            quoteDict.Add(3, "");

            // ADD Geoff_Dialogue_Evil" at address '3' in portalTexDict:
            portalTexDict.Add(3, Content.Load<Texture2D>("RIRR/VNSprites/Hell_Portal_Stones"));

            #endregion


            #region ITERATION 4

            // ADD Geoff quote string for Iteration 4:
            quoteDict.Add(4, "GEOFF: *monologues* ");

            // ADD Geoff_Dialogue_Evil" at address '4' in geoffTexDict:
            geoffTexDict.Add(4, Content.Load<Texture2D>("RIRR/VNSprites/Geoff_Dialogue_Evil"));

            #endregion


            #region ITERATION 5

            // ADD Geoff quote string for Iteration 5:
            quoteDict.Add(5, "GEOFF: *Continues to monologues* ");

            // ADD Geoff_Dialogue_Evil" at address '5' in geoffTexDict:
            geoffTexDict.Add(5, Content.Load<Texture2D>("RIRR/VNSprites/Geoff_Dialogue_Evil"));

            #endregion


            #region ITERATION 6

            // ADD Geoff quote string for Iteration 6:
            quoteDict.Add(6, "GEOFF: It was me, Geoff! I did it, it didnt exactly go to plan... But I did it!");

            // ADD Geoff_Dialogue_Evil" at address '6' in geoffTexDict:
            geoffTexDict.Add(6, Content.Load<Texture2D>("RIRR/VNSprites/Geoff_Dialogue_Evil"));

            #endregion


            #region ITERATION 7

            // ADD Gerald quote string for Iteration 7:
            quoteDict.Add(7, "GERALD: ...");

            // ADD Gerald_Dialogue_Thinking" at address '7' in geraldTexDict:
            geraldTexDict.Add(7, Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Thinking"));

            #endregion


            #region ITERATION 8

            // ADD Gerald quote string for Iteration 8:
            quoteDict.Add(8, "GERALD: Oh would you sod Off!?");

            // ADD Gerald_Dialogue_Angry" at address '8' in geraldTexDict:
            geraldTexDict.Add(8, Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Angry"));

            #endregion


            #region ITERATION 9

            // ADD Gerald quote string for Iteration 9:
            quoteDict.Add(9, "GERALD: *hits Geoff in face with whisky bottle*");

            // ADD Gerald_Dialogue_Angry" at address '9' in geraldTexDict:
            geraldTexDict.Add(9, Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Angry"));

            #endregion


            #region ITERATION 10

            // ADD Geoff quote string for Iteration 10:
            quoteDict.Add(10, "GEOFF: AGGHHHHHHHH!!!");

            // ADD Geoff_Dialogue_Shocked" at address '10' in geoffTexDict:
            geoffTexDict.Add(10, Content.Load<Texture2D>("RIRR/VNSprites/Geoff_Dialogue_Shocked"));

            #endregion


            #region ITERATION 11

            // ADD Geoff quote string for Iteration 11:
            quoteDict.Add(11, "GEOFF: *Geoff falls into hell portal*");

            // ADD Geoff_Dialogue_Shocked" at address '11' in geoffTexDict:
            geoffTexDict.Add(11, Content.Load<Texture2D>("RIRR/VNSprites/Geoff_Dialogue_Shocked"));

            #endregion


            #region ITERATION 12

            // ADD Blank quote string for Iteration 12:
            quoteDict.Add(12, "");

            // ADD Blank" at address '12' in geoffTexDict:
            geoffTexDict.Add(12, Content.Load<Texture2D>("RIRR/VNSprites/Blank"));

            #endregion


            #region ITERATION 13

            // ADD Gerald quote string for Iteration 13:
            quoteDict.Add(13, "GERALD: What a moron... Time to go to sleep, got work in t' morning");

            // ADD Gerald_Dialogue_Normal" at address '12' in geraldTexDict:
            geraldTexDict.Add(13, Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Normal"));

            #endregion

            #endregion


            #region DICTIONARY INITIALISATION

            // INITIALISE the current scene with "Gerald" and a reference to geraldTexDict:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string, IDictionary<int, Texture2D>>).Initialise("Gerald", geraldTexDict);

            // INITIALISE the current scene with "Portal" and a reference to geoffTexDict:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string, IDictionary<int, Texture2D>>).Initialise("Portal", portalTexDict);

            // INITIALISE the current scene with "Geoff" and a reference to geoffTexDict:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<string, IDictionary<int, Texture2D>>).Initialise("Geoff", geoffTexDict);
            
            // INITIALISE the current scene with a reference to quoteDict:
            (sceneManager.ReturnCurrentScene() as IInitialiseParam<IDictionary<int, string>>).Initialise(quoteDict);

            #endregion


            #region SPAWNING

            /// GERALD

            // INITIALISE gerald's Texture Property with "Gerald_Dialogue_Shocked":
            (gerald as ITexture).Texture = Content.Load<Texture2D>("RIRR/VNSprites/Gerald_Dialogue_Shocked");

            // SPAWN gerald in "VN4" at (40, 0):
            sceneManager.Spawn("VN4", gerald, new Vector2(40, 0));

            /// PORTAL

            // INITIALISE portal's Texture Property with "Blank":
            (portal as ITexture).Texture = Content.Load<Texture2D>("RIRR/VNSprites/Blank");

            // SPAWN geoff in "VN4" at (840, 0):
            sceneManager.Spawn("VN4", portal, new Vector2(870, 0));

            /// GEOFF

            // INITIALISE geoff's Texture Property with "Blank":
            (geoff as ITexture).Texture = Content.Load<Texture2D>("RIRR/VNSprites/Blank");

            // SPAWN geoff in "VN4" at (840, 0):
            sceneManager.Spawn("VN4", geoff, new Vector2(840, 0));

            /// TEXT BOX

            // INITIALISE textBox's Texture Property with "Text_Bubble":
            (textBox as ITexture).Texture = Content.Load<Texture2D>("RIRR/VNSprites/Text_Bubble");

            // SPAWN textBox in "VN4" at (-40, 500):
            sceneManager.Spawn("VN4", textBox, new Vector2(-40, 500));


            #endregion


            #region AUDIO

            // CALL PlayAudio on songMgr to play "VNTrack":
            (_engineManager.GetService<SongManager>() as IPlayAudio).PlayAudio("VNTrack");

            #endregion

            #endregion
        }

        #endregion


        #region THANK YOU SCREEN

        /// <summary>
        /// Creates a simple scene with a thank you message
        /// </summary>
        private void CreateThankYouScene()
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

            // INITIALISE playSFXCommand's MethodRef Property with reference to SFXManager.PlayAudio():
            (playSFXCommand as ICommandOneParam<string>).MethodRef = (_engineManager.GetService<SFXManager>() as IPlayAudio).PlayAudio;

            #endregion


            #region THANK YOU SCENE CREATION

            /// SCENE

            // DECLARE & INSTANTIATE an ICommand as a new CommandZeroParam(), name it 'loadTYScene':
            ICommand loadTYScene = (_engineManager.GetService<Factory<ICommand>>() as IFactory<ICommand>).Create<CommandZeroParam>();

            // INITIALISE loadTYScene with reference to CreateThankYouScene:
            (loadTYScene as ICommandZeroParam).MethodRef = CreateThankYouScene;

            // CALL CreateScene() on sceneManager, passing "TYScene", a new Dictionary<string, ICommand>, and loadTYScene as parameters:
            sceneManager.CreateScene("TYScene", (_engineManager.GetService<Factory<IEnumerable>>() as IFactory<IEnumerable>).Create<Dictionary<string, ICommand>>() as IDictionary<string, ICommand>, loadTYScene);

            // INITIALISE sceneManager with a CollisionManager instance from _engineManager, a new Dictionary<string, IEntity>() and a reference to createCommand for scene "TYScene":
            sceneManager.Initialise("TYScene", _engineManager.GetService<CollisionManager>() as ICollisionManager,
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

            #endregion


            #region THANK YOU IMAGE CREATION AND SPAWNING

            // DECLARE & INSTANTIATE an IEntity as a new DrawableEntity(), name it 'tempEntity':
            IEntity tempEntity = (_engineManager.GetService<EntityManager>() as IEntityManager).Create<DrawableEntity>("TYScreen");

            // LOAD "TYScreen" texture to "TYScreen":
            (tempEntity as ITexture).Texture = Content.Load<Texture2D>("RIRR/GUI/TYScreen");

            // SPAWN "TYScreen" in "TYScene" at position created from Tiled:
            (_engineManager.GetService<SceneManager>() as ISceneManager).Spawn("TYScene", tempEntity, Vector2.Zero);

            #endregion

            #region AUDIO

            // CALL PlayAudio() on SongManager to play "VNTrack":
            (_engineManager.GetService<SongManager>() as IPlayAudio).PlayAudio("VNTrack");

            #endregion
        }

        #endregion
    }
}
