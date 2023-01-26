using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace furdostat
{
    class beolvas
    {
        #region adatok
        /// <summary>
        /// Vendégek azonosítója
        /// </summary>
        public int vAzonosito { get; private set; }
        /// <summary>
        /// Részleg azonosítója
        /// </summary>
        public int rAzonosito { get; private set; }
        /// <summary>
        /// Megmutatja, hogy a vendég kifelé(0) vagy befelé(1) ment a részlegre 
        /// </summary>
        public int beKi { get; private set; }
        /// <summary>
        /// Megadja a kért pontos időt szöveggel (string formátumba)
        /// </summary>
        public string pontosIdo { get; private set; }
        /// <summary>
        /// Megadja a pontos időt másodpercben
        /// </summary>
        public int idoPont { get; private set; }
        #endregion

        #region konstruktor
        public beolvas(string sor)
        {
            //A beolvasott sort külön adatokra szedi
            string[] adatok = sor.Split(' ');
            vAzonosito = int.Parse(adatok[0]);
            rAzonosito = int.Parse(adatok[1]);
            beKi = int.Parse(adatok[2]);
            //Idő meghatározása mp-ben a "pontosIdo" függvénnyel
            idoPont = idoMP(int.Parse(adatok[3]), int.Parse(adatok[4]), int.Parse(adatok[5]));
            //Pontos idő meghatározása
            pontosIdo = idoPontos(adatok[3], adatok[4], adatok[5]);

        }
        #endregion

        #region Időváltó
        //Megadja a pontos idő szöveggel
        private string idoPontos(string h, string m, string mp)
        {
            return ($"{h} óra {m} perc {mp} másodperc");
        }
        //Megadja a pontos időt másodpercben
        private int idoMP(int h, int m, int mp)
        {
            return h * 60 * 60 + m * 60 + mp * 60;
        }
        #endregion
    }

    class vendeg
    {
        #region VendegAdatok
        public int azVendeg { get; private set; }
        public int bemegy { get; private set; }
        public int kimegy { get; private set; }
        #endregion
        #region Vendegkonstruktor
        public vendeg(int az, int be, int ki)
        {
            azVendeg = az;
            bemegy = be;
            kimegy = ki;
        }
        #endregion
        class Program
        {
            #region IdőVisszaváltó
            public static string Idovalto(int mp)
            {
                int h = mp / 60 / 60;
                int m = (mp - h * 60 * 60) / 60;
                mp = mp - (h * 60 * 60 + m * 60);
                return ($"{h}:{m}:{mp}");
            }
            #endregion
            static void Main(string[] args)
            {
                #region fájl beolvasása
                //Fájl beolvasása
                StreamReader beolvasas = new StreamReader("furdoadat.txt");
                //Lista a fájl sorainak tárolásásra beolvasás után
                List<beolvas> adatok = new List<beolvas>();
                //Összes sor hozzáadás a listához
                while (!beolvasas.EndOfStream)
                {
                    adatok.Add(new beolvas(beolvasas.ReadLine()));
                }
                #endregion

                #region 2.feladat
                Console.WriteLine("Másidik feldat:");
                Console.WriteLine($"Az első vendég {adatok[0].pontosIdo}kor lépett ki az öltözőből");
                for (int i = adatok.Count - 1; i > 0; i--)
                {
                    if (adatok[i].rAzonosito == 0 && adatok[i].beKi == 1)
                    {
                        Console.WriteLine($"Az utolsó vendég {adatok[i].pontosIdo}kor lépett ki az öltözőből");
                        break;
                    }
                }
                #endregion

                #region 3.feladat
                Console.WriteLine("Harmadik feladat");
                //Csak egy részlegen járt vendégek száma
                int csakEgy = 0;

                //Lista a vendégek neveivel
                List<int> vendegek = new List<int>();
                for (int i = 0; i < adatok.Count; i++)
                {
                    if (vendegek.Contains(adatok[i].vAzonosito) == false)
                    {
                        vendegek.Add(adatok[i].vAzonosito);
                    }
                }

                //Átnézi a teljes vendék listát, hogy....
                for (int i = 0; i < vendegek.Count - 1; i++)
                {
                    int szamolo = 0;
                    //Hányszor szerepelnek az egyes nevek az eredeti listában
                    for (int k = 0; k < adatok.Count; k++)
                    {
                        if (vendegek[i] == adatok[k].vAzonosito)
                        {
                            szamolo++;
                        }
                    }
                    //Ha négyszer szerepel valaki, azt jelenti, hogy csak 1 részlegen volt, az öltözőn kívül
                    if (szamolo == 4)
                    {
                        csakEgy++;
                    }
                }

                Console.WriteLine($"A fürdőben {csakEgy} vendég járt csak egy részlegen");
                #endregion

                Console.WriteLine("Negyedik feladat");
                #region A vendégek teljes listája
                List<int> be = new List<int>();
                List<int> ki = new List<int>();
                for (int i = 0; i < vendegek.Count; i++)
                {
                    for (int k = 0; k < adatok.Count; k++)
                    {
                        if (vendegek[i] == adatok[k].vAzonosito && adatok[k].rAzonosito == 0 && adatok[k].beKi == 1)
                        {
                            ki.Add(adatok[k].idoPont);
                        }
                        if (vendegek[i] == adatok[k].vAzonosito && adatok[k].rAzonosito == 0 && adatok[k].beKi == 0)
                        {
                            be.Add(adatok[k].idoPont);
                        }

                    }
                }
                List<vendeg> vendeglista = new List<vendeg>();
                for (int i = 0; i < vendegek.Count; i++)
                {
                    vendeglista.Add(new vendeg(vendegek[i], be[i], ki[i]));
                }
                #endregion

                int max = 0;
                int index = 0;
                for (int i = 0; i < vendeglista.Count; i++)
                {
                    if ((vendeglista[i].bemegy-vendeglista[i].kimegy) > max)
                    {
                        max = vendeglista[i].bemegy - vendeglista[i].kimegy;
                        index = i;
                    }
                }
                string legtobb = Idovalto(max);
                Console.WriteLine($"A {vendeglista[index].azVendeg} vendég tartózkodott a legtovább. ideje {legtobb}");
            }
        }
    }
}

