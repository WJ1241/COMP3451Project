using System;
using OrbitalEngine.Services.Commands.Interfaces;
using OrbitalEngine.Services.Interfaces;
using COMP3451Project.RIRRPackage.Interfaces;

namespace COMP3451Project.RIRRPackage
{
    /// <summary>
    /// Class which manages scores of Players in Pong and outputs scores to screen
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 20/02/22
    /// </summary>
    public class PongReferee : IPongReferee, IService
    {
        #region FIELD VARIABLES

        // DECLARE an ICommand, name it '_respawnBall':
        private ICommand _respawnBall;

        // DECLARE an int, name it '_p1Score':
        private int _p1Score;

        // DECLARE an int, name it '_p2Score':
        private int _p2Score;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of PongReferee
        /// </summary>
        public PongReferee()
        {
            // EMPTY CONSTRUCTOR
        }

        #endregion


        #region IMPLEMENTATION OF IPONGREFEREE

        /// <summary>
        /// Checks to see who scores a point
        /// </summary>
        /// <param name="pPlayerNum"> Player Number </param>
        public void CheckWhoScored(int pPlayerNum)
        {
            // IF Player 1:
            if (pPlayerNum == 1)
            {
                // INCREMENT _p1Score by '1':
                _p1Score++;

                // SET Console Text Colour to Red:
                Console.ForegroundColor = ConsoleColor.Red;

                // IF Player 1 score IS currently 1:
                if (_p1Score == 1)
                {
                    // PRINT to console that Player 1 has scored:
                    Console.WriteLine("Player 1 has just scored! They now have " + _p1Score + " point!");
                }
                // IF Player 1 score IS NOT currently 1:
                else
                {
                    // PRINT to console that Player 1 has scored:
                    Console.WriteLine("Player 1 has just scored! They now have " + _p1Score + " points!");
                }
            }
            // IF Player 2:
            else if (pPlayerNum == 2)
            {
                // INCREMENT _p2Score by '1':
                _p2Score++;

                // SET Console Text Colour to Blue:
                Console.ForegroundColor = ConsoleColor.Blue;

                // IF Player 2 score IS currently 1:
                if (_p2Score == 1)
                {
                    // PRINT to console that Player 2 has scored:
                    Console.WriteLine("Player 2 has just scored! They now have " + _p2Score + " point!");
                }
                // IF Player 2 score IS NOT currently 1:
                else
                {
                    // PRINT to console that Player 2 has scored:
                    Console.WriteLine("Player 2 has just scored! They now have " + _p2Score + " points!");
                }
            }

            // SET Console Text Colour to White:
            Console.ForegroundColor = ConsoleColor.White;

            // CALL _respawnBall():
            // ATTEMPT WAS MADE TO SCHEDULE THIS,
            // BUT FOUND ERROR WHEN SCHEDULING A METHOD TO BE SCHEDULED, WHICH MODIFIES COMMANDLIST WHEN ALREADY ITERATING
            _respawnBall.ExecuteMethod();
        }

        /// <summary>
        /// Property which allows only write access to a RespawnBall ICommand
        /// </summary>
        public ICommand RespawnBall
        {
            set
            {
                // SET value of _respawnBall to incoming value:
                _respawnBall = value;
            }
        }

        #endregion
    }
}
