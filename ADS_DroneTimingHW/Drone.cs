using System;

public class Drone
{
    public int ID {set; get;}
    public float Temperature { set; get; } = 0;
    public float Wind { set; get; } = 0;
    public float Battery { set; get; } = 0;
    
    private Random rnd = new Random();
    
    public Drone(int ID) 
    {
        this.ID = ID;
        Update();
    }
    
    public void Update()
    {
        Temperature = rnd.Next() * 100;
        Wind = rnd.Next() * 100;
        Battery = rnd.Next() * 100;
    }
}