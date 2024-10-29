
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace IMControls.CustomControls;

[DefaultEvent("_TextChanged")]
public partial class IMTextBox : UserControl
{
    private Color borderColor = Color.MediumSlateBlue;
    private int borderSize = 2;
    private bool underlinedStyle = false;
    private Color borderFocusColor = Color.HotPink;
    private bool isFocused = false;
    private int borderRadius = 0;

    public event EventHandler _TextChanged;

    public IMTextBox()
    {
        InitializeComponent();
    }

    #region "Properties"
    [Category("IM Controls")]
    public Color BorderColor
    {
        get { return borderColor; }
        set
        {
            borderColor = value;
            Invalidate();
        }
    }

    [Category("IM Controls")]
    public int BorderSize
    {
        get { return borderSize; }
        set
        {
            borderSize = value;
            Invalidate();
        }
    }

    [Category("IM Controls")]
    public int BorderRadius
    {
        get { return borderRadius; }
        set
        {
            if (value >= 0)
            {
                borderRadius = value;
                Invalidate();
            }
        }
    }

    [Category("IM Controls")]
    public bool UnderlinedStyle
    {
        get { return underlinedStyle; }
        set
        {
            underlinedStyle = value;
            Invalidate();
        }
    }

    [Category("IM Controls")]
    public bool PasswordChar
    {
        get { return textBox1.UseSystemPasswordChar; }
        set { textBox1.UseSystemPasswordChar = value; }
    }

    [Category("IM Controls")]
    public bool Multiline
    {
        get { return textBox1.Multiline; }
        set { textBox1.Multiline = value; }
    }

    [Category("IM Controls")]
    public override Color BackColor
    {
        get { return base.BackColor; }
        set
        {
            base.BackColor = value;
            textBox1.BackColor = value;
        }
    }

    [Category("IM Controls")]
    public override Color ForeColor
    {
        get { return base.ForeColor; }
        set
        {
            base.ForeColor = value;
            textBox1.ForeColor = value;
        }
    }

    [Category("IM Controls")]
    public override Font Font
    {
        get { return base.Font; }
        set
        {
            base.Font = value;
            textBox1.Font = value;
            if (DesignMode)
                UpdateControlHeight();
        }
    }

    [Category("IM Controls")]
    public string Texts
    {
        get { return textBox1.Text; }
        set { textBox1.Text = value; }
    }

    [Category("IM Controls")]
    public Color BorderFocusColor
    {
        get { return borderFocusColor; }
        set { borderFocusColor = value; }
    }
    #endregion

    #region "Methods"
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics graphics = e.Graphics;

        if (borderRadius > 1)
        {
            var rectBorderSmooth = ClientRectangle;
            var rectBorder = Rectangle.Inflate(rectBorderSmooth, -borderSize, -borderSize);
            int smoothSize = borderSize > 0 ? borderSize : 1;

            using (GraphicsPath pathBorderSmooth = GetFigurePath(rectBorderSmooth, borderRadius))
            using (GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius - borderSize))
            using (Pen penBorderSmooth = new Pen(Parent.BackColor, smoothSize))
            using (Pen penBorder = new Pen(borderColor, borderSize))
            {
                Region = new Region(pathBorderSmooth);
                if (borderRadius > 15)
                    SetTextBoxRoundedRegion();

                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                penBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
                if (isFocused) penBorder.Color = borderFocusColor;

                if (underlinedStyle)
                {
                    graphics.DrawPath(penBorderSmooth, pathBorderSmooth);
                    graphics.SmoothingMode = SmoothingMode.None;
                    graphics.DrawLine(penBorder, 0, Height - 1, Width, Height - 1);
                }
                else
                {
                    graphics.DrawPath(penBorderSmooth, pathBorderSmooth);
                    graphics.DrawPath(penBorder, pathBorder);
                }
            }
        }
        else
        {
            using (Pen penBorder = new Pen(borderColor, borderSize))
            {
                Region = new Region(ClientRectangle);
                penBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;

                if (isFocused)
                    penBorder.Color = borderFocusColor;

                if (underlinedStyle)
                    graphics.DrawLine(penBorder, 0, Height - 1, Width, Height - 1);
                else
                    graphics.DrawRectangle(penBorder, 0, 0, Width - 0.5F, Height - 0.5F);
            }
        }
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        if (DesignMode)
            UpdateControlHeight();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        UpdateControlHeight();
    }

    private void UpdateControlHeight()
    {
        if (textBox1.Multiline == false)
        {
            int txtHeight = TextRenderer.MeasureText("Text", Font).Height + 1;
            textBox1.Multiline = true;
            textBox1.MinimumSize = new Size(0, txtHeight);
            textBox1.Multiline = false;
            Height = textBox1.Height + Padding.Top + Padding.Bottom;
        }
    }

    private GraphicsPath GetFigurePath(Rectangle rect, int radius)
    {
        GraphicsPath path = new GraphicsPath();
        float curveSize = radius * 2F;

        path.StartFigure();
        path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
        path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
        path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
        path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
        path.CloseFigure();
        return path;
    }

    private void SetTextBoxRoundedRegion()
    {
        GraphicsPath pathTxt;
        if (Multiline)
        {
            pathTxt = GetFigurePath(textBox1.ClientRectangle, borderRadius - borderSize);
            textBox1.Region = new Region(pathTxt);
        }
        else
        {
            pathTxt = GetFigurePath(textBox1.ClientRectangle, borderSize * 2);
            textBox1.Region = new Region(pathTxt);
        }

        pathTxt.Dispose();
    }

    #endregion

    #region "Events"
    private void textBox1_TextChanged(object sender, EventArgs e)
    {
        if (_TextChanged != null)
            _TextChanged.Invoke(sender, e);
    }

    private void textBox1_Click(object sender, EventArgs e)
    {
        OnClick(e);
    }

    private void textBox1_MouseEnter(object sender, EventArgs e)
    {
        OnMouseEnter(e);
    }

    private void textBox1_MouseLeave(object sender, EventArgs e)
    {
        OnMouseLeave(e);
    }

    private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
    {
        OnKeyPress(e);
    }

    private void textBox1_Enter(object sender, EventArgs e)
    {
        isFocused = true;
        Invalidate();
    }

    private void textBox1_Leave(object sender, EventArgs e)
    {
        isFocused = false;
        Invalidate();
    }

    private void textBox1_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Control && (e.KeyCode == Keys.A))
        {
            textBox1.SelectAll();
            e.SuppressKeyPress = true;
        }
        if (e.Control && (e.KeyCode == Keys.C))
        {
            textBox1.Copy();
            e.SuppressKeyPress = true;
        }
        if (e.Control && (e.KeyCode == Keys.X))
        {
            textBox1.Cut();
            e.SuppressKeyPress = true;
        }
    }
    #endregion
}
