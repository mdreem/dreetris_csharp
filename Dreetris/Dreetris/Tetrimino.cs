using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Dreetris
{
    class Tetrimino
    {
        #region Shape definitions

        static int[,,] I_shape = {{{1,0,0,0},
                                   {1,0,0,0},
                                   {1,0,0,0},
                                   {1,0,0,0}},
                                  {{1,1,1,1},
                                   {0,0,0,0},
                                   {0,0,0,0},
                                   {0,0,0,0}}};

        static int[, ,] J_shape = {{{0,1,0,0},
                                    {0,1,0,0},
                                    {1,1,0,0},
                                    {0,0,0,0}},
                                  {{0,0,0,0},
                                   {1,1,1,0},
                                   {0,0,1,0},
                                   {0,0,0,0}},
                                  {{0,1,1,0},
                                   {0,1,0,0},
                                   {0,1,0,0},
                                   {0,0,0,0}},
                                  {{1,0,0,0},
                                   {1,1,1,0},
                                   {0,0,0,0},
                                   {0,0,0,0}}};

        static int[, ,] L_shape = {{{0,1,0,0},
                                    {0,1,0,0},
                                    {0,1,1,0},
                                    {0,0,0,0}},
                                  {{0,0,1,0},
                                   {1,1,1,0},
                                   {0,0,0,0},
                                   {0,0,0,0}},
                                  {{1,1,0,0},
                                   {0,1,0,0},
                                   {0,1,0,0},
                                   {0,0,0,0}},
                                  {{0,0,0,0},
                                   {1,1,1,0},
                                   {1,0,0,0},
                                   {0,0,0,0}}};

        static int[, ,] O_shape = {{{1,1,0,0},
                                    {1,1,0,0},
                                    {0,0,0,0},
                                    {0,0,0,0}},
                                   };

        static int[, ,] S_shape = {{{0,1,1,0},
                                    {1,1,0,0},
                                    {0,0,0,0},
                                    {0,0,0,0}},
                                  {{0,1,0,0},
                                   {0,1,1,0},
                                   {0,0,1,0},
                                   {0,0,0,0}}};

        static int[, ,] T_shape = {{{0,1,0,0},
                                    {1,1,1,0},
                                    {0,0,0,0},
                                    {0,0,0,0}},
                                  {{0,1,0,0},
                                   {0,1,1,0},
                                   {0,1,0,0},
                                   {0,0,0,0}},
                                  {{0,0,0,0},
                                   {1,1,1,0},
                                   {0,1,0,0},
                                   {0,0,0,0}},
                                  {{0,1,0,0},
                                   {1,1,0,0},
                                   {0,1,0,0},
                                   {0,0,0,0}}};

        static int[, ,] Z_shape = {{{1,1,0,0},
                                    {0,1,1,0},
                                    {0,0,0,0},
                                    {0,0,0,0}},
                                  {{0,0,1,0},
                                   {0,1,1,0},
                                   {0,1,0,0},
                                   {0,0,0,0}}};
        #endregion

        #region Fields

        public enum Type { None, I, J, L, O, S, T, Z };
        int[,] current_shape = new int[4,4];

        public static int BLOCK_WIDTH = 20;
        public static int BLOCK_HEIGHT = 20;

        int flip_state;
        int num_states;

        Texture2D sprite;
        Rectangle draw_rectangle;
        Tetrimino.Type type;

        private Point coordinates; // coordinates on the screen
        public Point board_position; // coordinates of the board on the screen
        public Point position; // position on the board

        #endregion

        #region Constructors

        /// <summary>
        ///  Handles a Tetrimino
        /// </summary>
        /// <param name="contentManager">the content manager for loading content</param>
        /// <param name="type">the tetrimino type</param>
        public Tetrimino(ContentManager contentManager, Tetrimino.Type type)
        {
            //System.Diagnostics.Debug.WriteLine("Yay!");
            LoadContent(contentManager);
            this.type = type;
            coordinates = new Point();
            board_position = new Point();
            position = new Point();
            flip_state = 0;

            switch (type)
            {
                case Type.I:
                    num_states = I_shape.GetLength(0);
                    break;
                case Type.J:
                    num_states = J_shape.GetLength(0);
                    break;
                case Type.L:
                    num_states = L_shape.GetLength(0);
                    break;
                case Type.O:
                    num_states = O_shape.GetLength(0);
                    break;
                case Type.S:
                    num_states = S_shape.GetLength(0);
                    break;
                case Type.T:
                    num_states = T_shape.GetLength(0);
                    break;
                case Type.Z:
                    num_states = Z_shape.GetLength(0);
                    break;
                default:
                    break;
            }    

            UpdateShape();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// ...
        /// </summary>
        public void Update(GameTime gameTime)
        {
            UpdateShape();     
        }

        /// <summary>
        /// ...
        /// </summary>
        public void UpdateShape()
        {
            switch (type)
            {
                case Type.I:
                    set_current_shape(I_shape);
                    break;
                case Type.J:
                    set_current_shape(J_shape);
                    break;
                case Type.L:
                    set_current_shape(L_shape);
                    break;
                case Type.O:
                    set_current_shape(O_shape);
                    break;
                case Type.S:
                    set_current_shape(S_shape);
                    break;
                case Type.T:
                    set_current_shape(T_shape);
                    break;
                case Type.Z:
                    set_current_shape(Z_shape);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Draws the tetrimino
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to use</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            coordinates.X = board_position.X + BLOCK_WIDTH * position.X;
            coordinates.Y = board_position.Y + BLOCK_HEIGHT * position.Y;
            Color current_color;

            switch (type)
            {
                case Tetrimino.Type.I:
                    current_color = Color.Cyan;
                    break;
                case Tetrimino.Type.J:
                    current_color = Color.DarkBlue;
                    break;
                case Tetrimino.Type.L:
                    current_color = Color.Orange;
                    break;
                case Tetrimino.Type.O:
                    current_color = Color.Yellow;
                    break;
                case Tetrimino.Type.S:
                    current_color = Color.Green;
                    break;
                case Tetrimino.Type.T:
                    current_color = Color.Purple;
                    break;
                case Tetrimino.Type.Z:
                    current_color = Color.Red;
                    break;
                default:
                    current_color = Color.Black;
                    break;
            }

            for (int i = 0; i < current_shape.GetLength(0); i++)
                for (int j = 0; j < current_shape.GetLength(1); j++)
                {
                    if (current_shape[i, j] == 1)
                    {
                        draw_rectangle.X = coordinates.X + i * draw_rectangle.Height;
                        draw_rectangle.Y = coordinates.Y + j * draw_rectangle.Width;
                        spriteBatch.Draw(sprite, draw_rectangle, current_color);
                    }
                }
        }

        public Tetrimino.Type get_type()
        {
            return type;
        }

        public void flip()
        {
            flip_state = (flip_state + 1) % num_states;
            UpdateShape();
        }

        public void unflip()
        {
            // Flip back, but stay at positive values
            flip_state = (flip_state + num_states - 1) % num_states;
            UpdateShape();
        }

        public int[,] getCurrentShape()
        {
            return current_shape;
        }

        #endregion

        #region Private methods

        private void LoadContent(ContentManager contentManager)
        {
            // load content and set remainder of draw rectangle
            sprite = contentManager.Load<Texture2D>("block");
            draw_rectangle = new Rectangle(0, 0, BLOCK_WIDTH, BLOCK_HEIGHT);
            //draw_rectangle = new Rectangle(x, y, sprite.Width, sprite.Height);
        }

        private void set_current_shape(int[,,] shape)
        {
            for (int i = 0; i < shape.GetLength(1); i++)
                for (int j = 0; j < shape.GetLength(2); j++)
                {
                    //System.Diagnostics.Debug.WriteLine("Flip: " + flip_state.ToString());
                    // i,j flipped?
                    current_shape[i, j] = shape[flip_state, j, i];
                }
        }

        #endregion
    }
}
