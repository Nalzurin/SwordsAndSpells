using Godot;
using System;

public class HealthComponent
{

    public int HealthTotal {  get; private set; }
    public int HealthCurrent { get; private set; }

    public void RestoreHealth(int value)
    {
        if(HealthCurrent + value > HealthTotal)
        {
            HealthCurrent = HealthTotal;
        }
        else
        {
            HealthCurrent += value;
        }
    }
    public void RemoveHealth(int value)
    { 
        if(HealthCurrent < 0)
        {
            HealthCurrent = 0;
        }
        else
        {
            HealthCurrent -= value;
        }
    }

}
