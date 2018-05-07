using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Jauge
{
    public partial class Jauge : UserControl
    {

        //Déclarer les proprieté
        // Les couleurs 
        private Color _corpColor = Color.DarkGray;
        private Color _borderColor = Color.Black;
        private Color _aiguilleColor = Color.OrangeRed;
        private Color _gradTickColor = Color.Gray;
        //la couleur de la police
        private Color _gradChiffreColor = Color.Black;

        //La ploice
        private Font _chiffreFont = new Font("Arial", 12);


        //Angle
        //Angle de depart
        private float _debutAngle = -225;
        //Angle de fin
        private float _finAngle = 45;

        //Nombre de graduation
        //Nombre de grandes graduation
        private int _nbreTickMajor = 11;
        //Nombre de ptites graduation
        private int _nbreTickMinor = 3;

        //Valeur Min et Max
        private double _minValue = 0;
        private double _maxValue = 100;
        private double _currentValue = 0;

        //propriete couleur 
        [Category("Configuration"), Browsable(true), Description("Couleur du corps de la jauge")]
        public Color CorpColor
        {
            get
            {
                return _corpColor;
            }

            set
            {
                _corpColor = value;
                this.Invalidate();
            }
        }

        [Category("Configuration"), Browsable(true), Description("MinValue")]
        public double MinValue
        {
            get
            {
                return _minValue;
            }

            set
            {
                _minValue = value;
            }
        }
        [Category("Configuration"), Browsable(true), Description("MaxValue")]
        public double MaxValue
        {
            get
            {
                return _maxValue;
            }

            set
            {
                _maxValue = value;
            }
        }

        [Category("Configuration"), Browsable(true), Description("CurrentValue")]
        public double CurrentValue
        {
            get
            {
                return _currentValue;
            }

            set
            {
                _currentValue = value;
                this.Invalidate();
            }
        }

        [Category("Configuration"), Browsable(true), Description("Couleur de la bordure")]
        public Color BordurColor
        {
            get
            {
                return _borderColor;
            }

            set
            {
               _borderColor = value;
            }
        }
        [Category("Configuration"), Browsable(true), Description("Couleur des chiffres")]
        public Color GradChiffreColor
        {
            get
            {
                return _gradChiffreColor;
            }

            set
            {
                _gradChiffreColor = value;
            }
        }

        public int NbreTickMajor
        {
            get
            {
                return _nbreTickMajor;
            }

            set
            {
                _nbreTickMajor = value;
            }
        }

        public int NbreTickMinor
        {
            get
            {
                return _nbreTickMinor;
            }

            set
            {
                _nbreTickMinor = value;
            }
        }
        [Category("Configuration"), Browsable(true), Description("Couleur de l'aiguille")]
        public Color AiguilleColor
        {
            get
            {
                return _aiguilleColor;
            }

            set
            {
                _aiguilleColor = value;
            }
        }

        public Jauge()
        {
            InitializeComponent();
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true
            );
            this.BackColor = Color.Transparent;
        }

        //Definir les dimonssions de la jauge
        private float GetJaugeSize()
        {
            float size = Math.Min(ClientRectangle.Width, ClientRectangle.Height);
            return size;
        }

        //Dessiner le rectangle qui recevera les cercles
        private RectangleF GetDrawRect()
        {
            float size = GetJaugeSize();
            RectangleF rect = new RectangleF(0, 0, size-1, size-1);
            return rect;
        }

        //definir le centre de la jauge
        private PointF GetJaugeCenter()
        {
            float size = GetJaugeSize();
            PointF point = new PointF(size / 2, size / 2);
            return point;
        }

        //Les radients
        protected double GetRadian(double valeur)
        {
            return valeur * Math.PI / 180;
        }

        //Dessiner le font
        protected virtual void DrawBackground(Graphics g)
        {
            SolidBrush br = new SolidBrush(this.BackColor);
            
            g.FillRectangle(br, this.ClientRectangle);
            
        }

        //Dessiner le corps de la jauge
        protected virtual void DrawBody(Graphics g)
        {
            RectangleF rc = GetDrawRect();

            LinearGradientBrush br = new LinearGradientBrush(rc, _borderColor, Color.DarkGray, 45);
            
            g.FillEllipse(br, rc);
            

            rc.Inflate(-20, -20);

            LinearGradientBrush br2 = new LinearGradientBrush(rc, Color.Gray, _borderColor, 10);
 
            g.FillEllipse(br2, rc);


            rc.Inflate(-2, -2);

            LinearGradientBrush br3 = new LinearGradientBrush(rc, Color.Black, Color.LightGray, 10);
   
            g.FillEllipse(br3, rc);
            
            rc.Inflate(-3, -3);

            LinearGradientBrush br4 = new LinearGradientBrush(rc, _corpColor, Color.WhiteSmoke, 10);

            g.FillEllipse(br4, rc);
            
            rc.Inflate(-3, -3);
        }


       

        //La graduation
        protected virtual void DrawTickMark(Graphics g)
        {
            PointF ptCenter = GetJaugeCenter();
            float sizeJauge = GetJaugeSize();

            float radius = sizeJauge / 2 - 70;

            int division = (this.NbreTickMajor - 1) * (this.NbreTickMinor + 1);

            Pen penMinor = new Pen(this._gradTickColor);
            Pen penMajor = new Pen(this._gradTickColor);
            SolidBrush brMajor = new SolidBrush(_gradTickColor);
            SolidBrush brFont = new SolidBrush(this.GradChiffreColor);
            
                int step = 0;
                for (int i = 0; i < this.NbreTickMajor; i++)
                {
                    double value = (this._finAngle - this._debutAngle) / division * step + this._debutAngle;

                    GraphicsPath path = new GraphicsPath();
                    
                    PointF pt1 = new PointF((float)(ptCenter.X + radius * Math.Cos(GetRadian(value + 1))),
                                                 (float)(ptCenter.Y + radius * Math.Sin(GetRadian(value + 1))));
                    PointF pt2 = new PointF((float)(ptCenter.X + (radius - 18) * Math.Cos(GetRadian(value + 0.5))),
                                                 (float)(ptCenter.Y + (radius - 18) * Math.Sin(GetRadian(value + 0.5))));
                    PointF pt3 = new PointF((float)(ptCenter.X + (radius - 18) * Math.Cos(GetRadian(value - 0.5))),
                                                 (float)(ptCenter.Y + (radius - 18) * Math.Sin(GetRadian(value - 0.5))));
                    PointF pt4 = new PointF((float)(ptCenter.X + radius * Math.Cos(GetRadian(value - 1))),
                                                 (float)(ptCenter.Y + radius * Math.Sin(GetRadian(value - 1))));

                    path.AddLine(pt1, pt2);
                    path.AddLine(pt2, pt3);
                    path.AddLine(pt3, pt4);
                    path.CloseFigure();

                    g.FillPath(brMajor, path);
                    g.DrawPath(penMajor, path);
                    

                    float x = (float)(ptCenter.X + (radius + 25) * Math.Cos(GetRadian(value)));
                    float y = (float)(ptCenter.Y + (radius + 25) * Math.Sin(GetRadian(value)));
                    string str = String.Format("{0,0:D}", (int)((this._maxValue - this._minValue) / division * step));
                    SizeF size = g.MeasureString(str, this._chiffreFont);
                    g.DrawString(str, this._chiffreFont, brFont, x - (float)(size.Width * 0.5), y - (float)(size.Height * 0.5));

                    step++;

                    if (i < this.NbreTickMajor - 1)
                    {
                        for (int j = 0; j < this.NbreTickMinor; j++)
                        {
                            value = (this._finAngle - this._debutAngle) / division * step + this._debutAngle;

                            PointF pt6 = new PointF((float)(ptCenter.X + radius * Math.Cos(GetRadian(value))),
                                                     (float)(ptCenter.Y + radius * Math.Sin(GetRadian(value))));
                            PointF pt7 = new PointF((float)(ptCenter.X + (radius - 14) * Math.Cos(GetRadian(value))),
                                                     (float)(ptCenter.Y + (radius - 14) * Math.Sin(GetRadian(value))));
                            g.DrawLine(penMinor, pt6, pt7);

                            step++;
                        }
                    }
                }
            
        }

        //Aiguille
        protected virtual void DrawNeedle(Graphics g)
        {
            PointF ptCenter = GetJaugeCenter();
            float sizeGuage = GetJaugeSize();
            float radius = sizeGuage / 2 - 40;

            double value = (this._finAngle - this._debutAngle) /
                           (this._maxValue - this._minValue) *
                           (this.CurrentValue - this._minValue) +
                           this._debutAngle;

            GraphicsPath path1 = new GraphicsPath();
            GraphicsPath path2 = new GraphicsPath();
            
            PointF pt1 = new PointF((float)(ptCenter.X + radius * Math.Cos(GetRadian(value))),
                                         (float)(ptCenter.Y + radius * Math.Sin(GetRadian(value))));
            PointF pt2 = new PointF((float)(ptCenter.X + 3 * Math.Cos(GetRadian(value - 80))),
                                         (float)(ptCenter.Y + 3 * Math.Sin(GetRadian(value - 80))));
            PointF pt3 = new PointF((float)(ptCenter.X + 3 * Math.Cos(GetRadian(value + 80))),
                                         (float)(ptCenter.Y + 3 * Math.Sin(GetRadian(value + 80))));

            path1.AddLine(ptCenter, pt1);
            path1.AddLine(pt1, pt2);
            path1.CloseFigure();

            path2.AddLine(ptCenter, pt1);
            path2.AddLine(pt1, pt3);
            path2.CloseFigure();

            SolidBrush br1 = new SolidBrush(AiguilleColor);
            SolidBrush br2 = new SolidBrush(AiguilleColor);
            Pen pen1 = new Pen(AiguilleColor);
            Pen pen2 = new Pen(AiguilleColor);
            g.FillPath(br2, path2);
            g.DrawPath(pen2, path2);
            g.FillPath(br1, path1);
            g.DrawPath(pen1, path1);
            

            RectangleF rcCenter = new RectangleF(ptCenter.X - 8, ptCenter.Y - 8, 16, 16);
            LinearGradientBrush br = new LinearGradientBrush(rcCenter, Color.DarkGray, Color.Black, 45);
            g.FillEllipse(br, rcCenter);
            
        }

        //Dessinuer le cadran coloré
        protected virtual void DrawZones(Graphics g)
        {
            RectangleF rc = GetDrawRect();
            rc.Inflate(-74, -74);

            double baseAngle = (this._finAngle - this._debutAngle) / (this.MaxValue - this.MinValue);
            GraphicsPath path1 = new GraphicsPath();
            GraphicsPath path2 = new GraphicsPath();
            GraphicsPath path3 = new GraphicsPath();

            Pen pen1 = new Pen(Color.LightGreen, 9);
            Pen pen2 = new Pen(Color.Yellow, 9);
            Pen pen3 = new Pen(Color.Red, 9);

            float startPathAngle1 = (float)(baseAngle *0 + _debutAngle);
            float endPathAngle1 = (float)(baseAngle * MaxValue*0.6);
            path1.AddArc(rc, startPathAngle1, endPathAngle1);
            g.DrawPath(pen1, path1);

            float startPathAngle2 = (float)(baseAngle * MaxValue * 0.6 + _debutAngle);
            float endPathAngle2 = (float)(baseAngle * MaxValue * 0.2);
            path2.AddArc(rc, startPathAngle2, endPathAngle2);
            g.DrawPath(pen2, path2);

            float startPathAngle3 = (float)(baseAngle * MaxValue * 0.8 + _debutAngle);
            float endPathAngle3 = (float)(baseAngle * MaxValue * 0.2);
            path3.AddArc(rc, startPathAngle3, endPathAngle3);
            g.DrawPath(pen3, path3);
        }

        private void Jauge_Paint_1(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            DrawBackground(e.Graphics);
            DrawBody(e.Graphics);
            DrawZones(e.Graphics);
            DrawTickMark(e.Graphics);
            DrawNeedle(e.Graphics);
        }

        public void UpdateProgress(int progress)
        {
            this._currentValue = progress;
            this.Invalidate();
        }
    }

}
