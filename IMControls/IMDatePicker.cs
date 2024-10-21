using System.Drawing.Drawing2D;

namespace CustomControls.IMControls;

public class IMDatePicker : DateTimePicker
{
    private Color skinColor = Color.FromArgb(51, 67, 190);
    private Color textColor = Color.White;
    private Color borderColor = Color.FromArgb(65, 60, 100); //Color.PaleVioletRed;
    private int borderSize = 2;
    private bool droppedDown = false;
    private Image calendarIcon= Properties.Resources.calendarWhite;
    private RectangleF iconButtonArea;
    private const int calendarIconWidth = 34;
    private const int arrowIconWidth = 18;

    #region "Properties"
    public Color SkinColor
    {
        get { return skinColor; }
        set
        {
            skinColor = value;
            if (skinColor.GetBrightness() >= 0.6F)
                calendarIcon = Properties.Resources.calendarDark;
            else 
                calendarIcon = Properties.Resources.calendarWhite;
            this.Invalidate();
        }
    }

    public Color TextColor
    {
        get { return textColor; }
        set
        {
            textColor = value;
            this.Invalidate();
        }
    }

    public Color BorderColor
    {
        get { return borderColor; }
        set
        {
            borderColor = value;
            this.Invalidate();
        }
    }

    public int BorderSize
    {
        get { return borderSize; }
        set
        {
            borderSize = value;
            this.Invalidate();
        }
    }
    #endregion

    public IMDatePicker()
    {
        this.SetStyle(ControlStyles.UserPaint, true);
        this.MinimumSize = new Size(0, 28);
        this.Font = new Font(this.Font.Name, 11F);
        this.Size = new Size(135, 28);
        this.Format = DateTimePickerFormat.Short;
    }

    #region "Methods"
    protected override void OnDropDown(EventArgs eventargs)
    {
        base.OnDropDown(eventargs);
        droppedDown = true;
    }

    protected override void OnCloseUp(EventArgs eventargs)
    {
        base.OnCloseUp(eventargs);
        droppedDown = false;
    }

    protected override void OnKeyPress(KeyPressEventArgs e)
    {
        base.OnKeyPress(e);
        e.Handled = true;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        using (Graphics graphics = this.CreateGraphics())
        using (Pen penBorder = new Pen(borderColor, borderSize))
        using (SolidBrush skinBrush = new SolidBrush(skinColor))
        using (SolidBrush openIconBrush = new SolidBrush(Color.FromArgb(50, 64, 64, 64)))
        using (SolidBrush textBrush = new SolidBrush(textColor))
        using (StringFormat textFormat = new StringFormat())
        {
            RectangleF clientArea = new RectangleF(0, 0, this.Width - 0.5F, this.Height - 0.5F);
            RectangleF iconArea = new RectangleF(clientArea.Width - calendarIconWidth, 0, calendarIconWidth, clientArea.Height);
            penBorder.Alignment = PenAlignment.Inset;
            textFormat.LineAlignment = StringAlignment.Center;

            graphics.FillRectangle(skinBrush, clientArea);

            // Draw text
            graphics.DrawString("   " + this.Text, this.Font, textBrush, clientArea, textFormat);
            if (droppedDown == true) graphics.FillRectangle(openIconBrush, iconArea);

            if (borderSize >= 1) 
                graphics.DrawRectangle(penBorder, clientArea.X, clientArea.Y, clientArea.Width, clientArea.Height);

            //Draw icon
            graphics.DrawImage(calendarIcon, this.Width - calendarIcon.Width - 9, (this.Height - calendarIcon.Height) / 2 - 2);

        }
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        int iconWidth = GetIconButtonWidth();
        iconButtonArea = new RectangleF(this.Width - iconWidth, 0, iconWidth, this.Height);
    }
    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (iconButtonArea.Contains(e.Location))
            this.Cursor = Cursors.Hand;
        else this.Cursor = Cursors.Default;
    }

    private int GetIconButtonWidth()
    {
        int textWidh = TextRenderer.MeasureText(this.Text, this.Font).Width;

        if (textWidh <= this.Width - (calendarIconWidth + 20))
            return calendarIconWidth;
        else 
            return arrowIconWidth;
    }
    #endregion
}