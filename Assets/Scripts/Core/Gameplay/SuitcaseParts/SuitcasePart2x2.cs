namespace Core.Gameplay
{
    public class SuitcasePart2x2 : SuitcasePart
    {
        protected override void CreateGrid()
        {
            base.Height = 2;
            base.Width = 2;            
            slots = new Grid(Width, Height);
        }
    }
}
