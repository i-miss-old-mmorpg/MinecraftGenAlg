public static class MapData
{
    private static int _seedValue;
    private static int _width;
    private static int _depth;
    private static int _height;
    private static System.Random random = new System.Random();

    static MapData()
    {
        _seedValue = GenerateRandomSeed();
        _width = 100;
        _depth = 100;
        _height = 30;
    }

    public static int Seed
    {
        get => _seedValue;
        set
        {
            if (value > 999999 || value < 0)
            {
                // TBD
                _seedValue = random.Next(0, 1000000);
            }
            else
            {
                _seedValue = value;
            }
        }
    }
    public static int Width
    {
        get => _width;
        set
        {
            if (value > 100 || value < 0)
            {
                // TBD
                _width = 100;
            }
            else
            {
                _width = value;
            }
        }
    }
    public static int Depth
    {
        get => _depth;
        set
        {
            if (value > 100 || value < 0)
            {
                // TBD
                _depth = 100;
            }
            else
            {
                _depth = value;
            }
        }
    }
    public static int Height
    {
        get => _height;
        set
        {
            if (value > 100 || value < 0)
            {
                // TBD
                _height = 30;
            }
            else
            {
                _height = value;
            }
        }
    }
    public static int GenerateRandomSeed()
    {
        _seedValue = random.Next(0, 1000000);
        return _seedValue;
    }
}