using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPINT_Wk3_Observer.Model
{
    public class Aankomsthal : IObserver<Baggageband>
    {
        // TODO: Hier een ObservableCollection van maken, dan weten we wanneer er vluchten bij de wachtrij bij komen of afgaan.
        public ObservableCollection<Vlucht> WachtendeVluchten { get; private set; }
        public ObservableCollection<Baggageband> Baggagebanden { get; private set; }

        public Aankomsthal()
        {
            WachtendeVluchten = new ObservableCollection<Vlucht>();
            Baggagebanden = new ObservableCollection<Baggageband>
            {
                // TODO: Als baggageband Observable is, gaan we subscriben op band 1 zodat we updates binnenkrijgen.
                new Baggageband("Band 1", 30),
                // TODO: Als baggageband Observable is, gaan we subscriben op band 2 zodat we updates binnenkrijgen.
                new Baggageband("Band 2", 60),
                // TODO: Als baggageband Observable is, gaan we subscriben op band 3 zodat we updates binnenkrijgen.
                new Baggageband("Band 3", 90)
            };

            foreach (var band in Baggagebanden)
            {
                band.Subscribe(this);
                OnNext(band);
            }
        }

        public void NieuweInkomendeVlucht(string vertrokkenVanuit, int aantalKoffers)
        {
            // TODO: Het proces moet straks automatisch gaan, dus als er lege banden zijn moet de vlucht niet in de wachtrij.
            // Dan moet de vlucht meteen naar die band.

            // Denk bijvoorbeeld aan: Baggageband legeBand = Baggagebanden.FirstOrDefault(b => b.AantalKoffers == 0);
            Baggageband legeBand = Baggagebanden.FirstOrDefault(b => b.AantalKoffers == 0);
            if (legeBand != null)
            {
                legeBand.HandelNieuweVluchtAf(new Vlucht(vertrokkenVanuit, aantalKoffers));
            }
            else
            {
                WachtendeVluchten.Add(new Vlucht(vertrokkenVanuit, aantalKoffers));
            }
        }

        public void OnNext(Baggageband value)
        {
            if (value.AantalKoffers <= 0 && WachtendeVluchten.Count > 0)
            {
                Baggageband legeBand = value;
                Vlucht volgendeVlucht = WachtendeVluchten.FirstOrDefault();
                WachtendeVluchten.RemoveAt(0);

                legeBand.HandelNieuweVluchtAf(volgendeVlucht);
            }
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }
    }
}
