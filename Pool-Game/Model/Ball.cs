using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Ball
    {
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double VelocityX { get; set; }
        public double VelocityY { get; set; }
        public double Radius { get; set; } = 10;
    }

}
