// Author: Eduard Varshavsky
// File Name: MainGame.cs
// Project Name: ElevensEd
// Creation Date: September 10, 2017
// Modified Date: September 17, 2017
// Description: This program is built to play the card game elevens as a review project
// code

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ElevensEd
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        //Initializes the graphics engine and spritebatches
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Stores the current and previous keyboard states
        KeyboardState kb;
        KeyboardState prevKb;

        //Store both the current and previous mouse states for user input
        MouseState mouse;
        MouseState prevMouse;

        //Stores sound for songs and effects
        Song btmNormal;
        Song btmRain;
        Song victorySong;
        Song loseSong;

        //Stores sound effects .wav
        SoundEffect swipeSound;
        SoundEffect clickSound;
        SoundEffect incorrectSound;
        SoundEffect loseSound;
        SoundEffect resetSound;
        SoundEffect switchSound;

        //Made array to store sound samples off 'Joker' character
        SoundEffect[] jokerSound = new SoundEffect[9];

        //Stores bools that detect if  a certain song has played
        bool didMainMenuSoundPlay;
        bool didGameplaySoundPlay;
        bool didLoseSoundPlay;
        bool didWinSoundPlay;

        //The random object allowing for random number generation during Shuffle
        Random rng = new Random();

        //Stores constants for gamestates that is used to determine current gamestate
        const int PREGAME = 0;
        const int GAMEPLAY = 1;
        const int WIN = 2;
        const int LOSE = 3;

        //Stores bools and integers for the win/loss timer delays (so player can see board)
        bool loseTimerStart;
        bool winTimerStart;
        int loseGameTimer;
        int winGameTimer;

        //Stores bool if a newgame has been started(for reseting cards and slots)
        bool newGame;

        //Stores the curretn gamestate
        int gameState = PREGAME;

        //Created a list to store cards in the deck and total amount of cards
        List<Card> deck = new List<Card>();
        string[] suitNames = new string[] {"Diamonds", "Hearts", "Clubs", "Spades"};

        //Constants which hold vital information related to deck dimensions
        const int CARDS_IN_DECK= 52;
        const int NUM_SUITS = 4;
        const int NUM_VALUES= 13;

        //Stores the width and height of each card in the deck
        int cardWidth;
        int cardHeight;

        //Created values to hold spacing values
        int spacingOffset = 30;

        //Stores texture data for images on screen
        Texture2D backgroundImg;
        Texture2D mainBackImg;
        Texture2D cursorImg;
        Texture2D logoImg;

        //Stores image data related to cards
        Texture2D cardDownImg;
        Texture2D allCardsImg;

        //Stores image data for buttons and details in the game
        Texture2D startButtonImg;
        Texture2D exitButtonImg;
        Texture2D mainMenuButtonImg;
        Texture2D bannerImg;
        Texture2D youWinImg;
        Texture2D youLoseImg;

        //Stores position and dimension data for background detail
        Rectangle backgroundLoc;
        Rectangle mainBackLoc;
        Rectangle cursorRec;
        Rectangle bannerLoc;
        Rectangle youWinLoc;
        Rectangle youLoseLoc;

        //Stores locations and dimensions for buttons
        Rectangle startButtonLoc;
        Rectangle exitButtonLoc;
        Rectangle mainMenuButtonLoc;
        Rectangle logoRec;

        //Stores color data for the cursor on screen
        Color currentCursorCol = Color.White;
        Color defaultCursorCol = Color.White;
        Color clickedCursorCol = Color.Red;

        //Stores position and dimension data for 
        Rectangle deckLoc;

        //Stores data relevent for location and values of cards in play as well as its color and selection
        Card[,] playingCards = new Card[2, 6];
        TableLocation[,] slots = new TableLocation[2, 6];
        Color[,] cardColor = new Color[2, 6];
        bool[,] isCardSelected = new bool[2, 6];

        //Stores the value of cards selected and each value for addition
        int numOfCardsSelected;
        int[] cardValues = new int[2];

        //Holds values and positions for cards being switched on the table
        TableLocation[,] selectedCardsLoc = new TableLocation[2, 6];
        int valuesSum;
        int[,] cardsBeingSwitched = new int[2, 2];

        //Stores the amount of frames passed (for delays)
        int waitFrames;

        //Stores colors for the card stacks
        Color defaultColor = Color.White;
        Color selectedColor = Color.Pink;
        Color wrongColor = Color.Red;

        //Bool that stores true/false for when cards have been taken from the deck
        bool takenFormDeck = false;

        //Store the dimensions of the screen, modify these if game resolution is changed
        int screenWidth = 1024;
        int screenHeight = 576;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //Set mouse visibility to be disabled and applied graphics changes on initilization
            this.graphics.PreferredBackBufferWidth = screenWidth;
            this.graphics.PreferredBackBufferHeight = screenHeight;
            IsMouseVisible = false;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            //Loading sound content for music and effects
            btmNormal = Content.Load<Song>("sound/music/Beneath the Mask (Instrumental)");
            btmRain = Content.Load<Song>("sound/music/Beneath the Mask Rain (Instrumental)");
            victorySong = Content.Load<Song>("sound/music/Victory");
            loseSong = Content.Load<Song>("sound/music/Lose");

            //Creates sound instance to loop background music
            MediaPlayer.IsRepeating = true;

            //Loads the sound effects for miscellaneous sounds 
            swipeSound = Content.Load<SoundEffect>("sound/soundeffects/Swipe");
            clickSound = Content.Load<SoundEffect>("sound/soundeffects/HitSound");
            incorrectSound = Content.Load<SoundEffect>("sound/soundeffects/IncorrectSound");
            loseSound = Content.Load<SoundEffect>("sound/soundeffects/LostSound");
            resetSound = Content.Load<SoundEffect>("sound/soundeffects/Reset");
            switchSound = Content.Load<SoundEffect>("sound/soundeffects/LetsSwitchSound");

            //Loads the variety of sound effects to be used when cancelling a pair
            jokerSound[0] = Content.Load<SoundEffect>("sound/soundeffects/cancelsounds/DontRushSound");
            jokerSound[1] = Content.Load<SoundEffect>("sound/soundeffects/cancelsounds/HmmSound");
            jokerSound[2] = Content.Load<SoundEffect>("sound/soundeffects/cancelsounds/HoldOnSound");
            jokerSound[3] = Content.Load<SoundEffect>("sound/soundeffects/cancelsounds/MaybeNotSound");
            jokerSound[4] = Content.Load<SoundEffect>("sound/soundeffects/cancelsounds/NeverMindSound");
            jokerSound[5] = Content.Load<SoundEffect>("sound/soundeffects/cancelsounds/NoSound");
            jokerSound[6] = Content.Load<SoundEffect>("sound/soundeffects/cancelsounds/NotThisSound");
            jokerSound[7] = Content.Load<SoundEffect>("sound/soundeffects/cancelsounds/SomeOtherSound");
            jokerSound[8] = Content.Load<SoundEffect>("sound/soundeffects/cancelsounds/SomethingElseSound");

            //Loading texture content for background and essential detail
            backgroundImg = Content.Load<Texture2D>("backgrounds/tableBackground");
            mainBackImg = Content.Load<Texture2D>("backgrounds/mainBackground");
            cursorImg = Content.Load<Texture2D>("backgrounds/new-cursor");
            logoImg = Content.Load<Texture2D>("images/elevens-logo");

            //Loading location and dimensions of images
            backgroundLoc = new Rectangle(0, 0, screenWidth, screenHeight);
            mainBackLoc = new Rectangle(0, 0, screenWidth, screenHeight);
            cursorRec = new Rectangle(0, 0, 32, 32);
            logoRec = new Rectangle(0, 0, logoImg.Width, logoImg.Height);

            //Loads images for buttons and messages
            startButtonImg = Content.Load<Texture2D>("images/StartButton");
            exitButtonImg = Content.Load<Texture2D>("images/exit-button-hi");
            mainMenuButtonImg = Content.Load<Texture2D>("images/main-menu-hi");
            bannerImg = Content.Load<Texture2D>("images/Banner-red-new");
            youWinImg = Content.Load<Texture2D>("images/you_win");
            youLoseImg = Content.Load<Texture2D>("images/you-lose-banner-sm-@x2");

            //Creates location and dimensions for buttons and messages
            startButtonLoc = new Rectangle(0, 0, startButtonImg.Width / 2, startButtonImg.Height / 2);
            exitButtonLoc = new Rectangle(0, 0, exitButtonImg.Width / 2, exitButtonImg.Height / 2);
            mainMenuButtonLoc = new Rectangle(0, 0, mainMenuButtonImg.Width / 2, mainMenuButtonImg.Height / 2);
            bannerLoc = new Rectangle(0, screenHeight / 2 - 75, screenWidth, 250);
            youWinLoc = new Rectangle((screenWidth / 2) - (youWinImg.Width / 2), 30, youWinImg.Width, youWinImg.Height);
            youLoseLoc = new Rectangle((screenWidth / 2) - (youLoseImg.Width / 2), -121, youLoseImg.Width, youLoseImg.Height);

            //Loads image content related to cards
            cardDownImg = Content.Load<Texture2D>("images/Eduard Varshavsky - CardBack");
            allCardsImg = Content.Load<Texture2D>("images/Eduard Varshavsky - CardFaces");

            //Creates values for each card width and card height
            cardWidth = allCardsImg.Width / NUM_VALUES;
            cardHeight = allCardsImg.Height / NUM_SUITS;

            //Set the location to display the deck
            deckLoc = new Rectangle(32, 32, allCardsImg.Width / 13, allCardsImg.Height / 4);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            //Detects the state of the mouse and buttons pressed
            mouse = Mouse.GetState();
            kb = Keyboard.GetState();

            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            //Determines position of the mouse and assigns the cursor to follow itas its new pointer
            var mousePos = new Point(mouse.X, mouse.Y);
            cursorRec.X = mousePos.X - 6;
            cursorRec.Y = mousePos.Y - 1;

            //Switch statement that tracks which gamestate is currently in use
            switch (gameState)
            {
                case PREGAME:
                    //Checks if main menu music has played initially 
                    if (didMainMenuSoundPlay == false)
                    {
                        //Assigns new positions to buttons based
                        startButtonLoc.X = 140;
                        startButtonLoc.Y = 330;
                        exitButtonLoc.X = 140;
                        exitButtonLoc.Y = 450;

                        //Stops the current song and plays the main menu song, changing the bool to true
                        MediaPlayer.Stop();
                        MediaPlayer.Play(btmNormal);
                        didMainMenuSoundPlay = true;
                    }

                    //Checks for a button press on the start button
                    if (startButtonLoc.Contains(mousePos) && (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed))
                    {
                        //Plays a sound effect to confirm click
                        switchSound.Play();

                        //Checks if this is a new game after a finished game
                        if (newGame == true)
                        {
                            //If there was already a completed game, some already made entities are reset
                            Reinitialize();
                        }
                        else
                        {
                            //Sets newgame to true if this is the first game of the session
                            newGame = true;
                        }

                        //Uses methods to set up play space and deck. Once complete, proceeds to gameplay
                        CreateDeck();
                        ShuffleDeck(1000);
                        PlayingCardLocations();
                        gameState = GAMEPLAY;
                    }
                    else if (exitButtonLoc.Contains(mousePos) && (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed))
                    {
                        //Exits the game if the player chooses the Exit option
                        this.Exit();
                    }
                    break;

                case GAMEPLAY:

                    //Checks if gameplay music has begun
                    if (didGameplaySoundPlay == false)
                    {
                        //Stops the current song and plays the correct music, setting the bool true to not repeat
                        MediaPlayer.Stop();
                        MediaPlayer.Play(btmRain);
                        didGameplaySoundPlay = true;
                    }

                    //Resets color of the curso to its default color
                    currentCursorCol = defaultCursorCol;
            
                    //Checks if 12 cards have been taken from deck and put on play area
                    if (takenFormDeck == true)
                    {
                        //Nested for loop based off the amount of rows and columns in playing area (2 x 6)
                        for (int i = 0; i < slots.GetLength(0); i++)
                        {
                            for (int j = 0; j < slots.GetLength(1); j++)
                            {
                                //Checks which card is currently not selected
                                if (isCardSelected[i, j] == false)
                                {
                                    //Non - Selected cards are set back to their default color
                                    cardColor[i, j] = defaultColor;
                                }

                                //Checks which card stack got clicked on screen without any other functions taking priority
                                if (slots[i, j].GetPosition().Contains(mousePos) && (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed) && numOfCardsSelected < 2
                                    && CheckWinCondition() == false)
                                {
                                    //Changes the color of the cursor to show that it was clicked
                                    currentCursorCol = clickedCursorCol;

                                    //Checks if the clicked card is higher than 10 (face card)
                                    if (slots[i, j].GetSlotValue() > 10)
                                    {
                                        //Holds values of both cards to check them after and replaces the face card with a new card
                                        //from the top of the deck
                                        int checkerBefore = slots[i, j].GetSlotValue();
                                        Card holder = slots[i, j].ReplaceCard(deck[deck.Count - 1]);
                                        int checkerAfter = slots[i, j].GetSlotValue();

                                        //Puts the current face card to the bottom of the deck and removes the top card from it
                                        deck.Insert(0, holder);
                                        deck.RemoveAt(deck.Count - 1);

                                        //Checks if the new card is the same as the previous card
                                        if (checkerBefore == checkerAfter)
                                        {
                                            //Plays an incorrect sound if the cards are the same
                                            incorrectSound.Play();
                                        }
                                        else
                                        {
                                            //Otherwise, plays a correct sound of cards switching
                                            swipeSound.Play();
                                        }
                                    }
                                    else if (slots[i, j].GetSlotValue() <= 10)
                                    {
                                        //Highlights and selects the cards if it is not a face card, and plays a sound
                                        cardColor[i, j] = selectedColor;
                                        isCardSelected[i, j] = true;
                                        clickSound.Play();

                                        //Records the value and position in play area to later be used in replacement
                                        cardValues[numOfCardsSelected] = slots[i, j].GetSlotValue();
                                        cardsBeingSwitched[numOfCardsSelected, 0] = i;
                                        cardsBeingSwitched[numOfCardsSelected, 1] = j;

                                        //Incriments the number of cards currently selected by 1
                                        numOfCardsSelected++;
                                    }
                                }
                            }
                        }
                    }

                    //Checks if there have been 2 cards selected by clicking
                    if (numOfCardsSelected >= 2)
                    {
                        //Waits until a split second has passed on the wait timer
                        if (waitFrames >= 30)
                        {
                            //Finds the value of the two selected cards
                            valuesSum = cardValues[0] + cardValues[1];

                            //Nested for loop based off the amount of rows and columns in playing area (2 x 6)
                            for (int i = 0; i < slots.GetLength(0); i++)
                            {
                                for (int j = 0; j < slots.GetLength(1); j++)
                                {
                                    //Resets the color of the cards to their default color
                                    cardColor[i, j] = defaultColor;
                                }
                            }

                            //Checks if the sum of the selected cards adds up to 11
                            if (valuesSum == 11)
                            {
                                //Places a new card into the first selected playing area and removes it from the deck
                                slots[cardsBeingSwitched[0, 0], cardsBeingSwitched[0, 1]].PlaceCard(deck[deck.Count - 1]);
                                deck.RemoveAt(deck.Count - 1);

                                //Places a new card into the second selected playing area and removes it from the deck
                                slots[cardsBeingSwitched[1, 0], cardsBeingSwitched[1, 1]].PlaceCard(deck[deck.Count - 1]);
                                deck.RemoveAt(deck.Count - 1);

                                //Places a sound to confirm this transaction
                                swipeSound.Play();
                            }
                            else if (slots[cardsBeingSwitched[0, 0], cardsBeingSwitched[0, 1]] == slots[cardsBeingSwitched[1, 0], cardsBeingSwitched[1, 1]])
                            {
                                //If the player selected the same card twice, then plays a variety of 'cancel sounds'
                                int randomNumber = rng.Next(0, 8);
                                jokerSound[randomNumber].Play();
                            }
                            else
                            {
                                //If the values don't check out, an incorrect sound plays to signify the wrongness
                                incorrectSound.Play();
                            }

                            //Resets all variables used to have a clean slate for the next pair
                            waitFrames = 0;
                            cardValues[0] = 0;
                            cardValues[1] = 0;
                            numOfCardsSelected = 0;
                            CheckLoseCondition();
                        }

                        //Incriments wait frames each update until they reach 30
                        waitFrames++;
                    }

                    //Checks win condition for being true
                    if (CheckWinCondition() == true)
                    {
                        //Starts the timer for delay between winning
                        winTimerStart = true;
                        winGameTimer++;

                        //When the timer reaches 2 seconds in, the gamestate is updated
                        if (winGameTimer >= 120)
                        {
                            //The win game state is used
                            gameState = WIN;
                        }
                    }

                    //Checks if the lose timer has started and all cards have been taken from the deck
                    if (loseTimerStart == true && takenFormDeck == true)
                    {
                        //Incriments the lose game timer
                        loseGameTimer++;

                        //Once the timer reaches 4 seconds, the gamestate is updated
                        if (loseGameTimer >= 240)
                        {
                            //The lose game state is set
                            gameState = LOSE;
                        }
                    }

                    break;

                case WIN:
                    //Resets variables used to get to this gamestate
                    winTimerStart = false;
                    winGameTimer = 0;

                    //Checks if the victory music has played
                    if (didWinSoundPlay == false)
                    {
                        //Sets a new postion for the main menu button and exit button
                        mainMenuButtonLoc.X = 100;
                        mainMenuButtonLoc.Y = 295;
                        exitButtonLoc.X = 600;
                        exitButtonLoc.Y = 300;

                        //Stops the current song and plays the victory music, setting it to true so it doesn't repeat
                        MediaPlayer.Stop();
                        didWinSoundPlay = true;
                        MediaPlayer.Play(victorySong);
                    }

                    //Checks if the player clicked on the main menu button
                    if (mainMenuButtonLoc.Contains(mousePos) && (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed))
                    {
                        //Resets variables and plays a sound to confirm button click, sets the current state to the main menu
                        didMainMenuSoundPlay = false;
                        loseTimerStart = false;
                        switchSound.Play();
                        gameState = PREGAME;
                    }
                    else if (exitButtonLoc.Contains(mousePos) && (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed))
                    {
                        //If the player chooses the exit option, it resets the music and closes out the game with a sound effect
                        didMainMenuSoundPlay = false;
                        switchSound.Play();
                        this.Exit();
                    }

                    didGameplaySoundPlay = false;
                    break;

                case LOSE:
                    //Resets variables used to get to this gamestate
                    loseTimerStart = false;
                    loseGameTimer = 0;

                    //Checks if the loss music has played
                    if (didLoseSoundPlay == false)
                    {
                        //Sets a new postion for the main menu button and exit button
                        mainMenuButtonLoc.X = 100;
                        mainMenuButtonLoc.Y = 295;
                        exitButtonLoc.X = 600;
                        exitButtonLoc.Y = 300;

                        //Stops the current song and plays the loss music, setting it to true so it doesn't repeat
                        MediaPlayer.Stop();
                        MediaPlayer.Play(loseSong);
                        didLoseSoundPlay = true;
                    }

                    if (mainMenuButtonLoc.Contains(mousePos) && (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed))
                    {
                        //Resets variables and plays a sound to confirm button click, sets the current state to the main menu
                        didMainMenuSoundPlay = false;
                        switchSound.Play();
                        gameState = PREGAME;
                    }
                    else if (exitButtonLoc.Contains(mousePos) && (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed))
                    {
                        //If the player chooses the exit option, it resets the music and closes out the game with a sound effect
                        didMainMenuSoundPlay = false;
                        switchSound.Play();
                        this.Exit();
                    }

                    //Resets main menu music boolean
                    didGameplaySoundPlay = false;
                    break;
        }

            //Remembers previous keyboard and mouse states for the next update
            prevKb = kb;
            prevMouse = mouse;
            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            //Begins drawing of all sprites
            spriteBatch.Begin();

            //Draws the default background for almost all of gameplay
            spriteBatch.Draw(backgroundImg, backgroundLoc, Color.White);

            //Changes what is drawn on screen based on which gamestate is currently active
            switch (gameState)
            {
                case PREGAME:
                    //Draws the background, start and exit button
                    spriteBatch.Draw(mainBackImg, mainBackLoc, Color.White);
                    spriteBatch.Draw(logoImg, logoRec, Color.White);
                    spriteBatch.Draw(startButtonImg, startButtonLoc, Color.White);
                    spriteBatch.Draw(exitButtonImg, exitButtonLoc, Color.White);
                    break;

                case GAMEPLAY:
                    //Uses the method to draw all gameplay related items
                    DrawEverything();

                    //Sets taken from deck to true once everything is drawn
                    takenFormDeck = true;
                    break;

                case WIN:
                    //Uses the method to draw all gameplay related items
                    DrawEverything();

                    //Draws the win message with a banner over it with buttons to exit or go back to main menu
                    spriteBatch.Draw(youWinImg, youWinLoc, Color.White);
                    spriteBatch.Draw(bannerImg, bannerLoc, Color.White * 0.9f);
                    spriteBatch.Draw(mainMenuButtonImg, mainMenuButtonLoc, Color.White);
                    spriteBatch.Draw(exitButtonImg, exitButtonLoc, Color.White);
                    break;

                case LOSE:
                    //Uses the method to draw all gameplay related items
                    DrawEverything();

                    //Draws the loss message with a banner over it with buttons to exit or go back to main menu
                    spriteBatch.Draw(youLoseImg, youLoseLoc, Color.White);
                    spriteBatch.Draw(bannerImg, bannerLoc, Color.White * 0.9f);
                    spriteBatch.Draw(mainMenuButtonImg, mainMenuButtonLoc, Color.White);
                    spriteBatch.Draw(exitButtonImg, exitButtonLoc, Color.White);
                    break;
            }

            //Draws the cursor over everything to display mouse positon
            spriteBatch.Draw(cursorImg, cursorRec, currentCursorCol);

            //End drawing sprites
            spriteBatch.End();

            base.Draw(gameTime);
        }

        //Pre: Requires only the deck dimensions and the card class
        //Post: Adds a card to the deck with percise values
        //Desc: Creates a card for each suit and value. It also determines the image data to be used and adds
        //      it to the deck to be used later
        private void CreateDeck()
        {
            //Nested for loop based off the suits in the deck and the values in each suit
            for (int i = 0; i < NUM_SUITS; i++)
            {
                for (int j = 0; j < NUM_VALUES; j++)
                {
                    //Holds a new card as it assigns a suit, vlaue, and image data for the specific card
                    Card c = new Card();
                    c.setSuit(suitNames[i]);
                    c.SetValue(j + 1);
                    c.SetSource(new Rectangle(cardWidth * j, cardHeight * i, cardWidth, cardHeight));

                    //Adds the card to the deck
                    deck.Add(c);
                }
            }
        }

        //Pre: Requires the number of times to shuffle deck and betting + dealing variables
        //Post: Gives a shuffled deck and many reset variables
        //Desc: Shuffles the deck by switching cards inside it hundreds of times and resets
        //      variables that will be used in the coming gamestates(BETTING and DEALING_CARDS)
        private void ShuffleDeck(int numShuffles)
        {
            //Loop numShuffles times and generate 2 random numbers from 0 and deck.Length
            for (int i = 0; i < numShuffles; i++)
            {
                //Swaps the elements in deck at those elements to create a new, different set of cards. 
                //This is done many times to randomize completely
                int randomNum1 = rng.Next(0, deck.Count);
                int randomNum2 = rng.Next(0, deck.Count);
                Card holder = deck[randomNum1];
                deck[randomNum1] = deck[randomNum2];
                deck[randomNum2] = holder;
            }
        }

        //Pre: Requires the playing stacks dimensions (2 rows x 6 columns) and each stack info
        //Post: Creates a new location on the table with access to the TableLocation class
        //Desc: Creates data to later be used for each slot in the playing area and sets their
        //      position to be used in drawing
        private void PlayingCardLocations()
        {
            //Nested for loop based off the amount of rows and columns in playing area (2 x 6)
            for (int i = 0; i < slots.GetLength(0); i++)
            {
                for (int j = 0; j < slots.GetLength(1); j++)
                {
                    //Assigns a TableLocation object for each slot and assigns them a new location
                    slots[i, j] = new TableLocation();
                    slots[i, j].SetPosition(new Rectangle(160 + ((cardWidth + spacingOffset) * j), 200 + (cardHeight + spacingOffset) * i, cardWidth, cardHeight));
                }
            }
        }

        //Pre: Requires player loss bool and the values of the playing slots
        //Post: Gives a bool saying if the game has moves remaining or not
        //Desc: Searches for pairs that add to 11 or face cards that are the first of their stack,
        //      thus allowing the game to continue on. Otherwise it starts the LOSE state
        private void CheckLoseCondition()
        {
            //Creates new variables to hold values and current value of the array (a)
            int[] values = new int[12];
            int a = 0;

            //Bool that determines if any moves remain
            bool hasPair = false;

            //Nested for loop based off the amount of rows and columns in playing area (2 x 6)
            for (int i = 0; i < slots.GetLength(0); i++)
            {
                for (int j = 0; j < slots.GetLength(1); j++)
                {
                    //Incriments value array by 1 to get every single value of each card stack
                    values[a++] = slots[i, j].GetSlotValue();

                    //Checks if any slots have a face card and only 1 in the stack, avoids situations where game will lose on start
                    //or with face cards remaining
                    if (slots[i, j].GetSlotValue() > 10 && slots[i, j].GetStackCount() == 1)
                    {
                        //Decided that there is a move remaining
                        hasPair = true;
                    }
                }
            }

            //Nested for loop based on the size of the each card stack value array
            for (int i = 0; i < values.Length; i++)
            {
                for (int j = 0; j < values.Length; j++)
                {
                    //Makes sure that the card doesn't add itself when seeing if the two values add up to 11
                    if (i != j && values[i] + values[j] == 11)
                    {
                        //If there is a move remaining, then set the variable to true
                        hasPair = true;
                    }
                }
            }

            //Checks if no pairs were found
            if (hasPair == false)
            {
                //Starts the timer that leads towards the loss gamestate
                loseTimerStart = true;
            }
        }

        //Pre: Requires player win bool and the values of each on screen slot
        //Post: Gives a bool describing if the player won
        //Desc: Checks each card in the deck to see if they are all face cards. If there aren't any then
        //      the game continues
        private bool CheckWinCondition()
        {
            //New bool set to true to see if the player won
            bool didPlayerWin = true;

            //Nested for loop based off the amount of rows and columns in playing area (2 x 6)
            for (int i = 0; i < slots.GetLength(0); i++)
            {
                for (int j = 0; j < slots.GetLength(1); j++)
                {
                    //Checks to see if the card on the slot is not a face card
                    if (slots[i, j].GetSlotValue() <= 10)
                    {
                        //Thus it sets the win to loss since not all cards are face cards
                        didPlayerWin = false;
                    }
                }
            }

            //Returns the true/false bool for if the player won
            return didPlayerWin;
        }

        //Pre: Requires image data for the cards and location of the deck. It finds the image of all the cards and
        //     uses the source data to find each slot image, location, and color
        //Post: Draws all gameplay objects on screen, both the deck and playing card slots to pair up
        //Desc: Makes use of nested for loops to draw each slot and then over top of it draws out the cards in the
        //      stack. The deck just has a repeated drawing with offset on it to simulate the deck shrinking in size
        private void DrawEverything()
        {
            //Created a for loop that draws a backwards card 52 times to simulate a deck.
            for (int i = 0; i < deck.Count; i++)
            {
                //Offsets the card and then draws the deck out for each remaining card in the deck
                deckLoc.X = 30 + i / 2;
                deckLoc.Y = 30 + i / 2;
                spriteBatch.Draw(cardDownImg, deckLoc, Color.White);
            }

            //Nested for loop based off the amount of rows and columns in playing area (2 x 6)
            for (int i = 0; i < slots.GetLength(0); i++)
            {
                for (int j = 0; j < slots.GetLength(1); j++)
                {
                    //Checks if a card for the playing slots have been taken out of the deck
                    if (takenFormDeck == false)
                    {
                        //Places a card inside each playing spot while removing it from the deck
                        slots[i, j].PlaceCard(deck[i + j]);
                        deck.RemoveAt(i + j);
                    }

                    //Draws the card using the source rectangle and the large image of all the playing cards, 
                    //changing the destination of each slot every time
                    spriteBatch.Draw(allCardsImg, new Rectangle(160 + ((cardWidth + spacingOffset) * j), 200 + (cardHeight + spacingOffset) * i, cardWidth, cardHeight),
                        slots[i, j].GetTopCard().GetSource(), cardColor[i, j]);
                }
            }

            //Nested for loop based off the amount of rows and columns in playing area and the amount of cards in each the selected stack
            for (int i = 0; i < slots.GetLength(0); i++)
            {
                for (int j = 0; j < slots.GetLength(1); j++)
                {
                    for (int k = 0; k < slots[i, j].GetStackCount(); k++)
                    {
                        //Draws the stack cards on top of of the normal cards with a slight offset based on the k value
                        spriteBatch.Draw(allCardsImg, new Rectangle((160 + k * 2) + ((cardWidth + spacingOffset) * j), (200 - k * 2) + (cardHeight + spacingOffset) * i, cardWidth, cardHeight),
                            slots[i, j].GetTopCard().GetSource(), cardColor[i, j]);
                    }
                }
            }
        }

        //Pre: Requires the playing slots on screen and the sound variables, as well as the deck
        //Post: Gives empty card slots and an empty deck, with a bunch of sound bools disabled
        //Desc: Clears and resets bools and lists for a new game
        private void Reinitialize()
        {
            //Nested for loop based off the amount of rows and columns in playing area (2 x 6)
            for (int i = 0; i < slots.GetLength(0); i++)
            {
                for (int j = 0; j < slots.GetLength(1); j++)
                {
                    //Use the reset method on each slot in the play area
                    slots[i, j].Reset();
                }
            }

            //Resets sound bools and clears deck for next game. Makes takenFromDeck false since cards are all back
            takenFormDeck = false;
            didWinSoundPlay = false;
            didLoseSoundPlay = false;
            didGameplaySoundPlay = false;
            deck.Clear();
        }
    }
}
