using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace streetmapping
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Vertex> _vertices = new List<Vertex>();

        private List<Vertex> _Q = new List<Vertex>();
        private Dictionary<Vertex, Vertex> _P = new Dictionary<Vertex, Vertex>();
        private Dictionary<Vertex, double> _D = new Dictionary<Vertex, double>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(@"ur.txt");

                if (lines == null)
                    return;
                string[] values = null;
                foreach (string line in lines)
                {
                    values = line.Split('\t');
                    if (values.Length != 4) continue;

                    if (values[0] == "i")
                    {
                        string location = values[1];
                        double latitude = Convert.ToDouble(values[2]);
                        double longitude = Convert.ToDouble(values[3]);

                        _vertices.Add(new Vertex()
                        {
                            name = location.ToUpper(),
                            coordinates = new System.Device.Location.GeoCoordinate(latitude, longitude),
                            adjList = new List<Vertex>()
                        });
                    }
                    else if (values[0] == "r")
                    {
                        string location1 = values[2].ToUpper();
                        string location2 = values[3].ToUpper();

                        Vertex v1 = _vertices.Find(x => x.name == location1);
                        Vertex v2 = _vertices.Find(x => x.name == location2);

                        if (v1 != null && v2 != null)
                        {
                            v1.adjList.Add(v2);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void distbutton_Click(object sender, RoutedEventArgs e)
        {
            string s = txtstart.Text;
            string d = txtend.Text;

            if (string.IsNullOrWhiteSpace(s) || string.IsNullOrWhiteSpace(d))
            {
                MessageBox.Show("source or destination city not given");
                return;
            }

            s = s.ToUpper();
            d = d.ToUpper();

            Vertex vStart = _vertices.Find(x => x.name == s);
            Vertex vEnd = _vertices.Find(x => x.name == d);

            if (vStart == null || vEnd == null)
            {
                MessageBox.Show("source or destination city not found");
                return;
            }

            CalcShortDist(vStart, vEnd);
        }

        private void ShowPath(Vertex vStart, Vertex vEnd)
        {
            Vertex v = vEnd;
            List<string> path = new List<string>();
           
            while(_P[v] != null)
            {
                path.Add(_P[v].name);
                v = _P[v];
            }

            for(int i = path.Count()-1; i >=0; --i)
            {
                txtanswer.Text += path[i];
                txtanswer.Text += Environment.NewLine;
            }
        }

        private Vertex GetNext()
        {
            double smallestdist = double.PositiveInfinity;
            Vertex vReturn = null;
            foreach (Vertex v in _Q)
            {
                if (_D[v] < smallestdist)
                {
                    smallestdist = _D[v];
                    vReturn = v;
                }
            }
            return vReturn;
        }

        private void CalcShortDist(Vertex vStart, Vertex vEnd)
        {
            txtanswer.Text = "";

            foreach (Vertex v in _vertices)
            {
                _Q.Add(v);
                _D[v] = double.PositiveInfinity;
                _P[v] = null;
            }

            _D[vStart] = 0;

            while(_Q.Count() > 0)
            {
                Vertex v = GetNext();
                if (v == null)
                    break;
                _Q.Remove(v);

                foreach(Vertex adjV in v.adjList)
                {
                    if (_Q.Contains(adjV))
                    {
                        double distFromV = v.coordinates.GetDistanceTo(adjV.coordinates);
                        if (_D[v] + distFromV < _D[adjV])
                        {
                            _D[adjV] = _D[v] + distFromV;
                            _P[adjV] = v;
                        }
                    }
                }
            }
            ShowPath(vStart, vEnd);
        }
    }
}
