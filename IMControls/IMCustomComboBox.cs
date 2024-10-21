﻿using System.ComponentModel;
using System.Drawing.Design;
using System.Drawing.Drawing2D;

namespace CustomControls.IMControls;

[DefaultEvent("OnSelectedIndexChanged")]
public class IMCustomComboBox : UserControl
{
    private Color backColor = Color.White;
    private Color iconColor = Color.MediumSlateBlue;
    private Color listBackColor = Color.FromArgb(230, 228, 245);
    private Color listTextColor = Color.DimGray;
    private Color borderColor = Color.MediumSlateBlue;
    private int borderSize = 1;
    private ComboBox cmbList;
    private Label lblText;
    private Button btnIcon;

    public event EventHandler OnSelectedIndexChanged;

    public IMCustomComboBox()
    {
        cmbList = new ComboBox();
        lblText = new Label();
        btnIcon = new Button();
        this.SuspendLayout();

        cmbList.BackColor = listBackColor;
        cmbList.Font = new Font(this.Font.Name, 10F);
        cmbList.ForeColor = listTextColor;
        cmbList.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);
        cmbList.TextChanged += new EventHandler(ComboBox_TextChanged);

        btnIcon.Dock = DockStyle.Right;
        btnIcon.FlatStyle = FlatStyle.Flat;
        btnIcon.FlatAppearance.BorderSize = 0;
        btnIcon.BackColor = backColor;
        btnIcon.Size = new Size(30, 30);
        btnIcon.Cursor = Cursors.Hand;
        btnIcon.Click += new EventHandler(Icon_Click);
        btnIcon.Paint += new PaintEventHandler(Icon_Paint);

        lblText.Dock = DockStyle.Fill;
        lblText.AutoSize = false;
        lblText.BackColor = backColor;
        lblText.TextAlign = ContentAlignment.MiddleLeft;
        lblText.Padding = new Padding(8, 0, 0, 0);
        lblText.Font = new Font(this.Font.Name, 10F);
        lblText.Click += new EventHandler(Surface_Click);
        lblText.MouseEnter += new EventHandler(Surface_MouseEnter);
        lblText.MouseLeave += new EventHandler(Surface_MouseLeave);

        this.Controls.Add(lblText); // 2
        this.Controls.Add(btnIcon); // 1
        this.Controls.Add(cmbList); // 0
        this.MinimumSize = new Size(200, 30);
        this.Size = new Size(200, 30);
        this.ForeColor = Color.DimGray;
        this.Padding = new Padding(borderSize);
        this.Font = new Font(this.Font.Name, 10F);
        base.BackColor = borderColor;
        this.ResumeLayout();
        AdjustComboBoxDimensions();
    }

    #region "Properties"
    [Category("Appearance")]
    public new Color BackColor
    {
        get => backColor;
        set
        {
            backColor = value;
            lblText.BackColor = backColor;
            btnIcon.BackColor = backColor;
        }
    }

    [Category("Appearance")]
    public Color IconColor
    {
        get => iconColor;
        set
        {
            iconColor = value;
            btnIcon.Invalidate();
        }
    }

    [Category("Appearance")]
    public Color ListBackColor
    {
        get => listBackColor;
        set
        {
            listBackColor = value;
            cmbList.BackColor = listBackColor;
        }
    }

    [Category("Appearance")]
    public Color ListTextColor
    {
        get => listTextColor;
        set
        {
            listTextColor = value;
            cmbList.ForeColor = listTextColor;
        }
    }

    [Category("Appearance")]
    public Color BorderColor
    {
        get => borderColor;
        set
        {
            borderColor = value;
            base.BackColor = borderColor;
        }
    }

    [Category("Appearance")]
    public int BorderSize
    {
        get => borderSize;
        set
        {
            borderSize = value;
            this.Padding = new Padding(borderSize);
            AdjustComboBoxDimensions();
        }
    }

    [Category("Appearance")]
    public override Color ForeColor
    {
        get => base.ForeColor;
        set
        {
            base.ForeColor = value;
            lblText.ForeColor = value;
        }
    }

    [Category("Appearance")]
    public override Font Font
    {
        get => base.Font;
        set
        {
            base.Font = value;
            lblText.Font = value;
            cmbList.Font = value;
        }
    }

    [Category("Appearance")]
    public string Texts
    {
        get => lblText.Text;
        set { lblText.Text = value; }
    }

    [Category("Appearance")]
    public ComboBoxStyle DropDownStyle
    {
        get => cmbList.DropDownStyle;
        set
        {
            if (cmbList.DropDownStyle != ComboBoxStyle.Simple)
                cmbList.DropDownStyle = value;
        }
    }

    [Category("Data")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
    [Localizable(true)]
    [MergableProperty(false)]
    public ComboBox.ObjectCollection Items
    {
        get { return cmbList.Items; }
    }

    [Category("Data")]
    [AttributeProvider(typeof(IListSource))]
    [DefaultValue(null)]
    public object DataSource
    {
        get { return cmbList.DataSource; }
        set { cmbList.DataSource = value; }
    }

    [Category("Data")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [Localizable(true)]
    public AutoCompleteStringCollection AutoCompleteCustomSource
    {
        get { return cmbList.AutoCompleteCustomSource; }
        set { cmbList.AutoCompleteCustomSource = value; }
    }

    [Category("Data")]
    [Browsable(true)]
    [DefaultValue(AutoCompleteSource.None)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public AutoCompleteSource AutoCompleteSource
    {
        get { return cmbList.AutoCompleteSource; }
        set { cmbList.AutoCompleteSource = value; }
    }

    [Category("Data")]
    [Browsable(true)]
    [DefaultValue(AutoCompleteMode.None)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public AutoCompleteMode AutoCompleteMode
    {
        get { return cmbList.AutoCompleteMode; }
        set { cmbList.AutoCompleteMode = value; }
    }

    [Category("Data")]
    [Bindable(true)]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public object SelectedItem
    {
        get { return cmbList.SelectedItem; }
        set { cmbList.SelectedItem = value; }
    }

    [Category("Data")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int SelectedIndex
    {
        get { return cmbList.SelectedIndex; }
        set { cmbList.SelectedIndex = value; }
    }

    [Category("Data")]
    [DefaultValue("")]
    [Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
    [TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public string DisplayMember
    {
        get { return cmbList.DisplayMember; }
        set { cmbList.DisplayMember = value; }
    }

    [Category("Data")]
    [DefaultValue("")]
    [Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
    public string ValueMember
    {
        get { return cmbList.ValueMember; }
        set { cmbList.ValueMember = value; }
    }

    #endregion

    #region "Methods"
    private void AdjustComboBoxDimensions()
    {
        cmbList.Width = lblText.Width;
        cmbList.Location = new Point()
        {
            X = this.Width - this.Padding.Right - cmbList.Width,
            Y = lblText.Bottom - cmbList.Height
        };
    }

    private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (OnSelectedIndexChanged != null)
            OnSelectedIndexChanged.Invoke(sender, e);
        lblText.Text = cmbList.Text;
    }

    private void Icon_Click(object sender, EventArgs e)
    {
        cmbList.Select();
        cmbList.DroppedDown = true;
    }
    private void Surface_Click(object sender, EventArgs e)
    {
        this.OnClick(e);
        cmbList.Select();
        if (cmbList.DropDownStyle == ComboBoxStyle.DropDownList)
            cmbList.DroppedDown = true;
    }

    private void ComboBox_TextChanged(object sender, EventArgs e) => lblText.Text = cmbList.Text;

    //-> Draw icon
    private void Icon_Paint(object sender, PaintEventArgs e)
    {
        int iconWidht = 14;
        int iconHeight = 6;
        var rectIcon = new Rectangle((btnIcon.Width - iconWidht) / 2, (btnIcon.Height - iconHeight) / 2, iconWidht, iconHeight);
        Graphics graph = e.Graphics;

        //Draw arrow down icon
        using (GraphicsPath path = new GraphicsPath())
        using (Pen pen = new Pen(iconColor, 2))
        {
            graph.SmoothingMode = SmoothingMode.AntiAlias;
            path.AddLine(rectIcon.X, rectIcon.Y, rectIcon.X + (iconWidht / 2), rectIcon.Bottom);
            path.AddLine(rectIcon.X + (iconWidht / 2), rectIcon.Bottom, rectIcon.Right, rectIcon.Y);
            graph.DrawPath(pen, path);
        }
    }

    private void Surface_MouseLeave(object sender, EventArgs e)
    {
        this.OnMouseLeave(e);
    }

    private void Surface_MouseEnter(object sender, EventArgs e)
    {
        this.OnMouseEnter(e);
    }
    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        AdjustComboBoxDimensions();
    }
    #endregion
}
