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
using Microsoft.Xna.Framework.Design;

namespace BreakBoll
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D myt, myt1, blc, brickImage;
        ClassSprite myclass, myclass2;
        bool endgameswitch;   //用來開關是否要關閉遊戲
        SpriteFont Sf;//加入一個文字
        int num, ang; //設一個數字與一個旋轉的角度
        Brick[,] brick;
        int bricksWide = 10;
        int bricksHigh = 5;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();  
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
            // TODO: Add your initialization logic here
            IsMouseVisible = true;
            num = 0;
            ang = 0;
            this.Window.AllowUserResizing = false;
            this.Window.Title = "打磚塊";
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

            Sf = Content.Load<SpriteFont>("SpriteFont1");//將文字給讀取進來
            myt = Content.Load<Texture2D>("BOLL");
            brickImage = Content.Load<Texture2D>("brick1");
            //做一個板子
            int gx = 64;
            int gy = 16;
            myt1 = new Texture2D(this.GraphicsDevice, gx, gy, true, this.graphics.PreferredBackBufferFormat);
            Color[] color = new Color[gx * gy];
            for (int i = 0; i < gx * gy; i++)
                color[i] = new Color(150, 0, 255, 255);
            myt1.SetData(color);

            myclass2 = new ClassSprite(myt1, new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - myt1.Width / 2, graphics.GraphicsDevice.Viewport.Height - 20), new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height));  //設定ClassSprite的三個參數

            myclass = new ClassSprite(myt, new Vector2(myclass2.position.X, myclass2.position.Y - 200), new Vector2(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height));  //設定ClassSprite的三個參數
            myclass.velocity = new Vector2(5, 3); //設定球的移動速度

            /*for (int i = 0; i < 6; i++)
            {
                for(int j = 0; j<0; j++)
                {
                    block[i, j] = new ClassSprite();
                }
            }*/
            // TODO: use this.Content to load your game content here
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();            

            //開啟鍵盤控制
            KeyboardState newkeyboard;
            newkeyboard = Keyboard.GetState();
            endgameswitch = myclass.Move();            
            
            if (!endgameswitch)
            {
                //做按下鍵盤時的動作
                if (newkeyboard.IsKeyDown(Keys.Right))
                {
                    if (myclass2.position.X < GraphicsDevice.Viewport.Width - myclass2.texture.Width)
                        myclass2.position.X += 10;
                }
                else if (newkeyboard.IsKeyDown(Keys.Left))
                    if (myclass2.position.X > 0)
                        myclass2.position.X -= 10;
            }
            

            // TODO: Add your update logic here

            //決定 myclass 的動作，若是回傳 true 就關掉遊戲

            //if (endgameswitch == true)
            //this.Exit();

            //myclass2.Move();
            if (myclass.Collides(myclass2))  //如果 myclass 碰到 myclass2
            {
                myclass.velocity.Y *= -1;  //當球碰到板子就會以 Y 的反方向來回彈
                num++;
            }
            
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
            spriteBatch.Begin();
            spriteBatch.Draw(myclass.texture, myclass.position, Color.White);           //使用myclass畫出來
            spriteBatch.Draw(myclass2.texture, myclass2.position, Color.White);         //畫板子
            
            string message = "Score: " + num.ToString();//做一個 message 的字串
            Vector2 fontorig = Sf.MeasureString(message) / 2;//將 fontorig 設為 message 字串長度的 1/2 倍
            Vector2 fontpos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, 50);

            foreach (Brick brick in brick)
                brick.Draw(spriteBatch);

            //載入 文字,字串   ,文字位置,文字顏色   ,角度                     ,文字中點,文字大小,效果        ,文字深度
            spriteBatch.DrawString(Sf, message, fontpos, Color.Red, 0, fontorig, 3.0f, SpriteEffects.None, 0);

            if (endgameswitch)
            {
                string message1 = "Game Over";
                Vector2 fontorig1 = Sf.MeasureString(message1) / 2;
                Vector2 fontpos1 = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);
                spriteBatch.DrawString(Sf, message1, fontpos1, Color.Red, 0, fontorig1, 3.0f, SpriteEffects.None, 0);
            }

            spriteBatch.End();

            base.Draw(gameTime);

        }
        private void StartGame()
        {
            brick = new Brick[bricksWide, bricksHigh];

            for (int y = 0; y < bricksHigh; y++)
            {
                Color tint = Color.White;

                switch (y)
                {
                    case 0:
                        tint = Color.Blue;
                        break;
                    case 1:
                        tint = Color.Red;
                        break;
                    case 2:
                        tint = Color.Green;
                        break;
                    case 3:
                        tint = Color.Yellow;
                        break;
                    case 4:
                        tint = Color.Purple;
                        break;
                }

                for (int x = 0; x < bricksWide; x++)
                {
                    brick[x, y] = new Brick(
                        brickImage,
                        new Rectangle(
                            x * brickImage.Width,
                            y * brickImage.Height,
                            brickImage.Width,
                            brickImage.Height),
                        tint);
                }
            }
        }
    }

}
