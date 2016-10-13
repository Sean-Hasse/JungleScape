using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelEditor
{
    public partial class MapEditor : Form
    {
        public MapEditor()
        {
            InitializeComponent();
        }

        private void MapEditor_Load(object sender, EventArgs e)
        {

        }

        //a mistake created, this is the label
        private void Height_Click(object sender, EventArgs e)
        {

        }

        // have the ability to change the height of the game assets
        private void HeightPicker_ValueChanged(object sender, EventArgs e)
        {

        }

        // have the ability to change the width of the game assets
        private void WidthPicker_ValueChanged(object sender, EventArgs e)
        {

        }

        // have the ability to change the title size of the map
        private void SizePicker_ValueChanged(object sender, EventArgs e)
        {

        }

        // have the ability to select multiple layers on the map
        private void LayerPicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // have the ability to schange the z value of the game assets
        private void zValuePicker_ValueChanged(object sender, EventArgs e)
        {

        }

        // have the ability to show/change the game asset
        private void isVisible_CheckedChanged(object sender, EventArgs e)
        {

        }

        // have the ability to make the blocks transparent the game asset
        private void isSolid_CheckedChanged(object sender, EventArgs e)
        {

        }

        //have the ability to select the options for the game asset
        private void setbutton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You have submitted your options.");
        }
    }
}
