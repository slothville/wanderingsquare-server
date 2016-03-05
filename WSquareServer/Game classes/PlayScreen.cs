#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

#endregion

namespace WSquareServer
{
	public class PlayScreen:GameScreen
	{

		Texture2D squareTexture;
		private Square square;
		public int WindowHeight{get;set;}
		public int WindowWidth{get;set;}

		public PlayScreen()
        {
			
			WindowWidth=Program.Game.Window.ClientBounds.Width;
			WindowHeight=Program.Game.Window.ClientBounds.Height;

        }
		
		
		
		public override void LoadContent (ContentManager Content)
		{
			base.LoadContent(Content);
			
			if(squareTexture==null)
			{
			squareTexture=content.Load<Texture2D>("pixel");
			}

			square = new Square (squareTexture);
			
	
		}
		
		public override void UnloadContent ()
		{
			base.UnloadContent();
		}
		
		public override void Update (GameTime gameTime)
		{
			square.Update ();
		}
		
		public override void Draw (SpriteBatch spriteBatch)
		{
			square.Draw(spriteBatch);	
		}

	}
}


