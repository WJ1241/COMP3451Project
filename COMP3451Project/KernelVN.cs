﻿using COMP3451Project.RIRRPackage.Behaviours;
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



        #endregion


        #region PRE LEVEL 2 VN



        #endregion


        #region PRE LEVEL 3 VN



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

            // INITIALISE loadTYScene with reference to CreateLevelThree:
            (loadTYScene as ICommandZeroParam).MethodRef = CreateLevelThree;

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
