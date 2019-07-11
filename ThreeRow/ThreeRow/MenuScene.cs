using ThreeRow.Engine;

namespace ThreeRow.ThreeRow
{
    public class MenuScene : IScene
    {
        public MenuScene()
        {
            Background bg = new Background();
            bg.transform.AddChild(new PlayButton().transform);
        }
    }
}
