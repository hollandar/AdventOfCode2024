using Command.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Command.Problems._2024;

record Computer(string name)
{
    public List<Connection> connections { get; } = new();

    public bool IsConnectedTo(Computer otherComputer)
    {
        return connections.Any(r => r.OtherComputer(this) == otherComputer);
    }

    public IEnumerable<Computer> ConnectedTo()
    {
        return connections.Select(r => r.OtherComputer(this));
    }

    public override string ToString()
    {
        return name;
    }
}

record Connection(Computer from, Computer to)
{
    public Computer OtherComputer(Computer computer)
    {
        return computer == from ? to : from;
    }
}

public partial class LANParty : ProblemBase<string>
{
    HashSet<Computer> computers = new();
    HashSet<Connection> connections = new();
    public LANParty()
    {
    }

    protected override void Line(string line)
    {
        var aName = line.Split("-")[0];
        var bName = line.Split("-")[1];
        var computerA = computers.Where(r => r.name == aName).FirstOrDefault() ?? new Computer(aName);
        var computerB = computers.Where(r => r.name == bName).FirstOrDefault() ?? new Computer(bName);
        computers.Add(computerA);
        computers.Add(computerB);
        var connection = new Connection(computerA, computerB);
        computerA.connections.Add(connection);
        computerB.connections.Add(connection);
    }

    public override string CalculateOne(bool exampleData)
    {
        HashSet<Computer> visitedComputers = new();
        HashSet<(Computer, Computer, Computer)> triplets = new();
        foreach (var computer in computers)
        {
            if (visitedComputers.Contains(computer))
                continue;

            foreach (var triplet in ConnectedTriplets(computer))
            {
                triplets.Add(triplet);
            }
        }

        var count = 0;
        foreach (var triplet in triplets.OrderBy(r => r.Item1.name))
        {
            if (triplet.Item1.name.StartsWith("t") || triplet.Item2.name.StartsWith("t") || triplet.Item3.name.StartsWith("t"))
            {
                count++;
            }
        }

        return count.ToString();
    }

    public override string CalculateTwo(bool exampleData)
    {
        string largestSet = String.Empty;
        int largestSetSize = 0;
        foreach (var computer in computers)
        {
            var interconnected = InterconnectedTo(computer);
            var setName = String.Join(",", interconnected.Select(r => r.name).OrderBy(r => r));

            if (setName.Length > largestSetSize)
            {
                largestSet = setName;
                largestSetSize = setName.Length;
            }
        }

        return largestSet;
    }

    Computer[] InterconnectedTo(Computer computer)
    {
        List<Computer> computers = new List<Computer>([computer]);
        HashSet<Computer> visitedComputers = new HashSet<Computer>([computer]);
        Queue<Computer> queue = new Queue<Computer>([computer]);
        while (queue.Any())
        {
            var thisComputer = queue.Dequeue();
            var connections = computer.ConnectedTo();
            foreach (var connected in connections)
            {
                if (!visitedComputers.Contains(connected))
                {
                    visitedComputers.Add(connected);
                    queue.Enqueue(connected);
                }

                bool interconnected = true;
                foreach (var connectedComputer in computers)
                {
                    if (!connected.IsConnectedTo(connectedComputer))
                        interconnected = false;
                }
                if (interconnected)
                    computers.Add(connected);
            }

        }
        return computers.ToArray();
    }

    IEnumerable<(Computer, Computer, Computer)> ConnectedTriplets(Computer computer)
    {
        var otherComputers = computer.ConnectedTo();

        foreach (var otherComputer in otherComputers)
        {
            var thirdComputers = otherComputer.ConnectedTo().Where(r => r != computer);

            foreach (var thirdComputer in thirdComputers)
            {
                if (computer.IsConnectedTo(otherComputer) && otherComputer.IsConnectedTo(thirdComputer) && thirdComputer.IsConnectedTo(computer))
                {
                    Computer[] computers = [computer, otherComputer, thirdComputer];
                    var orderedComputers = computers.OrderBy(r => r.name).ToArray();
                    yield return (orderedComputers[0], orderedComputers[1], orderedComputers[2]);
                }
            }
        }
    }
}
