public class Residential : Building
{
    public Residential(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;

        Behaviours = Behaviours.Waterable | Behaviours.Powerable | Behaviours.Taxable;
    }
    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}