using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace TicTacToe
{
    public class TicTacToe : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _layout;
        private Texture2D _x;
        private Texture2D _o;

        private Point[] tilePositions = new Point[9];
        private Rectangle[] tiles = new Rectangle[9];
        private int[] tileStates = new int[9] {
            -1, -1, -1,
            -1, -1, -1,
            -1, -1, -1,
        };

        int[][] moves = new int[8][]
        {
            new int[] { 0, 1, 2 },
            new int[] { 3, 4, 5 },
            new int[] { 6, 7, 8 },

            new int[] { 0, 3, 6 },
            new int[] { 1, 4, 7 },
            new int[] { 2, 5, 8 },

            new int[] { 0, 4, 8 },
            new int[] { 2, 4, 6 },
        };

        private int windowSize = 600;
        private int dividierSize = 6;
        private int tileSize = 192;

        private bool won = false;
        private int currentPlayer = 0;
        private int[] winTiles;

        public TicTacToe()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            
            _graphics.PreferredBackBufferWidth = windowSize; 
            _graphics.PreferredBackBufferHeight = windowSize;
            _graphics.ApplyChanges();

            Point size = new Point(tileSize, tileSize);
            int tile = 0;

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    int x = (tileSize * col) + (dividierSize * (col + 1));
                    int y = (tileSize * row) + (dividierSize * (row + 1));

                    Point position = new Point(x, y);

                    tiles[tile] = new Rectangle(position, size);
                    tilePositions[tile] = position;

                    tile++;
                }
            }
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _layout = Content.Load<Texture2D>("layout");
            _x = Content.Load<Texture2D>("X");
            _o = Content.Load<Texture2D>("O");
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
            
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                for (int i = 0; i < 9; i++) tileStates[i] = -1;
                winTiles = new int[3];
                won = false;
                currentPlayer = 0;

                return;
            }

            MouseState mouseState = Mouse.GetState();
            Point mousePosition = new Point(mouseState.X, mouseState.Y);

            for (int i = 0; i < 9; i++)
            {
                if (
                    tiles[i].Contains(mousePosition) &&
                    mouseState.LeftButton == ButtonState.Pressed &&
                    tileStates[i] == -1 && 
                    !won
                ) {
                    tileStates[i] = currentPlayer;

                    foreach (int[] move in moves)
                    {
                        if (SelectTiles(move).All(t => t == currentPlayer))
                        {
                            won = true;
                            winTiles = move;
                        }
                    }

                    currentPlayer = currentPlayer == 0 ? 1 : 0;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_layout, new Vector2(0, 0), Color.White);
            _spriteBatch.End();

            for (int i = 0; i < 9; i++) if (tileStates[i] != -1) DrawTile(i, Color.White);

            if (won) foreach (int tile in winTiles) DrawTile(tile, Color.Green);
         
            bool noMoves = tileStates.All(t => t != -1);
            if (noMoves && !won) for (int i = 0; i < 9; i++) DrawTile(i, Color.Red);

            base.Draw(gameTime);
        }

        private void DrawTile(int tile, Color color)
        {
            Point position = tilePositions[tile];
            Texture2D texture = tileStates[tile] == 0 ? _x : _o;

            _spriteBatch.Begin();
            _spriteBatch.Draw(texture, position.ToVector2(), color);
            _spriteBatch.End();
        }

        private int[] SelectTiles(int[] indexes)
        {
            int[] result = new int[3];
            for (int i = 0; i < 3; i++) result[i] = this.tileStates[indexes[i]];
            return result;
        }
    }
}
