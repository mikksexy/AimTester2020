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
    public override void Begin()
    {
        LuoKursori();
        NopeutaErää();
        AsetaOhjaimet();

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


    public void LuoVihu()
    {
        vihu = new GameObject(50, 50);
        vihu.Shape = Shape.Circle;
        vihu.Color = Color.Yellow;
        vihu.X = RandomGen.NextDouble(-512, 512);
        vihu.Y = RandomGen.NextDouble(-384, 384);
        Add(vihu);
    }


    /*public static void EränNopeus()
    {
        double[] erä = new double[10];
        erä[0] = 2;
        erä[1] = 1.5;
        erä[2] = 1.25;
        erä[3] = 1.1;
        erä[4] = 1.0;
        erä[5] = 0.9;
        erä[6] = 0.8;
        erä[7] = 0.7;
        erä[8] = 0.6;
        erä[9] = 0.5;
    }*/

    public void NopeutaErää()
    {
        for (double i = 5; i > 0.1; i -= 0.2)
        {
            Timer ajastin = new Timer();
            ajastin.Interval = i;
            ajastin.Timeout += LuoVihu;
            ajastin.Start();
            /*for (int u = 0; 10 > u; u++)
            {

            }*/
        }
    }
    void AsetaOhjaimet()
    {

        //Mouse.Listen(MouseButton.Left, ButtonState.Pressed, Tuhoa, "Tuhoaa olion");

        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }


    void LiikutaKursoria()
    {
        kursori.Position = Mouse.PositionOnWorld;
    }
    /*void Tuhoa()
    {
        if (kursori.Position == vihu.Position) ;

                // ympyröille
        if (Vector.Distance(a.Position, b.Position) < a.Width + b.Width)
            // on päällekkäistä aluetta
            {
            vihu.Destroy();
            }
    }*/
}
