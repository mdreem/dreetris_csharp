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

using Dreetris.Animation;
using Dreetris.Screens;

namespace Dreetris
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        Texture2D backgroundImage;
        Rectangle backgroundRectangle;

        Texture2D test_image;
        Rectangle test_rectangle;
        Dreetris.Animation.Animation animation = new Dreetris.Animation.Animation();

        const int WINDOW_WIDTH = 800;
        const int WINDOW_HEIGHT = 600;
        const int KEY_PRESSED_TIME = 150;

        DKeyboard keyboard = new DKeyboard();

        SpriteFont Font1;

        Writer writer = new Writer(500);
        BezierCurve bz;

        ScreenManager screenManager;
        AssetManager assetManager;
        private Texture2D blank;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Change resolution
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            /*
            animation.addKeyframe(new KeyframeStraight(new Vector2(600, 200),
                                                new Vector2(650, 250),
                                                1000));

            animation.addKeyframe(new KeyframeStraight(new Vector2(650, 250),
                                                new Vector2(450, 350),
                                                2000));

            animation.addKeyframe(new KeyframeStraight(new Vector2(450, 350),
                                                new Vector2(600, 200),
                                                1000));

            bz = new BezierCurve(new Vector2(600, 200), new Vector2(650, 375),
                                             new Vector2(750, 250),
                                             new Vector2(750, 350)
                                              );

            Keyframe_Bezier kb = new Keyframe_Bezier(bz, 2000);

            animation.addKeyframe(kb);

            blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
            */

            SoundEffect.MasterVolume = 0.025f;

            assetManager = new AssetManager(this.Content);
            screenManager = new ScreenManager(this, assetManager);

            GameScreen gs = new GameScreen(this, screenManager, assetManager);
            gs.Initialize();

            TitleScreen ts = new TitleScreen(this, screenManager);
            ts.Initialize();

            screenManager.push(gs);
            screenManager.push(ts);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            test_image = Content.Load<Texture2D>("block");
            test_rectangle = new Rectangle(600, 200, 20, 20);

            //assetManager.getSprite("block");

            blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
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
            screenManager.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            screenManager.Draw(gameTime);
            base.Draw(gameTime);
        }

        void DrawLongLine(List<Vector2> sublines, Color color)
        {
            for (int i = 0; i < sublines.Count - 1; i++)
            {
                DrawLine(1, color, sublines[i], sublines[i + 1]);
            }
        }

        void DrawLine(float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            spriteBatch.Draw(blank, point1, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 0);
        }
    }
}
