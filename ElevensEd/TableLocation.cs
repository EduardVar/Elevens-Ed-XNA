// Author: Eduard Varshavsky
// File Name: TableLocation.cs
// Project Name: ElevensEd
// Creation Date: September 12, 2017
// Modified Date: September 17, 2017
// Description: Class that holds the values, postion, and options to manipulate the cards placed on each
//              playing slot in game
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
    class TableLocation
    {
        //Created a list to hold a stack of cards on each slot, along with its position on screen
        List<Card> tableStack = new List<Card>();
        Rectangle position;

        //Holds the value of the top card in the pile
        int value;

        //Pre: Doesn't require anything
        //Post: Returns the value of the card in the slot stack
        //Desc: Call this method within MainGame to know the value of a card (for stack calculations)
        public int GetSlotValue()
        {
            //Gives the value of the first card in the stack
            return value;
        }

        //Pre: Requires the position of the card stack on the table
        //Post: Sets the position for the location of the stack on screen
        //Desc: Uses the position of the card stack to keep it seperate from the other stacks
        public void SetPosition(Rectangle placeOnTable)
        {
            //Sets position to the provided rectangle
            position = placeOnTable;
        }

        //Pre: Doesn't require anything
        //Post: Returns the position of the card in the slot stack
        //Desc: Call this method within MainGame to know the location of a card stack
        public Rectangle GetPosition()
        {
            //Gives the position of the stack
            return position;
        }

        //Pre: Requires the card that is going to be placed and the table stack list
        //Post: Returns the destination for the card being placed as well as its value
        //Desc: Adds the card to the list in a slot stack to keep track of what has been added
        public void PlaceCard(Card newCard)
        {
            //Adds a newcard into the stack with a destination and value with it
            newCard.SetDestination(position);
            tableStack.Add(newCard);
            value = newCard.GetValue();
        }

        //Pre: Requires the new card to be replaced and the stack information (value, position)
        //Post: Gives the new card atop the face stack with all required information
        //Desc: Used when the player clicks on a card that is a face card and is the first of the pile.
        //      Made for the option to put it to the bottom of the deck
        public Card ReplaceCard(Card newCard)
        {            
            //Checks for the card value being above 1 and being the first card of the stack
            if (tableStack[tableStack.Count - 1].GetValue() > 10 && tableStack.Count < 2)
            {
                //Stores the card to return into the deck and places a new card with a value over it
                Card cardToRerturn = tableStack[tableStack.Count - 1];
                tableStack[tableStack.Count - 1] = newCard;
                value = newCard.GetValue();

                //Tells which card must be returned to the stack
                return cardToRerturn;
            }
            
            //Gives back the card to be returned to the stack again (no change in cards)
            return newCard;
        }

        //Pre: Requires nothing
        //Post: Gives the top card of the stack
        //Desc: Used when determining the value of the top card of the deck
        public Card GetTopCard()
        {
            //Gives back the top card of the slot stack
            return tableStack[tableStack.Count - 1];
        }

        //Pre: Requires nothing
        //Post: Gives back the amount of card in the stack
        //Desc: Used when determining if a card is the first in the stack (for face cards) and for
        //      3D stacking when drawing
        public int GetStackCount()
        {
            //Gives back the amount of cards in the stack
            return tableStack.Count;
        }

        //Pre: Requires nothing
        //Post: Clears the list of card stacks and sets their values to 0
        //Desc: Used when reinitializing the game for a new match of elevens
        public void Reset()
        {
            //Clears the stack of cards and sets their values to 0
            tableStack.Clear();
            value = 0;
        }
    }
}
