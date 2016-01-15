// Generated code -- http://www.redblobgames.com/grids/hexagons/
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

//struct Point
//{
//    public Point(double x, double y)
//    {
//        this.x = x;
//        this.y = y;
//    }
//    public readonly double x;
//    public readonly double y;
//}


public struct Hex
{

    public Hex(int q, int r, int s)
    {
        this.q = q;
        this.r = r;
        this.s = s;
    }
    public readonly int q;
    public readonly int r;
    public readonly int s;

    
    static public bool Equals(Hex a, Hex b)
    {
        if(a.q == b.q && a.r == b.r && a.s == b.s)
        {
            return true;
        }
        return false;
    }
    static public Hex Add(Hex a, Hex b)
    {
        return new Hex(a.q + b.q, a.r + b.r, a.s + b.s);
    }


    static public Hex Subtract(Hex a, Hex b)
    {
        return new Hex(a.q - b.q, a.r - b.r, a.s - b.s);
    }


    static public Hex Scale(Hex a, int k)
    {
        return new Hex(a.q * k, a.r * k, a.s * k);
    }

    static public List<Hex> directions = new List<Hex>{new Hex(1, 0, -1), new Hex(1, -1, 0), new Hex(0, -1, 1), new Hex(-1, 0, 1), new Hex(-1, 1, 0), new Hex(0, 1, -1)};

    static public Hex Direction(int direction)
    {
        return Hex.directions[direction];
    }

    static public List<Hex> Neighbors(Hex Center)
    {
        List<Hex> Ns = new List<Hex>();
        for (int i = 0; i < 6; i++)
        {
            Ns.Add(Neighbor(Center, i));
        }
        
        return Ns;
    }
    static public Hex Neighbor(Hex hex, int direction)
    {
        return Hex.Add(hex, Hex.Direction(direction));
    }
    static public List<Hex> AstarPath(Hex start, Hex Target)
    {
        Dictionary<Hex, Hex> cameFrom = Astar(start, Target);
        List<Hex> Path = new List<Hex>();
        Path.Add(Target);
        int i = 0;
        while (true)
        {
            Path.Add(cameFrom[Path[i]]);

            i++;
            if (Hex.Equals(Path[i], start))
            {
                //Path.Add(start);
                break;
            }
        }
        Path.Reverse();
        return Path;
    }
    static public Dictionary<Hex, Hex> Astar(Hex start, Hex Target)
    {
        List<Hex> PathToTarget = new List<Hex>();
        PriorityQueue Fringes = new PriorityQueue();

        Dictionary<Hex, Hex> cameFrom = new Dictionary<Hex, Hex>();
        Dictionary<Hex, int> costSoFar = new Dictionary<Hex, int>();

        Fringes.Enqueue(start, 0);

        cameFrom[start] = start;
        costSoFar[start] = 0;


        while (Fringes.Count > 0)
        {
            Hex current = Fringes.Dequeue();

            if (Equals(current, Target))
            {
                break;
            }
            List<Hex> Ns = Neighbors(current);
            foreach (Hex n in Ns)
            {
                int newCost = costSoFar[current];

                if (BlockedHexes.Instance.HexData != null)
                {
                    if (BlockedHexes.Instance.HexData.Contains(current))
                    {
                        newCost += 5;
                    }
                }
                if (Equals(current, new Hex(0, 0, 0)))
                {

                }
                if (!costSoFar.ContainsKey(n) || newCost < costSoFar[n])
                {
                    costSoFar[n] = newCost + Distance(n, Target);
                    int pri = newCost + Distance(n, Target);
                    Fringes.Enqueue(n, pri);
                    cameFrom[n] = current;
                }
            }
        }


        return cameFrom;
    }
    static public List<Hex> Reachable(Hex start, int Movement)
    {

        List<Hex> Reachable = new List<Hex>();
        List<Hex> Fringes = new List<Hex>();
        List<Hex> Ns = new List<Hex>();

        Fringes.Add(start);
        Reachable.Add(start);
        for (int l = 0; l < Movement; l++)
        {
            //step loop
            Ns = new List<Hex>();
            foreach (Hex h in Fringes)
            {

                foreach (Hex n in Neighbors(h))
                {
                    Ns.Add(n);
                    if (Reachable.Contains(n))
                    {
                        //dont do anything, hex is already in the array

                    }
                    else
                    {
                        Reachable.Add(n);
                    }
                }
            }
            Fringes.Clear();
            Fringes = Ns;
        }
        return Reachable;
    }
    public class Tuple
    {
        public Tuple(Hex i, int p)
        {
            item = i;
            pri = p;
        }
        public Hex item { get; set; }
        public int pri { get; set; }
    }
    public class PriorityQueue
    {
        private List<Tuple> elements = new List<Tuple>();

        public int Count
        {
            get { return elements.Count; }
        }

        public void Enqueue(Hex item, int priority)
        {
            Tuple t = new Tuple(item, priority);
            elements.Add(t);
        }

        public Hex Dequeue()
        {
            int bestIndex = 0;

            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].pri < elements[bestIndex].pri)
                {
                    bestIndex = i;
                }
            }

            Hex bestItem = elements[bestIndex].item;
            elements.RemoveAt(bestIndex);
            return bestItem;
        }
    }

    static public List<Hex> diagonals = new List<Hex>{new Hex(2, -1, -1), new Hex(1, -2, 1), new Hex(-1, -1, 2), new Hex(-2, 1, 1), new Hex(-1, 2, -1), new Hex(1, 1, -2)};

    static public Hex DiagonalNeighbor(Hex hex, int direction)
    {
        return Hex.Add(hex, Hex.diagonals[direction]);
    }


    static public int Length(Hex hex)
    {
        return (int)((Math.Abs(hex.q) + Math.Abs(hex.r) + Math.Abs(hex.s)) / 2);
    }


    static public int Distance(Hex a, Hex b)
    {
        return Hex.Length(Hex.Subtract(a, b));
    }

}

struct FractionalHex
{
    public FractionalHex(double q, double r, double s)
    {
        this.q = q;
        this.r = r;
        this.s = s;
    }
    public readonly double q;
    public readonly double r;
    public readonly double s;

    static public Hex HexRound(FractionalHex h)
    {
        int q = (int)(Math.Round(h.q));
        int r = (int)(Math.Round(h.r));
        int s = (int)(Math.Round(h.s));
        double q_diff = Math.Abs(q - h.q);
        double r_diff = Math.Abs(r - h.r);
        double s_diff = Math.Abs(s - h.s);
        if (q_diff > r_diff && q_diff > s_diff)
        {
            q = -r - s;
        }
        else
            if (r_diff > s_diff)
            {
                r = -q - s;
            }
            else
            {
                s = -q - r;
            }
        return new Hex(q, r, s);
    }


    static public FractionalHex HexLerp(Hex a, Hex b, double t)
    {
        return new FractionalHex(a.q + (b.q - a.q) * t, a.r + (b.r - a.r) * t, a.s + (b.s - a.s) * t);
    }


    static public List<Hex> HexLinedraw(Hex a, Hex b)
    {
        int N = Hex.Distance(a, b);
        List<Hex> results = new List<Hex>{};
        double step = 1.0 / Math.Max(N, 1);
        for (int i = 0; i <= N; i++)
        {
            results.Add(FractionalHex.HexRound(FractionalHex.HexLerp(a, b, step * i)));
        }
        return results;
    }

}

struct OffsetCoord
{
    public OffsetCoord(int col, int row)
    {
        this.col = col;
        this.row = row;
    }
    public readonly int col;
    public readonly int row;
    static public int EVEN = 1;
    static public int ODD = -1;

    static public OffsetCoord QoffsetFromCube(int offset, Hex h)
    {
        int col = h.q;
        int row = h.r + (int)((h.q + offset * (h.q & 1)) / 2);
        return new OffsetCoord(col, row);
    }


    static public Hex QoffsetToCube(int offset, OffsetCoord h)
    {
        int q = h.col;
        int r = h.row - (int)((h.col + offset * (h.col & 1)) / 2);
        int s = -q - r;
        return new Hex(q, r, s);
    }


    static public OffsetCoord RoffsetFromCube(int offset, Hex h)
    {
        int col = h.q + (int)((h.r + offset * (h.r & 1)) / 2);
        int row = h.r;
        return new OffsetCoord(col, row);
    }


    static public Hex RoffsetToCube(int offset, OffsetCoord h)
    {
        int q = h.col - (int)((h.row + offset * (h.row & 1)) / 2);
        int r = h.row;
        int s = -q - r;
        return new Hex(q, r, s);
    }

}

struct Orientation
{
    public Orientation(double f0, double f1, double f2, double f3, double b0, double b1, double b2, double b3, double start_angle)
    {
        this.f0 = f0;
        this.f1 = f1;
        this.f2 = f2;
        this.f3 = f3;
        this.b0 = b0;
        this.b1 = b1;
        this.b2 = b2;
        this.b3 = b3;
        this.start_angle = start_angle;
    }
    public readonly double f0;
    public readonly double f1;
    public readonly double f2;
    public readonly double f3;
    public readonly double b0;
    public readonly double b1;
    public readonly double b2;
    public readonly double b3;
    public readonly double start_angle;
}

struct Layout
{
    public Layout(Orientation orientation, Vector3 size, Vector3 origin)
    {
        this.orientation = orientation;
        this.size = size;
        this.origin = origin;
    }
    public readonly Orientation orientation;
    public readonly Vector3 size;
    public readonly Vector3 origin;
    static public Orientation pointy = new Orientation(Math.Sqrt(3.0), Math.Sqrt(3.0) / 2.0, 0.0, 3.0 / 2.0, Math.Sqrt(3.0) / 3.0, -1.0 / 3.0, 0.0, 2.0 / 3.0, 0.5);
    static public Orientation flat = new Orientation(3.0 / 2.0, 0.0, Math.Sqrt(3.0) / 2.0, Math.Sqrt(3.0), 2.0 / 3.0, 0.0, -1.0 / 3.0, Math.Sqrt(3.0) / 3.0, 0.0);

    static public Vector3 HexToPixel(Layout layout, Hex h, float yPos)
    {
        Orientation M = layout.orientation;
        Vector3 size = layout.size;
        Vector3 origin = layout.origin;
        double x = (M.f0 * h.q + M.f1 * h.r) * size.x;
        double y = (M.f2 * h.q + M.f3 * h.r) * size.y;
        return new Vector3((float)(x + origin.x),  yPos, (float)(y + origin.y));
    }


    static public FractionalHex PixelToHex(Layout layout, Vector3 p)
    {
        Orientation M = layout.orientation;
        Vector3 size = layout.size;
        Vector3 origin = layout.origin;
        Vector3 pt = new Vector3((p.x - origin.x) / size.x, (p.z - origin.y) / size.y);
        double q = M.b0 * pt.x + M.b1 * pt.y;
        double r = M.b2 * pt.x + M.b3 * pt.y;
        return new FractionalHex(q, r, -q - r);
    }


    static public Vector3 HexCornerOffset(Layout layout, int corner)
    {
        Orientation M = layout.orientation;
        Vector3 size = layout.size;
        double angle = 2.0 * Math.PI * (corner + M.start_angle) / 6;
        return new Vector3((float)(size.x * Math.Cos(angle)), (float)(size.z * Math.Sin(angle)));
    }


    static public List<Vector3> PolygonCorners(Layout layout, Hex h, float yPos)
    {
        List<Vector3> corners = new List<Vector3> {};
        Vector3 center = Layout.HexToPixel(layout, h, yPos);
        for (int i = 0; i < 6; i++)
        {
            Vector3 offset = Layout.HexCornerOffset(layout, i);
            corners.Add(new Vector3(center.x + offset.x, center.z + offset.z));
        }
        return corners;
    }

}