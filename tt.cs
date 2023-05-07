using System.Windows.Forms;
using System.Drawing;
using System;
namespace fine_modul
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Run");
            Form x = new MainForm();
            Application.Run(x);
        }
        class MainForm : Form
        {
            int posx;
            int posy;

            string debmess;
            bool first;
            static cell[,] Fild;
            int lastfigureID;
            int lastfigureRt;
            int result = 0;

            public static Panel screen = new Panel();
            public static Label info = new Label();
            public static ListBox Infopanel = new ListBox();
            public Graphics g;
            bool daun = false;

            public int move = 0;
            Timer ss = new Timer();
            Random rnd = new Random();
            int nextFigureID;
            int timerspeed = 400;
            public struct cell
            {
                public bool paint;
                public bool movable;
            }
            public MainForm()
            {
                Fild = new cell[17, 20];
                Size = new Size(280, 401);
                FormBorderStyle = FormBorderStyle.None;
                screen.Size = new Size(280, 401);
                screen.BackColor = Color.Black;
                Infopanel.Dock = DockStyle.Right;
                Infopanel.Width = 19;
                info.Dock = DockStyle.Top;
                info.BackColor = Color.Yellow;
                info.ForeColor = Color.Black;
                info.Font = SystemFonts.StatusFont;
                info.Height = 18;
                Controls.Add(info);
                info.MouseDown += DragDown;
                info.MouseMove += DragMove;
                Controls.Add(screen);
                g = screen.CreateGraphics();
                paint_net();
                timerspeed = 400;
                this.KeyDown += shift;
                this.KeyUp += pressany;
                InitializeTimer();
                //Controls.Add(Infopanel);
            }
            private void DragDown(object sender, MouseEventArgs e)
            {
                posx = MousePosition.X;
                posy = MousePosition.Y;
            }
            public void shift(object sender, KeyEventArgs e)
            {
                if (e.KeyCode.ToString() == "Down")
                {
                    ss.Interval = timerspeed / 20;
                }
            }
            private void DragMove(object sender, MouseEventArgs e)
            {
                int tmpx = MousePosition.X;
                int tmpy = MousePosition.Y;
                if (e.Button == MouseButtons.Left)
                    this.Location = new Point(this.Location.X - (posx - tmpx), this.Location.Y - (posy - tmpy));
                posx = tmpx;
                posy = tmpy;
            }
            public void pressany(object sender, KeyEventArgs e)
            {
                Console.WriteLine(e.KeyCode.ToString());
                g.DrawRectangle(new Pen(Color.Yellow), 0, 18, 200, 382);
                if (e.KeyCode.ToString() == "Up")
                {
                    Rotate();
                }
                if (e.KeyCode.ToString() == "Back")
                    Close();
                if (e.KeyCode.ToString() == "Return" || e.KeyCode.ToString() == "Enter") //win&&linux
                {
                    RefreshP();
                    if (!daun)
                    {
                        if (info.Text.Length > 6)
                            ClearTab();
                        daun = true;
                    }
                    else
                        daun = false;
                }
                if (e.KeyCode.ToString() == "Left" || e.KeyCode.ToString() == "Right")
                {
                    Movie(e.KeyCode.ToString());
                }
                if (e.KeyCode.ToString() == "Down")
                {
                    ss.Interval = timerspeed;
                }
            }

            public void Movie(string vector)
            {
                int i = 0;
                int j = 0;
                int left = 20;
                int right = -1;
                try
                {

                    for (i = 0; i < 10; i++)
                        for (j = 0; j < 20; j++)
                            if (Fild[i, j].movable)
                            {
                                if (i > right)
                                    right = i;
                                if (i < left)
                                    left = i;
                                if (i > 0 && vector == "Left")
                                    if (Fild[i - 1, j].paint == true && Fild[i - 1, j].movable == false) vector = "ops";
                                if (vector == "Right" && i < 9)
                                    if (Fild[i + 1, j].paint == true && Fild[i + 1, j].movable == false) vector = "ops";
                            }
                }
                catch (Exception ou)
                {
                    MessageBox.Show(ou.Message + " орвоырвлоры " + i.ToString() + " 1 " + j.ToString());
                }


                try
                {

                    if (vector == "Left" && left > 0)
                    {
                        for (i = left; i < right + 1; i++)
                            for (j = 0; j < 20; j++)
                            {
                                if (Fild[i, j].movable && Fild[i, j].paint)
                                {
                                    Fild[i - 1, j].paint = true;
                                    Fild[i - 1, j].movable = true;
                                    Fild[i, j].paint = false;
                                    Fild[i, j].movable = false;
                                    RefreshP();
                                }
                            }
                    }
                    if (vector == "Right" && right < 9)
                        for (i = 9; i >= left; i--)
                            for (j = 19; j >= 0; j--)
                                if (Fild[i, j].movable && Fild[i, j].paint)
                                {
                                    Fild[i + 1, j].paint = true;
                                    Fild[i + 1, j].movable = true;
                                    Fild[i, j].paint = false;
                                    Fild[i, j].movable = false;
                                    RefreshP();
                                }
                }
                catch (Exception arrg)
                {
                    MessageBox.Show(arrg.Message + " 2  " + i.ToString() + " " + j.ToString());
                }

            }

            public void ClearTab()
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        Fild[i, j].movable = false;
                        Fild[i, j].paint = false;
                    }
                }
            }

            public void paint_net()
            {
                for (int i = 0; i < 10; i++)
                    for (int j = 0; j < 20; j++)
                        g.DrawRectangle(new Pen(Color.Yellow), i * 20, j * 20, 20, 20);
            }
            void InitializeTimer()
            {
                ss.Interval = 300;
                ss.Tick += new EventHandler(Timer1_Tick);
                ss.Enabled = true;
            }

            void Timer1_Tick(object Sender, EventArgs e)
            {
                Console.WriteLine(ss.Interval.ToString());
                if (daun)
                    movedown();
            }

            public void movedown()
            {
                    int i = 0;
                    int j = 0;
                    int moki = 0;
                    debmess = "";

                    try
                    {
                        bool can = true;
                        debmess += "1..";
                        for (i = 0; i < 10; i++)
                        {

                            for (j = 0; j < 20; j++)
                            {
                                if (j < 19)
                                {
                                    if (Fild[i, j].movable && Fild[i, j + 1].paint == true && Fild[i, j + 1].movable == false)
                                        can = false;
                                }
                                else if (Fild[i, j].movable && j == 19)
                                    can = false;
                                if (Fild[i, j].movable)
                                    moki++;
                            }
                        }
                        if (can && moki > 0)
                        {
                            debmess += "2..";
                            for (i = 9; i >= 0; i--)
                                for (j = 18; j >= 0; j--)
                                    if (Fild[i, j].movable && Fild[i, j].paint)
                                    {
                                        Fild[i, j + 1].paint = true;
                                        Fild[i, j + 1].movable = true;
                                        Fild[i, j].paint = false;
                                        Fild[i, j].movable = false;
                                    }
                        }
                        else
                        {
                            debmess += "3..";
                            for (i = 0; i < 10; i++)
                                for (j = 0; j < 20; j++)
                                    Fild[i, j].movable = false;
                            debmess += "4..";
                            newfig();
                            debmess += " 4/2  ..";
                        }
                        RefreshP();
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + " jjd " + i.ToString() + "   " + j.ToString() + Environment.NewLine + debmess);
                    }
            }

            public void RefreshP()
            {
                debmess += "5..";
                SolidBrush mylat0 = new SolidBrush(Color.Black);
                for (int i = 0; i < 10; i++)
                    for (int j = 1; j < 20; j++)
                        if (!Fild[i, j].paint)
                            g.FillRectangle(mylat0, (i * 20) + 1, (j * 20) + 1, 18, 18);
                SolidBrush mylat1 = new SolidBrush(Color.Yellow);
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 1; j < 20; j++)
                    {
                        if (Fild[i, j].paint)
                        {
                            g.FillRectangle(mylat1, (i * 20) + 1, (j * 20) + 1, 18, 18);
                            g.DrawRectangle(new Pen(Color.Black), (i * 20) + 2, (j * 20) + 2, 14, 14);
                        }
                    }
                }
            }

            public void newfig()
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        if (Fild[i, j].paint)
                        {
                            if (j < 2)
                            {
                                daun = false;
                                info.Text = "Geym over, man";
                                result = 0;
                                first = true;
                            }
                        }
                    }
                }
                if (daun)
                {
                    info.Text = "";
                    int row = 0;
                    int lines = 0;
                    bool wait = true;
                    for (int j = 19; j >= 0; j--)
                    {
                        row = 0;
                        for (int i = 0; i < 10; i++)
                            if (Fild[i, j].paint == true) { row++; }
                        if (row == 10)
                        {
                            lastfigureID = 88;
                            lines++;
                            for (int i = 0; i < 10; i++) Fild[i, j].paint = false;
                            for (int i = 0; i < 10; i++)
                                for (int k = 0; k < 20; k++)
                                    if (k != j && k < j)
                                        Fild[i, k].movable = true;
                        }
                        else wait = false;
                    }
                    for (int i = 0; i < lines; i++) movedown();
                    result += lines;
                    if (result > 0) timerspeed = 400 - result;
                        info.Text = result.ToString();
                    if (!wait)
                    {
                        for (int i = 0; i < 10; i++)
                            for (int j = 0; j < 20; j++)
                                Fild[i, j].movable = false;
                        if (first)
                            lastfigureID = rnd.Next(1, 7);
                        else
                            lastfigureID = nextFigureID;

                        if (lastfigureID == 1) fig_by_coord(4, 0, 4, 1, 4, 2, 4, 3);
                        if (lastfigureID == 2) fig_by_coord(4, 0, 5, 0, 4, 1, 4, 2);
                        if (lastfigureID == 3) fig_by_coord(5, 0, 5, 1, 5, 2, 6, 2);
                        if (lastfigureID == 4) fig_by_coord(5, 0, 5, 1, 6, 0, 6, 1);
                        if (lastfigureID == 5) fig_by_coord(5, 0, 5, 1, 4, 1, 6, 0);
                        if (lastfigureID == 6) fig_by_coord(5, 0, 5, 1, 4, 1, 6, 1);
                        if (lastfigureID == 7) fig_by_coord(4, 0, 5, 0, 5, 1, 6, 1);
                        nextFigureID = rnd.Next(1, 7);
                        rscreenclear();
                        if (!first)
                        {
                            info.Text += "            " + nextFigureID.ToString();
                            if (nextFigureID == 1) fig_by_coord(11, 2, 11, 3, 11, 4, 11, 5);
                            if (nextFigureID == 2) fig_by_coord(11, 2, 12, 2, 11, 3, 11, 4);
                            if (nextFigureID == 3) fig_by_coord(11, 2, 11, 3, 11, 4, 12, 4);
                            if (nextFigureID == 4) fig_by_coord(12, 2, 12, 3, 13, 2, 13, 3);
                            if (nextFigureID == 5) fig_by_coord(11, 2, 11, 3, 10, 3, 12, 2);
                            if (nextFigureID == 6) fig_by_coord(11, 2, 11, 3, 10, 3, 12, 3);
                            if (nextFigureID == 7) fig_by_coord(11, 2, 12, 2, 12, 3, 13, 3);
                        }
                        rscreendraw();
                        lastfigureRt = 0;
                        first = false;
                    }
                }
            }
            public void rscreenclear()
            {
                for (int i = 10; i < 14; i++)
                    for (int j = 1; j < 20; j++)
                        Fild[i, j].paint = false;

            }
            public void rscreendraw()
            {
                SolidBrush mylat0 = new SolidBrush(Color.Black);
                for (int i = 10; i < 14; i++)
                    for (int j = 1; j < 20; j++)
                        g.FillRectangle(mylat0, (i * 20) + 1, (j * 20) + 1, 18, 18);
                SolidBrush mylat1 = new SolidBrush(Color.Yellow);
                for (int i = 10; i < 14; i++)
                {
                    for (int j = 1; j < 20; j++)
                    {
                        if (Fild[i, j].paint)
                        {
                            g.FillRectangle(mylat1, (i * 20) + 1, (j * 20) + 1, 18, 18);
                            g.DrawRectangle(new Pen(Color.Black), (i * 20) + 2, (j * 20) + 2, 14, 14);
                        }
                    }
                }
            }

            public void fig_by_coord(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
            {
                Fild[x1, y1].movable = true; Fild[x1, y1].paint = true;
                Fild[x2, y2].movable = true; Fild[x2, y2].paint = true;
                Fild[x3, y3].movable = true; Fild[x3, y3].paint = true;
                Fild[x4, y4].movable = true; Fild[x4, y4].paint = true;
            }

            public bool Needtorotate(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
            {
                int count = 0;
                if (Correct_Rotating_Point(x1, y1)) count++;
                if (Correct_Rotating_Point(x2, y2)) count++;
                if (Correct_Rotating_Point(x3, y3)) count++;
                if (Correct_Rotating_Point(x4, y4)) count++;
                if (count == 4) return true;
                else return false;
            }
            public bool Correct_Rotating_Point(int x, int y)
            {
                bool localresult = true;
                if (x >= 0 && x <= 9)
                {
                    if (Fild[x, y].movable == false && Fild[x, y].paint == true)
                    {
                        localresult = false;
                    }
                }
                else
                {
                    localresult = false;
                }
                return localresult;
            }
            public void Rotate()
            {
                int L = 9, T = 19, B = 0, R = 0; //left, top, bottom, right;
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 19; j++)
                    {
                        if (Fild[i, j].movable == true)
                        {
                            if (j < T) T = j;
                            if (j > B) B = j;
                            if (i > R) R = i;
                            if (i < L) L = i;
                        }
                    }
                }
                if (lastfigureID == 1)
                {
                    if (lastfigureRt == 0)
                    {
                        if (Needtorotate(L - 1, T + 1, L, T + 1, L + 1, T + 1, L + 2, T + 1))
                        {
                            Clean_Cur_Figure();
                            fig_by_coord(L - 1, T + 1, L, T + 1, L + 1, T + 1, L + 2, T + 1);
                            lastfigureRt = 1;
                        }
                    }
                    else if (lastfigureRt == 1)
                    {
                        if (Needtorotate(L + 1, T - 1, L + 1, T, L + 1, T + 1, L + 1, T + 2))
                        {
                            Clean_Cur_Figure();
                            fig_by_coord(L + 1, T - 1, L + 1, T, L + 1, T + 1, L + 1, T + 2);
                            lastfigureRt = 0;
                        }
                    }
                }
                if (lastfigureID == 2)
                {
                    if (lastfigureRt == 0)
                    {
                        if (Needtorotate(L, T, L + 1, T, L + 2, T, L + 2, T + 1))
                        {
                            Clean_Cur_Figure();
                            fig_by_coord(L, T, L + 1, T, L + 2, T, L + 2, T + 1);
                            lastfigureRt = 1;
                        }
                    }
                    else if (lastfigureRt == 1)
                    {
                        if (Needtorotate(R - 1, T, R - 1, T + 1, R - 1, T + 2, R - 2, T + 2))
                        {
                            Clean_Cur_Figure();
                            fig_by_coord(R - 1, T, R - 1, T + 1, R - 1, T + 2, R - 2, T + 2);
                            lastfigureRt = 2;
                        }
                    }
                    else if (lastfigureRt == 2)
                    {
                        if (Needtorotate(R - 1, T + 1, R - 1, T + 2, R, T + 2, R + 1, T + 2))
                        {
                            Clean_Cur_Figure();
                            fig_by_coord(R - 1, T + 1, R - 1, T + 2, R, T + 2, R + 1, T + 2);
                            lastfigureRt = 3;
                        }
                    }
                    else if (lastfigureRt == 3)
                    {
                        if (Needtorotate(R - 2, T, R - 2, T + 1, R - 2, T + 2, R - 1, T))
                        {
                            Clean_Cur_Figure();
                            fig_by_coord(R - 2, T, R - 2, T + 1, R - 2, T + 2, R - 1, T);
                            lastfigureRt = 0;
                        }
                    }
                }
                if (lastfigureID == 3)
                {
                    if (lastfigureRt == 0)
                    {
                        if (Needtorotate(L, T + 2, L, T + 1, L + 1, T + 1, L + 2, T + 1))
                        {
                            Clean_Cur_Figure();
                            fig_by_coord(L, T + 2, L, T + 1, L + 1, T + 1, L + 2, T + 1);
                            lastfigureRt = 1;
                        }
                    }
                    else if (lastfigureRt == 1)
                    {
                        if (Needtorotate(L, T, L + 1, T, L + 1, T + 1, L + 1, T + 2))
                        {
                            Clean_Cur_Figure();
                            fig_by_coord(L, T, L + 1, T, L + 1, T + 1, L + 1, T + 2);
                            lastfigureRt = 2;
                        }
                    }
                    else if (lastfigureRt == 2)
                    {
                        if (Needtorotate(L, T + 1, L + 1, T + 1, L + 2, T + 1, L + 2, T))
                        {
                            Clean_Cur_Figure();
                            fig_by_coord(L, T + 1, L + 1, T + 1, L + 2, T + 1, L + 2, T);
                            lastfigureRt = 3;
                        }
                    }
                    else if (lastfigureRt == 3)
                    {
                        if (Needtorotate(L, T, L, T + 1, L, T + 2, L + 1, T + 2))
                        {
                            Clean_Cur_Figure();
                            fig_by_coord(L, T, L, T + 1, L, T + 2, L + 1, T + 2);
                            lastfigureRt = 0;
                        }
                    }
                }
                if (lastfigureID == 5)
                {

                    if (lastfigureRt == 0)
                    {
                        if (Needtorotate(L, T, L, T + 1, L + 1, T + 1, L + 1, T + 2))
                        {
                            Clean_Cur_Figure();
                            fig_by_coord(L, T, L, T + 1, L + 1, T + 1, L + 1, T + 2);
                            lastfigureRt = 1;
                        }
                    }
                    else if (lastfigureRt == 1)
                    {
                        if (Needtorotate(L, T + 2, L + 1, T + 2, L + 1, T + 1, L + 2, T + 1))
                        {
                            Clean_Cur_Figure();
                            fig_by_coord(L, T + 2, L + 1, T + 2, L + 1, T + 1, L + 2, T + 1);
                            lastfigureRt = 0;
                        }
                    }
                }
                if (lastfigureID == 6)
                {
                    if (lastfigureRt == 0)
                    {
                        if (Needtorotate(L, T, L, T + 1, L, T + 2, L + 1, T + 1))
                        {
                            Clean_Cur_Figure();
                            fig_by_coord(L, T, L, T + 1, L, T + 2, L + 1, T + 1);
                            lastfigureRt = 1;
                        }
                    }
                    else if (lastfigureRt == 1)
                    {
                        if (Needtorotate(L, T + 1, L + 1, T + 1, L + 2, T + 1, L + 1, T + 2))
                        {
                            Clean_Cur_Figure();
                            fig_by_coord(L, T + 1, L + 1, T + 1, L + 2, T + 1, L + 1, T + 2);
                            lastfigureRt = 2;
                        }
                    }
                    else if (lastfigureRt == 2)
                    {
                        if (Needtorotate(L, T + 1, L + 1, T, L + 1, T + 1, L + 1, T + 2))
                        {
                            Clean_Cur_Figure();
                            fig_by_coord(L, T + 1, L + 1, T, L + 1, T + 1, L + 1, T + 2);
                            lastfigureRt = 3;
                        }
                    }
                    else if (lastfigureRt == 3)
                    {
                        if (Needtorotate(L, T + 2, L + 1, T + 2, L + 2, T + 2, L + 1, T + 1))
                        {
                            Clean_Cur_Figure();
                            fig_by_coord(L, T + 2, L + 1, T + 2, L + 2, T + 2, L + 1, T + 1);
                            lastfigureRt = 0;
                        }
                    }
                }
                if (lastfigureID == 7)
                {

                    if (lastfigureRt == 0)
                    {
                        if (Needtorotate(L, T + 2, L, T + 1, L + 1, T + 1, L + 1, T))
                        {
                            Clean_Cur_Figure();
                            fig_by_coord(L, T + 2, L, T + 1, L + 1, T + 1, L + 1, T);
                            lastfigureRt = 1;
                        }
                    }
                    else if (lastfigureRt == 1)
                    {
                        if (Needtorotate(L, T + 1, L + 1, T + 1, L + 1, T + 2, L + 2, T + 2))
                        {
                            Clean_Cur_Figure();
                            fig_by_coord(L, T + 1, L + 1, T + 1, L + 1, T + 2, L + 2, T + 2);
                            lastfigureRt = 0;
                        }
                    }
                }
            }
            public void Clean_Cur_Figure()
            {
                for (int i = 0; i < 10; i++)
                    for (int j = 0; j < 19; j++)
                        if (Fild[i, j].movable)
                        {
                            Fild[i, j].movable = false;
                            Fild[i, j].paint = false;
                        }
            }
        }
    }
}