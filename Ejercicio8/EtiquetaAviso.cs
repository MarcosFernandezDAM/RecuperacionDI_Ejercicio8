using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ejercicio8
{
    public enum eMarca
    {
        Nada,
        Cruz,
        Circulo,
        Imagen
    }


    public partial class EtiquetaAviso : Control
    {
        private int y;
        private int x;
        private int width;
        private int height;
        
        public EtiquetaAviso()
        {
            InitializeComponent();
        }

        private eMarca marca = eMarca.Nada;
        [Category("Apariencia")]
        [Description("Coloca X, Círculo, o nada")]
        public eMarca Marca
        {
            set
            {
                marca = value;
                this.Refresh();
            }
            get { return marca; }
        }


        private bool deg = false;
        [Category("Apariencia")]
        [Description("Fondo con degradado")]
        public bool Degradado
        {
            set
            {
                deg = value;
                this.Refresh();
            }
            get
            {
                return deg;
            }
        }

        private Color colorIn;
        [Category("Apariencia")]
        [Description("Color de inicio del degradado")]
        public Color ColorInicio
        {
            set
            {
                colorIn = value;
                this.Refresh();
            }
            get
            {
                return colorIn;
            }
        }

        private Color colorFin;
        [Category("Apariencia")]
        [Description("Color final del degradado")]
        public Color ColorFinal
        {
            set
            {
                colorFin = value;
                this.Refresh();
            }
            get
            {
                return colorFin;
            }
        }

        private Image imagen;
        [Category("Apariencia")]
        [Description("Imagen del componente")]
        public Image ImagenMarca
        {
            set
            {
                imagen = value;
                this.Refresh();
            }
            get
            {
                return imagen;
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            int grosor = 0;
            int offsetX = 0;
            int offsetY = 0;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (deg)
            {
                LinearGradientBrush gradientColor = new LinearGradientBrush(new PointF(0, 0), new PointF(this.Width, this.Height), colorIn, colorFin);
                g.FillRectangle(gradientColor, new RectangleF(0, 0, this.Width, this.Height));
            }

            switch (Marca)
            {
                case eMarca.Circulo:
                    grosor = 5;
                    g.DrawEllipse(new Pen(Color.Green, grosor), grosor, grosor, this.Font.Height, this.Font.Height);
                    offsetX = this.Font.Height + grosor;
                    offsetY = grosor;
                    x = grosor;
                    y = grosor;
                    width = this.Font.Height;
                    height = this.Font.Height;
                    break;

                case eMarca.Cruz:
                    grosor = 5;
                    Pen lapiz = new Pen(Color.Red, grosor);
                    g.DrawLine(lapiz, grosor, grosor, this.Font.Height, this.Font.Height);
                    g.DrawLine(lapiz, this.Font.Height, grosor, grosor, this.Font.Height);
                    offsetX = this.Font.Height + grosor;
                    offsetY = grosor / 2;
                    x = grosor;
                    y = grosor;
                    width = this.Font.Height;
                    height = this.Font.Height;
                    lapiz.Dispose();
                    break;

                case eMarca.Imagen:
                    if (imagen != null)
                    {
                        grosor = 10;
                        int altoImagen = this.Font.Height;
                        int anchoImagen = (imagen.Width * this.Font.Height) / imagen.Height;
                        g.DrawImage(imagen, grosor, grosor, anchoImagen, altoImagen);
                        offsetX = anchoImagen + grosor;
                        offsetY = grosor;
                        x = grosor;
                        y = grosor;
                        width = anchoImagen;
                        height = altoImagen;
                    }
                    break;
            }

            SolidBrush b = new SolidBrush(this.ForeColor);
            g.DrawString(this.Text, this.Font, b, offsetX + grosor, offsetY);
            Size tam = g.MeasureString(this.Text, this.Font).ToSize();
            this.Size = new Size(tam.Width + offsetX + grosor, tam.Height + offsetY * 2);
            b.Dispose();
        }


        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.Refresh(); 
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.X >= this.x && e.X <= this.x + width && e.Y >= this.y && e.Y <= height)
            {
                ClickEnMarca?.Invoke(this, EventArgs.Empty);
            }
        }

        [Category("Click en marca")]
        [Description("Se lanza cuando se hace click en marca")]
        public event EventHandler ClickEnMarca;
    }
}