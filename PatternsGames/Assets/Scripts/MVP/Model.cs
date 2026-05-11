using System;

public class Model {
    public int Attack {get; private set;} = 15;
    public int Defense {get; private set;} = 5;

    public bool HasSword {get; private set;} = false;
    public bool HasRing {get; private set;} = false;
    public bool HasArmor {get; private set;} = false;
    public bool HasHelmet {get; private set;} = false;
    public bool HasShield {get; private set;} = false;

    public event Action OnDataChanged;

    public bool EquipSword() {
        if (!HasSword) {
            HasSword = true;
            Attack += 5;
            OnDataChanged?.Invoke();
            return true;
        }
        
        return false;
    }

    public bool EquipRing() {
        if (!HasRing) {
            HasRing = true;
            Attack += 10;
            OnDataChanged?.Invoke();
            return true;
        }
        return false;
    }

    public bool DrinkPotion() {
        Attack += 2;
        OnDataChanged?.Invoke();
        return true;
    }

    public bool EquipArmor() {
        if (!HasArmor) {
            HasArmor = true;
            Defense += 10;
            OnDataChanged?.Invoke();
            return true;
        }
        return false;
    }


    public bool EquipHelmet() {
        if (!HasHelmet) {
            HasHelmet = true;
            Defense += 8;
            OnDataChanged?.Invoke();
            return true;
        }
        return false;
    }

    public bool EquipShield() {
        if (!HasShield) {
            HasShield = true;
            Defense += 5;
            OnDataChanged?.Invoke();
            return true;
        }
        return false;
    }

    public void Reset() {
        Attack = 15;
        Defense = 5;
        HasSword = false;
        HasRing = false;
        HasArmor = false;
        HasHelmet = false;
        HasShield = false;
        OnDataChanged?.Invoke();
    }

    public string GetRawData(string highlightItem = null)
    {
        if (highlightItem == "Sword")
        {
            return $"<color=green><b>int attack  = {Attack};<b></color>\nint defense = {Defense};\n\n" +
                   $"<color=green><b>bool hasSword  = {HasSword.ToString().ToLower()};<b></color>\n" +
                   $"bool hasRing   = {HasRing.ToString().ToLower()};\n" +
                   $"bool hasArmor  = {HasArmor.ToString().ToLower()};\n" +
                   $"bool hasHelmet = {HasHelmet.ToString().ToLower()};\n" +
                   $"bool hasShield = {HasShield.ToString().ToLower()};";
        }
        else if (highlightItem == "Ring")
        {
            return $"<color=green><b>int attack  = {Attack};<b></color>\nint defense = {Defense};\n\n" +
                   $"bool hasSword  = {HasSword.ToString().ToLower()};\n" +
                   $"<color=green><b>bool hasRing   = {HasRing.ToString().ToLower()};<b></color>\n" +
                   $"bool hasArmor  = {HasArmor.ToString().ToLower()};\n" +
                   $"bool hasHelmet = {HasHelmet.ToString().ToLower()};\n" +
                   $"bool hasShield = {HasShield.ToString().ToLower()};";
        }
        else if (highlightItem == "Armor")
        {
            return $"int attack  = {Attack};\n<color=green><b>int defense = {Defense};<b></color>\n\n" +
                   $"bool hasSword  = {HasSword.ToString().ToLower()};\n" +
                   $"bool hasRing   = {HasRing.ToString().ToLower()};\n" +
                   $"<color=green><b>bool hasArmor  = {HasArmor.ToString().ToLower()};<b></color>\n" +
                   $"bool hasHelmet = {HasHelmet.ToString().ToLower()};\n" +
                   $"bool hasShield = {HasShield.ToString().ToLower()};";
        }
        else if (highlightItem == "Helmet")
        {
            return $"int attack  = {Attack};\n<color=green><b>int defense = {Defense};<b></color>\n\n" +
                   $"bool hasSword  = {HasSword.ToString().ToLower()};\n" +
                   $"bool hasRing   = {HasRing.ToString().ToLower()};\n" +
                   $"bool hasArmor  = {HasArmor.ToString().ToLower()};\n" +
                   $"<color=green><b>bool hasHelmet = {HasHelmet.ToString().ToLower()};<b></color>\n" +
                   $"bool hasShield = {HasShield.ToString().ToLower()};";
        }
        else if (highlightItem == "Shield")
        {
            return $"int attack  = {Attack};\n<color=green><b>int defense = {Defense};<b></color>\n\n" +
                   $"bool hasSword  = {HasSword.ToString().ToLower()};\n" +
                   $"bool hasRing   = {HasRing.ToString().ToLower()};\n" +
                   $"bool hasArmor  = {HasArmor.ToString().ToLower()};\n" +
                   $"bool hasHelmet = {HasHelmet.ToString().ToLower()};\n" +
                   $"<color=green><b>bool hasShield = {HasShield.ToString().ToLower()};<b></color>\n";
        }
        else if (highlightItem == "Potion")
        {
            return $"<color=green><b>int attack  = {Attack};<b></color>\nint defense = {Defense};\n\n" +
                   $"bool hasSword  = {HasSword.ToString().ToLower()};\n" +
                   $"bool hasRing   = {HasRing.ToString().ToLower()};\n" +
                   $"bool hasArmor  = {HasArmor.ToString().ToLower()};\n" +
                   $"bool hasHelmet = {HasHelmet.ToString().ToLower()};\n" +
                   $"bool hasShield = {HasShield.ToString().ToLower()};";
        }
        else if (highlightItem == "Reset")
        {
            return $"<color=green><b>int attack = {Attack};<b></color>\n<color=green><b>int defense = {Defense};<b></color>\n\n" +
                   $"<color=green><b>bool hasSword  = {HasSword.ToString().ToLower()};<b></color>\n" +
                   $"<color=green><b>bool hasRing   = {HasRing.ToString().ToLower()};<b></color>\n" +
                   $"<color=green><b>bool hasArmor  = {HasArmor.ToString().ToLower()};<b></color>\n" +
                   $"<color=green><b>bool hasHelmet = {HasHelmet.ToString().ToLower()};<b></color>\n" +
                   $"<color=green><b>bool hasShield = {HasShield.ToString().ToLower()};<b></color>\n";
        }
        else
        {
            return $"int attack  = {Attack};\nint defense = {Defense};\n\n" +
               $"bool hasSword  = {HasSword.ToString().ToLower()};\n" +
               $"bool hasRing   = {HasRing.ToString().ToLower()};\n" +
               $"bool hasArmor  = {HasArmor.ToString().ToLower()};\n" +
               $"bool hasHelmet = {HasHelmet.ToString().ToLower()};\n" +
               $"bool hasShield = {HasShield.ToString().ToLower()};";
        }
    }

}