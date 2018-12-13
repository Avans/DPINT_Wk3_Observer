using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DPINT_Wk3_Observer.Model
{
    public class Baggageband : Observable<Baggageband>
    {
        public string Naam { get => _naam; set { _naam = value; Notify(this); } }
        private int _aantalKoffersPerMinuut;
        public int AantalKoffers { get => _aantalKoffers; set { _aantalKoffers = value; Notify(this); } }
        public string VluchtVertrokkenVanuit { get => _vluchtVertrokkenVanuit; set { _vluchtVertrokkenVanuit = value; Notify(this); } }

        private Timer _huidigeVluchtTimer;
        private string _vluchtVertrokkenVanuit;
        private int _aantalKoffers;
        private string _naam;

        public Baggageband(string naam, int aantalKoffersPerMinuut)
        {
            Naam = naam;
            _aantalKoffersPerMinuut = aantalKoffersPerMinuut;
        }
        public bool IsBezigMetVluchtAfhandelen()
        {
            return _aantalKoffers > 0;
        }
        public void HandelNieuweVluchtAf(Vlucht vlucht)
        {
            if (IsBezigMetVluchtAfhandelen())
            {
                throw new InvalidOperationException();
            }
            VluchtVertrokkenVanuit = vlucht.VertrokkenVanuit;
            AantalKoffers = vlucht.AantalKoffers;

            if (_huidigeVluchtTimer != null)
            {
                _huidigeVluchtTimer.Stop();
            }

            _huidigeVluchtTimer = new Timer
            {
                Interval = (int)((60.0 / _aantalKoffersPerMinuut) * 1000)
            };
            _huidigeVluchtTimer.Tick += KofferVanBandGehaald;

            _huidigeVluchtTimer.Start();

            base.Notify(this);
        }

        private void KofferVanBandGehaald(object sender, EventArgs e)
        {
            AantalKoffers--;

            if (AantalKoffers == 0)
            {
                VluchtVertrokkenVanuit = null;
                _huidigeVluchtTimer.Stop();
            }

            base.Notify(this);
        }
    }
}
