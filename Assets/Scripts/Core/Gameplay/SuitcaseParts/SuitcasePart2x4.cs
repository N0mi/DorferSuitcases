namespace Core.Gameplay
{
    public class SuitcasePart2x4 : SuitcasePart
    {
        protected override void CreateGrid()
        {
            base.Height = 2;
            base.Width = 4;
            slots = new Grid(Width, Height);
        }
    }
}