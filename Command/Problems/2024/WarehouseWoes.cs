using Command.Framework;
using Command.Lib.Primitives;
using System.Text;

namespace Command.Problems._2024
{
    public enum MapObjectType { Crate, Wall, Empty, Robot }

    public class MapObject
    {
        public MapObject(MapObjectType type, Point[] points)
        {
            Type = type;
            Points = points;
        }

        public bool Contains(Point p) => Points.Where(r => r == p).Any();
        public MapObjectType Type { get; set; }
        public Point[] Points { get; set; }
        public Point Position => Points.First();

        public void MoveNorth() => Points = Points.Select(p => p.North()).ToArray();
        public void MoveSouth() => Points = Points.Select(p => p.South()).ToArray();
        public void MoveEast() => Points = Points.Select(p => p.East()).ToArray();
        public void MoveWest() => Points = Points.Select(p => p.West()).ToArray();
        public void MoveTo(Point p)
        {
            var diff = p - Points.First();
            Points = Points.Select(p => p + diff).ToArray();
        }
        public void MoveBy(Point diff)
        {
            Points = Points.Select(p => p + diff).ToArray();
        }
    }

    public partial class WarehouseWoes : ProblemBase<int>
    {
        int loadingPhase = 0;
        TextMap map = new TextMap();
        List<MapObject> crates = new List<MapObject>();
        string instructions = string.Empty;
        public WarehouseWoes()
        {
        }

        protected override void Line(string line)
        {
            if (String.IsNullOrWhiteSpace(line))
            {
                loadingPhase = 1;
            }

            if (loadingPhase == 0 && !string.IsNullOrWhiteSpace(line))
            {
                map.Add(line);
            }

            if (loadingPhase == 1 && !string.IsNullOrWhiteSpace(line))
            {
                instructions += line.Trim();
            }

        }

        // This solution is text based only
        public override int CalculateOne()
        {
            foreach (char instruction in instructions)
            {
                //Console.WriteLine(instruction);
                var robotPoint = map.FindFirst('@');
                ArgumentNullException.ThrowIfNull(robotPoint);
                switch (instruction)
                {
                    case '<':
                        {
                            var west = robotPoint.Value.West();

                            bool space = false;
                            while (map[west] != '#')
                            {
                                if (map[west] == '.')
                                {
                                    space = true;
                                    break;
                                }
                                west = west.West();
                            }

                            if (!space) break;

                            do
                            {
                                map.Set(west, map[west.East()]);
                                west = west.East();
                            }
                            while (map[west] != '@');
                            map.Set(west, '.');
                        }
                        break;
                    case '^':
                        {
                            var north = robotPoint.Value.North();

                            bool space = false;
                            while (map[north] != '#')
                            {
                                if (map[north] == '.')
                                {
                                    space = true;
                                    break;
                                }
                                north = north.North();
                            }

                            if (!space) break;

                            do
                            {
                                map.Set(north, map[north.South()]);
                                north = north.South();
                            }
                            while (map[north] != '@');
                            map.Set(north, '.');
                        }
                        break;
                    case '>':
                        {
                            var east = robotPoint.Value.East();

                            bool space = false;
                            while (map[east] != '#')
                            {
                                if (map[east] == '.')
                                {
                                    space = true;
                                    break;
                                }
                                east = east.East();
                            }

                            if (!space) break;

                            do
                            {
                                map.Set(east, map[east.West()]);
                                east = east.West();
                            }
                            while (map[east] != '@');
                            map.Set(east, '.');
                        }
                        break;
                    case 'v':
                        {
                            var south = robotPoint.Value.South();

                            bool space = false;
                            while (map[south] != '#')
                            {
                                if (map[south] == '.')
                                {
                                    space = true;
                                    break;
                                }
                                south = south.South();
                            }

                            if (!space) break;

                            do
                            {
                                map.Set(south, map[south.North()]);
                                south = south.North();
                            }
                            while (map[south] != '@');
                            map.Set(south, '.');
                        }
                        break;
                    default:
                        throw new Exception("Unknown instruction.");
                }

                //map.PrintMap();

            }

            return (int)map.Where(r => r == 'O').Sum(r => r.Y * 100 + r.X);
        }

        private MapObject? GetMapObject(Point p) => crates.FirstOrDefault(c => c.Contains(p));
        private (bool canMove, MapObject?[] affected) CanMove(MapObject mapObject, Point diff)
        {
            if (mapObject.Type == MapObjectType.Empty) return (true, []);
            if (mapObject.Type == MapObjectType.Wall) return (false, []);
            var directionalPoints = mapObject.Points.Select(r => r + diff).Where(p => !mapObject.Contains(p));
            var directionalObjects = directionalPoints.Select(p => GetMapObject(p));
            var canMove = directionalObjects.All(p => p?.Type == MapObjectType.Empty);
            if (canMove) return (true, directionalPoints.Select(r => GetMapObject(r)).ToArray());

            var movingObjects = directionalPoints.Select(p => GetMapObject(p)).ToArray();
            List<MapObject> affectedObjects = new(movingObjects);
            foreach (var movingObj in movingObjects)
            {
                var canMoveChild = CanMove(movingObj, diff);
                if (!canMoveChild.Item1) return (false, []);
                affectedObjects.AddRange(canMoveChild.affected);
            }

            return (true, affectedObjects.ToArray());
        }

        // This is object based and uses recursion to find moves.
        public override int CalculateTwo()
        {
            // Transform the map and create the objects
            var newMap = new TextMap();
            foreach (var line in map.Rows)
            {
                StringBuilder newLine = new StringBuilder();
                foreach (var c in line)
                {
                    newLine.Append(c switch
                    {
                        '#' => "##",
                        'O' => "[]",
                        '.' => "..",
                        '@' => "@.",
                        _ => throw new NotImplementedException()
                    });
                }
                newMap.Add(newLine.ToString());
            }
            crates = new List<MapObject>([
                .. newMap.Where('[').Select(p => new MapObject(MapObjectType.Crate, [p, p.East()])).ToArray(),
                .. newMap.Where('.').Select(p => new MapObject(MapObjectType.Empty, [p])).ToArray(),
                .. newMap.Where('#').Select(p => new MapObject(MapObjectType.Wall, [p])).ToArray(),
                .. newMap.Where('@').Select(p => new MapObject(MapObjectType.Robot, [p])).ToArray()
            ]);

            // Paing an object back to the original map
            


            foreach (char instruction in instructions)
            {
                var robot = crates.Single(c => c.Type == MapObjectType.Robot);
                var robotPoint = robot.Position;
                var diff = instruction switch
                {
                    '<' => (-1, 0),
                    '^' => (0, -1),
                    '>' => (1, 0),
                    'v' => (0, 1),
                    _ => throw new Exception("Unknown instruction.")
                };

                var canMove = CanMove(robot, diff);
                if (canMove.canMove)
                {
                    // Move the objects making a list of now possibly empty spaces
                    List<Point> possiblyEmpty = new();
                    var affectedList = canMove.affected.Distinct().ToList();
                    foreach (var affected in canMove.affected.Distinct())
                    {
                        if (affected.Type != MapObjectType.Empty)
                        {
                            possiblyEmpty.AddRange(affected.Points);
                            affected.MoveBy(diff);
                        } else
                        {
                            // Remove empties what were replaced
                            crates.Remove(affected);
                        }
                    }

                    possiblyEmpty.Add(robot.Position);
                    robot.MoveBy(diff);

                    // Create new empty locations if any points is not unoccupied
                    foreach (var empty in possiblyEmpty) if (GetMapObject(empty) is null) crates.Add(new MapObject(MapObjectType.Empty, [empty]));
                }

                // Rebuild and print
                
                //foreach (var point in newMap.Points()) newMap.Set(point, '*');
                //foreach (var crate in crates) PaintObject(crate, newMap);

                //newMap.PrintMap();

            }

            return (int)crates.Where(r => r.Type == MapObjectType.Crate).Sum(r => r.Position.Y * 100 + r.Position.X);

        }

        void PaintObject (MapObject obj, TextMap newMap) 
        {
            switch (obj.Type)
            {
                case MapObjectType.Robot:
                    {
                        newMap.Set(obj.Points.Single(), '@');
                    }
                    break;
                case MapObjectType.Crate:
                    {
                        newMap.Set(obj.Points.First(), '[');
                        newMap.Set(obj.Points.Last(), ']');
                    }
                    break;
                case MapObjectType.Empty:
                    {
                        newMap.Set(obj.Points.Single(), '.');
                    }
                    break;
                case MapObjectType.Wall:
                    {
                        newMap.Set(obj.Points.Single(), '#');
                    }
                    break;
                default:
                    throw new NotImplementedException();

            };
        }
    }
}
