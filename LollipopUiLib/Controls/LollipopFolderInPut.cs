using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

[DefaultEvent("TextChanged")]
public class LollipopFolderInPut : Control
{

    #region  Variables

    Button InPutBTN = new Button();
    TextBox LollipopTB = new TextBox();

    HorizontalAlignment ALNType;
    int maxchars = 32767;
    bool readOnly;
    bool previousReadOnly;
    bool isPasswordMasked = false;
    bool Enable = true;
       
    Timer AnimationTimer = new Timer { Interval = 1 };
    FontManager font = new FontManager();

    public FolderBrowserDialog Dialog;

    bool Focus = false;

    float SizeAnimation = 0;
    float SizeInc_Dec;

    float PointAnimation;
    float PointInc_Dec;

    string fontColor = "#999999";
    private string focusColor = "#508ef5";

    Color EnabledFocusedColor;
    Color EnabledStringColor;

    Color EnabledInPutColor = ColorTranslator.FromHtml("#acacac");
    Color EnabledUnFocusedColor = ColorTranslator.FromHtml("#dbdbdb");

    Color DisabledInputColor = ColorTranslator.FromHtml("#d1d2d4");
    Color DisabledUnFocusedColor = ColorTranslator.FromHtml("#e9ecee");
    Color DisabledStringColor = ColorTranslator.FromHtml("#babbbd");

    #endregion
    #region  Properties

    public HorizontalAlignment TextAlignment
    {
        get
        {
            return ALNType;
        }
        set
        {
            ALNType = value;
            Invalidate();
        }
    }

    [Category("Behavior")]
    public int MaxLength
    {
        get
        {
            return maxchars;
        }
        set
        {
            maxchars = value;
            LollipopTB.MaxLength = MaxLength;
            Invalidate();
        }
    }

    [Category("Behavior")]
    public bool UseSystemPasswordChar
    {
        get
        {
            return isPasswordMasked;
        }
        set
        {
            LollipopTB.UseSystemPasswordChar = UseSystemPasswordChar;
            isPasswordMasked = value;
            Invalidate();
        }
    }

    [Category("Behavior")]
    public bool ReadOnly
    {
        get
        {
            return readOnly;
        }
        set
        {
            readOnly = value;
            if (LollipopTB != null)
            {
                LollipopTB.ReadOnly = value;
            }
        }
    }

    [Category("Behavior")]
    public bool IsEnabled
    {
        get { return Enable; }
        set
        {
            Enable = value;

            if (IsEnabled)
            {
                readOnly = previousReadOnly;
                LollipopTB.ReadOnly = previousReadOnly;
                LollipopTB.ForeColor = EnabledStringColor;
                InPutBTN.Enabled = true;
            }
            else
            {
                previousReadOnly = ReadOnly;
                ReadOnly = true;
                LollipopTB.ForeColor = DisabledStringColor;
                InPutBTN.Enabled = false;
            }

            Invalidate();
        }
    }

    [Category("Appearance")]
    public string FocusedColor
    {
        get { return focusColor; }
        set
        {
            focusColor = value;
            Invalidate();
        }
    }

    [Category("Appearance")]
    public string FontColor
    {
        get { return fontColor; }
        set
        {
            fontColor = value;
            Invalidate();
        }
    }

    [Browsable(false)]
    public bool Enabled
    {
        get { return base.Enabled; }
        set { base.Enabled = value; }
    }

    [Browsable(false)]
    public Font Font
    {
        get { return base.Font; }
        set { base.Font = value; }
    }

    [Browsable(false)]
    public Color ForeColor
    {
        get { return base.ForeColor; }
        set { base.ForeColor = value; }
    }

    #endregion
    #region  Events

    protected void OnKeyDown(object Obj, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.A)
        {
            LollipopTB.SelectAll();
            e.SuppressKeyPress = true;
        }
        if (e.Control && e.KeyCode == Keys.C)
        {
            LollipopTB.Copy();
            e.SuppressKeyPress = true;
        }
        if (e.Control && e.KeyCode == Keys.X)
        {
            LollipopTB.Cut();
            e.SuppressKeyPress = true;
        }
    }
    private void InPutDown(object Obj, EventArgs e)
    {
        Dialog = new FolderBrowserDialog();
        Dialog.ShowDialog();
        Text = Dialog.SelectedPath;
        Focus();
    }
    protected override void OnTextChanged(System.EventArgs e)
    {
        base.OnTextChanged(e);
        Invalidate();
    }

    protected override void OnGotFocus(System.EventArgs e)
    {
        base.OnGotFocus(e);
        LollipopTB.Focus();
        LollipopTB.SelectionLength = 0;
    }
    protected override void OnResize(System.EventArgs e)
    {
        base.OnResize(e);

        Height = 24;

        PointAnimation = Width / 2;
        SizeInc_Dec = Width / 18;
        PointInc_Dec = Width / 36;

        LollipopTB.Width = Width - 21;
        InPutBTN.Location = new Point(Width - 21, 1);
        InPutBTN.Size = new Size(21, 20);
    }

    #endregion

    public void AddButton()
    {
        InPutBTN.Location = new Point(Width - 21, 1);
        InPutBTN.Size = new Size(21, 20);

        InPutBTN.ForeColor = Color.FromArgb(255, 255, 255);
        InPutBTN.TextAlign = ContentAlignment.MiddleCenter;
        InPutBTN.BackColor = Color.Transparent;
        
        InPutBTN.TabStop = false;
        InPutBTN.FlatStyle = FlatStyle.Flat;
        InPutBTN.FlatAppearance.MouseOverBackColor = Color.Transparent;
        InPutBTN.FlatAppearance.MouseDownBackColor = Color.Transparent;
        InPutBTN.FlatAppearance.BorderSize = 0;
        
        InPutBTN.MouseDown += InPutDown;
        InPutBTN.MouseEnter += (sender, args) => EnabledInPutColor = EnabledFocusedColor;
        InPutBTN.MouseLeave += (sender, args) => EnabledInPutColor = ColorTranslator.FromHtml("#acacac");
    }
    public void AddTextBox()
    {
        LollipopTB.Text = Text;
        LollipopTB.Location = new Point(0, 1);
        LollipopTB.Size = new Size(Width - 21, 20);

        LollipopTB.Multiline = false;
        LollipopTB.Font = font.Roboto_Regular10;
        LollipopTB.ScrollBars = ScrollBars.None;
        LollipopTB.BorderStyle = BorderStyle.None;
        LollipopTB.TextAlign = HorizontalAlignment.Left;
        LollipopTB.BackColor = Color.FromArgb(255, 255, 255);
        LollipopTB.UseSystemPasswordChar = UseSystemPasswordChar;       
        
        LollipopTB.KeyDown += OnKeyDown;

        LollipopTB.GotFocus += (sender, args) => Focus = true; AnimationTimer.Start();
        LollipopTB.LostFocus += (sender, args) => Focus = false; AnimationTimer.Start();
    }
    public LollipopFolderInPut()
    {
        Width = 300;
        DoubleBuffered = true;
        previousReadOnly = ReadOnly;

        AddButton();
        AddTextBox();
        Controls.Add(LollipopTB);
        Controls.Add(InPutBTN);

        LollipopTB.TextChanged += (sender, args) => Text = LollipopTB.Text;
        base.TextChanged += (sender, args) => LollipopTB.Text = Text;

        AnimationTimer.Tick += new EventHandler(AnimationTick);
    }

    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        base.OnPaint(e);
        Bitmap B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);
        G.Clear(Color.Transparent);

        EnabledStringColor = ColorTranslator.FromHtml(fontColor);
        EnabledFocusedColor = ColorTranslator.FromHtml(focusColor);

        LollipopTB.TextAlign = TextAlignment;
        LollipopTB.ForeColor = IsEnabled ? EnabledStringColor : DisabledStringColor;
        LollipopTB.UseSystemPasswordChar = UseSystemPasswordChar;

        G.DrawLine(new Pen(new SolidBrush(IsEnabled ? EnabledUnFocusedColor : DisabledUnFocusedColor)), new Point(0, Height - 2), new Point(Width, Height - 2));
        if (IsEnabled)
        { G.FillRectangle(new SolidBrush(EnabledFocusedColor), PointAnimation, (float)Height - 3, SizeAnimation, 2); }

        G.SmoothingMode = SmoothingMode.AntiAlias;
        G.FillEllipse(new SolidBrush(IsEnabled ? EnabledInPutColor : DisabledInputColor), Width - 5, 9, 4, 4);
        G.FillEllipse(new SolidBrush(IsEnabled ? EnabledInPutColor : DisabledInputColor), Width - 11, 9, 4, 4);
        G.FillEllipse(new SolidBrush(IsEnabled ? EnabledInPutColor : DisabledInputColor), Width - 17, 9, 4, 4);

        e.Graphics.DrawImage((Image)(B.Clone()), 0, 0);
        G.Dispose();
        B.Dispose();
    }

    protected void AnimationTick(object sender, EventArgs e)
    {
        if (Focus)
        {
            if (SizeAnimation < Width)
            {
                SizeAnimation += SizeInc_Dec;
                this.Invalidate();
            }

            if (PointAnimation > 0)
            {
                PointAnimation -= PointInc_Dec;
                this.Invalidate();
            }
        }
        else
        {
            if (SizeAnimation > 0)
            {
                SizeAnimation -= SizeInc_Dec;
                this.Invalidate();
            }

            if (PointAnimation < Width / 2)
            {
                PointAnimation += PointInc_Dec;
                this.Invalidate();
            }
        }
    }

}




