using System;
using System.Threading;

namespace Travel_c_sharp_adventure
{
    class Game
    {
        public static void fillArray(char[,] array, int level, char symb, int From, int To){
            for (int i = 0; i < To - From; i++)
            {
                array[level, From + i] = symb;
            }

        }


        static void Main()
        {

            /*!!!!!!!ПОФИКСИТЬ!!!!!!!
            Convert.ToInt32(command[2]);
            И выходы за пределы массива

            !!!!!!!!!!!!!
            */

            int worldSize = 100; // размер мира
            int worldHeight = 8; // высота мира
            int fov = 30; // видмое кол-во симолов
            int xPos = 1; // Горизонталььная позиция игрока
            int hPos = 4; // Вертикальная позиция

            // кол-во ходов следать в право или лево
            int PrepareMoveLeft = 0;
            int PrepareMoveRight = 0;
            
            int IdleCount = 0; // Счётчик ожидания
            int FallDistance = 0; // высота падения

            bool alive = true; // Жив?

            bool ret = true; // Главный цикл

            // world params ->
            char c_Way = '\\'; // Символ пути
            char c_Wall = '|'; // Символ стены ==

            char[,] BackGround = new char[worldHeight, worldSize];

            fillArray(BackGround, 3, c_Way, 25, 50);
            fillArray(BackGround, 4, c_Wall, 25, 50);

            fillArray(BackGround, 4, c_Way, 0, 26);
            fillArray(BackGround, 5, c_Wall, 0, 26);

            fillArray(BackGround, 6, c_Way, 50, 80);
            fillArray(BackGround, 7, c_Wall, 50, 80);

            while (ret)
            {
                // ГЛАВНЫЙ КАДР
                Console.WriteLine("xPos: {0} hPos: {1} Alive: {2} FD: {3}", xPos, hPos, alive, FallDistance); // debug
                char[,] MainFrame = new char[worldHeight, worldSize]; // Главный кадр
                Array.Copy(BackGround, MainFrame, BackGround.Length);
                string[] command = new string[0];
                // Отрисовка карты
                for (int i = 0; i < worldHeight; i++){// перебор строк

                    Console.WriteLine("");
                    for (int b = 0; b < fov; b++){// перебор столбцов

                        int cord = (xPos - fov / 2) + b; // текущие координаты для отображения

                        if (cord == xPos && i == hPos) Console.Write('*'); // отображение игрока
                        else if (cord >= 0) Console.Write(MainFrame[i, cord]); // отображение

                    }

                }

                // Подение 
                if (MainFrame[hPos, xPos] != c_Way && FallDistance == 0){

                    for (int i = 0; hPos + i < MainFrame.GetLength(0) - 1 && MainFrame[(hPos + i), xPos] != c_Way; i++){
                        FallDistance++;
                        alive = FallDistance >= 2 ? false : true;
                        PrepareMoveLeft = PrepareMoveRight = 0;
                    }

                }

                // если нету движения и ожидания и падения и смерти гг, то дать пользователю ввод команд
                if (PrepareMoveLeft == 0 && PrepareMoveRight == 0 && IdleCount == 0 && FallDistance == 0 && alive == true){
                    Console.Write("\nEnter command:                      "); // костыль для закрытия предудущей команды
                    Console.SetCursorPosition(0, worldHeight + 1); // место для вывода текста "Enter command" 
                    Console.Write("\nEnter command: ");
                    command = (Convert.ToString(Console.ReadLine())).ToLower().Split(' '); // строку разбить на массив и написать в нижнем регистре
                    // вывод значений DEBUG
                    for (int i = 0; i < command.Length; i++){
                        Console.Write(command[i] + "|");
                    }

                    if (command.Length > 0){ // Если массив больше 0
                        if (command[0] == "move"){ // Движение
                            if (command.Length >= 3){

                                if (command[1] == "left") PrepareMoveLeft += Convert.ToInt32(command[2]);
                                else if (command[1] == "right") PrepareMoveRight += Convert.ToInt32(command[2]);

                            }else if (command.Length == 2){
                                if (command[1] == "left" && MainFrame[hPos, xPos - 1] != c_Wall) xPos--;
                                // 				Коллизия
                                else if (command[1] == "right" && MainFrame[hPos, xPos + 1] != c_Wall) xPos++;

                                else if (command[1] == "up"){
                                    if (MainFrame[(hPos - 1), xPos] == c_Way) hPos--;

                                    else if (MainFrame[(hPos - 1), xPos] == c_Wall && MainFrame[(hPos - 2), xPos] == c_Way){

                                        hPos -= 2;
                                    }

                                }else if (command[1] == "down"){

                                    if (MainFrame[hPos + 1, xPos] == c_Way) hPos++;

                                    else if (MainFrame[hPos + 1, xPos] == c_Wall && MainFrame[hPos + 2, xPos] == c_Way){
                                        hPos += 2;

                                    }
                                }
                            }
                        }else if (command[0] == "idle"){

                            if (command.Length >= 2) IdleCount += Convert.ToInt32(command[1]);
                            else IdleCount = 1;

                        }else if (command[0] == "exit") ret = false; // ВЫХОД
                        else Console.WriteLine("Command not found!"); // Если комманда не найдена
                    }

                }
                else if (PrepareMoveLeft != 0 || PrepareMoveRight != 0 && FallDistance == 0){ // 
                    if (PrepareMoveLeft >= 1){

                        if (MainFrame[hPos, xPos - 1] != c_Wall){
                            xPos--;
                            PrepareMoveLeft--;
                            Thread.Sleep(300);
                        }else PrepareMoveLeft = 0;

                    }

                    if (PrepareMoveRight >= 1){
                        if (MainFrame[hPos, xPos + 1] != c_Wall){
                            xPos++;
                            PrepareMoveRight--;
                            Thread.Sleep(300);
                        }else PrepareMoveRight = 0;
                    }
                }else if (IdleCount > 0){
                    IdleCount--;
                    Thread.Sleep(300);

                }else if (FallDistance > 0){
                    FallDistance--;
                    hPos++;
                    Thread.Sleep(100);

                }else if (alive == false && FallDistance == 0){

                    Console.WriteLine("YOU DIE!");
                }

                Console.SetCursorPosition(0, 0); // вернуть курсор в начало
            }
            Console.WriteLine("To exit press any key...");
            Console.ReadKey();

        }
    }

}