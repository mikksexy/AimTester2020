using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;

public class AimTester2020 : PhysicsGame
{

    GameObject kursori;
    GameObject vihu;
    Timer ajastin;
    int tuhottuja = 0; // Tuhottujen vihujen määrä
    int eraNumero = 0;
    const int pelinLeveys = 800;
    const int pelinKorkeus = 600;
    public override void Begin()
    {
        SetWindowSize(pelinLeveys, pelinKorkeus, false);

        Level.Background.Color = Color.Black;

        Camera.ZoomToLevel();

        AloitaPeli();


    }


    public void AloitaPeli()
    {
        LuoKursori();

        ajastin = new Timer();
        Era();
        ajastin.Timeout += LuoVihu;
        ajastin.Start();

        Mouse.Listen(MouseButton.Left, ButtonState.Pressed, delegate ()
        {
            if (Vector.Distance(kursori.Position, vihu.Position) < vihu.Width / 2) // Jos kursori ja vihu ovat päällekkäin
            {
                vihu.Destroy();
                tuhottuja++;
                Era(); // Kutsuu ohjelmaa, jotta voidaan tarkistaa tarvitseeko vihuja spawnata nopeampaa
            }
        }, "Tuhoaa olion");

        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }
    

    public void LuoKursori()
    {
        kursori = new GameObject(50, 50);
        kursori.Shape = Shape.Triangle;
        kursori.Color = Color.Green;
        Add(kursori);
        Mouse.ListenMovement(0, delegate()
        {
            kursori.Position = Mouse.PositionOnWorld;
        }, "Liikuttaa hiirtä");
        kursori.Position = Mouse.PositionOnScreen;
    }


    public void LuoVihu()
    {
        vihu = new GameObject(50, 50);
        vihu.Shape = Shape.Circle;
        vihu.Color = Color.Yellow;
        vihu.X = RandomGen.NextDouble(-pelinLeveys / 2, pelinLeveys / 2);
        vihu.Y = RandomGen.NextDouble(-pelinKorkeus / 2, pelinKorkeus / 2);
        //vihu.Position = RandomGen.NextVector(pelinLeveys, pelinKorkeus);
        Add(vihu);
    }

    /// <summary>
    /// Aliohjelma tarkistaa onko eriä jäljellä ja onko erä suoritettu eli 10 vihua tuhottua.
    /// </summary>
    public void Era()
    {
        double[] era = { 2.0, 6.0, 1.0, 4.0, 3.0, 2.5, 2.0, 1.5, 1.2, 1.0 };
        if (eraNumero == era.Length) Lopetus(); // Jos kaikki erät suoritettu kutsuu Lopetus -aliohjelmaa
        if (tuhottuja % 10 == 0) // Jos erä suoritettu 
        {
            ajastin.Interval = era[eraNumero]; // hakee taulukosta uuden eränopeuden
            ajastin.Reset();
            ajastin.Start(); // käynnistää ajastimen uudella nopeudella
            eraNumero++; // päivittää eränumeron
        }
    }


    public void Lopetus()
    {
        ajastin.Reset();
        Label voititPelin = new Label("Voitit pelin!");
        Add(voititPelin);
    }
}
