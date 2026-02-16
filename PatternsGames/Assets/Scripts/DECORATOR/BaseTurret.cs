using UnityEngine;
using UnityEditor;

[System.Flags]
public enum TurretVisual
{
    None = 0,
    FireRate = 1,
    Range = 2
}

public interface ITurret
{
    float Range { get; }
    float FireRate { get; }
    int Cost { get; }
    TurretVisual Visuals { get;  }
}


public class BaseTurret : ITurret
{
    public float Range { get; protected set; }
    public float FireRate {get; protected set; }
    public int Cost { get; protected set; }
    public TurretVisual Visuals => TurretVisual.None;

    public BaseTurret(float range, float fireRate, int cost)
    {
        Range = range;
        FireRate = fireRate;
        Cost = cost;
    }

}

public abstract class TurretDecorator : ITurret
{
    protected ITurret turret;

    public virtual float Range => turret.Range;
    public virtual float FireRate => turret.FireRate;
    public virtual int Cost => turret.Cost;
    public virtual TurretVisual Visuals => turret.Visuals;
    public TurretDecorator(ITurret turret)
    {
        this.turret = turret;
    }

}

public class FireRateDecorator : TurretDecorator
{
    private float multiplier;
    private float rangeDecrease;

    public override float FireRate => base.FireRate * multiplier;
    public override int Cost => base.Cost + 400;

    public override float Range => base.Range - rangeDecrease;

    public override TurretVisual Visuals => base.Visuals | TurretVisual.FireRate;

    public FireRateDecorator(ITurret turret, float multiplier, float rangeDecrease) : base(turret)
    {
        this.multiplier = multiplier;
        this.rangeDecrease = rangeDecrease;
    }
}

public class RangeDecorator : TurretDecorator
{
    private float bonusRange;
    private float fireRateDecrease;
    public override float Range => base.Range + bonusRange;
    public override float FireRate => base.FireRate - fireRateDecrease;
    public override int Cost => base.Cost + 900;

    public override TurretVisual Visuals => base.Visuals | TurretVisual.Range;
    public RangeDecorator(ITurret turret, float bonusRange, float fireRateDecrease) : base(turret)
    {
        this.bonusRange = bonusRange;
        this.fireRateDecrease = fireRateDecrease;
    }
}
