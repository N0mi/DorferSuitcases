namespace Core.Gameplay
{
    public class SuitcasePart3x2 : SuitcasePart
    {        
        protected override void CreateGrid()
        {
            base.Height = 3;
            base.Width = 2;            
            slots = new Grid(Width, Height);
        }
    }
}

