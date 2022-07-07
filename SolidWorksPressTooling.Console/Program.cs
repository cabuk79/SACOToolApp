// See https://aka.ms/new-console-template for more information

using SolidWorksPressTooling.SwAPI;

Console.WriteLine("Hello, World!");

DieBodySW die = new DieBodySW(45.00, 25.00, 2.50, 1.50, 15.00);
//die.CreateDieBody();
die.CreateDrawing();

//Testing newSW = new Testing();
//newSW.ConnectToSW();

