﻿using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace IMControls.CustomControls;

public class IMButton: Button
{
    private int borderSize = 0;
    private int borderRadius = 10;
    private Color borderColor = Color.FromArgb(51, 67, 190);

    public IMButton()
    {
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        Size = new Size(150, 40);
        BackColor = Color.FromArgb(51, 67, 190);
        ForeColor = Color.WhiteSmoke;
        Resize += new EventHandler(Button_Resize);
        Font = new Font("Century Gothic", 10f, FontStyle.Bold);
    }

    [Category("Advanced Properties")]
    public int BorderSize
    {
        get { return borderSize; }
        set
        {
            borderSize = value;
            Invalidate();
        }
    }

    [Category("Advanced Properties")]
    public int BorderRadius
    {
        get { return borderRadius; }
        set
        {
            borderRadius = value;
            Invalidate();
        }
    }

    [Category("Advanced Properties")]
    public Color BorderColor
    {
        get { return borderColor; }
        set
        {
            borderColor = value;
            Invalidate();
        }
    }

    [Category("Advanced Properties")]
    public Color BackgroundColor
    {
        get { return BackColor; }
        set { BackColor = value; }
    }

    [Category("Advanced Properties")]
    public Color TextColor
    {
        get { return ForeColor; }
        set { ForeColor = value; }
    }

    private void Button_Resize(object sender, EventArgs e)
    {
        if (borderRadius > Height)
            borderRadius = Height;
    }

    private GraphicsPath GetFigurePath(RectangleF rect, float radius)
    {
        GraphicsPath path = new GraphicsPath();
        path.StartFigure();
        path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
        path.AddArc(rect.Width - radius, rect.Y, radius, radius, 270, 90);
        path.AddArc(rect.Width - radius, rect.Height - radius, radius, radius, 0, 90);
        path.AddArc(rect.X, rect.Height - radius, radius, radius, 90, 90);
        path.CloseFigure();

        return path;
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent);
        pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        RectangleF rectSurface = new RectangleF(0, 0, Width, Height);
        RectangleF rectBorder = new RectangleF(1, 1, Width - 0.8F, Height - 1);

        if (borderRadius > 2)
        {
            using (GraphicsPath pathSurface = GetFigurePath(rectSurface, borderRadius))
            using (GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius - 1F))
            using (Pen penSurface = new Pen(Parent.BackColor, 2))
            using (Pen penBorder = new Pen(borderColor, borderSize))
            {
                penBorder.Alignment = PenAlignment.Inset;

                // Button surface
                Region = new Region(pathSurface);
                pevent.Graphics.DrawPath(penSurface, pathSurface);

                if (borderSize >= 1)
                    pevent.Graphics.DrawPath(penBorder, pathBorder);
            }
        } else
        {
            Region = new Region(rectSurface);
            if (borderSize >= 1)
            {
                using (Pen penBorder = new Pen(borderColor, borderSize)) 
                {
                    penBorder.Alignment = PenAlignment.Inset;
                    pevent.Graphics.DrawRectangle(penBorder, 0, 0, Width - 1, Height - 1);
                }
            }
        }
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        Parent.BackColorChanged += new EventHandler(Container_BackColorChanged);
    }

    private void Container_BackColorChanged(object sender, EventArgs e)
    {
        Invalidate();
    }
}
