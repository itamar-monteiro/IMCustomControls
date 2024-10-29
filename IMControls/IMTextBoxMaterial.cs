using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace IMControls.CustomControls;

public class IMTextBoxMaterial : Control
{
    private TextBox textbox = new TextBox();
    private HorizontalAlignment ALNType;
    private int maxchars = 32767;
    private bool readOnly;
    private bool previousReadOnly;
    private bool isPasswordMasked = false;
    private bool Enable = true;
    private System.Windows.Forms.Timer AnimationTimer;
    private bool Focus;
    private float SizeAnimation;
    private float SizeInc_Dec;
    private float PointAnimation;
    private float PointInc_Dec;
    private Color focusColor;
    private Color EnabledFocusedColor;
    private Color EnabledUnFocusedColor;
    private Color DisabledUnfocusedColor;
    private Color textBackColor;
    private Color borderColor = Color.MediumSlateBlue;
    
    public IMTextBoxMaterial()
    {
        System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer
        {
            Interval = 1
        };
        Text = string.Empty;
        AnimationTimer = timer1;
        Focus = false;
        SizeAnimation = 0f;
        focusColor = ColorTranslator.FromHtml("#cc00cc");
        EnabledUnFocusedColor = Color.Gray; //ColorTranslator.FromHtml("#dbdbdb");
        DisabledUnfocusedColor = ColorTranslator.FromHtml("#e9ecee");
        textBackColor = Color.White;
        base.Width = 250;
        base.Height = 28;
        DoubleBuffered = true;
        previousReadOnly = ReadOnly;
        AddTextBox();
        base.Controls.Add(textbox);
        textbox.TextChanged += (sender, args) => Text = textbox.Text;
        base.TextChanged += (sender, args) => textbox.Text = Text;
        AnimationTimer.Tick += new EventHandler(AnimationTick);
    }

    #region "Events"
    protected override void OnGotFocus(EventArgs e)
    {
        base.OnGotFocus(e);
        textbox.Focus();
        textbox.SelectionLength = 0;
    }

    protected void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Control && (e.KeyCode == Keys.A))
        {
            textbox.SelectAll();
            e.SuppressKeyPress = true;
        }
        if (e.Control && (e.KeyCode == Keys.C))
        {
            textbox.Copy();
            e.SuppressKeyPress = true;
        }
        if (e.Control && (e.KeyCode == Keys.X))
        {
            textbox.Cut();
            e.SuppressKeyPress = true;
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Bitmap bitmap = new Bitmap(base.Width, base.Height);
        Graphics graphics = Graphics.FromImage((Image)bitmap);
        graphics.Clear(Color.Transparent);

        textbox.BackColor = BackColor;
        EnabledFocusedColor = focusColor;
        textbox.TextAlign = TextAlignment;
        textbox.UseSystemPasswordChar = UseSystemPasswordChar;

        graphics.DrawLine(
            new Pen((Brush) new SolidBrush(IsEnabled ? borderColor :
            DisabledUnfocusedColor)), 
            new Point(0, base.Height - 2), new Point(base.Width, base.Height - 2));

        if (IsEnabled)
        {
            graphics.FillRectangle((Brush)new SolidBrush(EnabledFocusedColor),
                PointAnimation, base.Height - 3f, SizeAnimation, 2f);
        }
        graphics.SmoothingMode = (SmoothingMode)SmoothingMode.AntiAlias;
        e.Graphics.DrawImage((Image)bitmap.Clone(), 0, 0);
        graphics.Dispose();
        bitmap.Dispose();
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        //base.Height = 30;
        PointAnimation = base.Width / 2;
        SizeInc_Dec = base.Width / 18;
        PointInc_Dec = base.Width / 36;
        textbox.Width = base.Width - 1;
    }

    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        base.Invalidate();
    }
    #endregion

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
    public HorizontalAlignment TextAlignment
    {
        get =>
            ALNType;
        set
        {
            ALNType = value;
            base.Invalidate();
        }
    }

    [Category("IM Controls")]
    public int MaxLength
    {
        get =>
            maxchars;
        set
        {
            maxchars = value;
            textbox.MaxLength = MaxLength;
            base.Invalidate();
        }
    }

    [Category("IM Controls")]
    public Color FocusedColor
    {
        get =>
            focusColor;
        set
        {
            focusColor = value;
            base.Invalidate();
        }
    }

    [Category("IM Controls")]
    public bool IsEnabled
    {
        get =>
            Enable;
        set
        {
            Enable = value;
            if (IsEnabled)
            {
                readOnly = previousReadOnly;
                textbox.ReadOnly = previousReadOnly;
                textbox.Enabled = true;
            }
            else
            {
                previousReadOnly = ReadOnly;
                ReadOnly = true;
            }
        }
    }

    [Category("IM Controls")]
    public bool ReadOnly
    {
        get =>
            readOnly;
        set
        {
            readOnly = value;
            if (textbox != null)
            {
                textbox.ReadOnly = value;
            }
        }
    }

    [Category("IM Controls")]
    public bool UseSystemPasswordChar
    {
        get =>
            isPasswordMasked;
        set
        {
            textbox.UseSystemPasswordChar = UseSystemPasswordChar;
            isPasswordMasked = value;
            base.Invalidate();
        }
    }

    [Category("IM Controls")]
    public Color FontColor
    {
        get =>
            textbox.ForeColor;
        set { textbox.ForeColor = value; Invalidate(); }
    }

    [Category("IM Controls")]
    public Font TextFont
    {
        get =>
            textbox.Font;
        set { textbox.Font = value; Invalidate(); }
    }

    public override Color BackColor
    {
        get => base.BackColor;
        set 
        {
            base.BackColor = value;
            textbox.BackColor = value;
            Invalidate();
        }
    }
    #endregion

    #region "Methods"
    private void AddTextBox()
    {
        textbox.Text = Text;
        textbox.Size = new Size(base.Width - 10, Height);
        textbox.Location = new Point(Location.X+4, Location.Y+7);
        textbox.Multiline = false;
        textbox.Font = new Font("Century Gothic", 10f);
        textbox.ScrollBars = ScrollBars.None;
        textbox.BorderStyle = BorderStyle.None;
        textbox.TextAlign = HorizontalAlignment.Left;
        textbox.UseSystemPasswordChar = UseSystemPasswordChar;
        textbox.ForeColor = FontColor;
        textbox.BackColor = BackColor;
        textbox.KeyDown += new KeyEventHandler(OnKeyDown);
        textbox.GotFocus += (sender, args) => Focus = true;
        AnimationTimer.Start();
        textbox.LostFocus += (sender, args) => Focus = false;
        AnimationTimer.Start();
    }

    public override string Text
    {
        get => base.Text;
        set { }
    }

    private void AnimationTick(object sender, EventArgs e)
    {
        if (Focus)
        {
            if (SizeAnimation < base.Width)
            {
                SizeAnimation += SizeInc_Dec;
                base.Invalidate();
            }
            if (PointAnimation > 0f)
            {
                PointAnimation -= PointInc_Dec;
                base.Invalidate();
            }
        }
        else
        {
            if (SizeAnimation > 0f)
            {
                SizeAnimation -= SizeInc_Dec;
                base.Invalidate();
            }
            if (PointAnimation < (base.Width / 2))
            {
                PointAnimation += PointInc_Dec;
                base.Invalidate();
            }
        }
    }
    #endregion
}
