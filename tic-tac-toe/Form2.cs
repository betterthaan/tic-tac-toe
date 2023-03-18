using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace tic_tac_toe
{
    public partial class Form2 : Form
    {
        //эта переменная будет содержать ссылку на TextBox1 из формы Form1
        private TextBox TxtB1;
     //   private TextBox TxtB2;
        private ComboBox ComboBox1;
        private ComboBox ComboBox2;


        //создаем массив контролов
        List<Control> control = new List<Control>();

        int[,] Field;
        int[,] board;

        // static System.Windows.Forms.Timer myTimer;

        Timer myTimer = new Timer();
        Random random = new Random();

        String move_p;
        String move_c;



        private bool team;
        public Form2(TextBox t1, ComboBox cb1, ComboBox cb2)
        {
            //получаем ссылку на textBox в переменные
            TxtB1 = t1; //размерность игрового поля
           // TxtB2 = t2;
            ComboBox1 = cb1;
            ComboBox2 = cb2;

            myTimer = new System.Windows.Forms.Timer();
           
            InitializeComponent();

            myTimer.Interval = 1000; //чекаем каждую секунду
            myTimer.Enabled = true;
            myTimer.Tick += myTimer_Tick;
            myTimer.Start();

            Field = new int [Convert.ToInt32(t1.Text), Convert.ToInt32(t1.Text)];
            board = new int[Convert.ToInt32(t1.Text), Convert.ToInt32(t1.Text)];

            //Field = new int[3][];

            int k = 1;

            //расположим кнопки на форме через цикл
            for (int i = 0; i < Convert.ToInt32(TxtB1.Text); i++) 
            {
                for (int ii = 0; ii < Convert.ToInt32(TxtB1.Text); ii++) 
                {
                    Field[i, ii] = 0;
               //     F[i, ii] = 0;
                    Button btn = new Button();
                    btn.Name = "button" + k;
                    btn.Top = 35 + 35 * i;
                    btn.Left = 10 + 35 * ii;
                    btn.Width = 35;
                    btn.Height = 35;
                    btn.Click += ButtonOnClick;
                    this.Controls.Add(btn); 
                    control.Add(btn);
                    k++;
                }
            }


            move_p = ComboBox2.SelectedItem.ToString();
            switch (move_p)
            {
                case ("х"):
                    move_c = "о";
                    break;
                case ("о"):
                    move_c = "х";
                    break;
            }

            //кто первый ходит в зависимости от того, какое значние выбрал пользователь в ComboBox формы1 
            switch (ComboBox1.SelectedItem.ToString())
            {
                case ("х"):
                    label1.Text = "Текущий ход крестика";
                    switch (ComboBox2.SelectedItem.ToString())//за кого играет человек
                    {
                        case ("х"):  //если team=true - ходит крестик
                            team = true; //ход игрока человек
                            break;
                        case ("о"):
                            team = false; //ход игрока компьютер
                            break;
                    }
                    break;
                case ("о"):
                    team = false; //ход игрока нолик
                    label1.Text = "Текущий ход нолика";
                    switch (ComboBox2.SelectedItem.ToString())//за кого играет человек
                    {
                        case ("х"):  //если team=true - ходит крестик
                            team = false; //ход игрока компьютер
                            break;
                        case ("о"):
                            team = true; //ход игрока человек
                            break;
                    }
                    break;
            }
                         //    this.Height = 200;
                         //    this.Width = 200;
        }

        //какие есть веса
        int user_win = -100;
        int draww = 0;
        int comp_win = 100;

        int s, b;

        /*
        struct DepthWin
        {
            public int depth;
            public int best_score;
        }
        DepthWin depWin;
        */

        int MiniMax(int[,] board, int depth, bool is_maximazing)
        {
            if (checkWinMiniMax(board) == 1)  //комп проверяет что если он выиграл
            {
                return comp_win; //Тогда возвращаем максимальный вес
            }
            if (checkWinMiniMax(board) == 2) //если победил человек
            {
                return user_win;
            }
            if (Not(board)) //если ничья
            {
                return draww;
            }

            if (is_maximazing)
            {
                //выбор хода, который нам выгодней
                b = -1000;
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int ii = 0; ii < board.GetLength(0); ii++)
                    {
                        if (board[i, ii] == 0)
                        {
                            board[i, ii] = 1; //1-move computer
                            //чтобы рассчитать вес этого хода - вызываем рекурсивно минимакс
                            s = MiniMax(board, depth + 1, false);
                            //после того как ход оценен - восстанавливаем позицию на доске
                            board[i, ii] = 0; 
                       
                            if (s > b)
                            {
                                b = s;
                            }
                        }
                    }
                }
            }
            else //когда ход противника
            {
                //противник выбирает ход, который выгодней для него 
                b = 1000;
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int ii = 0; ii < board.GetLength(0); ii++)
                    {
                        if (board[i, ii] == 0)
                        {
                            board[i, ii] = 2; //2-move people
                            //чтобы рассчитать вес этого хода - вызываем рекурсивно минимакс
                            s = MiniMax(board, depth + 1, true);
                            //после того как ход оценен - восстанавливаем позицию на доске
                            board[i, ii] = 0;
                          
                            if (s < b)
                            {
                                b = s;
                            }
                        }
                    }
                }
            }
            return b;
        }

        void move(int y, int x)
        {
           // textBox1.Text += y.ToString();
            control[y * Field.GetLength(0) + x].Text = move_c;
            Field[y, x] = 1; //1-move computer;
        }

        int score = 0;
        int best_score;

       struct Pos
       {
            public int y;
            public int x;
       }
       Pos position;

  
        void myTimer_Tick(object sender, EventArgs e)
        {
            //логика игры компьютера
            if (team == false) //т.е. наступило время хода компьютера 
            {
                //Реализация алгоритма Minimax
                best_score = -1000;

                //Копия нашей игровой доски, чтобы безопасно вносить изменения
                for (int l = 0; l < Field.GetLength(0); l++)
                {
                    for (int ll = 0; ll < Field.GetLength(0); ll++)
                    {
                        board[l, ll] = Field[l, ll];
                    }
                }

                for (int i = 0; i < Field.GetLength(0); i++) 
                {
                    for (int ii = 0; ii < Field.GetLength(0); ii++) 
                    {
                        if (board[i, ii] == 0)
                        {
                            board[i, ii] = 1; //1-move computer
                            //теперь оцениваем эту позицию
                            score = MiniMax(board, 0, false);
                            board[i, ii] = 0;
                            if (score > best_score)
                            {
                                best_score = score;
                              //  textBox1.Text = best_score.ToString();
                                position.y = i;
                                position.x = ii;
                            }
                        }
                    }
                }
                move(position.y, position.x); //расставляем на игровой доске ход
                label1.Text = "Текущий ход " + move_p;
                checkWin();
                //if (!checkWin())
                //{
                    team = true;
                //}
            }
        }

        //Добавим проверку ничьи
        int amount_p;//шаги человека
        int amount_c;//шаги компьютера
        int amount_0;
        bool Not(int[,] F)
        {
            amount_p = 0;//шаги человека
            amount_c = 0;//шаги компьютера
            amount_0 = 0;
            for (int i = 0; i < F.GetLength(0); i++)
            {
                for (int ii = 0; ii < F.GetLength(0); ii++)
                {
                    if (F[i, ii] == 2)
                    {
                        amount_p++;
                    }
                    if (F[i, ii] == 1)
                    {
                        amount_c++;
                    }
                    if (F[i, ii] == 0)
                    {
                        amount_0++;
                    }
                }
            }
            if ((amount_p == amount_c) && (amount_0 == 0) && (amount_p != 0))  
            {
                return true;
            }
            return false;
        }


        int checkWinMiniMax(int[,] F)
        {
            for (int i = 0; i <= F.GetLength(0) - 3; i++)
            {
                for (int ii = 0; ii <= F.GetLength(0) - 3; ii++)
                {
                    if ((F[0 + i, 0 + ii] == F[0 + i, 1 + ii]) & (F[0 + i, 1 + ii] == F[0 + i, 2 + ii]) & (F[0 + i, 2 + ii]) != 0)
                    {
                       return F[0 + i, 0 + ii];
                    }
                    if ((F[1 + i, 0 + ii] == F[1 + i, 1 + ii]) & (F[1 + i, 1 + ii] == F[1 + i, 2 + ii]) & (F[1 + i, 2 + ii]) != 0)
                    {
                       return F[1 + i, 0 + ii];
                    }
                    if ((F[2 + i, 0 + ii] == F[2 + i, 1 + ii]) & (F[2 + i, 1 + ii] == F[2 + i, 2 + ii]) & (F[2 + i, 2 + ii]) != 0)
                    {
                        return F[2 + i, 0 + ii];
                    }
                    if ((F[0 + i, 0 + ii] == F[1 + i, 0 + ii]) & (F[1 + i, 0 + ii] == F[2 + i, 0 + ii]) & (F[2 + i, 0 + ii]) != 0)
                    {
                        return F[0 + i, 0 + ii];
                    }
                    if ((F[0 + i, 1 + ii] == F[1 + i, 1 + ii]) & (F[1 + i, 1 + ii] == F[2 + i, 1 + ii]) & (F[2 + i, 1 + ii]) != 0)
                    {
                        return F[0 + i, 1 + ii];
                    }
                    if ((F[0 + i, 2 + ii] == F[1 + i, 2 + ii]) & (F[1 + i, 2 + ii] == F[2 + i, 2 + ii]) & (F[2 + i, 2 + ii]) != 0)
                    {
                       return F[0 + i, 2 + ii];
                    }
                    if ((F[0 + i, 0 + ii] == F[1 + i, 1 + ii]) & (F[1 + i, 1 + ii] == F[2 + i, 2 + ii]) & (F[2 + i, 2 + ii]) != 0)
                    {
                        return F[0 + i, 0 + ii];
                    }
                    if ((F[0 + i, 2 + ii] == F[1 + i, 1 + ii]) & (F[1 + i, 1 + ii] == F[2 + i, 0 + ii]) & (F[2 + i, 0 + ii]) != 0)
                    {
                        return F[0 + i, 2 + ii];
                    }
                }
            }
            return 0;
        }

        //чекаем выигрыш
        bool checkWin()
        {
            for (int i = 0; i <= Field.GetLength(0) - 3; i++)
            {
                for (int ii = 0; ii <= Field.GetLength(0) - 3; ii++)
                {
                    if ((Field[0 + i, 0 + ii] == Field[0 + i, 1 + ii]) & (Field[0 + i, 1 + ii] == Field[0 + i, 2 + ii]) & (Field[0 + i, 2 + ii]) != 0)
                    {
                        control[Convert.ToInt32(TxtB1.Text) * i + ii].BackColor = Color.Green; 
                        control[Convert.ToInt32(TxtB1.Text) * i + ii + 1].BackColor = Color.Green;
                        control[Convert.ToInt32(TxtB1.Text) * i + ii + 2].BackColor = Color.Green;
                        textBox1.Text = "Выиграл " + control[Convert.ToInt32(TxtB1.Text) * i + ii].Text;
                        textBox1.BackColor = Color.Pink;
                        return true;
                    }
                    if ((Field[1 + i, 0 + ii] == Field[1 + i, 1 + ii]) & (Field[1 + i, 1 + ii] == Field[1 + i, 2 + ii]) & (Field[1 + i, 2 + ii]) != 0)
                    {
                        control[Convert.ToInt32(TxtB1.Text) * (i + 1) + ii].BackColor = Color.Green;
                        control[Convert.ToInt32(TxtB1.Text) * (1 + i) + ii + 1].BackColor = Color.Green;
                        control[Convert.ToInt32(TxtB1.Text) * (1 + i) + ii + 2].BackColor = Color.Green;
                        textBox1.Text = "Выиграл " + control[Convert.ToInt32(TxtB1.Text) * (i + 1) + ii].Text;
                        textBox1.BackColor = Color.Pink;
                        return true;
                    }
                    if ((Field[2 + i, 0 + ii] == Field[2 + i, 1 + ii]) & (Field[2 + i, 1 + ii] == Field[2 + i, 2 + ii]) & (Field[2 + i, 2 + ii]) != 0)
                    {
                        control[Convert.ToInt32(TxtB1.Text) * (i + 2) + ii].BackColor = Color.Green; 
                        control[Convert.ToInt32(TxtB1.Text) * (2 + i) + ii + 1].BackColor = Color.Green;
                        control[Convert.ToInt32(TxtB1.Text) * (2 + i) + ii + 2].BackColor = Color.Green;
                        textBox1.Text = "Выиграл " + control[Convert.ToInt32(TxtB1.Text) * (i + 2) + ii].Text;
                        textBox1.BackColor = Color.Pink;
                        return true;
                    }
                    if ((Field[0 + i, 0 + ii] == Field[1 + i, 0 + ii]) & (Field[1 + i, 0 + ii] == Field[2 + i, 0 + ii]) & (Field[2 + i, 0 + ii]) != 0)
                    {
                        control[Convert.ToInt32(TxtB1.Text) * i + ii].BackColor = Color.Green; 
                        control[Convert.ToInt32(TxtB1.Text) * (i + 1) + ii].BackColor = Color.Green;
                        control[Convert.ToInt32(TxtB1.Text) * (i + 2) + ii].BackColor = Color.Green;
                        textBox1.Text = "Выиграл " + control[Convert.ToInt32(TxtB1.Text) * i + ii].Text;
                        textBox1.BackColor = Color.Pink;
                        return true;
                    }
                    if ((Field[0 + i, 1 + ii] == Field[1 + i, 1 + ii]) & (Field[1 + i, 1 + ii] == Field[2 + i, 1 + ii]) & (Field[2 + i, 1 + ii]) != 0)
                    {
                        control[Convert.ToInt32(TxtB1.Text) * i + ii + 1].BackColor = Color.Green; 
                        control[Convert.ToInt32(TxtB1.Text) * (1 + i) + ii + 1].BackColor = Color.Green;
                        control[Convert.ToInt32(TxtB1.Text) * (2 + i) + ii + 1].BackColor = Color.Green;
                        textBox1.Text = "Выиграл " + control[Convert.ToInt32(TxtB1.Text) * i + ii + 1].Text;
                        textBox1.BackColor = Color.Pink;
                        return true;
                    }
                    if ((Field[0 + i, 2 + ii] == Field[1 + i, 2 + ii]) & (Field[1 + i, 2 + ii] == Field[2 + i, 2 + ii]) & (Field[2 + i, 2 + ii]) != 0)
                    {
                        control[Convert.ToInt32(TxtB1.Text) * i + ii + 2].BackColor = Color.Green; 
                        control[Convert.ToInt32(TxtB1.Text) * (1 + i) + ii + 2].BackColor = Color.Green;
                        control[Convert.ToInt32(TxtB1.Text) * (2 + i) + ii + 2].BackColor = Color.Green;
                        textBox1.Text = "Выиграл " + control[Convert.ToInt32(TxtB1.Text) * i + ii + 2].Text;
                        textBox1.BackColor = Color.Pink;
                        return true;
                    }
                    if ((Field[0 + i, 0 + ii] == Field[1 + i, 1 + ii]) & (Field[1 + i, 1 + ii] == Field[2 + i, 2 + ii]) & (Field[2 + i, 2 + ii]) != 0)
                    {
                        control[Convert.ToInt32(TxtB1.Text) * i + ii].BackColor = Color.Green; 
                        control[Convert.ToInt32(TxtB1.Text) * (1 + i) + ii + 1].BackColor = Color.Green;
                        control[Convert.ToInt32(TxtB1.Text) * (2 + i) + ii + 2].BackColor = Color.Green;
                        textBox1.Text = "Выиграл " + control[Convert.ToInt32(TxtB1.Text) * i + ii].Text;
                        textBox1.BackColor = Color.Pink;
                        return true;
                    }
                    if ((Field[0 + i, 2 + ii] == Field[1 + i, 1 + ii]) & (Field[1 + i, 1 + ii] == Field[2 + i, 0 + ii]) & (Field[2 + i, 0 + ii]) != 0)
                    {
                        control[Convert.ToInt32(TxtB1.Text) * i + ii + 2].BackColor = Color.Green; 
                        control[Convert.ToInt32(TxtB1.Text) * (1 + i) + ii + 1].BackColor = Color.Green;
                        control[Convert.ToInt32(TxtB1.Text) * (2 + i) + ii].BackColor = Color.Green;
                        textBox1.Text = "Выиграл " + control[Convert.ToInt32(TxtB1.Text) * i + ii + 2].Text;
                        textBox1.BackColor = Color.Pink;
                        return true;
                    }
                }
            }
            return false;
        }

        int index_y = 0;
        void Field_index(int index_button, int num)
        {
            switch (index_button - 1 <= Field.GetLength(0) - 1)
            {
                case true:
                    Field[0, index_button - 1] = num;
                    break;
                case false:
                    index_y = 0;
                    while(index_button - 1 - index_y * (Field.GetLength(0)) >= 0)
                    {
                        index_y++;
                    }
                    Field[index_y - 1, (index_button - 1 - (index_y - 1) * (Field.GetLength(0)))] = num;
                    break;
            }
        }

        bool Field_i(int index_button)
        {
            switch (index_button - 1 <= Field.GetLength(0) - 1)
            {
                case true:
                    if (Field[0, index_button - 1] != 0)
                    {
                        return false;
                    }
                    break;
                case false:
                    index_y = 0;
                    while (index_button - 1 - index_y * (Field.GetLength(0)) >= 0)
                    {
                        index_y++;
                    }
                    if (Field[index_y - 1, (index_button - 1 - (index_y - 1) * (Field.GetLength(0)))] != 0)
                    {
                        return false;
                    }
                    break;
            }
            return true;
        }

        int number = 0;
        int IsNumber(String s)
        {
            number = 0;
            foreach (char ch in s)
            {
                if (Char.IsDigit(ch))
                {
                    number = number * 10 + Int32.Parse(ch.ToString());
                }
            }
            return number;
        }

   

        private void ButtonOnClick(object sender, EventArgs eventArgs)
        {
            if (!team)
            {
                label1.Text = "Текущий ход " + move_c;
            }
            if (team)
            {
                if (Field_i(IsNumber(sender.GetType().GetProperty("Name").GetValue(sender).ToString())))
                {
                    sender.GetType().GetProperty("Text").SetValue(sender, move_p);//move_p - символ, за который выбрал играть человек
                    Field_index(IsNumber(sender.GetType().GetProperty("Name").GetValue(sender).ToString()), 2); // 2 - человек
                    label1.Text = "Текущий ход " + move_c;
                    if (!checkWin())
                    {
                        team = false;
                    }
                }
            }
        }
    }
}
