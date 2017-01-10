using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace JHG.Utils.CustomControls
{
    public class CheckedComboBox : ComboBox
    {
        public class ComboboxData
        {
            public bool Checked { set; get; }

            public string Data { set; get; }

            public ComboboxData(string value, bool ischeck)
            {
                Data = value;
                Checked = ischeck;
            }

            public override string ToString()
            {
                return Data;
            }
        }

        public event EventHandler CheckChanged;

        public CheckedComboBox()
        {
            DrawMode = DrawMode.OwnerDrawVariable;
        }

        public List<ComboboxData> CheckItems
        {
            get
            {
                return Items.OfType<ComboboxData>().Select(item => item).ToList();
            }
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            var data = (ComboboxData)SelectedItem;
            if (data == null) return;
            data.Checked = !data.Checked;
            CheckChanged?.Invoke(data, e);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index == -1)
            {
                return;
            }

            if (Items[e.Index] is ComboboxData)
            {
                var data = Items[e.Index] as ComboboxData;
                CheckBoxRenderer.RenderMatchingApplicationState = true;
                CheckBoxRenderer.DrawCheckBox(e.Graphics, new Point(e.Bounds.X, e.Bounds.Y), e.Bounds, data.Data, Font,
                    (e.State & DrawItemState.Focus) == 0, data.Checked ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal);
            }
            else
            {
                e.Graphics.DrawString(Items[e.Index].ToString(), Font, Brushes.Black, new Point(e.Bounds.X, e.Bounds.Y));
                return;
            }

            base.OnDrawItem(e);
        }

    }
}
