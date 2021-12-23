namespace AoC2021.Days.Day22Utils
{
    struct Instruction
    {
        public bool On;
        public Cuboid Region;

        public Instruction(bool on, Cuboid cuboid)
        {
            On = on;
            Region = cuboid;
        }

    }
}