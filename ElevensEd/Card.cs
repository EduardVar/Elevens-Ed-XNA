// Author: Eduard Varshavsky
// File Name: Card.cs
// Project Name: ElevensEd
// Creation Date: September 11, 2017
// Modified Date: September 12, 2017
// Description: Class that holds numerical value, suit, image data, and location of each card in the game
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
    class Card
    {
        //Created new variables for each part of the card. Value, suit, destination, and image data
        private int value;
        private string suit;
        private Rectangle srcRec;
        private Rectangle destRec;

        //Pre: Requires the value of the card
        //Post: Sets the card value for its specific entity
        //Desc: Used in MainGame set the card value for this entity to be used in the deck and slots
        public void SetValue(int cardValue)
        {
            //Sets the value of the card to the one given in the function
            value = cardValue;
        }

        //Pre: Doesn't require anything
        //Post: Returns the value of the card
        //Desc: Call this method within MainGame to know the value of a card (for putting in stacks)
        public int GetValue()
        {
            //Gives the value of the card
            return value;
        }

        //Pre: Requires the suitSymbol and the suit variable
        //Post: Sets the card suit for its specific entity
        //Desc: Used in MainGame set the card suit for this entity to be used in the deck and slots
        public void setSuit(string suitSymbol)
        {
            //Sets the suit
            suit = suitSymbol;
        }

        //Pre: Doesn't require anything
        //Post: Returns the suit of the card
        //Desc: Call this method within MainGame to know the suit of a card (for putting in stacks)
        public string GetSuit()
        {
            //Gives the suit
            return suit;
        }

        //Pre: Requires the rectangle calculated to be the card's source rectangle
        //Post: Gives a source rectangle tied to the entity
        //Desc: Used in setting a source rectangle based on the card given
        public void SetSource(Rectangle source)
        {
            //Sets the source rectangle
            srcRec = source;
        }

        //Pre: Doesn't require anything
        //Post: Returns the source rectangle of the card
        //Desc: Used in drawing to get the image data to have unique cards
        public Rectangle GetSource()
        {
            //Gives the source data from the method
            return srcRec;
        }

        //Pre: Requires the slot destination of the card
        //Post: Sets the destination for this entity
        //Desc: Used in setting a location for the card in a stack
        public void SetDestination(Rectangle destination)
        {
            //Sets the destination of the card by the function
            destRec = destination;
        }

        //Pre: Doesn't require anything
        //Post: Gets the dimensions of the destination rectangle
        //Desc: Used when the location of the card needs to be detected
        public Rectangle GetDestination()
        {
            //Gives the destination data from the method
            return destRec;
        }

        //Pre: Requires the source image rectangle and image of all the cards
        //Post: Gives the single card image
        //Desc: Creates a new texture 2D based on the cut
        public Texture2D GetImage(Texture2D cardsTemplate, Rectangle srcRec)
        {
            //Sets the image data for the card and gives the image
            Texture2D cardImage = cardsTemplate;
            return cardImage;
        }
    }
}
