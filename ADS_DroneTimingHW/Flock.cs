public class Flock
{
    Drone[] agents;
    int num;
    
    public Flock(int maxnum)
    {
        agents = new Drone[maxnum];
    }
    
    // Actually add the drones
    public void Init(int num)
    {
        this.num = num;
        for (int i = 0; i < num; i++)
        {
            agents[i] = new Drone(i);
        }
    }
    
    // Update all drones
    public void Update()
    {
        for (int i = 0; i < num; i++)
        {
            agents[i].Update();
        }
    }

    // Calculate the average battery level of all drones
    public float average()
    {
        float totalBattery = 0;
        for (int i = 0; i < num; i++)
        {
            totalBattery += agents[i].Battery;
        }
        return totalBattery / num;
    }

    // Additional methods to be implemented later
    public int max()
    {
        if (num == 0) return 0;

        int maxValue = agents[0].ID;
        for (int i = 1; i < num; i++)
        {
            if (agents[i].ID > maxValue)
            {
                maxValue = agents[i].ID;
            }
        }
        return maxValue;
    }
    public int min()
    {
        if (num == 0) return 0;

        int minValue = agents[0].ID;
        for (int i = 1; i < num; i++)
        {
            if (agents[i].ID < minValue)
            {
                minValue = agents[i].ID;
            }
        }
        return minValue;
    }
    public void print(){

        using (StreamWriter file = new StreamWriter("results.csv"))
        {
            file.WriteLine("ID,Temperature,Wind,Battery");
            for (int i = 0; i < num; i++)
            {
                Drone drone = agents[i];
                file.WriteLine($"{drone.ID},{drone.Temperature},{drone.Wind},{drone.Battery}");
            }
        }
    }
    public void append(Drone val) {}
    public void appendFront(Drone val) {}
    public void insert(Drone val, int index) {}
    public void deleteFront(int index) {}
    public void deleteBack(int index) {}
    public void delete(int index) {}
    public void bubblesort()
    {
        for (int i = 0; i < num - 1; i++)
        {
            for (int j = 0; j < num - i - 1; j++)
            {
                if (agents[j].ID > agents[j + 1].ID)
                {
                    Drone temp = agents[j];
                    agents[j] = agents[j + 1];
                    agents[j + 1] = temp;
                }
            }
        }
    }
    public void insertionsort() {}
}