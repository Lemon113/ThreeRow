using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ThreeRow.Engine.Systems
{
    class InputSystem : ISystem
    {
        ButtonState prevLKMState = ButtonState.Released;
        ButtonState currState;

        public void Update(GameTime gameTime)
        {
            currState = Mouse.GetState().LeftButton;
            if (prevLKMState == ButtonState.Released && currState == ButtonState.Pressed)
            {
                Point p = Mouse.GetState().Position;
                GameObject go;
                if (GameObjects.GetInstance().TryGetObjectAtPoint(p, out go))
                {
                    go.OnClick();
                }
            }
            prevLKMState = currState;
        }


    }
}
