using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CCT.NUI.Core.Clustering;
using CCT.NUI.HandTracking;
using CCT.NUI.Core.Shape;

namespace CCT.NUI.Samples
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        public SettingsForm(ClusterDataSourceSettings clusterSettings, ShapeDataSourceSettings shapeDataSourceSettings, HandDataSourceSettings handDetectionSettings)
            : this()
        {
            this.propertyGridClustering.SelectedObject = clusterSettings;
            this.propertyGridShape.SelectedObject = shapeDataSourceSettings;
            this.propertyGridHandDetection.SelectedObject = handDetectionSettings;
        }

        private void toolStripButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void defaultToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ClusterDataSourceSettings.SetToDefault(this.propertyGridClustering.SelectedObject as ClusterDataSourceSettings);
            HandDataSourceSettings.SetToDefault(this.propertyGridHandDetection.SelectedObject as HandDataSourceSettings);

            this.propertyGridClustering.Refresh();
            this.propertyGridHandDetection.Refresh();
        }

        private void fastToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            HandDataSourceSettings.SetToFast(this.propertyGridHandDetection.SelectedObject as HandDataSourceSettings);

            this.propertyGridClustering.Refresh();
            this.propertyGridHandDetection.Refresh();
        }

        private void accurateToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            HandDataSourceSettings.SetToAccurate(this.propertyGridHandDetection.SelectedObject as HandDataSourceSettings);

            this.propertyGridClustering.Refresh();
            this.propertyGridHandDetection.Refresh();
        }
    }
}
