using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;


/// @author  Sami Mikkola
/// @version 11.03.2020
/// <summary>
/// Peli, jossa vihuja (palloja) tuhotaan klikkailemalla kursorilla niiden päältä mahdollisimman nopeaa.
/// </summary>
public class AimTester2020 : PhysicsGame
{

    GameObject kursori;
    GameObject vihu;
    Timer vihuAjastin;
    IntMeter eraNumero;
    int tuhottuja; // Tuhottujen vihujen määrä
    int vihuMaara;
    const int pelinLeveys = 800;
    const int pelinKorkeus = 600;
    public override void Begin()
    {
        SetWindowSize(pelinLeveys, pelinKorkeus, false);

        Level.Background.Color = Color.Black;

        Camera.ZoomToLevel();

        MultiSelectWindow alkuValikko = new MultiSelectWindow("AimTester2020", "Aloita peli", "Lopeta");
        Add(alkuValikko);
        alkuValikko.AddItemHandler(0, AloitaPeli);
        alkuValikko.AddItemHandler(1, ConfirmExit);



    }

    /// <summary>
    /// Aliohjelma aloittaa pelin eli aloittaa luo kursorin ja aloittaa vihujen spawnausajastimen
    /// </summary>
    public void AloitaPeli()
    {
        while (vihuMaara != 0)
        {
            vihu.Destroy();
            vihuMaara -= 1;
        }
        tuhottuja = 0;
        vihuMaara = 0;
        LuoKursori();

        LuoEraLaskuri();

        vihuAjastin = new Timer();
        Era();
        vihuAjastin.Timeout += LuoVihu;
        //vihuAjastin.Start();

        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }
    
    /// <summary>
    /// Aliohjelma luo kursorin ja määrittää samalla hiiren toiminnot
    /// </summary>
    public void LuoKursori()
    {
        kursori = new GameObject(50, 50);
        kursori.Shape = Shape.Triangle;
        kursori.Color = Color.Green;
        Add(kursori);

        Mouse.ListenMovement(0, delegate()
        {
            kursori.Position = Mouse.PositionOnWorld;
        }, "Hiiren liike liikuttaa kursoria");
        kursori.Position = Mouse.PositionOnScreen;

        Mouse.Listen(MouseButton.Left, ButtonState.Pressed, delegate ()
        {
            if (Vector.Distance(kursori.Position, vihu.Position) < vihu.Width / 2) // Jos kursori ja vihu ovat päällekkäin
            {
                vihu.Destroy();
                tuhottuja += 1;
                vihuMaara -= 1;
                Era(); // Kutsuu ohjelmaa, jotta voidaan tarkistaa tarvitseeko vihuja spawnata nopeampaa
            }
        }, "Hiiren vasenta painettaessa tuhoaa olion, jos ehto täyttyy");
    }


    public void LuoEraLaskuri()
    {
        eraNumero = new IntMeter(0);

        Label pisteNaytto = new Label();
        pisteNaytto.X = Screen.Left + 100;
        pisteNaytto.Y = Screen.Top - 100;
        pisteNaytto.TextColor = Color.Black;
        pisteNaytto.Color = Color.White;

        pisteNaytto.BindTo(eraNumero);
        Add(pisteNaytto);
    }


    /// <summary>
    /// Aliohjelma luo vihun eli objektin, joka pelissä halutaan tuhota.
    /// Samalla määrittää vihun ominaisuudet
    /// </summary>
    public void LuoVihu()
    {
        if (vihuMaara > 0)
        {
            MultiSelectWindow havisitPelin = new MultiSelectWindow("HÄHÄÄ!\nHävisit pelin", "Pelaa uudestaan", "Lopeta");
            Lopetus(havisitPelin);
        }
        vihu = new GameObject(50, 50);
        vihu.Shape = Shape.Circle;
        vihu.Color = Color.Yellow;
        vihu.X = RandomGen.NextDouble(-pelinLeveys / 2, pelinLeveys / 2);
        vihu.Y = RandomGen.NextDouble(-pelinKorkeus / 2, pelinKorkeus / 2);
        Add(vihu);
        vihuMaara += 1;
    }

    /// <summary>
    /// Aliohjelma tarkistaa onko eriä jäljellä ja onko erä suoritettu eli 10 vihua tuhottuna.
    /// </summary>
    public void Era()
    {
        double[] eraNopeus = { /*2.0, 6.0, 1.0, 4.0, 3.0, 2.5, */2.0, 1.5, 1.2, 1.0 };
        if (tuhottuja % 10 == 0) // Jos erä suoritettu 
        {
            vihuAjastin.Reset();
            vihuAjastin.Interval = eraNopeus[eraNumero]; // hakee taulukosta uuden eränopeuden
            vihuAjastin.Start(); // käynnistää ajastimen uudella nopeudella
            eraNumero.Value += 1; // päivittää eränumeron
        }
        if (eraNumero == eraNopeus.Length)
        {
            MultiSelectWindow lopetusValikko = new MultiSelectWindow("ONNITTELUT!\nLäpäisit kohdistamistestin", "Pelaa uudestaan", "Lopeta");
            Lopetus(lopetusValikko); // Jos kaikki erät suoritettu kutsuu Lopetus -aliohjelmaa
        }

    }

    /// <summary>
    /// Aliohjelma lopettaa vihujen spawnaamisen ja avaa valikon,
    /// jossa onnitellaan pelin läpäisemisestä sekä tarjotaan
    /// mahdollisuus pelata uudestaan tai lopettaa peli.
    /// </summary>
    public void Lopetus(MultiSelectWindow valikko)
    {
        vihuAjastin.Stop(); // Pysäyttää vihujen spawnaamisen
        vihu.Destroy();
        //vihuMaara -= 1;
        kursori.Destroy();
        tuhottuja = 0;
        eraNumero.Reset();
        //eraNumero.Stop();
        Add(valikko);
        valikko.AddItemHandler(0, Begin);
        valikko.AddItemHandler(1, Exit);
    }
}