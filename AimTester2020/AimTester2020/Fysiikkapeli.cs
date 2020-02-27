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
    //Timer ajastin;
    public override void Begin()
    {
        LuoKursori();
        AsetaOhjaimet();
        NopeutaEraa();

        SetWindowSize(1024, 768, false);

        Level.Background.Color = Color.Black;

        Camera.ZoomToLevel();
    }
    

    public void LuoKursori()
    {
        kursori = new GameObject(50, 50);
        kursori.Shape = Shape.Triangle;
        kursori.Color = Color.Green;
        Add(kursori);
        Mouse.ListenMovement(0, LiikutaKursoria, "Liikuttaa hiirtä");
        kursori.Position = Mouse.PositionOnScreen;
    }


    void LiikutaKursoria()
    {
        kursori.Position = Mouse.PositionOnWorld;
    }


    public void LuoVihu()
    {
        vihu = new GameObject(50, 50);
        vihu.Shape = Shape.Circle;
        vihu.Color = Color.Yellow;
        vihu.X = RandomGen.NextDouble(-512, 512);
        vihu.Y = RandomGen.NextDouble(-384, 384);
        Add(vihu);
    }


    public void NopeutaEraa()
    {
        double[] era = { 2.0, 6.0, 5.0, 4.0, 3.0, 2.5, 2.0, 1.5, 1.2, 1.0 };
        for (int i = 0; i < era.Length; i++)
        {
            /*ajastin = new Timer();
            ajastin.Interval = era[i];
            ajastin.Timeout += LuoVihu;
            ajastin.Start();
            ajastin.Reset();*/
            Timer.CreateAndStart(era[i], LuoVihu);
        }
    }


    void AsetaOhjaimet()
    {

        Mouse.Listen(MouseButton.Left, ButtonState.Pressed, Tuhoa, "Tuhoaa olion");

        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }


    void Tuhoa()
    {
        if (Vector.Distance(kursori.Position, vihu.Position) < vihu.Width / 2)
            // on päällekkäistä aluetta
            {
            vihu.Destroy();
            }
    }
}
