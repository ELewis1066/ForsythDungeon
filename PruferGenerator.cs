using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    /// <summary>
    /// Uses a Prufer sequence to generator a random dungeon ( a random graph, that we convert into dungeon map).
    /// </summary>
    public class PruferGenerator
    {
        class Edge
        {
            public Room From { get; set; }
            public Room To { get; set; }

            public Edge(Room from, Room to)
            {
                From = from;
                To = to;
            }
        }

        private static readonly Dictionary<string, string> PossibleDirections = new Dictionary<string, string>()
        {
            { "north", "south" },
            { "south", "north" },
            { "west", "east" },
            { "east", "west" }
        };

        private readonly Random Random;
        private List<Room> _sequence;

        public PruferGenerator(int hashCode)
        {
            Random = new Random(hashCode);
        }

        public PruferGenerator()
        {
            Random = new Random(Guid.NewGuid().GetHashCode());
        }

        public Room Generate(List<Room> rooms)
        {
            bool satisfied = false;
            Room start = rooms[^1]; // start is last room in the list.

            while (!satisfied)
            {
                List<Room> trial = new List<Room>();
                foreach (Room room in rooms)
                {
                    trial.Add(room);
                }

                _sequence = new List<Room>();
                int n = trial.Count;
                int m = n - 2;

                /*
                 * Construct a random Prufer sequence.
                 * Sequence of length m, each entry is random value [0,n).
                 */
                for (int i = 0; i < m; i++)
                {
                    _sequence.Add(rooms[Random.Next(0, n)]);
                }

                List<Edge> edges = new List<Edge>();
                while (_sequence.Count > 0)
                {
                    var found = trial.Find(room => !_sequence.Contains(room));
                    if (found != null)
                    {
                        edges.Add(new Edge(_sequence[0], found));
                        trial.Remove(found);
                        _sequence.RemoveAt(0);
                    }
                }
                Debug.Assert(trial.Count == 2); /* We should be left with 2 vertices. */

                edges.Add(new Edge(trial[1], trial[0]));

                /*
                 * I need to add a check that we don't have a vertex with more than 4 edges.
                 * If we do, then re-run with new random sequence, a maximum number of times?
                 *
                 * So, what is Mathematics? How to ensure (or can we) that we don't have
                 * a vertex with more than 4 edges. Better if we use n,s,nw,ne,sw,se,w,e?
                 *
                 */
                satisfied = true;

                /* Make connections from edges. */
                foreach (var edge in edges)
                {
                    var connectionsFrom = edge.From.GetConnections().Select(connection => connection.GetDirection());
                    var connectionsTo = edge.To.GetConnections().Select(connection => connection.GetDirection());
                    foreach (var direction in PossibleDirections)
                    {
                        if (connectionsFrom.Contains(direction.Key) || connectionsTo.Contains(direction.Value))
                            continue;
                        Connection.MakeConnection(edge.From, edge.To, direction.Key);
                        break;
                    }
                }
            }

            return start;
        }
    }
}
