using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;


/// @author  Sami Mikkola
/// @version 15.04.2020
/// <summary>
/// Peli, jossa vihuja (palloja) tuhotaan klikkailemalla kursorilla niiden päältä mahdollisimman nopeasti.
/// </summary>
/// TODO: funktio + silmukka, ks: https://tim.jyu.fi/view/kurssit/tie/ohj1/2020k/demot/demo9-tutkimus?answerNumber=3&b=qNc4xpHaic24&size=1&task=D9T1&user=mikksexy
public class AimTester2020 : PhysicsGame
{
    private GameObject kursori;
    private GameObject vihu;
    private Timer vihuAjastin;
    private IntMeter eraNumero;
    private IntMeter tuhottuja; // Tuhottujen vihujen määrä
    private IntMeter vihuMaara;

    private const int PELIN_LEVEYS = 800;
    private const int PELIN_KORKEUS = 600;


    /// <summary>
    /// Aloitusruutu
    /// </summary>
    public override void Begin()
    {
        Valikot(1);
        Level.Background.Color = Color.Black;
        SetWindowSize(PELIN_LEVEYS, PELIN_KORKEUS, false);
    }


    /// <summary>
    /// Käynnistää halutun valikon
    /// </summary>
    /// <param name="caseNumero">case numero</param>
    private void Valikot(int caseNumero)
    {
        switch (caseNumero)
        {
            case 1:
                MultiSelectWindow alkuValikko = new MultiSelectWindow("AimTester2020", "Aloita peli", "Lopeta");
                Add(alkuValikko);
                alkuValikko.AddItemHandler(0, AloitaPeli);
                alkuValikko.AddItemHandler(1, ConfirmExit);
                break;

            case 2:
                ClearAll();
                MultiSelectWindow havisitPelin = new MultiSelectWindow("HÄHÄÄ!\nHävisit pelin", "Pelaa uudestaan", "Lopeta");
                Add(havisitPelin);
                havisitPelin.AddItemHandler(0, AloitaPeli);
                havisitPelin.AddItemHandler(1, Exit);
                break;

            case 3:
                ClearAll();
                MultiSelectWindow lopetusValikko = new MultiSelectWindow("ONNITTELUT!\nLäpäisit kohdistamistestin", "Pelaa uudestaan", "Lopeta");
                Add(lopetusValikko);
                lopetusValikko.AddItemHandler(0, AloitaPeli);
                lopetusValikko.AddItemHandler(1, Exit);
                break;
        }
    }


    /// <summary>
    /// Aliohjelma aloittaa pelin eli aloittaa luo kursorin ja aloittaa vihujen spawnausajastimen
    /// </summary>
    private void AloitaPeli()
    {
        ClearAll();

        Camera.ZoomToLevel();

        vihuMaara = new IntMeter(0);

        LuoKursori();

        LuoEralaskuri();

        vihuAjastin = new Timer();
        tuhottuja = new IntMeter(0);
        vihuAjastin.Timeout += LuoVihu;
        Era();

        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }
    

    /// <summary>
    /// Aliohjelma luo kursorin ja määrittää samalla hiiren toiminnot
    /// </summary>
    private void LuoKursori()
    {
        kursori = new GameObject(50, 50);
        kursori.Shape = Shape.Triangle;
        kursori.Color = Color.Green;
        Add(kursori);

        Mouse.ListenMovement(0, delegate()
        {
            kursori.Position = Mouse.PositionOnWorld;
        }, "Hiiren liike liikuttaa kursoria");

        Mouse.Listen(MouseButton.Left, ButtonState.Pressed, delegate ()
        {
            if (Vector.Distance(kursori.Position, vihu.Position) < vihu.Width / 2) // Jos kursori ja vihu ovat päällekkäin
            {
                vihu.Destroy();
                tuhottuja.Value += 1;
                vihuMaara.Value -= 1;
                Era(); // Kutsuu ohjelmaa, jotta voidaan tarkistaa tarvitseeko vihuja spawnata nopeampaa
            }
        }, "Hiiren vasenta painettaessa tuhoaa olion, jos ehto täyttyy");
    }


    /// <summary>
    /// Luo eriä laskevan laskurin
    /// </summary>
    private void LuoEralaskuri()
    {
        eraNumero = new IntMeter(0);

        Label eralaskuri = new Label();
        eralaskuri.X = Screen.Left + 100;
        eralaskuri.Y = Screen.Top - 100;
        eralaskuri.TextColor = Color.Black;
        eralaskuri.Color = Color.White;
        eralaskuri.BindTo(eraNumero);
        eralaskuri.IntFormatString = "Erä {0:D1}";
        Add(eralaskuri);
    }


    /// <summary>
    /// Aliohjelma luo vihun eli objektin, joka pelissä halutaan tuhota.
    /// Samalla määrittää vihun ominaisuudet
    /// </summary>
    private void LuoVihu()
    {
        if (vihuMaara > 0) // Jos kentällä on vihuja enemmän kuin yksi, peli hävitään.
        {
            Valikot(2);
        }
        vihu = new GameObject(50, 50);
        vihu.Shape = Shape.Circle;
        vihu.Color = Color.Yellow;
        vihu.X = RandomGen.NextDouble(-PELIN_LEVEYS / 2, PELIN_LEVEYS / 2);
        vihu.Y = RandomGen.NextDouble(-PELIN_KORKEUS / 2, PELIN_KORKEUS / 2);
        Add(vihu);
        vihuMaara.Value += 1;
    }


    /// <summary>
    /// Aliohjelma tarkistaa onko eriä jäljellä ja onko erä suoritettu eli 10 vihua tuhottuna.
    /// </summary>
    private void Era()
    {
        double[] eraNopeus = { 2.0, 1.5, 1.2, 1.0 };
        if (tuhottuja % 10 == 0) // Jos erä suoritettu
        {
            vihuAjastin.Reset();
            vihuAjastin.Interval = eraNopeus[eraNumero]; // hakee taulukosta uuden eränopeuden
            vihuAjastin.Start(); // käynnistää ajastimen uudella nopeudella
            eraNumero.Value += 1; // päivittää eränumeron
        }
        if (eraNumero == eraNopeus.Length) // Jos eränumero on sama kuin määritettyjen erien määrä, peli voitetaan.
        {
            Valikot(3);
        }
    }
}