using System;
using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFML_Test
{
    public class Entity
    {
        public static Texture EntityTexture { get; } = new Texture("Texture/entity.png");
        public static IntRect EntitySize { get; } = new IntRect(0, 0, 32, 48);

        public Entity(Vector2f SpawnPoint, int EntityNumber = 0, float dx = 1.0f, float dy = 1.0f)
        {
            Position = SpawnPoint;
            this.EntityNumber = EntityNumber;
            Acceleration.X = dx;
            Acceleration.Y = dy;
            
            Sprite.Texture = EntityTexture;
            Sprite.TextureRect = EntitySize;
            SpritePosition = new Vector2f((Global.VM.Width - EntitySize.Width) / 2, (Global.VM.Height - EntitySize.Height) / 2);
        }

        ~Entity()
        {

        }

        public bool FacingRight = true;
        public bool onGround = false;
        public bool isMoving = false;

        private Vector2f Position = new Vector2f();
        public Vector2f GetPosition { get { return Position; } }

        private Vector2f Acceleration;

        private const float MaxSpeedX = 2f;
        private const float MaxSpeedY = 10f;

        private float JumpSpeed;

        public void Jump(float JumpSpeed = -MaxSpeedY)
        {
            if (onGround)
            {
                this.JumpSpeed = JumpSpeed;
                onGround = false;
            }
        }

        public void Teleport(Vector2f Point)
        {
            Position = Point;
        }

        // блок сверху
        private void CollisionUp(Vector2i BlockPosition)
        {
            try
            {
                if (Global.Map[BlockPosition.Y][BlockPosition.X] != ' ' && Global.Map[BlockPosition.Y][BlockPosition.X] != 'B')
                {
                    if (JumpSpeed < 0f)
                    {
                        Position.Y = (BlockPosition.Y + 1) * Resource.BlockSize;
                        JumpSpeed = 0f;
                    }
                }
            }
            catch
            {
                Teleport(Global.SpawnPoint);
            }
        }

        // блок снизу
        private void CollisionDown(Vector2i BlockPosition)
        {
            try
            {
                if (Global.Map[BlockPosition.Y][BlockPosition.X] != ' ' && Global.Map[BlockPosition.Y][BlockPosition.X] != 'B')
                {
                    if (JumpSpeed > 0f)
                    {
                        Position.Y = (BlockPosition.Y) * Resource.BlockSize - Math.Abs(Sprite.TextureRect.Height);
                        JumpSpeed = 0f;
                        onGround = true;
                    }
                }
            }
            catch
            {
                Teleport(Global.SpawnPoint);
            }
        }

        // блок слева
        private void CollisionLeft(Vector2i BlockPosition)
        {
            try
            {
                if (Global.Map[BlockPosition.Y][BlockPosition.X] != ' ' && Global.Map[BlockPosition.Y][BlockPosition.X] != 'B')
                {
                    Position.X = (BlockPosition.X + 1) * Resource.BlockSize;
                }
            }
            catch
            {
                Teleport(Global.SpawnPoint);
            }
        }

        // блок справа
        private void CollisionRight(Vector2i BlockPosition)
        {
            try
            {
                if (Global.Map[BlockPosition.Y][BlockPosition.X] != ' ' && Global.Map[BlockPosition.Y][BlockPosition.X] != 'B')
                {
                    Position.X = (BlockPosition.X - 1) * Resource.BlockSize;
                }
            }
            catch
            {
                Teleport(Global.SpawnPoint);
            }
        }

        protected virtual void Process()
        {
            Vector2i BlockPosition = new Vector2i();

            // сдвиг на основе вертикальной скорости
            Position.Y += JumpSpeed;
            JumpSpeed += Acceleration.Y;

            // коллизии
            BlockPosition.X = ((int)Position.X + 1) / Resource.BlockSize;
            BlockPosition.Y = (int)Position.Y / Resource.BlockSize;
            CollisionUp(BlockPosition);
            BlockPosition.Y = ((int)Position.Y + Sprite.TextureRect.Height) / Resource.BlockSize;
            CollisionDown(BlockPosition);

            BlockPosition.X = ((int)Position.X + Math.Abs(Sprite.TextureRect.Width) - 1) / Resource.BlockSize;
            BlockPosition.Y = (int)Position.Y / Resource.BlockSize;
            CollisionUp(BlockPosition);
            BlockPosition.Y = ((int)Position.Y + Sprite.TextureRect.Height) / Resource.BlockSize;
            CollisionDown(BlockPosition);

            // ходить
            if (isMoving)
            {
                if (FacingRight)
                {
                    Position.X += Acceleration.X;
                }
                else
                {
                    Position.X -= Acceleration.X;
                }
            }
            else
            {
                Frame = 0;
            }

            // коллизии 
            BlockPosition.X = ((int)Position.X) / Resource.BlockSize;
            BlockPosition.Y = ((int)Position.Y + 1) / Resource.BlockSize;
            CollisionLeft(BlockPosition);
            BlockPosition.X = ((int)Position.X + Math.Abs(Sprite.TextureRect.Width)) / Resource.BlockSize;
            CollisionRight(BlockPosition);

            BlockPosition.X = ((int)Position.X) / Resource.BlockSize;
            BlockPosition.Y = ((int)Position.Y + Sprite.TextureRect.Height - 1) / Resource.BlockSize;
            CollisionLeft(BlockPosition);
            BlockPosition.X = ((int)Position.X + Math.Abs(Sprite.TextureRect.Width)) / Resource.BlockSize;
            CollisionRight(BlockPosition);
        }

        public static float OffsetX = (Global.VM.Width - EntitySize.Width) / 2;
        public static float OffsetY = (Global.VM.Height - EntitySize.Height) / 2;

        protected byte Frame;
        protected Sprite Sprite = new Sprite();
        protected int EntityNumber;
        protected Vector2f SpritePosition;

        public virtual Sprite Draw()
        {
            Process();

            if (FacingRight)
            {
                if (isMoving)
                {
                    IntRect IR;
                    IR.Left = Frame % 64 / 16 * 32;
                    IR.Top = EntityNumber * 48;
                    IR.Width = 32;
                    IR.Height = 48;
                    Sprite.TextureRect = IR;
                }
                else
                {
                    IntRect IR;
                    IR.Left = 0;
                    IR.Top = EntityNumber * 48;
                    IR.Width = 32;
                    IR.Height = 48;
                    Sprite.TextureRect = IR;
                }
            }
            else
            {
                if (isMoving)
                {
                    IntRect IR;
                    IR.Left = Frame % 64 / 16 * 32 + 32;
                    IR.Top = EntityNumber * 48;
                    IR.Width = -32;
                    IR.Height = 48;
                    Sprite.TextureRect = IR;
                }
                else
                {
                    IntRect IR;
                    IR.Left = 32;
                    IR.Top = EntityNumber * 48;
                    IR.Width = -32;
                    IR.Height = 48;
                    Sprite.TextureRect = IR;
                }
            }

            Sprite.Position = SpritePosition;
            
            Frame++;
            return Sprite;
        }
    }
}
