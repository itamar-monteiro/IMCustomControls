using IMControls.Properties;
using System.Drawing.Drawing2D;
using System.Resources;
using System.Windows.Forms.VisualStyles;

namespace IMControls.CustomControls;

public class IMDatePicker : DateTimePicker
{
    private Color skinColor = Color.FromArgb(51, 67, 190);
    private Color textColor = Color.White;
    private Color borderColor = Color.FromArgb(65, 60, 100); //Color.PaleVioletRed;
    private int borderSize = 2;
    private bool droppedDown = false;
    private Image calendarIcon = Resources.WhiteCalendar;
    private RectangleF iconButtonArea;
    private const int calendarIconWidth = 34;
    private const int arrowIconWidth = 18;
    private ImageList calendarList = new ImageList();

    public IMDatePicker()
    {
        SetStyle(ControlStyles.UserPaint, true);
        MinimumSize = new Size(0, 28);
        Font = new Font(Font.Name, 11F);
        Size = new Size(135, 28);
        Format = DateTimePickerFormat.Short;
    }

    #region "Properties"
    public Color SkinColor
    {
        get { return skinColor; }
        set
        {
            skinColor = value;
            if (skinColor.GetBrightness() >= 0.6F)
                calendarIcon = Resources.DarkCalendar;
            else
                calendarIcon = Resources.WhiteCalendar;
            Invalidate();
        }
    }

    public Color TextColor
    {
        get { return textColor; }
        set
        {
            textColor = value;
            Invalidate();
        }
    }

    public Color BorderColor
    {
        get { return borderColor; }
        set
        {
            borderColor = value;
            Invalidate();
        }
    }

    public int BorderSize
    {
        get { return borderSize; }
        set
        {
            borderSize = value;
            Invalidate();
        }
    }
    #endregion

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
        using (Graphics graphics = CreateGraphics())
        using (Pen penBorder = new Pen(borderColor, borderSize))
        using (SolidBrush skinBrush = new SolidBrush(skinColor))
        using (SolidBrush openIconBrush = new SolidBrush(Color.FromArgb(50, 64, 64, 64)))
        using (SolidBrush textBrush = new SolidBrush(textColor))
        using (StringFormat textFormat = new StringFormat())
        {
            RectangleF clientArea = new RectangleF(0, 0, Width - 0.5F, Height - 0.5F);
            RectangleF iconArea = new RectangleF(clientArea.Width - calendarIconWidth, 0, calendarIconWidth, clientArea.Height);
            penBorder.Alignment = PenAlignment.Inset;
            textFormat.LineAlignment = StringAlignment.Center;

            graphics.FillRectangle(skinBrush, clientArea);

            // Draw text
            graphics.DrawString("   " + Text, Font, textBrush, clientArea, textFormat);
            if (droppedDown == true) graphics.FillRectangle(openIconBrush, iconArea);

            if (borderSize >= 1) 
                graphics.DrawRectangle(penBorder, clientArea.X, clientArea.Y, clientArea.Width, clientArea.Height);

            //Draw icon
            graphics.DrawImage(calendarIcon, Width - calendarIcon.Width - 9, (Height - calendarIcon.Height) / 2 - 2);

        }
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        int iconWidth = GetIconButtonWidth();
        iconButtonArea = new RectangleF(Width - iconWidth, 0, iconWidth, Height);
    }
    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (iconButtonArea.Contains(e.Location))
            Cursor = Cursors.Hand;
        else Cursor = Cursors.Default;
    }

    private int GetIconButtonWidth()
    {
        int textWidh = TextRenderer.MeasureText(Text, Font).Width;

        if (textWidh <= Width - (calendarIconWidth + 20))
            return calendarIconWidth;
        else 
            return arrowIconWidth;
    }
    #endregion
}